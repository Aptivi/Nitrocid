
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
    Public Module LighterSettings

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

    End Module
    Public Class LighterDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private ReadOnly CoveredPositions As New List(Of Tuple(Of Integer, Integer))

        Public Overrides Property ScreensaverName As String = "Lighter" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(LighterBackgroundColor), True)
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False

            'Select a position
            Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
            Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
            Console.SetCursorPosition(Left, Top)
            If Not CoveredPositions.Any(Function(t) t.Item1 = Left And t.Item2 = Top) Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covering position...")
                CoveredPositions.Add(New Tuple(Of Integer, Integer)(Left, Top))
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Position covered. Covered positions: {0}", CoveredPositions.Count)
            End If

            'Select a color and write the space
            Dim esc As Char = GetEsc()
            If LighterTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(LighterMinimumRedColorLevel, LighterMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(LighterMinimumGreenColorLevel, LighterMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(LighterMinimumBlueColorLevel, LighterMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(ColorStorage, True)
                    Console.Write(" ")
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                    CoveredPositions.Clear()
                End If
            ElseIf Lighter255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(LighterMinimumColorLevel, LighterMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color(ColorNum), True)
                    Console.Write(" ")
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                    CoveredPositions.Clear()
                End If
            Else
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.BackgroundColor = colors(RandomDriver.Next(LighterMinimumColorLevel, LighterMaximumColorLevel))
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    Console.Write(" ")
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                    CoveredPositions.Clear()
                End If
            End If

            'Simulate a trail effect
            If CoveredPositions.Count = LighterMaxPositions Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covered positions exceeded max positions of {0}", LighterMaxPositions)
                Dim WipeLeft As Integer = CoveredPositions(0).ToString.Substring(0, CoveredPositions(0).ToString.IndexOf(";"))
                Dim WipeTop As Integer = CoveredPositions(0).ToString.Substring(CoveredPositions(0).ToString.IndexOf(";") + 1)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Wiping in {0}, {1}...", WipeLeft, WipeTop)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.SetCursorPosition(WipeLeft, WipeTop)
                    SetConsoleColor(New Color(LighterBackgroundColor), True)
                    Console.Write(" ")
                    CoveredPositions.RemoveAt(0)
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...")
                    CoveredPositions.Clear()
                End If
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(LighterDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
