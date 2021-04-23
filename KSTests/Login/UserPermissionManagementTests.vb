
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

<TestClass()> Public Class UserPermissionManagementTests

    <ClassInitialize> Public Shared Sub AddNecessaryUser(Context As TestContext)
        AddUser("Account")
    End Sub

    ''' <summary>
    ''' Tests adding permissions to user
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestAddUserPerm()
        InitPaths()
        Assert.IsTrue(AddPermission(PermissionType.Administrator, "Account"), "Adding user to admin permission failed. Expected True, got False.")
        Assert.IsTrue(AddPermission(PermissionType.Disabled, "Account"), "Adding user to disabled permission failed. Expected True, got False.")
    End Sub

    ''' <summary>
    ''' Tests removing permissions from user
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestRemoveUserPerm()
        InitPaths()
        Assert.IsTrue(RemovePermission(PermissionType.Administrator, "Account"), "Removing user from admin permission failed. Expected True, got False.")
        Assert.IsTrue(RemovePermission(PermissionType.Disabled, "Account"), "Rmoving user from disabled permission failed. Expected True, got False.")
    End Sub

    ''' <summary>
    ''' Tests loading permissions
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestLoadPermissions()
        InitPaths()
        Assert.IsTrue(LoadPermissions, "Loading permissions. Expected True, got False.")
    End Sub

    <ClassCleanup> Public Shared Sub RemoveNecessaryUser()
        RemoveUser("Account")
    End Sub

End Class