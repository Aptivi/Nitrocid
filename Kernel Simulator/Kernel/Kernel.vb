
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

'TODO: Make debug syms mandatory: Download debug syms from GitHub from archive branch (https://github.com/EoflaOE/Kernel-Simulator/tree/archive/dbgsyms), install to the same folder as the executable, inform user that it needs to be shut down, and quit
Public Module Kernel

    'Variables
    Public KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String
    Public AvailableArgs() As String = {"quiet", "cmdinject", "debug", "maintenance", "help"}
    Public availableCMDLineArgs() As String = {"createConf", "promptArgs", "testMod"}
    Public configReader As New IniFile()
    Public MOTDMessage As String
    Public HName As String
    Public MAL As String
    Public EnvironmentOSType As String = Environment.OSVersion.ToString
    Public EventManager As New EventsAndExceptions

    Sub Main()
        'TODO: Re-write the whole kernel in Beta
        'TODO: Give the kernel name of "Meritorious Kernalism" and the simulator name of "MeritSim" in the final release.
        Try
            'A title
            Console.Title = "Kernel Simulator v" & KernelVersion & " - Compiled on " & GetCompileDate()
            InitPaths()

            'Download debug symbols if not found (loads automatically)
            If Not IO.File.Exists(GetExecutingAssembly.Location.Replace(".exe", ".pdb")) Then
                Dim pdbdown As New WebClient
                pdbdown.DownloadFile("https://github.com/EoflaOE/Kernel-Simulator/raw/archive/dbgsyms/" + KernelVersion + ".pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"))
            End If

            'Initialize everything
            InitEverything()

            'Phase 1: Probe hardware
            If Quiet = True Or quietProbe = True Then
                'Continue the kernel, and don't print messages
                ProbeHW(True)
            Else
                'Continue the kernel
                ProbeHW(False)
            End If

            'Phase 2: Username management
            adduser("root", RootPasswd)
            permission("Admin", "root", "Allow", Quiet)
            If enableDemo = True Then adduser("demo")
            LoginFlag = True

            'Phase 3: Check for pre-user making
            If CruserFlag = True Then adduser(arguser, argword)

            'Phase 4: Parse Mods and Screensavers
            ParseMods(True)
            Dim modPath As String = paths("Mods")
            For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                CompileCustom(modFile.Replace(modPath, ""))
            Next

            'Phase 5: Free unused RAM and raise the started event
            EventManager.RaiseStartKernel()
            DisposeAll()

            'Phase 6: Log-in
            If Not Quiet Then ShowTime()
            If LoginFlag = True And maintenance = False Then
                LoginPrompt()
            ElseIf LoginFlag = True And maintenance = True Then
                LoginFlag = False
                Wln(DoTranslation("Enter the admin password for maintenance.", currentLang), "neutralText")
                answeruser = "root"
                showPasswordPrompt(answeruser)
            End If
        Catch ex As Exception
            If DebugMode = True Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError("U", True, 5, DoTranslation("Kernel Error while booting: {0}", currentLang), ex, Err.Description)
        End Try
    End Sub

End Module
