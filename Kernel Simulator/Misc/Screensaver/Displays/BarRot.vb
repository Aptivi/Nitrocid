
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
    Public Module BarRotSettings

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

    End Module

    Public Class BarRotDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "BarRot" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

            'Select a color range for the ramp
            Dim RedColorNumFrom As Integer = RandomDriver.Next(BarRotMinimumRedColorLevelStart, BarRotMaximumRedColorLevelStart)
            Dim GreenColorNumFrom As Integer = RandomDriver.Next(BarRotMinimumGreenColorLevelStart, BarRotMaximumGreenColorLevelStart)
            Dim BlueColorNumFrom As Integer = RandomDriver.Next(BarRotMinimumBlueColorLevelStart, BarRotMaximumBlueColorLevelStart)
            Dim RedColorNumTo As Integer = RandomDriver.Next(BarRotMinimumRedColorLevelEnd, BarRotMaximumRedColorLevelEnd)
            Dim GreenColorNumTo As Integer = RandomDriver.Next(BarRotMinimumGreenColorLevelEnd, BarRotMaximumGreenColorLevelEnd)
            Dim BlueColorNumTo As Integer = RandomDriver.Next(BarRotMinimumBlueColorLevelEnd, BarRotMaximumBlueColorLevelEnd)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo)

            'Set start and end widths for the ramp frame
            Dim RampFrameStartWidth As Integer = 4
            Dim RampFrameEndWidth As Integer = Console.WindowWidth - RampFrameStartWidth
            Dim RampFrameSpaces As Integer = RampFrameEndWidth - RampFrameStartWidth
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces)

            'Set thresholds for color ramp
            Dim RampColorRedThreshold As Integer = RedColorNumFrom - RedColorNumTo
            Dim RampColorGreenThreshold As Integer = GreenColorNumFrom - GreenColorNumTo
            Dim RampColorBlueThreshold As Integer = BlueColorNumFrom - BlueColorNumTo
            Dim RampColorRedSteps As Double = RampColorRedThreshold / RampFrameSpaces
            Dim RampColorGreenSteps As Double = RampColorGreenThreshold / RampFrameSpaces
            Dim RampColorBlueSteps As Double = RampColorBlueThreshold / RampFrameSpaces
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Set thresholds (RGB: {0};{1};{2})", RampColorRedThreshold, RampColorGreenThreshold, RampColorBlueThreshold)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces (RGB: {1};{2};{3})", RampFrameSpaces, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps)

            'Let the ramp be printed in the center
            Dim RampCenterPosition As Integer = Console.WindowHeight / 2
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition)

            'Set the current positions
            Dim RampCurrentPositionLeft As Integer = RampFrameStartWidth + 1
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft)

            'Draw the frame
            If Not ResizeSyncing Then
                WriteWhere(BarRotUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, False, If(BarRotUseBorderColors, New Color(BarRotUpperLeftCornerColor), New Color(ConsoleColors.Gray)))
                Write(BarRotUpperFrameChar.Repeat(RampFrameSpaces), False, If(BarRotUseBorderColors, New Color(BarRotUpperFrameColor), New Color(ConsoleColors.Gray)))
                Write(BarRotUpperRightCornerChar, False, If(BarRotUseBorderColors, New Color(BarRotUpperRightCornerColor), New Color(ConsoleColors.Gray)))
                WriteWhere(BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(BarRotLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, False, If(BarRotUseBorderColors, New Color(BarRotLowerLeftCornerColor), New Color(ConsoleColors.Gray)))
                Write(BarRotLowerFrameChar.Repeat(RampFrameSpaces), False, If(BarRotUseBorderColors, New Color(BarRotLowerFrameColor), New Color(ConsoleColors.Gray)))
                Write(BarRotLowerRightCornerChar, False, If(BarRotUseBorderColors, New Color(BarRotLowerRightCornerColor), New Color(ConsoleColors.Gray)))
            End If

            'Set the current colors
            Dim RampCurrentColorRed As Double = RedColorNumFrom
            Dim RampCurrentColorGreen As Double = GreenColorNumFrom
            Dim RampCurrentColorBlue As Double = BlueColorNumFrom

            'Set the console color and fill the ramp!
            Do Until Convert.ToInt32(RampCurrentColorRed) = RedColorNumTo And Convert.ToInt32(RampCurrentColorGreen) = GreenColorNumTo And Convert.ToInt32(RampCurrentColorBlue) = BlueColorNumTo
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit Do

                'Populate the variables for sub-gradients
                Dim RampSubgradientRedColorNumFrom As Integer = RedColorNumFrom
                Dim RampSubgradientGreenColorNumFrom As Integer = GreenColorNumFrom
                Dim RampSubgradientBlueColorNumFrom As Integer = BlueColorNumFrom
                Dim RampSubgradientRedColorNumTo As Integer = RampCurrentColorRed
                Dim RampSubgradientGreenColorNumTo As Integer = RampCurrentColorGreen
                Dim RampSubgradientBlueColorNumTo As Integer = RampCurrentColorBlue
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got subgradient color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RampSubgradientRedColorNumFrom, RampSubgradientGreenColorNumFrom, RampSubgradientBlueColorNumFrom, RampSubgradientRedColorNumTo, RampSubgradientGreenColorNumTo, RampSubgradientBlueColorNumTo)

                'Set the sub-gradient current colors
                Dim RampSubgradientCurrentColorRed As Double = RampSubgradientRedColorNumFrom
                Dim RampSubgradientCurrentColorGreen As Double = RampSubgradientGreenColorNumFrom
                Dim RampSubgradientCurrentColorBlue As Double = RampSubgradientBlueColorNumFrom
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got subgradient current colors (R;G;B: {0};{1};{2})", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue)

                'Set the sub-gradient thresholds
                Dim RampSubgradientColorRedThreshold As Integer = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo
                Dim RampSubgradientColorGreenThreshold As Integer = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo
                Dim RampSubgradientColorBlueThreshold As Integer = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo
                Dim RampSubgradientColorRedSteps As Double = RampSubgradientColorRedThreshold / RampFrameSpaces
                Dim RampSubgradientColorGreenSteps As Double = RampSubgradientColorGreenThreshold / RampFrameSpaces
                Dim RampSubgradientColorBlueSteps As Double = RampSubgradientColorBlueThreshold / RampFrameSpaces
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Set subgradient thresholds (RGB: {0};{1};{2})", RampSubgradientColorRedThreshold, RampSubgradientColorGreenThreshold, RampSubgradientColorBlueThreshold)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Steps by {0} spaces for subgradient (RGB: {1};{2};{3})", RampFrameSpaces, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps)

                'Make a new instance
                Dim RampSubgradientCurrentColorInstance As New Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}")
                SetConsoleColor(RampSubgradientCurrentColorInstance, True)

                'Try to fill the ramp
                Dim RampSubgradientStepsMade As Integer = 0
                Do Until RampSubgradientStepsMade = RampFrameSpaces
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If ResizeSyncing Then Exit Do
                    Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1)
                    Console.Write(" "c)
                    Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition)
                    Console.Write(" "c)
                    Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1)
                    Console.Write(" "c)
                    RampCurrentPositionLeft = Console.CursorLeft
                    RampSubgradientStepsMade += 1

                    'Change the colors
                    RampSubgradientCurrentColorRed -= RampSubgradientColorRedSteps
                    RampSubgradientCurrentColorGreen -= RampSubgradientColorGreenSteps
                    RampSubgradientCurrentColorBlue -= RampSubgradientColorBlueSteps
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got new subgradient current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampSubgradientCurrentColorRed, RampSubgradientCurrentColorGreen, RampSubgradientCurrentColorBlue, RampSubgradientColorRedSteps, RampSubgradientColorGreenSteps, RampSubgradientColorBlueSteps)
                    RampSubgradientCurrentColorInstance = New Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}")
                    SetConsoleColor(RampSubgradientCurrentColorInstance, True)
                Loop

                'Change the colors
                RampCurrentColorRed -= RampColorRedSteps
                RampCurrentColorGreen -= RampColorGreenSteps
                RampCurrentColorBlue -= RampColorBlueSteps
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got new current colors (R;G;B: {0};{1};{2}) subtracting from {3};{4};{5}", RampCurrentColorRed, RampCurrentColorGreen, RampCurrentColorBlue, RampColorRedSteps, RampColorGreenSteps, RampColorBlueSteps)

                'Delay writing
                RampCurrentPositionLeft = RampFrameStartWidth + 1
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current left position: {0}", RampCurrentPositionLeft)
                SleepNoBlock(BarRotDelay, ScreensaverDisplayerThread)
            Loop

            'Clear the scene
            SleepNoBlock(BarRotNextRampDelay, ScreensaverDisplayerThread)
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
        End Sub

    End Class
End Namespace