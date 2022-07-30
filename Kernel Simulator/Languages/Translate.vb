
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

Imports Newtonsoft.Json.Linq

Namespace Languages
    Public Module Translate

        ''' <summary>
        ''' Translates string into current kernel language.
        ''' </summary>
        ''' <param name="text">Any string that exists in Kernel Simulator's translation files</param>
        ''' <returns>Translated string</returns>
        Public Function DoTranslation(text As String) As String
            Return DoTranslation(text, CurrentLanguage)
        End Function

        ''' <summary>
        ''' Translates string into another language, or to English if the language wasn't specified or if it's invalid.
        ''' </summary>
        ''' <param name="text">Any string that exists in Kernel Simulator's translation files</param>
        ''' <param name="lang">3 letter language</param>
        ''' <returns>Translated string</returns>
        Public Function DoTranslation(text As String, lang As String) As String
            If String.IsNullOrWhiteSpace(lang) Then lang = "eng"
            'Get language string and translate
            Dim translatedString As Dictionary(Of String, String)

            'If the language is available and is not English, translate
            If Languages.ContainsKey(lang) And lang <> "eng" Then
                'Prepare dictionary
                translatedString = PrepareDict(lang)
                Wdbg(DebugLevel.I, "Dictionary size: {0}", translatedString.Count)

                'Do translation
                If translatedString.ContainsKey(text) Then
                    Wdbg(DebugLevel.I, "Translating string to {0}: {1}", lang, text)
                    Return translatedString(text)
                Else 'String wasn't found
                    Wdbg(DebugLevel.W, "No string found in langlist. Lang: {0}, String: {1}", lang, text)
                    text = "(( " + text + " ))"
                    Return text
                End If
            ElseIf Languages.ContainsKey(lang) And lang = "eng" Then 'If the language is available, but is English, don't translate
                Return text
            Else 'If the language is invalid
                Wdbg(DebugLevel.E, "{0} isn't in language list", lang)
                Return text
            End If
        End Function

        ''' <summary>
        ''' Prepares the translation dictionary for a language
        ''' </summary>
        ''' <param name="lang">A specified language</param>
        ''' <returns>A dictionary of English strings and translated strings</returns>
        Public Function PrepareDict(lang As String) As Dictionary(Of String, String)
            Dim langStrings As New Dictionary(Of String, String)

            'Move final translations to dictionary
            For Each TranslatedProperty As JProperty In Languages(lang).LanguageResource.Properties
                langStrings.Add(TranslatedProperty.Name, TranslatedProperty.Value)
            Next
            Return langStrings
        End Function

    End Module
End Namespace