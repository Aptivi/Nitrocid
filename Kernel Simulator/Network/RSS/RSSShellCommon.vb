
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports System.Net.Http
Imports KS.Network.RSS.Commands
Imports KS.Network.RSS.Instance

Namespace Network.RSS
    Public Module RSSShellCommon

        'Variables
        Public ReadOnly RSSCommands As New Dictionary(Of String, CommandInfo) From {
            {"articleinfo", New CommandInfo("articleinfo", ShellType.RSSShell, "Gets the article info", New CommandArgumentInfo({"<feednum>"}, True, 1), New RSS_ArticleInfoCommand)},
            {"bookmark", New CommandInfo("bookmark", ShellType.RSSShell, "Bookmarks the feed", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RSS_BookmarkCommand)},
            {"chfeed", New CommandInfo("chfeed", ShellType.RSSShell, "Changes the feed link", New CommandArgumentInfo({"[-bookmark] <feedurl/bookmarknumber>"}, True, 1), New RSS_ChFeedCommand)},
            {"feedinfo", New CommandInfo("feedinfo", ShellType.RSSShell, "Gets the feed info", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RSS_FeedInfoCommand)},
            {"help", New CommandInfo("help", ShellType.RSSShell, "Shows help screen", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RSS_HelpCommand)},
            {"list", New CommandInfo("list", ShellType.RSSShell, "Lists all feeds", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RSS_ListCommand)},
            {"listbookmark", New CommandInfo("listbookmark", ShellType.RSSShell, "Lists all bookmarked feeds", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RSS_ListBookmarkCommand)},
            {"read", New CommandInfo("read", ShellType.RSSShell, "Reads a feed in a web browser", New CommandArgumentInfo({"<feednum>"}, True, 1), New RSS_ReadCommand)},
            {"selfeed", New CommandInfo("selfeed", ShellType.RSSShell, "Selects the feed from the existing feed list from online sources", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RSS_SelFeedCommand)},
            {"unbookmark", New CommandInfo("unbookmark", ShellType.RSSShell, "Removes the feed bookmark", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RSS_UnbookmarkCommand)}
        }
        Public RSSFeedInstance As RSSFeed
        Public RSSShellPromptStyle As String = ""
        Public RSSFeedUrlPromptStyle As String = ""
        Public RSSFetchTimeout As Integer = 60000
        Public RSSRefreshFeeds As Boolean = True
        Public RSSRefreshInterval As Integer = 60000
        Public RSSKeepAlive As Boolean
        Friend RSSRefresher As New KernelThread("RSS Feed Refresher", False, AddressOf RefreshFeeds)
        Friend RSSRefresherClient As New HttpClient() With {.Timeout = TimeSpan.FromMilliseconds(RSSFetchTimeout)}
        Friend RSSFeedLink As String
        Friend ReadOnly RSSModCommands As New Dictionary(Of String, CommandInfo)

    End Module
End Namespace
