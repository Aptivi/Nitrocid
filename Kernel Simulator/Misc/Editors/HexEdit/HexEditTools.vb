
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

Imports Extensification.ArrayExts
Imports Extensification.LongExts
Imports System.IO
Imports System.Threading

Namespace Misc.HexEdit
    Public Module HexEditTools

        ''' <summary>
        ''' Opens the binary file
        ''' </summary>
        ''' <param name="File">Target file. We recommend you to use <see cref="NeutralizePath(String, Boolean)"></see> to neutralize path.</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function HexEdit_OpenBinaryFile(File As String) As Boolean
            Try
                Wdbg(DebugLevel.I, "Trying to open file {0}...", File)
                HexEdit_FileStream = New FileStream(File, FileMode.Open)
                Wdbg(DebugLevel.I, "File {0} is open. Length: {1}, Pos: {2}", File, HexEdit_FileStream.Length, HexEdit_FileStream.Position)

                'Read the file
                Dim FileBytes(HexEdit_FileStream.Length) As Byte
                HexEdit_FileStream.Read(FileBytes, 0, HexEdit_FileStream.Length)
                HexEdit_FileStream.Seek(0, SeekOrigin.Begin)

                'Add the information to the arrays
                HexEdit_FileBytes = FileBytes
                HexEdit_FileBytesOrig = FileBytes
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Closes binary file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function HexEdit_CloseBinaryFile() As Boolean
            Try
                Wdbg(DebugLevel.I, "Trying to close file...")
                HexEdit_FileStream.Close()
                HexEdit_FileStream = Nothing
                Wdbg(DebugLevel.I, "File is no longer open.")
                HexEdit_FileBytes = {}
                HexEdit_FileBytesOrig = {}
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Closing file failed: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Saves binary file
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function HexEdit_SaveBinaryFile() As Boolean
            Try
                Dim FileBytes() As Byte = HexEdit_FileBytes
                Wdbg(DebugLevel.I, "Trying to save file...")
                HexEdit_FileStream.SetLength(0)
                Wdbg(DebugLevel.I, "Length set to 0.")
                HexEdit_FileStream.Write(FileBytes, 0, FileBytes.Length)
                HexEdit_FileStream.Flush()
                Wdbg(DebugLevel.I, "File is saved.")
                HexEdit_FileBytesOrig = FileBytes
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Saving file failed: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Handles autosave
        ''' </summary>
        Public Sub HexEdit_HandleAutoSaveBinaryFile()
            If HexEdit_AutoSaveFlag Then
                Try
                    Thread.Sleep(HexEdit_AutoSaveInterval * 1000)
                    If HexEdit_FileStream IsNot Nothing Then
                        HexEdit_SaveBinaryFile()
                    End If
                Catch ex As Exception
                    WStkTrc(ex)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Was binary edited?
        ''' </summary>
        Function HexEdit_WasHexEdited() As Boolean
            If HexEdit_FileBytes IsNot Nothing And HexEdit_FileBytesOrig IsNot Nothing Then
                Return Not HexEdit_FileBytes.SequenceEqual(HexEdit_FileBytesOrig)
            End If
            Return False
        End Function

        ''' <summary>
        ''' Adds a new byte to the current hex
        ''' </summary>
        ''' <param name="Content">New byte content</param>
        Public Sub HexEdit_AddNewByte(Content As Byte)
            If HexEdit_FileStream IsNot Nothing Then
                HexEdit_FileBytes.Add(Content)
            Else
                Throw New InvalidOperationException(DoTranslation("The hex editor hasn't opened a file stream yet."))
            End If
        End Sub

        ''' <summary>
        ''' Adds the new bytes to the current hex
        ''' </summary>
        ''' <param name="Bytes">New bytes</param>
        Public Sub HexEdit_AddNewBytes(Bytes() As Byte)
            If HexEdit_FileStream IsNot Nothing Then
                For Each ByteContent As Byte In Bytes
                    HexEdit_FileBytes.Add(ByteContent)
                Next
            Else
                Throw New InvalidOperationException(DoTranslation("The hex editor hasn't opened a file stream yet."))
            End If
        End Sub

        ''' <summary>
        ''' Deletes a byte
        ''' </summary>
        ''' <param name="ByteNumber">The byte number</param>
        Public Sub HexEdit_DeleteByte(ByteNumber As Long)
            If HexEdit_FileStream IsNot Nothing Then
                Dim FileBytesList As List(Of Byte) = HexEdit_FileBytes.ToList
                Dim ByteIndex As Long = ByteNumber - 1
                Wdbg(DebugLevel.I, "Byte index: {0}, number: {1}", ByteIndex, ByteNumber)
                Wdbg(DebugLevel.I, "File length: {0}", HexEdit_FileBytes.LongLength)

                'Actually remove a byte
                If ByteNumber <= HexEdit_FileBytes.LongLength Then
                    FileBytesList.RemoveAt(ByteIndex)
                    Wdbg(DebugLevel.I, "Removed {0}. Result: {1}", ByteIndex, HexEdit_FileBytes.LongLength)
                    HexEdit_FileBytes = FileBytesList.ToArray
                Else
                    Throw New ArgumentOutOfRangeException(NameOf(ByteNumber), ByteNumber, DoTranslation("The specified byte number may not be larger than the file size."))
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("The hex editor hasn't opened a file stream yet."))
            End If
        End Sub

        ''' <summary>
        ''' Deletes the bytes
        ''' </summary>
        ''' <param name="StartByteNumber">Start from the byte number</param>
        Public Sub HexEdit_DeleteBytes(StartByteNumber As Long)
            HexEdit_DeleteBytes(StartByteNumber, HexEdit_FileBytes.LongLength)
        End Sub

        ''' <summary>
        ''' Deletes the bytes
        ''' </summary>
        ''' <param name="StartByteNumber">Start from the byte number</param>
        ''' <param name="EndByteNumber">Ending byte number</param>
        Public Sub HexEdit_DeleteBytes(StartByteNumber As Long, EndByteNumber As Long)
            If HexEdit_FileStream IsNot Nothing Then
                StartByteNumber.SwapIfSourceLarger(EndByteNumber)
                Dim StartByteNumberIndex As Long = StartByteNumber - 1
                Dim EndByteNumberIndex As Long = EndByteNumber - 1
                Dim FileBytesList As List(Of Byte) = HexEdit_FileBytes.ToList
                Wdbg(DebugLevel.I, "Start byte number: {0}, end: {1}", StartByteNumber, EndByteNumber)
                Wdbg(DebugLevel.I, "Got start byte index: {0}", StartByteNumberIndex)
                Wdbg(DebugLevel.I, "Got end byte index: {0}", EndByteNumberIndex)
                Wdbg(DebugLevel.I, "File length: {0}", HexEdit_FileBytes.LongLength)

                'Actually remove the bytes
                If StartByteNumber <= HexEdit_FileBytes.LongLength And EndByteNumber <= HexEdit_FileBytes.LongLength Then
                    For ByteNumber As Long = EndByteNumber To StartByteNumber Step -1
                        FileBytesList.RemoveAt(ByteNumber - 1)
                    Next
                    Wdbg(DebugLevel.I, "Removed {0} to {1}. New length: {2}", StartByteNumber, EndByteNumber, HexEdit_FileBytes.LongLength)
                    HexEdit_FileBytes = FileBytesList.ToArray
                ElseIf StartByteNumber > HexEdit_FileBytes.LongLength Then
                    Throw New ArgumentOutOfRangeException(NameOf(StartByteNumber), StartByteNumber, DoTranslation("The specified start byte number may not be larger than the file size."))
                ElseIf EndByteNumber > HexEdit_FileBytes.LongLength Then
                    Throw New ArgumentOutOfRangeException(NameOf(EndByteNumber), EndByteNumber, DoTranslation("The specified end byte number may not be larger than the file size."))
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("The hex editor hasn't opened a file stream yet."))
            End If
        End Sub

        ''' <summary>
        ''' Renders the file in hex
        ''' </summary>
        Public Sub HexEdit_DisplayHex()
            HexEdit_DisplayHex(1, HexEdit_FileBytes.LongLength)
        End Sub

        ''' <summary>
        ''' Renders the file in hex
        ''' </summary>
        Public Sub HexEdit_DisplayHex(Start As Long)
            HexEdit_DisplayHex(Start, HexEdit_FileBytes.LongLength)
        End Sub

        ''' <summary>
        ''' Renders the file in hex
        ''' </summary>
        Public Sub HexEdit_DisplayHex(StartByte As Long, EndByte As Long)
            If HexEdit_FileStream IsNot Nothing Then
                Wdbg(DebugLevel.I, "File Bytes: {0}", HexEdit_FileBytes.LongLength)
                StartByte.SwapIfSourceLarger(EndByte)
                If StartByte <= HexEdit_FileBytes.LongLength And EndByte <= HexEdit_FileBytes.LongLength Then
                    'We need to know how to write the bytes and their contents in this shape:
                    '-> 0x00000010  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                    '   0x00000020  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                    '   0x00000030  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
                    '... and so on.
                    Dim ByteIterationEachSixteen As Integer = 1 + Math.Truncate(StartByte / 16)
                    WriteWhere($"0x{16 * ByteIterationEachSixteen:X8}", 0, Console.CursorTop, False, ColTypes.ListEntry)
                    Dim ByteWritePositionX As Integer = Console.CursorLeft + 2
                    Dim ByteCharWritePositionX As Integer = 61 + (ByteWritePositionX - 12)
                    Dim ByteNumberEachSixteen As Integer = 1
                    For CurrentByteNumber As Long = StartByte To EndByte
                        'Write the byte and the contents
                        Wdbg(DebugLevel.I, "Byte write position: {0}", ByteWritePositionX)
                        Wdbg(DebugLevel.I, "Byte char write position: {0}", ByteCharWritePositionX)
                        Wdbg(DebugLevel.I, "Byte iteration each sixteen: {0}", ByteIterationEachSixteen)
                        Wdbg(DebugLevel.I, "Byte number each sixteen: {0}", ByteNumberEachSixteen)
                        Dim CurrentByte As Byte = HexEdit_FileBytes(CurrentByteNumber - 1)
                        Wdbg(DebugLevel.I, "Byte: {0}", CurrentByte)
                        Dim ProjectedByteChar As Char = Convert.ToChar(CurrentByte)
                        Wdbg(DebugLevel.I, "Projected byte char: {0}", ProjectedByteChar)
                        Dim RenderedByteChar As Char = "."c
                        If Not Char.IsWhiteSpace(ProjectedByteChar) Then
                            'The renderer will actually render the character, not as a dot.
                            Wdbg(DebugLevel.I, "Char is not a whitespace.")
                            RenderedByteChar = ProjectedByteChar
                        End If
                        Wdbg(DebugLevel.I, "Rendered byte char: {0}", ProjectedByteChar)
                        WriteWhere($"{CurrentByte:X2}", ByteWritePositionX + (3 * (ByteNumberEachSixteen - 1)), Console.CursorTop, False, ColTypes.ListValue)
                        WriteWhere($"{RenderedByteChar}", ByteCharWritePositionX + (ByteNumberEachSixteen - 1), Console.CursorTop, False, ColTypes.ListValue)

                        'Increase the byte number
                        ByteNumberEachSixteen += 1

                        'Check to see if we've exceeded 16 bytes
                        If ByteNumberEachSixteen > 16 Then
                            'OK, let's increase the byte iteration and get the next line ready
                            ByteIterationEachSixteen += 1
                            Write("", True, ColTypes.Neutral)
                            WriteWhere($"0x{16 * ByteIterationEachSixteen:X8}", 0, Console.CursorTop, False, ColTypes.ListEntry)
                            ByteWritePositionX = Console.CursorLeft + 2
                            ByteCharWritePositionX = 61 + (ByteWritePositionX - 12)
                            ByteNumberEachSixteen = 1
                        End If
                    Next
                    Write("", True, ColTypes.Neutral)
                ElseIf StartByte > HexEdit_FileBytes.LongLength Then
                    Write(DoTranslation("The specified start byte number may not be larger than the file size."), True, ColTypes.Error)
                ElseIf EndByte > HexEdit_FileBytes.LongLength Then
                    Write(DoTranslation("The specified end byte number may not be larger than the file size."), True, ColTypes.Error)
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("The hex editor hasn't opened a file stream yet."))
            End If
        End Sub

        ''' <summary>
        ''' Queries the byte and displays the results
        ''' </summary>
        Public Sub HexEdit_QueryByteAndDisplay(ByteContent As Byte)
            HexEdit_QueryByteAndDisplay(ByteContent, 1, HexEdit_FileBytes.LongLength)
        End Sub

        ''' <summary>
        ''' Queries the byte and displays the results
        ''' </summary>
        Public Sub HexEdit_QueryByteAndDisplay(ByteContent As Byte, Start As Long)
            HexEdit_QueryByteAndDisplay(ByteContent, Start, HexEdit_FileBytes.LongLength)
        End Sub

        ''' <summary>
        ''' Queries the byte and displays the results
        ''' </summary>
        Public Sub HexEdit_QueryByteAndDisplay(ByteContent As Byte, StartByte As Long, EndByte As Long)
            If HexEdit_FileStream IsNot Nothing Then
                Wdbg(DebugLevel.I, "File Bytes: {0}", HexEdit_FileBytes.LongLength)
                If StartByte <= HexEdit_FileBytes.LongLength And EndByte <= HexEdit_FileBytes.LongLength Then
                    For ByteNumber As Long = StartByte To EndByte
                        If HexEdit_FileBytes(ByteNumber - 1) = ByteContent Then
                            Dim ByteRenderStart As Long = ByteNumber - 2
                            Dim ByteRenderEnd As Long = ByteNumber + 2
                            Write($"- 0x{ByteNumber:X8}: ", False, ColTypes.ListEntry)
                            For ByteRenderNumber As Long = ByteRenderStart To ByteRenderEnd
                                If ByteRenderStart < 0 Then ByteRenderStart = 1
                                If ByteRenderEnd > HexEdit_FileBytes.LongLength Then ByteRenderEnd = HexEdit_FileBytes.LongLength
                                Dim UseHighlight As Boolean = HexEdit_FileBytes(ByteRenderNumber - 1) = ByteContent
                                Dim CurrentByte As Byte = HexEdit_FileBytes(ByteRenderNumber - 1)
                                Wdbg(DebugLevel.I, "Byte: {0}", CurrentByte)
                                Dim ProjectedByteChar As Char = Convert.ToChar(CurrentByte)
                                Wdbg(DebugLevel.I, "Projected byte char: {0}", ProjectedByteChar)
                                Dim RenderedByteChar As Char = "."c
                                If Not Char.IsWhiteSpace(ProjectedByteChar) Then
                                    'The renderer will actually render the character, not as a dot.
                                    Wdbg(DebugLevel.I, "Char is not a whitespace.")
                                    RenderedByteChar = ProjectedByteChar
                                End If
                                Write($"0x{ByteRenderNumber:X2}({RenderedByteChar}) ", False, If(UseHighlight, ColTypes.Success, ColTypes.ListValue))
                            Next
                            Write("", True, ColTypes.Neutral)
                        End If
                    Next
                ElseIf StartByte > HexEdit_FileBytes.LongLength Then
                    Write(DoTranslation("The specified start byte number may not be larger than the file size."), True, ColTypes.Error)
                ElseIf EndByte > HexEdit_FileBytes.LongLength Then
                    Write(DoTranslation("The specified end byte number may not be larger than the file size."), True, ColTypes.Error)
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("The hex editor hasn't opened a file stream yet."))
            End If
        End Sub

        ''' <summary>
        ''' Replaces every occurence of a byte with the replacement
        ''' </summary>
        ''' <param name="FromByte">Byte to be replaced</param>
        ''' <param name="WithByte">Byte to replace with</param>
        Public Sub HexEdit_Replace(FromByte As Byte, WithByte As Byte)
            If HexEdit_FileStream IsNot Nothing Then
                Wdbg(DebugLevel.I, "Source: {0}, Target: {1}", FromByte, WithByte)
                For ByteIndex As Integer = 0 To HexEdit_FileBytes.Count - 1
                    If HexEdit_FileBytes(ByteIndex) = FromByte Then
                        Wdbg(DebugLevel.I, "Replacing ""{0}"" with ""{1}"" in byte {2}", FromByte, WithByte, ByteIndex + 1)
                        HexEdit_FileBytes(ByteIndex) = WithByte
                    End If
                Next
            Else
                Throw New InvalidOperationException(DoTranslation("The hex editor hasn't opened a file stream yet."))
            End If
        End Sub

    End Module
End Namespace