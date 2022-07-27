
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

Namespace Misc.Screensaver.Displays
    Public Module BeatEdgePulseSettings

        Private _beatedgepulse255Colors As Boolean
        Private _beatedgepulseTrueColor As Boolean = True
        Private _beatedgepulseCycleColors As Boolean = True
        Private _beatedgepulseBeatColor As String = 17
        Private _beatedgepulseDelay As Integer = 50
        Private _beatedgepulseMaxSteps As Integer = 25
        Private _beatedgepulseMinimumRedColorLevel As Integer = 0
        Private _beatedgepulseMinimumGreenColorLevel As Integer = 0
        Private _beatedgepulseMinimumBlueColorLevel As Integer = 0
        Private _beatedgepulseMinimumColorLevel As Integer = 0
        Private _beatedgepulseMaximumRedColorLevel As Integer = 255
        Private _beatedgepulseMaximumGreenColorLevel As Integer = 255
        Private _beatedgepulseMaximumBlueColorLevel As Integer = 255
        Private _beatedgepulseMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [BeatEdgePulse] Enable 255 color support. Has a higher priority than 16 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatEdgePulse255Colors As Boolean
            Get
                Return _beatedgepulse255Colors
            End Get
            Set(value As Boolean)
                _beatedgepulse255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatEdgePulseTrueColor As Boolean
            Get
                Return _beatedgepulseTrueColor
            End Get
            Set(value As Boolean)
                _beatedgepulseTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatEdgePulseBeatColor"/> color.)
        ''' </summary>
        Public Property BeatEdgePulseCycleColors As Boolean
            Get
                Return _beatedgepulseCycleColors
            End Get
            Set(value As Boolean)
                _beatedgepulseCycleColors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property BeatEdgePulseBeatColor As String
            Get
                Return _beatedgepulseBeatColor
            End Get
            Set(value As String)
                _beatedgepulseBeatColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property BeatEdgePulseDelay As Integer
            Get
                Return _beatedgepulseDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _beatedgepulseDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] How many fade steps to do?
        ''' </summary>
        Public Property BeatEdgePulseMaxSteps As Integer
            Get
                Return _beatedgepulseMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _beatedgepulseMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The minimum red color level (true color)
        ''' </summary>
        Public Property BeatEdgePulseMinimumRedColorLevel As Integer
            Get
                Return _beatedgepulseMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatedgepulseMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The minimum green color level (true color)
        ''' </summary>
        Public Property BeatEdgePulseMinimumGreenColorLevel As Integer
            Get
                Return _beatedgepulseMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatedgepulseMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The minimum blue color level (true color)
        ''' </summary>
        Public Property BeatEdgePulseMinimumBlueColorLevel As Integer
            Get
                Return _beatedgepulseMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatedgepulseMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatEdgePulseMinimumColorLevel As Integer
            Get
                Return _beatedgepulseMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_beatedgepulse255Colors Or _beatedgepulseTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _beatedgepulseMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The maximum red color level (true color)
        ''' </summary>
        Public Property BeatEdgePulseMaximumRedColorLevel As Integer
            Get
                Return _beatedgepulseMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatedgepulseMinimumRedColorLevel Then value = _beatedgepulseMinimumRedColorLevel
                If value > 255 Then value = 255
                _beatedgepulseMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The maximum green color level (true color)
        ''' </summary>
        Public Property BeatEdgePulseMaximumGreenColorLevel As Integer
            Get
                Return _beatedgepulseMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatedgepulseMinimumGreenColorLevel Then value = _beatedgepulseMinimumGreenColorLevel
                If value > 255 Then value = 255
                _beatedgepulseMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The maximum blue color level (true color)
        ''' </summary>
        Public Property BeatEdgePulseMaximumBlueColorLevel As Integer
            Get
                Return _beatedgepulseMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatedgepulseMinimumBlueColorLevel Then value = _beatedgepulseMinimumBlueColorLevel
                If value > 255 Then value = 255
                _beatedgepulseMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatEdgePulse] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatEdgePulseMaximumColorLevel As Integer
            Get
                Return _beatedgepulseMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_beatedgepulse255Colors Or _beatedgepulseTrueColor, 255, 15)
                If value <= _beatedgepulseMinimumColorLevel Then value = _beatedgepulseMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _beatedgepulseMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class BeatEdgePulseDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private BeatEdgePulseSettingsInstance As Animations.BeatEdgePulse.BeatEdgePulseSettings
        Private RandomDriver As Random

        Public Overrides Property ScreensaverName As String = "BeatEdgePulse" Implements IScreensaver.ScreensaverName

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
            BeatEdgePulseSettingsInstance = New Animations.BeatEdgePulse.BeatEdgePulseSettings With {
                .BeatEdgePulse255Colors = BeatEdgePulse255Colors,
                .BeatEdgePulseTrueColor = BeatEdgePulseTrueColor,
                .BeatEdgePulseBeatColor = BeatEdgePulseBeatColor,
                .BeatEdgePulseDelay = BeatEdgePulseDelay,
                .BeatEdgePulseMaxSteps = BeatEdgePulseMaxSteps,
                .BeatEdgePulseCycleColors = BeatEdgePulseCycleColors,
                .BeatEdgePulseMinimumRedColorLevel = BeatEdgePulseMinimumRedColorLevel,
                .BeatEdgePulseMinimumGreenColorLevel = BeatEdgePulseMinimumGreenColorLevel,
                .BeatEdgePulseMinimumBlueColorLevel = BeatEdgePulseMinimumBlueColorLevel,
                .BeatEdgePulseMinimumColorLevel = BeatEdgePulseMinimumColorLevel,
                .BeatEdgePulseMaximumRedColorLevel = BeatEdgePulseMaximumRedColorLevel,
                .BeatEdgePulseMaximumGreenColorLevel = BeatEdgePulseMaximumGreenColorLevel,
                .BeatEdgePulseMaximumBlueColorLevel = BeatEdgePulseMaximumBlueColorLevel,
                .BeatEdgePulseMaximumColorLevel = BeatEdgePulseMaximumColorLevel,
                .RandomDriver = RandomDriver
            }
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Animations.BeatEdgePulse.Simulate(BeatEdgePulseSettingsInstance)
            SleepNoBlock(BeatEdgePulseDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
