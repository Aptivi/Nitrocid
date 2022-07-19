
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

Imports KS.Files.Querying
Imports KS.Files.Read
Imports System.IO
Imports System.Text

Namespace Files.LineEndings
    Public Module LineEndingsConverter

        ''' <summary>
        ''' Converts the line endings to the newline style for the current platform
        ''' </summary>
        ''' <param name="TextFile">Text file name with extension or file path</param>
        Public Sub ConvertLineEndings(TextFile As String)
            ConvertLineEndings(TextFile, NewlineStyle)
        End Sub

        ''' <summary>
        ''' Converts the line endings to the specified newline style
        ''' </summary>
        ''' <param name="TextFile">Text file name with extension or file path</param>
        ''' <param name="LineEndingStyle">Line ending style</param>
        Public Sub ConvertLineEndings(TextFile As String, LineEndingStyle As FilesystemNewlineStyle)
            ThrowOnInvalidPath(TextFile)
            TextFile = NeutralizePath(TextFile)
            If Not FileExists(TextFile) Then Throw New IOException(DoTranslation("File {0} not found.").FormatString(TextFile))

            'Get all the file lines, regardless of the new line style on the target file
            Dim FileContents() As String = ReadAllLinesNoBlock(TextFile)
            Wdbg(DebugLevel.I, "Got {0} lines. Converting newlines in {1} to {2}...", FileContents.Length, TextFile, LineEndingStyle.ToString)

            'Get the newline string according to the current style
            Dim NewLineString As String = GetLineEndingString(LineEndingStyle)

            'Convert the newlines now
            Dim Result As New StringBuilder
            For Each FileContent As String In FileContents
                Result.Append(FileContent + NewLineString)
            Next

            'Save the changes
            File.WriteAllText(TextFile, Result.ToString)
        End Sub

    End Module
End Namespace
