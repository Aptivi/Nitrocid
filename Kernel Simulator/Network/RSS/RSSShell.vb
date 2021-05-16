
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

Imports System.Threading

Public Module RSSShell

    'Variables
    Public RSSExiting As Boolean
    Public RSSCommands As New Dictionary(Of String, CommandInfo) From {{"chfeed", New CommandInfo("chfeed", ShellCommandType.RSSShell, True, 1, False, False, False, False)},
                                                                       {"exit", New CommandInfo("exit", ShellCommandType.RSSShell, False, 0, False, False, False, False)},
                                                                       {"help", New CommandInfo("help", ShellCommandType.RSSShell, False, 0, False, False, False, False)},
                                                                       {"list", New CommandInfo("list", ShellCommandType.RSSShell, False, 0, False, False, False, False)},
                                                                       {"read", New CommandInfo("read", ShellCommandType.RSSShell, True, 1, False, False, False, False)}}
    Public RSSModCommands As New ArrayList
    Public RSSFeedInstance As RSSFeed
    Friend RSSFeedLink As String

    ''' <summary>
    ''' Opens an RSS shell to read feeds
    ''' </summary>
    ''' <param name="FeedUrl">A link to an RSS feed</param>
    Public Sub InitiateRSSShell(Optional FeedUrl As String = "")
        'Add handler for RSS shell
        AddHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        Dim OldRSSFeedLink As String = FeedUrl
        RSSFeedLink = FeedUrl

        While Not RSSExiting
            If String.IsNullOrWhiteSpace(RSSFeedLink) Then
                Do While String.IsNullOrWhiteSpace(RSSFeedLink)
                    Try
                        W(DoTranslation("Enter an RSS feed URL:") + " ", False, ColTypes.Input)
                        RSSFeedLink = Console.ReadLine
                        RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                        RSSFeedLink = RSSFeedInstance.FeedUrl
                        OldRSSFeedLink = RSSFeedLink
                    Catch ex As Exception
                        Wdbg("E", "Failed to parse RSS feed URL {0}: {1}", FeedUrl, ex.Message)
                        WStkTrc(ex)
                        W(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Err, ex.Message)
                        RSSFeedLink = ""
                    End Try
                Loop
            Else
                'Make a new RSS feed instance
                If OldRSSFeedLink <> RSSFeedLink Then
                    RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                    RSSFeedLink = RSSFeedInstance.FeedUrl
                End If
                OldRSSFeedLink = RSSFeedLink

                'Prepare for prompt
                If Not IsNothing(DefConsoleOut) Then
                    Console.SetOut(DefConsoleOut)
                End If
                W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, New Uri(RSSFeedLink).Host) : W("] > ", False, ColTypes.Gray)
                SetInputColor()

                'Prompt for command
                EventManager.RaiseRSSShellInitialized(RSSFeedLink)
                Dim WrittenCommand As String = Console.ReadLine

                'Check to see if the command doesn't start with spaces or if the command is nothing
                Try
                    Wdbg("I", "Starts with spaces: {0}, Is Nothing: {1}, Is Blank {2}", WrittenCommand.StartsWith(" "), IsNothing(WrittenCommand), WrittenCommand = "")
                    If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWith(" ") = True) Then
                        Wdbg("I", "Checking command {0} for existence.", WrittenCommand.Split(" ")(0))
                        If RSSCommands.ContainsKey(WrittenCommand.Split(" ")(0)) Then
                            Wdbg("I", "Command {0} found in the list of {1} commands.", WrittenCommand.Split(" ")(0), RSSCommands.Count)
                            RSSCommandThread = New Thread(AddressOf RSSParseCommand) With {.Name = "RSS Shell Command Thread"}
                            EventManager.RaiseRSSPreExecuteCommand(RSSFeedLink, WrittenCommand)
                            Wdbg("I", "Made new thread. Starting with argument {0}...", WrittenCommand)
                            RSSCommandThread.Start(WrittenCommand)
                            RSSCommandThread.Join()
                            EventManager.RaiseRSSPostExecuteCommand(RSSFeedLink, WrittenCommand)
                        ElseIf ZipShell_ModCommands.Contains(WrittenCommand.Split(" ")(0)) Then
                            Wdbg("I", "Mod command {0} executing...", WrittenCommand.Split(" ")(0))
                            EventManager.RaiseRSSPreExecuteCommand(RSSFeedLink, WrittenCommand)
                            ExecuteModCommand(WrittenCommand)
                            EventManager.RaiseRSSPostExecuteCommand(RSSFeedLink, WrittenCommand)
                        Else
                            W(DoTranslation("The specified RSS shell command is not found."), True, ColTypes.Err)
                            Wdbg("E", "Command {0} not found in the list of {1} commands.", WrittenCommand.Split(" ")(0), RSSCommands.Count)
                        End If
                    End If
                Catch ex As Exception
                    Wdbg("E", "Unknown RSS shell error: {0}", ex.Message)
                    WStkTrc(ex)
                    W(DoTranslation("Unknown shell error:") + " {0}", True, ColTypes.Err, ex.Message)
                End Try

                'This is to fix race condition between shell initialization and starting the event handler thread
                If IsNothing(WrittenCommand) Then
                    Thread.Sleep(30)
                End If
            End If
        End While

        'Remove handler for RSS shell
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
        RSSExiting = False
    End Sub

End Module
