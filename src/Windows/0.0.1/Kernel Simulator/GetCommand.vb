
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

    Public answernewuser As String
    Public answerpassword As String
    Public answerbeep As String
    Public answerbeepms As String
    Public key As Integer
    Public colors() As ConsoleColor = ConsoleColor.GetValues(GetType(ConsoleColor))
    Public WithEvents backgroundWorker1 As New System.ComponentModel.BackgroundWorker
    Public answerecho As String
    Public WithEvents backgroundWorker2 As New System.ComponentModel.BackgroundWorker

    Sub BeepFreq()

        System.Console.Write(vbNewLine + "Beep Frequency in Hz that has the limit of 37-32767 Hz: ")
        answerbeep = System.Console.ReadLine()
        If Integer.TryParse(answerbeep, key) Then
            If (answerbeep <= 36 Or answerbeep >= 32768) Then
                System.Console.WriteLine("Invalid value for beep frequency.")
            ElseIf (answerbeep > 2048) Then
                System.Console.WriteLine(vbNewLine + "WARNING: Beep may be loud, depending on speaker. Setting values higher than 2048 might cause your ears to damage, " + _
                                                     "and more importantly, your motherboard speaker might deafen, or malfunction.")
                System.Console.Write(vbNewLine + "Are you sure that you want to beep at this frequency, " + answerbeep + "? (y/n)[N]")
                Dim answerrape = System.Console.ReadKey.KeyChar
                If (answerrape = "n" Or answerrape = "N" Or answerrape = "") Then
                    System.Console.WriteLine(vbNewLine + "High frequency. Please read documentation for more info why high frequency shouldn't be used.")
                ElseIf (answerrape = "y" Or answerrape = "Y") Then
                    BeepSystem()
                End If
            Else
                BeepSystem()
            End If
        End If

    End Sub
    Sub BeepSystem()

        System.Console.Write(vbNewLine + "Beep Time in seconds that has the limit of 1-3600: ")
        answerbeepms = System.Console.ReadLine()
        If Double.TryParse(answerbeepms, key) Then
            If (answerbeepms <= 0 Or answerbeepms >= 3601) Then
                System.Console.WriteLine("Invalid value for beep time.")
            Else
                System.Console.WriteLine("Beeping in " + answerbeepms + " seconds in " + answerbeep + " Hz...")
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

            System.Console.WriteLine(vbNewLine + "Help commands:" + vbNewLine + vbNewLine + _
                                     "adduser: Adds users (Only root or useradd user can access this command)" + vbNewLine + _
                                     "annoying-sound (Alias: beep): Console will beep in Hz and time in milliseconds" + vbNewLine + _
                                     "cdir (Alias: currentdir): Shows current directory" + vbNewLine + _
                                     "changedir (Alias: chdir): Changes directory (WARNING: EXPERIMENTAL)" + vbNewLine + _
                                     "chhostname: Changes host name (Root only)" + vbNewLine + _
                                     "chmotd: Changes MOTD, the Message Of The Day (Root only)" + vbNewLine + _
                                     "choice: Makes user choices" + vbNewLine + _
                                     "disco: A disco effect! (press ENTER to quit)" + vbNewLine + _
                                     "echo: Writes a text into a console" + vbNewLine + _
                                     "future-eyes-destroyer: Like disco, but black/white version." + vbNewLine + _
                                     "help: Help page" + vbNewLine + _
                                     "list (Alias: ls): List file/folder contents in current folder (WARNING: EXPERIMENTAL, some of the features are not implemented)" + vbNewLine + _
                                     "logout: Logs you out." + vbNewLine + _
                                     "lsdrivers: Lists drivers that is recognized by the kernel." + vbNewLine + _
                                     "read: Writes file contents to the console" + vbNewLine + _
                                     "reboot: Restarts your computer (WARNING: No syncing, because it is not a final kernel)" + vbNewLine + _
                                     "shutdown: The kernel will be shut down" + vbNewLine + _
                                     "version: Shows kernel version")

        ElseIf (requestedCommand = "adduser") Then

            If (answeruser = "useradd" Or answeruser = "root") Then

                System.Console.Write(vbNewLine + "Write username: ")
                answernewuser = System.Console.ReadLine()
                System.Console.Write("Write password: ")
                answerpassword = System.Console.ReadLine()
                adduser(answernewuser, answerpassword)
                userword.Add(answernewuser, answerpassword)

            Else

                System.Console.Write("You don't have permission to use this command" + vbNewLine)

            End If

        ElseIf (requestedCommand = "annoying-sound" Or requestedCommand = "beep") Then

            On Error GoTo panic
            BeepFreq()
            Exit Sub
panic:
            KernelError("U", True, 5, Err.Description)

        ElseIf (requestedCommand = "cdir" Or requestedCommand = "currentdir") Then

            System.Console.WriteLine("Current directory: /kernel")

        ElseIf (requestedCommand = "changedir" Or requestedCommand = "chdir") Then

            System.Console.WriteLine(vbNewLine + "This kernel is not final.")

        ElseIf (requestedCommand = "chhostname") Then

            System.Console.Write(vbNewLine + "Write a new host name: ")
            Dim newhost As String
            newhost = System.Console.ReadLine()
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

        ElseIf (requestedCommand = "chmotd") Then

            System.Console.Write(vbNewLine + "Write a new Message Of The Day: ")
            Dim newmotd As String
            newmotd = System.Console.ReadLine()
            If (newmotd = "") Then
                System.Console.WriteLine("Blank host name.")
            Else
                System.Console.Write("Changing MOTD...")
                My.Settings.MOTD = newmotd
                System.Console.Write(" Done!" + vbNewLine + "Please log-out for the changes to take effect.")
            End If

        ElseIf (requestedCommand = "choice") Then

            System.Console.Write(vbNewLine + "Write a question: ")
            Dim question As String
            question = System.Console.ReadLine()
            If (question = "") Then
                System.Console.WriteLine("Blank question. Try again.")
            Else
                System.Console.Write(vbNewLine + "Write choice sets, Ex. Y/N/M/D/F/...: ")
                Dim sets As String
                sets = System.Console.ReadLine()
                If (sets = "") Then
                    System.Console.WriteLine("Blank choice sets. Try again.")
                ElseIf Not (sets.Contains("/")) And Not (sets.Length - 1 = 0) Then
                    System.Console.WriteLine("Cease using choice sets that is, Ex. YNMDF, Y,N,M,D,F, etc.")
                ElseIf (sets.Length - 1 = 0) Then
                    System.Console.WriteLine("One choice set. Try again.")
                Else
                    System.Console.Write(question + " <" + sets + ">")
                    Dim answerchoice As String
                    answerchoice = System.Console.ReadKey.KeyChar
                    System.Console.Write(vbNewLine)
                    Dim answerchoices() As String = sets.Split("/")
                    For Each choiceset In answerchoices
                        If (answerchoice = choiceset) Then
                            System.Console.WriteLine("Choice " + answerchoice + " selected.")
                        End If
                    Next
                End If
            End If

        ElseIf (requestedCommand = "disco") Then

            DiscoSystem()

        ElseIf (requestedCommand = "echo") Then

            System.Console.Write("Write any text: ")
            answerecho = System.Console.ReadLine()
            System.Console.WriteLine(answerecho)

        ElseIf (requestedCommand = "future-eyes-destroyer") Then

            DiscoSystem(True)

        ElseIf (requestedCommand = "ls" Or requestedCommand = "list") Then

            System.Console.ForegroundColor = ConsoleColor.DarkGreen
            System.Console.Write(vbNewLine + "boot         bin         ")
            System.Console.ForegroundColor = ConsoleColor.Green
            System.Console.Write("dev")
            System.Console.ForegroundColor = ConsoleColor.DarkGreen
            System.Console.Write("         etc         lib         ")
            System.Console.ForegroundColor = ConsoleColor.Green
            System.Console.WriteLine("proc" + vbNewLine + _
                                 "usr          var")
            System.Console.ResetColor()
            System.Console.WriteLine(vbNewLine + "Total number: 8 folders, 0 files")

        ElseIf (requestedCommand = "logout") Then

            LoginPrompt()

        ElseIf (requestedCommand = "lsdrivers") Then

            System.Console.ForegroundColor = ConsoleColor.White
            System.Console.WriteLine(vbNewLine + "CPU: " + Cpuname + " | Clock Speed: " + Cpuspeed + "MHz")
            System.Console.WriteLine("RAM: " + SysMem)
            System.Console.WriteLine("HDD: " + Hddmodel + " | Size: " + FormatNumber(Hddsize, 2) + "GB")
            System.Console.ResetColor()

        ElseIf (requestedCommand = "read") Then

            System.Console.WriteLine(vbNewLine + "This kernel is not final.")

        ElseIf (requestedCommand = "reboot") Then

            System.Console.WriteLine(vbNewLine + "Rebooting...")
            System.Console.Beep(870, 250)
            Sleep(3000)
            System.Console.Clear()
            Main()

        ElseIf (requestedCommand = "shutdown") Then

            System.Console.WriteLine(vbNewLine + "Shutting down...")
            System.Console.Beep(870, 250)
            Sleep(3000)
            System.Console.Clear()
            Environment.Exit(0)

        ElseIf (requestedCommand = "version") Then

            System.Console.WriteLine(vbNewLine + "Version: " + KernelVersion)

        End If

    End Sub

End Module
