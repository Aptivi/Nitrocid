
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

Imports KS.Network
Imports Newtonsoft.Json.Linq

Namespace Misc.Dictionary
    Public Module DictionaryManager

        Private CachedWords As New Dictionary(Of String, DictionaryWord)

        ''' <summary>
        ''' Gets the word information
        ''' </summary>
        Public Function GetWordInfo(Word As String) As DictionaryWord
            If CachedWords.ContainsKey(Word) Then
                'We already have a word, so there is no reason to download it again
                Return CachedWords(Word)
            Else
                'Download the word information
                Dim WordInfoString As String = DownloadString($"https://api.dictionaryapi.dev/api/v2/entries/en/{Word}", False)

                'Serialize it to DictionaryWord to cache it so that we don't have to download it again
                Dim CachedWord As DictionaryWord = JsonConvert.DeserializeObject(Of DictionaryWord())(WordInfoString)(0)
                CachedWords.Add(Word, CachedWord)

                'Return the word
                Return CachedWord
            End If
        End Function

    End Module
End Namespace