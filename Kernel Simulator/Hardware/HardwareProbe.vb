
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Module HardwareProbe

    Public HardwareInfo As Inxi

    ''' <summary>
    ''' Starts probing hardware
    ''' </summary>
    Public Sub StartProbing()
        If Not QuietHardwareProbe Then W(DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), True, ColTypes.Neutral)

        'We will probe hardware
        EventManager.RaiseHardwareProbing()
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

        If Not QuietHardwareProbe Then
            If HardwareInfo IsNot Nothing Then
                'We are checking to see if any of the probers reported a failure starting with CPU
                If HardwareInfo.Hardware.CPU Is Nothing Or (HardwareInfo.Hardware.CPU IsNot Nothing And HardwareInfo.Hardware.CPU.Count = 0) Then
                    Wdbg(DebugLevel.E, "CPU failed to probe.")
                    W(DoTranslation("CPU: One or more of the CPU cores failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'then RAM
                If HardwareInfo.Hardware.RAM Is Nothing Then
                    Wdbg(DebugLevel.E, "RAM failed to probe.")
                    W(DoTranslation("RAM: One or more of the RAM chips failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'then GPU
                If HardwareInfo.Hardware.GPU Is Nothing Then
                    Wdbg(DebugLevel.E, "GPU failed to probe.")
                    W(DoTranslation("GPU: One or more of the graphics cards failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'and finally HDD
                If HardwareInfo.Hardware.HDD Is Nothing Or (HardwareInfo.Hardware.HDD IsNot Nothing And HardwareInfo.Hardware.HDD.Count = 0) Then
                    Wdbg(DebugLevel.E, "HDD failed to probe.")
                    W(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'Print information about the probed hardware
                ListHardware()
            End If
        End If

        'Raise event
        EventManager.RaiseHardwareProbed()
    End Sub

    ''' <summary>
    ''' Write Inxi.NET hardware parsing completion to debugger and, if quiet probe is disabled, the console
    ''' </summary>
    Private Sub WriteWhatProbed(Hardware As InxiHardwareType)
        Wdbg(DebugLevel.I, "Hardware {0} ({1}) successfully probed.", Hardware, Hardware.ToString)
        If Not QuietHardwareProbe And VerboseHardwareProbe Then W(DoTranslation("Successfully probed {0}."), True, ColTypes.Neutral, Hardware.ToString)
    End Sub

    ''' <summary>
    ''' Write Inxi.NET debug data to debugger
    ''' </summary>
    Private Sub WriteInxiDebugData(Message As String, PlainMessage As String)
        Wdbg(DebugLevel.I, PlainMessage)
    End Sub

End Module
