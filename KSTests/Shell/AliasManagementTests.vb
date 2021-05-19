
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

<TestClass()> Public Class AliasManagementTests

    ''' <summary>
    ''' Tests alias addition
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestAddAlias()
        AddAlias("ls", "list", AliasType.Shell).ShouldBeTrue
        AddAlias("trc", "trace", AliasType.RDebug).ShouldBeTrue
        Aliases.ShouldContainKey("ls")
        RemoteDebugAliases.ShouldContainKey("trc")
    End Sub

    ''' <summary>
    ''' Tests alias removal
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestRemoveAlias()
        RemoveAlias("ls", AliasType.Shell).ShouldBeTrue
        RemoveAlias("trc", AliasType.RDebug).ShouldBeTrue
        Aliases.ShouldNotContainKey("ls")
        RemoteDebugAliases.ShouldNotContainKey("trc")
    End Sub

End Class