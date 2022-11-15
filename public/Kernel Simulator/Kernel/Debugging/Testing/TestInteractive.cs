
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using KS.ConsoleBase;
using KS.ConsoleBase.Inputs.Styles;
using KS.Languages;
using System.Collections.Generic;
using System.Linq;

namespace KS.Kernel.Debugging.Testing
{
    internal static class TestInteractive
    {
        internal static bool ShutdownFlag;
        private static bool exiting;
        internal static Dictionary<string, TestFacade> facades = new()
        {
            { "Print",   new Facades.Print() },
            { "PrintF",  new Facades.PrintF() },
            { "PrintD",  new Facades.PrintD() },
            { "PrintDF", new Facades.PrintDF() },
            { "Debug",   new Facades.Debug() }
        };

        internal static void Open()
        {
            exiting = false;

            // List facades and alt options
            int facadeCount = facades.Count;
            var listFacadesCodeNames = facades.Keys.Select((fac) => fac).ToArray();
            var listFacadesAltOptionName = new string[] 
            { 
                Translate.DoTranslation("Exit"),
                Translate.DoTranslation("Shutdown"),
            };
            var listFacadesAltOptionDesc = new string[] 
            { 
                Translate.DoTranslation("Exits the testing mode and starts the kernel"),
                Translate.DoTranslation("Exits the testing mode and shuts down the kernel"),
            };

            // Prompt for facade
            while (!exiting)
            {
                // We need to update the names in case the status updated
                var listFacadesNames = facades.Values.Select((fac) => $"[{(fac.TestStatus == TestStatus.Success ? "+" : fac.TestStatus == TestStatus.Failed ? "X" : "-")}] " + fac.TestName).ToArray();

                // Now, prompt for the selection of the facade
                int sel = SelectionStyle.PromptSelection(Translate.DoTranslation("Choose a test facade to run"), string.Join("/", listFacadesCodeNames), listFacadesNames, string.Join("/", listFacadesAltOptionName), listFacadesAltOptionDesc);
                if (sel <= facadeCount)
                {
                    // Try to...
                    try
                    {
                        // ...test the facade
                        ConsoleWrapper.Clear();
                        facades[listFacadesCodeNames[sel - 1]].status = TestStatus.Neutral;
                        facades[listFacadesCodeNames[sel - 1]].Run();
                    }
                    catch
                    {
                        // Facade failed unexpectedly
                        facades[listFacadesCodeNames[sel - 1]].status = TestStatus.Failed;
                    }

                    // Prompt the user to check to see if the test ran as expected
                    string answer = ChoiceStyle.PromptChoice(Translate.DoTranslation("Did the test run as expected?"), "Y/N");
                    if (answer == "Y")
                        facades[listFacadesCodeNames[sel - 1]].status = TestStatus.Success;
                    else
                        facades[listFacadesCodeNames[sel - 1]].status = TestStatus.Failed;
                }
                else
                {
                    // Selected alternative option. Exit [facadeCount + 1] or shutdown [facadeCount + 2]
                    if (sel >= facadeCount + 2)
                        ShutdownFlag = true;
                    exiting = true;
                }
            }
        }
    }
}
