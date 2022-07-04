
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

Imports HtmlAgilityPack
Imports System.Threading
Imports System.Xml
Imports KS.Misc.Notifications
Imports KS.Misc.Reflection
Imports KS.Network.RSS.Instance
Imports Newtonsoft.Json.Linq

Namespace Network.RSS
    Public Module RSSTools

        ''' <summary>
        ''' Whether to show the RSS headline each login
        ''' </summary>
        Public ShowHeadlineOnLogin As Boolean
        ''' <summary>
        ''' RSS headline URL
        ''' </summary>
        Public RssHeadlineUrl As String = "https://www.techrepublic.com/rssfeeds/articles/"
        ''' <summary>
        ''' Cached feed list JSON
        ''' </summary>
        Private FeedListJsonText As String = ""

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
            Try
                Dim OldFeedsList As New List(Of RSSArticle)(RSSFeedInstance.FeedArticles)
                Dim NewFeedsList As List(Of RSSArticle)
                While RSSFeedInstance IsNot Nothing
                    If RSSFeedInstance IsNot Nothing Then
                        'Refresh the feed
                        RSSFeedInstance.Refresh()

                        'Check for new feeds
                        NewFeedsList = RSSFeedInstance.FeedArticles.Except(OldFeedsList).ToList
                        Dim OldFeedTitle As String = If(OldFeedsList.Count = 0, "", OldFeedsList(0).ArticleTitle)
                        If NewFeedsList.Count > 0 AndAlso NewFeedsList(0).ArticleTitle <> OldFeedTitle Then
                            'Update the list
                            Wdbg(DebugLevel.W, "Feeds received! Recents count was {0}, Old count was {1}", RSSFeedInstance.FeedArticles.Count, OldFeedsList.Count)
                            OldFeedsList = New List(Of RSSArticle)(RSSFeedInstance.FeedArticles)
                            For Each NewFeed As RSSArticle In NewFeedsList
                                Dim FeedNotif As New Notification(NewFeed.ArticleTitle, NewFeed.ArticleDescription, NotifPriority.Low, NotifType.Normal)
                                NotifySend(FeedNotif)
                            Next
                        End If
                    End If
                    Thread.Sleep(RSSRefreshInterval)
                End While
            Catch ex As ThreadInterruptedException
                Wdbg(DebugLevel.W, "Aborting refresher...")
            End Try
        End Sub

        ''' <summary>
        ''' Show a headline on login
        ''' </summary>
        Sub ShowHeadlineLogin()
            If ShowHeadlineOnLogin Then
                Try
                    Dim Feed As New RSSFeed(RssHeadlineUrl, RSSFeedType.Infer)
                    If Not Feed.FeedArticles.Count = 0 Then
                        Write(DoTranslation("Latest news:") + " ", False, ColTypes.ListEntry)
                        Write(Feed.FeedArticles(0).ArticleTitle, True, ColTypes.ListValue)
                    End If
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to get latest news: {0}", ex.Message)
                    WStkTrc(ex)
                    Write(DoTranslation("Failed to get the latest news."), True, ColTypes.Error)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Opens the feed selector
        ''' </summary>
        Sub OpenFeedSelector()
            Dim StepNumber As Integer = 1
            Dim FeedListJson As JToken
            Dim FeedListJsonCountries() As JToken = Array.Empty(Of JToken)
            Dim FeedListJsonNewsSources() As JToken = Array.Empty(Of JToken)
            Dim FeedListJsonNewsSourceFeeds() As JToken = Array.Empty(Of JToken)
            Dim SelectedCountryIndex As Integer = 0
            Dim SelectedNewsSourceIndex As Integer
            Dim SelectedNewsSourceFeedIndex As Integer
            Dim FinalFeedUrl As String = ""

            'Try to get the feed list
            Try
                Write(DoTranslation("Downloading feed list..."), True, ColTypes.Progress)
                If String.IsNullOrEmpty(FeedListJsonText) Then FeedListJsonText = DownloadString("https://cdn.jsdelivr.net/gh/yavuz/news-feed-list-of-countries@master/news-feed-list-of-countries.json")
                FeedListJson = JToken.Parse(FeedListJsonText)
                FeedListJsonCountries = FeedListJson.SelectTokens("*").Where(Function(c) c("newSources").Any()).ToArray
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to get feed list: {0}", ex.Message)
                WStkTrc(ex)
                Write(DoTranslation("Failed to download feed list."), True, ColTypes.Error)
            End Try

            'Country selection
            While StepNumber = 1
                'If the JSON token is actually full, show the list of countries
                Console.Clear()
                WriteWhere(DoTranslation("Select your country by pressing the arrow left or arrow right keys. Press ENTER to confirm your selection."), 0, 1, False, ColTypes.Neutral)
                Write(NewLine + NewLine + "   < ", False, ColTypes.Gray)

                'The cursor positions for the arrow elements
                Dim MaxLength As Integer = FeedListJsonCountries.Max(Function(x) x("name").ToString.Length)
                Dim ItemName As String = $"{FeedListJsonCountries(SelectedCountryIndex)("name")} [{FeedListJsonCountries(SelectedCountryIndex)("iso")}]"
                Dim ArrowLeftXPosition As Integer = Console.CursorLeft + MaxLength + $" [{FeedListJsonCountries(SelectedCountryIndex)("iso")}]".Length
                Dim ItemNameXPosition As Integer = Console.CursorLeft + ((ArrowLeftXPosition - Console.CursorLeft) / 2) - (ItemName.Length / 2)
                WriteWhere(ItemName, ItemNameXPosition, Console.CursorTop, True, ColTypes.Option)
                WriteWhere(" >", ArrowLeftXPosition, Console.CursorTop, False, ColTypes.Gray)
                Write(NewLine + NewLine + DoTranslation("This country has {0} news sources."), True, ColTypes.Neutral, FeedListJsonCountries(SelectedCountryIndex)("newSources").Count)

                'Read and get response
                Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey(True)
                Wdbg(DebugLevel.I, "Keypress: {0}", ConsoleResponse.Key.ToString)
                If ConsoleResponse.Key = ConsoleKey.LeftArrow Then
                    'Decrement country index by 1
                    Wdbg(DebugLevel.I, "Decrementing number...")
                    If SelectedCountryIndex = 0 Then
                        Wdbg(DebugLevel.I, "Reached zero! Back to country index {0}.", FeedListJsonCountries.Length - 1)
                        SelectedCountryIndex = FeedListJsonCountries.Length - 1
                    Else
                        SelectedCountryIndex -= 1
                        Wdbg(DebugLevel.I, "Decremented to {0}", SelectedCountryIndex)
                    End If
                ElseIf ConsoleResponse.Key = ConsoleKey.RightArrow Then
                    'Increment country index by 1
                    If SelectedCountryIndex = FeedListJsonCountries.Length - 1 Then
                        Wdbg(DebugLevel.I, "Reached maximum country number! Back to zero.")
                        SelectedCountryIndex = 0
                    Else
                        SelectedCountryIndex += 1
                        Wdbg(DebugLevel.I, "Incremented to {0}", SelectedCountryIndex)
                    End If
                ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                    'Go to the next step
                    Wdbg(DebugLevel.I, "Selected country: {0}", FeedListJsonCountries(SelectedCountryIndex)("name"))
                    FeedListJsonNewsSources = FeedListJsonCountries(SelectedCountryIndex)("newSources").ToArray
                    Console.WriteLine()
                    StepNumber += 1
                End If
            End While

            'News source selection
            Write(DoTranslation("Select your favorite news source by writing the number. Press ENTER to confirm your selection.") + NewLine, True, ColTypes.Neutral)
            For SourceIndex As Integer = 0 To FeedListJsonNewsSources.Length - 1
                Dim NewsSource As JToken = FeedListJsonNewsSources(SourceIndex)
                Dim NewsSourceTitle As String = NewsSource("site")("title").ToString().Trim
                Write("{0}) {1}", True, ColTypes.Option, SourceIndex + 1, NewsSourceTitle)
            Next
            Console.WriteLine()
            While StepNumber = 2
                'Print input
                Wdbg(DebugLevel.W, "{0} news sources.", FeedListJsonNewsSources.Length)
                Write(">> ", False, ColTypes.Input)

                'Read and parse the answer
                Dim AnswerStr As String = ReadLine()
                If IsStringNumeric(AnswerStr) Then
                    'Got a numeric string! Check to see if we're in range before parsing it to index
                    Dim AnswerInt As Integer = AnswerStr
                    Wdbg(DebugLevel.W, "Got answer {0}.", AnswerInt)
                    If AnswerInt > 0 And AnswerInt <= FeedListJsonNewsSources.Length Then
                        Wdbg(DebugLevel.W, "Answer is in range.")
                        SelectedNewsSourceIndex = AnswerInt - 1
                        FeedListJsonNewsSourceFeeds = FeedListJsonNewsSources(SelectedNewsSourceIndex)("feedUrls").ToArray
                        Console.WriteLine()
                        StepNumber += 1
                    Else
                        Wdbg(DebugLevel.W, "Answer is out of range.")
                        Write(DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), True, ColTypes.Error, FeedListJsonNewsSources.Length)
                    End If
                ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                    Wdbg(DebugLevel.W, "Answer is not numeric.")
                    Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                Else
                    'We're aborting
                    Exit Sub
                End If
            End While

            'News feed selection
            Write(DoTranslation("Select a feed for your favorite news source. Press ENTER to confirm your selection.") + NewLine, True, ColTypes.Neutral)
            For SourceFeedIndex As Integer = 0 To FeedListJsonNewsSourceFeeds.Length - 1
                Dim NewsSourceFeed As JToken = FeedListJsonNewsSourceFeeds(SourceFeedIndex)
                Dim NewsSourceTitle As String = NewsSourceFeed("title")
                If String.IsNullOrEmpty(NewsSourceTitle) Then
                    'Some feeds like the French nouvelobs.com (Obs) don't have their feed title, so take it from the site title instead
                    NewsSourceTitle = FeedListJsonNewsSources(SelectedNewsSourceIndex)("site")("title")
                End If
                NewsSourceTitle = NewsSourceTitle.Trim()
                Write("{0}) {1}: {2}", True, ColTypes.Option, SourceFeedIndex + 1, NewsSourceTitle, NewsSourceFeed("url"))
            Next
            Console.WriteLine()
            While StepNumber = 3
                'Print input
                Wdbg(DebugLevel.W, "{0} news source feeds.", FeedListJsonNewsSourceFeeds.Length)
                Write(">> ", False, ColTypes.Input)

                'Read and parse the answer
                Dim AnswerStr As String = ReadLine()
                If IsStringNumeric(AnswerStr) Then
                    'Got a numeric string! Check to see if we're in range before parsing it to index
                    Dim AnswerInt As Integer = AnswerStr
                    Wdbg(DebugLevel.W, "Got answer {0}.", AnswerInt)
                    If AnswerInt > 0 And AnswerInt <= FeedListJsonNewsSourceFeeds.Length Then
                        Wdbg(DebugLevel.W, "Answer is in range.")
                        SelectedNewsSourceFeedIndex = AnswerInt - 1
                        FinalFeedUrl = FeedListJsonNewsSourceFeeds(SelectedNewsSourceFeedIndex)("url")
                        StepNumber += 1
                    Else
                        Wdbg(DebugLevel.W, "Answer is out of range.")
                        Write(DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), True, ColTypes.Error, FeedListJsonNewsSourceFeeds.Length)
                    End If
                ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                    Wdbg(DebugLevel.W, "Answer is not numeric.")
                    Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                Else
                    'We're aborting
                    Exit Sub
                End If
            End While

            'Actually change the feed
            RSSFeedLink = FinalFeedUrl
        End Sub

    End Module
End Namespace
