
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
    Public Module EdgePulseSettings

        Private _edgepulseDelay As Integer = 50
        Private _edgepulseMaxSteps As Integer = 25
        Private _edgepulseMinimumRedColorLevel As Integer = 0
        Private _edgepulseMinimumGreenColorLevel As Integer = 0
        Private _edgepulseMinimumBlueColorLevel As Integer = 0
        Private _edgepulseMaximumRedColorLevel As Integer = 255
        Private _edgepulseMaximumGreenColorLevel As Integer = 255
        Private _edgepulseMaximumBlueColorLevel As Integer = 255

        ''' <summary>
        ''' [EdgePulse] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property EdgePulseDelay As Integer
            Get
                Return _edgepulseDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _edgepulseDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [EdgePulse] How many fade steps to do?
        ''' </summary>
        Public Property EdgePulseMaxSteps As Integer
            Get
                Return _edgepulseMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _edgepulseMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [EdgePulse] The minimum red color level (true color)
        ''' </summary>
        Public Property EdgePulseMinimumRedColorLevel As Integer
            Get
                Return _edgepulseMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _edgepulseMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [EdgePulse] The minimum green color level (true color)
        ''' </summary>
        Public Property EdgePulseMinimumGreenColorLevel As Integer
            Get
                Return _edgepulseMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _edgepulseMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [EdgePulse] The minimum blue color level (true color)
        ''' </summary>
        Public Property EdgePulseMinimumBlueColorLevel As Integer
            Get
                Return _edgepulseMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _edgepulseMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [EdgePulse] The maximum red color level (true color)
        ''' </summary>
        Public Property EdgePulseMaximumRedColorLevel As Integer
            Get
                Return _edgepulseMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _edgepulseMinimumRedColorLevel Then value = _edgepulseMinimumRedColorLevel
                If value > 255 Then value = 255
                _edgepulseMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [EdgePulse] The maximum green color level (true color)
        ''' </summary>
        Public Property EdgePulseMaximumGreenColorLevel As Integer
            Get
                Return _edgepulseMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _edgepulseMinimumGreenColorLevel Then value = _edgepulseMinimumGreenColorLevel
                If value > 255 Then value = 255
                _edgepulseMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [EdgePulse] The maximum blue color level (true color)
        ''' </summary>
        Public Property EdgePulseMaximumBlueColorLevel As Integer
            Get
                Return _edgepulseMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _edgepulseMinimumBlueColorLevel Then value = _edgepulseMinimumBlueColorLevel
                If value > 255 Then value = 255
                _edgepulseMaximumBlueColorLevel = value
            End Set
        End Property

    End Module

    Public Class EdgePulseDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private EdgePulseSettingsInstance As Animations.EdgePulse.EdgePulseSettings
        Private RandomDriver As Random

        Public Overrides Property ScreensaverName As String = "EdgePulse" Implements IScreensaver.ScreensaverName

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
            EdgePulseSettingsInstance = New Animations.EdgePulse.EdgePulseSettings With {
                .EdgePulseDelay = EdgePulseDelay,
                .EdgePulseMaxSteps = EdgePulseMaxSteps,
                .EdgePulseMinimumRedColorLevel = EdgePulseMinimumRedColorLevel,
                .EdgePulseMinimumGreenColorLevel = EdgePulseMinimumGreenColorLevel,
                .EdgePulseMinimumBlueColorLevel = EdgePulseMinimumBlueColorLevel,
                .EdgePulseMaximumRedColorLevel = EdgePulseMaximumRedColorLevel,
                .EdgePulseMaximumGreenColorLevel = EdgePulseMaximumGreenColorLevel,
                .EdgePulseMaximumBlueColorLevel = EdgePulseMaximumBlueColorLevel,
                .RandomDriver = RandomDriver
            }
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Animations.EdgePulse.Simulate(EdgePulseSettingsInstance)
            SleepNoBlock(EdgePulseDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
