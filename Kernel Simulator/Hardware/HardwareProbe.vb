
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
Imports System.Management

Public Module HardwareProbe

    'TODO: Re-write in Beta
    'TODO: Remove the BIOS and GPU probing until the final release.
    Public Sub ProbeHW(Optional ByVal QuietHWProbe As Boolean = False)

        Wdbg("QuietHWProbe = {0}.", QuietHWProbe)
        If (QuietHWProbe = False) Then
            Wln(DoTranslation("hwprobe: Your hardware will be probed. Please wait...", currentLang), "neutralText")
            StartProbing()
        ElseIf (QuietHWProbe = True) Then
            Cpuinfo()
            SysMemory()
            Hddinfo()
            ProbeGPU()
            BiosInformation()
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
        ProbeGPU()

        'and finally BIOS
        BiosInformation()

        'We are checking to see if any of the probers reported a failure starting with CPU
        If (CPUDone = False) Then
            Wdbg("CPU failed to probe.", KernelVersion)
            Wln(DoTranslation("CPU: One or more of the CPU cores failed to be probed. Showing information anyway...", currentLang), "neutralText")
        End If

        'then RAM
        If (RAMDone = False) Then
            Wdbg("RAM failed to probe.", KernelVersion)
            Wln(DoTranslation("RAM: One or more of the RAM chips failed to be probed. Showing information anyway...", currentLang), "neutralText")
        End If

        'then HDD
        If (HDDDone = False) Then
            Wdbg("HDD failed to probe.", KernelVersion)
            Wln(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway...", currentLang), "neutralText")
        End If

        'then GPU
        If (GPUDone = False) Then
            Wdbg("GPU failed to probe.", KernelVersion)
            Wln(DoTranslation("GPU: One or more of the graphics card failed to be probed. Showing information anyway...", currentLang), "neutralText")
        End If

        'and finally BIOS
        If (BIOSDone = False) Then
            Wdbg("BIOS failed to probe.", KernelVersion)
            Wln(DoTranslation("BIOS: One or more of the BIOS chips failed to be probed. Showing information anyway...", currentLang), "neutralText")
        End If

        'Print information about the probed hardware
        ListDrivers()
    End Sub

    Public Sub ListDrivers()

        'CPU Info
        For Each processorinfo In CPUList
            Wln(DoTranslation("CPU: {0} {1}MHz", currentLang), "neutralText", processorinfo.Name, processorinfo.ClockSpeed)
        Next
        Wln(DoTranslation("CPU: Total number of processors: {0}", currentLang), "neutralText", Environment.ProcessorCount)

        'RAM Info
        Dim times As Integer = 1
        Dim total As UInt64
        Wdbg("RAM probed.", KernelVersion)

        'This is an expression totalling a RAM to print string
        For Each capacity In Capacities
            total = total + capacity
        Next

        'Print some info
        Wln(DoTranslation("RAM: {0}MB = {1}MB", currentLang), "neutralText", CStr(total), String.Join("MB + ", Capacities))

        'A loop to print names
        For Each slotname In RAMList
            If (times = 1) Then
                W(DoTranslation("RAM: Used slots (by names): {0}", currentLang), "neutralText", slotname.SlotName)
            Else
                W(" {0}", "neutralText", slotname.SlotName)
            End If
            times += 1
        Next
        times = 1

        'Number of slots used
        Wln(vbNewLine + DoTranslation("RAM: Used slots (by numbers): {0} / {1} ({2}%)", currentLang), "neutralText", CStr(slotsUsedNum), totalSlots, FormatNumber(CStr(slotsUsedNum * 100 / totalSlots), 1))

        'GPU Info
        Dim basics As String() = {"Microsoft Basic Display Driver", "Microsoft Basic Display Adapter", "Standard VGA Graphics Adapter"}
        For Each gpuinfo In GPUList
            'Check to see if the graphics card driver is basic (Standard VGA Graphics Adapter for Windows 7 or below, Microsoft Basic Display Adapter for Windows 8/8.1, and Microsoft Basic Display Driver for Windows 10)
            If (basics.Contains(gpuinfo.Name)) Then
                Wln(DoTranslation("GPU: No appropriate driver installed.", currentLang), "neutralText")
            Else
                Wln(DoTranslation("GPU: {0} {1}MB", currentLang), "neutralText", gpuinfo.Name, CStr(gpuinfo.Memory / 1024 / 1024))
            End If
        Next

        'Drive Info
        For Each driveinfo In HDDList
            If (driveinfo.Manufacturer = "(Standard disk drives)") Then
                Wln(DoTranslation("HDD: {0} {1}GB {2}", currentLang) + vbNewLine +
                    DoTranslation("HDD: CHS: {3} cylinders | {4} heads | {5} sectors", currentLang), "neutralText",
                    driveinfo.Model, FormatNumber(driveinfo.Size / 1024 / 1024 / 1024, 2),
                    driveinfo.InterfaceType, driveinfo.Cylinders,
                    driveinfo.Heads, driveinfo.Sectors)
            Else
                Wln(DoTranslation("HDD: {0} {1} {2}GB {3}", currentLang) + vbNewLine +
                    DoTranslation("HDD: CHS: {4} cylinders | {5} heads | {6} sectors", currentLang), "neutralText",
                    driveinfo.Manufacturer, driveinfo.Model, FormatNumber(driveinfo.Size / 1024 / 1024 / 1024, 2),
                    driveinfo.InterfaceType, driveinfo.Cylinders,
                    driveinfo.Heads, driveinfo.Sectors)
            End If
        Next

        'BIOS Info
        For Each BIOSInfo In BIOSList
            Wln(DoTranslation("BIOS: {0} {1} {2} {3}", currentLang), "neutralText", BIOSInfo.Manufacturer, BIOSInfo.Name, BIOSInfo.Version, BIOSInfo.SMBIOSVersion)
        Next

    End Sub

    'TODO: Re-organize parsers as one sub after removing GPU and BIOS probers.

    <Obsolete> Public Sub ProbeGPU()

        Dim colGPUs As New ManagementObjectSearcher("SELECT * FROM Win32_VideoController")
        GPUDone = True
        Wdbg("Searcher created = colGPUs.Win32_VideoController")
        For Each oGPU As ManagementBaseObject In colGPUs.Get
            Try
                If IsNothing(oGPU("AdapterRAM")) Then
                    oGPU("AdapterRAM") = 0
                End If
                GPUList.Add(New GPU With {.Name = oGPU("Caption"), .Memory = oGPU("AdapterRAM")})
                Wdbg("GPU: oGPU.Win32_VideoController.Caption = {0}, oGPU.Win32_VideoController.AdapterRAM = {1}", oGPU("Caption"), oGPU("AdapterRAM").ToString)
            Catch ex As Exception
                GPUDone = False
                If (DebugMode = True) Then
                    Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                End If
                Continue For
            End Try
        Next

    End Sub

    Public Sub Hddinfo()
        HDDDone = True
        Dim HDDSet As New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")
        Wdbg("Searcher created = HDDSet.Win32_DiskDrive")
        For Each Hdd As ManagementBaseObject In HDDSet.Get
            Try
                HDDList.Add(New HDD With {.Model = Hdd("Model"), .Size = Hdd("Size"), .Manufacturer = Hdd("Manufacturer"),
                                          .Cylinders = Hdd("TotalCylinders"), .Heads = Hdd("TotalHeads"), .Sectors = Hdd("TotalSectors"),
                                          .InterfaceType = Hdd("InterfaceType")})
                Wdbg("HDD: Hdd.Win32_DiskDrive.Manufacturer = {2}, Hdd.Win32_DiskDrive.Model = {0}, Hdd.Win32_DiskDrive.Size = {1}, " +
                     "Hdd.Win32_DiskDrive.InterfaceType = {3}, CHS: {4}, {5}, {6}",
                     HDDList(HDDList.Count - 1).Model, HDDList(HDDList.Count - 1).Size, HDDList(HDDList.Count - 1).Manufacturer,
                     HDDList(HDDList.Count - 1).InterfaceType, HDDList(HDDList.Count - 1).Cylinders,
                     HDDList(HDDList.Count - 1).Heads, HDDList(HDDList.Count - 1).Sectors)
            Catch ex As Exception
                HDDDone = False
                If (DebugMode = True) Then
                    Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                End If
                Continue For
            End Try
        Next

    End Sub

    Public Sub Cpuinfo()
        CPUDone = True
        Dim CPUSet As New ManagementObjectSearcher("SELECT * FROM Win32_Processor")
        Wdbg("Searcher created = CPUSet.Win32_Processor")
        For Each CPU As ManagementBaseObject In CPUSet.Get
            Try
                CPUList.Add(New CPU With {.Name = CPU("Name"), .ClockSpeed = CPU("CurrentClockSpeed")})
                Wdbg("CPU: CPU.Win32_Processor.Name = {0}, CPU.Win32_Processor.CurrentClockSpeed = {1}", CPUList(CPUList.Count - 1).Name, CPUList(CPUList.Count - 1).ClockSpeed)
            Catch ex As Exception
                CPUDone = False
                If (DebugMode = True) Then
                    Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                End If
                Continue For
            End Try
        Next
    End Sub

    Public Sub SysMemory()
        RAMDone = True
        Dim colInstances As New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory")
        Dim colSlots As New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemoryArray")
        Dim temp = ""
        Wdbg("colInstances = Win32_PhysicalMemory, colSlots = Win32_PhysicalMemoryArray")
        For Each oInstance As ManagementBaseObject In colInstances.Get
            Try
                RAMList.Add(New RAM With {.ChipCapacity = oInstance("Capacity"), .SlotName = "", .SlotNumber = 0})
                If (slotProbe = True) Then
                    RAMList(RAMList.Count - 1).SlotName = oInstance("DeviceLocator")
                End If
                temp = temp + CStr(RAMList(RAMList.Count - 1).ChipCapacity / 1024 / 1024) + " "
                Wdbg("oInstance.Win32_PhysicalMemory.Capacity = {0}", RAMList(RAMList.Count - 1).ChipCapacity)
            Catch ex As Exception
                RAMDone = False
                If (DebugMode = True) Then
                    Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                End If
                Continue For
            End Try
        Next
        Capacities = temp.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        If (slotProbe = True) Then
            RAMList(RAMList.Count - 1).SlotNumber = Capacities.Count() : slotsUsedNum = Capacities.Count()
            For Each oSlot As ManagementBaseObject In colSlots.Get
                Try
                    totalSlots = oSlot("MemoryDevices")
                    Wdbg("oSlot = Win32_PhysicalMemoryArray.MemoryDevices | totalSlots = {0}", totalSlots)
                Catch ex As Exception
                    RAMDone = False
                    If (DebugMode = True) Then
                        Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                    End If
                    Continue For
                End Try
            Next
        End If
    End Sub

    <Obsolete> Public Sub BiosInformation()
        BIOSDone = True
        Dim Info As New ManagementObjectSearcher("SELECT * FROM Win32_BIOS")
        Wdbg("Searcher created = Info.Win32_BIOS")
        For Each BiosInfoSpec As ManagementBaseObject In Info.Get
            Try
                BIOSList.Add(New BIOS With {.Name = BiosInfoSpec("Name"), .Manufacturer = BiosInfoSpec("Manufacturer"),
                                            .SMBIOSVersion = BiosInfoSpec("SMBIOSBIOSVersion"), .Version = BiosInfoSpec("Version")})
                Wdbg("Bios Object = BiosInfoSpec.Win32_BIOS.Name = {0}, BiosInfoSpec.Win32_BIOS.Manufacturer = {1}, BiosInfoSpec.Win32_BIOS.SMBIOSBIOSVersion = {2}, BiosInfoSpec.Win32_BIOS.Version = {3}, ",
                     BIOSList(BIOSList.Count - 1).Name, BIOSList(BIOSList.Count - 1).Manufacturer, BIOSList(BIOSList.Count - 1).SMBIOSVersion, BIOSList(BIOSList.Count - 1).Version)
                If (BIOSList(BIOSList.Count - 1).SMBIOSVersion = BIOSList(BIOSList.Count - 1).Name) Then
                    BIOSList(BIOSList.Count - 1).SMBIOSVersion = Nothing
                End If
            Catch ex As Exception
                BIOSDone = False
                If (DebugMode = True) Then
                    Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                End If
                Continue For
            End Try
        Next
    End Sub

End Module
