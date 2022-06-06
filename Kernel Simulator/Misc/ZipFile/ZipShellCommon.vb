
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports System.IO
Imports System.IO.Compression
Imports KS.Misc.ZipFile.Commands

Namespace Misc.ZipFile
    Module ZipShellCommon

        'Variables
        Public ReadOnly ZipShell_Commands As New Dictionary(Of String, CommandInfo) From {{"cdir", New CommandInfo("cdir", ShellType.ZIPShell, "Gets current local directory", {}, False, 0, New ZipShell_CDirCommand)},
                                                                                          {"chdir", New CommandInfo("chdir", ShellType.ZIPShell, "Changes directory", {"<directory>"}, True, 1, New ZipShell_ChDirCommand)},
                                                                                          {"chadir", New CommandInfo("chadir", ShellType.ZIPShell, "Changes archive directory", {"<archivedirectory>"}, True, 1, New ZipShell_ChADirCommand)},
                                                                                          {"exit", New CommandInfo("exit", ShellType.ZIPShell, "Exits the ZIP shell", {}, False, 0, New ZipShell_ExitCommand)},
                                                                                          {"get", New CommandInfo("get", ShellType.ZIPShell, "Extracts a file to a specified directory or a current directory", {"<entry> [where] [-absolute]"}, True, 1, New ZipShell_GetCommand, False, False, False, False, False)},
                                                                                          {"help", New CommandInfo("help", ShellType.ZIPShell, "Lists available commands", {"[command]"}, False, 0, New ZipShell_HelpCommand)},
                                                                                          {"list", New CommandInfo("list", ShellType.ZIPShell, "Lists all files inside the archive", {"[directory]"}, False, 0, New ZipShell_ListCommand)},
                                                                                          {"pack", New CommandInfo("pack", ShellType.ZIPShell, "Packs a local file to the archive", {"<localfile> [where]"}, True, 1, New ZipShell_PackCommand)}}
        Public ZipShell_ModCommands As New Dictionary(Of String, CommandInfo)
        Public ZipShell_FileStream As FileStream
        Public ZipShell_ZipArchive As ZipArchive
        Public ZipShell_CurrentDirectory As String
        Public ZipShell_CurrentArchiveDirectory As String
        Public ZipShell_PromptStyle As String = ""

    End Module
End Namespace
