
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
Imports Figgle
Imports KS.Misc.Writers.FancyWriters.Tools

Namespace Misc.Screensaver.Displays
    Public Module FigletDisplay

        Friend Figlet As New KernelThread("Figlet screensaver thread", True, AddressOf Figlet_DoWork)
        Private _figlet255Colors As Boolean
        Private _figletTrueColor As Boolean = True
        Private _figletDelay As Integer = 1000
        Private _figletText As String = "Kernel Simulator"
        Private _figletFont As String = "Small"
        Private _figletMinimumRedColorLevel As Integer = 0
        Private _figletMinimumGreenColorLevel As Integer = 0
        Private _figletMinimumBlueColorLevel As Integer = 0
        Private _figletMinimumColorLevel As Integer = 0
        Private _figletMaximumRedColorLevel As Integer = 255
        Private _figletMaximumGreenColorLevel As Integer = 255
        Private _figletMaximumBlueColorLevel As Integer = 255
        Private _figletMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Figlet] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Figlet255Colors As Boolean
            Get
                Return _figlet255Colors
            End Get
            Set(value As Boolean)
                _figlet255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property FigletTrueColor As Boolean
            Get
                Return _figletTrueColor
            End Get
            Set(value As Boolean)
                _figletTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FigletDelay As Integer
            Get
                Return _figletDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1000
                _figletDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] Text for Figlet. Shorter is better.
        ''' </summary>
        Public Property FigletText As String
            Get
                Return _figletText
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _figletText = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] Figlet font supported by the figlet library used.
        ''' </summary>
        Public Property FigletFont As String
            Get
                Return _figletFont
            End Get
            Set(value As String)
                _figletFont = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum red color level (true color)
        ''' </summary>
        Public Property FigletMinimumRedColorLevel As Integer
            Get
                Return _figletMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _figletMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum green color level (true color)
        ''' </summary>
        Public Property FigletMinimumGreenColorLevel As Integer
            Get
                Return _figletMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _figletMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum blue color level (true color)
        ''' </summary>
        Public Property FigletMinimumBlueColorLevel As Integer
            Get
                Return _figletMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _figletMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FigletMinimumColorLevel As Integer
            Get
                Return _figletMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_figlet255Colors Or _figletTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _figletMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum red color level (true color)
        ''' </summary>
        Public Property FigletMaximumRedColorLevel As Integer
            Get
                Return _figletMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _figletMinimumRedColorLevel Then value = _figletMinimumRedColorLevel
                If value > 255 Then value = 255
                _figletMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum green color level (true color)
        ''' </summary>
        Public Property FigletMaximumGreenColorLevel As Integer
            Get
                Return _figletMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _figletMinimumGreenColorLevel Then value = _figletMinimumGreenColorLevel
                If value > 255 Then value = 255
                _figletMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum blue color level (true color)
        ''' </summary>
        Public Property FigletMaximumBlueColorLevel As Integer
            Get
                Return _figletMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _figletMinimumBlueColorLevel Then value = _figletMinimumBlueColorLevel
                If value > 255 Then value = 255
                _figletMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Figlet] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property FigletMaximumColorLevel As Integer
            Get
                Return _figletMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_figlet255Colors Or _figletTrueColor, 255, 15)
                If value <= _figletMinimumColorLevel Then value = _figletMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _figletMaximumColorLevel = value
            End Set
        End Property

        ''' <summary>
        ''' Handles the code of Figlet
        ''' </summary>
        Sub Figlet_DoWork()
            Try
                'Variables
                Dim Randomizer As New Random
                Dim ConsoleMiddleWidth As Integer = Console.WindowWidth / 2
                Dim ConsoleMiddleHeight As Integer = Console.WindowHeight / 2
                Dim FigletFontUsed As FiggleFont = GetFigletFont(FigletFont)
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.White
                Console.Clear()

                'Sanity checks for color levels
                If FigletTrueColor Or Figlet255Colors Then
                    FigletMinimumRedColorLevel = If(FigletMinimumRedColorLevel >= 0 And FigletMinimumRedColorLevel <= 255, FigletMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", FigletMinimumRedColorLevel)
                    FigletMinimumGreenColorLevel = If(FigletMinimumGreenColorLevel >= 0 And FigletMinimumGreenColorLevel <= 255, FigletMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", FigletMinimumGreenColorLevel)
                    FigletMinimumBlueColorLevel = If(FigletMinimumBlueColorLevel >= 0 And FigletMinimumBlueColorLevel <= 255, FigletMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", FigletMinimumBlueColorLevel)
                    FigletMinimumColorLevel = If(FigletMinimumColorLevel >= 0 And FigletMinimumColorLevel <= 255, FigletMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FigletMinimumColorLevel)
                    FigletMaximumRedColorLevel = If(FigletMaximumRedColorLevel >= 0 And FigletMaximumRedColorLevel <= 255, FigletMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", FigletMaximumRedColorLevel)
                    FigletMaximumGreenColorLevel = If(FigletMaximumGreenColorLevel >= 0 And FigletMaximumGreenColorLevel <= 255, FigletMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", FigletMaximumGreenColorLevel)
                    FigletMaximumBlueColorLevel = If(FigletMaximumBlueColorLevel >= 0 And FigletMaximumBlueColorLevel <= 255, FigletMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", FigletMaximumBlueColorLevel)
                    FigletMaximumColorLevel = If(FigletMaximumColorLevel >= 0 And FigletMaximumColorLevel <= 255, FigletMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FigletMaximumColorLevel)
                Else
                    FigletMinimumColorLevel = If(FigletMinimumColorLevel >= 0 And FigletMinimumColorLevel <= 16, FigletMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FigletMinimumColorLevel)
                    FigletMaximumColorLevel = If(FigletMaximumColorLevel >= 0 And FigletMaximumColorLevel <= 16, FigletMaximumColorLevel, 16)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FigletMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    Console.Clear()

                    'Set colors
                    Dim ColorStorage As New Color(255, 255, 255)
                    If FigletTrueColor Then
                        Dim RedColorNum As Integer = Randomizer.Next(FigletMinimumRedColorLevel, FigletMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = Randomizer.Next(FigletMinimumGreenColorLevel, FigletMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = Randomizer.Next(FigletMinimumBlueColorLevel, FigletMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        ColorStorage = New Color(RedColorNum, GreenColorNum, BlueColorNum)
                    ElseIf Figlet255Colors Then
                        Dim ColorNum As Integer = Randomizer.Next(FigletMinimumColorLevel, FigletMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        ColorStorage = New Color(ColorNum)
                    Else
                        Console.BackgroundColor = CType(Randomizer.Next(FigletMinimumColorLevel, FigletMaximumColorLevel), ConsoleColor)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    End If

                    'Prepare the figlet font for writing
                    Dim FigletWrite As String = FigletText.ReplaceAll({vbCr, vbLf}, " - ")
                    FigletWrite = FigletFontUsed.Render(FigletWrite)
                    Dim FigletWriteLines() As String = FigletWrite.SplitNewLines.SkipWhile(Function(x) String.IsNullOrEmpty(x)).ToArray
                    Dim FigletHeight As Integer = ConsoleMiddleHeight - FigletWriteLines.Length / 2
                    Dim FigletWidth As Integer = ConsoleMiddleWidth - FigletWriteLines(0).Length / 2

                    'Actually write it
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        If Figlet255Colors Or FigletTrueColor Then
                            WriteWhere(FigletWrite, FigletWidth, FigletHeight, True, ColorStorage)
                        Else
                            WriteWherePlain(FigletWrite, FigletWidth, FigletHeight, True)
                        End If
                    End If
                    SleepNoBlock(FigletDelay, Figlet)

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
