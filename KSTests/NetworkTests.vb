
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
Imports System.Net.NetworkInformation

'Warning: Don't implement the unit tests related to downloading or uploading files. This causes AppVeyor to choke.
<TestClass()> Public Class NetworkTests

    ''' <summary>
    ''' Tests hostname change
    ''' </summary>
    <TestMethod()> Public Sub TestChangeHostname()
        InitPaths()
        Assert.IsTrue(ChangeHostname("NewHost"), "Changing hostname failed. Expected True, got False.")
    End Sub

    ''' <summary>
    ''' Tests pinging
    ''' </summary>
    <TestMethod()> Public Sub TestPingAddress()
        Assert.IsTrue(PingAddress("www.google.com").Status = IPStatus.Success, "Pinging failed.")
    End Sub

    ''' <summary>
    ''' Tests adding device to json
    ''' </summary>
    <TestMethod()> Public Sub TestAddDeviceToJson()
        Assert.IsTrue(AddDeviceToJson("123.123.123.123"), "Adding device failed.")
    End Sub

    ''' <summary>
    ''' Tests setting device property
    ''' </summary>
    <TestMethod()> Public Sub TestDeviceSetProperty()
        Assert.IsTrue(SetDeviceProperty("123.123.123.123", DeviceProperty.Name, "TestUser"), "Setting device property failed.")
    End Sub

    ''' <summary>
    ''' Tests getting device property
    ''' </summary>
    <TestMethod()> Public Sub TestGetDeviceProperty()
        Assert.AreEqual("TestUser", GetDeviceProperty("123.123.123.123", DeviceProperty.Name), "Getting device property failed.")
    End Sub

    ''' <summary>
    ''' Tests adding FTP speed dial entry
    ''' </summary>
    <TestMethod> Public Sub TestAddEntryToFTPSpeedDial()
        Assert.IsTrue(AddEntryToSpeedDial("ftp.riken.jp,21,anonymous,None", SpeedDialType.FTP, False))
    End Sub

    ''' <summary>
    ''' Tests adding SFTP speed dial entry
    ''' </summary>
    <TestMethod> Public Sub TestAddEntryToSFTPSpeedDial()
        Assert.IsTrue(AddEntryToSpeedDial("test.rebex.net,22,demo", SpeedDialType.SFTP, False))
    End Sub

    ''' <summary>
    ''' Tests listing FTP speed dial entries
    ''' </summary>
    <TestMethod> Public Sub TestListFTPSpeedDialEntries()
        Assert.IsTrue(ListSpeedDialEntries(SpeedDialType.FTP).Count > 0)
        Debug.WriteLine(String.Join(" | ", ListSpeedDialEntries(SpeedDialType.FTP)))
    End Sub

    ''' <summary>
    ''' Tests listing SFTP speed dial entries
    ''' </summary>
    <TestMethod> Public Sub TestListSFTPSpeedDialEntries()
        Assert.IsTrue(ListSpeedDialEntries(SpeedDialType.SFTP).Count > 0)
        Debug.WriteLine(String.Join(" | ", ListSpeedDialEntries(SpeedDialType.SFTP)))
    End Sub

End Class