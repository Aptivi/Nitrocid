
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

Imports Newtonsoft.Json.Linq

<TestFixture> Public Class UserManagementTests

    ''' <summary>
    ''' Tests user addition
    ''' </summary>
    <Test, Description("Management")> Public Sub TestAddUser()
        AddUser("Account1").ShouldBeTrue
        AddUser("Account2", "password").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests username change
    ''' </summary>
    <Test, Description("Management")> Public Sub TestChangeUser()
        TryChangeUsername("Account2", "Account3").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests username change
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetUserProperty()
        GetUserProperty("Account3", UserProperty.Username).ShouldBe("Account3")
        CType(GetUserProperty("Account3", UserProperty.Permissions), JArray).ShouldBeEmpty
    End Sub

    ''' <summary>
    ''' Tests removing user
    ''' </summary>
    <Test, Description("Management")> Public Sub TestRemoveUser()
        TryRemoveUser("Account1").ShouldBeTrue
        TryRemoveUser("Account3").ShouldBeTrue
    End Sub

End Class