
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
    Class FTP_LsrCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ShowFileDetails As Boolean = ListSwitchesOnly.Contains("-showdetails") OrElse FtpShowDetailsInList
            Dim Entries As New List(Of String)
            If Not ListArgsOnly.Length = 0 Then
                For Each TargetDirectory As String In ListArgsOnly
                    Entries = FTPListRemote(TargetDirectory, ShowFileDetails)
                Next
            Else
                Entries = FTPListRemote("", ShowFileDetails)
            End If
            Entries.Sort()
            For Each Entry As String In Entries
                Write(Entry, True, ColTypes.ListEntry)
            Next
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -showdetails: ", False, ColTypes.ListEntry) : Write(DoTranslation("Shows the file details in the list"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace