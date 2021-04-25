
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

Public Module HardwareList

    ''' <summary>
    ''' Lists simple information about hardware
    ''' </summary>
    Public Sub ListHardware()
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
                W(vbNewLine + DoTranslation("CPU: WARNING: SSE2 will be required in future development commits."), True, ColTypes.Err)
            End If
        Next
        W(DoTranslation("CPU: Total number of processors: {0}"), True, ColTypes.Neutral, Environment.ProcessorCount)

        'Print RAM info
        W(If(IsNumeric(HardwareInfo.Hardware.RAM.TotalMemory), "RAM: {0} MB", "RAM: {0}"), True, ColTypes.Neutral, If(IsNumeric(HardwareInfo.Hardware.RAM.TotalMemory), FormatNumber(HardwareInfo.Hardware.RAM.TotalMemory / 1024, 2), HardwareInfo.Hardware.RAM.TotalMemory))

        'GPU info
        For Each GPUInfo In HardwareInfo.Hardware.GPU.Keys
            W("GPU: {0}", True, ColTypes.Neutral, HardwareInfo.Hardware.GPU(GPUInfo).Name)
        Next

        'Drive Info
        For Each driveinfo In HardwareInfo.Hardware.HDD.Keys
            If HardwareInfo.Hardware.HDD(driveinfo).Vendor = "(Standard disk drives)" Then
                W(If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), "HDD: {0} {1} GB", "HDD: {0} {1}"), True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(driveinfo).Model, If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Size / 1024 / 1024 / 1024, 2), HardwareInfo.Hardware.HDD(driveinfo).Size))
            Else
                W(If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), "HDD: {0} {1} {2} GB", "HDD: {0} {1} {2}"), True, ColTypes.Neutral, HardwareInfo.Hardware.HDD(driveinfo).Vendor, HardwareInfo.Hardware.HDD(driveinfo).Model, If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Size), FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Size / 1024 / 1024 / 1024, 2), HardwareInfo.Hardware.HDD(driveinfo).Size))
            End If
            For Each PartInfo In HardwareInfo.Hardware.HDD(driveinfo).Partitions.Keys
                W(If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size), "HDD ({2}): {0} {1} GB", "HDD ({2}): {0} {1}"), True, ColTypes.Neutral,
                  HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).FileSystem, If(IsNumeric(HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size), FormatNumber(HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size / 1024 / 1024 / 1024, 2), HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).Size), HardwareInfo.Hardware.HDD(driveinfo).Partitions(PartInfo).ID)
            Next
        Next
    End Sub

End Module
