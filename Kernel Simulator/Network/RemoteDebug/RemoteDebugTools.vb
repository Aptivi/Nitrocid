
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Public Module RemoteDebugTools

    ''' <summary>
    ''' Blocks a remote debug device
    ''' </summary>
    ''' <param name="IP">An IP address for device</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    Public Function BlockDevice(ByVal IP As String) As Boolean
        DisconnectDbgDev(IP)
        RDebugBlocked.Add(IP)
        Return True
    End Function

    ''' <summary>
    ''' Disconnects a specified debug device
    ''' </summary>
    ''' <param name="IPAddr">An IP address of the connected debug device</param>
    Public Sub DisconnectDbgDev(ByVal IPAddr As String)
        Dim Found As Boolean
        For i As Integer = 0 To DebugDevices.Count - 1
            If Found Then
                Exit Sub
            Else
                If IPAddr = DebugDevices.Values(i) Then
                    Wdbg("I", "Debug device {0} disconnected.", DebugDevices.Values(i))
                    Found = True
                    DebugDevices.Keys(i).Disconnect(True)
                    EventManager.RaiseRemoteDebugConnectionDisconnected()
                    dbgConns.Remove(dbgConns.Keys(i))
                    DebugDevices.Remove(DebugDevices.Keys(i))
                End If
            End If
        Next
        If Not Found Then
            Throw New EventsAndExceptions.RemoteDebugDeviceNotFoundException(DoTranslation("Debug device {0} not found.", currentLang).FormatString(IPAddr))
        End If
    End Sub

    ''' <summary>
    ''' Unblocks a remote debug device
    ''' </summary>
    ''' <param name="IP">A blocked IP address for device</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    Public Function UnblockDevice(ByVal IP As String) As Boolean
        Return RDebugBlocked.Remove(IP)
    End Function

End Module
