
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Public Module Translate

    'TODO: Download language files from the Internet (GitHub repo "KSLangs")
    'Variables
    Public availableLangs() As String = {"chi", "cze", "dtc", "eng", "fin", "fre", "ger", "ind", "ita", "mal", "ptg", "spa", "swe", "tky"}
    Public engStrings As List(Of String) = My.Resources.eng.Replace(Chr(13), "").Split(Chr(10)).ToList
    Public currentLang As String = "eng" 'Default to English

    ''' <summary>
    ''' Translate string into another language, or to English if the language wasn't specified or if it's invalid.
    ''' </summary>
    ''' <param name="text">Any string that exists in Kernel Simulator's translation files</param>
    ''' <param name="lang">3 letter language</param>
    ''' <returns>Translated string</returns>
    ''' <remarks></remarks>
    Public Function DoTranslation(ByVal text As String, Optional ByVal lang As String = "eng") As String
        'Get language string and translate
        Dim translatedString As Dictionary(Of String, String)

        'If the language is available and is not English, translate
        If availableLangs.Contains(lang) And lang <> "eng" Then
            'Prepare dictionary
            translatedString = PrepareDict(lang)

            'Do translation
            If translatedString.Keys.Contains(text) Then
                Wdbg("Translating string to {0}: {1}", lang, text)
                Return translatedString(text)
            Else 'String wasn't found
                Wdbg("No string found in langlist. Lang: {0}, String: {1}", lang, text)
                Return text
            End If
        ElseIf availableLangs.Contains(lang) And lang = "eng" Then 'If the language is available, but is English, don't translate
            Wdbg("{0} is in language list but it's English", lang)
            Return text
        Else 'If the language is invalid
            Wdbg("{0} isn't in language list", lang)
            Return text
        End If
    End Function

    Private Function PrepareDict(ByVal lang As String) As Dictionary(Of String, String)
        Dim langStrings As New Dictionary(Of String, String)
        Dim translated As String = ""
        Select Case lang
            Case "chi"
                translated = My.Resources.chi
            Case "cze"
                translated = My.Resources.cze
            Case "dtc"
                translated = My.Resources.dtc
            Case "fre"
                translated = My.Resources.fre
            Case "fin"
                translated = My.Resources.fin
            Case "ger"
                translated = My.Resources.ger
            Case "ind"
                translated = My.Resources.ind
            Case "ita"
                translated = My.Resources.ita
            Case "mal"
                translated = My.Resources.mal
            Case "ptg"
                translated = My.Resources.ptg
            Case "spa"
                translated = My.Resources.spa
            Case "swe"
                translated = My.Resources.swe
            Case "tky"
                translated = My.Resources.tky
        End Select

        'Convert translated string list to Dictionary
        Dim translatedLs As List(Of String) = translated.Replace(Chr(13), "").Split(Chr(10)).ToList

        'Move final translations to dictionary
        For ind As Integer = 0 To translatedLs.Count - 1
            langStrings.Add(engStrings(ind), translatedLs(ind))
        Next
        Return langStrings
    End Function

    Public Sub SetLang(ByVal lang As String)
        If availableLangs.Contains(lang) Then
            'Set current language
            Wln(DoTranslation("Changing from: {0} to {1}...", currentLang), "neutralText", currentLang, lang)
            currentLang = lang
            Dim ksconf As New IniFile()
            Dim pathConfig As String = paths("Configuration")
            ksconf.Load(pathConfig)
            ksconf.Sections("General").Keys("Language").Value = currentLang
            ksconf.Save(pathConfig)

            'Update help list for translated help
            InitHelp()
        Else
            Wln(DoTranslation("Invalid language", currentLang), "neutralText")
        End If
    End Sub

End Module
