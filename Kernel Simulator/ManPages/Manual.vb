
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Public Class Manual

    Private Title As String
    Private Revision As String
    Private LayoutVersion As String
    Private Body_builder As New StringBuilder
    Private Color_dict As New Dictionary(Of String, ConsoleColor)
    Private Section_dict As New Dictionary(Of String, String)
    Private Todo_dict As New List(Of String)

    Public Property ManualTitle() As String
        Get
            Return Title
        End Get
        Set(ByVal Value As String)
            Title = Value
        End Set
    End Property

    Public Property ManualRevision() As String
        Get
            Return Revision
        End Get
        Set(ByVal Value As String)
            Revision = Value
        End Set
    End Property

    Public Property ManualLayoutVersion() As String
        Get
            Return LayoutVersion
        End Get
        Set(ByVal Value As String)
            LayoutVersion = Value
        End Set
    End Property

    Public Property Body() As StringBuilder
        Get
            Return Body_builder
        End Get
        Set(ByVal Value As StringBuilder)
            Body_builder = Value
        End Set
    End Property

    Public Property Colors() As Dictionary(Of String, ConsoleColor)
        Get
            Return Color_dict
        End Get
        Set(ByVal Value As Dictionary(Of String, ConsoleColor))
            Color_dict = Value
        End Set
    End Property

    Public Property Sections() As Dictionary(Of String, String)
        Get
            Return Section_dict
        End Get
        Set(ByVal Value As Dictionary(Of String, String))
            Section_dict = Value
        End Set
    End Property

    Public Property Todos() As List(Of String)
        Get
            Return Todo_dict
        End Get
        Set(ByVal Value As List(Of String))
            Todo_dict = Value
        End Set
    End Property

    Public Sub New(ByVal Title As String)
        If (AvailablePages.Contains(Title)) Then
            ManualTitle = Title
        End If
    End Sub

End Class
