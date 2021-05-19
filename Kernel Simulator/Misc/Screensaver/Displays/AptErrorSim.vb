
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

Module AptErrorSimDisplay

    Public WithEvents AptErrorSim As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of APT Error Sim
    ''' </summary>
    Sub AptErrorSim_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles AptErrorSim.DoWork
        Console.CursorVisible = False
        Do While True
            SleepNoBlock(100, AptErrorSim)
IFCANCEL:
            If AptErrorSim.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(New Color(InputColor).VTSequenceForeground)
                Console.Write(New Color(BackgroundColor).VTSequenceBackground)
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. apt Error Simulator screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Console.Clear()
                Console.Write("{0}@{1}:{2}$ ", HName, signedinusrnm, CurrDir)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(3000, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                WriteSlowly("sudo apt -y dist-upgrade", False, 100)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(200, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine()
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(50, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("Reading package lists... ")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(50, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.WriteLine("Building dependency tree")
                Console.Write("Reading state information... ")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(210, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.Write("Calculating upgrade...")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(80, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Done")
                Console.WriteLine("The following packages will be upgraded:")
                Console.WriteLine("  libnvpair1linux libuutil1linux libzfs2linux libzpool2linux zfs-initramfs zfs-zed zfsutils-linux")
                Console.WriteLine("7 upgraded, 0 newly installed, 0 to remove and 0 not upgraded")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(300, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.WriteLine("Need to get 1,510 kB of archives.")
                Console.WriteLine("After this operation, 155 kB of additional disk space will be used.")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(600, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("0% [Connecting to archive.ubuntu.com]")
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(10000, AptErrorSim)
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
                SleepNoBlock(100, AptErrorSim)
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
                SleepNoBlock(100, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                Console.Write("{0}@{1}:{2}$ ", HName, signedinusrnm, CurrDir)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
                SleepNoBlock(5000, AptErrorSim)
                If AptErrorSim.CancellationPending Then GoTo IFCANCEL
            End If
        Loop
    End Sub

End Module
