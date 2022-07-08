
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
    Public Module FlashTextSettings

        Private _flashText255Colors As Boolean
        Private _flashTextTrueColor As Boolean = True
        Private _flashTextDelay As Integer = 20
        Private _flashTextWrite As String = "Kernel Simulator"
        Private _flashTextBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _flashTextMinimumRedColorLevel As Integer = 0
        Private _flashTextMinimumGreenColorLevel As Integer = 0
        Private _flashTextMinimumBlueColorLevel As Integer = 0
        Private _flashTextMinimumColorLevel As Integer = 0
        Private _flashTextMaximumRedColorLevel As Integer = 255
        Private _flashTextMaximumGreenColorLevel As Integer = 255
        Private _flashTextMaximumBlueColorLevel As Integer = 255
        Private _flashTextMaximumColorLevel As Integer = 0

        ''' <summary>
        ''' [FlashText] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property FlashText255Colors As Boolean
            Get
                Return _flashText255Colors
            End Get
            Set(value As Boolean)
                _flashText255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FlashTextTrueColor As Boolean
            Get
                Return _flashTextTrueColor
            End Get
            Set(value As Boolean)
                _flashTextTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FlashTextDelay As Integer
            Get
                Return _flashTextDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _flashTextDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] Text for FlashText. Shorter is better.
        ''' </summary>
        Public Property FlashTextWrite As String
            Get
                Return _flashTextWrite
            End Get
            Set(value As String)
                _flashTextWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] Screensaver background color
        ''' </summary>
        Public Property FlashTextBackgroundColor As String
            Get
                Return _flashTextBackgroundColor
            End Get
            Set(value As String)
                _flashTextBackgroundColor = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum red color level (true color)
        ''' </summary>
        Public Property FlashTextMinimumRedColorLevel As Integer
            Get
                Return _flashTextMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashTextMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum green color level (true color)
        ''' </summary>
        Public Property FlashTextMinimumGreenColorLevel As Integer
            Get
                Return _flashTextMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashTextMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum blue color level (true color)
        ''' </summary>
        Public Property FlashTextMinimumBlueColorLevel As Integer
            Get
                Return _flashTextMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashTextMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashTextMinimumColorLevel As Integer
            Get
                Return _flashTextMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_flashText255Colors Or _flashTextTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _flashTextMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum red color level (true color)
        ''' </summary>
        Public Property FlashTextMaximumRedColorLevel As Integer
            Get
                Return _flashTextMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashTextMinimumRedColorLevel Then value = _flashTextMinimumRedColorLevel
                If value > 255 Then value = 255
                _flashTextMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum green color level (true color)
        ''' </summary>
        Public Property FlashTextMaximumGreenColorLevel As Integer
            Get
                Return _flashTextMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashTextMinimumGreenColorLevel Then value = _flashTextMinimumGreenColorLevel
                If value > 255 Then value = 255
                _flashTextMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum blue color level (true color)
        ''' </summary>
        Public Property FlashTextMaximumBlueColorLevel As Integer
            Get
                Return _flashTextMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashTextMinimumBlueColorLevel Then value = _flashTextMinimumBlueColorLevel
                If value > 255 Then value = 255
                _flashTextMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashTextMaximumColorLevel As Integer
            Get
                Return _flashTextMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_flashText255Colors Or _flashTextTrueColor, 255, 15)
                If value <= _flashTextMinimumColorLevel Then value = _flashTextMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _flashTextMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class FlashTextDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private Left, Top As Integer

        Public Overrides Property ScreensaverName As String = "FlashText" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(FlashTextBackgroundColor), True)
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

            'Select position
            Left = RandomDriver.Next(Console.WindowWidth)
            Top = RandomDriver.Next(Console.WindowHeight)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False

            'Make two delay halves to make up one half for screen with text and one half for screen with no text to make a flashing effect
            Dim HalfDelay As Integer = FlashTextDelay / 2

            'Make a flashing text
            Dim esc As Char = GetEsc()
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            If FlashTextTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(FlashTextMinimumRedColorLevel, FlashTextMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(FlashTextMinimumGreenColorLevel, FlashTextMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(FlashTextMinimumBlueColorLevel, FlashTextMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    WriteWhere(FlashTextWrite, Left, Top, True, ColorStorage)
                End If
            ElseIf FlashText255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(FlashTextMinimumColorLevel, FlashTextMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    WriteWhere(FlashTextWrite, Left, Top, True, New Color(ColorNum))
                End If
            Else
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    WriteWhere(FlashTextWrite, Left, Top, True, colors(RandomDriver.Next(FlashTextMinimumColorLevel, FlashTextMaximumColorLevel)))
                End If
            End If
            SleepNoBlock(HalfDelay, ScreensaverDisplayerThread)
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            SleepNoBlock(HalfDelay, ScreensaverDisplayerThread)

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
        End Sub

    End Class
End Namespace