
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
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq

Public Module Filesystem

    'Variables
    Public CurrDir As String = GetOtherPath(OtherPathType.Home)
    Public ShowFilesystemProgress As Boolean = True
    Public SortMode As FilesystemSortOptions = FilesystemSortOptions.FullName
    Public SortDirection As FilesystemSortDirection = FilesystemSortDirection.Ascending

    ''' <summary>
    ''' Sets the current working directory
    ''' </summary>
    ''' <param name="dir">A directory</param>
    ''' <returns>True if successful, False if unsuccessful.</returns>
    ''' <exception cref="DirectoryNotFoundException"></exception>
    Public Function SetCurrDir(dir As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(dir)
#End If
        dir = NeutralizePath(dir)
        Wdbg(DebugLevel.I, "Directory exists? {0}", Directory.Exists(dir))
        If Directory.Exists(dir) Then
            Dim Parser As New DirectoryInfo(dir)
            CurrDir = Parser.FullName.Replace("\", "/")

            'Raise event
            EventManager.RaiseCurrentDirectoryChanged()
            Return True
        Else
            Throw New DirectoryNotFoundException(DoTranslation("Directory {0} not found").FormatString(dir))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Reads the contents of a file and writes it to the console
    ''' </summary>
    ''' <param name="filename">Full path to file</param>
    Public Sub ReadContents(filename As String)
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(filename)
#End If

        'Read the contents
        filename = NeutralizePath(filename)
        Using FStream As New StreamReader(filename)
            Wdbg(DebugLevel.I, "Stream to file {0} opened.", filename)
            While Not FStream.EndOfStream
                W(FStream.ReadLine, True, ColTypes.Neutral)
            End While
        End Using
    End Sub

    ''' <summary>
    ''' List all files and folders in a specified folder
    ''' </summary>
    ''' <param name="folder">Full path to folder</param>
    Sub List(folder As String)
        Wdbg(DebugLevel.I, "Folder {0} will be listed...", folder)

#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(folder)
#End If

        'List files and folders
        folder = NeutralizePath(folder)
        If Directory.Exists(folder) Then
            Dim enumeration As New List(Of FileSystemInfo)
            WriteSeparator(folder, True)

            'Try to create a list
            Try
                enumeration = CreateList(folder, True)
                If enumeration.Count = 0 Then W(DoTranslation("Folder is empty."), True, ColTypes.Warning)
            Catch ex As Exception
                W(DoTranslation("Unknown error while listing in directory: {0}"), True, ColTypes.Error, ex.Message)
                WStkTrc(ex)
                Exit Sub
            End Try

            'Enumerate each entry
            For Each Entry As FileSystemInfo In enumeration
                Wdbg(DebugLevel.I, "Enumerating {0}...", Entry.FullName)
                Try
                    If File.Exists(Entry.FullName) Then
                        'Print information
                        If (Entry.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not Entry.Attributes.HasFlag(FileAttributes.Hidden) Then
                            If (IsOnWindows() And (Not Entry.Name.StartsWith(".") Or (Entry.Name.StartsWith(".") And HiddenFiles))) Or IsOnUnix() Then
                                If Entry.Name.EndsWith(".uesh") Then
                                    W("- " + Entry.Name + ": ", False, ColTypes.Stage)
                                Else
                                    W("- " + Entry.Name + ": ", False, ColTypes.ListEntry)
                                End If
                                W(DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), True, ColTypes.ListValue,
                                  DirectCast(Entry, FileInfo).Length.FileSizeToString, Entry.CreationTime.ToShortDateString, Entry.CreationTime.ToShortTimeString,
                                                                                       Entry.LastWriteTime.ToShortDateString, Entry.LastWriteTime.ToShortTimeString)
                            End If
                        End If
                    ElseIf Directory.Exists(Entry.FullName) Then
                        'Get all file sizes in a folder
                        Dim TotalSize As Long = GetAllSizesInFolder(DirectCast(Entry, DirectoryInfo))

                        'Print information
                        If (Entry.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not Entry.Attributes.HasFlag(FileAttributes.Hidden) Then
                            If (IsOnWindows() And (Not Entry.Name.StartsWith(".") Or (Entry.Name.StartsWith(".") And HiddenFiles))) Or IsOnUnix() Then
                                W("- " + Entry.Name + "/: ", False, ColTypes.ListEntry)
                                W(DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), True, ColTypes.ListValue,
                                  TotalSize.FileSizeToString, Entry.CreationTime.ToShortDateString, Entry.CreationTime.ToShortTimeString,
                                                              Entry.LastWriteTime.ToShortDateString, Entry.LastWriteTime.ToShortTimeString)
                            End If
                        End If
                    End If
                Catch ex As UnauthorizedAccessException 'Error while getting info
                    W("- " + DoTranslation("You are not authorized to get info for {0}."), True, ColTypes.Error, Entry.Name)
                    WStkTrc(ex)
                End Try
            Next
        Else
            W(DoTranslation("Directory {0} not found"), True, ColTypes.Error, folder)
            Wdbg(DebugLevel.I, "IO.Directory.Exists = {0}", Directory.Exists(folder))
        End If
    End Sub

    ''' <summary>
    ''' Creates a list of files and directories
    ''' </summary>
    ''' <param name="folder">Full path to folder</param>
    ''' <param name="Sorted">Whether the list is sorted or not</param>
    ''' <returns>List of filesystem entries if any. Empty list if folder is not found or is empty.</returns>
    ''' <exception cref="Exceptions.FilesystemException"></exception>
    Public Function CreateList(folder As String, Optional Sorted As Boolean = False) As List(Of FileSystemInfo)
        Wdbg(DebugLevel.I, "Folder {0} will be listed...", folder)
        Dim FilesystemEntries As New List(Of FileSystemInfo)
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(folder)
#End If

        'List files and folders
        folder = NeutralizePath(folder)
        If Directory.Exists(folder) Then
            Dim enumeration As IEnumerable(Of String)
            Try
                enumeration = Directory.EnumerateFileSystemEntries(folder)
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to make a list of filesystem entries for directory {0}: {1}", folder, ex.Message)
                WStkTrc(ex)
                Throw New Exceptions.FilesystemException(DoTranslation("Failed to make a list of filesystem entries for directory") + " {0}", ex, folder)
            End Try
            For Each Entry As String In enumeration
                Wdbg(DebugLevel.I, "Enumerating {0}...", Entry)
                Try
                    If File.Exists(Entry) Then
                        Wdbg(DebugLevel.I, "Entry is a file. Adding {0} to list...", Entry)
                        FilesystemEntries.Add(New FileInfo(Entry))
                    ElseIf Directory.Exists(Entry) Then
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
    ''' Simplifies the path to the correct one. It converts the path format to the unified format.
    ''' </summary>
    ''' <param name="Path">Target path, be it a file or a folder</param>
    ''' <returns>Absolute path</returns>
    ''' <exception cref="FileNotFoundException"></exception>
    Public Function NeutralizePath(Path As String, Optional Strict As Boolean = False) As String
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(Path)
#End If

        'Replace backslashes with slashes if any.
        Path = Path.Replace("\", "/")

        'Append current directory to path
        If (IsOnWindows() And Not Path.Contains(":/")) Or (IsOnUnix() And Not Path.StartsWith("/")) Then
            If Not CurrDir.EndsWith("/") Then
                Path = $"{CurrDir}/{Path}"
            Else
                Path = $"{CurrDir}{Path}"
            End If
        End If

        'Replace last occurrences of current directory of path with nothing.
        If Not CurrDir = "" Then
            If Path.Contains(CurrDir.Replace("\", "/")) And Path.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                Path = ReplaceLastOccurrence(Path, CurrDir, "")
            End If
        End If

        'If strict, checks for existence of file or directory
        If Strict Then
            If File.Exists(Path) Or Directory.Exists(Path) Then
                Return Path
            Else
                Throw New FileNotFoundException(DoTranslation("Neutralized a non-existent path.") + " {0}".FormatString(Path))
            End If
        Else
            Return Path
        End If
    End Function

    ''' <summary>
    ''' Simplifies the path to the correct one. It converts the path format to the unified format.
    ''' </summary>
    ''' <param name="Path">Target path, be it a file or a folder</param>
    ''' <param name="Source">Source path in which the target is found. Must be a directory</param>
    ''' <returns>Absolute path</returns>
    ''' <exception cref="FileNotFoundException"></exception>
    Public Function NeutralizePath(Path As String, Source As String, Optional Strict As Boolean = False) As String
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(Path)
        ThrowOnInvalidPath(Source)
#End If

        'Replace backslashes with slashes if any.
        Path = Path.Replace("\", "/")
        Source = Source.Replace("\", "/")

        'Append current directory to path
        If (IsOnWindows() And Not Path.Contains(":/")) Or (IsOnUnix() And Not Path.StartsWith("/")) Then
            If Not Source.EndsWith("/") Then
                Path = $"{Source}/{Path}"
            Else
                Path = $"{Source}{Path}"
            End If
        End If

        'Replace last occurrences of current directory of path with nothing.
        If Not Source = "" Then
            If Path.Contains(Source.Replace("\", "/")) And Path.AllIndexesOf(Source.Replace("\", "/")).Count > 1 Then
                Path = ReplaceLastOccurrence(Path, Source, "")
            End If
        End If

        'If strict, checks for existence of file
        If Strict Then
            If File.Exists(Path) Or Directory.Exists(Path) Then
                Return Path
            Else
                Throw New FileNotFoundException(DoTranslation("Neutralized a non-existent path.") + " {0}".FormatString(Path))
            End If
        Else
            Return Path
        End If
    End Function

    ''' <summary>
    ''' Copies a file or directory
    ''' </summary>
    ''' <param name="Source">Source file or directory</param>
    ''' <param name="Destination">Target file or directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function CopyFileOrDir(Source As String, Destination As String) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(Source)
            ThrowOnInvalidPath(Destination)
#End If
            Source = NeutralizePath(Source)
            Wdbg(DebugLevel.I, "Source directory: {0}", Source)
            Destination = NeutralizePath(Destination)
            Wdbg(DebugLevel.I, "Target directory: {0}", Destination)
            Dim FileName As String = Path.GetFileName(Source)
            Wdbg(DebugLevel.I, "Source file name: {0}", FileName)
            If Directory.Exists(Source) Then
                Wdbg(DebugLevel.I, "Source and destination are directories")
                CopyDirectory(Source, Destination)

                'Raise event
                EventManager.RaiseDirectoryCopied(Source, Destination)
                Return True
            ElseIf File.Exists(Source) And Directory.Exists(Destination) Then
                Wdbg(DebugLevel.I, "Source is a file and destination is a directory")
                File.Copy(Source, Destination + "/" + FileName, True)

                'Raise event
                EventManager.RaiseFileCopied(Source, Destination + "/" + FileName)
                Return True
            ElseIf File.Exists(Source) Then
                Wdbg(DebugLevel.I, "Source is a file and destination is a file")
                File.Copy(Source, Destination, True)

                'Raise event
                EventManager.RaiseFileCopied(Source, Destination)
                Return True
            Else
                Wdbg(DebugLevel.E, "Source or destination are invalid.")
                Throw New IOException(DoTranslation("The path is neither a file nor a directory."))
            End If
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Failed to copy {0} to {1}: {2}", Source, Destination, ex.Message)
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Failed to copy file or directory: {0}").FormatString(ex.Message))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Copies the directory from source to destination
    ''' </summary>
    ''' <param name="Source">Source directory</param>
    ''' <param name="Destination">Target directory</param>
    Private Sub CopyDirectory(Source As String, Destination As String)
        CopyDirectory(Source, Destination, ShowFilesystemProgress)
    End Sub

    ''' <summary>
    ''' Copies the directory from source to destination
    ''' </summary>
    ''' <param name="Source">Source directory</param>
    ''' <param name="Destination">Target directory</param>
    ''' <param name="ShowProgress">Whether or not to show what files are being copied</param>
    Private Sub CopyDirectory(Source As String, Destination As String, ShowProgress As Boolean)
        If Not Directory.Exists(Source) Then Throw New IOException(DoTranslation("Directory {0} not found.").FormatString(Source))

        'Get all source directories and files
        Dim SourceDirInfo As New DirectoryInfo(Source)
        Dim SourceDirectories As DirectoryInfo() = SourceDirInfo.GetDirectories
        Wdbg(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length)
        Dim SourceFiles As FileInfo() = SourceDirInfo.GetFiles
        Wdbg(DebugLevel.I, "Source files: {0}", SourceFiles.Length)

        'Make a destination directory if it doesn't exist
        If Not Directory.Exists(Destination) Then
            Wdbg(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", Destination)
            Directory.CreateDirectory(Destination)
        End If

        'Iterate through every file and copy them to destination
        For Each SourceFile As FileInfo In SourceFiles
            Dim DestinationFilePath As String = Path.Combine(Destination, SourceFile.Name)
            Wdbg(DebugLevel.I, "Copying file {0} to destination...", DestinationFilePath)
            If ShowProgress Then W("-> {0}", True, ColTypes.Neutral, DestinationFilePath)
            SourceFile.CopyTo(DestinationFilePath, True)
        Next

        'Iterate through every subdirectory and copy them to destination
        For Each SourceDirectory As DirectoryInfo In SourceDirectories
            Dim DestinationDirectoryPath As String = Path.Combine(Destination, SourceDirectory.Name)
            Wdbg(DebugLevel.I, "Calling CopyDirectory() with destination {0}...", DestinationDirectoryPath)
            CopyDirectory(SourceDirectory.FullName, DestinationDirectoryPath)
        Next
    End Sub

    ''' <summary>
    ''' Set size parse mode (whether to enable full size parse for directories or just the surface)
    ''' </summary>
    ''' <param name="Enable">To enable or to disable</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function SetSizeParseMode(Enable As Boolean) As Boolean
        Try
            FullParseMode = Enable
            Dim Token As JToken = GetConfigCategory(ConfigCategory.Filesystem)
            SetConfigValueAndWrite(ConfigCategory.Filesystem, Token, "Size parse mode", FullParseMode)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Error when trying to set parse mode. Check the value and try again. If this is correct, see the stack trace when kernel debugging is enabled.") + " " + ex.Message, ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Makes a directory
    ''' </summary>
    ''' <param name="NewDirectory">New directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function MakeDirectory(NewDirectory As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(NewDirectory)
#End If
        NewDirectory = NeutralizePath(NewDirectory)
        Wdbg(DebugLevel.I, "New directory: {0} ({1})", NewDirectory, Directory.Exists(NewDirectory))
        If Not Directory.Exists(NewDirectory) Then
            Directory.CreateDirectory(NewDirectory)

            'Raise event
            EventManager.RaiseDirectoryCreated(NewDirectory)
            Return True
        Else
            Throw New IOException(DoTranslation("Directory {0} already exists.").FormatString(NewDirectory))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Makes a file
    ''' </summary>
    ''' <param name="NewFile">New file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function MakeFile(NewFile As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(NewFile)
#End If
        NewFile = NeutralizePath(NewFile)
        Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", NewFile, File.Exists(NewFile))
        If Not File.Exists(NewFile) Then
            Try
                Dim NewFileStream As FileStream = File.Create(NewFile)
                Wdbg(DebugLevel.I, "File created")
                NewFileStream.Close()
                Wdbg(DebugLevel.I, "File closed")

                'Raise event
                EventManager.RaiseFileCreated(NewFile)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Throw New IOException(DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message))
            End Try
        Else
            Throw New IOException(DoTranslation("File already exists."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Moves a file or directory
    ''' </summary>
    ''' <param name="Source">Source file or directory</param>
    ''' <param name="Destination">Target file or directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function MoveFileOrDir(Source As String, Destination As String) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(Source)
            ThrowOnInvalidPath(Destination)
#End If
            Source = NeutralizePath(Source)
            Wdbg(DebugLevel.I, "Source directory: {0}", Source)
            Destination = NeutralizePath(Destination)
            Wdbg(DebugLevel.I, "Target directory: {0}", Destination)
            Dim FileName As String = Path.GetFileName(Source)
            Wdbg(DebugLevel.I, "Source file name: {0}", FileName)
            If Directory.Exists(Source) Then
                Wdbg(DebugLevel.I, "Source and destination are directories")
                Directory.Move(Source, Destination)

                'Raise event
                EventManager.RaiseDirectoryMoved(Source, Destination)
                Return True
            ElseIf File.Exists(Source) And Directory.Exists(Destination) Then
                Wdbg(DebugLevel.I, "Source is a file and destination is a directory")
                File.Move(Source, Destination + "/" + FileName)

                'Raise event
                EventManager.RaiseFileMoved(Source, Destination + "/" + FileName)
                Return True
            ElseIf File.Exists(Source) Then
                Wdbg(DebugLevel.I, "Source is a file and destination is a file")
                File.Move(Source, Destination)

                'Raise event
                EventManager.RaiseFileMoved(Source, Destination)
                Return True
            Else
                Wdbg(DebugLevel.E, "Source or destination are invalid.")
                Throw New IOException(DoTranslation("The path is neither a file nor a directory."))
            End If
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Failed to move {0} to {1}: {2}", Source, Destination, ex.Message)
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Failed to move file or directory: {0}").FormatString(ex.Message))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Removes a directory
    ''' </summary>
    ''' <param name="Target">Target directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function RemoveDirectory(Target As String) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(Target)
#End If
            Dim Dir As String = NeutralizePath(Target)
            Directory.Delete(Dir, True)

            'Raise event
            EventManager.RaiseDirectoryRemoved(Target)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Unable to remove directory: {0}").FormatString(ex.Message))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Removes a file
    ''' </summary>
    ''' <param name="Target">Target directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function RemoveFile(Target As String) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(Target)
#End If
            Dim Dir As String = NeutralizePath(Target)
            File.Delete(Dir)

            'Raise event
            EventManager.RaiseFileRemoved(Target)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Unable to remove file: {0}").FormatString(ex.Message))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Searches a file for string
    ''' </summary>
    ''' <param name="FilePath">File path</param>
    ''' <param name="StringLookup">String to find</param>
    ''' <returns>The list if successful; null if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function SearchFileForString(FilePath As String, StringLookup As String) As List(Of String)
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(FilePath)
#End If
            FilePath = NeutralizePath(FilePath)
            Dim Matches As New List(Of String)
            Dim Filebyte() As String = File.ReadAllLines(FilePath)
            Dim MatchNum As Integer = 1
            Dim LineNumber As Integer = 1
            For Each Str As String In Filebyte
                If Str.Contains(StringLookup) Then
                    Matches.Add($"[{LineNumber}] " + DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str))
                    MatchNum += 1
                End If
                LineNumber += 1
            Next
            Return Matches
        Catch ex As Exception
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Unable to find file to match string ""{0}"": {1}").FormatString(StringLookup, ex.Message))
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Searches a file for string using regexp
    ''' </summary>
    ''' <param name="FilePath">File path</param>
    ''' <param name="StringLookup">String to find</param>
    ''' <returns>The list if successful; null if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function SearchFileForStringRegexp(FilePath As String, StringLookup As Regex) As List(Of String)
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(FilePath)
#End If
            FilePath = NeutralizePath(FilePath)
            Dim Matches As New List(Of String)
            Dim Filebyte() As String = File.ReadAllLines(FilePath)
            Dim MatchNum As Integer = 1
            Dim LineNumber As Integer = 1
            For Each Str As String In Filebyte
                If StringLookup.IsMatch(Str) Then
                    Matches.Add($"[{LineNumber}] " + DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str))
                    MatchNum += 1
                End If
                LineNumber += 1
            Next
            Return Matches
        Catch ex As Exception
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Unable to find file to match string ""{0}"": {1}").FormatString(StringLookup, ex.Message))
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Removes attribute
    ''' </summary>
    ''' <param name="attributes">All attributes</param>
    ''' <param name="attributesToRemove">Attributes to remove</param>
    ''' <returns>Attributes without target attribute</returns>
    <Extension>
    Public Function RemoveAttribute(attributes As FileAttributes, attributesToRemove As FileAttributes) As FileAttributes
        Return attributes And (Not attributesToRemove)
    End Function

    ''' <summary>
    ''' Adds attribute to file
    ''' </summary>
    ''' <param name="FilePath">File path</param>
    ''' <param name="Attributes">Attributes</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function AddAttributeToFile(FilePath As String, Attributes As FileAttributes) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(FilePath)
#End If
            FilePath = NeutralizePath(FilePath)
            Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attributes)
            File.SetAttributes(FilePath, Attributes)

            'Raise event
            EventManager.RaiseFileAttributeAdded(FilePath, Attributes)
            Return True
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Failed to add attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Removes attribute from file
    ''' </summary>
    ''' <param name="FilePath">File path</param>
    ''' <param name="Attributes">Attributes</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function RemoveAttributeFromFile(FilePath As String, Attributes As FileAttributes) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(FilePath)
#End If
            FilePath = NeutralizePath(FilePath)
            Dim Attrib As FileAttributes = File.GetAttributes(FilePath)
            Wdbg(DebugLevel.I, "File attributes: {0}", Attrib)
            Attrib = Attrib.RemoveAttribute(Attributes)
            Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attrib)
            File.SetAttributes(FilePath, Attrib)

            'Raise event
            EventManager.RaiseFileAttributeRemoved(FilePath, Attributes)
            Return True
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Failed to remove attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

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

    ''' <summary>
    ''' Opens a file, reads all lines, and returns the array of lines
    ''' </summary>
    ''' <param name="path">Path to file</param>
    ''' <returns>Array of lines</returns>
    Public Function ReadAllLinesNoBlock(path As String) As String()
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(path)
#End If

        'Read all the lines, bypassing the restrictions.
        path = NeutralizePath(path)
        Dim AllLnList As New List(Of String)
        Dim FOpen As New StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        While Not FOpen.EndOfStream
            AllLnList.Add(FOpen.ReadLine)
        End While
        Return AllLnList.ToArray
    End Function

    ''' <summary>
    ''' Gets the lookup path list
    ''' </summary>
    Public Function GetPathList() As List(Of String)
        Return PathsToLookup.Split(PathLookupDelimiter).ToList
    End Function

    ''' <summary>
    ''' Adds a (non-)neutralized path to lookup
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function AddToPathLookup(Path As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(Path)
#End If
        Dim LookupPaths As List(Of String) = GetPathList()
        Path = NeutralizePath(Path)
        LookupPaths.Add(Path)
        PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
        Return True
    End Function

    ''' <summary>
    ''' Adds a (non-)neutralized path to lookup
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function AddToPathLookup(Path As String, RootPath As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(Path)
        ThrowOnInvalidPath(RootPath)
#End If
        Dim LookupPaths As List(Of String) = GetPathList()
        Path = NeutralizePath(Path, RootPath)
        LookupPaths.Add(Path)
        PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
        Return True
    End Function

    ''' <summary>
    ''' Removes an existing (non-)neutralized path from lookup
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function RemoveFromPathLookup(Path As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(Path)
#End If
        Dim LookupPaths As List(Of String) = GetPathList()
        Dim Returned As Boolean
        Path = NeutralizePath(Path)
        Returned = LookupPaths.Remove(Path)
        PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
        Return Returned
    End Function

    ''' <summary>
    ''' Removes an existing (non-)neutralized path from lookup
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function RemoveFromPathLookup(Path As String, RootPath As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(Path)
        ThrowOnInvalidPath(RootPath)
#End If
        Dim LookupPaths As List(Of String) = GetPathList()
        Dim Returned As Boolean
        Path = NeutralizePath(Path, RootPath)
        Returned = LookupPaths.Remove(Path)
        PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
        Return Returned
    End Function

    ''' <summary>
    ''' Checks to see if the file exists in PATH and writes the result (path to file) to a string variable, if any.
    ''' </summary>
    ''' <param name="FilePath">A full path to file or just a file name</param>
    ''' <param name="Result">The neutralized path</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FileExistsInPath(FilePath As String, ByRef Result As String) As Boolean
#If NTFSCorruptionFix Then
        ThrowOnInvalidPath(FilePath)
#End If
        Dim LookupPaths As List(Of String) = GetPathList()
        Dim ResultingPath As String
        For Each LookupPath As String In LookupPaths
            ResultingPath = NeutralizePath(FilePath, LookupPath)
            If File.Exists(ResultingPath) Then
                Result = ResultingPath
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Saves the current directory to configuration
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SaveCurrDir() As Boolean
        Try
            Dim Token As JToken = GetConfigCategory(ConfigCategory.Shell)
            SetConfigValueAndWrite(ConfigCategory.Shell, Token, "Current Directory", CurrDir)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Wdbg(DebugLevel.E, "Failed to save current directory: {0}", ex.Message)
            Throw New Exceptions.FilesystemException(DoTranslation("Failed to save current directory: {0}"), ex, ex.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Tries to parse the path (For file names and only names, use <see cref="TryParseFileName(String)"/> instead.)
    ''' </summary>
    ''' <param name="Path">The path to be parsed</param>
    ''' <returns>True if successful; false if unsuccessful</returns>
    Public Function TryParsePath(Path As String) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(Path)
#End If
            Return Not Path.IndexOfAny(IO.Path.GetInvalidPathChars()) >= 0
        Catch ex As Exception
            WStkTrc(ex)
            Wdbg(DebugLevel.E, "Failed to parse path {0}: {1}", Path, ex.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Tries to parse the file name (For full paths, use <see cref="TryParsePath(String)"/> instead.)
    ''' </summary>
    ''' <param name="Name">The file name to be parsed</param>
    ''' <returns>True if successful; false if unsuccessful</returns>
    Public Function TryParseFileName(Name As String) As Boolean
        Try
#If NTFSCorruptionFix Then
            ThrowOnInvalidPath(Name)
#End If
            Return Not Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
        Catch ex As Exception
            WStkTrc(ex)
            Wdbg(DebugLevel.E, "Failed to parse file name {0}: {1}", Name, ex.Message)
        End Try
        Return False
    End Function

#If NTFSCorruptionFix Then
    ''' <summary>
    ''' Mitigates Windows 10/11 NTFS corruption or Windows 10/11 BSOD bug
    ''' </summary>
    ''' <param name="Path">Target path</param>
    Public Sub ThrowOnInvalidPath(Path As String)
        If IsOnWindows() And (Path.Contains("$i30") Or Path.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
            Wdbg(DebugLevel.F, "Trying to access invalid path. Path was {0}", Path)
            Throw New ArgumentException(DoTranslation("Trying to access invalid path."))
        End If
    End Sub
#End If

End Module
