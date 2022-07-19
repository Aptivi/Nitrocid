
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

Imports KS.Files.Folders

<TestFixture> Public Class FilesystemNeutralizationTests

    ''' <summary>
    ''' Tests path neutralization on a folder in home directory
    ''' </summary>
    <Test, Description("Neutralization")> Public Sub TestNeutralizePaths()
        CurrentDir = HomePath
        Dim TestPath As String = "Documents"
        Dim ExpectedPath As String = HomePath + "/" + TestPath
        Dim NeutPath As String = NeutralizePath(TestPath)
        NeutPath.ShouldBe(ExpectedPath)
    End Sub

    ''' <summary>
    ''' Tests path neutralization on a folder in a custom directory
    ''' </summary>
    <Test, Description("Neutralization")> Public Sub TestNeutralizePathsCustom()
        Dim TestPath As String = "sources.list"
        Dim TargetPath As String = "/etc/apt"
        Dim NeutPath As String = NeutralizePath(TestPath, TargetPath)
        NeutPath.ShouldBe(TargetPath + "/" + TestPath)
    End Sub

End Class
