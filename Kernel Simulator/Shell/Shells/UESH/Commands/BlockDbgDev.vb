
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

Imports KS.Network.RemoteDebug

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' You can block an IP address from entering the remote debugger.
    ''' </summary>
    ''' <remarks>
    ''' If you wanted to moderate the remote debugger and block a device from joining it because it either causes problems or kept flooding the chat, you may use this command to block such offenders.
    ''' <br></br>
    ''' This command is available to administrators only. The blocked device can be unblocked using the unblockdbgdev command.
    ''' <br></br>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class BlockDbgDevCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If Not RDebugBlocked.Contains(ListArgsOnly(0)) Then
                If TryAddToBlockList(ListArgsOnly(0)) Then
                    Write(DoTranslation("{0} can't join remote debug now."), True, ColTypes.Neutral, ListArgsOnly(0))
                Else
                    Write(DoTranslation("Failed to block {0}."), True, ColTypes.Neutral, ListArgsOnly(0))
                End If
            Else
                Write(DoTranslation("{0} is already blocked."), True, ColTypes.Neutral, ListArgsOnly(0))
            End If
        End Sub

    End Class
End Namespace
