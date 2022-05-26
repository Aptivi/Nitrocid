
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

<TestFixture> Public Class CommandManagerQueryingTests

    ''' <summary>
    ''' Tests getting list of commands from specific shell type
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestGetCommandListFromSpecificShell()
        Dim Commands As Dictionary(Of String, CommandInfo) = GetCommands(ShellType.Shell)
        Debug.WriteLine(format:="Commands from Shell: {0} commands", Commands.Count)
        Debug.WriteLine(format:=String.Join(", ", Commands))
        Commands.ShouldNotBeNull
        Commands.ShouldNotBeEmpty
    End Sub

    ''' <summary>
    ''' Tests getting list of commands from all shells
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestGetCommandListFromAllShells()
        For Each ShellTypeName As String In [Enum].GetNames(GetType(ShellType))
            Dim Commands As Dictionary(Of String, CommandInfo) = GetCommands([Enum].Parse(GetType(ShellType), ShellTypeName))
            Debug.WriteLine(format:="Commands from {0}: {1} commands", ShellTypeName, Commands.Count)
            Debug.WriteLine(format:=String.Join(", ", Commands))
            Commands.ShouldNotBeNull
            Commands.ShouldNotBeEmpty
        Next
    End Sub

End Class