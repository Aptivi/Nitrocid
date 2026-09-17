//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using Magico.Enumeration;
using Nitrocid.Base.Kernel.Debugging.Testing.Facades;
using Nitrocid.Base.Languages;
using Terminaux.Base.Buffered;
using Terminaux.Themes.Colors;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.TestFixtures;
using Terminaux.Inputs.TestFixtures.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs;

namespace Nitrocid.Base.Kernel.Debugging.Testing
{
    internal class TestInteractive : BaseInteractiveTui<TestSection, Fixture>, IInteractiveTui<TestSection, Fixture>
    {
        internal static Fixture[]? fixtures;
        internal static TestFacade[] facades =
        [
            new PrintD(),
            new PrintDF(),
            new Debug(),
            new RDebug(),
            new TestCRC32(),
            new TestMD5(),
            new TestSHA1(),
            new TestSHA256(),
            new TestSHA384(),
            new TestSHA512(),
            new TestExecuteAssembly(),
            new TestEvent(),
            new ShowTime(),
            new ShowDate(),
            new ShowTimeDate(),
            new ShowTimeUtc(),
            new ShowDateUtc(),
            new ShowTimeDateUtc(),
            new CheckLocalizationLines(),
            new CheckSettingsEntries(),
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
            new InternetCheck(),
            new NetworkCheck(),
            new ChangeLanguage(),
            new ThreadInstanceTest(),
            new ThreadInstanceChildTest(),
            new FetchKernelUpdates(),
            new TestProgressHandler(),
            new TestFileSelector(),
            new TestFilesSelector(),
            new TestFolderSelector(),
            new TestFoldersSelector(),
            new WidgetCanvasRenderTest(),
        ];

        internal static Dictionary<TestSection, string> Sections => new()
        {
            { TestSection.ConsoleBase,          LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_CONSOLEBASE") },
            { TestSection.Drivers,              LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_DRIVER") },
            { TestSection.Files,                LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_FILESYSTEM") },
            { TestSection.Kernel,               LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_KERNEL") },
            { TestSection.Languages,            LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_LANGUAGES") },
            { TestSection.Misc,                 LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_MISC") },
            { TestSection.Network,              LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_SECTION_NETWORK") },
        };

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_KERNEL_DEBUGGING_TESTING_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_KERNEL_DEBUGGING_TESTING_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/diagnostics/testing",
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<TestSection> PrimaryDataSource =>
            Sections.Keys;

        /// <inheritdoc/>
        public override IEnumerable<Fixture> SecondaryDataSource
        {
            get
            {
                var section = (TestSection?)PrimaryDataSource.GetElementFromIndex(FirstPaneCurrentSelection - 1);
                if (section is null)
                    return [];
                return GetSectionInfo((TestSection)section);
            }
        }

        /// <inheritdoc/>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(TestSection section)
        {
            // Get a list of fixtures
            var fixtures = GetSectionInfo(section);
            return $"[{fixtures.Length}] {section}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(TestSection section) =>
            section.ToString();

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary(Fixture fixture)
        {
            var fixtureName = fixture.Name;
            var fixtureDescription = fixture.Description;
            return $"{fixtureName} - {fixtureDescription}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary(Fixture fixture) =>
            fixture.Name;

        internal void RunTest(Fixture? fixture)
        {
            if (fixture is null)
                return;
            var currentScreen = ScreenTools.CurrentScreen;
            if (currentScreen is null)
                return;
            try
            {
                ScreenTools.UnsetCurrent(currentScreen);
                ThemeColorsTools.LoadBackground();
                bool result = FixtureRunner.RunGeneralTest(fixture, out var exc, args: null);
                if (result)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTSUCCEEDED"), ThemeColorType.Success);
                    if (fixture.GetType() == typeof(FixtureConditional) || fixture.GetType().BaseType == typeof(FixtureConditional))
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTSUCCEEDED_MATCH"), ThemeColorType.Success);
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTFAILED"), ThemeColorType.Error);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTFAILED_MESSAGE") + ": " + exc?.Message ?? LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTUNKNOWNERROR"), ThemeColorType.Error);
                    if (fixture.GetType() == typeof(FixtureConditional) || fixture.GetType().BaseType == typeof(FixtureConditional))
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTFAILED_MATCH"), ThemeColorType.Error);
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTFAILED_MESSAGE") + ": " + ex?.Message ?? LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_TESTUNKNOWNERROR"), ThemeColorType.Error);
            }
            finally
            {
                ScreenTools.SetCurrent(currentScreen);
                Input.ReadKey();
            }
        }

        internal static Fixture[] GetSectionInfo(TestSection section)
        {
            fixtures = [.. facades
                .Where((facade) => facade.TestSection == section)
                .Select((facade) => (Fixture)
                    (facade.TestInteractive ?
                     new FixtureUnconditional(facade.GetType().Name, facade.TestName, facade.Run) :
                     new FixtureConditional(facade.GetType().Name, facade.TestName, () =>
                     {
                         facade.Run();
                         return facade.TestActualValue;
                     }, facade.TestExpectedValue)))];
            return fixtures;
        }

        internal static void OpenTestInteractiveCli()
        {
            var tui = new TestInteractive();
            tui.Bindings.Add(new InteractiveTuiBinding<TestSection, Fixture>(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTING_RUNTEST"), ConsoleKey.Enter, (_, _, fixture, _) => tui.RunTest(fixture), true));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
