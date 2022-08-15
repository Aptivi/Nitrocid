
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
Imports System.IO.Compression
Imports KS.Files.Folders

Namespace Shell.Shells.UESH.Commands
    Class UnZipCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Length = 1 Then
                Dim ZipArchiveName As String = NeutralizePath(ListArgs(0))
                ZipFile.ExtractToDirectory(ZipArchiveName, CurrentDir)
            ElseIf ListArgs?.Length > 1 Then
                Dim ZipArchiveName As String = NeutralizePath(ListArgs(0))
                Dim Destination As String = If(Not ListArgs(1) = "-createdir", NeutralizePath(ListArgs(1)), "")
                If ListArgs?.Contains("-createdir") Then
                    Destination = $"{If(Not ListArgs(1) = "-createdir", NeutralizePath(ListArgs(1)), "")}/{If(Not ListArgs(1) = "-createdir", Path.GetFileNameWithoutExtension(ZipArchiveName), NeutralizePath(Path.GetFileNameWithoutExtension(ZipArchiveName)))}"
                    If Destination(0) = "/" Then Destination = Destination.RemoveLetter(0)
                End If
                ZipFile.ExtractToDirectory(ZipArchiveName, Destination)
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -createdir: ", False, ColTypes.ListEntry) : Write(DoTranslation("Creates a directory that contains the contents of the ZIP file"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
