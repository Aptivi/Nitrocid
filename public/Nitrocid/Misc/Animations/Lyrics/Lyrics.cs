
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Drivers.RNG;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel.Threading;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using SharpLyrics;

namespace KS.Misc.Animations.Lyrics
{
    /// <summary>
    /// Lyrics animation module
    /// </summary>
    public static class Lyrics
    {

        // TODO: Make this customizable, as we don't want users to move / copy all their lrc files from their
        //       music library to $HOME/Music/.
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
            KernelColorTools.LoadBack();

            // Render the border in the lower part of the console for lyric line
            string fileName = Path.GetFileNameWithoutExtension(path);
            int infoHeight = ConsoleWrapper.WindowHeight - 3;
            int infoMaxChars = ConsoleWrapper.WindowWidth - 9;
            BorderColor.WriteBorder(2, ConsoleWrapper.WindowHeight - 4, ConsoleWrapper.WindowWidth - 6, 1);
            TextWriterWhereColor.WriteWhere($" {fileName.Truncate(infoMaxChars)} ", 4, ConsoleWrapper.WindowHeight - 4, KernelColorType.NeutralText);

            // If there is no lyric path, or if it doesn't exist, tell the user that they have to provide a path to the
            // lyrics folder.
            //
            // Beta 2 Note: Message will mention that they can customize the directory to the .LRC files, but they can't
            // in this beta, so be honest and say that this feature is due Beta 3.
            if (string.IsNullOrWhiteSpace(path) || !Checking.FileExists(path))
            {
                ConsoleWrapper.SetCursorPosition(2, 1);
                TextWriterColor.Write(Translate.DoTranslation("Make sure to specify the path to a directory containing your lyric files in the LRC format. You can also specify a custom path to your music library folder containing the lyric files.") +
                    // TODO: Remove this message at the start of the Beta 3 cycle.
                    " However, the custom paths feature isn't available in this beta version of Nitrocid KS 0.1.0.");
                return;
            }

            // Here, the lyric file is given. Process it...
            var lyric = LyricReader.GetLyrics(path);
            var lyricLines = lyric.Lines;
            var shownLines = new List<LyricLine>();

            // Start the elapsed time in 3...
            for (int i = 3; i > 0; i--)
            {
                TextWriterWhereColor.WriteWhere(new string(' ', infoMaxChars), 3, infoHeight);
                TextWriterWhereColor.WriteWhere($"{i}...", (ConsoleWrapper.WindowWidth / 2) - ($"{i}...".Length / 2), infoHeight, KernelColorType.NeutralText);
                ThreadManager.SleepNoBlock(1000, Thread.CurrentThread);
            }

            // Go!
            TextWriterWhereColor.WriteWhere(new string(' ', infoMaxChars), 3, infoHeight);
            TextWriterWhereColor.WriteWhere("Go!", (ConsoleWrapper.WindowWidth / 2) - ("Go!".Length / 2), infoHeight, KernelColorType.NeutralText);
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
                    TextWriterWhereColor.WriteWhere(new string(' ', infoMaxChars), 3, infoHeight);
                    TextWriterWhereColor.WriteWhere(tsLine, (ConsoleWrapper.WindowWidth / 2) - (tsLine.Length / 2), infoHeight, KernelColorType.NeutralText);
                    shownLines.Add(ts);
                    if (shownLines.Count == lyricLines.Count)
                        return;
                }
            }
            ConsoleWrapper.Clear();
        }

    }
}
