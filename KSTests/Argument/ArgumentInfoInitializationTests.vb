
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

Imports KS.Arguments.ArgumentBase

<TestFixture> Public Class ArgumentInfoInitializationTests

    Shared ArgumentInstance As ArgumentExecutor

    ''' <summary>
    ''' Tests initializing ArgumentInfo instance from a command line argument
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeArgumentInfoInstanceFromCommandLineArg()
        'Create instance
        Dim ArgumentInstance As New ArgumentInfo("help", ArgumentType.CommandLineArgs, "Help page", "", False, 0, Nothing)

        'Check for null
        ArgumentInstance.ShouldNotBeNull
        ArgumentInstance.Argument.ShouldNotBeNullOrEmpty
        ArgumentInstance.HelpDefinition.ShouldNotBeNullOrEmpty

        'Check for property correctness
        ArgumentInstance.Argument.ShouldBe("help")
        ArgumentInstance.ArgumentsRequired.ShouldBeFalse
        ArgumentInstance.HelpDefinition.ShouldBe("Help page")
        ArgumentInstance.MinimumArguments.ShouldBe(0)
        ArgumentInstance.Obsolete.ShouldBeFalse
        ArgumentInstance.Type.ShouldBe(ArgumentType.CommandLineArgs)
    End Sub

    ''' <summary>
    ''' Tests initializing the argument instance from base
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeArgumentFromBase()
        'Create instance
        ArgumentInstance = New ArgumentTest()

        'Check for null
        ArgumentInstance.ShouldNotBeNull
    End Sub

    ''' <summary>
    ''' Tests initializing the argument instance from base
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializedArgumentExecution()
        Should.NotThrow(New Action(Sub() ArgumentInstance.Execute("", Array.Empty(Of String), Array.Empty(Of String), Array.Empty(Of String))))
    End Sub

    ''' <summary>
    ''' Tests initializing the argument instance from base
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializedArgumentExecutionWithArguments()
        Should.NotThrow(New Action(Sub() ArgumentInstance.Execute("Hello World", {"Hello", "World"}, {"Hello", "World"}, Array.Empty(Of String))))
    End Sub

    ''' <summary>
    ''' Tests initializing the argument instance from base
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializedArgumentExecutionWithSwitches()
        Should.NotThrow(New Action(Sub() ArgumentInstance.Execute("-s", {"-s"}, Array.Empty(Of String), {"-s"})))
    End Sub

    ''' <summary>
    ''' Tests initializing the argument instance from base
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializedArgumentExecutionWithArgumentsAndSwitches()
        Should.NotThrow(New Action(Sub() ArgumentInstance.Execute("-s Hello!", {"-s", "Hello!"}, {"Hello!"}, {"-s"})))
    End Sub

End Class