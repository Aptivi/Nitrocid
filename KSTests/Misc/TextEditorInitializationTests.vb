﻿
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

Imports System.IO
Imports KS.Misc.TextEdit

<TestClass()> Public Class TextEditorInitializationTests

    ''' <summary>
    ''' Tests opening, saving, and closing text file
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestOpenSaveCloseTextFile()
        Dim PathToTestText As String = Path.GetFullPath("TestText.txt")
        TextEdit_OpenTextFile(PathToTestText).ShouldBeTrue
        TextEdit_FileLines.Add("Hello!")
        TextEdit_SaveTextFile(False).ShouldBeTrue
        TextEdit_FileLines.ShouldContain("Hello!")
        TextEdit_CloseTextFile().ShouldBeTrue
    End Sub

End Class