
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

<TestFixture> Public Class FilesystemQueryingTests

    ''' <summary>
    ''' Tests checking if file exists
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestFileExists()
        Dim TargetFile As String = Path.GetFullPath("TestData/TestText.txt")
        Dim TargetFile2 As String = Path.GetFullPath("TestData/TestTexts.txt")
        FileExists(TargetFile).ShouldBeTrue
        FileExists(TargetFile2).ShouldBeFalse
    End Sub

    ''' <summary>
    ''' Tests checking if directory exists
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestDirectoryExists()
        Dim TargetDirectory As String = Path.GetFullPath("EmptyFiles")
        Dim TargetDirectory2 As String = Path.GetFullPath("EmptyFile")
        FolderExists(TargetDirectory).ShouldBeTrue
        FolderExists(TargetDirectory2).ShouldBeFalse
    End Sub

    ''' <summary>
    ''' Tests getting the kernel path for each entry
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestGetKernelPaths()
        For Each PathType As KernelPathType In [Enum].GetValues(GetType(KernelPathType))
            Debug.WriteLine($"Path type: {PathType}")
            Dim TargetKernelPath As String = GetKernelPath(PathType)
            Debug.WriteLine($"Got path: {TargetKernelPath}")
            TargetKernelPath.ShouldNotBeNullOrEmpty
        Next
    End Sub

    ''' <summary>
    ''' Tests trying to parse the path name
    ''' </summary>
    <TestCase("C:\Windows", IncludePlatform:="win", ExpectedResult:=True),
     TestCase("C:\Windows<>", IncludePlatform:="win", ExpectedResult:=False),
     TestCase("/usr/bin", IncludePlatform:="linux,unix,macosx", ExpectedResult:=True),
     TestCase("/usr/bin<>", IncludePlatform:="linux,unix,macosx", ExpectedResult:=False),
     Description("Querying")>
    Public Function TestTryParsePath(Path As String) As Boolean
        Return TryParsePath(Path)
    End Function

    ''' <summary>
    ''' Tests trying to parse the file name
    ''' </summary>
    <TestCase("Windows", ExpectedResult:=True),
     TestCase("Windows/System32\", ExpectedResult:=False),
     Description("Querying")>
    Public Function TestTryParseFileName(Path As String) As Boolean
        Return TryParseFileName(Path)
    End Function

    ''' <summary>
    ''' Tests trying to get the line ending from text file
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestGetLineEndingFromFile()
        Dim ExpectedStyle As FilesystemNewlineStyle = FilesystemNewlineStyle.LF
        Dim ActualStyle As FilesystemNewlineStyle = GetLineEndingFromFile(Path.GetFullPath("TestData/TestText.txt"))
        ActualStyle.ShouldBe(ExpectedStyle)
    End Sub

End Class