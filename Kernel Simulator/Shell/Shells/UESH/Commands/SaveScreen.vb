
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

Imports KS.Misc.Screensaver

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Launches your current screen saver
    ''' </summary>
    ''' <remarks>
    ''' This command can protect your LCD screen from burn-in and shows you the current screensaver that is set by you or by the kernel. However it doesn't lock the user account, so we recommend to lock your screen for any purposes, unless you're testing your own screensaver from the screensaver modfile.
    ''' </remarks>
    Class SaveScreenCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If Not ListArgs?.Length = 0 Then
                ShowSavers(ListArgs(0))
            Else
                ShowSavers(DefSaverName)
            End If
        End Sub

    End Class
End Namespace
