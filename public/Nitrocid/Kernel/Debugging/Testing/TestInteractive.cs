
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

using KS.ConsoleBase;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
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
            { "Print",                          new Facades.Print() },
            { "PrintF",                         new Facades.PrintF() },
            { "PrintD",                         new Facades.PrintD() },
            { "PrintDF",                        new Facades.PrintDF() },
            { "PrintSep",                       new Facades.PrintSep() },
            { "PrintSepF",                      new Facades.PrintSepF() },
            { "PrintSepColor",                  new Facades.PrintSepColor() },
            { "PrintSepColorF",                 new Facades.PrintSepColorF() },
            { "PrintFiglet",                    new Facades.PrintFiglet() },
            { "PrintFigletF",                   new Facades.PrintFigletF() },
            { "PrintPlaces",                    new Facades.PrintPlaces() },
            { "PrintWithNewLines",              new Facades.PrintWithNewLines() },
            { "PrintWithNulls",                 new Facades.PrintWithNulls() },
            { "Debug",                          new Facades.Debug() },
            { "RDebug",                         new Facades.RDebug() },
            { "TestDictWriterStr",              new Facades.TestDictWriterStr() },
            { "TestDictWriterInt",              new Facades.TestDictWriterInt() },
            { "TestDictWriterChar",             new Facades.TestDictWriterChar() },
            { "TestListWriterStr",              new Facades.TestListWriterStr() },
            { "TestListWriterInt",              new Facades.TestListWriterInt() },
            { "TestListWriterChar",             new Facades.TestListWriterChar() },
            { "TestCRC32",                      new Facades.TestCRC32() },
            { "TestMD5",                        new Facades.TestMD5() },
            { "TestSHA1",                       new Facades.TestSHA1() },
            { "TestSHA256",                     new Facades.TestSHA256() },
            { "TestSHA384",                     new Facades.TestSHA384() },
            { "TestSHA512",                     new Facades.TestSHA512() },
            { "TestArgs",                       new Facades.TestArgs() },
            { "TestSwitches",                   new Facades.TestSwitches() },
            { "TestExecuteAssembly",            new Facades.TestExecuteAssembly() },
            { "TestEvent",                      new Facades.TestEvent() },
            { "TestTable",                      new Facades.TestTable() },
            { "ShowTime",                       new Facades.ShowTime() },
            { "ShowDate",                       new Facades.ShowDate() },
            { "ShowTimeDate",                   new Facades.ShowTimeDate() },
            { "ShowTimeUtc",                    new Facades.ShowTimeUtc() },
            { "ShowDateUtc",                    new Facades.ShowDateUtc() },
            { "ShowTimeDateUtc",                new Facades.ShowTimeDateUtc() },
            { "LoadMods",                       new Facades.LoadMods() },
            { "StopMods",                       new Facades.StopMods() },
            { "ReloadMods",                     new Facades.ReloadMods() },
            { "BlacklistMod",                   new Facades.BlacklistMod() },
            { "BlacklistModUndo",               new Facades.BlacklistModUndo() },
            { "Panic",                          new Facades.Panic() },
            { "PanicF",                         new Facades.PanicF() },
            { "CheckString",                    new Facades.CheckString() },
            { "CheckStrings",                   new Facades.CheckStrings() },
            { "CheckLocalizationLines",         new Facades.CheckLocalizationLines() },
            { "CheckSettingsEntries",           new Facades.CheckSettingsEntries() },
            { "ColorTest",                      new Facades.ColorTest() },
            { "ColorTrueTest",                  new Facades.ColorTrueTest() },
            { "ListCultures",                   new Facades.ListCultures() },
            { "ListCodepages",                  new Facades.ListCodepages() },
            { "BenchmarkSleepOne",              new Facades.BenchmarkSleepOne() },
            { "BenchmarkTickSleepOne",          new Facades.BenchmarkTickSleepOne() },
            { "ProbeHardware",                  new Facades.ProbeHardware() },
            { "EnableNotifications",            new Facades.EnableNotifications() },
            { "SendNotification",               new Facades.SendNotification() },
            { "SendNotificationSimple",         new Facades.SendNotificationSimple() },
            { "SendNotificationProg",           new Facades.SendNotificationProg() },
            { "SendNotificationProgF",          new Facades.SendNotificationProgF() },
            { "ShowDateDiffCalendar",           new Facades.ShowDateDiffCalendar() },
            { "TestTranslate",                  new Facades.TestTranslate() },
            { "GetCustomSaverSetting",          new Facades.GetCustomSaverSetting() },
            { "SetCustomSaverSetting",          new Facades.SetCustomSaverSetting() },
            { "TestRNG",                        new Facades.TestRNG() },
            { "TestCryptoRNG",                  new Facades.TestCryptoRNG() },
            { "TestInputSelection",             new Facades.TestInputSelection() },
            { "TestPresentation",               new Facades.TestPresentation() },
            { "TestPresentationKiosk",          new Facades.TestPresentationKiosk() },
            { "TestPresentationRequired",       new Facades.TestPresentationRequired() },
            { "TestPresentationKioskRequired",  new Facades.TestPresentationKioskRequired() },
            { "InternetCheck",                  new Facades.InternetCheck() },
            { "NetworkCheck",                   new Facades.NetworkCheck() },
            { "LoadSavers",                     new Facades.LoadSavers() },
            { "ChangeLanguage",                 new Facades.ChangeLanguage() },
            { "KernelThreadTest",               new Facades.KernelThreadTest() },
            { "KernelThreadChildTest",          new Facades.KernelThreadChildTest() },
            { "CliInfoPaneTest",                new Facades.CliInfoPaneTest() },
            { "CliInfoPaneTestRefreshing",      new Facades.CliInfoPaneTestRefreshing() },
            { "CliDoublePaneTest",              new Facades.CliDoublePaneTest() },
        };

        internal static void Open()
        {
            exiting = false;

            // List facades and alt options
            int facadeCount = facades.Count;
            var listFacadesCodeNames = facades.Keys.Select((fac) => fac).ToArray();
            var listFacadesAltOptionName = new string[] 
            { 
                Translate.DoTranslation("Test All"),
                Translate.DoTranslation("Stats"),
                Translate.DoTranslation("Exit"),
                Translate.DoTranslation("Shutdown"),
            };
            var listFacadesAltOptionDesc = new string[] 
            { 
                Translate.DoTranslation("Tests all facades"),
                Translate.DoTranslation("Shows the current test statistics"),
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
                    RunFacade(facades[listFacadesCodeNames[sel - 1]]);
                }
                else
                {
                    // Selected alternative option
                    if (sel == facadeCount + 1)
                    {
                        // Test All
                        foreach (var facade in facades.Values)
                            RunFacade(facade);
                        ConsoleWrapper.Clear();
                        PrintTestStats();
                    }
                    else if (sel == facadeCount + 2) 
                    {
                        // Stats
                        PrintTestStats();
                    }
                    else if (sel == facadeCount + 3)
                    {
                        // Exit
                        exiting = true;
                    }
                    else if (sel == facadeCount + 4 || sel == -1)
                    {
                        // Shutdown
                        ShutdownFlag = true;
                        exiting = true;
                    }
                }
            }
        }

        internal static void RunFacade(TestFacade facade)
        {
            // Try to...
            bool tested = false;
            try
            {
                while (!tested)
                {
                    // ...test the facade
                    ConsoleWrapper.Clear();
                    facade.status = TestStatus.Neutral;
                    facade.Run();

                    if (facade.TestInteractive)
                    {
                        // Prompt the user to check to see if the test ran as expected
                        ConsoleWrapper.SetCursorPosition(0, ConsoleWrapper.WindowHeight - 1);
                        string answer = ChoiceStyle.PromptChoice(Translate.DoTranslation("Did the test run as expected?"), "y/n/r");

                        // Set status or retry
                        switch (answer)
                        {
                            case "y":
                                facade.status = TestStatus.Success;
                                tested = true;
                                break;
                            case "r":
                                break;
                            default:
                                facade.status = TestStatus.Failed;
                                tested = true;
                                break;
                        }
                    }
                    else
                    {
                        // Compare the actual value with the expected value
                        if (!facade.TestActualValue.Equals(facade.TestExpectedValue))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("The test failed. Expected value is {0}, but actual value is {1}."), true, ConsoleBase.Colors.KernelColorType.Error, facade.TestExpectedValue.ToString(), facade.TestActualValue.ToString());
                            Input.DetectKeypress();
                            facade.status = TestStatus.Failed;
                            tested = true;
                        }
                        else
                        {
                            facade.status = TestStatus.Success;
                            tested = true;
                        }
                    }
                }
            }
            catch
            {
                // Facade failed unexpectedly
                facade.status = TestStatus.Failed;
            }
        }

        internal static void PrintTestStats()
        {
            TextWriterColor.Write(Translate.DoTranslation("Successful tests:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Success).Count());
            TextWriterColor.Write(Translate.DoTranslation("Failed tests:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Failed).Count());
            TextWriterColor.Write(Translate.DoTranslation("Tests to be run:") + " {0}", facades.Values.Where((fac) => fac.TestStatus == TestStatus.Neutral).Count());
            Input.DetectKeypress();
        }
    }
}
