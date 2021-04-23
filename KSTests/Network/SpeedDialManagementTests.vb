
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports KS

<TestClass()> Public Class SpeedDialManagementTests

    ''' <summary>
    ''' Tests adding FTP speed dial entry
    ''' </summary>
    <TestMethod> <TestCategory("Management")> Public Sub TestAddEntryToFTPSpeedDial()
        Assert.IsTrue(AddEntryToSpeedDial("ftp.riken.jp,21,anonymous,None", SpeedDialType.FTP, False))
    End Sub

    ''' <summary>
    ''' Tests adding SFTP speed dial entry
    ''' </summary>
    <TestMethod> <TestCategory("Management")> Public Sub TestAddEntryToSFTPSpeedDial()
        Assert.IsTrue(AddEntryToSpeedDial("test.rebex.net,22,demo", SpeedDialType.SFTP, False))
    End Sub

    ''' <summary>
    ''' Tests listing FTP speed dial entries
    ''' </summary>
    <TestMethod> <TestCategory("Management")> Public Sub TestListFTPSpeedDialEntries()
        Assert.IsTrue(ListSpeedDialEntries(SpeedDialType.FTP).Count > 0)
        Debug.WriteLine(String.Join(" | ", ListSpeedDialEntries(SpeedDialType.FTP)))
    End Sub

    ''' <summary>
    ''' Tests listing SFTP speed dial entries
    ''' </summary>
    <TestMethod> <TestCategory("Management")> Public Sub TestListSFTPSpeedDialEntries()
        Assert.IsTrue(ListSpeedDialEntries(SpeedDialType.SFTP).Count > 0)
        Debug.WriteLine(String.Join(" | ", ListSpeedDialEntries(SpeedDialType.SFTP)))
    End Sub

End Class