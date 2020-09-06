'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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
            End If
            Wdbg("I", "Deleted {0}", Target)
            Return True
        Else
            Throw New EventsAndExceptions.FTPFilesystemException(DoTranslation("You must connect to server with administrative privileges before performing the deletion.", currentLang))
        End If
        Return False
    End Function

End Module
