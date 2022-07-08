
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
    Public Module GlitterColorSettings

        Private _glitterColor255Colors As Boolean
        Private _glitterColorTrueColor As Boolean = True
        Private _glitterColorDelay As Integer = 1
        Private _glitterColorMinimumRedColorLevel As Integer = 0
        Private _glitterColorMinimumGreenColorLevel As Integer = 0
        Private _glitterColorMinimumBlueColorLevel As Integer = 0
        Private _glitterColorMinimumColorLevel As Integer = 0
        Private _glitterColorMaximumRedColorLevel As Integer = 255
        Private _glitterColorMaximumGreenColorLevel As Integer = 255
        Private _glitterColorMaximumBlueColorLevel As Integer = 255
        Private _glitterColorMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [GlitterColor] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property GlitterColor255Colors As Boolean
            Get
                Return _glitterColor255Colors
            End Get
            Set(value As Boolean)
                _glitterColor255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property GlitterColorTrueColor As Boolean
            Get
                Return _glitterColorTrueColor
            End Get
            Set(value As Boolean)
                _glitterColorTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property GlitterColorDelay As Integer
            Get
                Return _glitterColorDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1
                _glitterColorDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The minimum red color level (true color)
        ''' </summary>
        Public Property GlitterColorMinimumRedColorLevel As Integer
            Get
                Return _glitterColorMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _glitterColorMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The minimum green color level (true color)
        ''' </summary>
        Public Property GlitterColorMinimumGreenColorLevel As Integer
            Get
                Return _glitterColorMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _glitterColorMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The minimum blue color level (true color)
        ''' </summary>
        Public Property GlitterColorMinimumBlueColorLevel As Integer
            Get
                Return _glitterColorMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _glitterColorMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property GlitterColorMinimumColorLevel As Integer
            Get
                Return _glitterColorMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_glitterColor255Colors Or _glitterColorTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _glitterColorMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The maximum red color level (true color)
        ''' </summary>
        Public Property GlitterColorMaximumRedColorLevel As Integer
            Get
                Return _glitterColorMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _glitterColorMinimumRedColorLevel Then value = _glitterColorMinimumRedColorLevel
                If value > 255 Then value = 255
                _glitterColorMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The maximum green color level (true color)
        ''' </summary>
        Public Property GlitterColorMaximumGreenColorLevel As Integer
            Get
                Return _glitterColorMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _glitterColorMinimumGreenColorLevel Then value = _glitterColorMinimumGreenColorLevel
                If value > 255 Then value = 255
                _glitterColorMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The maximum blue color level (true color)
        ''' </summary>
        Public Property GlitterColorMaximumBlueColorLevel As Integer
            Get
                Return _glitterColorMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _glitterColorMinimumBlueColorLevel Then value = _glitterColorMinimumBlueColorLevel
                If value > 255 Then value = 255
                _glitterColorMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterColor] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property GlitterColorMaximumColorLevel As Integer
            Get
                Return _glitterColorMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_glitterColor255Colors Or _glitterColorTrueColor, 255, 15)
                If value <= _glitterColorMinimumColorLevel Then value = _glitterColorMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _glitterColorMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class GlitterColorDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "GlitterColor" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False

            'Select position
            Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
            Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
            Console.SetCursorPosition(Left, Top)

            'Make a glitter color
            Dim esc As Char = GetEsc()
            If GlitterColorTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(GlitterColorMinimumRedColorLevel, GlitterColorMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(GlitterColorMinimumGreenColorLevel, GlitterColorMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(GlitterColorMinimumBlueColorLevel, GlitterColorMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(ColorStorage, True)
                    Console.Write(" ")
                End If
            ElseIf GlitterColor255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(GlitterColorMinimumColorLevel, GlitterColorMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color(ColorNum), True)
                    Console.Write(" ")
                End If
            Else
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.BackgroundColor = colors(RandomDriver.Next(GlitterColorMinimumColorLevel, GlitterColorMaximumColorLevel))
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    Console.Write(" ")
                End If
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(GlitterColorDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
