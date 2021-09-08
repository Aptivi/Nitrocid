
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
Imports Newtonsoft.Json.Linq

Public Class LanguageInfo

    ''' <summary>
    ''' The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.
    ''' </summary>
    Public ReadOnly ThreeLetterLanguageName As String
    ''' <summary>
    ''' The full name of language without the country specifier.
    ''' </summary>
    Public ReadOnly FullLanguageName As String
    ''' <summary>
    ''' Whether or not the language is transliterable (Arabic, Korea, ...)
    ''' </summary>
    Public ReadOnly Transliterable As Boolean
    ''' <summary>
    ''' The localization information containing KS strings
    ''' </summary>
    Public ReadOnly LanguageResource As JObject
    ''' <summary>
    ''' List of cultures of language
    ''' </summary>
    Public ReadOnly Cultures As List(Of CultureInfo)

    ''' <summary>
    ''' Initializes the new instance of language information
    ''' </summary>
    ''' <param name="LangName">The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.</param>
    ''' <param name="FullLanguageName">The full name of language without the country specifier.</param>
    ''' <param name="Transliterable">Whether or not the language is transliterable (Arabic, Korea, ...)</param>
    Public Sub New(LangName As String, FullLanguageName As String, Transliterable As Boolean)
        'Check to see if the language being installed is found in resources
        If Not String.IsNullOrEmpty(My.Resources.ResourceManager.GetString(LangName.Replace("-", "_"))) Then
            'Install values to the object instance
            ThreeLetterLanguageName = LangName
            Me.FullLanguageName = FullLanguageName
            Me.Transliterable = Transliterable

            'Get all cultures associated with the language and install the parsed values. Additionally, it checks if the necessary cultures were added. If not,
            'the current culture is assumed.
            Dim Cults As CultureInfo() = CultureInfo.GetCultures(CultureTypes.AllCultures)
            Dim Cultures As New List(Of CultureInfo)
            For Each Cult As CultureInfo In Cults
                If Cult.EnglishName.ToLower.Contains(FullLanguageName.ToLower) Then
                    Cultures.Add(Cult)
                End If
            Next
            If Cultures.Count = 0 Then Cultures.Add(CultureInfo.CurrentCulture)
            Me.Cultures = Cultures

            'Get instance of langauge resource and install it
            Dim LanguageResource As JObject = JObject.Parse(My.Resources.ResourceManager.GetString(LangName.Replace("-", "_")))
            Me.LanguageResource = LanguageResource
        Else
            Throw New Exceptions.NoSuchLanguageException(DoTranslation("Invalid language") + " {0}", LangName)
        End If
    End Sub

End Class
