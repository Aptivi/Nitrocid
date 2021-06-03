
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

Module ColorMixDisplay

    Public WithEvents ColorMix As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of ColorMix
    ''' </summary>
    Sub ColorMix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim colorrand As New Random()
        Do While True
            SleepNoBlock(ColorMixDelay, ColorMix)
            If ColorMix.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                SetInputColor()
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Mix Colors screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Dim esc As Char = GetEsc()
                If ColorMixTrueColor Then
                    Dim RedColorNum As Integer = colorrand.Next(255)
                    Dim GreenColorNum As Integer = colorrand.Next(255)
                    Dim BlueColorNum As Integer = colorrand.Next(255)
                    Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                    Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m ")
                ElseIf ColorMix255Colors Then
                    Dim ColorNum As Integer = colorrand.Next(255)
                    Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
                Else
                    Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
                End If
            End If
        Loop
    End Sub

End Module
