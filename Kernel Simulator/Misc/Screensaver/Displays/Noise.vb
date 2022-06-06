
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
    Public Module NoiseDisplay

        Friend Noise As New KernelThread("Noise screensaver thread", True, AddressOf Noise_DoWork)
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

        ''' <summary>
        ''' Handles the code of Noise
        ''' </summary>
        Sub Noise_DoWork()
            Try
                'Variables
                Dim random As New Random()
                Dim NoiseDense As Double = If(NoiseDensity > 100, 100, NoiseDensity) / 100
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Screensaver logic
                Do While True
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
                            Dim CoverX As Integer = random.Next(Console.WindowWidth)
                            Dim CoverY As Integer = random.Next(Console.WindowHeight)
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
                    SleepNoBlock(NoiseNewScreenDelay, Noise)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
