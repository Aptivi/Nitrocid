
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Namespace ManPages
    Public Module PageManager

        'Variables
        Friend Pages As New Dictionary(Of String, Manual)

        ''' <summary>
        ''' Lists all manual pages
        ''' </summary>
        Public Function ListAllPages() As Dictionary(Of String, Manual)
            Return ListAllPages("")
        End Function

        ''' <summary>
        ''' Lists all manual pages
        ''' </summary>
        ''' <param name="SearchTerm">Keywords to search</param>
        Public Function ListAllPages(SearchTerm As String) As Dictionary(Of String, Manual)
            If String.IsNullOrEmpty(SearchTerm) Then
                Return Pages
            Else
                Dim FoundPages As New Dictionary(Of String, Manual)
                For Each ManualPage As String In Pages.Keys
                    If ManualPage.Contains(SearchTerm) Then
                        FoundPages.Add(ManualPage, Pages(ManualPage))
                    End If
                Next
                Return FoundPages
            End If
        End Function

        ''' <summary>
        ''' Adds a manual page to the pages list
        ''' </summary>
        ''' <param name="Name">Manual page name</param>
        ''' <param name="Page">Manual page instance</param>
        Public Sub AddManualPage(Name As String, Page As Manual)
            'Check to see if title is defined
            If String.IsNullOrWhiteSpace(Name) Then
                Wdbg(DebugLevel.W, "Title not defined.")
                Name = DoTranslation("Untitled manual page") + $" {Pages.Count}"
            End If

            'Add the page if valid
            If Not Pages.ContainsKey(Name) Then
                If Page.ValidManpage Then
                    Pages.Add(Name, Page)
                Else
                    Throw New Exceptions.InvalidManpageException(DoTranslation("The manual page {0} is invalid."), Name)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Removes a manual page from the list
        ''' </summary>
        ''' <param name="Name">Manual page name</param>
        Public Function RemoveManualPage(Name As String) As Boolean
            Return Pages.Remove(Name)
        End Function

    End Module
End Namespace