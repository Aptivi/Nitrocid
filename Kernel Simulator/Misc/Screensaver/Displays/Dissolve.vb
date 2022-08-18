
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

Imports System.Threading

Namespace Misc.Screensaver.Displays
    Public Module DissolveSettings

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

    End Module

    Public Class DissolveDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private ColorFilled As Boolean
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private ReadOnly CoveredPositions As New List(Of Tuple(Of Integer, Integer))

        Public Overrides Property ScreensaverName As String = "Dissolve" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            If ColorFilled Then Thread.Sleep(1)
            Dim EndLeft As Integer = Console.WindowWidth - 1
            Dim EndTop As Integer = Console.WindowHeight - 1
            Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
            Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Dissolving: {0}", ColorFilled)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", Left, Top)

            'Fill the color if not filled
            If Not ColorFilled Then
                'NOTICE: Mono seems to have a bug in Console.CursorLeft and Console.CursorTop when printing with VT escape sequences. For info, seek EB#2:7.
                If Not (Console.CursorLeft >= EndLeft And Console.CursorTop >= EndTop) Then
                    If DissolveTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(DissolveMinimumRedColorLevel, DissolveMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(DissolveMinimumGreenColorLevel, DissolveMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(DissolveMinimumBlueColorLevel, DissolveMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(Color.Empty)
                            SetConsoleColor(New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), True, True)
                            Console.Write(" ")
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                            ColorFilled = False
                            SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
                            Console.Clear()
                            CoveredPositions.Clear()
                        End If
                    ElseIf Dissolve255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(DissolveMinimumColorLevel, DissolveMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(Color.Empty)
                            SetConsoleColor(New Color(ColorNum), True, True)
                            Console.Write(" ")
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                            ColorFilled = False
                            SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
                            Console.Clear()
                            CoveredPositions.Clear()
                        End If
                    Else
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            Console.Write(" ")
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                            ColorFilled = False
                            SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
                            Console.Clear()
                            CoveredPositions.Clear()
                        End If
                    End If
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're now dissolving... L: {0} = {1} | T: {2} = {3}", Console.CursorLeft, EndLeft, Console.CursorTop, EndTop)
                    ColorFilled = True
                End If
            Else
                If Not CoveredPositions.Any(Function(t) t.Item1 = Left And t.Item2 = Top) Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covered position {0}", Left & " - " & Top)
                    CoveredPositions.Add(New Tuple(Of Integer, Integer)(Left, Top))
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Covered positions: {0}/{1}", CoveredPositions.Count, (EndLeft + 1) * (EndTop + 1))
                End If
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.SetCursorPosition(Left, Top)
                    SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
                    Console.Write(" ")
                    If CoveredPositions.Count = (EndLeft + 1) * (EndTop + 1) Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                        ColorFilled = False
                        SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
                        Console.Clear()
                        CoveredPositions.Clear()
                    End If
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're refilling...")
                    ColorFilled = False
                    SetConsoleColor(New Color(DissolveBackgroundColor), True, True)
                    Console.Clear()
                    CoveredPositions.Clear()
                End If
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
        End Sub

    End Class
End Namespace
