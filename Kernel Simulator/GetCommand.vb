
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

Module GetCommand

    'Variables
    Public answernewuser As String                                                          'Input for new user name
    Public answerpassword As String                                                         'Input for new password
    Public answerbeep As String                                                             'Input for beep frequency
    Public answerbeepms As String                                                           'Input for beep milliseconds
    Public key As Integer                                                                   'Key
    Public colors() As ConsoleColor = ConsoleColor.GetValues(GetType(ConsoleColor))         'Console Colors
    Public WithEvents backgroundWorker1 As New System.ComponentModel.BackgroundWorker       'Enable working while kernel runs (1)
    Public answerecho As String                                                             'Input for printing string
    Public WithEvents backgroundWorker2 As New System.ComponentModel.BackgroundWorker       'Enable working while kernel runs (2)

    Sub BeepFreq()

        System.Console.Write("Beep Frequency in Hz that has the limit of 37-32767 Hz: ")
        System.Console.ForegroundColor = ConsoleColor.White
        answerbeep = System.Console.ReadLine()
        System.Console.ResetColor()
        If (answerbeep > Int32.MaxValue) Then
            System.Console.WriteLine("Integer overflow on frequency.")
        Else
            If Integer.TryParse(answerbeep, key) Then
                If (answerbeep <= 36 Or answerbeep >= 32768) Then
                    System.Console.WriteLine("Invalid value for beep frequency.")
                ElseIf (answerbeep > 2048) Then
                    System.Console.WriteLine("WARNING: Beep may be loud, depending on speaker. Setting values higher than 2048 might cause your ears to damage, " + _
                                             "and more importantly, your motherboard speaker might deafen, or malfunction.")
                    System.Console.Write("Are you sure that you want to beep at this frequency, {0}? (y/N) ", answerbeep)
                    System.Console.ForegroundColor = ConsoleColor.White
                    Dim answerrape = System.Console.ReadKey.KeyChar
                    System.Console.ResetColor()
                    If (answerrape = "n" Or answerrape = "N" Or answerrape = "") Then
                        System.Console.WriteLine(vbNewLine + "High frequency. Please read documentation for more info why high frequency shouldn't be used.")
                    ElseIf (answerrape = "y" Or answerrape = "Y") Then
                        BeepSystem()
                    End If
                Else
                    BeepSystem()
                End If
            End If
        End If

    End Sub
    Sub BeepSystem()

        System.Console.Write(vbNewLine + "Beep Time in seconds that has the limit of 1-3600: ")
        System.Console.ForegroundColor = ConsoleColor.White
        answerbeepms = System.Console.ReadLine()
        System.Console.ResetColor()
        If Double.TryParse(answerbeepms, key) Then
            If (answerbeepms <= 0 Or answerbeepms >= 3601) Then
                System.Console.WriteLine("Invalid value for beep time.")
            Else
                System.Console.WriteLine("Beeping in {0} seconds in {1} Hz...", answerbeepms, answerbeep)
                System.Console.Beep(answerbeep, answerbeepms * 1000)
                System.Console.WriteLine("Beep complete.")
            End If
        End If

    End Sub

    Sub backgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker2.DoWork

        Dim ColorConsole As String = "Black"
        Do While True
            Sleep(50)
            If (backgroundWorker2.CancellationPending = True) Then
                e.Cancel = True
                Console.ResetColor()
                Console.Clear()
                If signedinusrnm = "root" Then
                    System.Console.Write("[")
                    System.Console.ForegroundColor = ConsoleColor.Green
                    System.Console.Write(signedinusrnm)
                    System.Console.ResetColor()
                    System.Console.Write("@")
                    System.Console.ForegroundColor = ConsoleColor.DarkGreen
                    System.Console.Write(My.Settings.HostName)
                    System.Console.ResetColor()
                    System.Console.Write("] # ")
                Else
                    System.Console.Write("[")
                    System.Console.ForegroundColor = ConsoleColor.Green
                    System.Console.Write(signedinusrnm)
                    System.Console.ResetColor()
                    System.Console.Write("@")
                    System.Console.ForegroundColor = ConsoleColor.DarkGreen
                    System.Console.Write(My.Settings.HostName)
                    System.Console.ResetColor()
                    System.Console.Write("] $ ")
                End If
                System.Console.ForegroundColor = ConsoleColor.White
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
                    If signedinusrnm = "root" Then
                        System.Console.Write("[")
                        System.Console.ForegroundColor = ConsoleColor.Green
                        System.Console.Write(signedinusrnm)
                        System.Console.ResetColor()
                        System.Console.Write("@")
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen
                        System.Console.Write(My.Settings.HostName)
                        System.Console.ResetColor()
                        System.Console.Write("] # ")
                    Else
                        System.Console.Write("[")
                        System.Console.ForegroundColor = ConsoleColor.Green
                        System.Console.Write(signedinusrnm)
                        System.Console.ResetColor()
                        System.Console.Write("@")
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen
                        System.Console.Write(My.Settings.HostName)
                        System.Console.ResetColor()
                        System.Console.Write("] $ ")
                    End If
                    System.Console.ForegroundColor = ConsoleColor.White
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

        If (requestedCommand = "help") Then

            'Shows available commands
            System.Console.WriteLine("Help commands:" + vbNewLine + vbNewLine + _
                                     "adduser: Adds users (Only root or useradd user can access this command)" + vbNewLine + _
                                     "annoying-sound (Alias: beep): Console will beep in Hz and time in milliseconds" + vbNewLine + _
                                     "cdir (Alias: currentdir): Shows current directory" + vbNewLine + _
                                     "changedir (Alias: chdir): Changes directory (WARNING: EXPERIMENTAL)" + vbNewLine + _
                                     "chhostname: Changes host name (Root only)" + vbNewLine + _
                                     "chmotd: Changes MOTD, the Message Of The Day (Root only)" + vbNewLine + _
                                     "choice: Makes user choices" + vbNewLine + _
                                     "chpwd: Changes password for current user" + vbNewLine + _
                                     "disco: A disco effect! (press ENTER to quit)" + vbNewLine + _
                                     "echo: Writes a text into a console" + vbNewLine + _
                                     "future-eyes-destroyer (Alias: fed): Like disco, but black/white version." + vbNewLine + _
                                     "help: Help page" + vbNewLine + _
                                     "hwprobe: Probe hardware manually (One time in 'nohwprobe' kernel)" + vbNewLine + _
                                     "list (Alias: ls): List file/folder contents in current folder (WARNING: EXPERIMENTAL, some of the features are not implemented)" + vbNewLine + _
                                     "logout: Logs you out." + vbNewLine + _
                                     "lsdrivers: Lists drivers that is recognized by the kernel." + vbNewLine + _
                                     "lsnet: Lists all network addresses on host" + vbNewLine + _
                                     "lsnettree: Lists all network addresses on host using the tree" + vbNewLine + _
                                     "ping: Check to see if specified address is available" + vbNewLine + _
                                     "read: Writes file contents to the console" + vbNewLine + _
                                     "reboot: Restarts your computer (WARNING: No syncing, because it is not a final kernel)" + vbNewLine + _
                                     "showmotd: Shows message of the day set by user or kernel" + vbNewLine + _
                                     "showtd: Shows date and time" + vbNewLine + _
                                     "shutdown: The kernel will be shut down" + vbNewLine + _
                                     "sysinfo: System information" + vbNewLine + _
                                     "version: Shows kernel version")

        ElseIf (requestedCommand = "adduser") Then

            If (answeruser = "useradd" Or answeruser = "root") Then

                'We verified that the username is useradd or root, executing command
                System.Console.Write("Write username: ")
                System.Console.ForegroundColor = ConsoleColor.White
                answernewuser = System.Console.ReadLine()
                System.Console.ResetColor()
                If InStr(answernewuser, " ") > 0 Then
                    System.Console.WriteLine("Spaces are not allowed.")
                ElseIf (answernewuser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                    System.Console.WriteLine("Special characters are not allowed.")
                Else
                    System.Console.Write("Write password: ")
                    System.Console.ForegroundColor = ConsoleColor.White
                    answerpassword = System.Console.ReadLine()
                    System.Console.ResetColor()
                    'TODO: Implement password confirmation
                    adduser(answernewuser, answerpassword)
                    userword.Add(answernewuser, answerpassword)
                End If

            Else

                System.Console.WriteLine("You don't have permission to use this command")

            End If

        ElseIf (requestedCommand = "annoying-sound" Or requestedCommand = "beep") Then

            'Beep system initialization
            On Error GoTo panic
            BeepFreq()
            Exit Sub
panic:
            KernelError("U", True, 5, Err.Description)

        ElseIf (requestedCommand = "cdir" Or requestedCommand = "currentdir") Then

            'Current directory
            System.Console.WriteLine("Current directory: /kernel")

        ElseIf (requestedCommand = "changedir" Or requestedCommand = "chdir") Then

            'Not implemented yet
            System.Console.WriteLine("This kernel is not final.")

        ElseIf (requestedCommand = "chhostname") Then

            If (answeruser = "root") Then
                'Change host-name to custom name
                System.Console.Write("Write a new host name: ")
                Dim newhost As String
                System.Console.ForegroundColor = ConsoleColor.White
                newhost = System.Console.ReadLine()
                System.Console.ResetColor()
                If (newhost = "") Then
                    System.Console.WriteLine("Blank host name.")
                ElseIf (newhost.Length <= 3) Then
                    System.Console.WriteLine("The host name length must be at least 4 characters.")
                ElseIf InStr(newhost, " ") > 0 Then
                    System.Console.WriteLine("Spaces are not allowed.")
                ElseIf (newhost.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                    System.Console.WriteLine("Special characters are not allowed.")
                Else
                    System.Console.Write("Changing from: ")
                    System.Console.ForegroundColor = ConsoleColor.White
                    System.Console.Write(My.Settings.HostName)
                    System.Console.ResetColor()
                    System.Console.Write(" to ")
                    System.Console.ForegroundColor = ConsoleColor.White
                    System.Console.Write(newhost)
                    System.Console.ResetColor()
                    System.Console.WriteLine("...")
                    My.Settings.HostName = newhost
                End If
            Else
                System.Console.WriteLine("You don't have permission to use this command")
            End If

        ElseIf (requestedCommand = "chmotd") Then

            If (answeruser = "root") Then
                'New message of the day
                System.Console.Write("Write a new Message Of The Day: ")
                Dim newmotd As String
                System.Console.ForegroundColor = ConsoleColor.White
                newmotd = System.Console.ReadLine()
                System.Console.ResetColor()
                If (newmotd = "") Then
                    System.Console.WriteLine("Blank message of the day.")       'Fixed the message being wrong
                Else
                    System.Console.Write("Changing MOTD...")
                    My.Settings.MOTD = newmotd
                    System.Console.WriteLine(" Done!" + vbNewLine + "Please log-out, or use 'showmotd' to see the changes")
                End If
            Else
                System.Console.WriteLine("You don't have permission to use this command")
            End If

        ElseIf (requestedCommand = "choice") Then

            'Prompts user to write a question then the user will write choice sets.
            System.Console.Write("Write a question: ")
            Dim question As String
            System.Console.ForegroundColor = ConsoleColor.White
            question = System.Console.ReadLine()
            System.Console.ResetColor()
            If (question = "") Then
                System.Console.WriteLine("Blank question. Try again.")
            Else
                System.Console.Write("Write choice sets, Ex. Y/N/M/D/F/...: ")
                Dim sets As String
                System.Console.ForegroundColor = ConsoleColor.White
                sets = System.Console.ReadLine()
                System.Console.ResetColor()
                If (sets = "") Then
                    System.Console.WriteLine("Blank choice sets. Try again.")
                ElseIf Not (sets.Contains("/")) And Not (sets.Length - 1 = 0) Then
                    System.Console.WriteLine("Cease using choice sets that is, Ex. YNMDF, Y,N,M,D,F, etc.")
                ElseIf (sets.Length - 1 = 0) Then
                    System.Console.WriteLine("One choice set. Try again.")
                Else
                    System.Console.Write("{0} <{1}> ", question, sets)
                    Dim answerchoice As String
                    System.Console.ForegroundColor = ConsoleColor.White
                    answerchoice = System.Console.ReadKey.KeyChar
                    System.Console.ResetColor()
                    Dim answerchoices() As String = sets.Split("/")
                    For Each choiceset In answerchoices
                        If (answerchoice = choiceset) Then
                            System.Console.Write(vbNewLine + "Choice {0} selected.", answerchoice)
                        End If
                    Next
                    System.Console.WriteLine()
                End If
            End If

        ElseIf (requestedCommand = "chpwd") Then

            changePassword()

        ElseIf (requestedCommand = "disco") Then

            'The disco system.
            DiscoSystem()

        ElseIf (requestedCommand = "echo") Then

            'Prints string
            System.Console.Write("Write any text: ")
            System.Console.ForegroundColor = ConsoleColor.White
            answerecho = System.Console.ReadLine()
            System.Console.ResetColor()
            System.Console.WriteLine(answerecho)

        ElseIf (requestedCommand = "future-eyes-destroyer" Or requestedCommand = "fed") Then

            'Disco system, in monochrome
            DiscoSystem(True)

        ElseIf (requestedCommand = "hwprobe") Then

            HardwareProbe.ProbeHW()

        ElseIf (requestedCommand = "ls" Or requestedCommand = "list") Then

            'Lists folders and files (not final)
            System.Console.ForegroundColor = ConsoleColor.DarkGreen
            System.Console.Write("boot         bin         ")
            System.Console.ForegroundColor = ConsoleColor.Green
            System.Console.Write("dev")
            System.Console.ForegroundColor = ConsoleColor.DarkGreen
            System.Console.Write("         etc         lib         ")
            System.Console.ForegroundColor = ConsoleColor.Green
            System.Console.WriteLine("proc" + vbNewLine + "usr          var")
            System.Console.ResetColor()
            System.Console.WriteLine(vbNewLine + "Total number: 8 folders, 0 files")

        ElseIf (requestedCommand = "logout") Then

            'Logs out of the user
            LoginPrompt()

        ElseIf (requestedCommand = "lsdrivers") Then

            'List probed drivers in the boot time
            System.Console.ForegroundColor = ConsoleColor.White
            System.Console.WriteLine("CPU: {0} | Clock Speed: {1}MHz", Cpuname, Cpuspeed)
            System.Console.WriteLine("RAM: {0}", SysMem)
            System.Console.WriteLine("HDD: {0} | Size: {1}GB", Hddmodel, FormatNumber(Hddsize, 2))
            System.Console.ResetColor()

        ElseIf (requestedCommand = "lsnet") Then

            NetworkList.ListHostsInNetwork()

        ElseIf (requestedCommand = "lsnettree") Then

            NetworkList.ListHostsInTree()

        ElseIf (requestedCommand = "ping") Then

            Network.CheckNetworkCommand()

        ElseIf (requestedCommand = "read") Then

            'Not implemented yet
            System.Console.WriteLine("This kernel is not final.")

        ElseIf (requestedCommand = "reboot") Then

            'Reboot the simulated system
            System.Console.WriteLine("Rebooting...")
            System.Console.Beep(870, 250)
            Sleep(3000)
            System.Console.Clear()
            Main()

        ElseIf (requestedCommand = "showtd") Then

            TimeDate.ShowTime()

        ElseIf (requestedCommand = "showmotd") Then

            'Show changes to MOTD, or current
            System.Console.WriteLine(My.Settings.MOTD)

        ElseIf (requestedCommand = "shutdown") Then

            'Shuts down the simulated system
            System.Console.WriteLine("Shutting down...")
            System.Console.Beep(870, 250)
            Sleep(3000)
            System.Console.Clear()
            Environment.Exit(0)

        ElseIf (requestedCommand = "sysinfo") Then

            'Shows system information
            System.Console.WriteLine("Kernel Version: {0}", KernelVersion)
            System.Console.WriteLine("Shell (uesh) version: {0}", ueshversion)
            System.Console.WriteLine("Networking: 0.0.1.0")
            System.Console.WriteLine(vbNewLine + "Look at hardware information using 'lsdrivers'")

        ElseIf (requestedCommand = "version") Then

            'Shows current kernel version
            System.Console.WriteLine("Version: {0}", KernelVersion)

            End If

    End Sub

End Module