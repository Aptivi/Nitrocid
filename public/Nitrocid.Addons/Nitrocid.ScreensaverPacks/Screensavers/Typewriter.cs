//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Drivers.RNG;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Textify.General;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Typewriter
    /// </summary>
    public class TypewriterDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Typewriter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(ScreensaverPackInit.SaversConfig.TypewriterTextColor));
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMin * 5;
            int CpmSpeedMax = ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMax * 5;
            string TypeWrite = ScreensaverPackInit.SaversConfig.TypewriterWrite;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Minimum speed from {0} WPM: {1} CPM", ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMin, CpmSpeedMin);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Maximum speed from {0} WPM: {1} CPM", ScreensaverPackInit.SaversConfig.TypewriterWritingSpeedMax, CpmSpeedMax);
            ConsoleWrapper.CursorVisible = false;
            // Typewriter can also deal with files written on the field that is used for storing text, so check to see if the path exists.
            DebugWriter.WriteDebug(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", ScreensaverPackInit.SaversConfig.TypewriterWrite);
            if (Parsing.TryParsePath(ScreensaverPackInit.SaversConfig.TypewriterWrite) && Checking.FileExists(ScreensaverPackInit.SaversConfig.TypewriterWrite))
            {
                // File found! Now, write the contents of it to the local variable that stores the actual written text.
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", ScreensaverPackInit.SaversConfig.TypewriterWrite);
                TypeWrite = Reading.ReadContentsText(ScreensaverPackInit.SaversConfig.TypewriterWrite);
            }

            // For each line, write four spaces, and extra two spaces if paragraph starts.
            foreach (string Paragraph in TypeWrite.SplitNewLines())
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "New paragraph: {0}", Paragraph);

                // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
                // sizes.
                var IncompleteSentences = TextTools.GetWrappedSentences(Paragraph, ConsoleWrapper.WindowWidth - 2, 4);

                // Prepare display (make a paragraph indentation)
                if (ConsoleWrapper.CursorTop != ConsoleWrapper.WindowHeight - 2)
                {
                    ConsoleWrapper.WriteLine();
                    ConsoleWrapper.Write("    ");
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                }

                // Get struck character and write it
                for (int SentenceIndex = 0; SentenceIndex <= IncompleteSentences.Length - 1; SentenceIndex++)
                {
                    string Sentence = IncompleteSentences[SentenceIndex];
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    foreach (char StruckChar in Sentence)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                        int SelectedCpm = RandomDriver.RandomIdx(CpmSpeedMin, CpmSpeedMax);
                        int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs);

                        // If we're at the end of the page, clear the screen
                        if (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 2)
                        {
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're at the end of the page! {0} = {1}", ConsoleWrapper.CursorTop, ConsoleWrapper.WindowHeight - 2);
                            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.TypewriterNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                            ConsoleWrapper.Clear();
                            ConsoleWrapper.WriteLine();
                            if (SentenceIndex == 0)
                            {
                                ConsoleWrapper.Write("    ");
                            }
                            else
                            {
                                ConsoleWrapper.Write(" ");
                            }
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                        }

                        // If we need to show the arrow indicator, update its position
                        if (ScreensaverPackInit.SaversConfig.TypewriterShowArrowPos)
                        {
                            int OldTop = ConsoleWrapper.CursorTop;
                            int OldLeft = ConsoleWrapper.CursorLeft;
                            ConsoleWrapper.SetCursorPosition(OldLeft, ConsoleWrapper.WindowHeight - 1);
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Arrow drawn in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                            ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "[1K^" + Convert.ToString(CharManager.GetEsc()) + "[K");
                            ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Returned to {0}, {1}", OldLeft, OldTop);
                        }

                        // Write the final character to the console and wait
                        ConsoleWrapper.Write(StruckChar);
                        ThreadManager.SleepNoBlock(WriteMs, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    }
                    ConsoleWrapper.WriteLine();
                    ConsoleWrapper.Write(" ");
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(ScreensaverPackInit.SaversConfig.TypewriterDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
