
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

'About: THIS KERNEL IS NEAR TO BETA BUT NOT FINAL! Final kernel will be developed through another language, ASM included, depending on system.

Imports System
Imports System.IO
Imports System.Reflection
Imports System.Reflection.Assembly

Public Module Kernel

    'Variables
    Public KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String
    Public AvailableArgs() As String = {"nohwprobe", "quiet", "cmdinject", "debug", "maintenance", "help"}
    Public availableCMDLineArgs() As String = {"createConf", "promptArgs"}
    Public configReader As New IniFile()
    Public MOTDMessage As String
    Public HName As String
    Public MAL As String
    Public EnvironmentOSType As String = Environment.OSVersion.ToString

    'Windows only (Nothing happens in Unix - no exceptions)
    Declare Sub Sleep Lib "kernel32" (ByVal milliseconds As Integer)

    Sub Main()

        'TODO: Re-write the whole kernel in Beta
        'TODO: Give the kernel name of "Meritorious Kernalism" and the simulator name of "MeritSim".
        Try
            'A title
            Console.Title = "Kernel Simulator v" & KernelVersion & " - Compiled on " & getCompileDate()

            'Initialize everything
            InitEverything()

            'Show introduction. Don't remove license.
            Wln("---===+++> Welcome to the kernel | Version {0} <+++===---", "neutralText", KernelVersion)
            Wln(vbNewLine + "    Kernel Simulator  Copyright (C) 2018  EoflaOE" + vbNewLine + _
                            "    This program comes with ABSOLUTELY NO WARRANTY, not even " + vbNewLine + _
                            "    MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine + _
                            "    This is free software, and you are welcome to redistribute it" + vbNewLine + _
                            "    under certain conditions; See COPYING file in source code." + vbNewLine, "license")
            If (instanceChecked = False) Then InstanceCheck.MultiInstance()

            'Check for Mono Runtime before probing
            If (EnvironmentOSType.Contains("Unix")) Then
                'TODO: Unix systems must have their own probers after re-organizing subs
                'Phase 1: Skip Phase 1
                Wln("hwprobe: Probing not supported because it's not designed to run probers on Unix.", "neutralText")
                Wdbg("OSVersion is Unix = True", True)
            Else
                Wdbg("OSVersion is Unix = False", True)
                'Phase 1: Probe hardware and BIOS if nohwprobe is not passed
                If (Quiet = True Or quietProbe = True) Then
                    'Continue the kernel, and don't print messages
                    HardwareProbe.ProbeHW(True, CChar("K"))
                Else
                    'Continue the kernel
                    HardwareProbe.ProbeHW(False, CChar("K"))
                End If
            End If

            'Phase 2: Username management
            UserManagement.initializeMainUsers()
            If (enableDemo = True) Then UserManagement.adduser("demo")
            LoginFlag = True

            'Phase 3: Check for pre-user making
            If (CruserFlag = True) Then adduser(arguser, argword)

            'Phase 4: Parse Mods
            ModParser.ParseMods()

            'Phase 5: Free unused RAM
            DisposeExit.DisposeAll()

            'Phase 6: Log-in
            ShowTime()
            If (LoginFlag = True And maintenance = False) Then
                Login.LoginPrompt()
            ElseIf (LoginFlag = True And maintenance = True) Then
                LoginFlag = False
                Wln("Enter the admin password for maintenance.", "neutralText")
                answeruser = "root"
                Login.showPasswordPrompt(answeruser)
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("U"), True, 5, "Kernel Error while booting: {0}", Err.Description)
        End Try

    End Sub

    Sub InitEverything()

        'Parse real command-line arguments
        For Each argu In Environment.GetCommandLineArgs
            CommandLineArgsParse.parseCMDArguments(argu)
        Next

        'Check arguments, initialize date and files, and continue.
        If (argsOnBoot = True) Then
            ArgumentPrompt.PromptArgs()
            If (argsFlag = True) Then ArgumentParse.ParseArguments()
        End If
        If (argsInjected = True) Then
            ArgumentParse.ParseArguments()
            answerargs = ""
            argsInjected = False
        End If
        If (DebugMode = True) And (Not EnvironmentOSType.Contains("Unix")) Then
            dbgWriter = New StreamWriter(Environ("USERPROFILE") + "\kernelDbg.log", True)
            dbgWriter.AutoFlush = True
        ElseIf (DebugMode = True) And (EnvironmentOSType.Contains("Unix")) Then
            dbgWriter = New StreamWriter(Environ("HOME") + "/kernelDbg.log", True)
            dbgWriter.AutoFlush = True
        End If
        If (TimeDateIsSet = False) Then
            InitializeTimeDate()
            TimeDateIsSet = True
        End If
        InitializeDirectoryFile.Init()

        'Create config file and then read it
        Dim pathConfig As String
        If (EnvironmentOSType.Contains("Unix")) Then
            pathConfig = Environ("HOME") & "/kernelConfig.ini"
        Else
            pathConfig = Environ("USERPROFILE") & "\kernelConfig.ini"
        End If
        If (File.Exists(pathConfig) = True) Then
            configReader.Load(pathConfig)
        Else
            Config.createConfig(False)
            configReader.Load(pathConfig)
        End If
        Config.checkForUpgrade()
        Config.readImportantConfig()
        Config.readConfig()
        Wdbg("Kernel initialized, version {0}.", True, KernelVersion)

    End Sub

    Function getCompileDate() As DateTime
        'Variables and Constants
        Const Offset As Integer = 60 : Const LTOff As Integer = 8
        Dim asmByte(2047) As Byte : Dim asmStream As Stream

        'Get compile date
        If (EnvironmentOSType.Contains("Unix")) Then
            asmStream = New FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location) + "/Kernel Simulator.exe", FileMode.Open, FileAccess.Read)
        Else
            asmStream = New FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly.Location) + "\Kernel Simulator.exe", FileMode.Open, FileAccess.Read)
        End If
        asmStream.Read(asmByte, 0, 2048)
        If Not asmStream Is Nothing Then asmStream.Close()

        'We are almost there
        Dim i64 As Integer = BitConverter.ToInt32(asmByte, Offset)
        Dim compileseconds As Integer = BitConverter.ToInt32(asmByte, i64 + LTOff)
        Dim dt As New DateTime(1970, 1, 1, 0, 0, 0)
        dt = dt.AddSeconds(compileseconds)
        dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours)

        'Now return compile date
        Return dt
    End Function

End Module
