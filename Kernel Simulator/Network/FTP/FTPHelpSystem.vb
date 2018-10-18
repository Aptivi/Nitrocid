
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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
    Public ftpDefinitions As New Dictionary(Of String, String) From {{"binary (Alias: bin)", "Switches transfer mode to binary (Recommended for most file transfers, mainly ISO files, .deb packages, or any downloaded file that isn't usable in text mode)"}, _
                                                                     {"currlocaldir (Alias: pwdl)", "Gets current local directory"}, _
                                                                     {"currremotedir (Alias: pwdr)", "Gets current remote directory"}, _
                                                                     {"connect", "Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")"}, _
                                                                     {"changelocaldir (Alias: cdl)", "Changes local directory to download to or upload from"}, _
                                                                     {"changeremotedir (Alias: cdr)", "Changes remote directory to download from or upload to"}, _
                                                                     {"delete (Alias: del)", "Deletes remote file from server"}, _
                                                                     {"disconnect", "Disconnects from server"}, _
                                                                     {"download (Alias: get)", "Downloads remote file to local directory using binary or text"}, _
                                                                     {"exit", "Exits FTP shell and returns to kernel"}, _
                                                                     {"help", "Shows help screen"}, _
                                                                     {"listlocal (Alias: lsl)", "Lists local directory"}, _
                                                                     {"listremote (Alias: lsr)", "Lists remote directory"}, _
                                                                     {"passive", "Use passive mode for transferring (Default: on, because common FTP servers only accept passive connections)"}, _
                                                                     {"ssl", "Uses SSL for encryption of a connection"}, _
                                                                     {"text (Alias: txt)", "Switches transfer mode to text (Recommended for .txt files, and all files that only contains readable text and not binary data)"}, _
                                                                     {"upload (Alias: put)", "Uploads local file to remote directory using binary or text"}}

    Public Sub FTPShowHelp()

        If (simHelp = False) Then
            For Each cmd As String In ftpDefinitions.Keys
                W("- {0}: ", "helpCmd", cmd) : Wln("{0}", "helpDef", ftpDefinitions(cmd))
            Next
        Else
            Wln(String.Join(", ", availableCommands), "neutralText")
        End If

    End Sub

End Module
