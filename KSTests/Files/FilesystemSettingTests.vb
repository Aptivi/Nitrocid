
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

<TestClass()> Public Class FilesystemSettingTests

    ''' <summary>
    ''' Tests current directory setting
    ''' </summary>
    <TestMethod()> <TestCategory("Setting")> Public Sub TestSetCurrDir()
        InitPaths()
        CurrDir = paths("Home")
        Dim Path As String = paths("Home") + "/Documents"
        Assert.IsTrue(SetCurrDir(Path), "Failed to set current path. Expected True, got False.")
        Assert.AreEqual(Path, CurrDir, "Current path is not properly set. Expected {0}, got {1}", Path, CurrDir)
    End Sub

    ''' <summary>
    ''' Tests setting size parse mode
    ''' </summary>
    <TestMethod()> <TestCategory("Setting")> Public Sub TestSetSizeParseMode()
        InitPaths()
        Assert.IsTrue(SetSizeParseMode(True), "Failed to set size parse mode to True. Expected True, got False.")
        Assert.IsTrue(SetSizeParseMode(False), "Failed to set size parse mode to False. Expected True, got False.")
        Assert.IsTrue(SetSizeParseMode(1), "Failed to set size parse mode to True using ""1"". Expected True, got False.")
        Assert.IsTrue(SetSizeParseMode(0), "Failed to set size parse mode to False using ""0"". Expected True, got False.")
    End Sub

End Class