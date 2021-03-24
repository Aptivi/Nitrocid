
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

Imports System.IO
Imports KS

<TestClass()> Public Class AliasTests

    ''' <summary>
    ''' Tests alias initialization
    ''' </summary>
    <TestMethod()> Public Sub TestInitAliases()
        InitPaths()
        Dim PathToTestAliases As String = Path.GetFullPath("TestAliases.json")
        If Not File.Exists(paths("Aliases")) Then File.Copy(PathToTestAliases, paths("Aliases"))
        InitAliases()
        Assert.IsTrue(Aliases.Count > 0, "Initialization of test aliases failed. Aliases.Count is {0}. Shell aliases below:" + vbNewLine + vbNewLine +
                                         "- " + String.Join(vbNewLine + "- ", Aliases.Keys), Aliases.Count)
        Assert.IsTrue(RemoteDebugAliases.Count > 0, "Initialization of test aliases failed. RemoteDebugAliases.Count is {0}. Remote aliases below:" + vbNewLine + vbNewLine +
                                                    "- " + String.Join(vbNewLine + "- ", RemoteDebugAliases.Keys), RemoteDebugAliases.Count)
    End Sub

    ''' <summary>
    ''' Tests alias initialization and saving
    ''' </summary>
    <TestMethod()> Public Sub TestInitAndSaveAliases()
        InitPaths()
        Dim PathToTestAliases As String = Path.GetFullPath("TestAliases.json")
        If Not File.Exists(paths("Aliases")) Then File.Copy(PathToTestAliases, paths("Aliases"))
        InitAliases()
        SaveAliases()
        Assert.IsTrue(Aliases.Count > 0, "Initialization and saving of test aliases failed. Aliases.Count is {0}. Shell aliases below:" + vbNewLine + vbNewLine +
                                         "- " + String.Join(vbNewLine + "- ", Aliases.Keys), Aliases.Count)
        Assert.IsTrue(RemoteDebugAliases.Count > 0, "Initialization and saving of test aliases failed. RemoteDebugAliases.Count is {0}. Remote aliases below:" + vbNewLine + vbNewLine +
                                                    "- " + String.Join(vbNewLine + "- ", RemoteDebugAliases.Keys), RemoteDebugAliases.Count)
    End Sub

    ''' <summary>
    ''' Tests alias addition
    ''' </summary>
    <TestMethod> Public Sub TestAddAlias()
        Assert.IsTrue(AddAlias("ls", "list", AliasType.Shell), "Adding shell alias failed. Returned False.")
        Assert.IsTrue(AddAlias("trc", "trace", AliasType.RDebug), "Adding remote debug alias failed. Returned False.")
    End Sub

    ''' <summary>
    ''' Tests alias removal
    ''' </summary>
    <TestMethod> Public Sub TestRemoveAlias()
        Assert.IsTrue(RemoveAlias("ls", AliasType.Shell), "Removing shell alias failed. Returned False.")
        Assert.IsTrue(RemoveAlias("trc", AliasType.RDebug), "Removing remote debug alias failed. Returned False.")
    End Sub

End Class