
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports System.Reflection
Imports InxiFrontend
Imports KS.Misc.Reflection

Namespace Hardware
    Public Module HardwareList

        ''' <summary>
        ''' Lists simple information about hardware
        ''' </summary>
        Public Sub ListHardware()
            If HardwareInfo IsNot Nothing Then
                'We are checking to see if any of the probers reported a failure starting with CPU
                If HardwareInfo.Hardware.CPU Is Nothing Or (HardwareInfo.Hardware.CPU IsNot Nothing And HardwareInfo.Hardware.CPU.Count = 0) Then
                    Wdbg(DebugLevel.E, "CPU failed to probe.")
                    Write(DoTranslation("CPU: One or more of the CPU cores failed to be probed. Showing information anyway..."), True, ColTypes.Warning)
                End If

                'then RAM
                If HardwareInfo.Hardware.RAM Is Nothing Then
                    Wdbg(DebugLevel.E, "RAM failed to probe.")
                    Write(DoTranslation("RAM: One or more of the RAM chips failed to be probed. Showing information anyway..."), True, ColTypes.Warning)
                End If

                'then GPU
                If HardwareInfo.Hardware.GPU Is Nothing Then
                    Wdbg(DebugLevel.E, "GPU failed to probe.")
                    Write(DoTranslation("GPU: One or more of the graphics cards failed to be probed. Showing information anyway..."), True, ColTypes.Warning)
                End If

                'and finally HDD
                If HardwareInfo.Hardware.HDD Is Nothing Or (HardwareInfo.Hardware.HDD IsNot Nothing And HardwareInfo.Hardware.HDD.Count = 0) Then
                    Wdbg(DebugLevel.E, "HDD failed to probe.")
                    Write(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway..."), True, ColTypes.Warning)
                End If

                'Print information about the probed hardware
                'CPU Info
                For Each ProcessorInfo As String In HardwareInfo.Hardware.CPU.Keys
                    Dim TargetProcessor As Processor = HardwareInfo.Hardware.CPU(ProcessorInfo)
                    Write("CPU: " + DoTranslation("Processor name:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, ProcessorInfo)
                    Write("CPU: " + DoTranslation("Processor clock speed:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, TargetProcessor.Speed)
                    Write("CPU: " + DoTranslation("Processor bits:"), False, ColTypes.ListEntry) : Write(" {0}-bit", True, ColTypes.ListValue, TargetProcessor.Bits)
                    Write("CPU: " + DoTranslation("Processor SSE2 support:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, TargetProcessor.Flags.Contains("sse2") Or TargetProcessor.Flags.Contains("SSE2"))
                Next
                Write("CPU: " + DoTranslation("Total number of processors:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, Environment.ProcessorCount)

                'Print RAM info
                Write("RAM: " + DoTranslation("Total memory:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, If(IsOnWindows(), CLng(HardwareInfo.Hardware.RAM.TotalMemory * 1024).FileSizeToString, HardwareInfo.Hardware.RAM.TotalMemory))

                'GPU info
                For Each GPUInfo As String In HardwareInfo.Hardware.GPU.Keys
                    Dim TargetGraphics As Graphics = HardwareInfo.Hardware.GPU(GPUInfo)
                    Write("GPU: " + DoTranslation("Graphics card:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, TargetGraphics.Name)
                Next

                'Drive Info
                For Each DriveInfo As String In HardwareInfo.Hardware.HDD.Keys
                    Dim TargetDrive As HardDrive = HardwareInfo.Hardware.HDD(DriveInfo)
                    If TargetDrive.Vendor = "(Standard disk drives)" Then
                        Write("HDD: " + DoTranslation("Disk model:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, TargetDrive.Model)
                        Write("HDD: " + DoTranslation("Disk size:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, If(IsOnWindows(), CLng(TargetDrive.Size).FileSizeToString, TargetDrive.Size))
                    Else
                        Write("HDD: " + DoTranslation("Disk model:"), False, ColTypes.ListEntry) : Write(" {0} {1}", True, ColTypes.ListValue, TargetDrive.Vendor, TargetDrive.Model)
                        Write("HDD: " + DoTranslation("Disk size:"), False, ColTypes.ListEntry) : Write(" {0}", True, ColTypes.ListValue, If(IsOnWindows(), CLng(TargetDrive.Size).FileSizeToString, TargetDrive.Size))
                    End If
                    For Each PartInfo As String In TargetDrive.Partitions.Keys
                        Dim TargetPart As Partition = TargetDrive.Partitions(PartInfo)
                        Write("HDD ({0}): " + DoTranslation("Partition size:"), False, ColTypes.ListEntry, TargetPart.ID) : Write(" {0}", True, ColTypes.ListValue, If(IsOnWindows(), CLng(TargetPart.Size).FileSizeToString, TargetPart.Size))
                        Write("HDD ({0}): " + DoTranslation("Partition filesystem:"), False, ColTypes.ListEntry, TargetPart.ID) : Write(" {0}", True, ColTypes.ListValue, TargetPart.FileSystem)
                    Next
                Next
            End If
        End Sub

        ''' <summary>
        ''' Lists information about hardware
        ''' </summary>
        ''' <param name="HardwareType">Hadrware type defined by Inxi.NET. If "all", prints all information.</param>
        Public Sub ListHardware(HardwareType As String)
            Dim HardwareField As FieldInfo = GetField(HardwareType, GetField(NameOf(HardwareInfo.Hardware), GetType(Inxi)).FieldType)
            Wdbg(DebugLevel.I, "Got hardware field {0}.", If(HardwareField IsNot Nothing, HardwareField.Name, "unknown"))
            If HardwareField IsNot Nothing Then
                ListHardwareProperties(HardwareField)
            ElseIf HardwareType.ToLower = "all" Then
                Dim HardwareFields As FieldInfo() = GetField(NameOf(HardwareInfo.Hardware), GetType(Inxi)).FieldType.GetFields
                For Each HardwareFieldType As FieldInfo In HardwareFields
                    ListHardwareProperties(HardwareFieldType)
                Next
            Else
                Write(DoTranslation("Either the hardware type {0} is not probed, or is not valid."), True, ColTypes.Error, HardwareType)
            End If
        End Sub

        Private Sub ListHardwareProperties(Field As FieldInfo)
            Wdbg(DebugLevel.I, "Got hardware field {0}.", Field.Name)
            WriteSeparator(Field.Name, True)
            Dim FieldValue As Object = Field.GetValue(HardwareInfo.Hardware)
            If FieldValue IsNot Nothing Then
                Dim FieldValueDict As IDictionary = TryCast(FieldValue, IDictionary)
                If FieldValueDict IsNot Nothing Then
                    For Each HardwareKey As String In FieldValueDict.Keys
                        Write("- {0}: ", True, ColTypes.ListEntry, HardwareKey)
                        For Each HardwareValuePropertyInfo As PropertyInfo In FieldValueDict(HardwareKey).GetType.GetProperties
                            Write("  - {0}: ", False, ColTypes.ListEntry, HardwareValuePropertyInfo.Name)
                            If Field.Name = "HDD" And HardwareValuePropertyInfo.Name = "Partitions" Then
                                Console.WriteLine()
                                Dim Partitions As IDictionary = TryCast(HardwareValuePropertyInfo.GetValue(FieldValueDict(HardwareKey)), IDictionary)
                                If Partitions IsNot Nothing Then
                                    For Each PartitionKey As String In Partitions.Keys
                                        Write("    - {0}: ", True, ColTypes.ListEntry, PartitionKey)
                                        For Each PartitionValuePropertyInfo As PropertyInfo In Partitions(PartitionKey).GetType.GetProperties
                                            Write("      - {0}: ", False, ColTypes.ListEntry, PartitionValuePropertyInfo.Name)
                                            Write(PartitionValuePropertyInfo.GetValue(Partitions(PartitionKey)), True, ColTypes.ListValue)
                                        Next
                                    Next
                                Else
                                    Write(DoTranslation("Partitions not parsed to list."), True, ColTypes.Error)
                                End If
                            ElseIf Field.Name = "CPU" And HardwareValuePropertyInfo.Name = "Flags" Then
                                Write(String.Join(", ", TryCast(HardwareValuePropertyInfo.GetValue(FieldValueDict(HardwareKey)), String())), True, ColTypes.ListValue)
                            Else
                                Write(HardwareValuePropertyInfo.GetValue(FieldValueDict(HardwareKey)), True, ColTypes.ListValue)
                            End If
                        Next
                    Next
                Else
                    For Each HardwareFieldInfo As FieldInfo In Field.FieldType.GetFields()
                        Write("- {0}: ", False, ColTypes.ListEntry, HardwareFieldInfo.Name)
                        Write(HardwareFieldInfo.GetValue(FieldValue), True, ColTypes.ListValue)
                    Next
                End If
            Else
                Write(DoTranslation("The hardware type {0} is not probed yet. If you're sure that it's probed, restart the kernel with debugging enabled."), True, ColTypes.Error, Field.Name)
            End If
        End Sub

    End Module
End Namespace