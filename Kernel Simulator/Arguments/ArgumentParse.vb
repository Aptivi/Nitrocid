
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
    Public CruserFlag As Boolean = False        'A signal to the kernel where user has to be created

    Sub ParseArguments()

        'Error Handler
        On Error GoTo ErrorPanic

        'Check for the arguments written by the user
        BootArgs = answerargs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        For i As Integer = 0 To BootArgs.Count - 1

            If (AvailableArgs.Contains(BootArgs(i))) Then

                If (BootArgs(i) = "motd") Then

                    ChangeMessage()

                ElseIf (BootArgs(i) = "nohwprobe") Then

                    'Disables automatic hardware probing.
                    ProbeFlag = False

                ElseIf (BootArgs(i) = "chkn=1") Then

                    'Makes a kernel check for connectivity on boot
                    Network.CheckNetworkKernel()

                ElseIf (BootArgs(i) = "preadduser") Then

                    'Prompts for new username
                    System.Console.Write("Write username: ")
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    arguser = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If InStr(arguser, " ") > 0 Then
                        System.Console.WriteLine("Spaces are not allowed.")
                    ElseIf (arguser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                        System.Console.WriteLine("Special characters are not allowed.")
                    ElseIf (arguser = "q") Then
                        System.Console.WriteLine("Username creation has been cancelled.")
                    Else
                        System.Console.Write("Write password: ")
                        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                        argword = System.Console.ReadLine()
                        System.Console.ResetColor()
                        If InStr(argword, " ") > 0 Then
                            System.Console.WriteLine("Spaces are not allowed.")
                        ElseIf (argword.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                            System.Console.WriteLine("Special characters are not allowed.")
                        ElseIf (argword = "q") Then
                            System.Console.WriteLine("Username creation has been cancelled.")
                        Else
                            System.Console.Write("Confirm: ")
                            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                            Dim answerpasswordconfirm As String = System.Console.ReadLine()
                            System.Console.ResetColor()
                            If InStr(answerpasswordconfirm, " ") > 0 Then
                                System.Console.WriteLine("Spaces are not allowed.")
                            ElseIf (answerpasswordconfirm.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                                System.Console.WriteLine("Special characters are not allowed.")
                            ElseIf (argword = answerpasswordconfirm) Then
                                CruserFlag = True
                            ElseIf (argword <> answerpasswordconfirm) Then
                                System.Console.WriteLine("Password doesn't match.")
                            ElseIf (answerpasswordconfirm = "q") Then
                                System.Console.WriteLine("Username creation has been cancelled.")
                            End If
                        End If
                    End If

                ElseIf (BootArgs(i) = "hostname") Then

                    HostName.ChangeHostName()

                ElseIf (BootArgs(i) = "quiet") Then

                    Quiet = True

                ElseIf (BootArgs(i) = "gpuprobe") Then

                    GPUProbeFlag = True

                ElseIf (BootArgs(i) = "cmdinject") Then

                    'Command Injector argument
                    System.Console.Write("Available commands: {0}" + vbNewLine + "Write command: ", String.Join(", ", availableCommands))
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    argcommands = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If (argcommands <> "q") Then
                        CommandFlag = True
                    Else
                        System.Console.WriteLine("Command injection has been cancelled.")
                    End If

                End If

            Else

                System.Console.WriteLine("bargs: The requested argument {0} is not found.", BootArgs(i))

            End If
        Next
        Exit Sub
ErrorPanic:
        KernelError(CChar("U"), True, 5, "bargs: Unrecoverable error in argument: " + Err.Description)

    End Sub

End Module
