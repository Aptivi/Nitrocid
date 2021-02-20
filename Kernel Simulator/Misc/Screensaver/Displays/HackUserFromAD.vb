
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
Imports System.Threading

Module HackUserFromADDisplay

    Public WithEvents HackUserFromAD As New BackgroundWorker

    ''' <summary>
    ''' Handles the code of Hack User from AD simulator
    ''' </summary>
    Sub HackUserFromAD_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles HackUserFromAD.DoWork
        Do While True
            Thread.Sleep(1000)
IFCANCEL:
            If HackUserFromAD.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Hacking Simulator for Active Domain users screensaver stopped.")
                Exit Do
            Else
                Console.Clear()
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("Microsoft Windows [Version 10.0.18362.449]" + vbNewLine +
                                  "(c) 2019 Microsoft Corporation. All rights reserved." + vbNewLine)
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(3000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("net user /domain", False, 100)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(200)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine()
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(50)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("The request will be processed at a domain controller for domain Community.Workspace." + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(2000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("User accounts for \\LOGON-HANDLER" + vbNewLine + vbNewLine +
                                  "-------------------------------------------------------------------------------" + vbNewLine +
                                  "Administrator            CommAdmin                EnterpriseManager" + vbNewLine +
                                  "Guest                    UserEmployees            Work" + vbNewLine +
                                  "The command completed successfully." + vbNewLine + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(1000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("net user EnterpriseManager /domain", False, 125)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(325)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine()
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(50)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("The request will be processed at a domain controller for domain Community.Workspace." + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(2000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("User name                    EnterpriseManager" + vbNewLine +
                                  "Full Name                    Enterprise Manager" + vbNewLine +
                                  "Comment                      Only usable by IT experts" + vbNewLine +
                                  "User's comment" + vbNewLine +
                                  "Country/region code          000 (System Default)" + vbNewLine +
                                  "Account active               Yes" + vbNewLine +
                                  "Account expires              Never" + vbNewLine + vbNewLine +
                                 $"Password last set            {FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.LongTime)}" + vbNewLine +
                                  "Password expires             Never" + vbNewLine +
                                 $"Password changeable          ‎{FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.LongTime)}" + vbNewLine +
                                  "Password required            No" + vbNewLine +
                                  "User may change password     Yes" + vbNewLine +
                                  "Workstations allowed         All" + vbNewLine + vbNewLine +
                                  "Logon script" + vbNewLine +
                                  "User profile" + vbNewLine +
                                  "Home directory               Z:\ENT\Private\EM" + vbNewLine +
                                  "Last logon                   ‎2/27/‎2018 5:05:41 PM" + vbNewLine + vbNewLine +
                                  "Logon hours allowed          All" + vbNewLine + vbNewLine +
                                  "Local Group Memberships      *None" + vbNewLine +
                                  "Global Group memberships     *Administrators" + vbNewLine +
                                  "The command completed successfully." + vbNewLine + vbNewLine)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(1000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("ntlm.py dump --user=EnterpriseManager --domain=Community.Workspace", False, 85)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(130)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Dumping NTLM Hash...")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(4000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Dump completed, and saved in ./Hash.txt")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(1000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("ntlm.py decrypt Hash.txt", False, 160)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(215)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Trying to decrypt...")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(2150)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Decryption complete. Plain-text password retrieved in ./Pass.txt")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write(CurrDir + ">")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(1000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("start.py --user=EnterpriseManager --pass=`write Pass.txt` ""start""", False, 130)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(115)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Microsoft Windows [Version 10.0.18362.449]" + vbNewLine +
                                  "(c) 2019 Microsoft Corporation. All rights reserved." + vbNewLine)
                Console.Write("Z:\ENT\Private\EM>")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(3000)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("shutdown /s /fw /t 00 /m \\LOGON-HANDLER", False, 130)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(115)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("LOGON-HANDLER: System shutdown is initiated.")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(115)
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Console.Write("Z:\ENT\Private\EM>")
                If HackUserFromAD.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(5000)
            End If
        Loop
    End Sub

End Module
