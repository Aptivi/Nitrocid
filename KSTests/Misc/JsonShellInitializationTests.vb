
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
Imports KS.Misc.JsonShell

<TestFixture> Public Class JsonShellInitializationTests

    ''' <summary>
    ''' Tests opening, saving, and closing a JSON file
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestOpenSaveCloseJsonFile()
        Dim PathToTestJson As String = Path.GetFullPath("TestData/TestJson.json")
        JsonShell_OpenJsonFile(PathToTestJson).ShouldBeTrue
        JsonShell_AddNewProperty("$", "HowText", "How are you today?")
        JsonShell_FileToken("HowText").ShouldNotBeNull
        JsonShell_GetProperty("HelloText").ShouldNotBeNull
        JsonShell_SerializeToString("HelloText").ShouldNotBeNullOrEmpty
        JsonShell_RemoveProperty("HowText")
        JsonShell_SaveFile(False).ShouldBeTrue
        JsonShell_CloseTextFile.ShouldBeTrue
    End Sub

End Class