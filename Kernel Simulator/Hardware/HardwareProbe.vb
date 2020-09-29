
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
    ''' <summary>
    ''' Starts probing hardware
    ''' </summary>
    Public Sub StartProbing()
        If Not quietProbe Then W(DoTranslation("hwprobe: Your hardware will be probed. Please wait...", currentLang), True, ColTypes.Neutral)

        'We will probe hardware
        If Not EnvironmentOSType.Contains("Unix") Then
            ProbeHardware()
        Else
            ProbeHardwareLinux()
        End If

        If Not quietProbe Then
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
        End If
    End Sub

    '----------> Windows hardware probers <----------
    ''' <summary>
    ''' [Windows] Probes hardware
    ''' </summary>
    Public Sub ProbeHardware()
        'Variables
        Dim HDDSet As New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")
        Dim PartsS As New ManagementObjectSearcher("SELECT * FROM Win32_DiskPartition")
        Dim LogicS As New ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk")
        Dim CPUSet As New ManagementObjectSearcher("SELECT * FROM Win32_Processor")
        Dim System As New ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem")
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
        Dim Arch As String = ""
        For Each Sys As ManagementBaseObject In System.Get
            Arch = Sys("OSArchitecture")
        Next
        For Each CPU As ManagementBaseObject In CPUSet.Get
            Try
                CPUList.Add(New CPU With {.Name = CPU("Name"), .ClockSpeed = CPU("CurrentClockSpeed"), .Arch = Arch})
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
    ''' <summary>
    ''' [Linux] Probes hardware
    ''' </summary>
    ''' <remarks>This uses inxi to get information about HDD. If you don't have it installed, use your appropriate package manager to install inxi or build from source.</remarks>
    Public Sub ProbeHardwareLinux()
        'Done sets
        HDDDone = True
        CPUDone = True
        RAMDone = True

        'CPU Prober
        Try
            Dim Name = "", Clock = "", Arch = "", SSE2 = False, Line As String
            Dim LscpuStdout As StreamReader
            Dim Lscpu As New Process
            Dim LscpuInfo As New ProcessStartInfo With {.FileName = "/usr/bin/lscpu",
                                                        .WindowStyle = ProcessWindowStyle.Hidden,
                                                        .CreateNoWindow = True,
                                                        .UseShellExecute = False,
                                                        .RedirectStandardOutput = True}
            Lscpu.StartInfo = LscpuInfo
            Lscpu.Start() : Lscpu.WaitForExit()
            LscpuStdout = Lscpu.StandardOutput
            Do While Not LscpuStdout.EndOfStream
                Line = LscpuStdout.ReadLine
                If Line.StartsWith("Architecture:") Then
                    Arch = Line.Replace("Architecture:", "").RemoveSpacesFromBeginning
                    Wdbg("I", "Arch: {0}", Arch)
                ElseIf Line.StartsWith("CPU max MHz:") Then 'For some systems that has max and min MHz, like some ARM and ARM64 systems
                    Clock = Line.Replace("CPU max MHz:", "").RemoveSpacesFromBeginning
                    Wdbg("I", "Clock: {0}", Clock)
                ElseIf Line.StartsWith("CPU MHz:") Then 'Common architectures
                    Clock = Line.Replace("CPU MHz:", "").RemoveSpacesFromBeginning
                    Wdbg("I", "Clock: {0}", Clock)
                ElseIf Line.StartsWith("CPU dynamic MHz:") Then 'For IBM System Z (S390X)
                    Clock = Line.Replace("CPU dynamic MHz:", "").RemoveSpacesFromBeginning
                    Wdbg("I", "Clock: {0}", Clock)
                ElseIf Line.StartsWith("Model name:") Then
                    Name = Line.Replace("Model name:", "").RemoveSpacesFromBeginning
                    Wdbg("I", "Model name: {0}", Name)
                ElseIf Line.StartsWith("Model:") And Arch.Contains("ppc") Then 'For PowerPC (PowerMac, etc.)
                    Name = Line.Replace("Model:", "").RemoveSpacesFromBeginning
                    Wdbg("I", "Model name: {0}", Name)
                ElseIf Line.StartsWith("Flags:") And Line.Contains("sse2") Then
                    SSE2 = True
                    Wdbg("I", "Machine has SSE2.")
                End If
            Loop
            If Clock = "" Then Clock = "0"
            CPUList.Add(New CPU_Linux With {.Clock = Clock, .CPUName = Name, .Arch = Arch, .SSE2 = SSE2})
        Catch ex As Exception
            CPUDone = False
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
            If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
        End Try

        'HDD Prober (You need to have inxi and libcpanel-json-xs-perl installed)
        Try
            If File.Exists("/usr/bin/inxi") Then
                Wdbg("I", "Inxi installed. Continuing...")
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

                'Get information about drives
                Dim inxitoken As JToken = JToken.Parse(inxiout)
                Dim inxiReady As Boolean = False
                For Each inxidrvs In inxitoken.SelectToken("000#Drives")
                    If inxiReady Then
                        Dim DriveSize As String = inxidrvs("004#size")
                        Wdbg("I", "Prototype drive size: {0}", DriveSize)
                        Dim DriveModel As String = inxidrvs("003#model")
                        Wdbg("I", "Prototype drive model: {0}", DriveModel)
                        Dim DriveVendor As String = inxidrvs("002#vendor")
                        Wdbg("I", "Drive vendor: {0}", DriveVendor)
                        If DriveVendor = "" Then
                            DriveSize = inxidrvs("003#size")
                            Wdbg("I", "Final drive size: {0}", DriveSize)
                            DriveModel = inxidrvs("002#model")
                            Wdbg("I", "Final drive model: {0}", DriveModel)
                        End If
                        HDDList.Add(New HDD_Linux With {.Size_LNX = DriveSize, .Model_LNX = DriveModel, .Vendor_LNX = DriveVendor})
                    End If
                    inxiReady = True
                Next

                'Get information about partitions
                Dim OldDrvChar As Char = "a"
                Dim DrvId As Integer = 0
                Dim PartitionToken As JToken = inxitoken.SelectToken("001#Partition")
                If Not IsNothing(PartitionToken) Then
                    For Each inxiparts In PartitionToken
                        Dim DrvDevPath As String = inxiparts("005#dev").ToString
                        Dim CurrDrvChar As Char

                        Wdbg("I", "Prototype old / current dev path: {0} / {1}", OldDrvChar, CurrDrvChar)
                        If DrvDevPath.ContainsAny({"hd", "sd", "vd"}) Then '/dev/hdX, /dev/sdX, /dev/vdX
                            Wdbg("I", "Drive seems to be one of /dev/(h|s|v)dXY format. {0}", DrvDevPath)
                            CurrDrvChar = DrvDevPath.Replace("/dev/sd", "").Replace("/dev/hd", "").Replace("/dev/vd", "").Chars(0)
                        ElseIf DrvDevPath.Contains("mmcblk") Then '/dev/mmcblkXpY
                            Wdbg("I", "Drive seems to be one of /dev/mmcblkXpY format. {0}", DrvDevPath)
                            If Not OldDrvChar.ToString.IsNumeric Then OldDrvChar = "0"
                            CurrDrvChar = DrvDevPath.Replace("/dev/mmcblk", "").Chars(0)
                        End If
                        If CurrDrvChar <> OldDrvChar Then
                            Wdbg("I", "Transporting to another drive.")
                            DrvId += 1
                        End If

                        Wdbg("I", "Adding drive with arguments [.Part = {0}, .FileSystem = {1}, .SizeMEAS = {2}, .Used = {3}] to partition list of drive {4}...", DrvDevPath, inxiparts("004#fs"), inxiparts("002#size"), inxiparts("003#used"), DrvId)
                        HDDList(DrvId).Parts.Add(New Part_Linux With {.Part = DrvDevPath, .FileSystem = inxiparts("004#fs"), .SizeMEAS = inxiparts("002#size"), .Used = inxiparts("003#used")})
                    Next
                End If
            Else
                HDDDone = False
                Wdbg("E", "Inxi not installed. Hard drive probing failed.")
                W(DoTranslation("You may not have inxi installed on your system. Refer to your package manager for installation. For Debian (and derivatives) systems, you might want to run ""sudo apt install inxi libcpanel-json-xs-perl"" in the terminal emulator.", currentLang), True, ColTypes.Err)
                Exit Sub
            End If
        Catch ex As Exception
            HDDDone = False
            If DebugMode = True Then W(ex.StackTrace, True, ColTypes.Uncontinuable) : WStkTrc(ex)
        End Try
    End Sub

    '----------> Hardware lists <----------
    ''' <summary>
    ''' [Windows] Lists all drivers
    ''' </summary>
    Public Sub ListDrivers()
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
                W(" : SSE2 @ {0}", True, ColTypes.Neutral, processorinfo.Arch)
            ElseIf processorinfo.Arch = "32-bit" Then
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
        Dim DriveNum As Integer = 0
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
            For Each PartInfo In HDDList(DriveNum).Parts
                W("HDD ({5}): {0} {1} GB " + DoTranslation("Primary: {2} Bootable: {3} Boot partition: {4}", currentLang), True, ColTypes.Neutral, PartInfo.Type, FormatNumber(PartInfo.Size / 1024 / 1024 / 1024, 2), PartInfo.Primary, PartInfo.Bootable, PartInfo.Boot, DriveNum + 1)
            Next
            DriveNum += 1
        Next
    End Sub

    ''' <summary>
    ''' [Linux] Lists all drivers
    ''' </summary>
    Sub ListDrivers_Linux()
        'CPU List
        For Each info As CPU_Linux In CPUList
            W("CPU: {0} {1} Mhz", False, ColTypes.Neutral, info.CPUName, info.Clock)
            If info.SSE2 Then 'After SSE2 requirement addition, remove the check.
                W(" : SSE2 @ {0}", True, ColTypes.Neutral, info.Arch)
            ElseIf info.Arch = "i686" Or info.Arch = "i586" Or info.Arch = "i486" Or info.Arch = "i386" Then
                W(vbNewLine + DoTranslation("CPU: WARNING: SSE2 will be required in future development commits.", currentLang), True, ColTypes.Err)
            Else
                Console.WriteLine()
            End If
        Next

        'HDD List
        Dim CurrDrv As Integer = 0
        For Each info As HDD_Linux In HDDList
            W("HDD: {0} {1} {2}", True, ColTypes.Neutral, info.Vendor_LNX, info.Model_LNX, info.Size_LNX)
            For Each part As Part_Linux In HDDList(CurrDrv).Parts
                W("HDD ({4}): {0} {1} {2} {3}", True, ColTypes.Neutral, part.Part, part.FileSystem, part.SizeMEAS, part.Used, CurrDrv + 1)
            Next
            CurrDrv += 1
        Next

        'RAM List
        For Each info As RAM_Linux In RAMList
            W("RAM: {0}", True, ColTypes.Neutral, info.Capacity)
        Next
    End Sub
End Module
