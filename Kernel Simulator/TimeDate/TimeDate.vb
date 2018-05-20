
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

Module TimeDate

    'Variables
    Public strKernelTimeDate As String
    Public KernelDateTime As New Date
    Public WithEvents TimeDateChange As New BackgroundWorker

    Sub TimeDateChange_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles TimeDateChange.DoWork

        Do While True

            If (TimeDateChange.CancellationPending = True) Then
                e.Cancel = True
                Exit Do
            Else
                KernelDateTime = Date.Now
                strKernelTimeDate = Date.Now.ToString
            End If
            Sleep(500)

        Loop

    End Sub

    Sub InitializeTimeDate()

        KernelDateTime = Date.Now
        strKernelTimeDate = Date.Now.ToString
        TimeDateChange.WorkerSupportsCancellation = True
        TimeDateChange.RunWorkerAsync()
        ShowTimeQuiet()

    End Sub

    Sub ShowTime()

        Wln("datetime: Time is {0}", FormatDateTime(CDate(strKernelTimeDate), DateFormat.LongTime))
        Wln("datetime: Today is {0}", FormatDateTime(CDate(strKernelTimeDate), DateFormat.LongDate))

    End Sub

    Sub ShowTimeQuiet()

        If (Quiet = False) Then
            ShowTime()
        End If

    End Sub

End Module
