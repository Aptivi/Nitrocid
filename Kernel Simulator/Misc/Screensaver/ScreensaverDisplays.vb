
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

Imports System.ComponentModel
Imports System.Threading

Public Module ScreensaverDisplays

    Public WithEvents ColorMix As New BackgroundWorker
    Public WithEvents Matrix As New BackgroundWorker
    Public WithEvents Disco As New BackgroundWorker
    Public WithEvents Lines As New BackgroundWorker
    Public WithEvents GlitterMatrix As New BackgroundWorker
    Public WithEvents GlitterColor As New BackgroundWorker
    Public WithEvents AptErrorSim As New BackgroundWorker
    Public WithEvents HackUserFromAD As New BackgroundWorker
    Public WithEvents ColorMix255 As New BackgroundWorker
    Public WithEvents GlitterColor255 As New BackgroundWorker
    Public WithEvents Custom As New BackgroundWorker
    Public finalSaver As ICustomSaver

    Sub Custom_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Custom.DoWork
        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.W() with System.Console.WriteLine() or System.Console.Write().
        'TODO: Let screensaver developers set delay, and end the screensaver.
        Console.CursorVisible = False
        finalSaver.PreDisplay()
        Do While True
            If Not finalSaver.DelayForEachWrite = Nothing Then
                Thread.Sleep(finalSaver.DelayForEachWrite)
            End If
            If Custom.CancellationPending = True Then
                Wdbg("Cancellation requested. Showing ending...")
                finalSaver.PostDisplay()
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Custom screensaver stopped.")
                Exit Do
            Else
                finalSaver.ScrnSaver()
            End If
        Loop
    End Sub

    Sub ColorMix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim colorrand As New Random()
        Do While True
            Thread.Sleep(1)
            If ColorMix.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Mix Colors screensaver stopped.")
                Exit Do
            Else
                Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
            End If
        Loop
    End Sub

    Sub Matrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Matrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            If Matrix.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Matrix screensaver stopped.")
                Exit Do
            Else
                Thread.Sleep(1)
                Console.Write(CStr(random.Next(2)))
            End If
        Loop
    End Sub

    Sub Disco_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Disco.DoWork
        Console.CursorVisible = False
        Do While True
            For Each color In colors
                Thread.Sleep(100)
                If Disco.CancellationPending = True Then
                    Wdbg("Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = inputColor
                    Console.BackgroundColor = backgroundColor
                    Load()
                    Console.CursorVisible = True
                    Wdbg("All clean. Disco screensaver stopped.")
                    Exit Do
                Else
                    Console.BackgroundColor = color
                    Console.Clear()
                End If
            Next
        Loop
    End Sub

    Sub Lines_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Lines.DoWork
        Console.CursorVisible = False
        Wdbg("Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            For Each color In colors
                Thread.Sleep(1000)
                If Lines.CancellationPending = True Then
                    Wdbg("Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = inputColor
                    Console.BackgroundColor = backgroundColor
                    Load()
                    Console.CursorVisible = True
                    Wdbg("All clean. Lines screensaver stopped.")
                    Exit Do
                Else
                    Console.Clear()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = color
                    Dim Line As String = ""
                    Dim Top As Integer = New Random().Next(Console.WindowHeight)
                    For i As Integer = 1 To Console.WindowWidth
                        Line += "-"
                    Next
                    Console.SetCursorPosition(0, Top)
                    Console.WriteLine(Line)
                End If
            Next
        Loop
    End Sub

    Sub GlitterMatrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterMatrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If GlitterMatrix.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Glitter Matrix screensaver stopped.")
                Exit Do
            Else
                Thread.Sleep(1)
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                Console.SetCursorPosition(Left, Top)
                Console.Write(CStr(RandomDriver.Next(2)))
            End If
        Loop
    End Sub

    Sub AptErrorSim_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles AptErrorSim.DoWork
        Console.CursorVisible = False
        Do While True
            Thread.Sleep(100)
IFCANCEL:
            If AptErrorSim.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. apt Error Simulator screensaver stopped.")
                Exit Do
            Else
                Console.Clear()
                Console.Write("{0}@{1}:{2}$ ", HName, signedinusrnm, CurrDir)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(3000)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("sudo apt -y dist-upgrade", False, 100)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(200)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine()
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(50)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("Reading package lists... ")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(50)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.WriteLine("Building dependency tree")
                Console.Write("Reading state information... ")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(210)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.Write("Calculating upgrade...")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(80)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.WriteLine("The following packages will be upgraded:")
                Console.WriteLine("  libnvpair1linux libuutil1linux libzfs2linux libzpool2linux zfs-initramfs zfs-zed zfsutils-linux")
                Console.WriteLine("7 upgraded, 0 newly installed, 0 to remove and 0 not upgraded")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(300)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Need to get 1,510 kB of archives.")
                Console.WriteLine("After this operation, 155 kB of additional disk space will be used.")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(600)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("0% [Connecting to archive.ubuntu.com]")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(10000)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine(vbNewLine + "Err:1 http://archive.ubuntu.com/ubuntu focal/main amd64 zfs-initramfs amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:2 http://archive.ubuntu.com/ubuntu focal/main amd64 zfs-zed amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:3 http://archive.ubuntu.com/ubuntu focal/main amd64 zfsutils-linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:4 http://archive.ubuntu.com/ubuntu focal/main amd64 libnvpair1linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:5 http://archive.ubuntu.com/ubuntu focal/main amd64 libuutil1linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:6 http://archive.ubuntu.com/ubuntu focal/main amd64 libzfs2linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("Err:7 http://archive.ubuntu.com/ubuntu focal/main amd64 libzpool2linux amd64 0.8.2-3ubuntu1")
                Console.WriteLine("  Connection timed out [IP: 91.189.88.31 80]")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(100)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/z/zfs-initramfs/zfs-initramfs_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/z/zfs-zed/zfs-zed_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/z/zfsutils-linux/zfsutils-linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libn/libnvpair1linux/libnvpair1linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libu/libuutil1linux/libuutil1linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libz/libzfs2linux/libzfs2linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Failed to fetch http://archive.ubuntu.com/ubuntu/pool/main/libz/libzpool2linux/libzpool2linux_0.8.2-3ubuntu1_amd64.deb  Connection timed out [IP: 91.189.88.31 80]")
                Console.WriteLine("E: Unable to fetch some archives, maybe run apt-get update or try with --fix-missing?")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(100)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("{0}@{1}:{2}$ ", HName, signedinusrnm, CurrDir)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Thread.Sleep(5000)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
            End If
        Loop
    End Sub

    Sub HackUserFromAD_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles HackUserFromAD.DoWork
        Do While True
            Thread.Sleep(1000)
IFCANCEL:
            If HackUserFromAD.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Hacking Simulator for Active Domain users screensaver stopped.")
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

    Sub GlitterColor_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterColor.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            For Each color In colors
                If GlitterColor.CancellationPending = True Then
                    Wdbg("Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = inputColor
                    Console.BackgroundColor = backgroundColor
                    Load()
                    Console.CursorVisible = True
                    Wdbg("All clean. Glitter Color screensaver stopped.")
                    Exit Do
                Else
                    Thread.Sleep(1)
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    Console.SetCursorPosition(Left, Top)
                    Console.BackgroundColor = color
                    Console.Write(" ")
                End If
            Next
        Loop
    End Sub

    Sub ColorMix255_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix255.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Dim RandomDriver As New Random
        Console.Clear()
        Console.CursorVisible = False
        Do While True
            Thread.Sleep(1)
            If ColorMix255.CancellationPending = True Then
                Wdbg("Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Console.ForegroundColor = inputColor
                Console.BackgroundColor = backgroundColor
                Load()
                Console.CursorVisible = True
                Wdbg("All clean. Mix 255 Colors screensaver stopped.")
                Exit Do
            Else
                Dim esc As Char = GetEsc()
                Dim ColorNum As Integer = RandomDriver.Next(255)
                Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
            End If
        Loop
    End Sub

    Sub GlitterColor255_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterColor255.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            For Each color In colors
                If GlitterColor255.CancellationPending = True Then
                    Wdbg("Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    Console.Clear()
                    Console.ForegroundColor = inputColor
                    Console.BackgroundColor = backgroundColor
                    Load()
                    Console.CursorVisible = True
                    Wdbg("All clean. Glitter 255 Colors screensaver stopped.")
                    Exit Do
                Else
                    Thread.Sleep(1)
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    Console.SetCursorPosition(Left, Top)
                    Dim esc As Char = GetEsc()
                    Dim ColorNum As Integer = RandomDriver.Next(255)
                    Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
                End If
            Next
        Loop
    End Sub

End Module
