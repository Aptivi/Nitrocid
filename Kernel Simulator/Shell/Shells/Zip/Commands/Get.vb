
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

Imports KS.Misc.ZipFile

Namespace Shell.Shells.Zip.Commands
    ''' <summary>
    ''' Extract a file from a ZIP archive
    ''' </summary>
    ''' <remarks>
    ''' If you want to get a single file from the ZIP archive, you can use this command to extract such file to the current working directory, or a specified directory.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-absolute</term>
    ''' <description>Uses the full target path</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class ZipShell_GetCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Where As String = ""
            Dim Absolute As Boolean
            If ListArgsOnly.Length > 1 Then
                If Not ListSwitchesOnly(0) = "-absolute" Then Where = NeutralizePath(ListArgsOnly(1))
                If ListSwitchesOnly.Contains("-absolute") Then
                    Absolute = True
                End If
            End If
            ExtractZipFileEntry(ListArgsOnly(0), Where, Absolute)
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -absolute: ", False, ColTypes.ListEntry) : Write(DoTranslation("Indicates that the target path is absolute"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
