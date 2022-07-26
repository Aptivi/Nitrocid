
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
Imports KS.Shell.ShellBase.Commands

<TestFixture> Public Class CommandManagerActionTests

    ''' <summary>
    ''' Tests seeing if the command is found in specific shell (test case: Normal shell, help command)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestIsCommandFoundInSpecificShell()
        IsCommandFound("help", ShellType.Shell).ShouldBeTrue
    End Sub

    ''' <summary>
    ''' Tests seeing if the command is found in all the shells (test case: connect command)
    ''' </summary>
    <Test, Description("Action")> Public Sub TestIsCommandFoundInAllTheShells()
        IsCommandFound("connect").ShouldBeTrue
    End Sub

End Class