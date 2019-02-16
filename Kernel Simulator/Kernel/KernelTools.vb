
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

Imports System.IO
Imports System.Reflection
Imports System.Threading

Public Module KernelTools

    ''' <summary>
    ''' Indicates that there's something wrong with the kernel.
    ''' </summary>
    ''' <param name="ErrorType">Specifies whether the error is serious, fatal, unrecoverable, or double panic. C/S/D/F/U</param>
    ''' <param name="Reboot">Specifies whether to reboot on panic or to show the message to press any key to shut down</param>
    ''' <param name="RebootTime">Specifies seconds before reboot. 0 is instant. Negative numbers are not allowed.</param>
    ''' <param name="Description">Explanation of what happened when it errored.</param>
    ''' <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
    ''' <remarks></remarks>
    Public Sub KernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal ParamArray Variables() As Object)
        Try
            'TODO: Debugging and crash dump files on 0.0.6
            'Check error types and its capabilities
            If (ErrorType = "S" Or ErrorType = "F" Or ErrorType = "U" Or ErrorType = "D" Or ErrorType = "C") Then
                If (ErrorType = "U" And RebootTime > 5 Or ErrorType = "D" And RebootTime > 5) Then
                    'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                    'generate a second kernel error stating that there is something wrong with the reboot time.
                    KernelError(CChar("D"), True, 5, DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug.", currentLang), CStr(ErrorType))
                    StopPanicAndGoToDoublePanic = True
                ElseIf (ErrorType = "U" And Reboot = False Or ErrorType = "D" And Reboot = False) Then
                    'If the error type is unrecoverable, or double, and the rebooting is false where it should
                    'not be false, then it can deal with this issue by enabling reboot.
                    Wln(DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}.", currentLang), "uncontError", ErrorType)
                    Reboot = True
                End If
                If (RebootTime > 3600) Then
                    'If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                    Wln(DoTranslation("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute.", currentLang), "uncontError", ErrorType, CStr(RebootTime))
                    RebootTime = 60
                End If
            Else
                'If the error type is other than D/F/C/U/S, then it will generate a second error.
                KernelError(CChar("D"), True, 5, DoTranslation("DOUBLE PANIC: Error Type {0} invalid.", currentLang), CStr(ErrorType))
                StopPanicAndGoToDoublePanic = True
            End If

            'Parse variables ({0}, {1}, ...) in the "Description" string variable
            For v As Integer = 0 To Variables.Length - 1
                Description = Description.Replace("{" + CStr(v) + "}", Variables(v))
            Next

            'Fire an event
            EventManager.RaiseKernelError()

            'Check error capabilities
            If (Description.Contains("DOUBLE PANIC: ") And ErrorType = "D") Then
                'If the description has a double panic tag and the error type is Double
                Wln(DoTranslation("[{0}] dpanic: {1} -- Rebooting in {2} seconds...", currentLang), "uncontError", ErrorType, CStr(Description), CStr(RebootTime))
                Thread.Sleep(CInt(RebootTime * 1000))
                PowerManage("reboot")
            ElseIf (StopPanicAndGoToDoublePanic = True) Then
                'Switch to Double Panic
                Exit Sub
            ElseIf (ErrorType = "C" And Reboot = True) Then
                'Check if error is Continuable and reboot is enabled
                Reboot = False
                Wln(DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}.", currentLang) + vbNewLine +
                    DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel.", currentLang), "contError", ErrorType, CStr(Description))
                Console.ReadKey()
            ElseIf (ErrorType = "C" And Reboot = False) Then
                'Check if error is Continuable and reboot is disabled
                EventManager.RaiseContKernelError()
                Wln(DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel.", currentLang), "contError", ErrorType, CStr(Description))
                Console.ReadKey()
            ElseIf ((Reboot = False And ErrorType <> "D") Or (Reboot = False And ErrorType <> "C")) Then
                'If rebooting is disabled and the error type does not equal Double or Continuable
                Wln(DoTranslation("[{0}] panic: {1} -- Press any key to shutdown.", currentLang), "uncontError", ErrorType, CStr(Description))
                Console.ReadKey()
                PowerManage("shutdown")
            Else
                'Everything else.
                Wln(DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds...", currentLang), "uncontError", ErrorType, CStr(Description), CStr(RebootTime))
                Thread.Sleep(CInt(RebootTime * 1000))
                PowerManage("reboot")
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
                KernelError(CChar("D"), True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}", currentLang), Err.Description)
            Else
                KernelError(CChar("D"), True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}", currentLang), Err.Description)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Manage computer's (actually, simulated computer) power
    ''' </summary>
    ''' <param name="PowerMode">Whether it would be "shutdown" or "reboot"</param>
    ''' <remarks></remarks>
    Public Sub PowerManage(ByVal PowerMode As String)

        Wdbg("Power management has the argument of {0}", PowerMode)
        If (PowerMode = "shutdown") Then
            EventManager.RaisePreShutdown()
            Wln(DoTranslation("Shutting down...", currentLang), "neutralText")
            KernelTools.ResetEverything()
            EventManager.RaisePostShutdown()

            'Stop all mods
            ParseMods(False)
            Environment.Exit(0)
        ElseIf (PowerMode = "reboot") Then
            EventManager.RaisePreReboot()
            Wln(DoTranslation("Rebooting...", currentLang), "neutralText")
            KernelTools.ResetEverything()
            EventManager.RaisePostReboot()

            'Stop all mods
            ParseMods(False)
            Console.Clear()
            Main()
        End If

    End Sub

    Sub ResetEverything()

        'Reset every variable that is resettable
        If (argsInjected = False) Then
            answerargs = Nothing
        End If
        Erase BootArgs
        argsFlag = False
        Computers = Nothing
        Quiet = False
        StopPanicAndGoToDoublePanic = False
        strcommand = Nothing
        slotsUsedName = Nothing
        slotsUsedNum = 0
        totalSlots = 0
        AvailableDirs.Clear()
        modcmnds.Clear()
        moddefs.Clear()
        scripts.Clear()
        Wdbg("General variables reset")

        'Reset users
        UserManagement.resetUsers()
        Wdbg("User variables reset")

        'Reset hardware info
        HDDList.Clear()
        BIOSList.Clear()
        GPUList.Clear()
        RAMList.Clear()
        CPUList.Clear()

        'Release RAM used
        DisposeExit.DisposeAll()
        Wdbg("Garbage collector finished")

        'Disable Debugger
        If (DebugMode = True) Then
            DebugMode = False
            dbgWriter.Close() : dbgWriter.Dispose()
        End If

        'Reset manpages
        Pages.Clear()

        'Close settings
        configReader = New IniFile()

    End Sub

    Sub InitEverything()

        'Parse real command-line arguments
        For Each argu In Environment.GetCommandLineArgs
            CommandLineArgsParse.parseCMDArguments(argu)
        Next

        'Check arguments and initialize date and files.
        If (argsOnBoot = True) Then
            ArgumentPrompt.PromptArgs()
            If (argsFlag = True) Then ArgumentParse.ParseArguments()
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

        'Initialize debugger
        If (DebugMode = True) And (Not EnvironmentOSType.Contains("Unix")) Then
            dbgWriter = New StreamWriter(Environ("USERPROFILE") + "\kernelDbg.log", True)
        ElseIf (DebugMode = True) And (EnvironmentOSType.Contains("Unix")) Then
            dbgWriter = New StreamWriter(Environ("HOME") + "/kernelDbg.log", True)
        End If
        If (DebugMode = True) Then dbgWriter.AutoFlush = True

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

        'Show introduction. Don't remove license.
        Wln(DoTranslation("---===+++> Welcome to the kernel | Version {0} <+++===---", currentLang), "neutralText", KernelVersion)
        Wln(vbNewLine + "    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE" + vbNewLine +
                        "    This program comes with ABSOLUTELY NO WARRANTY, not even " + vbNewLine +
                        "    MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine +
                        "    This is free software, and you are welcome to redistribute it" + vbNewLine +
                        "    under certain conditions; See COPYING file in source code." + vbNewLine, "license")
        Wln("OS: Running on {0}", "neutralText", EnvironmentOSType)

        'Parse current theme string
        ParseCurrentTheme()

        'Send a message on debugger
        If (instanceChecked = False) Then MultiInstance()
        Wdbg("Kernel initialized, version {0}.", KernelVersion)
        Wdbg("OS: {0}", EnvironmentOSType)

        'Initialize manual pages
        For Each titleMan As String In AvailablePages
            Pages.Add(titleMan, New Manual(titleMan))
            CheckManual(titleMan)
        Next

    End Sub

    Sub MultiInstance()

        'Check to see if multiple Kernel Simulator processes are running.
        Static ksInst As Mutex
        Dim ksOwner As Boolean
        ksInst = New Mutex(True, "Kernel Simulator", ksOwner)
        If (ksOwner = False) Then
            KernelError(CChar("F"), False, 0, DoTranslation("Another instance of Kernel Simulator is running. Shutting down in case of interference.", currentLang))
        End If
        instanceChecked = True

    End Sub

    Function GetCompileDate() As DateTime
        'Variables and Constants
        Const Offset As Integer = 60 : Const LTOff As Integer = 8
        Dim asmByte(2047) As Byte : Dim asmStream As Stream
        Dim codePath As Assembly = Assembly.GetExecutingAssembly

        'Get compile date
        asmStream = New FileStream(Path.GetFullPath(codePath.Location), FileMode.Open, FileAccess.Read)
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
