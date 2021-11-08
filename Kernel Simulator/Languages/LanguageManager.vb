
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

Imports Newtonsoft.Json.Linq

Public Module LanguageManager

    'PLEASE NOTE: "zul" language is Zulu and "swa" is Swahili for compatibility with Windows and Linux platforms. Windows considers "zul" as
    '             isiZulu and "swa" as Kiswahili, while Linux considers "zul" as Zulu and "swa" as Swahili.
    Public ReadOnly Languages As New Dictionary(Of String, LanguageInfo) From {{"arb", New LanguageInfo("arb", "Arabic", True)}, {"arb-T", New LanguageInfo("arb-T", "Arabic", True)},
                                                                               {"azr", New LanguageInfo("azr", "Azerbaijani", False)},
                                                                               {"ben", New LanguageInfo("ben", "Bangla", True)}, {"ben-T", New LanguageInfo("ben-T", "Bangla", True)},
                                                                               {"bsn", New LanguageInfo("bsq", "Bosnian", False)},
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
                                                                               {"guj", New LanguageInfo("guj", "Gujarati", True)}, {"guj-T", New LanguageInfo("ind-T", "Gujarati", True)},
                                                                               {"hwi", New LanguageInfo("hwi", "Hawaiian", False)},
                                                                               {"ind", New LanguageInfo("ind", "Hindi", True)}, {"ind-T", New LanguageInfo("ind-T", "Hindi", True)},
                                                                               {"iri", New LanguageInfo("iri", "Irish", False)},
                                                                               {"ita", New LanguageInfo("ita", "Italian", False)},
                                                                               {"jpn", New LanguageInfo("jpn", "Japanese", True)}, {"jpn-T", New LanguageInfo("jpn-T", "Japanese", True)},
                                                                               {"jvn", New LanguageInfo("jvn", "Javanese", False)},
                                                                               {"kor", New LanguageInfo("kor", "Korean", True)}, {"kor-T", New LanguageInfo("kor-T", "Korean", True)},
                                                                               {"kzk", New LanguageInfo("kzk", "Kazakh", True)}, {"kzk-T", New LanguageInfo("kzk-T", "Kazakh", True)},
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
                                                                               {"swa", New LanguageInfo("swa", "Swahili", False)},
                                                                               {"swe", New LanguageInfo("swe", "Swedish", False)},
                                                                               {"tky", New LanguageInfo("tky", "Turkish", False)},
                                                                               {"uzb", New LanguageInfo("uzb", "Uzbek", False)},
                                                                               {"vtn", New LanguageInfo("vtn", "Vietnamese", False)},
                                                                               {"wls", New LanguageInfo("wls", "Welsh", False)},
                                                                               {"yrb", New LanguageInfo("yrb", "Yoruba", False)},
                                                                               {"zul", New LanguageInfo("zul", "Zulu", False)}}

    'Variables
    Public CurrentLanguage As String = "eng" 'Default to English
    Private NotifyCodepageError As Boolean

    ''' <summary>
    ''' Sets a system language permanently
    ''' </summary>
    ''' <param name="lang">A specified language</param>
    ''' <returns>True if successful, False if unsuccessful.</returns>
    Public Function SetLang(lang As String) As Boolean
        If Languages.ContainsKey(lang) Then
            'Set appropriate codepage for incapable terminals
            Try
                Select Case lang
                    Case "arb-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(1256)
                        Console.InputEncoding = Text.Encoding.GetEncoding(1256)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Arabic to {0}.", Console.OutputEncoding.EncodingName)
                    Case "ben-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(57003)
                        Console.InputEncoding = Text.Encoding.GetEncoding(57003)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Bengali to {0}.", Console.OutputEncoding.EncodingName)
                    Case "chi-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(936)
                        Console.InputEncoding = Text.Encoding.GetEncoding(936)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Chinese to {0}.", Console.OutputEncoding.EncodingName)
                    Case "jpn-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(932)
                        Console.InputEncoding = Text.Encoding.GetEncoding(932)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Japanese to {0}.", Console.OutputEncoding.EncodingName)
                    Case "kor-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(949)
                        Console.InputEncoding = Text.Encoding.GetEncoding(949)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Korean to {0}.", Console.OutputEncoding.EncodingName)
                    Case "pun-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(57011)
                        Console.InputEncoding = Text.Encoding.GetEncoding(57011)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Punjabi to {0}.", Console.OutputEncoding.EncodingName)
                    Case "rus-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(866)
                        Console.InputEncoding = Text.Encoding.GetEncoding(866)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Russian to {0}.", Console.OutputEncoding.EncodingName)
                    Case "srb-T"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(21025)
                        Console.InputEncoding = Text.Encoding.GetEncoding(21025)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Serbian to {0}.", Console.OutputEncoding.EncodingName)
                    Case "dan"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(865)
                        Console.InputEncoding = Text.Encoding.GetEncoding(865)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Danish to {0}.", Console.OutputEncoding.EncodingName)
                    Case "vtn"
                        Console.OutputEncoding = Text.Encoding.GetEncoding(1258)
                        Console.InputEncoding = Text.Encoding.GetEncoding(1258)
                        Wdbg(DebugLevel.I, "Encoding set successfully for Vietnamese to {0}.", Console.OutputEncoding.EncodingName)
                    Case Else
                        Console.OutputEncoding = Text.Encoding.GetEncoding(65001)
                        Console.InputEncoding = Text.Encoding.GetEncoding(65001)
                        Wdbg(DebugLevel.I, "Encoding set successfully to {0}.", Console.OutputEncoding.EncodingName)
                End Select
            Catch ex As Exception
                NotifyCodepageError = True
                Wdbg(DebugLevel.W, "Codepage can't be set. {0}", ex.Message)
                WStkTrc(ex)
            End Try

            'Set current language
            Try
                Dim OldModDescGeneric As String = DoTranslation("Command defined by ")
                Wdbg(DebugLevel.I, "Translating kernel to {0}.", lang)
                CurrentLanguage = lang
                Dim Token As JToken = GetConfigCategory(ConfigCategory.General)
                SetConfigValue(ConfigCategory.General, Token, "Language", CurrentLanguage)
                Wdbg(DebugLevel.I, "Saved new language.")

                'Update help list for translated help
                ReloadGenericDefs(OldModDescGeneric)

                'Update Culture if applicable
                If LangChangeCulture Then
                    Wdbg(DebugLevel.I, "Updating culture.")
                    UpdateCulture()
                End If
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.W, "Language can't be set. {0}", ex.Message)
                WStkTrc(ex)
            End Try
        Else
            Throw New Exceptions.NoSuchLanguageException(DoTranslation("Invalid language") + " {0}", lang)
        End If
        Return False
    End Function

    ''' <summary>
    ''' Prompt for setting language
    ''' </summary>
    ''' <param name="lang">A specified language</param>
    ''' <param name="Force">Force changes</param>
    Sub PromptForSetLang(lang As String, Optional Force As Boolean = False, Optional AlwaysTransliterated As Boolean = False, Optional AlwaysTranslated As Boolean = False)
        If Languages.ContainsKey(lang) Then
            Wdbg(DebugLevel.I, "Forced {0}", Force)
            If Not Force Then
                If lang.EndsWith("-T") Then 'The condition prevents tricksters from using "chlang <lang>-T", if not forced.
                    Wdbg(DebugLevel.W, "Trying to bypass prompt.")
                    Exit Sub
                Else
                    'Check to see if the language is transliterable
                    Wdbg(DebugLevel.I, "Transliterable? {0}", Languages(lang).Transliterable)
                    If Languages(lang).Transliterable Then
                        If AlwaysTransliterated Then
                            lang.RemovePostfix("-T")
                        ElseIf AlwaysTranslated Then
                            If Not lang.EndsWith("-T") Then lang += "-T"
                        Else
                            Write(DoTranslation("The language you've selected contains two variants. Select one:") + vbNewLine, True, ColTypes.Neutral)
                            Write(" 1) " + DoTranslation("Transliterated version", lang), True, ColTypes.Option)
                            Write(" 2) " + DoTranslation("Translated version", lang + "-T") + vbNewLine, True, ColTypes.Option)
                            Dim LanguageSet As Boolean
                            While Not LanguageSet
                                Write(">> ", False, ColTypes.Input)
                                Dim Answer As Integer
                                If Integer.TryParse(Console.ReadLine, Answer) Then
                                    Wdbg(DebugLevel.I, "Choice: {0}", Answer)
                                    Select Case Answer
                                        Case 1, 2
                                            If Answer = 2 Then lang += "-T"
                                            LanguageSet = True
                                        Case Else
                                            Write(DoTranslation("Invalid choice. Try again."), True, ColTypes.Error)
                                    End Select
                                Else
                                    Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                                End If
                            End While
                        End If
                    End If
                End If
            End If

            Write(DoTranslation("Changing from: {0} to {1}..."), True, ColTypes.Neutral, CurrentLanguage, lang)
            If Not SetLang(lang) Then
                Write(DoTranslation("Failed to set language."), True, ColTypes.Error)
            End If
            If NotifyCodepageError Then
                Write(DoTranslation("Unable to set codepage. The language may not display properly."), True, ColTypes.Error)
            End If
        Else
            Write(DoTranslation("Invalid language") + " {0}", True, ColTypes.Error, lang)
        End If
    End Sub

End Module
