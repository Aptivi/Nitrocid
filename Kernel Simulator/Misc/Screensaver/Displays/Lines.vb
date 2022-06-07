
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
    Public Module LinesDisplay

        Friend Lines As New KernelThread("Lines screensaver thread", True, AddressOf Lines_DoWork)
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

        ''' <summary>
        ''' Handles the code of Lines
        ''' </summary>
        Sub Lines_DoWork()
            Try
                'Variables
                Dim random As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    'Select a color
                    Dim esc As Char = GetEsc()
                    If LinesTrueColor Then
                        SetConsoleColor(New Color(LinesBackgroundColor), True)
                        Console.Clear()
                        Dim RedColorNum As Integer = random.Next(LinesMinimumRedColorLevel, LinesMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = random.Next(LinesMinimumGreenColorLevel, LinesMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = random.Next(LinesMinimumBlueColorLevel, LinesMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        SetConsoleColor(ColorStorage)
                    ElseIf Lines255Colors Then
                        SetConsoleColor(New Color(LinesBackgroundColor), True)
                        Console.Clear()
                        Dim color As Integer = random.Next(LinesMinimumColorLevel, LinesMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", color)
                        SetConsoleColor(New Color(color))
                    Else
                        Console.Clear()
                        SetConsoleColor(New Color(LinesBackgroundColor), True)
                        Console.ForegroundColor = colors(random.Next(LinesMinimumColorLevel, LinesMaximumColorLevel))
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.ForegroundColor)
                    End If

                    'Draw a line
                    Dim Line As String = ""
                    Dim Top As Integer = New Random().Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top)
                    For i As Integer = 1 To Console.WindowWidth
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Forming line using {0} or the default ""-""...", LinesLineChar)
                        Line += If(Not String.IsNullOrWhiteSpace(LinesLineChar), LinesLineChar, "-")
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Line: {0}", Line)
                    Next
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        Console.SetCursorPosition(0, Top)
                        Console.WriteLine(Line)
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(LinesDelay, Lines)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
