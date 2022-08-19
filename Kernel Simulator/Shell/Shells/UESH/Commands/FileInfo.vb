
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

Imports System.IO
Imports KS.Files.LineEndings
Imports KS.Files.Querying
Imports KS.TimeDate

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Shows file information
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to view file information.
    ''' </remarks>
    Class FileInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            For Each FileName As String In ListArgsOnly
                Dim FilePath As String = NeutralizePath(FileName)
                Wdbg(DebugLevel.I, "Neutralized file path: {0} ({1})", FilePath, FileExists(FilePath))
                WriteSeparator(FileName, True)
                If FileExists(FilePath) Then
                    Dim FileInfo As New FileInfo(FilePath)
                    Dim Style As FilesystemNewlineStyle = GetLineEndingFromFile(FilePath)
                    Write(DoTranslation("Name: {0}"), True, ColTypes.Neutral, FileInfo.Name)
                    Write(DoTranslation("Full name: {0}"), True, ColTypes.Neutral, NeutralizePath(FileInfo.FullName))
                    Write(DoTranslation("File size: {0}"), True, ColTypes.Neutral, FileInfo.Length.FileSizeToString)
                    Write(DoTranslation("Creation time: {0}"), True, ColTypes.Neutral, Render(FileInfo.CreationTime))
                    Write(DoTranslation("Last access time: {0}"), True, ColTypes.Neutral, Render(FileInfo.LastAccessTime))
                    Write(DoTranslation("Last write time: {0}"), True, ColTypes.Neutral, Render(FileInfo.LastWriteTime))
                    Write(DoTranslation("Attributes: {0}"), True, ColTypes.Neutral, FileInfo.Attributes)
                    Write(DoTranslation("Where to find: {0}"), True, ColTypes.Neutral, NeutralizePath(FileInfo.DirectoryName))
                    Write(DoTranslation("Newline style:") + " {0}", True, ColTypes.Neutral, Style.ToString)
                Else
                    Write(DoTranslation("Can't get information about nonexistent file."), True, ColTypes.Error)
                End If
            Next
        End Sub

    End Class
End Namespace
