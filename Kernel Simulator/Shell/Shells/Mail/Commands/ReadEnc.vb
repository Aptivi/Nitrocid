
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

Imports KS.Network.Mail.Transfer
Imports KS.Misc.Reflection

Namespace Shell.Shells.Mail.Commands
    ''' <summary>
    ''' Shows you the content of an encrypted mail
    ''' </summary>
    ''' <remarks>
    ''' It shows you the content of a specified mail number, including whether or not it has attachments, sender and recipient information, time/date when the mail was sent, and the body.
    ''' <br></br>
    ''' This command will decrypt the mail message before displaying it, assuming that you have the public key.
    ''' </remarks>
    Class Mail_ReadEncCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Wdbg(DebugLevel.I, "Message number is numeric? {0}", IsStringNumeric(ListArgs(0)))
            If IsStringNumeric(ListArgs(0)) Then
                MailPrintMessage(ListArgs(0), True)
            Else
                Write(DoTranslation("Message number is not a numeric value."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
