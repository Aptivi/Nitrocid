
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
    ''' Changes current archive directory
    ''' </summary>
    ''' <remarks>
    ''' If you want to go to a folder inside the ZIP archive, you can use this command to change the working archive directory.
    ''' </remarks>
    Class ZipShell_ChADirCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If Not ChangeWorkingArchiveDirectory(ListArgs(0)) Then
                Write(DoTranslation("Archive directory {0} doesn't exist"), True, ColTypes.Error, ListArgs(0))
            End If
        End Sub

    End Class
End Namespace
