
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

Namespace Network.FTP.Transfer
    Public Module FTPTransferProgress

        ''' <summary>
        ''' Action of file progress. You can make your own handler by mods
        ''' </summary>
        Public FileProgress As New Action(Of FtpProgress)(AddressOf FileProgressHandler)
        ''' <summary>
        ''' Action of folder/multiple file progress. You can make your own handler by mods
        ''' </summary>
        Public MultipleProgress As New Action(Of FtpProgress)(AddressOf MultipleProgressHandler)

        ''' <summary>
        ''' Handles the individual file download/upload progress
        ''' </summary>
        Private Sub FileProgressHandler(Percentage As FtpProgress)
            'If the progress is not defined, disable progress bar
            If Percentage.Progress < 0 Then
                progressFlag = False
            Else
                ConsoleOriginalPosition_LEFT = Console.CursorLeft
                ConsoleOriginalPosition_TOP = Console.CursorTop
                If progressFlag = True And Percentage.Progress <> 100 Then
                    Write(" {0}% (ETA: {1}d {2}:{3}:{4} @ {5})", False, ColTypes.Progress, Percentage.Progress.ToString("N2"), Percentage.ETA.Days, Percentage.ETA.Hours, Percentage.ETA.Minutes, Percentage.ETA.Seconds, Percentage.TransferSpeedToString)
                    ClearLineToRight()
                End If
                Console.SetCursorPosition(ConsoleOriginalPosition_LEFT, ConsoleOriginalPosition_TOP)
            End If
        End Sub

        ''' <summary>
        ''' Handles the multiple files/folder download/upload progress
        ''' </summary>
        Private Sub MultipleProgressHandler(Percentage As FtpProgress)
            'If the progress is not defined, disable progress bar
            If Percentage.Progress < 0 Then
                progressFlag = False
            Else
                ConsoleOriginalPosition_LEFT = Console.CursorLeft
                ConsoleOriginalPosition_TOP = Console.CursorTop
                If progressFlag = True And Percentage.Progress <> 100 Then
                    Write("- [{0}/{1}] {2}: ", False, ColTypes.ListEntry, Percentage.FileIndex + 1, Percentage.FileCount, Percentage.RemotePath)
                    Write("{0}% (ETA: {1}d {2}:{3}:{4} @ {5})", False, ColTypes.Progress, Percentage.Progress.ToString("N2"), Percentage.ETA.Days, Percentage.ETA.Hours, Percentage.ETA.Minutes, Percentage.ETA.Seconds, Percentage.TransferSpeedToString)
                    ClearLineToRight()
                End If
                Console.SetCursorPosition(ConsoleOriginalPosition_LEFT, ConsoleOriginalPosition_TOP)
            End If
        End Sub

    End Module
End Namespace
