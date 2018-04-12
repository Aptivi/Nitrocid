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
    Public Hddsize As String                                                            'The size of simulated Hard Drive
    Public Dsize As String                                                              'Same as above, but in bytes
    Public Hddmodel As String                                                           'Model of hard drive
    Public Cpuname As String                                                            'CPU name
    Public Cpuspeed As String                                                           'CPU Clock Speed
    Public SysMem As String                                                             'Memory of simulated system
    Public BIOSCaption As String                                                        'BIOS Caption
    Public BIOSMan As String                                                            'BIOS Manufacturer
    Public StopPanicAndGoToDoublePanic As Boolean                                       'Double panic mode in kernel error
    Public KernelVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString() 'Kernel version (reserved)
    Public BootArgs() As String                                                         'Array for boot arguments
    Public AvailableArgs() As String = {"motd", "nohwprobe", "chkn=1", "preadduser", _
                                        "hostname", "quiet"}                            'Available arguments.
    Public ProbeFlag As Boolean = True                                                  'Check to see if the hardware can be probed
    Public Quiet As Boolean = False                                                     'Quiet mode
    Public TimeDateIsSet As Boolean = False                                             'To fix a bug after reboot

    'Sleep sub
    Declare Sub Sleep Lib "kernel32" (ByVal milliseconds As Integer)                    'Enable sleep (Mandatory, don't remove)

    'Hardware probing functions
    Function Hddinfo()
        Dim HDDSet As Object                                                            'Sets of hard drive
        Dim Hdd As Object                                                               'Needed to get model and size of hard drive.
        HDDSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_DiskDrive")      'it gets Winmgmts: to SELECT * FROM Win32_DiskDrive
        For Each Hdd In HDDSet
            Hddmodel = CStr(Hdd.Model)
            Dsize = CStr(Hdd.Size)
        Next
        Hddsize = Dsize / 1024 / 1024 / 1024                                            'Calculate size of Hard Drive in GB
        If (Hddsize = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get hard drive size.")
        ElseIf (Hddmodel = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get hard drive model.")
        End If
        Return True
    End Function

    Function Cpuinfo()
        Dim CPUSet As Object                                                            'Sets of CPU
        Dim CPU As Object                                                               'Needed to get name and clock speed of CPU
        CPUSet = GetObject("Winmgmts:").ExecQuery("SELECT * FROM Win32_Processor")      'it gets Winmgmts: to SELECT * FROM Win32_Processor
        For Each CPU In CPUSet
            Cpuname = CStr(CPU.Name)                                                    'Get name of CPU
            Cpuspeed = CStr(CPU.CurrentClockSpeed)                                      'Get name of clock speed
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
            dRam = dRam + oInstance.Capacity                                            'Calculate RAM in bytes
        Next
        SysMem = dRam / 1024 / 1024 & "MB"                                              'Calculate RAM in MB
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
        Next
        If (BIOSCaption = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get BIOS version.")
        ElseIf (BIOSMan = Nothing) Then
            KernelError("F", True, 15, "Machine Check Exception while trying to get BIOS manufacturer.")
        End If
        Return True
    End Function

    Function KernelError(ByVal ErrorType As Char, Optional ByVal Reboot As Boolean = True, Optional ByVal RebootTime As Long = 30, Optional ByVal Description As String = "General kernel error.")
        'ErrorType As Char: Specifies whether the error is serious, fatal, unrecoverable, or double panic. S/F/U/D
        'Reboot As Boolean: Optional. Specifies whether to reboot on panic or to show the message to press any key to shut down
        'RebootTime As Long: Optional. Specifies seconds before reboot. 0 is instant. Negative numbers not allowed.
        'Description As String: Optional. Explanation of what happened when it errored.
        On Error GoTo bug
        If (ErrorType = "S" Or ErrorType = "F" Or ErrorType = "U" Or ErrorType = "D") Then
            If (ErrorType = "U" And RebootTime > 5 Or ErrorType = "D" And RebootTime > 5) Then
                'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                'generate a second kernel error stating that there is something wrong with the reboot time.
                KernelError("D", True, 5, "DOUBLE PANIC: Reboot Time exceeds maximum allowed " + CStr(ErrorType) + " error reboot time. You found a kernel bug.")
                StopPanicAndGoToDoublePanic = True
            ElseIf (ErrorType = "U" And Reboot = False Or ErrorType = "D" And Reboot = False) Then
                'If the error type is unrecoverable, or double, and the rebooting is false where it should
                'not be false, then it can deal with this issue by enabling reboot.
                System.Console.WriteLine("[{0}] panic: Reboot enabled due to error level being {0}.", ErrorType)
                Reboot = True
            End If
            If (RebootTime > 3600) Then
                'If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                System.Console.WriteLine("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute.", ErrorType, CStr(RebootTime))
                RebootTime = 60
            End If
        Else
            'If the error type is other than D/F/U/S, then it will generate a second error.
            KernelError("D", True, 5, "DOUBLE PANIC: Error Type " + CStr(ErrorType) + " invalid.")
            StopPanicAndGoToDoublePanic = True
        End If
        If (Description.Contains("DOUBLE PANIC: ") And ErrorType = "D") Then
            'If the description has a double panic tag and the error type is Double
            System.Console.WriteLine("[{0}] panic: {1} -- Rebooting in {2} seconds...", ErrorType, CStr(Description), CStr(RebootTime))
            Sleep(RebootTime * 1000)
            System.Console.WriteLine("[{0}] panic: Rebooting now...", ErrorType)
            System.Console.Clear()
            Main()                                  'Restart kernel
        ElseIf (StopPanicAndGoToDoublePanic = True) Then
            Return "DoublePanic"
            Exit Function
        ElseIf (Reboot = False And ErrorType <> "D") Then
            'If rebooting is disabled and the error type does not equal Double
            System.Console.WriteLine("[{0}] panic: {1} -- Press any key to shutdown.", ErrorType, CStr(Description))
            Dim answerpanic = System.Console.ReadKey.KeyChar            'Edited to press any key instantly without having to press ENTER
            Environment.Exit(0)
        Else
            'Everything else.
            System.Console.WriteLine("[{0}] panic: {1} -- Rebooting in {2} seconds...", ErrorType, CStr(Description), CStr(RebootTime))
            Sleep(RebootTime * 1000)
            System.Console.WriteLine("[{0}] panic: Rebooting now...", ErrorType)
            System.Console.Clear()
            Main()                                  'Restart kernel
        End If
        Return ErrorType
        Exit Function
bug:
        KernelError("D", True, 5, "DOUBLE PANIC: Kernel bug: " + Err.Description)
    End Function

    Sub Main()

        'Main class
        'About this kernel: THIS KERNEL IS NOT FINAL! Final kernel will be developed through another language, ASM included, depending on system.
        My.Settings.Usernames = New System.Collections.Specialized.StringCollection     'Temporary
        My.Settings.Passwords = New System.Collections.Specialized.StringCollection     'Temporary
        System.Console.Write("-+---> Welcome to Eofla Kernel! Version {0} <---+-", KernelVersion)

        'License used. Do not remove.
        System.Console.ForegroundColor = ConsoleColor.White
        System.Console.WriteLine(vbNewLine + vbNewLine + "    Kernel Simulator  Copyright (C) 2018  EoflaOE" + vbNewLine + _
                                                         "    This program comes with ABSOLUTELY NO WARRANTY, not even " + vbNewLine + _
                                                         "    MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine + _
                                                         "    This is free software, and you are welcome to redistribute it" + vbNewLine + _
                                                         "    under certain conditions; See COPYING file in source code." + vbNewLine)
        System.Console.ResetColor()

        'Phase 0: Prompt and check for boot arguments, then initialize time and check for quietness
        ArgumentPrompt.PromptArgs()
        If (argsFlag = True) Then
            ArgumentParse.ParseArguments()
        End If

        'Fixed a bug in boot arguments
        If (TimeDateIsSet = False) Then
            InitializeTimeDate()
            TimeDateIsSet = True
        End If

        If (Quiet = True) Then
            'Continue the kernel, and don't print messages
            'Phase 1: Probe hardware if nohwprobe is not passed
            HardwareProbe.ProbeHW(True, "K")

            'Phase 2: Probe BIOS
            HardwareProbe.ProbeBIOS(True)

            'Phase 3: Username management
            Login.initializeMainUsers()
            Login.initializeUsers()
            Login.adduser("demo")
            LoginFlag = True

            'Phase 4: Check for pre-user making and log-in
            If (CruserFlag = True) Then
                adduser(arguser, argword)
            End If
            Login.initializeUsers()
        Else
            'Continue the kernel
            'Phase 1: Probe hardware if nohwprobe is not passed
            HardwareProbe.ProbeHW(False, "K")

            'Phase 2: Probe BIOS
            HardwareProbe.ProbeBIOS(False)

            'Phase 3: Username management
            Login.initializeMainUsers()
            Login.initializeUsers()
            Login.adduser("demo")
            LoginFlag = True

            'Phase 4: Check for pre-user making and log-in
            If (CruserFlag = True) Then
                adduser(arguser, argword)
            End If
            Login.initializeUsers()
        End If

    End Sub

End Module
