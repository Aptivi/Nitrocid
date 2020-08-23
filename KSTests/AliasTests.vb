
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

Imports System.IO
Imports KS

<TestClass()> Public Class AliasTests

    <TestMethod()> Public Sub TestInitAliases()
        Try
            InitPaths()
            Dim PathToTestAliases As String = Path.GetFullPath("TestAliases.csv")
            If Not File.Exists(paths("Aliases")) Then File.Copy(PathToTestAliases, paths("Aliases"))
            InitAliases()
            Assert.IsTrue(Aliases.Count > 0)
            Assert.IsTrue(RemoteDebugAliases.Count > 0)
            CloseAliasesFile()
        Catch afex As AssertFailedException
            Assert.Fail("Initialization of test aliases failed. Local and remote aliases below:" + vbNewLine + vbNewLine +
                        "- " + String.Join(vbNewLine + "- ", Aliases.Keys) + vbNewLine + vbNewLine +
                        "- " + String.Join(vbNewLine + "- ", RemoteDebugAliases.Keys))
        End Try
    End Sub

    <TestMethod()> Public Sub TestInitAndSaveAliases()
        Try
            InitPaths()
            Dim PathToTestAliases As String = Path.GetFullPath("TestAliases.csv")
            If Not File.Exists(paths("Aliases")) Then File.Copy(PathToTestAliases, paths("Aliases"))
            InitAliases()
            SaveAliases()
            Assert.IsTrue(Aliases.Count > 0)
            Assert.IsTrue(RemoteDebugAliases.Count > 0)
            CloseAliasesFile()
        Catch afex As AssertFailedException
            Assert.Fail("Initialization and saving of test aliases failed. Local and remote aliases below:" + vbNewLine + vbNewLine +
                        "- " + String.Join(vbNewLine + "- ", Aliases.Keys) + vbNewLine + vbNewLine +
                        "- " + String.Join(vbNewLine + "- ", RemoteDebugAliases.Keys))
        End Try
    End Sub

    <TestMethod> Public Sub TestAddAlias()
        Try
            Assert.IsTrue(AddAlias("ls", "list", AliasType.Shell))
            Assert.IsTrue(AddAlias("trc", "trace", AliasType.RDebug))
        Catch ex As Exception
            Assert.Fail("Adding aliases failed.")
        End Try
    End Sub

    <TestMethod> Public Sub TestRemoveAlias()
        Try
            Assert.IsTrue(RemoveAlias("ls", AliasType.Shell))
            Assert.IsTrue(RemoveAlias("trc", AliasType.RDebug))
        Catch ex As Exception
            Assert.Fail("Removing aliases failed.")
        End Try
    End Sub

End Class