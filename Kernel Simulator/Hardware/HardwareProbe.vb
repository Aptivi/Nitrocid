
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

Imports System.IO
Imports InxiFrontend

Public Module HardwareProbe

    Public HardwareInfo As Inxi

    'TODO: Re-write in Beta
    ''' <summary>
    ''' Starts probing hardware
    ''' </summary>
    Public Sub StartProbing()
        If Not quietProbe Then W(DoTranslation("hwprobe: Your hardware will be probed. Please wait...", currentLang), True, ColTypes.Neutral)

        'We will probe hardware
        Try
            If HardwareInfo Is Nothing Then
                Wdbg("I", "HardwareInfo is Nothing.")
                HardwareInfo = New Inxi
            Else
                Wdbg("I", "Cached hardware probe. HardwareInfo isn't Nothing.")
                W(DoTranslation("Cached hardware probe. Showing information anyway...", currentLang), True, ColTypes.Neutral)
            End If
        Catch ex As Exception
            Wdbg("E", "Failed to probe hardware: {0}", ex.Message)
            WStkTrc(ex)
            KernelError("F", True, 10, DoTranslation("There was an error when probing hardware: {0}", currentLang), ex, ex.Message)
        End Try

        If Not quietProbe Then
            'We are checking to see if any of the probers reported a failure starting with CPU
            If HardwareInfo.Hardware.CPU Is Nothing Or (HardwareInfo.Hardware.CPU IsNot Nothing And HardwareInfo.Hardware.CPU.Count = 0) Then
                Wdbg("E", "CPU failed to probe.", KernelVersion)
                W(DoTranslation("CPU: One or more of the CPU cores failed to be probed. Showing information anyway...", currentLang), True, ColTypes.Neutral)
            End If

            'then RAM
            If HardwareInfo.Hardware.RAM Is Nothing Then
                Wdbg("E", "RAM failed to probe.", KernelVersion)
                W(DoTranslation("RAM: One or more of the RAM chips failed to be probed. Showing information anyway...", currentLang), True, ColTypes.Neutral)
            End If

            'and finally HDD
            If HardwareInfo.Hardware.HDD Is Nothing Or (HardwareInfo.Hardware.HDD IsNot Nothing And HardwareInfo.Hardware.HDD.Count = 0) Then
                Wdbg("E", "HDD failed to probe.", KernelVersion)
                W(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway...", currentLang), True, ColTypes.Neutral)
            End If

            'Print information about the probed hardware
            ListDrivers()
        End If
    End Sub

    '----------> Hardware lists <----------
    ''' <summary>
    ''' Lists all drivers
    ''' </summary>
    Public Sub ListDrivers()
        'CPU Info
        For Each ProcessorInfo As String In HardwareInfo.Hardware.CPU.Keys
            If ProcessorInfo.Contains("@") And ProcessorInfo.EndsWith("GHz") Then
                W("CPU: {0}", False, ColTypes.Neutral, ProcessorInfo)
            Else
                W("CPU: {0} @ {1}", False, ColTypes.Neutral, ProcessorInfo, HardwareInfo.Hardware.CPU(ProcessorInfo).Speed)
            End If

            'SSE2 availability
            If HardwareInfo.Hardware.CPU(ProcessorInfo).Flags.Contains("sse2") Or HardwareInfo.Hardware.CPU(ProcessorInfo).Flags.Contains("SSE2") Then 'After SSE2 requirement addition, remove the check.
                W(" : SSE2 @ {0}-bit", True, ColTypes.Neutral, HardwareInfo.Hardware.CPU(ProcessorInfo).Bits)
            ElseIf HardwareInfo.Hardware.CPU(ProcessorInfo).Bits = 32 Then
                W(vbNewLine + DoTranslation("CPU: WARNING: SSE2 will be required in future development commits.", currentLang), True, ColTypes.Err)
            End If
        Next
        W(DoTranslation("CPU: Total number of processors: {0}", currentLang), True, ColTypes.Neutral, Environment.ProcessorCount)

        'Print some info
        W("RAM: {0} MB", True, ColTypes.Neutral, FormatNumber(HardwareInfo.Hardware.RAM.TotalMemory / 1024, 2))

        'Drive Info
        For Each driveinfo In HardwareInfo.Hardware.HDD.Keys
            If HardwareInfo.Hardware.HDD(driveinfo).Vendor = "(Standard disk drives)" Then
                W("HDD: {0} {1} GB", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(driveinfo).Model, FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Size / 1024 / 1024 / 1024, 2))
            Else
                W("HDD: {0} {1} {2} GB", True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(driveinfo).Vendor, HardwareInfo.Hardware.HDD(driveinfo).Model, FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Size / 1024 / 1024 / 1024, 2))
            End If
            For Each PartInfo In HardwareInfo.Hardware.HDD(driveinfo).Partitions.Keys
                W("HDD ({2}): {0} {1} GB", True, ColTypes.Neutral,
                  HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).FileSystem, FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size / 1024 / 1024 / 1024, 2), HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).ID)
            Next
        Next
    End Sub

End Module
