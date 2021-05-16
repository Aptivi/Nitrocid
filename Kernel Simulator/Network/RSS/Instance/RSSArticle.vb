Imports System.Xml

Public Class RSSArticle

    ''' <summary>
    ''' RSS Article Title
    ''' </summary>
    Public ReadOnly ArticleTitle As String
    ''' <summary>
    ''' RSS Article Link
    ''' </summary>
    Public ReadOnly ArticleLink As String
    ''' <summary>
    ''' RSS Article Descirption
    ''' </summary>
    Public ReadOnly ArticleDescription As String
    ''' <summary>
    ''' RSS Article Parameters
    ''' </summary>
    Public ReadOnly ArticleVariables As Dictionary(Of String, XmlNode)

    ''' <summary>
    ''' Makes a new instance of RSS article
    ''' </summary>
    ''' <param name="ArticleTitle"></param>
    ''' <param name="ArticleLink"></param>
    ''' <param name="ArticleDescription"></param>
    ''' <param name="ArticleVariables"></param>
    Public Sub New(ByVal ArticleTitle As String, ByVal ArticleLink As String, ByVal ArticleDescription As String, ByVal ArticleVariables As Dictionary(Of String, XmlNode))
        Me.ArticleTitle = ArticleTitle
        Me.ArticleLink = ArticleLink
        Me.ArticleDescription = ArticleDescription
        Me.ArticleVariables = ArticleVariables
    End Sub

End Class
