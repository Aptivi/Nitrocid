
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

Imports InxiFrontend
Imports InxiFrontend.InxiTrace
Imports KS.Misc.Splash

Namespace Hardware
    Public Module HardwareProbe

        Public HardwareInfo As Inxi

        ''' <summary>
        ''' Starts probing hardware
        ''' </summary>
        Public Sub StartProbing()
            'We will probe hardware
            KernelEventManager.RaiseHardwareProbing()
            Try
                AddHandler DebugDataReceived, AddressOf WriteInxiDebugData
                AddHandler HardwareParsed, AddressOf WriteWhatProbed
                If FullHardwareProbe Then
                    HardwareInfo = New Inxi()
                Else
                    HardwareInfo = New Inxi(InxiHardwareType.Processor Or InxiHardwareType.PCMemory Or InxiHardwareType.Graphics Or InxiHardwareType.HardDrive)
                End If
                RemoveHandler DebugDataReceived, AddressOf WriteInxiDebugData
                RemoveHandler HardwareParsed, AddressOf WriteWhatProbed
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to probe hardware: {0}", ex.Message)
                WStkTrc(ex)
                KernelError(KernelErrorLevel.F, True, 10, DoTranslation("There was an error when probing hardware: {0}"), ex, ex.Message)
            End Try

            'Raise event
            KernelEventManager.RaiseHardwareProbed()
        End Sub

        ''' <summary>
        ''' Write Inxi.NET hardware parsing completion to debugger and, if quiet probe is disabled, the console
        ''' </summary>
        Private Sub WriteWhatProbed(Hardware As InxiHardwareType)
            Wdbg(DebugLevel.I, "Hardware {0} ({1}) successfully probed.", Hardware, Hardware.ToString)
            If (Not QuietHardwareProbe And VerboseHardwareProbe) Or EnableSplash Then ReportProgress(DoTranslation("Successfully probed {0}.").FormatString(Hardware.ToString), 5, ColTypes.Neutral)
        End Sub

        ''' <summary>
        ''' Write Inxi.NET debug data to debugger
        ''' </summary>
        Private Sub WriteInxiDebugData(Message As String, PlainMessage As String)
            Wdbg(DebugLevel.I, PlainMessage)
        End Sub

    End Module
End Namespace