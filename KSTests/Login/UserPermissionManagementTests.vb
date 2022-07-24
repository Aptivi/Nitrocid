
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

<TestFixture> Public Class UserPermissionManagementTests

    <SetUp> Public Shared Sub AddNecessaryUser()
        AddUser("Account")
    End Sub

    ''' <summary>
    ''' Tests adding permissions to user
    ''' </summary>
    <Test, Description("Management")> Public Sub TestAddUserPerm()
        TryAddPermission(PermissionType.Administrator, "Account").ShouldBeTrue
        TryAddPermission(PermissionType.Disabled, "Account").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests removing permissions from user
    ''' </summary>
    <Test, Description("Management")> Public Sub TestRemoveUserPerm()
        TryRemovePermission(PermissionType.Administrator, "Account").ShouldBeTrue
        TryRemovePermission(PermissionType.Disabled, "Account").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests loading permissions
    ''' </summary>
    <Test, Description("Management")> Public Sub TestLoadPermissions()
        TryLoadPermissions.ShouldBeTrue
    End Sub

    <TearDown> Public Shared Sub RemoveNecessaryUser()
        RemoveUser("Account")
    End Sub

End Class