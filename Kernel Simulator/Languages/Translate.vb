
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

Imports System.Globalization
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Module Translate

    'Variables
    Public ReadOnly Languages As New Dictionary(Of String, LanguageInfo) From {{"arb", New LanguageInfo("arb", "Arabic", True)}, {"arb-T", New LanguageInfo("arb-T", "Arabic", True)},
                                                                               {"azr", New LanguageInfo("azr", "Azerbaijani", False)},
                                                                               {"ben", New LanguageInfo("ben", "Bangla", True)}, {"ben-T", New LanguageInfo("ben-T", "Bangla", True)},
                                                                               {"bsq", New LanguageInfo("bsq", "Basque", False)},
                                                                               {"chi", New LanguageInfo("chi", "Chinese", True)}, {"chi-T", New LanguageInfo("chi-T", "Chinese", True)},
                                                                               {"cro", New LanguageInfo("cro", "Croatian", False)},
                                                                               {"csc", New LanguageInfo("csc", "Corsican", False)},
                                                                               {"ctl", New LanguageInfo("ctl", "Catalan", False)},
                                                                               {"cze", New LanguageInfo("cze", "Czech", False)},
                                                                               {"dan", New LanguageInfo("dan", "Danish", False)},
                                                                               {"dtc", New LanguageInfo("dtc", "Dutch", False)},
                                                                               {"eng", New LanguageInfo("eng", "English", False)},
                                                                               {"fin", New LanguageInfo("fin", "Finnish", False)},
                                                                               {"flp", New LanguageInfo("flp", "Filipino", False)},
                                                                               {"fre", New LanguageInfo("fre", "French", False)},
                                                                               {"ger", New LanguageInfo("ger", "German", False)},
                                                                               {"glc", New LanguageInfo("glc", "Galician", False)},
                                                                               {"hwi", New LanguageInfo("hwi", "Hawaiian", False)},
                                                                               {"ind", New LanguageInfo("ind", "Hindi", True)}, {"ind-T", New LanguageInfo("ind-T", "Hindi", True)},
                                                                               {"iri", New LanguageInfo("iri", "Irish", False)},
                                                                               {"ita", New LanguageInfo("ita", "Italian", False)},
                                                                               {"jpn", New LanguageInfo("jpn", "Japanese", True)}, {"jpn-T", New LanguageInfo("jpn-T", "Japanese", True)},
                                                                               {"jvn", New LanguageInfo("jvn", "Javanese", False)},
                                                                               {"kor", New LanguageInfo("kor", "Korean", True)}, {"kor-T", New LanguageInfo("kor-T", "Korean", True)},
                                                                               {"ltn", New LanguageInfo("ltn", "Latin", False)},
                                                                               {"mal", New LanguageInfo("mal", "Malay", False)},
                                                                               {"mts", New LanguageInfo("mts", "Maltese", False)},
                                                                               {"ndo", New LanguageInfo("ndo", "Indonesian", False)},
                                                                               {"nwg", New LanguageInfo("nwg", "Norwegian", False)},
                                                                               {"pol", New LanguageInfo("pol", "Polish", False)},
                                                                               {"ptg", New LanguageInfo("ptg", "Portuguese", False)},
                                                                               {"pun", New LanguageInfo("pun", "Punjabi", True)}, {"pun-T", New LanguageInfo("pun-T", "Punjabi", True)},
                                                                               {"rmn", New LanguageInfo("rmn", "Romanian", False)},
                                                                               {"rus", New LanguageInfo("rus", "Russian", True)}, {"rus-T", New LanguageInfo("rus-T", "Russian", True)},
                                                                               {"slo", New LanguageInfo("slo", "Slovak", False)},
                                                                               {"som", New LanguageInfo("som", "Somali", False)},
                                                                               {"spa", New LanguageInfo("spa", "Spanish", False)},
                                                                               {"srb", New LanguageInfo("srb", "Serbian", True)}, {"srb-T", New LanguageInfo("srb-T", "Serbian", True)},
                                                                               {"swa", New LanguageInfo("swa", "Swahili", False)}, 'Don't use Kiswahili here. "Swahili" is saner than "Kiswahili".
                                                                               {"swe", New LanguageInfo("swe", "Swedish", False)},
                                                                               {"uzb", New LanguageInfo("uzb", "Uzbek", False)},
                                                                               {"vtn", New LanguageInfo("vtn", "Vietnamese", False)},
                                                                               {"wls", New LanguageInfo("wls", "Welsh", False)},
                                                                               {"zul", New LanguageInfo("zul", "Zulu", False)}} 'Don't use isiZulu here. isiZulu? Really? What is "isi" doing here?
    Public currentLang As String = "eng" 'Default to English
    Public CurrentCult As New CultureInfo("en-US")
    Private NotifyCodepageError As Boolean

    ''' <summary>
    ''' Translates string into current kernel language.
    ''' </summary>
    ''' <param name="text">Any string that exists in Kernel Simulator's translation files</param>
    ''' <returns>Translated string</returns>
    Public Function DoTranslation(ByVal text As String) As String
        Return DoTranslation(text, currentLang)
    End Function

    ''' <summary>
    ''' Translates string into another language, or to English if the language wasn't specified or if it's invalid.
    ''' </summary>
    ''' <param name="text">Any string that exists in Kernel Simulator's translation files</param>
    ''' <param name="lang">3 letter language</param>
    ''' <returns>Translated string</returns>
    Public Function DoTranslation(ByVal text As String, ByVal lang As String) As String
        If String.IsNullOrWhiteSpace(lang) Then lang = "eng"
        'Get language string and translate
        Dim translatedString As Dictionary(Of String, String)

        'If the language is available and is not English, translate
        If Languages.ContainsKey(lang) And lang <> "eng" Then
            'Prepare dictionary
            translatedString = PrepareDict(lang)
            Wdbg("I", "Dictionary size: {0}", translatedString.Count)

            'Do translation
            If translatedString.Keys.Contains(text) Then
                Wdbg("I", "Translating string to {0}: {1}", lang, text)
                Return translatedString(text)
            Else 'String wasn't found
                Wdbg("W", "No string found in langlist. Lang: {0}, String: {1}", lang, text)
                text = "(( " + text + " ))"
                Return text
            End If
        ElseIf Languages.ContainsKey(lang) And lang = "eng" Then 'If the language is available, but is English, don't translate
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

        'Move final translations to dictionary
        For Each TranslatedProperty As JProperty In Languages(lang).LanguageResource.Properties
            langStrings.Add(TranslatedProperty.Name, TranslatedProperty.Value)
        Next
        Return langStrings
    End Function

    ''' <summary>
    ''' Prompt for setting language
    ''' </summary>
    ''' <param name="lang">A specified language</param>
    ''' <param name="Force">Force changes</param>
    Sub PromptForSetLang(ByVal lang As String, Optional ByVal Force As Boolean = False)
        If Languages.ContainsKey(lang) Then
            Wdbg("I", "Forced {0}", Force)
            If Not Force Then
                If lang.EndsWith("-T") Then 'The condition prevents tricksters from using "chlang <lang>-T", if not forced.
                    Wdbg("W", "Trying to bypass prompt.")
                    Exit Sub
                Else
                    'Check to see if the language is transliterable
                    Wdbg("I", "Transliterable? {0}", Languages(lang).Transliterable)
                    If Languages(lang).Transliterable Then
                        W(DoTranslation("The language you've selected contains two variants. Select one:") + vbNewLine, True, ColTypes.Neutral)
                        W(DoTranslation("1. Transliterated", lang), True, ColTypes.Neutral)
                        W(DoTranslation("2. Translated", lang + "-T") + vbNewLine, True, ColTypes.Neutral)
CHOICE:
                        W(DoTranslation("Select your choice:"), False, ColTypes.Input)
                        Dim cho As String = Console.ReadKey(True).KeyChar
                        Console.WriteLine()
                        Wdbg("I", "Choice: {0}", cho)
                        If cho = "2" Then
                            lang += "-T"
                        ElseIf Not cho = "1" Then
                            W(DoTranslation("Invalid choice. Try again."), True, ColTypes.Error)
                            GoTo CHOICE
                        End If
                    End If
                End If
            End If

            W(DoTranslation("Changing from: {0} to {1}..."), True, ColTypes.Neutral, currentLang, lang)
            If Not SetLang(lang) Then
                W(DoTranslation("Failed to set language."), True, ColTypes.Error)
            End If
            If NotifyCodepageError Then
                W(DoTranslation("Unable to set codepage. The language may not display properly."), True, ColTypes.Error)
            End If
        Else
            W(DoTranslation("Invalid language") + " {0}", True, ColTypes.Error, lang)
        End If
    End Sub

    ''' <summary>
    ''' Sets a system language permanently
    ''' </summary>
    ''' <param name="lang">A specified language</param>
    ''' <returns>True if successful, False if unsuccessful.</returns>
    Public Function SetLang(ByVal lang As String) As Boolean
        If Languages.ContainsKey(lang) Then
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
                Dim OldModDescGeneric As String = DoTranslation("Command defined by ")
                Wdbg("I", "Translating kernel to {0}.", lang)
                currentLang = lang
                ConfigToken("General")("Language") = currentLang
                File.WriteAllText(paths("Configuration"), JsonConvert.SerializeObject(ConfigToken, Formatting.Indented))
                Wdbg("I", "Saved new language.")

                'Update help list for translated help
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
            Throw New Exceptions.NoSuchLanguageException(DoTranslation("Invalid language") + " {0}", lang)
        End If
        Return False
    End Function

    ''' <summary>
    ''' Updates current culture based on current language. If there are no cultures in the curent language, assume current culture.
    ''' </summary>
    Public Sub UpdateCulture()
        Dim StrCult As String = If(Not GetCulturesFromCurrentLang.Count = 0, GetCulturesFromCurrentLang(0).EnglishName, CultureInfo.CurrentCulture.EnglishName)
        Wdbg("I", "Culture for {0} is {1}", currentLang, StrCult)
        Dim Cults As CultureInfo() = CultureInfo.GetCultures(CultureTypes.AllCultures)
        Wdbg("I", "Parsing {0} cultures for {1}", Cults.Length, StrCult)
        For Each Cult As CultureInfo In Cults
            If Cult.EnglishName = StrCult Then
                Wdbg("I", "Found. Changing culture...")
                CurrentCult = Cult
                ConfigToken("General")("Culture") = CurrentCult.Name
                File.WriteAllText(paths("Configuration"), JsonConvert.SerializeObject(ConfigToken, Formatting.Indented))
                Wdbg("I", "Saved new culture.")
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Updates current culture based on current language and custom culture
    ''' </summary>
    ''' <param name="Culture">Full culture name</param>
    Public Sub UpdateCulture(ByVal Culture As String)
        Dim Cultures As List(Of CultureInfo) = GetCulturesFromCurrentLang()
        For Each Cult As CultureInfo In Cultures
            If Cult.EnglishName = Culture Then
                Wdbg("I", "Found. Changing culture...")
                CurrentCult = Cult
                ConfigToken("General")("Culture") = CurrentCult.Name
                File.WriteAllText(paths("Configuration"), JsonConvert.SerializeObject(ConfigToken, Formatting.Indented))
                Wdbg("I", "Saved new culture.")
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Gets all cultures available for the current language
    ''' </summary>
    Public Function GetCulturesFromCurrentLang() As List(Of CultureInfo)
        Return Languages(currentLang).Cultures
    End Function

End Module
