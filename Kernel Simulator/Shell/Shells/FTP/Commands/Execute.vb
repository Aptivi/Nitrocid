
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

Namespace Shell.Shells.FTP.Commands
    ''' <summary>
    ''' Executes a server command
    ''' </summary>
    ''' <remarks>
    ''' If you want to go advanced and execute a server command to the FTP server, you can use this command.
    ''' </remarks>
    Class FTP_ExecuteCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If FtpConnected Then
                Write("<<< C: {0}", True, ColTypes.Neutral, StringArgs)
                Dim ExecutedReply As FtpReply = ClientFTP.Execute(StringArgs)
                If ExecutedReply.Success Then
                    Write(">>> [{0}] M: {1}", True, ColTypes.Success, ExecutedReply.Code, ExecutedReply.Message)
                    Write(">>> [{0}] I: {1}", True, ColTypes.Success, ExecutedReply.Code, ExecutedReply.InfoMessages)
                Else
                    Write(">>> [{0}] M: {1}", True, ColTypes.Error, ExecutedReply.Code, ExecutedReply.Message)
                    Write(">>> [{0}] I: {1}", True, ColTypes.Error, ExecutedReply.Code, ExecutedReply.InfoMessages)
                    Write(">>> [{0}] E: {1}", True, ColTypes.Error, ExecutedReply.Code, ExecutedReply.ErrorMessage)
                End If
            Else
                Write(DoTranslation("You haven't connected to any server yet"), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
