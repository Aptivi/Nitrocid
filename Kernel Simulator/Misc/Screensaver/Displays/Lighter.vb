
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
    Module LighterDisplay

        Public Lighter As New KernelThread("Lighter screensaver thread", True, AddressOf Lighter_DoWork)

        ''' <summary>
        ''' Handles the code of Lighter
        ''' </summary>
        Sub Lighter_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CoveredPositions As New ArrayList
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(LighterBackgroundColor), True)
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If LighterTrueColor Or Lighter255Colors Then
                    LighterMinimumRedColorLevel = If(LighterMinimumRedColorLevel >= 0 And LighterMinimumRedColorLevel <= 255, LighterMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", LighterMinimumRedColorLevel)
                    LighterMinimumGreenColorLevel = If(LighterMinimumGreenColorLevel >= 0 And LighterMinimumGreenColorLevel <= 255, LighterMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", LighterMinimumGreenColorLevel)
                    LighterMinimumBlueColorLevel = If(LighterMinimumBlueColorLevel >= 0 And LighterMinimumBlueColorLevel <= 255, LighterMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", LighterMinimumBlueColorLevel)
                    LighterMinimumColorLevel = If(LighterMinimumColorLevel >= 0 And LighterMinimumColorLevel <= 255, LighterMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", LighterMinimumColorLevel)
                    LighterMaximumRedColorLevel = If(LighterMaximumRedColorLevel >= 0 And LighterMaximumRedColorLevel <= 255, LighterMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", LighterMaximumRedColorLevel)
                    LighterMaximumGreenColorLevel = If(LighterMaximumGreenColorLevel >= 0 And LighterMaximumGreenColorLevel <= 255, LighterMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", LighterMaximumGreenColorLevel)
                    LighterMaximumBlueColorLevel = If(LighterMaximumBlueColorLevel >= 0 And LighterMaximumBlueColorLevel <= 255, LighterMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", LighterMaximumBlueColorLevel)
                    LighterMaximumColorLevel = If(LighterMaximumColorLevel >= 0 And LighterMaximumColorLevel <= 255, LighterMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", LighterMaximumColorLevel)
                Else
                    LighterMinimumColorLevel = If(LighterMinimumColorLevel >= 0 And LighterMinimumColorLevel <= 15, LighterMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", LighterMinimumColorLevel)
                    LighterMaximumColorLevel = If(LighterMaximumColorLevel >= 0 And LighterMaximumColorLevel <= 15, LighterMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", LighterMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    'Select a position
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                    Console.SetCursorPosition(Left, Top)
                    If Not CoveredPositions.Contains(Left & ";" & Top) Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covering position...")
                        CoveredPositions.Add(Left & ";" & Top)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Position covered. Covered positions: {0}", CoveredPositions.Count)
                    End If

                    'Select a color and write the space
                    Dim esc As Char = GetEsc()
                    If LighterTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(LighterMinimumRedColorLevel, LighterMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(LighterMinimumGreenColorLevel, LighterMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(LighterMinimumBlueColorLevel, LighterMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(ColorStorage, True)
                            Console.Write(" ")
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                            CoveredPositions.Clear()
                        End If
                    ElseIf Lighter255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(LighterMinimumColorLevel, LighterMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(New Color(ColorNum), True)
                            Console.Write(" ")
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                            CoveredPositions.Clear()
                        End If
                    Else
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            Console.BackgroundColor = colors(RandomDriver.Next(LighterMinimumColorLevel, LighterMaximumColorLevel))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            Console.Write(" ")
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                            CoveredPositions.Clear()
                        End If
                    End If

                    'Simulate a trail effect
                    If CoveredPositions.Count = LighterMaxPositions Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covered positions exceeded max positions of {0}", LighterMaxPositions)
                        Dim WipeLeft As Integer = CoveredPositions(0).ToString.Substring(0, CoveredPositions(0).ToString.IndexOf(";"))
                        Dim WipeTop As Integer = CoveredPositions(0).ToString.Substring(CoveredPositions(0).ToString.IndexOf(";") + 1)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Wiping in {0}, {1}...", WipeLeft, WipeTop)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            Console.SetCursorPosition(WipeLeft, WipeTop)
                            SetConsoleColor(New Color(LighterBackgroundColor), True)
                            Console.Write(" ")
                            CoveredPositions.RemoveAt(0)
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                            CoveredPositions.Clear()
                        End If
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(LighterDelay, Lighter)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
