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

Module SFTPFilesystem

    ''' <summary>
    ''' Lists remote folders and files
    ''' </summary>
    ''' <param name="Path">Path to folder</param>
    ''' <returns>The list if successful; null if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.SFTPFilesystemException"></exception>
    ''' <exception cref="InvalidOperationException"></exception>
    Public Function SFTPListRemote(ByVal Path As String) As List(Of String)
        If SFTPConnected Then
            Dim EntryBuilder As New StringBuilder
            Dim Entries As New List(Of String)
            Dim FileSize As Long
            Dim ModDate As Date
            Dim Listing As IEnumerable(Of Sftp.SftpFile)

            Try
                If Path <> "" Then
                    Listing = ClientSFTP.ListDirectory(Path)
                Else
                    Listing = ClientSFTP.ListDirectory(SFTPCurrentRemoteDir)
                End If
                For Each DirListSFTP As Sftp.SftpFile In Listing
                    EntryBuilder.Append($"- {DirListSFTP.Name}")
                    If DirListSFTP.IsRegularFile Then
                        EntryBuilder.Append(": ")
                        FileSize = DirListSFTP.Length
                        ModDate = DirListSFTP.LastWriteTime
                        EntryBuilder.Append(DoTranslation("{0} KB | Modified in: {1}", currentLang).FormatString(FormatNumber(FileSize / 1024, 2), ModDate.ToString))
                    ElseIf DirListSFTP.IsDirectory Then
                        EntryBuilder.Append("/")
                    ElseIf DirListSFTP.IsSymbolicLink Then
                        'TODO: Find a way to get what it's linked to.
                        EntryBuilder.Append(">>")
                    End If
                    Entries.Add(EntryBuilder.ToString)
                    EntryBuilder.Clear()
                Next
                Return Entries
            Catch ex As Exception
                WStkTrc(ex)
                Throw New EventsAndExceptions.SFTPFilesystemException(DoTranslation("Failed to list remote files: {0}", currentLang).FormatString(ex.Message))
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
    ''' <exception cref="EventsAndExceptions.SFTPFilesystemException"></exception>
    Public Function SFTPDeleteRemote(ByVal Target As String) As Boolean
        If SFTPConnected Then
            Wdbg("I", "Deleting {0}...", Target)

            'Delete a file or folder
            If ClientSFTP.Exists(Target) Then
                Wdbg("I", "Deleting {0}...", Target)
                ClientSFTP.Delete(Target)
            Else
                Wdbg("E", "{0} is not found.", Target)
                Throw New EventsAndExceptions.SFTPFilesystemException(DoTranslation("{0} is not found in the server.", currentLang).FormatString(Target))
                Return False
            End If
            Wdbg("I", "Deleted {0}", Target)
            Return True
        Else
            Throw New EventsAndExceptions.SFTPFilesystemException(DoTranslation("You must connect to server with administrative privileges before performing the deletion.", currentLang))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Changes FTP remote directory
    ''' </summary>
    ''' <param name="Directory">Remote directory</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.SFTPFilesystemException"></exception>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="ArgumentNullException"></exception>
    Public Function SFTPChangeRemoteDir(ByVal Directory As String) As Boolean
        If SFTPConnected = True Then
            If Directory <> "" Then
                If ClientSFTP.Exists(Directory) Then
                    'Directory exists, go to the new directory
                    ClientSFTP.ChangeDirectory(Directory)
                    currentremoteDir = ClientFTP.GetWorkingDirectory
                    Return True
                Else
                    'Directory doesn't exist, go to the old directory
                    Throw New EventsAndExceptions.SFTPFilesystemException(DoTranslation("Directory {0} not found.", currentLang).FormatString(Directory))
                End If
            Else
                Throw New ArgumentNullException(DoTranslation("Enter a remote directory. "".."" to go back", currentLang))
            End If
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to a server before changing directory", currentLang))
        End If
        Return False
    End Function

    Public Function SFTPChangeLocalDir(ByVal Directory As String) As Boolean
        If Directory <> "" Then
            'Check if folder exists
            Dim targetDir As String
            targetDir = $"{currDirect}/{Directory}"
            If IO.Directory.Exists(targetDir) Then
                'Parse written directory
                Dim parser As New IO.DirectoryInfo(targetDir)
                currDirect = parser.FullName
                Return True
            Else
                Throw New EventsAndExceptions.SFTPFilesystemException(DoTranslation("Local directory {0} doesn't exist.", currentLang).FormatString(Directory))
            End If
        Else
            Throw New ArgumentNullException(DoTranslation("Enter a local directory. "".."" to go back.", currentLang))
        End If
        Return False
    End Function

End Module
