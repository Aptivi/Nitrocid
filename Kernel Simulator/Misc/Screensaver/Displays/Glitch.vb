
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
    Public Module GlitchSettings

        Private _GlitchDelay As Integer = 10
        Private _GlitchDensity As Integer = 40

        ''' <summary>
        ''' [Glitch] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property GlitchDelay As Integer
            Get
                Return _GlitchDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _GlitchDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Glitch] The Glitch density in percent
        ''' </summary>
        Public Property GlitchDensity As Integer
            Get
                Return _GlitchDensity
            End Get
            Set(value As Integer)
                If value < 0 Then value = 40
                If value > 100 Then value = 40
                _GlitchDensity = value
            End Set
        End Property

    End Module

    Public Class GlitchDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Glitch" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.CursorVisible = False
            Console.Clear()
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            'Select random positions to generate the glitch
            Dim GlitchDense As Double = If(GlitchDensity > 100, 100, GlitchDensity) / 100
            Dim AmountOfBlocks As Integer = Console.WindowWidth * Console.WindowHeight
            Dim BlocksToCover As Integer = AmountOfBlocks * GlitchDense
            Dim CoveredBlocks As New ArrayList
            Do Until CoveredBlocks.Count = BlocksToCover Or ResizeSyncing
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Dim CoverX As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim CoverY As Integer = RandomDriver.Next(Console.WindowHeight)
                    Console.SetCursorPosition(CoverX, CoverY)

                    'Select random glitch type
                    Dim GlitchType As GlitchType = [Enum].Parse(GetType(GlitchType), RandomDriver.Next(5))

                    'Select random letter
                    Dim LetterCapitalized As Boolean = RandomDriver.Next(2)
                    Dim LetterRangeStart As Integer = If(LetterCapitalized, 65, 97)
                    Dim LetterRangeEnd As Integer = If(LetterCapitalized, 90, 122)
                    Dim Letter As Char = Convert.ToChar(RandomDriver.Next(LetterRangeStart, LetterRangeEnd + 1))

                    'Select random symbol
                    Dim UseExtendedAscii As Boolean = RandomDriver.Next(2)
                    Dim SymbolRangeStart As Integer = If(UseExtendedAscii, 128, 33)
                    Dim SymbolRangeEnd As Integer = If(UseExtendedAscii, 256, 64)
                    Dim Symbol As Char = Convert.ToChar(RandomDriver.Next(SymbolRangeStart, SymbolRangeEnd + 1))

                    'Select red, green, or blue background and foreground
                    Dim GlitchBlockColorType As GlitchColorType = [Enum].Parse(GetType(GlitchColorType), RandomDriver.Next(3))
                    Dim GlitchLetterColorType As GlitchColorType = [Enum].Parse(GetType(GlitchColorType), RandomDriver.Next(3))
                    Dim ColorLetter As Boolean = RandomDriver.Next(2)
                    Dim ColorBlockNumber As Integer = RandomDriver.Next(0, 256)
                    Dim ColorLetterNumber As Integer = RandomDriver.Next(0, 256)
                    Dim ColorBlockInstance As Color = Color.Empty
                    Dim ColorLetterInstance As Color = Color.Empty

                    '...    for the block
                    Select Case GlitchBlockColorType
                        Case GlitchColorType.Red
                            ColorBlockInstance = New Color(ColorBlockNumber, 0, 0)
                        Case GlitchColorType.Green
                            ColorBlockInstance = New Color(0, ColorBlockNumber, 0)
                        Case GlitchColorType.Blue
                            ColorBlockInstance = New Color(0, 0, ColorBlockNumber)
                    End Select

                    '...and for the letter
                    Select Case GlitchLetterColorType
                        Case GlitchColorType.Red
                            ColorLetterInstance = New Color(ColorLetterNumber, 0, 0)
                        Case GlitchColorType.Green
                            ColorLetterInstance = New Color(0, ColorLetterNumber, 0)
                        Case GlitchColorType.Blue
                            ColorLetterInstance = New Color(0, 0, ColorLetterNumber)
                    End Select

                    'Now, print based on the glitch type
                    Select Case GlitchType
                        Case GlitchType.RandomLetter
                            If ColorLetter Then SetConsoleColor(ColorLetterInstance) Else Console.ForegroundColor = ConsoleColor.White
                            Console.Write(Letter)
                        Case GlitchType.RandomSymbol
                            If ColorLetter Then SetConsoleColor(ColorLetterInstance) Else Console.ForegroundColor = ConsoleColor.White
                            Console.Write(Symbol)
                        Case GlitchType.RedGreenBlueColor
                            SetConsoleColor(ColorBlockInstance, True)
                            Console.Write(" ")
                        Case GlitchType.RedGreenBlueColorWithRandomLetter
                            If ColorLetter Then SetConsoleColor(ColorLetterInstance) Else Console.ForegroundColor = ConsoleColor.White
                            SetConsoleColor(ColorBlockInstance, True)
                            Console.Write(Letter)
                        Case GlitchType.RedGreenBlueColorWithRandomSymbol
                            If ColorLetter Then SetConsoleColor(ColorLetterInstance) Else Console.ForegroundColor = ConsoleColor.White
                            SetConsoleColor(ColorBlockInstance, True)
                            Console.Write(Symbol)
                    End Select
                    If Not CoveredBlocks.Contains(CStr(CoverX) + ", " + CStr(CoverY)) Then CoveredBlocks.Add(CStr(CoverX) + ", " + CStr(CoverY))
                Else
                    'We're resizing.
                    Console.CursorVisible = False
                    Exit Do
                End If
                SleepNoBlock(GlitchDelay, ScreensaverDisplayerThread)
            Loop

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
        End Sub

    End Class

    Enum GlitchType
        ''' <summary>
        ''' A block with either red, green, or blue as color, and can be darkened
        ''' </summary>
        RedGreenBlueColor
        ''' <summary>
        ''' A block with either red, green, or blue as color, and can be darkened and feature a random letter printed in white
        ''' </summary>
        RedGreenBlueColorWithRandomLetter
        ''' <summary>
        ''' A block with either red, green, or blue as color, and can be darkened and feature a random symbol printed in white
        ''' </summary>
        RedGreenBlueColorWithRandomSymbol
        ''' <summary>
        ''' A random letter printed in white, red, green, or blue, and can be darkened
        ''' </summary>
        RandomLetter
        ''' <summary>
        ''' A random symbol printed in white, red, green, or blue, and can be darkened
        ''' </summary>
        RandomSymbol
    End Enum

    Enum GlitchColorType
        ''' <summary>
        ''' The red color
        ''' </summary>
        Red
        ''' <summary>
        ''' The green color
        ''' </summary>
        Green
        ''' <summary>
        ''' The blue color
        ''' </summary>
        Blue
    End Enum
End Namespace
