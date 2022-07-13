
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
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq
Imports KS.Misc.Configuration

Namespace Files
    Public Module Filesystem

        'Variables
        Public CurrDir As String = HomePath
        Public ShowFilesystemProgress As Boolean = True
        Public SortList As Boolean = True
        Public SortMode As FilesystemSortOptions = FilesystemSortOptions.FullName
        Public SortDirection As FilesystemSortDirection = FilesystemSortDirection.Ascending
        Public ShowFileDetailsList As Boolean = True
        Public ShowTotalSizeInList As Boolean

        ''' <summary>
        ''' The new line style used for the current platform
        ''' </summary>
        Public ReadOnly Property NewlineStyle As FilesystemNewlineStyle
            Get
                Select Case Environment.NewLine
                    Case vbCrLf
                        Return FilesystemNewlineStyle.CRLF
                    Case vbLf
                        Return FilesystemNewlineStyle.LF
                    Case vbCr
                        Return FilesystemNewlineStyle.CR
                    Case Else
                        Return FilesystemNewlineStyle.CRLF
                End Select
            End Get
        End Property

        ''' <summary>
        ''' Sets the current working directory
        ''' </summary>
        ''' <param name="dir">A directory</param>
        ''' <returns>True if successful, False if unsuccessful.</returns>
        ''' <exception cref="DirectoryNotFoundException"></exception>
        Public Function SetCurrDir(dir As String) As Boolean
            ThrowOnInvalidPath(dir)
            dir = NeutralizePath(dir)
            Wdbg(DebugLevel.I, "Directory exists? {0}", FolderExists(dir))
            If FolderExists(dir) Then
                Dim Parser As New DirectoryInfo(dir)
                CurrDir = Parser.FullName.Replace("\", "/")

                'Raise event
                KernelEventManager.RaiseCurrentDirectoryChanged()
                Return True
            Else
                Throw New DirectoryNotFoundException(DoTranslation("Directory {0} not found").FormatString(dir))
            End If
            Return False
        End Function

        ''' <summary>
        ''' Prints the contents of a file to the console
        ''' </summary>
        ''' <param name="filename">Full path to file</param>
        Public Sub PrintContents(filename As String)
            PrintContents(filename, PrintLineNumbers)
        End Sub

        ''' <summary>
        ''' Prints the contents of a file to the console
        ''' </summary>
        ''' <param name="filename">Full path to file with wildcards supported</param>
        Public Sub PrintContents(filename As String, PrintLineNumbers As Boolean)
            'Read the contents
            ThrowOnInvalidPath(filename)
            filename = NeutralizePath(filename)
            For Each FilePath As String In GetFilesystemEntries(filename, True)
                Dim Contents As String() = ReadContents(FilePath)
                For ContentIndex As Integer = 0 To Contents.Length - 1
                    If PrintLineNumbers Then
                        Write("{0,4}: ", False, ColTypes.ListEntry, ContentIndex + 1)
                    End If
                    Write(Contents(ContentIndex), True, ColTypes.Neutral)
                Next
            Next
        End Sub

        ''' <summary>
        ''' Reads the contents of a file and writes it to the array
        ''' </summary>
        ''' <param name="filename">Full path to file</param>
        ''' <returns>An array full of file contents</returns>
        Public Function ReadContents(filename As String) As String()
            'Read the contents
            ThrowOnInvalidPath(filename)
            Dim FileContents As New List(Of String)
            filename = NeutralizePath(filename)
            Using FStream As New StreamReader(filename)
                Wdbg(DebugLevel.I, "Stream to file {0} opened.", filename)
                While Not FStream.EndOfStream
                    FileContents.Add(FStream.ReadLine)
                End While
            End Using
            Return FileContents.ToArray
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
        ''' Checks to see if the file exists. Windows 10/11 bug aware.
        ''' </summary>
        ''' <param name="File">Target file</param>
        ''' <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        Public Function FileExists(File As String, Optional Neutralize As Boolean = False) As Boolean
            ThrowOnInvalidPath(File)
            If Neutralize Then File = NeutralizePath(File)
            Return IO.File.Exists(File)
        End Function

        ''' <summary>
        ''' Checks to see if the folder exists. Windows 10/11 bug aware.
        ''' </summary>
        ''' <param name="Folder">Target folder</param>
        ''' <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        Public Function FolderExists(Folder As String, Optional Neutralize As Boolean = False) As Boolean
            ThrowOnInvalidPath(Folder)
            If Neutralize Then Folder = NeutralizePath(Folder)
            Return Directory.Exists(Folder)
        End Function

        ''' <summary>
        ''' Simplifies the path to the correct one. It converts the path format to the unified format.
        ''' </summary>
        ''' <param name="Path">Target path, be it a file or a folder</param>
        ''' <returns>Absolute path</returns>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function NeutralizePath(Path As String, Optional Strict As Boolean = False) As String
            Return NeutralizePath(Path, CurrDir, Strict)
        End Function

        ''' <summary>
        ''' Simplifies the path to the correct one. It converts the path format to the unified format.
        ''' </summary>
        ''' <param name="Path">Target path, be it a file or a folder</param>
        ''' <param name="Source">Source path in which the target is found. Must be a directory</param>
        ''' <returns>Absolute path</returns>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function NeutralizePath(Path As String, Source As String, Optional Strict As Boolean = False) As String
            If Path Is Nothing Then Path = ""
            If Source Is Nothing Then Source = ""

            ThrowOnInvalidPath(Path)
            ThrowOnInvalidPath(Source)

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
                If Path.Contains(Source) And Path.AllIndexesOf(Source).Count > 1 Then
                    Path = ReplaceLastOccurrence(Path, Source, "")
                End If
            End If

            'If strict, checks for existence of file
            If Strict Then
                If FileExists(Path) Or FolderExists(Path) Then
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
                ThrowOnInvalidPath(Source)
                ThrowOnInvalidPath(Destination)
                Source = NeutralizePath(Source)
                Wdbg(DebugLevel.I, "Source directory: {0}", Source)
                Destination = NeutralizePath(Destination)
                Wdbg(DebugLevel.I, "Target directory: {0}", Destination)
                Dim FileName As String = Path.GetFileName(Source)
                Wdbg(DebugLevel.I, "Source file name: {0}", FileName)
                If FolderExists(Source) Then
                    Wdbg(DebugLevel.I, "Source and destination are directories")
                    CopyDirectory(Source, Destination)

                    'Raise event
                    KernelEventManager.RaiseDirectoryCopied(Source, Destination)
                    Return True
                ElseIf FileExists(Source) And FolderExists(Destination) Then
                    Wdbg(DebugLevel.I, "Source is a file and destination is a directory")
                    File.Copy(Source, Destination + "/" + FileName, True)

                    'Raise event
                    KernelEventManager.RaiseFileCopied(Source, Destination + "/" + FileName)
                    Return True
                ElseIf FileExists(Source) Then
                    Wdbg(DebugLevel.I, "Source is a file and destination is a file")
                    File.Copy(Source, Destination, True)

                    'Raise event
                    KernelEventManager.RaiseFileCopied(Source, Destination)
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
        Public Sub CopyDirectory(Source As String, Destination As String)
            CopyDirectory(Source, Destination, ShowFilesystemProgress)
        End Sub

        ''' <summary>
        ''' Copies the directory from source to destination
        ''' </summary>
        ''' <param name="Source">Source directory</param>
        ''' <param name="Destination">Target directory</param>
        ''' <param name="ShowProgress">Whether or not to show what files are being copied</param>
        Public Sub CopyDirectory(Source As String, Destination As String, ShowProgress As Boolean)
            ThrowOnInvalidPath(Source)
            ThrowOnInvalidPath(Destination)
            If Not FolderExists(Source) Then Throw New IOException(DoTranslation("Directory {0} not found.").FormatString(Source))

            'Get all source directories and files
            Dim SourceDirInfo As New DirectoryInfo(Source)
            Dim SourceDirectories As DirectoryInfo() = SourceDirInfo.GetDirectories
            Wdbg(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length)
            Dim SourceFiles As FileInfo() = SourceDirInfo.GetFiles
            Wdbg(DebugLevel.I, "Source files: {0}", SourceFiles.Length)

            'Make a destination directory if it doesn't exist
            If Not FolderExists(Destination) Then
                Wdbg(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", Destination)
                Directory.CreateDirectory(Destination)
            End If

            'Iterate through every file and copy them to destination
            For Each SourceFile As FileInfo In SourceFiles
                Dim DestinationFilePath As String = Path.Combine(Destination, SourceFile.Name)
                Wdbg(DebugLevel.I, "Copying file {0} to destination...", DestinationFilePath)
                If ShowProgress Then Write("-> {0}", True, ColTypes.Neutral, DestinationFilePath)
                SourceFile.CopyTo(DestinationFilePath, True)
            Next

            'Iterate through every subdirectory and copy them to destination
            For Each SourceDirectory As DirectoryInfo In SourceDirectories
                Dim DestinationDirectoryPath As String = Path.Combine(Destination, SourceDirectory.Name)
                Wdbg(DebugLevel.I, "Calling CopyDirectory() with destination {0}...", DestinationDirectoryPath)
                If ShowProgress Then Write("* {0}", True, ColTypes.Neutral, DestinationDirectoryPath)
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
                SetConfigValue(ConfigCategory.Filesystem, Token, "Size parse mode", FullParseMode)
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
        Public Function MakeDirectory(NewDirectory As String, Optional ThrowIfDirectoryExists As Boolean = True) As Boolean
            ThrowOnInvalidPath(NewDirectory)
            NewDirectory = NeutralizePath(NewDirectory)
            Wdbg(DebugLevel.I, "New directory: {0} ({1})", NewDirectory, FolderExists(NewDirectory))
            If Not FolderExists(NewDirectory) Then
                Directory.CreateDirectory(NewDirectory)

                'Raise event
                KernelEventManager.RaiseDirectoryCreated(NewDirectory)
                Return True
            ElseIf ThrowIfDirectoryExists Then
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
        Public Function MakeFile(NewFile As String, Optional ThrowIfFileExists As Boolean = True) As Boolean
            ThrowOnInvalidPath(NewFile)
            NewFile = NeutralizePath(NewFile)
            Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", NewFile, FileExists(NewFile))
            If Not FileExists(NewFile) Then
                Try
                    Dim NewFileStream As FileStream = File.Create(NewFile)
                    Wdbg(DebugLevel.I, "File created")
                    NewFileStream.Close()
                    Wdbg(DebugLevel.I, "File closed")

                    'Raise event
                    KernelEventManager.RaiseFileCreated(NewFile)
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New IOException(DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message))
                End Try
            ElseIf ThrowIfFileExists Then
                Throw New IOException(DoTranslation("File already exists."))
            End If
            Return False
        End Function

        ''' <summary>
        ''' Makes an empty JSON file
        ''' </summary>
        ''' <param name="NewFile">New JSON file</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function MakeJsonFile(NewFile As String, Optional ThrowIfFileExists As Boolean = True) As Boolean
            ThrowOnInvalidPath(NewFile)
            NewFile = NeutralizePath(NewFile)
            Wdbg(DebugLevel.I, "File path is {0} and .Exists is {0}", NewFile, FileExists(NewFile))
            If Not FileExists(NewFile) Then
                Try
                    Dim NewFileStream As FileStream = File.Create(NewFile)
                    Wdbg(DebugLevel.I, "File created")
                    Dim NewJsonObject As JObject = JObject.Parse("{}")
                    Dim NewFileWriter As New StreamWriter(NewFileStream)
                    NewFileWriter.WriteLine(JsonConvert.SerializeObject(NewJsonObject))
                    NewFileStream.Close()
                    Wdbg(DebugLevel.I, "File closed")

                    'Raise event
                    KernelEventManager.RaiseFileCreated(NewFile)
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New IOException(DoTranslation("Error trying to create a file: {0}").FormatString(ex.Message))
                End Try
            ElseIf ThrowIfFileExists Then
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
                ThrowOnInvalidPath(Source)
                ThrowOnInvalidPath(Destination)
                Source = NeutralizePath(Source)
                Wdbg(DebugLevel.I, "Source directory: {0}", Source)
                Destination = NeutralizePath(Destination)
                Wdbg(DebugLevel.I, "Target directory: {0}", Destination)
                Dim FileName As String = Path.GetFileName(Source)
                Wdbg(DebugLevel.I, "Source file name: {0}", FileName)
                If FolderExists(Source) Then
                    Wdbg(DebugLevel.I, "Source and destination are directories")
                    Directory.Move(Source, Destination)

                    'Raise event
                    KernelEventManager.RaiseDirectoryMoved(Source, Destination)
                    Return True
                ElseIf FileExists(Source) And FolderExists(Destination) Then
                    Wdbg(DebugLevel.I, "Source is a file and destination is a directory")
                    File.Move(Source, Destination + "/" + FileName)

                    'Raise event
                    KernelEventManager.RaiseFileMoved(Source, Destination + "/" + FileName)
                    Return True
                ElseIf FileExists(Source) Then
                    Wdbg(DebugLevel.I, "Source is a file and destination is a file")
                    File.Move(Source, Destination)

                    'Raise event
                    KernelEventManager.RaiseFileMoved(Source, Destination)
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
                ThrowOnInvalidPath(Target)
                Dim Dir As String = NeutralizePath(Target)
                Directory.Delete(Dir, True)

                'Raise event
                KernelEventManager.RaiseDirectoryRemoved(Target)
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
                ThrowOnInvalidPath(Target)
                Dim Dir As String = NeutralizePath(Target)
                File.Delete(Dir)

                'Raise event
                KernelEventManager.RaiseFileRemoved(Target)
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
                ThrowOnInvalidPath(FilePath)
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
                ThrowOnInvalidPath(FilePath)
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
                ThrowOnInvalidPath(FilePath)
                FilePath = NeutralizePath(FilePath)
                Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attributes)
                File.SetAttributes(FilePath, Attributes)

                'Raise event
                KernelEventManager.RaiseFileAttributeAdded(FilePath, Attributes)
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
                ThrowOnInvalidPath(FilePath)
                FilePath = NeutralizePath(FilePath)
                Dim Attrib As FileAttributes = File.GetAttributes(FilePath)
                Wdbg(DebugLevel.I, "File attributes: {0}", Attrib)
                Attrib = Attrib.RemoveAttribute(Attributes)
                Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attrib)
                File.SetAttributes(FilePath, Attrib)

                'Raise event
                KernelEventManager.RaiseFileAttributeRemoved(FilePath, Attributes)
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
            ThrowOnInvalidPath(path)

            'Read all the lines, bypassing the restrictions.
            path = NeutralizePath(path)
            Dim AllLnList As New List(Of String)
            Dim FOpen As New StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            While Not FOpen.EndOfStream
                AllLnList.Add(FOpen.ReadLine)
            End While
            FOpen.Close()
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
            ThrowOnInvalidPath(Path)
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
            ThrowOnInvalidPath(Path)
            ThrowOnInvalidPath(RootPath)
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
            ThrowOnInvalidPath(Path)
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
            ThrowOnInvalidPath(Path)
            ThrowOnInvalidPath(RootPath)
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
            ThrowOnInvalidPath(FilePath)
            Dim LookupPaths As List(Of String) = GetPathList()
            Dim ResultingPath As String
            For Each LookupPath As String In LookupPaths
                ResultingPath = NeutralizePath(FilePath, LookupPath)
                If FileExists(ResultingPath) Then
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
                SetConfigValue(ConfigCategory.Shell, Token, "Current Directory", CurrDir)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to save current directory: {0}", ex.Message)
                Throw New Exceptions.FilesystemException(DoTranslation("Failed to save current directory: {0}"), ex, ex.Message)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Gets all the invalid path characters
        ''' </summary>
        Public Function GetInvalidPathChars() As Char()
            Dim FinalInvalidPathChars As Char() = Path.GetInvalidPathChars()
            Dim WindowsInvalidPathChars As Char() = {"""", "<", ">"}
            If KernelSimulatorMoniker = ".NET CoreCLR" And IsOnWindows() Then
                'It's weird of .NET 6.0 to not consider the above three Windows invalid directory chars to be invalid,
                'so make them invalid as in .NET Framework.
                ReDim Preserve FinalInvalidPathChars(35)
                WindowsInvalidPathChars.CopyTo(FinalInvalidPathChars, FinalInvalidPathChars.Length - 3)
            End If
            Return FinalInvalidPathChars
        End Function

        ''' <summary>
        ''' Tries to parse the path (For file names and only names, use <see cref="TryParseFileName(String)"/> instead.)
        ''' </summary>
        ''' <param name="Path">The path to be parsed</param>
        ''' <returns>True if successful; false if unsuccessful</returns>
        Public Function TryParsePath(Path As String) As Boolean
            Try
                ThrowOnInvalidPath(Path)
                Return Not Path.IndexOfAny(GetInvalidPathChars()) >= 0
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
                ThrowOnInvalidPath(Name)
                Return Not Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to parse file name {0}: {1}", Name, ex.Message)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Combines the files and puts the combined output to the array
        ''' </summary>
        ''' <param name="Input">An input file</param>
        ''' <param name="TargetInputs">The target inputs to merge</param>
        Public Function CombineFiles(Input As String, TargetInputs() As String) As String()
            Try
                Dim CombinedContents As New List(Of String)

                'Add the input contents
                ThrowOnInvalidPath(Input)
                CombinedContents.AddRange(ReadContents(Input))

                'Enumerate the target inputs
                For Each TargetInput As String In TargetInputs
                    ThrowOnInvalidPath(TargetInput)
                    CombinedContents.AddRange(ReadContents(TargetInput))
                Next

                'Return the combined contents
                Return CombinedContents.ToArray
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to combine files: {0}", ex.Message)
                Throw New Exceptions.FilesystemException(DoTranslation("Failed to combine files."), ex)
            End Try
        End Function

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

        ''' <summary>
        ''' Converts the line endings to the newline style for the current platform
        ''' </summary>
        ''' <param name="TextFile">Text file name with extension or file path</param>
        Public Sub ConvertLineEndings(TextFile As String)
            ConvertLineEndings(TextFile, NewlineStyle)
        End Sub

        ''' <summary>
        ''' Converts the line endings to the specified newline style
        ''' </summary>
        ''' <param name="TextFile">Text file name with extension or file path</param>
        ''' <param name="LineEndingStyle">Line ending style</param>
        Public Sub ConvertLineEndings(TextFile As String, LineEndingStyle As FilesystemNewlineStyle)
            ThrowOnInvalidPath(TextFile)
            TextFile = NeutralizePath(TextFile)
            If Not FileExists(TextFile) Then Throw New IOException(DoTranslation("File {0} not found.").FormatString(TextFile))

            'Get all the file lines, regardless of the new line style on the target file
            Dim FileContents() As String = ReadAllLinesNoBlock(TextFile)
            Wdbg(DebugLevel.I, "Got {0} lines. Converting newlines in {1} to {2}...", FileContents.Length, TextFile, LineEndingStyle.ToString)

            'Get the newline string according to the current style
            Dim NewLineString As String = GetLineEndingString(LineEndingStyle)

            'Convert the newlines now
            Dim Result As New StringBuilder
            For Each FileContent As String In FileContents
                Result.Append(FileContent + NewLineString)
            Next

            'Save the changes
            File.WriteAllText(TextFile, Result.ToString)
        End Sub

        ''' <summary>
        ''' Gets the line ending string from the specified line ending style
        ''' </summary>
        ''' <param name="LineEndingStyle">Line ending style</param>
        Public Function GetLineEndingString(LineEndingStyle As FilesystemNewlineStyle) As String
            Select Case LineEndingStyle
                Case FilesystemNewlineStyle.CRLF
                    Return vbCrLf
                Case FilesystemNewlineStyle.LF
                    Return vbLf
                Case FilesystemNewlineStyle.CR
                    Return vbCr
                Case Else
                    Return Environment.NewLine
            End Select
        End Function

        ''' <summary>
        ''' Gets the line ending style from file
        ''' </summary>
        ''' <param name="TextFile">Target text file</param>
        Public Function GetLineEndingFromFile(TextFile As String) As FilesystemNewlineStyle
            ThrowOnInvalidPath(TextFile)
            TextFile = NeutralizePath(TextFile)
            If Not FileExists(TextFile) Then Throw New IOException(DoTranslation("File {0} not found.").FormatString(TextFile))

            'Open the file stream
            Dim NewlineStyle As FilesystemNewlineStyle = Filesystem.NewlineStyle
            Dim TextFileStream As New FileStream(TextFile, FileMode.Open, FileAccess.Read)
            Dim CarriageReturnCode As Integer = GetAsciiCode(GetLineEndingString(FilesystemNewlineStyle.CR), 0)
            Dim LineFeedCode As Integer = GetAsciiCode(GetLineEndingString(FilesystemNewlineStyle.LF), 0)
            Dim CarriageReturnSpotted As Boolean
            Dim LineFeedSpotted As Boolean
            Dim ExitOnSpotted As Boolean

            'Search for new line style
            Do Until TextFileStream.Position = TextFileStream.Length
                Dim Result As Integer = TextFileStream.ReadByte
                If Result = LineFeedCode Then
                    LineFeedSpotted = True
                    ExitOnSpotted = True
                End If
                If Result = CarriageReturnCode Then
                    CarriageReturnSpotted = True
                    ExitOnSpotted = True
                End If
                If ExitOnSpotted And (Result <> LineFeedCode And Result <> CarriageReturnCode) Then Exit Do
            Loop
            TextFileStream.Close()

            'Return the style used
            If LineFeedSpotted And CarriageReturnSpotted Then
                NewlineStyle = FilesystemNewlineStyle.CRLF
            ElseIf LineFeedSpotted Then
                NewlineStyle = FilesystemNewlineStyle.LF
            ElseIf CarriageReturnSpotted Then
                NewlineStyle = FilesystemNewlineStyle.CR
            End If
            Return NewlineStyle
        End Function

        ''' <summary>
        ''' Mitigates Windows 10/11 NTFS corruption/Blue Screen of Death (BSOD) bug
        ''' </summary>
        ''' <param name="Path">Target path</param>
        ''' <remarks>
        ''' - When we try to access the secret NTFS bitmap path, which contains <b>$i30</b>, from the partition root path, we'll trigger the "Your disk is corrupt." <br></br>
        ''' - When we try to access the <b>kernelconnect</b> secret device from the system partition root path, we'll trigger the BSOD. <br></br><br></br>
        ''' This sub will try to prevent access to these paths on unpatched systems and patched systems by throwing <see cref="ArgumentException"/>
        ''' </remarks>
        Public Sub ThrowOnInvalidPath(Path As String)
#If NTFSCorruptionFix Then
            If IsOnWindows() And (Path.Contains("$i30") Or Path.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
                Wdbg(DebugLevel.F, "Trying to access invalid path. Path was {0}", Path)
                Throw New ArgumentException(DoTranslation("Trying to access invalid path."), NameOf(Path))
            End If
#End If
        End Sub

    End Module
End Namespace