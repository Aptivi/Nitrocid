
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

Imports System.IO.Compression

Class ZipCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        Dim ZipArchiveName As String = NeutralizePath(ListArgs(0))
        Dim Destination As String = NeutralizePath(ListArgs(1))
        Dim ZipCompression As CompressionLevel = CompressionLevel.Optimal
        Dim ZipBaseDir As Boolean = True
        If ListArgs?.Contains("-fast") Then
            ZipCompression = CompressionLevel.Fastest
        ElseIf ListArgs?.Contains("-nocomp") Then
            ZipCompression = CompressionLevel.NoCompression
        End If
        If ListArgs?.Contains("-nobasedir") Then
            ZipBaseDir = False
        End If
        ZipFile.CreateFromDirectory(Destination, ZipArchiveName, ZipCompression, ZipBaseDir)
    End Sub

End Class