
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

Imports KS.Misc.Reflection

<TestFixture> Public Class ReflectionActionTests

    ''' <summary>
    ''' Tests checking to see if the string is numeric
    ''' </summary>
    <Test, Description("Action")> Public Sub TestIsStringNumeric()
        IsStringNumeric("64").ShouldBeTrue
        IsStringNumeric("64.5").ShouldBeTrue
        IsStringNumeric("64-5").ShouldBeFalse
        IsStringNumeric("Alsalaam 3lekom").ShouldBeFalse
        IsStringNumeric("").ShouldBeFalse
    End Sub

    ''' <summary>
    ''' Tests formatting the string
    ''' </summary>
    <Test, Description("Action")> Public Sub TestFormatString()
        FormatString("Hello, {0}!", "Alex").ShouldBe("Hello, Alex!")
        FormatString("We have 0x{0:X2} faults!", 15).ShouldBe("We have 0x0F faults!")
        FormatString("Destroy {0 ships!", 3).ShouldBe("Destroy {0 ships!")
    End Sub

End Class