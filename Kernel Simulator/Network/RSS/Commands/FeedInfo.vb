
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

Class RSS_FeedInfoCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        W("- " + DoTranslation("Title:") + " ", False, ColTypes.ListEntry)
        W(RSSFeedInstance.FeedTitle, True, ColTypes.ListValue)
        W("- " + DoTranslation("Link:") + " ", False, ColTypes.ListEntry)
        W(RSSFeedInstance.FeedUrl, True, ColTypes.ListValue)
        W("- " + DoTranslation("Description:") + " ", False, ColTypes.ListEntry)
        W(RSSFeedInstance.FeedDescription, True, ColTypes.ListValue)
        W("- " + DoTranslation("Feed type:") + " ", False, ColTypes.ListEntry)
        W(RSSFeedInstance.FeedType, True, ColTypes.ListValue)
        W("- " + DoTranslation("Number of articles:") + " ", False, ColTypes.ListEntry)
        W(RSSFeedInstance.FeedArticles.Count, True, ColTypes.ListValue)
    End Sub

End Class