
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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
Imports System.Management
Imports Newtonsoft.Json.Linq

Public Module HardwareProbe

    'TODO: Re-write in Beta
    Public Sub ProbeHW()
        Wdbg("I", "QuietProbe = {0}.", quietProbe)
        If Not quietProbe Then
            W(DoTranslation("hwprobe: Your hardware will be probed. Please wait...", currentLang), True, ColTypes.Neutral)
            StartProbing()
        Else
            If Not EnvironmentOSType.Contains("Unix") Then
                ProbeHardware()
            Else
                ProbeHardwareLinux()
            End If
        End If
    End Sub

    Public Sub StartProbing()
        'We will probe hardware
        If Not EnvironmentOSType.Contains("Unix") Then
            ProbeHardware()
        Else
            ProbeHardwareLinux()
        End If

        'We are checking to see if any of the probers reported a failure starting with CPU
        If CPUDone = False Then
            Wdbg("E", "CPU failed to probe.", KernelVersion)
            W(DoTranslation("CPU: One or more of the CPU cores failed to be probed. Showing information anyway...", currentLang), True, ColTypes.Neutral)
        End If

        'then RAM
        If RAMDone = False Then
            Wdbg("E", "RAM failed to probe.", KernelVersion)
            W(DoTranslation("RAM: One or more of the RAM chips failed to be probed. Showing information anyway...", currentLang), True, ColTypes.Neutral)
        End If

        'and finally HDD
        If HDDDone = False Then
            Wdbg("E", "HDD failed to probe.", KernelVersion)
            W(DoTranslation("HDD: One or more of the hard drives failed to be probed. Showing information anyway...", currentLang), True, ColTypes.Neutral)
        End If

        'Print information about the probed hardware
        If Not EnvironmentOSType.Contains("Unix") Then
            ListDrivers()
        Else
            ListDrivers_Linux()
        End If
    End Sub

    '----------> Windows hardware probers <----------
    Public Sub ProbeHardware()
        'Variables
        Dim HDDSet As New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")
        Dim PartsS As New ManagementObjectSearcher("SELECT * FROM Win32_DiskPartition")
        Dim LogicS As New ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk")
        Dim CPUSet As New ManagementObjectSearcher("SELECT * FROM Win32_Processor")
        Dim MemSet As New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory")
        Dim SltSet As New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemoryArray")
        Dim temp As String = ""

        'Done sets
        HDDDone = True
        ParDone = True
        CPUDone = True
        RAMDone = True

        'HDD Prober
        For Each Hdd As ManagementBaseObject In HDDSet.Get
            Try
                HDDList.Add(New HDD With {.Model = Hdd("Model"), .Size = Hdd("Size"), .Manufacturer = Hdd("Manufacturer"),
                                          .Cylinders = Hdd("TotalCylinders"), .Heads = Hdd("TotalHeads"), .Sectors = Hdd("TotalSectors"),
                                          .InterfaceType = Hdd("InterfaceType"), .ID = Hdd("DeviceID")})
                Wdbg("I", "HDD: Manufacturer = {2}, Model = {0}, Size = {1}, InterfaceType = {3}, CHS: {4}, {5}, {6}, ID: {7}",
                     HDDList(HDDList.Count - 1).Model, HDDList(HDDList.Count - 1).Size, HDDList(HDDList.Count - 1).Manufacturer,
                     HDDList(HDDList.Count - 1).InterfaceType, HDDList(HDDList.Count - 1).Cylinders, HDDList(HDDList.Count - 1).Heads,
                     HDDList(HDDList.Count - 1).Sectors, HDDList(HDDList.Count - 1).ID)
            Catch ex As Exception
                HDDDone = False
                If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                Continue For
            End Try
        Next

        'Partition Prober
        Dim DriveNo As Integer
        For Each Manage As ManagementBaseObject In PartsS.Get
            Try
                DriveNo = Manage("DiskIndex")
                HDDList(DriveNo).Parts.Add(New Part With {.Boot = Manage("BootPartition"), .Bootable = Manage("Bootable"), .Primary = Manage("PrimaryPartition"),
                                                          .Size = Manage("Size"), .Type = Manage("Type")})
            Catch ex As Exception
                ParDone = False
                If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                Continue For
            End Try
        Next
        For Each LogicM As ManagementBaseObject In LogicS.Get
            Try
                If LogicM("DriveType") = 3 Then
                    LogList.Add(New Logical With {.Compressed = LogicM("Compressed"), .FileSystem = LogicM("FileSystem"), .Name = LogicM("Name"),
                                                  .Size = LogicM("Size"), .Free = LogicM("FreeSpace")})
                End If
            Catch ex As Exception
                ParDone = False
                If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                Continue For
            End Try
        Next

        'CPU Prober
        For Each CPU As ManagementBaseObject In CPUSet.Get
            Try
                CPUList.Add(New CPU With {.Name = CPU("Name"), .ClockSpeed = CPU("CurrentClockSpeed")})
                Wdbg("I", "CPU: Name = {0}, CurrentClockSpeed = {1}", CPUList(CPUList.Count - 1).Name, CPUList(CPUList.Count - 1).ClockSpeed)
            Catch ex As Exception
                CPUDone = False
                If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                Continue For
            End Try
        Next

        'Memory Prober
        For Each RAM As ManagementBaseObject In MemSet.Get
            Try
                RAMList.Add(New RAM With {.ChipCapacity = RAM("Capacity"), .SlotName = "", .SlotNumber = 0})
                If slotProbe = True Then RAMList(RAMList.Count - 1).SlotName = RAM("DeviceLocator")
                temp += CStr(RAMList(RAMList.Count - 1).ChipCapacity / 1024 / 1024) + " "
                Wdbg("I", "RAM: Capacity = {0}", RAMList(RAMList.Count - 1).ChipCapacity)
            Catch ex As Exception
                RAMDone = False
                If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                Continue For
            End Try
        Next
        Capacities = temp.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

        'Slot Prober
        If slotProbe Then
            RAMList(RAMList.Count - 1).SlotNumber = Capacities.Count() : slotsUsedNum = Capacities.Count()
            For Each Slot As ManagementBaseObject In SltSet.Get
                Try
                    totalSlots = Slot("MemoryDevices")
                    Wdbg("I", "RAM: totalSlots = {0}", totalSlots)
                Catch ex As Exception
                    RAMDone = False
                    If DebugMode Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                    Continue For
                End Try
            Next
        End If

        'Dispose all
        HDDSet.Dispose()
        CPUSet.Dispose()
        MemSet.Dispose()
        SltSet.Dispose()
    End Sub

    '----------> Linux hardware prober <----------
    'This uses inxi to get information about HDD. If you don't have it installed, use your appropriate package manager to install inxi or build from source.
    Public Sub ProbeHardwareLinux()
        'Done sets
        HDDDone = True
        CPUDone = True
        RAMDone = True

        'CPU Prober
        Try
            Dim cpuinfo As New StreamReader("/proc/cpuinfo")
            Dim Name = "", Clock = "", SSE2 = False, ln As String
            Do While Not cpuinfo.EndOfStream
                ln = cpuinfo.ReadLine()
                If ln.StartsWith("model name") Then
                    Name = ln.Replace("model name" + vbTab + ": ", "")
                ElseIf ln.StartsWith("cpu MHz") Then
                    Clock = ln.Substring(ln.IndexOfAny({"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}))
                ElseIf ln.StartsWith("flags") Then
                    If ln.Contains("sse2") Then
                        SSE2 = True
                    End If
                End If
            Loop
            CPUList.Add(New CPU_Linux With {.Clock = Clock, .CPUName = Name, .SSE2 = SSE2})
        Catch ex As Exception
            CPUDone = False
            KernelError("C", False, 0, DoTranslation("Error while checking CPU: {0}", currentLang), ex, ex.Message)
            If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
        End Try

        'RAM Prober
        Try
            Dim raminfo As New StreamReader("/proc/meminfo")
            Dim mem As String = raminfo.ReadLine()
            mem = mem.Substring(mem.IndexOfAny({"1", "2", "3", "4", "5", "6", "7", "8", "9", "0"}))
            RAMList.Add(New RAM_Linux With {.Capacity = mem})
        Catch ex As Exception
            RAMDone = False
            KernelError("C", False, 0, DoTranslation("Error while checking RAM: {0}", currentLang), ex, ex.Message)
            If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
        End Try

        'HDD Prober (You need to have inxi and libcpanel-json-xs-perl installed)
        Try
            Dim inxi As New Process
            Dim inxinfo As New ProcessStartInfo With {.FileName = "/usr/bin/inxi", .Arguments = "-P -D --output json --output-file print",
                                                      .WindowStyle = ProcessWindowStyle.Hidden,
                                                      .CreateNoWindow = True,
                                                      .UseShellExecute = False,
                                                      .RedirectStandardOutput = True}
            inxi.StartInfo = inxinfo
            inxi.Start() : inxi.WaitForExit()
            Dim inxiout As String = inxi.StandardOutput.ReadToEnd
            If Not inxiout.StartsWith("{") And Not inxiout.EndsWith("}") Then 'If an error appeared while running perl
                HDDDone = False
                Wdbg("I", inxiout)
                W(DoTranslation("You may not have libcpanel-json-xs-perl installed on your system. Refer to your package manager for installation. For Debian (and derivatives) systems, you might want to run ""sudo apt install libcpanel-json-xs-perl"" in the terminal emulator. More details of an error:", currentLang) + vbNewLine + inxiout, True, ColTypes.Err)
                Exit Sub
            End If
            Dim inxitoken As JToken = JToken.Parse(inxiout)
            Dim inxiReady As Boolean = False
            For Each inxidrvs In inxitoken.SelectToken("000#Drives")
                If inxiReady Then
                    HDDList.Add(New HDD_Linux With {.Size_LNX = inxidrvs("004#size"), .Model_LNX = inxidrvs("003#model"), .Vendor_LNX = inxidrvs("002#vendor")})
                End If
                inxiReady = True
            Next
            Dim OldDrvChar As Char = "a"
            Dim DrvId As Integer = 0
            For Each inxiparts In inxitoken.SelectToken("001#Partition")
                Dim CurrDrvChar As Char = inxiparts("005#dev").ToString.Replace("/dev/sd", "").Remove(1)
                If CurrDrvChar <> OldDrvChar Then
                    DrvId += 1
                End If
                HDDList(DrvId).Parts.Add(New Part_Linux With {.Part = inxiparts("005#dev"), .FileSystem = inxiparts("004#fs"), .SizeMEAS = inxiparts("002#size"), .Used = inxiparts("003#used")})
            Next
        Catch ex As Exception
            HDDDone = False
            KernelError("C", False, 0, DoTranslation("Error while checking HDD: {0}", currentLang), ex, ex.Message)
            If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Uncontinuable) : WStkTrc(ex)
        End Try
    End Sub

    '----------> Hardware lists <----------
    Public Sub ListDrivers() 'Windows
        'Variables
        Dim times As Integer = 1
        Dim total As ULong

        'CPU Info
        For Each processorinfo In CPUList
            Dim CPUName As String = processorinfo.Name
            If CPUName.Contains("@") And CPUName.EndsWith("GHz") Then
                W("CPU: {0}", False, ColTypes.Neutral, processorinfo.Name)
            Else
                W("CPU: {0} @ {1} MHz", False, ColTypes.Neutral, processorinfo.Name, processorinfo.ClockSpeed)
            End If

            'SSE2 availability
            If CPUFeatures_Win.IsProcessorFeaturePresent(CPUFeatures_Win.SSEnum.InstructionsSSE2Available) Then 'After SSE2 requirement addition, remove the check.
                W(" : SSE2", True, ColTypes.Neutral)
            Else
                W(vbNewLine + DoTranslation("CPU: WARNING: SSE2 will be required in future development commits.", currentLang), True, ColTypes.Err)
            End If
        Next
        W(DoTranslation("CPU: Total number of processors: {0}", currentLang), True, ColTypes.Neutral, Environment.ProcessorCount)

        'This is an expression totalling a RAM to print string
        For Each capacity In Capacities
            total += capacity
        Next

        'Print some info
        W(DoTranslation("RAM: {0} MB = {1} MB", currentLang), True, ColTypes.Neutral, CStr(total), String.Join(" MB + ", Capacities))

        'A loop to print names
        For Each slotname In RAMList
            If times = 1 Then
                W(DoTranslation("RAM: Used slots (by names): {0}", currentLang), False, ColTypes.Neutral, slotname.SlotName)
            Else
                W(" {0}", False, ColTypes.Neutral, slotname.SlotName)
            End If
            times += 1
        Next

        'Number of slots used
        W(vbNewLine + DoTranslation("RAM: Used slots (by numbers): {0} / {1} ({2}%)", currentLang), True, ColTypes.Neutral, CStr(slotsUsedNum), totalSlots, FormatNumber(CStr(slotsUsedNum * 100 / totalSlots), 1))

        'Drive Info
        For Each driveinfo In HDDList
            If driveinfo.Manufacturer = "(Standard disk drives)" Then
                W("HDD: {0} {1} GB {2}" + vbNewLine +
                  DoTranslation("HDD: CHS: {3} cylinders | {4} heads | {5} sectors", currentLang), True, ColTypes.Neutral,
                  driveinfo.Model, FormatNumber(driveinfo.Size / 1024 / 1024 / 1024, 2),
                  driveinfo.InterfaceType, driveinfo.Cylinders, driveinfo.Heads, driveinfo.Sectors)
            Else
                W("HDD: {0} {1} {2} GB {3}" + vbNewLine +
                  DoTranslation("HDD: CHS: {4} cylinders | {5} heads | {6} sectors", currentLang), True, ColTypes.Neutral,
                  driveinfo.Manufacturer, driveinfo.Model, FormatNumber(driveinfo.Size / 1024 / 1024 / 1024, 2),
                  driveinfo.InterfaceType, driveinfo.Cylinders, driveinfo.Heads, driveinfo.Sectors)
            End If
        Next
    End Sub

    Sub ListDrivers_Linux()
        'CPU List
        For Each info As CPU_Linux In CPUList
            W("CPU: {0} {1} Mhz", False, ColTypes.Neutral, info.CPUName, info.Clock)
            If info.SSE2 Then 'After SSE2 requirement addition, remove the check.
                W(" : SSE2", False, ColTypes.Neutral)
            Else
                W(DoTranslation("CPU: WARNING: SSE2 will be required in future development commits.", currentLang), True, ColTypes.Err)
            End If
            Console.WriteLine()
        Next

        'HDD List
        Dim CurrDrv As Integer = 0
        For Each info As HDD_Linux In HDDList
            W("HDD: {0} {1} {2}", True, ColTypes.Neutral, info.Vendor_LNX, info.Model_LNX, info.Size_LNX)
            For Each part As Part_Linux In HDDList(CurrDrv).Parts
                W("HDD ({4}): {0} {1} {2} {3}", True, ColTypes.Neutral, part.Part, part.FileSystem, part.SizeMEAS, part.Used, CurrDrv)
            Next
            CurrDrv += 1
        Next

        'RAM List
        For Each info As RAM_Linux In RAMList
            W("RAM: {0}", True, ColTypes.Neutral, info.Capacity)
        Next
    End Sub
End Module
