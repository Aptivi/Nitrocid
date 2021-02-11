
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

Public Module NetworkTransfer

    Friend IsError As Boolean
    Friend ReasonError As Exception

    ''' <summary>
    ''' Downloads a file to the current working directory.
    ''' </summary>
    ''' <param name="URL">A URL to a file</param>
    ''' <param name="ShowProgress">Whether or not to show progress bar</param>
    ''' <param name="Credentials">Authentication information</param>
    ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
    Public Function DownloadFile(ByVal URL As String, ByVal ShowProgress As Boolean, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
        'Limit the filename to the name without any URL arguments
        Dim FileName As String = URL.Split("/").Last()
        Wdbg("I", "Prototype Filename: {0}", FileName)
        If FileName.Contains("?") Then
            FileName = FileName.Remove(FileName.IndexOf("?"c))
        End If
        Wdbg("I", "Finished Filename: {0}", FileName)

        'Download a file
        Wdbg("I", "Directory location: {0}", CurrDir)
        Dim WClient As New WebClient
        If Not IsNothing(Credentials) Then
            WClient.Credentials = Credentials
        End If
        If ShowProgress Then AddHandler WClient.DownloadProgressChanged, AddressOf DownloadManager
        AddHandler WClient.DownloadFileCompleted, AddressOf DownloadChecker
        WClient.DownloadFileAsync(New Uri(URL), NeutralizePath(FileName))
        While Not DFinish
        End While
        DFinish = False
        If IsError Then
            Throw ReasonError
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Uploads a file from the current working directory.
    ''' </summary>
    ''' <param name="File">A target file name. Use <see cref="NeutralizePath(String)"/> to get full path of source.</param>
    ''' <param name="URL">A URL</param>
    ''' <param name="ShowProgress">Whether or not to show progress bar</param>
    ''' <param name="Credentials">Authentication information</param>
    ''' <returns>True if successful. Throws exception if unsuccessful.</returns>
    Public Function UploadFile(ByVal File As String, ByVal URL As String, ByVal ShowProgress As Boolean, Optional ByVal Credentials As NetworkCredential = Nothing) As Boolean
        'Upload a file
        Wdbg("I", "Directory location: {0}", CurrDir)
        Dim WClient As New WebClient
        If Not IsNothing(Credentials) Then
            WClient.Credentials = Credentials
        End If
        If ShowProgress Then AddHandler WClient.UploadProgressChanged, AddressOf UploadManager
        AddHandler WClient.UploadFileCompleted, AddressOf UploadChecker
        WClient.UploadFileAsync(New Uri(URL), File)
        While Not UFinish
        End While
        UFinish = False
        If IsError Then
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
    ''' Thread to repeatedly report the download progress to the console.
    ''' </summary>
    Private Sub DownloadManager(sender As Object, e As DownloadProgressChangedEventArgs)
        If Not DFinish Then
            Console.SetCursorPosition(0, Console.CursorTop)
            WriteWhere(DoTranslation("{0} MB of {1} MB downloaded.", currentLang) + " | {2}%    ", 0, Console.CursorTop, ColTypes.Neutral, FormatNumber(e.BytesReceived / 1024 / 1024, 2), FormatNumber(e.TotalBytesToReceive / 1024 / 1024, 2), e.ProgressPercentage)
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
        If Not DFinish Then
            Console.SetCursorPosition(0, Console.CursorTop)
            WriteWhere(DoTranslation("{0} MB of {1} MB uploaded.", currentLang) + " | {2}%    ", 0, Console.CursorTop, ColTypes.Neutral, FormatNumber(e.BytesSent / 1024 / 1024, 2), FormatNumber(e.TotalBytesToSend / 1024 / 1024, 2), e.ProgressPercentage)
        End If
    End Sub

End Module
