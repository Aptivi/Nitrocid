
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

Imports KS.Files.Querying
Imports System.IO
Imports System.Text

Namespace Network.FTP.Filesystem
    Public Module FTPFilesystem

        ''' <summary>
        ''' Lists remote folders and files
        ''' </summary>
        ''' <param name="Path">Path to folder</param>
        ''' <returns>The list if successful; null if unsuccessful</returns>
        ''' <exception cref="Exceptions.FTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        Public Function FTPListRemote(Path As String) As List(Of String)
            FTPListRemote(Path, FtpShowDetailsInList)
        End Function

        ''' <summary>
        ''' Lists remote folders and files
        ''' </summary>
        ''' <param name="Path">Path to folder</param>
        ''' <param name="ShowDetails">Shows the details of the file</param>
        ''' <returns>The list if successful; null if unsuccessful</returns>
        ''' <exception cref="Exceptions.FTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        Public Function FTPListRemote(Path As String, ShowDetails As Boolean) As List(Of String)
            If FtpConnected Then
                Dim EntryBuilder As New StringBuilder
                Dim Entries As New List(Of String)
                Dim FileSize As Long
                Dim ModDate As Date
                Dim Listing As FtpListItem()

                Try
                    If Path <> "" Then
                        Listing = ClientFTP.GetListing(Path, FtpListOption.DerefLinks)
                    Else
                        Listing = ClientFTP.GetListing(FtpCurrentRemoteDir, FtpListOption.DerefLinks)
                    End If
                    For Each DirListFTP As FtpListItem In Listing
                        EntryBuilder.Append($"- {DirListFTP.Name}")
                        'Check to see if the file that we're dealing with is a symbolic link
                        If DirListFTP.Type = FtpObjectType.Link Then
                            EntryBuilder.Append(" >> ")
                            EntryBuilder.Append(DirListFTP.LinkTarget)
                            DirListFTP = DirListFTP.LinkObject
                        End If

                        If DirListFTP IsNot Nothing Then
                            If DirListFTP.Type = FtpObjectType.File Then
                                If ShowDetails Then
                                    EntryBuilder.Append(": ")
                                    FileSize = ClientFTP.GetFileSize(DirListFTP.FullName)
                                    ModDate = ClientFTP.GetModifiedTime(DirListFTP.FullName)
                                    EntryBuilder.Append(ListValueColor.VTSequenceForeground + DoTranslation("{0} KB | Modified in: {1}").FormatString((FileSize / 1024).ToString("N2"), ModDate.ToString))
                                End If
                            ElseIf DirListFTP.Type = FtpObjectType.Directory Then
                                EntryBuilder.Append("/")
                            End If
                        End If
                        Entries.Add(EntryBuilder.ToString)
                        EntryBuilder.Clear()
                    Next
                    Return Entries
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.FTPFilesystemException(DoTranslation("Failed to list remote files: {0}"), ex, ex.Message)
                End Try
            Else
                Throw New InvalidOperationException(DoTranslation("You should connect to server before listing all remote files."))
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Removes remote file or folder
        ''' </summary>
        ''' <param name="Target">Target folder or file</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.FTPFilesystemException"></exception>
        Public Function FTPDeleteRemote(Target As String) As Boolean
            If FtpConnected Then
                Wdbg(DebugLevel.I, "Deleting {0}...", Target)

                'Delete a file or folder
                If ClientFTP.FileExists(Target) Then
                    Wdbg(DebugLevel.I, "{0} is a file.", Target)
                    ClientFTP.DeleteFile(Target)
                ElseIf ClientFTP.DirectoryExists(Target) Then
                    Wdbg(DebugLevel.I, "{0} is a folder.", Target)
                    ClientFTP.DeleteDirectory(Target)
                Else
                    Wdbg(DebugLevel.E, "{0} is not found.", Target)
                    Throw New Exceptions.FTPFilesystemException(DoTranslation("{0} is not found in the server."), Target)
                    Return False
                End If
                Wdbg(DebugLevel.I, "Deleted {0}", Target)
                Return True
            Else
                Throw New Exceptions.FTPFilesystemException(DoTranslation("You must connect to server with administrative privileges before performing the deletion."))
            End If
            Return False
        End Function

        ''' <summary>
        ''' Changes FTP remote directory
        ''' </summary>
        ''' <param name="Directory">Remote directory</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.FTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        ''' <exception cref="ArgumentNullException"></exception>
        Public Function FTPChangeRemoteDir(Directory As String) As Boolean
            If FtpConnected = True Then
                If Directory <> "" Then
                    If ClientFTP.DirectoryExists(Directory) Then
                        'Directory exists, go to the new directory
                        ClientFTP.SetWorkingDirectory(Directory)
                        FtpCurrentRemoteDir = ClientFTP.GetWorkingDirectory
                        Return True
                    Else
                        'Directory doesn't exist, go to the old directory
                        Throw New Exceptions.FTPFilesystemException(DoTranslation("Directory {0} not found."), Directory)
                    End If
                Else
                    Throw New ArgumentNullException(Directory, DoTranslation("Enter a remote directory. "".."" to go back"))
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to a server before changing directory"))
            End If
            Return False
        End Function

        Public Function FTPChangeLocalDir(Directory As String) As Boolean
            If Directory <> "" Then
                Dim targetDir As String
                targetDir = $"{FtpCurrentDirectory}/{Directory}"
                ThrowOnInvalidPath(targetDir)

                'Check if folder exists
                If FolderExists(targetDir) Then
                    'Parse written directory
                    Dim parser As New IO.DirectoryInfo(targetDir)
                    FtpCurrentDirectory = parser.FullName
                    Return True
                Else
                    Throw New Exceptions.FTPFilesystemException(DoTranslation("Local directory {0} doesn't exist."), Directory)
                End If
            Else
                Throw New ArgumentNullException(Directory, DoTranslation("Enter a local directory. "".."" to go back."))
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
        Public Function FTPMoveItem(Source As String, Target As String) As Boolean
            If FtpConnected Then
                Dim Success As Boolean

                'Begin the moving process
                Dim SourceFile As String = Source.Split("/").Last
                Wdbg(DebugLevel.I, "Moving from {0} to {1} with the source file of {2}...", Source, Target, SourceFile)
                If ClientFTP.DirectoryExists(Source) Then
                    Success = ClientFTP.MoveDirectory(Source, Target)
                ElseIf ClientFTP.FileExists(Source) And ClientFTP.DirectoryExists(Target) Then
                    Success = ClientFTP.MoveFile(Source, Target + SourceFile)
                ElseIf ClientFTP.FileExists(Source) Then
                    Success = ClientFTP.MoveFile(Source, Target)
                End If
                Wdbg(DebugLevel.I, "Moved. Result: {0}", Success)
                Return Success
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
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
        Public Function FTPCopyItem(Source As String, Target As String) As Boolean
            If FtpConnected Then
                Dim Success As Boolean = True
                Dim Result As Object

                'Begin the copying process
                Dim SourceFile As String = Source.Split("/").Last
                Wdbg(DebugLevel.I, "Copying from {0} to {1} with the source file of {2}...", Source, Target, SourceFile)
                If ClientFTP.DirectoryExists(Source) Then
                    ClientFTP.DownloadDirectory(TempPath + "/FTPTransfer", Source)
                    Result = ClientFTP.UploadDirectory(TempPath + "/FTPTransfer/" + Source, Target)
                ElseIf ClientFTP.FileExists(Source) And ClientFTP.DirectoryExists(Target) Then
                    ClientFTP.DownloadFile(TempPath + "/FTPTransfer/" + SourceFile, Source)
                    Result = ClientFTP.UploadFile(TempPath + "/FTPTransfer/" + SourceFile, Target + "/" + SourceFile)
                ElseIf ClientFTP.FileExists(Source) Then
                    ClientFTP.DownloadFile(TempPath + "/FTPTransfer/" + SourceFile, Source)
                    Result = ClientFTP.UploadFile(TempPath + "/FTPTransfer/" + SourceFile, Target)
                End If
                Directory.Delete(TempPath + "/FTPTransfer", True)

                'See if copied successfully
#Disable Warning BC42104
                If Result.GetType = GetType(List(Of FtpResult)) Then
                    For Each FileResult As FtpResult In Result
                        If FileResult.IsFailed Then
                            Wdbg(DebugLevel.E, "Transfer for {0} failed: {1}", FileResult.Name, FileResult.Exception.Message)
                            WStkTrc(FileResult.Exception)
                            Success = False
                        End If
                    Next
                ElseIf Result.GetType = GetType(FtpStatus) Then
                    If CType(Result, FtpStatus).IsFailure Then
                        Wdbg(DebugLevel.E, "Transfer failed")
                        Success = False
                    End If
                End If
#Enable Warning BC42104
                Wdbg(DebugLevel.I, "Copied. Result: {0}", Success)
                Return Success
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
            End If
            Return False
        End Function

        ''' <summary>
        ''' Changes the permissions of a remote file
        ''' </summary>
        ''' <param name="Target">Target file</param>
        ''' <param name="Chmod">Permissions in CHMOD format. See https://man7.org/linux/man-pages/man2/chmod.2.html chmod(2) for more info.</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function FTPChangePermissions(Target As String, Chmod As Integer) As Boolean
            If FtpConnected Then
                Try
                    ClientFTP.Chmod(Target, Chmod)
                    Return True
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Error setting permissions ({0}) to file {1}: {2}", Chmod, Target, ex.Message)
                    WStkTrc(ex)
                End Try
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to server before performing filesystem operations."))
            End If
            Return False
        End Function

    End Module
End Namespace
