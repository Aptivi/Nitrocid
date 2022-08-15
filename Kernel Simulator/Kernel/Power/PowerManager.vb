
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

Imports KS.Network.RPC

Namespace Kernel.Power
    Public Module PowerManager

        ''' <summary>
        ''' Manage computer's (actually, simulated computer) power
        ''' </summary>
        ''' <param name="PowerMode">Selects the power mode</param>
        Public Sub PowerManage(PowerMode As PowerMode)
            PowerManage(PowerMode, "0.0.0.0", RPCPort)
        End Sub

        ''' <summary>
        ''' Manage computer's (actually, simulated computer) power
        ''' </summary>
        ''' <param name="PowerMode">Selects the power mode</param>
        Public Sub PowerManage(PowerMode As PowerMode, IP As String)
            PowerManage(PowerMode, IP, RPCPort)
        End Sub

        ''' <summary>
        ''' Manage computer's (actually, simulated computer) power
        ''' </summary>
        ''' <param name="PowerMode">Selects the power mode</param>
        Public Sub PowerManage(PowerMode As PowerMode, IP As String, Port As Integer)
            Wdbg(DebugLevel.I, "Power management has the argument of {0}", PowerMode)
            Select Case PowerMode
                Case PowerMode.Shutdown
                    KernelEventManager.RaisePreShutdown()
                    Write(DoTranslation("Shutting down..."), True, ColTypes.Neutral)
                    ResetEverything()
                    KernelEventManager.RaisePostShutdown()
                    RebootRequested = True
                    LogoutRequested = True
                    KernelShutdown = True
                Case PowerMode.Reboot, PowerMode.RebootSafe
                    KernelEventManager.RaisePreReboot()
                    Write(DoTranslation("Rebooting..."), True, ColTypes.Neutral)
                    ResetEverything()
                    KernelEventManager.RaisePostReboot()
                    Console.Clear()
                    RebootRequested = True
                    LogoutRequested = True
                Case PowerMode.RemoteShutdown
                    SendCommand("<Request:Shutdown>(" + IP + ")", IP, Port)
                Case PowerMode.RemoteRestart
                    SendCommand("<Request:Reboot>(" + IP + ")", IP, Port)
                Case PowerMode.RemoteRestartSafe
                    SendCommand("<Request:RebootSafe>(" + IP + ")", IP, Port)
            End Select
            SafeMode = PowerMode = PowerMode.RebootSafe
        End Sub

    End Module
End Namespace
