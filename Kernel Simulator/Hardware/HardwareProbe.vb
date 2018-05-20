
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module HardwareProbe

    Sub ProbeHW(Optional ByVal QuietHWProbe As Boolean = False, Optional ByVal KernelUser As Char = CChar("U"))

        Wdbg("QuietHWProbe = {0}, KernelUser = {1}, ProbeFlag = {2}.", True, QuietHWProbe, KernelUser, ProbeFlag)
        If (QuietHWProbe = False) Then
            If (KernelUser = "K") Then
                If (ProbeFlag = True) Then
                    Wln("hwprobe: Your hardware will be probed. Please wait...", "neutralText")
                    Cpuinfo()
                    Wdbg("CPU probed.", True, KernelVersion)
                    Wln("hwprobe: CPU: {0} {1}MHz", "neutralText", Cpuname, Cpuspeed)
                    SysMemory()
                    Wdbg("RAM probed.", True, KernelVersion)
                    Wln("hwprobe: RAM: {0} = {1}MB", "neutralText", SysMem, String.Join("MB + ", Capacities))
                    Hddinfo()
                    Wdbg("HDD probed.", True, KernelVersion)
                    If (GPUProbeFlag = True) Then
                        ProbeGPU(True, False)
                        Wdbg("GPU probed.", True, KernelVersion)
                    End If
                    BiosInformation()
                    Wdbg("BIOS probed.", True, KernelVersion)
                    Wln("hwprobe: BIOS: {0} {1} {2} {3}", "neutralText", BIOSMan, BIOSCaption, BIOSVersion, BIOSSMBIOSVersion)
                Else
                    Wln("hwprobe: Hardware is not probed. Probe using 'hwprobe'", "neutralText")
                End If
            ElseIf (KernelUser = "U") Then
                If (ProbeFlag = False) Then
                    Wln("hwprobe: Your hardware will be probed. Please wait...", "neutralText")
                    Cpuinfo()
                    Wdbg("CPU probed.", True, KernelVersion)
                    Wln("hwprobe: CPU: {0} {1}MHz", "neutralText", Cpuname, Cpuspeed)
                    SysMemory()
                    Wdbg("RAM probed.", True, KernelVersion)
                    Wln("hwprobe: RAM: {0} = {1}MB", "neutralText", SysMem, String.Join("MB + ", Capacities))
                    Hddinfo()
                    Wdbg("HDD probed.", True, KernelVersion)
                    If (GPUProbeFlag = True) Then
                        ProbeGPU(True, False)
                        Wdbg("GPU probed.", True, KernelVersion)
                    End If
                    BiosInformation()
                    Wdbg("BIOS probed.", True, KernelVersion)
                    Wln("hwprobe: BIOS: {0} {1} {2} {3}", "neutralText", BIOSMan, BIOSCaption, BIOSVersion, BIOSSMBIOSVersion)
                    ProbeFlag = True
                Else
                    Wln("hwprobe: Hardware already probed.", "neutralText")
                End If
            End If
        ElseIf (QuietHWProbe = True) Then
            If (ProbeFlag = True) Then
                Cpuinfo()
                SysMemory(True)
                Hddinfo(True)
                If (GPUProbeFlag = True) Then
                    ProbeGPU(True, True)
                End If
                BiosInformation()
            End If
        End If

    End Sub

    Sub ListDrivers()

        Wln("CPU: {0} {1}MHz", "neutralText", Cpuname, Cpuspeed)
        Wln("RAM: Used slots (by names): {0}", "neutralText", slotsUsedName)
        Wln("RAM: Used slots (by numbers): {0} / {1} ({2}%)", "neutralText", CStr(slotsUsedNum), totalSlots, FormatNumber(CStr(slotsUsedNum * 100 / totalSlots), 1))
        Wln("RAM: {0} = {1}MB", SysMem, String.Join("MB + ", "neutralText", Capacities))
        Hddinfo(False, False)
        ProbeGPU(False)
        BiosInformation()

    End Sub

    'TODO: Re-organize parsers as one sub.

    Sub ProbeGPU(Optional ByVal KernelMode As Boolean = True, Optional ByVal QuietMode As Boolean = False)

        Try
            If ProbeFlag = True And GPUProbeFlag = True Then
                Dim colGPUs As Object
                Dim oGPU As Object

                colGPUs = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_VideoController")
                Wdbg("Object created = {0}.Win32_VideoController", True, colGPUs)

                For Each oGPU In colGPUs
                    Wdbg("GPU Object = {0}.Win32_VideoController.Caption = {1}, {0}.Win32_VideoController.AdapterRAM = {2}", True, oGPU, oGPU.Caption, oGPU.AdapterRAM)
                    If (QuietMode = False And KernelMode = True) Then
                        Wln("hwprobe: GPU: {0} {1}MB", "neutralText", oGPU.Caption, CStr(oGPU.AdapterRAM / 1024 / 1024))
                    ElseIf (QuietMode = False And KernelMode = False) Then
                        Wln("GPU: {0} {1}MB", "neutralText", oGPU.Caption, CStr(oGPU.AdapterRAM / 1024 / 1024))
                    End If
                Next
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("C"), False, 0, "Error while trying to get video card information.")
        End Try

    End Sub

    Sub Hddinfo(Optional ByVal QuietMode As Boolean = False, Optional ByVal KernelMode As Boolean = True)
        Try
            Dim HDDSet As Object                                                            'Sets of hard drive
            Dim Hdd As Object                                                               'Needed to get model and size of hard drive.
            HDDSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_DiskDrive")      'it gets Winmgmts: to SELECT * FROM Win32_DiskDrive
            Wdbg("Object created = {0}.Win32_DiskDrive", True, HDDSet)
            For Each Hdd In HDDSet
                Hddmodel = CStr(Hdd.Model)
                Hddsize = CStr(Hdd.Size) / 1024 / 1024 / 1024                               'Calculate size from Hard Drive in GB
                Wdbg("HDD Object = {0}.Win32_DiskDrive.Model = {1}, {0}.Win32_DiskDrive.Size = {2}", True, Hdd, Hddmodel, Hddsize)
                If (QuietMode = False And Hddsize <> Nothing And Hddmodel <> Nothing And KernelMode = True) Then
                    Wln("hwprobe: HDD: {0} {1}GB", "neutralText", Hddmodel, FormatNumber(Hddsize, 2))
                ElseIf (QuietMode = False And Hddsize <> Nothing And Hddmodel <> Nothing And KernelMode = False) Then
                    Wln("HDD: {0} {1}GB", "neutralText", Hddmodel, FormatNumber(Hddsize, 2))
                End If
            Next
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("C"), False, 0, "Error while trying to get hard drive information.")
        End Try

    End Sub

    Sub Cpuinfo()
        Try
            Dim CPUSet As Object                                                            'Sets of CPU
            Dim CPU As Object                                                               'Needed to get name and clock speed of CPU
            CPUSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_Processor")      'it gets Winmgmts: to SELECT * FROM Win32_Processor
            Wdbg("Object created = {0}.Win32_Processor", True, CPUSet)
            For Each CPU In CPUSet
                Cpuname = CStr(CPU.Name)                                                    'Get name of CPU
                Cpuspeed = CStr(CPU.CurrentClockSpeed)                                      'Get name of clock speed
                Wdbg("CPU Object = {0}.Win32_Processor.Name = {1}, {0}.Win32_Processor.CurrentClockSpeed = {2}", True, CPU, Cpuname, Cpuspeed)
            Next
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("C"), False, 0, "Error while trying to get CPU information.")
        End Try
    End Sub

    Sub SysMemory(Optional ByVal QuietMode As Boolean = False)
        Try
            Dim oInstance As Object
            Dim colInstances As Object
            Dim dRam As Double
            Dim colSlots As Object
            Dim oSlot As Object
            Dim temp = ""
            colInstances = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_PhysicalMemory")
            colSlots = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_PhysicalMemoryArray")
            Wdbg("colInstances = {0}.Win32_PhysicalMemory, colSlots = {1}.Win32_PhysicalMemoryArray", True, colInstances, colSlots)
            For Each oInstance In colInstances
                dRam = dRam + oInstance.Capacity                                            'Calculate RAM in bytes
                If (slotProbe = True) Then
                    slotsUsedName = slotsUsedName + oInstance.DeviceLocator + " "               'Get used slots
                End If
                temp = temp + CStr(oInstance.Capacity / 1024 / 1024) + " "
                Wdbg("oInstance = {0}.Win32_PhysicalMemory.Capacity = {1}, Total = {2}", True, oInstance, oInstance.Capacity, dRam)
            Next
            Capacities = temp.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
            If (slotProbe = True) Then
                slotsUsedNum = Capacities.Count()
                For Each oSlot In colSlots
                    totalSlots = oSlot.MemoryDevices
                    Wdbg("oSlot = {0}.Win32_PhysicalMemoryArray.MemoryDevices", True, oSlot, totalSlots)
                Next
                If (QuietMode = False) Then
                    Wln("hwprobe: RAM: Used slots (by names): {0}", "neutralText", slotsUsedName)
                    Wln("hwprobe: RAM: Used slots (by numbers): {0} / {1} ({2}%)", "neutralText", CStr(slotsUsedNum), totalSlots, FormatNumber(CStr(slotsUsedNum * 100 / totalSlots), 1))
                End If
            End If
            SysMem = dRam / 1024 / 1024 & "MB"                                              'Calculate RAM in MB
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("C"), False, 0, "Error while trying to get RAM information.")
        End Try
    End Sub

    Sub BiosInformation()
        Try
            Dim BiosInfoSpec As Object
            Dim Info As Object = GetObject("winmgmts:").ExecQuery("Select * from Win32_BIOS")
            Wdbg("Object created = {0}.Win32_BIOS", True, Info)
            For Each BiosInfoSpec In Info
                BIOSCaption = BiosInfoSpec.Name
                BIOSMan = BiosInfoSpec.Manufacturer
                BIOSSMBIOSVersion = BiosInfoSpec.SMBIOSBIOSVersion
                BIOSVersion = BiosInfoSpec.Version
                Wdbg("Bios Object = {0}.Win32_BIOS.Name = {1}, {0}.Win32_BIOS.Manufacturer = {2}, {0}.Win32_BIOS.SMBIOSBIOSVersion = {3}, {0}.Win32_BIOS.Version = {4}, ", True, BiosInfoSpec, BIOSCaption, BIOSMan, BIOSSMBIOSVersion, BIOSVersion)
            Next
            If (BIOSSMBIOSVersion = BIOSCaption) Then
                BIOSSMBIOSVersion = Nothing
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("C"), False, 0, "Error while trying to get BIOS information.")
        End Try

    End Sub

End Module
