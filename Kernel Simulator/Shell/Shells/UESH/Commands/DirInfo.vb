
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

Imports KS.Files.Querying
Imports KS.TimeDate
Imports System.IO

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Shows directory information
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to view directory information.
    ''' </remarks>
    Class DirInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            For Each Dir As String In ListArgsOnly
                Dim DirectoryPath As String = NeutralizePath(Dir)
                Wdbg(DebugLevel.I, "Neutralized directory path: {0} ({1})", DirectoryPath, FolderExists(DirectoryPath))
                WriteSeparator(Dir, True)
                If FolderExists(DirectoryPath) Then
                    Dim DirInfo As New DirectoryInfo(DirectoryPath)
                    Write(DoTranslation("Name: {0}"), True, ColTypes.Neutral, DirInfo.Name)
                    Write(DoTranslation("Full name: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.FullName))
                    Write(DoTranslation("Size: {0}"), True, ColTypes.Neutral, GetAllSizesInFolder(DirInfo).FileSizeToString)
                    Write(DoTranslation("Creation time: {0}"), True, ColTypes.Neutral, Render(DirInfo.CreationTime))
                    Write(DoTranslation("Last access time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastAccessTime))
                    Write(DoTranslation("Last write time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastWriteTime))
                    Write(DoTranslation("Attributes: {0}"), True, ColTypes.Neutral, DirInfo.Attributes)
                    Write(DoTranslation("Parent directory: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.Parent.FullName))
                Else
                    Write(DoTranslation("Can't get information about nonexistent directory."), True, ColTypes.Error)
                End If
            Next
        End Sub

    End Class
End Namespace
