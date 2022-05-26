
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

Imports KS.Shell.ShellBase

<TestFixture> Public Class CommandInfoInitializationTests

    ''' <summary>
    ''' Tests initializing CommandInfo instance from a command line Command
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeCommandInfoInstanceFromCommandLineArg()
        'Create instance
        Dim CommandInstance As New CommandInfo("help", ShellType.Shell, "Help page", {""}, False, 0, Nothing)

        'Check for null
        CommandInstance.ShouldNotBeNull
        CommandInstance.Command.ShouldNotBeNullOrEmpty
        CommandInstance.HelpDefinition.ShouldNotBeNullOrEmpty
        CommandInstance.HelpUsages.ShouldNotBeNull

        'Check for property correctness
        CommandInstance.Command.ShouldBe("help")
        CommandInstance.HelpDefinition.ShouldBe("Help page")
        CommandInstance.HelpUsages.ShouldNotBeEmpty
        CommandInstance.Type.ShouldBe(ShellType.Shell)
        CommandInstance.Strict.ShouldBeFalse
        CommandInstance.Obsolete.ShouldBeFalse
        CommandInstance.Wrappable.ShouldBeFalse
        CommandInstance.NoMaintenance.ShouldBeFalse
        CommandInstance.SettingVariable.ShouldBeFalse
    End Sub

End Class