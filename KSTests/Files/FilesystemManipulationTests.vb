
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

<TestClass()> Public Class FilesystemManipulationTests

    ''' <summary>
    ''' Tests copying directory to directory
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestCopyDirectoryToDirectory()
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
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestCopyFileToDirectory()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = IO.Path.GetFullPath("TestText.txt")
        Dim TargetPath As String = "/Documents"
        Assert.IsTrue(CopyFileOrDir(SourcePath, TargetPath), "Failed to copy file ""{0}"" to directory ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests copying file to file
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestCopyFileToFile()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = IO.Path.GetFullPath("TestText.txt")
        Dim TargetPath As String = "/Documents/Text.txt"
        Assert.IsTrue(CopyFileOrDir(SourcePath, TargetPath), "Failed to copy file ""{0}"" to file ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests making directory
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestMakeDirectory()
        InitPaths()
        CurrDir = paths("Home")
        Assert.IsTrue(MakeDirectory("/NewDirectory"), "Failed to create new directory. Expected True, got False.")
    End Sub

    ''' <summary>
    ''' Tests making file
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestMakeFile()
        InitPaths()
        CurrDir = paths("Home")
        Assert.IsTrue(MakeFile("/NewFile.txt"), "Failed to create new file. Expected True, got False.")
    End Sub

    ''' <summary>
    ''' Tests moving directory to directory
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestMoveDirectoryToDirectory()
        InitPaths()
        CurrDir = paths("Home")
        IO.Directory.CreateDirectory(paths("Home") + "/TestMovedDir")
        Dim SourcePath As String = "/TestMovedDir"
        Dim TargetPath As String = "/TestMovedDir2"
        Assert.IsTrue(MoveFileOrDir(SourcePath, TargetPath), "Failed to move directory ""{0}"" to directory ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests moving file to directory
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestMoveFileToDirectory()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = IO.Path.GetFullPath("TestMove.txt")
        Dim TargetPath As String = "/Documents"
        Assert.IsTrue(MoveFileOrDir(SourcePath, TargetPath), "Failed to move file ""{0}"" to directory ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests moving file to file
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestMoveFileToFile()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = "/Documents/TestMove.txt"
        Dim TargetPath As String = IO.Path.GetFullPath("TestMove.txt")
        Assert.IsTrue(MoveFileOrDir(SourcePath, TargetPath), "Failed to move file ""{0}"" to file ""{1}"". Expected True, got False.", SourcePath, TargetPath)
    End Sub

    ''' <summary>
    ''' Tests removing directory
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestRemoveDirectory()
        InitPaths()
        CurrDir = paths("Home")
        Dim TargetPath As String = "/TestDir2"
        Assert.IsTrue(RemoveDirectory(TargetPath), "Failed to remove directory ""{0}"". Expected True, got False.", TargetPath)
    End Sub

    ''' <summary>
    ''' Tests removing file
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestRemoveFile()
        InitPaths()
        CurrDir = paths("Home")
        Dim TargetPath As String = "/Documents/Text.txt"
        Assert.IsTrue(RemoveFile(TargetPath), "Failed to remove directory ""{0}"". Expected True, got False.", TargetPath)
    End Sub

    ''' <summary>
    ''' Tests searching file for string
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestSearchFileForString()
        InitPaths()
        CurrDir = paths("Home")
        Dim TargetPath As String = IO.Path.GetFullPath("TestText.txt")
        Dim Matches As List(Of String) = SearchFileForString(TargetPath, "test")
        Assert.IsNotNull(Matches, "Failed to search file for matches.")
        Assert.IsTrue(Matches.Count = 1, "Expected one instance of ""test"", got {0}", Matches.Count)
    End Sub

    ''' <summary>
    ''' Tests adding attribute
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestAddAttribute()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = IO.Path.GetFullPath("TestText.txt")
        Assert.IsTrue(AddAttributeToFile(SourcePath, IO.FileAttributes.Hidden), "Failed to add attrbute ""Hidden"" to file. Got {0}", IO.File.GetAttributes(SourcePath))
    End Sub

    ''' <summary>
    ''' Tests deleting attribute
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestDeleteAttribute()
        InitPaths()
        CurrDir = paths("Home")
        Dim SourcePath As String = IO.Path.GetFullPath("TestText.txt")
        Assert.IsTrue(RemoveAttributeFromFile(SourcePath, IO.FileAttributes.Hidden), "Failed to remove attrbute ""Hidden"" to file. Got {0}", IO.File.GetAttributes(SourcePath))
    End Sub

    ''' <summary>
    ''' Tests reading all lines without roadblocks
    ''' </summary>
    <TestMethod()> <TestCategory("Manipulation")> Public Sub TestReadAllLinesNoBlock()
        Dim PathToTestText As String = Path.GetFullPath("TestText.txt")
        Dim LinesTestText As String() = ReadAllLinesNoBlock(PathToTestText)
        Assert.IsInstanceOfType(LinesTestText, GetType(String()), "Reading all lines failed.")
    End Sub

End Class