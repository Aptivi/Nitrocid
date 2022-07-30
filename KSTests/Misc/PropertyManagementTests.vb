
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

Imports System.Reflection
Imports Figgle
Imports KS.Languages
Imports KS.Misc.Reflection

<TestFixture> Public Class PropertyManagementTests

    ''' <summary>
    ''' Tests checking field
    ''' </summary>
    <Test, Description("Management")> Public Sub TestCheckProperty()
        CheckProperty("PersonLookupDelay").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests getting value
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetPropertyValue()
        Dim Value As String = GetPropertyValue("PersonLookupDelay")
        Value.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests setting value
    ''' </summary>
    <Test, Description("Management")> Public Sub TestSetPropertyValue()
        SetPropertyValue("PersonLookupDelay", 100)
        Dim Value As Integer = GetPropertyValue("PersonLookupDelay")
        Value.ShouldBe(100)
    End Sub

    ''' <summary>
    ''' Tests getting variable
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetConfigProperty()
        Dim PropertyInfo As PropertyInfo = GetProperty("PersonLookupDelay")
        PropertyInfo.Name.ShouldBe("PersonLookupDelay")
    End Sub

    ''' <summary>
    ''' Tests getting properties
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetProperties()
        Dim Properties As Dictionary(Of String, Object) = GetProperties(GetType(FiggleFonts))
        Properties.ShouldNotBeNull
        Properties.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests getting properties
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetPropertiesNoEvaluation()
        Dim Properties As Dictionary(Of String, Type) = GetPropertiesNoEvaluation(GetType(FiggleFonts))
        Properties.ShouldNotBeNull
        Properties.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests getting property value from variable
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetPropertyValueInVariable()
        Dim Value As String = GetPropertyValueInVariable(NameOf(CurrentCult), NameOf(CurrentCult.Name))
        Value.ShouldNotBeNullOrEmpty
    End Sub

End Class