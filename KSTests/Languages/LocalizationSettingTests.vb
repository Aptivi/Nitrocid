
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

Imports KS.Languages

<TestFixture> Public Class LocalizationSettingTests

    ''' <summary>
    ''' Tests updating the culture
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestUpdateCulture()
        CurrentLanguage = "spa"
        Dim ExpectedCulture As String = "Spanish"
        UpdateCulture()
        CurrentCult.EnglishName.ShouldBe(ExpectedCulture)
    End Sub

    ''' <summary>
    ''' Tests updating the culture using custom culture
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestUpdateCultureCustom()
        CurrentLanguage = "spa"
        Dim ExpectedCulture As String = "Spanish (Spain, International Sort)"
        UpdateCulture(ExpectedCulture)
        CurrentCult.EnglishName.ShouldBe(ExpectedCulture)
    End Sub

    ''' <summary>
    ''' Tests language setting
    ''' </summary>
    <Test, Description("Setting")> Public Sub TestSetLang()
        SetLang("spa").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Restores the language
    ''' </summary>
    <TearDown> Public Sub RestoreLanguage()
        SetLang("eng")
    End Sub

End Class