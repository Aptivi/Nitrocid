
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports SharpCompress.Archives.Zip
Imports System.IO
Imports KS.Misc.ZipFile.Commands

Namespace Misc.ZipFile
    Module ZipShellCommon

        'Variables
        Public ReadOnly ZipShell_Commands As New Dictionary(Of String, CommandInfo) From {
            {"cdir", New CommandInfo("cdir", ShellType.ZIPShell, "Gets current local directory", New CommandArgumentInfo(), New ZipShell_CDirCommand)},
            {"chdir", New CommandInfo("chdir", ShellType.ZIPShell, "Changes directory", New CommandArgumentInfo({"<directory>"}, True, 1), New ZipShell_ChDirCommand)},
            {"chadir", New CommandInfo("chadir", ShellType.ZIPShell, "Changes archive directory", New CommandArgumentInfo({"<archivedirectory>"}, True, 1), New ZipShell_ChADirCommand)},
            {"get", New CommandInfo("get", ShellType.ZIPShell, "Extracts a file to a specified directory or a current directory", New CommandArgumentInfo({"<entry> [where] [-absolute]"}, True, 1), New ZipShell_GetCommand)},
            {"help", New CommandInfo("help", ShellType.ZIPShell, "Lists available commands", New CommandArgumentInfo({"[command]"}, False, 0), New ZipShell_HelpCommand)},
            {"list", New CommandInfo("list", ShellType.ZIPShell, "Lists all files inside the archive", New CommandArgumentInfo({"[directory]"}, False, 0), New ZipShell_ListCommand)},
            {"pack", New CommandInfo("pack", ShellType.ZIPShell, "Packs a local file to the archive", New CommandArgumentInfo({"<localfile> [where]"}, True, 1), New ZipShell_PackCommand)}
        }
        Friend ReadOnly ZipShell_ModCommands As New Dictionary(Of String, CommandInfo)
        Public ZipShell_FileStream As FileStream
        Public ZipShell_ZipArchive As ZipArchive
        Public ZipShell_CurrentDirectory As String
        Public ZipShell_CurrentArchiveDirectory As String

    End Module
End Namespace
