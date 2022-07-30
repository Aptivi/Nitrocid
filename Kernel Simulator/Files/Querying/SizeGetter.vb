
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

Namespace Files.Querying
    Public Module SizeGetter

        ''' <summary>
        ''' Gets all file sizes in a folder, depending on the kernel setting <see cref="FullParseMode"/>
        ''' </summary>
        ''' <param name="DirectoryInfo">Directory information</param>
        ''' <returns>Directory Size</returns>
        Public Function GetAllSizesInFolder(DirectoryInfo As DirectoryInfo) As Long
            Return GetAllSizesInFolder(DirectoryInfo, FullParseMode)
        End Function

        ''' <summary>
        ''' Gets all file sizes in a folder, and optionally parses the entire folder
        ''' </summary>
        ''' <param name="DirectoryInfo">Directory information</param>
        ''' <returns>Directory Size</returns>
        Public Function GetAllSizesInFolder(DirectoryInfo As DirectoryInfo, FullParseMode As Boolean) As Long
            Dim Files As List(Of FileInfo)
            If FullParseMode Then
                Files = DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).ToList
            Else
                Files = DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).ToList
            End If
            Wdbg(DebugLevel.I, "{0} files to be parsed", Files.Count)
            Dim TotalSize As Long = 0 'In bytes
            For Each DFile As FileInfo In Files
                If (DFile.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not DFile.Attributes.HasFlag(FileAttributes.Hidden) Then
                    If (IsOnWindows() And (Not DFile.Name.StartsWith(".") Or (DFile.Name.StartsWith(".") And HiddenFiles))) Or IsOnUnix() Then
                        Wdbg(DebugLevel.I, "File {0}, Size {1} bytes", DFile.Name, DFile.Length)
                        TotalSize += DFile.Length
                    End If
                End If
            Next
            Return TotalSize
        End Function

    End Module
End Namespace
