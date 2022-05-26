
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

Imports System.Reflection
Imports Figgle
Imports KS.Languages
Imports KS.Misc.Reflection

<TestFixture> Public Class FieldManagementTests

    ''' <summary>
    ''' Tests checking field
    ''' </summary>
    <Test, Description("Management")> Public Sub TestCheckField()
        CheckField("HiddenFiles").ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests getting value
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetValue()
        Dim Value As String = GetValue("HiddenFiles")
        Value.ShouldNotBeNullOrEmpty
    End Sub

    ''' <summary>
    ''' Tests setting value
    ''' </summary>
    <Test, Description("Management")> Public Sub TestSetValue()
        SetValue("HiddenFiles", False)
        Dim Value As String = GetValue("HiddenFiles")
        Value.ShouldBe("False")
    End Sub

    ''' <summary>
    ''' Tests getting variable
    ''' </summary>
    <Test, Description("Management")> Public Sub TestGetConfigField()
        Dim Field As FieldInfo = GetField("HiddenFiles")
        Field.Name.ShouldBe("HiddenFiles")
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