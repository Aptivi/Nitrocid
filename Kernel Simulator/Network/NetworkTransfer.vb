
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

Imports System.ComponentModel
Imports System.Threading
Imports KS.Misc.Notifications

Namespace Network
    Public Module NetworkTransfer

        Public DownloadPercentagePrint As String = ""
        Public UploadPercentagePrint As String = ""
        Public DownloadNotificationProvoke As Boolean
        Public UploadNotificationProvoke As Boolean
        Friend IsError As Boolean
        Friend ReasonError As Exception
        Friend CancellationToken As New CancellationTokenSource
        Friend WClient As New WebClient
        Friend DownloadedString As String
        Friend DownloadNotif As Notification
        Friend UploadNotif As Notification
        Friend SuppressDownloadMessage As Boolean
        Friend SuppressUploadMessage As Boolean

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String, Optional Credentials As NetworkCredential = Nothing) As Boolean
            Return DownloadFile(URL, ShowProgress, Credentials)
        End Function

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String, ShowProgress As Boolean, Optional Credentials As NetworkCredential = Nothing) As Boolean
            Dim FileName As String = GetFilenameFromUrl(URL)
            Return DownloadFile(URL, ShowProgress, FileName, Credentials)
        End Function

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="FileName">File name to download to</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String, FileName As String, Optional Credentials As NetworkCredential = Nothing) As Boolean
            Return DownloadFile(URL, ShowProgress, FileName, Credentials)
        End Function

        ''' <summary>
        ''' Downloads a file to the current working directory.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <param name="FileName">File name to download to</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadFile(URL As String, ShowProgress As Boolean, FileName As String, Optional Credentials As NetworkCredential = Nothing) As Boolean
            'Intialize variables
            WClient = New WebClient
            Dim FileUri As New Uri(URL)

            'Check the credentials
            Wdbg(DebugLevel.I, "Directory location: {0}", CurrDir)
            If Credentials IsNot Nothing Then
                WClient.Credentials = Credentials
            End If

            'Initialize the progress bar indicator and the file completed event handler
            If DownloadNotificationProvoke Then
                DownloadNotif = New Notification(DoTranslation("Downloading..."), FileUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(DownloadNotif)
            End If
            If ShowProgress Then AddHandler WClient.DownloadProgressChanged, AddressOf DownloadManager
            AddHandler WClient.DownloadFileCompleted, AddressOf DownloadChecker

            'Try to download the file asynchronously
            WClient.DownloadFileAsync(FileUri, NeutralizePath(FileName), CancellationToken.Token)
            While Not DFinish
                If CancelRequested Then
                    CancellationToken.Cancel()
                End If
            End While

            'We're done downloading. Check to see if it's actually an error
            DFinish = False
            If ShowProgress And Not SuppressDownloadMessage Then Console.WriteLine()
            SuppressDownloadMessage = False
            If IsError Then
                DownloadNotif.ProgressFailed = True
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                DownloadNotif.Progress = 100
                Return True
            End If
        End Function

        ''' <summary>
        ''' Uploads a file to the current working directory.
        ''' </summary>
        ''' <param name="File">A target file name. Use <see cref="NeutralizePath(String, Boolean)"/> to get full path of source.</param>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function UploadFile(File As String, URL As String, Optional Credentials As NetworkCredential = Nothing) As Boolean
            Return UploadFile(File, URL, ShowProgress, Credentials)
        End Function

        ''' <summary>
        ''' Uploads a file from the current working directory.
        ''' </summary>
        ''' <param name="File">A target file name. Use <see cref="NeutralizePath(String, Boolean)"/> to get full path of source.</param>
        ''' <param name="URL">A URL</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function UploadFile(File As String, URL As String, ShowProgress As Boolean, Optional Credentials As NetworkCredential = Nothing) As Boolean
            'Intialize variables
            WClient = New WebClient
            Dim FileUri As New Uri(URL)

            'Check the credentials
            Wdbg(DebugLevel.I, "Directory location: {0}", CurrDir)
            If Credentials IsNot Nothing Then
                WClient.Credentials = Credentials
            End If

            'Initialize the progress bar indicator and the file completed event handler
            If UploadNotificationProvoke Then
                UploadNotif = New Notification(DoTranslation("Uploading..."), FileUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(UploadNotif)
            End If
            If ShowProgress Then AddHandler WClient.UploadProgressChanged, AddressOf UploadManager
            AddHandler WClient.UploadFileCompleted, AddressOf UploadChecker

            'Try to upload the file asynchronously
            WClient.UploadFileAsync(FileUri, Nothing, File, CancellationToken.Token)
            While Not UFinish
                If CancelRequested Then
                    CancellationToken.Cancel()
                End If
            End While

            'We're done uploading. Check to see if it's actually an error
            UFinish = False
            If ShowProgress And Not SuppressUploadMessage Then Console.WriteLine()
            SuppressUploadMessage = False
            If IsError Then
                UploadNotif.ProgressFailed = True
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                UploadNotif.Progress = 100
                Return True
            End If
        End Function

        ''' <summary>
        ''' Downloads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function DownloadString(URL As String, Optional Credentials As NetworkCredential = Nothing) As String
            Return DownloadString(URL, ShowProgress, Credentials)
        End Function

        ''' <summary>
        ''' Downloads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        Public Function DownloadString(URL As String, ShowProgress As Boolean, Optional Credentials As NetworkCredential = Nothing) As String
            'Intialize variables
            WClient = New WebClient
            Dim FileUri As New Uri(URL)
            DownloadedString = ""

            'Check the credentials
            Wdbg(DebugLevel.I, "Resource location: {0}", URL)
            If Credentials IsNot Nothing Then
                WClient.Credentials = Credentials
            End If

            'Initialize the progress bar indicator and the resource completed event handler
            If DownloadNotificationProvoke Then
                DownloadNotif = New Notification(DoTranslation("Downloading..."), FileUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(DownloadNotif)
            End If
            If ShowProgress Then AddHandler WClient.DownloadProgressChanged, AddressOf DownloadManager
            AddHandler WClient.DownloadStringCompleted, AddressOf DownloadStringChecker

            'Try to download the resource asynchronously
            WClient.DownloadStringAsync(New Uri(URL), CancellationToken.Token)
            While Not DFinish
                If CancelRequested Then
                    CancellationToken.Cancel()
                End If
            End While

            'We're done downloading. Check to see if it's actually an error
            DFinish = False
            If ShowProgress And Not SuppressDownloadMessage Then Console.WriteLine()
            SuppressDownloadMessage = False
            If IsError Then
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                Return DownloadedString
            End If
        End Function

        ''' <summary>
        ''' Uploads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL to a file</param>
        ''' <param name="Data">Content to upload</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
        Public Function UploadString(URL As String, Data As String, Optional Credentials As NetworkCredential = Nothing) As Boolean
            Return UploadString(URL, Data, ShowProgress, Credentials)
        End Function

        ''' <summary>
        ''' Uploads a resource from URL as a string.
        ''' </summary>
        ''' <param name="URL">A URL</param>
        ''' <param name="Data">Content to upload</param>
        ''' <param name="ShowProgress">Whether or not to show progress bar</param>
        ''' <param name="Credentials">Authentication information</param>
        ''' <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        Public Function UploadString(URL As String, Data As String, ShowProgress As Boolean, Optional Credentials As NetworkCredential = Nothing) As Boolean
            'Intialize variables
            WClient = New WebClient
            Dim FileUri As New Uri(URL)

            'Check the credentials
            Wdbg(DebugLevel.I, "Resource location: {0}", URL)
            If Credentials IsNot Nothing Then
                WClient.Credentials = Credentials
            End If

            'Initialize the progress bar indicator and the resource completed event handler
            If UploadNotificationProvoke Then
                UploadNotif = New Notification(DoTranslation("Uploading..."), FileUri.AbsoluteUri, NotifPriority.Low, NotifType.Progress)
                NotifySend(UploadNotif)
            End If
            If ShowProgress Then AddHandler WClient.UploadProgressChanged, AddressOf UploadManager
            AddHandler WClient.UploadFileCompleted, AddressOf UploadChecker

            'Try to upload the resource asynchronously
            WClient.UploadStringAsync(New Uri(URL), Nothing, Data, CancellationToken.Token)
            While Not UFinish
                If CancelRequested Then
                    CancellationToken.Cancel()
                End If
            End While

            'We're done uploading. Check to see if it's actually an error
            UFinish = False
            If ShowProgress And Not SuppressUploadMessage Then Console.WriteLine()
            SuppressUploadMessage = False
            If IsError Then
                CancellationToken.Cancel()
                Throw ReasonError
            Else
                Return True
            End If
        End Function

        ''' <summary>
        ''' Thread to check for errors on download completion.
        ''' </summary>
        Private Sub DownloadChecker(sender As Object, e As AsyncCompletedEventArgs)
            Wdbg(DebugLevel.I, "Download complete. Error: {0}", e.Error?.Message)
            If e.Error IsNot Nothing Then
                If DownloadNotificationProvoke Then DownloadNotif.ProgressFailed = True
                ReasonError = e.Error
                IsError = True
            End If
            DFinish = True
        End Sub

        ''' <summary>
        ''' Thread to check for errors on download completion.
        ''' </summary>
        Private Sub DownloadStringChecker(sender As Object, e As DownloadStringCompletedEventArgs)
            Wdbg(DebugLevel.I, "Download complete. Error: {0}", e.Error?.Message)
            DownloadedString = e.Result
            If e.Error IsNot Nothing Then
                If DownloadNotificationProvoke Then DownloadNotif.ProgressFailed = True
                ReasonError = e.Error
                IsError = True
            End If
            DFinish = True
        End Sub

        ''' <summary>
        ''' Thread to repeatedly report the download progress to the console.
        ''' </summary>
        Private Sub DownloadManager(sender As Object, e As DownloadProgressChangedEventArgs)
            If CancellationToken.Token.IsCancellationRequested Then
                WClient.CancelAsync()
            End If
            If Not DFinish Then
                If e.TotalBytesToReceive >= 0 And Not SuppressDownloadMessage Then
                    'We know the total bytes. Print it out.
                    If DownloadNotificationProvoke Then
                        DownloadNotif.Progress = e.ProgressPercentage
                    Else
                        If Not String.IsNullOrWhiteSpace(DownloadPercentagePrint) Then
                            WriteWhere(ProbePlaces(DownloadPercentagePrint), 0, Console.CursorTop, False, ColTypes.Neutral, e.BytesReceived.FileSizeToString, e.TotalBytesToReceive.FileSizeToString, e.ProgressPercentage)
                        Else
                            WriteWhere(DoTranslation("{0} of {1} downloaded.") + " | {2}%", 0, Console.CursorTop, False, ColTypes.Neutral, e.BytesReceived.FileSizeToString, e.TotalBytesToReceive.FileSizeToString, e.ProgressPercentage)
                        End If
                        ClearLineToRight()
                    End If
                Else
                    SuppressDownloadMessage = True
                End If
            End If
        End Sub

        ''' <summary>
        ''' Thread to check for errors on download completion.
        ''' </summary>
        Private Sub UploadChecker(sender As Object, e As AsyncCompletedEventArgs)
            Wdbg(DebugLevel.I, "Upload complete. Error: {0}", e.Error?.Message)
            If e.Error IsNot Nothing Then
                If UploadNotificationProvoke Then UploadNotif.ProgressFailed = True
                ReasonError = e.Error
                IsError = True
            End If
            UFinish = True
        End Sub

        ''' <summary>
        ''' Thread to repeatedly report the download progress to the console.
        ''' </summary>
        Private Sub UploadManager(sender As Object, e As UploadProgressChangedEventArgs)
            If CancellationToken.Token.IsCancellationRequested Then
                WClient.CancelAsync()
            End If
            If Not UFinish Then
                If e.TotalBytesToSend >= 0 Then
                    'We know the total bytes. Print it out.
                    If UploadNotificationProvoke And Not SuppressUploadMessage Then
                        UploadNotif.Progress = e.ProgressPercentage
                    Else
                        If Not String.IsNullOrWhiteSpace(UploadPercentagePrint) Then
                            WriteWhere(ProbePlaces(UploadPercentagePrint), 0, Console.CursorTop, False, ColTypes.Neutral, e.BytesReceived.FileSizeToString, e.TotalBytesToReceive.FileSizeToString, e.ProgressPercentage)
                        Else
                            WriteWhere(DoTranslation("{0} of {1} uploaded.") + " | {2}%", 0, Console.CursorTop, False, ColTypes.Neutral, e.BytesSent.FileSizeToString, e.TotalBytesToSend.FileSizeToString, e.ProgressPercentage)
                        End If
                        ClearLineToRight()
                    End If
                Else
                    SuppressUploadMessage = True
                End If
            End If
        End Sub

    End Module
End Namespace
