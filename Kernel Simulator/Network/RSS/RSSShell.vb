
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
    Public RSSExiting, RSSKeepAlive As Boolean
    Public ReadOnly RSSCommands As New Dictionary(Of String, CommandInfo) From {{"articleinfo", New CommandInfo("articleinfo", ShellCommandType.RSSShell, "Gets the article info", "<feednum>", True, 1, New RSS_ArticleInfoCommand)},
                                                                                {"chfeed", New CommandInfo("chfeed", ShellCommandType.RSSShell, "Changes the feed link", "<feedurl>", True, 1, New RSS_ChFeedCommand)},
                                                                                {"exit", New CommandInfo("exit", ShellCommandType.RSSShell, "Exits RSS shell and returns to kernel", "", False, 0, New RSS_ExitCommand)},
                                                                                {"feedinfo", New CommandInfo("feedinfo", ShellCommandType.RSSShell, "Gets the feed info", "", False, 0, New RSS_FeedInfoCommand)},
                                                                                {"help", New CommandInfo("help", ShellCommandType.RSSShell, "Shows help screen", "", False, 0, New RSS_HelpCommand)},
                                                                                {"list", New CommandInfo("list", ShellCommandType.RSSShell, "Lists all feeds", "", False, 0, New RSS_ListCommand)},
                                                                                {"read", New CommandInfo("read", ShellCommandType.RSSShell, "Reads a feed in a web browser", "<feednum>", True, 1, New RSS_ReadCommand)}}
    Public RSSModCommands As New ArrayList
    Public RSSFeedInstance As RSSFeed
    Public RSSShellPromptStyle As String = ""
    Friend RSSRefresher As New Thread(AddressOf RefreshFeeds) With {.Name = "RSS Feed Refresher"}
    Friend RSSFeedLink As String

    ''' <summary>
    ''' Opens an RSS shell to read feeds
    ''' </summary>
    ''' <param name="FeedUrl">A link to an RSS feed</param>
    Public Sub InitiateRSSShell(Optional FeedUrl As String = "")
        'Add handler for RSS shell
        SwitchCancellationHandler(ShellCommandType.RSSShell)
        Dim OldRSSFeedLink As String = ""
        RSSFeedLink = FeedUrl

        While Not RSSExiting
Begin:
            If String.IsNullOrWhiteSpace(RSSFeedLink) Then
                Do While String.IsNullOrWhiteSpace(RSSFeedLink)
                    Try
                        W(DoTranslation("Enter an RSS feed URL:") + " ", False, ColTypes.Input)
                        RSSFeedLink = Console.ReadLine
                        RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                        RSSFeedLink = RSSFeedInstance.FeedUrl
                        OldRSSFeedLink = RSSFeedLink
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", FeedUrl, ex.Message)
                        WStkTrc(ex)
                        W(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                        RSSFeedLink = ""
                    End Try
                Loop
            Else
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
                    W(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                    RSSFeedLink = ""
                    GoTo Begin
                End Try

                'Send ping to keep the connection alive
                If Not RSSKeepAlive And Not RSSRefresher.IsAlive Then RSSRefresher.Start()
                Wdbg(DebugLevel.I, "Made new thread about RefreshFeeds()")

                'Prepare for prompt
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If
                Wdbg(DebugLevel.I, "RSSShellPromptStyle = {0}", RSSShellPromptStyle)
                If RSSShellPromptStyle = "" Then
                    W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, New Uri(RSSFeedLink).Host) : W("] > ", False, ColTypes.Gray)
                Else
                    Dim ParsedPromptStyle As String = ProbePlaces(RSSShellPromptStyle)
                    ParsedPromptStyle.ConvertVTSequences
                    W(ParsedPromptStyle, False, ColTypes.Gray)
                End If
                SetInputColor()

                'Prompt for command
                EventManager.RaiseRSSShellInitialized(RSSFeedLink)
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
                            EventManager.RaiseRSSPreExecuteCommand(RSSFeedLink, WrittenCommand)
                            Wdbg(DebugLevel.I, "Made new thread. Starting with argument {0}...", WrittenCommand)
                            RSSCommandThread.Start(Params)
                            RSSCommandThread.Join()
                            EventManager.RaiseRSSPostExecuteCommand(RSSFeedLink, WrittenCommand)
                        ElseIf RSSModCommands.Contains(Command) Then
                            Wdbg(DebugLevel.I, "Mod command {0} executing...", Command)
                            ExecuteModCommand(WrittenCommand)
                        ElseIf RSSShellAliases.Keys.Contains(Command) Then
                            Wdbg(DebugLevel.I, "RSS shell alias command found.")
                            WrittenCommand = WrittenCommand.Replace($"""{Command}""", Command)
                            ExecuteRSSAlias(WrittenCommand)
                        Else
                            W(DoTranslation("The specified RSS shell command is not found."), True, ColTypes.Error)
                            Wdbg(DebugLevel.E, "Command {0} not found in the list of {1} commands.", WrittenCommand.Split(" ")(0), RSSCommands.Count)
                        End If
                    End If
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Unknown RSS shell error: {0}", ex.Message)
                    WStkTrc(ex)
                    W(DoTranslation("Unknown shell error:") + " {0}", True, ColTypes.Error, ex.Message)
                End Try

                'This is to fix race condition between shell initialization and starting the event handler thread
                If WrittenCommand Is Nothing Then
                    Thread.Sleep(30)
                End If
            End If
        End While

        'Disconnect the session
        If RSSKeepAlive Then
            Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.")
        Else
            Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...")
            RSSRefresher.Abort()
            RSSFeedLink = ""
            RSSFeedInstance = Nothing
            RSSRefresher = New Thread(AddressOf RefreshFeeds) With {.Name = "RSS Feed Refresher"}
        End If
        RSSExiting = False

        'Remove handler for RSS shell
        SwitchCancellationHandler(LastShellType)
    End Sub

    ''' <summary>
    ''' Executes the RSS shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteRSSAlias(aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, RSSShellAliases(FirstWordCmd))
        Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
        Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellCommandType.RSSShell, Nothing)
        RSSCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "RSS Shell Command Thread"}
        RSSCommandThread.Start(Params)
        RSSCommandThread.Join()
    End Sub

End Module
