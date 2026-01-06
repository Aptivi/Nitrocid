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
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Textify.General;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Files;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for SpotWrite
    /// </summary>
    public class SpotWriteDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "SpotWrite";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(ScreensaverPackInit.SaversConfig.SpotWriteTextColor));
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            string TypeWrite = ScreensaverPackInit.SaversConfig.SpotWriteWrite;

            // SpotWrite can also deal with files written on the field that is used for storing text, so check to see if the path exists.
            DebugWriter.WriteDebug(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", vars: [ScreensaverPackInit.SaversConfig.SpotWriteWrite]);
            if (FilesystemTools.TryParsePath(ScreensaverPackInit.SaversConfig.SpotWriteWrite) && FilesystemTools.FileExists(ScreensaverPackInit.SaversConfig.SpotWriteWrite))
            {
                // File found! Now, write the contents of it to the local variable that stores the actual written text.
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", vars: [ScreensaverPackInit.SaversConfig.SpotWriteWrite]);
                TypeWrite = FilesystemTools.ReadContentsText(ScreensaverPackInit.SaversConfig.SpotWriteWrite);
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
                if (ConsoleWrapper.CursorTop != ConsoleWrapper.WindowHeight - 2)
                {
                    ConsoleWrapper.SetCursorPosition(0, ConsoleWrapper.CursorTop + 1);
                    ConsoleWrapper.Write("    ");
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", vars: [ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop]);
                }

                // Get struck character and write it
                for (int SentenceIndex = 0; SentenceIndex <= IncompleteSentences.Length - 1; SentenceIndex++)
                {
                    string Sentence = IncompleteSentences[SentenceIndex];
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    if (ScreensaverManager.Bailing)
                        return;

                    foreach (char StruckChar in Sentence)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;
                        if (ScreensaverManager.Bailing)
                            return;

                        // If we're at the end of the page, clear the screen
                        if (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 2)
                        {
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're at the end of the page! {0} = {1}", vars: [ConsoleWrapper.CursorTop, ConsoleWrapper.WindowHeight - 2]);
                            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SpotWriteNewScreenDelay);
                            ConsoleWrapper.Clear();
                            ConsoleWrapper.WriteLine();
                            if (SentenceIndex == 0)
                                ConsoleWrapper.Write("    ");
                            else
                                ConsoleWrapper.Write(" ");
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", vars: [ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop]);
                        }

                        // Write the final character to the console and wait
                        ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "[1K" + Convert.ToString(StruckChar) + Convert.ToString(CharManager.GetEsc()) + "[K");
                        ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SpotWriteDelay);
                    }
                    ConsoleWrapper.Write(Convert.ToString(CharManager.GetEsc()) + "[1K");
                }
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SpotWriteDelay);
        }

    }
}
