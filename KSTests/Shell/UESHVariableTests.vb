
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

<TestClass()> Public Class UESHVariableTests

    ''' <summary>
    ''' Tests initializing, setting, and getting $variable
    ''' </summary>
    <TestMethod> <TestCategory("Action")> Public Sub TestVariables()
        InitializeVariable("$test_var")
        ShellVariables.ShouldNotBeEmpty
        SetVariable("$test_var", "test").ShouldBeTrue
        ShellVariables("$test_var").ShouldBe("test")
        Dim ExpectedCommand As String = "echo test"
        Dim ActualCommand As String = GetVariableCommand("$test_var", "echo $test_var")
        ActualCommand.ShouldBe(ExpectedCommand)
        GetVariable("$test_var").ShouldBe("test")
    End Sub

End Class