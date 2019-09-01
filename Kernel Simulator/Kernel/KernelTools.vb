
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

    ' A dictionary for storing paths and files (used for mods, screensavers, etc.)
    Public paths As New Dictionary(Of String, String)

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
    ''' <remarks></remarks>
    Public Sub KernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal ParamArray Variables() As Object)
        Try
            'Check error types and its capabilities
            If ErrorType = "S" Or ErrorType = "F" Or ErrorType = "U" Or ErrorType = "D" Or ErrorType = "C" Then
                If ErrorType = "U" And RebootTime > 5 Or ErrorType = "D" And RebootTime > 5 Then
                    'If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                    'generate a second kernel error stating that there is something wrong with the reboot time.
                    Wdbg("Errors that have {0} type shouldn't exceed 5 seconds. RebootTime was {1} seconds", ErrorType, RebootTime)
                    KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug.", currentLang), Nothing, CStr(ErrorType))
                    StopPanicAndGoToDoublePanic = True
                ElseIf ErrorType = "U" And Reboot = False Or ErrorType = "D" And Reboot = False Then
                    'If the error type is unrecoverable, or double, and the rebooting is false where it should
                    'not be false, then it can deal with this issue by enabling reboot.
                    Wdbg("Errors that have {0} type enforced Reboot = True.", ErrorType)
                    W(DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}.", currentLang), True, ColTypes.Uncontinuable, ErrorType)
                    Reboot = True
                End If
                If RebootTime > 3600 Then
                    'If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                    Wdbg("RebootTime shouldn't exceed 1 hour. Was {0} seconds", RebootTime)
                    W(DoTranslation("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute.", currentLang), True, ColTypes.Uncontinuable, ErrorType, CStr(RebootTime))
                    RebootTime = 60
                End If
            Else
                'If the error type is other than D/F/C/U/S, then it will generate a second error.
                Wdbg("Error type {0} is not valid.", ErrorType)
                KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Error Type {0} invalid.", currentLang), Nothing, CStr(ErrorType))
                StopPanicAndGoToDoublePanic = True
            End If

            'Parse variables ({0}, {1}, ...) in the "Description" string variable
            For v As Integer = 0 To Variables.Length - 1
                Description = Description.Replace($"{{{CStr(v)}}}", Variables(v))
            Next

            'Fire an event
            EventManager.RaiseKernelError()

            'Make a dump file
            GeneratePanicDump(Description, ErrorType, Exc)

            'Check error capabilities
            If Description.Contains("DOUBLE PANIC: ") And ErrorType = "D" Then
                'If the description has a double panic tag and the error type is Double
                Wdbg("Double panic caused by bug in kernel crash.")
                W(DoTranslation("[{0}] dpanic: {1} -- Rebooting in {2} seconds...", currentLang), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                Thread.Sleep(RebootTime * 1000)
                Wdbg("Rebooting")
                PowerManage("reboot")
            ElseIf StopPanicAndGoToDoublePanic = True Then
                'Switch to Double Panic
                Exit Sub
            ElseIf ErrorType = "C" And Reboot = True Then
                'Check if error is Continuable and reboot is enabled
                Wdbg("Continuable kernel errors shouldn't have Reboot = True.")
                W(DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}.", currentLang) + vbNewLine +
                  DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel.", currentLang), True, ColTypes.Continuable, ErrorType, Description)
                Console.ReadKey()
            ElseIf ErrorType = "C" And Reboot = False Then
                'Check if error is Continuable and reboot is disabled
                EventManager.RaiseContKernelError()
                W(DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel.", currentLang), True, ColTypes.Continuable, ErrorType, Description)
                Console.ReadKey()
            ElseIf (Reboot = False And ErrorType <> "D") Or (Reboot = False And ErrorType <> "C") Then
                'If rebooting is disabled and the error type does not equal Double or Continuable
                Wdbg("Reboot is False, ErrorType is not double or continuable.")
                W(DoTranslation("[{0}] panic: {1} -- Press any key to shutdown.", currentLang), True, ColTypes.Uncontinuable, ErrorType, Description)
                Console.ReadKey()
                PowerManage("shutdown")
            Else
                'Everything else.
                Wdbg("Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", RebootTime, ErrorType)
                W(DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds...", currentLang), True, ColTypes.Uncontinuable, ErrorType, Description, CStr(RebootTime))
                Thread.Sleep(RebootTime * 1000)
                PowerManage("reboot")
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(ex.StackTrace, True, ColTypes.Uncontinuable) : WStkTrc(ex)
                KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}", currentLang), ex, Err.Description)
            Else
                KernelError("D", True, 5, DoTranslation("DOUBLE PANIC: Kernel bug: {0}", currentLang), ex, Err.Description)
            End If
        End Try
    End Sub

    Sub GeneratePanicDump(ByVal Description As String, ByVal ErrorType As Char, ByVal Exc As Exception)
        'Open a file stream for dump
        Dim Dump As New StreamWriter($"{paths("Home")}/dmp_{Date.Now.ToShortDateString.Replace("/", "-")}_{Date.Now.ToLongTimeString.Replace(":", "-")}.txt")
        Wdbg("Opened file stream in home directory, saved as dmp_{0}_{1}.txt", $"{Date.Now.ToShortDateString.Replace("/", "-")}_{Date.Now.ToLongTimeString.Replace(":", "-")}")

        'Write info (Header)
        Dump.AutoFlush = True
        Dump.WriteLine(DoTranslation("----------------------------- Kernel panic dump -----------------------------", currentLang) + vbNewLine + vbNewLine +
                       DoTranslation(">> Panic information <<", currentLang) + vbNewLine +
                       DoTranslation("> Description: {0}", currentLang) + vbNewLine +
                       DoTranslation("> Error type: {1}", currentLang) + vbNewLine +
                       DoTranslation("> Date and Time: {2}", currentLang) + vbNewLine, Description, ErrorType, FormatDateTime(Date.Now, DateFormat.GeneralDate))

        'Write Info (Exception)
        If Not IsNothing(Exc) Then
            Dim Count As Integer = 1
            Dump.WriteLine(DoTranslation(">> Exception information <<", currentLang) + vbNewLine +
                           DoTranslation("> Exception: {0}", currentLang) + vbNewLine +
                           DoTranslation("> Description: {1}", currentLang) + vbNewLine +
                           DoTranslation("> HRESULT: {2}", currentLang) + vbNewLine +
                           DoTranslation("> Source: {3}", currentLang) + vbNewLine + vbNewLine +
                           DoTranslation("> Stack trace <", currentLang) + vbNewLine + vbNewLine +
                           Exc.StackTrace + vbNewLine + vbNewLine +
                           DoTranslation(">> Inner exception {0} information <<", currentLang), Exc.ToString.Substring(0, Exc.ToString.IndexOf(":")), Exc.Message, Exc.HResult, Exc.Source)

            'Write info (Inner exceptions)
            Dim InnerExc As Exception = Exc.InnerException
            While Not InnerExc Is Nothing
                Count += 1
                Dump.WriteLine(DoTranslation("> Exception: {0}", currentLang) + vbNewLine +
                               DoTranslation("> Description: {1}", currentLang) + vbNewLine +
                               DoTranslation("> HRESULT: {2}", currentLang) + vbNewLine +
                               DoTranslation("> Source: {3}", currentLang) + vbNewLine + vbNewLine +
                               DoTranslation("> Stack trace <", currentLang) + vbNewLine + vbNewLine +
                               InnerExc.StackTrace + vbNewLine, InnerExc.ToString.Substring(0, InnerExc.ToString.IndexOf(":")), InnerExc.Message, InnerExc.HResult, InnerExc.Source)
                InnerExc = InnerExc.InnerException
                If Not InnerExc Is Nothing Then
                    Dump.WriteLine(DoTranslation(">> Inner exception {0} information <<", currentLang), Count)
                Else
                    Dump.WriteLine(DoTranslation(">> Exception {0} is the root cause <<", currentLang) + vbNewLine, Count)
                End If
            End While
        Else
            Dump.WriteLine(DoTranslation(">> No exception; might be a kernel error. <<", currentLang) + vbNewLine)
        End If

        'Write info (Frames)
        Dump.WriteLine(DoTranslation(">> Frames, files, lines, and columns <<", currentLang))
        Try
            Dim ExcTrace As New StackTrace(Exc, True)
            Dim FrameNo As Integer = 1
            For Each Frame As StackFrame In ExcTrace.GetFrames
                Dump.WriteLine(DoTranslation("> Frame {0}: File: {1} | Line: {2} | Column: {3}", currentLang), FrameNo, Frame.GetFileName, Frame.GetFileLineNumber, Frame.GetFileColumnNumber)
                FrameNo += 1
            Next
        Catch ex As Exception
            Dump.WriteLine(DoTranslation("> There is an error when trying to get frame information. {0}: {1}", currentLang), ex.ToString.Substring(0, ex.ToString.IndexOf(":")), ex.Message.Replace(vbNewLine, " | "))
        End Try

        'Close stream
        Wdbg("Closing file stream for dump...")
        Dump.Flush() : Dump.Close()
    End Sub

    ' ----------------------------------------------- Power management -----------------------------------------------

    ''' <summary>
    ''' Manage computer's (actually, simulated computer) power
    ''' </summary>
    ''' <param name="PowerMode">Whether it would be "shutdown" or "reboot"</param>
    ''' <remarks></remarks>
    Public Sub PowerManage(ByVal PowerMode As String)
        Wdbg("Power management has the argument of {0}", PowerMode)
        If PowerMode = "shutdown" Then
            EventManager.RaisePreShutdown()
            W(DoTranslation("Shutting down...", currentLang), True, ColTypes.Neutral)
            ResetEverything()
            EventManager.RaisePostShutdown()
            Environment.Exit(0)
        ElseIf PowerMode = "reboot" Then
            EventManager.RaisePreReboot()
            W(DoTranslation("Rebooting...", currentLang), True, ColTypes.Neutral)
            ResetEverything()
            EventManager.RaisePostReboot()
            Console.Clear()
            LogoutRequested = True
        End If
    End Sub

    ' ----------------------------------------------- Init and reset -----------------------------------------------

    Sub ResetEverything()
        'Reset every variable that is resettable
        If argsInjected = False Then
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
        CurrDirStructure.Clear()
        modcmnds.Clear()
        moddefs.Clear()
        scripts.Clear()
        paths.Clear()
        Wdbg("General variables reset")

        'Reset hardware info
        HDDList.Clear()
        RAMList.Clear()
        CPUList.Clear()

        'Release RAM used
        DisposeAll()
        Wdbg("Garbage collector finished")

        'Disconnect all hosts from remote debugger
        RebootRequested = True

        'Close settings
        configReader = New IniFile()

        'Stop all mods
        ParseMods(False)

        'Disable Debugger
        If DebugMode = True Then
            DebugMode = False
            dbgWriter.Close() : dbgWriter.Dispose()
        End If
    End Sub

    Sub InitEverything()
        'Initialize help
        InitHelp()
        InitFTPHelp()

        'We need to create a file so InitAliases() won't give out an error
        If Not File.Exists($"{paths("Home")}/aliases.csv") Then
            Dim fstream As FileStream = File.Create($"{paths("Home")}/aliases.csv")
            fstream.Close()
        End If

        'Initialize aliases
        InitAliases()

        'Check for multiple instances of KS
        If instanceChecked = False Then MultiInstance()

        'Create config file and then read it
        InitializeConfig()

        If RebootRequested Then
            Exit Sub
        End If

        'Show welcome message. Don't remove license
        W(DoTranslation("---===+++> Welcome to the kernel | Version {0} <+++===---", currentLang), True, ColTypes.Neutral, KernelVersion)
        W(vbNewLine + "    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE" + vbNewLine +
                      "    This program comes with ABSOLUTELY NO WARRANTY, not even " + vbNewLine +
                      "    MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine +
                      "    This is free software, and you are welcome to redistribute it" + vbNewLine +
                      "    under certain conditions; See COPYING file in source code." + vbNewLine, True, ColTypes.License)
        W("OS: " + DoTranslation("Running on {0}", currentLang), True, ColTypes.Neutral, EnvironmentOSType)

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

        'Write header for debug
        StartRDebugThread()
        Wdbg("-------------------------------------------------------------------")
        Wdbg("Kernel initialized, version {0}.", KernelVersion)
        Wdbg("OS: {0}", EnvironmentOSType)

        'Initialize date and files
        If Not TimeDateIsSet Then
            InitTimeDate()
            TimeDateIsSet = True
        End If
        InitFS()

        'Parse current theme string
        ParseCurrentTheme()
    End Sub

    Sub InitPaths()
        'TODO: Make dirs to more appropriate dirs
        If EnvironmentOSType.Contains("Unix") Then
            If Not paths.ContainsKey("Mods") Then paths.Add("Mods", Environ("HOME") + "/KSMods/")
            If Not paths.ContainsKey("Configuration") Then paths.Add("Configuration", Environ("HOME") + "/kernelConfig.ini")
            If Not paths.ContainsKey("Debugging") Then paths.Add("Debugging", Environ("HOME") + "/kernelDbg.log")
            If Not paths.ContainsKey("Aliases") Then paths.Add("Aliases", Environ("HOME") + "/aliases.csv")
            If Not paths.ContainsKey("Home") Then paths.Add("Home", Environ("HOME"))
        Else
            If Not paths.ContainsKey("Mods") Then paths.Add("Mods", Environ("USERPROFILE") + "\KSMods\".Replace("\", "/"))
            If Not paths.ContainsKey("Configuration") Then paths.Add("Configuration", Environ("USERPROFILE") + "\kernelConfig.ini".Replace("\", "/"))
            If Not paths.ContainsKey("Debugging") Then paths.Add("Debugging", Environ("USERPROFILE") + "\kernelDbg.log".Replace("\", "/"))
            If Not paths.ContainsKey("Aliases") Then paths.Add("Aliases", Environ("USERPROFILE") + "\aliases.csv".Replace("\", "/"))
            If Not paths.ContainsKey("Home") Then paths.Add("Home", Environ("USERPROFILE").Replace("\", "/"))
        End If
    End Sub

    ' ----------------------------------------------- Misc -----------------------------------------------

    Sub MultiInstance()
        'Check to see if multiple Kernel Simulator processes are running.
        Static ksInst As Mutex
        Dim ksOwner As Boolean
        ksInst = New Mutex(True, "Kernel Simulator", ksOwner)
        If Not ksOwner Then
            KernelError("F", False, 0, DoTranslation("Another instance of Kernel Simulator is running. Shutting down in case of interference.", currentLang), Nothing)
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
