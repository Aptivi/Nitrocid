
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
    Public Module BeatFaderSettings

        Private _beatFader255Colors As Boolean
        Private _beatFaderTrueColor As Boolean = True
        Private _beatFaderCycleColors As Boolean = True
        Private _beatFaderBeatColor As String = 17
        Private _beatFaderDelay As Integer = 120
        Private _beatFaderMaxSteps As Integer = 25
        Private _beatFaderMinimumRedColorLevel As Integer = 0
        Private _beatFaderMinimumGreenColorLevel As Integer = 0
        Private _beatFaderMinimumBlueColorLevel As Integer = 0
        Private _beatFaderMinimumColorLevel As Integer = 0
        Private _beatFaderMaximumRedColorLevel As Integer = 255
        Private _beatFaderMaximumGreenColorLevel As Integer = 255
        Private _beatFaderMaximumBlueColorLevel As Integer = 255
        Private _beatFaderMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [BeatFader] Enable 255 color support. Has a higher priority than 16 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatFader255Colors As Boolean
            Get
                Return _beatFader255Colors
            End Get
            Set(value As Boolean)
                _beatFader255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatFaderTrueColor As Boolean
            Get
                Return _beatFaderTrueColor
            End Get
            Set(value As Boolean)
                _beatFaderTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        ''' </summary>
        Public Property BeatFaderCycleColors As Boolean
            Get
                Return _beatFaderCycleColors
            End Get
            Set(value As Boolean)
                _beatFaderCycleColors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property BeatFaderBeatColor As String
            Get
                Return _beatFaderBeatColor
            End Get
            Set(value As String)
                _beatFaderBeatColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] How many beats per minute to wait before making the next write?
        ''' </summary>
        Public Property BeatFaderDelay As Integer
            Get
                Return _beatFaderDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 120
                _beatFaderDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] How many fade steps to do?
        ''' </summary>
        Public Property BeatFaderMaxSteps As Integer
            Get
                Return _beatFaderMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _beatFaderMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum red color level (true color)
        ''' </summary>
        Public Property BeatFaderMinimumRedColorLevel As Integer
            Get
                Return _beatFaderMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatFaderMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum green color level (true color)
        ''' </summary>
        Public Property BeatFaderMinimumGreenColorLevel As Integer
            Get
                Return _beatFaderMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatFaderMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum blue color level (true color)
        ''' </summary>
        Public Property BeatFaderMinimumBlueColorLevel As Integer
            Get
                Return _beatFaderMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatFaderMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatFaderMinimumColorLevel As Integer
            Get
                Return _beatFaderMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_beatFader255Colors Or _beatFaderTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _beatFaderMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum red color level (true color)
        ''' </summary>
        Public Property BeatFaderMaximumRedColorLevel As Integer
            Get
                Return _beatFaderMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatFaderMinimumRedColorLevel Then value = _beatFaderMinimumRedColorLevel
                If value > 255 Then value = 255
                _beatFaderMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum green color level (true color)
        ''' </summary>
        Public Property BeatFaderMaximumGreenColorLevel As Integer
            Get
                Return _beatFaderMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatFaderMinimumGreenColorLevel Then value = _beatFaderMinimumGreenColorLevel
                If value > 255 Then value = 255
                _beatFaderMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum blue color level (true color)
        ''' </summary>
        Public Property BeatFaderMaximumBlueColorLevel As Integer
            Get
                Return _beatFaderMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatFaderMinimumBlueColorLevel Then value = _beatFaderMinimumBlueColorLevel
                If value > 255 Then value = 255
                _beatFaderMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatFaderMaximumColorLevel As Integer
            Get
                Return _beatFaderMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_beatFader255Colors Or _beatFaderTrueColor, 255, 15)
                If value <= _beatFaderMinimumColorLevel Then value = _beatFaderMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _beatFaderMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class BeatFaderDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private BeatFaderSettingsInstance As Animations.BeatFader.BeatFaderSettings

        Public Overrides Property ScreensaverName As String = "BeatFader" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
            BeatFaderSettingsInstance = New Animations.BeatFader.BeatFaderSettings With {
                .BeatFader255Colors = BeatFader255Colors,
                .BeatFaderTrueColor = BeatFaderTrueColor,
                .BeatFaderBeatColor = BeatFaderBeatColor,
                .BeatFaderDelay = BeatFaderDelay,
                .BeatFaderMaxSteps = BeatFaderMaxSteps,
                .BeatFaderCycleColors = BeatFaderCycleColors,
                .BeatFaderMinimumRedColorLevel = BeatFaderMinimumRedColorLevel,
                .BeatFaderMinimumGreenColorLevel = BeatFaderMinimumGreenColorLevel,
                .BeatFaderMinimumBlueColorLevel = BeatFaderMinimumBlueColorLevel,
                .BeatFaderMinimumColorLevel = BeatFaderMinimumColorLevel,
                .BeatFaderMaximumRedColorLevel = BeatFaderMaximumRedColorLevel,
                .BeatFaderMaximumGreenColorLevel = BeatFaderMaximumGreenColorLevel,
                .BeatFaderMaximumBlueColorLevel = BeatFaderMaximumBlueColorLevel,
                .BeatFaderMaximumColorLevel = BeatFaderMaximumColorLevel,
                .RandomDriver = RandomDriver
            }
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Animations.BeatFader.Simulate(BeatFaderSettingsInstance)
        End Sub

    End Class
End Namespace
