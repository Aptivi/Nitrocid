
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Misc.Screensaver;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using SharpLyrics;

namespace KS.Misc.Animations.Lyrics
{
    /// <summary>
    /// Lyrics animation module
    /// </summary>
    public static class Lyrics
    {

        private static string[] lyricsLrc = Listing.GetFilesystemEntries(Paths.HomePath + "/Music/", "*.lrc");

        /// <summary>
        /// Simulates the lyric animation
        /// </summary>
        public static void Simulate(LyricsSettings Settings)
        {
            // Select random lyric file from $HOME/Music/*.LRC
            var lyricPath = lyricsLrc.Length > 0 ? lyricsLrc[RandomDriver.RandomIdx(lyricsLrc.Length)] : "";

            // Visualize it!
            VisualizeLyric(lyricPath);
            ThreadManager.SleepNoBlock(Settings.LyricsDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

        /// <summary>
        /// Visualizes the lyric
        /// </summary>
        /// <param name="path">Path to lyric file</param>
        public static void VisualizeLyric(string path)
        {
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Render the border in the lower part of the console for lyric line
            BorderColor.WriteBorder(2, ConsoleWrapper.WindowHeight - 4, ConsoleWrapper.WindowWidth - 6, 1);
            int infoHeight = ConsoleWrapper.WindowHeight - 3;
            int infoMaxChars = ConsoleWrapper.WindowWidth - 9;

            // If there is no lyric path, or if it doesn't exist, just bail
            if (string.IsNullOrWhiteSpace(path) || !Checking.FileExists(path))
                return;

            // Here, the lyric file is given. Process it...
            var lyric = LyricReader.GetLyrics(path);
            var lyricLines = lyric.Lines;
            var shownLines = new List<LyricLine>();

            // Start the elapsed time in 3...
            for (int i = 3; i > 0; i--)
            {
                TextWriterWhereColor.WriteWhere(" ".Repeat(infoMaxChars), 3, infoHeight);
                TextWriterWhereColor.WriteWhere($"{i}...", (ConsoleWrapper.WindowWidth / 2) - ($"{i}...".Length / 2), infoHeight);
                ThreadManager.SleepNoBlock(1000, Thread.CurrentThread);
            }

            // Go!
            TextWriterWhereColor.WriteWhere(" ".Repeat(infoMaxChars), 3, infoHeight);
            TextWriterWhereColor.WriteWhere("Go!", (ConsoleWrapper.WindowWidth / 2) - ("Go!".Length / 2), infoHeight);
            var sw = new Stopwatch();
            sw.Start();
            foreach (var ts in lyricLines)
            {
                while (sw.Elapsed < ts.LineSpan)
                {
                    ThreadManager.SleepNoBlock(1, Thread.CurrentThread);
                    if (ConsoleWrapper.KeyAvailable)
                        return;
                }

                if (sw.Elapsed > ts.LineSpan)
                {
                    string tsLine = ts.Line.Truncate(infoMaxChars);
                    TextWriterWhereColor.WriteWhere(" ".Repeat(infoMaxChars), 3, infoHeight);
                    TextWriterWhereColor.WriteWhere(tsLine, (ConsoleWrapper.WindowWidth / 2) - (tsLine.Length / 2), infoHeight);
                    shownLines.Add(ts);
                    if (shownLines.Count == lyricLines.Count)
                        return;
                }
            }
            ConsoleWrapper.Clear();
        }

    }
}
