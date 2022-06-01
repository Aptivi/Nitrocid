
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
    Module LinesDisplay

        Public Lines As New KernelThread("Lines screensaver thread", True, AddressOf Lines_DoWork)

        ''' <summary>
        ''' Handles the code of Lines
        ''' </summary>
        Sub Lines_DoWork()
            Try
                'Variables
                Dim random As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If LinesTrueColor Or Lines255Colors Then
                    LinesMinimumRedColorLevel = If(LinesMinimumRedColorLevel >= 0 And LinesMinimumRedColorLevel <= 255, LinesMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", LinesMinimumRedColorLevel)
                    LinesMinimumGreenColorLevel = If(LinesMinimumGreenColorLevel >= 0 And LinesMinimumGreenColorLevel <= 255, LinesMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", LinesMinimumGreenColorLevel)
                    LinesMinimumBlueColorLevel = If(LinesMinimumBlueColorLevel >= 0 And LinesMinimumBlueColorLevel <= 255, LinesMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", LinesMinimumBlueColorLevel)
                    LinesMinimumColorLevel = If(LinesMinimumColorLevel >= 0 And LinesMinimumColorLevel <= 255, LinesMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", LinesMinimumColorLevel)
                    LinesMaximumRedColorLevel = If(LinesMaximumRedColorLevel >= 0 And LinesMaximumRedColorLevel <= 255, LinesMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", LinesMaximumRedColorLevel)
                    LinesMaximumGreenColorLevel = If(LinesMaximumGreenColorLevel >= 0 And LinesMaximumGreenColorLevel <= 255, LinesMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", LinesMaximumGreenColorLevel)
                    LinesMaximumBlueColorLevel = If(LinesMaximumBlueColorLevel >= 0 And LinesMaximumBlueColorLevel <= 255, LinesMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", LinesMaximumBlueColorLevel)
                    LinesMaximumColorLevel = If(LinesMaximumColorLevel >= 0 And LinesMaximumColorLevel <= 255, LinesMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", LinesMaximumColorLevel)
                Else
                    LinesMinimumColorLevel = If(LinesMinimumColorLevel >= 0 And LinesMinimumColorLevel <= 15, LinesMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", LinesMinimumColorLevel)
                    LinesMaximumColorLevel = If(LinesMaximumColorLevel >= 0 And LinesMaximumColorLevel <= 15, LinesMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", LinesMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    'Select a color
                    Dim esc As Char = GetEsc()
                    If LinesTrueColor Then
                        SetConsoleColor(New Color(LinesBackgroundColor), True)
                        Console.Clear()
                        Dim RedColorNum As Integer = random.Next(LinesMinimumRedColorLevel, LinesMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = random.Next(LinesMinimumGreenColorLevel, LinesMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = random.Next(LinesMinimumBlueColorLevel, LinesMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        SetConsoleColor(ColorStorage)
                    ElseIf Lines255Colors Then
                        SetConsoleColor(New Color(LinesBackgroundColor), True)
                        Console.Clear()
                        Dim color As Integer = random.Next(LinesMinimumColorLevel, LinesMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", color)
                        SetConsoleColor(New Color(color))
                    Else
                        Console.Clear()
                        SetConsoleColor(New Color(LinesBackgroundColor), True)
                        Console.ForegroundColor = colors(random.Next(LinesMinimumColorLevel, LinesMaximumColorLevel))
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.ForegroundColor)
                    End If

                    'Draw a line
                    Dim Line As String = ""
                    Dim Top As Integer = New Random().Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top)
                    For i As Integer = 1 To Console.WindowWidth
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Forming line using {0} or the default ""-""...", LinesLineChar)
                        Line += If(Not String.IsNullOrWhiteSpace(LinesLineChar), LinesLineChar, "-")
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Line: {0}", Line)
                    Next
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        Console.SetCursorPosition(0, Top)
                        Console.WriteLine(Line)
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(LinesDelay, Lines)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
