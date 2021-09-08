
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
    Public Sub New(ArticleTitle As String, ArticleLink As String, ArticleDescription As String, ArticleVariables As Dictionary(Of String, XmlNode))
        Me.ArticleTitle = ArticleTitle
        Me.ArticleLink = ArticleLink
        Me.ArticleDescription = ArticleDescription
        Me.ArticleVariables = ArticleVariables
    End Sub

End Class
