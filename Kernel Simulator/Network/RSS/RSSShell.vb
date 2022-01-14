
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

Imports System.Threading

Public Module RSSShellCommon

    'Variables
    Public ReadOnly RSSCommands As New Dictionary(Of String, CommandInfo) From {{"articleinfo", New CommandInfo("articleinfo", ShellCommandType.RSSShell, "Gets the article info", {"<feednum>"}, True, 1, New RSS_ArticleInfoCommand)},
                                                                                {"chfeed", New CommandInfo("chfeed", ShellCommandType.RSSShell, "Changes the feed link", {"<feedurl>"}, True, 1, New RSS_ChFeedCommand)},
                                                                                {"exit", New CommandInfo("exit", ShellCommandType.RSSShell, "Exits RSS shell and returns to kernel", {}, False, 0, New RSS_ExitCommand)},
                                                                                {"feedinfo", New CommandInfo("feedinfo", ShellCommandType.RSSShell, "Gets the feed info", {}, False, 0, New RSS_FeedInfoCommand)},
                                                                                {"help", New CommandInfo("help", ShellCommandType.RSSShell, "Shows help screen", {}, False, 0, New RSS_HelpCommand)},
                                                                                {"list", New CommandInfo("list", ShellCommandType.RSSShell, "Lists all feeds", {}, False, 0, New RSS_ListCommand)},
                                                                                {"read", New CommandInfo("read", ShellCommandType.RSSShell, "Reads a feed in a web browser", {"<feednum>"}, True, 1, New RSS_ReadCommand)}}
    Public RSSModCommands As New ArrayList
    Public RSSFeedInstance As RSSFeed
    Public RSSShellPromptStyle As String = ""
    Public RSSFeedUrlPromptStyle As String = ""
    Public RSSFetchTimeout As Integer = 60000
    Public RSSRefreshFeeds As Boolean = True
    Public RSSRefreshInterval As Integer = 60000
    Public RSSKeepAlive As Boolean
    Friend RSSRefresher As New Thread(AddressOf RefreshFeeds) With {.Name = "RSS Feed Refresher"}
    Friend RSSFeedLink As String

End Module
