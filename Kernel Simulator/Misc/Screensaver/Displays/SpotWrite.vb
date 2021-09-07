
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

Imports System.ComponentModel
Imports System.IO
Imports System.Text

Module SpotWriteDisplay

    Public WithEvents SpotWrite As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    Sub SpotWrite_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles SpotWrite.DoWork
        Console.Clear()
        Console.CursorVisible = False
        Try
            Dim RandomDriver As New Random()
            Dim TypeWrite As String = SpotWriteWrite
            Do While True
                SleepNoBlock(SpotWriteDelay, SpotWrite)
                If SpotWrite.CancellationPending = True Then
                    Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg("I", "All clean. SpotWrite screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    'SpotWrite can also deal with files written on the field that is used for storing text, so check to see if the path exists.
                    If TryParsePath(SpotWriteWrite) AndAlso File.Exists(SpotWriteWrite) Then
                        'File found! Now, write the contents of it to the local variable that stores the actual written text.
                        WdbgConditional(ScreensaverDebug, "I", "Opening file {0} to write...", SpotWriteWrite)
                        TypeWrite = File.ReadAllText(SpotWriteWrite)
                    End If

                    'For each line, write four spaces, and extra two spaces if paragraph starts.
                    For Each Paragraph As String In TypeWrite.SplitNewLines
                        If SpotWrite.CancellationPending Then Exit For
                        WdbgConditional(ScreensaverDebug, "I", "New paragraph: {0}", Paragraph)

                        'Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
                        'sizes.
                        Dim IncompleteSentences As New List(Of String)
                        Dim IncompleteSentenceBuilder As New StringBuilder
                        Dim CharactersParsed As Integer = 0

                        'This reserved characters count tells us how many spaces are used for indenting the paragraph. This is only four for
                        'the first time and will be reverted back to zero after the incomplete sentence is formed.
                        Dim ReservedCharacters As Integer = 4
                        For Each ParagraphChar As Char In Paragraph
                            If SpotWrite.CancellationPending Then Exit For

                            'Append the character into the incomplete sentence builder.
                            IncompleteSentenceBuilder.Append(ParagraphChar)
                            CharactersParsed += 1

                            'Check to see if we're at the maximum character number
                            If IncompleteSentenceBuilder.Length = Console.WindowWidth - 2 - ReservedCharacters Or Paragraph.Length = CharactersParsed Then
                                'We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                                WdbgConditional(ScreensaverDebug, "I", "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString, IncompleteSentences.Count)
                                IncompleteSentences.Add(IncompleteSentenceBuilder.ToString)

                                'Clean everything up
                                IncompleteSentenceBuilder.Clear()
                                ReservedCharacters = 0
                            End If
                        Next

                        'Prepare display (make a paragraph indentation)
                        If Not Console.CursorTop = Console.WindowHeight - 2 Then
                            Console.SetCursorPosition(0, Console.CursorTop + 1)
                            Console.Write("    ")
                            WdbgConditional(ScreensaverDebug, "I", "Indented in {0}, {1}", Console.CursorLeft, Console.CursorTop)
                        End If

                        'Get struck character and write it
                        For SentenceIndex As Integer = 0 To IncompleteSentences.Count - 1
                            Dim Sentence As String = IncompleteSentences(SentenceIndex)
                            For Each StruckChar As Char In Sentence
                                If SpotWrite.CancellationPending Then Exit For

                                'If we're at the end of the page, clear the screen
                                If Console.CursorTop = Console.WindowHeight - 2 Then
                                    SleepNoBlock(SpotWriteNewScreenDelay, SpotWrite)
                                    Console.Clear()
                                    Console.WriteLine()
                                    If SentenceIndex = 0 Then
                                        Console.Write("    ")
                                    Else
                                        Console.Write(" ")
                                    End If
                                    WdbgConditional(ScreensaverDebug, "I", "Indented in {0}, {1}", Console.CursorLeft, Console.CursorTop)
                                End If

                                'Write the final character to the console and wait
                                Console.Write(GetEsc() + "[1K" + StruckChar + GetEsc() + "[K")
                                SleepNoBlock(SpotWriteDelay, SpotWrite)
                            Next
                            Console.Write(GetEsc() + "[1K")
                        Next
                    Next
                End If
            Loop
        Catch ex As Exception
            Wdbg("W", "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg("I", "All clean. SpotWrite screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
