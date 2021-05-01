
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
    Public EventManager As New Events
    Public DefConsoleOut As TextWriter
    Public ScrnTimeout As Integer = 300000
    Public ConsoleTitle As String = $"Kernel Simulator v{KernelVersion} - Milestone 5"

    ''' <summary>
    ''' Entry point
    ''' </summary>
    Sub Main()
        While True
            Try
                'A title
                Console.Title = ConsoleTitle

                'Initialize crucial things
                If Not NotifThread.IsAlive Then NotifThread.Start()
                InitPaths()
                If Not IsOnUnix() Then Initialize255()

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
                        NotifyDebugDownloadError = True
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
                If Not File.Exists(paths("Home") + "/MOTD.txt") Then SetMOTD(DoTranslation("Welcome to Kernel!"), MessageType.MOTD)
                If Not File.Exists(paths("Home") + "/MAL.txt") Then SetMOTD(DoTranslation("Logged in successfully as <user>"), MessageType.MAL)

                'Initialize stage counter
                W(vbNewLine + DoTranslation("- Stage 1: System initialization"), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 1: Initializing system")
                StartRDebugThread(True)
                W(DoTranslation("Starting RPC..."), True, ColTypes.Neutral)
                StartRPC()

                'Check for kernel updates
#If SPECIFIER <> "DEV" And SPECIFIER <> "RC" And SPECIFIER <> "NEARING" Then
                If CheckUpdateStart Then
                    CheckKernelUpdates()
                End If
#End If

                'Phase 1: Probe hardware
                W(vbNewLine + DoTranslation("- Stage 2: Hardware detection"), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 2: Probing hardware")
                StartProbing()

                'Phase 2: Parse Mods and Screensavers
                W(vbNewLine + DoTranslation("- Stage 3: Mods and screensavers detection"), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 3: Parse mods and screensavers")
                Wdbg("I", "Safe mode flag is set to {0}", SafeMode)
                If Not SafeMode Then
                    ParseMods(True)
                Else
                    W(DoTranslation("Running in safe mode. Skipping stage..."), True, ColTypes.Neutral)
                End If
                EventManager.RaiseStartKernel()

                'Phase 3: Log-in
                W(vbNewLine + DoTranslation("- Stage 4: Log in"), True, ColTypes.Stage)
                Wdbg("I", "- Kernel Phase 4: Log in")
                InitializeSystemAccount()
                LoginFlag = True
                If BootArgs IsNot Nothing Then
                    If BootArgs.Contains("quiet") Then
                        Console.SetOut(DefConsoleOut)
                    End If
                End If
                InitializeUsers()
                LoadPermissions()

                'Show current time
                ShowCurrentTimes()

                'Notify user of errors if appropriate
                If NotifyConfigError Then
                    NotifyConfigError = False
                    NotifySend(New Notification With {.Title = DoTranslation("Error loading settings"),
                                                      .Desc = DoTranslation("There is an error while loading settings. You may need to check the settings file."),
                                                      .Priority = NotifPriority.Medium})
                End If
                If NotifyDebugDownloadError Then
                    NotifyDebugDownloadError = False
                    NotifySend(New Notification With {.Title = DoTranslation("Error downloading debug data"),
                                                      .Desc = DoTranslation("There is an error while downloading debug data. Check your internet connection."),
                                                      .Priority = NotifPriority.Medium})
                End If

                'Initialize login prompt
                DisposeAll()
                If LoginFlag = True And maintenance = False Then
                    LoginPrompt()
                ElseIf LoginFlag = True And maintenance = True Then
                    ReadMOTDFromFile(MessageType.MOTD)
                    ReadMOTDFromFile(MessageType.MAL)
                    LoginFlag = False
                    W(DoTranslation("Enter the admin password for maintenance."), True, ColTypes.Neutral)
                    answeruser = "root"
                    ShowPasswordPrompt(answeruser)
                End If
            Catch ex As Exception
                If DebugMode = True Then
                    W(ex.StackTrace, True, ColTypes.Err) : WStkTrc(ex)
                End If
                KernelError("U", True, 5, DoTranslation("Kernel Error while booting: {0}"), ex, ex.Message)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Is this system a Windows system?
    ''' </summary>
    Public Function IsOnWindows()
        Return Environment.OSVersion.Platform = PlatformID.Win32NT
    End Function

    ''' <summary>
    ''' Is this system a Unix system?
    ''' </summary>
    Public Function IsOnUnix()
        Return Environment.OSVersion.Platform = PlatformID.Unix
    End Function

End Module
