
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

Imports System.IO
Imports Newtonsoft.Json.Linq
Imports KS.Files.Querying
Imports KS.Files.Folders
Imports KS.Misc.Configuration

Namespace Languages
    Public Module LanguageManager

        'Variables
        Public CurrentLanguage As String = "eng" 'Default to English
        Friend BaseLanguages As New Dictionary(Of String, LanguageInfo)
        Friend CustomLanguages As New Dictionary(Of String, LanguageInfo)
        Private NotifyCodepageError As Boolean
        Private ReadOnly LanguageMetadata As JToken = JToken.Parse(My.Resources.LanguageMetadata)

        ''' <summary>
        ''' The installed languages list.
        ''' </summary>
        Public ReadOnly Property Languages As Dictionary(Of String, LanguageInfo)
            Get
                Dim InstalledLanguages As New Dictionary(Of String, LanguageInfo)

                'For each language, get information for localization and cache them
                For Each Language As JToken In LanguageMetadata
                    Dim LanguageName As String = Language.Path
                    Dim LanguageFullName As String = Language.First.SelectToken("name")
                    Dim LanguageTransliterable As Boolean = Language.First.SelectToken("transliterable")

                    'If the language is not found in the base languages cache dictionary, add it
                    If Not BaseLanguages.ContainsKey(LanguageName) Then
                        Dim LanguageInfo As New LanguageInfo(LanguageName, LanguageFullName, LanguageTransliterable)
                        BaseLanguages.AddIfNotFound(LanguageName, LanguageInfo)
                    End If
                Next

                'Add the base languages to the final dictionary
                For Each BaseLanguage As String In BaseLanguages.Keys
                    InstalledLanguages.Add(BaseLanguage, BaseLanguages(BaseLanguage))
                Next

                'Now, get the custom languages and add them to the languages list
                For Each CustomLanguage As String In CustomLanguages.Keys
                    InstalledLanguages.Add(CustomLanguage, CustomLanguages(CustomLanguage))
                Next

                'Return the list
                Return InstalledLanguages
            End Get
        End Property

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
                        Case "chi-T"
                            Console.OutputEncoding = Text.Encoding.GetEncoding(936)
                            Console.InputEncoding = Text.Encoding.GetEncoding(936)
                            Wdbg(DebugLevel.I, "Encoding set successfully for Chinese to {0}.", Console.OutputEncoding.EncodingName)
                        Case "jpn"
                            Console.OutputEncoding = Text.Encoding.GetEncoding(932)
                            Console.InputEncoding = Text.Encoding.GetEncoding(932)
                            Wdbg(DebugLevel.I, "Encoding set successfully for Japanese to {0}.", Console.OutputEncoding.EncodingName)
                        Case "kor-T"
                            Console.OutputEncoding = Text.Encoding.GetEncoding(949)
                            Console.InputEncoding = Text.Encoding.GetEncoding(949)
                            Wdbg(DebugLevel.I, "Encoding set successfully for Korean to {0}.", Console.OutputEncoding.EncodingName)
                        Case "rus-T"
                            Console.OutputEncoding = Text.Encoding.GetEncoding(866)
                            Console.InputEncoding = Text.Encoding.GetEncoding(866)
                            Wdbg(DebugLevel.I, "Encoding set successfully for Russian to {0}.", Console.OutputEncoding.EncodingName)
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
                                Write(DoTranslation("The language you've selected contains two variants. Select one:") + NewLine, True, ColTypes.Neutral)
                                Write(" 1) " + DoTranslation("Transliterated version", lang), True, ColTypes.Option)
                                Write(" 2) " + DoTranslation("Translated version", lang + "-T") + NewLine, True, ColTypes.Option)
                                Dim LanguageSet As Boolean
                                While Not LanguageSet
                                    Write(">> ", False, ColTypes.Input)
                                    Dim Answer As Integer
                                    Dim AnswerString As String = ReadLine(False)
                                    If Integer.TryParse(AnswerString, Answer) Then
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

                'Gangsta language contains strong language, so warn the user before setting
                If lang = "pla" Then
                    Write(DoTranslation("The gangsta language contains strong language that may make you feel uncomfortable reading it. Are you sure that you want to set the language anyways?"), True, ColTypes.Warning)
                    If Console.ReadKey(True).Key <> ConsoleKey.Y Then
                        Exit Sub
                    End If
                End If

                'Now, set the language!
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

        ''' <summary>
        ''' Installs the custom language to the installed languages
        ''' </summary>
        ''' <param name="LanguageName">The custom three-letter language name found in KSLanguages directory</param>
        Public Sub InstallCustomLanguage(LanguageName As String, Optional ThrowOnAlreadyInstalled As Boolean = True)
            If Not SafeMode Then
                Try
                    Dim LanguagePath As String = GetKernelPath(KernelPathType.CustomLanguages) + LanguageName + ".json"
                    If FileExists(LanguagePath) Then
                        Wdbg(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath)

                        'Check the metadata to see if it has relevant information for the language
                        Dim MetadataToken As JToken = JObject.Parse(File.ReadAllText(LanguagePath))
                        Wdbg(DebugLevel.I, "MetadataToken is null: {0}", MetadataToken Is Nothing)
                        If MetadataToken IsNot Nothing Then
                            Wdbg(DebugLevel.I, "Metadata exists!")

                            'Parse the values and install the language
                            Dim ParsedLanguageName As String = If(MetadataToken.SelectToken("Name"), LanguageName)
                            Dim ParsedLanguageTransliterable As Boolean = If(MetadataToken.SelectToken("Transliterable"), False)
                            Dim ParsedLanguageLocalizations As JToken = MetadataToken.SelectToken("Localizations")
                            Wdbg(DebugLevel.I, "Metadata says: Name: {0}, Transliterable: {1}", ParsedLanguageName, ParsedLanguageTransliterable)

                            'Check the localizations...
                            Wdbg(DebugLevel.I, "Checking localizations... (Null: {0})", ParsedLanguageLocalizations Is Nothing)
                            If ParsedLanguageLocalizations IsNot Nothing Then
                                Wdbg(DebugLevel.I, "Valid localizations found! Length: {0}", ParsedLanguageLocalizations.Count)

                                'Try to install the language info
                                Dim ParsedLanguageInfo As New LanguageInfo(LanguageName, ParsedLanguageName, ParsedLanguageTransliterable, ParsedLanguageLocalizations)
                                Wdbg(DebugLevel.I, "Made language info! Checking for existence... (Languages.ContainsKey returns {0})", Languages.ContainsKey(LanguageName))
                                If Not Languages.ContainsKey(LanguageName) Then
                                    Wdbg(DebugLevel.I, "Language exists. Installing...")
                                    CustomLanguages.Add(LanguageName, ParsedLanguageInfo)
                                    KernelEventManager.RaiseLanguageInstalled(LanguageName)
                                ElseIf ThrowOnAlreadyInstalled Then
                                    Wdbg(DebugLevel.E, "Can't add existing language.")
                                    Throw New Exceptions.LanguageInstallException(DoTranslation("The language already exists and can't be overwritten."))
                                End If
                            Else
                                Wdbg(DebugLevel.E, "Metadata doesn't contain valid localizations!")
                                Throw New Exceptions.LanguageInstallException(DoTranslation("The metadata information needed to install the custom language doesn't provide the necessary localizations needed."))
                            End If
                        Else
                            Wdbg(DebugLevel.E, "Metadata for language doesn't exist!")
                            Throw New Exceptions.LanguageInstallException(DoTranslation("The metadata information needed to install the custom language doesn't exist."))
                        End If
                    End If
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to install custom language {0}: {1}", LanguageName, ex.Message)
                    WStkTrc(ex)
                    KernelEventManager.RaiseLanguageInstallError(LanguageName, ex)
                    Throw New Exceptions.LanguageInstallException(DoTranslation("Failed to install custom language {0}."), ex, LanguageName)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Installs all the custom languages found in KSLanguages
        ''' </summary>
        Public Sub InstallCustomLanguages()
            If Not SafeMode Then
                Try
                    'Enumerate all the JSON files generated by KSJsonifyLocales
                    For Each Language As String In GetFilesystemEntries(GetKernelPath(KernelPathType.CustomLanguages), "*.json")
                        'Install a custom language
                        Dim LanguageName As String = Path.GetFileNameWithoutExtension(Language)
                        InstallCustomLanguage(LanguageName, False)
                    Next
                    KernelEventManager.RaiseLanguagesInstalled()
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to install custom languages: {0}", ex.Message)
                    WStkTrc(ex)
                    KernelEventManager.RaiseLanguagesInstallError(ex)
                    Throw New Exceptions.LanguageInstallException(DoTranslation("Failed to install custom languages."), ex)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Uninstalls the custom language to the installed languages
        ''' </summary>
        ''' <param name="LanguageName">The custom three-letter language name found in KSLanguages directory</param>
        Public Sub UninstallCustomLanguage(LanguageName As String)
            If Not SafeMode Then
                Try
                    Dim LanguagePath As String = GetKernelPath(KernelPathType.CustomLanguages) + LanguageName + ".json"
                    If FileExists(LanguagePath) Then
                        Wdbg(DebugLevel.I, "Language {0} exists in {1}", LanguageName, LanguagePath)

                        'Now, check the metadata to see if it has relevant information for the language
                        Dim MetadataToken As JToken = JObject.Parse(File.ReadAllText(LanguagePath))
                        Wdbg(DebugLevel.I, "MetadataToken is null: {0}", MetadataToken Is Nothing)
                        If MetadataToken IsNot Nothing Then
                            Wdbg(DebugLevel.I, "Metadata exists!")

                            'Uninstall the language
                            If Not CustomLanguages.Remove(LanguageName) Then
                                Wdbg(DebugLevel.E, "Failed to uninstall custom language")
                                Throw New Exceptions.LanguageUninstallException(DoTranslation("Failed to uninstall custom language. It most likely doesn't exist."))
                            End If
                            KernelEventManager.RaiseLanguageUninstalled(LanguageName)
                        Else
                            Wdbg(DebugLevel.E, "Metadata for language doesn't exist!")
                            Throw New Exceptions.LanguageUninstallException(DoTranslation("The metadata information needed to uninstall the custom language doesn't exist."))
                        End If
                    End If
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to uninstall custom language {0}: {1}", LanguageName, ex.Message)
                    WStkTrc(ex)
                    KernelEventManager.RaiseLanguageUninstallError(LanguageName, ex)
                    Throw New Exceptions.LanguageUninstallException(DoTranslation("Failed to uninstall custom language {0}."), ex, LanguageName)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Uninstalls all the custom languages found in KSLanguages
        ''' </summary>
        Public Sub UninstallCustomLanguages()
            If Not SafeMode Then
                Try
                    'Enumerate all the installed languages and query for the custom status to uninstall the custom languages
                    For LanguageIndex As Integer = Languages.Count - 1 To 0
                        Dim Language As String = Languages.Keys(LanguageIndex)
                        Dim LanguageInfo As LanguageInfo = Languages(Language)

                        'Check the status
                        If LanguageInfo.Custom Then
                            'Actually uninstall
                            If Not CustomLanguages.Remove(Language) Then
                                Wdbg(DebugLevel.E, "Failed to uninstall custom languages")
                                Throw New Exceptions.LanguageUninstallException(DoTranslation("Failed to uninstall custom languages."))
                            End If
                        End If
                        KernelEventManager.RaiseLanguagesUninstalled()
                    Next
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to uninstall custom languages: {0}", ex.Message)
                    WStkTrc(ex)
                    KernelEventManager.RaiseLanguagesUninstallError(ex)
                    Throw New Exceptions.LanguageUninstallException(DoTranslation("Failed to uninstall custom languages. See the inner exception for more info."), ex)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Lists the languages
        ''' </summary>
        ''' <param name="SearchTerm">Search term</param>
        Public Function ListLanguages(SearchTerm As String) As Dictionary(Of String, LanguageInfo)
            Dim ListedLanguages As New Dictionary(Of String, LanguageInfo)

            'List the Languages using the search term
            For Each LanguageName As String In Languages.Keys
                If LanguageName.Contains(SearchTerm) Then
                    ListedLanguages.Add(LanguageName, Languages(LanguageName))
                End If
            Next
            Return ListedLanguages
        End Function

    End Module
End Namespace