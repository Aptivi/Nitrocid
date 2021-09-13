
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

<TestClass()> Public Class FilesystemQueryingTests

    ''' <summary>
    ''' Tests getting the kernel path for each entry
    ''' </summary>
    <TestMethod()> <TestCategory("Querying")> Public Sub TestGetKernelPaths()
        For Each PathType As KernelPathType In [Enum].GetValues(GetType(KernelPathType))
            Debug.WriteLine($"Path type: {PathType}")
            Dim TargetKernelPath As String = GetKernelPath(PathType)
            Debug.WriteLine($"Got path: {TargetKernelPath}")
            TargetKernelPath.ShouldNotBeNullOrEmpty
        Next
    End Sub

    ''' <summary>
    ''' Tests getting the "other" path for each entry
    ''' </summary>
    <TestMethod()> <TestCategory("Querying")> Public Sub TestGetOtherPaths()
        For Each PathType As OtherPathType In [Enum].GetValues(GetType(OtherPathType))
            Debug.WriteLine($"Path type: {PathType}")
            Dim TargetOtherPath As String = GetOtherPath(PathType)
            Debug.WriteLine($"Got path: {TargetOtherPath}")
            TargetOtherPath.ShouldNotBeNullOrEmpty
        Next
    End Sub

    ''' <summary>
    ''' Tests trying to parse the path name
    ''' </summary>
    <TestMethod()> <TestCategory("Querying")> Public Sub TestTryParsePath()
        If IsOnWindows() Then
            TryParsePath("C:\Windows").ShouldBeTrue
            TryParsePath("C:\Windows<>").ShouldBeFalse
        Else
            TryParsePath("/usr/bin").ShouldBeTrue
            TryParsePath("/usr/bin<>").ShouldBeFalse
        End If
    End Sub

    ''' <summary>
    ''' Tests trying to parse the file name
    ''' </summary>
    <TestMethod()> <TestCategory("Querying")> Public Sub TestTryParseFileName()
        TryParseFileName("Windows").ShouldBeTrue
        TryParseFileName("Windows/System32\").ShouldBeFalse
    End Sub

End Class