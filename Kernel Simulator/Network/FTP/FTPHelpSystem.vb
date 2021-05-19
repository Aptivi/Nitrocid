
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
        FTPDefinitions = New Dictionary(Of String, String) From {{"currlocaldir (pwdl)", DoTranslation("Gets current local directory")},
                                                                 {"currremotedir (pwdr)", DoTranslation("Gets current remote directory")},
                                                                 {"connect", DoTranslation("Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")")},
                                                                 {"changelocaldir (cdl)", DoTranslation("Changes local directory to download to or upload from")},
                                                                 {"changeremotedir (cdr)", DoTranslation("Changes remote directory to download from or upload to")},
                                                                 {"copy (cp)", DoTranslation("Copies file or directory to another file or directory.")},
                                                                 {"delete (del)", DoTranslation("Deletes remote file from server")},
                                                                 {"disconnect", DoTranslation("Disconnects from server")},
                                                                 {"download (get)", DoTranslation("Downloads remote file to local directory using binary or text")},
                                                                 {"exit", DoTranslation("Exits FTP shell and returns to kernel")},
                                                                 {"help", DoTranslation("Shows help screen")},
                                                                 {"listlocal (lsl)", DoTranslation("Lists local directory")},
                                                                 {"listremote (lsr)", DoTranslation("Lists remote directory")},
                                                                 {"move (mv)", DoTranslation("Moves file or directory to another file or directory. You can also use that to rename files.")},
                                                                 {"perm", DoTranslation("Sets file permissions. This is supported only on FTP servers that run Unix.")},
                                                                 {"quickconnect", DoTranslation("Uses information from Speed Dial to connect to any network quickly")},
                                                                 {"type", DoTranslation("Sets the type for this session")},
                                                                 {"upload (put)", DoTranslation("Uploads local file to remote directory using binary or text")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub FTPShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                For Each cmd As String In FTPDefinitions.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, FTPDefinitions(cmd))
                Next
                For Each cmd As String In FTPModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, FTPModDefs(cmd))
                Next
                For Each cmd As String In FTPShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, FTPDefinitions(FTPShellAliases(cmd)))
                Next
            Else
                W(String.Join(", ", FTPCommands.Keys), True, ColTypes.Neutral)
            End If
        ElseIf command = "currlocaldir" Or command = "pwdl" Then
            W(DoTranslation("Usage:") + " currlocaldir or pwdl", True, ColTypes.Neutral)
        ElseIf command = "currremotedir" Or command = "pwdr" Then
            W(DoTranslation("Usage:") + " currremotedir or pwdr", True, ColTypes.Neutral)
        ElseIf command = "connect" Then
            W(DoTranslation("Usage:") + " connect <server>", True, ColTypes.Neutral)
        ElseIf command = "changelocaldir" Or command = "cdl" Then
            W(DoTranslation("Usage:") + " changelocaldir <directory> or cdl <directory>", True, ColTypes.Neutral)
        ElseIf command = "changeremotedir" Or command = "cdr" Then
            W(DoTranslation("Usage:") + " changeremotedir <directory> or cdr <directory>", True, ColTypes.Neutral)
        ElseIf command = "copy" Or command = "cp" Then
            W(DoTranslation("Usage:") + " copy <sourcefileordir> <targetfileordir> or cp <sourcefileordir> <targetfileordir>", True, ColTypes.Neutral)
        ElseIf command = "delete" Or command = "del" Then
            W(DoTranslation("Usage:") + " delete <file> or del <file>", True, ColTypes.Neutral)
        ElseIf command = "disconnect" Then
            W(DoTranslation("Usage:") + " disconnect", True, ColTypes.Neutral)
        ElseIf command = "download" Or command = "get" Then
            W(DoTranslation("Usage:") + " download <file> or get <file>", True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf command = "listlocal" Or command = "lsl" Then
            W(DoTranslation("Usage:") + " listlocal [dir] or lsl [dir]", True, ColTypes.Neutral)
        ElseIf command = "listremote" Or command = "ldr" Then
            W(DoTranslation("Usage:") + " listremote [dir] or lsr [dir]", True, ColTypes.Neutral)
        ElseIf command = "move" Or command = "mv" Then
            W(DoTranslation("Usage:") + " move <sourcefileordir> <targetfileordir> or ren <sourcefileordir> <targetfileordir>", True, ColTypes.Neutral)
        ElseIf command = "perm" Then
            W(DoTranslation("Usage:") + " perm <file> <permnumber>", True, ColTypes.Neutral)
        ElseIf command = "type" Then
            W(DoTranslation("Usage:") + " type <a/b>", True, ColTypes.Neutral)
        ElseIf command = "upload" Or command = "put" Then
            W(DoTranslation("Usage:") + " upload <file> or put <file>", True, ColTypes.Neutral)
        ElseIf command = "quickconnect" Then
            W(DoTranslation("Usage:") + " quickconnect", True, ColTypes.Neutral)
        End If

    End Sub

End Module
