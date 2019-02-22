
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

'TODO: Change app icon in 0.0.6
Imports System.Reflection.Assembly

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

            'Initialize everything
            InitEverything()

            'Check for Unix before probing
            If (EnvironmentOSType.Contains("Unix")) Then
                'TODO: Unix systems must have their own probers after re-organizing subs in either 0.0.5.13 or 0.0.6
                'Phase 1: Skip Phase 1
                Wln(DoTranslation("hwprobe: Probing not supported because it's not designed to run probers on Unix.", currentLang), "neutralText")
                Wdbg("OSVersion is Unix = True")
            Else
                Wdbg("OSVersion is Unix = False")
                'Phase 1: Probe hardware and BIOS if nohwprobe is not passed
                If (Quiet = True Or quietProbe = True) Then
                    'Continue the kernel, and don't print messages
                    HardwareProbe.ProbeHW(True)
                Else
                    'Continue the kernel
                    HardwareProbe.ProbeHW(False)
                End If
            End If

            'Phase 2: Username management
            UserManagement.adduser("root", RootPasswd)
            permission("Admin", "root", "Allow", Quiet)
            If (enableDemo = True) Then UserManagement.adduser("demo")
            LoginFlag = True

            'Phase 3: Check for pre-user making
            If (CruserFlag = True) Then adduser(arguser, argword)

            'Phase 4: Parse Mods and Screensavers
            ModParser.ParseMods(True)
            Dim modPath As String = paths("Mods")
            For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                CompileCustom(modFile.Replace(modPath, ""))
            Next

            'Phase 5: Free unused RAM and raise the started event
            EventManager.RaiseStartKernel()
            DisposeExit.DisposeAll()

            'Phase 6: Log-in
            ShowTime()
            If (userword.Count = 0) Then 'Check if user amount is zero
                Throw New EventsAndExceptions.NullUsersException(DoTranslation("There is no more users remaining in the list.", currentLang))
            End If
            If (LoginFlag = True And maintenance = False) Then
                Login.LoginPrompt()
            ElseIf (LoginFlag = True And maintenance = True) Then
                LoginFlag = False
                Wln(DoTranslation("Enter the admin password for maintenance.", currentLang), "neutralText")
                answeruser = "root"
                Login.showPasswordPrompt(answeruser)
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(ex.StackTrace, "uncontError") : Wdbg(ex.StackTrace, True)
            End If
            KernelError(CChar("U"), True, 5, DoTranslation("Kernel Error while booting: {0}", currentLang), Err.Description)
        End Try

    End Sub

End Module
