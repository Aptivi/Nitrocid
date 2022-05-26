
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

<TestFixture> Public Class LocalizationQueryingTests

    ''' <summary>
    ''' Tests getting cultures from current language
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestGetCulturesFromCurrentLang()
        GetCulturesFromCurrentLang.ShouldNotBeNull
        GetCulturesFromCurrentLang.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests getting cultures from specific language
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestGetCulturesFromLang()
        GetCulturesFromLang("spa").ShouldNotBeNull
        GetCulturesFromLang("spa").ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests getting cultures from specific language
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestListLanguages()
        ListLanguages("arb").ShouldNotBeNull
        ListLanguages("arb").ShouldNotBeEmpty
        ListLanguages("arb").Count.ShouldBe(2)
    End Sub

End Class