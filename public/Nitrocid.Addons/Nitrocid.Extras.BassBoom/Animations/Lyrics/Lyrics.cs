﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using BassBoom.Basolia.Lyrics;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Files.Folders;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Writer.FancyWriters;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Paths;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters;
using Listing = Nitrocid.Files.Folders.Listing;

namespace Nitrocid.Extras.BassBoom.Animations.Lyrics
{
    /// <summary>
    /// Lyrics animation module
    /// </summary>
    public static class Lyrics
    {

        internal static string lyricsPath = PathsManagement.HomePath + "/Music/";
        internal static string[]? lyricsLrc;

        /// <summary>
        /// Path to the lyrics
        /// </summary>
        public static string LyricsPath
        {
            get => BassBoomInit.BassBoomConfig.LyricsPath;
            set
            {
                BassBoomInit.BassBoomConfig.LyricsPath = Checking.FolderExists(value) ? FilesystemTools.NeutralizePath(value) : BassBoomInit.BassBoomConfig.LyricsPath;
                lyricsLrc = Listing.GetFilesystemEntries(BassBoomInit.BassBoomConfig.LyricsPath, "*.lrc");
            }
        }

        /// <summary>
        /// Simulates the lyric animation
        /// </summary>
        public static void Simulate(LyricsSettings? Settings)
        {
            // Check the settings
            if (Settings is null)
                return;

            // Select random lyric file from $HOME/Music/*.LRC
            lyricsLrc ??= Listing.GetFilesystemEntries(LyricsPath, "*.lrc");
            var lyricPath = lyricsLrc.Length > 0 ? lyricsLrc[RandomDriver.RandomIdx(lyricsLrc.Length)] : "";
            DebugWriter.WriteDebug(DebugLevel.I, "Lyric path is {0}", lyricPath);

            // Visualize it!
            VisualizeLyric(lyricPath);
            ThreadManager.SleepNoBlock(Settings.LyricsDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

        /// <summary>
        /// Visualizes the lyric
        /// </summary>
        /// <param name="path">Path to lyric file</param>
        public static void VisualizeLyric(string path)
        {
            // Neutralize the path
            path = FilesystemTools.NeutralizePath(path);
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBack();

            // Get the height and the maximum number of characters
            int infoHeight = ConsoleWrapper.WindowHeight - 3;
            int infoMaxChars = ConsoleWrapper.WindowWidth - 9;

            // If there is no lyric path, or if it doesn't exist, tell the user that they have to provide a path to the
            // lyrics folder.
            if (string.IsNullOrWhiteSpace(path) || !Checking.FileExists(path))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Lyrics file {0} not found!", path);
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Make sure to specify the path to a directory containing your lyric files in the LRC format. You can also specify a custom path to your music library folder containing the lyric files."), false);
                return;
            }

            // Here, the lyric file is given. Process it...
            string fileName = Path.GetFileNameWithoutExtension(path);
            var lyric = LyricReader.GetLyrics(path);
            var lyricLines = lyric.Lines;
            var shownLines = new List<LyricLine>();
            DebugWriter.WriteDebug(DebugLevel.I, "{0} lyric lines", lyricLines.Count);
            var boxFrame = new BoxFrame()
            {
                Text = fileName,
                Left = 2,
                Top = ConsoleWrapper.WindowHeight - 4,
                InteriorWidth = ConsoleWrapper.WindowWidth - 6,
                InteriorHeight = 1,
            };
            TextWriterRaw.WriteRaw(boxFrame.Render());
            DebugWriter.WriteDebug(DebugLevel.I, "Visualizing lyric file {0} [file name: {1}]", path, fileName);

            // Start the elapsed time in 3...
            bool bail = false;
            for (int i = 3; i > 0; i--)
            {
                TextWriterWhereColor.WriteWhere(new string(' ', infoMaxChars), 3, infoHeight);
                TextWriters.WriteWhere($"{i}...", ConsoleWrapper.WindowWidth / 2 - $"{i}...".Length / 2, infoHeight, KernelColorType.NeutralText);
                if (ThreadManager.SleepUntilInput(1000))
                {
                    bail = true;
                    break;
                }
            }

            // Go!
            DebugWriter.WriteDebug(DebugLevel.I, "Let's do this!");
            TextWriterWhereColor.WriteWhere(new string(' ', infoMaxChars), 3, infoHeight);
            TextWriters.WriteWhere("Go!", ConsoleWrapper.WindowWidth / 2 - "Go!".Length / 2, infoHeight, KernelColorType.NeutralText);
            var sw = new Stopwatch();
            sw.Start();
            foreach (var ts in lyricLines)
            {
                if (bail)
                    break;

                while (sw.Elapsed < ts.LineSpan)
                {
                    if (ThreadManager.SleepUntilInput(10))
                    {
                        bail = true;
                        break;
                    }
                }

                if (bail)
                    break;

                if (sw.Elapsed > ts.LineSpan)
                {
                    string tsLine = ts.Line;
                    DebugWriter.WriteDebug(DebugLevel.I, "New lyric occurred at {0}! {1}.", ts.LineSpan, tsLine);
                    if (ts.LineSpan != lyricLines[^1].LineSpan)
                        DebugWriter.WriteDebug(DebugLevel.I, "Next lyric occurs at {0}. {1}", lyricLines[lyricLines.IndexOf(ts) + 1].LineSpan, lyricLines[lyricLines.IndexOf(ts) + 1].Line);
                    TextWriterWhereColor.WriteWhere(new string(' ', infoMaxChars), 3, infoHeight);
                    TextWriters.WriteWhere(tsLine.Truncate(infoMaxChars), ConsoleWrapper.WindowWidth / 2 - tsLine.Length / 2, infoHeight, KernelColorType.NeutralText);
                    shownLines.Add(ts);
                    DebugWriter.WriteDebug(DebugLevel.I, "shownLines = {0} / {1}", shownLines.Count, lyricLines.Count);
                    if (shownLines.Count == lyricLines.Count)
                        break;
                }
            }
            ConsoleWrapper.Clear();
        }

        /// <summary>
        /// Gets the lyric lines
        /// </summary>
        /// <param name="path">Path to the lyric file (may or may not be neutralized)</param>
        /// <returns>Lyric lines</returns>
        /// <exception cref="KernelException"></exception>
        public static LyricLine[] GetLyricLines(string path)
        {
            // Neutralize the path
            path = FilesystemTools.NeutralizePath(path);

            // If there is no lyric file, bail
            if (string.IsNullOrWhiteSpace(path) || !Checking.FileExists(path))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Lyrics file {0} not found!", path);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("This lyrics file doesn't exist.") + $" {path}");
            }

            // Here, the lyric file is given. Process it...
            var lyric = LyricReader.GetLyrics(path);
            var lyricLines = lyric.Lines;
            return [.. lyricLines];
        }

    }
}
