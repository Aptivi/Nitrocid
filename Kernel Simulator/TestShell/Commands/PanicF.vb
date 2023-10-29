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

Namespace TestShell.Commands
    Class Test_PanicFCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim EType As KernelErrorLevel = [Enum].Parse(GetType(KernelErrorLevel), ListArgs(0))
            Dim Reboot As Boolean = ListArgs(1)
            Dim RTime As Long = ListArgs(2)
            Dim Args As String = ListArgs(3)
            Dim Exc As New Exception
            Dim Message As String = ListArgs(4)
            KernelError(EType, Reboot, RTime, Message, Exc, Args)
        End Sub

    End Class
End Namespace