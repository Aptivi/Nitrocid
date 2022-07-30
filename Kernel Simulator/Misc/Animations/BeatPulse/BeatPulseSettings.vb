
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

Namespace Misc.Animations.BeatPulse
    Public Class BeatPulseSettings

        Friend RandomDriver As Random
        Private _beatpulse255Colors As Boolean
        Private _beatpulseTrueColor As Boolean = True
        Private _beatpulseDelay As Integer = 120
        Private _beatpulseMaxSteps As Integer = 30
        Private _beatpulseCycleColors As Boolean = True
        Private _beatpulseBeatColor As String = 17
        Private _beatpulseMinimumRedColorLevel As Integer = 0
        Private _beatpulseMinimumGreenColorLevel As Integer = 0
        Private _beatpulseMinimumBlueColorLevel As Integer = 0
        Private _beatpulseMinimumColorLevel As Integer = 0
        Private _beatpulseMaximumRedColorLevel As Integer = 255
        Private _beatpulseMaximumGreenColorLevel As Integer = 255
        Private _beatpulseMaximumBlueColorLevel As Integer = 255
        Private _beatpulseMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [BeatPulse] Enable 255 color support. Has a higher priority than 16 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatPulse255Colors As Boolean
            Get
                Return _beatpulse255Colors
            End Get
            Set(value As Boolean)
                _beatpulse255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatPulseTrueColor As Boolean
            Get
                Return _beatpulseTrueColor
            End Get
            Set(value As Boolean)
                _beatpulseTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatPulseBeatColor"/> color.)
        ''' </summary>
        Public Property BeatPulseCycleColors As Boolean
            Get
                Return _beatpulseCycleColors
            End Get
            Set(value As Boolean)
                _beatpulseCycleColors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property BeatPulseBeatColor As String
            Get
                Return _beatpulseBeatColor
            End Get
            Set(value As String)
                _beatpulseBeatColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] How many beats per minute to wait before making the next write?
        ''' </summary>
        Public Property BeatPulseDelay As Integer
            Get
                Return _beatpulseDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 120
                _beatpulseDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] How many fade steps to do?
        ''' </summary>
        Public Property BeatPulseMaxSteps As Integer
            Get
                Return _beatpulseMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _beatpulseMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The minimum red color level (true color)
        ''' </summary>
        Public Property BeatPulseMinimumRedColorLevel As Integer
            Get
                Return _beatpulseMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatpulseMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The minimum green color level (true color)
        ''' </summary>
        Public Property BeatPulseMinimumGreenColorLevel As Integer
            Get
                Return _beatpulseMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatpulseMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The minimum blue color level (true color)
        ''' </summary>
        Public Property BeatPulseMinimumBlueColorLevel As Integer
            Get
                Return _beatpulseMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatpulseMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatPulseMinimumColorLevel As Integer
            Get
                Return _beatpulseMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_beatpulse255Colors Or _beatpulseTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _beatpulseMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The maximum red color level (true color)
        ''' </summary>
        Public Property BeatPulseMaximumRedColorLevel As Integer
            Get
                Return _beatpulseMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatpulseMinimumRedColorLevel Then value = _beatpulseMinimumRedColorLevel
                If value > 255 Then value = 255
                _beatpulseMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The maximum green color level (true color)
        ''' </summary>
        Public Property BeatPulseMaximumGreenColorLevel As Integer
            Get
                Return _beatpulseMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatpulseMinimumGreenColorLevel Then value = _beatpulseMinimumGreenColorLevel
                If value > 255 Then value = 255
                _beatpulseMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The maximum blue color level (true color)
        ''' </summary>
        Public Property BeatPulseMaximumBlueColorLevel As Integer
            Get
                Return _beatpulseMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatpulseMinimumBlueColorLevel Then value = _beatpulseMinimumBlueColorLevel
                If value > 255 Then value = 255
                _beatpulseMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatPulse] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatPulseMaximumColorLevel As Integer
            Get
                Return _beatpulseMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_beatpulse255Colors Or _beatpulseTrueColor, 255, 15)
                If value <= _beatpulseMinimumColorLevel Then value = _beatpulseMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _beatpulseMaximumColorLevel = value
            End Set
        End Property

    End Class
End Namespace
