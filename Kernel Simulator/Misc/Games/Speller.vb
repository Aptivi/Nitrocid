
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

Imports KS.Network.Transfer

Namespace Misc.Games
    Module Speller

        Public Words As New List(Of String)

        ''' <summary>
        ''' Initializes the game
        ''' </summary>
        Sub InitializeWords()
            Dim RandomDriver As New Random
            Dim RandomWord As String
            Dim SpeltWord As String
            Write(DoTranslation("Press CTRL+C to exit."), True, ColTypes.Tip)
            If Words.Count = 0 Then
                Wdbg(DebugLevel.I, "Downloading words...")
                Words.AddRange(DownloadString("https://cdn.jsdelivr.net/gh/sindresorhus/word-list/words.txt").SplitNewLines.ToList)
            End If
            While True
                RandomWord = Words.ElementAt(RandomDriver.Next(Words.Count))
                Wdbg(DebugLevel.I, "Word: {0}", RandomWord)
                Write(RandomWord, True, ColTypes.Input)
                SpeltWord = ReadLineNoInput("")

                If SpeltWord = RandomWord Then
                    Wdbg(DebugLevel.I, "Spelt: {0} = {1}", SpeltWord, RandomWord)
                    Write(DoTranslation("Spelt perfectly!"), True, ColTypes.Success)
                Else
                    Wdbg(DebugLevel.I, "Spelt: {0} != {1}", SpeltWord, RandomWord)
                    Write(DoTranslation("Spelt incorrectly."), True, ColTypes.Warning)
                End If
            End While
        End Sub

    End Module
End Namespace