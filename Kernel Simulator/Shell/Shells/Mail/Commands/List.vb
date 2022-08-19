
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
    ''' Lists all messages in the current folder
    ''' </summary>
    ''' <remarks>
    ''' It allows you to list all the messages in the current working folder in pages. It lists 10 messages in a page, so you can optionally specify the page number.
    ''' </remarks>
    Class Mail_ListCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly.Length > 0 Then
                Wdbg(DebugLevel.I, "Page is numeric? {0}", IsStringNumeric(ListArgsOnly(0)))
                If IsStringNumeric(ListArgsOnly(0)) Then
                    MailListMessages(ListArgsOnly(0))
                Else
                    Write(DoTranslation("Page is not a numeric value."), True, ColTypes.Error)
                End If
            Else
                MailListMessages(1)
            End If
        End Sub

    End Class
End Namespace
