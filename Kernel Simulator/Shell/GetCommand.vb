
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
        Dim index As Integer = requestedCommand.IndexOf(" ")
        If (index = -1) Then index = requestedCommand.Length
        Dim words = requestedCommand.Split({" "c})
        Dim strArgs As String = requestedCommand.Substring(index)
        If Not (index = requestedCommand.Length) Then strArgs = strArgs.Substring(1)
        Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim Done As Boolean = False
        Try
            If (requestedCommand.Substring(0, index) = "help") Then

                If (requestedCommand = "help") Then
                    ShowHelp()
                Else
                    If (args.Count - 1 = 0) Then
                        ShowHelp(args(0))
                    Else
                        ShowHelp(words(0))
                    End If
                End If
                Done = True

            ElseIf (requestedCommand.Substring(0, index) = "adduser") Then

                If (requestedCommand <> "adduser") Then
                    If (args.Count - 1 = 2) Then
                        adduser(args(0), args(1))
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "alias") Then

                If (requestedCommand <> "alias") Then
                    If (args.Count - 1 > 1) Then
                        If (args(0) = "add") Then
                            manageAlias(args(0), args(1), args(2))
                            Done = True
                        End If
                    ElseIf (args.Count - 1 = 1) Then
                        If (args(0) = "rem") Then
                            manageAlias(args(0), args(1))
                            Done = True
                        End If
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "arginj") Then

                'Argument Injection
                If (requestedCommand <> "arginj") Then
                    If (args.Count - 1 >= 0) Then
                        answerargs = String.Join(",", args)
                        argsInjected = True
                        Wln(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot.", currentLang), "neutralText", answerargs)
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "calc") Then

                If (requestedCommand <> "calc") Then
                    If (args.Count - 1 > 1) Then
                        stdCalc.expressionCalculate(args)
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "cdir") Then

                'Current directory
                Wln(DoTranslation("Current directory: {0}", currentLang), "neutralText", currDir)
                Done = True

            ElseIf (requestedCommand.Substring(0, index) = "chdir") Then

                If (args.Count - 1 = 0) Then
                    If (AvailableDirs.Contains(args(0)) And currDir = "/") Then
                        setCurrDir(args(0))
                    ElseIf (args(0) = "..") Then
                        setCurrDir("")
                    Else
                        Wln(DoTranslation("Directory {0} not found", currentLang), "neutralText", args(0))
                    End If
                    Done = True
                End If

            ElseIf (requestedCommand.Substring(0, index) = "chhostname") Then

                If (requestedCommand <> "chhostname") Then
                    Dim newhost As String = requestedCommand.Substring(11)
                    If (newhost = "") Then
                        Wln(DoTranslation("Blank host name.", currentLang), "neutralText")
                    ElseIf (newhost.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                        Wln(DoTranslation("Special characters are not allowed.", currentLang), "neutralText")
                    Else
                        Done = True
                        Wln(DoTranslation("Changing from: {0} to {1}...", currentLang), "neutralText", HName, newhost)
                        HName = newhost
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Login").Keys("Host Name").Value = newhost
                        ksconf.Save(pathConfig)
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "chmotd") Then

                If (requestedCommand <> "chmotd") Then
                    Dim newmotd = requestedCommand.Substring(7)
                    If (newmotd = "") Then
                        Wln(DoTranslation("Blank message of the day.", currentLang), "neutralText")
                    Else
                        Wln(DoTranslation("Changing MOTD...", currentLang), "neutralText")
                        MOTDMessage = newmotd
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Login").Keys("MOTD").Value = MOTDMessage
                        ksconf.Save(pathConfig)
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "chmal") Then

                If (requestedCommand <> "chmal") Then
                    Dim newmal = requestedCommand.Substring(6)
                    If (newmal = "") Then
                        Wln(DoTranslation("Blank MAL After Login.", currentLang), "neutralText")
                    Else
                        Wln(DoTranslation("Changing MAL...", currentLang), "neutralText")
                        MAL = newmal
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Login").Keys("MOTD After Login").Value = newmal
                        ksconf.Save(pathConfig)
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "chpwd") Then

                If (requestedCommand <> "chpwd") Then
                    If (args.Count - 1 = 3) Then
                        If InStr(args(3), " ") > 0 Then
                            Wln(DoTranslation("Spaces are not allowed.", currentLang), "neutralText")
                        ElseIf (args(3) = args(2)) Then
                            If (args(1) = userword(args(0))) Then
                                userword.Item(args(0)) = args(2)
                            Else
                                Wln(DoTranslation("Wrong user password.", currentLang), "neutralText")
                            End If
                        ElseIf (args(3) <> args(2)) Then
                            Wln(DoTranslation("Passwords doesn't match.", currentLang), "neutralText")
                        End If
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "chusrname") Then

                If (requestedCommand <> "chusrname") Then
                    Dim DoneFlag As Boolean = False
                    If (args.Count - 1 = 1) Then
                        If (userword.ContainsKey(args(0)) = True) Then
                            If Not (userword.ContainsKey(args(1)) = True) Then
                                DoneFlag = True
                                Dim temporary As String = userword(args(0))
                                userword.Remove(args(0))
                                userword.Add(args(1), temporary)
                                permissionEditForNewUser(args(0), args(1))
                                Wln(DoTranslation("Username has been changed to {0}!", currentLang), "neutralText", args(1))
                                If (args(0) = signedinusrnm) Then
                                    LoginPrompt()
                                End If
                            Else
                                Wln(DoTranslation("The new name you entered is already found.", currentLang), "neutralText")
                                Exit Sub
                            End If
                        End If
                        If (DoneFlag = False) Then
                            Wln(DoTranslation("User {0} not found.", currentLang), "neutralText", args(0))
                        End If
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "cls") Then

                Console.Clear() : Done = True

            ElseIf (requestedCommand.Substring(0, index) = "currency") Then

                If (requestedCommand <> "currency") Then
                    If (args.Count - 1 >= 2) Then
                        Done = True
                        CurrencyConvert(args(0), args(1), args(2))
                    End If
                End If

            ElseIf (requestedCommand = "debuglog") Then

                Dim line As String
                Dim debugPath As String = paths("Debugger")
                Using dbglog = File.Open(debugPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite), reader As New StreamReader(dbglog)
                    line = reader.ReadLine()
                    Do While (reader.EndOfStream <> True)
                        Wln(line, "neutralText")
                        line = reader.ReadLine
                    Loop
                End Using
                Done = True

            ElseIf (requestedCommand = "ftp") Then

                InitiateShell() : Done = True

            ElseIf (requestedCommand = "lockscreen") Then

                Done = True
                LockMode = True
                ShowSavers(defSaverName)
                showPasswordPrompt(signedinusrnm)

            ElseIf (requestedCommand.Substring(0, index) = "list") Then

                'Lists folders and files
                If (requestedCommand = "list") Then
                    If (currDir = "/") Then
                        Wln(String.Join(", ", AvailableDirs), "neutralText")
                    Else
                        List(currDir.Substring(1))
                    End If
                    Done = True
                Else
                    If (args.Count - 1 = 0) Then
                        If (AvailableDirs.Contains(args(0)) Or args(0) = ".." Or args(0) = "/" Or (AvailableDirs.Contains(args(0).Substring(1)) And (args(0).StartsWith("/") Or args(0).StartsWith("..")))) Then
                            List(args(0))
                        Else
                            Wln(DoTranslation("Directory {0} not found", currentLang), "neutralText", args(0))
                        End If
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "lscomp") Then

                If (EnvironmentOSType.Contains("Unix")) Then
                    Wln(DoTranslation("Listing PCs is not supported yet on Unix.", currentLang), "neutralText")
                Else
                    GetNetworkComputers()
                    ListOnlineAndOfflineHosts()
                End If
                Done = True

            ElseIf (requestedCommand = "lsnet") Then

                If (EnvironmentOSType.Contains("Unix")) Then
                    Wln(DoTranslation("Listing PCs is not supported yet on Unix.", currentLang), "neutralText")
                Else
                    GetNetworkComputers()
                    ListHostsInNetwork()
                End If
                Done = True

            ElseIf (requestedCommand = "lsnettree") Then

                If (EnvironmentOSType.Contains("Unix")) Then
                    Wln(DoTranslation("Listing PCs is not supported yet on Unix.", currentLang), "neutralText")
                Else
                    GetNetworkComputers()
                    ListHostsInTree()
                End If
                Done = True

            ElseIf (requestedCommand = "logout") Then

                'Logs out of the user
                Done = True
                LogoutRequested = True

            ElseIf (requestedCommand = "netinfo") Then

                getProperties()
                Done = True

            ElseIf (requestedCommand.Substring(0, index) = "md") Then

                If (requestedCommand <> "md") Then
                    If (args.Count - 1 = 0) Then
                        AvailableDirs.Add(args(0))
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "noaliases") Then

                Wln(DoTranslation("Aliases that are forbidden: {0}", currentLang), "neutralText", String.Join(", ", forbidden))
                Done = True

            ElseIf (requestedCommand.Substring(0, index) = "perm") Then

                If (requestedCommand <> "perm") Then
                    If (args.Count - 1 = 2) Then
                        permission(args(1), args(0), args(2))
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "ping") Then

                If (requestedCommand <> "ping") Then
                    If (args.Count - 1 = 0) Then
                        PingTarget(args(0), 1)
                        Done = True
                    ElseIf (args.Count - 1 = 1) Then
                        PingTarget(args(0), args(1))
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "read") Then

                If (requestedCommand <> "read") Then
                    If (args.Count - 1 = 0) Then
                        If (AvailableFiles.Contains(args(0))) Then
                            readContents(args(0))
                        Else
                            Wln(DoTranslation("{0} is not found.", currentLang), "neutralText", args(0))
                        End If
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "reloadconfig") Then

                'Reload configuration
                Done = True
                InitializeConfig()
                Wln(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect.", currentLang), "neutralText")

            ElseIf (requestedCommand = "reboot") Then

                'Reboot the simulated system
                Done = True
                Wln(DoTranslation("Rebooting...", currentLang), "neutralText")
                ResetEverything()
                Console.Clear()
                Main()

            ElseIf (requestedCommand.Substring(0, index) = "reloadsaver") Then

                If (requestedCommand <> "reloadsaver") Then
                    If (args.Count - 1 >= 0) Then
                        CompileCustom(strArgs)
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "rd") Then

                If (requestedCommand <> "rd") Then
                    If (args.Count - 1 = 0) Then
                        AvailableDirs.Remove(args(0))
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "rmuser") Then

                If (requestedCommand <> "rmuser") Then
                    If (args.Count - 1 = 0) Then
                        removeUserFromDatabase(args(0))
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "savescreen") Then

                Done = True
                ShowSavers(defSaverName)

            ElseIf (requestedCommand.Substring(0, index) = "scical") Then

                If (requestedCommand <> "scical") Then
                    If ((args(0) <> "sqrt" And args(0) <> "tan" And args(0) <> "sin" And args(0) <> "cos" And args(0) <> "floor" And args(0) <> "ceiling" And args(0) <> "abs") And args.Count - 1 > 1) Then
                        sciCalc.expressionCalculate(False, args)
                        Done = True
                    ElseIf ((args(0) = "sqrt" Or args(0) = "tan" Or args(0) = "sin" Or args(0) = "cos" Or args(0) = "floor" Or args(0) = "ceiling" Or args(0) = "abs") And args.Count - 1 = 1) Then
                        sciCalc.expressionCalculate(True, args)
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "setcolors") Then

                If (requestedCommand <> "setcolors") Then
                    If (args.Count - 1 = 9) Then
                        Done = True
                        If (ColoredShell = True) Then
                            If (availableColors.Contains(args(0)) And availableColors.Contains(args(1)) And availableColors.Contains(args(2)) And
                                availableColors.Contains(args(3)) And availableColors.Contains(args(4)) And availableColors.Contains(args(5)) And
                                availableColors.Contains(args(6)) And availableColors.Contains(args(7)) And availableColors.Contains(args(8)) And
                                availableColors.Contains(args(9))) Then
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
                            ElseIf (args.Contains("def")) Then
                                If (Array.IndexOf(args, "") = 0) Then
                                    args(0) = "White"
                                    inputColor = CType([Enum].Parse(GetType(ConsoleColor), args(0)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 1) Then
                                    args(1) = "White"
                                    licenseColor = CType([Enum].Parse(GetType(ConsoleColor), args(1)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 2) Then
                                    args(2) = "Yellow"
                                    contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(2)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 3) Then
                                    args(3) = "Red"
                                    uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(3)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 4) Then
                                    args(4) = "DarkGreen"
                                    hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(4)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 5) Then
                                    args(5) = "Green"
                                    userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(5)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 6) Then
                                    args(6) = "Black"
                                    backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), args(6)), ConsoleColor)
                                    Load()
                                ElseIf (Array.IndexOf(args, "") = 7) Then
                                    args(7) = "Gray"
                                    neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), args(7)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 8) Then
                                    args(8) = "DarkYellow"
                                    cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), args(8)), ConsoleColor)
                                ElseIf (Array.IndexOf(args, "") = 9) Then
                                    args(9) = "DarkGray"
                                    cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), args(9)), ConsoleColor)
                                End If
                            ElseIf (args.Contains("RESET")) Then
                                ResetColors()
                                Wln(DoTranslation("Everything is reset to normal settings.", currentLang), "neutralText")
                            Else
                                Wln(DoTranslation("One or more of the colors is invalid.", currentLang), "neutralText")
                            End If
                            MakePermanent()
                        Else
                            Wln(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), "neutralText")
                        End If
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "setsaver") Then

                Dim modPath As String = paths("Mods")
                If (requestedCommand <> "setsaver") Then
                    If (args.Count >= 0) Then
                        If (strArgs = "colorMix" Or strArgs = "matrix" Or strArgs = "disco") Then
                            SetDefaultScreensaver(strArgs)
                        Else
                            If (FileIO.FileSystem.FileExists(modPath + strArgs)) Then
                                SetDefaultScreensaver(strArgs)
                            Else
                                Wln(DoTranslation("Screensaver {0} not found.", currentLang), "neutralText", strArgs)
                            End If
                        End If
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "setthemes") Then

                If (requestedCommand <> "setthemes") Then
                    If (args.Count - 1 = 0) Then
                        If (ColoredShell = True) Then templateSet(args(0)) Else Wln(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), "neutralText")
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "showtd") Then

                ShowTime() : Done = True

            ElseIf (requestedCommand.Substring(0, index) = "showtdzone") Then

                If (requestedCommand <> "showtdzone") Then
                    initTimesInZones()
                    Dim DoneFlag As Boolean = False
                    For Each zoneName In zoneTimes.Keys
                        If (zoneName = strArgs) Then
                            DoneFlag = True : Done = True
                            showTimesInZones(strArgs)
                        End If
                    Next
                    If (DoneFlag = False) Then
                        If (args(0) = "all") Then
                            showTimesInZones()
                            Done = True
                        End If
                    End If
                End If

            ElseIf (requestedCommand = "showmotd") Then

                'Show changes to MOTD, or current
                Wln(ProbePlaces(MOTDMessage), "neutralText")
                Done = True

            ElseIf (requestedCommand = "showmal") Then

                'Show changes to MAL, or current
                Wln(ProbePlaces(MAL), "neutralText")
                Done = True

            ElseIf (requestedCommand = "shutdown") Then

                'Shuts down the simulated system
                Wln(DoTranslation("Shutting down...", currentLang), "neutralText")
                ResetEverything()
                If (DebugMode = True) Then
                    dbgWriter.Close()
                    dbgWriter.Dispose()
                End If
                Environment.Exit(0)

            ElseIf (requestedCommand = "sysinfo") Then

                Done = True

                'Shows system information
                Wln(DoTranslation("{0}[ Kernel settings (Running on {1}) ]", currentLang), "helpCmd", vbNewLine, EnvironmentOSType)

                'Kernel section
                Wln(vbNewLine + DoTranslation("Kernel Version:", currentLang) + " {0}" + vbNewLine +
                                DoTranslation("Debug Mode:", currentLang) + " {1}" + vbNewLine +
                                DoTranslation("Colored Shell:", currentLang) + " {2}" + vbNewLine +
                                DoTranslation("Arguments on Boot:", currentLang) + " {3}" + vbNewLine +
                                DoTranslation("Help command simplified:", currentLang) + " {4}" + vbNewLine +
                                DoTranslation("MOTD on Login:", currentLang) + " {5}" + vbNewLine +
                                DoTranslation("Time/Date on corner:", currentLang) + " {6}" + vbNewLine +
                                DoTranslation("Current theme:", currentLang) + " {7}" + vbNewLine, "neutralText", KernelVersion, DebugMode.ToString, ColoredShell.ToString, argsOnBoot.ToString, simHelp.ToString, showMOTD.ToString, CornerTD.ToString, currentTheme)

                'Hardware section
                Wln(DoTranslation("[ Hardware settings ]{0}", currentLang), "helpCmd", vbNewLine)
                If Not EnvironmentOSType.Contains("Unix") then
                    ListDrivers()
                Else
                    ListDrivers_Linux()
                End If

                'User section
                Wln(DoTranslation("{0}[ User settings ]", currentLang), "helpCmd", vbNewLine)
                Wln(vbNewLine + DoTranslation("Current user name:", currentLang) + " {0}" + vbNewLine +
                                DoTranslation("Current host name:", currentLang) + " {1}" + vbNewLine +
                                DoTranslation("Available usernames:", currentLang) + " {2}" + vbNewLine +
                                DoTranslation("Computer host name:", currentLang) + " {3}", "neutralText", signedinusrnm, HName, String.Join(", ", userword.Keys), My.Computer.Name)

                'Messages Section
                Wln(DoTranslation("{0}[ Messages Settings ]", currentLang), "helpCmd", vbNewLine)
                Wln(vbNewLine + "MOTD: {0}" + vbNewLine +
                                "MAL: {1}", "neutralText", ProbePlaces(MOTDMessage), ProbePlaces(MAL))

            ElseIf (requestedCommand.Substring(0, index) = "unitconv") Then

                If (requestedCommand <> "unitconv") Then
                    If (args.Count - 1 = 2) Then
                        Converter(args(0), args(1), args(2))
                        Done = True
                    End If
                End If

            ElseIf (requestedCommand = "useddeps") Then

                Done = True
                Wln("MadMilkman.Ini" + vbNewLine +
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
                    DoTranslation("Copyright (c)", currentLang) + " 2016-" + DoTranslation("present", currentLang) + ", Robin Rodricks" + vbNewLine +
                    DoTranslation("License", currentLang) + " (MIT): https://github.com/robinrodricks/FluentFTP/blob/master/LICENSE.TXT", "neutralText")

            ElseIf (requestedCommand.Substring(0, index) = "usermanual") Then

                Done = True
                If (requestedCommand <> "usermanual") Then
                    ViewPage(strArgs)
                Else
                    ViewPage("Available manual pages")
                End If

            End If
            If (Done = False) Then
                Throw New EventsAndExceptions.NotEnoughArgumentsException(DoTranslation("There was not enough arguments. See below for usage:", currentLang))
            End If
        Catch neaex As EventsAndExceptions.NotEnoughArgumentsException
            Wln(neaex.Message, "neutralText")
            ShowHelp(requestedCommand.Substring(0, index))
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(DoTranslation("Error trying to execute command", currentLang) + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", "neutralText",
                    Err.Number, Err.Description, ex.StackTrace, words(0))
                Wdbg(ex.StackTrace, True)
            Else
                Wln(DoTranslation("Error trying to execute command", currentLang) + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), "neutralText", Err.Number, Err.Description, words(0))
            End If
        End Try
    End Sub

End Module
