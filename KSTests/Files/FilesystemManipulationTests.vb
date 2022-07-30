
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports System.Text.RegularExpressions
Imports KS.Files.Folders
Imports KS.Files.Operations
Imports KS.Files.Querying
Imports KS.Files.PathLookup
Imports KS.Files.Read
Imports KS.Misc.Platform
Imports KS.Shell

<TestFixture> Public Class FilesystemManipulationTests

    ''' <summary>
    ''' Tests copying directory to directory
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestCopyDirectoryToDirectory()
        CurrentDir = HomePath
        Directory.CreateDirectory(HomePath + "/TestDir")
        Dim SourcePath As String = "/TestDir"
        Dim TargetPath As String = "/TestDir2"
        TryCopyFileOrDir(SourcePath, TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests copying file to directory
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestCopyFileToDirectory()
        CurrentDir = HomePath
        Dim SourcePath As String = Path.GetFullPath("TestData/TestText.txt")
        Dim TargetPath As String = "/Documents"
        TryCopyFileOrDir(SourcePath, TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests copying file to file
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestCopyFileToFile()
        CurrentDir = HomePath
        Dim SourcePath As String = Path.GetFullPath("TestData/TestText.txt")
        Dim TargetPath As String = "/Documents/Text.txt"
        TryCopyFileOrDir(SourcePath, TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests making directory
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestMakeDirectory()
        CurrentDir = HomePath
        TryMakeDirectory("/NewDirectory").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests making file
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestMakeFile()
        CurrentDir = HomePath
        TryMakeFile("/NewFile.txt").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests making file
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestMakeJsonFile()
        CurrentDir = HomePath
        TryMakeJsonFile("/NewFile.json").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests moving directory to directory
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestMoveDirectoryToDirectory()
        CurrentDir = HomePath
        Directory.CreateDirectory(HomePath + "/TestMovedDir")
        Dim SourcePath As String = "/TestMovedDir"
        Dim TargetPath As String = "/TestMovedDir2"
        TryMoveFileOrDir(SourcePath, TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests moving file to directory
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestMoveFileToDirectory()
        CurrentDir = HomePath
        Dim SourcePath As String = Path.GetFullPath("TestData/TestMove.txt")
        Dim TargetPath As String = "/Documents"
        TryMoveFileOrDir(SourcePath, TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests moving file to file
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestMoveFileToFile()
        CurrentDir = HomePath
        Dim SourcePath As String = "/Documents/TestMove.txt"
        Dim TargetPath As String = Path.GetFullPath("TestData/TestMove.txt")
        TryMoveFileOrDir(SourcePath, TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests attribute removal implementation
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestRemoveAttribute()
        Dim ExpectedAttributes As FileAttributes = FileAttributes.Encrypted Or FileAttributes.Directory
        Dim InitialAttributes As FileAttributes = FileAttributes.Encrypted Or FileAttributes.Directory Or FileAttributes.Hidden
        InitialAttributes = InitialAttributes.RemoveAttribute(FileAttributes.Hidden)
        InitialAttributes.ShouldBe(ExpectedAttributes)
    End Sub

    ''' <summary>
    ''' Tests removing directory
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestRemoveDirectory()
        CurrentDir = HomePath
        Dim TargetPath As String = "/TestDir2"
        TryRemoveDirectory(TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests removing file
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestRemoveFile()
        CurrentDir = HomePath
        Dim TargetPath As String = "/Documents/Text.txt"
        TryRemoveFile(TargetPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests searching file for string
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestSearchFileForString()
        CurrentDir = HomePath
        Dim TargetPath As String = Path.GetFullPath("TestData/TestText.txt")
        Dim Matches As List(Of String) = SearchFileForString(TargetPath, "test")
        Matches.ShouldNotBeNull
        Matches.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests searching file for string using regular expressions
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestSearchFileForStringRegexp()
        CurrentDir = HomePath
        Dim TargetPath As String = Path.GetFullPath("TestData/TestText.txt")
        Dim Matches As List(Of String) = SearchFileForStringRegexp(TargetPath, New Regex("test"))
        Matches.ShouldNotBeNull
        Matches.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests adding attribute
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestAddAttribute()
        CurrentDir = HomePath
        Dim SourcePath As String = Path.GetFullPath("TestData/TestText.txt")
        TryAddAttributeToFile(SourcePath, FileAttributes.Hidden).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests deleting attribute
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestDeleteAttribute()
        CurrentDir = HomePath
        Dim SourcePath As String = Path.GetFullPath("TestData/TestText.txt")
        TryRemoveAttributeFromFile(SourcePath, FileAttributes.Hidden).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests reading all lines without roadblocks
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestReadAllLinesNoBlock()
        Dim PathToTestText As String = Path.GetFullPath("TestData/TestText.txt")
        Dim LinesTestText As String() = ReadAllLinesNoBlock(PathToTestText)
        LinesTestText.ShouldBeOfType(GetType(String()))
        LinesTestText.ShouldNotBeNull
        LinesTestText.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests reading all lines
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestReadContents()
        Dim PathToTestText As String = Path.GetFullPath("TestData/TestText.txt")
        Dim LinesTestText As String() = ReadContents(PathToTestText)
        LinesTestText.ShouldBeOfType(GetType(String()))
        LinesTestText.ShouldNotBeNull
        LinesTestText.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests getting lookup path list
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestGetPathList()
        GetPathList.ShouldNotBeNull
        GetPathList.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests adding a neutralized path to lookup
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestAddToPathLookupNeutralized()
        Dim Path As String = If(IsOnWindows(), "C:\Program Files\dotnet", "/bin")
        Dim NeutralizedPath As String = NeutralizePath(Path)
        TryAddToPathLookup(NeutralizedPath).ShouldBeTrue
        PathsToLookup.ShouldContain(NeutralizedPath)
    End Sub

    ''' <summary>
    ''' Tests adding a non-neutralized path to lookup
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestAddToPathLookupNonNeutralized()
        Dim Path As String = If(IsOnWindows(), "dotnet", "bin")
        Dim NeutralizedPath As String = NeutralizePath(Path)
        TryAddToPathLookup(Path).ShouldBeTrue
        PathsToLookup.ShouldContain(NeutralizedPath)
    End Sub

    ''' <summary>
    ''' Tests adding a neutralized path to lookup with the root path specified
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestAddToPathLookupNeutralizedWithRootPath()
        Dim Path As String = If(IsOnWindows(), "C:\Program Files\dotnet", "/bin")
        Dim RootPath As String = If(IsOnWindows(), "C:\Program Files", "/")
        Dim NeutralizedPath As String = NeutralizePath(Path, RootPath)
        TryAddToPathLookup(NeutralizedPath, RootPath).ShouldBeTrue
        PathsToLookup.ShouldContain(NeutralizedPath)
    End Sub

    ''' <summary>
    ''' Tests adding a non-neutralized path to lookup with the root path specified
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestAddToPathLookupNonNeutralizedWithRootPath()
        Dim Path As String = If(IsOnWindows(), "dotnet", "bin")
        Dim RootPath As String = If(IsOnWindows(), "C:\Program Files", "/")
        Dim NeutralizedPath As String = NeutralizePath(Path, RootPath)
        TryAddToPathLookup(Path, RootPath).ShouldBeTrue
        PathsToLookup.ShouldContain(NeutralizedPath)
    End Sub

    ''' <summary>
    ''' Tests removing a neutralized path to lookup
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestRemoveFromPathLookupNeutralized()
        Dim Path As String = If(IsOnWindows(), "C:\Program Files\dotnet", "/bin")
        Dim NeutralizedPath As String = NeutralizePath(Path)
        TryRemoveFromPathLookup(NeutralizedPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests removing a non-neutralized path to lookup
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestRemoveFromPathLookupNonNeutralized()
        Dim Path As String = If(IsOnWindows(), "dotnet", "bin")
        Dim NeutralizedPath As String = NeutralizePath(Path)
        TryRemoveFromPathLookup(Path).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests removing a neutralized path to lookup with the root path specified
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestRemoveFromPathLookupNeutralizedWithRootPath()
        Dim Path As String = If(IsOnWindows(), "C:\Program Files\dotnet", "/bin")
        Dim RootPath As String = If(IsOnWindows(), "C:\Program Files", "/")
        Dim NeutralizedPath As String = NeutralizePath(Path, RootPath)
        TryRemoveFromPathLookup(NeutralizedPath, RootPath).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests removing a non-neutralized path to lookup with the root path specified
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestRemoveFromPathLookupNonNeutralizedWithRootPath()
        Dim Path As String = If(IsOnWindows(), "dotnet", "bin")
        Dim RootPath As String = If(IsOnWindows(), "C:\Program Files", "/")
        Dim NeutralizedPath As String = NeutralizePath(Path, RootPath)
        TryRemoveFromPathLookup(Path, RootPath).ShouldBeTrue
        PathsToLookup.ShouldNotContain(NeutralizedPath)
    End Sub

    ''' <summary>
    ''' Tests checking to see if the file exists in any of the lookup paths
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestFileExistsInPath()
        Dim Path As String = If(IsOnWindows(), "netstat.exe", "bash")
        Dim NeutralizedPath As String = ""
        FileExistsInPath(Path, NeutralizedPath).ShouldBeTrue
        NeutralizedPath.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests creating filesystem entries list
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestCreateList()
        Dim CreatedList As List(Of FileSystemInfo) = CreateList(HomePath)
        CreatedList.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests combining files
    ''' </summary>
    <Test, Description("Manipulation")> Public Sub TestCombineFiles()
        Dim PathToTestText As String = Path.GetFullPath("TestData/TestText.txt")
        Dim PathToTestTextToBeCombined As String = Path.GetFullPath("TestData/TestText.txt")
        Dim Combined As String() = CombineFiles(PathToTestText, {PathToTestTextToBeCombined})
        Combined.ShouldBeOfType(GetType(String()))
        Combined.ShouldNotBeNull
        Combined.ShouldNotBeEmpty
        Combined.Length.ShouldBeGreaterThan(1)
    End Sub

End Class