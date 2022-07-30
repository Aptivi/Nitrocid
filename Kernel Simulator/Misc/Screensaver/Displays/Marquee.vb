
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
    Public Module MarqueeSettings

        Private _marquee255Colors As Boolean
        Private _marqueeTrueColor As Boolean = True
        Private _marqueeDelay As Integer = 10
        Private _marqueeWrite As String = "Kernel Simulator"
        Private _marqueeAlwaysCentered As Boolean = True
        Private _marqueeUseConsoleAPI As Boolean = False
        Private _marqueeBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _marqueeMinimumRedColorLevel As Integer = 0
        Private _marqueeMinimumGreenColorLevel As Integer = 0
        Private _marqueeMinimumBlueColorLevel As Integer = 0
        Private _marqueeMinimumColorLevel As Integer = 0
        Private _marqueeMaximumRedColorLevel As Integer = 255
        Private _marqueeMaximumGreenColorLevel As Integer = 255
        Private _marqueeMaximumBlueColorLevel As Integer = 255
        Private _marqueeMaximumColorLevel As Integer = 0

        ''' <summary>
        ''' [Marquee] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Marquee255Colors As Boolean
            Get
                Return _marquee255Colors
            End Get
            Set(value As Boolean)
                _marquee255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property MarqueeTrueColor As Boolean
            Get
                Return _marqueeTrueColor
            End Get
            Set(value As Boolean)
                _marqueeTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property MarqueeDelay As Integer
            Get
                Return _marqueeDelay
            End Get
            Set(value As Integer)
                _marqueeDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Text for Marquee. Shorter is better.
        ''' </summary>
        Public Property MarqueeWrite As String
            Get
                Return _marqueeWrite
            End Get
            Set(value As String)
                _marqueeWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Whether the text is always on center.
        ''' </summary>
        Public Property MarqueeAlwaysCentered As Boolean
            Get
                Return _marqueeAlwaysCentered
            End Get
            Set(value As Boolean)
                _marqueeAlwaysCentered = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Whether to use the Console.Clear() API (slow) or use the line-clearing VT sequence (fast).
        ''' </summary>
        Public Property MarqueeUseConsoleAPI As Boolean
            Get
                Return _marqueeUseConsoleAPI
            End Get
            Set(value As Boolean)
                _marqueeUseConsoleAPI = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] Screensaver background color
        ''' </summary>
        Public Property MarqueeBackgroundColor As String
            Get
                Return _marqueeBackgroundColor
            End Get
            Set(value As String)
                _marqueeBackgroundColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum red color level (true color)
        ''' </summary>
        Public Property MarqueeMinimumRedColorLevel As Integer
            Get
                Return _marqueeMinimumRedColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum green color level (true color)
        ''' </summary>
        Public Property MarqueeMinimumGreenColorLevel As Integer
            Get
                Return _marqueeMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum blue color level (true color)
        ''' </summary>
        Public Property MarqueeMinimumBlueColorLevel As Integer
            Get
                Return _marqueeMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property MarqueeMinimumColorLevel As Integer
            Get
                Return _marqueeMinimumColorLevel
            End Get
            Set(value As Integer)
                _marqueeMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum red color level (true color)
        ''' </summary>
        Public Property MarqueeMaximumRedColorLevel As Integer
            Get
                Return _marqueeMaximumRedColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum green color level (true color)
        ''' </summary>
        Public Property MarqueeMaximumGreenColorLevel As Integer
            Get
                Return _marqueeMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum blue color level (true color)
        ''' </summary>
        Public Property MarqueeMaximumBlueColorLevel As Integer
            Get
                Return _marqueeMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Marquee] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property MarqueeMaximumColorLevel As Integer
            Get
                Return _marqueeMaximumColorLevel
            End Get
            Set(value As Integer)
                _marqueeMaximumColorLevel = value
            End Set
        End Property

    End Module
    Public Class MarqueeDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Marquee" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(MarqueeBackgroundColor), True)
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            MarqueeWrite = MarqueeWrite.ReplaceAll({vbCr, vbLf}, " - ")
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            Console.Clear()

            'Ensure that the top position of the written text is always centered if AlwaysCentered is enabled. Else, select a random height.
            Dim TopPrinted As Integer = Console.WindowHeight / 2
            If Not MarqueeAlwaysCentered Then
                TopPrinted = RandomDriver.Next(Console.WindowHeight - 1)
            End If
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Top position: {0}", TopPrinted)

            'Start with the left position as the right position.
            Dim CurrentLeft As Integer = Console.WindowWidth - 1
            Dim CurrentLeftOtherEnd As Integer = Console.WindowWidth - 1
            Dim CurrentCharacterNum As Integer = 0

            'We need to set colors for the text.
            If MarqueeTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(MarqueeMinimumRedColorLevel, MarqueeMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(MarqueeMinimumGreenColorLevel, MarqueeMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(MarqueeMinimumBlueColorLevel, MarqueeMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                SetConsoleColor(New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"))
            ElseIf Marquee255Colors Then
                Dim color As Integer = RandomDriver.Next(MarqueeMinimumColorLevel, MarqueeMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", color)
                SetConsoleColor(New Color(color))
            Else
                Console.ForegroundColor = colors(RandomDriver.Next(MarqueeMinimumColorLevel, MarqueeMaximumColorLevel))
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.ForegroundColor)
            End If

            'If the text is at the right and is longer than the console width, crop it until it's complete.
            Do Until CurrentLeftOtherEnd = 0
                SleepNoBlock(MarqueeDelay, ScreensaverDisplayerThread)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit Do
                If MarqueeUseConsoleAPI Then Console.Clear()
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current left: {0} | Current left on other end: {1}", CurrentLeft, CurrentLeftOtherEnd)

                'Declare variable for written marquee text
                Dim MarqueeWritten As String = MarqueeWrite
                Dim Middle As Boolean = MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft) <> CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Middle of long text: {0}", Middle)

                'If the current left position is not zero (not on the left), take the substring starting from the beginning of the string until the
                'written variable equals the base text variable. However, if we're on the left, take the substring so that the character which was
                'shown previously won't be shown again.
                If Not CurrentLeft = 0 Then
                    MarqueeWritten = MarqueeWritten.Substring(0, CurrentLeftOtherEnd - CurrentLeft)
                ElseIf CurrentLeft = 0 And Middle Then
                    MarqueeWritten = MarqueeWritten.Substring(CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft), CurrentLeftOtherEnd - CurrentLeft)
                Else
                    MarqueeWritten = MarqueeWritten.Substring(MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft))
                End If
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written result: {0}", MarqueeWritten)
                If Not MarqueeUseConsoleAPI Then MarqueeWritten += GetEsc() + "[0K"

                'Set the appropriate cursor position and write the results
                Console.SetCursorPosition(CurrentLeft, TopPrinted)
                Console.Write(MarqueeWritten)
                If Middle Then CurrentCharacterNum += 1

                'If we're not on the left, decrement the current left position
                If Not CurrentLeft = 0 Then
                    CurrentLeft -= 1
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Not on left. Decremented left position {0}", CurrentLeft)
                End If

                'If we're on the left or the entire text is written, decrement the current left other end position
                If Not Middle Then
                    CurrentLeftOtherEnd -= 1
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "On left or entire text written. Decremented left other end position {0}", CurrentLeftOtherEnd)
                End If
            Loop

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(MarqueeDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
