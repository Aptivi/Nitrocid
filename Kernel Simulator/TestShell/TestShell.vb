
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
    Public ReadOnly Test_Commands As New Dictionary(Of String, CommandInfo) From {{"print", New CommandInfo("print", ShellCommandType.TestShell, "Prints a string to console using color type and line print", "<Color> <Line> <Message>", True, 3)},
                                                                                  {"printf", New CommandInfo("printf", ShellCommandType.TestShell, "Prints a string to console using color type and line print with format support", "<Color> <Line> <Variable1;Variable2;Variable3;...> <Message>", True, 4)},
                                                                                  {"printd", New CommandInfo("printd", ShellCommandType.TestShell, "Prints a string to debugger", "<Message>", True, 1)},
                                                                                  {"printdf", New CommandInfo("printdf", ShellCommandType.TestShell, "Prints a string to debugger with format support", "<Variable1;Variable2;Variable3;...> <Message>", True, 2)},
                                                                                  {"printsep", New CommandInfo("printsep", ShellCommandType.TestShell, "Prints a separator", "<Message>", True, 1)},
                                                                                  {"printsepf", New CommandInfo("printsepf", ShellCommandType.TestShell, "Prints a separator with format support", "<Variable1;Variable2;Variable3;...> <Message>", True, 2)},
                                                                                  {"printsepcolor", New CommandInfo("printsepcolor", ShellCommandType.TestShell, "Prints a separator with color support", "<Color> <Message>", True, 2)},
                                                                                  {"printsepcolorf", New CommandInfo("printsepcolorf", ShellCommandType.TestShell, "Prints a separator with color and format support", "<Color> <Variable1;Variable2;Variable3;...> <Message>", True, 3)},
                                                                                  {"testevent", New CommandInfo("testevent", ShellCommandType.TestShell, "Tests raising the specific event", "<event>", True, 1)},
                                                                                  {"probehw", New CommandInfo("probehw", ShellCommandType.TestShell, "Tests probing the hardware", "", False, 0)},
                                                                                  {"garbage", New CommandInfo("garbage", ShellCommandType.TestShell, "Tests the garbage collector", "", False, 0)},
                                                                                  {"panic", New CommandInfo("panic", ShellCommandType.TestShell, "Tests the kernel error facility", "<ErrorType> <Reboot> <RebootTime> <Description>", True, 4)},
                                                                                  {"panicf", New CommandInfo("panicf", ShellCommandType.TestShell, "Tests the kernel error facility with format support", "<ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>", True, 5)},
                                                                                  {"translate", New CommandInfo("translate", ShellCommandType.TestShell, "Tests translating a string that exists in resources to specific language", "<Lang> <Message>", True, 2)},
                                                                                  {"places", New CommandInfo("places", ShellCommandType.TestShell, "Prints a string to console and parses the placeholders found", "<Message>", True, 1)},
                                                                                  {"testsha512", New CommandInfo("testsha512", ShellCommandType.TestShell, "Encrypts a string using SHA512", "<string>", True, 1)},
                                                                                  {"testsha256", New CommandInfo("testsha256", ShellCommandType.TestShell, "Encrypts a string using SHA256", "<string>", True, 1)},
                                                                                  {"testsha1", New CommandInfo("testsha1", ShellCommandType.TestShell, "Encrypts a string using SHA1", "<string>", True, 1)},
                                                                                  {"testmd5", New CommandInfo("testmd5", ShellCommandType.TestShell, "Encrypts a string using MD5", "<string>", True, 1)},
                                                                                  {"testregexp", New CommandInfo("testregexp", ShellCommandType.TestShell, "Tests the regular expression facility", "<pattern> <string>", True, 2)},
                                                                                  {"loadmods", New CommandInfo("loadmods", ShellCommandType.TestShell, "Starts all mods", "", False, 0)},
                                                                                  {"stopmods", New CommandInfo("stopmods", ShellCommandType.TestShell, "Stops all mods", "", False, 0)},
                                                                                  {"debug", New CommandInfo("debug", ShellCommandType.TestShell, "Enables or disables debug", "<Enable:True/False>", True, 1)},
                                                                                  {"rdebug", New CommandInfo("rdebug", ShellCommandType.TestShell, "Enables or disables remote debug", "<Enable:True/False>", True, 1)},
                                                                                  {"colortest", New CommandInfo("colortest", ShellCommandType.TestShell, "Tests the VT sequence for 255 colors", "<1-255>", True, 1)},
                                                                                  {"colortruetest", New CommandInfo("colortruetest", ShellCommandType.TestShell, "Tests the VT sequence for true color", "<R;G;B>", True, 1)},
                                                                                  {"sendnot", New CommandInfo("sendnot", ShellCommandType.TestShell, "Sends a notification to test the receiver", "<Priority> <title> <desc>", True, 3)},
                                                                                  {"dcalend", New CommandInfo("dcalend", ShellCommandType.TestShell, "Tests printing date using different calendars", "<calendar>", True, 1)},
                                                                                  {"listcodepages", New CommandInfo("listcodepages", ShellCommandType.TestShell, "Lists all supported codepages", "", False, 0)},
                                                                                  {"lscompilervars", New CommandInfo("lscompilervars", ShellCommandType.TestShell, "What compiler variables are enabled in the application?", "", False, 0)},
                                                                                  {"testdictwriterstr", New CommandInfo("testdictwriterstr", ShellCommandType.TestShell, "Tests the dictionary writer with the string and string array", "", False, 0)},
                                                                                  {"testdictwriterint", New CommandInfo("testdictwriterint", ShellCommandType.TestShell, "Tests the dictionary writer with the integer and integer array", "", False, 0)},
                                                                                  {"testdictwriterchar", New CommandInfo("testdictwriterchar", ShellCommandType.TestShell, "Tests the dictionary writer with the char and char array", "", False, 0)},
                                                                                  {"testlistwriterstr", New CommandInfo("testlistwriterstr", ShellCommandType.TestShell, "Tests the list writer with the string and string array", "", False, 0)},
                                                                                  {"testlistwriterint", New CommandInfo("testlistwriterint", ShellCommandType.TestShell, "Tests the list writer with the integer and integer array", "", False, 0)},
                                                                                  {"testlistwriterchar", New CommandInfo("testlistwriterchar", ShellCommandType.TestShell, "Tests the list writer with the char and char array", "", False, 0)},
                                                                                  {"lscultures", New CommandInfo("lscultures", ShellCommandType.TestShell, "Lists supported cultures", "[search]", False, 0)},
                                                                                  {"getcustomsaversetting", New CommandInfo("getcustomsaversetting", ShellCommandType.TestShell, "Gets custom saver settings", "<saver> <setting>", True, 2)},
                                                                                  {"setcustomsaversetting", New CommandInfo("setcustomsaversetting", ShellCommandType.TestShell, "Sets custom saver settings", "<saver> <setting> <value>", True, 3)},
                                                                                  {"help", New CommandInfo("help", ShellCommandType.TestShell, "Shows help screen", "[command]", False, 0)},
                                                                                  {"exit", New CommandInfo("exit", ShellCommandType.TestShell, "Exits the test shell and starts the kernel", "", False, 0)},
                                                                                  {"shutdown", New CommandInfo("shutdown", ShellCommandType.TestShell, "Exits the test shell and shuts down the kernel", "", False, 0)}}
    Public Test_ExitFlag As Boolean
    Public Test_ShutdownFlag As Boolean

    ''' <summary>
    ''' Initiates the test shell
    ''' </summary>
    Sub InitTShell()
        Dim FullCmd As String
        AddHandler Console.CancelKeyPress, AddressOf TCancelCommand
        StageTimer.Stop()
        Console.WriteLine()
        WriteSeparator(DoTranslation("Welcome to Test Shell!"), True, ColTypes.Stage)

        While Not Test_ExitFlag
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
            End If
            W("(t)> ", False, ColTypes.Input)
            FullCmd = Console.ReadLine
            Try
                If Not (FullCmd = Nothing Or FullCmd?.StartsWithAnyOf({" ", "#"}) = True) Then
                    Wdbg("I", "Command: {0}", FullCmd)
                    Dim Command As String = FullCmd.SplitEncloseDoubleQuotes(" ")(0)
                    If Test_Commands.ContainsKey(Command) Then
                        TStartCommandThread = New Thread(AddressOf TParseCommand) With {.Name = "Test Shell Command Thread"}
                        TStartCommandThread.Start(FullCmd)
                        TStartCommandThread.Join()
                    ElseIf Test_ModCommands.Contains(Command) Then
                        Wdbg("I", "Mod command found.")
                        ExecuteModCommand(FullCmd)
                    ElseIf TestShellAliases.Keys.Contains(Command) Then
                        Wdbg("I", "Test shell alias command found.")
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
                Wdbg("E", "Error: {0}", ex.Message)
                WStkTrc(ex)
            End Try
        End While

        StageTimer.Start()
    End Sub

    ''' <summary>
    ''' Executes the test shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteTestAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, TestShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        TStartCommandThread = New Thread(AddressOf TParseCommand) With {.Name = "Test Shell Command Thread"}
        TStartCommandThread.Start(actualCmd)
        TStartCommandThread.Join()
    End Sub

End Module
