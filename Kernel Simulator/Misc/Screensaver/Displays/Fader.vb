
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
    Public Module FaderSettings

        Private _faderDelay As Integer = 50
        Private _faderFadeOutDelay As Integer = 3000
        Private _faderWrite As String = "Kernel Simulator"
        Private _faderMaxSteps As Integer = 25
        Private _faderBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _faderMinimumRedColorLevel As Integer = 0
        Private _faderMinimumGreenColorLevel As Integer = 0
        Private _faderMinimumBlueColorLevel As Integer = 0
        Private _faderMaximumRedColorLevel As Integer = 255
        Private _faderMaximumGreenColorLevel As Integer = 255
        Private _faderMaximumBlueColorLevel As Integer = 255

        ''' <summary>
        ''' [Fader] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FaderDelay As Integer
            Get
                Return _faderDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _faderDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] How many milliseconds to wait before fading the text out?
        ''' </summary>
        Public Property FaderFadeOutDelay As Integer
            Get
                Return _faderFadeOutDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _faderFadeOutDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] Text for Fader. Shorter is better.
        ''' </summary>
        Public Property FaderWrite As String
            Get
                Return _faderWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _faderWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] How many fade steps to do?
        ''' </summary>
        Public Property FaderMaxSteps As Integer
            Get
                Return _faderMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _faderMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] Screensaver background color
        ''' </summary>
        Public Property FaderBackgroundColor As String
            Get
                Return _faderBackgroundColor
            End Get
            Set(value As String)
                _faderBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The minimum red color level (true color)
        ''' </summary>
        Public Property FaderMinimumRedColorLevel As Integer
            Get
                Return _faderMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The minimum green color level (true color)
        ''' </summary>
        Public Property FaderMinimumGreenColorLevel As Integer
            Get
                Return _faderMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The minimum blue color level (true color)
        ''' </summary>
        Public Property FaderMinimumBlueColorLevel As Integer
            Get
                Return _faderMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The maximum red color level (true color)
        ''' </summary>
        Public Property FaderMaximumRedColorLevel As Integer
            Get
                Return _faderMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderMinimumRedColorLevel Then value = _faderMinimumRedColorLevel
                If value > 255 Then value = 255
                _faderMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The maximum green color level (true color)
        ''' </summary>
        Public Property FaderMaximumGreenColorLevel As Integer
            Get
                Return _faderMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderMinimumGreenColorLevel Then value = _faderMinimumGreenColorLevel
                If value > 255 Then value = 255
                _faderMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The maximum blue color level (true color)
        ''' </summary>
        Public Property FaderMaximumBlueColorLevel As Integer
            Get
                Return _faderMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderMinimumBlueColorLevel Then value = _faderMinimumBlueColorLevel
                If value > 255 Then value = 255
                _faderMaximumBlueColorLevel = value
            End Set
        End Property

    End Module

    Public Class FaderDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private FaderSettingsInstance As Animations.Fader.FaderSettings

        Public Overrides Property ScreensaverName As String = "Fader" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            SetConsoleColor(New Color(FaderBackgroundColor), True)
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
            FaderSettingsInstance = New Animations.Fader.FaderSettings With {
                .FaderDelay = FaderDelay,
                .FaderWrite = FaderWrite,
                .FaderBackgroundColor = FaderBackgroundColor,
                .FaderFadeOutDelay = FaderFadeOutDelay,
                .FaderMaxSteps = FaderMaxSteps,
                .FaderMinimumRedColorLevel = FaderMinimumRedColorLevel,
                .FaderMinimumGreenColorLevel = FaderMinimumGreenColorLevel,
                .FaderMinimumBlueColorLevel = FaderMinimumBlueColorLevel,
                .FaderMaximumRedColorLevel = FaderMaximumRedColorLevel,
                .FaderMaximumGreenColorLevel = FaderMaximumGreenColorLevel,
                .FaderMaximumBlueColorLevel = FaderMaximumBlueColorLevel,
                .RandomDriver = RandomDriver
            }
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Animations.Fader.Simulate(FaderSettingsInstance)
        End Sub

    End Class
End Namespace
