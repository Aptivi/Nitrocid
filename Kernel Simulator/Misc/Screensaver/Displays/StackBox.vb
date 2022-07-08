
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
Imports Extensification.IntegerExts

Namespace Misc.Screensaver.Displays
    Public Module StackBoxSettings

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

    End Module

    Public Class StackBoxDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "StackBox" Implements IScreensaver.ScreensaverName

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
            Console.CursorVisible = False
            If ResizeSyncing Then
                Console.BackgroundColor = ConsoleColor.Black
                Console.Clear()

                'Reset resize sync
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
            Else
                Dim Drawable As Boolean = True
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                'Get the required positions for the box
                Dim BoxStartX As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim BoxEndX As Integer = RandomDriver.Next(Console.WindowWidth)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Box X position {0} -> {1}", BoxStartX, BoxEndX)
                Dim BoxStartY As Integer = RandomDriver.Next(Console.WindowHeight)
                Dim BoxEndY As Integer = RandomDriver.Next(Console.WindowHeight)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Box Y position {0} -> {1}", BoxStartY, BoxEndY)

                'Check to see if start is less than or equal to end
                BoxStartX.SwapIfSourceLarger(BoxEndX)
                BoxStartY.SwapIfSourceLarger(BoxEndY)
                If BoxStartX = BoxEndX Or BoxStartY = BoxEndY Then
                    'Don't draw; it won't be shown anyways
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Asking StackBox not to draw. Consult above two lines.")
                    Drawable = False
                End If

                If Drawable Then
                    'Select color
                    If StackBoxTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(StackBoxMinimumRedColorLevel, StackBoxMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(StackBoxMinimumGreenColorLevel, StackBoxMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(StackBoxMinimumBlueColorLevel, StackBoxMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        SetConsoleColor(New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), True)
                    ElseIf StackBox255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(StackBoxMinimumColorLevel, StackBoxMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        SetConsoleColor(New Color(ColorNum), True)
                    Else
                        Console.BackgroundColor = colors(RandomDriver.Next(StackBoxMinimumColorLevel, StackBoxMaximumColorLevel))
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    End If

                    'Draw the box
                    If StackBoxFill Then
                        'Cover all the positions
                        For X As Integer = BoxStartX To BoxEndX
                            For Y As Integer = BoxStartY To BoxEndY
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Filling {0},{1}...", X, Y)
                                Console.SetCursorPosition(X, Y)
                                Console.Write(" ")
                            Next
                        Next
                    Else
                        'Draw the upper and lower borders
                        For X As Integer = BoxStartX To BoxEndX
                            Console.SetCursorPosition(X, BoxStartY)
                            Console.Write(" ")
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Drawn upper border at {0}", X)
                            Console.SetCursorPosition(X, BoxEndY)
                            Console.Write(" ")
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Drawn lower border at {0}", X)
                        Next

                        'Draw the left and right borders
                        For Y As Integer = BoxStartY To BoxEndY
                            Console.SetCursorPosition(BoxStartX, Y)
                            Console.Write(" ")
                            If Not BoxStartX >= Console.WindowWidth - 1 Then Console.Write(" ")
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Drawn left border at {0}", Y)
                            Console.SetCursorPosition(BoxEndX, Y)
                            Console.Write(" ")
                            If Not BoxEndX >= Console.WindowWidth - 1 Then Console.Write(" ")
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Drawn right border at {0}", Y)
                        Next
                    End If
                End If
            End If
            SleepNoBlock(StackBoxDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
