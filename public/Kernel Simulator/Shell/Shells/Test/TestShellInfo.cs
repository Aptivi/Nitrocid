
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
            { "print", new CommandInfo("print", ShellType.TestShell, "Prints a string to console using color type and line print", new CommandArgumentInfo(new[] { "<Color> <Line> <Message>" }, true, 3), new Test_PrintCommand()) },
            { "printf", new CommandInfo("printf", ShellType.TestShell, "Prints a string to console using color type and line print with format support", new CommandArgumentInfo(new[] { "<Color> <Line> <Variable1;Variable2;Variable3;...> <Message>" }, true, 4), new Test_PrintFCommand()) },
            { "printd", new CommandInfo("printd", ShellType.TestShell, "Prints a string to debugger", new CommandArgumentInfo(new[] { "<Message>" }, true, 1), new Test_PrintDCommand()) },
            { "printdf", new CommandInfo("printdf", ShellType.TestShell, "Prints a string to debugger with format support", new CommandArgumentInfo(new[] { "<Variable1;Variable2;Variable3;...> <Message>" }, true, 2), new Test_PrintDFCommand()) },
            { "printsep", new CommandInfo("printsep", ShellType.TestShell, "Prints a separator", new CommandArgumentInfo(new[] { "<Message>" }, true, 1), new Test_PrintSepCommand()) },
            { "printsepf", new CommandInfo("printsepf", ShellType.TestShell, "Prints a separator with format support", new CommandArgumentInfo(new[] { "<Variable1;Variable2;Variable3;...> <Message>" }, true, 2), new Test_PrintSepCommand()) },
            { "printsepcolor", new CommandInfo("printsepcolor", ShellType.TestShell, "Prints a separator with color support", new CommandArgumentInfo(new[] { "<Color> <Message>" }, true, 2), new Test_PrintSepColorCommand()) },
            { "printsepcolorf", new CommandInfo("printsepcolorf", ShellType.TestShell, "Prints a separator with color and format support", new CommandArgumentInfo(new[] { "<Color> <Variable1;Variable2;Variable3;...> <Message>" }, true, 3), new Test_PrintSepColorFCommand()) },
            { "probehw", new CommandInfo("probehw", ShellType.TestShell, "Tests probing the hardware", new CommandArgumentInfo(), new Test_ProbeHwCommand()) },
            { "panic", new CommandInfo("panic", ShellType.TestShell, "Tests the kernel error facility", new CommandArgumentInfo(new[] { "<ErrorType> <Reboot> <RebootTime> <Description>" }, true, 4), new Test_PanicCommand()) },
            { "panicf", new CommandInfo("panicf", ShellType.TestShell, "Tests the kernel error facility with format support", new CommandArgumentInfo(new[] { "<ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>" }, true, 5), new Test_PanicFCommand()) },
            { "translate", new CommandInfo("translate", ShellType.TestShell, "Tests translating a string that exists in resources to specific language", new CommandArgumentInfo(new[] { "<Lang> <Message>" }, true, 2), new Test_TranslateCommand()) },
            { "places", new CommandInfo("places", ShellType.TestShell, "Prints a string to console and parses the placeholders found", new CommandArgumentInfo(new[] { "<Message>" }, true, 1), new Test_PlacesCommand()) },
            { "testcrc32", new CommandInfo("testcrc32", ShellType.TestShell, "Encrypts a string using CRC32", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_TestCRC32Command()) },
            { "testsha512", new CommandInfo("testsha512", ShellType.TestShell, "Encrypts a string using SHA512", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_TestSHA512Command()) },
            { "testsha384", new CommandInfo("testsha384", ShellType.TestShell, "Encrypts a string using SHA384", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_TestSHA384Command()) },
            { "testsha256", new CommandInfo("testsha256", ShellType.TestShell, "Encrypts a string using SHA256", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_TestSHA256Command()) },
            { "testsha1", new CommandInfo("testsha1", ShellType.TestShell, "Encrypts a string using SHA1", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_TestSHA1Command()) },
            { "testmd5", new CommandInfo("testmd5", ShellType.TestShell, "Encrypts a string using MD5", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_TestMD5Command()) },
            { "testregexp", new CommandInfo("testregexp", ShellType.TestShell, "Tests the regular expression facility", new CommandArgumentInfo(new[] { "<pattern> <string>" }, true, 2), new Test_TestRegExpCommand()) },
            { "loadmods", new CommandInfo("loadmods", ShellType.TestShell, "Starts all mods", new CommandArgumentInfo(), new Test_LoadModsCommand()) },
            { "stopmods", new CommandInfo("stopmods", ShellType.TestShell, "Stops all mods", new CommandArgumentInfo(), new Test_StopModsCommand()) },
            { "reloadmods", new CommandInfo("reloadmods", ShellType.TestShell, "Reloads all mods", new CommandArgumentInfo(), new Test_ReloadModsCommand()) },
            { "blacklistmod", new CommandInfo("blacklistmod", ShellType.TestShell, "Adds a mod to the blacklist", new CommandArgumentInfo(new[] { "<mod>" }, true, 1), new Test_BlacklistModCommand()) },
            { "unblacklistmod", new CommandInfo("unblacklistmod", ShellType.TestShell, "Removes a mod from the blacklist", new CommandArgumentInfo(new[] { "<mod>" }, true, 1), new Test_UnblacklistModCommand()) },
            { "debug", new CommandInfo("debug", ShellType.TestShell, "Enables or disables debug", new CommandArgumentInfo(new[] { "<Enable:True/False>" }, true, 1), new Test_DebugCommand()) },
            { "rdebug", new CommandInfo("rdebug", ShellType.TestShell, "Enables or disables remote debug", new CommandArgumentInfo(new[] { "<Enable:True/False>" }, true, 1), new Test_RDebugCommand()) },
            { "colortest", new CommandInfo("colortest", ShellType.TestShell, "Tests the VT sequence for 255 colors", new CommandArgumentInfo(new[] { "<1-255>" }, true, 1), new Test_ColorTestCommand()) },
            { "colortruetest", new CommandInfo("colortruetest", ShellType.TestShell, "Tests the VT sequence for true color", new CommandArgumentInfo(new[] { "<R;G;B>" }, true, 1), new Test_ColorTrueTestCommand()) },
            { "colorwheel", new CommandInfo("colorwheel", ShellType.TestShell, "Tests the color wheel", new CommandArgumentInfo(), new Test_ColorWheelCommand()) },
            { "sendnot", new CommandInfo("sendnot", ShellType.TestShell, "Sends a notification to test the receiver", new CommandArgumentInfo(new[] { "<Priority> <title> <desc>" }, true, 3), new Test_SendNotCommand()) },
            { "sendnotprog", new CommandInfo("sendnotprog", ShellType.TestShell, "Sends a progress notification to test the receiver", new CommandArgumentInfo(new[] { "<Priority> <title> <desc> <failat>" }, true, 4), new Test_SendNotProgCommand()) },
            { "dcalend", new CommandInfo("dcalend", ShellType.TestShell, "Tests printing date using different calendars", new CommandArgumentInfo(new[] { "<calendar>" }, true, 1), new Test_DCalendCommand()) },
            { "listcodepages", new CommandInfo("listcodepages", ShellType.TestShell, "Lists all supported codepages", new CommandArgumentInfo(), new Test_ListCodePagesCommand()) },
            { "lscompilervars", new CommandInfo("lscompilervars", ShellType.TestShell, "What compiler variables are enabled in the application?", new CommandArgumentInfo(), new Test_LsCompilerVarsCommand()) },
            { "testdictwriterstr", new CommandInfo("testdictwriterstr", ShellType.TestShell, "Tests the dictionary writer with the string and string array", new CommandArgumentInfo(), new Test_TestDictWriterStrCommand()) },
            { "testdictwriterint", new CommandInfo("testdictwriterint", ShellType.TestShell, "Tests the dictionary writer with the integer and integer array", new CommandArgumentInfo(), new Test_TestDictWriterIntCommand()) },
            { "testdictwriterchar", new CommandInfo("testdictwriterchar", ShellType.TestShell, "Tests the dictionary writer with the char and char array", new CommandArgumentInfo(), new Test_TestDictWriterCharCommand()) },
            { "testlistwriterstr", new CommandInfo("testlistwriterstr", ShellType.TestShell, "Tests the list writer with the string and string array", new CommandArgumentInfo(), new Test_TestListWriterStrCommand()) },
            { "testlistwriterint", new CommandInfo("testlistwriterint", ShellType.TestShell, "Tests the list writer with the integer and integer array", new CommandArgumentInfo(), new Test_TestListWriterIntCommand()) },
            { "testlistwriterchar", new CommandInfo("testlistwriterchar", ShellType.TestShell, "Tests the list writer with the char and char array", new CommandArgumentInfo(), new Test_TestListWriterCharCommand()) },
            { "lscultures", new CommandInfo("lscultures", ShellType.TestShell, "Lists supported cultures", new CommandArgumentInfo(new[] { "[search]" }, false, 0), new Test_LsCulturesCommand()) },
            { "getcustomsaversetting", new CommandInfo("getcustomsaversetting", ShellType.TestShell, "Gets custom saver settings", new CommandArgumentInfo(new[] { "<saver> <setting>" }, true, 2), new Test_GetCustomSaverSettingCommand()) },
            { "setcustomsaversetting", new CommandInfo("setcustomsaversetting", ShellType.TestShell, "Sets custom saver settings", new CommandArgumentInfo(new[] { "<saver> <setting> <value>" }, true, 3), new Test_SetCustomSaverSettingCommand()) },
            { "showtime", new CommandInfo("showtime", ShellType.TestShell, "Shows local kernel time", new CommandArgumentInfo(), new Test_ShowTimeCommand()) },
            { "showdate", new CommandInfo("showdate", ShellType.TestShell, "Shows local kernel date", new CommandArgumentInfo(), new Test_ShowDateCommand()) },
            { "showtd", new CommandInfo("showtd", ShellType.TestShell, "Shows local kernel date and time", new CommandArgumentInfo(), new Test_ShowTDCommand()) },
            { "showtimeutc", new CommandInfo("showtimeutc", ShellType.TestShell, "Shows UTC kernel time", new CommandArgumentInfo(), new Test_ShowTimeUtcCommand()) },
            { "showdateutc", new CommandInfo("showdateutc", ShellType.TestShell, "Shows UTC kernel date", new CommandArgumentInfo(), new Test_ShowDateUtcCommand()) },
            { "showtdutc", new CommandInfo("showtdutc", ShellType.TestShell, "Shows UTC kernel date and time", new CommandArgumentInfo(), new Test_ShowTDUtcCommand()) },
            { "testtable", new CommandInfo("testtable", ShellType.TestShell, "Tests the table functionality", new CommandArgumentInfo(new[] { "[margin]" }, false, 0), new Test_TestTableCommand()) },
            { "checkstring", new CommandInfo("checkstring", ShellType.TestShell, "Checks to see if the translatable string exists in the KS resources", new CommandArgumentInfo(new[] { "<string>" }, true, 1), new Test_CheckStringCommand()) },
            { "checksettingsentryvars", new CommandInfo("checksettingsentryvars", ShellType.TestShell, "Checks all the KS settings to see if the variables are written correctly", new CommandArgumentInfo(), new Test_CheckSettingsEntryVarsCommand()) },
            { "checklocallines", new CommandInfo("checklocallines", ShellType.TestShell, "Checks all the localization text line numbers to see if they're all equal", new CommandArgumentInfo(), new Test_CheckLocalLinesCommand()) },
            { "checkstrings", new CommandInfo("checkstrings", ShellType.TestShell, "Checks to see if the translatable strings exist in the KS resources", new CommandArgumentInfo(new[] { "[-missingonly] <stringlistfile>" }, true, 1), new Test_CheckStringsCommand()) },
            { "sleeptook", new CommandInfo("sleeptook", ShellType.TestShell, "How many milliseconds did it really take to sleep?", new CommandArgumentInfo(new[] { "[-t] <sleepms>" }, true, 1), new Test_SleepTookCommand()) },
            { "getlinestyle", new CommandInfo("getlinestyle", ShellType.TestShell, "Gets the line ending style from text file", new CommandArgumentInfo(new[] { "<textfile>" }, true, 1), new Test_GetLineStyleCommand()) },
            { "printfiglet", new CommandInfo("printfiglet", ShellType.TestShell, "Prints a string to console using color type and line print with Figlet support", new CommandArgumentInfo(new[] { "<Color> <FigletFont> <Message>" }, true, 3), new Test_PrintFigletCommand()) },
            { "printfigletf", new CommandInfo("printfigletf", ShellType.TestShell, "Prints a string to console using color type and line print with format and Figlet support", new CommandArgumentInfo(new[] { "<Color> <FigletFont> <Variable1;Variable2;Variable3;...> <Message>" }, true, 4), new Test_PrintFigletFCommand()) },
            { "powerlinetest", new CommandInfo("powerlinetest", ShellType.TestShell, "Tests your console for PowerLine support", new CommandArgumentInfo(), new Test_PowerLineTestCommand()) },
            { "testexecuteasm", new CommandInfo("testexecuteasm", ShellType.TestShell, "Tests assembly entry point execution", new CommandArgumentInfo(new[] { "<pathtoasm>" }, true, 1), new Test_TestExecuteAsmCommand()) },
            { "testevent", new CommandInfo("testevent", ShellType.TestShell, "Tests an event", new CommandArgumentInfo(new[] { "<event>" }, true, 1), new Test_TestEventCommand()) },
            { "testargs", new CommandInfo("testargs", ShellType.TestShell, "Tests arguments", new CommandArgumentInfo(), new Test_TestArgsCommand()) },
            { "testswitches", new CommandInfo("testswitches", ShellType.TestShell, "Tests switches", new CommandArgumentInfo(), new Test_TestSwitchesCommand()) },
            { "start", new CommandInfo("start", ShellType.TestShell, "Exits the test shell and starts the kernel", new CommandArgumentInfo(), new Test_StartCommand()) },
            { "shutdown", new CommandInfo("shutdown", ShellType.TestShell, "Exits the test shell and shuts down the kernel", new CommandArgumentInfo(), new Test_ShutdownCommand()) }
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
