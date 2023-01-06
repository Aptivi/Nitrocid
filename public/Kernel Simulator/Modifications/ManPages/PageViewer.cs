
// Kernel Simulator  Copyright (C) 2018-2019  Aptivi
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

using System;
using System.Collections.Generic;
using System.Text;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.ConsoleBase.Inputs;
using VT.NET;
using KS.ConsoleBase;

namespace KS.Modifications.ManPages
{
    /// <summary>
    /// Mod manual page viewer module
    /// </summary>
    public static class PageViewer
    {

        /// <summary>
        /// Manual page information style
        /// </summary>
        public static string ManpageInfoStyle = "";

        /// <summary>
        /// Previews the manual page
        /// </summary>
        /// <param name="ManualTitle">A manual title</param>
        public static void ViewPage(string ManualTitle)
        {
            if (PageManager.Pages.ContainsKey(ManualTitle))
            {
                // Variables
                int InfoPlace;

                // Get the bottom place
                InfoPlace = ConsoleWrapper.WindowHeight - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Bottom info height is {0}", InfoPlace);

                // If there is any To-do, write them to the console
                DebugWriter.WriteDebug(DebugLevel.I, "Todo count for \"{0}\": {1}", ManualTitle, PageManager.Pages[ManualTitle].Todos.Count.ToString());
                if (PageManager.Pages[ManualTitle].Todos.Count != 0)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Todos are found in manpage.");
                    TextWriterColor.Write(Translate.DoTranslation("This manual page needs work for:"), true, KernelColorType.Warning);
                    ListWriterColor.WriteList(PageManager.Pages[ManualTitle].Todos, true);
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Press any key to read the manual page..."), false, KernelColorType.Warning);
                    Input.DetectKeypress();
                }

                // Clear screen for readability
                ConsoleWrapper.Clear();

                // Write the information to the console
                if (!string.IsNullOrWhiteSpace(ManpageInfoStyle))
                {
                    TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ManpageInfoStyle), ConsoleWrapper.CursorLeft, InfoPlace, true, ColorTools.GetColor(KernelColorType.Background), ColorTools.GetColor(KernelColorType.NeutralText), PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
                }
                else
                {
                    TextWriterWhereColor.WriteWhere(" {0} [v{1}] ", ConsoleWrapper.CursorLeft, InfoPlace, true, ColorTools.GetColor(KernelColorType.Background), ColorTools.GetColor(KernelColorType.NeutralText), PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
                }

                // Disable blinking cursor
                ConsoleWrapper.CursorVisible = false;

                // Split the sentences to parts to deal with sentence lengths that are longer than the console window width
                var IncompleteSentences = new List<string>();
                var IncompleteSentenceBuilder = new StringBuilder();
                foreach (string line in PageManager.Pages[ManualTitle].Body.ToString().SplitNewLines())
                {
                    // Deal with empty lines
                    if (string.IsNullOrEmpty(line))
                    {
                        IncompleteSentences.Add("");
                        continue;
                    }

                    // Now, enumerate through each character in the string
                    for (int i = 0; i < line.Length; i++)
                    {
                        char LineChar = line[i];

                        // Append the character into the incomplete sentence builder.
                        IncompleteSentenceBuilder.Append(LineChar);

                        // Check to see if the length of the VT-filtered built string is equal or greater than the console width
                        string built = IncompleteSentenceBuilder.ToString();
                        string vtFilteredBuilt = Filters.FilterVTSequences(built);
                        if (vtFilteredBuilt.Length >= ConsoleWrapper.WindowWidth ||
                            i == line.Length - 1)
                        {
                            // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                            DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString(), IncompleteSentences.Count);
                            IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

                            // Clean everything up
                            IncompleteSentenceBuilder.Clear();
                        }
                    }
                }

                // Write the body
                foreach (string line in IncompleteSentences)
                {
                    // Write the line
                    int OldTop = ConsoleWrapper.CursorTop + 1;
                    TextWriterColor.Write(line);
                    if (OldTop != ConsoleWrapper.CursorTop)
                        ConsoleWrapper.CursorTop = OldTop;

                    // Check to see if we're at the end
                    if (ConsoleWrapper.CursorTop == InfoPlace - 1)
                    {
                        var PressedKey = Input.DetectKeypress();
                        if (PressedKey.Key == ConsoleKey.Escape)
                        {
                            ConsoleWrapper.Clear();
                            return;
                        }
                        else
                        {
                            ConsoleWrapper.Clear();
                            if (!string.IsNullOrWhiteSpace(ManpageInfoStyle))
                            {
                                TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(ManpageInfoStyle), ConsoleWrapper.CursorLeft, InfoPlace, true, ColorTools.GetColor(KernelColorType.Background), ColorTools.GetColor(KernelColorType.NeutralText), PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
                            }
                            else
                            {
                                TextWriterWhereColor.WriteWhere(" {0} (v{1}) ", ConsoleWrapper.CursorLeft, InfoPlace, true, ColorTools.GetColor(KernelColorType.Background), ColorTools.GetColor(KernelColorType.NeutralText), PageManager.Pages[ManualTitle].Title, PageManager.Pages[ManualTitle].Revision);
                            }
                        }
                    }
                }

                // Stop on last page
                DebugWriter.WriteDebug(DebugLevel.I, "We're on the last page.");
                Input.DetectKeypress();

                // Clean up
                DebugWriter.WriteDebug(DebugLevel.I, "Exiting...");
                ConsoleWrapper.Clear();
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Manual page {0} not found."), ManualTitle);
            }
        }

    }
}
