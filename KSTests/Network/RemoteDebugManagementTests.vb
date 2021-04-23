
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

<TestClass()> Public Class RemoteDebugManagementTests

    ''' <summary>
    ''' Tests adding device to json
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestAddDeviceToJson()
        Assert.IsTrue(AddDeviceToJson("123.123.123.123"), "Adding device failed.")
    End Sub

    ''' <summary>
    ''' Tests setting device property
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestDeviceSetProperty()
        Assert.IsTrue(SetDeviceProperty("123.123.123.123", DeviceProperty.Name, "TestUser"), "Setting device property failed.")
    End Sub

    ''' <summary>
    ''' Tests getting device property
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestGetDeviceProperty()
        Assert.AreEqual("TestUser", GetDeviceProperty("123.123.123.123", DeviceProperty.Name), "Getting device property failed.")
    End Sub

End Class