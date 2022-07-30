
'    Kernel Simulator  Copyright (C) 2018-2019  Aptivi
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

Imports System.Text

Namespace ManPages
    Public Class Manual

        ''' <summary>
        ''' The manual page title
        ''' </summary>
        Public ReadOnly Property Title As String
        ''' <summary>
        ''' The manual page revision
        ''' </summary>
        Public ReadOnly Property Revision As String
        ''' <summary>
        ''' The body string (the contents of manual)
        ''' </summary>
        Public ReadOnly Property Body As StringBuilder
        ''' <summary>
        ''' The list of todos
        ''' </summary>
        Public ReadOnly Property Todos As List(Of String)
        ''' <summary>
        ''' Is the manual page valid?
        ''' </summary>
        Public ReadOnly Property ValidManpage As Boolean

        ''' <summary>
        ''' Makes a new instance of manual
        ''' </summary>
        Friend Sub New(ManualFileName As String)
            Dim Title As String = ""
            Dim Revision As String = ""
            Dim Body As New StringBuilder
            Dim Todos As New List(Of String)
            ValidManpage = CheckManual(ManualFileName, Title, Revision, Body, Todos)
            If ValidManpage Then
                Me.Title = Title
                Me.Revision = Revision
                Me.Body = Body
                Me.Todos = Todos
            End If
        End Sub

    End Class
End Namespace