
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

    ''' <summary>
    ''' Starts probing hardware
    ''' </summary>
    Public Sub StartProbing()
        If Not quietProbe Then W(DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), True, ColTypes.Neutral)

        'We will probe hardware
        EventManager.RaiseHardwareProbing()
        Try
            HardwareInfo = New Inxi(InxiParseFlags.Processor Or InxiParseFlags.PCMemory Or InxiParseFlags.Graphics Or InxiParseFlags.HardDrive)
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

                'then GPU
                If HardwareInfo.Hardware.GPU Is Nothing Then
                    Wdbg("E", "GPU failed to probe.")
                    W(DoTranslation("GPU: One or more of the graphics cards failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'and finally HDD
                If HardwareInfo.Hardware.HDD Is Nothing Or (HardwareInfo.Hardware.HDD IsNot Nothing And HardwareInfo.Hardware.HDD.Count = 0) Then
                    Wdbg("E", "HDD failed to probe.")
                    W(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway..."), True, ColTypes.Neutral)
                End If

                'Print information about the probed hardware
                ListDrivers()
            End If
        End If

        'Raise event
        EventManager.RaiseHardwareProbed()
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
