
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

<TestClass()> Public Class UserTests

    <TestMethod()> Public Sub TestAddUser()
        InitPaths()
        Assert.IsTrue(AddUser("Account1"), "User addition without password failed. Expected True, got False.")
        Assert.IsTrue(AddUser("Account2", "password"), "User addition with password failed. Expected True, got False.")
    End Sub

    <TestMethod()> Public Sub TestInitializeUsers()
        InitPaths()
        InitializeUsers()
        Assert.IsTrue(userword.Count > 0, "User initialization failed. Got {0}.", userword.Count)
    End Sub

    <TestMethod()> Public Sub TestChangeUser()
        InitPaths()
        Assert.IsTrue(ChangeUsername("Account2", "Account3"), "Username change failed. Expected True, got False.")
    End Sub

    <TestMethod()> Public Sub TestPermAddUser()
        InitPaths()
        Assert.IsTrue(AddPermission(PermissionType.Administrator, "Account3"), "Adding user to admin permission failed. Expected True, got False.")
        Assert.IsTrue(AddPermission(PermissionType.Disabled, "Account3"), "Adding user to disabled permission failed. Expected True, got False.")
    End Sub

    <TestMethod()> Public Sub TestPermRemoveUser()
        InitPaths()
        Assert.IsTrue(RemovePermission(PermissionType.Administrator, "Account3"), "Removing user from admin permission failed. Expected True, got False.")
        Assert.IsTrue(RemovePermission(PermissionType.Disabled, "Account3"), "Rmoving user from disabled permission failed. Expected True, got False.")
    End Sub

    <TestMethod()> Public Sub TestLoadPermissions()
        InitPaths()
        Assert.IsTrue(LoadPermissions, "Loading permissions. Expected True, got False.")
    End Sub

    <TestMethod()> Public Sub TestRemoveUser()
        InitPaths()
        Assert.IsTrue(RemoveUser("Account1"), "User removal without password failed. Expected True, got False.")
        Assert.IsTrue(RemoveUser("Account3"), "User removal with password failed. Expected True, got False.")
    End Sub

End Class