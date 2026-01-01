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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.Kernel.Debugging.Testing.Facades;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Kernel.Debugging.Testing
{
    internal static class TestInteractive
    {
        internal static Dictionary<string, TestFacade> facades = new()
        {
            { "Print",                                          new Print() },
            { "PrintF",                                         new PrintF() },
            { "PrintD",                                         new PrintD() },
            { "PrintDF",                                        new PrintDF() },
            { "PrintSep",                                       new PrintSep() },
            { "PrintSepF",                                      new PrintSepF() },
            { "PrintSepColor",                                  new PrintSepColor() },
            { "PrintSepColorF",                                 new PrintSepColorF() },
            { "PrintPlaces",                                    new PrintPlaces() },
            { "PrintWithNewLines",                              new PrintWithNewLines() },
            { "PrintWithNulls",                                 new PrintWithNulls() },
            { "PrintHighlighted",                               new PrintHighlighted() },
            { "Debug",                                          new Debug() },
            { "RDebug",                                         new RDebug() },
            { "TestDictWriterStr",                              new TestDictWriterStr() },
            { "TestDictWriterInt",                              new TestDictWriterInt() },
            { "TestDictWriterChar",                             new TestDictWriterChar() },
            { "TestListWriterStr",                              new TestListWriterStr() },
            { "TestListWriterInt",                              new TestListWriterInt() },
            { "TestListWriterChar",                             new TestListWriterChar() },
            { "TestListEntryWriter",                            new TestListEntryWriter() },
            { "TestCRC32",                                      new TestCRC32() },
            { "TestMD5",                                        new TestMD5() },
            { "TestSHA1",                                       new TestSHA1() },
            { "TestSHA256",                                     new TestSHA256() },
            { "TestSHA384",                                     new TestSHA384() },
            { "TestSHA512",                                     new TestSHA512() },
            { "TestArgs",                                       new TestArgs() },
            { "TestSwitches",                                   new TestSwitches() },
            { "TestExecuteAssembly",                            new TestExecuteAssembly() },
            { "TestEvent",                                      new TestEvent() },
            { "TestTable",                                      new TestTable() },
            { "ShowTime",                                       new ShowTime() },
            { "ShowDate",                                       new ShowDate() },
            { "ShowTimeDate",                                   new ShowTimeDate() },
            { "ShowTimeUtc",                                    new ShowTimeUtc() },
            { "ShowDateUtc",                                    new ShowDateUtc() },
            { "ShowTimeDateUtc",                                new ShowTimeDateUtc() },
            { "LoadMods",                                       new LoadMods() },
            { "StopMods",                                       new StopMods() },
            { "ReloadMods",                                     new ReloadMods() },
            { "BlacklistMod",                                   new BlacklistMod() },
            { "BlacklistModUndo",                               new BlacklistModUndo() },
            { "CheckString",                                    new CheckString() },
            { "CheckStrings",                                   new CheckStrings() },
            { "CheckLocalizationLines",                         new CheckLocalizationLines() },
            { "CheckSettingsEntries",                           new CheckSettingsEntries() },
            { "ColorTest",                                      new ColorTest() },
            { "ColorTrueTest",                                  new ColorTrueTest() },
            { "ListCultures",                                   new ListCultures() },
            { "ListCodepages",                                  new ListCodepages() },
            { "BenchmarkSleepOne",                              new BenchmarkSleepOne() },
            { "BenchmarkTickSleepOne",                          new BenchmarkTickSleepOne() },
            { "ProbeHardware",                                  new ProbeHardware() },
            { "EnableNotifications",                            new EnableNotifications() },
            { "SendNotification",                               new SendNotification() },
            { "SendNotificationProgIndeterminate",              new SendNotificationProgIndeterminate() },
            { "SendNotificationSimple",                         new SendNotificationSimple() },
            { "SendNotificationProg",                           new SendNotificationProg() },
            { "SendNotificationProgF",                          new SendNotificationProgF() },
            { "DismissNotifications",                           new DismissNotifications() },
            { "TestTranslate",                                  new TestTranslate() },
            { "TestRNG",                                        new TestRNG() },
            { "TestCryptoRNG",                                  new TestCryptoRNG() },
            { "TestInputSelection",                             new TestInputSelection() },
            { "TestInputMultiSelection",                        new TestInputMultiSelection() },
            { "TestInputSelectionLarge",                        new TestInputSelectionLarge() },
            { "TestInputSelectionLargeMultiple",                new TestInputSelectionLargeMultiple() },
            { "TestInputInfoBoxSelection",                      new TestInputInfoBoxSelection() },
            { "TestInputInfoBoxMultiSelection",                 new TestInputInfoBoxMultiSelection() },
            { "TestInputInfoBoxSelectionLarge",                 new TestInputInfoBoxSelectionLarge() },
            { "TestInputInfoBoxSelectionLargeMultiple",         new TestInputInfoBoxSelectionLargeMultiple() },
            { "TestInputInfoBoxButtons",                        new TestInputInfoBoxButtons() },
            { "TestInputInfoBoxInput",                          new TestInputInfoBoxInput() },
            { "TestInputInfoBoxColoredInput",                   new TestInputInfoBoxColoredInput() },
            { "TestInputInfoBoxSelectionTitled",                new TestInputInfoBoxSelectionTitled() },
            { "TestInputInfoBoxMultiSelectionTitled",           new TestInputInfoBoxMultiSelectionTitled() },
            { "TestInputInfoBoxSelectionLargeTitled",           new TestInputInfoBoxSelectionLargeTitled() },
            { "TestInputInfoBoxSelectionLargeMultipleTitled",   new TestInputInfoBoxSelectionLargeMultipleTitled() },
            { "TestInputInfoBoxButtonsTitled",                  new TestInputInfoBoxButtonsTitled() },
            { "TestInputInfoBoxInputTitled",                    new TestInputInfoBoxInputTitled() },
            { "TestInputInfoBoxColoredInputTitled",             new TestInputInfoBoxColoredInputTitled() },
            { "InternetCheck",                                  new InternetCheck() },
            { "NetworkCheck",                                   new NetworkCheck() },
            { "ChangeLanguage",                                 new ChangeLanguage() },
            { "KernelThreadTest",                               new KernelThreadTest() },
            { "KernelThreadChildTest",                          new KernelThreadChildTest() },
            { "CliInfoPaneTest",                                new CliInfoPaneTest() },
            { "CliInfoPaneTestRefreshing",                      new CliInfoPaneTestRefreshing() },
            { "CliDoublePaneTest",                              new CliDoublePaneTest() },
            { "CliInfoPaneSlowTest",                            new CliInfoPaneSlowTest() },
            { "CliInfoPaneSlowTestRefreshing",                  new CliInfoPaneSlowTestRefreshing() },
            { "CliDoublePaneSlowTest",                          new CliDoublePaneSlowTest() },
            { "FetchKernelUpdates",                             new FetchKernelUpdates() },
            { "TestProgressHandler",                            new TestProgressHandler() },
            { "TestScreen",                                     new TestScreen() },
            { "TestFileSelector",                               new TestFileSelector() },
            { "TestFilesSelector",                              new TestFilesSelector() },
            { "TestFolderSelector",                             new TestFolderSelector() },
            { "TestFoldersSelector",                            new TestFoldersSelector() },
        };
        internal static Dictionary<TestSection, string> sections = new()
        {
            { TestSection.ConsoleBase,          /* Localizable */ "Base console tests" },
            { TestSection.Drivers,              /* Localizable */ "Driver tests" },
            { TestSection.Files,                /* Localizable */ "Filesystem tests" },
            { TestSection.Kernel,               /* Localizable */ "Main kernel component tests" },
            { TestSection.Languages,            /* Localizable */ "Language and localization tests" },
            { TestSection.Misc,                 /* Localizable */ "Miscellaneous tests" },
            { TestSection.Modification,         /* Localizable */ "Kernel modification tests" },
            { TestSection.Network,              /* Localizable */ "Network system tests" },
            { TestSection.Shell,                /* Localizable */ "UESH shell tests" },
            { TestSection.Users,                /* Localizable */ "User tests" },
        };
        private static bool exiting;

        internal static void Open()
        {
            exiting = false;

            // List sections and alt options
            int sectionCount = sections.Count;
            var listFacadesCodeNames = sections.Keys.Select((sec) => sec).ToArray();
            var listFacades = sections.Select((kvp) => ($"{kvp.Key}", Translate.DoTranslation(kvp.Value))).ToArray();
            var listFacadesAlt = new (string, string)[]
            {
                (Translate.DoTranslation("Stats"), Translate.DoTranslation("Shows the current test statistics")),
                (Translate.DoTranslation("Shutdown"), Translate.DoTranslation("Exits the testing mode and shuts down the kernel"))
            };

            // Prompt for section
            while (!exiting)
            {
                // Now, prompt for the selection of the section
                int sel = SelectionStyle.PromptSelection(Translate.DoTranslation("Choose a test section to run"), listFacades, listFacadesAlt, true);
                if (sel <= sectionCount)
                {
                    OpenSection(listFacadesCodeNames[sel - 1]);
                }
                else
                {
                    // Selected alternative option
                    if (sel == sectionCount + 1)
                    {
                        // Stats
                        PrintTestStats();
                    }
                    else if (sel == sectionCount + 2 || sel == -1)
                    {
                        // Shutdown
                        exiting = true;
                    }
                }
            }
        }

        internal static void OpenSection(TestSection section)
        {
            bool sectionExiting = false;

            // List facades and alt options
            var facadesList = facades
                .Where((kvp) => kvp.Value.TestSection == section)
                .ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
            int facadeCount = facadesList.Count;
            var listFacadesCodeNames = facadesList.Keys.Select((fac) => fac).ToArray();
            var listFacadesAlt = new (string, string)[]
            {
                (Translate.DoTranslation("Test All"), Translate.DoTranslation("Tests all facades")),
                (Translate.DoTranslation("Stats"), Translate.DoTranslation("Shows the current test statistics")),
                (Translate.DoTranslation("Go Back..."), Translate.DoTranslation("Goes back to the section selection"))
            };

            // Prompt for facade
            while (!sectionExiting)
            {
                // We need to update the names in case the status updated
                var listFacadesFinal = facadesList.Select((fac) =>
                    ($"[{(fac.Value.TestStatus == TestStatus.Success ? "+" : fac.Value.TestStatus == TestStatus.Failed ? "X" : "-")}|" +
                    $"{(fac.Value.TestOptionalParameters > 0 ? "O" : " ")}] " +
                    fac.Key, fac.Value.TestName)).ToArray();

                // Now, prompt for the selection of the facade
                int sel = SelectionStyle.PromptSelection(Translate.DoTranslation("Choose a test facade to run"), listFacadesFinal, listFacadesAlt, true);
                if (sel <= facadeCount)
                {
                    RunFacade(facadesList[listFacadesCodeNames[sel - 1]]);
                }
                else
                {
                    // Selected alternative option
                    if (sel == facadeCount + 1)
                    {
                        // Test All
                        foreach (var facade in facadesList.Values)
                            RunFacade(facade);
                        ConsoleWrapper.Clear();
                        PrintTestStats();
                    }
                    else if (sel == facadeCount + 2)
                    {
                        // Stats
                        PrintTestStatsSection(section);
                    }
                    else if (sel == facadeCount + 3 || sel == -1)
                    {
                        // Go back
                        sectionExiting = true;
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
                    // Check for optional test parameters
                    List<string> parameters = [];
                    for (int i = 0; i < facade.TestOptionalParameters; i++)
                    {
                        string answer = InfoBoxInputColor.WriteInfoBoxInput(
                            Translate.DoTranslation("This test contains optional parameters. Enter the value of the parameter to add to the test parameters.") + "\n" +
                            Translate.DoTranslation("Parameters count") + $": {i + 1}/{facade.TestOptionalParameters}"
                        );
                        parameters.Add(answer);
                    }

                    // ...test the facade
                    ConsoleWrapper.Clear();
                    facade.status = TestStatus.Neutral;
                    facade.Run([.. parameters]);

                    if (facade.TestInteractive)
                    {
                        // Prompt the user to check to see if the test ran as expected
                        int answer = InfoBoxButtonsColor.WriteInfoBoxButtons(
                            [
                                new InputChoiceInfo("yes", Translate.DoTranslation("Yes!")),
                                new InputChoiceInfo("no", Translate.DoTranslation("No")),
                                new InputChoiceInfo("retry", Translate.DoTranslation("Retry")),
                            ],
                            Translate.DoTranslation("Did the test run as expected?")
                        );

                        // Set status or retry
                        switch (answer)
                        {
                            case 0:
                                facade.status = TestStatus.Success;
                                tested = true;
                                break;
                            case 2:
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
                        if (facade.TestActualValue is null || !facade.TestActualValue.Equals(facade.TestExpectedValue))
                        {
                            InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("The test failed. Expected value is {0}, but actual value is {1}."), KernelColorTools.GetColor(KernelColorType.Error), facade.TestExpectedValue?.ToString() ?? "<null>", facade.TestActualValue?.ToString() ?? "<null>");
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
            catch (Exception ex)
            {
                // Facade failed unexpectedly
                InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("The test failed unexpectedly.") + $" {ex.Message}", KernelColorTools.GetColor(KernelColorType.Error));
                facade.status = TestStatus.Failed;
            }
        }

        internal static void PrintTestStatsSection(TestSection section)
        {
            int successCount = facades.Values.Where((fac) => fac.TestStatus == TestStatus.Success && fac.TestSection == section).Count();
            int failureCount = facades.Values.Where((fac) => fac.TestStatus == TestStatus.Failed && fac.TestSection == section).Count();
            int neutralCount = facades.Values.Where((fac) => fac.TestStatus == TestStatus.Neutral && fac.TestSection == section).Count();
            InfoBoxModalColor.WriteInfoBoxModal(
                "  * " + Translate.DoTranslation("Successful tests:") + " {0}\n" +
                "  * " + Translate.DoTranslation("Failed tests:") + " {1}\n" +
                "  * " + Translate.DoTranslation("Tests to be run:") + " {2}",
                successCount, failureCount, neutralCount);
        }

        internal static void PrintTestStats()
        {
            int successCount = facades.Values.Where((fac) => fac.TestStatus == TestStatus.Success).Count();
            int failureCount = facades.Values.Where((fac) => fac.TestStatus == TestStatus.Failed).Count();
            int neutralCount = facades.Values.Where((fac) => fac.TestStatus == TestStatus.Neutral).Count();
            InfoBoxModalColor.WriteInfoBoxModal(
                "  * " + Translate.DoTranslation("Successful tests:") + " {0}\n" +
                "  * " + Translate.DoTranslation("Failed tests:") + " {1}\n" +
                "  * " + Translate.DoTranslation("Tests to be run:") + " {2}",
                successCount, failureCount, neutralCount);
        }
    }
}
