
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
        If Not quietProbe Then W(DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), True, ColTypes.Neutral)

        'We will probe hardware
        EventManager.RaiseHardwareProbing()
        Try
            AddHandler DebugDataReceived, AddressOf WriteInxiDebugData
            If FullProbe Then
                HardwareInfo = New Inxi()
            Else
                HardwareInfo = New Inxi(InxiParseFlags.Processor Or InxiParseFlags.PCMemory Or InxiParseFlags.Graphics Or InxiParseFlags.HardDrive)
            End If
            RemoveHandler DebugDataReceived, AddressOf WriteInxiDebugData
        Catch ex As Exception
            Wdbg("E", "Failed to probe hardware: {0}", ex.Message)
            WStkTrc(ex)
            KernelError("F", True, 10, DoTranslation("There was an error when probing hardware: {0}"), ex, ex.Message)
        End Try

        If Not quietProbe Then
            If HardwareInfo IsNot Nothing Then
                'We are checking to see if any of the probers reported a failure starting with CPU
                If HardwareInfo.Hardware.CPU Is Nothing Or (HardwareInfo.Hardware.CPU IsNot Nothing And HardwareInfo.Hardware.CPU.Count = 0) Then
                    Wdbg("E", "CPU failed to probe.")
                    W(DoTranslation("CPU: One or more of the CPU cores failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'then RAM
                If HardwareInfo.Hardware.RAM Is Nothing Then
                    Wdbg("E", "RAM failed to probe.")
                    W(DoTranslation("RAM: One or more of the RAM chips failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'and finally HDD
                If HardwareInfo.Hardware.HDD Is Nothing Or (HardwareInfo.Hardware.HDD IsNot Nothing And HardwareInfo.Hardware.HDD.Count = 0) Then
                    Wdbg("E", "HDD failed to probe.", KernelVersion)
                    W(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway...", currentLang), True, ColTypes.Neutral)
                End If

                'and finally HDD
                If HardwareInfo.Hardware.HDD Is Nothing Or (HardwareInfo.Hardware.HDD IsNot Nothing And HardwareInfo.Hardware.HDD.Count = 0) Then
                    Wdbg("E", "HDD failed to probe.")
                    W(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'Print information about the probed hardware
                ListHardware()
            End If
    End Sub

    ''' <summary>
    ''' Write Inxi.NET debug data to debugger
    ''' </summary>

    ''' <summary>
    ''' Write Inxi.NET debug data to debugger
    ''' </summary>
    Private Sub WriteInxiDebugData(Message As String, PlainMessage As String)
        Wdbg("I", PlainMessage)
    End Sub

End Module
