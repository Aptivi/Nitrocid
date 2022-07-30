
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

Namespace Shell.Commands
    Class ChPwdCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                If ListArgs(3).Contains(" ") Then
                    Write(DoTranslation("Spaces are not allowed."), True, ColTypes.Error)
                ElseIf ListArgs(3) = ListArgs(2) Then
                    ChangePassword(ListArgs(0), ListArgs(1), ListArgs(2))
                ElseIf ListArgs(3) <> ListArgs(2) Then
                    Write(DoTranslation("Passwords doesn't match."), True, ColTypes.Error)
                End If
            Catch ex As Exception
                Write(DoTranslation("Failed to change password of username: {0}"), True, ColTypes.Error, ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

    End Class
End Namespace