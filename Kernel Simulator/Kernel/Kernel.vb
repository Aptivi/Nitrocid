
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
Imports System.Reflection.Assembly

Public Module Kernel

    'Variables
    Public MOTDMessage, MAL As String
    Public HostName As String = "kernel"
    Public EventManager As New Events
    Public DefConsoleOut As TextWriter
    Public ReadOnly KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
    Public ReadOnly ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - Milestone 4"
    Public ReadOnly vbNewLine As String = Environment.NewLine
    Friend StageTimer As New Stopwatch

    ''' <summary>
    ''' Entry point
    ''' </summary>
    Sub Main(Args() As String)
        While True
            Try
                'A title
                Console.Title = ConsoleTitle

                'Check for terminal (macOS only). This check is needed because we have the stock Terminal.app (Apple_Terminal according to $TERM_PROGRAM) that
                'has incompatibilities with VT sequences, causing broken display. It claims it supports XTerm, yet it isn't fully XTerm-compliant, so we exit
                'the program early when this stock terminal is spotted.
#If STOCKTERMINALMACOS = False Then
                If IsOnMacOS() Then
                    If GetTerminalEmulator() = "Apple_Terminal" Then
                        Console.WriteLine("Kernel Simulator makes use of VT escape sequences, but Terminal.app has broken support for 255 and true colors. This program can't continue.")
                        Environment.Exit(5)
                    End If
                End If
#End If

                'Initialize crucial things
                InitPaths()
                If Not IsOnUnix() Then Initialize255()

                'Check if factory reset is required
                If Args.Contains("reset") Then
                    FactoryReset()
                End If

                'Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
#If SPECIFIER <> "DEV" And SPECIFIER <> "RC" Then
                NotifyDebugDownloadNetworkUnavailable = Not NetworkAvailable
                If NetworkAvailable Then
                    If Not FileExists(GetExecutingAssembly.Location.Replace(".exe", ".pdb")) Then
                        Dim pdbdown As New WebClient
                        Try
                            pdbdown.DownloadFile($"https://github.com/EoflaOE/Kernel-Simulator/raw/archive/dbgsyms/{KernelVersion}.pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"))
                        Catch ex As Exception
                            NotifyDebugDownloadError = True
                        End Try
                    End If
                End If
#End If

                'Initialize everything
                StageTimer.Start()
                InitEverything(Args)
                CheckErrored()

                'Stage 1: Initialize the system
                If ShowStageFinishTimes Then
                    Write(DoTranslation("Internal initialization finished in") + " {0}", True, ColTypes.StageTime, StageTimer.Elapsed)
                    StageTimer.Restart()
                End If
                Console.WriteLine()
                WriteSeparator(DoTranslation("- Stage 1: System initialization"), False, ColTypes.Stage)
                Wdbg(DebugLevel.I, "- Kernel Phase 1: Initializing system")
                If RDebugAutoStart Then StartRDebugThread()
                Write(DoTranslation("Starting RPC..."), True, ColTypes.Neutral)
                StartRPC()

                'If the two files are not found, create two MOTD files with current config.
                If Not FileExists(GetKernelPath(KernelPathType.MOTD)) Then SetMOTD(DoTranslation("Welcome to Kernel!"), MessageType.MOTD)
                If Not FileExists(GetKernelPath(KernelPathType.MAL)) Then SetMOTD(DoTranslation("Logged in successfully as <user>"), MessageType.MAL)

                'Check for kernel updates
#If SPECIFIER <> "DEV" And SPECIFIER <> "RC" Then
                If CheckUpdateStart Then
                    CheckKernelUpdates()
                End If
#End If

                'Phase 2: Probe hardware
                If ShowStageFinishTimes Then
                    Write(DoTranslation("Stage finished in") + " {0}", True, ColTypes.StageTime, StageTimer.Elapsed)
                    StageTimer.Restart()
                End If
                Console.WriteLine()
                WriteSeparator(DoTranslation("- Stage 2: Hardware detection"), False, ColTypes.Stage)
                Wdbg(DebugLevel.I, "- Kernel Phase 2: Probing hardware")
                StartProbing()
                CheckErrored()

                'Phase 3: Parse Mods and Screensavers
                If ShowStageFinishTimes Then
                    Write(DoTranslation("Stage finished in") + " {0}", True, ColTypes.StageTime, StageTimer.Elapsed)
                    StageTimer.Restart()
                End If
                Console.WriteLine()
                WriteSeparator(DoTranslation("- Stage 3: Mods and screensavers detection"), False, ColTypes.Stage)
                Wdbg(DebugLevel.I, "- Kernel Phase 3: Parse mods and screensavers")
                Wdbg(DebugLevel.I, "Safe mode flag is set to {0}", SafeMode)
                If Not SafeMode Then
                    If StartKernelMods Then StartMods()
                Else
                    Write(DoTranslation("Running in safe mode. Skipping stage..."), True, ColTypes.Neutral)
                End If
                EventManager.RaiseStartKernel()

                'Phase 4: Log-in
                If ShowStageFinishTimes Then
                    Write(DoTranslation("Stage finished in") + " {0}", True, ColTypes.StageTime, StageTimer.Elapsed)
                    StageTimer.Restart()
                End If
                Console.WriteLine()
                WriteSeparator(DoTranslation("- Stage 4: Log in"), False, ColTypes.Stage)
                Wdbg(DebugLevel.I, "- Kernel Phase 4: Log in")
                InitializeSystemAccount()
                InitializeUsers()
                LoadPermissions()

                'Reset console state and stop stage timer
                If ShowStageFinishTimes Then
                    Write(DoTranslation("Stage finished in") + " {0}", True, ColTypes.StageTime, StageTimer.Elapsed)
                    StageTimer.Reset()
                End If
                Console.WriteLine()

                'Unquiet the kernel if quieted
                If EnteredArguments IsNot Nothing Then
                    If EnteredArguments.Contains("quiet") Then
                        Console.SetOut(DefConsoleOut)
                    End If
                End If

                'Show current time
                If ShowCurrentTimeBeforeLogin Then ShowCurrentTimes()

                'Notify user of errors if appropriate
                If NotifyFaultsBoot Then NotifyStartupFaults()

                'Initialize login prompt
                DisposeAll()
                If Not Maintenance Then
                    LoginPrompt()
                Else
                    ReadMOTD(MessageType.MOTD)
                    ReadMOTD(MessageType.MAL)
                    Write(DoTranslation("Enter the admin password for maintenance."), True, ColTypes.Neutral)
                    If Users.ContainsKey("root") Then
                        Wdbg(DebugLevel.I, "Root account found. Prompting for password...")
                        ShowPasswordPrompt("root")
                    Else
                        'Some malicious mod removed the root account, or rare situation happened and it was gone.
                        Wdbg(DebugLevel.W, "Root account not found for maintenance. Initializing it...")
                        InitializeSystemAccount()
                        ShowPasswordPrompt("root")
                    End If
                End If
            Catch kee As Exceptions.KernelErrorException
                WStkTrc(kee)
                KernelErrored = False
                RebootRequested = False
                LogoutRequested = False
                SafeMode = False
            Catch ex As Exception
                WStkTrc(ex)
                KernelError(KernelErrorLevel.U, True, 5, DoTranslation("Kernel Error while booting: {0}"), ex, ex.Message)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Check to see if KernelError has been called
    ''' </summary>
    Sub CheckErrored()
        If KernelErrored Then Throw New Exceptions.KernelErrorException(DoTranslation("Kernel Error while booting: {0}"), LastKernelErrorException, LastKernelErrorException.Message)
    End Sub

End Module
