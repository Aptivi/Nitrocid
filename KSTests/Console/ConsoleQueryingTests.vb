
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

Imports KS.ConsoleBase
Imports KS.ConsoleBase.Colors

<TestFixture> Public Class ConsoleQueryingTests

    ''' <summary>
    ''' Tests getting how many times to repeat the character to represent the appropriate percentage level for the specified number.
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestPercentRepeatTargeted()
        PercentRepeatTargeted(25, 200, 100).ShouldBe(12)
    End Sub

    ''' <summary>
    ''' Tests filtering the VT sequences that matches the regex
    ''' </summary>
    <Test, Description("Querying")> Public Sub TestFilterVTSequences()
        FilterVTSequences($"Hello!{GetEsc()}[38;5;43m").ShouldBe("Hello!")
    End Sub

End Class