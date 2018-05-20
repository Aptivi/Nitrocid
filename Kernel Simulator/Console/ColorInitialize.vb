
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

Module ColorInitialize

    'Variables for colors used by previous versions of Kernel.
    Public inputColor As Object = ConsoleColor.White
    Public licenseColor As Object = ConsoleColor.White
    Public contKernelErrorColor As Object = ConsoleColor.Yellow
    Public uncontKernelErrorColor As Object = ConsoleColor.Red
    Public hostNameShellColor As Object = ConsoleColor.DarkGreen
    Public userNameShellColor As Object = ConsoleColor.Green
    Public backgroundColor As Object = ConsoleColor.Black
    Public neutralTextColor As Object = ConsoleColor.Gray

    'Array for available colors
    Public availableColors() As String = {"White", "Gray", "DarkGray", "DarkRed", "Red", "DarkYellow", "Yellow", "DarkGreen", "Green", _
                                          "DarkCyan", "Cyan", "DarkBlue", "Blue", "DarkMagenta", "Magenta", "RESET", "THEME"}

End Module
