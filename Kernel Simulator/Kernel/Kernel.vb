
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

Public Module Kernel

    'Variables
    Public ReadOnly KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String
    Public AvailableArgs() As String = {"quiet", "cmdinject", "debug", "maintenance", "help"}
    Public availableCMDLineArgs() As String = {"createConf", "testMod"}
    Public configReader As New IniFile()
    Public MOTDMessage As String
    Public HName As String
    Public MAL As String
    Public ReadOnly EnvironmentOSType As String = Environment.OSVersion.ToString
    Public EventManager As New EventsAndExceptions

    Sub Main()
        'TODO: Re-write the whole kernel in Beta
        'TODO: Give the kernel name of "Meritorious Kernalism" and the simulator name of "MeritSim" in the final release.
        While True
            Try
                'A title
                Console.Title = $"Kernel Simulator v{KernelVersion} - Compiled on {GetCompileDate()}"
                InitPaths()

                'Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
                If Not IO.File.Exists(GetExecutingAssembly.Location.Replace(".exe", ".pdb")) Then
                    Dim pdbdown As New WebClient
                    pdbdown.DownloadFile($"https://github.com/EoflaOE/Kernel-Simulator/raw/archive/dbgsyms/{KernelVersion}.pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"))
                End If

                'Initialize everything
                InitEverything()

                'For config
                If RebootRequested Then
                    RebootRequested = False
                    LogoutRequested = False
                    Exit Try
                End If

                'Phase 1: Probe hardware
                Wdbg("- Kernel Phase 1: Probing hardware")
                ProbeHW()

                'Phase 2: Username management
                Wdbg("- Kernel Phase 2: Manage internal usernames")
                Adduser("root", RootPasswd)
                Permission("Admin", "root", "Allow", Quiet)
                If enableDemo = True Then Adduser("demo")
                LoginFlag = True

                'Phase 3: Parse Mods and Screensavers
                Wdbg("- Kernel Phase 3: Parse mods and screensavers")
                ParseMods(True)
                Dim modPath As String = paths("Mods")
                For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                    CompileCustom(modFile.Replace(modPath, ""))
                Next

                'Phase 4: Free unused RAM and raise the started event
                Wdbg("- Kernel Phase 4: Garbage collection starts and the events now work")
                EventManager.RaiseStartKernel()
                DisposeAll()

                'Phase 5: Log-in
                Wdbg("- Kernel Phase 5: Log in")
                If Not Quiet Then ShowTime()
                If LoginFlag = True And maintenance = False Then
                    LoginPrompt()
                ElseIf LoginFlag = True And maintenance = True Then
                    LoginFlag = False
                    W(DoTranslation("Enter the admin password for maintenance.", currentLang), True, "neutralText")
                    answeruser = "root"
                    ShowPasswordPrompt(answeruser)
                End If
            Catch ex As Exception
                If DebugMode = True Then
                    W(ex.StackTrace, True, "uncontError") : WStkTrc(ex)
                End If
                KernelError("U", True, 5, DoTranslation("Kernel Error while booting: {0}", currentLang), ex, Err.Description)
            End Try
        End While
    End Sub

End Module
