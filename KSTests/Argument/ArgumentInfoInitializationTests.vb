
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

<TestClass()> Public Class ArgumentInfoInitializationTests

    ''' <summary>
    ''' Tests initializing ArgumentInfo instance from a command line argument
    ''' </summary>
    <TestMethod()> <TestCategory("Initialization")> Public Sub TestInitializeArgumentInfoInstanceFromCommandLineArg()
        'Create instance
        Dim ColorInstance As New ArgumentInfo("help", ArgumentType.CommandLineArgs, "Help page", False, 0)

        'Check for null
        ColorInstance.ShouldNotBeNull
        ColorInstance.Argument.ShouldNotBeNullOrEmpty
        ColorInstance.HelpDefinition.ShouldNotBeNullOrEmpty

        'Check for property correctness
        ColorInstance.Argument.ShouldBe("help")
        ColorInstance.ArgumentsRequired.ShouldBeFalse
        ColorInstance.HelpDefinition.ShouldBe("Help page")
        ColorInstance.MinimumArguments.ShouldBe(0)
        ColorInstance.Obsolete.ShouldBeFalse
        ColorInstance.Type.ShouldBe(ArgumentType.CommandLineArgs)
    End Sub

End Class