
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

Public Module GetCommand

    Public Sub ExecuteCommand(ByVal requestedCommand As String)
        '1. Get the index of the first space (Used for step 3)
        Dim index As Integer = requestedCommand.IndexOf(" ")
        If index = -1 Then index = requestedCommand.Length
        Wdbg("Index: {0}", index)

        '2. Split the requested command string into words
        Dim words() As String = requestedCommand.Split({" "c})
        For i As Integer = 0 To words.Length - 1
            Wdbg("Word {0}: {1}", i + 1, words(i))
        Next

        '3. Get the string of arguments
        Dim strArgs As String = requestedCommand.Substring(index)
        Wdbg("Prototype strArgs: {0}", strArgs)
        If Not index = requestedCommand.Length Then strArgs = strArgs.Substring(1)
        Wdbg("Finished strArgs: {0}", strArgs)

        '4. Split the arguments string into array (args: No empty entries | eargs: Empty entries)
        Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim eargs() As String = strArgs.Split({" "c})

        '4a. Debug: get all arguments from args()
        Wdbg("Arguments parsed from args():")
        For Each arg As String In args
            Wdbg(arg)
        Next

        '4b. Debug: get all arguments from eargs()
        Wdbg("Arguments parsed from eargs():")
        For Each earg As String In eargs
            Wdbg(earg)
        Next

        'The command is done
        Dim Done As Boolean = False

        '5. Check to see is a requested command is obsolete
        If obsoleteCmds.Contains(words(0)) Then
            Wdbg("The command requested {0} is obsolete", words(0))
            W(DoTranslation("This command is obsolete and will be removed in a future release.", currentLang), True, "neutralText")
        End If

        '6. Execute a command
        Try
            If words(0) = "help" Then

                If requestedCommand = "help" Then
                    ShowHelp()
                Else
                    If args.Count - 1 = 0 Then
                        ShowHelp(args(0))
                    Else
                        ShowHelp(words(0))
                    End If
                End If
                Done = True

            ElseIf words(0) = "adduser" Then

                If requestedCommand <> "adduser" Then
                    If args.Count - 1 = 0 Then
                        Adduser(args(0))
                        Done = True
                    ElseIf args.Count - 1 = 2 Then
                        If args(1) = args(2) Then
                            Adduser(args(0), args(1))
                            Done = True
                        Else
                            W(DoTranslation("Passwords doesn't match.", currentLang), True, "neutralText")
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "alias" Then

                If requestedCommand <> "alias" Then
                    If args.Count - 1 > 1 Then
                        If args(0) = "add" Then
                            ManageAlias(args(0), args(1), args(2))
                            Done = True
                        End If
                    ElseIf args.Count - 1 = 1 Then
                        If args(0) = "rem" Then
                            ManageAlias(args(0), args(1))
                            Done = True
                        End If
                    End If
                End If

            ElseIf words(0) = "arginj" Then

                'Argument Injection
                If requestedCommand <> "arginj" Then
                    If args.Count - 1 >= 0 Then
                        answerargs = String.Join(",", args)
                        argsInjected = True
                        W(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot.", currentLang), True, "neutralText", answerargs)
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "cdbglog" Then

                dbgWriter.Close()
                dbgWriter = New StreamWriter(paths("Debugging"))
                Done = True

            ElseIf words(0) = "chdir" Then

                If args.Count - 1 = 0 Then
                    SetCurrDir(strArgs)
                    Done = True
                End If

            ElseIf words(0) = "chhostname" Then

                If requestedCommand <> "chhostname" Then
                    If words(1) = "" Then
                        W(DoTranslation("Blank host name.", currentLang), True, "neutralText")
                    ElseIf words(1).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                        W(DoTranslation("Special characters are not allowed.", currentLang), True, "neutralText")
                    Else
                        Done = True
                        W(DoTranslation("Changing from: {0} to {1}...", currentLang), True, "neutralText", HName, words(1))
                        HName = words(1)
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Login").Keys("Host Name").Value = HName
                        ksconf.Save(pathConfig)
                    End If
                End If

            ElseIf words(0) = "chlang" Then

                If requestedCommand <> "chlang" Then
                    SetLang(words(1))
                    Done = True
                End If

            ElseIf words(0) = "chmotd" Then

                If requestedCommand <> "chmotd" Then
                    If words(1) = "" Then
                        W(DoTranslation("Blank message of the day.", currentLang), True, "neutralText")
                    Else
                        W(DoTranslation("Changing MOTD...", currentLang), True, "neutralText")
                        MOTDMessage = words(1)
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Login").Keys("MOTD").Value = MOTDMessage
                        ksconf.Save(pathConfig)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chmal" Then

                If requestedCommand <> "chmal" Then
                    If words(1) = "" Then
                        W(DoTranslation("Blank MAL After Login.", currentLang), True, "neutralText")
                    Else
                        W(DoTranslation("Changing MAL...", currentLang), True, "neutralText")
                        MAL = words(1)
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Login").Keys("MOTD After Login").Value = MAL
                        ksconf.Save(pathConfig)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chpwd" Then

                If requestedCommand <> "chpwd" Then
                    If eargs.Count - 1 = 3 Then
                        If InStr(eargs(3), " ") > 0 Then
                            W(DoTranslation("Spaces are not allowed.", currentLang), True, "neutralText")
                        ElseIf eargs(3) = eargs(2) Then
                            If eargs(1) = userword(eargs(0)) Then
                                If adminList(eargs(0)) And adminList(signedinusrnm) Then
                                    userword.Item(eargs(0)) = eargs(2)
                                ElseIf adminList(eargs(0)) And Not adminList(signedinusrnm) Then
                                    W(DoTranslation("You are not authorized to change password of {0} because the target was an admin.", currentLang), True, "neutralText", eargs(0))
                                End If
                            Else
                                W(DoTranslation("Wrong user password.", currentLang), True, "neutralText")
                            End If
                        ElseIf eargs(3) <> eargs(2) Then
                            W(DoTranslation("Passwords doesn't match.", currentLang), True, "neutralText")
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chusrname" Then

                If requestedCommand <> "chusrname" Then
                    Dim DoneFlag As Boolean = False
                    If args.Count - 1 = 1 Then
                        If userword.ContainsKey(args(0)) = True Then
                            If Not userword.ContainsKey(args(1)) = True Then
                                DoneFlag = True
                                Dim temporary As String = userword(args(0))
                                userword.Remove(args(0))
                                userword.Add(args(1), temporary)
                                PermissionEditForNewUser(args(0), args(1))
                                W(DoTranslation("Username has been changed to {0}!", currentLang), True, "neutralText", args(1))
                                If args(0) = signedinusrnm Then
                                    LoginPrompt()
                                End If
                            Else
                                W(DoTranslation("The new name you entered is already found.", currentLang), True, "neutralText")
                                Exit Sub
                            End If
                        End If
                        If DoneFlag = False Then
                            W(DoTranslation("User {0} not found.", currentLang), True, "neutralText", args(0))
                        End If
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "cls" Then

                Console.Clear() : Done = True

            ElseIf requestedCommand = "debuglog" Then

                Dim line As String
                Using dbglog = File.Open(paths("Debugging"), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite), reader As New StreamReader(dbglog)
                    line = reader.ReadLine()
                    Do While reader.EndOfStream <> True
                        W(line, True, "neutralText")
                        line = reader.ReadLine
                    Loop
                End Using
                Done = True

            ElseIf words(0) = "ftp" Then

                If requestedCommand = "ftp" Then
                    InitiateShell()
                Else
                    InitiateShell(True, words(1))
                End If
                Done = True

            ElseIf requestedCommand = "lockscreen" Then

                Done = True
                LockMode = True
                ShowSavers(defSaverName)
                ShowPasswordPrompt(signedinusrnm)

            ElseIf words(0) = "list" Then

                'Lists folders and files
                If requestedCommand = "list" Then
                    List(CurrDir)
                    Done = True
                Else
                    If args.Count - 1 = 0 Then
                        List(strArgs)
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "lscomp" Then

                If EnvironmentOSType.Contains("Unix") Then
                    W(DoTranslation("Listing PCs is not supported yet on Unix.", currentLang), True, "neutralText")
                Else
                    Try
                        GetNetworkComputers()
                        ListOnlineAndOfflineHosts()
                    Catch ex As NullReferenceException
                        W(DoTranslation("Couldn't get computers. If you are running the latest version of Windows 10, you need to enable SMBv1. This protocol is however insecure and we will try to solve this problem.", currentLang), True, "neutralText")
                    End Try
                End If
                Done = True

            ElseIf requestedCommand = "lsnet" Then

                If EnvironmentOSType.Contains("Unix") Then
                    W(DoTranslation("Listing PCs is not supported yet on Unix.", currentLang), True, "neutralText")
                Else
                    Try
                        GetNetworkComputers()
                        ListHostsInNetwork()
                    Catch ex As NullReferenceException
                        W(DoTranslation("Couldn't get computers. If you are running the latest version of Windows 10, you need to enable SMBv1. This protocol is however insecure and we will try to solve this problem.", currentLang), True, "neutralText")
                    End Try
                End If
                Done = True

            ElseIf requestedCommand = "lsnettree" Then

                If EnvironmentOSType.Contains("Unix") Then
                    W(DoTranslation("Listing PCs is not supported yet on Unix.", currentLang), True, "neutralText")
                Else
                    Try
                        GetNetworkComputers()
                        ListHostsInTree()
                    Catch ex As NullReferenceException
                        W(DoTranslation("Couldn't get computers. If you are running the latest version of Windows 10, you need to enable SMBv1. This protocol is however insecure and we will try to solve this problem.", currentLang), True, "neutralText")
                    End Try
                End If
                Done = True

            ElseIf requestedCommand = "logout" Then

                'Logs out of the user
                Done = True
                LogoutRequested = True

            ElseIf requestedCommand = "netinfo" Then

                GetProperties()
                Done = True

            ElseIf words(0) = "md" Then

                If requestedCommand <> "md" Then
                    If args.Count - 1 = 0 Then
                        'Create directory
                        Directory.CreateDirectory($"{CurrDir}/{strArgs}")
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "noaliases" Then

                W(DoTranslation("Aliases that are forbidden: {0}", currentLang), True, "neutralText", String.Join(", ", forbidden))
                Done = True

            ElseIf words(0) = "perm" Then

                If requestedCommand <> "perm" Then
                    If args.Count - 1 = 2 Then
                        Permission(args(1), args(0), args(2))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "ping" Then

                If requestedCommand <> "ping" Then
                    If args.Count - 1 = 0 Then
                        PingTarget(args(0), 1)
                        Done = True
                    ElseIf args.Count - 1 = 1 Then
                        PingTarget(args(0), args(1))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "read" Then

                If requestedCommand <> "read" Then
                    If args.Count - 1 = 0 Then
                        If CurrDirStructure.Contains($"{CurrDir}/{strArgs}") Then
                            ReadContents($"{CurrDir}/{strArgs}")
                        Else
                            W(DoTranslation("{0} is not found.", currentLang), True, "neutralText", strArgs)
                        End If
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "reloadconfig" Then

                'Reload configuration
                Done = True
                InitializeConfig()
                W(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect.", currentLang), True, "neutralText")

            ElseIf requestedCommand = "reboot" Then

                'Reboot the simulated system
                Done = True
                PowerManage("reboot")

            ElseIf words(0) = "reloadmods" Then

                'Reload mods
                Done = True
                ReloadMods()
                W(DoTranslation("Mods reloaded.", currentLang), True, "neutralText")

            ElseIf words(0) = "reloadsaver" Then

                If requestedCommand <> "reloadsaver" Then
                    If args.Count - 1 >= 0 Then
                        CompileCustom(strArgs)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "rd" Then

                If requestedCommand <> "rd" Then
                    If args.Count - 1 >= 0 Then
                        Directory.Delete($"{CurrDir}/{strArgs}", True)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "rmuser" Then

                If requestedCommand <> "rmuser" Then
                    If args.Count - 1 = 0 Then
                        RemoveUserFromDatabase(args(0))
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "savescreen" Then

                Done = True
                ShowSavers(defSaverName)

            ElseIf words(0) = "setcolors" Then

                If requestedCommand <> "setcolors" Then
                    If args.Count - 1 = 9 Then
                        Done = True
                        If ColoredShell = True Then
                            If availableColors.Contains(args(0)) And availableColors.Contains(args(1)) And availableColors.Contains(args(2)) And
                                availableColors.Contains(args(3)) And availableColors.Contains(args(4)) And availableColors.Contains(args(5)) And
                                availableColors.Contains(args(6)) And availableColors.Contains(args(7)) And availableColors.Contains(args(8)) And
                                availableColors.Contains(args(9)) Then
                                inputColor = CType([Enum].Parse(GetType(ConsoleColor), args(0)), ConsoleColor)
                                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), args(1)), ConsoleColor)
                                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(2)), ConsoleColor)
                                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(3)), ConsoleColor)
                                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(4)), ConsoleColor)
                                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(5)), ConsoleColor)
                                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), args(6)), ConsoleColor)
                                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), args(7)), ConsoleColor)
                                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), args(8)), ConsoleColor)
                                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), args(9)), ConsoleColor)
                                Load()
                            ElseIf args.Contains("def") Then
                                If Array.IndexOf(args, "") = 0 Then
                                    args(0) = "White"
                                    inputColor = CType([Enum].Parse(GetType(ConsoleColor), args(0)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 1 Then
                                    args(1) = "White"
                                    licenseColor = CType([Enum].Parse(GetType(ConsoleColor), args(1)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 2 Then
                                    args(2) = "Yellow"
                                    contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(2)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 3 Then
                                    args(3) = "Red"
                                    uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(3)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 4 Then
                                    args(4) = "DarkGreen"
                                    hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(4)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 5 Then
                                    args(5) = "Green"
                                    userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(5)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 6 Then
                                    args(6) = "Black"
                                    backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), args(6)), ConsoleColor)
                                    Load()
                                ElseIf Array.IndexOf(args, "") = 7 Then
                                    args(7) = "Gray"
                                    neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), args(7)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 8 Then
                                    args(8) = "DarkYellow"
                                    cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), args(8)), ConsoleColor)
                                ElseIf Array.IndexOf(args, "") = 9 Then
                                    args(9) = "DarkGray"
                                    cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), args(9)), ConsoleColor)
                                End If
                            ElseIf args.Contains("RESET") Then
                                ResetColors()
                                W(DoTranslation("Everything is reset to normal settings.", currentLang), True, "neutralText")
                            Else
                                W(DoTranslation("One or more of the colors is invalid.", currentLang), True, "neutralText")
                            End If
                            MakePermanent()
                        Else
                            W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), True, "neutralText")
                        End If
                    End If
                End If

            ElseIf words(0) = "setsaver" Then

                Dim modPath As String = paths("Mods")
                If requestedCommand <> "setsaver" Then
                    If args.Count >= 0 Then
                        If ScrnSvrdb.ContainsKey(strArgs) Then
                            SetDefaultScreensaver(strArgs)
                        Else
                            If FileIO.FileSystem.FileExists($"{modPath}{strArgs}") Then
                                SetDefaultScreensaver(strArgs)
                            Else
                                W(DoTranslation("Screensaver {0} not found.", currentLang), True, "neutralText", strArgs)
                            End If
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "setthemes" Then

                If requestedCommand <> "setthemes" Then
                    If args.Count - 1 = 0 Then
                        If ColoredShell = True Then TemplateSet(args(0)) Else W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), True, "neutralText")
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "showtd" Then

                ShowTime()
                Done = True

            ElseIf words(0) = "showtdzone" Then

                If requestedCommand <> "showtdzone" Then
                    InitTimesInZones()
                    Dim DoneFlag As Boolean = False
                    For Each zoneName In zoneTimes.Keys
                        If zoneName = strArgs Then
                            DoneFlag = True : Done = True
                            ShowTimesInZones(strArgs)
                        End If
                    Next
                    If DoneFlag = False Then
                        If args(0) = "all" Then
                            ShowTimesInZones()
                            Done = True
                        End If
                    End If
                End If

            ElseIf requestedCommand = "showmotd" Then

                'Show changes to MOTD, or current
                W(ProbePlaces(MOTDMessage), True, "neutralText")
                Done = True

            ElseIf requestedCommand = "showmal" Then

                'Show changes to MAL, or current
                W(ProbePlaces(MAL), True, "neutralText")
                Done = True

            ElseIf requestedCommand = "shutdown" Then

                'Shuts down the simulated system
                PowerManage("shutdown")

            ElseIf requestedCommand = "sses" Then

                If EnvironmentOSType.Contains("Unix") Then
                    Dim feat As New CPUFeatures
                    feat.CheckSSEs()
                Else
                    W("SSE:  {0}" + vbNewLine +
                      "SSE2: {1}" + vbNewLine +
                      "SSE3: {2}", True, "neutralText", CPUFeatures.IsProcessorFeaturePresent(CPUFeatures.SSEnum.InstructionsXMMIAvailable),
                                                        CPUFeatures.IsProcessorFeaturePresent(CPUFeatures.SSEnum.InstructionsXMMI64Available),
                                                        CPUFeatures.IsProcessorFeaturePresent(CPUFeatures.SSEnum.InstructionsSSE3Available))
                End If
                Done = True

            ElseIf requestedCommand = "sysinfo" Then

                Done = True

                'Shows system information
                W(DoTranslation("{0}[ Kernel settings (Running on {1}) ]", currentLang), True, "helpCmd", vbNewLine, EnvironmentOSType)

                'Kernel section
                W(vbNewLine + DoTranslation("Kernel Version:", currentLang) + " {0}" + vbNewLine +
                                DoTranslation("Debug Mode:", currentLang) + " {1}" + vbNewLine +
                                DoTranslation("Colored Shell:", currentLang) + " {2}" + vbNewLine +
                                DoTranslation("Arguments on Boot:", currentLang) + " {3}" + vbNewLine +
                                DoTranslation("Help command simplified:", currentLang) + " {4}" + vbNewLine +
                                DoTranslation("MOTD on Login:", currentLang) + " {5}" + vbNewLine +
                                DoTranslation("Time/Date on corner:", currentLang) + " {6}" + vbNewLine +
                                DoTranslation("Current theme:", currentLang) + " {7}" + vbNewLine, True, "neutralText", KernelVersion, DebugMode.ToString, ColoredShell.ToString, argsOnBoot.ToString, simHelp.ToString, showMOTD.ToString, CornerTD.ToString, currentTheme)

                'Hardware section
                W(DoTranslation("[ Hardware settings ]{0}", currentLang), True, "helpCmd", vbNewLine)
                If Not EnvironmentOSType.Contains("Unix") Then
                    ListDrivers()
                Else
                    ListDrivers_Linux()
                End If

                'User section
                W(DoTranslation("{0}[ User settings ]", currentLang), True, "helpCmd", vbNewLine)
                W(vbNewLine + DoTranslation("Current user name:", currentLang) + " {0}" + vbNewLine +
                                DoTranslation("Current host name:", currentLang) + " {1}" + vbNewLine +
                                DoTranslation("Available usernames:", currentLang) + " {2}" + vbNewLine +
                                DoTranslation("Computer host name:", currentLang) + " {3}", True, "neutralText", signedinusrnm, HName, String.Join(", ", userword.Keys), My.Computer.Name)

                'Messages Section
                W(DoTranslation("{0}[ Messages Settings ]", currentLang), True, "helpCmd", vbNewLine)
                W(vbNewLine + "MOTD: {0}" + vbNewLine +
                              "MAL: {1}", True, "neutralText", ProbePlaces(MOTDMessage), ProbePlaces(MAL))

            ElseIf requestedCommand = "useddeps" Then

                Done = True
                W("MadMilkman.Ini" + vbNewLine +
                  DoTranslation("Source code:", currentLang) + " https://github.com/MarioZ/MadMilkman.Ini" + vbNewLine +
                  DoTranslation("Copyright (c)", currentLang) + " 2016, Mario Zorica" + vbNewLine +
                  DoTranslation("License", currentLang) + " (Apache 2.0): https://github.com/MarioZ/MadMilkman.Ini/blob/master/LICENSE" + vbNewLine +
                  "Newtonsoft.Json" + vbNewLine +
                  DoTranslation("Source code:", currentLang) + " https://github.com/JamesNK/Newtonsoft.Json" + vbNewLine +
                  DoTranslation("Copyright (c)", currentLang) + " 2007, James Newton-King" + vbNewLine +
                  DoTranslation("License", currentLang) + " (MIT): https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md" + vbNewLine +
                  "FluentFTP" + vbNewLine +
                  DoTranslation("Source code:", currentLang) + " https://github.com/robinrodricks/FluentFTP" + vbNewLine +
                  DoTranslation("Copyright (c)", currentLang) + " 2011-2016, J.P. Trosclair" + vbNewLine +
                  DoTranslation("Copyright (c)", currentLang) + " 2016-2019" + ", Robin Rodricks" + vbNewLine +
                  DoTranslation("License", currentLang) + " (MIT): https://github.com/robinrodricks/FluentFTP/blob/master/LICENSE.TXT", True, "neutralText")

            ElseIf words(0) = "usermanual" Then

                Done = True
                If requestedCommand <> "usermanual" Then
                    'Uncomment if there is a new language version for the below manpages and the codeblock fixes are not done. Replace "NewLang" with a three letter language code.
                    'If currentLang = "NewLang" And (strArgs = "Modding guide" Or strArgs = "Screensaver modding guide") Then
                    '    ' Google is weird about translating code blocks for Visual Basic that THEY SHOULDN'T HAVE DONE TO THESE DOCUMENTATIONS!
                    '    W(DoTranslation("You have set your system language to anything other than English. Sorry for the inconvenience, but it seems " +
                    '                    "that Google and its translator are very weird about translating code blocks for any programming language. " +
                    '                    "While we fix all the code blocks for translated manual pages, we suggest you open a ticket to Google Translate " +
                    '                    "for translating code blocks that aren't meant to be translated, and set your system language to English if you " +
                    '                    "speak it.", currentLang), True, "neutralText")
                    '    Exit Sub
                    'End If
                    ViewPage(strArgs)
                Else
                    ViewPage("Available manual pages")
                End If

            End If
            If Done = False Then
                Throw New EventsAndExceptions.NotEnoughArgumentsException(DoTranslation("There was not enough arguments. See below for usage:", currentLang))
            End If
        Catch neaex As EventsAndExceptions.NotEnoughArgumentsException
            Wdbg("User hasn't provided enough arguments for {0}", words(0))
            W(neaex.Message, True, "neutralText")
            ShowHelp(words(0))
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command", currentLang) + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, "neutralText",
                    Err.Number, Err.Description, ex.StackTrace, words(0))
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command", currentLang) + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), True, "neutralText", Err.Number, Err.Description, words(0))
            End If
        End Try
    End Sub

End Module
