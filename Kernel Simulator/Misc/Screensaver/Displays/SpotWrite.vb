
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

Imports System.Threading
Imports System.IO
Imports System.Text

Namespace Misc.Screensaver.Displays
    Module SpotWriteDisplay

        Public SpotWrite As New KernelThread("SpotWrite screensaver thread", True, AddressOf SpotWrite_DoWork)

        Sub SpotWrite_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim TypeWrite As String = SpotWriteWrite
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(SpotWriteTextColor))
                Console.Clear()

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False

                    'SpotWrite can also deal with files written on the field that is used for storing text, so check to see if the path exists.
                    Wdbg(DebugLevel.I, "Checking ""{0}"" to see if it's a file path", SpotWriteWrite)
                    If TryParsePath(SpotWriteWrite) AndAlso FileExists(SpotWriteWrite) Then
                        'File found! Now, write the contents of it to the local variable that stores the actual written text.
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", SpotWriteWrite)
                        TypeWrite = File.ReadAllText(SpotWriteWrite)
                    End If

                    'For each line, write four spaces, and extra two spaces if paragraph starts.
                    For Each Paragraph As String In TypeWrite.SplitNewLines
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "New paragraph: {0}", Paragraph)

                        'Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
                        'sizes.
                        Dim IncompleteSentences As New List(Of String)
                        Dim IncompleteSentenceBuilder As New StringBuilder
                        Dim CharactersParsed As Integer = 0

                        'This reserved characters count tells us how many spaces are used for indenting the paragraph. This is only four for
                        'the first time and will be reverted back to zero after the incomplete sentence is formed.
                        Dim ReservedCharacters As Integer = 4
                        For Each ParagraphChar As Char In Paragraph
                            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                            If ResizeSyncing Then Exit For

                            'Append the character into the incomplete sentence builder.
                            IncompleteSentenceBuilder.Append(ParagraphChar)
                            CharactersParsed += 1

                            'Check to see if we're at the maximum character number
                            If IncompleteSentenceBuilder.Length = Console.WindowWidth - 2 - ReservedCharacters Or Paragraph.Length = CharactersParsed Then
                                'We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString, IncompleteSentences.Count)
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
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", Console.CursorLeft, Console.CursorTop)
                        End If

                        'Get struck character and write it
                        For SentenceIndex As Integer = 0 To IncompleteSentences.Count - 1
                            Dim Sentence As String = IncompleteSentences(SentenceIndex)
                            If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                            If ResizeSyncing Then Exit For
                            For Each StruckChar As Char In Sentence
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If ResizeSyncing Then Exit For

                                'If we're at the end of the page, clear the screen
                                If Console.CursorTop = Console.WindowHeight - 2 Then
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're at the end of the page! {0} = {1}", Console.CursorTop, Console.WindowHeight - 2)
                                    SleepNoBlock(SpotWriteNewScreenDelay, SpotWrite)
                                    Console.Clear()
                                    Console.WriteLine()
                                    If SentenceIndex = 0 Then
                                        Console.Write("    ")
                                    Else
                                        Console.Write(" ")
                                    End If
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", Console.CursorLeft, Console.CursorTop)
                                End If

                                'Write the final character to the console and wait
                                Console.Write(GetEsc() + "[1K" + StruckChar + GetEsc() + "[K")
                                SleepNoBlock(SpotWriteDelay, SpotWrite)
                            Next
                            Console.Write(GetEsc() + "[1K")
                        Next
                    Next

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(SpotWriteDelay, SpotWrite)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
