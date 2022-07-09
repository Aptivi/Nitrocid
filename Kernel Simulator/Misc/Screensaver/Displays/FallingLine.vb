
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
    Public Module FallingLineSettings

        Private _fallingLine255Colors As Boolean
        Private _fallingLineTrueColor As Boolean = True
        Private _fallingLineDelay As Integer = 10
        Private _fallingLineMaxSteps As Integer = 25
        Private _fallingLineMinimumRedColorLevel As Integer = 0
        Private _fallingLineMinimumGreenColorLevel As Integer = 0
        Private _fallingLineMinimumBlueColorLevel As Integer = 0
        Private _fallingLineMinimumColorLevel As Integer = 0
        Private _fallingLineMaximumRedColorLevel As Integer = 255
        Private _fallingLineMaximumGreenColorLevel As Integer = 255
        Private _fallingLineMaximumBlueColorLevel As Integer = 255
        Private _fallingLineMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [FallingLine] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property FallingLine255Colors As Boolean
            Get
                Return _fallingLine255Colors
            End Get
            Set(value As Boolean)
                _fallingLine255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FallingLineTrueColor As Boolean
            Get
                Return _fallingLineTrueColor
            End Get
            Set(value As Boolean)
                _fallingLineTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FallingLineDelay As Integer
            Get
                Return _fallingLineDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _fallingLineDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] How many fade steps to do?
        ''' </summary>
        Public Property FallingLineMaxSteps As Integer
            Get
                Return _fallingLineMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _fallingLineMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The minimum red color level (true color)
        ''' </summary>
        Public Property FallingLineMinimumRedColorLevel As Integer
            Get
                Return _fallingLineMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fallingLineMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The minimum green color level (true color)
        ''' </summary>
        Public Property FallingLineMinimumGreenColorLevel As Integer
            Get
                Return _fallingLineMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fallingLineMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The minimum blue color level (true color)
        ''' </summary>
        Public Property FallingLineMinimumBlueColorLevel As Integer
            Get
                Return _fallingLineMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _fallingLineMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FallingLineMinimumColorLevel As Integer
            Get
                Return _fallingLineMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_fallingLine255Colors Or _fallingLineTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _fallingLineMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The maximum red color level (true color)
        ''' </summary>
        Public Property FallingLineMaximumRedColorLevel As Integer
            Get
                Return _fallingLineMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _fallingLineMinimumRedColorLevel Then value = _fallingLineMinimumRedColorLevel
                If value > 255 Then value = 255
                _fallingLineMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The maximum green color level (true color)
        ''' </summary>
        Public Property FallingLineMaximumGreenColorLevel As Integer
            Get
                Return _fallingLineMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _fallingLineMinimumGreenColorLevel Then value = _fallingLineMinimumGreenColorLevel
                If value > 255 Then value = 255
                _fallingLineMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The maximum blue color level (true color)
        ''' </summary>
        Public Property FallingLineMaximumBlueColorLevel As Integer
            Get
                Return _fallingLineMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _fallingLineMinimumBlueColorLevel Then value = _fallingLineMinimumBlueColorLevel
                If value > 255 Then value = 255
                _fallingLineMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FallingLine] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FallingLineMaximumColorLevel As Integer
            Get
                Return _fallingLineMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_fallingLine255Colors Or _fallingLineTrueColor, 255, 15)
                If value <= _fallingLineMinimumColorLevel Then value = _fallingLineMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _fallingLineMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class FallingLineDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private ColumnLine As Integer
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private ReadOnly CoveredPositions As New List(Of Tuple(Of Integer, Integer))

        Public Overrides Property ScreensaverName As String = "FallingLine" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Console.CursorVisible = False
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            'Choose the column for the falling line
            ColumnLine = RandomDriver.Next(Console.WindowWidth)

            'Now, determine the fall start and end position
            Dim FallStart As Integer = 0
            Dim FallEnd As Integer = Console.WindowHeight - 1

            'Select the color
            Dim ColorStorage As Color
            If FallingLineTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(FallingLineMinimumRedColorLevel, FallingLineMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(FallingLineMinimumGreenColorLevel, FallingLineMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(FallingLineMinimumBlueColorLevel, FallingLineMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                ColorStorage = New Color(RedColorNum, GreenColorNum, BlueColorNum)
                SetConsoleColor(ColorStorage, True)
            ElseIf FallingLine255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(FallingLineMinimumColorLevel, FallingLineMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                ColorStorage = New Color(ColorNum)
                SetConsoleColor(ColorStorage, True)
            Else
                Console.BackgroundColor = colors(RandomDriver.Next(FallingLineMinimumColorLevel, FallingLineMaximumColorLevel))
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                ColorStorage = New Color(Console.BackgroundColor)
            End If

            'Make the line fall down
            For Fall As Integer = FallStart To FallEnd
                'Check to see if user decided to resize
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For

                'Print a block and add the covered position to the list so fading down can be done
                WriteWherePlain(" ", ColumnLine, Fall, False)
                Dim PositionTuple As New Tuple(Of Integer, Integer)(ColumnLine, Fall)
                CoveredPositions.Add(PositionTuple)

                'Delay
                SleepNoBlock(FallingLineDelay, ScreensaverDisplayerThread)
            Next

            'Fade the line down. Please note that this requires true-color support in the terminal to work properly.
            For StepNum As Integer = 0 To FallingLineMaxSteps
                'Check to see if user decided to resize
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For

                'Set thresholds
                Dim ThresholdRed As Double = ColorStorage.R / FallingLineMaxSteps
                Dim ThresholdGreen As Double = ColorStorage.G / FallingLineMaxSteps
                Dim ThresholdBlue As Double = ColorStorage.B / FallingLineMaxSteps
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

                'Set color fade steps
                Dim CurrentColorRedOut As Integer = ColorStorage.R - ThresholdRed * StepNum
                Dim CurrentColorGreenOut As Integer = ColorStorage.G - ThresholdGreen * StepNum
                Dim CurrentColorBlueOut As Integer = ColorStorage.B - ThresholdBlue * StepNum
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)

                'Get the positions and write the block with new color
                Dim CurrentFadeColor As New Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                For Each PositionTuple As Tuple(Of Integer, Integer) In CoveredPositions
                    'Check to see if user decided to resize
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If ResizeSyncing Then Exit For

                    'Actually fade the line out
                    Dim PositionLeft As Integer = PositionTuple.Item1
                    Dim PositionTop As Integer = PositionTuple.Item2
                    WriteWhere(" ", PositionLeft, PositionTop, False, Color.Empty, CurrentFadeColor)
                Next

                'Delay
                SleepNoBlock(FallingLineDelay, ScreensaverDisplayerThread)
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(FallingLineDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
