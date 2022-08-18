
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
    Public Module IndeterminateSettings

        Private _indeterminate255Colors As Boolean
        Private _indeterminateTrueColor As Boolean = True
        Private _indeterminateDelay As Integer = 20
        Private _indeterminateUpperLeftCornerChar As String = "╔"
        Private _indeterminateUpperRightCornerChar As String = "╗"
        Private _indeterminateLowerLeftCornerChar As String = "╚"
        Private _indeterminateLowerRightCornerChar As String = "╝"
        Private _indeterminateUpperFrameChar As String = "═"
        Private _indeterminateLowerFrameChar As String = "═"
        Private _indeterminateLeftFrameChar As String = "║"
        Private _indeterminateRightFrameChar As String = "║"
        Private _indeterminateMinimumRedColorLevel As Integer = 0
        Private _indeterminateMinimumGreenColorLevel As Integer = 0
        Private _indeterminateMinimumBlueColorLevel As Integer = 0
        Private _indeterminateMinimumColorLevel As Integer = 0
        Private _indeterminateMaximumRedColorLevel As Integer = 255
        Private _indeterminateMaximumGreenColorLevel As Integer = 255
        Private _indeterminateMaximumBlueColorLevel As Integer = 255
        Private _indeterminateMaximumColorLevel As Integer = 255
        Private _indeterminateUpperLeftCornerColor As String = 7
        Private _indeterminateUpperRightCornerColor As String = 7
        Private _indeterminateLowerLeftCornerColor As String = 7
        Private _indeterminateLowerRightCornerColor As String = 7
        Private _indeterminateUpperFrameColor As String = 7
        Private _indeterminateLowerFrameColor As String = 7
        Private _indeterminateLeftFrameColor As String = 7
        Private _indeterminateRightFrameColor As String = 7
        Private _indeterminateUseBorderColors As Boolean

        ''' <summary>
        ''' [Indeterminate] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Indeterminate255Colors As Boolean
            Get
                Return _indeterminate255Colors
            End Get
            Set(value As Boolean)
                _indeterminate255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property IndeterminateTrueColor As Boolean
            Get
                Return _indeterminateTrueColor
            End Get
            Set(value As Boolean)
                _indeterminateTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property IndeterminateDelay As Integer
            Get
                Return _indeterminateDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _indeterminateDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Upper left corner character 
        ''' </summary>
        Public Property IndeterminateUpperLeftCornerChar As String
            Get
                Return _indeterminateUpperLeftCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╔"
                _indeterminateUpperLeftCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Upper right corner character 
        ''' </summary>
        Public Property IndeterminateUpperRightCornerChar As String
            Get
                Return _indeterminateUpperRightCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╗"
                _indeterminateUpperRightCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Lower left corner character 
        ''' </summary>
        Public Property IndeterminateLowerLeftCornerChar As String
            Get
                Return _indeterminateLowerLeftCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╚"
                _indeterminateLowerLeftCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Lower right corner character 
        ''' </summary>
        Public Property IndeterminateLowerRightCornerChar As String
            Get
                Return _indeterminateLowerRightCornerChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "╝"
                _indeterminateLowerRightCornerChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Upper frame character 
        ''' </summary>
        Public Property IndeterminateUpperFrameChar As String
            Get
                Return _indeterminateUpperFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _indeterminateUpperFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Lower frame character 
        ''' </summary>
        Public Property IndeterminateLowerFrameChar As String
            Get
                Return _indeterminateLowerFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "═"
                _indeterminateLowerFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Left frame character 
        ''' </summary>
        Public Property IndeterminateLeftFrameChar As String
            Get
                Return _indeterminateLeftFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _indeterminateLeftFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Right frame character 
        ''' </summary>
        Public Property IndeterminateRightFrameChar As String
            Get
                Return _indeterminateRightFrameChar
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "║"
                _indeterminateRightFrameChar = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The minimum red color level (true color)
        ''' </summary>
        Public Property IndeterminateMinimumRedColorLevel As Integer
            Get
                Return _indeterminateMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _indeterminateMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The minimum green color level (true color)
        ''' </summary>
        Public Property IndeterminateMinimumGreenColorLevel As Integer
            Get
                Return _indeterminateMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _indeterminateMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The minimum blue color level (true color)
        ''' </summary>
        Public Property IndeterminateMinimumBlueColorLevel As Integer
            Get
                Return _indeterminateMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _indeterminateMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property IndeterminateMinimumColorLevel As Integer
            Get
                Return _indeterminateMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_indeterminate255Colors Or _indeterminateTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _indeterminateMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The maximum red color level (true color)
        ''' </summary>
        Public Property IndeterminateMaximumRedColorLevel As Integer
            Get
                Return _indeterminateMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _indeterminateMinimumRedColorLevel Then value = _indeterminateMinimumRedColorLevel
                If value > 255 Then value = 255
                _indeterminateMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The maximum green color level (true color)
        ''' </summary>
        Public Property IndeterminateMaximumGreenColorLevel As Integer
            Get
                Return _indeterminateMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _indeterminateMinimumGreenColorLevel Then value = _indeterminateMinimumGreenColorLevel
                If value > 255 Then value = 255
                _indeterminateMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The maximum blue color level (true color)
        ''' </summary>
        Public Property IndeterminateMaximumBlueColorLevel As Integer
            Get
                Return _indeterminateMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _indeterminateMinimumBlueColorLevel Then value = _indeterminateMinimumBlueColorLevel
                If value > 255 Then value = 255
                _indeterminateMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property IndeterminateMaximumColorLevel As Integer
            Get
                Return _indeterminateMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_indeterminate255Colors Or _indeterminateTrueColor, 255, 15)
                If value <= _indeterminateMinimumColorLevel Then value = _indeterminateMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _indeterminateMaximumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Upper left corner color.
        ''' </summary>
        Public Property IndeterminateUpperLeftCornerColor As String
            Get
                Return _indeterminateUpperLeftCornerColor
            End Get
            Set(value As String)
                _indeterminateUpperLeftCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Upper right corner color.
        ''' </summary>
        Public Property IndeterminateUpperRightCornerColor As String
            Get
                Return _indeterminateUpperRightCornerColor
            End Get
            Set(value As String)
                _indeterminateUpperRightCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Lower left corner color.
        ''' </summary>
        Public Property IndeterminateLowerLeftCornerColor As String
            Get
                Return _indeterminateLowerLeftCornerColor
            End Get
            Set(value As String)
                _indeterminateLowerLeftCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Lower right corner color.
        ''' </summary>
        Public Property IndeterminateLowerRightCornerColor As String
            Get
                Return _indeterminateLowerRightCornerColor
            End Get
            Set(value As String)
                _indeterminateLowerRightCornerColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Upper frame color.
        ''' </summary>
        Public Property IndeterminateUpperFrameColor As String
            Get
                Return _indeterminateUpperFrameColor
            End Get
            Set(value As String)
                _indeterminateUpperFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Lower frame color.
        ''' </summary>
        Public Property IndeterminateLowerFrameColor As String
            Get
                Return _indeterminateLowerFrameColor
            End Get
            Set(value As String)
                _indeterminateLowerFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Left frame color.
        ''' </summary>
        Public Property IndeterminateLeftFrameColor As String
            Get
                Return _indeterminateLeftFrameColor
            End Get
            Set(value As String)
                _indeterminateLeftFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Right frame color.
        ''' </summary>
        Public Property IndeterminateRightFrameColor As String
            Get
                Return _indeterminateRightFrameColor
            End Get
            Set(value As String)
                _indeterminateRightFrameColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Indeterminate] Use the border colors.
        ''' </summary>
        Public Property IndeterminateUseBorderColors As Boolean
            Get
                Return _indeterminateUseBorderColors
            End Get
            Set(value As Boolean)
                _indeterminateUseBorderColors = value
            End Set
        End Property

    End Module
    Public Class IndeterminateDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private ReadOnly RampFrameBlockStartWidth As Integer = 5
        Private ReadOnly RampFrameBlockWidth As Integer = 3
        Private IndeterminateCurrentBlockStart As Integer = RampFrameBlockStartWidth
        Private IndeterminateCurrentBlockEnd As Integer = IndeterminateCurrentBlockStart + RampFrameBlockWidth
        Private IndeterminateCurrentBlockDirection As IndeterminateDirection = IndeterminateDirection.LeftToRight
        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Indeterminate" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Dim RedColorNum As Integer = RandomDriver.Next(IndeterminateMinimumRedColorLevel, IndeterminateMaximumRedColorLevel)
            Dim GreenColorNum As Integer = RandomDriver.Next(IndeterminateMinimumGreenColorLevel, IndeterminateMaximumGreenColorLevel)
            Dim BlueColorNum As Integer = RandomDriver.Next(IndeterminateMinimumBlueColorLevel, IndeterminateMaximumBlueColorLevel)
            Dim ColorNum As Integer = RandomDriver.Next(IndeterminateMinimumColorLevel, IndeterminateMaximumColorLevel)

            'Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
            Console.CursorVisible = False
            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

            'Set start and end widths for the ramp frame
            Dim RampFrameStartWidth As Integer = 4
            Dim RampFrameEndWidth As Integer = Console.WindowWidth - RampFrameStartWidth
            Dim RampFrameSpaces As Integer = RampFrameEndWidth - RampFrameStartWidth
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces)

            'Let the ramp be printed in the center
            Dim RampCenterPosition As Integer = Console.WindowHeight / 2
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Center position: {0}", RampCenterPosition)

            'Draw the frame
            If Not ResizeSyncing Then
                WriteWhere(IndeterminateUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, False, If(IndeterminateUseBorderColors, New Color(IndeterminateUpperLeftCornerColor), New Color(ConsoleColors.Gray)))
                Write(IndeterminateUpperFrameChar.Repeat(RampFrameSpaces), False, If(IndeterminateUseBorderColors, New Color(IndeterminateUpperFrameColor), New Color(ConsoleColors.Gray)))
                Write(IndeterminateUpperRightCornerChar, False, If(IndeterminateUseBorderColors, New Color(IndeterminateUpperRightCornerColor), New Color(ConsoleColors.Gray)))
                WriteWhere(IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(IndeterminateLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(IndeterminateRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLeftFrameColor), New Color(ConsoleColors.Gray)))
                WriteWhere(IndeterminateLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLowerLeftCornerColor), New Color(ConsoleColors.Gray)))
                Write(IndeterminateLowerFrameChar.Repeat(RampFrameSpaces), False, If(IndeterminateUseBorderColors, New Color(IndeterminateLowerFrameColor), New Color(ConsoleColors.Gray)))
                Write(IndeterminateLowerRightCornerChar, False, If(IndeterminateUseBorderColors, New Color(IndeterminateLowerRightCornerColor), New Color(ConsoleColors.Gray)))
            End If

            'Draw the ramp
            Dim RampFrameBlockEndWidth As Integer = RampFrameEndWidth
            Dim RampCurrentColorInstance As Color
            If IndeterminateTrueColor Then
                'Set the current colors
                RampCurrentColorInstance = New Color($"{Convert.ToInt32(RedColorNum)};{Convert.ToInt32(GreenColorNum)};{Convert.ToInt32(BlueColorNum)}")
            Else
                'Set the current colors
                RampCurrentColorInstance = New Color(Convert.ToInt32(ColorNum))
            End If

            'Fill the ramp!
            Do Until (IndeterminateCurrentBlockEnd = RampFrameBlockEndWidth And IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight) Or
                     (IndeterminateCurrentBlockStart = RampFrameBlockStartWidth And IndeterminateCurrentBlockDirection = IndeterminateDirection.RightToLeft)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit Do

                'Clear the ramp
                Console.BackgroundColor = ConsoleColor.Black
                If IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight Then
                    For BlockPos As Integer = RampFrameBlockStartWidth To IndeterminateCurrentBlockStart
                        WriteWherePlain(" ", BlockPos, RampCenterPosition - 1, True)
                        WriteWherePlain(" ", BlockPos, RampCenterPosition, True)
                        WriteWherePlain(" ", BlockPos, RampCenterPosition + 1, True)
                    Next
                Else
                    For BlockPos As Integer = IndeterminateCurrentBlockEnd To RampFrameBlockEndWidth
                        WriteWherePlain(" ", BlockPos, RampCenterPosition - 1, True)
                        WriteWherePlain(" ", BlockPos, RampCenterPosition, True)
                        WriteWherePlain(" ", BlockPos, RampCenterPosition + 1, True)
                    Next
                End If

                'Fill the ramp
                SetConsoleColor(RampCurrentColorInstance, True, True)
                For BlockPos As Integer = IndeterminateCurrentBlockStart To IndeterminateCurrentBlockEnd
                    WriteWherePlain(" ", BlockPos, RampCenterPosition - 1, True)
                    WriteWherePlain(" ", BlockPos, RampCenterPosition, True)
                    WriteWherePlain(" ", BlockPos, RampCenterPosition + 1, True)
                Next

                'Change the start and end positions
                Select Case IndeterminateCurrentBlockDirection
                    Case IndeterminateDirection.LeftToRight
                        IndeterminateCurrentBlockStart += 1
                        IndeterminateCurrentBlockEnd += 1
                    Case IndeterminateDirection.RightToLeft
                        IndeterminateCurrentBlockStart -= 1
                        IndeterminateCurrentBlockEnd -= 1
                End Select

                'Delay writing
                SleepNoBlock(IndeterminateDelay, ScreensaverDisplayerThread)
            Loop

            'Change the direction enumeration
            Select Case IndeterminateCurrentBlockDirection
                Case IndeterminateDirection.LeftToRight
                    IndeterminateCurrentBlockDirection = IndeterminateDirection.RightToLeft
                Case IndeterminateDirection.RightToLeft
                    IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight
            End Select

            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(IndeterminateDelay, ScreensaverDisplayerThread)
        End Sub

        Private Enum IndeterminateDirection
            LeftToRight
            RightToLeft
        End Enum

    End Class
End Namespace
