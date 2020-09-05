
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
    ''' Gets home directory depending on platform (not a test method)
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
        CurrDir = GetHome()
        Dim TestPath As String = "Documents"
        Dim ExpectedPath As String = CurrDir + "/" + TestPath
        Dim NeutPath As String = NeutralizePath(TestPath)
        Assert.AreEqual(ExpectedPath, NeutPath, "Path is not properly neutralized. Expected {0}, got {1}", ExpectedPath, NeutPath)
    End Sub

    ''' <summary>
    ''' Tests current directory setting
    ''' </summary>
    <TestMethod()> Public Sub TestSetCurrDir()
        InitPaths()
        CurrDir = paths("Home")
        Dim Path As String = paths("Home") + "/Documents"
        Assert.IsTrue(SetCurrDir(Path), "Failed to set current path. Expected True, got False.")
        Assert.AreEqual(Path, CurrDir, "Current path is not properly set. Expected {0}, got {1}", Path, CurrDir)
    End Sub

    ''' <summary>
    ''' Tests copying directory to directory
    ''' </summary>
    <TestMethod()> Public Sub TestCopyDirectoryToDirectory()
        InitPaths()
        CurrDir = paths("Home")
        IO.Directory.CreateDirectory(paths("Home") + "/TestDir")
        Dim SourcePath As String = "/TestDir"
        Dim TargetPath As String = "/TestDir2"
        Assert.IsTrue(CopyFileOrDir(SourcePath, TargetPath), "Failed to copy directory ""{0}"" to directory ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests copying file to directory
    ''' </summary>
    <TestMethod()> Public Sub TestCopyFileToDirectory()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = IO.Path.GetFullPath("TestText.txt")
        Dim TargetPath As String = "/Documents"
        Assert.IsTrue(CopyFileOrDir(SourcePath, TargetPath), "Failed to copy file ""{0}"" to directory ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests copying file to file
    ''' </summary>
    <TestMethod()> Public Sub TestCopyFileToFile()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = IO.Path.GetFullPath("TestText.txt")
        Dim TargetPath As String = "/Documents/Text.txt"
        Assert.IsTrue(CopyFileOrDir(SourcePath, TargetPath), "Failed to copy file ""{0}"" to file ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests copying file to file
    ''' </summary>
    <TestMethod()> Public Sub TestSetSizeParseMode()
        InitPaths()
        Assert.IsTrue(SetSizeParseMode(True), "Failed to set size parse mode to True. Expected True, got False.")
        Assert.IsTrue(SetSizeParseMode(False), "Failed to set size parse mode to False. Expected True, got False.")
        Assert.IsTrue(SetSizeParseMode(1), "Failed to set size parse mode to True using ""1"". Expected True, got False.")
        Assert.IsTrue(SetSizeParseMode(0), "Failed to set size parse mode to False using ""0"". Expected True, got False.")
    End Sub

    ''' <summary>
    ''' Tests copying file to file
    ''' </summary>
    <TestMethod()> Public Sub TestMakeDirectory()
        InitPaths()
        CurrDir = paths("Home")
        Assert.IsTrue(MakeDirectory("/NewDirectory"), "Failed to set size parse mode to True. Expected True, got False.")
    End Sub

End Class