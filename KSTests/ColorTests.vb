
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

<TestClass()> Public Class ColorTests

    ''' <summary>
    ''' Tests setting colors
    ''' </summary>
    <TestMethod()> Public Sub TestSetColors()
        InitPaths()
        Assert.IsTrue(SetColors(ConsoleColors.White, ConsoleColors.White, ConsoleColors.Yellow, ConsoleColors.Red, ConsoleColors.DarkGreen, ConsoleColors.Green,
                                ConsoleColors.Black, ConsoleColors.Gray, ConsoleColors.DarkYellow, ConsoleColors.DarkGray, ConsoleColors.Green, ConsoleColors.Red,
                                ConsoleColors.Yellow, ConsoleColors.DarkYellow),
                      "Colors are not set properly. The following colors are applied:" + vbNewLine + vbNewLine +
                      "- InputC = {0}" + vbNewLine +
                      "- LicenseC = {1}" + vbNewLine +
                      "- ContKernelErrorC = {2}" + vbNewLine +
                      "- UncontKernelErrorC = {3}" + vbNewLine +
                      "- HostNameC = {4}" + vbNewLine +
                      "- UserNameC = {5}" + vbNewLine +
                      "- BackC = {6}" + vbNewLine +
                      "- NeutralTextC = {7}" + vbNewLine +
                      "- CmdListC = {8}" + vbNewLine +
                      "- CmdDefC = {9}" + vbNewLine +
                      "- StageC = {10}" + vbNewLine +
                      "- ErrorC = {11}" + vbNewLine +
                      "- WarningC = {12}" + vbNewLine +
                      "- OptionC = {13}", InputColor, LicenseColor, ContKernelErrorColor, UncontKernelErrorColor, HostNameShellColor, UserNameShellColor, BackgroundColor,
                                          NeutralTextColor, ListEntryColor, ListValueColor, StageColor, ErrorColor, WarningColor, OptionColor)
    End Sub

End Class