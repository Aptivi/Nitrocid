
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

Namespace Shell.ShellBase
    Public Class ShellInfo

        ''' <summary>
        ''' Shell type
        ''' </summary>
        Public ReadOnly ShellType As ShellType
        ''' <summary>
        ''' Shell executor
        ''' </summary>
        Public ReadOnly ShellExecutor As ShellExecutor
        ''' <summary>
        ''' Shell command thread
        ''' </summary>
        Public ReadOnly ShellCommandThread As KernelThread

        ''' <summary>
        ''' Installs the values to a new instance of ShellInfo
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        ''' <param name="ShellExecutor">Shell executor</param>
        ''' <param name="ShellCommandThread">Shell command thread</param>
        Public Sub New(ShellType As ShellType, ShellExecutor As ShellExecutor, ShellCommandThread As KernelThread)
            Me.ShellType = ShellType
            Me.ShellExecutor = ShellExecutor
            Me.ShellCommandThread = ShellCommandThread
        End Sub

    End Class
End Namespace
