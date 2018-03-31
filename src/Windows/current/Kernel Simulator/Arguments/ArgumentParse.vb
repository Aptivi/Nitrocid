
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
    Public CruserFlag As Boolean = False        'A signal to the kernel where user has to be created

    Sub ParseArguments()

        'Check for the arguments written by the user
        BootArgs = answerargs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        For i As Integer = 0 To BootArgs.Count - 1
            If (AvailableArgs.Contains(BootArgs(i))) Then
                If (BootArgs(i) = "motd") Then
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
                        System.Console.WriteLine(" Done!")
                    End If
                ElseIf (BootArgs(i) = "nohwprobe") Then
                    'Disables automatic hardware probing.
                    ProbeFlag = False
                ElseIf (BootArgs(i) = "chkn=1") Then
                    'Makes a kernel check for connectivity on boot
                    Network.CheckNetworkKernel()
                ElseIf (BootArgs(i) = "preadduser") Then
                    'Prompts for new username
                    System.Console.Write("Write username: ")
                    System.Console.ForegroundColor = ConsoleColor.White
                    arguser = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If InStr(arguser, " ") > 0 Then
                        System.Console.WriteLine("Spaces are not allowed.")
                    ElseIf (arguser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                        System.Console.WriteLine("Special characters are not allowed.")
                    Else
                        System.Console.Write("Write password: ")
                        System.Console.ForegroundColor = ConsoleColor.White
                        argword = System.Console.ReadLine()
                        System.Console.ResetColor()
                        CruserFlag = True
                    End If
                ElseIf (BootArgs(i) = "hostname") Then
                    'Changes hostname
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
                ElseIf (BootArgs(i) = "quiet") Then
                    Quiet = True
                End If
            Else
                System.Console.WriteLine("bargs: The requested argument {0} is not found.", BootArgs(i))
            End If
        Next

    End Sub

End Module
