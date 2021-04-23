
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

Imports System.Reflection
Imports KS

<TestClass()> Public Class FieldManagementTests

    ''' <summary>
    ''' Tests getting value
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestGetValue()
        Dim Value As String = GetConfigValue("HiddenFiles")
        Assert.IsNotNull(Value, "Value of variable HiddenFiles isn't get properly. Got null.")
    End Sub

    ''' <summary>
    ''' Tests setting value
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestSetValue()
        SetConfigValue("HiddenFiles", False)
        Dim Value As String = GetConfigValue("HiddenFiles")
        Assert.AreEqual(Value, "False", "Value of variable HiddenFiles isn't set properly. Got {0}", Value)
    End Sub

    ''' <summary>
    ''' Tests getting variable
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestGetConfigField()
        Dim Field As FieldInfo = GetConfigField("HiddenFiles")
        Assert.IsTrue(Field.Name = "HiddenFiles", "Field HiddenFiles isn't get properly. Name: {0}", Field.Name)
    End Sub

End Class