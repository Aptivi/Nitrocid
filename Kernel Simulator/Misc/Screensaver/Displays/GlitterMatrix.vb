
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

Module GlitterMatrixDisplay

    Public WithEvents GlitterMatrix As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Glitter Matrix
    ''' </summary>
    Sub GlitterMatrix_DoWork(sender As Object, e As DoWorkEventArgs) Handles GlitterMatrix.DoWork
        'Variables
        Dim RandomDriver As New Random()
        Dim CurrentWindowWidth As Integer = Console.WindowWidth
        Dim CurrentWindowHeight As Integer = Console.WindowHeight
        Dim ResizeSyncing As Boolean

        'Preparations
        SetConsoleColor(New Color(GlitterMatrixBackgroundColor), True)
        SetConsoleColor(New Color(GlitterMatrixForegroundColor))
        Console.Clear()
        Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

        'Screensaver logic
        Do While True
            Console.CursorVisible = False
            If GlitterMatrix.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            Else
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                Console.SetCursorPosition(Left, Top)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.Write(CStr(RandomDriver.Next(2)))
                Else
                    Console.Clear()
                End If

                'Reset resize sync
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
            End If
            SleepNoBlock(GlitterMatrixDelay, GlitterMatrix)
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles GlitterMatrix.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
