
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

Public Module SFTPTransfer

    ''' <summary>
    ''' Downloads a file from the currently connected SFTP server
    ''' </summary>
    ''' <param name="File">A remote file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SFTPGetFile(File As String) As Boolean
        If SFTPConnected Then
            Try
                'Show a message to download
                EventManager.RaiseSFTPPreDownload(File)
                Wdbg("I", "Downloading file {0}...", File)

                'Try to download
                Dim DownloadFileStream As New IO.FileStream($"{SFTPCurrDirect}/{File}", IO.FileMode.OpenOrCreate)
                ClientSFTP.DownloadFile($"{SFTPCurrentRemoteDir}/{File}", DownloadFileStream)

                'Show a message that it's downloaded
                Wdbg("I", "Downloaded file {0}.", File)
                EventManager.RaiseSFTPPostDownload(File)
                Return True
            Catch ex As Exception
                Wdbg("E", "Download failed for file {0}: {1}", File, ex.Message)
                EventManager.RaiseSFTPDownloadError(File, ex)
            End Try
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Uploads a file to the currently connected SFTP server
    ''' </summary>
    ''' <param name="File">A local file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SFTPUploadFile(File As String) As Boolean
        If SFTPConnected Then
            Try
                'Show a message to download
                EventManager.RaiseSFTPPreUpload(File)
                Wdbg("I", "Uploading file {0}...", File)

                'Try to upload
                Dim UploadFileStream As New IO.FileStream($"{SFTPCurrDirect}/{File}", IO.FileMode.Open)
                ClientSFTP.UploadFile(UploadFileStream, $"{SFTPCurrentRemoteDir}/{File}")
                Wdbg("I", "Uploaded file {0}", File)
                EventManager.RaiseSFTPPostUpload(File)
                Return True
            Catch ex As Exception
                Wdbg("E", "Upload failed for file {0}: {1}", File, ex.Message)
                EventManager.RaiseSFTPUploadError(File, ex)
            End Try
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

End Module
