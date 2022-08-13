
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
    Public Module StarfieldSettings

        Private _starfieldDelay As Integer = 10

        ''' <summary>
        ''' [Starfield] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property StarfieldDelay As Integer
            Get
                Return _starfieldDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _starfieldDelay = value
            End Set
        End Property

    End Module

    Public Class StarfieldDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private ReadOnly Stars As New List(Of Tuple(Of Integer, Integer))

        Public Overrides Property ScreensaverName As String = "Starfield" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False

            'Move the stars left
            For Star As Integer = 0 To Stars.Count - 1
                Dim StarX As Integer = Stars(Star).Item1 - 1
                Dim StarY As Integer = Stars(Star).Item2
                Stars(Star) = New Tuple(Of Integer, Integer)(StarX, StarY)
            Next

            'If any star is out of X range, delete it
            For StarIndex As Integer = Stars.Count - 1 To 0 Step -1
                Dim Star = Stars(StarIndex)
                If Star.Item1 < 0 Then
                    'The star went beyond. Remove it.
                    Stars.RemoveAt(StarIndex)
                End If
            Next

            'Add new star if guaranteed
            Dim StarShowProbability As Double = 10 / 100
            Dim StarShowGuaranteed As Boolean = RandomDriver.NextDouble < StarShowProbability
            If StarShowGuaranteed Then
                Dim StarX As Integer = Console.WindowWidth - 1
                Dim StarY As Integer = RandomDriver.Next(Console.WindowHeight - 1)
                Stars.Add(New Tuple(Of Integer, Integer)(StarX, StarY))
            End If

            'Draw stars
            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
            If Not ResizeSyncing Then
                For StarIndex As Integer = Stars.Count - 1 To 0 Step -1
                    Dim Star = Stars(StarIndex)
                    Dim StarSymbol As Char = "*"c
                    Dim StarX As Integer = Star.Item1
                    Dim StarY As Integer = Star.Item2
                    WriteWhere(StarSymbol, StarX, StarY, False, ConsoleColor.White)
                Next
            Else
                WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...")
                Stars.Clear()
            End If

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(StarfieldDelay, ScreensaverDisplayerThread)
            Console.Clear()
        End Sub

    End Class
End Namespace
