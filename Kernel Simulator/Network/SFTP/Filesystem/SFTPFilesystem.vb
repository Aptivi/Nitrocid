
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
Imports KS.Shell.Shells.SFTP
Imports System.Reflection
Imports System.Text

Namespace Network.SFTP.Filesystem
    Module SFTPFilesystem

        ''' <summary>
        ''' Lists remote folders and files
        ''' </summary>
        ''' <param name="Path">Path to folder</param>
        ''' <returns>The list if successful; null if unsuccessful</returns>
        ''' <exception cref="Exceptions.SFTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        Public Function SFTPListRemote(Path As String) As List(Of String)
            SFTPListRemote(Path, SFTPShowDetailsInList)
        End Function

        ''' <summary>
        ''' Lists remote folders and files
        ''' </summary>
        ''' <param name="Path">Path to folder</param>
        ''' <param name="ShowDetails">Shows the details of the file</param>
        ''' <returns>The list if successful; null if unsuccessful</returns>
        ''' <exception cref="Exceptions.SFTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        Public Function SFTPListRemote(Path As String, ShowDetails As Boolean) As List(Of String)
            If SFTPConnected Then
                Dim EntryBuilder As New StringBuilder
                Dim Entries As New List(Of String)
                Dim FileSize As Long
                Dim ModDate As Date
                Dim Listing As IEnumerable(Of Renci.SshNet.Sftp.SftpFile)

                Try
                    If Path <> "" Then
                        Listing = ClientSFTP.ListDirectory(Path)
                    Else
                        Listing = ClientSFTP.ListDirectory(SFTPCurrentRemoteDir)
                    End If
                    For Each DirListSFTP As Renci.SshNet.Sftp.SftpFile In Listing
                        EntryBuilder.Append($"- {DirListSFTP.Name}")
                        'Check to see if the file that we're dealing with is a symbolic link
                        If DirListSFTP.IsSymbolicLink Then
                            EntryBuilder.Append(" >> ")
                            EntryBuilder.Append(SFTPGetCanonicalPath(DirListSFTP.FullName))
                        End If

                        If DirListSFTP.IsRegularFile Then
                            EntryBuilder.Append(": ")
                            If ShowDetails Then
                                FileSize = DirListSFTP.Length
                                ModDate = DirListSFTP.LastWriteTime
                                EntryBuilder.Append(ListValueColor.VTSequenceForeground + DoTranslation("{0} KB | Modified in: {1}").FormatString((FileSize / 1024).ToString("N2"), ModDate.ToString))
                            End If
                        ElseIf DirListSFTP.IsDirectory Then
                            EntryBuilder.Append("/")
                        End If
                        Entries.Add(EntryBuilder.ToString)
                        EntryBuilder.Clear()
                    Next
                    Return Entries
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.SFTPFilesystemException(DoTranslation("Failed to list remote files: {0}"), ex, ex.Message)
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
        ''' <exception cref="Exceptions.SFTPFilesystemException"></exception>
        Public Function SFTPDeleteRemote(Target As String) As Boolean
            If SFTPConnected Then
                Wdbg(DebugLevel.I, "Deleting {0}...", Target)

                'Delete a file or folder
                If ClientSFTP.Exists(Target) Then
                    Wdbg(DebugLevel.I, "Deleting {0}...", Target)
                    ClientSFTP.Delete(Target)
                Else
                    Wdbg(DebugLevel.E, "{0} is not found.", Target)
                    Throw New Exceptions.SFTPFilesystemException(DoTranslation("{0} is not found in the server."), Target)
                    Return False
                End If
                Wdbg(DebugLevel.I, "Deleted {0}", Target)
                Return True
            Else
                Throw New Exceptions.SFTPFilesystemException(DoTranslation("You must connect to server with administrative privileges before performing the deletion."))
            End If
            Return False
        End Function

        ''' <summary>
        ''' Changes FTP remote directory
        ''' </summary>
        ''' <param name="Directory">Remote directory</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.SFTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        ''' <exception cref="ArgumentNullException"></exception>
        Public Function SFTPChangeRemoteDir(Directory As String) As Boolean
            If SFTPConnected = True Then
                If Directory <> "" Then
                    If ClientSFTP.Exists(Directory) Then
                        'Directory exists, go to the new directory
                        ClientSFTP.ChangeDirectory(Directory)
                        SFTPCurrentRemoteDir = ClientSFTP.WorkingDirectory
                        Return True
                    Else
                        'Directory doesn't exist, go to the old directory
                        Throw New Exceptions.SFTPFilesystemException(DoTranslation("Directory {0} not found."), Directory)
                    End If
                Else
                    Throw New ArgumentNullException(Directory, DoTranslation("Enter a remote directory. "".."" to go back"))
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to a server before changing directory"))
            End If
            Return False
        End Function

        Public Function SFTPChangeLocalDir(Directory As String) As Boolean
            If Directory <> "" Then
                Dim targetDir As String
                targetDir = $"{SFTPCurrDirect}/{Directory}"
                ThrowOnInvalidPath(targetDir)

                'Check if folder exists
                If FolderExists(targetDir) Then
                    'Parse written directory
                    Dim parser As New IO.DirectoryInfo(targetDir)
                    SFTPCurrDirect = parser.FullName
                    Return True
                Else
                    Throw New Exceptions.SFTPFilesystemException(DoTranslation("Local directory {0} doesn't exist."), Directory)
                End If
            Else
                Throw New ArgumentNullException(Directory, DoTranslation("Enter a local directory. "".."" to go back."))
            End If
            Return False
        End Function

        ''' <summary>
        ''' Gets the absolute path for the given path
        ''' </summary>
        ''' <param name="Path">The remote path</param>
        ''' <returns>Absolute path for a remote path</returns>
        Public Function SFTPGetCanonicalPath(Path As String) As String
            If SFTPConnected Then
                'GetCanonicalPath was supposed to be public, but it's in a private class called SftpSession. It should be in SftpClient, which is public.
                Dim SFTPType As Type = ClientSFTP.GetType
                Dim SFTPSessionField As FieldInfo = SFTPType.GetField("_sftpSession", BindingFlags.Instance Or BindingFlags.NonPublic)
                Dim SFTPSession As Object = SFTPSessionField.GetValue(ClientSFTP)
                Dim SFTPSessionType As Type = SFTPSession.GetType
                Dim SFTPSessionCanon As MethodInfo = SFTPSessionType.GetMethod("GetCanonicalPath")
                Dim CanonicalPath As String = SFTPSessionCanon.Invoke(SFTPSession, New String() {Path})
                Wdbg(DebugLevel.I, "Canonical path: {0}", CanonicalPath)
                Return CanonicalPath
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to server before performing filesystem operations."))
            End If
        End Function

    End Module
End Namespace
