
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

Public Module ColorSet

    Public Sub ResetColors()
        inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorDef), ConsoleColor)
        licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorDef), ConsoleColor)
        contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorDef), ConsoleColor)
        uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorDef), ConsoleColor)
        hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorDef), ConsoleColor)
        userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorDef), ConsoleColor)
        backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorDef), ConsoleColor)
        neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorDef), ConsoleColor)
        cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorDef), ConsoleColor)
        cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorDef), ConsoleColor)
        LoadBackground.Load()
    End Sub

End Module
