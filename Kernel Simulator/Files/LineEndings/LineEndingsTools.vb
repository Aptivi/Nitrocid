
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

Imports Extensification.CharExts.Querying
Imports KS.Files.Querying
Imports System.IO

Namespace Files.LineEndings
    Public Module LineEndingsTools

        ''' <summary>
        ''' The new line style used for the current platform
        ''' </summary>
        Public ReadOnly Property NewlineStyle As FilesystemNewlineStyle
            Get
                Select Case Environment.NewLine
                    Case vbCrLf
                        Return FilesystemNewlineStyle.CRLF
                    Case vbLf
                        Return FilesystemNewlineStyle.LF
                    Case vbCr
                        Return FilesystemNewlineStyle.CR
                    Case Else
                        Return FilesystemNewlineStyle.CRLF
                End Select
            End Get
        End Property

        ''' <summary>
        ''' Gets the line ending string from the specified line ending style
        ''' </summary>
        ''' <param name="LineEndingStyle">Line ending style</param>
        Public Function GetLineEndingString(LineEndingStyle As FilesystemNewlineStyle) As String
            Select Case LineEndingStyle
                Case FilesystemNewlineStyle.CRLF
                    Return vbCrLf
                Case FilesystemNewlineStyle.LF
                    Return vbLf
                Case FilesystemNewlineStyle.CR
                    Return vbCr
                Case Else
                    Return Environment.NewLine
            End Select
        End Function

        ''' <summary>
        ''' Gets the line ending style from file
        ''' </summary>
        ''' <param name="TextFile">Target text file</param>
        Public Function GetLineEndingFromFile(TextFile As String) As FilesystemNewlineStyle
            ThrowOnInvalidPath(TextFile)
            TextFile = NeutralizePath(TextFile)
            If Not FileExists(TextFile) Then Throw New IOException(DoTranslation("File {0} not found.").FormatString(TextFile))

            'Open the file stream
            Dim NewlineStyle As FilesystemNewlineStyle = NewlineStyle
            Dim TextFileStream As New FileStream(TextFile, FileMode.Open, FileAccess.Read)
            Dim CarriageReturnCode As Integer = GetAsciiCode(GetLineEndingString(FilesystemNewlineStyle.CR))
            Dim LineFeedCode As Integer = GetAsciiCode(GetLineEndingString(FilesystemNewlineStyle.LF))
            Dim CarriageReturnSpotted As Boolean
            Dim LineFeedSpotted As Boolean
            Dim ExitOnSpotted As Boolean

            'Search for new line style
            Do Until TextFileStream.Position = TextFileStream.Length
                Dim Result As Integer = TextFileStream.ReadByte
                If Result = LineFeedCode Then
                    LineFeedSpotted = True
                    ExitOnSpotted = True
                End If
                If Result = CarriageReturnCode Then
                    CarriageReturnSpotted = True
                    ExitOnSpotted = True
                End If
                If ExitOnSpotted And (Result <> LineFeedCode And Result <> CarriageReturnCode) Then Exit Do
            Loop
            TextFileStream.Close()

            'Return the style used
            If LineFeedSpotted And CarriageReturnSpotted Then
                NewlineStyle = FilesystemNewlineStyle.CRLF
            ElseIf LineFeedSpotted Then
                NewlineStyle = FilesystemNewlineStyle.LF
            ElseIf CarriageReturnSpotted Then
                NewlineStyle = FilesystemNewlineStyle.CR
            End If
            Return NewlineStyle
        End Function

    End Module
End Namespace