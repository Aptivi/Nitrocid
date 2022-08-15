
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

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' You can log out of your account.
    ''' </summary>
    ''' <remarks>
    ''' If there is a change that requires log-out and log-in for the changes to take effect, you must log off and log back in.
    ''' <br></br>
    ''' This command lets you off your account and sign in as somebody else. When you're finished with your account, and you want to use either the root account, or let someone else use their account, you must sign out.
    ''' </remarks>
    Class LogoutCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ShellStack.Count = 1 Then
                LogoutRequested = True
            Else
                Write(DoTranslation("Cannot log out from the subshell."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
