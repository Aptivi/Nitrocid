
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

    Public TestModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub TestShowHelp(Optional ByVal command As String = "")
        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(command) And Test_Commands.ContainsKey(command) Then
            Dim HelpDefinition As String = Test_Commands(command).GetTranslatedHelpEntry
            Select Case command
                Case "print"
                    W(DoTranslation("Usage:") + " print <Color> <Line> <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "printf"
                    W(DoTranslation("Usage:") + " printf <Color> <Line> <Variable1;Variable2;Variable3;...> <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "printd"
                    W(DoTranslation("Usage:") + " printd <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "printdf"
                    W(DoTranslation("Usage:") + " printdf <Variable1;Variable2;Variable3;...> <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "printsep"
                    W(DoTranslation("Usage:") + " printsep <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "printsepf"
                    W(DoTranslation("Usage:") + " printsepf <Variable1;Variable2;Variable3;...> <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "printsepcolor"
                    W(DoTranslation("Usage:") + " printsepcolor <Color> <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "printsepcolorf"
                    W(DoTranslation("Usage:") + " printsepcolorf <Color> <Variable1;Variable2;Variable3;...> <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testevent"
                    W(DoTranslation("Usage:") + " testevent <event>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "probehw"
                    W(DoTranslation("Usage:") + " probehw: " + HelpDefinition, True, ColTypes.Neutral)
                Case "garbage"
                    W(DoTranslation("Usage:") + " garbage: " + HelpDefinition, True, ColTypes.Neutral)
                Case "panic"
                    W(DoTranslation("Usage:") + " panic <ErrorType> <Reboot> <RebootTime> <Description>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "panicf"
                    W(DoTranslation("Usage:") + " panicf <ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "translate"
                    W(DoTranslation("Usage:") + " translate <Lang> <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "places"
                    W(DoTranslation("Usage:") + " places <Message>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testsha512"
                    W(DoTranslation("Usage:") + " testsha512 <string>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testsha256"
                    W(DoTranslation("Usage:") + " testsha256 <string>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testsha1"
                    W(DoTranslation("Usage:") + " testsha1 <string>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testmd5"
                    W(DoTranslation("Usage:") + " testmd5 <string>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testregexp"
                    W(DoTranslation("Usage:") + " testregexp <pattern> <string>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "loadmods"
                    W(DoTranslation("Usage:") + " loadmods: " + HelpDefinition, True, ColTypes.Neutral)
                Case "stopmods"
                    W(DoTranslation("Usage:") + " stopmods: " + HelpDefinition, True, ColTypes.Neutral)
                Case "debug"
                    W(DoTranslation("Usage:") + " debug <Enable:True/False>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rdebug"
                    W(DoTranslation("Usage:") + " rdebug <Enable:True/False>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "colortest"
                    W(DoTranslation("Usage:") + " colortest <R;G;B>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "colortruetest"
                    W(DoTranslation("Usage:") + " colortruetest <R;G;B>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "sendnot"
                    W(DoTranslation("Usage:") + " sendnot <Priority> <title> <desc>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "dcalend"
                    W(DoTranslation("Usage:") + " dcalend <calendar>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "listcodepages"
                    W(DoTranslation("Usage:") + " listcodepages: " + HelpDefinition, True, ColTypes.Neutral)
                Case "lscompilervars"
                    W(DoTranslation("Usage:") + " lscompilervars: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testdictwriterstr"
                    W(DoTranslation("Usage:") + " testdictwriterstr: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testdictwriterint"
                    W(DoTranslation("Usage:") + " testdictwriterint: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testdictwriterchar"
                    W(DoTranslation("Usage:") + " testdictwriterchar: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testlistwriterstr"
                    W(DoTranslation("Usage:") + " testlistwriterstr: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testlistwriterint"
                    W(DoTranslation("Usage:") + " testlistwriterint: " + HelpDefinition, True, ColTypes.Neutral)
                Case "testlistwriterchar"
                    W(DoTranslation("Usage:") + " testlistwriterchar: " + HelpDefinition, True, ColTypes.Neutral)
                Case "lscultures"
                    W(DoTranslation("Usage:") + " lscultures [search]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "getcustomsaversetting"
                    W(DoTranslation("Usage:") + " getcustomsaversetting <saver> <setting>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "setcustomsaversetting"
                    W(DoTranslation("Usage:") + " setcustomsaversetting <saver> <setting> <value>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "help"
                    W(DoTranslation("Usage:") + " help [command]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "exit"
                    W(DoTranslation("Usage:") + " exit: " + HelpDefinition, True, ColTypes.Neutral)
                Case "shutdown"
                    W(DoTranslation("Usage:") + " shutdown: " + HelpDefinition, True, ColTypes.Neutral)
            End Select
        ElseIf String.IsNullOrWhiteSpace(command) Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In Test_Commands.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, Test_Commands(cmd).GetTranslatedHelpEntry)
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If TestModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In TestModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, TestModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If TestShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In TestShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, Test_Commands(TestShellAliases(cmd)).GetTranslatedHelpEntry)
                Next
            Else
                For Each cmd As String In Test_Commands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In TestModDefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", TestShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
        End If
    End Sub

End Module
