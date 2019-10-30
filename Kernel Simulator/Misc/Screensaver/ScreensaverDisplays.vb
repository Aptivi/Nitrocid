
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

Public Module ScreensaverDisplays

    Public WithEvents ColorMix As New BackgroundWorker
    Public WithEvents Matrix As New BackgroundWorker
    Public WithEvents Disco As New BackgroundWorker
    Public WithEvents Lines As New BackgroundWorker
    Public WithEvents GlitterMatrix As New BackgroundWorker
    Public WithEvents Custom As New BackgroundWorker
    Public finalSaver As ICustomSaver

    Sub Custom_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Custom.DoWork
        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.W() with System.Console.WriteLine() or System.Console.Write().
        'TODO: Let screensaver developers set delay, and end the screensaver.
        Console.CursorVisible = False
        finalSaver.PreDisplay()
        Do While True
            'Thread.Sleep(1)
            If Custom.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Custom screensaver stopped.")
                Exit Do
            Else
                finalSaver.ScrnSaver()
            End If
        Loop
    End Sub

    Sub ColorMix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim colorrand As New Random()
        Do While True
            Thread.Sleep(1)
            If ColorMix.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Custom screensaver stopped.")
                Exit Do
            Else
                Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
            End If
        Loop
    End Sub

    Sub Matrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Matrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            If Matrix.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Custom screensaver stopped.")
                Exit Do
            Else
                Thread.Sleep(1)
                Console.Write(CStr(random.Next(2)))
            End If
        Loop
    End Sub

    Sub Disco_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Disco.DoWork
        Console.CursorVisible = False
        Do While True
            For Each color In colors
                Thread.Sleep(100)
                If Disco.CancellationPending = True Then
                    Wdbg("Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = inputColor
                    Console.BackgroundColor = backgroundColor
                    Load()
                    Console.CursorVisible = True
                    Wdbg("All clean. Custom screensaver stopped.")
                    Exit Do
                Else
                    Console.BackgroundColor = color
                    Console.Clear()
                End If
            Next
        Loop
    End Sub

    Sub Lines_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Lines.DoWork
        Console.CursorVisible = False
        Wdbg("Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            For Each color In colors
                Thread.Sleep(1000)
                If Lines.CancellationPending = True Then
                    Wdbg("Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = inputColor
                    Console.BackgroundColor = backgroundColor
                    Load()
                    Console.CursorVisible = True
                    Wdbg("All clean. Custom screensaver stopped.")
                    Exit Do
                Else
                    Console.Clear()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = color
                    Dim Line As String = ""
                    Dim Top As Integer = New Random().Next(Console.WindowHeight)
                    For i As Integer = 1 To Console.WindowWidth
                        Line += "-"
                    Next
                    Console.SetCursorPosition(0, Top)
                    Console.WriteLine(Line)
                End If
            Next
        Loop
    End Sub

    Sub GlitterMatrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterMatrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If GlitterMatrix.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Glitter Matrix screensaver stopped.")
                Exit Do
            Else
                Thread.Sleep(1)
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                Console.SetCursorPosition(Left, Top)
                Console.Write(CStr(RandomDriver.Next(2)))
            End If
        Loop
    End Sub

End Module
