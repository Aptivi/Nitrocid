
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

Namespace Shell.ShellBase.Shells
    Public MustInherit Class ShellExecutor
        Implements IShell

        Public Overridable ReadOnly Property ShellType As ShellType Implements IShell.ShellType
        Public Overridable Property Bail As Boolean Implements IShell.Bail

        Public Overridable Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            Wdbg(DebugLevel.F, "We shouldn't be here!!!")
            Throw New InvalidOperationException()
        End Sub

    End Class
End Namespace
