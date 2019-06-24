
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
    Public ftpDefinitions As New Dictionary(Of String, String) From {{"currlocaldir (Alias: pwdl)", DoTranslation("Gets current local directory", currentLang)},
                                                                     {"currremotedir (Alias: pwdr)", DoTranslation("Gets current remote directory", currentLang)},
                                                                     {"connect", DoTranslation("Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")", currentLang)},
                                                                     {"changelocaldir (Alias: cdl)", DoTranslation("Changes local directory to download to or upload from", currentLang)},
                                                                     {"changeremotedir (Alias: cdr)", DoTranslation("Changes remote directory to download from or upload to", currentLang)},
                                                                     {"delete (Alias: del)", DoTranslation("Deletes remote file from server", currentLang)},
                                                                     {"disconnect", DoTranslation("Disconnects from server", currentLang)},
                                                                     {"download (Alias: get)", DoTranslation("Downloads remote file to local directory using binary or text", currentLang)},
                                                                     {"exit", DoTranslation("Exits FTP shell and returns to kernel", currentLang)},
                                                                     {"help", DoTranslation("Shows help screen", currentLang)},
                                                                     {"listlocal (Alias: lsl)", DoTranslation("Lists local directory", currentLang)},
                                                                     {"listremote (Alias: lsr)", DoTranslation("Lists remote directory", currentLang)},
                                                                     {"rename (Alias: ren)", DoTranslation("Renames specific file or directory", currentLang)},
                                                                     {"upload (Alias: put)", DoTranslation("Uploads local file to remote directory using binary or text", currentLang)}}

    Public Sub FTPShowHelp()

        If simHelp = False Then
            For Each cmd As String In ftpDefinitions.Keys
                W("- {0}: ", "helpCmd", cmd) : Wln("{0}", "helpDef", ftpDefinitions(cmd))
            Next
        Else
            Wln(String.Join(", ", availableCommands), "neutralText")
        End If

    End Sub

End Module
