
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

Imports KS.Network.Mail.Directory
Imports KS.Misc.Reflection

Namespace Shell.Shells.Mail.Commands
    ''' <summary>
    ''' Moves a message to a folder
    ''' </summary>
    ''' <remarks>
    ''' This command lets you move a message to a folder. It is especially useful when organizing mail messages manually.
    ''' </remarks>
    Class Mail_MvCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Wdbg(DebugLevel.I, "Message number is numeric? {0}", IsStringNumeric(ListArgsOnly(0)))
            If IsStringNumeric(ListArgsOnly(0)) Then
                MailMoveMessage(ListArgsOnly(0), ListArgsOnly(1))
            Else
                Write(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
