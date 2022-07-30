
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

Namespace Misc.Games
    Public Module SpeedPress

        Public SpeedPressCurrentDifficulty As SpeedPressDifficulty = SpeedPressDifficulty.Medium
        Public SpeedPressTimeout As Integer = 3000

        ''' <summary>
        ''' SpeedPress difficulty
        ''' </summary>
        Public Enum SpeedPressDifficulty
            ''' <summary>
            ''' Easy difficulty (timeout for three seconds)
            ''' </summary>
            Easy
            ''' <summary>
            ''' Medium difficulty (timeout for a second and a half)
            ''' </summary>
            Medium
            ''' <summary>
            ''' Hard difficulty (timeout for a half second)
            ''' </summary>
            Hard
            ''' <summary>
            ''' Very hard difficulty (timeout for a quarter of a second)
            ''' </summary>
            VeryHard
            ''' <summary>
            ''' Custom difficulty (custom timeout according to either a switch or the kernel settings)
            ''' </summary>
            Custom
        End Enum

        ''' <summary>
        ''' Initializes the SpeedPress game
        ''' </summary>
        ''' <param name="Difficulty">The difficulty of the game</param>
        Sub InitializeSpeedPress(Difficulty As SpeedPressDifficulty, Optional CustomTimeout As Integer = 0)
            Dim SpeedTimeout As Integer
            Dim SelectedChar As Char
            Dim WrittenChar As ConsoleKeyInfo
            Dim RandomEngine As New Random

            'Change timeout based on difficulty
            Select Case Difficulty
                Case SpeedPressDifficulty.Easy
                    SpeedTimeout = 3000
                Case SpeedPressDifficulty.Medium
                    SpeedTimeout = 1500
                Case SpeedPressDifficulty.Hard
                    SpeedTimeout = 500
                Case SpeedPressDifficulty.VeryHard
                    SpeedTimeout = 250
                Case SpeedPressDifficulty.Custom
                    If CustomTimeout > 0 Then
                        SpeedTimeout = Math.Abs(CustomTimeout)
                    ElseIf SpeedPressTimeout > 0 Then
                        SpeedTimeout = Math.Abs(SpeedPressTimeout)
                    Else
                        SpeedTimeout = 1500
                    End If
            End Select

            'Enter the loop until the user presses ESC
            Write(DoTranslation("Press ESC to exit.") + NewLine, True, ColTypes.Tip)
            While Not WrittenChar.Key = ConsoleKey.Escape Or Not WrittenChar.Modifiers = ConsoleModifiers.Control And WrittenChar.Key = ConsoleKey.C
                'Select a random character
                SelectedChar = Convert.ToChar(RandomEngine.Next(97, 122))

                'Prompt user for character
                Try
                    Write(DoTranslation("Current character:") + " {0}", True, ColTypes.Neutral, SelectedChar)
                    Write("> ", False, ColTypes.Input)
                    WrittenChar = ReadKeyTimeout(False, TimeSpan.FromMilliseconds(SpeedTimeout))
                    Console.WriteLine()

                    'Check to see if the user has pressed the correct character
                    If WrittenChar.KeyChar = SelectedChar Then
                        Write(DoTranslation("You've pressed the right character!"), True, ColTypes.Success)
                    ElseIf Not WrittenChar.Key = ConsoleKey.Escape Or Not WrittenChar.Modifiers = ConsoleModifiers.Control And WrittenChar.Key = ConsoleKey.C Then
                        Write(DoTranslation("You've pressed the wrong character."), True, ColTypes.Warning)
                    End If
                Catch ex As Exceptions.ConsoleReadTimeoutException
                    Console.WriteLine()
                    Write(DoTranslation("Character not pressed on time."), True, ColTypes.Warning)
                End Try
            End While
        End Sub

    End Module
End Namespace