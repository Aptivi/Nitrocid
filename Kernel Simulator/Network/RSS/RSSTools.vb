Imports System.Xml

Public Module RSSTools

    ''' <summary>
    ''' Make instances of RSS Article from feed node and type
    ''' </summary>
    ''' <param name="FeedNode">Feed XML node</param>
    ''' <param name="FeedType">Feed type</param>
    Function MakeRssArticlesFromFeed(ByVal FeedNode As XmlNodeList, ByVal FeedType As RSSFeedType) As List(Of RSSArticle)
#Disable Warning BC42104
        Dim Articles As New List(Of RSSArticle)
        Select Case FeedType
            Case RSSFeedType.RSS2
                For Each Node As XmlNode In FeedNode(0) '<channel>
                    For Each Child As XmlNode In Node.ChildNodes '<item>
                        Dim Parameters As New Dictionary(Of String, XmlNode)
                        Dim Title, Link, Description As String
                        If Child.Name = "item" Then
                            For Each ArticleNode As XmlNode In Child.ChildNodes 'Children of <item>
                                If ArticleNode.Name = "title" Then
                                    Title = ArticleNode.InnerText
                                End If
                                If ArticleNode.Name = "link" Then
                                    Link = ArticleNode.InnerText
                                End If
                                If ArticleNode.Name = "description" Then
                                    Description = ArticleNode.InnerText
                                End If
                                Parameters.Add(ArticleNode.Name, ArticleNode)
                            Next
                            Articles.Add(New RSSArticle(Title, Link, Description, Parameters))
                        End If
                    Next
                Next
            Case RSSFeedType.RSS1
                For Each Node As XmlNode In FeedNode(0) '<channel> or <item>
                    If Node.Name = "item" Then
                        Dim Parameters As New Dictionary(Of String, XmlNode)
                        Dim Title, Link, Description As String
                        For Each ArticleNode As XmlNode In Node.ChildNodes 'Children of <item>
                            If ArticleNode.Name = "title" Then
                                Title = ArticleNode.InnerText
                            End If
                            If ArticleNode.Name = "link" Then
                                Link = ArticleNode.InnerText
                            End If
                            If ArticleNode.Name = "description" Then
                                Description = ArticleNode.InnerText
                            End If
                            Parameters.Add(ArticleNode.Name, ArticleNode)
                        Next
                        Articles.Add(New RSSArticle(Title, Link, Description, Parameters))
                    End If
                Next
            Case RSSFeedType.Atom
                For Each Node As XmlNode In FeedNode(0) 'Children of <feed>
                    If Node.Name = "entry" Then
                        Dim Parameters As New Dictionary(Of String, XmlNode)
                        Dim Title, Link, Description As String
                        For Each ArticleNode As XmlNode In Node.ChildNodes 'Children of <entry>
                            If ArticleNode.Name = "title" Then
                                Title = ArticleNode.InnerText
                            End If
                            If ArticleNode.Name = "link" Then
                                Link = ArticleNode.InnerText
                            End If
                            If ArticleNode.Name = "summary" Or ArticleNode.Name = "content" Then
                                Description = ArticleNode.InnerText
                            End If
                            Parameters.Add(ArticleNode.Name, ArticleNode)
                        Next
                        Articles.Add(New RSSArticle(Title, Link, Description, Parameters))
                    End If
                Next
            Case Else
                Throw New Exceptions.InvalidFeedTypeException(DoTranslation("Invalid RSS feed type."))
        End Select
#Enable Warning BC42104
        Return Articles
    End Function

    ''' <summary>
    ''' Gets a feed property
    ''' </summary>
    ''' <param name="FeedProperty">Feed property name</param>
    ''' <param name="FeedNode">Feed XML node</param>
    ''' <param name="FeedType">Feed type</param>
    Function GetFeedProperty(ByVal FeedProperty As String, ByVal FeedNode As XmlNodeList, ByVal FeedType As RSSFeedType) As Object
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

End Module
