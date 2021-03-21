
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
        SFTPDefinitions = New Dictionary(Of String, String) From {{"currlocaldir (pwdl)", DoTranslation("Gets current local directory")},
                                                                  {"currremotedir (pwdr)", DoTranslation("Gets current remote directory")},
                                                                  {"connect", DoTranslation("Connects to an SFTP server (it must start with ""sftp://"")")},
                                                                  {"changelocaldir (cdl)", DoTranslation("Changes local directory to download to or upload from")},
                                                                  {"changeremotedir (cdr)", DoTranslation("Changes remote directory to download from or upload to")},
                                                                  {"delete (del)", DoTranslation("Deletes remote file from server")},
                                                                  {"disconnect", DoTranslation("Disconnects from server")},
                                                                  {"download (get)", DoTranslation("Downloads remote file to local directory using binary or text")},
                                                                  {"exit", DoTranslation("Exits SFTP shell and returns to kernel")},
                                                                  {"help", DoTranslation("Shows help screen")},
                                                                  {"listlocal (lsl)", DoTranslation("Lists local directory")},
                                                                  {"listremote (lsr)", DoTranslation("Lists remote directory")},
                                                                  {"quickconnect", DoTranslation("Uses information from Speed Dial to connect to any network quickly")},
                                                                  {"upload (put)", DoTranslation("Uploads local file to remote directory using binary or text")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub SFTPShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                For Each cmd As String In SFTPDefinitions.Keys
                    W("- {0}: ", False, ColTypes.HelpCmd, cmd) : W("{0}", True, ColTypes.HelpDef, SFTPDefinitions(cmd))
                Next
                For Each cmd As String In SFTPModDefs.Keys
                    W("- {0}: ", False, ColTypes.HelpCmd, cmd) : W("{0}", True, ColTypes.HelpDef, SFTPModDefs(cmd))
                Next
            Else
                W(String.Join(", ", availsftpcmds), True, ColTypes.Neutral)
            End If
        ElseIf command = ("currlocaldir" Or "pwdl") Then
            W(DoTranslation("Usage:") + " currlocaldir or pwdl", True, ColTypes.Neutral)
        ElseIf command = ("currremotedir" Or "pwdr") Then
            W(DoTranslation("Usage:") + " currremotedir or pwdr", True, ColTypes.Neutral)
        ElseIf command = "connect" Then
            W(DoTranslation("Usage:") + " connect <server>", True, ColTypes.Neutral)
        ElseIf command = ("changelocaldir" Or "cdl") Then
            W(DoTranslation("Usage:") + " changelocaldir <directory> or cdl <directory>", True, ColTypes.Neutral)
        ElseIf command = ("changeremotedir" Or "cdr") Then
            W(DoTranslation("Usage:") + " changeremotedir <directory> or cdr <directory>", True, ColTypes.Neutral)
        ElseIf command = ("delete" Or "del") Then
            W(DoTranslation("Usage:") + " delete <file> or del <file>", True, ColTypes.Neutral)
        ElseIf command = "disconnect" Then
            W(DoTranslation("Usage:") + " disconnect", True, ColTypes.Neutral)
        ElseIf command = ("download" Or "get") Then
            W(DoTranslation("Usage:") + " download <file> or get <file>", True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf command = ("listlocal" Or "lsl") Then
            W(DoTranslation("Usage:") + " listlocal [dir] or lsl [dir]", True, ColTypes.Neutral)
        ElseIf command = ("listremote" Or "ldr") Then
            W(DoTranslation("Usage:") + " listremote [dir] or lsr [dir]", True, ColTypes.Neutral)
        ElseIf command = ("upload" Or "put") Then
            W(DoTranslation("Usage:") + " upload <file> or put <file>", True, ColTypes.Neutral)
        ElseIf command = "quickconnect" Then
            W(DoTranslation("Usage:") + " quickconnect", True, ColTypes.Neutral)
        End If

    End Sub

End Module
