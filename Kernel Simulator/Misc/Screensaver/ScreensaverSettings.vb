
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

Imports KS.Misc.Screensaver.Displays

Namespace Misc.Screensaver
    Public Module ScreensaverSettings

        'TODO: This module is too fat! Move the settings to their own <saver>Display module, publicizing them in the process.
        '-> ColorMix
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

        '-> Disco
        Private _disco255Colors As Boolean
        Private _discoTrueColor As Boolean = True
        Private _discoCycleColors As Boolean
        Private _discoDelay As Integer = 100
        Private _discoUseBeatsPerMinute As Boolean
        Private _discoEnableFedMode As Boolean
        Private _discoMinimumRedColorLevel As Integer = 0
        Private _discoMinimumGreenColorLevel As Integer = 0
        Private _discoMinimumBlueColorLevel As Integer = 0
        Private _discoMinimumColorLevel As Integer = 0
        Private _discoMaximumRedColorLevel As Integer = 255
        Private _discoMaximumGreenColorLevel As Integer = 255
        Private _discoMaximumBlueColorLevel As Integer = 255
        Private _discoMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Disco] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Disco255Colors As Boolean
            Get
                Return _disco255Colors
            End Get
            Set(value As Boolean)
                _disco255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property DiscoTrueColor As Boolean
            Get
                Return _discoTrueColor
            End Get
            Set(value As Boolean)
                _discoTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Enable color cycling
        ''' </summary>
        Public Property DiscoCycleColors As Boolean
            Get
                Return _discoCycleColors
            End Get
            Set(value As Boolean)
                _discoCycleColors = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        ''' </summary>
        Public Property DiscoDelay As Integer
            Get
                Return _discoDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 100
                _discoDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        ''' </summary>
        Public Property DiscoUseBeatsPerMinute As Boolean
            Get
                Return _discoUseBeatsPerMinute
            End Get
            Set(value As Boolean)
                _discoUseBeatsPerMinute = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        ''' </summary>
        Public Property DiscoEnableFedMode As Boolean
            Get
                Return _discoEnableFedMode
            End Get
            Set(value As Boolean)
                _discoEnableFedMode = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum red color level (true color)
        ''' </summary>
        Public Property DiscoMinimumRedColorLevel As Integer
            Get
                Return _discoMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _discoMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum green color level (true color)
        ''' </summary>
        Public Property DiscoMinimumGreenColorLevel As Integer
            Get
                Return _discoMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _discoMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum blue color level (true color)
        ''' </summary>
        Public Property DiscoMinimumBlueColorLevel As Integer
            Get
                Return _discoMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _discoMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DiscoMinimumColorLevel As Integer
            Get
                Return _discoMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_disco255Colors Or _discoTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _discoMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum red color level (true color)
        ''' </summary>
        Public Property DiscoMaximumRedColorLevel As Integer
            Get
                Return _discoMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _discoMinimumRedColorLevel Then value = _discoMinimumRedColorLevel
                If value > 255 Then value = 255
                _discoMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum green color level (true color)
        ''' </summary>
        Public Property DiscoMaximumGreenColorLevel As Integer
            Get
                Return _discoMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _discoMinimumGreenColorLevel Then value = _discoMinimumGreenColorLevel
                If value > 255 Then value = 255
                _discoMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum blue color level (true color)
        ''' </summary>
        Public Property DiscoMaximumBlueColorLevel As Integer
            Get
                Return _discoMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _discoMinimumBlueColorLevel Then value = _discoMinimumBlueColorLevel
                If value > 255 Then value = 255
                _discoMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DiscoMaximumColorLevel As Integer
            Get
                Return _discoMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_disco255Colors Or _discoTrueColor, 255, 15)
                If value <= _discoMinimumColorLevel Then value = _discoMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _discoMaximumColorLevel = value
            End Set
        End Property

        '-> GlitterColor
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

        '-> Lines
        Private _lines255Colors As Boolean
        Private _linesTrueColor As Boolean = True
        Private _linesDelay As Integer = 500
        Private _linesLineChar As String = "-"
        Private _linesBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _linesMinimumRedColorLevel As Integer = 0
        Private _linesMinimumGreenColorLevel As Integer = 0
        Private _linesMinimumBlueColorLevel As Integer = 0
        Private _linesMinimumColorLevel As Integer = 0
        Private _linesMaximumRedColorLevel As Integer = 255
        Private _linesMaximumGreenColorLevel As Integer = 255
        Private _linesMaximumBlueColorLevel As Integer = 255
        Private _linesMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Lines] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Lines255Colors As Boolean
            Get
                Return _lines255Colors
            End Get
            Set(value As Boolean)
                _lines255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property LinesTrueColor As Boolean
            Get
                Return _linesTrueColor
            End Get
            Set(value As Boolean)
                _linesTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property LinesDelay As Integer
            Get
                Return _linesDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 500
                _linesDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] Line character
        ''' </summary>
        Public Property LinesLineChar As String
            Get
                Return _linesLineChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "-"
                _linesLineChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] Screensaver background color
        ''' </summary>
        Public Property LinesBackgroundColor As String
            Get
                Return _linesBackgroundColor
            End Get
            Set(value As String)
                _linesBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The minimum red color level (true color)
        ''' </summary>
        Public Property LinesMinimumRedColorLevel As Integer
            Get
                Return _linesMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _linesMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The minimum green color level (true color)
        ''' </summary>
        Public Property LinesMinimumGreenColorLevel As Integer
            Get
                Return _linesMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _linesMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The minimum blue color level (true color)
        ''' </summary>
        Public Property LinesMinimumBlueColorLevel As Integer
            Get
                Return _linesMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _linesMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property LinesMinimumColorLevel As Integer
            Get
                Return _linesMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_lines255Colors Or _linesTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _linesMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The maximum red color level (true color)
        ''' </summary>
        Public Property LinesMaximumRedColorLevel As Integer
            Get
                Return _linesMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _linesMinimumRedColorLevel Then value = _linesMinimumRedColorLevel
                If value > 255 Then value = 255
                _linesMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The maximum green color level (true color)
        ''' </summary>
        Public Property LinesMaximumGreenColorLevel As Integer
            Get
                Return _linesMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _linesMinimumGreenColorLevel Then value = _linesMinimumGreenColorLevel
                If value > 255 Then value = 255
                _linesMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The maximum blue color level (true color)
        ''' </summary>
        Public Property LinesMaximumBlueColorLevel As Integer
            Get
                Return _linesMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _linesMinimumBlueColorLevel Then value = _linesMinimumBlueColorLevel
                If value > 255 Then value = 255
                _linesMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lines] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property LinesMaximumColorLevel As Integer
            Get
                Return _linesMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_lines255Colors Or _linesTrueColor, 255, 15)
                If value <= _linesMinimumColorLevel Then value = _linesMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _linesMaximumColorLevel = value
            End Set
        End Property

        '-> Dissolve
        Private _dissolve255Colors As Boolean
        Private _dissolveTrueColor As Boolean = True
        Private _dissolveBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _dissolveMinimumRedColorLevel As Integer = 0
        Private _dissolveMinimumGreenColorLevel As Integer = 0
        Private _dissolveMinimumBlueColorLevel As Integer = 0
        Private _dissolveMinimumColorLevel As Integer = 0
        Private _dissolveMaximumRedColorLevel As Integer = 255
        Private _dissolveMaximumGreenColorLevel As Integer = 255
        Private _dissolveMaximumBlueColorLevel As Integer = 255
        Private _dissolveMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Dissolve] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Dissolve255Colors As Boolean
            Get
                Return _dissolve255Colors
            End Get
            Set(value As Boolean)
                _dissolve255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property DissolveTrueColor As Boolean
            Get
                Return _dissolveTrueColor
            End Get
            Set(value As Boolean)
                _dissolveTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] Screensaver background color
        ''' </summary>
        Public Property DissolveBackgroundColor As String
            Get
                Return _dissolveBackgroundColor
            End Get
            Set(value As String)
                _dissolveBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The minimum red color level (true color)
        ''' </summary>
        Public Property DissolveMinimumRedColorLevel As Integer
            Get
                Return _dissolveMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dissolveMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The minimum green color level (true color)
        ''' </summary>
        Public Property DissolveMinimumGreenColorLevel As Integer
            Get
                Return _dissolveMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dissolveMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The minimum blue color level (true color)
        ''' </summary>
        Public Property DissolveMinimumBlueColorLevel As Integer
            Get
                Return _dissolveMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dissolveMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DissolveMinimumColorLevel As Integer
            Get
                Return _dissolveMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_dissolve255Colors Or _dissolveTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _dissolveMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The maximum red color level (true color)
        ''' </summary>
        Public Property DissolveMaximumRedColorLevel As Integer
            Get
                Return _dissolveMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _dissolveMinimumRedColorLevel Then value = _dissolveMinimumRedColorLevel
                If value > 255 Then value = 255
                _dissolveMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The maximum green color level (true color)
        ''' </summary>
        Public Property DissolveMaximumGreenColorLevel As Integer
            Get
                Return _dissolveMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _dissolveMinimumGreenColorLevel Then value = _dissolveMinimumGreenColorLevel
                If value > 255 Then value = 255
                _dissolveMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The maximum blue color level (true color)
        ''' </summary>
        Public Property DissolveMaximumBlueColorLevel As Integer
            Get
                Return _dissolveMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _dissolveMinimumBlueColorLevel Then value = _dissolveMinimumBlueColorLevel
                If value > 255 Then value = 255
                _dissolveMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Dissolve] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DissolveMaximumColorLevel As Integer
            Get
                Return _dissolveMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_dissolve255Colors Or _dissolveTrueColor, 255, 15)
                If value <= _dissolveMinimumColorLevel Then value = _dissolveMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _dissolveMaximumColorLevel = value
            End Set
        End Property

        '-> BouncingBlock
        Private _bouncingBlock255Colors As Boolean
        Private _bouncingBlockTrueColor As Boolean = True
        Private _bouncingBlockDelay As Integer = 10
        Private _bouncingBlockBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _bouncingBlockForegroundColor As String = New Color(ConsoleColor.White).PlainSequence
        Private _bouncingBlockMinimumRedColorLevel As Integer = 0
        Private _bouncingBlockMinimumGreenColorLevel As Integer = 0
        Private _bouncingBlockMinimumBlueColorLevel As Integer = 0
        Private _bouncingBlockMinimumColorLevel As Integer = 0
        Private _bouncingBlockMaximumRedColorLevel As Integer = 255
        Private _bouncingBlockMaximumGreenColorLevel As Integer = 255
        Private _bouncingBlockMaximumBlueColorLevel As Integer = 255
        Private _bouncingBlockMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [BouncingBlock] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property BouncingBlock255Colors As Boolean
            Get
                Return _bouncingBlock255Colors
            End Get
            Set(value As Boolean)
                _bouncingBlock255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property BouncingBlockTrueColor As Boolean
            Get
                Return _bouncingBlockTrueColor
            End Get
            Set(value As Boolean)
                _bouncingBlockTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property BouncingBlockDelay As Integer
            Get
                Return _bouncingBlockDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _bouncingBlockDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] Screensaver background color
        ''' </summary>
        Public Property BouncingBlockBackgroundColor As String
            Get
                Return _bouncingBlockBackgroundColor
            End Get
            Set(value As String)
                _bouncingBlockBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] Screensaver foreground color
        ''' </summary>
        Public Property BouncingBlockForegroundColor As String
            Get
                Return _bouncingBlockForegroundColor
            End Get
            Set(value As String)
                _bouncingBlockForegroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The minimum red color level (true color)
        ''' </summary>
        Public Property BouncingBlockMinimumRedColorLevel As Integer
            Get
                Return _bouncingBlockMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingBlockMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The minimum green color level (true color)
        ''' </summary>
        Public Property BouncingBlockMinimumGreenColorLevel As Integer
            Get
                Return _bouncingBlockMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingBlockMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The minimum blue color level (true color)
        ''' </summary>
        Public Property BouncingBlockMinimumBlueColorLevel As Integer
            Get
                Return _bouncingBlockMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingBlockMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BouncingBlockMinimumColorLevel As Integer
            Get
                Return _bouncingBlockMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_bouncingBlock255Colors Or _bouncingBlockTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _bouncingBlockMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The maximum red color level (true color)
        ''' </summary>
        Public Property BouncingBlockMaximumRedColorLevel As Integer
            Get
                Return _bouncingBlockMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingBlockMinimumRedColorLevel Then value = _bouncingBlockMinimumRedColorLevel
                If value > 255 Then value = 255
                _bouncingBlockMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The maximum green color level (true color)
        ''' </summary>
        Public Property BouncingBlockMaximumGreenColorLevel As Integer
            Get
                Return _bouncingBlockMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingBlockMinimumGreenColorLevel Then value = _bouncingBlockMinimumGreenColorLevel
                If value > 255 Then value = 255
                _bouncingBlockMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The maximum blue color level (true color)
        ''' </summary>
        Public Property BouncingBlockMaximumBlueColorLevel As Integer
            Get
                Return _bouncingBlockMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingBlockMinimumBlueColorLevel Then value = _bouncingBlockMinimumBlueColorLevel
                If value > 255 Then value = 255
                _bouncingBlockMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingBlock] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BouncingBlockMaximumColorLevel As Integer
            Get
                Return _bouncingBlockMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_bouncingBlock255Colors Or _bouncingBlockTrueColor, 255, 15)
                If value <= _bouncingBlockMinimumColorLevel Then value = _bouncingBlockMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _bouncingBlockMaximumColorLevel = value
            End Set
        End Property

        '-> BouncingText
        Private _bouncingText255Colors As Boolean
        Private _bouncingTextTrueColor As Boolean = True
        Private _bouncingTextDelay As Integer = 10
        Private _bouncingTextWrite As String = "Kernel Simulator"
        Private _bouncingTextBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _bouncingTextForegroundColor As String = New Color(ConsoleColor.White).PlainSequence
        Private _bouncingTextMinimumRedColorLevel As Integer = 0
        Private _bouncingTextMinimumGreenColorLevel As Integer = 0
        Private _bouncingTextMinimumBlueColorLevel As Integer = 0
        Private _bouncingTextMinimumColorLevel As Integer = 0
        Private _bouncingTextMaximumRedColorLevel As Integer = 255
        Private _bouncingTextMaximumGreenColorLevel As Integer = 255
        Private _bouncingTextMaximumBlueColorLevel As Integer = 255
        Private _bouncingTextMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [BouncingText] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property BouncingText255Colors As Boolean
            Get
                Return _bouncingText255Colors
            End Get
            Set(value As Boolean)
                _bouncingText255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property BouncingTextTrueColor As Boolean
            Get
                Return _bouncingTextTrueColor
            End Get
            Set(value As Boolean)
                _bouncingTextTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property BouncingTextDelay As Integer
            Get
                Return _bouncingTextDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _bouncingTextDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Text for Bouncing Text. Shorter is better.
        ''' </summary>
        Public Property BouncingTextWrite As String
            Get
                Return _bouncingTextWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _bouncingTextWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Screensaver background color
        ''' </summary>
        Public Property BouncingTextBackgroundColor As String
            Get
                Return _bouncingTextBackgroundColor
            End Get
            Set(value As String)
                _bouncingTextBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Screensaver foreground color
        ''' </summary>
        Public Property BouncingTextForegroundColor As String
            Get
                Return _bouncingTextForegroundColor
            End Get
            Set(value As String)
                _bouncingTextForegroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum red color level (true color)
        ''' </summary>
        Public Property BouncingTextMinimumRedColorLevel As Integer
            Get
                Return _bouncingTextMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingTextMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum green color level (true color)
        ''' </summary>
        Public Property BouncingTextMinimumGreenColorLevel As Integer
            Get
                Return _bouncingTextMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingTextMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum blue color level (true color)
        ''' </summary>
        Public Property BouncingTextMinimumBlueColorLevel As Integer
            Get
                Return _bouncingTextMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingTextMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BouncingTextMinimumColorLevel As Integer
            Get
                Return _bouncingTextMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_bouncingText255Colors Or _bouncingTextTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _bouncingTextMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum red color level (true color)
        ''' </summary>
        Public Property BouncingTextMaximumRedColorLevel As Integer
            Get
                Return _bouncingTextMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingTextMinimumRedColorLevel Then value = _bouncingTextMinimumRedColorLevel
                If value > 255 Then value = 255
                _bouncingTextMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum green color level (true color)
        ''' </summary>
        Public Property BouncingTextMaximumGreenColorLevel As Integer
            Get
                Return _bouncingTextMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingTextMinimumGreenColorLevel Then value = _bouncingTextMinimumGreenColorLevel
                If value > 255 Then value = 255
                _bouncingTextMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum blue color level (true color)
        ''' </summary>
        Public Property BouncingTextMaximumBlueColorLevel As Integer
            Get
                Return _bouncingTextMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingTextMinimumBlueColorLevel Then value = _bouncingTextMinimumBlueColorLevel
                If value > 255 Then value = 255
                _bouncingTextMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BouncingTextMaximumColorLevel As Integer
            Get
                Return _bouncingTextMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_bouncingText255Colors Or _bouncingTextTrueColor, 255, 15)
                If value <= _bouncingTextMinimumColorLevel Then value = _bouncingTextMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _bouncingTextMaximumColorLevel = value
            End Set
        End Property

        '-> ProgressClock
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

        '-> Lighter
        Private _lighter255Colors As Boolean
        Private _lighterTrueColor As Boolean = True
        Private _lighterDelay As Integer = 100
        Private _lighterMaxPositions As Integer = 10
        Private _lighterBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _lighterMinimumRedColorLevel As Integer = 0
        Private _lighterMinimumGreenColorLevel As Integer = 0
        Private _lighterMinimumBlueColorLevel As Integer = 0
        Private _lighterMinimumColorLevel As Integer = 0
        Private _lighterMaximumRedColorLevel As Integer = 255
        Private _lighterMaximumGreenColorLevel As Integer = 255
        Private _lighterMaximumBlueColorLevel As Integer = 255
        Private _lighterMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Lighter] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Lighter255Colors As Boolean
            Get
                Return _lighter255Colors
            End Get
            Set(value As Boolean)
                _lighter255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property LighterTrueColor As Boolean
            Get
                Return _lighterTrueColor
            End Get
            Set(value As Boolean)
                _lighterTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property LighterDelay As Integer
            Get
                Return _lighterDelay
            End Get
            Set(value As Integer)
                _lighterDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] How many positions to write before starting to blacken them?
        ''' </summary>
        Public Property LighterMaxPositions As Integer
            Get
                Return _lighterMaxPositions
            End Get
            Set(value As Integer)
                _lighterMaxPositions = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] Screensaver background color
        ''' </summary>
        Public Property LighterBackgroundColor As String
            Get
                Return _lighterBackgroundColor
            End Get
            Set(value As String)
                _lighterBackgroundColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The minimum red color level (true color)
        ''' </summary>
        Public Property LighterMinimumRedColorLevel As Integer
            Get
                Return _lighterMinimumRedColorLevel
            End Get
            Set(value As Integer)
                _lighterMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The minimum green color level (true color)
        ''' </summary>
        Public Property LighterMinimumGreenColorLevel As Integer
            Get
                Return _lighterMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                _lighterMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The minimum blue color level (true color)
        ''' </summary>
        Public Property LighterMinimumBlueColorLevel As Integer
            Get
                Return _lighterMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                _lighterMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property LighterMinimumColorLevel As Integer
            Get
                Return _lighterMinimumColorLevel
            End Get
            Set(value As Integer)
                _lighterMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The maximum red color level (true color)
        ''' </summary>
        Public Property LighterMaximumRedColorLevel As Integer
            Get
                Return _lighterMaximumRedColorLevel
            End Get
            Set(value As Integer)
                _lighterMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The maximum green color level (true color)
        ''' </summary>
        Public Property LighterMaximumGreenColorLevel As Integer
            Get
                Return _lighterMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                _lighterMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The maximum blue color level (true color)
        ''' </summary>
        Public Property LighterMaximumBlueColorLevel As Integer
            Get
                Return _lighterMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                _lighterMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Lighter] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property LighterMaximumColorLevel As Integer
            Get
                Return _lighterMaximumColorLevel
            End Get
            Set(value As Integer)
                _lighterMaximumColorLevel = value
            End Set
        End Property

        '-> Wipe
        Private _wipe255Colors As Boolean
        Private _wipeTrueColor As Boolean = True
        Private _wipeDelay As Integer = 10
        Private _wipeWipesNeededToChangeDirection As Integer = 10
        Private _wipeBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _wipeMinimumRedColorLevel As Integer = 0
        Private _wipeMinimumGreenColorLevel As Integer = 0
        Private _wipeMinimumBlueColorLevel As Integer = 0
        Private _wipeMinimumColorLevel As Integer = 0
        Private _wipeMaximumRedColorLevel As Integer = 255
        Private _wipeMaximumGreenColorLevel As Integer = 255
        Private _wipeMaximumBlueColorLevel As Integer = 255
        Private _wipeMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Wipe] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Wipe255Colors As Boolean
            Get
                Return _wipe255Colors
            End Get
            Set(value As Boolean)
                _wipe255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property WipeTrueColor As Boolean
            Get
                Return _wipeTrueColor
            End Get
            Set(value As Boolean)
                _wipeTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property WipeDelay As Integer
            Get
                Return _wipeDelay
            End Get
            Set(value As Integer)
                _wipeDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] How many wipes needed to change direction?
        ''' </summary>
        Public Property WipeWipesNeededToChangeDirection As Integer
            Get
                Return _wipeWipesNeededToChangeDirection
            End Get
            Set(value As Integer)
                _wipeWipesNeededToChangeDirection = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] Screensaver background color
        ''' </summary>
        Public Property WipeBackgroundColor As String
            Get
                Return _wipeBackgroundColor
            End Get
            Set(value As String)
                _wipeBackgroundColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The minimum red color level (true color)
        ''' </summary>
        Public Property WipeMinimumRedColorLevel As Integer
            Get
                Return _wipeMinimumRedColorLevel
            End Get
            Set(value As Integer)
                _wipeMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The minimum green color level (true color)
        ''' </summary>
        Public Property WipeMinimumGreenColorLevel As Integer
            Get
                Return _wipeMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                _wipeMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The minimum blue color level (true color)
        ''' </summary>
        Public Property WipeMinimumBlueColorLevel As Integer
            Get
                Return _wipeMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                _wipeMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property WipeMinimumColorLevel As Integer
            Get
                Return _wipeMinimumColorLevel
            End Get
            Set(value As Integer)
                _wipeMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The maximum red color level (true color)
        ''' </summary>
        Public Property WipeMaximumRedColorLevel As Integer
            Get
                Return _wipeMaximumRedColorLevel
            End Get
            Set(value As Integer)
                _wipeMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The maximum green color level (true color)
        ''' </summary>
        Public Property WipeMaximumGreenColorLevel As Integer
            Get
                Return _wipeMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                _wipeMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The maximum blue color level (true color)
        ''' </summary>
        Public Property WipeMaximumBlueColorLevel As Integer
            Get
                Return _wipeMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                _wipeMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Wipe] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property WipeMaximumColorLevel As Integer
            Get
                Return _wipeMaximumColorLevel
            End Get
            Set(value As Integer)
                _wipeMaximumColorLevel = value
            End Set
        End Property

        '-> Marquee
        Private _marquee255Colors As Boolean
        Private _marqueeTrueColor As Boolean = True
        Private _marqueeDelay As Integer = 10
        Private _marqueeWrite As String = "Kernel Simulator"
        Private _marqueeAlwaysCentered As Boolean = True
        Private _marqueeUseConsoleAPI As Boolean = False
        Private _marqueeBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _marqueeMinimumRedColorLevel As Integer = 0
        Private _marqueeMinimumGreenColorLevel As Integer = 0
        Private _marqueeMinimumBlueColorLevel As Integer = 0
        Private _marqueeMinimumColorLevel As Integer = 0
        Private _marqueeMaximumRedColorLevel As Integer = 255
        Private _marqueeMaximumGreenColorLevel As Integer = 255
        Private _marqueeMaximumBlueColorLevel As Integer = 255
        Private _marqueeMaximumColorLevel As Integer = 0

        ''' <summary>
        ''' [Marquee] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Marquee255Colors As Boolean
            Get
                Return _marquee255Colors
            End Get
            Set(value As Boolean)
                _marquee255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property MarqueeTrueColor As Boolean
            Get
                Return _marqueeTrueColor
            End Get
            Set(value As Boolean)
                _marqueeTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property MarqueeDelay As Integer
            Get
                Return _marqueeDelay
            End Get
            Set(value As Integer)
                _marqueeDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Text for Marquee. Shorter is better.
        ''' </summary>
        Public Property MarqueeWrite As String
            Get
                Return _marqueeWrite
            End Get
            Set(value As String)
                _marqueeWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Whether the text is always on center.
        ''' </summary>
        Public Property MarqueeAlwaysCentered As Boolean
            Get
                Return _marqueeAlwaysCentered
            End Get
            Set(value As Boolean)
                _marqueeAlwaysCentered = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Whether to use the Console.Clear() API (slow) or use the line-clearing VT sequence (fast).
        ''' </summary>
        Public Property MarqueeUseConsoleAPI As Boolean
            Get
                Return _marqueeUseConsoleAPI
            End Get
            Set(value As Boolean)
                _marqueeUseConsoleAPI = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Screensaver background color
        ''' </summary>
        Public Property MarqueeBackgroundColor As String
            Get
                Return _marqueeBackgroundColor
            End Get
            Set(value As String)
                _marqueeBackgroundColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum red color level (true color)
        ''' </summary>
        Public Property MarqueeMinimumRedColorLevel As Integer
            Get
                Return _marqueeMinimumRedColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum green color level (true color)
        ''' </summary>
        Public Property MarqueeMinimumGreenColorLevel As Integer
            Get
                Return _marqueeMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum blue color level (true color)
        ''' </summary>
        Public Property MarqueeMinimumBlueColorLevel As Integer
            Get
                Return _marqueeMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property MarqueeMinimumColorLevel As Integer
            Get
                Return _marqueeMinimumColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum red color level (true color)
        ''' </summary>
        Public Property MarqueeMaximumRedColorLevel As Integer
            Get
                Return _marqueeMaximumRedColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum green color level (true color)
        ''' </summary>
        Public Property MarqueeMaximumGreenColorLevel As Integer
            Get
                Return _marqueeMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum blue color level (true color)
        ''' </summary>
        Public Property MarqueeMaximumBlueColorLevel As Integer
            Get
                Return _marqueeMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property MarqueeMaximumColorLevel As Integer
            Get
                Return _marqueeMaximumColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumColorLevel = value
            End Set
        End Property

        '-> BeatFader
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

        '-> GlitterMatrix
        Private _glitterMatrixDelay As Integer = 1
        Private _glitterMatrixBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _glitterMatrixForegroundColor As String = New Color(ConsoleColor.Green).PlainSequence

        ''' <summary>
        ''' [GlitterMatrix] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property GlitterMatrixDelay As Integer
            Get
                Return _glitterMatrixDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1
                _glitterMatrixDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [GlitterMatrix] Screensaver background color
        ''' </summary>
        Public Property GlitterMatrixBackgroundColor As String
            Get
                Return _glitterMatrixBackgroundColor
            End Get
            Set(value As String)
                _glitterMatrixBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [GlitterMatrix] Screensaver foreground color
        ''' </summary>
        Public Property GlitterMatrixForegroundColor As String
            Get
                Return _glitterMatrixForegroundColor
            End Get
            Set(value As String)
                _glitterMatrixForegroundColor = New Color(value).PlainSequence
            End Set
        End Property

        '-> Matrix
        Private _matrixDelay As Integer = 1

        ''' <summary>
        ''' [Matrix] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property MatrixDelay As Integer
            Get
                Return _matrixDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1
                _matrixDelay = value
            End Set
        End Property

        '-> Fader
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

        '-> FaderBack
        Private _faderBackDelay As Integer = 10
        Private _faderBackFadeOutDelay As Integer = 3000
        Private _faderBackMaxSteps As Integer = 25
        Private _faderBackMinimumRedColorLevel As Integer = 0
        Private _faderBackMinimumGreenColorLevel As Integer = 0
        Private _faderBackMinimumBlueColorLevel As Integer = 0
        Private _faderBackMaximumRedColorLevel As Integer = 255
        Private _faderBackMaximumGreenColorLevel As Integer = 255
        Private _faderBackMaximumBlueColorLevel As Integer = 255

        ''' <summary>
        ''' [FaderBack] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FaderBackDelay As Integer
            Get
                Return _faderBackDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _faderBackDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] How many milliseconds to wait before fading the text out?
        ''' </summary>
        Public Property FaderBackFadeOutDelay As Integer
            Get
                Return _faderBackFadeOutDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _faderBackFadeOutDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] How many fade steps to do?
        ''' </summary>
        Public Property FaderBackMaxSteps As Integer
            Get
                Return _faderBackMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _faderBackMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum red color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumRedColorLevel As Integer
            Get
                Return _faderBackMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum green color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumGreenColorLevel As Integer
            Get
                Return _faderBackMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum blue color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumBlueColorLevel As Integer
            Get
                Return _faderBackMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum red color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumRedColorLevel As Integer
            Get
                Return _faderBackMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumRedColorLevel Then value = _faderBackMinimumRedColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum green color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumGreenColorLevel As Integer
            Get
                Return _faderBackMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumGreenColorLevel Then value = _faderBackMinimumGreenColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum blue color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumBlueColorLevel As Integer
            Get
                Return _faderBackMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumBlueColorLevel Then value = _faderBackMinimumBlueColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumBlueColorLevel = value
            End Set
        End Property

        '-> Typo
        Private _typoDelay As Integer = 50
        Private _typoWriteAgainDelay As Integer = 3000
        Private _typoWrite As String = "Kernel Simulator"
        Private _typoWritingSpeedMin As Integer = 50
        Private _typoWritingSpeedMax As Integer = 80
        Private _typoMissStrikePossibility As Integer = 20
        Private _typoMissPossibility As Integer = 10
        Private _typoTextColor As String = New Color(ConsoleColor.White).PlainSequence

        ''' <summary>
        ''' [Typo] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property TypoDelay As Integer
            Get
                Return _typoDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _typoDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] How many milliseconds to wait before writing the text again?
        ''' </summary>
        Public Property TypoWriteAgainDelay As Integer
            Get
                Return _typoWriteAgainDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _typoWriteAgainDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Text for Typo. Longer is better.
        ''' </summary>
        Public Property TypoWrite As String
            Get
                Return _typoWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _typoWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Minimum writing speed in WPM
        ''' </summary>
        Public Property TypoWritingSpeedMin As Integer
            Get
                Return _typoWritingSpeedMin
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _typoWritingSpeedMin = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Maximum writing speed in WPM
        ''' </summary>
        Public Property TypoWritingSpeedMax As Integer
            Get
                Return _typoWritingSpeedMax
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 80
                _typoWritingSpeedMax = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Possibility that the writer made a typo in percent
        ''' </summary>
        Public Property TypoMissStrikePossibility As Integer
            Get
                Return _typoMissStrikePossibility
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _typoMissStrikePossibility = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Possibility that the writer missed a character in percent
        ''' </summary>
        Public Property TypoMissPossibility As Integer
            Get
                Return _typoMissPossibility
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _typoMissPossibility = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Text color
        ''' </summary>
        Public Property TypoTextColor As String
            Get
                Return _typoTextColor
            End Get
            Set(value As String)
                _typoTextColor = New Color(value).PlainSequence
            End Set
        End Property

        '-> Linotypo
        Private _linotypoDelay As Integer = 50
        Private _linotypoNewScreenDelay As Integer = 3000
        Private _linotypoWrite As String = "Kernel Simulator"
        Private _linotypoWritingSpeedMin As Integer = 50
        Private _linotypoWritingSpeedMax As Integer = 80
        Private _linotypoMissStrikePossibility As Integer = 1
        Private _linotypoTextColumns As Integer = 3
        Private _linotypoEtaoinThreshold As Integer = 5
        Private _linotypoEtaoinCappingPossibility As Integer = 5
        Private _linotypoEtaoinType As FillType = FillType.EtaoinPattern
        Private _linotypoMissPossibility As Integer = 10
        Private _linotypoTextColor As String = New Color(ConsoleColor.White).PlainSequence

        ''' <summary>
        ''' [Linotypo] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property LinotypoDelay As Integer
            Get
                Return _linotypoDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _linotypoDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] How many milliseconds to wait before writing the text in the new screen again?
        ''' </summary>
        Public Property LinotypoNewScreenDelay As Integer
            Get
                Return _linotypoNewScreenDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _linotypoNewScreenDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Text for Linotypo. Longer is better.
        ''' </summary>
        Public Property LinotypoWrite As String
            Get
                Return _linotypoWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _linotypoWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Minimum writing speed in WPM
        ''' </summary>
        Public Property LinotypoWritingSpeedMin As Integer
            Get
                Return _linotypoWritingSpeedMin
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _linotypoWritingSpeedMin = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Maximum writing speed in WPM
        ''' </summary>
        Public Property LinotypoWritingSpeedMax As Integer
            Get
                Return _linotypoWritingSpeedMax
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 80
                _linotypoWritingSpeedMax = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Possibility that the writer made a typo in percent
        ''' </summary>
        Public Property LinotypoMissStrikePossibility As Integer
            Get
                Return _linotypoMissStrikePossibility
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1
                _linotypoMissStrikePossibility = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] The text columns to be printed.
        ''' </summary>
        Public Property LinotypoTextColumns As Integer
            Get
                Return _linotypoTextColumns
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3
                _linotypoTextColumns = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] How many characters to write before triggering the "line fill"?
        ''' </summary>
        Public Property LinotypoEtaoinThreshold As Integer
            Get
                Return _linotypoEtaoinThreshold
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5
                _linotypoEtaoinThreshold = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Possibility that the Etaoin pattern will be printed in all caps in percent
        ''' </summary>
        Public Property LinotypoEtaoinCappingPossibility As Integer
            Get
                Return _linotypoEtaoinCappingPossibility
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5
                _linotypoEtaoinCappingPossibility = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Line fill pattern type
        ''' </summary>
        Public Property LinotypoEtaoinType As FillType
            Get
                Return _linotypoEtaoinType
            End Get
            Set(value As FillType)
                _linotypoEtaoinType = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Possibility that the writer missed a character in percent
        ''' </summary>
        Public Property LinotypoMissPossibility As Integer
            Get
                Return _linotypoMissPossibility
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _linotypoMissPossibility = value
            End Set
        End Property
        ''' <summary>
        ''' [Linotypo] Text color
        ''' </summary>
        Public Property LinotypoTextColor As String
            Get
                Return _linotypoTextColor
            End Get
            Set(value As String)
                _linotypoTextColor = New Color(value).PlainSequence
            End Set
        End Property

        '-> Typewriter
        Private _typewriterDelay As Integer = 50
        Private _typewriterNewScreenDelay As Integer = 3000
        Private _typewriterWrite As String = "Kernel Simulator"
        Private _typewriterWritingSpeedMin As Integer = 50
        Private _typewriterWritingSpeedMax As Integer = 80
        Private _typewriterShowArrowPos As Boolean = True
        Private _typewriterTextColor As String = New Color(ConsoleColor.White).PlainSequence

        ''' <summary>
        ''' [Typewriter] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property TypewriterDelay As Integer
            Get
                Return _typewriterDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _typewriterDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Typewriter] How many milliseconds to wait before writing the text in the new screen again?
        ''' </summary>
        Public Property TypewriterNewScreenDelay As Integer
            Get
                Return _typewriterNewScreenDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _typewriterNewScreenDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Typewriter] Text for Typewriter. Longer is better.
        ''' </summary>
        Public Property TypewriterWrite As String
            Get
                Return _typewriterWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _typewriterWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Typewriter] Minimum writing speed in WPM
        ''' </summary>
        Public Property TypewriterWritingSpeedMin As Integer
            Get
                Return _typewriterWritingSpeedMin
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _typewriterWritingSpeedMin = value
            End Set
        End Property
        ''' <summary>
        ''' [Typewriter] Maximum writing speed in WPM
        ''' </summary>
        Public Property TypewriterWritingSpeedMax As Integer
            Get
                Return _typewriterWritingSpeedMax
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 80
                _typewriterWritingSpeedMax = value
            End Set
        End Property
        ''' <summary>
        ''' [Typewriter] Shows the typewriter letter column position by showing this key on the bottom of the screen: <code>^</code>
        ''' </summary>
        Public Property TypewriterShowArrowPos As Boolean
            Get
                Return _typewriterShowArrowPos
            End Get
            Set(value As Boolean)
                _typewriterShowArrowPos = value
            End Set
        End Property
        ''' <summary>
        ''' [Typewriter] Text color
        ''' </summary>
        Public Property TypewriterTextColor As String
            Get
                Return _typewriterTextColor
            End Get
            Set(value As String)
                _typewriterTextColor = New Color(value).PlainSequence
            End Set
        End Property

        '-> FlashColor
        Private _flashColor255Colors As Boolean
        Private _flashColorTrueColor As Boolean = True
        Private _flashColorDelay As Integer = 20
        Private _flashColorBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _flashColorMinimumRedColorLevel As Integer = 0
        Private _flashColorMinimumGreenColorLevel As Integer = 0
        Private _flashColorMinimumBlueColorLevel As Integer = 0
        Private _flashColorMinimumColorLevel As Integer = 0
        Private _flashColorMaximumRedColorLevel As Integer = 255
        Private _flashColorMaximumGreenColorLevel As Integer = 255
        Private _flashColorMaximumBlueColorLevel As Integer = 255
        Private _flashColorMaximumColorLevel As Integer = 0

        ''' <summary>
        ''' [FlashColor] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property FlashColor255Colors As Boolean
            Get
                Return _flashColor255Colors
            End Get
            Set(value As Boolean)
                _flashColor255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FlashColorTrueColor As Boolean
            Get
                Return _flashColorTrueColor
            End Get
            Set(value As Boolean)
                _flashColorTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FlashColorDelay As Integer
            Get
                Return _flashColorDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _flashColorDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] Screensaver background color
        ''' </summary>
        Public Property FlashColorBackgroundColor As String
            Get
                Return _flashColorBackgroundColor
            End Get
            Set(value As String)
                _flashColorBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum red color level (true color)
        ''' </summary>
        Public Property FlashColorMinimumRedColorLevel As Integer
            Get
                Return _flashColorMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashColorMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum green color level (true color)
        ''' </summary>
        Public Property FlashColorMinimumGreenColorLevel As Integer
            Get
                Return _flashColorMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashColorMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum blue color level (true color)
        ''' </summary>
        Public Property FlashColorMinimumBlueColorLevel As Integer
            Get
                Return _flashColorMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashColorMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashColorMinimumColorLevel As Integer
            Get
                Return _flashColorMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_flashColor255Colors Or _flashColorTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _flashColorMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum red color level (true color)
        ''' </summary>
        Public Property FlashColorMaximumRedColorLevel As Integer
            Get
                Return _flashColorMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashColorMinimumRedColorLevel Then value = _flashColorMinimumRedColorLevel
                If value > 255 Then value = 255
                _flashColorMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum green color level (true color)
        ''' </summary>
        Public Property FlashColorMaximumGreenColorLevel As Integer
            Get
                Return _flashColorMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashColorMinimumGreenColorLevel Then value = _flashColorMinimumGreenColorLevel
                If value > 255 Then value = 255
                _flashColorMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum blue color level (true color)
        ''' </summary>
        Public Property FlashColorMaximumBlueColorLevel As Integer
            Get
                Return _flashColorMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashColorMinimumBlueColorLevel Then value = _flashColorMinimumBlueColorLevel
                If value > 255 Then value = 255
                _flashColorMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashColor] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashColorMaximumColorLevel As Integer
            Get
                Return _flashColorMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_flashColor255Colors Or _flashColorTrueColor, 255, 15)
                If value <= _flashColorMinimumColorLevel Then value = _flashColorMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _flashColorMaximumColorLevel = value
            End Set
        End Property

        '-> SpotWrite
        Private _spotWriteDelay As Integer = 100
        Private _spotWriteWrite As String = "Kernel Simulator"
        Private _spotWriteNewScreenDelay As Integer = 3000
        Private _spotWriteTextColor As String = New Color(ConsoleColor.White).PlainSequence

        ''' <summary>
        ''' [SpotWrite] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property SpotWriteDelay As Integer
            Get
                Return _spotWriteDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 100
                _spotWriteDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [SpotWrite] Text for SpotWrite. Longer is better.
        ''' </summary>
        Public Property SpotWriteWrite As String
            Get
                Return _spotWriteWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _spotWriteWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [SpotWrite] How many milliseconds to wait before writing the text in the new screen again?
        ''' </summary>
        Public Property SpotWriteNewScreenDelay As Integer
            Get
                Return _spotWriteNewScreenDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _spotWriteNewScreenDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [SpotWrite] Text color
        ''' </summary>
        Public Property SpotWriteTextColor As String
            Get
                Return _spotWriteTextColor
            End Get
            Set(value As String)
                _spotWriteTextColor = New Color(value).PlainSequence
            End Set
        End Property

        '-> Ramp
        Private _ramp255Colors As Boolean
        Private _rampTrueColor As Boolean = True
        Private _rampDelay As Integer = 20
        Private _rampNextRampDelay As Integer = 250
        Private _rampUpperLeftCornerChar As String = "╔"
        Private _rampUpperRightCornerChar As String = "╗"
        Private _rampLowerLeftCornerChar As String = "╚"
        Private _rampLowerRightCornerChar As String = "╝"
        Private _rampUpperFrameChar As String = "═"
        Private _rampLowerFrameChar As String = "═"
        Private _rampLeftFrameChar As String = "║"
        Private _rampRightFrameChar As String = "║"
        Private _rampMinimumRedColorLevelStart As Integer = 0
        Private _rampMinimumGreenColorLevelStart As Integer = 0
        Private _rampMinimumBlueColorLevelStart As Integer = 0
        Private _rampMinimumColorLevelStart As Integer = 0
        Private _rampMaximumRedColorLevelStart As Integer = 255
        Private _rampMaximumGreenColorLevelStart As Integer = 255
        Private _rampMaximumBlueColorLevelStart As Integer = 255
        Private _rampMaximumColorLevelStart As Integer = 255
        Private _rampMinimumRedColorLevelEnd As Integer = 0
        Private _rampMinimumGreenColorLevelEnd As Integer = 0
        Private _rampMinimumBlueColorLevelEnd As Integer = 0
        Private _rampMinimumColorLevelEnd As Integer = 0
        Private _rampMaximumRedColorLevelEnd As Integer = 255
        Private _rampMaximumGreenColorLevelEnd As Integer = 255
        Private _rampMaximumBlueColorLevelEnd As Integer = 255
        Private _rampMaximumColorLevelEnd As Integer = 255
        Private _rampUpperLeftCornerColor As String = 7
        Private _rampUpperRightCornerColor As String = 7
        Private _rampLowerLeftCornerColor As String = 7
        Private _rampLowerRightCornerColor As String = 7
        Private _rampUpperFrameColor As String = 7
        Private _rampLowerFrameColor As String = 7
        Private _rampLeftFrameColor As String = 7
        Private _rampRightFrameColor As String = 7
        Private _rampUseBorderColors As Boolean

        ''' <summary>
        ''' [Ramp] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Ramp255Colors As Boolean
            Get
                Return _ramp255Colors
            End Get
            Set(value As Boolean)
                _ramp255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property RampTrueColor As Boolean
            Get
                Return _rampTrueColor
            End Get
            Set(value As Boolean)
                _rampTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property RampDelay As Integer
            Get
                Return _rampDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _rampDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] How many milliseconds to wait before starting the next ramp?
        ''' </summary>
        Public Property RampNextRampDelay As Integer
            Get
                Return _rampNextRampDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 250
                _rampNextRampDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Upper left corner character 
        ''' </summary>
        Public Property RampUpperLeftCornerChar As String
            Get
                Return _rampUpperLeftCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╔"
                _rampUpperLeftCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Upper right corner character 
        ''' </summary>
        Public Property RampUpperRightCornerChar As String
            Get
                Return _rampUpperRightCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╗"
                _rampUpperRightCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Lower left corner character 
        ''' </summary>
        Public Property RampLowerLeftCornerChar As String
            Get
                Return _rampLowerLeftCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╚"
                _rampLowerLeftCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Lower right corner character 
        ''' </summary>
        Public Property RampLowerRightCornerChar As String
            Get
                Return _rampLowerRightCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╝"
                _rampLowerRightCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Upper frame character 
        ''' </summary>
        Public Property RampUpperFrameChar As String
            Get
                Return _rampUpperFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _rampUpperFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Lower frame character 
        ''' </summary>
        Public Property RampLowerFrameChar As String
            Get
                Return _rampLowerFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _rampLowerFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Left frame character 
        ''' </summary>
        Public Property RampLeftFrameChar As String
            Get
                Return _rampLeftFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _rampLeftFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Right frame character 
        ''' </summary>
        Public Property RampRightFrameChar As String
            Get
                Return _rampRightFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _rampRightFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum red color level (true color - start)
        ''' </summary>
        Public Property RampMinimumRedColorLevelStart As Integer
            Get
                Return _rampMinimumRedColorLevelStart
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _rampMinimumRedColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum green color level (true color - start)
        ''' </summary>
        Public Property RampMinimumGreenColorLevelStart As Integer
            Get
                Return _rampMinimumGreenColorLevelStart
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _rampMinimumGreenColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum blue color level (true color - start)
        ''' </summary>
        Public Property RampMinimumBlueColorLevelStart As Integer
            Get
                Return _rampMinimumBlueColorLevelStart
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _rampMinimumBlueColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum color level (255 colors or 16 colors - start)
        ''' </summary>
        Public Property RampMinimumColorLevelStart As Integer
            Get
                Return _rampMinimumColorLevelStart
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_ramp255Colors Or _rampTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _rampMinimumColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum red color level (true color - start)
        ''' </summary>
        Public Property RampMaximumRedColorLevelStart As Integer
            Get
                Return _rampMaximumRedColorLevelStart
            End Get
            Set(value As Integer)
                If value <= _rampMinimumRedColorLevelStart Then value = _rampMinimumRedColorLevelStart
                If value > 255 Then value = 255
                _rampMaximumRedColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum green color level (true color - start)
        ''' </summary>
        Public Property RampMaximumGreenColorLevelStart As Integer
            Get
                Return _rampMaximumGreenColorLevelStart
            End Get
            Set(value As Integer)
                If value <= _rampMinimumGreenColorLevelStart Then value = _rampMinimumGreenColorLevelStart
                If value > 255 Then value = 255
                _rampMaximumGreenColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum blue color level (true color - start)
        ''' </summary>
        Public Property RampMaximumBlueColorLevelStart As Integer
            Get
                Return _rampMaximumBlueColorLevelStart
            End Get
            Set(value As Integer)
                If value <= _rampMinimumBlueColorLevelStart Then value = _rampMinimumBlueColorLevelStart
                If value > 255 Then value = 255
                _rampMaximumBlueColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum color level (255 colors or 16 colors - start)
        ''' </summary>
        Public Property RampMaximumColorLevelStart As Integer
            Get
                Return _rampMaximumColorLevelStart
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_ramp255Colors Or _rampTrueColor, 255, 15)
                If value <= _rampMinimumColorLevelStart Then value = _rampMinimumColorLevelStart
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _rampMaximumColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum red color level (true color - end)
        ''' </summary>
        Public Property RampMinimumRedColorLevelEnd As Integer
            Get
                Return _rampMinimumRedColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _rampMinimumRedColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum green color level (true color - end)
        ''' </summary>
        Public Property RampMinimumGreenColorLevelEnd As Integer
            Get
                Return _rampMinimumGreenColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _rampMinimumGreenColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum blue color level (true color - end)
        ''' </summary>
        Public Property RampMinimumBlueColorLevelEnd As Integer
            Get
                Return _rampMinimumBlueColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _rampMinimumBlueColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The minimum color level (255 colors or 16 colors - end)
        ''' </summary>
        Public Property RampMinimumColorLevelEnd As Integer
            Get
                Return _rampMinimumColorLevelEnd
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_ramp255Colors Or _rampTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _rampMinimumColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum red color level (true color - end)
        ''' </summary>
        Public Property RampMaximumRedColorLevelEnd As Integer
            Get
                Return _rampMaximumRedColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= _rampMinimumRedColorLevelEnd Then value = _rampMinimumRedColorLevelEnd
                If value > 255 Then value = 255
                _rampMaximumRedColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum green color level (true color - end)
        ''' </summary>
        Public Property RampMaximumGreenColorLevelEnd As Integer
            Get
                Return _rampMaximumGreenColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= _rampMinimumGreenColorLevelEnd Then value = _rampMinimumGreenColorLevelEnd
                If value > 255 Then value = 255
                _rampMaximumGreenColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum blue color level (true color - end)
        ''' </summary>
        Public Property RampMaximumBlueColorLevelEnd As Integer
            Get
                Return _rampMaximumBlueColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= _rampMinimumBlueColorLevelEnd Then value = _rampMinimumBlueColorLevelEnd
                If value > 255 Then value = 255
                _rampMaximumBlueColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] The maximum color level (255 colors or 16 colors - end)
        ''' </summary>
        Public Property RampMaximumColorLevelEnd As Integer
            Get
                Return _rampMaximumColorLevelEnd
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_ramp255Colors Or _rampTrueColor, 255, 15)
                If value <= _rampMinimumColorLevelEnd Then value = _rampMinimumColorLevelEnd
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _rampMaximumColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Upper left corner color.
        ''' </summary>
        Public Property RampUpperLeftCornerColor As String
            Get
                Return _rampUpperLeftCornerColor
            End Get
            Set(value As String)
                _rampUpperLeftCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Upper right corner color.
        ''' </summary>
        Public Property RampUpperRightCornerColor As String
            Get
                Return _rampUpperRightCornerColor
            End Get
            Set(value As String)
                _rampUpperRightCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Lower left corner color.
        ''' </summary>
        Public Property RampLowerLeftCornerColor As String
            Get
                Return _rampLowerLeftCornerColor
            End Get
            Set(value As String)
                _rampLowerLeftCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Lower right corner color.
        ''' </summary>
        Public Property RampLowerRightCornerColor As String
            Get
                Return _rampLowerRightCornerColor
            End Get
            Set(value As String)
                _rampLowerRightCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Upper frame color.
        ''' </summary>
        Public Property RampUpperFrameColor As String
            Get
                Return _rampUpperFrameColor
            End Get
            Set(value As String)
                _rampUpperFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Lower frame color.
        ''' </summary>
        Public Property RampLowerFrameColor As String
            Get
                Return _rampLowerFrameColor
            End Get
            Set(value As String)
                _rampLowerFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Left frame color.
        ''' </summary>
        Public Property RampLeftFrameColor As String
            Get
                Return _rampLeftFrameColor
            End Get
            Set(value As String)
                _rampLeftFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Right frame color.
        ''' </summary>
        Public Property RampRightFrameColor As String
            Get
                Return _rampRightFrameColor
            End Get
            Set(value As String)
                _rampRightFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Ramp] Use the border colors.
        ''' </summary>
        Public Property RampUseBorderColors As Boolean
            Get
                Return _rampUseBorderColors
            End Get
            Set(value As Boolean)
                _rampUseBorderColors = value
            End Set
        End Property

        '-> StackBox
        Private _stackBox255Colors As Boolean
        Private _stackBoxTrueColor As Boolean = True
        Private _stackBoxDelay As Integer = 10
        Private _stackBoxFill As Boolean = True
        Private _stackBoxMinimumRedColorLevel As Integer = 0
        Private _stackBoxMinimumGreenColorLevel As Integer = 0
        Private _stackBoxMinimumBlueColorLevel As Integer = 0
        Private _stackBoxMinimumColorLevel As Integer = 0
        Private _stackBoxMaximumRedColorLevel As Integer = 255
        Private _stackBoxMaximumGreenColorLevel As Integer = 255
        Private _stackBoxMaximumBlueColorLevel As Integer = 255
        Private _stackBoxMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [StackBox] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property StackBox255Colors As Boolean
            Get
                Return _stackBox255Colors
            End Get
            Set(value As Boolean)
                _stackBox255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property StackBoxTrueColor As Boolean
            Get
                Return _stackBoxTrueColor
            End Get
            Set(value As Boolean)
                _stackBoxTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property StackBoxDelay As Integer
            Get
                Return _stackBoxDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _stackBoxDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] Whether to fill in the boxes drawn, or only draw the outline
        ''' </summary>
        Public Property StackBoxFill As Boolean
            Get
                Return _stackBoxFill
            End Get
            Set(value As Boolean)
                _stackBoxFill = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The minimum red color level (true color)
        ''' </summary>
        Public Property StackBoxMinimumRedColorLevel As Integer
            Get
                Return _stackBoxMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _stackBoxMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The minimum green color level (true color)
        ''' </summary>
        Public Property StackBoxMinimumGreenColorLevel As Integer
            Get
                Return _stackBoxMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _stackBoxMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The minimum blue color level (true color)
        ''' </summary>
        Public Property StackBoxMinimumBlueColorLevel As Integer
            Get
                Return _stackBoxMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _stackBoxMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property StackBoxMinimumColorLevel As Integer
            Get
                Return _stackBoxMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_stackBox255Colors Or _stackBoxTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _stackBoxMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The maximum red color level (true color)
        ''' </summary>
        Public Property StackBoxMaximumRedColorLevel As Integer
            Get
                Return _stackBoxMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _stackBoxMinimumRedColorLevel Then value = _stackBoxMinimumRedColorLevel
                If value > 255 Then value = 255
                _stackBoxMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The maximum green color level (true color)
        ''' </summary>
        Public Property StackBoxMaximumGreenColorLevel As Integer
            Get
                Return _stackBoxMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _stackBoxMinimumGreenColorLevel Then value = _stackBoxMinimumGreenColorLevel
                If value > 255 Then value = 255
                _stackBoxMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The maximum blue color level (true color)
        ''' </summary>
        Public Property StackBoxMaximumBlueColorLevel As Integer
            Get
                Return _stackBoxMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _stackBoxMinimumBlueColorLevel Then value = _stackBoxMinimumBlueColorLevel
                If value > 255 Then value = 255
                _stackBoxMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [StackBox] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property StackBoxMaximumColorLevel As Integer
            Get
                Return _stackBoxMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_stackBox255Colors Or _stackBoxTrueColor, 255, 15)
                If value <= _stackBoxMinimumColorLevel Then value = _stackBoxMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _stackBoxMaximumColorLevel = value
            End Set
        End Property

        '-> Snaker
        Private _snaker255Colors As Boolean
        Private _snakerTrueColor As Boolean = True
        Private _snakerDelay As Integer = 100
        Private _snakerStageDelay As Integer = 5000
        Private _snakerMinimumRedColorLevel As Integer = 0
        Private _snakerMinimumGreenColorLevel As Integer = 0
        Private _snakerMinimumBlueColorLevel As Integer = 0
        Private _snakerMinimumColorLevel As Integer = 0
        Private _snakerMaximumRedColorLevel As Integer = 255
        Private _snakerMaximumGreenColorLevel As Integer = 255
        Private _snakerMaximumBlueColorLevel As Integer = 255
        Private _snakerMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Snaker] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Snaker255Colors As Boolean
            Get
                Return _snaker255Colors
            End Get
            Set(value As Boolean)
                _snaker255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property SnakerTrueColor As Boolean
            Get
                Return _snakerTrueColor
            End Get
            Set(value As Boolean)
                _snakerTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property SnakerDelay As Integer
            Get
                Return _snakerDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 100
                _snakerDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] How many milliseconds to wait before making the next stage?
        ''' </summary>
        Public Property SnakerStageDelay As Integer
            Get
                Return _snakerStageDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5000
                _snakerStageDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum red color level (true color)
        ''' </summary>
        Public Property SnakerMinimumRedColorLevel As Integer
            Get
                Return _snakerMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _snakerMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum green color level (true color)
        ''' </summary>
        Public Property SnakerMinimumGreenColorLevel As Integer
            Get
                Return _snakerMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _snakerMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum blue color level (true color)
        ''' </summary>
        Public Property SnakerMinimumBlueColorLevel As Integer
            Get
                Return _snakerMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _snakerMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property SnakerMinimumColorLevel As Integer
            Get
                Return _snakerMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_snaker255Colors Or _snakerTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _snakerMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum red color level (true color)
        ''' </summary>
        Public Property SnakerMaximumRedColorLevel As Integer
            Get
                Return _snakerMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _snakerMinimumRedColorLevel Then value = _snakerMinimumRedColorLevel
                If value > 255 Then value = 255
                _snakerMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum green color level (true color)
        ''' </summary>
        Public Property SnakerMaximumGreenColorLevel As Integer
            Get
                Return _snakerMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _snakerMinimumGreenColorLevel Then value = _snakerMinimumGreenColorLevel
                If value > 255 Then value = 255
                _snakerMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum blue color level (true color)
        ''' </summary>
        Public Property SnakerMaximumBlueColorLevel As Integer
            Get
                Return _snakerMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _snakerMinimumBlueColorLevel Then value = _snakerMinimumBlueColorLevel
                If value > 255 Then value = 255
                _snakerMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property SnakerMaximumColorLevel As Integer
            Get
                Return _snakerMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_snaker255Colors Or _snakerTrueColor, 255, 15)
                If value <= _snakerMinimumColorLevel Then value = _snakerMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _snakerMaximumColorLevel = value
            End Set
        End Property

        '-> BarRot
        Private _barRot255Colors As Boolean
        Private _barRotTrueColor As Boolean = True
        Private _barRotDelay As Integer = 10
        Private _barRotNextRampDelay As Integer = 250
        Private _barRotUpperLeftCornerChar As String = "╔"
        Private _barRotUpperRightCornerChar As String = "╗"
        Private _barRotLowerLeftCornerChar As String = "╚"
        Private _barRotLowerRightCornerChar As String = "╝"
        Private _barRotUpperFrameChar As String = "═"
        Private _barRotLowerFrameChar As String = "═"
        Private _barRotLeftFrameChar As String = "║"
        Private _barRotRightFrameChar As String = "║"
        Private _barRotMinimumRedColorLevelStart As Integer = 0
        Private _barRotMinimumGreenColorLevelStart As Integer = 0
        Private _barRotMinimumBlueColorLevelStart As Integer = 0
        Private _barRotMaximumRedColorLevelStart As Integer = 255
        Private _barRotMaximumGreenColorLevelStart As Integer = 255
        Private _barRotMaximumBlueColorLevelStart As Integer = 255
        Private _barRotMinimumRedColorLevelEnd As Integer = 0
        Private _barRotMinimumGreenColorLevelEnd As Integer = 0
        Private _barRotMinimumBlueColorLevelEnd As Integer = 0
        Private _barRotMaximumRedColorLevelEnd As Integer = 255
        Private _barRotMaximumGreenColorLevelEnd As Integer = 255
        Private _barRotMaximumBlueColorLevelEnd As Integer = 255
        Private _barRotUpperLeftCornerColor As String = "192;192;192"
        Private _barRotUpperRightCornerColor As String = "192;192;192"
        Private _barRotLowerLeftCornerColor As String = "192;192;192"
        Private _barRotLowerRightCornerColor As String = "192;192;192"
        Private _barRotUpperFrameColor As String = "192;192;192"
        Private _barRotLowerFrameColor As String = "192;192;192"
        Private _barRotLeftFrameColor As String = "192;192;192"
        Private _barRotRightFrameColor As String = "192;192;192"
        Private _barRotUseBorderColors As Boolean

        ''' <summary>
        ''' [BarRot] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property BarRot255Colors As Boolean
            Get
                Return _barRot255Colors
            End Get
            Set(value As Boolean)
                _barRot255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property BarRotTrueColor As Boolean
            Get
                Return _barRotTrueColor
            End Get
            Set(value As Boolean)
                _barRotTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property BarRotDelay As Integer
            Get
                Return _barRotDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _barRotDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] How many milliseconds to wait before rotting the next ramp's one end?
        ''' </summary>
        Public Property BarRotNextRampDelay As Integer
            Get
                Return _barRotNextRampDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 250
                _barRotNextRampDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Upper left corner character 
        ''' </summary>
        Public Property BarRotUpperLeftCornerChar As String
            Get
                Return _barRotUpperLeftCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╔"
                _barRotUpperLeftCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Upper right corner character 
        ''' </summary>
        Public Property BarRotUpperRightCornerChar As String
            Get
                Return _barRotUpperRightCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╗"
                _barRotUpperRightCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Lower left corner character 
        ''' </summary>
        Public Property BarRotLowerLeftCornerChar As String
            Get
                Return _barRotLowerLeftCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╚"
                _barRotLowerLeftCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Lower right corner character 
        ''' </summary>
        Public Property BarRotLowerRightCornerChar As String
            Get
                Return _barRotLowerRightCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╝"
                _barRotLowerRightCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Upper frame character 
        ''' </summary>
        Public Property BarRotUpperFrameChar As String
            Get
                Return _barRotUpperFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _barRotUpperFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Lower frame character 
        ''' </summary>
        Public Property BarRotLowerFrameChar As String
            Get
                Return _barRotLowerFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _barRotLowerFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Left frame character 
        ''' </summary>
        Public Property BarRotLeftFrameChar As String
            Get
                Return _barRotLeftFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _barRotLeftFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Right frame character 
        ''' </summary>
        Public Property BarRotRightFrameChar As String
            Get
                Return _barRotRightFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _barRotRightFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The minimum red color level (true color - start)
        ''' </summary>
        Public Property BarRotMinimumRedColorLevelStart As Integer
            Get
                Return _barRotMinimumRedColorLevelStart
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _barRotMinimumRedColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The minimum green color level (true color - start)
        ''' </summary>
        Public Property BarRotMinimumGreenColorLevelStart As Integer
            Get
                Return _barRotMinimumGreenColorLevelStart
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _barRotMinimumGreenColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The minimum blue color level (true color - start)
        ''' </summary>
        Public Property BarRotMinimumBlueColorLevelStart As Integer
            Get
                Return _barRotMinimumBlueColorLevelStart
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _barRotMinimumBlueColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The maximum red color level (true color - start)
        ''' </summary>
        Public Property BarRotMaximumRedColorLevelStart As Integer
            Get
                Return _barRotMaximumRedColorLevelStart
            End Get
            Set(value As Integer)
                If value <= _barRotMinimumRedColorLevelStart Then value = _barRotMinimumRedColorLevelStart
                If value > 255 Then value = 255
                _barRotMaximumRedColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The maximum green color level (true color - start)
        ''' </summary>
        Public Property BarRotMaximumGreenColorLevelStart As Integer
            Get
                Return _barRotMaximumGreenColorLevelStart
            End Get
            Set(value As Integer)
                If value <= _barRotMinimumGreenColorLevelStart Then value = _barRotMinimumGreenColorLevelStart
                If value > 255 Then value = 255
                _barRotMaximumGreenColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The maximum blue color level (true color - start)
        ''' </summary>
        Public Property BarRotMaximumBlueColorLevelStart As Integer
            Get
                Return _barRotMaximumBlueColorLevelStart
            End Get
            Set(value As Integer)
                If value <= _barRotMinimumBlueColorLevelStart Then value = _barRotMinimumBlueColorLevelStart
                If value > 255 Then value = 255
                _barRotMaximumBlueColorLevelStart = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The minimum red color level (true color - end)
        ''' </summary>
        Public Property BarRotMinimumRedColorLevelEnd As Integer
            Get
                Return _barRotMinimumRedColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _barRotMinimumRedColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The minimum green color level (true color - end)
        ''' </summary>
        Public Property BarRotMinimumGreenColorLevelEnd As Integer
            Get
                Return _barRotMinimumGreenColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _barRotMinimumGreenColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The minimum blue color level (true color - end)
        ''' </summary>
        Public Property BarRotMinimumBlueColorLevelEnd As Integer
            Get
                Return _barRotMinimumBlueColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _barRotMinimumBlueColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The maximum red color level (true color - end)
        ''' </summary>
        Public Property BarRotMaximumRedColorLevelEnd As Integer
            Get
                Return _barRotMaximumRedColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= _barRotMinimumRedColorLevelEnd Then value = _barRotMinimumRedColorLevelEnd
                If value > 255 Then value = 255
                _barRotMaximumRedColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The maximum green color level (true color - end)
        ''' </summary>
        Public Property BarRotMaximumGreenColorLevelEnd As Integer
            Get
                Return _barRotMaximumGreenColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= _barRotMinimumGreenColorLevelEnd Then value = _barRotMinimumGreenColorLevelEnd
                If value > 255 Then value = 255
                _barRotMaximumGreenColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] The maximum blue color level (true color - end)
        ''' </summary>
        Public Property BarRotMaximumBlueColorLevelEnd As Integer
            Get
                Return _barRotMaximumBlueColorLevelEnd
            End Get
            Set(value As Integer)
                If value <= _barRotMinimumBlueColorLevelEnd Then value = _barRotMinimumBlueColorLevelEnd
                If value > 255 Then value = 255
                _barRotMaximumBlueColorLevelEnd = value
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Upper left corner color.
        ''' </summary>
        Public Property BarRotUpperLeftCornerColor As String
            Get
                Return _barRotUpperLeftCornerColor
            End Get
            Set(value As String)
                _barRotUpperLeftCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Upper right corner color.
        ''' </summary>
        Public Property BarRotUpperRightCornerColor As String
            Get
                Return _barRotUpperRightCornerColor
            End Get
            Set(value As String)
                _barRotUpperRightCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Lower left corner color.
        ''' </summary>
        Public Property BarRotLowerLeftCornerColor As String
            Get
                Return _barRotLowerLeftCornerColor
            End Get
            Set(value As String)
                _barRotLowerLeftCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Lower right corner color.
        ''' </summary>
        Public Property BarRotLowerRightCornerColor As String
            Get
                Return _barRotLowerRightCornerColor
            End Get
            Set(value As String)
                _barRotLowerRightCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Upper frame color.
        ''' </summary>
        Public Property BarRotUpperFrameColor As String
            Get
                Return _barRotUpperFrameColor
            End Get
            Set(value As String)
                _barRotUpperFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Lower frame color.
        ''' </summary>
        Public Property BarRotLowerFrameColor As String
            Get
                Return _barRotLowerFrameColor
            End Get
            Set(value As String)
                _barRotLowerFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Left frame color.
        ''' </summary>
        Public Property BarRotLeftFrameColor As String
            Get
                Return _barRotLeftFrameColor
            End Get
            Set(value As String)
                _barRotLeftFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Right frame color.
        ''' </summary>
        Public Property BarRotRightFrameColor As String
            Get
                Return _barRotRightFrameColor
            End Get
            Set(value As String)
                _barRotRightFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BarRot] Use the border colors.
        ''' </summary>
        Public Property BarRotUseBorderColors As Boolean
            Get
                Return _barRotUseBorderColors
            End Get
            Set(value As Boolean)
                _barRotUseBorderColors = value
            End Set
        End Property

        '-> Fireworks
        Private _fireworks255Colors As Boolean
        Private _fireworksTrueColor As Boolean = True
        Private _fireworksDelay As Integer = 10
        Private _fireworksRadius As Integer = 5
        Private _fireworksMinimumRedColorLevel As Integer = 0
        Private _fireworksMinimumGreenColorLevel As Integer = 0
        Private _fireworksMinimumBlueColorLevel As Integer = 0
        Private _fireworksMinimumColorLevel As Integer = 0
        Private _fireworksMaximumRedColorLevel As Integer = 255
        Private _fireworksMaximumGreenColorLevel As Integer = 255
        Private _fireworksMaximumBlueColorLevel As Integer = 255
        Private _fireworksMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Fireworks] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Fireworks255Colors As Boolean
            Get
                Return _fireworks255Colors
            End Get
            Set(value As Boolean)
                _fireworks255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FireworksTrueColor As Boolean
            Get
                Return _fireworksTrueColor
            End Get
            Set(value As Boolean)
                _fireworksTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FireworksDelay As Integer
            Get
                Return _fireworksDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _fireworksDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The radius of the explosion
        ''' </summary>
        Public Property FireworksRadius As Integer
            Get
                Return _fireworksRadius
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5
                _fireworksRadius = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum red color level (true color)
        ''' </summary>
        Public Property FireworksMinimumRedColorLevel As Integer
            Get
                Return _fireworksMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fireworksMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum green color level (true color)
        ''' </summary>
        Public Property FireworksMinimumGreenColorLevel As Integer
            Get
                Return _fireworksMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fireworksMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum blue color level (true color)
        ''' </summary>
        Public Property FireworksMinimumBlueColorLevel As Integer
            Get
                Return _fireworksMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fireworksMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FireworksMinimumColorLevel As Integer
            Get
                Return _fireworksMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_fireworks255Colors Or _fireworksTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _fireworksMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum red color level (true color)
        ''' </summary>
        Public Property FireworksMaximumRedColorLevel As Integer
            Get
                Return _fireworksMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _fireworksMinimumRedColorLevel Then value = _fireworksMinimumRedColorLevel
                If value > 255 Then value = 255
                _fireworksMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum green color level (true color)
        ''' </summary>
        Public Property FireworksMaximumGreenColorLevel As Integer
            Get
                Return _fireworksMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _fireworksMinimumGreenColorLevel Then value = _fireworksMinimumGreenColorLevel
                If value > 255 Then value = 255
                _fireworksMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum blue color level (true color)
        ''' </summary>
        Public Property FireworksMaximumBlueColorLevel As Integer
            Get
                Return _fireworksMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _fireworksMinimumBlueColorLevel Then value = _fireworksMinimumBlueColorLevel
                If value > 255 Then value = 255
                _fireworksMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fireworks] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FireworksMaximumColorLevel As Integer
            Get
                Return _fireworksMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_fireworks255Colors Or _fireworksTrueColor, 255, 15)
                If value <= _fireworksMinimumColorLevel Then value = _fireworksMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _fireworksMaximumColorLevel = value
            End Set
        End Property

        '-> Figlet
        Private _figlet255Colors As Boolean
        Private _figletTrueColor As Boolean = True
        Private _figletDelay As Integer = 1000
        Private _figletText As String = "Kernel Simulator"
        Private _figletFont As String = "Small"
        Private _figletMinimumRedColorLevel As Integer = 0
        Private _figletMinimumGreenColorLevel As Integer = 0
        Private _figletMinimumBlueColorLevel As Integer = 0
        Private _figletMinimumColorLevel As Integer = 0
        Private _figletMaximumRedColorLevel As Integer = 255
        Private _figletMaximumGreenColorLevel As Integer = 255
        Private _figletMaximumBlueColorLevel As Integer = 255
        Private _figletMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Figlet] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Figlet255Colors As Boolean
            Get
                Return _figlet255Colors
            End Get
            Set(value As Boolean)
                _figlet255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FigletTrueColor As Boolean
            Get
                Return _figletTrueColor
            End Get
            Set(value As Boolean)
                _figletTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FigletDelay As Integer
            Get
                Return _figletDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1000
                _figletDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] Text for Figlet. Shorter is better.
        ''' </summary>
        Public Property FigletText As String
            Get
                Return _figletText
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _figletText = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] Figlet font supported by the figlet library used.
        ''' </summary>
        Public Property FigletFont As String
            Get
                Return _figletFont
            End Get
            Set(value As String)
                _figletFont = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum red color level (true color)
        ''' </summary>
        Public Property FigletMinimumRedColorLevel As Integer
            Get
                Return _figletMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _figletMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum green color level (true color)
        ''' </summary>
        Public Property FigletMinimumGreenColorLevel As Integer
            Get
                Return _figletMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _figletMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum blue color level (true color)
        ''' </summary>
        Public Property FigletMinimumBlueColorLevel As Integer
            Get
                Return _figletMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _figletMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FigletMinimumColorLevel As Integer
            Get
                Return _figletMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_figlet255Colors Or _figletTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _figletMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum red color level (true color)
        ''' </summary>
        Public Property FigletMaximumRedColorLevel As Integer
            Get
                Return _figletMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _figletMinimumRedColorLevel Then value = _figletMinimumRedColorLevel
                If value > 255 Then value = 255
                _figletMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum green color level (true color)
        ''' </summary>
        Public Property FigletMaximumGreenColorLevel As Integer
            Get
                Return _figletMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _figletMinimumGreenColorLevel Then value = _figletMinimumGreenColorLevel
                If value > 255 Then value = 255
                _figletMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum blue color level (true color)
        ''' </summary>
        Public Property FigletMaximumBlueColorLevel As Integer
            Get
                Return _figletMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _figletMinimumBlueColorLevel Then value = _figletMinimumBlueColorLevel
                If value > 255 Then value = 255
                _figletMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FigletMaximumColorLevel As Integer
            Get
                Return _figletMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_figlet255Colors Or _figletTrueColor, 255, 15)
                If value <= _figletMinimumColorLevel Then value = _figletMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _figletMaximumColorLevel = value
            End Set
        End Property

        '-> FlashText
        Private _flashText255Colors As Boolean
        Private _flashTextTrueColor As Boolean = True
        Private _flashTextDelay As Integer = 20
        Private _flashTextWrite As String = "Kernel Simulator"
        Private _flashTextBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _flashTextMinimumRedColorLevel As Integer = 0
        Private _flashTextMinimumGreenColorLevel As Integer = 0
        Private _flashTextMinimumBlueColorLevel As Integer = 0
        Private _flashTextMinimumColorLevel As Integer = 0
        Private _flashTextMaximumRedColorLevel As Integer = 255
        Private _flashTextMaximumGreenColorLevel As Integer = 255
        Private _flashTextMaximumBlueColorLevel As Integer = 255
        Private _flashTextMaximumColorLevel As Integer = 0

        ''' <summary>
        ''' [FlashText] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property FlashText255Colors As Boolean
            Get
                Return _flashText255Colors
            End Get
            Set(value As Boolean)
                _flashText255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FlashTextTrueColor As Boolean
            Get
                Return _flashTextTrueColor
            End Get
            Set(value As Boolean)
                _flashTextTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FlashTextDelay As Integer
            Get
                Return _flashTextDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _flashTextDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] Text for FlashText. Shorter is better.
        ''' </summary>
        Public Property FlashTextWrite As String
            Get
                Return _flashTextWrite
            End Get
            Set(value As String)
                _flashTextWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] Screensaver background color
        ''' </summary>
        Public Property FlashTextBackgroundColor As String
            Get
                Return _flashTextBackgroundColor
            End Get
            Set(value As String)
                _flashTextBackgroundColor = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum red color level (true color)
        ''' </summary>
        Public Property FlashTextMinimumRedColorLevel As Integer
            Get
                Return _flashTextMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashTextMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum green color level (true color)
        ''' </summary>
        Public Property FlashTextMinimumGreenColorLevel As Integer
            Get
                Return _flashTextMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashTextMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum blue color level (true color)
        ''' </summary>
        Public Property FlashTextMinimumBlueColorLevel As Integer
            Get
                Return _flashTextMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _flashTextMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashTextMinimumColorLevel As Integer
            Get
                Return _flashTextMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_flashText255Colors Or _flashTextTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _flashTextMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum red color level (true color)
        ''' </summary>
        Public Property FlashTextMaximumRedColorLevel As Integer
            Get
                Return _flashTextMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashTextMinimumRedColorLevel Then value = _flashTextMinimumRedColorLevel
                If value > 255 Then value = 255
                _flashTextMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum green color level (true color)
        ''' </summary>
        Public Property FlashTextMaximumGreenColorLevel As Integer
            Get
                Return _flashTextMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashTextMinimumGreenColorLevel Then value = _flashTextMinimumGreenColorLevel
                If value > 255 Then value = 255
                _flashTextMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum blue color level (true color)
        ''' </summary>
        Public Property FlashTextMaximumBlueColorLevel As Integer
            Get
                Return _flashTextMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _flashTextMinimumBlueColorLevel Then value = _flashTextMinimumBlueColorLevel
                If value > 255 Then value = 255
                _flashTextMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FlashText] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FlashTextMaximumColorLevel As Integer
            Get
                Return _flashTextMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_flashText255Colors Or _flashTextTrueColor, 255, 15)
                If value <= _flashTextMinimumColorLevel Then value = _flashTextMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _flashTextMaximumColorLevel = value
            End Set
        End Property

        '-> Noise
        Private _noiseNewScreenDelay As Integer = 5000
        Private _noiseDensity As Integer = 40

        ''' <summary>
        ''' [Noise] How many milliseconds to wait before making the new screen?
        ''' </summary>
        Public Property NoiseNewScreenDelay As Integer
            Get
                Return _noiseNewScreenDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5000
                _noiseNewScreenDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Noise] The noise density in percent
        ''' </summary>
        Public Property NoiseDensity As Integer
            Get
                Return _noiseDensity
            End Get
            Set(value As Integer)
                If value < 0 Then value = 40
                If value > 100 Then value = 40
                _noiseDensity = value
            End Set
        End Property

        '-> PersonLookup
        Private _personLookupDelay As Integer = 75
        Private _personLookupLookedUpDelay As Integer = 10000
        Private _personLookupMinimumNames As Integer = 10
        Private _personLookupMaximumNames As Integer = 100
        Private _personLookupMinimumAgeYears As Integer = 18
        Private _personLookupMaximumAgeYears As Integer = 100

        ''' <summary>
        ''' [PersonLookup] How many milliseconds to wait before getting the new name?
        ''' </summary>
        Public Property PersonLookupDelay As Integer
            Get
                Return _personLookupDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 75
                _personLookupDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] How many milliseconds to show the looked up name?
        ''' </summary>
        Public Property PersonLookupLookedUpDelay As Integer
            Get
                Return _personLookupLookedUpDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10000
                _personLookupLookedUpDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Minimum names count
        ''' </summary>
        Public Property PersonLookupMinimumNames As Integer
            Get
                Return _personLookupMinimumNames
            End Get
            Set(value As Integer)
                If value <= 10 Then value = 10
                If value > 1000 Then value = 1000
                _personLookupMinimumNames = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Maximum names count
        ''' </summary>
        Public Property PersonLookupMaximumNames As Integer
            Get
                Return _personLookupMaximumNames
            End Get
            Set(value As Integer)
                If value <= _personLookupMinimumNames Then value = _personLookupMinimumNames
                If value > 1000 Then value = 1000
                _personLookupMaximumNames = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Minimum age years
        ''' </summary>
        Public Property PersonLookupMinimumAgeYears As Integer
            Get
                Return _personLookupMinimumAgeYears
            End Get
            Set(value As Integer)
                If value <= 18 Then value = 18
                If value > 100 Then value = 100
                _personLookupMinimumAgeYears = value
            End Set
        End Property
        ''' <summary>
        ''' [PersonLookup] Maximum age years
        ''' </summary>
        Public Property PersonLookupMaximumAgeYears As Integer
            Get
                Return _personLookupMaximumAgeYears
            End Get
            Set(value As Integer)
                If value <= _personLookupMinimumAgeYears Then value = _personLookupMinimumAgeYears
                If value > 100 Then value = 100
                _personLookupMaximumAgeYears = value
            End Set
        End Property

        '-> DateAndTime
        Private _dateAndTime255Colors As Boolean
        Private _dateAndTimeTrueColor As Boolean = True
        Private _dateAndTimeDelay As Integer = 1000
        Private _dateAndTimeMinimumRedColorLevel As Integer = 0
        Private _dateAndTimeMinimumGreenColorLevel As Integer = 0
        Private _dateAndTimeMinimumBlueColorLevel As Integer = 0
        Private _dateAndTimeMinimumColorLevel As Integer = 0
        Private _dateAndTimeMaximumRedColorLevel As Integer = 255
        Private _dateAndTimeMaximumGreenColorLevel As Integer = 255
        Private _dateAndTimeMaximumBlueColorLevel As Integer = 255
        Private _dateAndTimeMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [DateAndTime] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property DateAndTime255Colors As Boolean
            Get
                Return _dateAndTime255Colors
            End Get
            Set(value As Boolean)
                _dateAndTime255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property DateAndTimeTrueColor As Boolean
            Get
                Return _dateAndTimeTrueColor
            End Get
            Set(value As Boolean)
                _dateAndTimeTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property DateAndTimeDelay As Integer
            Get
                Return _dateAndTimeDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1000
                _dateAndTimeDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum red color level (true color)
        ''' </summary>
        Public Property DateAndTimeMinimumRedColorLevel As Integer
            Get
                Return _dateAndTimeMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dateAndTimeMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum green color level (true color)
        ''' </summary>
        Public Property DateAndTimeMinimumGreenColorLevel As Integer
            Get
                Return _dateAndTimeMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dateAndTimeMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum blue color level (true color)
        ''' </summary>
        Public Property DateAndTimeMinimumBlueColorLevel As Integer
            Get
                Return _dateAndTimeMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dateAndTimeMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DateAndTimeMinimumColorLevel As Integer
            Get
                Return _dateAndTimeMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_dateAndTime255Colors Or _dateAndTimeTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _dateAndTimeMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum red color level (true color)
        ''' </summary>
        Public Property DateAndTimeMaximumRedColorLevel As Integer
            Get
                Return _dateAndTimeMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _dateAndTimeMinimumRedColorLevel Then value = _dateAndTimeMinimumRedColorLevel
                If value > 255 Then value = 255
                _dateAndTimeMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum green color level (true color)
        ''' </summary>
        Public Property DateAndTimeMaximumGreenColorLevel As Integer
            Get
                Return _dateAndTimeMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _dateAndTimeMinimumGreenColorLevel Then value = _dateAndTimeMinimumGreenColorLevel
                If value > 255 Then value = 255
                _dateAndTimeMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum blue color level (true color)
        ''' </summary>
        Public Property DateAndTimeMaximumBlueColorLevel As Integer
            Get
                Return _dateAndTimeMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _dateAndTimeMinimumBlueColorLevel Then value = _dateAndTimeMinimumBlueColorLevel
                If value > 255 Then value = 255
                _dateAndTimeMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DateAndTimeMaximumColorLevel As Integer
            Get
                Return _dateAndTimeMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_dateAndTime255Colors Or _dateAndTimeTrueColor, 255, 15)
                If value <= _dateAndTimeMinimumColorLevel Then value = _dateAndTimeMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _dateAndTimeMaximumColorLevel = value
            End Set
        End Property

    End Module
End Namespace