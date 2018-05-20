
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module Templates

    'Templates array (available ones)
    Public colorTemplates() As String = {"Default", "RedConsole", "Bluespire", "Hacker", "LinuxUncolored", "LinuxColoredDef"}

    'Variables for the "Default" theme
    Public inputColorDef As Object = inputColor
    Public licenseColorDef As Object = licenseColor
    Public contKernelErrorColorDef As Object = contKernelErrorColor
    Public uncontKernelErrorColorDef As Object = uncontKernelErrorColor
    Public hostNameShellColorDef As Object = hostNameShellColor
    Public userNameShellColorDef As Object = userNameShellColor
    Public backgroundColorDef As Object = backgroundColor
    Public neutralTextColorDef As Object = neutralTextColor

    'Variables for the "RedConsole" theme
    Public inputColorRC As Object = ConsoleColor.Red
    Public licenseColorRC As Object = ConsoleColor.Red
    Public contKernelErrorColorRC As Object = ConsoleColor.Red
    Public uncontKernelErrorColorRC As Object = ConsoleColor.DarkRed
    Public hostNameShellColorRC As Object = ConsoleColor.DarkRed
    Public userNameShellColorRC As Object = ConsoleColor.Red
    Public backgroundColorRC As Object = ConsoleColor.Black
    Public neutralTextColorRC As Object = ConsoleColor.Red

    'Variables for the "Bluespire" theme
    Public inputColorBS As Object = ConsoleColor.Cyan
    Public licenseColorBS As Object = ConsoleColor.Cyan
    Public contKernelErrorColorBS As Object = ConsoleColor.Blue
    Public uncontKernelErrorColorBS As Object = ConsoleColor.Blue
    Public hostNameShellColorBS As Object = ConsoleColor.Blue
    Public userNameShellColorBS As Object = ConsoleColor.Blue
    Public backgroundColorBS As Object = ConsoleColor.DarkCyan
    Public neutralTextColorBS As Object = ConsoleColor.Cyan

    'Variables for the "Hacker" theme
    Public inputColorHckr As Object = ConsoleColor.Green
    Public licenseColorHckr As Object = ConsoleColor.Green
    Public contKernelErrorColorHckr As Object = ConsoleColor.Green
    Public uncontKernelErrorColorHckr As Object = ConsoleColor.Green
    Public hostNameShellColorHckr As Object = ConsoleColor.Green
    Public userNameShellColorHckr As Object = ConsoleColor.Green
    Public backgroundColorHckr As Object = ConsoleColor.DarkGray
    Public neutralTextColorHckr As Object = ConsoleColor.Green

    'Variables for the "LinuxUncolored" theme
    Public inputColorLUnc As Object = ConsoleColor.Gray
    Public licenseColorLUnc As Object = ConsoleColor.Gray
    Public contKernelErrorColorLUnc As Object = ConsoleColor.Gray
    Public uncontKernelErrorColorLUnc As Object = ConsoleColor.Gray
    Public hostNameShellColorLUnc As Object = ConsoleColor.Gray
    Public userNameShellColorLUnc As Object = ConsoleColor.Gray
    Public backgroundColorLUnc As Object = ConsoleColor.Black
    Public neutralTextColorLUnc As Object = ConsoleColor.Gray

    'Variables for the "LinuxColoredDef" theme
    'If there is a mistake in colors, please fix it.
    Public inputColorLcDef As Object = ConsoleColor.Gray
    Public licenseColorLcDef As Object = ConsoleColor.Gray
    Public contKernelErrorColorLcDef As Object = ConsoleColor.Gray
    Public uncontKernelErrorColorLcDef As Object = ConsoleColor.Gray
    Public hostNameShellColorLcDef As Object = ConsoleColor.Blue
    Public userNameShellColorLcDef As Object = ConsoleColor.Blue
    Public backgroundColorLcDef As Object = ConsoleColor.Black
    Public neutralTextColorLcDef As Object = ConsoleColor.Gray

End Module
