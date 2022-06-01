
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

Imports System.Threading

Namespace Misc.Screensaver.Displays
    Module DissolveDisplay

        Public Dissolve As New KernelThread("Dissolve screensaver thread", True, AddressOf Dissolve_DoWork)

        ''' <summary>
        ''' Handles the code of Dissolve
        ''' </summary>
        Sub Dissolve_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim ColorFilled As Boolean
                Dim CoveredPositions As New ArrayList
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(DissolveBackgroundColor), True)
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If DissolveTrueColor Or Dissolve255Colors Then
                    DissolveMinimumRedColorLevel = If(DissolveMinimumRedColorLevel >= 0 And DissolveMinimumRedColorLevel <= 255, DissolveMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", DissolveMinimumRedColorLevel)
                    DissolveMinimumGreenColorLevel = If(DissolveMinimumGreenColorLevel >= 0 And DissolveMinimumGreenColorLevel <= 255, DissolveMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", DissolveMinimumGreenColorLevel)
                    DissolveMinimumBlueColorLevel = If(DissolveMinimumBlueColorLevel >= 0 And DissolveMinimumBlueColorLevel <= 255, DissolveMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", DissolveMinimumBlueColorLevel)
                    DissolveMinimumColorLevel = If(DissolveMinimumColorLevel >= 0 And DissolveMinimumColorLevel <= 255, DissolveMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", DissolveMinimumColorLevel)
                    DissolveMaximumRedColorLevel = If(DissolveMaximumRedColorLevel >= 0 And DissolveMaximumRedColorLevel <= 255, DissolveMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", DissolveMaximumRedColorLevel)
                    DissolveMaximumGreenColorLevel = If(DissolveMaximumGreenColorLevel >= 0 And DissolveMaximumGreenColorLevel <= 255, DissolveMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", DissolveMaximumGreenColorLevel)
                    DissolveMaximumBlueColorLevel = If(DissolveMaximumBlueColorLevel >= 0 And DissolveMaximumBlueColorLevel <= 255, DissolveMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", DissolveMaximumBlueColorLevel)
                    DissolveMaximumColorLevel = If(DissolveMaximumColorLevel >= 0 And DissolveMaximumColorLevel <= 255, DissolveMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", DissolveMaximumColorLevel)
                Else
                    DissolveMinimumColorLevel = If(DissolveMinimumColorLevel >= 0 And DissolveMinimumColorLevel <= 15, DissolveMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", DissolveMinimumColorLevel)
                    DissolveMaximumColorLevel = If(DissolveMaximumColorLevel >= 0 And DissolveMaximumColorLevel <= 15, DissolveMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", DissolveMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    If ColorFilled Then Thread.Sleep(1)
                    Dim EndLeft As Integer = Console.WindowWidth - 1
                    Dim EndTop As Integer = Console.WindowHeight - 1
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", ColorFilled)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", Left, Top)

                    'Fill the color if not filled
                    If Not ColorFilled Then
                        'NOTICE: Mono seems to have a bug in Console.CursorLeft and Console.CursorTop when printing with VT escape sequences. For info, seek EB#2:7.
                        If Not (Console.CursorLeft >= EndLeft And Console.CursorTop >= EndTop) Then
                            Dim esc As Char = GetEsc()
                            If DissolveTrueColor Then
                                Dim RedColorNum As Integer = RandomDriver.Next(DissolveMinimumRedColorLevel, DissolveMaximumRedColorLevel)
                                Dim GreenColorNum As Integer = RandomDriver.Next(DissolveMinimumGreenColorLevel, DissolveMaximumGreenColorLevel)
                                Dim BlueColorNum As Integer = RandomDriver.Next(DissolveMinimumBlueColorLevel, DissolveMaximumBlueColorLevel)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If Not ResizeSyncing Then
                                    SetConsoleColor(New Color("0;0;0"))
                                    SetConsoleColor(New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), True)
                                    Console.Write(" ")
                                Else
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                    ColorFilled = False
                                    SetConsoleColor(New Color(DissolveBackgroundColor), True)
                                    Console.Clear()
                                    CoveredPositions.Clear()
                                End If
                            ElseIf Dissolve255Colors Then
                                Dim ColorNum As Integer = RandomDriver.Next(DissolveMinimumColorLevel, DissolveMaximumColorLevel)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If Not ResizeSyncing Then
                                    SetConsoleColor(New Color("0"))
                                    SetConsoleColor(New Color(ColorNum), True)
                                    Console.Write(" ")
                                Else
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                    ColorFilled = False
                                    SetConsoleColor(New Color(DissolveBackgroundColor), True)
                                    Console.Clear()
                                    CoveredPositions.Clear()
                                End If
                            Else
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If Not ResizeSyncing Then
                                    SetConsoleColor(New Color(DissolveBackgroundColor), True)
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                                    Console.Write(" ")
                                Else
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                    ColorFilled = False
                                    SetConsoleColor(New Color(DissolveBackgroundColor), True)
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
                            SetConsoleColor(New Color(DissolveBackgroundColor), True)
                            Console.Write(" ")
                            If CoveredPositions.Count = (EndLeft + 1) * (EndTop + 1) Then
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                                ColorFilled = False
                                SetConsoleColor(New Color(DissolveBackgroundColor), True)
                                Console.Clear()
                                CoveredPositions.Clear()
                            End If
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                            ColorFilled = False
                            SetConsoleColor(New Color(DissolveBackgroundColor), True)
                            Console.Clear()
                            CoveredPositions.Clear()
                        End If
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
