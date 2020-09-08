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

Imports System.Globalization

Public Module Translate

    'Variables
    Public availableLangs() As String = {"arb", "arb-T", "ben", "ben-T", "chi", "chi-T", "cro", "ctl", "cze", "dan", "dtc", "eng", "fin", "flp", "fre", "ger", "ind", "ind-T", "ita", "jpn", "jpn-T", "kor", "kor-T", "mal", "ndo", "nwg", "pol", "ptg", "pun", "pun-T", "rmn", "rus", "rus-T", "slo", "spa", "srb", "srb-T", "swe", "uzb", "vtn"}
    Public Transliterables() As String = {"arb", "ben", "chi", "ind", "jpn", "kor", "pun", "rus", "srb"}
    Public engStrings As List(Of String) = My.Resources.eng.Replace(Chr(13), "").Split(Chr(10)).ToList
    Public currentLang As String = "eng" 'Default to English
    Public CurrentCult As New CultureInfo("en-US")

    ''' <summary>
    ''' Translates string into another language, or to English if the language wasn't specified or if it's invalid.
    ''' </summary>
    ''' <param name="text">Any string that exists in Kernel Simulator's translation files</param>
    ''' <param name="lang">3 letter language</param>
    ''' <returns>Translated string</returns>
    Public Function DoTranslation(ByVal text As String, Optional ByVal lang As String = "eng") As String
        'Get language string and translate
        Dim translatedString As Dictionary(Of String, String)

        'If the language is available and is not English, translate
        If availableLangs.Contains(lang) And lang <> "eng" Then
            'Prepare dictionary
            translatedString = PrepareDict(lang)
            Wdbg("I", "Dictionary size: {0}", translatedString.Count)

            'Do translation
            If translatedString.Keys.Contains(text) Then
                Wdbg("I", "Translating string to {0}: {1}", lang, text)
                Return translatedString(text)
            Else 'String wasn't found
                Wdbg("W", "No string found in langlist. Lang: {0}, String: {1}", lang, text)
                text = " ! Needs Localization ! " + text
                Return text
            End If
        ElseIf availableLangs.Contains(lang) And lang = "eng" Then 'If the language is available, but is English, don't translate
            Wdbg("W", "{0} is in language list but it's English", lang)
            Return text
        Else 'If the language is invalid
            Wdbg("E", "{0} isn't in language list", lang)
            Return text
        End If
    End Function

    ''' <summary>
    ''' Prepares the translation dictionary for a language
    ''' </summary>
    ''' <param name="lang">A specified language</param>
    ''' <returns>A dictionary of English strings and translated strings</returns>
    Public Function PrepareDict(ByVal lang As String) As Dictionary(Of String, String)
        Dim langStrings As New Dictionary(Of String, String)
        Dim translated As String = ""
        Select Case lang
            Case "arb" 'Arabic (transliterated)
                translated = My.Resources.arb
            Case "arb-T" 'Arabic (translated)
                translated = My.Resources.arb_T
            Case "ben" 'Bengali (transliterated)
                translated = My.Resources.ben
            Case "ben-T" 'Bengali (translated)
                translated = My.Resources.ben_T
            Case "chi" 'Chinese (transliterated)
                translated = My.Resources.chi
            Case "chi-T" 'Chinese (translated)
                translated = My.Resources.chi_T
            Case "cro" 'Croatian
                translated = My.Resources.cro
            Case "ctl" 'Catalan
                translated = My.Resources.ctl
            Case "cze" 'Czech
                translated = My.Resources.cze
            Case "dan" 'Danish
                translated = My.Resources.dan
            Case "dtc" 'Dutch
                translated = My.Resources.dtc
            Case "fre" 'French
                translated = My.Resources.fre
            Case "flp" 'Filipino
                translated = My.Resources.flp
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
            Case "jpn-T" 'Japanese (translated)
                translated = My.Resources.jpn_T
            Case "kor" 'Korean (transliterated)
                translated = My.Resources.kor
            Case "kor-T" 'Korean (translated)
                translated = My.Resources.kor_T
            Case "mal" 'Malay (not Malayalam)
                translated = My.Resources.mal
            Case "ndo" 'Indonesian
                translated = My.Resources.ndo
            Case "nwg" 'Norwegian
                translated = My.Resources.nwg
            Case "pol" 'Polish
                translated = My.Resources.pol
            Case "ptg" 'Portuguese
                translated = My.Resources.ptg
            Case "pun" 'Punjabi (transliterated)
                translated = My.Resources.pun
            Case "pun-T" 'Punjabi (translated)
                translated = My.Resources.pun_T
            Case "rmn" 'Romanian
                translated = My.Resources.rmn
            Case "rus" 'Russian (transliterated)
                translated = My.Resources.rus
            Case "rus-T" 'Hindi (translated)
                translated = My.Resources.rus_T
            Case "slo" 'Slovak
                translated = My.Resources.slo
            Case "spa" 'Spanish
                translated = My.Resources.spa
            Case "srb" 'Serbian (transliterated)
                translated = My.Resources.srb
            Case "srb-T" 'Serbian (translated)
                translated = My.Resources.srb_T
            Case "swe" 'Swedish
                translated = My.Resources.swe
            Case "uzb" 'Uzbekistan
                translated = My.Resources.uzb
            Case "vtn" 'Vietnamese
                translated = My.Resources.vtn
        End Select

        'Convert translated string list to Dictionary
        Dim translatedLs As List(Of String) = translated.Replace(Chr(13), "").Split(Chr(10)).ToList

        'Move final translations to dictionary
        For ind As Integer = 0 To translatedLs.Count - 1
            langStrings.Add(engStrings(ind), translatedLs(ind))
        Next
        Return langStrings
    End Function

    Private NotifyCodepageError As Boolean

    ''' <summary>
    ''' Prompt for setting language
    ''' </summary>
    ''' <param name="lang">A specified language</param>
    ''' <param name="Force">Force changes</param>
    Sub PromptForSetLang(ByVal lang As String, Optional ByVal Force As Boolean = False)
        If availableLangs.Contains(lang) Then
            Wdbg("I", "Forced {0}", Force)
            If Not Force Then
                If lang.EndsWith("-T") Then 'The condition prevents tricksters from using "chlang <lang>-T", if not forced.
                    Wdbg("W", "Trying to bypass prompt.")
                    Exit Sub
                Else
                    'Check to see if the language is transliterable
                    Wdbg("I", "Transliterable? {0}", Transliterables.Contains(lang))
                    If Transliterables.Contains(lang) Then
                        W(DoTranslation("The language you've selected contains two variants. Select one:", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        W(DoTranslation("1. Transliterated", lang), True, ColTypes.Neutral)
                        W(DoTranslation("2. Translated", lang + "-T") + vbNewLine, True, ColTypes.Neutral)
CHOICE:
                        W(DoTranslation("Select your choice:", currentLang), False, ColTypes.Input)
                        Dim cho As String = Console.ReadKey(True).KeyChar
                        Console.WriteLine()
                        Wdbg("I", "Choice: {0}", cho)
                        If cho = "2" Then
                            lang += "-T"
                        ElseIf Not cho = "1" Then
                            W(DoTranslation("Invalid choice. Try again.", currentLang), True, ColTypes.Err)
                            GoTo CHOICE
                        End If
                    End If
                End If
            End If

            W(DoTranslation("Changing from: {0} to {1}...", currentLang), True, ColTypes.Neutral, currentLang, lang)
            If Not SetLang(lang) Then
                W(DoTranslation("Failed to set language.", currentLang), True, ColTypes.Err)
            End If
            If NotifyCodepageError Then
                W(DoTranslation("Unable to set codepage. The language may not display properly.", currentLang), True, ColTypes.Err)
            End If
        Else
            W(DoTranslation("Invalid language", currentLang) + " {0}", True, ColTypes.Err, lang)
        End If
    End Sub

    ''' <summary>
    ''' Sets a system language permanently
    ''' </summary>
    ''' <param name="lang">A specified language</param>
    ''' <returns>True if successful, False if unsuccessful.</returns>
    Public Function SetLang(ByVal lang As String) As Boolean
        If availableLangs.Contains(lang) Then
            'Set appropriate codepage for incapable terminals
            Try
                Select Case lang
                    Case "arb-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(1256)
                        Console.InputEncoding = Text.Encoding.GetEncoding(1256)
                        Wdbg("I", "Encoding set successfully for Arabic to {0}.", Console.OutputEncoding.EncodingName)
                    Case "ben-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(57003)
                        Console.InputEncoding = Text.Encoding.GetEncoding(57003)
                        Wdbg("I", "Encoding set successfully for Bengali to {0}.", Console.OutputEncoding.EncodingName)
                    Case "chi-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(936)
                        Console.InputEncoding = Text.Encoding.GetEncoding(936)
                        Wdbg("I", "Encoding set successfully for Chinese to {0}.", Console.OutputEncoding.EncodingName)
                    Case "jpn-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(932)
                        Console.InputEncoding = Text.Encoding.GetEncoding(932)
                        Wdbg("I", "Encoding set successfully for Japanese to {0}.", Console.OutputEncoding.EncodingName)
                    Case "kor-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(949)
                        Console.InputEncoding = Text.Encoding.GetEncoding(949)
                        Wdbg("I", "Encoding set successfully for Korean to {0}.", Console.OutputEncoding.EncodingName)
                    Case "pun-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(57011)
                        Console.InputEncoding = Text.Encoding.GetEncoding(57011)
                        Wdbg("I", "Encoding set successfully for Punjabi to {0}.", Console.OutputEncoding.EncodingName)
                    Case "rus-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(866)
                        Console.InputEncoding = Text.Encoding.GetEncoding(866)
                        Wdbg("I", "Encoding set successfully for Russian to {0}.", Console.OutputEncoding.EncodingName)
                    Case "srb-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(21025)
                        Console.InputEncoding = Text.Encoding.GetEncoding(21025)
                        Wdbg("I", "Encoding set successfully for Serbian to {0}.", Console.OutputEncoding.EncodingName)
                    Case "dan"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(865)
                        Console.InputEncoding = Text.Encoding.GetEncoding(865)
                        Wdbg("I", "Encoding set successfully for Danish to {0}.", Console.OutputEncoding.EncodingName)
                    Case "vtn"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(1258)
                        Console.InputEncoding = Text.Encoding.GetEncoding(1258)
                        Wdbg("I", "Encoding set successfully for Vietnamese to {0}.", Console.OutputEncoding.EncodingName)
                    Case Else
                        Console.OutputEncoding = Text.Encoding.GetEncoding(65001)
                        Console.InputEncoding = Text.Encoding.GetEncoding(65001)
                        Wdbg("I", "Encoding set successfully to {0}.", Console.OutputEncoding.EncodingName)
                End Select
            Catch ex As Exception
                NotifyCodepageError = True
                Wdbg("W", "Codepage can't be set. {0}", ex.Message)
                WStkTrc(ex)
            End Try

            'Set current language
            Try
                Dim OldModDescGeneric As String = DoTranslation("Command defined by ", currentLang)
                Wdbg("I", "Translating kernel to {0}.", lang)
                currentLang = lang
                Dim ksconf As New IniFile()
                Dim pathConfig As String = paths("Configuration")
                ksconf.Load(pathConfig)
                ksconf.Sections("General").Keys("Language").Value = currentLang
                ksconf.Save(pathConfig)
                Wdbg("I", "Saved new language.")

                'Update help list for translated help
                InitHelp()
                InitFTPHelp()
                IMAPInitHelp()
                TextEdit_UpdateHelp()
                ReloadGenericDefs(OldModDescGeneric)

                'Update Culture if applicable
                If LangChangeCulture Then
                    Wdbg("I", "Updating culture.")
                    UpdateCulture()
                End If
                Return True
            Catch ex As Exception
                Wdbg("W", "Language can't be set. {0}", ex.Message)
                WStkTrc(ex)
            End Try
        Else
            Throw New EventsAndExceptions.NoSuchLanguageException(DoTranslation("Invalid language", currentLang) + " {0}".FormatString(lang))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Updates current culture based on current language
    ''' </summary>
    Public Sub UpdateCulture()
        Dim StrCult As String = GetCultureFromLang()
        Wdbg("I", "Culture for {0} is {1}", currentLang, StrCult)
        Dim Cults As CultureInfo() = CultureInfo.GetCultures(CultureTypes.AllCultures)
        Wdbg("I", "Parsing {0} cultures for {1}", Cults.Length, StrCult)
        For Each Cult As CultureInfo In Cults
            If Cult.EnglishName = StrCult Then
                Wdbg("I", "Found. Changing culture...")
                CurrentCult = Cult
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Get culture from current language
    ''' </summary>
    ''' <returns>English culture name</returns>
    Public Function GetCultureFromLang() As String
        Select Case currentLang
            Case "arb-T", "arb"
                Return "Arabic (Saudi Arabia)"
            Case "ben-T", "ben"
                Return "Bangla (Bangladesh)"
            Case "chi-T", "chi"
                Return "Chinese (Simplified, China)"
            Case "cro"
                Return "Croatian (Croatia)"
            Case "ctl"
                Return "Catalan (Catalan)"
            Case "cze"
                Return "Czech (Czech Republic)"
            Case "dan"
                Return "Danish (Denmark)"
            Case "dtc"
                Return "Dutch (Netherlands)"
            Case "eng"
                Return "English (United States)"
            Case "fin"
                Return "Finnish (Finland)"
            Case "flp"
                Return "Filipino (Philippines)"
            Case "fre"
                Return "French (France)"
            Case "ger"
                Return "German (Germany)"
            Case "ind-T", "ind"
                Return "Hindi (India)"
            Case "ita"
                Return "Italian (Italy)"
            Case "jpn-T", "jpn"
                Return "Japanese (Japan)"
            Case "kor-T", "kor"
                Return "Korean (Korea)"
            Case "mal"
                Return "Malay (Malaysia)"
            Case "ndo"
                Return "Indonesian (Indonesia)"
            Case "nwg"
                Return "Norwegian Bokmål (Norway)"
            Case "pol"
                Return "Polish (Poland)"
            Case "ptg"
                Return "Portuguese (Brazil)"
            Case "pun-T", "pun"
                Return "Punjabi (India)"
            Case "rmn"
                Return "Romanian (Romania)"
            Case "rus-T", "rus"
                Return "Russian (Russia)"
            Case "slo"
                Return "Slovak (Slovakia)"
            Case "spa"
                Return "Spanish (Spain, International Sort)"
            Case "srb-T"
                Return "Serbian (Cyrillic, Serbia)"
            Case "srb"
                Return "Serbian (Latin, Serbia)"
            Case "swe"
                Return "Swedish (Sweden)"
            Case "uzb"
                Return "Uzbek (Cyrillic)"
            Case "vtn"
                Return "Vietnamese (Vietnam)"
        End Select
    End Function

End Module
