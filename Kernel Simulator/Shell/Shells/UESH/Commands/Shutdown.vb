
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
    ''' Shuts down your computer
    ''' </summary>
    ''' <remarks>
    ''' If you're finished with everything and don't want to do something else on your computer, instead of leaving it on to consume energy and pay high energy bills, you have to use this command to shutdown your computer and conserve power.
    ''' </remarks>
    Class ShutdownCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If Not ListArgsOnly.Length = 0 Then
                If ListArgsOnly.Length = 1 Then
                    PowerManage(PowerMode.RemoteShutdown, ListArgsOnly(0))
                Else
                    PowerManage(PowerMode.RemoteShutdown, ListArgsOnly(0), ListArgsOnly(1))
                End If
            Else
                PowerManage(PowerMode.Shutdown)
            End If
        End Sub

    End Class
End Namespace
