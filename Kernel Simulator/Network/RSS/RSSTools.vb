
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

Imports HtmlAgilityPack
Imports System.Threading
Imports System.Xml

Public Module RSSTools

    ''' <summary>
    ''' Make instances of RSS Article from feed node and type
    ''' </summary>
    ''' <param name="FeedNode">Feed XML node</param>
    ''' <param name="FeedType">Feed type</param>
    Function MakeRssArticlesFromFeed(FeedNode As XmlNodeList, FeedType As RSSFeedType) As List(Of RSSArticle)
        Dim Articles As New List(Of RSSArticle)
        Select Case FeedType
            Case RSSFeedType.RSS2
                For Each Node As XmlNode In FeedNode(0) '<channel>
                    For Each Child As XmlNode In Node.ChildNodes '<item>
                        If Child.Name = "item" Then
                            Dim Article As RSSArticle = MakeArticleFromFeed(Child)
                            Articles.Add(Article)
                        End If
                    Next
                Next
            Case RSSFeedType.RSS1
                For Each Node As XmlNode In FeedNode(0) '<channel> or <item>
                    If Node.Name = "item" Then
                        Dim Article As RSSArticle = MakeArticleFromFeed(Node)
                        Articles.Add(Article)
                    End If
                Next
            Case RSSFeedType.Atom
                For Each Node As XmlNode In FeedNode(0) '<feed>
                    If Node.Name = "entry" Then
                        Dim Article As RSSArticle = MakeArticleFromFeed(Node)
                        Articles.Add(Article)
                    End If
                Next
            Case Else
                Throw New Exceptions.InvalidFeedTypeException(DoTranslation("Invalid RSS feed type."))
        End Select
        Return Articles
    End Function

    ''' <summary>
    ''' Generates an instance of article from feed
    ''' </summary>
    ''' <param name="Article">The child node which holds the entire article</param>
    ''' <returns>An article</returns>
    Function MakeArticleFromFeed(Article As XmlNode) As RSSArticle
        'Variables
        Dim Parameters As New Dictionary(Of String, XmlNode)
        Dim Title, Link, Description As String

        'Parse article
        For Each ArticleNode As XmlNode In Article.ChildNodes
            'Check the title
            If ArticleNode.Name = "title" Then
                'Trimming newlines and spaces is necessary, since some RSS feeds (GitHub commits) might return string with trailing and leading spaces and newlines.
                Title = ArticleNode.InnerText.Trim(vbCr, vbLf, " ")
            End If

            'Check the link
            If ArticleNode.Name = "link" Then
                'Links can be in href attribute, so check that.
                If ArticleNode.Attributes.Count <> 0 And ArticleNode.Attributes.GetNamedItem("href") IsNot Nothing Then
                    Link = ArticleNode.Attributes.GetNamedItem("href").InnerText
                Else
                    Link = ArticleNode.InnerText
                End If
            End If

            'Check the summary
            If ArticleNode.Name = "summary" Or ArticleNode.Name = "content" Or ArticleNode.Name = "description" Then
                'It can be of HTML type, or plain text type.
                If ArticleNode.Attributes.Count <> 0 And ArticleNode.Attributes.GetNamedItem("type") IsNot Nothing Then
                    If ArticleNode.Attributes.GetNamedItem("type").Value = "html" Then
                        'Extract plain text from HTML
                        Dim HtmlContent As New HtmlDocument
                        HtmlContent.LoadHtml(ArticleNode.InnerText.Trim(vbCr, vbLf, " "))

                        'Some feeds have no node called "pre," so work around this...
                        Dim PreNode As HtmlNode = HtmlContent.DocumentNode.SelectSingleNode("pre")
                        If PreNode Is Nothing Then
                            Description = HtmlContent.DocumentNode.InnerText
                        Else
                            Description = PreNode.InnerText
                        End If
                    Else
                        Description = ArticleNode.InnerText.Trim(vbCr, vbLf, " ")
                    End If
                Else
                    Description = ArticleNode.InnerText.Trim(vbCr, vbLf, " ")
                End If
            End If
            Parameters.AddIfNotFound(ArticleNode.Name, ArticleNode)
        Next
#Disable Warning BC42104
        Return New RSSArticle(Title, Link, Description, Parameters)
#Enable Warning BC42104
    End Function

    ''' <summary>
    ''' Gets a feed property
    ''' </summary>
    ''' <param name="FeedProperty">Feed property name</param>
    ''' <param name="FeedNode">Feed XML node</param>
    ''' <param name="FeedType">Feed type</param>
    Function GetFeedProperty(FeedProperty As String, FeedNode As XmlNodeList, FeedType As RSSFeedType) As Object
        Select Case FeedType
            Case RSSFeedType.RSS2
                For Each Node As XmlNode In FeedNode(0) '<channel>
                    For Each Child As XmlNode In Node.ChildNodes
                        If Child.Name = FeedProperty Then
                            Return Child.InnerXml
                        End If
                    Next
                Next
            Case RSSFeedType.RSS1
                For Each Node As XmlNode In FeedNode(0) '<channel> or <item>
                    For Each Child As XmlNode In Node.ChildNodes
                        If Child.Name = FeedProperty Then
                            Return Child.InnerXml
                        End If
                    Next
                Next
            Case RSSFeedType.Atom
                For Each Node As XmlNode In FeedNode(0) 'Children of <feed>
                    If Node.Name = FeedProperty Then
                        Return Node.InnerXml
                    End If
                Next
            Case Else
                Throw New Exceptions.InvalidFeedTypeException(DoTranslation("Invalid RSS feed type."))
        End Select
    End Function

    ''' <summary>
    ''' Refreshes the feeds
    ''' </summary>
    Friend Sub RefreshFeeds()
        Dim OldFeedsList As New List(Of RSSArticle)(RSSFeedInstance.FeedArticles)
        Dim NewFeedsList As List(Of RSSArticle)
        While RSSFeedInstance IsNot Nothing
            If RSSFeedInstance IsNot Nothing Then
                'Refresh the feed
                RSSFeedInstance.Refresh()

                'Check for new feeds
                NewFeedsList = RSSFeedInstance.FeedArticles.Except(OldFeedsList).ToList
                If NewFeedsList.Count > 0 And NewFeedsList(0).ArticleTitle <> OldFeedsList(0).ArticleTitle Then
                    'Update the list
                    Wdbg(DebugLevel.W, "Feeds received! Recents count was {0}, Old count was {1}", RSSFeedInstance.FeedArticles.Count, OldFeedsList.Count)
                    OldFeedsList = New List(Of RSSArticle)(RSSFeedInstance.FeedArticles)
                    For Each NewFeed As RSSArticle In NewFeedsList
                        Dim FeedNotif As New Notification(NewFeed.ArticleTitle, NewFeed.ArticleDescription, NotifPriority.Low, NotifType.Normal)
                        NotifySend(FeedNotif)
                    Next
                End If
            End If
            Thread.Sleep(60000)
        End While
    End Sub

End Module
