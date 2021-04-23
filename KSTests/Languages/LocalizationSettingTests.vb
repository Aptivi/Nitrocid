
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

Imports KS

<TestClass()> Public Class LocalizationSettingTests

    ''' <summary>
    ''' Tests updating the culture
    ''' </summary>
    <TestMethod> <TestCategory("Setting")> Public Sub TestUpdateCulture()
        currentLang = "spa"
        Dim ExpectedCulture As String = "Spanish (Spain, International Sort)"
        UpdateCulture()
        Assert.AreEqual(ExpectedCulture, CurrentCult.EnglishName, "Culture update test is not done properly. Got {0}", CurrentCult.EnglishName)
    End Sub

    ''' <summary>
    ''' Tests language setting
    ''' </summary>
    <TestMethod> <TestCategory("Setting")> Public Sub TestSetLang()
        InitPaths()
        Assert.IsTrue(SetLang("spa"), "Setting language failed. Returned False.")
    End Sub

End Class