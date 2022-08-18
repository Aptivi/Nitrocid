
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

Imports KS.Kernel.Power

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Restarts the kernel
    ''' </summary>
    ''' <remarks>
    ''' This command restarts your simulated kernel and reloads all the config that are not loaded using reloadconfig. This is especially useful if you want to change colors of text, set arguments into the kernel, inject arguments, and so on.
    ''' <br></br>
    ''' > [!WARNING]
    ''' > There is no file system syncing because the current kernel version doesn't have the real file system to sync, and the kernel is not final.
    ''' </remarks>
    Class RebootCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If Not ListArgs?.Length = 0 Then
                If ListArgs(0) = "safe" Then
                    PowerManage(PowerMode.RebootSafe)
                ElseIf ListArgs(0) <> "" Then
                    If ListArgs?.Length > 1 Then
                        PowerManage(PowerMode.RemoteRestart, ListArgs(0), ListArgs(1))
                    Else
                        PowerManage(PowerMode.RemoteRestart, ListArgs(0))
                    End If
                Else
                    PowerManage(PowerMode.Reboot)
                End If
            Else
                PowerManage(PowerMode.Reboot)
            End If
        End Sub

    End Class
End Namespace
