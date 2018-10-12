
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Public Module TimeDate

    'Variables
    Public strKernelTimeDate As String
    Public KernelDateTime As New Date
    Public WithEvents TimeDateChange As New BackgroundWorker
    Private originalRow As Integer
    Private originalCol As Integer

    Sub TimeDateChange_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles TimeDateChange.DoWork

        Do While True
            If (TimeDateChange.CancellationPending = True) Then
                e.Cancel = True
                Exit Do
            Else
                KernelDateTime = Date.Now
                strKernelTimeDate = Date.Now.ToString
                initTimesInZones()

                'We're sorry, but this is the only way of writing text on the corner, so some lines in runtime might be out of position...
                'We also have to slow down calling this function every second.
                originalRow = Console.CursorTop
                originalCol = Console.CursorLeft
                If (CornerTD = True) Then
                    Console.SetCursorPosition(64, Console.WindowTop)
                    Wln("{0} {1}", "neutralText", FormatDateTime(CDate(strKernelTimeDate), DateFormat.ShortTime), FormatDateTime(CDate(strKernelTimeDate), DateFormat.ShortDate))
                    Console.SetCursorPosition(originalCol, originalRow)
                End If
            End If
            Sleep(1000)
        Loop

    End Sub

    Sub InitializeTimeDate()

        KernelDateTime = Date.Now
        strKernelTimeDate = Date.Now.ToString
        TimeDateChange.WorkerSupportsCancellation = True
        TimeDateChange.RunWorkerAsync()

    End Sub

    Public Sub ShowTime()

        Wln("datetime: Time is {0}", "neutralText", FormatDateTime(CDate(strKernelTimeDate), DateFormat.LongTime))
        Wln("datetime: Today is {0}", "neutralText", FormatDateTime(CDate(strKernelTimeDate), DateFormat.LongDate))
        Wln("datetime: Time Zone: {0}", "neutralText", TimeZone.CurrentTimeZone.StandardName)

    End Sub

End Module
