
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
    ''' Lets you unblock a blocked debug device
    ''' </summary>
    ''' <remarks>
    ''' If you wanted to let a device whose IP address is blocked join the remote debugging again, you can unblock it using this command.
    ''' <br></br>
    ''' The user must have at least the administrative privileges before they can run the below commands.
    ''' </remarks>
    Class UnblockDbgDevCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If RDebugBlocked.Contains(ListArgs(0)) Then
                If TryRemoveFromBlockList(ListArgs(0)) Then
                    Write(DoTranslation("{0} can now join remote debug again."), True, ColTypes.Neutral, ListArgs(0))
                Else
                    Write(DoTranslation("Failed to unblock {0}."), True, ColTypes.Neutral, ListArgs(0))
                End If
            Else
                Write(DoTranslation("{0} is not blocked yet."), True, ColTypes.Neutral, ListArgs(0))
            End If
        End Sub

    End Class
End Namespace
