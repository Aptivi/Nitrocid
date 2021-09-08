
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

Imports System.IO
Imports System.Text

Public Module TextEditTools

    ''' <summary>
    ''' Opens the text file
    ''' </summary>
    ''' <param name="File">Target file. We recommend you to use <see cref="NeutralizePath(String, Boolean)"></see> to neutralize path.</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function TextEdit_OpenTextFile(File As String) As Boolean
        Try
            Wdbg("I", "Trying to open file {0}...", File)
            TextEdit_FileStream = New FileStream(File, FileMode.Open)
            If TextEdit_FileLines Is Nothing Then TextEdit_FileLines = New List(Of String)
            If TextEdit_FileLinesOrig Is Nothing Then TextEdit_FileLinesOrig = New List(Of String)
            Wdbg("I", "File {0} is open. Length: {1}, Pos: {2}", File, TextEdit_FileStream.Length, TextEdit_FileStream.Position)
            Dim TextFileStreamReader As New StreamReader(TextEdit_FileStream)
            Do While Not TextFileStreamReader.EndOfStream
                Dim StreamLine As String = TextFileStreamReader.ReadLine
                TextEdit_FileLines.Add(StreamLine)
                TextEdit_FileLinesOrig.Add(StreamLine)
            Loop
            TextEdit_FileStream.Seek(0, SeekOrigin.Begin)
            Return True
        Catch ex As Exception
            Wdbg("E", "Open file {0} failed: {1}", File, ex.Message)
            WStkTrc(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Closes text file
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function TextEdit_CloseTextFile() As Boolean
        Try
            Wdbg("I", "Trying to close file...")
            TextEdit_FileStream.Close()
            TextEdit_FileStream = Nothing
            Wdbg("I", "File is no longer open.")
            TextEdit_FileLines.Clear()
            TextEdit_FileLinesOrig.Clear()
            Return True
        Catch ex As Exception
            Wdbg("E", "Closing file failed: {0}", ex.Message)
            WStkTrc(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Saves text file
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function TextEdit_SaveTextFile(ClearLines As Boolean) As Boolean
        Try
            Wdbg("I", "Trying to save file...")
            TextEdit_FileStream.SetLength(0)
            Wdbg("I", "Length set to 0.")
            Dim FileLinesByte() As Byte = Encoding.Default.GetBytes(TextEdit_FileLines.ToArray.Join(vbNewLine))
            Wdbg("I", "Converted lines to bytes. Length: {0}", FileLinesByte.Length)
            TextEdit_FileStream.Write(FileLinesByte, 0, FileLinesByte.Length)
            TextEdit_FileStream.Flush()
            Wdbg("I", "File is saved.")
            If ClearLines Then
                TextEdit_FileLines.Clear()
            End If
            TextEdit_FileLinesOrig.Clear()
            TextEdit_FileLinesOrig.AddRange(TextEdit_FileLines)
            Return True
        Catch ex As Exception
            Wdbg("E", "Saving file failed: {0}", ex.Message)
            WStkTrc(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Handles autosave
    ''' </summary>
    Public Sub TextEdit_HandleAutoSaveTextFile()
        If TextEdit_AutoSaveFlag Then
            Try
                Threading.Thread.Sleep(TextEdit_AutoSaveInterval * 1000)
                If TextEdit_FileStream IsNot Nothing Then
                    TextEdit_SaveTextFile(False)
                End If
            Catch ex As Exception
                WStkTrc(ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Was text edited?
    ''' </summary>
    Function TextEdit_WasTextEdited() As Boolean
        If TextEdit_FileLines IsNot Nothing And TextEdit_FileLinesOrig IsNot Nothing Then
            Return Not TextEdit_FileLines.SequenceEqual(TextEdit_FileLinesOrig)
        End If
        Return False
    End Function

    ''' <summary>
    ''' Adds a new line to the current text
    ''' </summary>
    ''' <param name="Content">New line content</param>
    Public Sub TextEdit_AddNewLine(Content As String)
        If TextEdit_FileStream IsNot Nothing Then
            TextEdit_FileLines.Add(Content)
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Sub

    ''' <summary>
    ''' Removes a line from the current text
    ''' </summary>
    ''' <param name="LineNumber">The line number to remove</param>
    Public Sub TextEdit_RemoveLine(LineNumber As Integer)
        If TextEdit_FileStream IsNot Nothing Then
            Dim LineIndex As Integer = LineNumber - 1
            Wdbg("I", "Got line index: {0}", LineIndex)
            Wdbg("I", "Old file lines: {0}", TextEdit_FileLines.Count)
            If LineNumber <= TextEdit_FileLines.Count Then
                TextEdit_FileLines.RemoveAt(LineIndex)
                Wdbg("I", "New file lines: {0}", TextEdit_FileLines.Count)
            Else
                Throw New ArgumentOutOfRangeException(NameOf(LineNumber), LineNumber, DoTranslation("The specified line number may not be larger than the last file line number."))
            End If
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Sub

    ''' <summary>
    ''' Replaces every occurence of a string with the replacement
    ''' </summary>
    ''' <param name="From">String to be replaced</param>
    ''' <param name="[With]">String to replace with</param>
    Public Sub TextEdit_Replace(From As String, [With] As String)
        If String.IsNullOrEmpty(From) Then Throw New ArgumentNullException(NameOf(From))
        If TextEdit_FileStream IsNot Nothing Then
            Wdbg("I", "Source: {0}, Target: {1}", From, [With])
            For LineIndex As Integer = 0 To TextEdit_FileLines.Count - 1
                Wdbg("I", "Replacing ""{0}"" with ""{1}"" in line {2}", From, [With], LineIndex + 1)
                TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Replace(From, [With])
            Next
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Sub

    ''' <summary>
    ''' Replaces every occurence of a string with the replacement
    ''' </summary>
    ''' <param name="From">String to be replaced</param>
    ''' <param name="[With]">String to replace with</param>
    ''' <param name="LineNumber">The line number</param>
    Public Sub TextEdit_Replace(From As String, [With] As String, LineNumber As Integer)
        If String.IsNullOrEmpty(From) Then Throw New ArgumentNullException(NameOf(From))
        If TextEdit_FileStream IsNot Nothing Then
            Wdbg("I", "Source: {0}, Target: {1}, Line Number: {2}", From, [With], LineNumber)
            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
            Dim LineIndex As Long = LineNumber - 1
            If LineNumber <= TextEdit_FileLines.Count Then
                Wdbg("I", "Replacing ""{0}"" with ""{1}"" in line {2}", From, [With], LineIndex + 1)
                TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Replace(From, [With])
            Else
                Throw New ArgumentOutOfRangeException(NameOf(LineNumber), LineNumber, DoTranslation("The specified line number may not be larger than the last file line number."))
            End If
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Sub

    ''' <summary>
    ''' Deletes a word or a phrase from the line
    ''' </summary>
    ''' <param name="Word">The word or phrase</param>
    ''' <param name="LineNumber">The line number</param>
    Public Sub TextEdit_DeleteWord(Word As String, LineNumber As Integer)
        If String.IsNullOrEmpty(Word) Then Throw New ArgumentNullException(NameOf(Word))
        If TextEdit_FileStream IsNot Nothing Then
            Dim LineIndex As Integer = LineNumber - 1
            Wdbg("I", "Word/Phrase: {0}, Line: {1}", Word, LineNumber)
            Wdbg("I", "Got line index: {0}", LineIndex)
            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
            If LineNumber <= TextEdit_FileLines.Count Then
                TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Replace(Word, "")
                Wdbg("I", "Removed {0}. Result: {1}", LineIndex, TextEdit_FileLines.Count)
            Else
                Throw New ArgumentOutOfRangeException(NameOf(LineNumber), LineNumber, DoTranslation("The specified line number may not be larger than the last file line number."))
            End If
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Sub

    ''' <summary>
    ''' Deletes a character from the line
    ''' </summary>
    ''' <param name="CharNumber">The character number</param>
    ''' <param name="LineNumber">The line number</param>
    Public Sub TextEdit_DeleteChar(CharNumber As Integer, LineNumber As Integer)
        If TextEdit_FileStream IsNot Nothing Then
            Dim LineIndex As Integer = LineNumber - 1
            Dim CharIndex As Integer = CharNumber - 1
            Wdbg("I", "Char number: {0}, Line: {1}", CharNumber, LineNumber)
            Wdbg("I", "Got line index: {0}", LineIndex)
            Wdbg("I", "Got char index: {0}", CharIndex)
            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
            If LineNumber <= TextEdit_FileLines.Count Then
                TextEdit_FileLines(LineIndex) = TextEdit_FileLines(LineIndex).Remove(CharIndex, 1)
                Wdbg("I", "Removed {0}. Result: {1}", LineIndex, TextEdit_FileLines(LineIndex))
            Else
                Throw New ArgumentOutOfRangeException(NameOf(LineNumber), LineNumber, DoTranslation("The specified line number may not be larger than the last file line number."))
            End If
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Sub

    ''' <summary>
    ''' Queries a character in all lines.
    ''' </summary>
    ''' <param name="Char">The character to query</param>
    Public Function TextEdit_QueryChar([Char] As Char) As Dictionary(Of Integer, Dictionary(Of Integer, String))
        If TextEdit_FileStream IsNot Nothing Then
            Dim Lines As New Dictionary(Of Integer, Dictionary(Of Integer, String))
            Dim Results As New Dictionary(Of Integer, String)
            Wdbg("I", "Char: {0}", [Char])
            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
            For LineIndex As Integer = 0 To TextEdit_FileLines.Count - 1
                For CharIndex As Integer = 0 To TextEdit_FileLines(LineIndex).Length - 1
                    If TextEdit_FileLines(LineIndex)(CharIndex) = [Char] Then
                        Results.Add(CharIndex, TextEdit_FileLines(LineIndex))
                    End If
                Next
                Lines.Add(LineIndex, New Dictionary(Of Integer, String)(Results))
                Results.Clear()
            Next
            Return Lines
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Function

    ''' <summary>
    ''' Queries a character in specific line.
    ''' </summary>
    ''' <param name="Char">The character to query</param>
    ''' <param name="LineNumber">The line number</param>
    Public Function TextEdit_QueryChar([Char] As Char, LineNumber As Integer) As Dictionary(Of Integer, String)
        If TextEdit_FileStream IsNot Nothing Then
            Dim LineIndex As Integer = LineNumber - 1
            Dim Results As New Dictionary(Of Integer, String)
            Wdbg("I", "Char: {0}, Line: {1}", [Char], LineNumber)
            Wdbg("I", "Got line index: {0}", LineIndex)
            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
            If LineNumber <= TextEdit_FileLines.Count Then
                For CharIndex As Integer = 0 To TextEdit_FileLines(LineIndex).Length - 1
                    If TextEdit_FileLines(LineIndex)(CharIndex) = [Char] Then
                        Results.Add(CharIndex, TextEdit_FileLines(LineIndex))
                    End If
                Next
            Else
                Throw New ArgumentOutOfRangeException(NameOf(LineNumber), LineNumber, DoTranslation("The specified line number may not be larger than the last file line number."))
            End If
            Return Results
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Function

    ''' <summary>
    ''' Queries a word in all lines.
    ''' </summary>
    ''' <param name="Word">The word to query</param>
    Public Function TextEdit_QueryWord(Word As String) As Dictionary(Of Integer, Dictionary(Of Integer, String))
        If TextEdit_FileStream IsNot Nothing Then
            Dim Lines As New Dictionary(Of Integer, Dictionary(Of Integer, String))
            Dim Results As New Dictionary(Of Integer, String)
            Wdbg("I", "Word: {0}", Word)
            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
            For LineIndex As Integer = 0 To TextEdit_FileLines.Count - 1
                Dim Words() As String = TextEdit_FileLines(LineIndex).Split(" ")
                For WordIndex As Integer = 0 To Words.Length - 1
                    If Words(WordIndex).ToLower.Contains(Word.ToLower) Then
                        Results.Add(WordIndex, TextEdit_FileLines(LineIndex))
                    End If
                Next
                Lines.Add(LineIndex, New Dictionary(Of Integer, String)(Results))
                Results.Clear()
            Next
            Return Lines
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Function

    ''' <summary>
    ''' Queries a word in specific line.
    ''' </summary>
    ''' <param name="Word">The word to query</param>
    ''' <param name="LineNumber">The line number</param>
    Public Function TextEdit_QueryWord(Word As String, LineNumber As Integer) As Dictionary(Of Integer, String)
        If TextEdit_FileStream IsNot Nothing Then
            Dim LineIndex As Integer = LineNumber - 1
            Dim Results As New Dictionary(Of Integer, String)
            Wdbg("I", "Word: {0}, Line: {1}", Word, LineNumber)
            Wdbg("I", "Got line index: {0}", LineIndex)
            Wdbg("I", "File lines: {0}", TextEdit_FileLines.Count)
            If LineNumber <= TextEdit_FileLines.Count Then
                Dim Words() As String = TextEdit_FileLines(LineIndex).Split(" ")
                For WordIndex As Integer = 0 To Words.Length - 1
                    If Words(WordIndex).ToLower.Contains(Word.ToLower) Then
                        Results.Add(WordIndex, TextEdit_FileLines(LineIndex))
                    End If
                Next
            Else
                Throw New ArgumentOutOfRangeException(NameOf(LineNumber), LineNumber, DoTranslation("The specified line number may not be larger than the last file line number."))
            End If
            Return Results
        Else
            Throw New InvalidOperationException(DoTranslation("The text editor hasn't opened a file stream yet."))
        End If
    End Function

End Module
