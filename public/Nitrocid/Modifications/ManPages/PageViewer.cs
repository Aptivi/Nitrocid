
// Nitrocid KS  Copyright (C) 2018-2019  Aptivi
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

using System;
using System.Collections.Generic;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase;
using KS.Misc.Presentation;
using KS.Misc.Presentation.Elements;

namespace KS.Modifications.ManPages
{
    /// <summary>
    /// Mod manual page viewer module
    /// </summary>
    public static class PageViewer
    {

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
                var man = PageManager.Pages[ManualTitle];

                // Get the bottom place
                InfoPlace = ConsoleWrapper.WindowHeight - 1;
                DebugWriter.WriteDebug(DebugLevel.I, "Bottom info height is {0}", InfoPlace);

                // If there is any To-do, write them to the console
                DebugWriter.WriteDebug(DebugLevel.I, "Todo count for \"{0}\": {1}", ManualTitle, man.Todos.Count.ToString());
                if (man.Todos.Count != 0)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Todos are found in manpage.");
                    TextWriterColor.Write(Translate.DoTranslation("This manual page needs work for:"), true, KernelColorType.Warning);
                    ListWriterColor.WriteList(man.Todos, true);
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Press any key to read the manual page..."), false, KernelColorType.Warning);
                    Input.DetectKeypress();
                }

                // Prepare the presentation for the manual page
                var manPres = new Presentation
                (
                    $" {man.Title} [v{man.Revision}] ",
                    new List<PresentationPage>()
                    {
                        new PresentationPage
                        (
                            // Kept blank because manual pages don't currently natively support page names
                            "",
                            new List<IElement>()
                            {
                                new TextElement()
                                {
                                    Arguments = new object[] { man.Body.ToString() }
                                }
                            }
                        )
                    }
                );

                // Display!
                PresentationTools.Present(manPres);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Manual page {0} not found."), ManualTitle);
            }
        }

    }
}
