
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
    Public BIOSSMBIOSVersion As String                                                  'BIOS Version from SMBIOS
    Public BIOSVersion As String                                                        'BIOS Version (some AMI BIOSes output "AMIINT - 10") 
    Public StopPanicAndGoToDoublePanic As Boolean                                       'Double panic mode in kernel error
    Public KernelVersion As String = Assembly.GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String                                                         'Array for boot arguments
    Public AvailableArgs() As String = {"motd", "nohwprobe", "chkn=1", "preadduser", "hostname", "quiet", "gpuprobe", "cmdinject"}
    Public ProbeFlag As Boolean = True                                                  'Check to see if the hardware can be probed
    Public Quiet As Boolean = False                                                     'Quiet mode
    Public TimeDateIsSet As Boolean = False                                             'To fix a bug after reboot
    Public slotsUsedName As String                                                      'Lists slots by names
    Public slotsUsedNum As Integer                                                      'Lists slots by numbers
    Public Capacities() As String                                                       'Capacity (in MB)
    Public totalSlots As Integer                                                        'Total slots
    Public GPUProbeFlag As Boolean = False                                              'No GPU probe (Probe GPU = 'gpuprobe' kernel argument)

    'Sleep sub
    Declare Sub Sleep Lib "kernel32" (ByVal milliseconds As Integer)                    'Enable sleep (Mandatory, don't remove)

    Function KernelError(ByVal ErrorType As Char, Optional ByVal Reboot As Boolean = True, Optional ByVal RebootTime As Long = 30, Optional ByVal Description As String = "General kernel error.") As Char
        'ErrorType As Char: Specifies whether the error is serious, fatal, unrecoverable, or double panic. C/S/D/F/U
        'Reboot As Boolean: Optional. Specifies whether to reboot on panic or to show the message to press any key to shut down
        'RebootTime As Long: Optional. Specifies seconds before reboot. 0 is instant. Negative numbers not allowed.
        'Description As String: Optional. Explanation of what happened when it errored.

        'Error Handler
        On Error GoTo bug

        'Check error types and its capabilities
        If (ErrorType = "S" Or ErrorType = "F" Or ErrorType = "U" Or ErrorType = "D" Or ErrorType = "C") Then
            If (ErrorType = "U" And RebootTime > 5 Or ErrorType = "D" And RebootTime > 5) Then
                'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                'generate a second kernel error stating that there is something wrong with the reboot time.
                KernelError(CChar("D"), True, 5, "DOUBLE PANIC: Reboot Time exceeds maximum allowed " + CStr(ErrorType) + " error reboot time. You found a kernel bug.")
                StopPanicAndGoToDoublePanic = True
            ElseIf (ErrorType = "U" And Reboot = False Or ErrorType = "D" And Reboot = False) Then
                'If the error type is unrecoverable, or double, and the rebooting is false where it should
                'not be false, then it can deal with this issue by enabling reboot.
                System.Console.ForegroundColor = CType(uncontKernelErrorColor, ConsoleColor)
                System.Console.WriteLine("[{0}] panic: Reboot enabled due to error level being {0}.", ErrorType)
                System.Console.ResetColor()
                Reboot = True
            End If
            If (RebootTime > 3600) Then
                'If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                System.Console.ForegroundColor = CType(uncontKernelErrorColor, ConsoleColor)
                System.Console.WriteLine("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute.", ErrorType, CStr(RebootTime))
                System.Console.ResetColor()
                RebootTime = 60
            End If
        Else
            'If the error type is other than D/F/C/U/S, then it will generate a second error.
            KernelError(CChar("D"), True, 5, "DOUBLE PANIC: Error Type " + CStr(ErrorType) + " invalid.")
            StopPanicAndGoToDoublePanic = True
        End If

        'Check error capabilities
        If (Description.Contains("DOUBLE PANIC: ") And ErrorType = "D") Then
            'If the description has a double panic tag and the error type is Double
            System.Console.ForegroundColor = CType(uncontKernelErrorColor, ConsoleColor)
            System.Console.WriteLine("[{0}] dpanic: {1} -- Rebooting in {2} seconds...", ErrorType, CStr(Description), CStr(RebootTime))
            System.Console.ResetColor()
            Sleep(CInt(RebootTime * 1000))
            System.Console.Clear()
            ResetEverything()
            Main()                                  'Restart kernel
        ElseIf (StopPanicAndGoToDoublePanic = True) Then
            'Switch to Double Panic
            Return CChar("D")
            Exit Function
        ElseIf (ErrorType = "C" And Reboot = True) Then
            'Check if error is Continuable and reboot is enabled
            System.Console.ForegroundColor = CType(contKernelErrorColor, ConsoleColor)
            System.Console.WriteLine("[{0}] panic: Reboot disabled due to error level being {0}.", ErrorType)
            Reboot = False
            System.Console.WriteLine("[{0}] panic: {1} -- Press any key to continue using the kernel.", ErrorType, CStr(Description))
            System.Console.ResetColor()
            Dim answercontpanic = System.Console.ReadKey.KeyChar
        ElseIf (ErrorType = "C" And Reboot = False) Then
            'Check if error is Continuable and reboot is disabled
            System.Console.ForegroundColor = CType(contKernelErrorColor, ConsoleColor)
            System.Console.WriteLine("[{0}] panic: {1} -- Press any key to continue using the kernel.", ErrorType, CStr(Description))
            System.Console.ResetColor()
            Dim answercontpanic = System.Console.ReadKey.KeyChar
        ElseIf ((Reboot = False And ErrorType <> "D") Or (Reboot = False And ErrorType <> "C")) Then
            'If rebooting is disabled and the error type does not equal Double or Continuable
            System.Console.ForegroundColor = CType(uncontKernelErrorColor, ConsoleColor)
            System.Console.WriteLine("[{0}] panic: {1} -- Press any key to shutdown.", ErrorType, CStr(Description))
            Dim answerpanic = System.Console.ReadKey.KeyChar            'Edited to press any key instantly without having to press ENTER
            Environment.Exit(0)
        Else
            'Everything else.
            System.Console.ForegroundColor = CType(uncontKernelErrorColor, ConsoleColor)
            System.Console.WriteLine("[{0}] panic: {1} -- Rebooting in {2} seconds...", ErrorType, CStr(Description), CStr(RebootTime))
            Sleep(CInt(RebootTime * 1000))
            System.Console.Clear()
            ResetEverything()
            Main()                                  'Restart kernel
        End If
        Return ErrorType
        Exit Function
bug:
        KernelError(CChar("D"), True, 5, "DOUBLE PANIC: Kernel bug: " + Err.Description)
    End Function

    Function ResetEverything() As Boolean

        'Reset every variable that is resettable
        If (argsInjected = False) Then
            answerargs = Nothing
        End If
        Erase BootArgs
        answerbeep = Nothing
        answerbeepms = Nothing
        answerecho = Nothing
        argsFlag = False
        Computers = Nothing
        ProbeFlag = True
        GPUProbeFlag = False
        Quiet = False
        StopPanicAndGoToDoublePanic = False
        strcommand = Nothing

        'Reset users
        UserManagement.resetUsers()

        'Reset time and date
        TimeDate.ResetTimeDate()

        'Reset color
        System.Console.ResetColor()

        Return True

    End Function

    Sub Main()

        'Main class
        'About this kernel: THIS KERNEL IS NOT FINAL! Final kernel will be developed through another language, ASM included, depending on system.
        On Error GoTo KernelBug
        System.Console.Write("-+---> Welcome to Eofla Kernel! Version {0} <---+-", KernelVersion)

        'License used. Do not remove.
        System.Console.ForegroundColor = CType(licenseColor, ConsoleColor)
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
            HardwareProbe.ProbeHW(True, CChar("K"))
            HardwareProbe.ProbeGPU()

            'Phase 2: Probe BIOS
            HardwareProbe.ProbeBIOS(True)

            'Phase 3: Username management
            UserManagement.initializeMainUsers()
            UserManagement.adduser("demo")
            LoginFlag = True
            Groups.permission("Admin", False, "demo", "Add", True)
            Groups.permission("Disabled", False, "demo", "Add", True)

            'Phase 4: Check for pre-user making and log-in
            If (CruserFlag = True) Then
                adduser(arguser, argword)
            End If
            If (LoginFlag = True) Then
                Login.LoginPrompt()
            End If
        Else
            'Continue the kernel
            'Phase 1: Probe hardware if nohwprobe is not passed
            HardwareProbe.ProbeHW(False, CChar("K"))

            'Phase 2: Probe BIOS
            HardwareProbe.ProbeBIOS(False)

            'Phase 3: Username management
            UserManagement.initializeMainUsers()
            UserManagement.adduser("demo")
            LoginFlag = True
            Groups.permission("Admin", False, "demo", "Add", True)
            Groups.permission("Disabled", False, "demo", "Add", True)

            'Phase 4: Check for pre-user making and log-in
            If (CruserFlag = True) Then
                adduser(arguser, argword)
            End If
            If (LoginFlag = True) Then
                Login.LoginPrompt()
            End If

        End If
        Exit Sub
KernelBug:
        KernelError(CChar("U"), True, 5, "Kernel Error while booting: " + Err.Description)

    End Sub

End Module
