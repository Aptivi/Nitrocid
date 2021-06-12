
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

<TestClass()> Public Class UserManagementTests

    ''' <summary>
    ''' Tests user addition
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestAddUser()
        InitPaths()
        AddUser("Account1").ShouldBeTrue
        AddUser("Account2", "password").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests username change
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestChangeUser()
        InitPaths()
        ChangeUsername("Account2", "Account3").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests removing user
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestRemoveUser()
        InitPaths()
        RemoveUser("Account1").ShouldBeTrue
        RemoveUser("Account3").ShouldBeTrue
    End Sub

End Class