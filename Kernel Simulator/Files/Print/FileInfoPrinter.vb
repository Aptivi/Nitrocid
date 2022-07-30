
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

Imports KS.Files.Folders
Imports KS.Files.Querying
Imports System.IO

Namespace Files.Print
    Public Module FileInfoPrinter

        ''' <summary>
        ''' Prints the file information to the console
        ''' </summary>
        Public Sub PrintFileInfo(FileInfo As FileSystemInfo)
            PrintFileInfo(FileInfo, ShowFileDetailsList)
        End Sub

        ''' <summary>
        ''' Prints the file information to the console
        ''' </summary>
        Public Sub PrintFileInfo(FileInfo As FileSystemInfo, ShowFileDetails As Boolean)
            If FileExists(FileInfo.FullName) Then
                If (FileInfo.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not FileInfo.Attributes.HasFlag(FileAttributes.Hidden) Then
                    If (IsOnWindows() And (Not FileInfo.Name.StartsWith(".") Or (FileInfo.Name.StartsWith(".") And HiddenFiles))) Or IsOnUnix() Then
                        If FileInfo.Name.EndsWith(".uesh") Then
                            Write("- " + FileInfo.Name, False, ColTypes.Stage)
                            If ShowFileDetails Then Write(": ", False, ColTypes.Stage)
                        Else
                            Write("- " + FileInfo.Name, False, ColTypes.ListEntry)
                            If ShowFileDetails Then Write(": ", False, ColTypes.ListEntry)
                        End If
                        If ShowFileDetails Then
                            Write(DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), False, ColTypes.ListValue,
                                        DirectCast(FileInfo, FileInfo).Length.FileSizeToString, FileInfo.CreationTime.ToShortDateString, FileInfo.CreationTime.ToShortTimeString,
                                                                                                FileInfo.LastWriteTime.ToShortDateString, FileInfo.LastWriteTime.ToShortTimeString)
                        End If
                        Console.WriteLine()
                    End If
                End If
            Else
                Write(DoTranslation("File {0} not found"), True, ColTypes.Error, FileInfo.FullName)
                Wdbg(DebugLevel.I, "IO.FileExists = {0}", FileExists(FileInfo.FullName))
            End If
        End Sub

    End Module
End Namespace