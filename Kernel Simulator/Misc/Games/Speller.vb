
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Module Speller

    Public Words As New List(Of String)
    Sub InitializeWords()
        Dim Downloader As New WebClient
        Dim RandomDriver As New Random
        Dim RandomWord As String
        Dim SpeltWord As String = ""
        W(DoTranslation("Write ""GetMeOut"" to exit.", currentLang), True, ColTypes.Neutral)
        If Words.Count = 0 Then
            Wdbg("Downloading words...")
            Words.AddRange(Downloader.DownloadString("https://raw.githubusercontent.com/sindresorhus/word-list/master/words.txt").Replace(Chr(13), "").Split(Chr(10)).ToList)
            Downloader.Dispose()
        End If
        While True
            RandomWord = Words.ElementAt(RandomDriver.Next(Words.Count))
            Wdbg("Word: {0}", RandomWord)
            W(RandomWord, True, ColTypes.Input)
            SpeltWord = ReadLineNoInput()

            If SpeltWord = RandomWord Then
                Wdbg("Spelt: {0} = {1}", SpeltWord, RandomWord)
                W(DoTranslation("Spelt perfectly!", currentLang), True, ColTypes.Neutral)
            ElseIf SpeltWord = "GetMeOut" Then
                Wdbg("Exit")
                Exit Sub
            Else
                Wdbg("Spelt: {0} != {1}", SpeltWord, RandomWord)
                W(DoTranslation("Spelt incorrectly.", currentLang), True, ColTypes.Neutral)
            End If
            SpeltWord = ""
        End While
    End Sub

End Module
