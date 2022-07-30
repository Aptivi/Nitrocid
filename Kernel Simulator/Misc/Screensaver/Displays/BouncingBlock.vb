
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
    Public Module BouncingBlockSettings

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

    End Module

    Public Class BouncingBlockDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private Direction As String = "BottomRight"
        Private RowBlock, ColumnBlock As Integer
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "BouncingBlock" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            RowBlock = Console.WindowHeight / 2
            ColumnBlock = Console.WindowWidth / 2
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            SetConsoleColor(New Color(BouncingBlockBackgroundColor), True)
            SetConsoleColor(New Color(BouncingBlockForegroundColor))
            Console.Clear()
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Row block: {0} | Column block: {1}", RowBlock, ColumnBlock)

            'Change the color
            If BouncingBlockTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumRedColorLevel, BouncingBlockMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumGreenColorLevel, BouncingBlockMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumBlueColorLevel, BouncingBlockMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    WriteWhere(" ", ColumnBlock, RowBlock, True, Color.Empty, New Color(RedColorNum, GreenColorNum, BlueColorNum))
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowBlock and ColumnBlock to its original position...")
                    RowBlock = Console.WindowHeight / 2
                    ColumnBlock = Console.WindowWidth / 2
                End If
            ElseIf BouncingBlock255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumColorLevel, BouncingBlockMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    WriteWhere(" ", ColumnBlock, RowBlock, True, Color.Empty, New Color(ColorNum))
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowBlock and ColumnBlock to its original position...")
                    RowBlock = Console.WindowHeight / 2
                    ColumnBlock = Console.WindowWidth / 2
                End If
            Else
                Dim OldColumn As Integer = Console.CursorLeft
                Dim OldRow As Integer = Console.CursorTop
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.BackgroundColor = colors(RandomDriver.Next(BouncingBlockMinimumColorLevel, BouncingBlockMaximumColorLevel))
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    Console.SetCursorPosition(ColumnBlock, RowBlock)
                    Console.Write(" ")
                    Console.SetCursorPosition(OldColumn, OldRow)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Write(" ")
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowBlock and ColumnBlock to its original position...")
                    RowBlock = Console.WindowHeight / 2
                    ColumnBlock = Console.WindowWidth / 2
                End If
            End If

            If RowBlock = Console.WindowHeight - 2 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the bottom.")
                Direction = Direction.Replace("Bottom", "Top")
            ElseIf RowBlock = 1 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the top.")
                Direction = Direction.Replace("Top", "Bottom")
            End If

            If ColumnBlock = Console.WindowWidth - 1 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the right.")
                Direction = Direction.Replace("Right", "Left")
            ElseIf ColumnBlock = 1 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the left.")
                Direction = Direction.Replace("Left", "Right")
            End If

            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Block is facing {0}.", Direction)
            If Direction = "BottomRight" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and column block position")
                RowBlock += 1
                ColumnBlock += 1
            ElseIf Direction = "BottomLeft" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column block position")
                RowBlock += 1
                ColumnBlock -= 1
            ElseIf Direction = "TopRight" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column block position")
                RowBlock -= 1
                ColumnBlock += 1
            ElseIf Direction = "TopLeft" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and column block position")
                RowBlock -= 1
                ColumnBlock -= 1
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(BouncingBlockDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
