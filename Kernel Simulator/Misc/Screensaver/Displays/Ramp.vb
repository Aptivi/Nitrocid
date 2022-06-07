
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
    Public Module RampDisplay

        Friend Ramp As New KernelThread("Ramp screensaver thread", True, AddressOf Ramp_DoWork)
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

        Sub Ramp_DoWork()
            Try

                'Variables
                Dim RandomDriver As New Random()
                Dim RedColorNumFrom As Integer = RandomDriver.Next(RampMinimumRedColorLevelStart, RampMaximumRedColorLevelStart)
                Dim GreenColorNumFrom As Integer = RandomDriver.Next(RampMinimumGreenColorLevelStart, RampMaximumGreenColorLevelStart)
                Dim BlueColorNumFrom As Integer = RandomDriver.Next(RampMinimumBlueColorLevelStart, RampMaximumBlueColorLevelStart)
                Dim ColorNumFrom As Integer = RandomDriver.Next(RampMinimumColorLevelStart, RampMaximumColorLevelStart)
                Dim RedColorNumTo As Integer = RandomDriver.Next(RampMinimumRedColorLevelEnd, RampMaximumRedColorLevelEnd)
                Dim GreenColorNumTo As Integer = RandomDriver.Next(RampMinimumGreenColorLevelEnd, RampMaximumGreenColorLevelEnd)
                Dim BlueColorNumTo As Integer = RandomDriver.Next(RampMinimumBlueColorLevelEnd, RampMaximumBlueColorLevelEnd)
                Dim ColorNumTo As Integer = RandomDriver.Next(RampMinimumColorLevelEnd, RampMaximumColorLevelEnd)
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.White
                Console.Clear()

                'Screensaver logic
                Do While True
                    'Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
                    Console.CursorVisible = False
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                    'Select a color range for the ramp
                    If RampTrueColor Then
                        RedColorNumFrom = RandomDriver.Next(RampMinimumRedColorLevelStart, RampMaximumRedColorLevelStart)
                        GreenColorNumFrom = RandomDriver.Next(RampMinimumGreenColorLevelStart, RampMaximumGreenColorLevelStart)
                        BlueColorNumFrom = RandomDriver.Next(RampMinimumBlueColorLevelStart, RampMaximumBlueColorLevelStart)
                        RedColorNumTo = RandomDriver.Next(RampMinimumRedColorLevelEnd, RampMaximumRedColorLevelEnd)
                        GreenColorNumTo = RandomDriver.Next(RampMinimumGreenColorLevelEnd, RampMaximumGreenColorLevelEnd)
                        BlueColorNumTo = RandomDriver.Next(RampMinimumBlueColorLevelEnd, RampMaximumBlueColorLevelEnd)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo)
                    ElseIf Ramp255Colors Then
                        ColorNumFrom = RandomDriver.Next(RampMinimumColorLevelStart, RampMaximumColorLevelStart)
                        ColorNumTo = RandomDriver.Next(RampMinimumColorLevelEnd, RampMaximumColorLevelEnd)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from ({0}) to ({1})", ColorNumFrom, ColorNumTo)
                    Else
                        ColorNumFrom = RandomDriver.Next(RampMinimumColorLevelStart, RampMaximumColorLevelStart)
                        ColorNumTo = RandomDriver.Next(RampMinimumColorLevelEnd, RampMaximumColorLevelEnd)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    End If

                    'Set start and end widths for the ramp frame
                    Dim RampFrameStartWidth As Integer = 4
                    Dim RampFrameEndWidth As Integer = Console.WindowWidth - RampFrameStartWidth
                    Dim RampFrameSpaces As Integer = RampFrameEndWidth - RampFrameStartWidth
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces)

                    'Set thresholds for color ramps
                    Dim RampColorRedThreshold As Integer = RedColorNumFrom - RedColorNumTo
                    Dim RampColorGreenThreshold As Integer = GreenColorNumFrom - GreenColorNumTo
                    Dim RampColorBlueThreshold As Integer = BlueColorNumFrom - BlueColorNumTo
                    Dim RampColorThreshold As Integer = ColorNumFrom - ColorNumTo
                    Dim RampColorRedSteps As Double = RampColorRedThreshold / RampFrameSpaces
                    Dim RampColorGreenSteps As Double = RampColorGreenThreshold / RampFrameSpaces
                    Dim RampColorBlueSteps As Double = RampColorBlueThreshold / RampFrameSpaces
                    Dim RampColorSteps As Double = RampColorThreshold / RampFrameSpaces
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2} | Normal: {3})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold, RampColorThreshold)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3} | Normal: {4})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps, RampColorSteps)

                    'Let the ramp be printed in the center
                    Dim RampCenterPosition As Integer = Console.WindowHeight / 2
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition)

                    'Set the current positions
                    Dim RampCurrentPositionLeft As Integer = RampFrameStartWidth + 1
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft)

                    'Draw the frame
                    If Not ResizeSyncing Then
                        WriteWhere(RampUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, False, If(RampUseBorderColors, New Color(RampUpperLeftCornerColor), New Color(ConsoleColors.Gray)))
                        Write(RampUpperFrameChar.Repeat(RampFrameSpaces), False, If(RampUseBorderColors, New Color(RampUpperFrameColor), New Color(ConsoleColors.Gray)))
                        Write(RampUpperRightCornerChar, False, If(RampUseBorderColors, New Color(RampUpperRightCornerColor), New Color(ConsoleColors.Gray)))
                        WriteWhere(RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                        WriteWhere(RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                        WriteWhere(RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                        WriteWhere(RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                        WriteWhere(RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                        WriteWhere(RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                        WriteWhere(RampLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, False, If(RampUseBorderColors, New Color(RampLowerLeftCornerColor), New Color(ConsoleColors.Gray)))
                        Write(RampLowerFrameChar.Repeat(RampFrameSpaces), False, If(RampUseBorderColors, New Color(RampLowerFrameColor), New Color(ConsoleColors.Gray)))
                        Write(RampLowerRightCornerChar, False, If(RampUseBorderColors, New Color(RampLowerRightCornerColor), New Color(ConsoleColors.Gray)))
                    End If

                    'Draw the ramp
                    If RampTrueColor Then
                        'Set the current colors
                        Dim RampCurrentColorRed As Double = RedColorNumFrom
                        Dim RampCurrentColorGreen As Double = GreenColorNumFrom
                        Dim RampCurrentColorBlue As Double = BlueColorNumFrom
                        Dim RampCurrentColorInstance As New Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}")

                        'Set the console color and fill the ramp!
                        SetConsoleColor(RampCurrentColorInstance, True)
                        Do Until Convert.ToInt32(RampCurrentColorRed) = RedColorNumTo And Convert.ToInt32(RampCurrentColorGreen) = GreenColorNumTo And Convert.ToInt32(RampCurrentColorBlue) = BlueColorNumTo
                            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                            If ResizeSyncing Then Exit Do
                            Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1)
                            Console.Write(" "c)
                            Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition)
                            Console.Write(" "c)
                            Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1)
                            Console.Write(" "c)
                            RampCurrentPositionLeft = Console.CursorLeft

                            'Change the colors
                            RampCurrentColorRed -= RampColorRedSteps
                            RampCurrentColorGreen -= RampColorGreenSteps
                            RampCurrentColorBlue -= RampColorBlueSteps
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps)
                            RampCurrentColorInstance = New Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}")
                            SetConsoleColor(RampCurrentColorInstance, True)

                            'Delay writing
                            SleepNoBlock(RampDelay, Ramp)
                        Loop
                    Else
                        'Set the current colors
                        Dim RampCurrentColor As Double = ColorNumFrom
                        Dim RampCurrentColorInstance As New Color(Convert.ToInt32(RampCurrentColor))

                        'Set the console color and fill the ramp!
                        SetConsoleColor(RampCurrentColorInstance, True)
                        Do Until Convert.ToInt32(RampCurrentColor) = ColorNumTo
                            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                            If ResizeSyncing Then Exit Do
                            Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1)
                            Console.Write(" "c)
                            Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition)
                            Console.Write(" "c)
                            Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1)
                            Console.Write(" "c)
                            RampCurrentPositionLeft = Console.CursorLeft

                            'Change the colors
                            RampCurrentColor -= RampColorSteps
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got new current colors (Normal: {0}) subtracting from {1}", RampCurrentColor, RampColorSteps)
                            RampCurrentColorInstance = New Color(Convert.ToInt32(RampCurrentColor))
                            SetConsoleColor(RampCurrentColorInstance, True)

                            'Delay writing
                            SleepNoBlock(RampDelay, Ramp)
                        Loop
                    End If
                    SleepNoBlock(RampNextRampDelay, Ramp)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(RampDelay, Ramp)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
