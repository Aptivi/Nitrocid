
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
    Public answernewuser As String                                                                          'Input for new user name
    Public answerpassword As String                                                                         'Input for new password
    Public answerbeep As String                                                                             'Input for beep frequency
    Public answerbeepms As String                                                                           'Input for beep milliseconds
    Public key As Double
    Public colors() As ConsoleColor = CType(ConsoleColor.GetValues(GetType(ConsoleColor)), ConsoleColor())  'Console Colors
    Public WithEvents backgroundWorker1 As New System.ComponentModel.BackgroundWorker                       'Black / White disco
    Public answerecho As String                                                                             'Input for printing string
    Public WithEvents backgroundWorker2 As New System.ComponentModel.BackgroundWorker                       '16-bit Colored Disco

    Sub BeepFreq()

        System.Console.Write("Beep Frequency in Hz that has the limit of 37-32767 Hz: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        answerbeep = System.Console.ReadLine()
        System.Console.ResetColor()
        If (CDbl(answerbeep) > Int32.MaxValue) Then
            System.Console.WriteLine("Integer overflow on frequency.")
        ElseIf (answerbeep = "q") Then
            System.Console.WriteLine("Computer beeping has been cancelled.")
        Else
            If Integer.TryParse(answerbeep, CInt(key)) Then
                If (CDbl(answerbeep) <= 36 Or CDbl(answerbeep) >= 32768) Then
                    System.Console.WriteLine("Invalid value for beep frequency.")
                ElseIf (CDbl(answerbeep) > 2048) Then
                    System.Console.WriteLine("WARNING: Beep may be loud, depending on speaker. Setting values higher than 2048 might cause your ears to damage, " + _
                                             "and more importantly, your motherboard speaker might deafen, or malfunction.")
                    System.Console.Write("Are you sure that you want to beep at this frequency, {0}? (y/N) ", answerbeep)
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Dim answerrape = System.Console.ReadKey.KeyChar
                    System.Console.ResetColor()
                    If (answerrape = "n" Or answerrape = "N" Or answerrape = "" Or answerrape = "q") Then
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
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        answerbeepms = System.Console.ReadLine()
        System.Console.ResetColor()
        If Double.TryParse(answerbeepms, key) Then
            If (CDbl(answerbeepms) <= 0 Or CDbl(answerbeepms) >= 3601) Then
                System.Console.WriteLine("Invalid value for beep time.")
            ElseIf (answerbeepms = "q") Then
                System.Console.WriteLine("Computer beeping has been cancelled.")
            Else
                System.Console.WriteLine("Beeping in {0} seconds in {1} Hz...", answerbeepms, answerbeep)
                System.Console.Beep(CInt(answerbeep), CInt(CDbl(answerbeepms) * 1000))
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

        If (requestedCommand = "help") Then

            'Shows available commands
            System.Console.WriteLine("Help commands:" + vbNewLine + vbNewLine + _
                                     "adduser: Adds users (Only admins can access this command)" + vbNewLine + _
                                     "addperm: Adds permissions for users (Only admins can access this command)" + vbNewLine + _
                                     "annoying-sound (Alias: beep): Console will beep in Hz and time in milliseconds" + vbNewLine + _
                                     "arginj: Injects arguments to the kernel (reboot required, admins only)" + vbNewLine + _
                                     "cdir (Alias: currentdir): Shows current directory" + vbNewLine + _
                                     "changedir (Alias: chdir): Changes directory (WARNING: EXPERIMENTAL)" + vbNewLine + _
                                     "chhostname: Changes host name (Admins only)" + vbNewLine + _
                                     "chmotd: Changes MOTD, the Message Of The Day (Admins only)" + vbNewLine + _
                                     "choice: Makes user choices" + vbNewLine + _
                                     "chpwd: Changes password for current user" + vbNewLine + _
                                     "chusrname: Changes user name" + vbNewLine + _
                                     "cls: Clears the screen" + vbNewLine + _
                                     "disco: A disco effect! (press ENTER to quit)" + vbNewLine + _
                                     "echo: Writes a text into a console" + vbNewLine + _
                                     "editperm: Edit permissions for user" + vbNewLine + _
                                     "future-eyes-destroyer (Alias: fed): Like disco, but black/white version." + vbNewLine + _
                                     "help: Help page" + vbNewLine + _
                                     "hwprobe: Probe hardware manually (One time in 'nohwprobe' kernel)" + vbNewLine + _
                                     "list (Alias: ls): List file/folder contents in current folder (WARNING: EXPERIMENTAL)" + vbNewLine + _
                                     "logout: Logs you out." + vbNewLine + _
                                     "lsdrivers: Lists drivers that is recognized by the kernel." + vbNewLine + _
                                     "lsnet: Lists all network addresses on host" + vbNewLine + _
                                     "lsnettree: Lists all network addresses on host using the tree" + vbNewLine + _
                                     "panicsim: Kernel Panic Simulator (real)" + vbNewLine + _
                                     "ping: Check to see if specified address is available" + vbNewLine + _
                                     "read: Writes file contents to the console" + vbNewLine + _
                                     "reboot: Restarts your computer (WARNING: No syncing, because it is not a final kernel)" + vbNewLine + _
                                     "rmuser: Removes a user from the list" + vbNewLine + _
                                     "showmotd: Shows message of the day set by user or kernel" + vbNewLine + _
                                     "showtd: Shows date and time" + vbNewLine + _
                                     "shutdown: The kernel will be shut down" + vbNewLine + _
                                     "sysinfo: System information" + vbNewLine + _
                                     "version: Shows kernel version")

        ElseIf (requestedCommand = "adduser") Then

            UserManagement.addUser()
            System.Console.WriteLine("Tip: You can add permissions to new users by using 'addperm' and then writing their username." + vbNewLine + _
                                     "     You can also edit permissions for existing usernames by using 'editperm'.")

        ElseIf (requestedCommand = "addperm") Then

            Groups.permissionPrompt()

        ElseIf (requestedCommand = "annoying-sound" Or requestedCommand = "beep") Then

            'Beep system initialization
            On Error GoTo panic
            BeepFreq()
            Exit Sub
panic:
            KernelError(CChar("U"), True, 5, Err.Description)

        ElseIf (requestedCommand = "arginj") Then

            'Argument Injection
            answerargs = ""
            ArgumentPrompt.PromptArgs(True)

        ElseIf (requestedCommand = "cdir" Or requestedCommand = "currentdir") Then

            'Current directory
            System.Console.WriteLine("Current directory: /kernel")

        ElseIf (requestedCommand = "changedir" Or requestedCommand = "chdir") Then

            'Not implemented yet
            System.Console.WriteLine("This kernel is not final.")

        ElseIf (requestedCommand = "chhostname") Then

            HostName.ChangeHostName()

        ElseIf (requestedCommand = "chmotd") Then

            ChangeMessage()

        ElseIf (requestedCommand = "choice") Then

            'Prompts user to write a question then the user will write choice sets.
            System.Console.Write("Write a question: ")
            Dim question As String
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            question = System.Console.ReadLine()
            System.Console.ResetColor()
            If (question = "") Then
                System.Console.WriteLine("Blank question. Try again.")
            ElseIf (question = "q") Then
                System.Console.WriteLine("Choice creation has been cancelled.")
            Else
                System.Console.Write("Write choice sets, Ex. Y/N/M/D/F/...: ")
                Dim sets As String
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                sets = System.Console.ReadLine()
                System.Console.ResetColor()
                If (sets = "") Then
                    System.Console.WriteLine("Blank choice sets. Try again.")
                ElseIf Not (sets.Contains("/")) And Not (sets.Length - 1 = 0) Then
                    System.Console.WriteLine("Cease using choice sets that is, Ex. YNMDF, Y,N,M,D,F, etc.")
                ElseIf (sets.Length - 1 = 0) Then
                    System.Console.WriteLine("One choice set. Try again.")
                ElseIf (sets = "q") Then
                    System.Console.WriteLine("Choice creation has been cancelled.")
                Else
                    System.Console.Write("{0} <{1}> ", question, sets)
                    Dim answerchoice As String
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    answerchoice = System.Console.ReadKey.KeyChar
                    System.Console.ResetColor()
                    Dim answerchoices() As String = sets.Split(CChar("/"))
                    For Each choiceset In answerchoices
                        If (answerchoice = choiceset) Then
                            System.Console.Write(vbNewLine + "Choice {0} selected.", answerchoice)
                        ElseIf (answerchoice = "q") Then
                            System.Console.WriteLine("Choice has been cancelled.")
                        End If
                    Next
                    System.Console.WriteLine()
                End If
            End If

        ElseIf (requestedCommand = "chpwd") Then

            changePassword()

        ElseIf (requestedCommand = "chusrname") Then

            UserManagement.changeName()

        ElseIf (requestedCommand = "cls") Then

            System.Console.Clear()

        ElseIf (requestedCommand = "disco") Then

            'The disco system.
            DiscoSystem()

        ElseIf (requestedCommand = "echo") Then

            'Prints string
            System.Console.Write("Write any text: ")
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            answerecho = System.Console.ReadLine()
            System.Console.ResetColor()
            If (answerecho = "q") Then
                System.Console.WriteLine("Text printing has been cancelled.")
            Else
                System.Console.WriteLine(answerecho)
            End If

        ElseIf (requestedCommand = "editperm") Then

            Groups.permissionEditingPrompt()

        ElseIf (requestedCommand = "future-eyes-destroyer" Or requestedCommand = "fed") Then

            'Disco system, in monochrome
            DiscoSystem(True)

        ElseIf (requestedCommand = "hwprobe") Then

            HardwareProbe.ProbeHW()

        ElseIf (requestedCommand = "ls" Or requestedCommand = "list") Then

            'Lists folders and files (not final, unreal)
            System.Console.Write("Write a folder: ")
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            Dim lsdir As String = System.Console.ReadLine()
            System.Console.ResetColor()
            If (lsdir = "" Or lsdir = "." Or lsdir = "/" Or lsdir = "./") Then
                System.Console.WriteLine(String.Join(", ", AvailableDirs))
            ElseIf (lsdir = "q") Then
                System.Console.WriteLine("Listing has been cancelled.")
            ElseIf (AvailableDirs.Contains(lsdir)) Then
                ListFolders.list(lsdir)
            Else
                System.Console.WriteLine("{0} is not found.", lsdir)
            End If

        ElseIf (requestedCommand = "logout") Then

            'Logs out of the user
            LoginPrompt()

        ElseIf (requestedCommand = "lsdrivers") Then

            'List probed drivers in the boot time
            HardwareProbe.ListDrivers()

        ElseIf (requestedCommand = "lsnet") Then

            NetworkList.GetNetworkComputers()
            NetworkList.ListHostsInNetwork()

        ElseIf (requestedCommand = "lsnettree") Then

            NetworkList.GetNetworkComputers()
            NetworkList.ListHostsInTree()

        ElseIf (requestedCommand = "panicsim") Then

            'Kernel panic simulator
            On Error Resume Next
            System.Console.Write("Write a message: ")
            Dim kpmsg As String
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            kpmsg = System.Console.ReadLine()
            System.Console.ResetColor()
            If (kpmsg = "") Then
                System.Console.WriteLine("Blank message.")
            ElseIf (kpmsg = "q") Then
                System.Console.WriteLine("Text printing has been cancelled.")
            Else
                System.Console.Write("Write error type: ")
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Dim kpet = System.Console.ReadKey.KeyChar
                System.Console.ResetColor()
                If (kpet = "") Then
                    System.Console.WriteLine(vbNewLine + "Blank error type")
                ElseIf (kpet = "q") Then
                    System.Console.WriteLine("Text printing has been cancelled.")
                ElseIf (kpet = "S" Or kpet = "U" Or kpet = "D" Or kpet = "F" Or kpet = "C") Then
                    System.Console.Write(vbNewLine + "Restart time in seconds: ")
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Dim kptime = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If (kptime = "") Then
                        System.Console.WriteLine("Blank time")
                    ElseIf (CDbl(kptime) <= 3600 And kpet <> "C") Then
                        KernelError(kpet, True, CLng(kptime), kpmsg)
                    ElseIf (CDbl(kptime) <= 3600 And kpet = "C" Or CDbl(kptime) <= 0 And kpet = "C") Then
                        KernelError(kpet, False, 0, kpmsg)
                    ElseIf (CDbl(kptime) <= 0 And kpet <> "C") Then
                        System.Console.WriteLine("Invalid time.")
                    ElseIf (kptime = "q") Then
                        System.Console.WriteLine("Text printing has been cancelled.")
                    End If
                Else
                    System.Console.WriteLine(vbNewLine + "Invalid error type")
                End If
            End If

        ElseIf (requestedCommand = "ping") Then

            Network.CheckNetworkCommand()

        ElseIf (requestedCommand = "read") Then

            'Reads file contents
            System.Console.Write("Write a file (directories will be scanned): ")
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            Dim readfile As String = System.Console.ReadLine()
            System.Console.ResetColor()
            If (readfile = "") Then
                System.Console.WriteLine(String.Join(", ", AvailableDirs))
            ElseIf (readfile = "q") Then
                System.Console.WriteLine("Listing has been cancelled.")
            ElseIf (AvailableFiles.Contains(readfile)) Then
                FileContents.readContents(readfile)
            Else
                System.Console.WriteLine("{0} is not found.", readfile)
            End If

        ElseIf (requestedCommand = "reboot") Then

            'Reboot the simulated system
            System.Console.WriteLine("Rebooting...")
            System.Console.Beep(870, 250)
            Kernel.ResetEverything()
            Sleep(3000)
            System.Console.Clear()
            Main()

        ElseIf (requestedCommand = "rmuser") Then

            UserManagement.removeUser()

        ElseIf (requestedCommand = "setcolors") Then

            System.Console.WriteLine("Available Colors: {0}" + vbNewLine + _
                                     "Press ENTER only on questions and defaults will be used.", _
                                     String.Join(", ", availableColors))
            ColorSet.SetColorSteps()

        ElseIf (requestedCommand = "showtd") Then

            TimeDate.ShowTime()

        ElseIf (requestedCommand = "showmotd") Then

            'Show changes to MOTD, or current
            System.Console.WriteLine(My.Settings.MOTD)

        ElseIf (requestedCommand = "shutdown") Then

            'Shuts down the simulated system
            System.Console.WriteLine("Shutting down...")
            System.Console.Beep(870, 250)
            Kernel.ResetEverything()
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