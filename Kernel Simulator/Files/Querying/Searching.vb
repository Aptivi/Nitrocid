
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports System.Text.RegularExpressions

Namespace Files.Querying
    Public Module Searching

        ''' <summary>
        ''' Searches a file for string
        ''' </summary>
        ''' <param name="FilePath">File path</param>
        ''' <param name="StringLookup">String to find</param>
        ''' <returns>The list if successful; null if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function SearchFileForString(FilePath As String, StringLookup As String) As List(Of String)
            Try
                ThrowOnInvalidPath(FilePath)
                FilePath = NeutralizePath(FilePath)
                Dim Matches As New List(Of String)
                Dim Filebyte() As String = File.ReadAllLines(FilePath)
                Dim MatchNum As Integer = 1
                Dim LineNumber As Integer = 1
                For Each Str As String In Filebyte
                    If Str.Contains(StringLookup) Then
                        Matches.Add($"[{LineNumber}] " + DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str))
                        MatchNum += 1
                    End If
                    LineNumber += 1
                Next
                Return Matches
            Catch ex As Exception
                WStkTrc(ex)
                Throw New IOException(DoTranslation("Unable to find file to match string ""{0}"": {1}").FormatString(StringLookup, ex.Message))
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Searches a file for string using regexp
        ''' </summary>
        ''' <param name="FilePath">File path</param>
        ''' <param name="StringLookup">String to find</param>
        ''' <returns>The list if successful; null if unsuccessful</returns>
        ''' <exception cref="IOException"></exception>
        Public Function SearchFileForStringRegexp(FilePath As String, StringLookup As Regex) As List(Of String)
            Try
                ThrowOnInvalidPath(FilePath)
                FilePath = NeutralizePath(FilePath)
                Dim Matches As New List(Of String)
                Dim Filebyte() As String = File.ReadAllLines(FilePath)
                Dim MatchNum As Integer = 1
                Dim LineNumber As Integer = 1
                For Each Str As String In Filebyte
                    If StringLookup.IsMatch(Str) Then
                        Matches.Add($"[{LineNumber}] " + DoTranslation("Match {0}: {1}").FormatString(MatchNum, Str))
                        MatchNum += 1
                    End If
                    LineNumber += 1
                Next
                Return Matches
            Catch ex As Exception
                WStkTrc(ex)
                Throw New IOException(DoTranslation("Unable to find file to match string ""{0}"": {1}").FormatString(StringLookup, ex.Message))
            End Try
            Return Nothing
        End Function

    End Module
End Namespace
