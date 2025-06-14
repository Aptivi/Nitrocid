//
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

using Terminaux.Inputs.Styles.Selection;
using Nitrocid.Kernel.Debugging.Testing.Facades;
using Nitrocid.Languages;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.TestFixtures;
using Terminaux.Inputs.TestFixtures.Tools;

namespace Nitrocid.Kernel.Debugging.Testing
{
    internal static class TestInteractive
    {
        internal static TestFacade[] facades =
        [
            new Print(),
            new PrintF(),
            new PrintD(),
            new PrintDF(),
            new PrintSep(),
            new PrintSepF(),
            new PrintPlaces(),
            new PrintWithNewLines(),
            new PrintWithNulls(),
            new PrintHighlighted(),
            new Debug(),
            new RDebug(),
            new TestDictWriterStr(),
            new TestDictWriterInt(),
            new TestDictWriterChar(),
            new TestListWriterStr(),
            new TestListWriterInt(),
            new TestListWriterChar(),
            new TestListEntryWriter(),
            new TestCRC32(),
            new TestMD5(),
            new TestSHA1(),
            new TestSHA256(),
            new TestSHA384(),
            new TestSHA512(),
            new TestArgs(),
            new TestSwitches(),
            new TestExecuteAssembly(),
            new TestEvent(),
            new TestTable(),
            new ShowTime(),
            new ShowDate(),
            new ShowTimeDate(),
            new ShowTimeUtc(),
            new ShowDateUtc(),
            new ShowTimeDateUtc(),
            new CheckString(),
            new CheckStrings(),
            new CheckLocalizationLines(),
            new CheckSettingsEntries(),
            new ColorTest(),
            new ColorTrueTest(),
            new ListCultures(),
            new ListCodepages(),
            new BenchmarkSleepOne(),
            new BenchmarkTickSleepOne(),
            new ProbeHardware(),
            new EnableNotifications(),
            new SendNotification(),
            new SendNotificationProgIndeterminate(),
            new SendNotificationSimple(),
            new SendNotificationProg(),
            new SendNotificationProgF(),
            new DismissNotifications(),
            new TestTranslate(),
            new TestRNG(),
            new TestCryptoRNG(),
            new TestInputSelection(),
            new TestInputMultiSelection(),
            new TestInputSelectionLarge(),
            new TestInputSelectionLargeMultiple(),
            new TestInputInfoBoxSelection(),
            new TestInputInfoBoxMultiSelection(),
            new TestInputInfoBoxSelectionLarge(),
            new TestInputInfoBoxSelectionLargeMultiple(),
            new TestInputInfoBoxButtons(),
            new TestInputInfoBoxInput(),
            new TestInputInfoBoxColoredInput(),
            new TestInputInfoBoxSelectionTitled(),
            new TestInputInfoBoxMultiSelectionTitled(),
            new TestInputInfoBoxSelectionLargeTitled(),
            new TestInputInfoBoxSelectionLargeMultipleTitled(),
            new TestInputInfoBoxButtonsTitled(),
            new TestInputInfoBoxInputTitled(),
            new TestInputInfoBoxColoredInputTitled(),
            new InternetCheck(),
            new NetworkCheck(),
            new ChangeLanguage(),
            new KernelThreadTest(),
            new KernelThreadChildTest(),
            new CliInfoPaneTest(),
            new CliInfoPaneTestRefreshing(),
            new CliDoublePaneTest(),
            new CliInfoPaneSlowTest(),
            new CliInfoPaneSlowTestRefreshing(),
            new CliDoublePaneSlowTest(),
            new FetchKernelUpdates(),
            new TestProgressHandler(),
            new TestScreen(),
            new TestFileSelector(),
            new TestFilesSelector(),
            new TestFolderSelector(),
            new TestFoldersSelector(),
        ];

        internal static Dictionary<TestSection, string> Sections => new()
        {
            { TestSection.ConsoleBase,          LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_CONSOLEBASE") },
            { TestSection.Drivers,              LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_DRIVER") },
            { TestSection.Files,                LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_FILESYSTEM") },
            { TestSection.Kernel,               LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_KERNEL") },
            { TestSection.Languages,            LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_LANGUAGES") },
            { TestSection.Misc,                 LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_MISC") },
            { TestSection.Modification,         LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_MODIFICATION") },
            { TestSection.Network,              LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_NETWORK") },
            { TestSection.Shell,                LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_SHELL") },
            { TestSection.Users,                LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_USERS") },
        };
        private static bool exiting;

        internal static void Open()
        {
            exiting = false;

            // List sections and alt options
            int sectionCount = Sections.Count;
            var listFacadesCodeNames = Sections.Keys.Select((sec) => sec).ToArray();
            var listFacades = Sections.Select((kvp) => ($"{kvp.Key}", kvp.Value)).ToArray();
            var listFacadesAlt = new (string, string)[]
            {
                (LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_ALTCHOICE_SHUTDOWN"), LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_ALTCHOICE_SHUTDOWN_DESC"))
            };

            // Prompt for section
            while (!exiting)
            {
                // Now, prompt for the selection of the section
                int sel = SelectionStyle.PromptSelection(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTIONPROMPT"), listFacades, listFacadesAlt, true);
                if (sel <= sectionCount)
                    OpenSection(listFacadesCodeNames[sel - 1]);
                else
                {
                    // Selected alternative option
                    if (sel == sectionCount + 1 || sel == -1)
                        exiting = true;
                }
            }
        }

        internal static void OpenSection(TestSection section)
        {
            var facadesList = facades
                .Where((facade) => facade.TestSection == section)
                .Select((facade) => (Fixture)
                    (facade.TestInteractive ?
                     new FixtureUnconditional(facade.GetType().Name, facade.TestName, facade.Run) :
                     new FixtureConditional(facade.GetType().Name, facade.TestName, () =>
                     {
                         facade.Run();
                         return facade.TestActualValue;
                     }, facade.TestExpectedValue))).ToArray();
            FixtureSelector.OpenFixtureSelector(facadesList);
        }
    }
}
