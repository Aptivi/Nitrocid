
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

Namespace Shell.ShellBase
    Module CancellationHandlers

        Friend CurrentShellType As ShellType = ShellType.Shell
        Friend LastShellType As ShellType = ShellType.Shell
        Friend CancelSync, EditorCancelSync, FTPCancelSync, HTTPCancelSync, JsonShellCancelSync, MailCancelSync, RssShellCancelSync,
               SFTPCancelSync, TestCancelSync, ZipShellCancelSync, HexEditorCancelSync As New Object

        Sub CancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock CancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    CancelRequested = True
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    StartCommandThread.Abort()
                    ProcessStartCommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub EditorCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock EditorCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    TextEdit_CommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub ZipShellCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock ZipShellCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    ZipShell_CommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub FTPCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock FTPCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    FTPStartCommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub MailCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock MailCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    MailStartCommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub RssShellCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock RssShellCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    RSSCommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub SFTPCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock SFTPCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    SFTPStartCommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub TestCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock TestCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    TStartCommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub JsonShell_CancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock JsonShellCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    JsonShell_CommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub HTTPCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock HTTPCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    HTTPCommandThread.Abort()
                End If
            End SyncLock
        End Sub

        Sub HexEditorCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
            SyncLock HexEditorCancelSync
                If e.SpecialKey = ConsoleSpecialKey.ControlC Then
                    Console.WriteLine()
                    DefConsoleOut = Console.Out
                    Console.SetOut(StreamWriter.Null)
                    e.Cancel = True
                    HexEditorCommandThread.Abort()
                End If
            End SyncLock
        End Sub

    End Module
End Namespace
