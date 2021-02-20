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
Imports System.Text

Module FTPFilesystem

    ''' <summary>
    ''' Lists remote folders and files
    ''' </summary>
    ''' <param name="Path">Path to folder</param>
    ''' <returns>The list if successful; null if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.FTPFilesystemException"></exception>
    ''' <exception cref="InvalidOperationException"></exception>
    Public Function FTPListRemote(ByVal Path As String) As List(Of String)
        If connected Then
            Dim EntryBuilder As New StringBuilder
            Dim Entries As New List(Of String)
            Dim FileSize As Long
            Dim ModDate As Date
            Dim Listing As FtpListItem()

            Try
                If Path <> "" Then
                    Listing = ClientFTP.GetListing(Path)
                Else
                    Listing = ClientFTP.GetListing(currentremoteDir)
                End If
                For Each DirListFTP As FtpListItem In Listing
                    EntryBuilder.Append($"- {DirListFTP.Name}")
                    If DirListFTP.Type = FtpFileSystemObjectType.File Then
                        EntryBuilder.Append(": ")
                        FileSize = ClientFTP.GetFileSize(DirListFTP.FullName)
                        ModDate = ClientFTP.GetModifiedTime(DirListFTP.FullName)
                        EntryBuilder.Append(DoTranslation("{0} KB | Modified in: {1}", currentLang).FormatString(FormatNumber(FileSize / 1024, 2), ModDate.ToString))
                    ElseIf DirListFTP.Type = FtpFileSystemObjectType.Directory Then
                        EntryBuilder.Append("/")
                    ElseIf DirListFTP.Type = FtpFileSystemObjectType.Link Then
                        EntryBuilder.Append(" >> ")
                        EntryBuilder.Append(ClientFTP.DereferenceLink(DirListFTP).FullName)
                    End If
                    Entries.Add(EntryBuilder.ToString)
                    EntryBuilder.Clear()
                Next
                Return Entries
            Catch ex As Exception
                WStkTrc(ex)
                Throw New EventsAndExceptions.FTPFilesystemException(DoTranslation("Failed to list remote files: {0}", currentLang).FormatString(ex.Message))
            End Try
        Else
            Throw New InvalidOperationException(DoTranslation("You should connect to server before listing all remote files.", currentLang))
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Removes remote file or folder
    ''' </summary>
    ''' <param name="Target">Target folder or file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.FTPFilesystemException"></exception>
    Public Function FTPDeleteRemote(ByVal Target As String) As Boolean
        If connected Then
            Wdbg("I", "Deleting {0}...", Target)

            'Delete a file or folder
            If ClientFTP.FileExists(Target) Then
                Wdbg("I", "{0} is a file.", Target)
                ClientFTP.DeleteFile(Target)
            ElseIf ClientFTP.DirectoryExists(Target) Then
                Wdbg("I", "{0} is a folder.", Target)
                ClientFTP.DeleteDirectory(Target)
            Else
                Wdbg("E", "{0} is not found.", Target)
                Throw New EventsAndExceptions.FTPFilesystemException(DoTranslation("{0} is not found in the server.", currentLang).FormatString(Target))
                Return False
            End If
            Wdbg("I", "Deleted {0}", Target)
            Return True
        Else
            Throw New EventsAndExceptions.FTPFilesystemException(DoTranslation("You must connect to server with administrative privileges before performing the deletion.", currentLang))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Changes FTP remote directory
    ''' </summary>
    ''' <param name="Directory">Remote directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.FTPFilesystemException"></exception>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="ArgumentNullException"></exception>
    Public Function FTPChangeRemoteDir(ByVal Directory As String) As Boolean
        If connected = True Then
            If Directory <> "" Then
                If ClientFTP.DirectoryExists(Directory) Then
                    'Directory exists, go to the new directory
                    ClientFTP.SetWorkingDirectory(Directory)
                    currentremoteDir = ClientFTP.GetWorkingDirectory
                    Return True
                Else
                    'Directory doesn't exist, go to the old directory
                    Throw New EventsAndExceptions.FTPFilesystemException(DoTranslation("Directory {0} not found.", currentLang).FormatString(Directory))
                End If
            Else
                Throw New ArgumentNullException(DoTranslation("Enter a remote directory. "".."" to go back", currentLang))
            End If
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to a server before changing directory", currentLang))
        End If
        Return False
    End Function

    Public Function FTPChangeLocalDir(ByVal Directory As String) As Boolean
        If Directory <> "" Then
            Dim targetDir As String
            targetDir = $"{currDirect}/{Directory}"

#If NTFSCorruptionFix Then
            'Mitigate Windows 10 NTFS corruption or Windows 10 BSOD bug
            If IsOnWindows() And (targetDir.Contains("$i30") Or targetDir.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
                Wdbg("F", "Trying to access invalid path. Path was {0}", targetDir)
                Throw New ArgumentException(DoTranslation("Trying to access invalid path.", currentLang))
            End If
#End If

            'Check if folder exists
            If IO.Directory.Exists(targetDir) Then
                'Parse written directory
                Dim parser As New IO.DirectoryInfo(targetDir)
                currDirect = parser.FullName
                Return True
            Else
                Throw New EventsAndExceptions.FTPFilesystemException(DoTranslation("Local directory {0} doesn't exist.", currentLang).FormatString(Directory))
            End If
        Else
            Throw New ArgumentNullException(DoTranslation("Enter a local directory. "".."" to go back.", currentLang))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Move file or directory to another area, or rename the file
    ''' </summary>
    ''' <param name="Source">Source file or folder</param>
    ''' <param name="Target">Target file or folder</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    Public Function FTPMoveItem(ByVal Source As String, ByVal Target As String) As Boolean
        If connected Then
            Dim Success As Boolean

            'Begin the moving process
            Dim SourceFile As String = Source.Split("/").Last
            Wdbg("I", "Moving from {0} to {1} with the source file of {2}...", Source, Target, SourceFile)
            If ClientFTP.DirectoryExists(Source) Then
                Success = ClientFTP.MoveDirectory(Source, Target)
            ElseIf ClientFTP.FileExists(Source) And ClientFTP.DirectoryExists(Target) Then
                Success = ClientFTP.MoveFile(Source, Target + SourceFile)
            ElseIf ClientFTP.FileExists(Source) Then
                Success = ClientFTP.MoveFile(Source, Target)
            End If
            Wdbg("I", "Moved. Result: {0}", Success)
            Return Success
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission.", currentLang))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Copy file or directory to another area, or rename the file
    ''' </summary>
    ''' <param name="Source">Source file or folder</param>
    ''' <param name="Target">Target file or folder</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    Public Function FTPCopyItem(ByVal Source As String, ByVal Target As String) As Boolean
        If connected Then
            Dim Success As Boolean = True
            Dim Result As Object

            'Begin the copying process
            'TODO: FluentFTP currently doesn't support .CopyFile and .CopyDirectory
            Dim SourceFile As String = Source.Split("/").Last
            Wdbg("I", "Copying from {0} to {1} with the source file of {2}...", Source, Target, SourceFile)
            If ClientFTP.DirectoryExists(Source) Then
                ClientFTP.DownloadDirectory(paths("Temp") + "/FTPTransfer", Source)
                Result = ClientFTP.UploadDirectory(paths("Temp") + "/FTPTransfer/" + Source, Target)
            ElseIf ClientFTP.FileExists(Source) And ClientFTP.DirectoryExists(Target) Then
                ClientFTP.DownloadFile(paths("Temp") + "/FTPTransfer/" + SourceFile, Source)
                Result = ClientFTP.UploadFile(paths("Temp") + "/FTPTransfer/" + SourceFile, Target + "/" + SourceFile)
            ElseIf ClientFTP.FileExists(Source) Then
                ClientFTP.DownloadFile(paths("Temp") + "/FTPTransfer/" + SourceFile, Source)
                Result = ClientFTP.UploadFile(paths("Temp") + "/FTPTransfer/" + SourceFile, Target)
            End If
            Directory.Delete(paths("Temp") + "/FTPTransfer", True)

            'See if copied successfully
#Disable Warning BC42104
            If Result.GetType = GetType(List(Of FtpResult)) Then
                For Each FileResult As FtpResult In Result
                    If FileResult.IsFailed Then
                        Wdbg("E", "Transfer for {0} failed: {1}", FileResult.Name, FileResult.Exception.Message)
                        WStkTrc(FileResult.Exception)
                        Success = False
                    End If
                Next
            ElseIf Result.GetType = GetType(FtpStatus) Then
                If CType(Result, FtpStatus).IsFailure Then
                    Wdbg("E", "Transfer failed")
                    Success = False
                End If
            End If
#Enable Warning BC42104
            Wdbg("I", "Copied. Result: {0}", Success)
            Return Success
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission.", currentLang))
        End If
        Return False
    End Function

End Module
