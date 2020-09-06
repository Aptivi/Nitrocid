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

End Module
