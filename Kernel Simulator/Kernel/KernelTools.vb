
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

    'Variables
    Friend RPCPowerListener As New Thread(AddressOf PowerManage) With {.Name = "RPC Power Listener Thread"}
    Friend LastKernelErrorException As Exception
    Friend StopPanicAndGoToDoublePanic As Boolean
    Friend InstanceChecked As Boolean

    'Windows function pinvokes
    Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (hProcess As IntPtr, dwMinimumWorkingSetSize As Integer, dwMaximumWorkingSetSize As Integer) As Integer

    '----------------------------------------------- Kernel errors -----------------------------------------------

    ''' <summary>
    ''' Indicates that there's something wrong with the kernel.
    ''' </summary>
    ''' <param name="ErrorType">Specifies the error type.</param>
    ''' <param name="Reboot">Specifies whether to reboot on panic or to show the message to press any key to shut down</param>
    ''' <param name="RebootTime">Specifies seconds before reboot. 0 is instant. Negative numbers are not allowed.</param>
    ''' <param name="Description">Explanation of what happened when it errored.</param>
    ''' <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
    ''' <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
    Public Sub KernelError(ErrorType As KernelErrorLevel, Reboot As Boolean, RebootTime As Long, Description As String, Exc As Exception, ParamArray Variables() As Object)
        KernelErrored = True
        LastKernelErrorException = Exc
        NotifyKernelError = True

        Try
            'Unquiet
            If EnteredArguments IsNot Nothing Then
                If EnteredArguments.Contains("quiet") Then
                    Wdbg(DebugLevel.I, "Removing quiet...")
                    Console.SetOut(DefConsoleOut)
                End If
            End If

            'Check error types and its capabilities
            Wdbg(DebugLevel.I, "Error type: {0}", ErrorType)
            If [Enum].IsDefined(GetType(KernelErrorLevel), ErrorType) Then
                If (ErrorType = KernelErrorLevel.U Or ErrorType = KernelErrorLevel.D) And RebootTime > 5 Then
                    'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                    'generate a second kernel error stating that there is something wrong with the reboot time.
                    Wdbg(DebugLevel.W, "Errors that have type {0} shouldn't exceed 5 seconds. RebootTime was {1} seconds", ErrorType, RebootTime)
                    KernelError(KernelErrorLevel.D, True, 5, DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug."), Nothing, CStr(ErrorType))
                    StopPanicAndGoToDoublePanic = True
                ElseIf (ErrorType = KernelErrorLevel.U Or ErrorType = KernelErrorLevel.D) And Reboot = False Then
                    'If the error type is unrecoverable, or double, and the rebooting is false where it should
                    'not be false, then it can deal with this issue by enabling reboot.
                    Wdbg(DebugLevel.W, "Errors that have type {0} enforced Reboot = True.", ErrorType)
                    Write(DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}."), True, ColTypes.Uncontinuable, ErrorType)
                    Reboot = True
                End If
                If RebootTime > 3600 Then
                    'If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                    Wdbg(DebugLevel.W, "RebootTime shouldn't exceed 1 hour. Was {0} seconds", RebootTime)
                    Write(DoTranslation("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute."), True, ColTypes.Uncontinuable, ErrorType, CStr(RebootTime))
                    RebootTime = 60
                End If
            Else
                'If the error type is other than D/F/C/U/S, then it will generate a second error.
                Wdbg(DebugLevel.E, "Error type {0} is not valid.", ErrorType)
                KernelError(KernelErrorLevel.D, True, 5, DoTranslation("DOUBLE PANIC: Error Type {0} invalid."), Nothing, CStr(ErrorType))
                StopPanicAndGoToDoublePanic = True
            End If

            'Format the "Description" string variable
            Description = String.Format(Description, Variables)

            'Fire an event
            Kernel.EventManager.RaiseKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)

            'Make a dump file
            GeneratePanicDump(Description, ErrorType, Exc)

            'Check error capabilities
            If Description.Contains("DOUBLE PANIC: ") And ErrorType = KernelErrorLevel.D Then
                'If the description has a double panic tag and the error type is Double
                Wdbg(DebugLevel.F, "Double panic caused by bug in kernel crash.")
                Write(DoTranslation("[{0}] dpanic: {1} -- Rebooting in {2} seconds..."), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                Thread.Sleep(RebootTime * 1000)
                Wdbg(DebugLevel.F, "Rebooting")
                PowerManage(PowerMode.Reboot)
            ElseIf StopPanicAndGoToDoublePanic = True Then
                'Switch to Double Panic
                StopPanicAndGoToDoublePanic = False
                Exit Sub
            ElseIf ErrorType = KernelErrorLevel.C And Reboot = True Then
                'Check if error is Continuable and reboot is enabled
                Wdbg(DebugLevel.W, "Continuable kernel errors shouldn't have Reboot = True.")
                Write(DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}.") + vbNewLine +
                  DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), True, ColTypes.Continuable, ErrorType, Description)
                If ShowStackTraceOnKernelError And Exc IsNot Nothing Then Write(Exc.StackTrace, True, ColTypes.Continuable)
                Console.ReadKey()
            ElseIf ErrorType = KernelErrorLevel.C And Reboot = False Then
                'Check if error is Continuable and reboot is disabled
                Kernel.EventManager.RaiseContKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)
                Write(DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), True, ColTypes.Continuable, ErrorType, Description)
                If ShowStackTraceOnKernelError And Exc IsNot Nothing Then Write(Exc.StackTrace, True, ColTypes.Continuable)
                Console.ReadKey()
            ElseIf (Reboot = False And ErrorType <> KernelErrorLevel.D) Or (Reboot = False And ErrorType <> KernelErrorLevel.C) Then
                'If rebooting is disabled and the error type does not equal Double or Continuable
                Wdbg(DebugLevel.W, "Reboot is False, ErrorType is not double or continuable.")
                Write(DoTranslation("[{0}] panic: {1} -- Press any key to shutdown."), True, ColTypes.Uncontinuable, ErrorType, Description)
                If ShowStackTraceOnKernelError And Exc IsNot Nothing Then Write(Exc.StackTrace, True, ColTypes.Uncontinuable)
                Console.ReadKey()
                PowerManage(PowerMode.Shutdown)
            Else
                'Everything else.
                Wdbg(DebugLevel.F, "Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", RebootTime, ErrorType)
                Write(DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds..."), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                If ShowStackTraceOnKernelError And Exc IsNot Nothing Then Write(Exc.StackTrace, True, ColTypes.Uncontinuable)
                Thread.Sleep(RebootTime * 1000)
                PowerManage(PowerMode.Reboot)
            End If
        Catch ex As Exception
            WStkTrc(ex)
            KernelError(KernelErrorLevel.D, True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}"), ex, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Generates the stack trace dump file for kernel panics
    ''' </summary>
    ''' <param name="Description">Error description</param>
    ''' <param name="ErrorType">Error type</param>
    ''' <param name="Exc">Exception</param>
    Sub GeneratePanicDump(Description As String, ErrorType As KernelErrorLevel, Exc As Exception)
        Try
            'Open a file stream for dump
            Dim Dump As New StreamWriter($"{GetOtherPath(OtherPathType.Home)}/dmp_{RenderDate(FormatType.Short).Replace("/", "-")}_{RenderTime(FormatType.Short).Replace(":", "-")}.txt")
            Wdbg(DebugLevel.I, "Opened file stream in home directory, saved as dmp_{0}.txt", $"{RenderDate(FormatType.Short).Replace("/", "-")}_{RenderTime(FormatType.Short).Replace(":", "-")}")

            'Write info (Header)
            Dump.AutoFlush = True
            Dump.WriteLine(DoTranslation("----------------------------- Kernel panic dump -----------------------------") + vbNewLine + vbNewLine +
                           DoTranslation(">> Panic information <<") + vbNewLine +
                           DoTranslation("> Description: {0}") + vbNewLine +
                           DoTranslation("> Error type: {1}") + vbNewLine +
                           DoTranslation("> Date and Time: {2}") + vbNewLine, Description, ErrorType.ToString, Render)

            'Write Info (Exception)
            If Exc IsNot Nothing Then
                Dim Count As Integer = 1
                Dump.WriteLine(DoTranslation(">> Exception information <<") + vbNewLine +
                               DoTranslation("> Exception: {0}") + vbNewLine +
                               DoTranslation("> Description: {1}") + vbNewLine +
                               DoTranslation("> HRESULT: {2}") + vbNewLine +
                               DoTranslation("> Source: {3}") + vbNewLine + vbNewLine +
                               DoTranslation("> Stack trace <") + vbNewLine + vbNewLine +
                               Exc.StackTrace + vbNewLine + vbNewLine, Exc.GetType.FullName, Exc.Message, Exc.HResult, Exc.Source)
                Dump.WriteLine(DoTranslation(">> Inner exception {0} information <<"), Count)

                'Write info (Inner exceptions)
                Dim InnerExc As Exception = Exc.InnerException
                While InnerExc IsNot Nothing
                    Dump.WriteLine(DoTranslation("> Exception: {0}") + vbNewLine +
                                   DoTranslation("> Description: {1}") + vbNewLine +
                                   DoTranslation("> HRESULT: {2}") + vbNewLine +
                                   DoTranslation("> Source: {3}") + vbNewLine + vbNewLine +
                                   DoTranslation("> Stack trace <") + vbNewLine + vbNewLine +
                                   InnerExc.StackTrace + vbNewLine, InnerExc.GetType.FullName, InnerExc.Message, InnerExc.HResult, InnerExc.Source)
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

                'If there are frames to print the file information, write them down to the dump file.
                If ExcTrace.FrameCount <> 0 Then
                    For Each Frame As StackFrame In ExcTrace.GetFrames
                        If Not (Frame.GetFileName = "" And Frame.GetFileLineNumber = 0 And Frame.GetFileColumnNumber = 0) Then
                            Dump.WriteLine(DoTranslation("> Frame {0}: File: {1} | Line: {2} | Column: {3}"), FrameNo, Frame.GetFileName, Frame.GetFileLineNumber, Frame.GetFileColumnNumber)
                        End If
                        FrameNo += 1
                    Next
                Else
                    Dump.WriteLine(DoTranslation("> There are no information about frames."))
                End If
            Catch ex As Exception
                WStkTrc(ex)
                Dump.WriteLine(DoTranslation("> There is an error when trying to get frame information. {0}: {1}"), ex.GetType.FullName, ex.Message.Replace(vbNewLine, " | "))
            End Try

            'Close stream
            Wdbg(DebugLevel.I, "Closing file stream for dump...")
            Dump.Flush() : Dump.Close()
        Catch ex As Exception
            Write(DoTranslation("Dump information gatherer crashed when trying to get information about {0}: {1}"), True, ColTypes.Error, Exc.GetType.FullName, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    '----------------------------------------------- Power management -----------------------------------------------

    ''' <summary>
    ''' Manage computer's (actually, simulated computer) power
    ''' </summary>
    ''' <param name="PowerMode">Whether it would be "shutdown", "rebootsafe", or "reboot"</param>
    Public Sub PowerManage(PowerMode As String)
        PowerManage(PowerMode, "0.0.0.0", RPCPort)
    End Sub

    ''' <summary>
    ''' Manage computer's (actually, simulated computer) power
    ''' </summary>
    ''' <param name="PowerMode">Selects the power mode</param>
    Public Sub PowerManage(PowerMode As PowerMode, IP As String)
        PowerManage(PowerMode, IP, RPCPort)
    End Sub

    ''' <summary>
    ''' Manage computer's (actually, simulated computer) power
    ''' </summary>
    ''' <param name="PowerMode">Selects the power mode</param>
    Public Sub PowerManage(PowerMode As PowerMode, IP As String, Port As Integer)
        Wdbg(DebugLevel.I, "Power management has the argument of {0}", PowerMode)
        Select Case PowerMode
            Case PowerMode.Shutdown
                Kernel.EventManager.RaisePreShutdown()
                Write(DoTranslation("Shutting down..."), True, ColTypes.Neutral)
                ResetEverything()
                Kernel.EventManager.RaisePostShutdown()
                Environment.Exit(0)
            Case PowerMode.Reboot
                Kernel.EventManager.RaisePreReboot()
                Write(DoTranslation("Rebooting..."), True, ColTypes.Neutral)
                ResetEverything()
                Kernel.EventManager.RaisePostReboot()
                Console.Clear()
                RebootRequested = True
                LogoutRequested = True
                SafeMode = False
            Case PowerMode.RebootSafe
                Kernel.EventManager.RaisePreReboot()
                Write(DoTranslation("Rebooting..."), True, ColTypes.Neutral)
                ResetEverything()
                Kernel.EventManager.RaisePostReboot()
                Console.Clear()
                RebootRequested = True
                LogoutRequested = True
                SafeMode = True
            Case PowerMode.RemoteShutdown
                SendCommand("<Request:Shutdown>(" + IP + ")", IP, Port)
            Case PowerMode.RemoteRestart
                SendCommand("<Request:Reboot>(" + IP + ")", IP, Port)
        End Select
    End Sub

    '----------------------------------------------- Init and reset -----------------------------------------------
    ''' <summary>
    ''' Reset everything for the next restart
    ''' </summary>
    Sub ResetEverything()
        'Reset every variable below
        If ArgsInjected = False Then EnteredArguments.Clear()
        Test_ExitFlag = False
        Aliases.Clear()
        RemoteDebugAliases.Clear()
        FTPShellAliases.Clear()
        SFTPShellAliases.Clear()
        MailShellAliases.Clear()
        UserPermissions.Clear()
        Reminders.Clear()
        CalendarEvents.Clear()
        Wdbg(DebugLevel.I, "General variables reset")

        'Reset hardware info
        HardwareInfo = Nothing
        Wdbg(DebugLevel.I, "Hardware info reset.")

        'Release RAM used
        DisposeAll()
        Wdbg(DebugLevel.I, "Garbage collector finished")

        'Disconnect all hosts from remote debugger
        StopRDebugThread()
        Wdbg(DebugLevel.I, "Remote debugger stopped")

        'Stop all mods
        StopMods()
        Wdbg(DebugLevel.I, "Mods stopped")

        'Disable Debugger
        If DebugMode Then
            Wdbg(DebugLevel.I, "Shutting down debugger")
            DebugMode = False
            DebugStreamWriter.Close() : DebugStreamWriter.Dispose()
        End If

        'Stop RPC
        RPCThread.Abort()
        RPCListen?.Close()
        RPCListen = Nothing
        RPCThread = New Thread(AddressOf RecCommand) With {.IsBackground = True, .Name = "RPC Thread"}

        'Disconnect from mail
        IMAP_Client.Disconnect(True)
        SMTP_Client.Disconnect(True)
        POP3_Client.Disconnect(True)

        'Disable safe mode
        SafeMode = False
    End Sub

    ''' <summary>
    ''' Initializes everything
    ''' </summary>
    Sub InitEverything(Args() As String)
        'Initialize notifications
        If Not NotifThread.IsAlive Then NotifThread.Start()

        'Initialize events and reminders
        If Not ReminderThread.IsAlive Then ReminderThread.Start()
        If Not EventThread.IsAlive Then EventThread.Start()

        'Initialize aliases
        InitAliases()

        'Initialize date
        InitTimeDate()

        'Check for multiple instances of KS
        If InstanceChecked = False Then MultiInstance()

        'Create config file and then read it
        InitializeConfig()

        'Load user token
        LoadUserToken()

        'Show welcome message.
        WriteMessage()

        'Some information
        If ShowAppInfoOnBoot Then
            WriteSeparator(DoTranslation("App information"), True, ColTypes.Stage)
            Write("OS: " + DoTranslation("Running on {0}"), True, ColTypes.Neutral, Environment.OSVersion.ToString)
            Write("KS: " + DoTranslation("Built in {0}"), True, ColTypes.Neutral, Render(GetCompileDate()))
        End If

        'Show dev version notice
#If SPECIFIER = "DEV" Then 'WARNING: When the development nearly ends, change the compiler constant value to "REL" to suppress this message out of stable versions
        Write(DoTranslation("Looks like you were running the development version of the kernel. While you can see the aspects, it is frequently updated and might introduce bugs. It is recommended that you stay on the stable version."), True, ColTypes.DevelopmentWarning)
#ElseIf SPECIFIER = "RC" Then
        Write(DoTranslation("Looks like you were running the release candidate version. It is recommended that you stay on the stable version."), True, ColTypes.DevelopmentWarning)
#End If

        'Parse real command-line arguments
        If ParseCommandLineArguments Then ParseCMDArguments(Args)

        'Check arguments
        If ArgsOnBoot Then
            StageTimer.Stop()
            PromptArgs()
            StageTimer.Start()
        End If
        If ArgsInjected Then
            ArgsInjected = False
            ParseArguments()
        End If

        'Write headers for debug
        Wdbg(DebugLevel.I, "-------------------------------------------------------------------")
        Wdbg(DebugLevel.I, "Kernel initialized, version {0}.", KernelVersion)
        Wdbg(DebugLevel.I, "OS: {0}", Environment.OSVersion.ToString)

        'Populate ban list for debug devices
        PopulateBlockedDevices()

        'Start screensaver timeout
        If Not Timeout.IsBusy Then Timeout.RunWorkerAsync()

        'Load all events and reminders
        LoadEvents()
        LoadReminders()
    End Sub

    '----------------------------------------------- Misc -----------------------------------------------

    ''' <summary>
    ''' Check to see if multiple Kernel Simulator processes are running.
    ''' </summary>
    Sub MultiInstance()
        Static ksInst As Mutex
        Dim ksOwner As Boolean
        ksInst = New Mutex(True, "Kernel Simulator", ksOwner)
        If Not ksOwner Then
            KernelError(KernelErrorLevel.F, False, 0, DoTranslation("Another instance of Kernel Simulator is running. Shutting down in case of interference."), Nothing)
        End If
        InstanceChecked = True
    End Sub

    ''' <summary>
    ''' Fetches the GitHub repo to see if there are any updates
    ''' </summary>
    ''' <returns>A list which contains both the version and the URL</returns>
    Public Function FetchKernelUpdates() As List(Of String)
        Try
            'Variables
            Dim UpdateSpecifier As New List(Of String)
            Dim UpdateDown As New WebClient

            'Because api.github.com requires the UserAgent header to be put, else, 403 error occurs. Fortunately for us, "EoflaOE" is enough.
            UpdateDown.Headers.Add(HttpRequestHeader.UserAgent, "EoflaOE")

            'Populate the following variables with information
            Dim UpdateStr As String = UpdateDown.DownloadString("https://api.github.com/repos/EoflaOE/Kernel-Simulator/releases")
            Dim UpdateToken As JToken = JToken.Parse(UpdateStr)
            Dim UpdateVer As New Version(UpdateToken.First.SelectToken("tag_name").ToString.ReplaceAll({"v", "-alpha"}, ""))
            Dim UpdateURL As String = UpdateToken.First.SelectToken("html_url")
            Dim CurrentVer As New Version(KernelVersion)

            'Check to see if the updated version is newer than the current version
            If UpdateVer > CurrentVer Then
                UpdateSpecifier.Add(UpdateVer.ToString)
                UpdateSpecifier.Add(UpdateURL)
            End If
            Return UpdateSpecifier
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Failed to check for updates: {0}", ex.Message)
            WStkTrc(ex)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Prompt for checking for kernel updates
    ''' </summary>
    Sub CheckKernelUpdates()
        Write(DoTranslation("Checking for system updates..."), True, ColTypes.Neutral)
        Dim AvailableUpdates As List(Of String) = FetchKernelUpdates()
        If AvailableUpdates IsNot Nothing AndAlso AvailableUpdates.Count > 0 Then
            Write(DoTranslation("Found new version: "), False, ColTypes.ListEntry)
            Write(AvailableUpdates(0), True, ColTypes.ListValue)
            Write(DoTranslation("You can download it at: "), False, ColTypes.ListEntry)
            Write(AvailableUpdates(1), True, ColTypes.ListValue)
        ElseIf AvailableUpdates Is Nothing Then
            Write(DoTranslation("Failed to check for updates."), True, ColTypes.Error)
        End If
    End Sub

    ''' <summary>
    ''' Gets the Kernel Simulator compilation date.
    ''' </summary>
    Function GetCompileDate() As Date 'Always successful, no need to put Try Catch
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

    ''' <summary>
    ''' Disposes all unused memory.
    ''' </summary>
    Public Sub DisposeAll()
        Try
            Dim proc As Process = GetCurrentProcess()
            Wdbg(DebugLevel.I, "Before garbage collection: {0} bytes", proc.PrivateMemorySize64)
            Wdbg(DebugLevel.I, "Garbage collector starting... Max generators: {0}", GC.MaxGeneration.ToString)
            GC.Collect()
            GC.WaitForPendingFinalizers()
            If IsOnWindows() Then
                SetProcessWorkingSetSize(GetCurrentProcess().Handle, -1, -1)
            End If
            Wdbg(DebugLevel.I, "After garbage collection: {0} bytes", proc.PrivateMemorySize64)
            proc.Dispose()
            Kernel.EventManager.RaiseGarbageCollected()
        Catch ex As Exception
            Wdbg("Error freeing RAM: {0}", ex.Message)
            WStkTrc(ex)
            Write(DoTranslation("Error trying to free RAM: {0} - Continuing..."), True, ColTypes.Error, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Removes all configuration files
    ''' </summary>
    Sub FactoryReset()
        For Each PathName As String In KernelPaths.Keys
            If FileExists(KernelPaths(PathName)) Then
                File.Delete(KernelPaths(PathName))
            Else
                Directory.Delete(KernelPaths(PathName), True)
            End If
        Next
        Environment.Exit(0)
    End Sub

    ''' <summary>
    ''' Notifies the user of any startup faults occuring
    ''' </summary>
    Friend Sub NotifyStartupFaults()
        'Configuration error (loading)
        If NotifyConfigError Then
            NotifyConfigError = False
            NotifySend(New Notification(DoTranslation("Error loading settings"),
                                        DoTranslation("There is an error while loading settings. You may need to check the settings file."),
                                        NotifPriority.Medium, NotifType.Normal))
        End If

        'Debug data download error
        If NotifyDebugDownloadError Then
            NotifyDebugDownloadError = False
            NotifySend(New Notification(DoTranslation("Error downloading debug data"),
                                        DoTranslation("There is an error while downloading debug data. Check your internet connection."),
                                        NotifPriority.Medium, NotifType.Normal))
        End If

        'Debug data download when network unavailable
        If NotifyDebugDownloadNetworkUnavailable Then
            NotifyDebugDownloadNetworkUnavailable = False
            NotifySend(New Notification(DoTranslation("No network while downloading debug data"),
                                        DoTranslation("Check your internet connection and try again."),
                                        NotifPriority.Medium, NotifType.Normal))
        End If

        'Previous boot failure
        If NotifyKernelError Then
            NotifyKernelError = False
            NotifySend(New Notification(DoTranslation("Previous boot failed"),
                                        LastKernelErrorException.Message,
                                        NotifPriority.High, NotifType.Normal))
        End If
    End Sub

    ''' <summary>
    ''' Polls $TERM_PROGRAM to get terminal emulator
    ''' </summary>
    Public Function GetTerminalEmulator() As String
        Return Environ("TERM_PROGRAM")
    End Function

    ''' <summary>
    ''' Gets the used compiler variables for building Kernel Simulator
    ''' </summary>
    ''' <returns>An array containing used compiler variables</returns>
    Public Function GetCompilerVars() As String()
        Dim CompilerVars As New List(Of String)

        'Determine the compiler vars used to build KS using conditional checks
#If NTFSCorruptionFix Then
        CompilerVars.Add("NTFSCorruptionFix")
#End If

#If NOWRITELOCK Then
        CompilerVars.Add("NOWRITELOCK")
#End If

#If SPECIFIER = "DEV" Then
        CompilerVars.Add("SPECIFIER = ""DEV""")
#ElseIf SPECIFIER = "RC" Then
        CompilerVars.Add("SPECIFIER = ""RC""")
#ElseIf SPECIFIER = "REL" Then
        CompilerVars.Add("SPECIFIER = ""REL""")
#End If

#If ENABLEIMMEDIATEWINDOWDEBUG Then
        CompilerVars.Add("ENABLEIMMEDIATEWINDOWDEBUG")
#End If

#If STOCKTERMINALMACOS Then
        CompilerVars.Add("STOCKTERMINALMACOS")
#End If

        'Return the compiler vars
        Return CompilerVars.ToArray
    End Function

End Module
