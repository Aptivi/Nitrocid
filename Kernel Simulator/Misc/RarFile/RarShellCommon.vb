
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

Imports SharpCompress.Archives.Rar
Imports System.IO
Imports KS.Misc.RarFile.Commands

Namespace Misc.RarFile
    Module RarShellCommon

        'Variables
        Public ReadOnly RarShell_Commands As New Dictionary(Of String, CommandInfo) From {
            {"cdir", New CommandInfo("cdir", ShellType.RARShell, "Gets current local directory", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RarShell_CDirCommand)},
            {"chdir", New CommandInfo("chdir", ShellType.RARShell, "Changes directory", New CommandArgumentInfo({"<directory>"}, True, 1), New RarShell_ChDirCommand)},
            {"chadir", New CommandInfo("chadir", ShellType.RARShell, "Changes archive directory", New CommandArgumentInfo({"<archivedirectory>"}, True, 1), New RarShell_ChADirCommand)},
            {"exit", New CommandInfo("exit", ShellType.RARShell, "Exits the RAR shell", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RarShell_ExitCommand)},
            {"get", New CommandInfo("get", ShellType.RARShell, "Extracts a file to a specified directory or a current directory", New CommandArgumentInfo({"<entry> [where] [-absolute]"}, True, 1), New RarShell_GetCommand, False, False, False, False, False)},
            {"help", New CommandInfo("help", ShellType.RARShell, "Lists available commands", New CommandArgumentInfo({"[command]"}, False, 0), New RarShell_HelpCommand)},
            {"list", New CommandInfo("list", ShellType.RARShell, "Lists all files inside the archive", New CommandArgumentInfo({"[directory]"}, False, 0), New RarShell_ListCommand)}
        }
        Friend ReadOnly RarShell_ModCommands As New Dictionary(Of String, CommandInfo)
        Public RarShell_FileStream As FileStream
        Public RarShell_RarArchive As RarArchive
        Public RarShell_CurrentDirectory As String
        Public RarShell_CurrentArchiveDirectory As String
        Public RarShell_PromptStyle As String = ""

    End Module
End Namespace
