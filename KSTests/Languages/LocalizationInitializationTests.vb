﻿
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

Imports KS
Imports Newtonsoft.Json.Linq
Imports KS.Languages

<TestClass()> Public Class LocalizationInitializationTests

    ''' <summary>
    ''' Tests translation dictionary preparation for a language
    ''' </summary>
    <TestMethod> <TestCategory("Initialization")> Public Sub TestPrepareDictForOneLanguage()
        Dim ExpectedLength As Integer = JObject.Parse(KS.My.Resources.spa).SelectToken("Localizations").Count
        Dim ActualLength As Integer = PrepareDict("spa").Values.Count
        ActualLength.ShouldBe(ExpectedLength)
    End Sub

    ''' <summary>
    ''' Tests translation dictionary preparation for all languages
    ''' </summary>
    <TestMethod> <TestCategory("Initialization")> Public Sub TestPrepareDictForAllLanguages()
        For Each Lang As String In Languages.Languages.Keys
            Dim ExpectedLength As Integer = JObject.Parse(KS.My.Resources.ResourceManager.GetString(Lang.Replace("-", "_"))).SelectToken("Localizations").Count
            Dim ActualLength As Integer = PrepareDict(Lang).Values.Count
            ActualLength.ShouldBe(ExpectedLength, $"Lang: {Lang}")
        Next
    End Sub

End Class