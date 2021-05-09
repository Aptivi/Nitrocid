
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
Imports System.Diagnostics.Process
Imports Newtonsoft.Json.Linq

Public Module KernelTools

    ' A dictionary for storing paths and files (used for mods, screensavers, etc.)
    Public paths As New Dictionary(Of String, String)

    Friend RPCPowerListener As New Thread(AddressOf PowerManage)

    ' ----------------------------------------------- Kernel errors -----------------------------------------------

    ''' <summary>
    ''' Indicates that there's something wrong with the kernel.
    ''' </summary>
    ''' <param name="ErrorType">Specifies whether the error is serious, fatal, unrecoverable, or double panic. C/S/D/F/U</param>
    ''' <param name="Reboot">Specifies whether to reboot on panic or to show the message to press any key to shut down</param>
    ''' <param name="RebootTime">Specifies seconds before reboot. 0 is instant. Negative numbers are not allowed.</param>
    ''' <param name="Description">Explanation of what happened when it errored.</param>
    ''' <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
    ''' <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
    Public Sub KernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal ParamArray Variables() As Object)
        Try
            'Unquiet
            If BootArgs IsNot Nothing Then
                If BootArgs.Contains("quiet") Then
                    Wdbg("I", "Removing quiet...")
                    Console.SetOut(DefConsoleOut)
                End If
            End If

            'Check error types and its capabilities
            Wdbg("I", "Error type: {0}", ErrorType)
            If ErrorType = "S" Or ErrorType = "F" Or ErrorType = "U" Or ErrorType = "D" Or ErrorType = "C" Then
                If ErrorType = "U" And RebootTime > 5 Or ErrorType = "D" And RebootTime > 5 Then
                    'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                    'generate a second kernel error stating that there is something wrong with the reboot time.
                    Wdbg("W", "Errors that have type {0} shouldn't exceed 5 seconds. RebootTime was {1} seconds", ErrorType, RebootTime)
                    KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug."), Nothing, CStr(ErrorType))
                    StopPanicAndGoToDoublePanic = True
                ElseIf ErrorType = "U" And Reboot = False Or ErrorType = "D" And Reboot = False Then
                    'If the error type is unrecoverable, or double, and the rebooting is false where it should
                    'not be false, then it can deal with this issue by enabling reboot.
                    Wdbg("W", "Errors that have type {0} enforced Reboot = True.", ErrorType)
                    W(DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}."), True, ColTypes.Uncontinuable, ErrorType)
                    Reboot = True
                End If
                If RebootTime > 3600 Then
                    'If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                    Wdbg("W", "RebootTime shouldn't exceed 1 hour. Was {0} seconds", RebootTime)
                    W(DoTranslation("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute."), True, ColTypes.Uncontinuable, ErrorType, CStr(RebootTime))
                    RebootTime = 60
                End If
            Else
                'If the error type is other than D/F/C/U/S, then it will generate a second error.
                Wdbg("E", "Error type {0} is not valid.", ErrorType)
                KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Error Type {0} invalid."), Nothing, CStr(ErrorType))
                StopPanicAndGoToDoublePanic = True
            End If

            'Parse variables ({0}, {1}, ...) in the "Description" string variable
            Description = Description.FormatString(Variables)

            'Fire an event
            EventManager.RaiseKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)

            'Make a dump file
            GeneratePanicDump(Description, ErrorType, Exc)

            'Check error capabilities
            If Description.Contains("DOUBLE PANIC: ") And ErrorType = "D" Then
                'If the description has a double panic tag and the error type is Double
                Wdbg("F", "Double panic caused by bug in kernel crash.")
                W(DoTranslation("[{0}] dpanic: {1} -- Rebooting in {2} seconds..."), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                Thread.Sleep(RebootTime * 1000)
                Wdbg("F", "Rebooting")
                PowerManage("reboot")
                adminList.Clear()
                disabledList.Clear()
            ElseIf StopPanicAndGoToDoublePanic = True Then
                'Switch to Double Panic
                Exit Sub
            ElseIf ErrorType = "C" And Reboot = True Then
                'Check if error is Continuable and reboot is enabled
                Wdbg("W", "Continuable kernel errors shouldn't have Reboot = True.")
                W(DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}.") + vbNewLine +
                  DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), True, ColTypes.Continuable, ErrorType, Description)
                Console.ReadKey()
            ElseIf ErrorType = "C" And Reboot = False Then
                'Check if error is Continuable and reboot is disabled
                EventManager.RaiseContKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)
                W(DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), True, ColTypes.Continuable, ErrorType, Description)
                Console.ReadKey()
            ElseIf (Reboot = False And ErrorType <> "D") Or (Reboot = False And ErrorType <> "C") Then
                'If rebooting is disabled and the error type does not equal Double or Continuable
                Wdbg("W", "Reboot is False, ErrorType is not double or continuable.")
                W(DoTranslation("[{0}] panic: {1} -- Press any key to shutdown."), True, ColTypes.Uncontinuable, ErrorType, Description)
                Console.ReadKey()
                PowerManage("shutdown")
            Else
                'Everything else.
                Wdbg("F", "Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", RebootTime, ErrorType)
                W(DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds..."), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                Thread.Sleep(RebootTime * 1000)
                PowerManage("reboot")
                adminList.Clear()
                disabledList.Clear()
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(ex.StackTrace, True, ColTypes.Uncontinuable) : WStkTrc(ex)
                KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}"), ex, ex.Message)
            Else
                KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}"), ex, ex.Message)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Generates the stack trace dump file for kernel panics
    ''' </summary>
    ''' <param name="Description">Error description</param>
    ''' <param name="ErrorType">Error type</param>
    ''' <param name="Exc">Exception</param>
    Sub GeneratePanicDump(ByVal Description As String, ByVal ErrorType As Char, ByVal Exc As Exception)
        Try
            'Open a file stream for dump
            Dim Dump As New StreamWriter($"{paths("Home")}/dmp_{RenderDate.Replace("/", "-")}_{RenderTime.Replace(":", "-")}.txt")
            Wdbg("I", "Opened file stream in home directory, saved as dmp_{0}_{1}.txt", $"{RenderDate.Replace("/", "-")}_{RenderTime.Replace(":", "-")}")

            'Write info (Header)
            Dump.AutoFlush = True
            Dump.WriteLine(DoTranslation("----------------------------- Kernel panic dump -----------------------------") + vbNewLine + vbNewLine +
                           DoTranslation(">> Panic information <<") + vbNewLine +
                           DoTranslation("> Description: {0}") + vbNewLine +
                           DoTranslation("> Error type: {1}") + vbNewLine +
                           DoTranslation("> Date and Time: {2}") + vbNewLine, Description, ErrorType, FormatDateTime(Date.Now, DateFormat.GeneralDate))

            'Write Info (Exception)
            If Not IsNothing(Exc) Then
                Dim Count As Integer = 1
                Dump.WriteLine(DoTranslation(">> Exception information <<") + vbNewLine +
                               DoTranslation("> Exception: {0}") + vbNewLine +
                               DoTranslation("> Description: {1}") + vbNewLine +
                               DoTranslation("> HRESULT: {2}") + vbNewLine +
                               DoTranslation("> Source: {3}") + vbNewLine + vbNewLine +
                               DoTranslation("> Stack trace <") + vbNewLine + vbNewLine +
                               Exc.StackTrace + vbNewLine + vbNewLine +
                               DoTranslation(">> Inner exception {0} information <<"), Exc.ToString.Substring(0, Exc.ToString.IndexOf(":")), Exc.Message, Exc.HResult, Exc.Source)

                'Write info (Inner exceptions)
                Dim InnerExc As Exception = Exc.InnerException
                While InnerExc IsNot Nothing
                    Dump.WriteLine(DoTranslation("> Exception: {0}") + vbNewLine +
                                   DoTranslation("> Description: {1}") + vbNewLine +
                                   DoTranslation("> HRESULT: {2}") + vbNewLine +
                                   DoTranslation("> Source: {3}") + vbNewLine + vbNewLine +
                                   DoTranslation("> Stack trace <") + vbNewLine + vbNewLine +
                                   InnerExc.StackTrace + vbNewLine, InnerExc.ToString.Substring(0, InnerExc.ToString.IndexOf(":")), InnerExc.Message, InnerExc.HResult, InnerExc.Source)
                    InnerExc = InnerExc.InnerException
                    If InnerExc IsNot Nothing Then
                        Dump.WriteLine(DoTranslation(">> Inner exception {0} information <<"), Count)
                    Else
                        Dump.WriteLine(DoTranslation(">> Exception {0} is the root cause <<"), Count - 1)
                    End If
                    Count += 1
                End While
                Dump.WriteLine()
            Else
                Dump.WriteLine(DoTranslation(">> No exception; might be a kernel error. <<") + vbNewLine)
            End If

            'Write info (Frames)
            Dump.WriteLine(DoTranslation(">> Frames, files, lines, and columns <<"))
            Try
                Dim ExcTrace As New StackTrace(Exc, True)
                Dim FrameNo As Integer = 1
                For Each Frame As StackFrame In ExcTrace.GetFrames
                    If Not (Frame.GetFileName = "" And Frame.GetFileLineNumber = 0 And Frame.GetFileColumnNumber = 0) Then
                        Dump.WriteLine(DoTranslation("> Frame {0}: File: {1} | Line: {2} | Column: {3}"), FrameNo, Frame.GetFileName, Frame.GetFileLineNumber, Frame.GetFileColumnNumber)
                    End If
                    FrameNo += 1
                Next
            Catch ex As Exception
                WStkTrc(ex)
                Dump.WriteLine(DoTranslation("> There is an error when trying to get frame information. {0}: {1}"), ex.ToString.Substring(0, ex.ToString.IndexOf(":")), ex.Message.Replace(vbNewLine, " | "))
            End Try

            'Close stream
            Wdbg("I", "Closing file stream for dump...")
            Dump.Flush() : Dump.Close()
        Catch ex As Exception
            W(DoTranslation("Dump information gatherer crashed when trying to get information about {0}: {1}"), True, ColTypes.Err, Exc.ToString.Substring(0, Exc.ToString.IndexOf(":")), ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    ' ----------------------------------------------- Power management -----------------------------------------------

    ''' <summary>
    ''' Manage computer's (actually, simulated computer) power
    ''' </summary>
    ''' <param name="PowerMode">Whether it would be "shutdown", "rebootsafe", or "reboot"</param>
    Public Sub PowerManage(ByVal PowerMode As String, Optional ByVal IP As String = "0.0.0.0")
        Wdbg("I", "Power management has the argument of {0}", PowerMode)
        If PowerMode = "shutdown" Then
            EventManager.RaisePreShutdown()
            W(DoTranslation("Shutting down..."), True, ColTypes.Neutral)
            ResetEverything()
            EventManager.RaisePostShutdown()
            Environment.Exit(0)
        ElseIf PowerMode = "reboot" Then
            EventManager.RaisePreReboot()
            W(DoTranslation("Rebooting..."), True, ColTypes.Neutral)
            ResetEverything()
            EventManager.RaisePostReboot()
            Console.Clear()
            RebootRequested = True
            LogoutRequested = True
            SafeMode = False
        ElseIf PowerMode = "rebootsafe" Then
            EventManager.RaisePreReboot()
            W(DoTranslation("Rebooting..."), True, ColTypes.Neutral)
            ResetEverything()
            EventManager.RaisePostReboot()
            Console.Clear()
            RebootRequested = True
            LogoutRequested = True
            SafeMode = True
        ElseIf PowerMode = "remoteshutdown" Then
            SendCommand("<Request:Shutdown>(" + IP + ")", IP)
        ElseIf PowerMode = "remoterestart" Then
            SendCommand("<Request:Reboot>(" + IP + ")", IP)
        End If
    End Sub

    ' ----------------------------------------------- Init and reset -----------------------------------------------
    ''' <summary>
    ''' Reset everything for the next restart
    ''' </summary>
    Sub ResetEverything()
        'Reset every variable that is resettable
        If argsInjected = False Then
            answerargs = Nothing
        End If
        Erase BootArgs
        argsFlag = False
        StopPanicAndGoToDoublePanic = False
        strcommand = Nothing
        modcmnds.Clear()
        moddefs.Clear()
        FTPModCommands.Clear()
        FTPModDefs.Clear()
        SFTPModCommands.Clear()
        SFTPModDefs.Clear()
        MailModCommands.Clear()
        MailModDefs.Clear()
        RDebugModDefs.Clear()
        DebugModCmds.Clear()
        TestModDefs.Clear()
        Test_ModCommands.Clear()
        TextEdit_ModCommands.Clear()
        TextEdit_ModHelpEntries.Clear()
        ZipShell_ModCommands.Clear()
        ZipShell_ModHelpEntries.Clear()
        scripts.Clear()
        Aliases.Clear()
        RemoteDebugAliases.Clear()
        FTPShellAliases.Clear()
        SFTPShellAliases.Clear()
        MailShellAliases.Clear()
        Wdbg("I", "General variables reset")

        'Reset hardware info
        HardwareInfo = Nothing
        Wdbg("I", "Hardware info reset.")

        'Release RAM used
        DisposeAll()
        Wdbg("I", "Garbage collector finished")

        'Disconnect all hosts from remote debugger
        StartRDebugThread(False)
        Wdbg("I", "Remote debugger stopped")

        'Close settings
        configReader = New IniFile()
        Wdbg("I", "Settings closed")

        'Stop all mods
        ParseMods(False)
        Wdbg("I", "Mods stopped")

        'Disable Debugger
        If DebugMode = True Then
            Wdbg("I", "Shutting down debugger")
            DebugMode = False
            dbgWriter.Close() : dbgWriter.Dispose()
        End If

        'Stop RPC
        RPCThread.Abort()
        RPCListen?.Close()
        RPCListen = Nothing
        RPCThread = New Thread(AddressOf RecCommand) With {.IsBackground = True}

        'Disconnect from mail
        IMAP_Client.Disconnect(True)
        SMTP_Client.Disconnect(True)

        'Disable safe mode
        SafeMode = False
    End Sub

    ''' <summary>
    ''' Initializes everything
    ''' </summary>
    Sub InitEverything()
        'Initialize help
        InitHelp()
        InitFTPHelp()
        InitSFTPHelp()
        IMAPInitHelp()
        InitRDebugHelp()
        InitTestHelp()
        TextEdit_UpdateHelp()
        ZipShell_UpdateHelp()

        'We need to create a file so InitAliases() won't give out an error
        If Not File.Exists(paths("Aliases")) Then
            Dim fstream As FileStream = File.Create(paths("Aliases"))
            fstream.Close()
        End If

        'Initialize aliases
        InitAliases()

        'Initialize date
        If Not TimeDateIsSet Then
            InitTimeDate()
            TimeDateIsSet = True
        End If

        'Check for multiple instances of KS
        If instanceChecked = False Then MultiInstance()

        'Create config file and then read it
        InitializeConfig()

        'Load user token
        LoadUserToken()

        If RebootRequested Then
            Exit Sub
        End If

        'Show welcome message.
        If StartScroll Then
            WriteSlowlyC(">> " + DoTranslation("Welcome to the kernel! - Version {0}") + " <<", True, 10, ColTypes.Neutral, KernelVersion)
        Else
            W(">> " + DoTranslation("Welcome to the kernel! - Version {0}") + " <<", True, ColTypes.Neutral, KernelVersion)
        End If

        'Show license
        W(vbNewLine + "    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE" + vbNewLine +
                      "    This program comes with ABSOLUTELY NO WARRANTY, not even " + vbNewLine +
                      "    MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine +
                      "    This is free software, and you are welcome to redistribute it" + vbNewLine +
                      "    under certain conditions; See COPYING file in source code." + vbNewLine, True, ColTypes.License)
        W("OS: " + DoTranslation("Running on {0}"), True, ColTypes.Neutral, Environment.OSVersion.ToString)
        W("KS: " + DoTranslation("Built in {0}"), True, ColTypes.Neutral, Render(GetCompileDate()))

        'Show dev version notice
#If SPECIFIER = "DEV" Then 'WARNING: When the development nearly ends after "NEARING" stage, change the compiler constant value to "REL" to suppress this message out of stable versions
        W(DoTranslation("Looks like you were running the development version of the kernel. While you can see the aspects, it is frequently updated and might introduce bugs. It is recommended that you stay on the stable version."), True, ColTypes.Neutral)
#ElseIf SPECIFIER = "RC" Then
        W(DoTranslation("Looks like you were running the release candidate version. It is recommended that you stay on the stable version."), True, ColTypes.Neutral)
#ElseIf SPECIFIER = "NEARING" Then
        W(DoTranslation("Looks like you were running the nearing-release version. While it's safer to use now, it is recommended that you stay on the stable version."), True, ColTypes.Neutral)
#End If

        'Parse real command-line arguments
        For Each argu In Environment.GetCommandLineArgs
            ParseCMDArguments(argu)
        Next

        'Check arguments
        If argsOnBoot Then
            PromptArgs()
            If argsFlag Then ParseArguments()
        End If
        If argsInjected Then
            ParseArguments()
            answerargs = ""
            argsInjected = False
        End If

        'Write headers for debug
        Wdbg("I", "-------------------------------------------------------------------")
        Wdbg("I", "Kernel initialized, version {0}.", KernelVersion)
        Wdbg("I", "OS: {0}", Environment.OSVersion.ToString)

        'Populate ban list for debug devices
        PopulateBlockedDevices()

        'Start screensaver timeout
        If Not Timeout.IsBusy Then Timeout.RunWorkerAsync()
    End Sub

    ''' <summary>
    ''' Initializes the paths
    ''' </summary>
    Sub InitPaths()
        If IsOnUnix() Then
            paths.AddIfNotFound("Mods", Environ("HOME") + "/KSMods/")
            paths.AddIfNotFound("Configuration", Environ("HOME") + "/kernelConfig.ini")
            paths.AddIfNotFound("Debugging", Environ("HOME") + "/kernelDbg.log")
            paths.AddIfNotFound("Aliases", Environ("HOME") + "/Aliases.json")
            paths.AddIfNotFound("Users", Environ("HOME") + "/Users.json")
            paths.AddIfNotFound("FTPSpeedDial", Environ("HOME") + "/FTP_SpeedDial.json")
            paths.AddIfNotFound("SFTPSpeedDial", Environ("HOME") + "/SFTP_SpeedDial.json")
            paths.AddIfNotFound("DebugDevNames", Environ("HOME") + "/DebugDeviceNames.json")
            paths.AddIfNotFound("Home", Environ("HOME"))
            paths.AddIfNotFound("Temp", "/tmp")
        Else
            paths.AddIfNotFound("Mods", Environ("USERPROFILE").Replace("\", "/") + "/KSMods/")
            paths.AddIfNotFound("Configuration", Environ("USERPROFILE").Replace("\", "/") + "/kernelConfig.ini")
            paths.AddIfNotFound("Debugging", Environ("USERPROFILE").Replace("\", "/") + "/kernelDbg.log")
            paths.AddIfNotFound("Aliases", Environ("USERPROFILE").Replace("\", "/") + "/Aliases.json")
            paths.AddIfNotFound("Users", Environ("USERPROFILE").Replace("\", "/") + "/Users.json")
            paths.AddIfNotFound("FTPSpeedDial", Environ("USERPROFILE").Replace("\", "/") + "/FTP_SpeedDial.json")
            paths.AddIfNotFound("SFTPSpeedDial", Environ("USERPROFILE").Replace("\", "/") + "/SFTP_SpeedDial.json")
            paths.AddIfNotFound("DebugDevNames", Environ("USERPROFILE").Replace("\", "/") + "/DebugDeviceNames.json")
            paths.AddIfNotFound("Home", Environ("USERPROFILE").Replace("\", "/"))
            paths.AddIfNotFound("Temp", Environ("TEMP").Replace("\", "/"))
        End If
    End Sub

    ' ----------------------------------------------- Misc -----------------------------------------------

    Sub MultiInstance()
        'Check to see if multiple Kernel Simulator processes are running.
        Static ksInst As Mutex
        Dim ksOwner As Boolean
        ksInst = New Mutex(True, "Kernel Simulator", ksOwner)
        If Not ksOwner Then
            KernelError("F", False, 0, DoTranslation("Another instance of Kernel Simulator is running. Shutting down in case of interference."), Nothing)
        End If
        instanceChecked = True
    End Sub

    ''' <summary>
    ''' Fetches the GitHub repo to see if there are any updates
    ''' </summary>
    ''' <returns>A list which contains both the version and the URL</returns>
    Public Function FetchKernelUpdates() As List(Of String)
        Try
            Dim UpdateSpecifier As New List(Of String)
            Dim UpdateDown As New WebClient
            UpdateDown.Headers.Add(HttpRequestHeader.UserAgent, "EoflaOE") 'Because api.github.com requires the UserAgent header to be put, else, 403 error occurs.
            Dim UpdateStr As String = UpdateDown.DownloadString("https://api.github.com/repos/EoflaOE/Kernel-Simulator/releases")
            Dim UpdateToken As JToken = JToken.Parse(UpdateStr)
            Dim UpdateVer As New Version(UpdateToken.First.SelectToken("tag_name").ToString.ReplaceAll({"v", "-alpha"}, ""))
            Dim UpdateURL As String = UpdateToken.First.SelectToken("html_url")
            Dim CurrentVer As New Version(KernelVersion)
            If UpdateVer > CurrentVer Then
                'Found a new version
                UpdateSpecifier.Add(UpdateVer.ToString)
                UpdateSpecifier.Add(UpdateURL)
            End If
            Return UpdateSpecifier
        Catch ex As Exception
            Wdbg("E", "Failed to check for updates: {0}", ex.Message)
            WStkTrc(ex)
        End Try
        Return Nothing
    End Function

    Sub CheckKernelUpdates()
        W(DoTranslation("Checking for system updates..."), True, ColTypes.Neutral)
        Dim AvailableUpdates As List(Of String) = FetchKernelUpdates()
        If Not IsNothing(AvailableUpdates) And AvailableUpdates.Count > 0 Then
            W(DoTranslation("Found new version: "), False, ColTypes.ListEntry)
            W(AvailableUpdates(0), True, ColTypes.ListValue)
            W(DoTranslation("You can download it at: "), False, ColTypes.ListEntry)
            W(AvailableUpdates(1), True, ColTypes.ListValue)
        ElseIf IsNothing(AvailableUpdates) Then
            W(DoTranslation("Failed to check for updates."), True, ColTypes.Err)
        End If
    End Sub

    Function GetCompileDate() As DateTime 'Always successful, no need to put Try Catch
        'Variables and Constants
        Const Offset As Integer = 60 : Const LTOff As Integer = 8
        Dim asmByte(2047) As Byte : Dim asmStream As Stream
        Dim codePath As Assembly = Assembly.GetExecutingAssembly

        'Get compile date
        asmStream = New FileStream(Path.GetFullPath(codePath.Location), FileMode.Open, FileAccess.Read)
        asmStream.Read(asmByte, 0, 2048)
        If asmStream IsNot Nothing Then asmStream.Close()

        'We are almost there
        Dim i64 As Integer = BitConverter.ToInt32(asmByte, Offset)
        Dim compileseconds As Integer = BitConverter.ToInt32(asmByte, i64 + LTOff)
        Dim dt As New DateTime(1970, 1, 1, 0, 0, 0)
        dt = dt.AddSeconds(compileseconds)
        dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours)

        'Now return compile date
        Return dt
    End Function
#If SPECIFIER = "DEV" Then
    Function GetCompileDate(ByVal Asm As Assembly) As DateTime 'Only exists in development version.
        Dim dt As New DateTime(1970, 1, 1, 0, 0, 0)
        Try
            'Variables and Constants
            Const Offset As Integer = 60 : Const LTOff As Integer = 8
            Dim asmByte(2047) As Byte : Dim asmStream As Stream
            Dim codePath As Assembly = Asm

            'Get compile date
            asmStream = New FileStream(Path.GetFullPath(codePath.Location), FileMode.Open, FileAccess.Read)
            asmStream.Read(asmByte, 0, 2048)
            If asmStream IsNot Nothing Then asmStream.Close()

            'We are almost there
            Dim i64 As Integer = BitConverter.ToInt32(asmByte, Offset)
            Dim compileseconds As Integer = BitConverter.ToInt32(asmByte, i64 + LTOff)
            dt = dt.AddSeconds(compileseconds)
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours)
        Catch ex As Exception
            W(DoTranslation("Error while trying to get compile date of assembly {0}: {1}"), True, ColTypes.Err, Asm.CodeBase, ex.Message)
        End Try

        'Now return compile date
        Return dt
    End Function
#End If

    Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal dwMinimumWorkingSetSize As Int32, ByVal dwMaximumWorkingSetSize As Int32) As Int32

    ''' <summary>
    ''' Disposes all unused memory.
    ''' </summary>
    Public Sub DisposeAll()

        Try
            Dim proc As Process = GetCurrentProcess()
            Wdbg("I", "Before garbage collection: {0} bytes", proc.PrivateMemorySize64)
            Wdbg("I", "Garbage collector starting... Max generators: {0}", GC.MaxGeneration.ToString)
            GC.Collect()
            GC.WaitForPendingFinalizers()
            If IsOnWindows() Then
                SetProcessWorkingSetSize(GetCurrentProcess().Handle, -1, -1)
            End If
            Wdbg("I", "After garbage collection: {0} bytes", proc.PrivateMemorySize64)
            proc.Dispose()
            EventManager.RaiseGarbageCollected()
        Catch ex As Exception
            W(DoTranslation("Error trying to free RAM: {0} - Continuing..."), True, ColTypes.Err, ex.Message)
            If DebugMode = True Then
                W(ex.StackTrace, True, ColTypes.Neutral) : Wdbg("Error freeing RAM: {0}", ex.Message) : WStkTrc(ex)
            End If
        End Try

    End Sub

    Sub FactoryReset()
        File.Delete(paths("Aliases"))
        File.Delete(paths("Configuration"))
        File.Delete(paths("Debugging"))
        File.Delete(paths("Users"))
        File.Delete(paths("FTPSpeedDial"))
        File.Delete(paths("Home") + "/MOTD.txt")
        File.Delete(paths("Home") + "/MAL.txt")
        Directory.Delete(paths("Mods"))
        Environment.Exit(0)
    End Sub

End Module
