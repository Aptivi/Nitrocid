
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports KS.Network

<TestFixture> Public Class SpeedDialManagementTests

    ''' <summary>
    ''' Tests adding FTP speed dial entry
    ''' </summary>
    <Test, Description("Management")> Public Sub TestAddEntryToFTPSpeedDial()
        AddEntryToSpeedDial("ftp.riken.jp", "21", "anonymous", SpeedDialType.FTP, FluentFTP.FtpEncryptionMode.None, False).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests adding SFTP speed dial entry
    ''' </summary>
    <Test, Description("Management")> Public Sub TestAddEntryToSFTPSpeedDial()
        AddEntryToSpeedDial("test.rebex.net", "22", "demo", SpeedDialType.SFTP, FluentFTP.FtpEncryptionMode.None, False).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests listing FTP speed dial entries
    ''' </summary>
    <Test, Description("Management")> Public Sub TestListFTPSpeedDialEntries()
        ListSpeedDialEntries(SpeedDialType.FTP).ShouldNotBeEmpty
        Debug.WriteLine(String.Join(" | ", ListSpeedDialEntries(SpeedDialType.FTP).Keys))
    End Sub

    ''' <summary>
    ''' Tests listing SFTP speed dial entries
    ''' </summary>
    <Test, Description("Management")> Public Sub TestListSFTPSpeedDialEntries()
        ListSpeedDialEntries(SpeedDialType.SFTP).ShouldNotBeEmpty
        Debug.WriteLine(String.Join(" | ", ListSpeedDialEntries(SpeedDialType.SFTP).Keys))
    End Sub

End Class