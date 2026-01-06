//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using System;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Textify.General;
using Textify.General.Structures;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Typewriter
    /// </summary>
    public class TypewriterDisplay : BaseScreensaver, IScreensaver
    {
        private int linesWritten = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Typewriter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            linesWritten = 0;
            ColorTools.SetConsoleColor(new Color(ScreensaverPackInit.SaversConfig.TypewriterTextColor));
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMin * 5;
            int CpmSpeedMax = ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMax * 5;
            string TypeWrite = ScreensaverPackInit.SaversConfig.TypewriterWrite;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Minimum speed from {0} WPM: {1} CPM", vars: [ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMin, CpmSpeedMin]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Maximum speed from {0} WPM: {1} CPM", vars: [ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMax, CpmSpeedMax]);

            // Typewriter can also deal with files written on the field that is used for storing text, so check to see if the path exists.
            DebugWriter.WriteDebug(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", vars: [ScreensaverPackInit.SaversConfig.TypewriterWrite]);
            if (FilesystemTools.TryParsePath(ScreensaverPackInit.SaversConfig.TypewriterWrite) && FilesystemTools.FileExists(ScreensaverPackInit.SaversConfig.TypewriterWrite))
            {
                // File found! Now, write the contents of it to the local variable that stores the actual written text.
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", vars: [ScreensaverPackInit.SaversConfig.TypewriterWrite]);
                TypeWrite = FilesystemTools.ReadContentsText(ScreensaverPackInit.SaversConfig.TypewriterWrite);
            }

            // For each line, write four spaces, and extra two spaces if paragraph starts.
            foreach (string Paragraph in TypeWrite.SplitNewLines())
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "New paragraph: {0}", vars: [Paragraph]);

                // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
                // sizes.
                var IncompleteSentences = TextTools.GetWrappedSentences(Paragraph, ConsoleWrapper.WindowWidth - 2, 4);

                // Prepare display (make a paragraph indentation)
                if (linesWritten != ConsoleWrapper.WindowHeight - 2)
                {
                    linesWritten++;
                    ConsoleWrapper.WriteLine();
                    ConsoleWrapper.Write("    ");
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in 4, {0}", vars: [linesWritten]);
                }

                // Get struck character and write it
                for (int SentenceIndex = 0; SentenceIndex <= IncompleteSentences.Length - 1; SentenceIndex++)
                {
                    string Sentence = IncompleteSentences[SentenceIndex];
                    var wideChars = ConsoleChar.GetWideChars(Sentence);
                    int processedWidth = 0;
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    if (ScreensaverManager.Bailing)
                        return;

                    foreach (WideChar StruckChar in wideChars)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;
                        if (ScreensaverManager.Bailing)
                            return;

                        // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                        int SelectedCpm = RandomDriver.RandomIdx(CpmSpeedMin, CpmSpeedMax);
                        int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", vars: [SelectedCpm, WriteMs]);

                        // If we're at the end of the page, clear the screen
                        int indentTimes = SentenceIndex == 0 ? 4 : 1;
                        if (linesWritten == ConsoleWrapper.WindowHeight - 2)
                        {
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're at the end of the page! {0} = {1}", vars: [linesWritten, ConsoleWrapper.WindowHeight - 2]);
                            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MirrorWriteNewScreenDelay);
                            ConsoleWrapper.Clear();
                            linesWritten = 1;
                            ConsoleWrapper.WriteLine();
                            ConsoleWrapper.Write(new string(' ', indentTimes));
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", vars: [SentenceIndex == 0 ? 4 : 1, linesWritten]);
                        }

                        // Write the final character to the console and wait
                        int charWidth = ConsoleChar.EstimateCellWidth(StruckChar);
                        processedWidth += charWidth;
                        ConsoleWrapper.Write(StruckChar);

                        // If we need to show the arrow indicator, update its position
                        if (ScreensaverPackInit.SaversConfig.TypewriterShowArrowPos)
                        {
                            ConsoleWrapper.Write(ConsolePositioning.RenderChangePosition(indentTimes + processedWidth - charWidth, ConsoleWrapper.WindowHeight - 1));
                            ConsoleWrapper.Write("\x1b[1K" + "^" + "\x1b[K");
                            ConsoleWrapper.Write(ConsolePositioning.RenderChangePosition(indentTimes + processedWidth, linesWritten));
                        }
                        ScreensaverManager.Delay(WriteMs);
                    }
                    linesWritten++;
                    ConsoleWrapper.WriteLine();
                    ConsoleWrapper.Write(" ");
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in 1, {0}", vars: [linesWritten]);
                }
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.TypewriterDelay);
        }

    }
}
