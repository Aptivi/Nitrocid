
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
    Public Module BouncingTextSettings

        Private _bouncingText255Colors As Boolean
        Private _bouncingTextTrueColor As Boolean = True
        Private _bouncingTextDelay As Integer = 10
        Private _bouncingTextWrite As String = "Kernel Simulator"
        Private _bouncingTextBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _bouncingTextForegroundColor As String = New Color(ConsoleColor.White).PlainSequence
        Private _bouncingTextMinimumRedColorLevel As Integer = 0
        Private _bouncingTextMinimumGreenColorLevel As Integer = 0
        Private _bouncingTextMinimumBlueColorLevel As Integer = 0
        Private _bouncingTextMinimumColorLevel As Integer = 0
        Private _bouncingTextMaximumRedColorLevel As Integer = 255
        Private _bouncingTextMaximumGreenColorLevel As Integer = 255
        Private _bouncingTextMaximumBlueColorLevel As Integer = 255
        Private _bouncingTextMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [BouncingText] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property BouncingText255Colors As Boolean
            Get
                Return _bouncingText255Colors
            End Get
            Set(value As Boolean)
                _bouncingText255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property BouncingTextTrueColor As Boolean
            Get
                Return _bouncingTextTrueColor
            End Get
            Set(value As Boolean)
                _bouncingTextTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property BouncingTextDelay As Integer
            Get
                Return _bouncingTextDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _bouncingTextDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Text for Bouncing Text. Shorter is better.
        ''' </summary>
        Public Property BouncingTextWrite As String
            Get
                Return _bouncingTextWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _bouncingTextWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Screensaver background color
        ''' </summary>
        Public Property BouncingTextBackgroundColor As String
            Get
                Return _bouncingTextBackgroundColor
            End Get
            Set(value As String)
                _bouncingTextBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] Screensaver foreground color
        ''' </summary>
        Public Property BouncingTextForegroundColor As String
            Get
                Return _bouncingTextForegroundColor
            End Get
            Set(value As String)
                _bouncingTextForegroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum red color level (true color)
        ''' </summary>
        Public Property BouncingTextMinimumRedColorLevel As Integer
            Get
                Return _bouncingTextMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingTextMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum green color level (true color)
        ''' </summary>
        Public Property BouncingTextMinimumGreenColorLevel As Integer
            Get
                Return _bouncingTextMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingTextMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum blue color level (true color)
        ''' </summary>
        Public Property BouncingTextMinimumBlueColorLevel As Integer
            Get
                Return _bouncingTextMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _bouncingTextMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BouncingTextMinimumColorLevel As Integer
            Get
                Return _bouncingTextMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_bouncingText255Colors Or _bouncingTextTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _bouncingTextMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum red color level (true color)
        ''' </summary>
        Public Property BouncingTextMaximumRedColorLevel As Integer
            Get
                Return _bouncingTextMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingTextMinimumRedColorLevel Then value = _bouncingTextMinimumRedColorLevel
                If value > 255 Then value = 255
                _bouncingTextMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum green color level (true color)
        ''' </summary>
        Public Property BouncingTextMaximumGreenColorLevel As Integer
            Get
                Return _bouncingTextMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingTextMinimumGreenColorLevel Then value = _bouncingTextMinimumGreenColorLevel
                If value > 255 Then value = 255
                _bouncingTextMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum blue color level (true color)
        ''' </summary>
        Public Property BouncingTextMaximumBlueColorLevel As Integer
            Get
                Return _bouncingTextMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _bouncingTextMinimumBlueColorLevel Then value = _bouncingTextMinimumBlueColorLevel
                If value > 255 Then value = 255
                _bouncingTextMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BouncingText] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BouncingTextMaximumColorLevel As Integer
            Get
                Return _bouncingTextMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_bouncingText255Colors Or _bouncingTextTrueColor, 255, 15)
                If value <= _bouncingTextMinimumColorLevel Then value = _bouncingTextMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _bouncingTextMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class BouncingTextDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private Direction As String = "BottomRight"
        Private RowText, ColumnFirstLetter, ColumnLastLetter As Integer
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private BouncingColor As Color

        Public Overrides Property ScreensaverName As String = "BouncingText" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(BouncingTextBackgroundColor), True, True)
            SetConsoleColor(New Color(BouncingTextForegroundColor))
            Console.Clear()
            RowText = Console.WindowHeight / 2
            ColumnFirstLetter = (Console.WindowWidth / 2) - BouncingTextWrite.Length / 2
            ColumnLastLetter = (Console.WindowWidth / 2) + BouncingTextWrite.Length / 2
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            Console.Clear()

            'Define the color
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Row text: {0}", RowText)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Column first letter of text: {0}", ColumnFirstLetter)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Column last letter of text: {0}", ColumnLastLetter)
            If BouncingColor Is Nothing Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Defining color...")
                BouncingColor = ChangeBouncingTextColor()
            End If
            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
            If Not ResizeSyncing Then
                WriteWhere(BouncingTextWrite, ColumnFirstLetter, RowText, True, BouncingColor)
            Else
                WdbgConditional(ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowText, ColumnFirstLetter, and ColumnLastLetter to its original position...")
                RowText = Console.WindowHeight / 2
                ColumnFirstLetter = (Console.WindowWidth / 2) - BouncingTextWrite.Length / 2
                ColumnLastLetter = (Console.WindowWidth / 2) + BouncingTextWrite.Length / 2
            End If

            'Change the direction of text
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Text is facing {0}.", Direction)
            If Direction = "BottomRight" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and column text position")
                RowText += 1
                ColumnFirstLetter += 1
                ColumnLastLetter += 1
            ElseIf Direction = "BottomLeft" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column text position")
                RowText += 1
                ColumnFirstLetter -= 1
                ColumnLastLetter -= 1
            ElseIf Direction = "TopRight" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column text position")
                RowText -= 1
                ColumnFirstLetter += 1
                ColumnLastLetter += 1
            ElseIf Direction = "TopLeft" Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and column text position")
                RowText -= 1
                ColumnFirstLetter -= 1
                ColumnLastLetter -= 1
            End If

            'Check to see if the text is on the edge
            If RowText = Console.WindowHeight - 2 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the bottom.")
                Direction = Direction.Replace("Bottom", "Top")
                BouncingColor = ChangeBouncingTextColor()
            ElseIf RowText = 1 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the top.")
                Direction = Direction.Replace("Top", "Bottom")
                BouncingColor = ChangeBouncingTextColor()
            End If

            If ColumnLastLetter = Console.WindowWidth - 1 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the right.")
                Direction = Direction.Replace("Right", "Left")
                BouncingColor = ChangeBouncingTextColor()
            ElseIf ColumnFirstLetter = 1 Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the left.")
                Direction = Direction.Replace("Left", "Right")
                BouncingColor = ChangeBouncingTextColor()
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(BouncingTextDelay, ScreensaverDisplayerThread)
        End Sub

        ''' <summary>
        ''' Changes the color of bouncing text
        ''' </summary>
        Function ChangeBouncingTextColor() As Color
            Dim RandomDriver As New Random
            Dim ColorInstance As Color
            If BouncingTextTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(BouncingTextMinimumRedColorLevel, BouncingTextMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(BouncingTextMinimumGreenColorLevel, BouncingTextMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(BouncingTextMinimumBlueColorLevel, BouncingTextMaximumBlueColorLevel)
                ColorInstance = New Color(RedColorNum, GreenColorNum, BlueColorNum)
            ElseIf BouncingText255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(BouncingTextMinimumColorLevel, BouncingTextMaximumColorLevel)
                ColorInstance = New Color(ColorNum)
            Else
                ColorInstance = New Color(colors(RandomDriver.Next(BouncingTextMinimumColorLevel, BouncingTextMaximumColorLevel)))
            End If
            Return ColorInstance
        End Function

    End Class
End Namespace
