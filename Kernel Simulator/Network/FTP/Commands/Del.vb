
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

Imports KS.Network.FTP.Filesystem

Namespace Network.FTP.Commands
    Class FTP_DelCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If FtpConnected = True Then
                'Print a message
                Write(DoTranslation("Deleting {0}..."), True, ColTypes.Progress, ListArgs(0))

                'Make a confirmation message so user will not accidentally delete a file or folder
                Write(DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", False, ColTypes.Input, ListArgs(0))
                Dim answer As String = Console.ReadKey.KeyChar
                Console.WriteLine()

                Try
                    FTPDeleteRemote(ListArgs(0))
                Catch ex As Exception
                    Write(ex.Message, True, ColTypes.Error)
                End Try
            Else
                Write(DoTranslation("You must connect to server with administrative privileges before performing the deletion."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace