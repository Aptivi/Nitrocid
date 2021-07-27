
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

Imports System.IO
Imports System.ServiceModel.Security
Imports KS
Imports Newtonsoft.Json.Linq

<TestClass()> Public Class ConfigManagementTests

    ''' <summary>
    ''' Tests config repair (Actually, it checks to see if any of the config entries is missing. If any one of them is missing, unit test fails.)
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestRepairConfig()
        RepairConfig.ShouldBeFalse
    End Sub

    ''' <summary>
    ''' Tests getting a config category
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestGetConfigCategoryStandard()
        GetConfigCategory(ConfigCategory.General).ShouldNotBeNull
    End Sub

    ''' <summary>
    ''' Tests getting a config category with a sub-category
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestGetConfigCategoryWithSubcategory()
        GetConfigCategory(ConfigCategory.Screensaver, "Matrix").ShouldNotBeNull
    End Sub

    ''' <summary>
    ''' Tests setting the value of an entry in a category
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestSetConfigValueAndWriteStandard()
        Dim Token As JToken = GetConfigCategory(ConfigCategory.General)
        SetConfigValueAndWrite(ConfigCategory.General, Token, "Prompt for Arguments on Boot", True)
        Token("Prompt for Arguments on Boot").ToObject(Of Boolean).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests setting the value of an entry in a category with the sub-category
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestSetConfigValueAndWriteWithSubcategory()
        Dim Token As JToken = GetConfigCategory(ConfigCategory.Screensaver, "Matrix")
        SetConfigValueAndWrite(ConfigCategory.Screensaver, Token, "Delay in Milliseconds", 2)
        Token("Delay in Milliseconds").ToObject(Of Integer).ShouldBe(2)
    End Sub

End Class