
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

Imports System.ComponentModel
Imports System.Threading

Public Module NetworkTransfer

    Friend IsError As Boolean
    Friend ReasonError As Exception
    Friend CancellationToken As New CancellationTokenSource
    Friend WClient As New WebClient
    Friend DownloadedString As String

    ''' <summary>
    ''' Downloads a file to the current working directory.
    ''' </summary>
    ''' <param name="URL">A URL to a file</param>
    ''' <param name="Credentials">Authentication information</param>
    ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
    Public Function DownloadFile(ByVal URL As String, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
        Return DownloadFile(URL, ShowProgress, Credentials)
    End Function

    ''' <summary>
    ''' Downloads a file to the current working directory.
    ''' </summary>
    ''' <param name="URL">A URL to a file</param>
    ''' <param name="ShowProgress">Whether or not to show progress bar</param>
    ''' <param name="Credentials">Authentication information</param>
    ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
    Public Function DownloadFile(ByVal URL As String, ByVal ShowProgress As Boolean, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
        WClient = New WebClient

        'Limit the filename to the name without any URL arguments
        Dim FileName As String = URL.Split("/").Last()
        Wdbg("I", "Prototype Filename: {0}", FileName)
        If FileName.Contains("?") Then
            FileName = FileName.Remove(FileName.IndexOf("?"c))
        End If
        Wdbg("I", "Finished Filename: {0}", FileName)

        'Download a file
        Wdbg("I", "Directory location: {0}", CurrDir)
        If Not IsNothing(Credentials) Then
            WClient.Credentials = Credentials
        End If
        If ShowProgress Then AddHandler WClient.DownloadProgressChanged, AddressOf DownloadManager
        AddHandler WClient.DownloadFileCompleted, AddressOf DownloadChecker
        WClient.DownloadFileAsync(New Uri(URL), NeutralizePath(FileName), CancellationToken.Token)
        While Not DFinish
            If CancelRequested Then
                CancellationToken.Cancel()
            End If
        End While
        DFinish = False
        If ShowProgress Then Console.WriteLine()
        If IsError Then
            CancellationToken.Cancel()
            Throw ReasonError
        Else
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
    Public Function UploadFile(ByVal File As String, ByVal URL As String, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
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
    Public Function UploadFile(ByVal File As String, ByVal URL As String, ByVal ShowProgress As Boolean, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
        WClient = New WebClient

        'Upload a file
        Wdbg("I", "Directory location: {0}", CurrDir)
        If Not IsNothing(Credentials) Then
            WClient.Credentials = Credentials
        End If
        If ShowProgress Then AddHandler WClient.UploadProgressChanged, AddressOf UploadManager
        AddHandler WClient.UploadFileCompleted, AddressOf UploadChecker
        WClient.UploadFileAsync(New Uri(URL), Nothing, File, CancellationToken.Token)
        While Not UFinish
            If CancelRequested Then
                CancellationToken.Cancel()
            End If
        End While
        UFinish = False
        If ShowProgress Then Console.WriteLine()
        If IsError Then
            CancellationToken.Cancel()
            Throw ReasonError
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Downloads a resource from URL as a string.
    ''' </summary>
    ''' <param name="URL">A URL to a file</param>
    ''' <param name="Credentials">Authentication information</param>
    ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
    Public Function DownloadString(ByVal URL As String, Optional ByVal Credentials As NetworkCredential = Nothing) As String
        Return DownloadString(URL, ShowProgress, Credentials)
    End Function

    ''' <summary>
    ''' Downloads a resource from URL as a string.
    ''' </summary>
    ''' <param name="URL">A URL</param>
    ''' <param name="ShowProgress">Whether or not to show progress bar</param>
    ''' <param name="Credentials">Authentication information</param>
    ''' <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
    Public Function DownloadString(ByVal URL As String, ByVal ShowProgress As Boolean, Optional ByVal Credentials As NetworkCredential = Nothing) As String
        WClient = New WebClient
        DownloadedString = ""

        'Download a resource
        Wdbg("I", "Resource location: {0}", URL)
        If Not IsNothing(Credentials) Then
            WClient.Credentials = Credentials
        End If
        If ShowProgress Then AddHandler WClient.DownloadProgressChanged, AddressOf DownloadManager
        AddHandler WClient.DownloadStringCompleted, AddressOf DownloadStringChecker
        WClient.DownloadStringAsync(New Uri(URL), CancellationToken.Token)
        While Not DFinish
            If CancelRequested Then
                CancellationToken.Cancel()
            End If
        End While
        DFinish = False
        If ShowProgress Then Console.WriteLine()
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
    Public Function UploadString(ByVal URL As String, ByVal Data As String, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
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
    Public Function UploadString(ByVal URL As String, ByVal Data As String, ByVal ShowProgress As Boolean, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
        WClient = New WebClient

        'Download a resource
        Wdbg("I", "Resource location: {0}", URL)
        If Not IsNothing(Credentials) Then
            WClient.Credentials = Credentials
        End If
        If ShowProgress Then AddHandler WClient.UploadProgressChanged, AddressOf UploadManager
        AddHandler WClient.UploadFileCompleted, AddressOf UploadChecker
        WClient.UploadStringAsync(New Uri(URL), Nothing, Data, CancellationToken.Token)
        While Not UFinish
            If CancelRequested Then
                CancellationToken.Cancel()
            End If
        End While
        UFinish = False
        If ShowProgress Then Console.WriteLine()
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
        Wdbg("I", "Download complete. Error: {0}", e.Error?.Message)
        If Not IsNothing(e.Error) Then
            ReasonError = e.Error
            IsError = True
        End If
        DFinish = True
    End Sub

    ''' <summary>
    ''' Thread to check for errors on download completion.
    ''' </summary>
    Private Sub DownloadStringChecker(sender As Object, e As DownloadStringCompletedEventArgs)
        Wdbg("I", "Download complete. Error: {0}", e.Error?.Message)
        DownloadedString = e.Result
        If Not IsNothing(e.Error) Then
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
            WriteWhere(DoTranslation("{0} of {1} downloaded.") + " | {2}%    ", 0, Console.CursorTop, True, ColTypes.Neutral, e.BytesReceived.FileSizeToString, e.TotalBytesToReceive.FileSizeToString, e.ProgressPercentage)
        End If
    End Sub

    ''' <summary>
    ''' Thread to check for errors on download completion.
    ''' </summary>
    Private Sub UploadChecker(sender As Object, e As AsyncCompletedEventArgs)
        Wdbg("I", "Upload complete. Error: {0}", e.Error?.Message)
        If Not IsNothing(e.Error) Then
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
        If Not DFinish Then
            WriteWhere(DoTranslation("{0} of {1} uploaded.") + " | {2}%    ", 0, Console.CursorTop, True, ColTypes.Neutral, e.BytesSent.FileSizeToString, e.TotalBytesToSend.FileSizeToString, e.ProgressPercentage)
        End If
    End Sub

End Module
