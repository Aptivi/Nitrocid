
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

<TestClass()> Public Class FilesystemTests

    ''' <summary>
    ''' Gets home directory depending on platform
    ''' </summary>
    ''' <returns>Home directory</returns>
    Public Function GetHome()
        If Environment.OSVersion.ToString.Contains("Unix") Then
            Return Environ("HOME").Replace("\", "/")
        Else
            Return Environ("USERPROFILE").Replace("\", "/")
        End If
    End Function

    ''' <summary>
    ''' Tests path neutralization on a folder in home directory
    ''' </summary>
    <TestMethod()> Public Sub TestNeutralizePaths()
        Try
            CurrDir = GetHome()
            Dim TestPath As String = "Documents"
            Dim ExpectedPath As String = CurrDir + "/" + TestPath
            Dim NeutPath As String = NeutralizePath(TestPath)
            Assert.AreEqual(ExpectedPath, NeutPath)
        Catch afex As AssertFailedException
            Assert.Fail("Path is not properly neutralized.")
        End Try
    End Sub

End Class