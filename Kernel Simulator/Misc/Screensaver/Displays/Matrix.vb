
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Namespace Misc.Screensaver.Displays
    Module MatrixDisplay

        Public WithEvents Matrix As New NamedBackgroundWorker("Matrix screensaver thread") With {.WorkerSupportsCancellation = True}

        ''' <summary>
        ''' Handles the code of Matrix
        ''' </summary>
        Sub Matrix_DoWork(sender As Object, e As DoWorkEventArgs) Handles Matrix.DoWork
            'Variables
            Dim random As New Random()
            Dim CurrentWindowWidth As Integer = Console.WindowWidth
            Dim CurrentWindowHeight As Integer = Console.WindowHeight
            Dim ResizeSyncing As Boolean

            'Preparations
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.Green
            Console.Clear()

            'Screensaver logic
            Do While True
                Console.CursorVisible = False
                If Matrix.CancellationPending = True Then
                    HandleSaverCancel()
                    Exit Do
                Else
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        Console.Write(CStr(random.Next(2)))
                    Else
                        WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...")
                        Console.Clear()
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                End If
                SleepNoBlock(MatrixDelay, Matrix)
            Loop
        End Sub

        ''' <summary>
        ''' Checks for any screensaver error
        ''' </summary>
        Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles Matrix.RunWorkerCompleted
            HandleSaverError(e.Error)
        End Sub

    End Module
End Namespace
