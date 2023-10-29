﻿
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

Imports KS.ConsoleBase

<TestClass()> Public Class ColorSettingTests

    ''' <summary>
    ''' Tests setting colors
    ''' </summary>
    <TestMethod()> <TestCategory("Setting")> Public Sub TestSetColors()
        SetColors(ConsoleColors.White, ConsoleColors.White, ConsoleColors.Yellow, ConsoleColors.Red, ConsoleColors.DarkGreen, ConsoleColors.Green,
                  ConsoleColors.Black, ConsoleColors.Gray, ConsoleColors.DarkYellow, ConsoleColors.DarkGray, ConsoleColors.Green, ConsoleColors.Red,
                  ConsoleColors.Yellow, ConsoleColors.DarkYellow, ConsoleColors.Green, ConsoleColors.White, ConsoleColors.Gray, ConsoleColors.DarkYellow,
                  ConsoleColors.Red, ConsoleColors.Yellow, ConsoleColors.Green, ConsoleColors.Gray, ConsoleColors.Gray, ConsoleColors.White, ConsoleColors.Gray,
                  ConsoleColors.White, ConsoleColors.Yellow, ConsoleColors.Gray, ConsoleColors.DarkYellow, ConsoleColors.DarkRed, ConsoleColors.White,
                  ConsoleColors.Yellow, ConsoleColors.Red, ConsoleColors.DarkGray, ConsoleColors.White, ConsoleColors.Gray, ConsoleColors.Yellow,
                  ConsoleColors.DarkGreen).ShouldBeTrue
    End Sub

End Class