
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

Module ArgumentParse

    'Variables
    Public arguser As String                    'A username
    Public argword As String                    'A password
    Public argcommands As String                'Commands entered
    Public argcmds() As String

    Sub ParseArguments()

        'Check for the arguments written by the user
        Try
            BootArgs = answerargs.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
            For i As Integer = 0 To BootArgs.Count - 1
                Dim indexArg As Integer = BootArgs(i).IndexOf(" ")
                If (indexArg = -1) Then
                    indexArg = BootArgs(i).Count
                    BootArgs(i) = BootArgs(i).Substring(0, indexArg)
                End If
                If (AvailableArgs.Contains(BootArgs(i).Substring(0, indexArg))) Then
                    If (BootArgs(i).Contains("motd")) Then

                        If (BootArgs(i) = "motd") Then
                            ChangeMessage()
                        Else
                            Dim newmotd = BootArgs(i).Substring(5)
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

                    ElseIf (BootArgs(i) = "nohwprobe") Then

                        'Disables automatic hardware probing.
                        ProbeFlag = False

                    ElseIf (BootArgs(i).Contains("chkn=1")) Then

                        'Makes a kernel check for connectivity on boot
                        Network.CheckNetworkKernel()

                    ElseIf (BootArgs(i) = "preadduser") Then

                        W("Write username: ", "input")
                        arguser = System.Console.ReadLine()
                        If InStr(arguser, " ") > 0 Then
                            Wln("Spaces are not allowed.", "neutralText")
                        ElseIf (arguser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                            Wln("Special characters are not allowed.", "neutralText")
                        ElseIf (arguser = "q") Then
                            Wln("Username creation has been cancelled.", "neutralText")
                        Else
                            W("Write password: ", "input")
                            argword = System.Console.ReadLine()
                            If InStr(argword, " ") > 0 Then
                                Wln("Spaces are not allowed.", "neutralText")
                            ElseIf (argword.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                                Wln("Special characters are not allowed.", "neutralText")
                            ElseIf (argword = "q") Then
                                Wln("Username creation has been cancelled.", "neutralText")
                            Else
                                W("Confirm: ", "input")
                                Dim answerpasswordconfirm As String = System.Console.ReadLine()
                                If InStr(answerpasswordconfirm, " ") > 0 Then
                                    Wln("Spaces are not allowed.", "neutralText")
                                ElseIf (answerpasswordconfirm.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                                    Wln("Special characters are not allowed.", "neutralText")
                                ElseIf (argword = answerpasswordconfirm) Then
                                    CruserFlag = True
                                ElseIf (argword <> answerpasswordconfirm) Then
                                    Wln("Password doesn't match.", "neutralText")
                                ElseIf (answerpasswordconfirm = "q") Then
                                    Wln("Username creation has been cancelled.", "neutralText")
                                End If
                            End If
                        End If


                    ElseIf (BootArgs(i).Contains("hostname")) Then

                        If (BootArgs(i) = "hostname") Then
                            HostName.ChangeHostName()
                        Else
                            Dim newhost As String = BootArgs(i).Substring(9)
                            If (newhost = "") Then
                                Wln("Blank host name.", "neutralText")
                            ElseIf (newhost.Length <= 3) Then
                                Wln("The host name length must be at least 4 characters.", "neutralText")
                            ElseIf InStr(newhost, " ") > 0 Then
                                Wln("Spaces are not allowed.", "neutralText")
                            ElseIf (newhost.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                                Wln("Special characters are not allowed.", "neutralText")
                            ElseIf (newhost = "q") Then
                                Wln("Host name changing has been cancelled.", "neutralText")
                            Else
                                Wln("Changing from: {0} to {1}...", "neutralText", My.Settings.HostName, newhost)
                                My.Settings.HostName = newhost
                            End If
                        End If

                    ElseIf (BootArgs(i) = "quiet") Then

                        Quiet = True

                    ElseIf (BootArgs(i) = "gpuprobe") Then

                        GPUProbeFlag = True

                    ElseIf (BootArgs(i).Contains("cmdinject")) Then

                        'Command Injector argument
                        If (BootArgs(i) = "cmdinject") Then
                            W("Available commands: {0}" + vbNewLine + "Write command: ", "input", String.Join(", ", availableCommands))
                            argcmds = System.Console.ReadLine().Split({":"c}, StringSplitOptions.RemoveEmptyEntries)
                            argcommands = String.Join(", ", argcmds)
                            If (argcommands <> "q") Then
                                CommandFlag = True
                            Else
                                Wln("Command injection has been cancelled.", "neutralText")
                            End If
                        Else
                            argcmds = BootArgs(i).Substring(10).Split({":"c}, StringSplitOptions.RemoveEmptyEntries)
                            argcommands = String.Join(", ", argcmds)
                            CommandFlag = True
                        End If

                    ElseIf (BootArgs(i) = "debug") Then

                        DebugMode = True
                        dbgWriter.AutoFlush = True

                    ElseIf (BootArgs(i) = "help") Then

                        Wln("Separate boot arguments with commas without spaces, for example, 'motd,gpuprobe'" + vbNewLine + _
                            "Separate commands on 'cmdinject' with colons without spaces, for example, 'cmdinject setthemes Hacker:beep 1024 0.5'" + vbNewLine + _
                            "Note that the 'debug' argument does not fully cover the kernel.", "neutralText")
                        answerargs = "" : argsFlag = False : argsInjected = False
                        ArgumentPrompt.PromptArgs()
                        If (argsFlag = True) Then
                            ArgumentParse.ParseArguments()
                        End If

                    End If
                Else
                    Wln("bargs: The requested argument {0} is not found.", "neutralText", BootArgs(i).Substring(0, indexArg))
                End If
            Next
        Catch ex As Exception
            KernelError(CChar("U"), True, 5, "bargs: Unrecoverable error in argument: " + Err.Description)
        End Try

    End Sub

End Module
