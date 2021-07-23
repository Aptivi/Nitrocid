
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

Public Module SFTPHelpSystem

    'This dictionary is the definitions for commands.
    Public SFTPDefinitions As Dictionary(Of String, String)
    Public SFTPModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitSFTPHelp()
        SFTPDefinitions = New Dictionary(Of String, String) From {{"pwdl", DoTranslation("Gets current local directory")},
                                                                  {"pwdr", DoTranslation("Gets current remote directory")},
                                                                  {"connect", DoTranslation("Connects to an SFTP server (it must start with ""sftp://"")")},
                                                                  {"cdl", DoTranslation("Changes local directory to download to or upload from")},
                                                                  {"cdr", DoTranslation("Changes remote directory to download from or upload to")},
                                                                  {"del", DoTranslation("Deletes remote file from server")},
                                                                  {"disconnect", DoTranslation("Disconnects from server")},
                                                                  {"get", DoTranslation("Downloads remote file to local directory using binary or text")},
                                                                  {"exit", DoTranslation("Exits SFTP shell and returns to kernel")},
                                                                  {"help", DoTranslation("Shows help screen")},
                                                                  {"lsl", DoTranslation("Lists local directory")},
                                                                  {"lsr", DoTranslation("Lists remote directory")},
                                                                  {"quickconnect", DoTranslation("Uses information from Speed Dial to connect to any network quickly")},
                                                                  {"put", DoTranslation("Uploads local file to remote directory using binary or text")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub SFTPShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In SFTPDefinitions.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, SFTPDefinitions(cmd))
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If SFTPModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In SFTPModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, SFTPModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If SFTPShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In SFTPShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, SFTPDefinitions(SFTPShellAliases(cmd)))
                Next
            Else
                For Each cmd As String In SFTPCommands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In SFTPModDefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", SFTPShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        ElseIf command = "pwdl" Then
            W(DoTranslation("Usage:") + " pwdl: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "pwdr" Then
            W(DoTranslation("Usage:") + " pwdr: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "connect" Then
            W(DoTranslation("Usage:") + " connect <server>: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "cdl" Then
            W(DoTranslation("Usage:") + " cdl <directory>: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "cdr" Then
            W(DoTranslation("Usage:") + " cdr <directory>: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "del" Then
            W(DoTranslation("Usage:") + " del <file>: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "disconnect" Then
            W(DoTranslation("Usage:") + " disconnect: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "get" Then
            W(DoTranslation("Usage:") + " get <file>: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "lsl" Then
            W(DoTranslation("Usage:") + " lsl [dir]: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "ldr" Then
            W(DoTranslation("Usage:") + " lsr [dir]: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "put" Then
            W(DoTranslation("Usage:") + " put <file>: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "quickconnect" Then
            W(DoTranslation("Usage:") + " quickconnect: " + SFTPDefinitions(command), True, ColTypes.Neutral)
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
        End If

    End Sub

End Module
