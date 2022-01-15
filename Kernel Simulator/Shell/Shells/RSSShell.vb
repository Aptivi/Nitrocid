
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
Imports System.IO.Compression
Imports System.Threading

Public Class RSSShell
    Inherits ShellExecutor
    Implements IShell

    Public Overrides ReadOnly Property ShellType As ShellCommandType Implements IShell.ShellType
        Get
            Return ShellCommandType.RSSShell
        End Get
    End Property

    Public Overrides Property Bail As Boolean Implements IShell.Bail

    Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
        'Add handler for RSS shell
        SwitchCancellationHandler(ShellCommandType.RSSShell)
        Dim OldRSSFeedLink As String = ""
        Dim FeedUrl As String = ""
        If ShellArgs.Length > 0 Then
            FeedUrl = ShellArgs(0)
        End If
        RSSFeedLink = FeedUrl

        While Not Bail
Begin:
            If String.IsNullOrWhiteSpace(RSSFeedLink) Then
                Do While String.IsNullOrWhiteSpace(RSSFeedLink)
                    Try
                        If Not String.IsNullOrWhiteSpace(RSSFeedUrlPromptStyle) Then
                            Write(ProbePlaces(RSSFeedUrlPromptStyle), False, ColTypes.Input)
                        Else
                            Write(DoTranslation("Enter an RSS feed URL:") + " ", False, ColTypes.Input)
                        End If
                        RSSFeedLink = Console.ReadLine
                        RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                        RSSFeedLink = RSSFeedInstance.FeedUrl
                        OldRSSFeedLink = RSSFeedLink
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", FeedUrl, ex.Message)
                        WStkTrc(ex)
                        Write(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                        RSSFeedLink = ""
                    End Try
                Loop
            Else
                SyncLock RssShellCancelSync
                    'Make a new RSS feed instance
                    Try
                        If OldRSSFeedLink <> RSSFeedLink Then
                            RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                            RSSFeedLink = RSSFeedInstance.FeedUrl
                        End If
                        OldRSSFeedLink = RSSFeedLink
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", RSSFeedLink, ex.Message)
                        WStkTrc(ex)
                        Write(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                        RSSFeedLink = ""
                        GoTo Begin
                    End Try

                    'Send ping to keep the connection alive
                    If Not RSSKeepAlive And Not RSSRefresher.IsAlive And RSSRefreshFeeds Then RSSRefresher.Start()
                    Wdbg(DebugLevel.I, "Made new thread about RefreshFeeds()")

                    'Prepare for prompt
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    Wdbg(DebugLevel.I, "RSSShellPromptStyle = {0}", RSSShellPromptStyle)
                    If RSSShellPromptStyle = "" Then
                        Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, New Uri(RSSFeedLink).Host) : Write("] > ", False, ColTypes.Gray)
                    Else
                        Dim ParsedPromptStyle As String = ProbePlaces(RSSShellPromptStyle)
                        ParsedPromptStyle.ConvertVTSequences
                        Write(ParsedPromptStyle, False, ColTypes.Gray)
                    End If
                    SetInputColor()

                    'Prompt for command
                    KernelEventManager.RaiseRSSShellInitialized(RSSFeedLink)
                    Dim WrittenCommand As String = Console.ReadLine

                    'Check to see if the command doesn't start with spaces or if the command is nothing
                    Try
                        Wdbg(DebugLevel.I, "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand.StartsWith(" "), WrittenCommand Is Nothing, WrittenCommand = "")
                        If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"}) = True) Then
                            Dim Command As String = WrittenCommand.SplitEncloseDoubleQuotes(" ")(0)
                            Wdbg(DebugLevel.I, "Checking command {0} for existence.", Command)
                            If RSSCommands.ContainsKey(Command) Then
                                Wdbg(DebugLevel.I, "Command {0} found in the list of {1} commands.", Command, RSSCommands.Count)
                                Dim Params As New ExecuteCommandThreadParameters(WrittenCommand, ShellCommandType.RSSShell, Nothing)
                                RSSCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "RSS Shell Command Thread"}
                                KernelEventManager.RaiseRSSPreExecuteCommand(RSSFeedLink, WrittenCommand)
                                Wdbg(DebugLevel.I, "Made new thread. Starting with argument {0}...", WrittenCommand)
                                RSSCommandThread.Start(Params)
                                RSSCommandThread.Join()
                                KernelEventManager.RaiseRSSPostExecuteCommand(RSSFeedLink, WrittenCommand)
                            ElseIf RSSModCommands.Contains(Command) Then
                                Wdbg(DebugLevel.I, "Mod command {0} executing...", Command)
                                ExecuteModCommand(WrittenCommand)
                            ElseIf RSSShellAliases.Keys.Contains(Command) Then
                                Wdbg(DebugLevel.I, "RSS shell alias command found.")
                                WrittenCommand = WrittenCommand.Replace($"""{Command}""", Command)
                                ExecuteRSSAlias(WrittenCommand)
                            Else
                                Write(DoTranslation("The specified RSS shell command is not found."), True, ColTypes.Error)
                                Wdbg(DebugLevel.E, "Command {0} not found in the list of {1} commands.", WrittenCommand.Split(" ")(0), RSSCommands.Count)
                            End If
                        End If
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Unknown RSS shell error: {0}", ex.Message)
                        WStkTrc(ex)
                        Write(DoTranslation("Unknown shell error:") + " {0}", True, ColTypes.Error, ex.Message)
                    End Try
                End SyncLock
            End If
        End While

        'Disconnect the session
        If RSSKeepAlive Then
            Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.")
        Else
            Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...")
            If RSSRefreshFeeds Then RSSRefresher.Abort()
            RSSFeedLink = ""
            RSSFeedInstance = Nothing
            RSSRefresher = New Thread(AddressOf RefreshFeeds) With {.Name = "RSS Feed Refresher"}
        End If

        'Remove handler for RSS shell
        SwitchCancellationHandler(LastShellType)
    End Sub

End Class
