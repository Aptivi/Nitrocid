
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
Imports KS.Files.Querying
Imports System.IO

Namespace Files.Print
    Public Module DirectoryInfoPrinter

        ''' <summary>
        ''' Prints the directory information to the console
        ''' </summary>
        Public Sub PrintDirectoryInfo(DirectoryInfo As FileSystemInfo)
            PrintDirectoryInfo(DirectoryInfo, ShowFileDetailsList)
        End Sub

        ''' <summary>
        ''' Prints the directory information to the console
        ''' </summary>
        Public Sub PrintDirectoryInfo(DirectoryInfo As FileSystemInfo, ShowDirectoryDetails As Boolean)
            If FolderExists(DirectoryInfo.FullName) Then
                'Get all file sizes in a folder
                Dim TotalSize As Long = GetAllSizesInFolder(DirectCast(DirectoryInfo, DirectoryInfo))

                'Print information
                If (DirectoryInfo.Attributes = FileAttributes.Hidden And HiddenFiles) Or Not DirectoryInfo.Attributes.HasFlag(FileAttributes.Hidden) Then
                    If (IsOnWindows() And (Not DirectoryInfo.Name.StartsWith(".") Or (DirectoryInfo.Name.StartsWith(".") And HiddenFiles))) Or IsOnUnix() Then
                        Write("- " + DirectoryInfo.Name + "/", False, ColTypes.ListEntry)
                        If ShowDirectoryDetails Then
                            Write(": ", False, ColTypes.ListEntry)
                            Write(DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), False, ColTypes.ListValue,
                          TotalSize.FileSizeToString, DirectoryInfo.CreationTime.ToShortDateString, DirectoryInfo.CreationTime.ToShortTimeString,
                                                      DirectoryInfo.LastWriteTime.ToShortDateString, DirectoryInfo.LastWriteTime.ToShortTimeString)
                        End If
                        Console.WriteLine()
                    End If
                End If
            Else
                Write(DoTranslation("Directory {0} not found"), True, ColTypes.Error, DirectoryInfo.FullName)
                Wdbg(DebugLevel.I, "IO.FolderExists = {0}", FolderExists(DirectoryInfo.FullName))
            End If
        End Sub

    End Module
End Namespace