
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

Public Module ScreensaverDisplays

    'TODO: Move each screensaver to their own individual files to save time looking for the specific screen saver code.
    Public WithEvents ColorMix As New BackgroundWorker
    Public WithEvents Matrix As New BackgroundWorker
    Public WithEvents Disco As New BackgroundWorker
    Public WithEvents Lines As New BackgroundWorker
    Public WithEvents GlitterMatrix As New BackgroundWorker
    Public WithEvents GlitterColor As New BackgroundWorker
    Public WithEvents AptErrorSim As New BackgroundWorker
    Public WithEvents HackUserFromAD As New BackgroundWorker
    Public WithEvents BouncingText As New BackgroundWorker
    Public WithEvents BouncingBlock As New BackgroundWorker
    Public WithEvents Dissolve As New BackgroundWorker
    Public WithEvents Custom As New BackgroundWorker
    Public finalSaver As ICustomSaver
    Public colors() As ConsoleColor = CType([Enum].GetValues(GetType(ConsoleColor)), ConsoleColor())        '15 Console Colors
    Public colors255() As ConsoleColors = CType([Enum].GetValues(GetType(ConsoleColors)), ConsoleColors())  '255 Console Colors

    ''' <summary>
    ''' Handles custom screensaver code
    ''' </summary>
    Sub Custom_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Custom.DoWork
        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.W() with System.Console.WriteLine() or System.Console.Write().
        Console.CursorVisible = False
        finalSaver.PreDisplay()
        Do While True
            If Not finalSaver.DelayForEachWrite = Nothing Then
                Thread.Sleep(finalSaver.DelayForEachWrite)
            End If
            If Custom.CancellationPending = True Then
                Wdbg("W", "Cancellation requested. Showing ending...")
                finalSaver.PostDisplay()
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Custom screensaver stopped.")
                Exit Do
            Else
                finalSaver.ScrnSaver()
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of ColorMix
    ''' </summary>
    Sub ColorMix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ColorMix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim colorrand As New Random()
        Do While True
            Thread.Sleep(1)
            If ColorMix.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Mix Colors screensaver stopped.")
                Exit Do
            Else
                If ColorMixTrueColor Then
                    Dim esc As Char = GetEsc()
                    Dim RedColorNum As Integer = colorrand.Next(255)
                    Dim GreenColorNum As Integer = colorrand.Next(255)
                    Dim BlueColorNum As Integer = colorrand.Next(255)
                    Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                    Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m ")
                ElseIf ColorMix255Colors Then
                    Dim esc As Char = GetEsc()
                    Dim ColorNum As Integer = colorrand.Next(255)
                    Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
                Else
                    Console.BackgroundColor = CType(colorrand.Next(1, 16), ConsoleColor) : Console.Write(" ")
                End If
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of Matrix
    ''' </summary>
    Sub Matrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Matrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim random As New Random()
        Do While True
            If Matrix.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Matrix screensaver stopped.")
                Exit Do
            Else
                Thread.Sleep(1)
                Console.Write(CStr(random.Next(2)))
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of Disco
    ''' </summary>
    Sub Disco_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Disco.DoWork
        Console.CursorVisible = False
        Dim MaximumColors As Integer = 15
        Dim MaximumColorsR As Integer = 255
        Dim MaximumColorsG As Integer = 255
        Dim MaximumColorsB As Integer = 255
        Dim CurrentColor As Integer = 0
        Dim CurrentColorR, CurrentColorG, CurrentColorB As Integer
        Dim random As New Random()
        Do While True
            Thread.Sleep(100)
            If Disco.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Disco screensaver stopped.")
                Exit Do
            Else
                If DiscoTrueColor Then
                    Dim esc As Char = GetEsc()
                    If Not DiscoCycleColors Then
                        Dim RedColorNum As Integer = random.Next(255)
                        Dim GreenColorNum As Integer = random.Next(255)
                        Dim BlueColorNum As Integer = random.Next(255)
                        Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                        Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m")
                    Else
                        Dim ColorStorage As New RGB(CurrentColorR, CurrentColorG, CurrentColorB)
                        Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m")
                    End If
                ElseIf Disco255Colors Then
                    Dim esc As Char = GetEsc()
                    If Not DiscoCycleColors Then
                        Dim color As Integer = random.Next(255)
                        Console.Write(esc + "[48;5;" + CStr(color) + "m")
                    Else
                        MaximumColors = 255
                        Console.Write(esc + "[48;5;" + CStr(CurrentColor) + "m")
                    End If
                Else
                    If Not DiscoCycleColors Then
                        Console.BackgroundColor = colors(random.Next(colors.Length - 1))
                    Else
                        Console.BackgroundColor = colors(CurrentColor)
                    End If
                End If
                Console.Clear()
                If DiscoTrueColor Then
                    If CurrentColorR >= MaximumColorsR Then
                        CurrentColorR = 0
                    Else
                        CurrentColorR += 1
                    End If
                    If CurrentColorG >= MaximumColorsG Then
                        CurrentColorG = 0
                    ElseIf CurrentColorR = 0 Then
                        CurrentColorG += 1
                    End If
                    If CurrentColorB >= MaximumColorsB Then
                        CurrentColorB = 0
                    ElseIf CurrentColorG = 0 And CurrentColorR = 0 Then
                        CurrentColorB += 1
                    End If
                    If CurrentColorB = 0 And CurrentColorG = 0 And CurrentColorR = 0 Then
                        CurrentColorB = 0
                        CurrentColorG = 0
                        CurrentColorR = 0
                    End If
                Else
                    If CurrentColor >= MaximumColors Then
                        CurrentColor = 0
                    Else
                        CurrentColor += 1
                    End If
                End If
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of Lines
    ''' </summary>
    Sub Lines_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Lines.DoWork
        Console.CursorVisible = False
        Dim random As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            Thread.Sleep(500)
            If Lines.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Lines screensaver stopped.")
                Exit Do
            Else
                If LinesTrueColor Then
                    Dim esc As Char = GetEsc()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    Dim RedColorNum As Integer = random.Next(255)
                    Dim GreenColorNum As Integer = random.Next(255)
                    Dim BlueColorNum As Integer = random.Next(255)
                    Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                    Console.Write(esc + "[38;2;" + ColorStorage.ToString + "m")
                ElseIf Lines255Colors Then
                    Dim esc As Char = GetEsc()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    Dim color As Integer = random.Next(255)
                    Console.Write(esc + "[38;5;" + CStr(color) + "m")
                Else
                    Console.Clear()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = colors(random.Next(colors.Length - 1))
                End If
                Dim Line As String = ""
                Dim Top As Integer = New Random().Next(Console.WindowHeight)
                For i As Integer = 1 To Console.WindowWidth
                    Line += "-"
                Next
                Console.SetCursorPosition(0, Top)
                Console.WriteLine(Line)
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of Glitter Matrix
    ''' </summary>
    Sub GlitterMatrix_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterMatrix.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.Green
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If GlitterMatrix.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Glitter Matrix screensaver stopped.")
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

    ''' <summary>
    ''' Handles the code of APT Error Sim
    ''' </summary>
    Sub AptErrorSim_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles AptErrorSim.DoWork
        Console.CursorVisible = False
        Do While True
            Thread.Sleep(100)
IFCANCEL:
            If AptErrorSim.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. apt Error Simulator screensaver stopped.")
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

    ''' <summary>
    ''' Handles the code of Glitter Colors
    ''' </summary>
    Sub GlitterColor_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles GlitterColor.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If GlitterColor.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Glitter Color screensaver stopped.")
                Exit Do
            Else
                Thread.Sleep(1)
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                Console.SetCursorPosition(Left, Top)
                If GlitterColorTrueColor Then
                    Dim esc As Char = GetEsc()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    Dim RedColorNum As Integer = RandomDriver.Next(255)
                    Dim GreenColorNum As Integer = RandomDriver.Next(255)
                    Dim BlueColorNum As Integer = RandomDriver.Next(255)
                    Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                    Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m ")
                ElseIf GlitterColor255Colors Then
                    Dim esc As Char = GetEsc()
                    Dim ColorNum As Integer = RandomDriver.Next(255)
                    Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
                Else
                    Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                    Console.Write(" ")
                End If
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of Bouncing Text
    ''' </summary>
    Sub BouncingText_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BouncingText.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim Direction As String = "BottomRight"
        Dim RowText, ColumnFirstLetter, ColumnLastLetter As Integer
        RowText = Console.WindowHeight / 2
        ColumnFirstLetter = (Console.WindowWidth / 2) - BouncingTextWrite.Length / 2
        ColumnLastLetter = (Console.WindowWidth / 2) + BouncingTextWrite.Length / 2
        Do While True
            Thread.Sleep(10)
            Console.Clear()
            If BouncingText.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Bouncing Text screensaver stopped.")
                Exit Do
            Else
                WriteWhere(BouncingTextWrite, ColumnFirstLetter, RowText, ColTypes.Neutral)

                If Direction = "BottomRight" Then
                    RowText += 1
                    ColumnFirstLetter += 1
                    ColumnLastLetter += 1
                ElseIf Direction = "BottomLeft" Then
                    RowText += 1
                    ColumnFirstLetter -= 1
                    ColumnLastLetter -= 1
                ElseIf Direction = "TopRight" Then
                    RowText -= 1
                    ColumnFirstLetter += 1
                    ColumnLastLetter += 1
                ElseIf Direction = "TopLeft" Then
                    RowText -= 1
                    ColumnFirstLetter -= 1
                    ColumnLastLetter -= 1
                End If

                If RowText = Console.WindowHeight - 2 Then
                    Direction = Direction.Replace("Bottom", "Top")
                ElseIf RowText = 1 Then
                    Direction = Direction.Replace("Top", "Bottom")
                End If

                If ColumnLastLetter = Console.WindowWidth - 1 Then
                    Direction = Direction.Replace("Right", "Left")
                ElseIf ColumnFirstLetter = 1 Then
                    Direction = Direction.Replace("Left", "Right")
                End If
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of Dissolve
    ''' </summary>
    Sub Dissolve_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Dissolve.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim ColorFilled As Boolean
        Dim CoveredPositions As New ArrayList
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If Dissolve.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Dissolve screensaver stopped.")
                Exit Do
            Else
                If ColorFilled Then Thread.Sleep(1)
                Dim EndLeft As Integer = Console.WindowWidth - 1
                Dim EndTop As Integer = Console.WindowHeight - 1
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                If Not ColorFilled Then
                    'NOTICE: Mono seems to have a bug in Console.CursorLeft and Console.CursorTop when printing with VT escape sequences.
                    If Not (Console.CursorLeft = EndLeft And Console.CursorTop = EndTop) Then
                        If DissolveTrueColor Then
                            Dim esc As Char = GetEsc()
                            Dim RedColorNum As Integer = RandomDriver.Next(255)
                            Dim GreenColorNum As Integer = RandomDriver.Next(255)
                            Dim BlueColorNum As Integer = RandomDriver.Next(255)
                            Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                            Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m ")
                        ElseIf Dissolve255Colors Then
                            Dim esc As Char = GetEsc()
                            Dim ColorNum As Integer = RandomDriver.Next(255)
                            Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
                        Else
                            Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                            Console.Write(" ")
                        End If
                    Else
                        ColorFilled = True
                    End If
                Else
                    If Not CoveredPositions.Contains(Left & " - " & Top) Then CoveredPositions.Add(Left & " - " & Top)
                    Console.SetCursorPosition(Left, Top)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Write(" ")
                    If CoveredPositions.Count = (EndLeft + 1) * (EndTop + 1) Then
                        ColorFilled = False
                        Console.BackgroundColor = ConsoleColor.Black
                        Console.Clear()
                        CoveredPositions.Clear()
                    End If
                End If
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Handles the code of Bouncing Block
    ''' </summary>
    Sub BouncingBlock_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BouncingBlock.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim Direction As String = "BottomRight"
        Dim RowBlock, ColumnBlock As Integer
        RowBlock = Console.WindowHeight / 2
        ColumnBlock = Console.WindowWidth / 2
        Do While True
            Thread.Sleep(10)
            Console.Clear()
            If BouncingBlock.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Bouncing Text screensaver stopped.")
                Exit Do
            Else
                If BouncingBlockTrueColor Then
                    Dim esc As Char = GetEsc()
                    Dim RedColorNum As Integer = RandomDriver.Next(255)
                    Dim GreenColorNum As Integer = RandomDriver.Next(255)
                    Dim BlueColorNum As Integer = RandomDriver.Next(255)
                    Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                    WriteWhereTrueColor(" ", ColumnBlock, RowBlock, New RGB(255, 255, 255), ColorStorage)
                ElseIf BouncingBlock255Colors Then
                    Dim esc As Char = GetEsc()
                    Dim ColorNum As Integer = RandomDriver.Next(255)
                    WriteWhereC(" ", ColumnBlock, RowBlock, ConsoleColors.White, BackgroundColor:=[Enum].Parse(GetType(ConsoleColors), ColorNum))
                Else
                    Dim OldColumn As Integer = Console.CursorLeft
                    Dim OldRow As Integer = Console.CursorTop
                    Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                    Console.SetCursorPosition(ColumnBlock, RowBlock)
                    Console.Write(" ")
                    Console.SetCursorPosition(OldColumn, OldRow)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Write(" ")
                End If

                If Direction = "BottomRight" Then
                    RowBlock += 1
                    ColumnBlock += 1
                ElseIf Direction = "BottomLeft" Then
                    RowBlock += 1
                    ColumnBlock -= 1
                ElseIf Direction = "TopRight" Then
                    RowBlock -= 1
                    ColumnBlock += 1
                ElseIf Direction = "TopLeft" Then
                    RowBlock -= 1
                    ColumnBlock -= 1
                End If

                If RowBlock = Console.WindowHeight - 2 Then
                    Direction = Direction.Replace("Bottom", "Top")
                ElseIf RowBlock = 1 Then
                    Direction = Direction.Replace("Top", "Bottom")
                End If

                If ColumnBlock = Console.WindowWidth - 1 Then
                    Direction = Direction.Replace("Right", "Left")
                ElseIf ColumnBlock = 1 Then
                    Direction = Direction.Replace("Left", "Right")
                End If
            End If
        Loop
    End Sub

End Module
