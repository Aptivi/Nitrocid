
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

Imports System.IO

Class DirInfoCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        For Each Dir As String In ListArgs
            Dim DirectoryPath As String = NeutralizePath(Dir)
            Wdbg(DebugLevel.I, "Neutralized directory path: {0} ({1})", DirectoryPath, Directory.Exists(DirectoryPath))
            WriteSeparator(Dir, True)
            If Directory.Exists(DirectoryPath) Then
                Dim DirInfo As New DirectoryInfo(DirectoryPath)
                W(DoTranslation("Name: {0}"), True, ColTypes.Neutral, DirInfo.Name)
                W(DoTranslation("Full name: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.FullName))
                W(DoTranslation("Size: {0}"), True, ColTypes.Neutral, GetAllSizesInFolder(DirInfo).FileSizeToString)
                W(DoTranslation("Creation time: {0}"), True, ColTypes.Neutral, Render(DirInfo.CreationTime))
                W(DoTranslation("Last access time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastAccessTime))
                W(DoTranslation("Last write time: {0}"), True, ColTypes.Neutral, Render(DirInfo.LastWriteTime))
                W(DoTranslation("Attributes: {0}"), True, ColTypes.Neutral, DirInfo.Attributes)
                W(DoTranslation("Parent directory: {0}"), True, ColTypes.Neutral, NeutralizePath(DirInfo.Parent.FullName))
            Else
                W(DoTranslation("Can't get information about nonexistent directory."), True, ColTypes.Error)
            End If
        Next
    End Sub

End Class