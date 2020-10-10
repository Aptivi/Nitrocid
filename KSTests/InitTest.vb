
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

<TestClass()> Public Class InitTest

    ''' <summary>
    ''' Initialize everything that is required before starting unit tests
    ''' </summary>
    ''' <param name="Context">Test context</param>
    <AssemblyInitialize()> Public Shared Sub ReadyEverything(Context As TestContext)
        InitPaths()
        CreateConfig(False)
    End Sub

    ''' <summary>
    ''' Clean up everything that the unit tests made
    ''' </summary>
    <AssemblyCleanup()> Public Shared Sub CleanEverything()
        InitPaths()
        If File.Exists(paths("Home") + "/Documents/TestText.txt") Then File.Delete(paths("Home") + "/Documents/TestText.txt")
        If File.Exists(paths("Home") + "/Documents/Text.txt") Then File.Delete(paths("Home") + "/Documents/Text.txt")
        If File.Exists(paths("Home") + "/NewFile.txt") Then File.Delete(paths("Home") + "/NewFile.txt")
        If File.Exists(paths("Home") + "/1mb-test.csv") Then File.Delete(paths("Home") + "/1mb-test.csv")
        If Directory.Exists(paths("Home") + "/TestMovedDir2") Then Directory.Delete(paths("Home") + "/TestMovedDir2", True)
        If Directory.Exists(paths("Home") + "/NewDirectory") Then Directory.Delete(paths("Home") + "/NewDirectory", True)
        If Directory.Exists(paths("Home") + "/TestDir") Then Directory.Delete(paths("Home") + "/TestDir", True)
        If Directory.Exists(paths("Home") + "/TestDir2") Then Directory.Delete(paths("Home") + "/TestDir2", True)
        CreateConfig(False)
    End Sub

End Class