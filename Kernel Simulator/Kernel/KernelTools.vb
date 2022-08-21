
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports System.Threading
Imports KS.Arguments
Imports KS.Arguments.ArgumentBase
Imports KS.Files.Querying
Imports KS.Hardware
Imports KS.Misc.Calendar.Events
Imports KS.Misc.Calendar.Reminders
Imports KS.Kernel.Configuration
Imports KS.Kernel.Power
Imports KS.Misc.Notifications
Imports KS.Misc.Reflection
Imports KS.Misc.Screensaver
Imports KS.Misc.Splash
Imports KS.Misc.Writers.MiscWriters
Imports KS.Modifications
Imports KS.Network.Mail
Imports KS.Network.RemoteDebug
Imports KS.Network.RPC
Imports KS.Shell.ShellBase.Aliases
Imports KS.Scripting
Imports KS.TimeDate

Namespace Kernel
    Public Module KernelTools

        'Variables
        Public BannerFigletFont As String = "Banner"
        Friend RPCPowerListener As New KernelThread("RPC Power Listener Thread", True, AddressOf PowerManage)
        Friend LastKernelErrorException As Exception
        Friend InstanceChecked As Boolean

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
                QuietKernel = False

                'Check error types and its capabilities
                Wdbg(DebugLevel.I, "Error type: {0}", ErrorType)
                If [Enum].IsDefined(GetType(KernelErrorLevel), ErrorType) Then
                    If ErrorType = KernelErrorLevel.U Or ErrorType = KernelErrorLevel.D Then
                        If RebootTime > 5 Then
                            'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                            'generate a second kernel error stating that there is something wrong with the reboot time.
                            Wdbg(DebugLevel.W, "Errors that have type {0} shouldn't exceed 5 seconds. RebootTime was {1} seconds", ErrorType, RebootTime)
                            KernelError(KernelErrorLevel.D, True, 5, DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug."), Nothing, CStr(ErrorType))
                            Exit Sub
                        ElseIf Not Reboot Then
                            'If the error type is unrecoverable, or double, and the rebooting is false where it should
                            'not be false, then it can deal with this issue by enabling reboot.
                            Wdbg(DebugLevel.W, "Errors that have type {0} enforced Reboot = True.", ErrorType)
                            Write(DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}."), True, ColTypes.Uncontinuable, ErrorType)
                            Reboot = True
                        End If
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
                    Exit Sub
                End If

                'Format the "Description" string variable
                Description = FormatString(Description, Variables)

                'Fire an event
                KernelEventManager.RaiseKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)

                'Make a dump file
                GeneratePanicDump(Description, ErrorType, Exc)

                'Check error type
                Select Case ErrorType
                    Case KernelErrorLevel.D
                        'Double panic printed and reboot initiated
                        Wdbg(DebugLevel.F, "Double panic caused by bug in kernel crash.")
                        Write(DoTranslation("[{0}] dpanic: {1} -- Rebooting in {2} seconds..."), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                        Thread.Sleep(RebootTime * 1000)
                        Wdbg(DebugLevel.F, "Rebooting")
                        PowerManage(PowerMode.Reboot)
                    Case KernelErrorLevel.C
                        If Reboot Then
                            'Continuable kernel errors shouldn't cause the kernel to reboot.
                            Wdbg(DebugLevel.W, "Continuable kernel errors shouldn't have Reboot = True.")
                            Write(DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}."), True, ColTypes.Warning, ErrorType)
                        End If
                        'Print normally
                        KernelEventManager.RaiseContKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)
                        Write(DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), True, ColTypes.Continuable, ErrorType, Description)
                        If ShowStackTraceOnKernelError And Exc IsNot Nothing Then Write(Exc.StackTrace, True, ColTypes.Continuable)
                        Console.ReadKey()
                    Case Else
                        If Reboot Then
                            'Offer the user to wait for the set time interval before the kernel reboots.
                            Wdbg(DebugLevel.F, "Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", RebootTime, ErrorType)
                            Write(DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds..."), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                            If ShowStackTraceOnKernelError And Exc IsNot Nothing Then Write(Exc.StackTrace, True, ColTypes.Uncontinuable)
                            Thread.Sleep(RebootTime * 1000)
                            PowerManage(PowerMode.Reboot)
                        Else
                            'If rebooting is disabled, offer the user to shutdown the kernel
                            Wdbg(DebugLevel.W, "Reboot is False, ErrorType is not double or continuable.")
                            Write(DoTranslation("[{0}] panic: {1} -- Press any key to shutdown."), True, ColTypes.Uncontinuable, ErrorType, Description)
                            If ShowStackTraceOnKernelError And Exc IsNot Nothing Then Write(Exc.StackTrace, True, ColTypes.Uncontinuable)
                            Console.ReadKey()
                            PowerManage(PowerMode.Shutdown)
                        End If
                End Select
            Catch ex As Exception
                'Check to see if it's a double panic
                If ErrorType = KernelErrorLevel.D Then
                    'Trigger triple fault
                    Wdbg(DebugLevel.F, "TRIPLE FAULT: Kernel bug: {0}", ex.Message)
                    WStkTrc(ex)
                    Environment.FailFast("TRIPLE FAULT in trying to handle DOUBLE PANIC. KS can't continue.", ex)
                Else
                    'Alright, we have a double panic.
                    Wdbg(DebugLevel.F, "DOUBLE PANIC: Kernel bug: {0}", ex.Message)
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.D, True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}"), ex, ex.Message)
                End If
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
                Dim Dump As New StreamWriter($"{HomePath}/dmp_{RenderDate(FormatType.Short).Replace("/", "-")}_{RenderTime(FormatType.Long).Replace(":", "-")}.txt")
                Wdbg(DebugLevel.I, "Opened file stream in home directory, saved as dmp_{0}.txt", $"{RenderDate(FormatType.Short).Replace("/", "-")}_{RenderTime(FormatType.Long).Replace(":", "-")}")

                'Write info (Header)
                Dump.AutoFlush = True
                Dump.WriteLine(DoTranslation("----------------------------- Kernel panic dump -----------------------------") + NewLine + NewLine +
                           DoTranslation(">> Panic information <<") + NewLine +
                           DoTranslation("> Description: {0}") + NewLine +
                           DoTranslation("> Error type: {1}") + NewLine +
                           DoTranslation("> Date and Time: {2}") + NewLine +
                           DoTranslation("> Framework Type: {3}") + NewLine, Description, ErrorType.ToString, Render, KernelSimulatorMoniker)

                'Write Info (Exception)
                If Exc IsNot Nothing Then
                    Dim Count As Integer = 1
                    Dump.WriteLine(DoTranslation(">> Exception information <<") + NewLine +
                               DoTranslation("> Exception: {0}") + NewLine +
                               DoTranslation("> Description: {1}") + NewLine +
                               DoTranslation("> HRESULT: {2}") + NewLine +
                               DoTranslation("> Source: {3}") + NewLine + NewLine +
                               DoTranslation("> Stack trace <") + NewLine + NewLine +
                               Exc.StackTrace + NewLine + NewLine, Exc.GetType.FullName, Exc.Message, Exc.HResult, Exc.Source)
                    Dump.WriteLine(DoTranslation(">> Inner exception {0} information <<"), Count)

                    'Write info (Inner exceptions)
                    Dim InnerExc As Exception = Exc.InnerException
                    While InnerExc IsNot Nothing
                        Dump.WriteLine(DoTranslation("> Exception: {0}") + NewLine +
                                   DoTranslation("> Description: {1}") + NewLine +
                                   DoTranslation("> HRESULT: {2}") + NewLine +
                                   DoTranslation("> Source: {3}") + NewLine + NewLine +
                                   DoTranslation("> Stack trace <") + NewLine + NewLine +
                                   InnerExc.StackTrace + NewLine, InnerExc.GetType.FullName, InnerExc.Message, InnerExc.HResult, InnerExc.Source)
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
                    Dump.WriteLine(DoTranslation(">> No exception; might be a kernel error. <<") + NewLine)
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
                    Dump.WriteLine(DoTranslation("> There is an error when trying to get frame information. {0}: {1}"), ex.GetType.FullName, ex.Message.Replace(NewLine, " | "))
                End Try

                'Close stream
                Wdbg(DebugLevel.I, "Closing file stream for dump...")
                Dump.Flush() : Dump.Close()
            Catch ex As Exception
                Write(DoTranslation("Dump information gatherer crashed when trying to get information about {0}: {1}"), True, ColTypes.Error, Exc.GetType.FullName, ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

        '----------------------------------------------- Init and reset -----------------------------------------------
        ''' <summary>
        ''' Reset everything for the next restart
        ''' </summary>
        Sub ResetEverything()
            'Reset every variable below
            If ArgsInjected = False Then EnteredArguments.Clear()
            UserPermissions.Clear()
            Reminders.Clear()
            CalendarEvents.Clear()
            ArgsOnBoot = False
            SafeMode = False
            QuietKernel = False
            Maintenance = False
            _Progress = 0
            _ProgressText = ""
            _KernelBooted = False
            Wdbg(DebugLevel.I, "General variables reset")

            'Reset hardware info
            HardwareInfo = Nothing
            Wdbg(DebugLevel.I, "Hardware info reset.")

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
            StopRPC()

            'Disconnect from mail
            IMAP_Client.Disconnect(True)
            SMTP_Client.Disconnect(True)

            'Unload all splashes
            UnloadSplashes()

            'Disable safe mode
            SafeMode = False

            'Stop the time/date change thread
            TimeDateChange.Stop()
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

            'Install cancellation handler
            If Not CancellationHandlerInstalled Then AddHandler Console.CancelKeyPress, AddressOf CancelCommand

            'Initialize aliases
            InitAliases()

            'Initialize date
            InitTimeDate()

            'Initialize custom languages
            InstallCustomLanguages()

            'Check for multiple instances of KS
            If InstanceChecked = False Then MultiInstance()

            'Initialize splashes
            LoadSplashes()

            'Create config file and then read it
            InitializeConfig()

            'Load user token
            LoadUserToken()

            'Show welcome message.
            WriteMessage()

            'Some information
            If ShowAppInfoOnBoot And Not EnableSplash Then
                WriteSeparator(DoTranslation("App information"), True, ColTypes.Stage)
                Write("OS: " + DoTranslation("Running on {0}"), True, ColTypes.Neutral, Environment.OSVersion.ToString)
                Write("KS: " + DoTranslation("Running from GRILO?") + " {0}", True, ColTypes.Neutral, IsRunningFromGrilo())
            End If

            'Show dev version notice
            If Not EnableSplash Then
#If SPECIFIER = "DEV" Then 'WARNING: When the development nearly ends, change the compiler constant value to "REL" to suppress this message out of stable versions
                Write(DoTranslation("Looks like you were running the development version of the kernel. While you can see the aspects, it is frequently updated and might introduce bugs. It is recommended that you stay on the stable version."), True, ColTypes.DevelopmentWarning)
#elif SPECIFIERRC
                Write(DoTranslation("Looks like you were running the release candidate version. It is recommended that you stay on the stable version."), True, ColTypes.DevelopmentWarning)
#ElseIf SPECIFIER <> "REL" Then
                Write(DoTranslation("Looks like you were running an unsupported version. It's highly advisable not to use this version."), True, ColTypes.DevelopmentWarning)
#End If
            End If

            'Parse real command-line arguments
            If ParseCommandLineArguments Then ParseArguments(Args.ToList, ArgumentType.CommandLineArgs)

            'Check arguments
            If ArgsOnBoot Then
                StageTimer.Stop()
                PromptArgs()
                StageTimer.Start()
            End If
            If ArgsInjected Then
                ArgsInjected = False
                ParseArguments(EnteredArguments, ArgumentType.KernelArgs)
            End If

            'Load splash
            OpenSplash()

            'Write headers for debug
            Wdbg(DebugLevel.I, "-------------------------------------------------------------------")
            Wdbg(DebugLevel.I, "Kernel initialized, version {0}.", KernelVersion)
            Wdbg(DebugLevel.I, "OS: {0}", Environment.OSVersion.ToString)
            Wdbg(DebugLevel.I, "Framework: {0}", KernelSimulatorMoniker)

            'Populate ban list for debug devices
            PopulateBlockedDevices()

            'Start screensaver timeout
            If Not Screensaver.Timeout.IsAlive Then Screensaver.Timeout.Start()

            'Load all events and reminders
            LoadEvents()
            LoadReminders()

            'Load system env vars and convert them
            ConvertSystemEnvironmentVariables()
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
        ''' Removes all configuration files
        ''' </summary>
        Sub FactoryReset()
            'Delete every single thing found in KernelPaths
            For Each PathName As String In [Enum].GetNames(GetType(KernelPathType))
                Dim TargetPath As String = GetKernelPath(PathName)
                If FileExists(TargetPath) Then
                    File.Delete(TargetPath)
                Else
                    Directory.Delete(TargetPath, True)
                End If
            Next

            'Clear the console and reset the colors
            Console.ResetColor()
            Console.Clear()
            Environment.Exit(0)
        End Sub

        ''' <summary>
        ''' Reports the new kernel stage
        ''' </summary>
        ''' <param name="StageNumber">The stage number</param>
        ''' <param name="StageText">The stage text</param>
        Sub ReportNewStage(StageNumber As Integer, StageText As String)
            'Show the stage finish times
            If StageNumber <= 1 Then
                If ShowStageFinishTimes Then
                    ReportProgress(DoTranslation("Internal initialization finished in") + $" {StageTimer.Elapsed}", 0, ColTypes.StageTime)
                    StageTimer.Restart()
                End If
            ElseIf StageNumber >= 5 Then
                If ShowStageFinishTimes Then
                    ReportProgress(DoTranslation("Stage finished in") + $" {StageTimer.Elapsed}", 10, ColTypes.StageTime)
                    StageTimer.Reset()
                    Console.WriteLine()
                End If
            Else
                If ShowStageFinishTimes Then
                    ReportProgress(DoTranslation("Stage finished in") + $" {StageTimer.Elapsed}", 10, ColTypes.StageTime)
                    StageTimer.Restart()
                End If
            End If

            'Actually report the stage
            If StageNumber >= 1 And StageNumber <= 4 Then
                If Not EnableSplash And Not QuietKernel Then
                    Console.WriteLine()
                    WriteSeparator(StageText, False, ColTypes.Stage)
                End If
                Wdbg(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}")
            End If
        End Sub

        ''' <summary>
        ''' Gets the used compiler variables for building Kernel Simulator
        ''' </summary>
        ''' <returns>An array containing used compiler variables</returns>
        Public Function GetCompilerVars() As String()
            Dim CompilerVars As New List(Of String)

            'Determine the compiler vars used to build KS using conditional checks
#If SPECIFIER = "DEV" Then
            CompilerVars.Add("SPECIFIER = ""DEV""")
#elif SPECIFIERRC
            CompilerVars.Add("SPECIFIER = ""RC""")
#ElseIf SPECIFIER = "REL" Then
            CompilerVars.Add("SPECIFIER = ""REL""")
#End If

#If ENABLEIMMEDIATEWINDOWDEBUG Then
            CompilerVars.Add("ENABLEIMMEDIATEWINDOWDEBUG")
#End If

#If POP3Feature Then
            CompilerVars.Add("POP3Feature")
#End If

            'Return the compiler vars
            Return CompilerVars.ToArray
        End Function

    End Module
End Namespace
