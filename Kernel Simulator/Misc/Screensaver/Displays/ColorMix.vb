
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
    Public Module ColorMixSettings

        Private _colorMix255Colors As Boolean
        Private _colorMixTrueColor As Boolean = True
        Private _colorMixDelay As Integer = 1
        Private _colorMixBackgroundColor As String = New Color(ConsoleColor.Red).PlainSequence
        Private _colorMixMinimumRedColorLevel As Integer = 0
        Private _colorMixMinimumGreenColorLevel As Integer = 0
        Private _colorMixMinimumBlueColorLevel As Integer = 0
        Private _colorMixMinimumColorLevel As Integer = 0
        Private _colorMixMaximumRedColorLevel As Integer = 255
        Private _colorMixMaximumGreenColorLevel As Integer = 255
        Private _colorMixMaximumBlueColorLevel As Integer = 255
        Private _colorMixMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [ColorMix] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property ColorMix255Colors As Boolean
            Get
                Return _colorMix255Colors
            End Get
            Set(value As Boolean)
                _colorMix255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property ColorMixTrueColor As Boolean
            Get
                Return _colorMixTrueColor
            End Get
            Set(value As Boolean)
                _colorMixTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property ColorMixDelay As Integer
            Get
                Return _colorMixDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1
                _colorMixDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] Screensaver background color
        ''' </summary>
        Public Property ColorMixBackgroundColor As String
            Get
                Return _colorMixBackgroundColor
            End Get
            Set(value As String)
                _colorMixBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The minimum red color level (true color)
        ''' </summary>
        Public Property ColorMixMinimumRedColorLevel As Integer
            Get
                Return _colorMixMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _colorMixMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The minimum green color level (true color)
        ''' </summary>
        Public Property ColorMixMinimumGreenColorLevel As Integer
            Get
                Return _colorMixMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _colorMixMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The minimum blue color level (true color)
        ''' </summary>
        Public Property ColorMixMinimumBlueColorLevel As Integer
            Get
                Return _colorMixMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _colorMixMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property ColorMixMinimumColorLevel As Integer
            Get
                Return _colorMixMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_colorMix255Colors Or _colorMixTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _colorMixMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The maximum red color level (true color)
        ''' </summary>
        Public Property ColorMixMaximumRedColorLevel As Integer
            Get
                Return _colorMixMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _colorMixMinimumRedColorLevel Then value = _colorMixMinimumRedColorLevel
                If value > 255 Then value = 255
                _colorMixMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The maximum green color level (true color)
        ''' </summary>
        Public Property ColorMixMaximumGreenColorLevel As Integer
            Get
                Return _colorMixMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _colorMixMinimumGreenColorLevel Then value = _colorMixMinimumGreenColorLevel
                If value > 255 Then value = 255
                _colorMixMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The maximum blue color level (true color)
        ''' </summary>
        Public Property ColorMixMaximumBlueColorLevel As Integer
            Get
                Return _colorMixMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _colorMixMinimumBlueColorLevel Then value = _colorMixMinimumBlueColorLevel
                If value > 255 Then value = 255
                _colorMixMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ColorMix] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property ColorMixMaximumColorLevel As Integer
            Get
                Return _colorMixMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_colorMix255Colors Or _colorMixTrueColor, 255, 15)
                If value <= _colorMixMinimumColorLevel Then value = _colorMixMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _colorMixMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class ColorMixDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "ColorMix" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(ColorMixBackgroundColor), True)
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            'Set colors
            If ColorMixTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(ColorMixMinimumRedColorLevel, ColorMixMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(ColorMixMinimumGreenColorLevel, ColorMixMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(ColorMixMinimumBlueColorLevel, ColorMixMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(ColorStorage, True)
                    Console.Write(" ")
                End If
            ElseIf ColorMix255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(ColorMixMinimumColorLevel, ColorMixMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color(ColorNum), True)
                    Console.Write(" ")
                End If
            Else
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.BackgroundColor = CType(RandomDriver.Next(ColorMixMinimumColorLevel, ColorMixMaximumColorLevel), ConsoleColor)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    Console.Write(" ")
                End If
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(ColorMixDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
