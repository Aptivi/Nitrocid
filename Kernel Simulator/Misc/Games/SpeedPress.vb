
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Module SpeedPress

    ''' <summary>
    ''' Initializes the SpeedPress game
    ''' </summary>
    ''' <param name="Difficulty">Whether it's (e)asy, (m)edium, or (h)ard</param>
    Sub InitializeSpeedPress(Difficulty As Char)
        Dim SpeedTimeout As Integer
        Dim SelectedChar As Char
        Dim WrittenChar As ConsoleKeyInfo
        Dim RandomEngine As New Random

        'Change timeout based on difficulty
        Select Case Difficulty
            Case "e"c
                SpeedTimeout = 3000
            Case "m"c
                SpeedTimeout = 1500
            Case "h"c
                SpeedTimeout = 500
        End Select

        'Enter the loop until the user presses ESC
        W(DoTranslation("Press ESC to exit.") + vbNewLine, True, ColTypes.Neutral)
        While Not WrittenChar.Key = ConsoleKey.Escape Or Not WrittenChar.Modifiers = ConsoleModifiers.Control And WrittenChar.Key = ConsoleKey.C
            'Select a random character
            SelectedChar = Convert.ToChar(RandomEngine.Next(97, 122))

            'Prompt user for character
            Try
                W(DoTranslation("Current character:") + " {0}", True, ColTypes.Neutral, SelectedChar)
                W("> ", False, ColTypes.Input)
                WrittenChar = ReadKeyTimeout(False, TimeSpan.FromMilliseconds(SpeedTimeout))
                Console.WriteLine()

                'Check to see if the user has pressed the correct character
                If WrittenChar.KeyChar = SelectedChar Then
                    W(DoTranslation("You've pressed the right character!"), True, ColTypes.Neutral)
                ElseIf Not WrittenChar.Key = ConsoleKey.Escape Or Not WrittenChar.Modifiers = ConsoleModifiers.Control And WrittenChar.Key = ConsoleKey.C Then
                    W(DoTranslation("You've pressed the wrong character."), True, ColTypes.Warning)
                End If
            Catch ex As Exceptions.ConsoleReadTimeoutException
                Console.WriteLine()
                W(DoTranslation("Character not pressed on time."), True, ColTypes.Warning)
            End Try
        End While
    End Sub

End Module
