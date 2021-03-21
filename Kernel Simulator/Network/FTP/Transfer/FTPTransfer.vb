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

Public Module FTPTransfer

    'Progress Bar Enabled
    Dim progressFlag As Boolean = True
    Dim ConsoleOriginalPosition_LEFT As Integer
    Dim ConsoleOriginalPosition_TOP As Integer

    'To enable progress
    Public Complete As New Action(Of FtpProgress)(Sub(percentage)
                                                      'If the progress is not defined, disable progress bar
                                                      If percentage.Progress < 0 Then
                                                          progressFlag = False
                                                      Else
                                                          ConsoleOriginalPosition_LEFT = Console.CursorLeft
                                                          ConsoleOriginalPosition_TOP = Console.CursorTop
                                                          If progressFlag = True And percentage.Progress <> 100 Then
                                                              W(" {0}% (ETA: {1}d {2}:{3}:{4} @ {5})", False, ColTypes.Neutral, FormatNumber(percentage.Progress, 1), percentage.ETA.Days, percentage.ETA.Hours, percentage.ETA.Minutes, percentage.ETA.Seconds, percentage.TransferSpeedToString)
                                                          End If
                                                          Console.SetCursorPosition(ConsoleOriginalPosition_LEFT, ConsoleOriginalPosition_TOP)
                                                      End If
                                                  End Sub)

    ''' <summary>
    ''' Downloads a file from the currently connected FTP server
    ''' </summary>
    ''' <param name="File">A remote file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPGetFile(ByVal File As String) As Boolean
        If connected Then
            Try
                'Show a message to download
                EventManager.RaiseFTPPreDownload(File)
                Wdbg("I", "Downloading file {0}...", File)

                'Try to download 3 times
                Dim Result As FtpStatus = ClientFTP.DownloadFile($"{currDirect}/{File}", File, True, FtpVerify.Retry + FtpVerify.Throw, Complete)

                'Show a message that it's downloaded
                Wdbg("I", "Downloaded file {0}.", File)
                EventManager.RaiseFTPPostDownload(File, Result.IsSuccess)
                Return True
            Catch ex As Exception
                Wdbg("E", "Download failed for file {0}: {1}", File, ex.Message)
                EventManager.RaiseFTPPostDownload(File, False)
            End Try
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Uploads a file to the currently connected FTP server
    ''' </summary>
    ''' <param name="File">A local file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPUploadFile(ByVal File As String) As Boolean
        If connected Then
            'Show a message to download
            EventManager.RaiseFTPPreUpload(File)
            Wdbg("I", "Uploading file {0}...", File)

            'Try to upload
            Dim Success As Boolean = ClientFTP.UploadFile($"{currDirect}/{File}", File, True, True, FtpVerify.Retry, Complete)
            Wdbg("I", "Uploaded file {0} with status {1}.", File, Success)
            EventManager.RaiseFTPPostUpload(File, Success)
            Return Success
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

End Module
