
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

Public Module HardwareProbe

    'TODO: Re-write in Beta
    Public Sub ProbeHW(Optional ByVal QuietHWProbe As Boolean = False, Optional ByVal KernelUser As Char = CChar("U"))

        Wdbg("QuietHWProbe = {0}, KernelUser = {1}, ProbeFlag = {2}.", True, QuietHWProbe, KernelUser, ProbeFlag)
        If (QuietHWProbe = False) Then
                If (ProbeFlag = True And KernelUser = "K") Or (ProbeFlag = False And KernelUser = "U") Then
                    Wln("hwprobe: Your hardware will be probed. Please wait...", "neutralText")
                    StartProbing()
                ElseIf (KernelUser = "U" And ProbeFlag = True) Then
                    Wln("hwprobe: Hardware already probed.", "neutralText")
                ElseIf (ProbeFlag = False And KernelUser = "K") Then
                    Wln("hwprobe: Hardware is not probed. Probe using 'hwprobe'", "neutralText")
                End If
        ElseIf (QuietHWProbe = True) Then
            If (ProbeFlag = True) Then
                Cpuinfo()
                SysMemory(True)
                Hddinfo(True)
                ProbeGPU(True, True)
                BiosInformation()
            End If
        End If

    End Sub

    'This sub has fixed disorganization.
    Public Sub StartProbing()
        'We will probe first the CPU
        Cpuinfo()

        'then RAM
        SysMemory()

        'then HDD
        Hddinfo()

        'then GPU
        ProbeGPU(True, False)

        'and finally BIOS
        BiosInformation()

        'We are checking to see if any of the probers reported a failure starting with CPU
        If (CPUDone = False) Then
            Wdbg("CPU failed to probe.", True, KernelVersion)
            Wln("CPU: failed to probe", "neutralText")
        End If

        'then RAM
        If (RAMDone = False) Then
            Wdbg("RAM failed to probe.", True, KernelVersion)
            Wln("RAM: failed to probe", "neutralText")
        End If

        'then HDD
        If (HDDDone = False) Then
            Wdbg("HDD failed to probe.", True, KernelVersion)
            Wln("HDD: failed to probe", "neutralText")
        End If

        'then GPU
        If (GPUDone = False) Then
            Wdbg("GPU failed to probe.", True, KernelVersion)
            Wln("GPU: failed to probe", "neutralText")
        End If

        'and finally BIOS
        If (BIOSDone = False) Then
            Wdbg("BIOS failed to probe.", True, KernelVersion)
            Wln("BIOS: failed to probe", "neutralText")
        End If

        'Inform users
        ProbeFlag = True
        ListDrivers()
    End Sub

    Public Sub ListDrivers()

        'CPU Info
        If (CPUDone = True) Then
            For Each processorinfo In CPUList
                Wln("CPU: {0} {1}MHz", "neutralText", processorinfo.Name, processorinfo.ClockSpeed)
            Next
            Wln("CPU: Total number of processors: {0}", "neutralText", Environment.ProcessorCount)
        End If

        'RAM Info
        If (RAMDone = True) Then
            Dim times As Integer = 1
            Dim total As UInt64
            Wdbg("RAM probed.", True, KernelVersion)

            'This is an expression totalling a RAM to print string
            For Each capacity In Capacities
                total = total + capacity
            Next

            'Print some info
            Wln("RAM: {0}MB = {1}MB", "neutralText", CStr(total), String.Join("MB + ", Capacities))

            'A loop to print names
            For Each slotname In RAMList
                If (times = 1) Then
                    W("RAM: Used slots (by names): {0} ", "neutralText", slotname.SlotName)
                Else
                    W("{0} ", "neutralText", slotname.SlotName)
                End If
                times += 1
            Next
            times = 1

            'Number of slots used
            Wln(vbNewLine + "RAM: Used slots (by numbers): {0} / {1} ({2}%)", "neutralText", CStr(slotsUsedNum), totalSlots, FormatNumber(CStr(slotsUsedNum * 100 / totalSlots), 1))

            'This one is a necessary string, so we will do another loop.
            For Each memstat In RAMList
                If (times = 1) Then
                    W("RAM: {0}: {1} ", "neutralText", memstat.SlotName, memstat.Status)
                Else
                    W("| {0}: {1} ", "neutralText", memstat.SlotName, memstat.Status)
                End If
                times += 1
            Next
            Wln(vbNewLine + "RAM: Probing status is deprecated and will be removed in future release.", "neutralText")
        End If

        'GPU Info
        If (GPUDone = True) Then
            For Each gpuinfo In GPUList
                If (gpuinfo.Name = "Microsoft Basic Display Adapter" Or gpuinfo.Name = "Standard VGA Graphics Adapter") Then
                    Wln("GPU: No appropriate driver installed.", "neutralText")
                Else
                    Wln("GPU: {0} {1}MB", "neutralText", gpuinfo.Name, CStr(gpuinfo.Memory / 1024 / 1024))
                End If
            Next
        End If

        'Drive Info
        If (HDDDone = True) Then
            For Each driveinfo In HDDList
                If (driveinfo.Manufacturer = "(Standard disk drives)") Then
                    Wln("HDD: {0} {1}GB" + vbNewLine + _
                        "HDD: Type: {2} | Status: {3}" + vbNewLine + _
                        "HDD: CHS: {4} cylinders | {5} heads | {6} sectors", "neutralText", _
                        driveinfo.Model, FormatNumber(driveinfo.Size / 1024 / 1024 / 1024, 2), _
                        driveinfo.InterfaceType, driveinfo.Status, driveinfo.Cylinders, _
                        driveinfo.Heads, driveinfo.Sectors)
                Else
                    Wln("HDD: {0} {1} {2}GB" + vbNewLine + _
                        "HDD: Type: {3} | Status: {4}" + vbNewLine + _
                        "HDD: CHS: {5} cylinders | {6} heads | {7} sectors", "neutralText", _
                        driveinfo.Manufacturer, driveinfo.Model, FormatNumber(driveinfo.Size / 1024 / 1024 / 1024, 2), _
                        driveinfo.InterfaceType, driveinfo.Status, driveinfo.Cylinders, _
                        driveinfo.Heads, driveinfo.Sectors)
                End If
            Next
        End If

        'BIOS Info
        If (BIOSDone = True) Then
            For Each BIOSInfo In BIOSList
                Wln("BIOS: {0} {1} {2} {3}", "neutralText", BIOSInfo.Manufacturer, BIOSInfo.Name, BIOSInfo.Version, BIOSInfo.SMBIOSVersion)
            Next
        End If

    End Sub

    'TODO: Re-organize parsers as one sub.

    Public Sub ProbeGPU(Optional ByVal KernelMode As Boolean = True, Optional ByVal QuietMode As Boolean = False)

        Try
            If ProbeFlag = True Then
                GPUDone = True

                Dim colGPUs As Object = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_VideoController")
                Wdbg("Object created = {0}.Win32_VideoController", True, colGPUs)

                For Each oGPU As Object In colGPUs
                    GPUList.Add(New GPU With {.Name = oGPU.Caption, .Memory = oGPU.AdapterRAM})
                    Wdbg("GPU Object = {0}.Win32_VideoController.Caption = {1}, {0}.Win32_VideoController.AdapterRAM = {2}", True, oGPU, oGPU.Caption, oGPU.AdapterRAM)
                Next
            End If
        Catch ex As Exception
            GPUDone = False
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
        End Try

    End Sub

    Public Sub Hddinfo(Optional ByVal QuietMode As Boolean = False)
        Try
            HDDDone = True

            Dim HDDSet As Object = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_DiskDrive")
            Wdbg("Object created = {0}.Win32_DiskDrive", True, HDDSet)

            For Each Hdd As Object In HDDSet
                HDDList.Add(New HDD With {.Model = Hdd.Model, .Size = Hdd.Size, .Status = Hdd.Status, _
                                          .Manufacturer = Hdd.Manufacturer, .Cylinders = Hdd.TotalCylinders, .Heads = Hdd.TotalHeads, _
                                          .Sectors = Hdd.TotalSectors, .InterfaceType = Hdd.InterfaceType})
                Wdbg("HDD Object = {0}.Win32_DiskDrive.Manufacturer = {3}, {0}.Win32_DiskDrive.Model = {1}, {0}.Win32_DiskDrive.Size = {2}, " + _
                     "{0}.Win32_DiskDrive.InterfaceType = {4}, {0}.Win32_DiskDrive.Status = {5}, CHS: {6}, {7}, {8}", True, _
                     Hdd, HDDList(HDDList.Count - 1).Model, HDDList(HDDList.Count - 1).Size, HDDList(HDDList.Count - 1).Manufacturer, _
                     HDDList(HDDList.Count - 1).InterfaceType, HDDList(HDDList.Count - 1).Status, HDDList(HDDList.Count - 1).Cylinders, _
                     HDDList(HDDList.Count - 1).Heads, HDDList(HDDList.Count - 1).Sectors)
            Next
        Catch ex As Exception
            HDDDone = False
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
        End Try

    End Sub

    Public Sub Cpuinfo()
        Try
            CPUDone = True

            Dim CPUSet As Object = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_Processor")
            Wdbg("Object created = {0}.Win32_Processor", True, CPUSet)

            For Each CPU As Object In CPUSet
                CPUList.Add(New CPU With {.Name = CPU.Name, .ClockSpeed = CPU.CurrentClockSpeed})
                Wdbg("CPU Object = {0}.Win32_Processor.Name = {1}, {0}.Win32_Processor.CurrentClockSpeed = {2}", True, CPU, CPUList(CPUList.Count - 1).Name, CPUList(CPUList.Count - 1).ClockSpeed)
            Next
        Catch ex As Exception
            CPUDone = False
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
        End Try
    End Sub

    Public Sub SysMemory(Optional ByVal QuietMode As Boolean = False)
        Try
            RAMDone = True

            Dim colInstances As Object = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_PhysicalMemory")
            Dim colSlots As Object = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_PhysicalMemoryArray")
            Dim temp = ""

            Wdbg("colInstances = {0}.Win32_PhysicalMemory, colSlots = {1}.Win32_PhysicalMemoryArray", True, colInstances, colSlots)

            For Each oInstance In colInstances
                RAMList.Add(New RAM With {.ChipCapacity = oInstance.Capacity, .SlotName = "", .SlotNumber = 0, .Status = ""})
                If (slotProbe = True) Then
                    RAMList(RAMList.Count - 1).SlotName = oInstance.DeviceLocator
                    If (oInstance.Status.ToString = "") Then
                        RAMList(RAMList.Count - 1).Status = "Unknown"
                    Else
                        RAMList(RAMList.Count - 1).Status = oInstance.Status.ToString
                    End If
                End If
                temp = temp + CStr(RAMList(RAMList.Count - 1).ChipCapacity / 1024 / 1024) + " "
                Wdbg("oInstance = {0}.Win32_PhysicalMemory.Capacity = {1}", True, oInstance, RAMList(RAMList.Count - 1).ChipCapacity)
            Next

            Capacities = temp.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

            If (slotProbe = True) Then
                RAMList(RAMList.Count - 1).SlotNumber = Capacities.Count() : slotsUsedNum = Capacities.Count()
                For Each oSlot In colSlots
                    totalSlots = oSlot.MemoryDevices
                    Wdbg("oSlot = {0}.Win32_PhysicalMemoryArray.MemoryDevices | totalSlots = {1}", True, oSlot, totalSlots)
                Next
            End If
        Catch ex As Exception
            RAMDone = False
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
        End Try
    End Sub

    Public Sub BiosInformation()
        Try
            BIOSDone = True

            Dim Info As Object = GetObject("winmgmts:").ExecQuery("Select * from Win32_BIOS")
            Wdbg("Object created = {0}.Win32_BIOS", True, Info)

            For Each BiosInfoSpec As Object In Info
                BIOSList.Add(New BIOS With {.Name = BiosInfoSpec.Name, .Manufacturer = BiosInfoSpec.Manufacturer, .SMBIOSVersion = BiosInfoSpec.SMBIOSBIOSVersion, _
                                            .Version = BiosInfoSpec.Version})
                Wdbg("Bios Object = {0}.Win32_BIOS.Name = {1}, {0}.Win32_BIOS.Manufacturer = {2}, {0}.Win32_BIOS.SMBIOSBIOSVersion = {3}, {0}.Win32_BIOS.Version = {4}, ", True, BiosInfoSpec, BIOSList(BIOSList.Count - 1).Name, BIOSList(BIOSList.Count - 1).Manufacturer, BIOSList(BIOSList.Count - 1).SMBIOSVersion, BIOSList(BIOSList.Count - 1).Version)
                If (BIOSList(BIOSList.Count - 1).SMBIOSVersion = BIOSList(BIOSList.Count - 1).Name) Then
                    BIOSList(BIOSList.Count - 1).SMBIOSVersion = Nothing
                End If
            Next
        Catch ex As Exception
            BIOSDone = False
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
        End Try

    End Sub

End Module
