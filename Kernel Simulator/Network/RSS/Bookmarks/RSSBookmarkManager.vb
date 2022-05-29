
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

Imports KS.Kernel.Exceptions
Imports KS.Network.RSS

Public Module RSSBookmarkManager

    Private ReadOnly RssBookmarks As New List(Of String)

    ''' <summary>
    ''' Adds the current RSS feed to the bookmarks
    ''' </summary>
    Public Sub AddRSSFeedToBookmark()
        If Not String.IsNullOrEmpty(RSSFeedLink) Then
            AddRSSFeedToBookmark(RSSFeedLink)
        Else
            Wdbg(DebugLevel.W, "Trying to add null feed link to bookmarks. Ignored.")
        End If
    End Sub

    ''' <summary>
    ''' Adds the RSS feed URL to the bookmarks
    ''' </summary>
    ''' <param name="FeedURL">The feed URL to parse</param>
    Public Sub AddRSSFeedToBookmark(FeedURL As String)
        If Not String.IsNullOrEmpty(FeedURL) Then
            Try
                'Form a URI of feed
                Dim FinalFeedUri As New Uri(FeedURL)
                Dim FinalFeedUriString As String = FinalFeedUri.AbsoluteUri

                'Add the feed to bookmarks if not found
                If Not RssBookmarks.Contains(FinalFeedUriString) Then
                    Wdbg(DebugLevel.I, "Adding {0} to feed bookmark list...", FinalFeedUriString)
                    RssBookmarks.Add(FinalFeedUriString)
                End If
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to add {0} to RSS bookmarks: {1}", FeedURL, ex.Message)
                WStkTrc(ex)
                If ex.GetType.Name = NameOf(UriFormatException) Then
                    Wdbg(DebugLevel.E, "Verify that {0} is actually valid.", FeedURL)
                    Throw New InvalidFeedLinkException(DoTranslation("Failed to parse feed URL:") + " {0}", ex.Message)
                Else
                    Throw New InvalidFeedException(DoTranslation("Failed to parse feed URL:") + " {0}", ex.Message)
                End If
            End Try
        Else
            Wdbg(DebugLevel.W, "Trying to add null feed link to bookmarks. Ignored.")
        End If
    End Sub

    ''' <summary>
    ''' Gets the bookmark URL from number
    ''' </summary>
    Public Function GetBookmark(Num As Integer) As String
        'Return nothing if there are no bookmarks
        If RssBookmarks.Count = 0 Then Return ""

        'Get the bookmark
        If Num <= 0 Then Num = 1
        Return RssBookmarks(Num - 1)
    End Function

End Module
