
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

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.Test;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Test.Commands;

namespace KS.Shell.Shells.Test
{
    /// <summary>
    /// Common test shell class
    /// </summary>
    internal class TestShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Test commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "probehw", new CommandInfo("probehw", ShellType.TestShell, "Tests probing the hardware", new CommandArgumentInfo(), new Test_ProbeHwCommand()) },
            { "translate", new CommandInfo("translate", ShellType.TestShell, "Tests translating a string that exists in resources to specific language", new CommandArgumentInfo(new[] { "<Lang> <Message>" }, true, 2), new Test_TranslateCommand()) },
            { "testregexp", new CommandInfo("testregexp", ShellType.TestShell, "Tests the regular expression facility", new CommandArgumentInfo(new[] { "<pattern> <string>" }, true, 2), new Test_TestRegExpCommand()) },
            { "colortest", new CommandInfo("colortest", ShellType.TestShell, "Tests the VT sequence for 255 colors", new CommandArgumentInfo(new[] { "<1-255>" }, true, 1), new Test_ColorTestCommand()) },
            { "colortruetest", new CommandInfo("colortruetest", ShellType.TestShell, "Tests the VT sequence for true color", new CommandArgumentInfo(new[] { "<R;G;B>" }, true, 1), new Test_ColorTrueTestCommand()) },
            { "colorwheel", new CommandInfo("colorwheel", ShellType.TestShell, "Tests the color wheel", new CommandArgumentInfo(), new Test_ColorWheelCommand()) },
            { "sendnot", new CommandInfo("sendnot", ShellType.TestShell, "Sends a notification to test the receiver", new CommandArgumentInfo(new[] { "<Priority> <title> <desc>" }, true, 3), new Test_SendNotCommand()) },
            { "sendnotprog", new CommandInfo("sendnotprog", ShellType.TestShell, "Sends a progress notification to test the receiver", new CommandArgumentInfo(new[] { "<Priority> <title> <desc> <failat>" }, true, 4), new Test_SendNotProgCommand()) },
            { "dcalend", new CommandInfo("dcalend", ShellType.TestShell, "Tests printing date using different calendars", new CommandArgumentInfo(new[] { "<calendar>" }, true, 1), new Test_DCalendCommand()) },
            { "listcodepages", new CommandInfo("listcodepages", ShellType.TestShell, "Lists all supported codepages", new CommandArgumentInfo(), new Test_ListCodePagesCommand()) },
            { "lscompilervars", new CommandInfo("lscompilervars", ShellType.TestShell, "What compiler variables are enabled in the application?", new CommandArgumentInfo(), new Test_LsCompilerVarsCommand()) },
            { "lscultures", new CommandInfo("lscultures", ShellType.TestShell, "Lists supported cultures", new CommandArgumentInfo(new[] { "[search]" }, false, 0), new Test_LsCulturesCommand()) },
            { "getcustomsaversetting", new CommandInfo("getcustomsaversetting", ShellType.TestShell, "Gets custom saver settings", new CommandArgumentInfo(new[] { "<saver> <setting>" }, true, 2), new Test_GetCustomSaverSettingCommand()) },
            { "setcustomsaversetting", new CommandInfo("setcustomsaversetting", ShellType.TestShell, "Sets custom saver settings", new CommandArgumentInfo(new[] { "<saver> <setting> <value>" }, true, 3), new Test_SetCustomSaverSettingCommand()) },
            { "testtable", new CommandInfo("testtable", ShellType.TestShell, "Tests the table functionality", new CommandArgumentInfo(new[] { "[margin]" }, false, 0), new Test_TestTableCommand()) },
            { "checkstring", new CommandInfo("checkstring", ShellType.TestShell, "Checks to see if the translatable string exists in the KS resources", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_CheckStringCommand()) },
            { "checksettingsentryvars", new CommandInfo("checksettingsentryvars", ShellType.TestShell, "Checks all the KS settings to see if the variables are written correctly", new CommandArgumentInfo(), new Test_CheckSettingsEntryVarsCommand()) },
            { "checklocallines", new CommandInfo("checklocallines", ShellType.TestShell, "Checks all the localization text line numbers to see if they're all equal", new CommandArgumentInfo(), new Test_CheckLocalLinesCommand()) },
            { "checkstrings", new CommandInfo("checkstrings", ShellType.TestShell, "Checks to see if the translatable strings exist in the KS resources", new CommandArgumentInfo(new[] { "[-missingonly] <stringlistfile>" }, true, 1), new Test_CheckStringsCommand()) },
            { "sleeptook", new CommandInfo("sleeptook", ShellType.TestShell, "How many milliseconds did it really take to sleep?", new CommandArgumentInfo(new[] { "[-t] <sleepms>" }, true, 1), new Test_SleepTookCommand()) },
            { "getlinestyle", new CommandInfo("getlinestyle", ShellType.TestShell, "Gets the line ending style from text file", new CommandArgumentInfo(new[] { "<textfile>" }, true, 1), new Test_GetLineStyleCommand()) },
            { "powerlinetest", new CommandInfo("powerlinetest", ShellType.TestShell, "Tests your console for PowerLine support", new CommandArgumentInfo(), new Test_PowerLineTestCommand()) },
            { "testexecuteasm", new CommandInfo("testexecuteasm", ShellType.TestShell, "Tests assembly entry point execution", new CommandArgumentInfo(new[] { "<pathtoasm>" }, true, 1), new Test_TestExecuteAsmCommand()) },
            { "testevent", new CommandInfo("testevent", ShellType.TestShell, "Tests an event", new CommandArgumentInfo(new[] { "<event>" }, true, 1), new Test_TestEventCommand()) },
            { "testargs", new CommandInfo("testargs", ShellType.TestShell, "Tests arguments", new CommandArgumentInfo(), new Test_TestArgsCommand()) },
            { "testswitches", new CommandInfo("testswitches", ShellType.TestShell, "Tests switches", new CommandArgumentInfo(), new Test_TestSwitchesCommand()) }
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new TestDefaultPreset() },
            { "PowerLine1", new TestPowerLine1Preset() },
            { "PowerLine2", new TestPowerLine2Preset() },
            { "PowerLine3", new TestPowerLine3Preset() },
            { "PowerLineBG1", new TestPowerLineBG1Preset() },
            { "PowerLineBG2", new TestPowerLineBG2Preset() },
            { "PowerLineBG3", new TestPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new TestShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["TestShell"];

    }
}
