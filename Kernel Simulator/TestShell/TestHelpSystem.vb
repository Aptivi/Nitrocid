
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

Public Module TestHelpSystem

    'This dictionary is the definitions for commands.
    Public TestDefinitions As Dictionary(Of String, String)
    Public TestModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitTestHelp()
        TestDefinitions = New Dictionary(Of String, String) From {{"print", DoTranslation("Prints a string to console using color type and line print")},
                                                                  {"printf", DoTranslation("Prints a string to console using color type and line print with format support")},
                                                                  {"printd", DoTranslation("Prints a string to debugger")},
                                                                  {"printdf", DoTranslation("Prints a string to debugger with format support")},
                                                                  {"printsep", DoTranslation("Prints a separator")},
                                                                  {"printsepf", DoTranslation("Prints a separator with format support")},
                                                                  {"printsepcolor", DoTranslation("Prints a separator with color support")},
                                                                  {"printsepcolorf", DoTranslation("Prints a separator with color and format support")},
                                                                  {"panic", DoTranslation("Tests the kernel error facility")},
                                                                  {"panicf", DoTranslation("Tests the kernel error facility with format support")},
                                                                  {"translate", DoTranslation("Tests translating a string that exists in resources to specific language")},
                                                                  {"places", DoTranslation("Prints a string to console and parses the placeholders found")},
                                                                  {"testregexp", DoTranslation("Tests the regular expression facility")},
                                                                  {"testsha512", DoTranslation("Encrypts a string using") + " SHA512"},
                                                                  {"testsha256", DoTranslation("Encrypts a string using") + " SHA256"},
                                                                  {"testsha1", DoTranslation("Encrypts a string using") + " SHA1"},
                                                                  {"testmd5", DoTranslation("Encrypts a string using") + " MD5"},
                                                                  {"loadmods", DoTranslation("Starts or stops all mods")},
                                                                  {"debug", DoTranslation("Enables or disables debug")},
                                                                  {"rdebug", DoTranslation("Enables or disables remote debug")},
                                                                  {"colortest", DoTranslation("Tests the VT sequence for 255 colors")},
                                                                  {"colortruetest", DoTranslation("Tests the VT sequence for true color")},
                                                                  {"sendnot", DoTranslation("Sends a notification to test the receiver")},
                                                                  {"dcalend", DoTranslation("Tests printing date using different calendars")},
                                                                  {"listcodepages", DoTranslation("Lists all supported codepages")},
                                                                  {"lscompilervars", DoTranslation("What compiler variables are enabled in the application?")},
                                                                  {"testevent", DoTranslation("Tests raising the specific event")},
                                                                  {"probehw", DoTranslation("Tests probing the hardware")},
                                                                  {"garbage", DoTranslation("Tests the garbage collector")},
                                                                  {"testdictwriterstr", DoTranslation("Tests the dictionary writer with the string and string array")},
                                                                  {"testdictwriterint", DoTranslation("Tests the dictionary writer with the integer and integer array")},
                                                                  {"testdictwriterchar", DoTranslation("Tests the dictionary writer with the char and char array")},
                                                                  {"testlistwriterstr", DoTranslation("Tests the list writer with the string and string array")},
                                                                  {"testlistwriterint", DoTranslation("Tests the list writer with the integer and integer array")},
                                                                  {"testlistwriterchar", DoTranslation("Tests the list writer with the char and char array")},
                                                                  {"lscultures", DoTranslation("Lists supported cultures")},
                                                                  {"getcustomsaversetting", DoTranslation("Gets custom saver settings")},
                                                                  {"setcustomsaversetting", DoTranslation("Sets custom saver settings")},
                                                                  {"help", DoTranslation("Shows help screen")},
                                                                  {"exit", DoTranslation("Exits the test shell and starts the kernel")},
                                                                  {"shutdown", DoTranslation("Exits the test shell and shuts down the kernel")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub TestShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In TestDefinitions.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, TestDefinitions(cmd))
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If TestModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In TestModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, TestModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If TestShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In TestShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, TestDefinitions(TestShellAliases(cmd)))
                Next
            Else
                For Each cmd As String In TestDefinitions.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In TestModDefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", TestShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        ElseIf command = "print" Then
            W(DoTranslation("Usage:") + " print <Color> <Line> <Message>", True, ColTypes.Neutral)
        ElseIf command = "printf" Then
            W(DoTranslation("Usage:") + " printf <Color> <Line> <Variable1;Variable2;Variable3;...> <Message>", True, ColTypes.Neutral)
        ElseIf command = "printd" Then
            W(DoTranslation("Usage:") + " printd <Message>", True, ColTypes.Neutral)
        ElseIf command = "printdf" Then
            W(DoTranslation("Usage:") + " printdf <Variable1;Variable2;Variable3;...> <Message>", True, ColTypes.Neutral)
        ElseIf command = "printsep" Then
            W(DoTranslation("Usage:") + " printsep <Message>", True, ColTypes.Neutral)
        ElseIf command = "printsepf" Then
            W(DoTranslation("Usage:") + " printsepf <Variable1;Variable2;Variable3;...> <Message>", True, ColTypes.Neutral)
        ElseIf command = "printsepcolor" Then
            W(DoTranslation("Usage:") + " printsepcolor <Color> <Message>", True, ColTypes.Neutral)
        ElseIf command = "printsepcolorf" Then
            W(DoTranslation("Usage:") + " printsepcolorf <Color> <Variable1;Variable2;Variable3;...> <Message>", True, ColTypes.Neutral)
        ElseIf command = "testevent" Then
            W(DoTranslation("Usage:") + " testevent <event>", True, ColTypes.Neutral)
        ElseIf command = "probehw" Then
            W(DoTranslation("Usage:") + " probehw", True, ColTypes.Neutral)
        ElseIf command = "garbage" Then
            W(DoTranslation("Usage:") + " garbage", True, ColTypes.Neutral)
        ElseIf command = "panic" Then
            W(DoTranslation("Usage:") + " panic <ErrorType> <Reboot> <RebootTime> <Description>", True, ColTypes.Neutral)
        ElseIf command = "panicf" Then
            W(DoTranslation("Usage:") + " panicf <ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>", True, ColTypes.Neutral)
        ElseIf command = "translate" Then
            W(DoTranslation("Usage:") + " translate <Lang> <Message>", True, ColTypes.Neutral)
        ElseIf command = "places" Then
            W(DoTranslation("Usage:") + " places <Message>", True, ColTypes.Neutral)
        ElseIf command = "testsha512" Then
            W(DoTranslation("Usage:") + " testsha512 <string>", True, ColTypes.Neutral)
        ElseIf command = "testsha256" Then
            W(DoTranslation("Usage:") + " testsha256 <string>", True, ColTypes.Neutral)
        ElseIf command = "testsha1" Then
            W(DoTranslation("Usage:") + " testsha1 <string>", True, ColTypes.Neutral)
        ElseIf command = "testmd5" Then
            W(DoTranslation("Usage:") + " testmd5 <string>", True, ColTypes.Neutral)
        ElseIf command = "testregexp" Then
            W(DoTranslation("Usage:") + " testregexp <pattern> <string>", True, ColTypes.Neutral)
        ElseIf command = "loadmods" Then
            W(DoTranslation("Usage:") + " loadmods <Enable:True/False>", True, ColTypes.Neutral)
        ElseIf command = "debug" Then
            W(DoTranslation("Usage:") + " debug <Enable:True/False>", True, ColTypes.Neutral)
        ElseIf command = "rdebug" Then
            W(DoTranslation("Usage:") + " rdebug <Enable:True/False>", True, ColTypes.Neutral)
        ElseIf command = "colortest" Then
            W(DoTranslation("Usage:") + " colortest <R;G;B>", True, ColTypes.Neutral)
        ElseIf command = "colortruetest" Then
            W(DoTranslation("Usage:") + " colortruetest <R;G;B>", True, ColTypes.Neutral)
        ElseIf command = "sendnot" Then
            W(DoTranslation("Usage:") + " sendnot <Priority> <title> <desc>", True, ColTypes.Neutral)
        ElseIf command = "dcalend" Then
            W(DoTranslation("Usage:") + " dcalend <calendar>", True, ColTypes.Neutral)
        ElseIf command = "listcodepages" Then
            W(DoTranslation("Usage:") + " listcodepages", True, ColTypes.Neutral)
        ElseIf command = "lscompilervars" Then
            W(DoTranslation("Usage:") + " lscompilervars", True, ColTypes.Neutral)
        ElseIf command = "testdictwriterstr" Then
            W(DoTranslation("Usage:") + " testdictwriterstr", True, ColTypes.Neutral)
        ElseIf command = "testdictwriterint" Then
            W(DoTranslation("Usage:") + " testdictwriterint", True, ColTypes.Neutral)
        ElseIf command = "testdictwriterchar" Then
            W(DoTranslation("Usage:") + " testdictwriterchar", True, ColTypes.Neutral)
        ElseIf command = "testlistwriterstr" Then
            W(DoTranslation("Usage:") + " testlistwriterstr", True, ColTypes.Neutral)
        ElseIf command = "testlistwriterint" Then
            W(DoTranslation("Usage:") + " testlistwriterint", True, ColTypes.Neutral)
        ElseIf command = "testlistwriterchar" Then
            W(DoTranslation("Usage:") + " testlistwriterchar", True, ColTypes.Neutral)
        ElseIf command = "lscultures" Then
            W(DoTranslation("Usage:") + " lscultures [search]", True, ColTypes.Neutral)
        ElseIf command = "getcustomsaversetting" Then
            W(DoTranslation("Usage:") + " getcustomsaversetting <saver> <setting>", True, ColTypes.Neutral)
        ElseIf command = "setcustomsaversetting" Then
            W(DoTranslation("Usage:") + " setcustomsaversetting <saver> <setting> <value>", True, ColTypes.Neutral)
        ElseIf command = "help" Then
            W(DoTranslation("Usage:") + " help [command]", True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf command = "shutdown" Then
            W(DoTranslation("Usage:") + " shutdown", True, ColTypes.Neutral)
        End If

    End Sub

End Module
