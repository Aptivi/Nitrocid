
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

Imports System.IO
Imports System.Text

Public Class StreamReaderExtensions

    Public Function ReadLineWithNewLine(ByVal reader As StreamReader) As String

        'Define a new string builder
        Dim strBuilder As New StringBuilder()

        'Read files as integers and convert the integer to string
        While Not (reader.EndOfStream)
            Dim readFile As Integer = reader.Read
            strBuilder.Append(ChrW(readFile))
            If (readFile = 10) Then
                Exit While 'For C-Sharp KS, use break;
            End If
        End While
        Return strBuilder.ToString()

    End Function

End Class
