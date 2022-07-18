
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

Namespace Misc.Screensaver.Displays
    Public Module WipeSettings

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

    End Module
    Public Class WipeDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private ToDirection As WipeDirections = WipeDirections.Right
        Private TimesWiped As Integer = 0

        Public Overrides Property ScreensaverName As String = "Wipe" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(WipeBackgroundColor), True)
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Console.CursorVisible = False
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

            'Select a color
            If WipeTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(WipeMinimumRedColorLevel, WipeMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(WipeMinimumGreenColorLevel, WipeMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(WipeMinimumBlueColorLevel, WipeMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                If Not ResizeSyncing Then SetConsoleColor(New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), True)
            ElseIf Wipe255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(WipeMinimumColorLevel, WipeMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                If Not ResizeSyncing Then SetConsoleColor(New Color(ColorNum), True)
            Else
                If Not ResizeSyncing Then Console.BackgroundColor = colors(RandomDriver.Next(WipeMinimumColorLevel, WipeMaximumColorLevel))
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
            End If

            'Set max height according to platform
            Dim MaxWindowHeight As Integer = Console.WindowHeight
            If IsOnUnix() Then MaxWindowHeight -= 1
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight)

            'Print a space {Column} times until the entire screen is wiped.
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Wipe direction {0}", ToDirection.ToString)
            Select Case ToDirection
                Case WipeDirections.Right
                    For Column As Integer = 0 To Console.WindowWidth
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
                        For Row As Integer = 0 To MaxWindowHeight
                            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                            If ResizeSyncing Then Exit For

                            'Do the actual writing
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row)
                            Console.SetCursorPosition(0, Row)
                            Console.Write(" ".Repeat(Column))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Column)
                        Next
                        SleepNoBlock(WipeDelay, ScreensaverDisplayerThread)
                    Next
                Case WipeDirections.Left
                    For Column As Integer = Console.WindowWidth To 1 Step -1
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
                        For Row As Integer = 0 To MaxWindowHeight
                            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                            If ResizeSyncing Then Exit For

                            'Do the actual writing
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Setting position to {0}", Column - 1, Row)
                            Console.SetCursorPosition(Column - 1, Row)
                            Console.Write(" ".Repeat(Console.WindowWidth - Column + 1))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Console.WindowWidth - Column + 1)
                        Next
                        SleepNoBlock(WipeDelay, ScreensaverDisplayerThread)
                    Next
                Case WipeDirections.Top
                    For Row As Integer = MaxWindowHeight To 0 Step -1
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For

                        'Do the actual writing
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row)
                        Console.SetCursorPosition(0, Row)
                        Console.Write(" ".Repeat(Console.WindowWidth))
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Console.WindowWidth)
                        SleepNoBlock(WipeDelay, ScreensaverDisplayerThread)
                    Next
                Case WipeDirections.Bottom
                    For Row As Integer = 0 To MaxWindowHeight
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For

                        'Do the actual writing
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Console.WindowWidth)
                        Console.Write(" ".Repeat(Console.WindowWidth))
                        SleepNoBlock(WipeDelay, ScreensaverDisplayerThread)
                    Next
                    Console.SetCursorPosition(0, 0)
            End Select

            If Not ResizeSyncing Then
                TimesWiped += 1
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Wiped {0} times out of {1}", TimesWiped, WipeWipesNeededToChangeDirection)

                'Check if the number of times wiped is equal to the number of required times to change wiping direction.
                If TimesWiped = WipeWipesNeededToChangeDirection Then
                    TimesWiped = 0
                    ToDirection = [Enum].Parse(GetType(WipeDirections), RandomDriver.Next(0, 3))
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Changed direction to {0}", ToDirection.ToString)
                End If
            Else
                WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...")
                SetConsoleColor(New Color(WipeBackgroundColor), True)
                Console.Clear()
            End If

            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(WipeDelay, ScreensaverDisplayerThread)
        End Sub

        ''' <summary>
        ''' Wipe directions
        ''' </summary>
        Private Enum WipeDirections
            ''' <summary>
            ''' Wipe from right to left
            ''' </summary>
            Left
            ''' <summary>
            ''' Wipe from left to right
            ''' </summary>
            Right
            ''' <summary>
            ''' Wipe from bottom to top
            ''' </summary>
            Top
            ''' <summary>
            ''' Wipe from top to bottom
            ''' </summary>
            Bottom
        End Enum

    End Class
End Namespace
