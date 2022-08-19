
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

Imports KS.Network.FTP

Namespace Shell.Shells.FTP.Commands
    ''' <summary>
    ''' Connects your FTP client to any FTP server that is valid
    ''' </summary>
    ''' <remarks>
    ''' This command must be executed before running any interactive FTP server commands, like get, put, cdl, cdr, etc.
    ''' <br></br>
    ''' This command opens a new session to connect your FTP client to any FTP server that is open to the public, and valid. It then asks for your credentials. Try with anonymous first, then usernames.
    ''' </remarks>
    Class FTP_ConnectCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly(0).StartsWith("ftp://") Or ListArgsOnly(0).StartsWith("ftps://") Or ListArgsOnly(0).StartsWith("ftpes://") Then
                TryToConnect(ListArgsOnly(0))
            Else
                TryToConnect($"ftp://{ListArgsOnly(0)}")
            End If
        End Sub

    End Class
End Namespace
