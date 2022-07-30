
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
    Public Module NoiseSettings

        Private _noiseNewScreenDelay As Integer = 5000
        Private _noiseDensity As Integer = 40

        ''' <summary>
        ''' [Noise] How many milliseconds to wait before making the new screen?
        ''' </summary>
        Public Property NoiseNewScreenDelay As Integer
            Get
                Return _noiseNewScreenDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5000
                _noiseNewScreenDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Noise] The noise density in percent
        ''' </summary>
        Public Property NoiseDensity As Integer
            Get
                Return _noiseDensity
            End Get
            Set(value As Integer)
                If value < 0 Then value = 40
                If value > 100 Then value = 40
                _noiseDensity = value
            End Set
        End Property

    End Module

    Public Class NoiseDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Noise" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Dim NoiseDense As Double = If(NoiseDensity > 100, 100, NoiseDensity) / 100

            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.CursorVisible = False
            Console.Clear()
            Console.BackgroundColor = ConsoleColor.Black

            'Select random positions to generate noise
            Dim AmountOfBlocks As Integer = Console.WindowWidth * Console.WindowHeight
            Dim BlocksToCover As Integer = AmountOfBlocks * NoiseDense
            Dim CoveredBlocks As New ArrayList
            Do Until CoveredBlocks.Count = BlocksToCover Or ResizeSyncing
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Dim CoverX As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim CoverY As Integer = RandomDriver.Next(Console.WindowHeight)
                    Console.SetCursorPosition(CoverX, CoverY)
                    Console.Write(" ")
                    If Not CoveredBlocks.Contains(CStr(CoverX) + ", " + CStr(CoverY)) Then CoveredBlocks.Add(CStr(CoverX) + ", " + CStr(CoverY))
                Else
                    'We're resizing.
                    Exit Do
                End If
            Loop

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(NoiseNewScreenDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
