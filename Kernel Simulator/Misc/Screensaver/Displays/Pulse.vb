
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
    Public Module PulseSettings

        Private _pulseDelay As Integer = 50
        Private _pulseMaxSteps As Integer = 25
        Private _pulseMinimumRedColorLevel As Integer = 0
        Private _pulseMinimumGreenColorLevel As Integer = 0
        Private _pulseMinimumBlueColorLevel As Integer = 0
        Private _pulseMaximumRedColorLevel As Integer = 255
        Private _pulseMaximumGreenColorLevel As Integer = 255
        Private _pulseMaximumBlueColorLevel As Integer = 255

        ''' <summary>
        ''' [Pulse] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property PulseDelay As Integer
            Get
                Return _pulseDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _pulseDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Pulse] How many fade steps to do?
        ''' </summary>
        Public Property PulseMaxSteps As Integer
            Get
                Return _pulseMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _pulseMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [Pulse] The minimum red color level (true color)
        ''' </summary>
        Public Property PulseMinimumRedColorLevel As Integer
            Get
                Return _pulseMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _pulseMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Pulse] The minimum green color level (true color)
        ''' </summary>
        Public Property PulseMinimumGreenColorLevel As Integer
            Get
                Return _pulseMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _pulseMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Pulse] The minimum blue color level (true color)
        ''' </summary>
        Public Property PulseMinimumBlueColorLevel As Integer
            Get
                Return _pulseMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _pulseMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Pulse] The maximum red color level (true color)
        ''' </summary>
        Public Property PulseMaximumRedColorLevel As Integer
            Get
                Return _pulseMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _pulseMinimumRedColorLevel Then value = _pulseMinimumRedColorLevel
                If value > 255 Then value = 255
                _pulseMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Pulse] The maximum green color level (true color)
        ''' </summary>
        Public Property PulseMaximumGreenColorLevel As Integer
            Get
                Return _pulseMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _pulseMinimumGreenColorLevel Then value = _pulseMinimumGreenColorLevel
                If value > 255 Then value = 255
                _pulseMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Pulse] The maximum blue color level (true color)
        ''' </summary>
        Public Property PulseMaximumBlueColorLevel As Integer
            Get
                Return _pulseMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _pulseMinimumBlueColorLevel Then value = _pulseMinimumBlueColorLevel
                If value > 255 Then value = 255
                _pulseMaximumBlueColorLevel = value
            End Set
        End Property

    End Module

    Public Class PulseDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private PulseSettingsInstance As Animations.Pulse.PulseSettings
        Private RandomDriver As Random

        Public Overrides Property ScreensaverName As String = "Pulse" Implements IScreensaver.ScreensaverName

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
            PulseSettingsInstance = New Animations.Pulse.PulseSettings With {
                .PulseDelay = PulseDelay,
                .PulseMaxSteps = PulseMaxSteps,
                .PulseMinimumRedColorLevel = PulseMinimumRedColorLevel,
                .PulseMinimumGreenColorLevel = PulseMinimumGreenColorLevel,
                .PulseMinimumBlueColorLevel = PulseMinimumBlueColorLevel,
                .PulseMaximumRedColorLevel = PulseMaximumRedColorLevel,
                .PulseMaximumGreenColorLevel = PulseMaximumGreenColorLevel,
                .PulseMaximumBlueColorLevel = PulseMaximumBlueColorLevel,
                .RandomDriver = RandomDriver
            }
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Animations.Pulse.Simulate(PulseSettingsInstance)
            SleepNoBlock(PulseDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
