
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

Imports KS.Network.RemoteDebug

<TestFixture> Public Class RemoteDebugManagementTests

    ''' <summary>
    ''' Tests adding device to json
    ''' </summary>
    <Test, Description("Management")> Public Sub TestAddDeviceToJson()
        AddDeviceToJson("123.123.123.123").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests setting device property
    ''' </summary>
    <Test, Description("Management")> Public Sub TestDeviceSetProperty()
        SetDeviceProperty("123.123.123.123", DeviceProperty.Name, "TestUser").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests getting device property
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetDeviceProperty()
        CStr(GetDeviceProperty("123.123.123.123", DeviceProperty.Name)).ShouldBe("TestUser")
    End Sub

    ''' <summary>
    ''' Removes a test device created by <see cref="TestAddDeviceToJson()"/>
    ''' </summary>
    <OneTimeTearDown> Public Shared Sub TestRemoveTestDevice()
        RemoveDeviceFromJson("123.123.123.123").ShouldBeTrue
    End Sub

End Class