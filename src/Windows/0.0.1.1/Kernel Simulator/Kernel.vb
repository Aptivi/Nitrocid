
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

Imports System
Imports System.IO
Imports System.Reflection

Module Kernel

    'Variables
    Public KernelVersion As String
    Public Hddsize As String
    Public Dsize As String
    Public Hddmodel As String
    Public Cpuname As String
    Public Cpuspeed As String
    Public SysMem As String
    Public BIOSCaption As String
    Public BIOSMan As String
    Public BIOSStatus As String
    Public StopPanicAndGoToDoublePanic As Boolean

    'Sleep sub
    Declare Sub Sleep Lib "kernel32" (ByVal milliseconds As Integer)

    'Hardware probing functions
    Function Hddinfo()
        Dim HDDSet As Object                                                                            'Sets of hard drive
        Dim Hdd As Object                                                                               'Needed to get model and size of hard drive.
        HDDSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_DiskDrive")                      'it gets Winmgmts: to SELECT * FROM Win32_DiskDrive
        For Each Hdd In HDDSet
            Hddmodel = CStr(Hdd.Model)
            Dsize = CStr(Hdd.Size)
        Next
        Hddsize = Dsize / 1024 / 1024 / 1024
        If (Hddsize = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get hard drive size.")
        ElseIf (Hddmodel = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get hard drive model.")
        End If
        Return True
    End Function

    Function Cpuinfo()
        Dim CPUSet As Object                                                                            'Sets of CPU
        Dim CPU As Object                                                                               'Needed to get name and clock speed of CPU
        CPUSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_Processor")                      'it gets Winmgmts: to SELECT * FROM Win32_Processor
        For Each CPU In CPUSet
            Cpuname = CStr(CPU.Name)
            Cpuspeed = CStr(CPU.CurrentClockSpeed)
        Next
        If (Cpuname = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get CPU name.")
        ElseIf (Cpuspeed = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get CPU speed.")
        End If
        Return True
    End Function

    Function SysMemory()
        Dim oInstance
        Dim colInstances
        Dim dRam As Double
        colInstances = GetObject("winmgmts:").ExecQuery("SELECT * FROM Win32_PhysicalMemory")
        For Each oInstance In colInstances
            dRam = dRam + oInstance.Capacity
        Next
        SysMem = dRam / 1024 / 1024 & "MB"
        If (dRam = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get RAM size.")
        End If
        Return True
    End Function

    Function BiosInformation()
        On Error Resume Next
        Dim BiosInfoSpec
        Dim Computer = "."
        Dim WMIService As Object = GetObject("winmgmts:")
        Dim Items = WMIService.ExecQuery("Select * from Win32_BIOS")
        For Each BiosInfoSpec In Items
            BIOSCaption = BiosInfoSpec.Caption
            BIOSMan = BiosInfoSpec.Manufacturer
            BIOSStatus = BiosInfoSpec.Status
        Next
        If (BIOSCaption = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get BIOS version.")
        ElseIf (BIOSMan = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get BIOS manufacturer.")
        ElseIf (BIOSStatus = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get BIOS status.")
        End If
        Return True
    End Function

    Function KernelError(ByVal ErrorType As Char, Optional ByVal Reboot As Boolean = True, Optional ByVal RebootTime As Long = 30, Optional ByVal Description As String = "General kernel error.")
        'ErrorType As Char: Specifies whether the error is serious, fatal, unrecoverable, or double panic. S/F/U/D
        'Reboot As Boolean: Optional. Specifies whether to reboot on panic or to show the message to press any key to shut down
        'RebootTime As Long: Optional. Specifies seconds before reboot. 0 is instant. Negative numbers not allowed.
        'Description As String: Optional. Explanation of what happened when it errored.
        If (ErrorType = "S" Or ErrorType = "F" Or ErrorType = "U" Or ErrorType = "D") Then
            If (ErrorType = "U" And RebootTime > 5 Or ErrorType = "D" And RebootTime > 5) Then
                KernelError("D", True, 5, "DOUBLE PANIC: Reboot Time exceeds maximum allowed " + CStr(ErrorType) + " error reboot time. You found a kernel bug.")
                StopPanicAndGoToDoublePanic = True
            ElseIf (ErrorType = "U" And Reboot = False Or ErrorType = "D" And Reboot = False) Then
                System.Console.WriteLine("[" + ErrorType + "] panic: Reboot enabled due to error level being " + ErrorType + ".")
                Reboot = True
            End If
            If (RebootTime > 3600) Then
                System.Console.WriteLine("[" + ErrorType + "] panic: Time to reboot: " + CStr(RebootTime) + " seconds, exceeds 1 hour. Set to at least 1 minute.")
                RebootTime = 60
            End If
        Else
            KernelError("D", True, 5, "DOUBLE PANIC: Error Type " + CStr(ErrorType) + " invalid.")
            StopPanicAndGoToDoublePanic = True
        End If
        If (Description.Contains("DOUBLE PANIC: ") And ErrorType = "D") Then
            System.Console.WriteLine("[" + ErrorType + "] panic: " + CStr(Description) + " -- Rebooting in " + CStr(RebootTime) + " seconds...")
            Sleep(RebootTime * 1000)
            System.Console.Write("[" + ErrorType + "] panic: Rebooting now..." + vbNewLine)
            System.Console.Clear()
            Main()                                  'Restart kernel
        ElseIf (StopPanicAndGoToDoublePanic = True) Then
            Return "DoublePanic"
            Exit Function
        ElseIf (Reboot = False And ErrorType <> "D") Then
            System.Console.WriteLine("[" + ErrorType + "] panic: " + CStr(Description) + " -- Press any key to shutdown.")
            System.Console.Read()
            Environment.Exit(0)
        Else
            System.Console.WriteLine("[" + ErrorType + "] panic: " + CStr(Description) + " -- Rebooting in " + CStr(RebootTime) + " seconds...")
            Sleep(RebootTime * 1000)
            System.Console.Write("[" + ErrorType + "] panic: Rebooting now..." + vbNewLine)
            System.Console.Clear()
            Main()                                  'Restart kernel
        End If
        Return ErrorType
    End Function

    Sub Main()

        'Setting variables (mostly version of kernel)
        KernelVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()

        'Main class
        'About this kernel: THIS KERNEL IS NOT FINAL! Final kernel will be developed through another language, .ASM included. depending on system.
        My.Settings.Usernames = New System.Collections.Specialized.StringCollection
        My.Settings.Passwords = New System.Collections.Specialized.StringCollection
        System.Console.Write("-+---> Welcome to Eofla Kernel! Version " + KernelVersion + " <---+-")
        System.Console.WriteLine(vbNewLine + vbNewLine + "    Kernel Simulator  Copyright (C) 2018  EoflaOE" + vbNewLine + _
                                                         "    This program comes with ABSOLUTELY NO WARRANTY, not even MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine + _
                                                         "    This is free software, and you are welcome to redistribute it" + vbNewLine + _
                                                         "    under certain conditions; See COPYING file in source code." + vbNewLine)
        System.Console.WriteLine("hwprobe: Your hardware will be probed. Please wait...")
        Cpuinfo()
        System.Console.Write("hwprobe: CPU: " + Cpuname + " " + Cpuspeed + "MHz" + vbNewLine)
        SysMemory()
        System.Console.Write("hwprobe: RAM: " + SysMem + vbNewLine)
        Hddinfo()
        System.Console.Write("hwprobe: HDD: " + Hddmodel + " " + FormatNumber(Hddsize, 2) + "GB" + vbNewLine)
        System.Console.Write("hwprobe: Your BIOS will be probed. Please wait..." + vbNewLine)
        BiosInformation()
        System.Console.Write("hwprobe: BIOS: " + BIOSMan + " " + BIOSCaption + " | Status: " + BIOSStatus + vbNewLine)
        System.Console.Write("usrmgr: System Usernames: ")
        Login.initializeMainUsers()
        System.Console.Write("created." + vbNewLine)
        Login.initializeUsers()
        Login.adduser("demo")
        LoginFlag = True
        Login.initializeUsers()

    End Sub

End Module
