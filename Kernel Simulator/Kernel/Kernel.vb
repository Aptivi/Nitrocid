
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

Imports System.Reflection.Assembly

'TODOs:
'   TODO: Interactive host system file manager (Planned in the bootable version)
'   TODO: Permanent list of usernames and passwords (passwords in encrypted form)
Public Module Kernel

    'Variables
    Public ReadOnly KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String
    Public configReader As New IniFile()
    Public MOTDMessage, HName, MAL As String
    Public ReadOnly EnvironmentOSType As String = Environment.OSVersion.ToString
    Public EventManager As New EventsAndExceptions
    Public DefConsoleOut As IO.TextWriter

    Sub Main()
        'TODO: Re-write the whole kernel in Beta
        'TODO: Target .NET Framework 4.8 on Day 10
        While True
            Try
                'A title
                Console.Title = $"Kernel Simulator v{KernelVersion} - Compiled on {GetCompileDate()}"
                If Not NotifThread.IsAlive Then NotifThread.Start()
                InitPaths()
                If Not EnvironmentOSType.Contains("Unix") Then Initialize255()

                'Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
#If SPECIFIER <> "DEV" And SPECIFIER <> "RC" And SPECIFIER <> "NEARING" Then
                If Not IO.File.Exists(GetExecutingAssembly.Location.Replace(".exe", ".pdb")) Then
                    Dim pdbdown As New WebClient
                    Try
                        pdbdown.DownloadFile($"https://github.com/EoflaOE/Kernel-Simulator/raw/archive/dbgsyms/{KernelVersion}.pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"))
                    Catch ex As Exception
                        'Do nothing, because KS runs fine without debugging symbols
                    End Try
                End If
#End If

                'Initialize everything
                InitEverything()

                'For config
                If RebootRequested Then
                    RebootRequested = False
                    LogoutRequested = False
                    Exit Try
                End If

                'If the two files are not found, create two MOTD files with current config.
                If Not IO.File.Exists(paths("Home") + "/MOTD.txt") Then SetMOTD(DoTranslation("Welcome to Kernel!", currentLang), MessageType.MOTD)
                If Not IO.File.Exists(paths("Home") + "/MAL.txt") Then SetMOTD(DoTranslation("Logged in successfully as <user>", currentLang), MessageType.MAL)

                'Initialize stage counter
                W(vbNewLine + DoTranslation("- Stage 0: System initialization", currentLang), True, ColTypes.Stage)
                W(DoTranslation("Starting RPC...", currentLang), True, ColTypes.Neutral)
                StartRPC()
                W(DoTranslation("Initializing filesystem...", currentLang), True, ColTypes.Neutral)
                InitFS()

                'Phase 1: Probe hardware
                W(vbNewLine + DoTranslation("- Stage 1: Hardware detection", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 1: Probing hardware")
                ProbeHW()

                'Phase 2: Username management
                W(vbNewLine + DoTranslation("- Stage 2: Internal username management", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 2: Manage internal usernames")
                AddUser("root", RootPasswd)
                Permission("Admin", "root", "Allow")
                If enableDemo = True Then AddUser("demo")
                LoginFlag = True

                'Phase 3: Parse Mods and Screensavers
                W(vbNewLine + DoTranslation("- Stage 3: Mods and screensavers detection", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 3: Parse mods and screensavers")
                Wdbg("I", "Safe mode flag is set to {0}", SafeMode)
                If Not SafeMode Then
                    ParseMods(True)
                    Dim modPath As String = paths("Mods")
                    For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                        CompileCustom(modFile.Replace(modPath, ""))
                    Next
                End If

                'Phase 4: Free unused RAM and raise the started event
                W(vbNewLine + DoTranslation("- Stage 4: Garbage collection", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 4: Garbage collection starts and the events now work")
                Dim proc As Process = Process.GetCurrentProcess
                W(DoTranslation("Before garbage collection: {0} bytes", currentLang), True, ColTypes.Neutral, proc.PrivateMemorySize64)
                EventManager.RaiseStartKernel()
                DisposeAll()
                W(DoTranslation("After garbage collection: {0} bytes", currentLang), True, ColTypes.Neutral, proc.PrivateMemorySize64)
                proc.Dispose()

                'Check to see if CI flags were enabled
                If CI_TestWdbg Then
                    StartRDebugThread(False)
                    DebugMode = False
                    PrintLog()
                    PowerManage("shutdown")
                ElseIf CI_TestInit Then
                    CI_TestInitStopwatch.Stop()
                    W(CStr(CI_TestInitStopwatch.ElapsedMilliseconds) + " ms", True, ColTypes.Neutral)
                    PowerManage("shutdown")
                End If

                'Phase 5: Log-in
                W(vbNewLine + DoTranslation("- Stage 5: Log in", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 5: Log in")
                If Not BootArgs Is Nothing Then
                    If BootArgs.Contains("quiet") Then
                        Console.SetOut(DefConsoleOut)
                    End If
                End If

                'Show current time
                ShowCurrentTimes()

                'Initialize login prompt
                If LoginFlag = True And maintenance = False Then
                    LoginPrompt()
                ElseIf LoginFlag = True And maintenance = True Then
                    LoginFlag = False
                    W(DoTranslation("Enter the admin password for maintenance.", currentLang), True, ColTypes.Neutral)
                    answeruser = "root"
                    ShowPasswordPrompt(answeruser)
                End If
            Catch ex As Exception
                If DebugMode = True Then
                    W(ex.StackTrace, True, ColTypes.Uncontinuable) : WStkTrc(ex)
                End If
                KernelError("U", True, 5, DoTranslation("Kernel Error while booting: {0}", currentLang), ex, ex.Message)
            End Try
        End While
    End Sub

End Module
