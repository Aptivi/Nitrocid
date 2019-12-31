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

Public Module Translate

    'Variables
    Public availableLangs() As String = {"arb", "arb-T", "chi", "chi-T", "cro", "cze", "dtc", "eng", "fin", "fre", "ger", "ind", "ind-T", "ita", "jpn", "jpn-T", "mal", "ndo", "pol", "ptg", "rmn", "rus", "rus-T", "spa", "swe", "tky", "uzb"}
    Public Transliterables() As String = {"arb", "chi", "ind", "jpn", "rus"}
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
            Wdbg("Dictionary size: {0}", translatedString.Count)

            'Do translation
            If translatedString.Keys.Contains(text) Then
                Wdbg("Translating string to {0}: {1}", lang, text)
                Return translatedString(text)
            Else 'String wasn't found
                Wdbg("No string found in langlist. Lang: {0}, String: {1}", lang, text)
                text = " ! Needs Localization ! " + text
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
            Case "arb" 'Arabic (transliterated)
                translated = My.Resources.arb
            Case "arb-T" 'Arabic (translated)
                translated = My.Resources.arb_T
            Case "chi" 'Chinese (transliterated)
                translated = My.Resources.chi
            Case "chi-T" 'Chinese (translated)
                translated = My.Resources.chi_T
            Case "cro" 'Croatian
                translated = My.Resources.cro
            Case "cze" 'Czech
                translated = My.Resources.cze
            Case "dtc" 'Dutch
                translated = My.Resources.dtc
            Case "fre" 'French
                translated = My.Resources.fre
            Case "fin" 'Finnish
                translated = My.Resources.fin
            Case "ger" 'Germany
                translated = My.Resources.ger
            Case "ind" 'Hindi (transliterated)
                translated = My.Resources.ind
            Case "ind-T" 'Hindi (translated)
                translated = My.Resources.ind_T
            Case "ita" 'Italian
                translated = My.Resources.ita
            Case "jpn" 'Japanese (transliterated)
                translated = My.Resources.jpn
            Case "jpn-T" 'Hindi (translated)
                translated = My.Resources.jpn_T
            Case "mal" 'Malay (not Malayalam)
                translated = My.Resources.mal
            Case "ndo" 'Indonesian
                translated = My.Resources.ndo
            Case "pol" 'Polish
                translated = My.Resources.pol
            Case "ptg" 'Portuguese
                translated = My.Resources.ptg
            Case "rmn" 'Romanian
                translated = My.Resources.rmn
            Case "rus" 'Russian (transliterated)
                translated = My.Resources.rus
            Case "rus-T" 'Hindi (translated)
                translated = My.Resources.rus_T
            Case "spa" 'Spanish
                translated = My.Resources.spa
            Case "swe" 'Swedish
                translated = My.Resources.swe
            Case "tky" 'Turkish
                translated = My.Resources.tky
            Case "uzb" 'Uzbekistan
                translated = My.Resources.uzb
        End Select

        'Convert translated string list to Dictionary
        Dim translatedLs As List(Of String) = translated.Replace(Chr(13), "").Split(Chr(10)).ToList

        'Move final translations to dictionary
        For ind As Integer = 0 To translatedLs.Count - 1
            langStrings.Add(engStrings(ind), translatedLs(ind))
        Next
        Return langStrings
    End Function

    Public Sub SetLang(ByVal lang As String, Optional ByVal Force As Boolean = False)
        If availableLangs.Contains(lang) Then
            Wdbg("Forced {0}", Force)
            If Not Force Then
                If lang.EndsWith("-T") Then 'The condition prevents tricksters from using "chlang <lang>-T", if not forced.
                    Wdbg("Trying to bypass prompt.")
                    Exit Sub
                Else
                    'Check to see if the language is transliterable
                    Wdbg("Transliterable? {0}", Transliterables.Contains(lang))
                    If Transliterables.Contains(lang) Then
                        W(DoTranslation("The language you've selected contains two variants. Select one:", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        W(DoTranslation("1. Transliterated", lang), True, ColTypes.Neutral)
                        W(DoTranslation("2. Translated", lang + "-T") + vbNewLine, True, ColTypes.Neutral)
CHOICE:
                        W(DoTranslation("Select your choice:", currentLang), False, ColTypes.Input)
                        Dim cho As String = Console.ReadKey(True).KeyChar
                        Console.WriteLine()
                        Wdbg("Choice: {0}", cho)
                        If cho = "2" Then
                            lang += "-T"
                        ElseIf Not cho = "1" Then
                            W(DoTranslation("Invalid choice. Try again.", currentLang), True, ColTypes.Neutral)
                            GoTo CHOICE
                        End If
                    End If
                End If
            End If

            'Set appropriate codepage for incapable terminals
            Try
                Select Case lang
                    Case "arb-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(1256)
                        Console.InputEncoding = Text.Encoding.GetEncoding(1256)
                        Wdbg("Encoding set successfully for Arabic to {0}.", Console.OutputEncoding.EncodingName)
                    Case "chi-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(936)
                        Console.InputEncoding = Text.Encoding.GetEncoding(936)
                        Wdbg("Encoding set successfully for Chinese to {0}.", Console.OutputEncoding.EncodingName)
                    Case "jpn-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(932)
                        Console.InputEncoding = Text.Encoding.GetEncoding(932)
                        Wdbg("Encoding set successfully for Japanese to {0}.", Console.OutputEncoding.EncodingName)
                    Case "rus-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(866)
                        Console.InputEncoding = Text.Encoding.GetEncoding(866)
                        Wdbg("Encoding set successfully for Russian to {0}.", Console.OutputEncoding.EncodingName)
                    Case Else
                        Console.OutputEncoding = Text.Encoding.GetEncoding(65001)
                        Console.InputEncoding = Text.Encoding.GetEncoding(65001)
                        Wdbg("Encoding set successfully to {0}.", Console.OutputEncoding.EncodingName)
                End Select
            Catch ex As Exception
                W(DoTranslation("Unable to set codepage. The language may not display properly.", currentLang), True, ColTypes.Neutral)
                WStkTrc(ex)
            End Try

            'Set current language
            W(DoTranslation("Changing from: {0} to {1}...", currentLang), True, ColTypes.Neutral, currentLang, lang)
            currentLang = lang
            Dim ksconf As New IniFile()
            Dim pathConfig As String = paths("Configuration")
            ksconf.Load(pathConfig)
            ksconf.Sections("General").Keys("Language").Value = currentLang
            ksconf.Save(pathConfig)

            'Update help list for translated help
            InitHelp()
            InitFTPHelp()
        Else
            W(DoTranslation("Invalid language", currentLang) + " {0}", True, ColTypes.Neutral, lang)
        End If
    End Sub

End Module
