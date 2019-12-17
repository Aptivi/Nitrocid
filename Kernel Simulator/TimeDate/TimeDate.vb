
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Public Module TimeDate

    'Variables
    Public KernelDateTime As New Date
    Public WithEvents TimeDateChange As New BackgroundWorker
    Private originalRow As Integer
    Private originalCol As Integer

    Sub TimeDateChange_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles TimeDateChange.DoWork

        Do While True
            If TimeDateChange.CancellationPending = True Then
                e.Cancel = True
                Exit Do
            Else
                KernelDateTime = Date.Now
                originalRow = Console.CursorTop
                originalCol = Console.CursorLeft
                If CornerTD = True Then
                    Console.SetCursorPosition(Console.WindowWidth - 19, Console.WindowTop)
                    W("{0} {1}", True, ColTypes.Neutral, KernelDateTime.ToShortTimeString, KernelDateTime.ToShortDateString)
                    Console.SetCursorPosition(originalCol, originalRow)
                End If
            End If
            Thread.Sleep(1000)
        Loop

    End Sub

    Sub InitTimeDate()

        KernelDateTime = Date.Now
        TimeDateChange.WorkerSupportsCancellation = True
        TimeDateChange.RunWorkerAsync()

    End Sub

    Public Sub CurrentTimes()

        W(DoTranslation("datetime: Time is {0}", currentLang), True, ColTypes.Neutral, RenderTime)
        W(DoTranslation("datetime: Today is {0}", currentLang), True, ColTypes.Neutral, RenderDate)
        W(DoTranslation("datetime: Time Zone: {0}", currentLang) + " ({1})", True, ColTypes.Neutral, TimeZone.CurrentTimeZone.StandardName, TimeZone.CurrentTimeZone.GetUtcOffset(KernelDateTime).ToString)

    End Sub

    Public Function RenderTime()
        If LongTimeDate Then
            Return KernelDateTime.ToLongTimeString
        Else
            Return KernelDateTime.ToShortTimeString
        End If
    End Function
    Public Function RenderTime(ByVal DT As Date)
        If LongTimeDate Then
            Return DT.ToLongTimeString
        Else
            Return DT.ToShortTimeString
        End If
    End Function
    Public Function RenderDate()
        If LongTimeDate Then
            Return KernelDateTime.ToLongDateString
        Else
            Return KernelDateTime.ToShortDateString
        End If
    End Function
    Public Function RenderDate(ByVal DT As Date)
        If LongTimeDate Then
            Return DT.ToLongDateString
        Else
            Return DT.ToShortDateString
        End If
    End Function

End Module
