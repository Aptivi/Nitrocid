
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

Imports KS.Files.Print
Imports KS.Files.Querying
Imports System.IO

Namespace Files.Folders
    Public Module Listing

        Public SortList As Boolean = True
        Public SortMode As FilesystemSortOptions = FilesystemSortOptions.FullName
        Public SortDirection As FilesystemSortDirection = FilesystemSortDirection.Ascending
        Public ShowFileDetailsList As Boolean = True
        Public ShowTotalSizeInList As Boolean

        ''' <summary>
        ''' Creates a list of files and directories
        ''' </summary>
        ''' <param name="folder">Full path to folder</param>
        ''' <param name="Sorted">Whether the list is sorted or not</param>
        ''' <returns>List of filesystem entries if any. Empty list if folder is not found or is empty.</returns>
        ''' <exception cref="Exceptions.FilesystemException"></exception>
        Public Function CreateList(folder As String, Optional Sorted As Boolean = False) As List(Of FileSystemInfo)
            ThrowOnInvalidPath(folder)
            Wdbg(DebugLevel.I, "Folder {0} will be listed...", folder)
            Dim FilesystemEntries As New List(Of FileSystemInfo)

            'List files and folders
            folder = NeutralizePath(folder)
            If FolderExists(folder) Or folder.ContainsAnyOf({"?", "*"}) Then
                Dim enumeration As IEnumerable(Of String)
                Try
                    enumeration = GetFilesystemEntries(folder)
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to make a list of filesystem entries for directory {0}: {1}", folder, ex.Message)
                    WStkTrc(ex)
                    Throw New Exceptions.FilesystemException(DoTranslation("Failed to make a list of filesystem entries for directory") + " {0}", ex, folder)
                End Try
                For Each Entry As String In enumeration
                    Wdbg(DebugLevel.I, "Enumerating {0}...", Entry)
                    Try
                        If FileExists(Entry) Then
                            Wdbg(DebugLevel.I, "Entry is a file. Adding {0} to list...", Entry)
                            FilesystemEntries.Add(New FileInfo(Entry))
                        ElseIf FolderExists(Entry) Then
                            Wdbg(DebugLevel.I, "Entry is a folder. Adding {0} to list...", Entry)
                            FilesystemEntries.Add(New DirectoryInfo(Entry))
                        End If
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Failed to enumerate {0} for directory {1}: {2}", Entry, folder, ex.Message)
                        WStkTrc(ex)
                    End Try
                Next
            End If

            'Return the resulting list immediately if not sorted. Otherwise, sort it.
            If Sorted And Not FilesystemEntries.Count = 0 Then
                'We define the max string length for the largest size. This is to overcome the limitation of sorting when it comes to numbers.
                Dim MaxLength As Integer = FilesystemEntries.Max(Function(x) If(TryCast(x, FileInfo) IsNot Nothing, TryCast(x, FileInfo).Length, 0).ToString.Length)

                'Select whether or not to sort descending.
                Select Case SortDirection
                    Case FilesystemSortDirection.Ascending
                        FilesystemEntries = FilesystemEntries.OrderBy(Function(x) SortSelector(x, MaxLength), StringComparer.OrdinalIgnoreCase).ToList
                    Case FilesystemSortDirection.Descending
                        FilesystemEntries = FilesystemEntries.OrderByDescending(Function(x) SortSelector(x, MaxLength), StringComparer.OrdinalIgnoreCase).ToList
                End Select
            End If
            Return FilesystemEntries
        End Function

        ''' <summary>
        ''' Helper for sorting filesystem entries
        ''' </summary>
        ''' <param name="FileSystemEntry">File system entry</param>
        ''' <param name="MaxLength">For size, how many zeroes to pad the size string to the left?</param>
        Private Function SortSelector(FileSystemEntry As FileSystemInfo, MaxLength As Integer) As String
            Select Case SortMode
                Case FilesystemSortOptions.FullName
                    Return FileSystemEntry.FullName
                Case FilesystemSortOptions.Length
                    Return If(TryCast(FileSystemEntry, FileInfo) IsNot Nothing, TryCast(FileSystemEntry, FileInfo).Length, 0).ToString.PadLeft(MaxLength, "0")
                Case FilesystemSortOptions.CreationTime
                    Return FileSystemEntry.CreationTime
                Case FilesystemSortOptions.LastAccessTime
                    Return FileSystemEntry.LastAccessTime
                Case FilesystemSortOptions.LastWriteTime
                    Return FileSystemEntry.LastWriteTime
            End Select
        End Function

        ''' <summary>
        ''' List all files and folders in a specified folder
        ''' </summary>
        ''' <param name="folder">Full path to folder</param>
        Public Sub List(folder As String)
            List(folder, ShowFileDetailsList, SuppressUnauthorizedMessages, SortList)
        End Sub

        ''' <summary>
        ''' List all files and folders in a specified folder
        ''' </summary>
        ''' <param name="folder">Full path to folder</param>
        Public Sub List(folder As String, Sort As Boolean)
            List(folder, ShowFileDetailsList, SuppressUnauthorizedMessages, Sort)
        End Sub

        ''' <summary>
        ''' List all files and folders in a specified folder
        ''' </summary>
        ''' <param name="folder">Full path to folder</param>
        Public Sub List(folder As String, ShowFileDetails As Boolean, SuppressUnauthorizedMessage As Boolean)
            List(folder, ShowFileDetails, SuppressUnauthorizedMessage, SortList)
        End Sub

        ''' <summary>
        ''' List all files and folders in a specified folder
        ''' </summary>
        ''' <param name="folder">Full path to folder</param>
        Public Sub List(folder As String, ShowFileDetails As Boolean, SuppressUnauthorizedMessage As Boolean, Sort As Boolean)
            ThrowOnInvalidPath(folder)
            Wdbg(DebugLevel.I, "Folder {0} will be listed...", folder)

            'List files and folders
            folder = NeutralizePath(folder)
            If FolderExists(folder) Or folder.ContainsAnyOf({"?", "*"}) Then
                Dim enumeration As List(Of FileSystemInfo)
                WriteSeparator(folder, True)

                'Try to create a list
                Try
                    enumeration = CreateList(folder, Sort)
                    If enumeration.Count = 0 Then Write(DoTranslation("Folder is empty."), True, ColTypes.Warning)

                    'Enumerate each entry
                    Dim TotalSize As Long = 0
                    For Each Entry As FileSystemInfo In enumeration
                        Wdbg(DebugLevel.I, "Enumerating {0}...", Entry.FullName)
                        Try
                            If FileExists(Entry.FullName) Then
                                TotalSize += CType(Entry, FileInfo).Length
                                PrintFileInfo(Entry)
                            ElseIf FolderExists(Entry.FullName) Then
                                PrintDirectoryInfo(Entry)
                            End If
                        Catch ex As UnauthorizedAccessException
                            If Not SuppressUnauthorizedMessage Then Write("- " + DoTranslation("You are not authorized to get info for {0}."), True, ColTypes.Error, Entry.Name)
                            WStkTrc(ex)
                        End Try
                    Next

                    'Show total size in list optionally
                    If ShowTotalSizeInList Then Write(NewLine + DoTranslation("Total size in folder:") + " {0}", True, ColTypes.Neutral, TotalSize.FileSizeToString)
                Catch ex As Exception
                    Write(DoTranslation("Unknown error while listing in directory: {0}"), True, ColTypes.Error, ex.Message)
                    WStkTrc(ex)
                End Try
            ElseIf FileExists(folder) Then
                Try
                    PrintFileInfo(New FileInfo(folder), ShowFileDetails)
                Catch ex As UnauthorizedAccessException
                    If Not SuppressUnauthorizedMessage Then Write("- " + DoTranslation("You are not authorized to get info for {0}."), True, ColTypes.Error, folder)
                    WStkTrc(ex)
                End Try
            Else
                Write(DoTranslation("Directory {0} not found"), True, ColTypes.Error, folder)
                Wdbg(DebugLevel.I, "IO.FolderExists = {0}", FolderExists(folder))
            End If
        End Sub

        ''' <summary>
        ''' Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
        ''' </summary>
        ''' <param name="Path">The path, including the pattern</param>
        ''' <returns>The array of full paths</returns>
        Public Function GetFilesystemEntries(Path As String, Optional IsFile As Boolean = False) As String()
            Dim Entries As String() = Array.Empty(Of String)()
            Try
                ThrowOnInvalidPath(Path)

                'Select the pattern index
                Dim SelectedPatternIndex As Integer = 0
                Dim SplitPath As String() = Path.Split("/").Skip(1).ToArray
                Dim SplitParent As New List(Of String) From {Path.Split("/")(0)}
                For PatternIndex As Integer = 0 To SplitPath.Length - 1
                    If SplitPath(PatternIndex).ContainsAnyOf(IO.Path.GetInvalidFileNameChars.Select(Function(Character) Character.ToString).ToArray) Then
                        SelectedPatternIndex = PatternIndex
                        Exit For
                    End If
                    SplitParent.Add(SplitPath(PatternIndex))
                Next

                'Split the path and the pattern
                Dim Parent As String = NeutralizePath(IO.Path.GetDirectoryName(Path) + "/" + IO.Path.GetFileName(Path))
                Dim Pattern As String = If(IsFile, "", "*")
                If Parent.ContainsAnyOf(GetInvalidPathChars.Select(Function(Character) Character.ToString).ToArray) Then
                    Parent = IO.Path.GetDirectoryName(Path)
                    Pattern = IO.Path.GetFileName(Path)
                End If
                If SelectedPatternIndex <> 0 Then
                    Parent = String.Join("/", SplitParent)
                    Pattern = String.Join("/", SplitPath.Skip(SelectedPatternIndex))
                End If

                'Split the path and the pattern and return the final result
                Entries = GetFilesystemEntries(Parent, Pattern)
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to combine files: {0}", ex.Message)
            End Try
            Return Entries
        End Function

        ''' <summary>
        ''' Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
        ''' </summary>
        ''' <param name="Parent">The parent path. It can be neutralized if necessary</param>
        ''' <param name="Pattern">The pattern</param>
        ''' <returns>The array of full paths</returns>
        Public Function GetFilesystemEntries(Parent As String, Pattern As String) As String()
            Dim Entries As String() = Array.Empty(Of String)()
            Try
                ThrowOnInvalidPath(Parent)
                ThrowOnInvalidPath(Pattern)
                Parent = NeutralizePath(Parent)

                'Get the entries
                If Directory.Exists(Parent) Then
                    Entries = Directory.EnumerateFileSystemEntries(Parent, Pattern).ToArray
                    Wdbg(DebugLevel.I, "Enumerated {0} entries from parent {1} using pattern {2}", Entries.Length, Parent, Pattern)
                Else
                    Entries = {Parent}
                End If
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to combine files: {0}", ex.Message)
            End Try
            Return Entries
        End Function

    End Module
End Namespace
