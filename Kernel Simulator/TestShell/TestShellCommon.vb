﻿
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.TestShell.Commands

Namespace TestShell
    Module TestShellCommon

        Friend ReadOnly Test_ModCommands As New Dictionary(Of String, CommandInfo)
        Public ReadOnly Test_Commands As New Dictionary(Of String, CommandInfo) From {
            {"print", New CommandInfo("print", ShellType.TestShell, "Prints a string to console using color type and line print", New CommandArgumentInfo({"<Color> <Line> <Message>"}, True, 3), New Test_PrintCommand)},
            {"printf", New CommandInfo("printf", ShellType.TestShell, "Prints a string to console using color type and line print with format support", New CommandArgumentInfo({"<Color> <Line> <Variable1;Variable2;Variable3;...> <Message>"}, True, 4), New Test_PrintFCommand)},
            {"printd", New CommandInfo("printd", ShellType.TestShell, "Prints a string to debugger", New CommandArgumentInfo({"<Message>"}, True, 1), New Test_PrintDCommand)},
            {"printdf", New CommandInfo("printdf", ShellType.TestShell, "Prints a string to debugger with format support", New CommandArgumentInfo({"<Variable1;Variable2;Variable3;...> <Message>"}, True, 2), New Test_PrintDFCommand)},
            {"printsep", New CommandInfo("printsep", ShellType.TestShell, "Prints a separator", New CommandArgumentInfo({"<Message>"}, True, 1), New Test_PrintSepCommand)},
            {"printsepf", New CommandInfo("printsepf", ShellType.TestShell, "Prints a separator with format support", New CommandArgumentInfo({"<Variable1;Variable2;Variable3;...> <Message>"}, True, 2), New Test_PrintSepCommand)},
            {"printsepcolor", New CommandInfo("printsepcolor", ShellType.TestShell, "Prints a separator with color support", New CommandArgumentInfo({"<Color> <Message>"}, True, 2), New Test_PrintSepColorCommand)},
            {"printsepcolorf", New CommandInfo("printsepcolorf", ShellType.TestShell, "Prints a separator with color and format support", New CommandArgumentInfo({"<Color> <Variable1;Variable2;Variable3;...> <Message>"}, True, 3), New Test_PrintSepColorFCommand)},
            {"testevent", New CommandInfo("testevent", ShellType.TestShell, "Tests raising the specific event", New CommandArgumentInfo({"<event>"}, True, 1), New Test_TestEventCommand)},
            {"probehw", New CommandInfo("probehw", ShellType.TestShell, "Tests probing the hardware", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ProbeHwCommand)},
            {"panic", New CommandInfo("panic", ShellType.TestShell, "Tests the kernel error facility", New CommandArgumentInfo({"<ErrorType> <Reboot> <RebootTime> <Description>"}, True, 4), New Test_PanicCommand)},
            {"panicf", New CommandInfo("panicf", ShellType.TestShell, "Tests the kernel error facility with format support", New CommandArgumentInfo({"<ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>"}, True, 5), New Test_PanicFCommand)},
            {"translate", New CommandInfo("translate", ShellType.TestShell, "Tests translating a string that exists in resources to specific language", New CommandArgumentInfo({"<Lang> <Message>"}, True, 2), New Test_TranslateCommand)},
            {"places", New CommandInfo("places", ShellType.TestShell, "Prints a string to console and parses the placeholders found", New CommandArgumentInfo({"<Message>"}, True, 1), New Test_PlacesCommand)},
            {"testcrc32", New CommandInfo("testcrc32", ShellType.TestShell, "Encrypts a string using CRC32", New CommandArgumentInfo({"<string>"}, True, 1), New Test_TestCRC32Command)},
            {"testsha512", New CommandInfo("testsha512", ShellType.TestShell, "Encrypts a string using SHA512", New CommandArgumentInfo({"<string>"}, True, 1), New Test_TestSHA512Command)},
            {"testsha384", New CommandInfo("testsha384", ShellType.TestShell, "Encrypts a string using SHA384", New CommandArgumentInfo({"<string>"}, True, 1), New Test_TestSHA384Command)},
            {"testsha256", New CommandInfo("testsha256", ShellType.TestShell, "Encrypts a string using SHA256", New CommandArgumentInfo({"<string>"}, True, 1), New Test_TestSHA256Command)},
            {"testsha1", New CommandInfo("testsha1", ShellType.TestShell, "Encrypts a string using SHA1", New CommandArgumentInfo({"<string>"}, True, 1), New Test_TestSHA1Command)},
            {"testmd5", New CommandInfo("testmd5", ShellType.TestShell, "Encrypts a string using MD5", New CommandArgumentInfo({"<string>"}, True, 1), New Test_TestMD5Command)},
            {"testregexp", New CommandInfo("testregexp", ShellType.TestShell, "Tests the regular expression facility", New CommandArgumentInfo({"<pattern> <string>"}, True, 2), New Test_TestRegExpCommand)},
            {"loadmods", New CommandInfo("loadmods", ShellType.TestShell, "Starts all mods", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_LoadModsCommand)},
            {"stopmods", New CommandInfo("stopmods", ShellType.TestShell, "Stops all mods", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_StopModsCommand)},
            {"reloadmods", New CommandInfo("reloadmods", ShellType.TestShell, "Reloads all mods", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ReloadModsCommand)},
            {"blacklistmod", New CommandInfo("blacklistmod", ShellType.TestShell, "Adds a mod to the blacklist", New CommandArgumentInfo({"<mod>"}, True, 1), New Test_BlacklistModCommand)},
            {"unblacklistmod", New CommandInfo("unblacklistmod", ShellType.TestShell, "Removes a mod from the blacklist", New CommandArgumentInfo({"<mod>"}, True, 1), New Test_UnblacklistModCommand)},
            {"debug", New CommandInfo("debug", ShellType.TestShell, "Enables or disables debug", New CommandArgumentInfo({"<Enable:True/False>"}, True, 1), New Test_DebugCommand)},
            {"rdebug", New CommandInfo("rdebug", ShellType.TestShell, "Enables or disables remote debug", New CommandArgumentInfo({"<Enable:True/False>"}, True, 1), New Test_RDebugCommand)},
            {"colortest", New CommandInfo("colortest", ShellType.TestShell, "Tests the VT sequence for 255 colors", New CommandArgumentInfo({"<1-255>"}, True, 1), New Test_ColorTestCommand)},
            {"colortruetest", New CommandInfo("colortruetest", ShellType.TestShell, "Tests the VT sequence for true color", New CommandArgumentInfo({"<R;G;B>"}, True, 1), New Test_ColorTrueTestCommand)},
            {"colorwheel", New CommandInfo("colorwheel", ShellType.TestShell, "Tests the color wheel", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ColorWheelCommand)},
            {"sendnot", New CommandInfo("sendnot", ShellType.TestShell, "Sends a notification to test the receiver", New CommandArgumentInfo({"<Priority> <title> <desc>"}, True, 3), New Test_SendNotCommand)},
            {"sendnotprog", New CommandInfo("sendnotprog", ShellType.TestShell, "Sends a progress notification to test the receiver", New CommandArgumentInfo({"<Priority> <title> <desc> <failat>"}, True, 4), New Test_SendNotProgCommand)},
            {"dcalend", New CommandInfo("dcalend", ShellType.TestShell, "Tests printing date using different calendars", New CommandArgumentInfo({"<calendar>"}, True, 1), New Test_DCalendCommand)},
            {"listcodepages", New CommandInfo("listcodepages", ShellType.TestShell, "Lists all supported codepages", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ListCodePagesCommand)},
            {"lscompilervars", New CommandInfo("lscompilervars", ShellType.TestShell, "What compiler variables are enabled in the application?", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_LsCompilerVarsCommand)},
            {"testdictwriterstr", New CommandInfo("testdictwriterstr", ShellType.TestShell, "Tests the dictionary writer with the string and string array", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_TestDictWriterStrCommand)},
            {"testdictwriterint", New CommandInfo("testdictwriterint", ShellType.TestShell, "Tests the dictionary writer with the integer and integer array", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_TestDictWriterIntCommand)},
            {"testdictwriterchar", New CommandInfo("testdictwriterchar", ShellType.TestShell, "Tests the dictionary writer with the char and char array", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_TestDictWriterCharCommand)},
            {"testlistwriterstr", New CommandInfo("testlistwriterstr", ShellType.TestShell, "Tests the list writer with the string and string array", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_TestListWriterStrCommand)},
            {"testlistwriterint", New CommandInfo("testlistwriterint", ShellType.TestShell, "Tests the list writer with the integer and integer array", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_TestListWriterIntCommand)},
            {"testlistwriterchar", New CommandInfo("testlistwriterchar", ShellType.TestShell, "Tests the list writer with the char and char array", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_TestListWriterCharCommand)},
            {"lscultures", New CommandInfo("lscultures", ShellType.TestShell, "Lists supported cultures", New CommandArgumentInfo({"[search]"}, False, 0), New Test_LsCulturesCommand)},
            {"getcustomsaversetting", New CommandInfo("getcustomsaversetting", ShellType.TestShell, "Gets custom saver settings", New CommandArgumentInfo({"<saver> <setting>"}, True, 2), New Test_GetCustomSaverSettingCommand)},
            {"setcustomsaversetting", New CommandInfo("setcustomsaversetting", ShellType.TestShell, "Sets custom saver settings", New CommandArgumentInfo({"<saver> <setting> <value>"}, True, 3), New Test_SetCustomSaverSettingCommand)},
            {"showtime", New CommandInfo("showtime", ShellType.TestShell, "Shows local kernel time", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ShowTimeCommand)},
            {"showdate", New CommandInfo("showdate", ShellType.TestShell, "Shows local kernel date", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ShowDateCommand)},
            {"showtd", New CommandInfo("showtd", ShellType.TestShell, "Shows local kernel date and time", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ShowTDCommand)},
            {"showtimeutc", New CommandInfo("showtimeutc", ShellType.TestShell, "Shows UTC kernel time", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ShowTimeUtcCommand)},
            {"showdateutc", New CommandInfo("showdateutc", ShellType.TestShell, "Shows UTC kernel date", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ShowDateUtcCommand)},
            {"showtdutc", New CommandInfo("showtdutc", ShellType.TestShell, "Shows UTC kernel date and time", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ShowTDUtcCommand)},
            {"testtable", New CommandInfo("testtable", ShellType.TestShell, "Tests the table functionality", New CommandArgumentInfo({"[margin]"}, False, 0), New Test_TestTableCommand)},
            {"checkstring", New CommandInfo("checkstring", ShellType.TestShell, "Checks to see if the translatable string exists in the KS resources", New CommandArgumentInfo({"<string>"}, True, 1), New Test_CheckStringCommand)},
            {"checksettingsentryvars", New CommandInfo("checksettingsentryvars", ShellType.TestShell, "Checks all the KS settings to see if the variables are written correctly", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_CheckSettingsEntryVarsCommand)},
            {"checklocallines", New CommandInfo("checklocallines", ShellType.TestShell, "Checks all the localization text line numbers to see if they're all equal", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_CheckLocalLinesCommand)},
            {"checkstrings", New CommandInfo("checkstrings", ShellType.TestShell, "Checks to see if the translatable strings exist in the KS resources", New CommandArgumentInfo({"<stringlistfile>"}, True, 1), New Test_CheckStringsCommand)},
            {"sleeptook", New CommandInfo("sleeptook", ShellType.TestShell, "How many milliseconds did it really take to sleep?", New CommandArgumentInfo({"[-t] <sleepms>"}, True, 1), New Test_SleepTookCommand, False, False, False, False, False)},
            {"getlinestyle", New CommandInfo("getlinestyle", ShellType.TestShell, "Gets the line ending style from text file", New CommandArgumentInfo({"<textfile>"}, True, 1), New Test_GetLineStyleCommand)},
            {"printfiglet", New CommandInfo("printfiglet", ShellType.TestShell, "Prints a string to console using color type and line print with Figlet support", New CommandArgumentInfo({"<Color> <FigletFont> <Message>"}, True, 3), New Test_PrintFigletCommand)},
            {"printfigletf", New CommandInfo("printfigletf", ShellType.TestShell, "Prints a string to console using color type and line print with format and Figlet support", New CommandArgumentInfo({"<Color> <FigletFont> <Variable1;Variable2;Variable3;...> <Message>"}, True, 4), New Test_PrintFigletFCommand)},
            {"powerlinetest", New CommandInfo("powerlinetest", ShellType.TestShell, "Tests your console for PowerLine support", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_PowerLineTestCommand)},
            {"testexecuteasm", New CommandInfo("testexecuteasm", ShellType.TestShell, "Tests assembly entry point execution", New CommandArgumentInfo({"<pathtoasm>"}, True, 1), New Test_TestExecuteAsmCommand)},
            {"help", New CommandInfo("help", ShellType.TestShell, "Shows help screen", New CommandArgumentInfo({"[command]"}, False, 0), New Test_HelpCommand)},
            {"start", New CommandInfo("start", ShellType.TestShell, "Exits the test shell and starts the kernel", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_StartCommand)},
            {"shutdown", New CommandInfo("shutdown", ShellType.TestShell, "Exits the test shell and shuts down the kernel", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New Test_ShutdownCommand)}
        }
        Public Test_ShutdownFlag As Boolean

    End Module
End Namespace