
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
'   TODO: Permanent list of usernames and passwords (passwords in encrypted form - in beta)
Public Module Kernel

    'Variables
    Public ReadOnly KernelVersion As String = GetExecutingAssembly().GetName().Version.ToString()
    Public BootArgs() As String
    Public configReader As New IniFile()
    Public MOTDMessage, HName, MAL As String
    Public ReadOnly EnvironmentOSType As String = Environment.OSVersion.ToString
    Public EventManager As New EventsAndExceptions

    Sub Main()
        'TODO: Re-write the whole kernel in Beta
        While True
            Try
                'A title
                Console.Title = $"Kernel Simulator v{KernelVersion} - Compiled on {GetCompileDate()}" 'Does MonoDevelop have the ability to compile interpolated strings?
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

                'If the two files are not found, create two MOTD files with current config.
                If Not IO.File.Exists(paths("Home") + "/MOTD.txt") Then SetMOTD(DoTranslation("Welcome to Kernel!", currentLang), MessageType.MOTD)
                If Not IO.File.Exists(paths("Home") + "/MAL.txt") Then SetMOTD(DoTranslation("Logged in successfully as <user>", currentLang), MessageType.MAL)

                'Phase 1: Probe hardware
                WriteWhere("1/5", Console.WindowWidth - 4, Console.WindowHeight - 1, ColTypes.Neutral)
                Wdbg("- Kernel Phase 1: Probing hardware")
                ProbeHW()

                'Phase 2: Username management
                WriteWhere("2/5", Console.WindowWidth - 4, Console.WindowHeight - 1, ColTypes.Neutral)
                Wdbg("- Kernel Phase 2: Manage internal usernames")
                Adduser("root", RootPasswd)
                Permission("Admin", "root", "Allow")
                If enableDemo = True Then Adduser("demo")
                LoginFlag = True

                'Phase 3: Parse Mods and Screensavers
                WriteWhere("3/5", Console.WindowWidth - 4, Console.WindowHeight - 1, ColTypes.Neutral)
                Wdbg("- Kernel Phase 3: Parse mods and screensavers")
                ParseMods(True)
                Dim modPath As String = paths("Mods")
                For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                    CompileCustom(modFile.Replace(modPath, ""))
                Next

                'Phase 4: Free unused RAM and raise the started event
                WriteWhere("4/5", Console.WindowWidth - 4, Console.WindowHeight - 1, ColTypes.Neutral)
                Wdbg("- Kernel Phase 4: Garbage collection starts and the events now work")
                EventManager.RaiseStartKernel()
                DisposeAll()

                'Phase 5: Log-in
                WriteWhere("5/5", Console.WindowWidth - 4, Console.WindowHeight - 1, ColTypes.Neutral)
                Wdbg("- Kernel Phase 5: Log in")
                Console.SetOut(DefConsoleOut)
                ShowTime()
                WriteWhere("   ", Console.WindowWidth - 4, Console.WindowHeight - 1, ColTypes.Neutral)
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
                KernelError("U", True, 5, DoTranslation("Kernel Error while booting: {0}", currentLang), ex, Err.Description)
            End Try
        End While
    End Sub

End Module
