
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

Public Module FTPHelpSystem

    'This dictionary is the definitions for commands.
    Public FTPDefinitions As Dictionary(Of String, String)
    Public FTPModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitFTPHelp()
        FTPDefinitions = New Dictionary(Of String, String) From {{"connect", DoTranslation("Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")")},
                                                                 {"cdl", DoTranslation("Changes local directory to download to or upload from")},
                                                                 {"cdr", DoTranslation("Changes remote directory to download from or upload to")},
                                                                 {"cp", DoTranslation("Copies file or directory to another file or directory.")},
                                                                 {"del", DoTranslation("Deletes remote file from server")},
                                                                 {"disconnect", DoTranslation("Disconnects from server")},
                                                                 {"get", DoTranslation("Downloads remote file to local directory using binary or text")},
                                                                 {"exit", DoTranslation("Exits FTP shell and returns to kernel")},
                                                                 {"help", DoTranslation("Shows help screen")},
                                                                 {"lsl", DoTranslation("Lists local directory")},
                                                                 {"lsr", DoTranslation("Lists remote directory")},
                                                                 {"mv", DoTranslation("Moves file or directory to another file or directory. You can also use that to rename files.")},
                                                                 {"perm", DoTranslation("Sets file permissions. This is supported only on FTP servers that run Unix.")},
                                                                 {"quickconnect", DoTranslation("Uses information from Speed Dial to connect to any network quickly")},
                                                                 {"put", DoTranslation("Uploads local file to remote directory using binary or text")},
                                                                 {"pwdl", DoTranslation("Gets current local directory")},
                                                                 {"pwdr", DoTranslation("Gets current remote directory")},
                                                                 {"type", DoTranslation("Sets the type for this session")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub FTPShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In FTPDefinitions.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, FTPDefinitions(cmd))
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If FTPModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In FTPModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, FTPModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If FTPShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In FTPShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, FTPDefinitions(FTPShellAliases(cmd)))
                Next
            Else
                For Each cmd As String In FTPCommands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In FTPModDefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", FTPShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        ElseIf command = "pwdl" Then
            W(DoTranslation("Usage:") + " currlocaldir or pwdl: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "pwdr" Then
            W(DoTranslation("Usage:") + " currremotedir or pwdr: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "connect" Then
            W(DoTranslation("Usage:") + " connect <server>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "cdl" Then
            W(DoTranslation("Usage:") + " cdl <directory>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "cdr" Then
            W(DoTranslation("Usage:") + " cdr <directory>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "cp" Then
            W(DoTranslation("Usage:") + " cp <sourcefileordir> <targetfileordir>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "del" Then
            W(DoTranslation("Usage:") + " del <file>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "disconnect" Then
            W(DoTranslation("Usage:") + " disconnect: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "get" Then
            W(DoTranslation("Usage:") + " get <file>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "lsl" Then
            W(DoTranslation("Usage:") + " lsl [dir]: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "ldr" Then
            W(DoTranslation("Usage:") + " lsr [dir]: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "mv" Then
            W(DoTranslation("Usage:") + " mv <sourcefileordir> <targetfileordir>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "perm" Then
            W(DoTranslation("Usage:") + " perm <file> <permnumber>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "type" Then
            W(DoTranslation("Usage:") + " type <a/b>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "put" Then
            W(DoTranslation("Usage:") + " put <file>: " + FTPDefinitions(command), True, ColTypes.Neutral)
        ElseIf command = "quickconnect" Then
            W(DoTranslation("Usage:") + " quickconnect: " + FTPDefinitions(command), True, ColTypes.Neutral)
        End If

    End Sub

End Module
