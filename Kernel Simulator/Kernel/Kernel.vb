
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
    Public ReadOnly KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String
    Public configReader As New IniFile()
    Public MOTDMessage, HName, MAL As String
    Public ReadOnly EnvironmentOSType As String = Environment.OSVersion.ToString
    Public EventManager As New EventsAndExceptions
    Public DefConsoleOut As TextWriter
    Public ScrnTimeout As Integer = 300000
    Public ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - Compiled on {GetCompileDate()} - Milestone 2"

    ''' <summary>
    ''' Entry point
    ''' </summary>
    Sub Main()
        'TODO: Re-write the whole kernel in Beta
        While True
            Try
                'A title
                Console.Title = ConsoleTitle

                'Initialize crucial things
                If Not NotifThread.IsAlive Then NotifThread.Start()
                InitPaths()
                If Not EnvironmentOSType.Contains("Unix") Then Initialize255()

                'Check if factory reset is required
                If Environment.GetCommandLineArgs.Contains("reset") Then
                    FactoryReset()
                End If

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
                StartRDebugThread(True)
                W(DoTranslation("Starting RPC...", currentLang), True, ColTypes.Neutral)
                StartRPC()
                W(DoTranslation("Initializing filesystem...", currentLang), True, ColTypes.Neutral)
                CurrDir = paths("Home")

                'Check for kernel updates
#If SPECIFIER <> "DEV" And SPECIFIER <> "RC" And SPECIFIER <> "NEARING" Then
                If CheckUpdateStart Then
                    CheckKernelUpdates()
                End If
#End If

                'Phase 1: Probe hardware
                W(vbNewLine + DoTranslation("- Stage 1: Hardware detection", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 1: Probing hardware")
                StartProbing()

                'Phase 2: Parse Mods and Screensavers
                W(vbNewLine + DoTranslation("- Stage 2: Mods and screensavers detection", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 2: Parse mods and screensavers")
                Wdbg("I", "Safe mode flag is set to {0}", SafeMode)
                If Not SafeMode Then
                    ParseMods(True)
                    Dim modPath As String = paths("Mods")
                    Dim ModFiles = FileIO.FileSystem.GetFiles(modPath)
                    If Not ModFiles.Count = 0 Then
                        For Each modFile As String In ModFiles
                            CompileCustom(modFile.Replace(modPath, ""))
                        Next
                    Else
                        W(DoTranslation("No mods detected. Skipping stage...", currentLang), True, ColTypes.Neutral)
                    End If
                Else
                    W(DoTranslation("Running in safe mode. Skipping stage...", currentLang), True, ColTypes.Neutral)
                End If
                EventManager.RaiseStartKernel()

                'Phase 3: Log-in
                W(vbNewLine + DoTranslation("- Stage 3: Log in", currentLang), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 3: Log in")
                InitializeSystemAccount()
                LoginFlag = True
                If Not BootArgs Is Nothing Then
                    If BootArgs.Contains("quiet") Then
                        Console.SetOut(DefConsoleOut)
                    End If
                End If
                InitializeUsers()
                LoadPermissions()

                'Show current time
                ShowCurrentTimes()

                'Notify user if there is config error
                If NotifyConfigError Then
                    NotifyConfigError = False
                    NotifySend(New Notification With {.Title = DoTranslation("Error loading settings", currentLang),
                                                      .Desc = DoTranslation("There is an error while loading settings. You may need to check the settings file.", currentLang),
                                                      .Priority = NotifPriority.Medium})
                End If

                'Initialize login prompt
                DisposeAll()
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
                    W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                End If
                KernelError("U", True, 5, DoTranslation("Kernel Error while booting: {0}", currentLang), ex, ex.Message)
            End Try
        End While
    End Sub

End Module
