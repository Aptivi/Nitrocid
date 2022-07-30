
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

Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Handlers
Imports System.Threading
Imports System.Threading.Tasks
Imports KS.Files.Folders
Imports KS.Misc.Notifications

Namespace Network.Transfer
    Public Module NetworkTransfer

        Public DownloadPercentagePrint As String = ""
        Public UploadPercentagePrint As String = ""
        Public DownloadNotificationProvoke As Boolean
        Public UploadNotificationProvoke As Boolean
        Friend IsError As Boolean
        Friend ReasonError As Exception
        Friend CancellationToken As New CancellationTokenSource
        Friend WClientProgress As New ProgressMessageHandler(New HttpClientHandler)
        Friend WClient As New HttpClient(WClientProgress)
        Friend DownloadedString As String
        Friend DownloadNotif As Notification
        Friend UploadNotif As Notification
        Friend SuppressDownloadMessage As Boolean
        Friend SuppressUploadMessage As Boolean

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String) As Boolean
            Return DownloadFile(URL, ShowProgress)
        End Function

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String, ShowProgress As Boolean) As Boolean
            Dim FileName As String = GetFilenameFromUrl(URL)
            Return DownloadFile(URL, ShowProgress, FileName)
        End Function

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="FileName">File name to download to</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String, FileName As String) As Boolean
            Return DownloadFile(URL, ShowProgress, FileName)
        End Function

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <param name="FileName">File name to download to</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String, ShowProgress As Boolean, FileName As String) As Boolean
            'Intialize variables
            Dim FileUri As New Uri(URL)

            'Initialize the progress bar indicator and the file completed event handler
            If DownloadNotificationProvoke Then
                DownloadNotif = New Notification(DoTranslation("Downloading..."), FileUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(DownloadNotif)
            End If
            If ShowProgress Then AddHandler WClientProgress.HttpReceiveProgress, AddressOf HttpReceiveProgressWatch

            'Send the GET request to the server for the file
            Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDir)
            Dim Response As HttpResponseMessage = WClient.GetAsync(FileUri, CancellationToken.Token).Result
            Response.EnsureSuccessStatusCode()

            'Get the file stream
            Dim FilePath As String = NeutralizePath(FileName)
            Dim FileStream As New FileStream(FilePath, FileMode.Create, FileAccess.Write)

            'Try to download the file asynchronously
            Task.Run(Sub()
                         Try
                             Response.Content.ReadAsStreamAsync.Result.CopyTo(FileStream)
                             FileStream.Flush()
                             FileStream.Close()
                             DownloadChecker(Nothing)
                         Catch ex As Exception
                             DownloadChecker(ex)
                         End Try
                     End Sub, CancellationToken.Token)
            While Not TransferFinished
                If CancelRequested Then
                    TransferFinished = True
                    CancellationToken.Cancel()
                End If
            End While

            'We're done downloading. Check to see if it's actually an error
            TransferFinished = False
            If ShowProgress And Not SuppressDownloadMessage Then Console.WriteLine()
            SuppressDownloadMessage = False
            If IsError Then
                If DownloadNotificationProvoke Then DownloadNotif.ProgressFailed = True
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                If DownloadNotificationProvoke Then DownloadNotif.Progress = 100
                Return True
            End If
        End Function

        ''' <summary>
        ''' Uploads a file to the current working directory.
        ''' </summary>
        ''' <param name="FileName">A target file name. Use <see cref="NeutralizePath(String, Boolean)"/> to get full path of source.</param>
        ''' <param name="URL">A URL to a file</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function UploadFile(FileName As String, URL As String) As Boolean
            Return UploadFile(FileName, URL, ShowProgress)
        End Function

        ''' <summary>
        ''' Uploads a file from the current working directory.
        ''' </summary>
        ''' <param name="FileName">A target file name. Use <see cref="NeutralizePath(String, Boolean)"/> to get full path of source.</param>
        ''' <param name="URL">A URL</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function UploadFile(FileName As String, URL As String, ShowProgress As Boolean) As Boolean
            'Intialize variables
            Dim FileUri As New Uri(URL)

            'Initialize the progress bar indicator and the file completed event handler
            If UploadNotificationProvoke Then
                UploadNotif = New Notification(DoTranslation("Uploading..."), FileUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(DownloadNotif)
            End If
            If ShowProgress Then AddHandler WClientProgress.HttpSendProgress, AddressOf HttpSendProgressWatch

            'Send the GET request to the server for the file after getting the stream and target file stream
            Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDir)
            Dim FilePath As String = NeutralizePath(FileName)
            Dim FileStream As New FileStream(FilePath, FileMode.Open, FileAccess.Read)
            Dim Content As New StreamContent(FileStream)

            'Upload now
            Try
                Dim Response As HttpResponseMessage = WClient.PutAsync(URL, Content, CancellationToken.Token).Result
                Response.EnsureSuccessStatusCode()
                UploadChecker(Nothing)
            Catch ex As Exception
                UploadChecker(ex)
            End Try

            'We're done uploading. Check to see if it's actually an error
            TransferFinished = False
            If ShowProgress And Not SuppressUploadMessage Then Console.WriteLine()
            SuppressUploadMessage = False
            If IsError Then
                If UploadNotificationProvoke Then UploadNotif.ProgressFailed = True
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                If UploadNotificationProvoke Then UploadNotif.Progress = 100
                Return True
            End If
        End Function

        ''' <summary>
        ''' Downloads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadString(URL As String) As String
            Return DownloadString(URL, ShowProgress)
        End Function

        ''' <summary>
        ''' Downloads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        Public Function DownloadString(URL As String, ShowProgress As Boolean) As String
            'Intialize variables
            Dim StringUri As New Uri(URL)

            'Initialize the progress bar indicator and the file completed event handler
            If DownloadNotificationProvoke Then
                DownloadNotif = New Notification(DoTranslation("Downloading..."), StringUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(DownloadNotif)
            End If
            If ShowProgress Then AddHandler WClientProgress.HttpReceiveProgress, AddressOf HttpReceiveProgressWatch

            'Send the GET request to the server for the file
            Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDir)
            Dim Response As HttpResponseMessage = WClient.GetAsync(StringUri, CancellationToken.Token).Result
            Response.EnsureSuccessStatusCode()

            'Get the memory stream
            Dim ContentStream As New MemoryStream

            'Try to download the string asynchronously
            Task.Run(Sub()
                         Try
                             Response.Content.ReadAsStreamAsync.Result.CopyTo(ContentStream)
                             ContentStream.Seek(0, SeekOrigin.Begin)
                             DownloadChecker(Nothing)
                         Catch ex As Exception
                             DownloadChecker(ex)
                         End Try
                     End Sub, CancellationToken.Token)
            While Not TransferFinished
                If CancelRequested Then
                    TransferFinished = True
                    CancellationToken.Cancel()
                End If
            End While

            'We're done downloading. Check to see if it's actually an error
            TransferFinished = False
            If ShowProgress And Not SuppressDownloadMessage Then Console.WriteLine()
            SuppressDownloadMessage = False
            If IsError Then
                If DownloadNotificationProvoke Then DownloadNotif.ProgressFailed = True
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                If DownloadNotificationProvoke Then DownloadNotif.Progress = 100
                Return New StreamReader(ContentStream).ReadToEnd
            End If
        End Function

        ''' <summary>
        ''' Uploads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="Data">Content to upload</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function UploadString(URL As String, Data As String) As Boolean
            Return UploadString(URL, Data, ShowProgress)
        End Function

        ''' <summary>
        ''' Uploads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL</param>
        ''' <param name="Data">Content to upload</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        Public Function UploadString(URL As String, Data As String, ShowProgress As Boolean) As Boolean
            'Intialize variables
            Dim StringUri As New Uri(URL)

            'Initialize the progress bar indicator and the file completed event handler
            If UploadNotificationProvoke Then
                UploadNotif = New Notification(DoTranslation("Uploading..."), StringUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(UploadNotif)
            End If
            If ShowProgress Then AddHandler WClientProgress.HttpSendProgress, AddressOf HttpSendProgressWatch

            'Send the GET request to the server for the file
            Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDir)
            Dim StringContent As New StringContent(Data)

            Try
                Dim Response As HttpResponseMessage = WClient.PutAsync(URL, StringContent, CancellationToken.Token).Result
                Response.EnsureSuccessStatusCode()
                UploadChecker(Nothing)
            Catch ex As Exception
                UploadChecker(ex)
            End Try

            'We're done uploading. Check to see if it's actually an error
            TransferFinished = False
            If ShowProgress And Not SuppressUploadMessage Then Console.WriteLine()
            SuppressUploadMessage = False
            If IsError Then
                If UploadNotificationProvoke Then UploadNotif.ProgressFailed = True
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                If UploadNotificationProvoke Then UploadNotif.Progress = 100
                Return True
            End If
        End Function

        ''' <summary>
        ''' Check for errors on download completion.
        ''' </summary>
        Private Sub DownloadChecker(e As Exception)
            Wdbg(DebugLevel.I, "Download complete. Error: {0}", e?.Message)
            If e IsNot Nothing Then
                If DownloadNotificationProvoke Then DownloadNotif.ProgressFailed = True
                ReasonError = e
                IsError = True
            End If
            TransferFinished = True
        End Sub

        ''' <summary>
        ''' Thread to check for errors on download completion.
        ''' </summary>
        Private Sub UploadChecker(e As Exception)
            Wdbg(DebugLevel.I, "Upload complete. Error: {0}", e?.Message)
            If e IsNot Nothing Then
                If UploadNotificationProvoke Then UploadNotif.ProgressFailed = True
                ReasonError = e
                IsError = True
            End If
            TransferFinished = True
        End Sub

        Private Sub HttpReceiveProgressWatch(sender As Object, e As HttpProgressEventArgs)
            Dim TotalBytes As Integer = If(e.TotalBytes, -1)
            Dim TransferInfo As New NetworkTransferInfo(e.BytesTransferred, TotalBytes, NetworkTransferType.Download)
            SuppressDownloadMessage = TotalBytes = -1
            TransferProgress(TransferInfo)
        End Sub

        Private Sub HttpSendProgressWatch(sender As Object, e As HttpProgressEventArgs)
            Dim TotalBytes As Integer = If(e.TotalBytes, -1)
            Dim TransferInfo As New NetworkTransferInfo(e.BytesTransferred, TotalBytes, NetworkTransferType.Upload)
            SuppressUploadMessage = TotalBytes = -1
            TransferProgress(TransferInfo)
        End Sub

        ''' <summary>
        ''' Report the progress to the console.
        ''' </summary>
        Private Sub TransferProgress(TransferInfo As NetworkTransferInfo)
            Try
                'Distinguish download from upload
                Dim NotificationProvoke As Boolean = If(TransferInfo.TransferType = NetworkTransferType.Download, DownloadNotificationProvoke, UploadNotificationProvoke)
                Dim NotificationInstance As Notification = If(TransferInfo.TransferType = NetworkTransferType.Download, DownloadNotif, UploadNotif)

                'Report the progress
                If Not TransferFinished Then
                    If TransferInfo.FileSize >= 0 And Not TransferInfo.MessageSuppressed Then
                        'We know the total bytes. Print it out.
                        Dim Progress As Double = 100 * (TransferInfo.DoneSize / TransferInfo.FileSize)
                        If NotificationProvoke Then
                            NotificationInstance.Progress = Progress
                        Else
                            If Not String.IsNullOrWhiteSpace(DownloadPercentagePrint) Then
                                WriteWhere(ProbePlaces(DownloadPercentagePrint), 0, Console.CursorTop, False, ColTypes.Neutral, TransferInfo.DoneSize.FileSizeToString, TransferInfo.FileSize.FileSizeToString, Progress)
                            Else
                                WriteWhere(DoTranslation("{0} of {1} downloaded.") + " | {2}%", 0, Console.CursorTop, False, ColTypes.Neutral, TransferInfo.DoneSize.FileSizeToString, TransferInfo.FileSize.FileSizeToString, Progress)
                            End If
                            ClearLineToRight()
                        End If
                    Else
                        TransferInfo.MessageSuppressed = True
                    End If
                End If
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Error trying to report transfer progress: {0}", ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

    End Module
End Namespace
