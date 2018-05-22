
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Imports System.ComponentModel
Imports System.IO

Module GetCommand

    'Variables
    Public answernewuser As String                                                                          'Input for new user name
    Public answerpassword As String                                                                         'Input for new password
    Public answerbeep As String                                                                             'Input for beep frequency
    Public answerbeepms As String                                                                           'Input for beep milliseconds
    Public key As Double
    Public colors() As ConsoleColor = CType(ConsoleColor.GetValues(GetType(ConsoleColor)), ConsoleColor())  'Console Colors
    Public WithEvents backgroundWorker1 As New System.ComponentModel.BackgroundWorker                       'Black / White disco
    Public answerecho As String                                                                             'Input for printing string
    Public WithEvents backgroundWorker2 As New System.ComponentModel.BackgroundWorker                       '16-bit Colored Disco

    Sub backgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker2.DoWork

        Dim ColorConsole As String = "Black"
        Do While True
            Sleep(50)
            If (backgroundWorker2.CancellationPending = True) Then
                e.Cancel = True
                Console.ResetColor()
                Console.Clear()
                Shell.commandPromptWrite()
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Exit Do
            End If
            If (ColorConsole = "White") Then
                Console.BackgroundColor = ConsoleColor.Black
                ColorConsole = "Black"
                Console.Clear()
            ElseIf (ColorConsole = "Black") Then
                Console.BackgroundColor = ConsoleColor.White
                ColorConsole = "White"
                Console.Clear()
            End If
        Loop

    End Sub

    Sub backgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker1.DoWork

        Do While True
            For Each color In colors
                Sleep(250)
                If (backgroundWorker1.CancellationPending = True) Then
                    e.Cancel = True
                    Console.ResetColor()
                    Console.Clear()
                    Shell.commandPromptWrite()
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Exit Do
                Else
                    Console.BackgroundColor = color
                    Console.Clear()
                End If
            Next
        Loop

    End Sub

    Sub DiscoSystem(Optional ByVal BlackWhite As Boolean = False)

        If (BlackWhite = False) Then
            backgroundWorker1.WorkerSupportsCancellation = True
            backgroundWorker1.RunWorkerAsync()
            If (Console.ReadKey(True).Key = ConsoleKey.Enter) Then
                backgroundWorker1.CancelAsync()
            End If
        ElseIf (BlackWhite = True) Then
            backgroundWorker2.WorkerSupportsCancellation = True
            backgroundWorker2.RunWorkerAsync()
            If (Console.ReadKey(True).Key = ConsoleKey.Enter) Then
                backgroundWorker2.CancelAsync()
            End If
        End If

    End Sub

    Sub ExecuteCommand(ByVal requestedCommand As String)

        Try
            Dim index As Integer = requestedCommand.IndexOf(" ")
            If (index = -1) Then
                index = requestedCommand.Length
            End If
            If (requestedCommand.Substring(0, index) = "help") Then

                If (requestedCommand = "help") Then
                    HelpSystem.ShowHelp()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        HelpSystem.ShowHelp(args(0))
                    Else
                        Wln("Usage: help [command]" + vbNewLine + _
                            "       help: to get all commands", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "adduser") Then

                If (requestedCommand = "adduser") Then
                    UserManagement.addUser()
                    Wln("Tip: You can add permissions to new users by using 'addperm' and then writing their username." + vbNewLine + _
                        "     You can also edit permissions for existing usernames by using 'editperm'.", "neutralText")
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        newPassword(args(0))
                    ElseIf (args.Count - 1 = 2) Then
                        adduser(args(0), args(1))
                    Else
                        Wln("Usage: adduser <userName> [password] [confirm]" + vbNewLine + _
                            "       adduser: to be prompted about new username and password", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "annoying-sound" Or requestedCommand.Substring(0, index) = "beep") Then

                'Beep system initialization
                If (requestedCommand = "annoying-sound" Or requestedCommand = "beep") Then
                    BeepFreq()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 1) Then
                        Beep.Beep(CInt(args(0)), CDbl(args(1)))
                    Else
                        Wln("Usage: annoying-sound/beep <Frequency:Hz> <Time:Seconds>" + vbNewLine + _
                            "       annoying-sound/beep: to be prompted about beeping.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "arginj") Then

                'Argument Injection
                If (requestedCommand = "arginj") Then
                    answerargs = ""
                    ArgumentPrompt.PromptArgs(True)
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" ") + 1, c - 1)
                    Dim args() As String = strArgs.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 >= 0) Then
                        answerargs = String.Join(",", args)
                        argsInjected = True
                        Wln("Injected arguments, {0}, will be scheduled to run at next reboot.", "neutralText", answerargs)
                    Else
                        Wln("Usage: arginj [Arguments sep. by commas]" + vbNewLine + _
                            "       arginj: to be prompted about boot arguments.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "calc") Then

                If (requestedCommand <> "calc") Then
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 > 1) Then
                        stdCalc.expressionCalculate(args)
                    Else
                        Wln("Usage: calc <expression> ...", "neutralText")
                    End If
                Else
                    Wln("Usage: calc <expression> ...", "neutralText")
                End If

            ElseIf (requestedCommand = "cdir" Or requestedCommand = "currentdir") Then

                'Current directory
                Wln("Current directory: {0}", "neutralText", currDir)

            ElseIf (requestedCommand.Substring(0, index) = "cd" Or requestedCommand.Substring(0, index) = "chdir" Or requestedCommand.Substring(0, index) = "changedir") Then

                Dim words = requestedCommand.Split({" "c})
                Dim c As Integer
                For arg = 1 To words.Count - 1
                    c = c + words(arg).Count + 1
                Next
                Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                If (args.Count - 1 = 0) Then
                    If (AvailableDirs.Contains(args(0)) And currDir = "/") Then
                        CurrentDir.setCurrDir(args(0))
                    ElseIf (args(0) = "..") Then
                        CurrentDir.setCurrDir("")
                    Else
                        Wln("Directory {0} not found", "neutralText", args(0))
                    End If
                Else
                    Wln("Usage: chdir/changedir/cd <directory> OR ..", "neutralText")
                End If

            ElseIf (requestedCommand.Substring(0, index) = "chhostname") Then

                If (requestedCommand = "chhostname") Then
                    HostName.ChangeHostName()
                Else
                    Dim newhost As String = requestedCommand.Substring(11)
                    If (newhost = "") Then
                        Wln("Blank host name.", "neutralText")
                        Wln("Usage: chhostname <HostName>" + vbNewLine + _
                            "       chhostname: to be prompted about changing host name.", "neutralText")
                    ElseIf (newhost.Length <= 3) Then
                        Wln("The host name length must be at least 4 characters.", "neutralText")
                        Wln("Usage: chhostname <HostName>" + vbNewLine + _
                            "       chhostname: to be prompted about changing host name.", "neutralText")
                    ElseIf InStr(newhost, " ") > 0 Then
                        Wln("Spaces are not allowed.", "neutralText")
                        Wln("Usage: chhostname <HostName>" + vbNewLine + _
                            "       chhostname: to be prompted about changing host name.", "neutralText")
                    ElseIf (newhost.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                        Wln("Special characters are not allowed.", "neutralText")
                        Wln("Usage: chhostname <HostName>" + vbNewLine + _
                            "       chhostname: to be prompted about changing host name.", "neutralText")
                    ElseIf (newhost = "q") Then
                        Wln("Host name changing has been cancelled.", "neutralText")
                    Else
                        Wln("Changing from: {0} to {1}...", "neutralText", My.Settings.HostName, newhost)
                        My.Settings.HostName = newhost
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "chmotd") Then

                If (requestedCommand = "chmotd") Then
                    ChangeMessage()
                Else
                    Dim newmotd = requestedCommand.Substring(7)
                    If (newmotd = "") Then
                        Wln("Blank message of the day.", "neutralText")
                    ElseIf (newmotd = "q") Then
                        Wln("MOTD changing has been cancelled.", "neutralText")
                    Else
                        W("Changing MOTD...", "neutralText")
                        My.Settings.MOTD = newmotd
                        Wln(" Done!" + vbNewLine + "Please log-out, or use 'showmotd' to see the changes", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "choice") Then

                If (requestedCommand = "choice") Then
                    W("Write a question: ", "input")
                    Dim question As String = System.Console.ReadLine()
                    If (question = "") Then
                        Wln("Blank question. Try again.", "neutralText")
                    ElseIf (question = "q") Then
                        Wln("Choice creation has been cancelled.", "neutralText")
                    Else
                        W("Write choice sets, Ex. Y/N/M/D/F/...: ", "input")
                        Dim sets As String = System.Console.ReadLine()
                        If (sets = "") Then
                            Wln("Blank choice sets. Try again.", "neutralText")
                        ElseIf Not (sets.Contains("/")) And Not (sets.Length - 1 = 0) Then
                            Wln("Cease using choice sets that is, Ex. YNMDF, Y,N,M,D,F, etc.", "neutralText")
                        ElseIf (sets.Length - 1 = 0) Then
                            Wln("One choice set. Try again.", "neutralText")
                        ElseIf (sets = "q") Then
                            Wln("Choice creation has been cancelled.", "neutralText")
                        Else
                            W("{0} <{1}> ", "input", question, sets)
                            Dim answerchoice As String = System.Console.ReadKey.KeyChar
                            Dim answerchoices() As String = sets.Split(CChar("/"))
                            For Each choiceset In answerchoices
                                If (answerchoice = choiceset) Then
                                    Wln(vbNewLine + "Choice {0} selected.", "neutralText", answerchoice)
                                ElseIf (answerchoice = "q") Then
                                    Wln(vbNewLine + "Choice has been cancelled.", "neutralText")
                                End If
                            Next
                        End If
                    End If
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 1) Then
                        W("{0} <{1}> ", "input", args(0), args(1))
                        Dim answerchoice As String = System.Console.ReadKey.KeyChar
                        Dim answerchoices() As String = args(1).Split(CChar("/"))
                        For Each choiceset In answerchoices
                            If (answerchoice = choiceset) Then
                                Wln(vbNewLine + "Choice {0} selected.", "neutralText", answerchoice)
                            ElseIf (answerchoice = "q") Then
                                Wln(vbNewLine + "Choice has been cancelled.", "neutralText")
                            End If
                        Next
                    Else
                        Wln("Usage: choice <Question> <sets>" + vbNewLine + _
                            "       choice: to be prompted about choices.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand = "chpwd") Then

                changePassword()

            ElseIf (requestedCommand.Substring(0, index) = "chusrname") Then

                If (requestedCommand = "chusrname") Then
                    UserManagement.changeName()
                Else
                    Dim DoneFlag As Boolean = False
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 1) Then
                        If (userword.ContainsKey(args(0)) = True) Then
                            If Not (userword.ContainsKey(args(1)) = True) Then
                                DoneFlag = True
                                Dim temporary As String = userword(args(0))
                                userword.Remove(args(0))
                                userword.Add(args(1), temporary)
                                Groups.permissionEditForNewUser(args(0), args(1))
                                Wln("Username has been changed to {0}!", "neutralText", args(1))
                                If (args(0) = signedinusrnm) Then
                                    LoginPrompt()
                                End If
                            Else
                                Wln("The new name you entered is already found.", "neutralText")
                                Exit Sub
                            End If
                        End If
                        If (DoneFlag = False) Then
                            Wln("User {0} not found.", "neutralText", args(0))
                            changePassword()
                        End If
                    Else
                        Wln("Usage: chusrname <oldUserName> <newUserName>" + vbNewLine + _
                            "       chusrname: to be prompted about changing usernames.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand = "cls") Then

                System.Console.Clear()

            ElseIf (requestedCommand = "debuglog") Then

                If (DebugMode = True) Then
                    Dim line As String
                    Using dbglog = File.Open(Environ("USERPROFILE") + "\kernelDbg.log", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite), reader As New StreamReader(dbglog)
                        line = reader.ReadLine()
                        Do While (reader.EndOfStream <> True)
                            Wln(line, "neutralText")
                            line = reader.ReadLine
                        Loop
                    End Using
                Else
                    Wln("Debugging not enabled.", "neutralText")
                End If

            ElseIf (requestedCommand = "disco") Then

                'The disco system.
                DiscoSystem()

            ElseIf (requestedCommand.Substring(0, index) = "echo") Then

                If (requestedCommand = "echo") Then
                    W("Write any text: ", "input")
                    answerecho = System.Console.ReadLine()
                    If (answerecho = "q") Then
                        Wln("Text printing has been cancelled.", "neutralText")
                    Else
                        Wln(answerecho, "neutralText")
                    End If
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 >= 0) Then
                        Wln(String.Join(" ", args), "neutralText")
                    Else
                        Wln("Usage: echo <text>" + vbNewLine + _
                            "       echo: to be prompted about text printing.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand = "future-eyes-destroyer" Or requestedCommand = "fed") Then

                'Disco system, in monochrome
                DiscoSystem(True)

            ElseIf (requestedCommand = "hwprobe") Then

                HardwareProbe.ProbeHW()

            ElseIf (requestedCommand.Substring(0, index) = "ls" Or requestedCommand.Substring(0, index) = "list") Then

                'Lists folders and files
                If (requestedCommand = "ls" Or requestedCommand = "list") Then
                    If (currDir = "/") Then
                        Wln(String.Join(", ", AvailableDirs), "neutralText")
                    Else
                        ListFolders.list(currDir.Substring(1))
                    End If
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        If (AvailableDirs.Contains(args(0)) Or args(0) = ".." Or args(0) = "/" Or (AvailableDirs.Contains(args(0).Substring(1)) And (args(0).StartsWith("/") Or args(0).StartsWith("..")))) Then
                            ListFolders.list(args(0))
                        Else
                            Wln("Directory {0} not found", "neutralText", args(0))
                        End If
                    Else
                        Wln("Usage: ls/list [oneDirectory]" + vbNewLine + _
                            "       ls/list: to get current directory.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand = "lsdrivers") Then

                HardwareProbe.ListDrivers()

            ElseIf (requestedCommand = "lsnet") Then

                NetworkList.GetNetworkComputers()
                NetworkList.ListHostsInNetwork()

            ElseIf (requestedCommand = "lsnettree") Then

                NetworkList.GetNetworkComputers()
                NetworkList.ListHostsInTree()

            ElseIf (requestedCommand = "logout") Then

                'Logs out of the user
                LoginPrompt()

            ElseIf (requestedCommand = "netinfo") Then

                NetworkTools.getProperties()

            ElseIf (requestedCommand.Substring(0, index) = "mkdir" Or requestedCommand.Substring(0, index) = "md") Then

                If (requestedCommand <> "mkdir" Or requestedCommand <> "md") Then
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        AvailableDirs.Add(args(0))
                    Else
                        Wln("Usage: md/mkdir <anything>", "neutralText")
                    End If
                Else
                    Wln("Usage: md/mkdir <anything>", "neutralText")
                End If

            ElseIf (requestedCommand.Substring(0, index) = "panicsim") Then

                'Kernel panic simulator
                If (requestedCommand = "panicsim") Then
                    PanicSim.panicPrompt()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        KernelError(CChar("C"), False, 0, args(0))
                    ElseIf (args.Count - 1 = 1) Then
                        If (args(1) <> "C") Then
                            KernelError(CChar(args(1)), True, 30, args(0))
                        ElseIf (args(1) = "C") Then
                            KernelError(CChar(args(1)), False, 0, args(0))
                        ElseIf (args(1) = "D") Then
                            KernelError(CChar(args(1)), True, 5, args(0))
                        Else
                            Wln("Usage: panicsim <message> [S/F/D/[C]/U] [RebootTime:Seconds]" + vbNewLine + _
                                "       panicsim: to be prompted about panic simulator options.", "neutralText")
                        End If
                    ElseIf (args.Count - 1 = 2) Then
                        If (CDbl(args(2)) <= 3600 And (args(1) <> "C" Or args(1) <> "D")) Then
                            KernelError(CChar(args(1)), True, CLng(args(2)), args(0))
                        ElseIf (CDbl(args(2)) <= 3600 And args(1) = "C") Or (CDbl(args(2)) <= 0 And args(1) = "C") Then
                            KernelError(CChar(args(1)), False, 0, args(0))
                        ElseIf (CDbl(args(2)) <= 5 And args(1) = "D") Then
                            KernelError(CChar(args(1)), True, CLng(args(2)), args(0))
                        Else
                            Wln("Usage: panicsim <message> [S/F/D/[C]/U] [RebootTime:Seconds]" + vbNewLine + _
                                "       panicsim: to be prompted about panic simulator options.", "neutralText")
                        End If
                    Else
                        Wln("Usage: panicsim <message> [S/F/D/[C]/U] [RebootTime:Seconds]" + vbNewLine + _
                            "       panicsim: to be prompted about panic simulator options.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "perm") Then

                If (requestedCommand = "perm") Then
                    Groups.permissionPrompt()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 2) Then
                        permission(args(1), args(0), args(2))
                    Else
                        Wln("Usage: perm <userName> <Admin/Disabled> <Allow/Disallow>" + vbNewLine + _
                            "       perm: to be prompted about permission setting.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "ping") Then

                If (requestedCommand = "ping") Then
                    Network.CheckNetworkCommand()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        Network.PingTarget(args(0), 1)
                    ElseIf (args.Count - 1 = 1) Then
                        Network.PingTarget(args(0), args(1))
                    Else
                        Wln("Usage: ping <Address> [repeatTimes]" + vbNewLine + _
                            "       ping: to get prompted about writing address.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "read") Then

                If (requestedCommand = "read") Then
                    W("Write a file (directories will be scanned): ", "input")
                    Dim readfile As String = System.Console.ReadLine()
                    If (readfile = "") Then
                        Wln(String.Join(", ", AvailableDirs), "neutralText")
                    ElseIf (readfile = "q") Then
                        Wln("Listing has been cancelled.", "neutralText")
                    ElseIf (AvailableFiles.Contains(readfile)) Then
                        FileContents.readContents(readfile)
                    Else
                        Wln("{0} is not found.", "neutralText", readfile)
                    End If
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        If (AvailableFiles.Contains(args(0))) Then
                            FileContents.readContents(args(0))
                        Else
                            Wln("{0} is not found.", "neutralText", args(0))
                        End If
                    Else
                        Wln("Usage: read <file>" + vbNewLine + _
                            "       read: to get prompted about reading file contents.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand = "reloadconfig") Then

                'Reload configuration
                If (File.Exists(Environ("USERPROFILE") + "\kernelConfig.ini") = True) Then
                    configReader = My.Computer.FileSystem.OpenTextFileReader(Environ("USERPROFILE") + "\kernelConfig.ini")
                Else
                    Config.createConfig(False)
                    configReader = My.Computer.FileSystem.OpenTextFileReader(Environ("USERPROFILE") + "\kernelConfig.ini")
                End If
                Config.readConfig()
                Wln("Configuration reloaded. You might need to reboot the kernel for some changes to take effect.", "neutralText")

            ElseIf (requestedCommand = "reboot") Then

                'Reboot the simulated system
                Wln("Rebooting...", "neutralText")
                System.Console.Beep(870, 250)
                KernelTools.ResetEverything()
                System.Console.Clear()
                Main()

            ElseIf (requestedCommand.Substring(0, index) = "rmdir" Or requestedCommand.Substring(0, index) = "rd") Then

                If (requestedCommand <> "rmdir" Or requestedCommand <> "rd") Then
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        AvailableDirs.Remove(args(0))
                    Else
                        Wln("Usage: rd/rmdir <directory>", "neutralText")
                    End If
                Else
                    Wln("Usage: rd/rmdir <directory>", "neutralText")
                End If

            ElseIf (requestedCommand.Substring(0, index) = "rmuser") Then

                If (requestedCommand = "rmuser") Then
                    UserManagement.removeUser()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        UserManagement.removeUserFromDatabase(args(0))
                    Else
                        Wln("Usage: rmuser <Username>" + vbNewLine + _
                            "       rmuser: to get prompted about removing usernames.", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "scical") Then

                If (requestedCommand <> "scical") Then
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If ((args(0) <> "sqrt" And args(0) <> "tan" And args(0) <> "sin" And args(0) <> "cos" And args(0) <> "floor" And args(0) <> "ceiling" And args(0) <> "abs") And args.Count - 1 > 1) Then
                        sciCalc.expressionCalculate(False, args)
                    ElseIf ((args(0) = "sqrt" Or args(0) = "tan" Or args(0) = "sin" Or args(0) = "cos" Or args(0) = "floor" Or args(0) = "ceiling" Or args(0) = "abs") And args.Count - 1 = 1) Then
                        sciCalc.expressionCalculate(True, args)
                    Else
                        Wln("Usage: scical <expression1|pi|e> <+|-|*|/|%> <expression2|pi|e> ..." + vbNewLine + _
                            "       scical <sqrt|tan|sin|cos> <number>", "neutralText")
                    End If
                Else
                    Wln("Usage: scical <expression1|pi|e> <+|-|*|/|%> <expression2|pi|e> ..." + vbNewLine + _
                        "       scical <sqrt|tan|sin|cos> <number>", "neutralText")
                End If

            ElseIf (requestedCommand.Substring(0, index) = "setcolors") Then

                If (requestedCommand = "setcolors") Then
                    Wln("Available Colors: {0}" + vbNewLine + _
                        "Press ENTER only on questions and defaults will be used.", "neutralText", String.Join(", ", availableColors))
                    ColorSet.SetColorSteps()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 7) Then
                        If (availableColors.Contains(args(0)) And availableColors.Contains(args(1)) And availableColors.Contains(args(2)) And _
                            availableColors.Contains(args(3)) And availableColors.Contains(args(4)) And availableColors.Contains(args(5)) And _
                            availableColors.Contains(args(6)) And availableColors.Contains(args(7))) Then
                            inputColor = CType([Enum].Parse(GetType(ConsoleColor), args(0)), ConsoleColor)
                            licenseColor = CType([Enum].Parse(GetType(ConsoleColor), args(1)), ConsoleColor)
                            contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(2)), ConsoleColor)
                            uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), args(3)), ConsoleColor)
                            hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(4)), ConsoleColor)
                            userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), args(5)), ConsoleColor)
                            backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), args(6)), ConsoleColor)
                            neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), args(7)), ConsoleColor)
                            LoadBackground.Load()
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
                                LoadBackground.Load()
                            ElseIf (Array.IndexOf(args, "") = 7) Then
                                args(7) = "Gray"
                                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), args(7)), ConsoleColor)
                            End If
                        ElseIf (args.Contains("RESET")) Then
                            ResetColors()
                            Wln("Everything is reset to normal settings.", "neutralText")
                        Else
                            Wln("One or more of the colors is invalid.", "neutralText")
                        End If
                    Else
                        Wln("Usage: setcolors <inputColor/def> <licenseColor/def> <contKernelErrorColor/def> <uncontKernelErrorColor/def> <hostNameShellColor/def> <userNameShellColor/def> <backgroundColor/def> <neutralTextColor/def>" + vbNewLine + _
                            "       setcolors: to get prompted about setting colors." + vbNewLine + _
                            "       Friends of setcolors: setthemes", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand.Substring(0, index) = "setthemes") Then

                If (requestedCommand = "setthemes") Then
                    TemplateSet.TemplatePrompt()
                Else
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 0) Then
                        TemplateSet.templateSet(args(0))
                    Else
                        Wln("Usage: setthemes <Theme>" + vbNewLine + _
                            "       setthemes: to get prompted about setting themes." + vbNewLine + _
                            "       Friends of setthemes: setcolors", "neutralText")
                    End If
                End If

            ElseIf (requestedCommand = "showtd") Then

                TimeDate.ShowTime()

            ElseIf (requestedCommand = "showmotd") Then

                'Show changes to MOTD, or current
                Wln(My.Settings.MOTD, "neutralText")

            ElseIf (requestedCommand = "shutdown") Then

                'Shuts down the simulated system
                Wln("Shutting down...", "neutralText")
                System.Console.Beep(870, 250)
                KernelTools.ResetEverything()
                dbgWriter.Close()
                dbgWriter.Dispose()
                System.Console.Clear()
                Environment.Exit(0)

            ElseIf (requestedCommand = "sysinfo") Then

                'Shows system information
                Wln("Kernel Version: {0}" + vbNewLine + _
                    "Shell (uesh) version: {1}" + vbNewLine + vbNewLine + _
                    "Look at hardware information using 'lsdrivers'", "neutralText", KernelVersion, ueshversion)

            ElseIf (requestedCommand.Substring(0, index) = "unitconv") Then

                If (requestedCommand <> "unitconv") Then
                    Dim words = requestedCommand.Split({" "c})
                    Dim c As Integer
                    For arg = 1 To words.Count - 1
                        c = c + words(arg).Count + 1
                    Next
                    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
                    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                    If (args.Count - 1 = 2) Then
                        unitConv.Converter(args(0), args(1), args(2))
                    Else
                        Wln("Usage: unitconv <sourceUnit> <targetUnit> <value>" + vbNewLine + _
                            "Units: B, KB, MB, GB, TB, Bits, Octal, Binary, Decimal, Hexadecimal, mm, cm, m, km, Fahrenheit, Celsius, Kelvin, " + _
                            "j, kj, m/s, km/h, cm/ms, Kilograms, Grams, Tons, Kilotons, Megatons, kn, n, Hz, kHz, MHz, GHz, Number (source only), " + _
                            "Money (target only), Percent (target only), Centivolts, Volts, Kilovolts, Watts, Kilowatts, Milliliters, Liters, " + _
                            "Kiloliters, Gallons, Ounces, Feet, Inches, Yards and Miles.", "neutralText")
                    End If
                Else
                    Wln("Usage: unitconv <sourceUnit> <targetUnit> <value>" + vbNewLine + _
                            "Units: B, KB, MB, GB, TB, Bits, Octal, Binary, Decimal, Hexadecimal, mm, cm, m, km, Fahrenheit, Celsius, Kelvin, " + _
                            "j, kj, m/s, km/h, cm/ms, Kilograms, Grams, Tons, Kilotons, Megatons, kn, n, Hz, kHz, MHz, GHz, Number (source only), " + _
                            "Money (target only), Percent (target only), Centivolts, Volts, Kilovolts, Watts, Kilowatts, Milliliters, Liters, " + _
                            "Kiloliters, Gallons, Ounces, Feet, Inches, Yards and Miles.", "neutralText")
                End If

            ElseIf (requestedCommand = "version") Then

                'Shows current kernel version
                Wln("Version: {0}", "neutralText", KernelVersion)

            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("Error trying to execute command." + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", "neutralText", _
                    Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln("Error trying to execute command." + vbNewLine + "Error {0}: {1}", "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

End Module
