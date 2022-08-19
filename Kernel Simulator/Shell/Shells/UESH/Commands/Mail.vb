
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

Imports KS.Network.Mail
Imports KS.Shell.Shells.Mail

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Opens the mail shell
    ''' </summary>
    ''' <remarks>
    ''' This command is an entry point to the mail shell that lets you view and list messages.
    ''' <br></br>
    ''' If no address is specified, it will prompt you for the address, password, and the mail server (IMAP) if the address is not found in the ISP database. Currently, it connects with necessary requirements to ensure successful connection.
    ''' </remarks>
    Class MailCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If KeepAlive Then
                StartShell(ShellType.MailShell)
            Else
                If ListArgsOnly.Length = 0 Then
                    PromptUser()
                ElseIf Not ListArgsOnly(0) = "" Then
                    PromptPassword(ListArgsOnly(0))
                End If
            End If
        End Sub

    End Class
End Namespace
