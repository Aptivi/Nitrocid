
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
        Wdbg("Arguments parsed from args(): " + String.Join(", ", args))

        '4b. Debug: get all arguments from eargs()
        Wdbg("Arguments parsed from eargs(): " + String.Join(", ", eargs))

        'The command is done
        Dim Done As Boolean = False

        '5. Check to see if a requested command is obsolete
        If obsoleteCmds.Contains(words(0)) Then
            Wdbg("The command requested {0} is obsolete", words(0))
            W(DoTranslation("This command is obsolete and will be removed in a future release.", currentLang), True, ColTypes.Neutral)
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
                            W(DoTranslation("Passwords doesn't match.", currentLang), True, ColTypes.Neutral)
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
                        Dim FinalArgs As New List(Of String)
                        For Each arg As String In args
                            Wdbg("arginj: Parsing argument {0}...", arg)
                            If AvailableArgs.Contains(arg) Then
                                Wdbg("arginj: Adding argument {0}...", arg)
                                FinalArgs.Add(arg)
                            End If
                        Next
                        If FinalArgs.Count = 0 Then
                            W(DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces", currentLang), True, ColTypes.Neutral)
                            Done = True
                        Else
                            answerargs = String.Join(",", FinalArgs)
                            argsInjected = True
                            W(DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot.", currentLang), True, ColTypes.Neutral, answerargs)
                            Done = True
                        End If
                    End If
                End If

            ElseIf requestedCommand = "cdbglog" Then

                Try
                    dbgWriter.Close()
                    dbgWriter = New StreamWriter(paths("Debugging")) With {.AutoFlush = True}
                    W(DoTranslation("Debug log removed. All connected debugging devices may still view messages.", currentLang), True, ColTypes.Neutral)
                Catch ex As Exception
                    W(DoTranslation("Debug log removal failed: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                    WStkTrc(ex)
                End Try
                Done = True

            ElseIf words(0) = "chdir" Then

                Try
                    SetCurrDir(strArgs)
                Catch ex As Exception
                    W(DoTranslation("Changing directory has failed: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                    WStkTrc(ex)
                End Try
                Done = True

            ElseIf words(0) = "chhostname" Then

                If requestedCommand <> "chhostname" Then
                    If words(1) = "" Then
                        W(DoTranslation("Blank host name.", currentLang), True, ColTypes.Neutral)
                    ElseIf words(1).IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                        W(DoTranslation("Special characters are not allowed.", currentLang), True, ColTypes.Neutral)
                    Else
                        Done = True
                        W(DoTranslation("Changing from: {0} to {1}...", currentLang), True, ColTypes.Neutral, HName, words(1))
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
                    If strArgs = "" Then
                        W(DoTranslation("Blank message of the day.", currentLang), True, ColTypes.Neutral)
                    Else
                        W(DoTranslation("Changing MOTD...", currentLang), True, ColTypes.Neutral)
                        SetMOTD(strArgs, MessageType.MOTD)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chmal" Then

                If requestedCommand <> "chmal" Then
                    If strArgs = "" Then
                        W(DoTranslation("Blank MAL After Login.", currentLang), True, ColTypes.Neutral)
                    Else
                        W(DoTranslation("Changing MAL...", currentLang), True, ColTypes.Neutral)
                        SetMOTD(strArgs, MessageType.MAL)
                        Done = True
                    End If
                End If

            ElseIf words(0) = "chpwd" Then

                If requestedCommand <> "chpwd" Then
                    If eargs.Count - 1 = 3 Then
                        Try
                            If InStr(eargs(3), " ") > 0 Then
                                W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Neutral)
                            ElseIf eargs(3) = eargs(2) Then
                                If eargs(1) = userword(eargs(0)) Then
                                    If adminList(eargs(0)) And adminList(signedinusrnm) Then
                                        userword.Item(eargs(0)) = eargs(2)
                                    ElseIf adminList(eargs(0)) And Not adminList(signedinusrnm) Then
                                        W(DoTranslation("You are not authorized to change password of {0} because the target was an admin.", currentLang), True, ColTypes.Neutral, eargs(0))
                                    End If
                                Else
                                    W(DoTranslation("Wrong user password.", currentLang), True, ColTypes.Neutral)
                                End If
                            ElseIf eargs(3) <> eargs(2) Then
                                W(DoTranslation("Passwords doesn't match.", currentLang), True, ColTypes.Neutral)
                            End If
                        Catch ex As KeyNotFoundException
                            W(DoTranslation("Username is wrong", currentLang), True, ColTypes.Neutral)
                            WStkTrc(ex)
                        End Try
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
                                W(DoTranslation("Username has been changed to {0}!", currentLang), True, ColTypes.Neutral, args(1))
                                If args(0) = signedinusrnm Then
                                    LoginPrompt()
                                End If
                            Else
                                W(DoTranslation("The new name you entered is already found.", currentLang), True, ColTypes.Neutral)
                                Exit Sub
                            End If
                        End If
                        If DoneFlag = False Then
                            W(DoTranslation("User {0} not found.", currentLang), True, ColTypes.Neutral, args(0))
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
                        W(line, True, ColTypes.Neutral)
                        line = reader.ReadLine
                    Loop
                End Using
                Done = True

            ElseIf words(0) = "disconndbgdev" Then

                If requestedCommand <> "disconndbgdev" Then
                    If args.Count - 1 = 0 Then
                        DisconnectDbgDevCmd(args(0))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "ftp" Then

                If requestedCommand = "ftp" Then
                    InitiateShell()
                Else
                    InitiateShell(True, words(1))
                End If
                Done = True

            ElseIf words(0) = "get" Then

                If args.Count <> 0 Then
                    DownloadFile(args(0))
                    Done = True
                End If

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
                    List(strArgs)
                    Done = True
                End If

            ElseIf words(0) = "lset" Then

                If Not args.Count = 0 Then
                    Done = True
                    Try
                        FullParseMode = args(0)
                        Dim ksconf As New IniFile()
                        Dim pathConfig As String = paths("Configuration")
                        ksconf.Load(pathConfig)
                        ksconf.Sections("Misc").Keys("Size parse mode").Value = FullParseMode
                        ksconf.Save(pathConfig)
                        W(DoTranslation("Set successfully.", currentLang), True, ColTypes.Neutral)
                    Catch ex As Exception
                        W(DoTranslation("Error when trying to set parse mode. Check the value and try again. If this is correct, see the stack trace when kernel debugging is enabled.", currentLang), True, ColTypes.Neutral)
                        WStkTrc(ex)
                    End Try
                End If

            ElseIf requestedCommand = "logout" Then

                'Logs out of the user
                Done = True
                LogoutRequested = True

            ElseIf requestedCommand = "lsdbgdev" Then

                Done = True
                For Each DebugDevice As String In DebugDevices.Values
                    W($"- {DebugDevice}", True, ColTypes.HelpCmd)
                Next

            ElseIf requestedCommand = "netinfo" Then

                GetProperties()
                Done = True

            ElseIf words(0) = "md" Then

                If eargs.Count - 1 >= 0 Then
                    'Create directory
                    Directory.CreateDirectory($"{CurrDir}/{strArgs}")
                    Done = True
                End If

            ElseIf requestedCommand = "noaliases" Then

                W(DoTranslation("Aliases that are forbidden: {0}", currentLang), True, ColTypes.Neutral, String.Join(", ", forbidden))
                Done = True

            ElseIf words(0) = "perm" Then

                If requestedCommand <> "perm" Then
                    If args.Count - 1 = 2 Then
                        Permission(args(1), args(0), args(2))
                        Done = True
                    End If
                End If

            ElseIf words(0) = "read" Then

                If requestedCommand <> "read" Then
                    If args.Count - 1 = 0 Then
                        If CurrDirStructure.Contains($"{CurrDir}/{strArgs}") Then
                            ReadContents($"{CurrDir}/{strArgs}")
                        Else
                            W(DoTranslation("{0} is not found.", currentLang), True, ColTypes.Neutral, strArgs)
                        End If
                        Done = True
                    End If
                End If

            ElseIf requestedCommand = "reloadconfig" Then

                'Reload configuration
                Done = True
                InitializeConfig()
                W(DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect.", currentLang), True, ColTypes.Neutral)

            ElseIf words(0) = "reboot" Then

                'Reboot the simulated system
                Done = True
                If args(1) = "safe" Then
                    PowerManage("rebootsafe")
                Else
                    PowerManage("reboot")
                End If

            ElseIf words(0) = "reloadmods" Then

                'Reload mods
                Done = True
                If Not SafeMode Then
                    ReloadMods()
                    W(DoTranslation("Mods reloaded.", currentLang), True, ColTypes.Neutral)
                Else
                    W(DoTranslation("Reloading not allowed in safe mode.", currentLang), True, ColTypes.Neutral)
                End If

            ElseIf words(0) = "reloadsaver" Then

                If requestedCommand <> "reloadsaver" Then
                    If args.Count - 1 >= 0 Then
                        If Not SafeMode Then
                            CompileCustom(strArgs)
                        Else
                            W(DoTranslation("Reloading not allowed in safe mode.", currentLang), True, ColTypes.Neutral)
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "rd" Then

                If args.Count - 1 >= 0 Then
                    Try
                        Directory.Delete($"{CurrDir}/{strArgs}", True)
                    Catch ex As Exception
                        W(DoTranslation("Unable to remove directory: {0}", currentLang), True, ColTypes.Neutral, strArgs)
                        WStkTrc(ex)
                    End Try
                    Done = True
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
                            If args.Contains("RESET") Then
                                ResetColors()
                                W(DoTranslation("Everything is reset to normal settings.", currentLang), True, ColTypes.Neutral)
                            ElseIf args.Contains("def") Then
                                If args(0) = "def" Then
                                    args(0) = "White"
                                    inputColor = CType([Enum].Parse(GetType(ConsoleColor), args(0)), ConsoleColor)
                                End If
                                If args(1) = "def" Then
                                    args(1) = "White"
                                    licenseColor = CType([Enum].Parse(GetType(ConsoleColor), args(1)), ConsoleColor)
                                End If
                                If args(2) = "def" Then
                                    args(2) = "Yellow"
                                    contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(2)), ConsoleColor)
                                End If
                                If args(3) = "def" Then
                                    args(3) = "Red"
                                    uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(3)), ConsoleColor)
                                End If
                                If args(4) = "def" Then
                                    args(4) = "DarkGreen"
                                    hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(4)), ConsoleColor)
                                End If
                                If args(5) = "def" Then
                                    args(5) = "Green"
                                    userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(5)), ConsoleColor)
                                End If
                                If args(6) = "def" Then
                                    args(6) = "Black"
                                    backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), args(6)), ConsoleColor)
                                    Load()
                                End If
                                If args(7) = "def" Then
                                    args(7) = "Gray"
                                    neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), args(7)), ConsoleColor)
                                End If
                                If args(8) = "def" Then
                                    args(8) = "DarkYellow"
                                    cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), args(8)), ConsoleColor)
                                End If
                                If args(9) = "def" Then
                                    args(9) = "DarkGray"
                                    cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), args(9)), ConsoleColor)
                                End If
                            ElseIf availableColors.Contains(args(0)) And availableColors.Contains(args(1)) And availableColors.Contains(args(2)) And
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
                            Else
                                W(DoTranslation("One or more of the colors is invalid.", currentLang), True, ColTypes.Neutral)
                            End If
                            MakePermanent()
                        Else
                            W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), True, ColTypes.Neutral)
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
                            If FileIO.FileSystem.FileExists($"{modPath}{strArgs}") And Not SafeMode Then
                                SetDefaultScreensaver(strArgs)
                            Else
                                W(DoTranslation("Screensaver {0} not found.", currentLang), True, ColTypes.Neutral, strArgs)
                            End If
                        End If
                        Done = True
                    End If
                End If

            ElseIf words(0) = "setthemes" Then

                If requestedCommand <> "setthemes" Then
                    If args.Count - 1 = 0 Then
                        If ColoredShell = True Then TemplateSet(args(0)) Else W(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang), True, ColTypes.Neutral)
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
                    If zoneTimes.Keys.Contains(strArgs) Then
                        DoneFlag = True : Done = True
                        ShowTimesInZones(strArgs)
                    End If
                    If DoneFlag = False Then
                        If eargs(0) = "all" Then
                            ShowTimesInZones()
                            Done = True
                        Else
                            W(DoTranslation("Timezone is specified incorrectly.", currentLang), True, ColTypes.Neutral)
                            Done = True
                        End If
                    End If
                End If

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
                      "SSE3: {2}", True, ColTypes.Neutral, CPUFeatures.IsProcessorFeaturePresent(CPUFeatures.SSEnum.InstructionsXMMIAvailable),
                                                        CPUFeatures.IsProcessorFeaturePresent(CPUFeatures.SSEnum.InstructionsXMMI64Available),
                                                        CPUFeatures.IsProcessorFeaturePresent(CPUFeatures.SSEnum.InstructionsSSE3Available))
                End If
                Done = True

            ElseIf requestedCommand = "sysinfo" Then

                Done = True

                'Shows system information
                W(DoTranslation("{0}[ Kernel settings (Running on {1}) ]", currentLang), True, ColTypes.HelpCmd, vbNewLine, EnvironmentOSType)

                'Kernel section
                W(vbNewLine + DoTranslation("Kernel Version:", currentLang) + " {0}" + vbNewLine +
                              DoTranslation("Debug Mode:", currentLang) + " {1}" + vbNewLine +
                              DoTranslation("Colored Shell:", currentLang) + " {2}" + vbNewLine +
                              DoTranslation("Arguments on Boot:", currentLang) + " {3}" + vbNewLine +
                              DoTranslation("Help command simplified:", currentLang) + " {4}" + vbNewLine +
                              DoTranslation("MOTD on Login:", currentLang) + " {5}" + vbNewLine +
                              DoTranslation("Time/Date on corner:", currentLang) + " {6}" + vbNewLine +
                              DoTranslation("Current theme:", currentLang) + " {7}" + vbNewLine, True, ColTypes.Neutral, KernelVersion, DebugMode.ToString, ColoredShell.ToString, argsOnBoot.ToString, simHelp.ToString, showMOTD.ToString, CornerTD.ToString, currentTheme)

                'Hardware section
                W(DoTranslation("[ Hardware settings ]{0}", currentLang), True, ColTypes.HelpCmd, vbNewLine)
                If Not EnvironmentOSType.Contains("Unix") Then
                    ListDrivers()
                Else
                    ListDrivers_Linux()
                End If

                'User section
                W(DoTranslation("{0}[ User settings ]", currentLang), True, ColTypes.HelpCmd, vbNewLine)
                W(vbNewLine + DoTranslation("Current user name:", currentLang) + " {0}" + vbNewLine +
                              DoTranslation("Current host name:", currentLang) + " {1}" + vbNewLine +
                              DoTranslation("Available usernames:", currentLang) + " {2}" + vbNewLine +
                              DoTranslation("Computer host name:", currentLang) + " {3}", True, ColTypes.Neutral, signedinusrnm, HName, String.Join(", ", userword.Keys), My.Computer.Name)

                'Messages Section
                W(vbNewLine + "[ MOTD ]", True, ColTypes.HelpCmd)
                W(vbNewLine + ProbePlaces(MOTDMessage), True, ColTypes.Neutral)
                W(vbNewLine + "[ MAL ]", True, ColTypes.HelpCmd)
                W(vbNewLine + ProbePlaces(MAL), True, ColTypes.Neutral)

            ElseIf words(0) = "usermanual" Then

                Done = True
                Process.Start("https://github.com/EoflaOE/Kernel-Simulator/wiki")

            End If
            If Done = False Then
                Throw New EventsAndExceptions.NotEnoughArgumentsException(DoTranslation("There was not enough arguments. See below for usage:", currentLang))
            End If
        Catch neaex As EventsAndExceptions.NotEnoughArgumentsException
            Wdbg("User hasn't provided enough arguments for {0}", words(0))
            W(neaex.Message, True, ColTypes.Neutral)
            ShowHelp(words(0))
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command", currentLang) + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Neutral,
                    Err.Number, Err.Description, ex.StackTrace, words(0))
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command", currentLang) + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Neutral, Err.Number, Err.Description, words(0))
            End If
        End Try
    End Sub

End Module
