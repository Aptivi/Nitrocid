
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

Namespace Shell.Shells.RSS.Commands
    ''' <summary>
    ''' Opens a web browser to an article
    ''' </summary>
    ''' <remarks>
    ''' If you want to read more about the article, you can use this command to open your current web browser to the article link, if one is found.
    ''' </remarks>
    Class RSS_ReadCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ArticleIndex As Integer = ListArgs(0) - 1
            If ArticleIndex > RSSFeedInstance.FeedArticles.Count - 1 Then
                Write(DoTranslation("Article number couldn't be bigger than the available articles."), True, ColTypes.Error)
                Wdbg(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSFeedInstance.FeedArticles.Count - 1)
            Else
                If Not String.IsNullOrWhiteSpace(RSSFeedInstance.FeedArticles(ArticleIndex).ArticleLink) Then
                    Wdbg(DebugLevel.I, "Opening web browser to {0}...", RSSFeedInstance.FeedArticles(ArticleIndex).ArticleLink)
                    Process.Start(RSSFeedInstance.FeedArticles(ArticleIndex).ArticleLink)
                Else
                    Write(DoTranslation("Article doesn't have a link!"), True, ColTypes.Error)
                    Wdbg(DebugLevel.E, "Tried to open a web browser to link of article number {0}, but it's empty. ""{1}""", ArticleIndex, RSSFeedInstance.FeedArticles(ArticleIndex).ArticleLink)
                End If
            End If
        End Sub

    End Class
End Namespace
