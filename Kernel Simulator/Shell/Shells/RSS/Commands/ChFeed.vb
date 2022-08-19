
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

Imports KS.Network.RSS

Namespace Shell.Shells.RSS.Commands
    ''' <summary>
    ''' Changes current feed
    ''' </summary>
    ''' <remarks>
    ''' If you want to read another feed, you can use this command to provide a second feed URL to the shell so you can interact with it.
    ''' </remarks>
    Class RSS_ChFeedCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim UseBookmarkNum As Boolean = ListSwitchesOnly.Contains("-bookmark")
            Dim BookmarkNum As Integer
            If UseBookmarkNum Then
                BookmarkNum = Integer.Parse(ListArgsOnly(0))
                RSSFeedLink = GetBookmark(BookmarkNum)
            Else
                RSSFeedLink = ListArgsOnly(0)
            End If
        End Sub

    End Class
End Namespace
