
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.Threading

Module TestShell

    Public Test_ModCommands As New ArrayList
    Public ReadOnly Test_Commands As New Dictionary(Of String, CommandInfo) From {{"print", New CommandInfo("print", ShellCommandType.TestShell, "Prints a string to console using color type and line print", "<Color> <Line> <Message>", True, 3, New Test_PrintCommand)},
                                                                                  {"printf", New CommandInfo("printf", ShellCommandType.TestShell, "Prints a string to console using color type and line print with format support", "<Color> <Line> <Variable1;Variable2;Variable3;...> <Message>", True, 4, New Test_PrintFCommand)},
                                                                                  {"printd", New CommandInfo("printd", ShellCommandType.TestShell, "Prints a string to debugger", "<Message>", True, 1, New Test_PrintDCommand)},
                                                                                  {"printdf", New CommandInfo("printdf", ShellCommandType.TestShell, "Prints a string to debugger with format support", "<Variable1;Variable2;Variable3;...> <Message>", True, 2, New Test_PrintDFCommand)},
                                                                                  {"printsep", New CommandInfo("printsep", ShellCommandType.TestShell, "Prints a separator", "<Message>", True, 1, New Test_PrintSepCommand)},
                                                                                  {"printsepf", New CommandInfo("printsepf", ShellCommandType.TestShell, "Prints a separator with format support", "<Variable1;Variable2;Variable3;...> <Message>", True, 2, New Test_PrintSepCommand)},
                                                                                  {"printsepcolor", New CommandInfo("printsepcolor", ShellCommandType.TestShell, "Prints a separator with color support", "<Color> <Message>", True, 2, New Test_PrintSepColorCommand)},
                                                                                  {"printsepcolorf", New CommandInfo("printsepcolorf", ShellCommandType.TestShell, "Prints a separator with color and format support", "<Color> <Variable1;Variable2;Variable3;...> <Message>", True, 3, New Test_PrintSepColorFCommand)},
                                                                                  {"testevent", New CommandInfo("testevent", ShellCommandType.TestShell, "Tests raising the specific event", "<event>", True, 1, New Test_TestEventCommand)},
                                                                                  {"probehw", New CommandInfo("probehw", ShellCommandType.TestShell, "Tests probing the hardware", "", False, 0, New Test_ProbeHwCommand)},
                                                                                  {"garbage", New CommandInfo("garbage", ShellCommandType.TestShell, "Tests the garbage collector", "", False, 0, New Test_GarbageCommand)},
                                                                                  {"panic", New CommandInfo("panic", ShellCommandType.TestShell, "Tests the kernel error facility", "<ErrorType> <Reboot> <RebootTime> <Description>", True, 4, New Test_PanicCommand)},
                                                                                  {"panicf", New CommandInfo("panicf", ShellCommandType.TestShell, "Tests the kernel error facility with format support", "<ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>", True, 5, New Test_PanicFCommand)},
                                                                                  {"translate", New CommandInfo("translate", ShellCommandType.TestShell, "Tests translating a string that exists in resources to specific language", "<Lang> <Message>", True, 2, New Test_TranslateCommand)},
                                                                                  {"places", New CommandInfo("places", ShellCommandType.TestShell, "Prints a string to console and parses the placeholders found", "<Message>", True, 1, New Test_PlacesCommand)},
                                                                                  {"testcrc32", New CommandInfo("testcrc32", ShellCommandType.TestShell, "Encrypts a string using CRC32", "<string>", True, 1, New Test_TestCRC32Command)},
                                                                                  {"testsha512", New CommandInfo("testsha512", ShellCommandType.TestShell, "Encrypts a string using SHA512", "<string>", True, 1, New Test_TestSHA512Command)},
                                                                                  {"testsha384", New CommandInfo("testsha384", ShellCommandType.TestShell, "Encrypts a string using SHA384", "<string>", True, 1, New Test_TestSHA384Command)},
                                                                                  {"testsha256", New CommandInfo("testsha256", ShellCommandType.TestShell, "Encrypts a string using SHA256", "<string>", True, 1, New Test_TestSHA256Command)},
                                                                                  {"testsha1", New CommandInfo("testsha1", ShellCommandType.TestShell, "Encrypts a string using SHA1", "<string>", True, 1, New Test_TestSHA1Command)},
                                                                                  {"testmd5", New CommandInfo("testmd5", ShellCommandType.TestShell, "Encrypts a string using MD5", "<string>", True, 1, New Test_TestMD5Command)},
                                                                                  {"testregexp", New CommandInfo("testregexp", ShellCommandType.TestShell, "Tests the regular expression facility", "<pattern> <string>", True, 2, New Test_TestRegExpCommand)},
                                                                                  {"loadmods", New CommandInfo("loadmods", ShellCommandType.TestShell, "Starts all mods", "", False, 0, New Test_LoadModsCommand)},
                                                                                  {"stopmods", New CommandInfo("stopmods", ShellCommandType.TestShell, "Stops all mods", "", False, 0, New Test_StopModsCommand)},
                                                                                  {"reloadmods", New CommandInfo("reloadmods", ShellCommandType.TestShell, "Reloads all mods", "", False, 0, New Test_ReloadModsCommand)},
                                                                                  {"blacklistmod", New CommandInfo("blacklistmod", ShellCommandType.TestShell, "Adds a mod to the blacklist", "<mod>", True, 1, New Test_BlacklistModCommand)},
                                                                                  {"unblacklistmod", New CommandInfo("unblacklistmod", ShellCommandType.TestShell, "Removes a mod from the blacklist", "<mod>", True, 1, New Test_UnblacklistModCommand)},
                                                                                  {"debug", New CommandInfo("debug", ShellCommandType.TestShell, "Enables or disables debug", "<Enable:True/False>", True, 1, New Test_DebugCommand)},
                                                                                  {"rdebug", New CommandInfo("rdebug", ShellCommandType.TestShell, "Enables or disables remote debug", "<Enable:True/False>", True, 1, New Test_RDebugCommand)},
                                                                                  {"colortest", New CommandInfo("colortest", ShellCommandType.TestShell, "Tests the VT sequence for 255 colors", "<1-255>", True, 1, New Test_ColorTestCommand)},
                                                                                  {"colortruetest", New CommandInfo("colortruetest", ShellCommandType.TestShell, "Tests the VT sequence for true color", "<R;G;B>", True, 1, New Test_ColorTrueTestCommand)},
                                                                                  {"colorwheel", New CommandInfo("colorwheel", ShellCommandType.TestShell, "Tests the color wheel", "", False, 0, New Test_ColorWheelCommand)},
                                                                                  {"sendnot", New CommandInfo("sendnot", ShellCommandType.TestShell, "Sends a notification to test the receiver", "<Priority> <title> <desc>", True, 3, New Test_SendNotCommand)},
                                                                                  {"sendnotprog", New CommandInfo("sendnotprog", ShellCommandType.TestShell, "Sends a progress notification to test the receiver", "<Priority> <title> <desc> <failat>", True, 4, New Test_SendNotProgCommand)},
                                                                                  {"dcalend", New CommandInfo("dcalend", ShellCommandType.TestShell, "Tests printing date using different calendars", "<calendar>", True, 1, New Test_DCalendCommand)},
                                                                                  {"listcodepages", New CommandInfo("listcodepages", ShellCommandType.TestShell, "Lists all supported codepages", "", False, 0, New Test_ListCodePagesCommand)},
                                                                                  {"lscompilervars", New CommandInfo("lscompilervars", ShellCommandType.TestShell, "What compiler variables are enabled in the application?", "", False, 0, New Test_LsCompilerVarsCommand)},
                                                                                  {"testdictwriterstr", New CommandInfo("testdictwriterstr", ShellCommandType.TestShell, "Tests the dictionary writer with the string and string array", "", False, 0, New Test_TestDictWriterStrCommand)},
                                                                                  {"testdictwriterint", New CommandInfo("testdictwriterint", ShellCommandType.TestShell, "Tests the dictionary writer with the integer and integer array", "", False, 0, New Test_TestDictWriterIntCommand)},
                                                                                  {"testdictwriterchar", New CommandInfo("testdictwriterchar", ShellCommandType.TestShell, "Tests the dictionary writer with the char and char array", "", False, 0, New Test_TestDictWriterCharCommand)},
                                                                                  {"testlistwriterstr", New CommandInfo("testlistwriterstr", ShellCommandType.TestShell, "Tests the list writer with the string and string array", "", False, 0, New Test_TestListWriterStrCommand)},
                                                                                  {"testlistwriterint", New CommandInfo("testlistwriterint", ShellCommandType.TestShell, "Tests the list writer with the integer and integer array", "", False, 0, New Test_TestListWriterIntCommand)},
                                                                                  {"testlistwriterchar", New CommandInfo("testlistwriterchar", ShellCommandType.TestShell, "Tests the list writer with the char and char array", "", False, 0, New Test_TestListWriterCharCommand)},
                                                                                  {"lscultures", New CommandInfo("lscultures", ShellCommandType.TestShell, "Lists supported cultures", "[search]", False, 0, New Test_LsCulturesCommand)},
                                                                                  {"getcustomsaversetting", New CommandInfo("getcustomsaversetting", ShellCommandType.TestShell, "Gets custom saver settings", "<saver> <setting>", True, 2, New Test_GetCustomSaverSettingCommand)},
                                                                                  {"setcustomsaversetting", New CommandInfo("setcustomsaversetting", ShellCommandType.TestShell, "Sets custom saver settings", "<saver> <setting> <value>", True, 3, New Test_SetCustomSaverSettingCommand)},
                                                                                  {"showtime", New CommandInfo("showtime", ShellCommandType.TestShell, "Shows local kernel time", "", False, 0, New Test_ShowTimeCommand)},
                                                                                  {"showdate", New CommandInfo("showdate", ShellCommandType.TestShell, "Shows local kernel date", "", False, 0, New Test_ShowDateCommand)},
                                                                                  {"showtd", New CommandInfo("showtd", ShellCommandType.TestShell, "Shows local kernel date and time", "", False, 0, New Test_ShowTDCommand)},
                                                                                  {"showtimeutc", New CommandInfo("showtimeutc", ShellCommandType.TestShell, "Shows UTC kernel time", "", False, 0, New Test_ShowTimeUtcCommand)},
                                                                                  {"showdateutc", New CommandInfo("showdateutc", ShellCommandType.TestShell, "Shows UTC kernel date", "", False, 0, New Test_ShowDateUtcCommand)},
                                                                                  {"showtdutc", New CommandInfo("showtdutc", ShellCommandType.TestShell, "Shows UTC kernel date and time", "", False, 0, New Test_ShowTDUtcCommand)},
                                                                                  {"testtable", New CommandInfo("testtable", ShellCommandType.TestShell, "Tests the table functionality", "[margin]", False, 0, New Test_TestTableCommand)},
                                                                                  {"checkstring", New CommandInfo("checkstring", ShellCommandType.TestShell, "Checks to see if the translatable string exists in the KS resources", "<string>", True, 1, New Test_CheckStringCommand)},
                                                                                  {"help", New CommandInfo("help", ShellCommandType.TestShell, "Shows help screen", "[command]", False, 0, New Test_HelpCommand)},
                                                                                  {"exit", New CommandInfo("exit", ShellCommandType.TestShell, "Exits the test shell and starts the kernel", "", False, 0, New Test_ExitCommand)},
                                                                                  {"shutdown", New CommandInfo("shutdown", ShellCommandType.TestShell, "Exits the test shell and shuts down the kernel", "", False, 0, New Test_ShutdownCommand)}}
    Public Test_ExitFlag As Boolean
    Public Test_ShutdownFlag As Boolean
    Public Test_PromptStyle As String = ""

    ''' <summary>
    ''' Initiates the test shell
    ''' </summary>
    Sub InitTShell(InvokedFromCommand As Boolean)
        Dim FullCmd As String
        If InvokedFromCommand Then Test_ExitFlag = False
        SwitchCancellationHandler(ShellCommandType.TestShell)
        StageTimer.Stop()
        If Not Test_ExitFlag Then
            Console.WriteLine()
            WriteSeparator(DoTranslation("Welcome to Test Shell!"), True)
        End If

        While Not Test_ExitFlag
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
            End If
            Wdbg(DebugLevel.I, "Test_PromptStyle = {0}", Test_PromptStyle)
            If Test_PromptStyle = "" Then
                W("(t)> ", False, ColTypes.Input)
            Else
                Dim ParsedPromptStyle As String = ProbePlaces(Test_PromptStyle)
                ParsedPromptStyle.ConvertVTSequences
                W(ParsedPromptStyle, False, ColTypes.Gray)
            End If
            FullCmd = Console.ReadLine
            Try
                If Not (FullCmd = Nothing Or FullCmd?.StartsWithAnyOf({" ", "#"}) = True) Then
                    Wdbg(DebugLevel.I, "Command: {0}", FullCmd)
                    Dim Command As String = FullCmd.SplitEncloseDoubleQuotes(" ")(0)
                    If Test_Commands.ContainsKey(Command) Then
                        Dim Params As New ExecuteCommandThreadParameters(FullCmd, ShellCommandType.TestShell, Nothing)
                        TStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Test Shell Command Thread"}
                        TStartCommandThread.Start(Params)
                        TStartCommandThread.Join()
                    ElseIf Test_ModCommands.Contains(Command) Then
                        Wdbg(DebugLevel.I, "Mod command found.")
                        ExecuteModCommand(FullCmd)
                    ElseIf TestShellAliases.Keys.Contains(Command) Then
                        Wdbg(DebugLevel.I, "Test shell alias command found.")
                        FullCmd = FullCmd.Replace($"""{Command}""", Command)
                        ExecuteTestAlias(FullCmd)
                    Else
                        W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands."), True, ColTypes.Error, Command)
                    End If
                Else
                    Thread.Sleep(30) 'This is to fix race condition between test shell initialization and starting the event handler thread
                End If
            Catch ex As Exception
                W(DoTranslation("Error in test shell: {0}"), True, ColTypes.Error, ex.Message)
                Wdbg(DebugLevel.E, "Error: {0}", ex.Message)
                WStkTrc(ex)
            End Try
        End While

        StageTimer.Start()
    End Sub

End Module
