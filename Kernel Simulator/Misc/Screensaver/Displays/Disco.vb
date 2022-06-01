
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
    Module DiscoDisplay

        Public WithEvents Disco As New KernelThread("Disco screensaver thread", True, AddressOf Disco_DoWork)

        ''' <summary>
        ''' Handles the code of Disco
        ''' </summary>
        Sub Disco_DoWork()
            Try
                'Variables
                Dim MaximumColors As Integer = If(DiscoMaximumColorLevel >= 0 And DiscoMaximumColorLevel <= 255, DiscoMaximumColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", MaximumColors)
                Dim MaximumColorsR As Integer = If(DiscoMaximumRedColorLevel >= 0 And DiscoMaximumRedColorLevel <= 255, DiscoMaximumRedColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", MaximumColorsR)
                Dim MaximumColorsG As Integer = If(DiscoMaximumGreenColorLevel >= 0 And DiscoMaximumGreenColorLevel <= 255, DiscoMaximumGreenColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", MaximumColorsG)
                Dim MaximumColorsB As Integer = If(DiscoMaximumBlueColorLevel >= 0 And DiscoMaximumBlueColorLevel <= 255, DiscoMaximumBlueColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", MaximumColorsB)
                Dim CurrentColor As Integer = 0
                Dim CurrentColorR, CurrentColorG, CurrentColorB As Integer
                Dim FedColors As ConsoleColors() = {ConsoleColors.Black, ConsoleColors.White}
                Dim random As New Random()

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False

                    'Select the background color
                    Dim esc As Char = GetEsc()
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", DiscoCycleColors)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "fed (future-eyes-destroyer) mode: {0}", DiscoEnableFedMode)
                    If Not DiscoEnableFedMode Then
                        If DiscoTrueColor Then
                            If Not DiscoCycleColors Then
                                Dim RedColorNum As Integer = random.Next(255)
                                Dim GreenColorNum As Integer = random.Next(255)
                                Dim BlueColorNum As Integer = random.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                                Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                                SetConsoleColor(ColorStorage, True)
                            Else
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB)
                                Dim ColorStorage As New Color(CurrentColorR, CurrentColorG, CurrentColorB)
                                SetConsoleColor(ColorStorage, True)
                            End If
                        ElseIf Disco255Colors Then
                            If Not DiscoCycleColors Then
                                Dim color As Integer = random.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", color)
                                SetConsoleColor(New Color(color), True)
                            Else
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor)
                                SetConsoleColor(New Color(CurrentColor), True)
                            End If
                        Else
                            If Not DiscoCycleColors Then
                                Console.BackgroundColor = colors(random.Next(colors.Length - 1))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            Else
                                MaximumColors = If(DiscoMaximumColorLevel >= 0 And DiscoMaximumColorLevel <= 15, DiscoMaximumColorLevel, 15)
                                Console.BackgroundColor = colors(CurrentColor)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            End If
                        End If
                    Else
                        If CurrentColor = ConsoleColors.Black Then
                            CurrentColor = ConsoleColors.White
                        Else
                            CurrentColor = ConsoleColors.Black
                        End If
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor)
                        SetConsoleColor(New Color(CurrentColor), True)
                    End If

                    'Make the disco effect!
                    Console.Clear()

                    'Switch to the next color
                    If DiscoTrueColor Then
                        If CurrentColorR >= MaximumColorsR Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Red level exceeded maximum color. {0} >= {1}", CurrentColorR, MaximumColorsR)
                            CurrentColorR = 0
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one (R)...")
                            CurrentColorR += 1
                        End If
                        If CurrentColorG >= MaximumColorsG Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Green level exceeded maximum color. {0} >= {1}", CurrentColorG, MaximumColorsG)
                            CurrentColorG = 0
                        ElseIf CurrentColorR = 0 Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one (G)...")
                            CurrentColorG += 1
                        End If
                        If CurrentColorB >= MaximumColorsB Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Blue level exceeded maximum color. {0} >= {1}", CurrentColorB, MaximumColorsB)
                            CurrentColorB = 0
                        ElseIf CurrentColorG = 0 And CurrentColorR = 0 Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one (B)...")
                            CurrentColorB += 1
                        End If
                        If CurrentColorB = 0 And CurrentColorG = 0 And CurrentColorR = 0 Then
                            CurrentColorB = 0
                            CurrentColorG = 0
                            CurrentColorR = 0
                        End If
                    Else
                        If CurrentColor >= MaximumColors Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color level exceeded maximum color. {0} >= {1}", CurrentColor, MaximumColors)
                            CurrentColor = 0
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one...")
                            CurrentColor += 1
                        End If
                    End If

                    'Check to see if we're dealing with beats per minute
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Using BPM: {0}", DiscoUseBeatsPerMinute)
                    If DiscoUseBeatsPerMinute Then
                        Dim BeatInterval As Integer = 60000 / DiscoDelay
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1} ms", DiscoDelay, BeatInterval)
                        SleepNoBlock(BeatInterval, Disco)
                    Else
                        SleepNoBlock(DiscoDelay, Disco)
                    End If
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
