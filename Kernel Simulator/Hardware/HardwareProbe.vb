
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

        If (QuietHWProbe = False) Then
            If (KernelUser = "K") Then
                If (ProbeFlag = True) Then
                    System.Console.WriteLine("hwprobe: Your hardware will be probed. Please wait...")
                    Cpuinfo()
                    System.Console.WriteLine("hwprobe: CPU: {0} {1}MHz", Cpuname, Cpuspeed)
                    SysMemory()
                    System.Console.WriteLine("hwprobe: RAM: {0} = {1}MB", SysMem, String.Join("MB + ", Capacities))
                    Hddinfo()
                    If (GPUProbeFlag = True) Then
                        ProbeGPU()
                    End If
                Else
                    System.Console.WriteLine("hwprobe: Hardware is not probed. Probe using 'hwprobe'")
                End If
            ElseIf (KernelUser = "U") Then
                If (ProbeFlag = False) Then
                    System.Console.WriteLine("hwprobe: Your hardware will be probed. Please wait...")
                    Cpuinfo()
                    System.Console.WriteLine("hwprobe: CPU: {0} {1}MHz", Cpuname, Cpuspeed)
                    SysMemory()
                    System.Console.WriteLine("hwprobe: RAM: {0} = {1}MB", SysMem, String.Join("MB + ", Capacities))
                    Hddinfo()
                    If (GPUProbeFlag = True) Then
                        ProbeGPU()
                    End If
                    ProbeFlag = True
                Else
                    System.Console.WriteLine("hwprobe: Hardware already probed.")
                End If
            End If
        ElseIf (QuietHWProbe = True) Then
            If (ProbeFlag = True) Then
                Cpuinfo()
                SysMemory(True)
                Hddinfo(True)
                If (GPUProbeFlag = True) Then
                    ProbeGPU()
                End If
            End If
        End If

    End Sub

    Sub ProbeBIOS(Optional ByVal QuietBIOS As Boolean = False)

        If (QuietBIOS = False) Then
            System.Console.WriteLine("hwprobe: Your BIOS will be probed. Please wait...")
            BiosInformation()
            System.Console.WriteLine("hwprobe: BIOS: {0} {1} {2} {3}", BIOSMan, BIOSCaption, BIOSVersion, BIOSSMBIOSVersion)
        ElseIf (QuietBIOS = True) Then
            BiosInformation()
        End If

    End Sub

    Sub ListDrivers()

        System.Console.WriteLine("CPU: {0} {1}MHz", Cpuname, Cpuspeed)
        System.Console.WriteLine("RAM: Used slots (by names): {0}", slotsUsedName)
        System.Console.WriteLine("RAM: Used slots (by numbers): {0} / {1} ({2}%)", CStr(slotsUsedNum), totalSlots, FormatNumber(CStr(slotsUsedNum * 100 / totalSlots), 1))
        System.Console.WriteLine("RAM: {0} = {1}MB", SysMem, String.Join("MB + ", Capacities))
        Hddinfo(False, False)
        ProbeGPU(False)

    End Sub

    'TODO: Re-organize parsers as one sub.

    Sub ProbeGPU(Optional ByVal KernelMode As Boolean = True)

        If ProbeFlag = True And GPUProbeFlag = True Then
            Dim colGPUs As Object
            Dim oGPU As Object

            colGPUs = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_VideoController")

            For Each oGPU In colGPUs
                If (Quiet = False And KernelMode = True) Then
                    System.Console.WriteLine("hwprobe: GPU: {0} {1}MB", oGPU.Caption, CStr(oGPU.AdapterRAM / 1024 / 1024))
                ElseIf (Quiet = False And KernelMode = False) Then
                    System.Console.WriteLine("GPU: {0} {1}MB", oGPU.Caption, CStr(oGPU.AdapterRAM / 1024 / 1024))
                End If
            Next
        End If

    End Sub

    Function Hddinfo(Optional ByVal QuietMode As Boolean = False, Optional ByVal KernelMode As Boolean = True) As Boolean
        Dim HDDSet As Object                                                            'Sets of hard drive
        Dim Hdd As Object                                                               'Needed to get model and size of hard drive.
        HDDSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_DiskDrive")      'it gets Winmgmts: to SELECT * FROM Win32_DiskDrive
        For Each Hdd In HDDSet
            Hddmodel = CStr(Hdd.Model)
            Hddsize = CStr(Hdd.Size) / 1024 / 1024 / 1024                               'Calculate size from Hard Drive in GB
            If (QuietMode = False And Hddsize <> Nothing And Hddmodel <> Nothing And KernelMode = True) Then
                System.Console.WriteLine("hwprobe: HDD: {0} {1}GB", Hddmodel, FormatNumber(Hddsize, 2))
            ElseIf (QuietMode = False And Hddsize <> Nothing And Hddmodel <> Nothing And KernelMode = False) Then
                System.Console.WriteLine("HDD: {0} {1}GB", Hddmodel, FormatNumber(Hddsize, 2))
            ElseIf (Hddsize = Nothing) Then
                KernelError("C", False, 0, "Error while trying to get hard drive size.")
            ElseIf (Hddmodel = Nothing) Then
                KernelError("C", False, 0, "Error while trying to get hard drive model.")
            End If
        Next
        Return True
    End Function

    Function Cpuinfo() As Boolean
        Dim CPUSet As Object                                                            'Sets of CPU
        Dim CPU As Object                                                               'Needed to get name and clock speed of CPU
        CPUSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_Processor")      'it gets Winmgmts: to SELECT * FROM Win32_Processor
        For Each CPU In CPUSet
            Cpuname = CStr(CPU.Name)                                                    'Get name of CPU
            Cpuspeed = CStr(CPU.CurrentClockSpeed)                                      'Get name of clock speed
        Next
        If (Cpuname = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get CPU name.")
        ElseIf (Cpuspeed = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get CPU speed.")
        End If
        Return True
    End Function

    Function SysMemory(Optional ByVal QuietMode As Boolean = False) As Boolean
        Dim oInstance As Object
        Dim colInstances As Object
        Dim dRam As Double
        Dim colSlots As Object
        Dim oSlot As Object
        Dim temp = ""
        colInstances = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_PhysicalMemory")
        colSlots = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_PhysicalMemoryArray")
        For Each oInstance In colInstances
            dRam = dRam + oInstance.Capacity                                            'Calculate RAM in bytes
            slotsUsedName = slotsUsedName + oInstance.DeviceLocator + " "               'Get used slots
            temp = temp + CStr(oInstance.Capacity / 1024 / 1024) + " "
        Next
        Capacities = temp.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        slotsUsedNum = Capacities.Count()
        For Each oSlot In colSlots
            totalSlots = oSlot.MemoryDevices
        Next
        If (QuietMode = False) Then
            System.Console.WriteLine("hwprobe: RAM: Used slots (by names): {0}", slotsUsedName)
            System.Console.WriteLine("hwprobe: RAM: Used slots (by numbers): {0} / {1} ({2}%)", CStr(slotsUsedNum), totalSlots, FormatNumber(CStr(slotsUsedNum * 100 / totalSlots), 1))
        End If
        SysMem = dRam / 1024 / 1024 & "MB"                                              'Calculate RAM in MB
        If (dRam = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get RAM size.")
        ElseIf (slotsUsedName = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get installed slots in names.")
        End If
        Return True
    End Function

    Function BiosInformation() As Boolean
        On Error Resume Next
        Dim BiosInfoSpec As Object
        Dim Info As Object = GetObject("winmgmts:").ExecQuery("Select * from Win32_BIOS")
        For Each BiosInfoSpec In Info
            BIOSCaption = BiosInfoSpec.Name
            BIOSMan = BiosInfoSpec.Manufacturer
            BIOSSMBIOSVersion = BiosInfoSpec.SMBIOSBIOSVersion
            BIOSVersion = BiosInfoSpec.Version
        Next
        If (BIOSCaption = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get BIOS name.")
        ElseIf (BIOSMan = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get BIOS manufacturer.")
        ElseIf (BIOSSMBIOSVersion = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get BIOS version from SMBIOS.")
        ElseIf (BIOSVersion = Nothing) Then
            KernelError(CChar("C"), False, 0, "Error while trying to get BIOS version.")
        ElseIf (BIOSSMBIOSVersion = BIOSCaption) Then
            BIOSSMBIOSVersion = Nothing
        End If
        Return True
    End Function

End Module
