
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Module ScreensaverSettings

    '-> ColorMix
    ''' <summary>
    ''' [ColorMix] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public ColorMix255Colors As Boolean
    ''' <summary>
    ''' [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public ColorMixTrueColor As Boolean = True
    ''' <summary>
    ''' [ColorMix] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public ColorMixDelay As Integer = 1
    ''' <summary>
    ''' [ColorMix] Screensaver background color
    ''' </summary>
    Public ColorMixBackgroundColor As String = New Color(ConsoleColor.Red).PlainSequence
    ''' <summary>
    ''' [ColorMix] The minimum red color level (true color)
    ''' </summary>
    Public ColorMixMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [ColorMix] The minimum green color level (true color)
    ''' </summary>
    Public ColorMixMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [ColorMix] The minimum blue color level (true color)
    ''' </summary>
    Public ColorMixMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [ColorMix] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public ColorMixMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [ColorMix] The maximum red color level (true color)
    ''' </summary>
    Public ColorMixMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [ColorMix] The maximum green color level (true color)
    ''' </summary>
    Public ColorMixMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [ColorMix] The maximum blue color level (true color)
    ''' </summary>
    Public ColorMixMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [ColorMix] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public ColorMixMaximumColorLevel As Integer = 255

    '-> Disco
    ''' <summary>
    ''' [Disco] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Disco255Colors As Boolean
    ''' <summary>
    ''' [Disco] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public DiscoTrueColor As Boolean = True
    ''' <summary>
    ''' [Disco] Enable color cycling
    ''' </summary>
    Public DiscoCycleColors As Boolean
    ''' <summary>
    ''' [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
    ''' </summary>
    Public DiscoDelay As Integer = 100
    ''' <summary>
    ''' [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
    ''' </summary>
    Public DiscoUseBeatsPerMinute As Boolean = False
    ''' <summary>
    ''' [Disco] The minimum red color level (true color)
    ''' </summary>
    Public DiscoMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [Disco] The minimum green color level (true color)
    ''' </summary>
    Public DiscoMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [Disco] The minimum blue color level (true color)
    ''' </summary>
    Public DiscoMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [Disco] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public DiscoMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [Disco] The maximum red color level (true color)
    ''' </summary>
    Public DiscoMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [Disco] The maximum green color level (true color)
    ''' </summary>
    Public DiscoMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [Disco] The maximum blue color level (true color)
    ''' </summary>
    Public DiscoMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [Disco] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public DiscoMaximumColorLevel As Integer = 255

    '-> GlitterColor
    ''' <summary>
    ''' [GlitterColor] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public GlitterColor255Colors As Boolean
    ''' <summary>
    ''' [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public GlitterColorTrueColor As Boolean = True
    ''' <summary>
    ''' [GlitterColor] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public GlitterColorDelay As Integer = 1
    ''' <summary>
    ''' [GlitterColor] The minimum red color level (true color)
    ''' </summary>
    Public GlitterColorMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [GlitterColor] The minimum green color level (true color)
    ''' </summary>
    Public GlitterColorMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [GlitterColor] The minimum blue color level (true color)
    ''' </summary>
    Public GlitterColorMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [GlitterColor] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public GlitterColorMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [GlitterColor] The maximum red color level (true color)
    ''' </summary>
    Public GlitterColorMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [GlitterColor] The maximum green color level (true color)
    ''' </summary>
    Public GlitterColorMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [GlitterColor] The maximum blue color level (true color)
    ''' </summary>
    Public GlitterColorMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [GlitterColor] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public GlitterColorMaximumColorLevel As Integer = 255

    '-> Lines
    ''' <summary>
    ''' [Lines] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Lines255Colors As Boolean
    ''' <summary>
    ''' [Lines] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public LinesTrueColor As Boolean = True
    ''' <summary>
    ''' [Lines] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public LinesDelay As Integer = 500
    ''' <summary>
    ''' [Lines] Line character
    ''' </summary>
    Public LinesLineChar As String = "-"
    ''' <summary>
    ''' [Lines] Screensaver background color
    ''' </summary>
    Public LinesBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
    ''' <summary>
    ''' [Lines] The minimum red color level (true color)
    ''' </summary>
    Public LinesMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [Lines] The minimum green color level (true color)
    ''' </summary>
    Public LinesMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [Lines] The minimum blue color level (true color)
    ''' </summary>
    Public LinesMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [Lines] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public LinesMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [Lines] The maximum red color level (true color)
    ''' </summary>
    Public LinesMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [Lines] The maximum green color level (true color)
    ''' </summary>
    Public LinesMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [Lines] The maximum blue color level (true color)
    ''' </summary>
    Public LinesMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [Lines] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public LinesMaximumColorLevel As Integer = 255

    '-> Dissolve
    ''' <summary>
    ''' [Dissolve] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Dissolve255Colors As Boolean
    ''' <summary>
    ''' [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public DissolveTrueColor As Boolean = True
    ''' <summary>
    ''' [Dissolve] Screensaver background color
    ''' </summary>
    Public DissolveBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
    ''' <summary>
    ''' [Dissolve] The minimum red color level (true color)
    ''' </summary>
    Public DissolveMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [Dissolve] The minimum green color level (true color)
    ''' </summary>
    Public DissolveMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [Dissolve] The minimum blue color level (true color)
    ''' </summary>
    Public DissolveMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [Dissolve] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public DissolveMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [Dissolve] The maximum red color level (true color)
    ''' </summary>
    Public DissolveMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [Dissolve] The maximum green color level (true color)
    ''' </summary>
    Public DissolveMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [Dissolve] The maximum blue color level (true color)
    ''' </summary>
    Public DissolveMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [Dissolve] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public DissolveMaximumColorLevel As Integer = 255

    '-> BouncingBlock
    ''' <summary>
    ''' [BouncingBlock] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public BouncingBlock255Colors As Boolean
    ''' <summary>
    ''' [BouncingBlock] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public BouncingBlockTrueColor As Boolean = True
    ''' <summary>
    ''' [BouncingBlock] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public BouncingBlockDelay As Integer = 10
    ''' <summary>
    ''' [BouncingBlock] Screensaver background color
    ''' </summary>
    Public BouncingBlockBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
    ''' <summary>
    ''' [BouncingBlock] Screensaver foreground color
    ''' </summary>
    Public BouncingBlockForegroundColor As String = New Color(ConsoleColor.White).PlainSequence
    ''' <summary>
    ''' [BouncingBlock] The minimum red color level (true color)
    ''' </summary>
    Public BouncingBlockMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingBlock] The minimum green color level (true color)
    ''' </summary>
    Public BouncingBlockMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingBlock] The minimum blue color level (true color)
    ''' </summary>
    Public BouncingBlockMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingBlock] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public BouncingBlockMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingBlock] The maximum red color level (true color)
    ''' </summary>
    Public BouncingBlockMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [BouncingBlock] The maximum green color level (true color)
    ''' </summary>
    Public BouncingBlockMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [BouncingBlock] The maximum blue color level (true color)
    ''' </summary>
    Public BouncingBlockMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [BouncingBlock] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public BouncingBlockMaximumColorLevel As Integer = 255

    '-> BouncingText
    ''' <summary>
    ''' [BouncingText] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public BouncingText255Colors As Boolean
    ''' <summary>
    ''' [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public BouncingTextTrueColor As Boolean = True
    ''' <summary>
    ''' [BouncingText] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public BouncingTextDelay As Integer = 10
    ''' <summary>
    ''' [BouncingText] Text for Bouncing Text. Shorter is better.
    ''' </summary>
    Public BouncingTextWrite As String = "Kernel Simulator"
    ''' <summary>
    ''' [BouncingText] Screensaver background color
    ''' </summary>
    Public BouncingTextBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
    ''' <summary>
    ''' [BouncingText] Screensaver foreground color
    ''' </summary>
    Public BouncingTextForegroundColor As String = New Color(ConsoleColor.White).PlainSequence
    ''' <summary>
    ''' [BouncingText] The minimum red color level (true color)
    ''' </summary>
    Public BouncingTextMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingText] The minimum green color level (true color)
    ''' </summary>
    Public BouncingTextMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingText] The minimum blue color level (true color)
    ''' </summary>
    Public BouncingTextMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingText] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public BouncingTextMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [BouncingText] The maximum red color level (true color)
    ''' </summary>
    Public BouncingTextMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [BouncingText] The maximum green color level (true color)
    ''' </summary>
    Public BouncingTextMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [BouncingText] The maximum blue color level (true color)
    ''' </summary>
    Public BouncingTextMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [BouncingText] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public BouncingTextMaximumColorLevel As Integer = 255

    '-> ProgressClock
    ''' <summary>
    ''' [ProgressClock] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public ProgressClock255Colors As Boolean
    ''' <summary>
    ''' [ProgressClock] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public ProgressClockTrueColor As Boolean = True
    ''' <summary>
    ''' [ProgressClock] Enable color cycling (uses RNG. If disabled, uses the <see cref="ProgressClockSecondsProgressColor"/>, <see cref="ProgressClockMinutesProgressColor"/>, and <see cref="ProgressClockHoursProgressColor"/> colors.)
    ''' </summary>
    Public ProgressClockCycleColors As Boolean = True
    ''' <summary>
    ''' [ProgressClock] The color of seconds progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
    ''' </summary>
    Public ProgressClockSecondsProgressColor As String = 4
    ''' <summary>
    ''' [ProgressClock] The color of minutes progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
    ''' </summary>
    Public ProgressClockMinutesProgressColor As String = 5
    ''' <summary>
    ''' [ProgressClock] The color of hours progress bar. It can be 1-16, 1-255, or "1-255;1-255;1-255".
    ''' </summary>
    Public ProgressClockHoursProgressColor As String = 6
    ''' <summary>
    ''' [ProgressClock] The color of date information. It can be 1-16, 1-255, or "1-255;1-255;1-255".
    ''' </summary>
    Public ProgressClockProgressColor As String = 7
    ''' <summary>
    ''' [ProgressClock] If color cycling is enabled, how many ticks before changing colors? 1 tick = 0.5 seconds
    ''' </summary>
    Public ProgressClockCycleColorsTicks As Long = 20
    ''' <summary>
    ''' [ProgressClock] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public ProgressClockDelay As Integer = 500
    ''' <summary>
    ''' [ProgressClock] Upper left corner character for hours bar
    ''' </summary>
    Public ProgressClockUpperLeftCornerCharHours As String = "+"
    ''' <summary>
    ''' [ProgressClock] Upper left corner character for minutes bar
    ''' </summary>
    Public ProgressClockUpperLeftCornerCharMinutes As String = "+"
    ''' <summary>
    ''' [ProgressClock] Upper left corner character for seconds bar
    ''' </summary>
    Public ProgressClockUpperLeftCornerCharSeconds As String = "+"
    ''' <summary>
    ''' [ProgressClock] Upper right corner character for hours bar
    ''' </summary>
    Public ProgressClockUpperRightCornerCharHours As String = "+"
    ''' <summary>
    ''' [ProgressClock] Upper right corner character for minutes bar
    ''' </summary>
    Public ProgressClockUpperRightCornerCharMinutes As String = "+"
    ''' <summary>
    ''' [ProgressClock] Upper right corner character for seconds bar
    ''' </summary>
    Public ProgressClockUpperRightCornerCharSeconds As String = "+"
    ''' <summary>
    ''' [ProgressClock] Lower left corner character for hours bar
    ''' </summary>
    Public ProgressClockLowerLeftCornerCharHours As String = "+"
    ''' <summary>
    ''' [ProgressClock] Lower left corner character for minutes bar
    ''' </summary>
    Public ProgressClockLowerLeftCornerCharMinutes As String = "+"
    ''' <summary>
    ''' [ProgressClock] Lower left corner character for seconds bar
    ''' </summary>
    Public ProgressClockLowerLeftCornerCharSeconds As String = "+"
    ''' <summary>
    ''' [ProgressClock] Lower right corner character for hours bar
    ''' </summary>
    Public ProgressClockLowerRightCornerCharHours As String = "+"
    ''' <summary>
    ''' [ProgressClock] Lower right corner character for minutes bar
    ''' </summary>
    Public ProgressClockLowerRightCornerCharMinutes As String = "+"
    ''' <summary>
    ''' [ProgressClock] Lower right corner character for seconds bar
    ''' </summary>
    Public ProgressClockLowerRightCornerCharSeconds As String = "+"
    ''' <summary>
    ''' [ProgressClock] Upper frame character for hours bar
    ''' </summary>
    Public ProgressClockUpperFrameCharHours As String = "-"
    ''' <summary>
    ''' [ProgressClock] Upper frame character for minutes bar
    ''' </summary>
    Public ProgressClockUpperFrameCharMinutes As String = "-"
    ''' <summary>
    ''' [ProgressClock] Upper frame character for seconds bar
    ''' </summary>
    Public ProgressClockUpperFrameCharSeconds As String = "-"
    ''' <summary>
    ''' [ProgressClock] Lower frame character for hours bar
    ''' </summary>
    Public ProgressClockLowerFrameCharHours As String = "-"
    ''' <summary>
    ''' [ProgressClock] Lower frame character for minutes bar
    ''' </summary>
    Public ProgressClockLowerFrameCharMinutes As String = "-"
    ''' <summary>
    ''' [ProgressClock] Lower frame character for seconds bar
    ''' </summary>
    Public ProgressClockLowerFrameCharSeconds As String = "-"
    ''' <summary>
    ''' [ProgressClock] Left frame character for hours bar
    ''' </summary>
    Public ProgressClockLeftFrameCharHours As String = "|"
    ''' <summary>
    ''' [ProgressClock] Left frame character for minutes bar
    ''' </summary>
    Public ProgressClockLeftFrameCharMinutes As String = "|"
    ''' <summary>
    ''' [ProgressClock] Left frame character for seconds bar
    ''' </summary>
    Public ProgressClockLeftFrameCharSeconds As String = "|"
    ''' <summary>
    ''' [ProgressClock] Right frame character for hours bar
    ''' </summary>
    Public ProgressClockRightFrameCharHours As String = "|"
    ''' <summary>
    ''' [ProgressClock] Right frame character for minutes bar
    ''' </summary>
    Public ProgressClockRightFrameCharMinutes As String = "|"
    ''' <summary>
    ''' [ProgressClock] Right frame character for seconds bar
    ''' </summary>
    Public ProgressClockRightFrameCharSeconds As String = "|"
    ''' <summary>
    ''' [ProgressClock] Information text for hours bar
    ''' </summary>
    Public ProgressClockInfoTextHours As String = ""
    ''' <summary>
    ''' [ProgressClock] Information text for minutes bar
    ''' </summary>
    Public ProgressClockInfoTextMinutes As String = ""
    ''' <summary>
    ''' [ProgressClock] Information text for seconds bar
    ''' </summary>
    Public ProgressClockInfoTextSeconds As String = ""
    ''' <summary>
    ''' [ProgressClock] The minimum red color level (true color - hours)
    ''' </summary>
    Public ProgressClockMinimumRedColorLevelHours As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum green color level (true color - hours)
    ''' </summary>
    Public ProgressClockMinimumGreenColorLevelHours As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum blue color level (true color - hours)
    ''' </summary>
    Public ProgressClockMinimumBlueColorLevelHours As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum color level (255 colors or 16 colors - hours)
    ''' </summary>
    Public ProgressClockMinimumColorLevelHours As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The maximum red color level (true color - hours)
    ''' </summary>
    Public ProgressClockMaximumRedColorLevelHours As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum green color level (true color - hours)
    ''' </summary>
    Public ProgressClockMaximumGreenColorLevelHours As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum blue color level (true color - hours)
    ''' </summary>
    Public ProgressClockMaximumBlueColorLevelHours As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum color level (255 colors or 16 colors - hours)
    ''' </summary>
    Public ProgressClockMaximumColorLevelHours As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The minimum red color level (true color - minutes)
    ''' </summary>
    Public ProgressClockMinimumRedColorLevelMinutes As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum green color level (true color - minutes)
    ''' </summary>
    Public ProgressClockMinimumGreenColorLevelMinutes As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum blue color level (true color - minutes)
    ''' </summary>
    Public ProgressClockMinimumBlueColorLevelMinutes As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum color level (255 colors or 16 colors - minutes)
    ''' </summary>
    Public ProgressClockMinimumColorLevelMinutes As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The maximum red color level (true color - minutes)
    ''' </summary>
    Public ProgressClockMaximumRedColorLevelMinutes As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum green color level (true color - minutes)
    ''' </summary>
    Public ProgressClockMaximumGreenColorLevelMinutes As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum blue color level (true color - minutes)
    ''' </summary>
    Public ProgressClockMaximumBlueColorLevelMinutes As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum color level (255 colors or 16 colors - minutes)
    ''' </summary>
    Public ProgressClockMaximumColorLevelMinutes As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The minimum red color level (true color - seconds)
    ''' </summary>
    Public ProgressClockMinimumRedColorLevelSeconds As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum green color level (true color - seconds)
    ''' </summary>
    Public ProgressClockMinimumGreenColorLevelSeconds As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum blue color level (true color - seconds)
    ''' </summary>
    Public ProgressClockMinimumBlueColorLevelSeconds As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum color level (255 colors or 16 colors - seconds)
    ''' </summary>
    Public ProgressClockMinimumColorLevelSeconds As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The maximum red color level (true color - seconds)
    ''' </summary>
    Public ProgressClockMaximumRedColorLevelSeconds As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum green color level (true color - seconds)
    ''' </summary>
    Public ProgressClockMaximumGreenColorLevelSeconds As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum blue color level (true color - seconds)
    ''' </summary>
    Public ProgressClockMaximumBlueColorLevelSeconds As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum color level (255 colors or 16 colors - seconds)
    ''' </summary>
    Public ProgressClockMaximumColorLevelSeconds As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The minimum red color level (true color)
    ''' </summary>
    Public ProgressClockMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum green color level (true color)
    ''' </summary>
    Public ProgressClockMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum blue color level (true color)
    ''' </summary>
    Public ProgressClockMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public ProgressClockMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [ProgressClock] The maximum red color level (true color)
    ''' </summary>
    Public ProgressClockMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum green color level (true color)
    ''' </summary>
    Public ProgressClockMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum blue color level (true color)
    ''' </summary>
    Public ProgressClockMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [ProgressClock] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public ProgressClockMaximumColorLevel As Integer = 255

    '-> Lighter
    ''' <summary>
    ''' [Lighter] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Lighter255Colors As Boolean
    ''' <summary>
    ''' [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public LighterTrueColor As Boolean = True
    ''' <summary>
    ''' [Lighter] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public LighterDelay As Integer = 100
    ''' <summary>
    ''' [Lighter] How many positions to write before starting to blacken them?
    ''' </summary>
    Public LighterMaxPositions As Integer = 10
    ''' <summary>
    ''' [Lighter] Screensaver background color
    ''' </summary>
    Public LighterBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
    ''' <summary>
    ''' [Lighter] The minimum red color level (true color)
    ''' </summary>
    Public LighterMinimumRedColorLevel As Integer = 0
    ''' <summary>
    ''' [Lighter] The minimum green color level (true color)
    ''' </summary>
    Public LighterMinimumGreenColorLevel As Integer = 0
    ''' <summary>
    ''' [Lighter] The minimum blue color level (true color)
    ''' </summary>
    Public LighterMinimumBlueColorLevel As Integer = 0
    ''' <summary>
    ''' [Lighter] The minimum color level (255 colors or 16 colors)
    ''' </summary>
    Public LighterMinimumColorLevel As Integer = 0
    ''' <summary>
    ''' [Lighter] The maximum red color level (true color)
    ''' </summary>
    Public LighterMaximumRedColorLevel As Integer = 255
    ''' <summary>
    ''' [Lighter] The maximum green color level (true color)
    ''' </summary>
    Public LighterMaximumGreenColorLevel As Integer = 255
    ''' <summary>
    ''' [Lighter] The maximum blue color level (true color)
    ''' </summary>
    Public LighterMaximumBlueColorLevel As Integer = 255
    ''' <summary>
    ''' [Lighter] The maximum color level (255 colors or 16 colors)
    ''' </summary>
    Public LighterMaximumColorLevel As Integer = 255

    '-> Wipe
    ''' <summary>
    ''' [Wipe] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Wipe255Colors As Boolean
    ''' <summary>
    ''' [Wipe] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public WipeTrueColor As Boolean = True
    ''' <summary>
    ''' [Wipe] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public WipeDelay As Integer = 10
    ''' <summary>
    ''' [Wipe] How many wipes needed to change direction?
    ''' </summary>
    Public WipeWipesNeededToChangeDirection As Integer = 10

    '-> HackUserFromAD
    ''' <summary>
    ''' [HackUserFromAD] Sets the console foreground color to green to represent "Hacker Mode"
    ''' </summary>
    Public HackUserFromADHackerMode As Boolean = True

    '-> AptErrorSim
    ''' <summary>
    ''' [AptErrorSim] Sets the console foreground color to green to represent "Hacker Mode"
    ''' </summary>
    Public AptErrorSimHackerMode As Boolean

    '-> Marquee
    ''' <summary>
    ''' [Marquee] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Marquee255Colors As Boolean
    ''' <summary>
    ''' [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public MarqueeTrueColor As Boolean = True
    ''' <summary>
    ''' [Marquee] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public MarqueeDelay As Integer = 10
    ''' <summary>
    ''' [Marquee] Text for Marquee. Shorter is better.
    ''' </summary>
    Public MarqueeWrite As String = "Kernel Simulator"
    ''' <summary>
    ''' [Marquee] Whether the text is always on center.
    ''' </summary>
    Public MarqueeAlwaysCentered As Boolean = True
    ''' <summary>
    ''' [Marquee] Whether to use the Console.Clear() API (slow) or use the line-clearing VT sequence (fast).
    ''' </summary>
    Public MarqueeUseConsoleAPI As Boolean = False

    '-> BeatFader
    ''' <summary>
    ''' [BeatFader] Enable 255 color support. Has a higher priority than 16 color support. Please note that it only works if color cycling is enabled.
    ''' </summary>
    Public BeatFader255Colors As Boolean
    ''' <summary>
    ''' [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
    ''' </summary>
    Public BeatFaderTrueColor As Boolean = True
    ''' <summary>
    ''' [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
    ''' </summary>
    Public BeatFaderCycleColors As Boolean = True
    ''' <summary>
    ''' [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
    ''' </summary>
    Public BeatFaderBeatColor As String = 17
    ''' <summary>
    ''' [BeatFader] How many beats per minute to wait before making the next write?
    ''' </summary>
    Public BeatFaderDelay As Integer = 120
    ''' <summary>
    ''' [BeatFader] How many fade steps to do?
    ''' </summary>
    Public BeatFaderMaxSteps As Integer = 25

    '-> GlitterMatrix
    ''' <summary>
    ''' [GlitterMatrix] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public GlitterMatrixDelay As Integer = 1
    ''' <summary>
    ''' [GlitterMatrix] Screensaver background color
    ''' </summary>
    Public GlitterMatrixBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
    ''' <summary>
    ''' [GlitterMatrix] Screensaver foreground color
    ''' </summary>
    Public GlitterMatrixForegroundColor As String = New Color(ConsoleColor.Green).PlainSequence

    '-> Matrix
    ''' <summary>
    ''' [Matrix] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public MatrixDelay As Integer = 1

    '-> Fader
    ''' <summary>
    ''' [Fader] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public FaderDelay As Integer = 50
    ''' <summary>
    ''' [Fader] How many milliseconds to wait before fading the text out?
    ''' </summary>
    Public FaderFadeOutDelay As Integer = 3000
    ''' <summary>
    ''' [FaderBack] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public FaderBackDelay As Integer = 10
    ''' <summary>
    ''' [FaderBack] How many milliseconds to wait before fading the text out?
    ''' </summary>
    Public FaderBackFadeOutDelay As Integer = 3000
    ''' <summary>
    ''' [Fader] Text for Fader. Shorter is better.
    ''' </summary>
    Public FaderWrite As String = "Kernel Simulator"
    ''' <summary>
    ''' [Fader] How many fade steps to do?
    ''' </summary>
    Public FaderMaxSteps As Integer = 25
    ''' <summary>
    ''' [FaderBack] How many fade steps to do?
    ''' </summary>
    Public FaderBackMaxSteps As Integer = 25

    '-> Typo
    ''' <summary>
    ''' [Typo] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public TypoDelay As Integer = 50
    ''' <summary>
    ''' [Typo] How many milliseconds to wait before writing the text again?
    ''' </summary>
    Public TypoWriteAgainDelay As Integer = 3000
    ''' <summary>
    ''' [Typo] Text for Typo. Longer is better.
    ''' </summary>
    Public TypoWrite As String = "Kernel Simulator"
    ''' <summary>
    ''' [Typo] Minimum writing speed in WPM
    ''' </summary>
    Public TypoWritingSpeedMin As Integer = 50
    ''' <summary>
    ''' [Typo] Maximum writing speed in WPM
    ''' </summary>
    Public TypoWritingSpeedMax As Integer = 80
    ''' <summary>
    ''' [Typo] Possibility that the writer made a typo in percent
    ''' </summary>
    Public TypoMissStrikePossibility As Integer = 20
    ''' <summary>
    ''' [Typo] Possibility that the writer missed a character in percent
    ''' </summary>
    Public TypoMissPossibility As Integer = 10

    '-> Linotypo
    ''' <summary>
    ''' [Linotypo] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public LinotypoDelay As Integer = 50
    ''' <summary>
    ''' [Linotypo] How many milliseconds to wait before writing the text in the new screen again?
    ''' </summary>
    Public LinotypoNewScreenDelay As Integer = 3000
    ''' <summary>
    ''' [Linotypo] Text for Linotypo. Longer is better.
    ''' </summary>
    Public LinotypoWrite As String = "Kernel Simulator"
    ''' <summary>
    ''' [Linotypo] Minimum writing speed in WPM
    ''' </summary>
    Public LinotypoWritingSpeedMin As Integer = 50
    ''' <summary>
    ''' [Linotypo] Maximum writing speed in WPM
    ''' </summary>
    Public LinotypoWritingSpeedMax As Integer = 80
    ''' <summary>
    ''' [Linotypo] Possibility that the writer made a typo in percent
    ''' </summary>
    Public LinotypoMissStrikePossibility As Integer = 1
    ''' <summary>
    ''' [Linotypo] The text columns to be printed.
    ''' </summary>
    Public LinotypoTextColumns As Integer = 3
    ''' <summary>
    ''' [Linotypo] How many characters to write before triggering the "line fill"?
    ''' </summary>
    Public LinotypoEtaoinThreshold As Integer = 5
    ''' <summary>
    ''' [Linotypo] Possibility that the Etaoin pattern will be printed in all caps in percent
    ''' </summary>
    Public LinotypoEtaoinCappingPossibility As Integer = 5
    ''' <summary>
    ''' [Linotypo] Line fill pattern type
    ''' </summary>
    Public LinotypoEtaoinType As FillType = FillType.EtaoinPattern
    ''' <summary>
    ''' [Linotypo] Possibility that the writer missed a character in percent
    ''' </summary>
    Public LinotypoMissPossibility As Integer = 10

    '-> Typewriter
    ''' <summary>
    ''' [Typewriter] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public TypewriterDelay As Integer = 50
    ''' <summary>
    ''' [Typewriter] How many milliseconds to wait before writing the text in the new screen again?
    ''' </summary>
    Public TypewriterNewScreenDelay As Integer = 3000
    ''' <summary>
    ''' [Typewriter] Text for Typewriter. Longer is better.
    ''' </summary>
    Public TypewriterWrite As String = "Kernel Simulator"
    ''' <summary>
    ''' [Typewriter] Minimum writing speed in WPM
    ''' </summary>
    Public TypewriterWritingSpeedMin As Integer = 50
    ''' <summary>
    ''' [Typewriter] Maximum writing speed in WPM
    ''' </summary>
    Public TypewriterWritingSpeedMax As Integer = 80
    ''' <summary>
    ''' [Typewriter] Shows the typewriter letter column position by showing this key on the bottom of the screen: <code>^</code>
    ''' </summary>
    Public TypewriterShowArrowPos As Boolean = True

    '-> FlashColor
    ''' <summary>
    ''' [FlashColor] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public FlashColor255Colors As Boolean
    ''' <summary>
    ''' [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public FlashColorTrueColor As Boolean = True
    ''' <summary>
    ''' [FlashColor] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public FlashColorDelay As Integer = 20

    '-> SpotWrite
    ''' <summary>
    ''' [SpotWrite] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public SpotWriteDelay As Integer = 100
    ''' <summary>
    ''' [SpotWrite] Text for SpotWrite. Longer is better.
    ''' </summary>
    Public SpotWriteWrite As String = "Kernel Simulator"
    ''' <summary>
    ''' [SpotWrite] How many milliseconds to wait before writing the text in the new screen again?
    ''' </summary>
    Public SpotWriteNewScreenDelay As Integer = 3000

    '-> Ramp
    ''' <summary>
    ''' [Ramp] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Ramp255Colors As Boolean
    ''' <summary>
    ''' [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public RampTrueColor As Boolean = True
    ''' <summary>
    ''' [Ramp] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public RampDelay As Integer = 20
    ''' <summary>
    ''' [Ramp] How many milliseconds to wait before starting the next ramp?
    ''' </summary>
    Public RampNextRampDelay As Integer = 250

End Module
