
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
    Public Module FlashColorDisplay

        Friend FlashColor As New KernelThread("FlashColor screensaver thread", True, AddressOf FlashColor_DoWork)
        Private _flashColor255Colors As Boolean
        Private _flashColorTrueColor As Boolean = True
        Private _flashColorDelay As Integer = 20
        Private _flashColorBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _flashColorMinimumRedColorLevel As Integer = 0
        Private _flashColorMinimumGreenColorLevel As Integer = 0
        Private _flashColorMinimumBlueColorLevel As Integer = 0
        Private _flashColorMinimumColorLevel As Integer = 0
        Private _flashColorMaximumRedColorLevel As Integer = 255
        Private _flashColorMaximumGreenColorLevel As Integer = 255
        Private _flashColorMaximumBlueColorLevel As Integer = 255
        Private _flashColorMaximumColorLevel As Integer = 0

        ''' <summary>
        ''' [FlashColor] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property FlashColor255Colors As Boolean
            Get
                Return _flashColor255Colors
            End Get
            Set(value As Boolean)
                _flashColor255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FlashColorTrueColor As Boolean
            Get
                Return _flashColorTrueColor
            End Get
            Set(value As Boolean)
                _flashColorTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FlashColorDelay As Integer
            Get
                Return _flashColorDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _flashColorDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] Screensaver background color
        ''' </summary>
        Public Property FlashColorBackgroundColor As String
            Get
                Return _flashColorBackgroundColor
            End Get
            Set(value As String)
                _flashColorBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum red color level (true color)
        ''' </summary>
        Public Property FlashColorMinimumRedColorLevel As Integer
            Get
                Return _flashColorMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashColorMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum green color level (true color)
        ''' </summary>
        Public Property FlashColorMinimumGreenColorLevel As Integer
            Get
                Return _flashColorMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashColorMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum blue color level (true color)
        ''' </summary>
        Public Property FlashColorMinimumBlueColorLevel As Integer
            Get
                Return _flashColorMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashColorMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashColorMinimumColorLevel As Integer
            Get
                Return _flashColorMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_flashColor255Colors Or _flashColorTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _flashColorMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum red color level (true color)
        ''' </summary>
        Public Property FlashColorMaximumRedColorLevel As Integer
            Get
                Return _flashColorMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashColorMinimumRedColorLevel Then value = _flashColorMinimumRedColorLevel
                If value > 255 Then value = 255
                _flashColorMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum green color level (true color)
        ''' </summary>
        Public Property FlashColorMaximumGreenColorLevel As Integer
            Get
                Return _flashColorMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashColorMinimumGreenColorLevel Then value = _flashColorMinimumGreenColorLevel
                If value > 255 Then value = 255
                _flashColorMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum blue color level (true color)
        ''' </summary>
        Public Property FlashColorMaximumBlueColorLevel As Integer
            Get
                Return _flashColorMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashColorMinimumBlueColorLevel Then value = _flashColorMinimumBlueColorLevel
                If value > 255 Then value = 255
                _flashColorMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashColorMaximumColorLevel As Integer
            Get
                Return _flashColorMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_flashColor255Colors Or _flashColorTrueColor, 255, 15)
                If value <= _flashColorMinimumColorLevel Then value = _flashColorMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _flashColorMaximumColorLevel = value
            End Set
        End Property

        ''' <summary>
        ''' Handles the code of Flash Colors
        ''' </summary>
        Sub FlashColor_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(FlashColorBackgroundColor), True)
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If FlashColorTrueColor Or FlashColor255Colors Then
                    FlashColorMinimumRedColorLevel = If(FlashColorMinimumRedColorLevel >= 0 And FlashColorMinimumRedColorLevel <= 255, FlashColorMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", FlashColorMinimumRedColorLevel)
                    FlashColorMinimumGreenColorLevel = If(FlashColorMinimumGreenColorLevel >= 0 And FlashColorMinimumGreenColorLevel <= 255, FlashColorMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", FlashColorMinimumGreenColorLevel)
                    FlashColorMinimumBlueColorLevel = If(FlashColorMinimumBlueColorLevel >= 0 And FlashColorMinimumBlueColorLevel <= 255, FlashColorMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", FlashColorMinimumBlueColorLevel)
                    FlashColorMinimumColorLevel = If(FlashColorMinimumColorLevel >= 0 And FlashColorMinimumColorLevel <= 255, FlashColorMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FlashColorMinimumColorLevel)
                    FlashColorMaximumRedColorLevel = If(FlashColorMaximumRedColorLevel >= 0 And FlashColorMaximumRedColorLevel <= 255, FlashColorMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", FlashColorMaximumRedColorLevel)
                    FlashColorMaximumGreenColorLevel = If(FlashColorMaximumGreenColorLevel >= 0 And FlashColorMaximumGreenColorLevel <= 255, FlashColorMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", FlashColorMaximumGreenColorLevel)
                    FlashColorMaximumBlueColorLevel = If(FlashColorMaximumBlueColorLevel >= 0 And FlashColorMaximumBlueColorLevel <= 255, FlashColorMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", FlashColorMaximumBlueColorLevel)
                    FlashColorMaximumColorLevel = If(FlashColorMaximumColorLevel >= 0 And FlashColorMaximumColorLevel <= 255, FlashColorMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FlashColorMaximumColorLevel)
                Else
                    FlashColorMinimumColorLevel = If(FlashColorMinimumColorLevel >= 0 And FlashColorMinimumColorLevel <= 15, FlashColorMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FlashColorMinimumColorLevel)
                    FlashColorMaximumColorLevel = If(FlashColorMaximumColorLevel >= 0 And FlashColorMaximumColorLevel <= 15, FlashColorMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FlashColorMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    'Select position
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                    Console.SetCursorPosition(Left, Top)

                    'Make a flash color
                    Dim esc As Char = GetEsc()
                    Console.BackgroundColor = ConsoleColor.Black
                    ClearKeepPosition()
                    If FlashColorTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(FlashColorMinimumRedColorLevel, FlashColorMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(FlashColorMinimumGreenColorLevel, FlashColorMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(FlashColorMinimumBlueColorLevel, FlashColorMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(ColorStorage, True)
                            Console.Write(" ")
                        End If
                    ElseIf FlashColor255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(FlashColorMinimumColorLevel, FlashColorMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(New Color(ColorNum), True)
                            Console.Write(" ")
                        End If
                    Else
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            Console.BackgroundColor = colors(RandomDriver.Next(FlashColorMinimumColorLevel, FlashColorMaximumColorLevel))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            Console.Write(" ")
                        End If
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(FlashColorDelay, FlashColor)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace