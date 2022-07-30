
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

Imports KS.TimeDate

Namespace Misc.Screensaver.Displays
    Public Module ProgressClockSettings

        Private _progressClock255Colors As Boolean
        Private _progressClockTrueColor As Boolean = True
        Private _progressClockCycleColors As Boolean = True
        Private _progressClockSecondsProgressColor As String = 4
        Private _progressClockMinutesProgressColor As String = 5
        Private _progressClockHoursProgressColor As String = 6
        Private _progressClockProgressColor As String = 7
        Private _progressClockCycleColorsTicks As Long = 20
        Private _progressClockDelay As Integer = 500
        Private _progressClockUpperLeftCornerCharHours As String = "╔"
        Private _progressClockUpperLeftCornerCharMinutes As String = "╔"
        Private _progressClockUpperLeftCornerCharSeconds As String = "╔"
        Private _progressClockUpperRightCornerCharHours As String = "╗"
        Private _progressClockUpperRightCornerCharMinutes As String = "╗"
        Private _progressClockUpperRightCornerCharSeconds As String = "╗"
        Private _progressClockLowerLeftCornerCharHours As String = "╚"
        Private _progressClockLowerLeftCornerCharMinutes As String = "╚"
        Private _progressClockLowerLeftCornerCharSeconds As String = "╚"
        Private _progressClockLowerRightCornerCharHours As String = "╝"
        Private _progressClockLowerRightCornerCharMinutes As String = "╝"
        Private _progressClockLowerRightCornerCharSeconds As String = "╝"
        Private _progressClockUpperFrameCharHours As String = "═"
        Private _progressClockUpperFrameCharMinutes As String = "═"
        Private _progressClockUpperFrameCharSeconds As String = "═"
        Private _progressClockLowerFrameCharHours As String = "═"
        Private _progressClockLowerFrameCharMinutes As String = "═"
        Private _progressClockLowerFrameCharSeconds As String = "═"
        Private _progressClockLeftFrameCharHours As String = "║"
        Private _progressClockLeftFrameCharMinutes As String = "║"
        Private _progressClockLeftFrameCharSeconds As String = "║"
        Private _progressClockRightFrameCharHours As String = "║"
        Private _progressClockRightFrameCharMinutes As String = "║"
        Private _progressClockRightFrameCharSeconds As String = "║"
        Private _progressClockInfoTextHours As String = ""
        Private _progressClockInfoTextMinutes As String = ""
        Private _progressClockInfoTextSeconds As String = ""
        Private _progressClockMinimumRedColorLevelHours As Integer = 0
        Private _progressClockMinimumGreenColorLevelHours As Integer = 0
        Private _progressClockMinimumBlueColorLevelHours As Integer = 0
        Private _progressClockMinimumColorLevelHours As Integer = 0
        Private _progressClockMaximumRedColorLevelHours As Integer = 255
        Private _progressClockMaximumGreenColorLevelHours As Integer = 255
        Private _progressClockMaximumBlueColorLevelHours As Integer = 255
        Private _progressClockMaximumColorLevelHours As Integer = 255
        Private _progressClockMinimumRedColorLevelMinutes As Integer = 0
        Private _progressClockMinimumGreenColorLevelMinutes As Integer = 0
        Private _progressClockMinimumBlueColorLevelMinutes As Integer = 0
        Private _progressClockMinimumColorLevelMinutes As Integer = 0
        Private _progressClockMaximumRedColorLevelMinutes As Integer = 255
        Private _progressClockMaximumGreenColorLevelMinutes As Integer = 255
        Private _progressClockMaximumBlueColorLevelMinutes As Integer = 255
        Private _progressClockMaximumColorLevelMinutes As Integer = 255
        Private _progressClockMinimumRedColorLevelSeconds As Integer = 0
        Private _progressClockMinimumGreenColorLevelSeconds As Integer = 0
        Private _progressClockMinimumBlueColorLevelSeconds As Integer = 0
        Private _progressClockMinimumColorLevelSeconds As Integer = 0
        Private _progressClockMaximumRedColorLevelSeconds As Integer = 255
        Private _progressClockMaximumGreenColorLevelSeconds As Integer = 255
        Private _progressClockMaximumBlueColorLevelSeconds As Integer = 255
        Private _progressClockMaximumColorLevelSeconds As Integer = 255
        Private _progressClockMinimumRedColorLevel As Integer = 0
        Private _progressClockMinimumGreenColorLevel As Integer = 0
        Private _progressClockMinimumBlueColorLevel As Integer = 0
        Private _progressClockMinimumColorLevel As Integer = 0
        Private _progressClockMaximumRedColorLevel As Integer = 255
        Private _progressClockMaximumGreenColorLevel As Integer = 255
        Private _progressClockMaximumBlueColorLevel As Integer = 255
        Private _progressClockMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [ProgressClock] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property ProgressClock255Colors As Boolean
            Get
                Return _progressClock255Colors
            End Get
            Set(value As Boolean)
                _progressClock255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property ProgressClockTrueColor As Boolean
            Get
                Return _progressClockTrueColor
            End Get
            Set(value As Boolean)
                _progressClockTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
        ''' </summary>
        Public Property ProgressClockCycleColors As Boolean
            Get
                Return _progressClockCycleColors
            End Get
            Set(value As Boolean)
                _progressClockCycleColors = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property ProgressClockSecondsProgressColor As String
            Get
                Return _progressClockSecondsProgressColor
            End Get
            Set(value As String)
                _progressClockSecondsProgressColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property ProgressClockMinutesProgressColor As String
            Get
                Return _progressClockMinutesProgressColor
            End Get
            Set(value As String)
                _progressClockMinutesProgressColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property ProgressClockHoursProgressColor As String
            Get
                Return _progressClockHoursProgressColor
            End Get
            Set(value As String)
                _progressClockHoursProgressColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property ProgressClockProgressColor As String
            Get
                Return _progressClockProgressColor
            End Get
            Set(value As String)
                _progressClockProgressColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
        ''' </summary>
        Public Property ProgressClockCycleColorsTicks As Long
            Get
                Return _progressClockCycleColorsTicks
            End Get
            Set(value As Long)
                If value <= 0 Then value = 20
                _progressClockCycleColorsTicks = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property ProgressClockDelay As Integer
            Get
                Return _progressClockDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 500
                _progressClockDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper left corner character for hours bar
        ''' </summary>
        Public Property ProgressClockUpperLeftCornerCharHours As String
            Get
                Return _progressClockUpperLeftCornerCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╔"
                _progressClockUpperLeftCornerCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper left corner character for minutes bar
        ''' </summary>
        Public Property ProgressClockUpperLeftCornerCharMinutes As String
            Get
                Return _progressClockUpperLeftCornerCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╔"
                _progressClockUpperLeftCornerCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper left corner character for seconds bar
        ''' </summary>
        Public Property ProgressClockUpperLeftCornerCharSeconds As String
            Get
                Return _progressClockUpperLeftCornerCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╔"
                _progressClockUpperLeftCornerCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper right corner character for hours bar
        ''' </summary>
        Public Property ProgressClockUpperRightCornerCharHours As String
            Get
                Return _progressClockUpperRightCornerCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╗"
                _progressClockUpperRightCornerCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper right corner character for minutes bar
        ''' </summary>
        Public Property ProgressClockUpperRightCornerCharMinutes As String
            Get
                Return _progressClockUpperRightCornerCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╗"
                _progressClockUpperRightCornerCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper right corner character for seconds bar
        ''' </summary>
        Public Property ProgressClockUpperRightCornerCharSeconds As String
            Get
                Return _progressClockUpperRightCornerCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╗"
                _progressClockUpperRightCornerCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower left corner character for hours bar
        ''' </summary>
        Public Property ProgressClockLowerLeftCornerCharHours As String
            Get
                Return _progressClockLowerLeftCornerCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╚"
                _progressClockLowerLeftCornerCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower left corner character for minutes bar
        ''' </summary>
        Public Property ProgressClockLowerLeftCornerCharMinutes As String
            Get
                Return _progressClockLowerLeftCornerCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╚"
                _progressClockLowerLeftCornerCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower left corner character for seconds bar
        ''' </summary>
        Public Property ProgressClockLowerLeftCornerCharSeconds As String
            Get
                Return _progressClockLowerLeftCornerCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╚"
                _progressClockLowerLeftCornerCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower right corner character for hours bar
        ''' </summary>
        Public Property ProgressClockLowerRightCornerCharHours As String
            Get
                Return _progressClockLowerRightCornerCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╝"
                _progressClockLowerRightCornerCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower right corner character for minutes bar
        ''' </summary>
        Public Property ProgressClockLowerRightCornerCharMinutes As String
            Get
                Return _progressClockLowerRightCornerCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╝"
                _progressClockLowerRightCornerCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower right corner character for seconds bar
        ''' </summary>
        Public Property ProgressClockLowerRightCornerCharSeconds As String
            Get
                Return _progressClockLowerRightCornerCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╝"
                _progressClockLowerRightCornerCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper frame character for hours bar
        ''' </summary>
        Public Property ProgressClockUpperFrameCharHours As String
            Get
                Return _progressClockUpperFrameCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _progressClockUpperFrameCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper frame character for minutes bar
        ''' </summary>
        Public Property ProgressClockUpperFrameCharMinutes As String
            Get
                Return _progressClockUpperFrameCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _progressClockUpperFrameCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Upper frame character for seconds bar
        ''' </summary>
        Public Property ProgressClockUpperFrameCharSeconds As String
            Get
                Return _progressClockUpperFrameCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _progressClockUpperFrameCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower frame character for hours bar
        ''' </summary>
        Public Property ProgressClockLowerFrameCharHours As String
            Get
                Return _progressClockLowerFrameCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _progressClockLowerFrameCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower frame character for minutes bar
        ''' </summary>
        Public Property ProgressClockLowerFrameCharMinutes As String
            Get
                Return _progressClockLowerFrameCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _progressClockLowerFrameCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Lower frame character for seconds bar
        ''' </summary>
        Public Property ProgressClockLowerFrameCharSeconds As String
            Get
                Return _progressClockLowerFrameCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _progressClockLowerFrameCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Left frame character for hours bar
        ''' </summary>
        Public Property ProgressClockLeftFrameCharHours As String
            Get
                Return _progressClockLeftFrameCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _progressClockLeftFrameCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Left frame character for minutes bar
        ''' </summary>
        Public Property ProgressClockLeftFrameCharMinutes As String
            Get
                Return _progressClockLeftFrameCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _progressClockLeftFrameCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Left frame character for seconds bar
        ''' </summary>
        Public Property ProgressClockLeftFrameCharSeconds As String
            Get
                Return _progressClockLeftFrameCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _progressClockLeftFrameCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Right frame character for hours bar
        ''' </summary>
        Public Property ProgressClockRightFrameCharHours As String
            Get
                Return _progressClockRightFrameCharHours
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _progressClockRightFrameCharHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Right frame character for minutes bar
        ''' </summary>
        Public Property ProgressClockRightFrameCharMinutes As String
            Get
                Return _progressClockRightFrameCharMinutes
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _progressClockRightFrameCharMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Right frame character for seconds bar
        ''' </summary>
        Public Property ProgressClockRightFrameCharSeconds As String
            Get
                Return _progressClockRightFrameCharSeconds
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _progressClockRightFrameCharSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Information text for hours bar
        ''' </summary>
        Public Property ProgressClockInfoTextHours As String
            Get
                Return _progressClockInfoTextHours
            End Get
            Set(value As String)
                _progressClockInfoTextHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Information text for minutes bar
        ''' </summary>
        Public Property ProgressClockInfoTextMinutes As String
            Get
                Return _progressClockInfoTextMinutes
            End Get
            Set(value As String)
                _progressClockInfoTextMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] Information text for seconds bar
        ''' </summary>
        Public Property ProgressClockInfoTextSeconds As String
            Get
                Return _progressClockInfoTextSeconds
            End Get
            Set(value As String)
                _progressClockInfoTextSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum red color level (true color - hours)
        ''' </summary>
        Public Property ProgressClockMinimumRedColorLevelHours As Integer
            Get
                Return _progressClockMinimumRedColorLevelHours
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumRedColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum green color level (true color - hours)
        ''' </summary>
        Public Property ProgressClockMinimumGreenColorLevelHours As Integer
            Get
                Return _progressClockMinimumGreenColorLevelHours
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumGreenColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum blue color level (true color - hours)
        ''' </summary>
        Public Property ProgressClockMinimumBlueColorLevelHours As Integer
            Get
                Return _progressClockMinimumBlueColorLevelHours
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumBlueColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
        ''' </summary>
        Public Property ProgressClockMinimumColorLevelHours As Integer
            Get
                Return _progressClockMinimumColorLevelHours
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _progressClockMinimumColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum red color level (true color - hours)
        ''' </summary>
        Public Property ProgressClockMaximumRedColorLevelHours As Integer
            Get
                Return _progressClockMaximumRedColorLevelHours
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumRedColorLevelHours Then value = _progressClockMinimumRedColorLevelHours
                If value > 255 Then value = 255
                _progressClockMaximumRedColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum green color level (true color - hours)
        ''' </summary>
        Public Property ProgressClockMaximumGreenColorLevelHours As Integer
            Get
                Return _progressClockMaximumGreenColorLevelHours
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumGreenColorLevelHours Then value = _progressClockMinimumGreenColorLevelHours
                If value > 255 Then value = 255
                _progressClockMaximumGreenColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum blue color level (true color - hours)
        ''' </summary>
        Public Property ProgressClockMaximumBlueColorLevelHours As Integer
            Get
                Return _progressClockMaximumBlueColorLevelHours
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumBlueColorLevelHours Then value = _progressClockMinimumBlueColorLevelHours
                If value > 255 Then value = 255
                _progressClockMaximumBlueColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
        ''' </summary>
        Public Property ProgressClockMaximumColorLevelHours As Integer
            Get
                Return _progressClockMaximumColorLevelHours
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= _progressClockMinimumColorLevelHours Then value = _progressClockMinimumColorLevelHours
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _progressClockMaximumColorLevelHours = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum red color level (true color - minutes)
        ''' </summary>
        Public Property ProgressClockMinimumRedColorLevelMinutes As Integer
            Get
                Return _progressClockMinimumRedColorLevelMinutes
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumRedColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum green color level (true color - minutes)
        ''' </summary>
        Public Property ProgressClockMinimumGreenColorLevelMinutes As Integer
            Get
                Return _progressClockMinimumGreenColorLevelMinutes
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumGreenColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum blue color level (true color - minutes)
        ''' </summary>
        Public Property ProgressClockMinimumBlueColorLevelMinutes As Integer
            Get
                Return _progressClockMinimumBlueColorLevelMinutes
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumBlueColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
        ''' </summary>
        Public Property ProgressClockMinimumColorLevelMinutes As Integer
            Get
                Return _progressClockMinimumColorLevelMinutes
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _progressClockMinimumColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum red color level (true color - minutes)
        ''' </summary>
        Public Property ProgressClockMaximumRedColorLevelMinutes As Integer
            Get
                Return _progressClockMaximumRedColorLevelMinutes
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumRedColorLevelMinutes Then value = _progressClockMinimumRedColorLevelMinutes
                If value > 255 Then value = 255
                _progressClockMaximumRedColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum green color level (true color - minutes)
        ''' </summary>
        Public Property ProgressClockMaximumGreenColorLevelMinutes As Integer
            Get
                Return _progressClockMaximumGreenColorLevelMinutes
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumGreenColorLevelMinutes Then value = _progressClockMinimumGreenColorLevelMinutes
                If value > 255 Then value = 255
                _progressClockMaximumGreenColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum blue color level (true color - minutes)
        ''' </summary>
        Public Property ProgressClockMaximumBlueColorLevelMinutes As Integer
            Get
                Return _progressClockMaximumBlueColorLevelMinutes
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumBlueColorLevelMinutes Then value = _progressClockMinimumBlueColorLevelMinutes
                If value > 255 Then value = 255
                _progressClockMaximumBlueColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
        ''' </summary>
        Public Property ProgressClockMaximumColorLevelMinutes As Integer
            Get
                Return _progressClockMaximumColorLevelMinutes
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= _progressClockMinimumColorLevelMinutes Then value = _progressClockMinimumColorLevelMinutes
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _progressClockMaximumColorLevelMinutes = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum red color level (true color - seconds)
        ''' </summary>
        Public Property ProgressClockMinimumRedColorLevelSeconds As Integer
            Get
                Return _progressClockMinimumRedColorLevelSeconds
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumRedColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum green color level (true color - seconds)
        ''' </summary>
        Public Property ProgressClockMinimumGreenColorLevelSeconds As Integer
            Get
                Return _progressClockMinimumGreenColorLevelSeconds
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumGreenColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum blue color level (true color - seconds)
        ''' </summary>
        Public Property ProgressClockMinimumBlueColorLevelSeconds As Integer
            Get
                Return _progressClockMinimumBlueColorLevelSeconds
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumBlueColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
        ''' </summary>
        Public Property ProgressClockMinimumColorLevelSeconds As Integer
            Get
                Return _progressClockMinimumColorLevelSeconds
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _progressClockMinimumColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum red color level (true color - seconds)
        ''' </summary>
        Public Property ProgressClockMaximumRedColorLevelSeconds As Integer
            Get
                Return _progressClockMaximumRedColorLevelSeconds
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumRedColorLevelSeconds Then value = _progressClockMinimumRedColorLevelSeconds
                If value > 255 Then value = 255
                _progressClockMaximumRedColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum green color level (true color - seconds)
        ''' </summary>
        Public Property ProgressClockMaximumGreenColorLevelSeconds As Integer
            Get
                Return _progressClockMaximumGreenColorLevelSeconds
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumGreenColorLevelSeconds Then value = _progressClockMinimumGreenColorLevelSeconds
                If value > 255 Then value = 255
                _progressClockMaximumGreenColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum blue color level (true color - seconds)
        ''' </summary>
        Public Property ProgressClockMaximumBlueColorLevelSeconds As Integer
            Get
                Return _progressClockMaximumBlueColorLevelSeconds
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumBlueColorLevelSeconds Then value = _progressClockMinimumBlueColorLevelSeconds
                If value > 255 Then value = 255
                _progressClockMaximumBlueColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
        ''' </summary>
        Public Property ProgressClockMaximumColorLevelSeconds As Integer
            Get
                Return _progressClockMaximumColorLevelSeconds
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= _progressClockMinimumColorLevelSeconds Then value = _progressClockMinimumColorLevelSeconds
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _progressClockMaximumColorLevelSeconds = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum red color level (true color)
        ''' </summary>
        Public Property ProgressClockMinimumRedColorLevel As Integer
            Get
                Return _progressClockMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum green color level (true color)
        ''' </summary>
        Public Property ProgressClockMinimumGreenColorLevel As Integer
            Get
                Return _progressClockMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum blue color level (true color)
        ''' </summary>
        Public Property ProgressClockMinimumBlueColorLevel As Integer
            Get
                Return _progressClockMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _progressClockMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property ProgressClockMinimumColorLevel As Integer
            Get
                Return _progressClockMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _progressClockMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum red color level (true color)
        ''' </summary>
        Public Property ProgressClockMaximumRedColorLevel As Integer
            Get
                Return _progressClockMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumRedColorLevel Then value = _progressClockMinimumRedColorLevel
                If value > 255 Then value = 255
                _progressClockMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum green color level (true color)
        ''' </summary>
        Public Property ProgressClockMaximumGreenColorLevel As Integer
            Get
                Return _progressClockMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumGreenColorLevel Then value = _progressClockMinimumGreenColorLevel
                If value > 255 Then value = 255
                _progressClockMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum blue color level (true color)
        ''' </summary>
        Public Property ProgressClockMaximumBlueColorLevel As Integer
            Get
                Return _progressClockMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _progressClockMinimumBlueColorLevel Then value = _progressClockMinimumBlueColorLevel
                If value > 255 Then value = 255
                _progressClockMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [ProgressClock] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property ProgressClockMaximumColorLevel As Integer
            Get
                Return _progressClockMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_progressClock255Colors Or _progressClockTrueColor, 255, 15)
                If value <= _progressClockMinimumColorLevel Then value = _progressClockMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _progressClockMaximumColorLevel = value
            End Set
        End Property

    End Module
    Public Class ProgressClockDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private CurrentTicks As Long

        Public Overrides Property ScreensaverName As String = "ProgressClock" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            CurrentTicks = ProgressClockCycleColorsTicks
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            Console.Clear()

            'Prepare colors
            Dim RedColorNumHours, GreenColorNumHours, BlueColorNumHours As Integer
            Dim RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes As Integer
            Dim RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds As Integer
            Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
            Dim ColorNumHours, ColorNumMinutes, ColorNumSeconds, ColorNum As Integer
            Dim ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds As Integer
            Dim InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds As Integer
            Dim ColorStorageHours, ColorStorageMinutes, ColorStorageSeconds, ColorStorage As Color

            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks)
            If ProgressClockCycleColors Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                If CurrentTicks >= ProgressClockCycleColorsTicks Then
                    If ProgressClockTrueColor Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.")
                        RedColorNumHours = RandomDriver.Next(ProgressClockMinimumRedColorLevelHours, ProgressClockMaximumRedColorLevelHours)
                        GreenColorNumHours = RandomDriver.Next(ProgressClockMinimumGreenColorLevelHours, ProgressClockMaximumGreenColorLevelHours)
                        BlueColorNumHours = RandomDriver.Next(ProgressClockMinimumBlueColorLevelHours, ProgressClockMaximumBlueColorLevelHours)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", RedColorNumHours, GreenColorNumHours, BlueColorNumHours)
                        RedColorNumMinutes = RandomDriver.Next(ProgressClockMinimumRedColorLevelMinutes, ProgressClockMaximumRedColorLevelMinutes)
                        GreenColorNumMinutes = RandomDriver.Next(ProgressClockMinimumGreenColorLevelMinutes, ProgressClockMaximumGreenColorLevelMinutes)
                        BlueColorNumMinutes = RandomDriver.Next(ProgressClockMinimumBlueColorLevelMinutes, ProgressClockMaximumBlueColorLevelMinutes)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes)
                        RedColorNumSeconds = RandomDriver.Next(ProgressClockMinimumRedColorLevelSeconds, ProgressClockMaximumRedColorLevelSeconds)
                        GreenColorNumSeconds = RandomDriver.Next(ProgressClockMinimumGreenColorLevelSeconds, ProgressClockMaximumGreenColorLevelSeconds)
                        BlueColorNumSeconds = RandomDriver.Next(ProgressClockMinimumBlueColorLevelSeconds, ProgressClockMaximumBlueColorLevelSeconds)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds)
                        RedColorNum = RandomDriver.Next(ProgressClockMinimumRedColorLevel, ProgressClockMaximumRedColorLevel)
                        GreenColorNum = RandomDriver.Next(ProgressClockMinimumGreenColorLevel, ProgressClockMaximumGreenColorLevel)
                        BlueColorNum = RandomDriver.Next(ProgressClockMinimumBlueColorLevel, ProgressClockMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        ColorStorageHours = New Color(RedColorNumHours, GreenColorNumHours, BlueColorNumHours)
                        ColorStorageMinutes = New Color(RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes)
                        ColorStorageSeconds = New Color(RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds)
                        ColorStorage = New Color(RedColorNum, GreenColorNum, BlueColorNum)
                    Else
                        ColorNumHours = RandomDriver.Next(ProgressClockMinimumColorLevelHours, ProgressClockMaximumColorLevelHours)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Hours) ({0})", ColorNumHours)
                        ColorNumMinutes = RandomDriver.Next(ProgressClockMinimumColorLevelMinutes, ProgressClockMaximumColorLevelMinutes)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Minutes) ({0})", ColorNumMinutes)
                        ColorNumSeconds = RandomDriver.Next(ProgressClockMinimumColorLevelSeconds, ProgressClockMaximumColorLevelSeconds)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Seconds) ({0})", ColorNumSeconds)
                        ColorNum = RandomDriver.Next(ProgressClockMinimumColorLevel, ProgressClockMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        ColorStorageHours = New Color(ColorNumHours)
                        ColorStorageMinutes = New Color(ColorNumMinutes)
                        ColorStorageSeconds = New Color(ColorNumSeconds)
                        ColorStorage = New Color(ColorNum)
                    End If
                    CurrentTicks = 0
                End If
            Else
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors...")
                ColorStorageHours = New Color(ProgressClockHoursProgressColor)
                ColorStorageMinutes = New Color(ProgressClockMinutesProgressColor)
                ColorStorageSeconds = New Color(ProgressClockSecondsProgressColor)
                ColorStorage = New Color(ProgressClockProgressColor)
            End If
            ProgressFillPositionHours = CInt(Console.WindowHeight / 2) - 10
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours)
            ProgressFillPositionMinutes = CInt(Console.WindowHeight / 2) - 1
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes)
            ProgressFillPositionSeconds = CInt(Console.WindowHeight / 2) + 8
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds)
            InformationPositionHours = CInt(Console.WindowHeight / 2) - 12
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours)
            InformationPositionMinutes = CInt(Console.WindowHeight / 2) - 3
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes)
            InformationPositionSeconds = CInt(Console.WindowHeight / 2) + 6
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds)

#Disable Warning BC42104
            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
            If Not ResizeSyncing Then
                'Hours
                WriteWhere(ProgressClockLowerLeftCornerCharHours + ProgressClockLowerFrameCharHours.Repeat(Console.WindowWidth - 10) + ProgressClockLowerRightCornerCharHours, 4, CInt(Console.WindowHeight / 2) - 9, True, ColorStorageHours)         'Bottom of Hours
                WriteWhere(ProgressClockLeftFrameCharHours + " ".Repeat(Console.WindowWidth - 10) + ProgressClockRightFrameCharHours, 4, ProgressFillPositionHours, True, ColorStorageHours)                                                           'Medium of Hours
                WriteWhere(ProgressClockUpperLeftCornerCharHours + ProgressClockUpperFrameCharHours.Repeat(Console.WindowWidth - 10) + ProgressClockUpperRightCornerCharHours, 4, CInt(Console.WindowHeight / 2) - 11, True, ColorStorageHours)        'Top of Hours

                'Minutes
                WriteWhere(ProgressClockLowerLeftCornerCharMinutes + ProgressClockLowerFrameCharMinutes.Repeat(Console.WindowWidth - 10) + ProgressClockLowerRightCornerCharMinutes, 4, CInt(Console.WindowHeight / 2), True, ColorStorageMinutes)     'Bottom of Minutes
                WriteWhere(ProgressClockLeftFrameCharMinutes + " ".Repeat(Console.WindowWidth - 10) + ProgressClockRightFrameCharMinutes, 4, ProgressFillPositionMinutes, True, ColorStorageMinutes)                                                   'Medium of Minutes
                WriteWhere(ProgressClockUpperLeftCornerCharMinutes + ProgressClockUpperFrameCharMinutes.Repeat(Console.WindowWidth - 10) + ProgressClockUpperRightCornerCharMinutes, 4, CInt(Console.WindowHeight / 2) - 2, True, ColorStorageMinutes) 'Top of Minutes

                'Seconds
                WriteWhere(ProgressClockLowerLeftCornerCharSeconds + ProgressClockLowerFrameCharSeconds.Repeat(Console.WindowWidth - 10) + ProgressClockLowerRightCornerCharSeconds, 4, CInt(Console.WindowHeight / 2) + 9, True, ColorStorageSeconds) 'Bottom of Seconds
                WriteWhere(ProgressClockLeftFrameCharSeconds + " ".Repeat(Console.WindowWidth - 10) + ProgressClockRightFrameCharSeconds, 4, ProgressFillPositionSeconds, True, ColorStorageSeconds)                                                   'Medium of Seconds
                WriteWhere(ProgressClockUpperLeftCornerCharSeconds + ProgressClockUpperFrameCharSeconds.Repeat(Console.WindowWidth - 10) + ProgressClockUpperRightCornerCharSeconds, 4, CInt(Console.WindowHeight / 2) + 7, True, ColorStorageSeconds) 'Top of Seconds

                'Fill progress for hours, minutes, and seconds
                If Not KernelDateTime.Hour = 0 Then WriteWhere(" ".Repeat(PercentRepeat(KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, True, Color.Empty, ColorStorageHours)
                If Not KernelDateTime.Minute = 0 Then WriteWhere(" ".Repeat(PercentRepeat(KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, True, Color.Empty, ColorStorageMinutes)
                If Not KernelDateTime.Second = 0 Then WriteWhere(" ".Repeat(PercentRepeat(KernelDateTime.Second, 60, 10)), 5, ProgressFillPositionSeconds, True, Color.Empty, ColorStorageSeconds)

                'Print information
                If Not String.IsNullOrEmpty(ProgressClockInfoTextHours) Then
                    WriteWhere(ProbePlaces(ProgressClockInfoTextHours), 4, InformationPositionHours, True, ColorStorageHours, KernelDateTime.Hour)
                Else
                    WriteWhere("H: {0}/24", 4, InformationPositionHours, True, ColorStorageHours, KernelDateTime.Hour)
                End If
                If Not String.IsNullOrEmpty(ProgressClockInfoTextMinutes) Then
                    WriteWhere(ProbePlaces(ProgressClockInfoTextMinutes), 4, InformationPositionMinutes, True, ColorStorageMinutes, KernelDateTime.Minute)
                Else
                    WriteWhere("M: {0}/60", 4, InformationPositionMinutes, True, ColorStorageMinutes, KernelDateTime.Minute)
                End If
                If Not String.IsNullOrEmpty(ProgressClockInfoTextHours) Then
                    WriteWhere(ProbePlaces(ProgressClockInfoTextSeconds), 4, InformationPositionSeconds, True, ColorStorageSeconds, KernelDateTime.Second)
                Else
                    WriteWhere("S: {0}/60", 4, InformationPositionSeconds, True, ColorStorageSeconds, KernelDateTime.Second)
                End If

                'Print date information
                WriteWhere(Render, Console.WindowWidth / 2 - Render.Length / 2, Console.WindowHeight - 2, ColorStorageSeconds)
            End If
            If ProgressClockCycleColors Then CurrentTicks += 1

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(ProgressClockDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
