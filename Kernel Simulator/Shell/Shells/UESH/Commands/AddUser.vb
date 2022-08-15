
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
    ''' You can add the user's name whenever you need, with the password if required.
    ''' </summary>
    ''' <remarks>
    ''' If you need to add a person that wants to use the kernel, you can add users for them, and let them specify the password if they need. This way, adduser will only create an account and gets the permissions for the new user ready, and the new user will be a normal account for security reasons.
    ''' <br></br>
    ''' However if you need to add a person that has admin rights, you should set the permission for the user to allow admin rights. If you want to temporarily disable an account so it blocks the log-on request to that account, you should set the disabled permission to Enabled.
    ''' <br></br>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class AddUserCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Length = 1 Then
                Write(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, ListArgs(0))
                AddUser(ListArgs(0))
            ElseIf ListArgs?.Length > 2 Then
                If ListArgs(1) = ListArgs(2) Then
                    Write(DoTranslation("usrmgr: Creating username {0}..."), True, ColTypes.Neutral, ListArgs(0))
                    AddUser(ListArgs(0), ListArgs(1))
                Else
                    Write(DoTranslation("Passwords don't match."), True, ColTypes.Error)
                End If
            End If
        End Sub

    End Class
End Namespace
