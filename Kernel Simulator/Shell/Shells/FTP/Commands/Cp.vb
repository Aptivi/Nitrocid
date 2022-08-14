
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

Namespace Shell.Shells.FTP.Commands
    Class FTP_CpCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If FtpConnected Then
                Write(DoTranslation("Copying {0} to {1}..."), True, ColTypes.Neutral, ListArgs(0), ListArgs(1))
                If FTPCopyItem(ListArgs(0), ListArgs(1)) Then
                    Write(NewLine + DoTranslation("Copied successfully"), True, ColTypes.Success)
                Else
                    Write(NewLine + DoTranslation("Failed to copy {0} to {1}."), True, ColTypes.Error, ListArgs(0), ListArgs(1))
                End If
            Else
                Write(DoTranslation("You must connect to server before performing transmission."), True, ColTypes.Error)
            End If
        End Sub

    End Class
End Namespace
