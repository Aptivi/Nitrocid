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

Public Module Filesystem

    'Variables
    'TODO: Use Directory.SetCurrentDirectory in subsequent development commits
    Public CurrDir As String = paths("Home")

    ''' <summary>
    ''' Sets the current working directory
    ''' </summary>
    ''' <param name="dir">A directory</param>
    ''' <returns>True if successful, False if unsuccessful.</returns>
    ''' <exception cref="DirectoryNotFoundException"></exception>
    Public Function SetCurrDir(ByVal dir As String) As Boolean
        dir = NeutralizePath(dir)
        Wdbg("I", "Directory exists? {0}", Directory.Exists(dir))
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
    Public Sub ReadContents(ByVal filename As String)
#If NTFSCorruptionFix Then
        'Mitigate Windows 10 NTFS corruption or Windows 10 BSOD bug
        If IsOnWindows() And (filename.Contains("$i30") Or filename.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
            Wdbg("F", "Trying to access invalid path. Path was {0}", filename)
            Throw New ArgumentException(DoTranslation("Trying to access invalid path."))
        End If
#End If

        'Read the contents
        filename = NeutralizePath(filename)
        Using FStream As New StreamReader(filename)
            Wdbg("I", "Stream to file {0} opened.", filename)
            While Not FStream.EndOfStream
                W(FStream.ReadLine, True, ColTypes.Neutral)
            End While
        End Using
    End Sub

    ''' <summary>
    ''' List all files and folders in a specified folder
    ''' </summary>
    ''' <param name="folder">Full path to folder</param>
    Public Sub List(ByVal folder As String)
        Wdbg("I", "Folder {0} will be listed...", folder)

#If NTFSCorruptionFix Then
        'Mitigate Windows 10 NTFS corruption or Windows 10 BSOD bug
        If IsOnWindows() And (folder.Contains("$i30") Or folder.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
            Wdbg("F", "Trying to access invalid path. Path was {0}", folder)
            Throw New ArgumentException(DoTranslation("Trying to access invalid path."))
        End If
#End If

        'List files and folders
        folder = NeutralizePath(folder)
        If Directory.Exists(folder) Then
            Dim enumeration As IEnumerable(Of String)
            Try
                enumeration = Directory.EnumerateFileSystemEntries(folder)
            Catch sex As Security.SecurityException
                W(DoTranslation("You are unauthorized to list in {0}: {1}"), True, ColTypes.Err, folder, sex.Message)
                W(DoTranslation("Permission {0} failed"), True, ColTypes.Err, sex.PermissionType)
                WStkTrc(sex)
                Exit Sub
            Catch ptlex As PathTooLongException
                W(DoTranslation("The path you've specified is too long."), True, ColTypes.Err)
                WStkTrc(ptlex)
                Exit Sub
            Catch ex As Exception
                W(DoTranslation("Unknown error while listing in directory: {0}"), True, ColTypes.Err, ex.Message)
                WStkTrc(ex)
                Exit Sub
            End Try
            W(">> {0}", True, ColTypes.Stage, folder)
            For Each Entry As String In enumeration
                Wdbg("I", "Enumerating {0}...", Entry)
                Try
                    If File.Exists(Entry) Then
                        Dim FInfo As New FileInfo(Entry)

                        'Print information
                        If (FInfo.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not FInfo.Attributes.HasFlag(FileAttributes.Hidden) Then
                            If FInfo.Name.EndsWith(".uesh") Then
                                W("- " + FInfo.Name + ": ", False, ColTypes.Stage)
                            Else
                                W("- " + FInfo.Name + ": ", False, ColTypes.ListEntry)
                            End If
                            W(DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), True, ColTypes.ListValue,
                              FInfo.Length.FileSizeToString, FInfo.CreationTime.ToShortDateString, FInfo.CreationTime.ToShortTimeString,
                                                             FInfo.LastWriteTime.ToShortDateString, FInfo.LastWriteTime.ToShortTimeString)
                        End If
                    ElseIf Directory.Exists(Entry) Then
                        Dim DInfo As New DirectoryInfo(Entry)

                        'Get all file sizes in a folder
                        Dim TotalSize As Long = GetAllSizesInFolder(DInfo)

                        'Print information
                        If (DInfo.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not DInfo.Attributes.HasFlag(FileAttributes.Hidden) Then
                            W("- " + DInfo.Name + "/: ", False, ColTypes.ListEntry)
                            W(DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), True, ColTypes.ListValue,
                              TotalSize.FileSizeToString, DInfo.CreationTime.ToShortDateString, DInfo.CreationTime.ToShortTimeString,
                                                          DInfo.LastWriteTime.ToShortDateString, DInfo.LastWriteTime.ToShortTimeString)
                        End If
                    End If
                Catch ex As UnauthorizedAccessException 'Error while getting info
                    Dim Directory As String = Entry.Replace("\", "/").Split("/")(Entry.Replace("\", "/").Split("/").Length - 1)
                    W("- " + DoTranslation("You are not authorized to get info for {0}."), True, ColTypes.Err, Directory)
                    WStkTrc(ex)
                End Try
            Next
        Else
            W(DoTranslation("Directory {0} not found"), True, ColTypes.Err, folder)
            Wdbg("I", "IO.Directory.Exists = {0}", Directory.Exists(folder))
        End If
    End Sub

    ''' <summary>
    ''' Simplifies the path to the correct one. It converts the path format to the unified format.
    ''' </summary>
    ''' <param name="Path">Target path, be it a file or a folder</param>
    ''' <returns>Absolute path</returns>
    Public Function NeutralizePath(ByVal Path As String, Optional ByVal Strict As Boolean = False) As String
#If NTFSCorruptionFix Then
        'Mitigate Windows 10 NTFS corruption or Windows 10 BSOD bug
        If IsOnWindows() And (Path.Contains("$i30") Or Path.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
            Throw New ArgumentException(DoTranslation("Trying to access invalid path."))
        End If
#End If

        'Replace backslashes with slashes if any.
        Path = Path.Replace("\", "/")

        'Append current directory to path
        If (IsOnWindows() And Not Path.Contains(":/")) Or (IsOnUnix() And Not Path.StartsWith("/")) Then
            Path = $"{CurrDir}/{Path}"
        End If

        'Replace last occurrences of current directory of path with nothing.
        If Not CurrDir = "" Then
            If Path.Contains(CurrDir.Replace("\", "/")) And Path.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
                Path = ReplaceLastOccurrence(Path, CurrDir, "")
            End If
        End If

        'If strict, checks for existence of file
        If Strict Then
            If File.Exists(Path) Then
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
    Public Function NeutralizePath(ByVal Path As String, ByVal Source As String)
#If NTFSCorruptionFix Then
        'Mitigate Windows 10 NTFS corruption or Windows 10 BSOD bug
        If IsOnWindows() And (Path.Contains("$i30") Or Path.Contains("\\.\globalroot\device\condrv\kernelconnect") Or
                              Source.Contains("$i30") Or Source.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
            Throw New ArgumentException(DoTranslation("Trying to access invalid path."))
        End If
#End If

        'Replace backslashes with slashes if any.
        Path = Path.Replace("\", "/")
        Source = Source.Replace("\", "/")

        'Append current directory to path
        If (IsOnWindows() And Not Path.Contains(":/")) Or (IsOnUnix() And Not Path.StartsWith("/")) Then
            Path = $"{Source}/{Path}"
        End If

        'Replace last occurrences of current directory of path with nothing.
        If Not Source = "" Then
            If Path.Contains(Source.Replace("\", "/")) And Path.AllIndexesOf(Source.Replace("\", "/")).Count > 1 Then
                Path = ReplaceLastOccurrence(Path, Source, "")
            End If
        End If

        Return Path
    End Function

    ''' <summary>
    ''' Copies a file or directory
    ''' </summary>
    ''' <param name="Source">Source file or directory</param>
    ''' <param name="Destination">Target file or directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function CopyFileOrDir(ByVal Source As String, ByVal Destination As String) As Boolean
        Try
            Source = NeutralizePath(Source)
            Wdbg("I", "Source directory: {0}", Source)
            Destination = NeutralizePath(Destination)
            Wdbg("I", "Target directory: {0}", Destination)
            Dim FileName As String = Path.GetFileName(Source)
            Wdbg("I", "Source file name: {0}", FileName)
            If Directory.Exists(Source) Then
                Wdbg("I", "Source and destination are directories")
                FileIO.FileSystem.CopyDirectory(Source, Destination, True) 'There is no IO.Directory.Copy yet.

                'Raise event
                EventManager.RaiseDirectoryCopied(Source, Destination)
                Return True
            ElseIf File.Exists(Source) And Directory.Exists(Destination) Then
                Wdbg("I", "Source is a file and destination is a directory")
                File.Copy(Source, Destination + "/" + FileName, True)

                'Raise event
                EventManager.RaiseFileCopied(Source, Destination + "/" + FileName)
                Return True
            ElseIf File.Exists(Source) Then
                Wdbg("I", "Source is a file and destination is a file")
                File.Copy(Source, Destination, True)

                'Raise event
                EventManager.RaiseFileCopied(Source, Destination)
                Return True
            Else
                Wdbg("E", "Source or destination are invalid.")
                Throw New IOException(DoTranslation("The path is neither a file nor a directory."))
            End If
        Catch ex As Exception
            Wdbg("E", "Failed to copy {0} to {1}: {2}", Source, Destination, ex.Message)
            WStkTrc(ex)
            Throw New IOException(DoTranslation("Failed to copy file or directory: {0}").FormatString(ex.Message))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Set size parse mode (whether to enable full size parse for directories or just the surface)
    ''' </summary>
    ''' <param name="Enable">To enable or to disable</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function SetSizeParseMode(ByVal Enable As Boolean) As Boolean
        Try
            FullParseMode = Enable
            Dim ksconf As New IniFile()
            Dim pathConfig As String = paths("Configuration")
            ksconf.Load(pathConfig)
            ksconf.Sections("Misc").Keys("Size parse mode").Value = FullParseMode
            ksconf.Save(pathConfig)
            Return True
        Catch ex As Exception
            Throw New IOException(DoTranslation("Error when trying to set parse mode. Check the value and try again. If this is correct, see the stack trace when kernel debugging is enabled.") + " " + ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Makes a directory
    ''' </summary>
    ''' <param name="NewDirectory">New directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="IOException"></exception>
    Public Function MakeDirectory(ByVal NewDirectory As String) As Boolean
        NewDirectory = NeutralizePath(NewDirectory)
        Wdbg("I", "New directory: {0} ({1})", NewDirectory, Directory.Exists(NewDirectory))
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
    Public Function MakeFile(ByVal NewFile As String) As Boolean
        NewFile = NeutralizePath(NewFile)
        Wdbg("I", "File path is {0} and .Exists is {0}", NewFile, File.Exists(NewFile))
        If Not File.Exists(NewFile) Then
            Try
                Dim NewFileStream As FileStream = File.Create(NewFile)
                Wdbg("I", "File created")
                NewFileStream.Close()
                Wdbg("I", "File closed")

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
    Public Function MoveFileOrDir(ByVal Source As String, ByVal Destination As String) As Boolean
        Try
            Source = NeutralizePath(Source)
            Wdbg("I", "Source directory: {0}", Source)
            Destination = NeutralizePath(Destination)
            Wdbg("I", "Target directory: {0}", Destination)
            Dim FileName As String = Path.GetFileName(Source)
            Wdbg("I", "Source file name: {0}", FileName)
            If Directory.Exists(Source) Then
                Wdbg("I", "Source and destination are directories")
                Directory.Move(Source, Destination)

                'Raise event
                EventManager.RaiseDirectoryMoved(Source, Destination)
                Return True
            ElseIf File.Exists(Source) And Directory.Exists(Destination) Then
                Wdbg("I", "Source is a file and destination is a directory")
                File.Move(Source, Destination + "/" + FileName)

                'Raise event
                EventManager.RaiseFileMoved(Source, Destination + "/" + FileName)
                Return True
            ElseIf File.Exists(Source) Then
                Wdbg("I", "Source is a file and destination is a file")
                File.Move(Source, Destination)

                'Raise event
                EventManager.RaiseFileMoved(Source, Destination)
                Return True
            Else
                Wdbg("E", "Source or destination are invalid.")
                Throw New IOException(DoTranslation("The path is neither a file nor a directory."))
            End If
        Catch ex As Exception
            Wdbg("E", "Failed to move {0} to {1}: {2}", Source, Destination, ex.Message)
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
    Public Function RemoveDirectory(ByVal Target As String) As Boolean
        Try
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
    Public Function RemoveFile(ByVal Target As String) As Boolean
        Try
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
    Public Function SearchFileForString(ByVal FilePath As String, ByVal StringLookup As String) As List(Of String)
        Try
            FilePath = NeutralizePath(FilePath)
            Dim Matches As New List(Of String)
            Dim Filebyte() As String = File.ReadAllLines(FilePath)
            Dim MatchNum As Integer = 1
            Dim LineNumber As Integer = 1
            For Each Str As String In Filebyte
                If Str.Contains(StringLookup) Then
                    Matches.Add("[{0}] ".FormatString(LineNumber) + DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str))
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
    Public Function RemoveAttribute(ByVal attributes As FileAttributes, ByVal attributesToRemove As FileAttributes) As FileAttributes
        Return attributes And (Not attributesToRemove)
    End Function

    ''' <summary>
    ''' Adds attribute to file
    ''' </summary>
    ''' <param name="FilePath">File path</param>
    ''' <param name="Attributes">Attributes</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function AddAttributeToFile(ByVal FilePath As String, ByVal Attributes As FileAttributes) As Boolean
        Try
            FilePath = NeutralizePath(FilePath)
            Wdbg("I", "Setting file attribute to {0}...", Attributes)
            File.SetAttributes(FilePath, Attributes)

            'Raise event
            EventManager.RaiseFileAttributeAdded(FilePath, Attributes)
            Return True
        Catch ex As Exception
            Wdbg("E", "Failed to add attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message)
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
    Public Function RemoveAttributeFromFile(ByVal FilePath As String, ByVal Attributes As FileAttributes) As Boolean
        Try
            FilePath = NeutralizePath(FilePath)
            Dim Attrib As FileAttributes = File.GetAttributes(FilePath)
            Wdbg("I", "File attributes: {0}", Attrib)
            Attrib = Attrib.RemoveAttribute(Attributes)
            Wdbg("I", "Setting file attribute to {0}...", Attrib)
            File.SetAttributes(FilePath, Attrib)

            'Raise event
            EventManager.RaiseFileAttributeRemoved(FilePath, Attributes)
            Return True
        Catch ex As Exception
            Wdbg("E", "Failed to remove attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Gets all file sizes in a folder, depending on the kernel setting <see cref="FullParseMode"/>
    ''' </summary>
    ''' <param name="DirectoryInfo">Directory information</param>
    ''' <returns>Directory Size</returns>
    Public Function GetAllSizesInFolder(ByVal DirectoryInfo As DirectoryInfo) As Long
        Return GetAllSizesInFolder(DirectoryInfo, FullParseMode)
    End Function

    ''' <summary>
    ''' Gets all file sizes in a folder, and optionally parses the entire folder
    ''' </summary>
    ''' <param name="DirectoryInfo">Directory information</param>
    ''' <returns>Directory Size</returns>
    Public Function GetAllSizesInFolder(ByVal DirectoryInfo As DirectoryInfo, ByVal FullParseMode As Boolean) As Long
        Dim Files As List(Of FileInfo)
        If FullParseMode Then
            Files = DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).ToList
        Else
            Files = DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).ToList
        End If
        Wdbg("I", "{0} files to be parsed", Files.Count)
        Dim TotalSize As Long = 0 'In bytes
        For Each DFile As FileInfo In Files
            If (DFile.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not DFile.Attributes.HasFlag(FileAttributes.Hidden) Then
                Wdbg("I", "File {0}, Size {1} bytes", DFile.Name, DFile.Length)
                TotalSize += DFile.Length
            End If
        Next
        Return TotalSize
    End Function

    ''' <summary>
    ''' Opens a file, reads all lines, and returns the array of lines
    ''' </summary>
    ''' <param name="path">Path to file</param>
    ''' <returns>Array of lines</returns>
    Public Function ReadAllLinesNoBlock(ByVal path As String) As String()
#If NTFSCorruptionFix Then
        'Mitigate Windows 10 NTFS corruption or Windows 10 BSOD bug
        If IsOnWindows() And (path.Contains("$i30") Or path.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
            Wdbg("F", "Trying to access invalid path. Path was {0}", path)
            Throw New ArgumentException(DoTranslation("Trying to access invalid path."))
        End If
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

End Module
