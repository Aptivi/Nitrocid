
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

Imports System.Globalization
Imports Newtonsoft.Json.Linq
Imports KS.Misc.Configuration

Namespace Languages
    Public Module CultureManager

        'Variables
        Public CurrentCult As New CultureInfo("en-US")

        ''' <summary>
        ''' Updates current culture based on current language. If there are no cultures in the curent language, assume current culture.
        ''' </summary>
        Public Sub UpdateCulture()
            Dim StrCult As String = If(Not GetCulturesFromCurrentLang.Count = 0, GetCulturesFromCurrentLang(0).EnglishName, CultureInfo.CurrentCulture.EnglishName)
            Wdbg(DebugLevel.I, "Culture for {0} is {1}", CurrentLanguage, StrCult)
            Dim Cults As CultureInfo() = CultureInfo.GetCultures(CultureTypes.AllCultures)
            Wdbg(DebugLevel.I, "Parsing {0} cultures for {1}", Cults.Length, StrCult)
            For Each Cult As CultureInfo In Cults
                If Cult.EnglishName = StrCult Then
                    Wdbg(DebugLevel.I, "Found. Changing culture...")
                    CurrentCult = Cult
                    Dim Token As JToken = GetConfigCategory(ConfigCategory.General)
                    SetConfigValue(ConfigCategory.General, Token, "Culture", CurrentCult.Name)
                    Wdbg(DebugLevel.I, "Saved new culture.")
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' Updates current culture based on current language and custom culture
        ''' </summary>
        ''' <param name="Culture">Full culture name</param>
        Public Sub UpdateCulture(Culture As String)
            Dim Cultures As List(Of CultureInfo) = GetCulturesFromCurrentLang()
            For Each Cult As CultureInfo In Cultures
                If Cult.EnglishName = Culture Then
                    Wdbg(DebugLevel.I, "Found. Changing culture...")
                    CurrentCult = Cult
                    Dim Token As JToken = GetConfigCategory(ConfigCategory.General)
                    SetConfigValue(ConfigCategory.General, Token, "Culture", CurrentCult.Name)
                    Wdbg(DebugLevel.I, "Saved new culture.")
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' Gets all cultures available for the current language
        ''' </summary>
        Public Function GetCulturesFromCurrentLang() As List(Of CultureInfo)
            Return Languages(CurrentLanguage).Cultures
        End Function

        ''' <summary>
        ''' Gets all cultures available for the current language
        ''' </summary>
        Public Function GetCulturesFromLang(Language As String) As List(Of CultureInfo)
            If Languages.ContainsKey(Language) Then
                Return Languages(CurrentLanguage).Cultures
            End If
            Return Nothing
        End Function

    End Module
End Namespace