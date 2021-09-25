
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
Imports System.Threading

Module DissolveDisplay

    Public WithEvents Dissolve As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Dissolve
    ''' </summary>
    Sub Dissolve_DoWork(sender As Object, e As DoWorkEventArgs) Handles Dissolve.DoWork
        Try
            'Variables
            Dim RandomDriver As New Random()
            Dim ColorFilled As Boolean
            Dim CoveredPositions As New ArrayList
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
                If Dissolve.CancellationPending = True Then
                    Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg(DebugLevel.I, "All clean. Dissolve screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    If ColorFilled Then Thread.Sleep(1)
                    Dim EndLeft As Integer = Console.WindowWidth - 1
                    Dim EndTop As Integer = Console.WindowHeight - 1
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", ColorFilled)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop)
                    If Not ColorFilled Then
                        'NOTICE: Mono seems to have a bug in Console.CursorLeft and Console.CursorTop when printing with VT escape sequences.
                        If Not (Console.CursorLeft >= EndLeft And Console.CursorTop >= EndTop) Then
                            Dim esc As Char = GetEsc()
                            If DissolveTrueColor Then
                                Dim RedColorNum As Integer = RandomDriver.Next(255)
                                Dim GreenColorNum As Integer = RandomDriver.Next(255)
                                Dim BlueColorNum As Integer = RandomDriver.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If Not ResizeSyncing Then
                                    WriteC(" ", False, New Color("0;0;0"), New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"))
                                Else
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                    ColorFilled = False
                                    Console.BackgroundColor = ConsoleColor.Black
                                    Console.Clear()
                                    CoveredPositions.Clear()
                                End If
                            ElseIf Dissolve255Colors Then
                                Dim ColorNum As Integer = RandomDriver.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If Not ResizeSyncing Then
                                    WriteC(" ", False, New Color("0"), New Color(ColorNum))
                                Else
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                    ColorFilled = False
                                    Console.BackgroundColor = ConsoleColor.Black
                                    Console.Clear()
                                    CoveredPositions.Clear()
                                End If
                            Else
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If Not ResizeSyncing Then
                                    Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                                    Console.Write(" ")
                                Else
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                    ColorFilled = False
                                    Console.BackgroundColor = ConsoleColor.Black
                                    Console.Clear()
                                    CoveredPositions.Clear()
                                End If
                            End If
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", Console.CursorLeft, EndLeft, Console.CursorTop, EndTop)
                            ColorFilled = True
                        End If
                    Else
                        If Not CoveredPositions.Contains(Left & " - " & Top) Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covered position {0}", Left & " - " & Top)
                            CoveredPositions.Add(Left & " - " & Top)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covered positions: {0}/{1}", CoveredPositions.Count, (EndLeft + 1) * (EndTop + 1))
                        End If
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            Console.SetCursorPosition(Left, Top)
                            Console.BackgroundColor = ConsoleColor.Black
                            Console.Write(" ")
                            If CoveredPositions.Count = (EndLeft + 1) * (EndTop + 1) Then
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                ColorFilled = False
                                Console.BackgroundColor = ConsoleColor.Black
                                Console.Clear()
                                CoveredPositions.Clear()
                            End If
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                            ColorFilled = False
                            Console.BackgroundColor = ConsoleColor.Black
                            Console.Clear()
                            CoveredPositions.Clear()
                        End If
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                End If
            Loop
        Catch ex As Exception
            Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg(DebugLevel.I, "All clean. Dissolve screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
