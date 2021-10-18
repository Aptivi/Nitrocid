
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

Module WindowsLogoDisplay

    Public WithEvents WindowsLogo As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of WindowsLogo
    ''' </summary>
    Sub WindowsLogo_DoWork(sender As Object, e As DoWorkEventArgs) Handles WindowsLogo.DoWork
        'Variables
        Dim RandomDriver As New Random()
        Dim Drawn As Boolean
        Dim CurrentWindowWidth As Integer = Console.WindowWidth
        Dim CurrentWindowHeight As Integer = Console.WindowHeight
        Dim ResizeSyncing As Boolean

        'Preparations
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

        'Screensaver logic
        Do While True
            Console.CursorVisible = False
            If WindowsLogo.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            Else
                If ResizeSyncing Then
                    Drawn = False

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                Else
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                    'Get the required positions for the four boxes
                    Dim UpperLeftBoxEndX As Integer = (Console.WindowWidth / 2) - 1
                    Dim UpperLeftBoxStartX As Integer = UpperLeftBoxEndX / 2
                    Dim UpperLeftBoxStartY As Integer = 2
                    Dim UpperLeftBoxEndY As Integer = (Console.WindowHeight / 2) - 1
                    Dim LowerLeftBoxEndX As Integer = (Console.WindowWidth / 2) - 1
                    Dim LowerLeftBoxStartX As Integer = LowerLeftBoxEndX / 2
                    Dim LowerLeftBoxStartY As Integer = (Console.WindowHeight / 2) + 1
                    Dim LowerLeftBoxEndY As Integer = Console.WindowHeight - 2
                    Dim UpperRightBoxStartX As Integer = (Console.WindowWidth / 2) + 2
                    Dim UpperRightBoxEndX As Integer = (Console.WindowWidth / 2) + UpperRightBoxStartX / 2
                    Dim UpperRightBoxStartY As Integer = 2
                    Dim UpperRightBoxEndY As Integer = (Console.WindowHeight / 2) - 1
                    Dim LowerRightBoxStartX As Integer = (Console.WindowWidth / 2) + 2
                    Dim LowerRightBoxEndX As Integer = (Console.WindowWidth / 2) + LowerRightBoxStartX / 2
                    Dim LowerRightBoxStartY As Integer = (Console.WindowHeight / 2) + 1
                    Dim LowerRightBoxEndY As Integer = Console.WindowHeight - 2

                    'Draw the Windows 11 logo
                    If Not Drawn Then
                        Console.BackgroundColor = ConsoleColor.Black
                        Console.Clear()
                        SetConsoleColor(New Color($"0;120;212"), True)

                        'First, draw the upper left box
                        For X As Integer = UpperLeftBoxStartX To UpperLeftBoxEndX
                            For Y As Integer = UpperLeftBoxStartY To UpperLeftBoxEndY
                                Console.SetCursorPosition(X, Y)
                                Console.Write(" ")
                            Next
                        Next

                        'Second, draw the lower left box
                        For X As Integer = LowerLeftBoxStartX To LowerLeftBoxEndX
                            For Y As Integer = LowerLeftBoxStartY To LowerLeftBoxEndY
                                Console.SetCursorPosition(X, Y)
                                Console.Write(" ")
                            Next
                        Next

                        'Third, draw the upper right box
                        For X As Integer = UpperRightBoxStartX To UpperRightBoxEndX
                            For Y As Integer = UpperRightBoxStartY To UpperRightBoxEndY
                                Console.SetCursorPosition(X, Y)
                                Console.Write(" ")
                            Next
                        Next

                        'Fourth, draw the lower right box
                        For X As Integer = LowerRightBoxStartX To LowerRightBoxEndX
                            For Y As Integer = LowerRightBoxStartY To LowerRightBoxEndY
                                Console.SetCursorPosition(X, Y)
                                Console.Write(" ")
                            Next
                        Next

                        'Set drawn
                        Drawn = True
                    End If
                End If
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles WindowsLogo.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
