
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

Imports KS.Shell.ShellBase.Shells

<TestFixture> Public Class ShellExecutorInitializationTests

    Shared ShellInstance As ShellExecutor

    ''' <summary>
    ''' Tests initializing the shell instance from base
    ''' </summary>
    <Test, Description("Initialization"), SetUp> Public Sub TestInitializeShellExecutorFromBase()
        'Create instance
        ShellInstance = New ShellTest()

        'Check for null
        ShellInstance.ShouldNotBeNull
    End Sub

    ''' <summary>
    ''' Tests initializing the shell instance from base
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializedShellExecution()
        Should.NotThrow(New Action(Sub() ShellInstance.InitializeShell()))
        ShellInstance.Bail.ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests initializing the shell instance from base
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializedShellExecutionWithArguments()
        Should.NotThrow(New Action(Sub() ShellInstance.InitializeShell("Hello", "World")))
        ShellInstance.Bail.ShouldBeTrue
    End Sub

End Class