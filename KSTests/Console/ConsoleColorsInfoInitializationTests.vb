
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

Imports KS.ConsoleBase.Colors

<TestFixture> Public Class ConsoleColorsInfoInitializationTests

    ''' <summary>
    ''' Tests initializing an instance of ConsoleColorsInfo from a bright color
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeConsoleColorsInfoInstanceBright()
        'Create instance
        Dim ConsoleColorsInfoInstance As New ConsoleColorsInfo(ConsoleColors.Grey85)

        'Check for property correctness
        ConsoleColorsInfoInstance.ColorID.ShouldBe(253)
        ConsoleColorsInfoInstance.R.ShouldBe(218)
        ConsoleColorsInfoInstance.G.ShouldBe(218)
        ConsoleColorsInfoInstance.B.ShouldBe(218)
        ConsoleColorsInfoInstance.IsBright.ShouldBeTrue
        ConsoleColorsInfoInstance.IsDark.ShouldBeFalse
    End Sub

    ''' <summary>
    ''' Tests initializing an instance of ConsoleColorsInfo from a dark color
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeConsoleColorsInfoInstanceDark()
        'Create instance
        Dim ConsoleColorsInfoInstance As New ConsoleColorsInfo(ConsoleColors.Grey11)

        'Check for property correctness
        ConsoleColorsInfoInstance.ColorID.ShouldBe(234)
        ConsoleColorsInfoInstance.R.ShouldBe(28)
        ConsoleColorsInfoInstance.G.ShouldBe(28)
        ConsoleColorsInfoInstance.B.ShouldBe(28)
        ConsoleColorsInfoInstance.IsBright.ShouldBeFalse
        ConsoleColorsInfoInstance.IsDark.ShouldBeTrue
    End Sub

End Class