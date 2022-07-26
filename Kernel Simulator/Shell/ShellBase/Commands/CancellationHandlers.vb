
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports System.IO
Imports KS.Shell.ShellBase.Shells

Namespace Shell.ShellBase
    Module CancellationHandlers

        Friend CurrentShellType As ShellType = ShellType.Shell
        Friend LastShellType As ShellType = ShellType.Shell
        Friend CancelSync, EditorCancelSync, FTPCancelSync, HTTPCancelSync, JsonShellCancelSync, MailCancelSync, RssShellCancelSync,
               SFTPCancelSync, TestCancelSync, ZipShellCancelSync, HexEditorCancelSync As New Object

        Sub CancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock GetCancelSyncLock(ShellStack(ShellStack.Count - 1).ShellType)
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    CancelRequested = True
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    Dim StartCommandThread As KernelThread = ShellStack(ShellStack.Count - 1).ShellCommandThread
                    StartCommandThread.Stop()
                    ProcessStartCommandThread.Stop()
                    Console.SetOut(DefConsoleOut)
                End If
            End SyncLock
        End Sub

        Function GetCancelSyncLock(ShellType As ShellType) As Object
            Select Case ShellType
                Case ShellType.Shell
                    Return CancelSync
                Case ShellType.FTPShell
                    Return FTPCancelSync
                Case ShellType.MailShell
                    Return MailCancelSync
                Case ShellType.SFTPShell
                    Return SFTPCancelSync
                Case ShellType.TextShell
                    Return EditorCancelSync
                Case ShellType.TestShell
                    Return TestCancelSync
                Case ShellType.ZIPShell
                    Return ZipShellCancelSync
                Case ShellType.RSSShell
                    Return RssShellCancelSync
                Case ShellType.JsonShell
                    Return JsonShellCancelSync
                Case ShellType.HTTPShell
                    Return HTTPCancelSync
                Case ShellType.HexShell
                    Return HexEditorCancelSync
            End Select
        End Function

    End Module
End Namespace
