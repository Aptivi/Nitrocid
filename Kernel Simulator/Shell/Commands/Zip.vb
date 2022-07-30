
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

Imports System.IO.Compression

Namespace Shell.Commands
    Class ZipCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ZipArchiveName As String = NeutralizePath(ListArgsOnly(0))
            Dim Destination As String = NeutralizePath(ListArgsOnly(1))
            Dim ZipCompression As CompressionLevel = CompressionLevel.Optimal
            Dim ZipBaseDir As Boolean = True
            If ListSwitchesOnly.Contains("-fast") Then
                ZipCompression = CompressionLevel.Fastest
            ElseIf ListSwitchesOnly.Contains("-nocomp") Then
                ZipCompression = CompressionLevel.NoCompression
            End If
            If ListSwitchesOnly.Contains("-nobasedir") Then
                ZipBaseDir = False
            End If
            ZipFile.CreateFromDirectory(Destination, ZipArchiveName, ZipCompression, ZipBaseDir)
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -fast: ", False, ColTypes.ListEntry) : Write(DoTranslation("Fast compression"), True, ColTypes.ListValue)
            Write("  -nocomp: ", False, ColTypes.ListEntry) : Write(DoTranslation("No compression"), True, ColTypes.ListValue)
            Write("  -nobasedir: ", False, ColTypes.ListEntry) : Write(DoTranslation("Don't create base directory in archive"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace