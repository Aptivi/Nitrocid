
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

'About this kernel: THIS KERNEL IS NEAR TO BETA BUT NOT FINAL! Final kernel will be developed through another language, ASM included, depending on system.

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
    Public KernelVersion As String = Assembly.GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String                                                         'Array for boot arguments
    Public AvailableArgs() As String = {"motd", "nohwprobe", "chkn=1", "preadduser", "hostname", "quiet", "gpuprobe", "cmdinject", "debug", "help"}
    Public availableCMDLineArgs() As String = {"createConf", "promptArgs"}              'Array for available command-line arguments
    Public slotsUsedName As String                                                      'Lists slots by names
    Public slotsUsedNum As Integer                                                      'Lists slots by numbers
    Public Capacities() As String                                                       'Capacity (in MB)
    Public totalSlots As Integer                                                        'Total slots
    Public configReader As StreamReader                                                 'Configuration file
    Declare Sub Sleep Lib "kernel32" (ByVal milliseconds As Integer)                    'Enable sleep (Mandatory, don't remove)

    Sub Main()

        'A title
        Console.Title = "Kernel Simulator version " & KernelVersion

        Try
            'Parse real command-line arguments
            For Each argu In Environment.GetCommandLineArgs
                CommandLineArgsParse.parseCMDArguments(argu)
            Next

            'Create config file and then read it
            Config.checkForUpgrade()
            If (File.Exists(Environ("USERPROFILE") + "\kernelConfig.ini") = True) Then
                configReader = My.Computer.FileSystem.OpenTextFileReader(Environ("USERPROFILE") + "\kernelConfig.ini")
            Else
                Config.createConfig(False)
                configReader = My.Computer.FileSystem.OpenTextFileReader(Environ("USERPROFILE") + "\kernelConfig.ini")
            End If
            Config.readConfig()

            'Show introduction. Don't remove license.
            Wln("|--+---> Welcome to the kernel, version {0} <---+--|", "neutralText", KernelVersion)
            Wln(vbNewLine + "    Kernel Simulator  Copyright (C) 2018  EoflaOE" + vbNewLine + _
                            "    This program comes with ABSOLUTELY NO WARRANTY, not even " + vbNewLine + _
                            "    MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine + _
                            "    This is free software, and you are welcome to redistribute it" + vbNewLine + _
                            "    under certain conditions; See COPYING file in source code." + vbNewLine, "license")

            'Phase 0: Initialize time and files, and check for quietness
            If (argsOnBoot = True) Then
                ArgumentPrompt.PromptArgs()
                If (argsFlag = True) Then
                    ArgumentParse.ParseArguments()
                End If
            End If
            If (argsInjected = True) Then
                ArgumentParse.ParseArguments()
                answerargs = ""
                argsInjected = False
            End If
            If (TimeDateIsSet = False) Then
                InitializeTimeDate()
                TimeDateIsSet = True
            End If
            InitializeDirectoryFile.Init()
            Wdbg("Kernel initialized, version {0}.", True, KernelVersion)
            If (Quiet = True Or quietProbe = True) Then
                'Continue the kernel, and don't print messages
                'Phase 1: Probe hardware and BIOS if nohwprobe is not passed
                HardwareProbe.ProbeHW(True, CChar("K"))

                'Phase 2: Username management
                UserManagement.initializeMainUsers()
                If (enableDemo = True) Then
                    UserManagement.addUser("demo")
                End If
                LoginFlag = True

                'Phase 3: Check for pre-user making
                If (CruserFlag = True) Then
                    adduser(arguser, argword)
                End If

                'Phase 4: Free unused RAM and log-in
                DisposeExit.DisposeAll()
                If (LoginFlag = True And maintenance = False) Then
                    Login.LoginPrompt()
                ElseIf (LoginFlag = True And maintenance = True) Then
                    LoginFlag = False
                    Wln("Enter the admin password for maintenance.", "neutralText")
                    answeruser = "root"
                    Login.showPasswordPrompt("root")
                End If
            Else
                'Continue the kernel
                'Phase 1: Probe hardware if nohwprobe is not passed
                HardwareProbe.ProbeHW(False, CChar("K"))

                'Phase 2: Username management
                UserManagement.initializeMainUsers()
                If (enableDemo = True) Then
                    UserManagement.addUser("demo")
                End If
                LoginFlag = True

                'Phase 3: Check for pre-user making
                If (CruserFlag = True) Then
                    adduser(arguser, argword)
                End If

                'Phase 4: Free unused RAM and log-in if the kernel isn't on maintenance mode
                DisposeExit.DisposeAll()
                If (LoginFlag = True And maintenance = False) Then
                    Login.LoginPrompt()
                ElseIf (LoginFlag = True And maintenance = True) Then
                    LoginFlag = False
                    Wln("Enter the admin password for maintenance.", "neutralText")
                    answeruser = "root"
                    Login.showPasswordPrompt("root")
                End If
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("U"), True, 5, "Kernel Error while booting: " + Err.Description)
        End Try

    End Sub

End Module
