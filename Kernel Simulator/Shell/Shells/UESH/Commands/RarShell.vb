
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

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Opens an RAR shell
    ''' </summary>
    ''' <remarks>
    ''' If you wanted to interact with an RAR file more thoroughly, you can use this command to open a shell to an RAR file.
    ''' </remarks>
    Class RarShellCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            ListArgsOnly(0) = NeutralizePath(ListArgsOnly(0))
            Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", ListArgsOnly(0), FileExists(ListArgsOnly(0)))
            If FileExists(ListArgsOnly(0)) Then
                StartShell(ShellType.RARShell, ListArgsOnly(0))
            Else
                Write(DoTranslation("File doesn't exist."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
