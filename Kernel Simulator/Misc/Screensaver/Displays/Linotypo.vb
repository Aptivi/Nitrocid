
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

Public Module LinotypoDisplay

    Friend WithEvents Linotypo As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Plain
    ''' </summary>
    Sub Linotypo_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Linotypo.DoWork
        Console.Clear()
        Console.CursorVisible = False
        Try
            'Sanity checks
            If LinotypoTextColumns <= 0 Then
                'We can't do zero columns! We would print nothing!
                LinotypoTextColumns = 1
            ElseIf LinotypoTextColumns > 3 Then
                'We can't exceed three columns as we would like to make the lines readable in all the columns, especially in small terminals (80x24)
                LinotypoTextColumns = 3
            End If
            If LinotypoEtaoinThreshold <= 0 Then
                'We can't have a negative threshold!
                LinotypoEtaoinThreshold = 1
            ElseIf LinotypoEtaoinThreshold > 8 Then
                'Writers usually spot errors after less than or equal to eight characters written.
                LinotypoEtaoinThreshold = 8
            End If

            'Some variables
            Dim RandomDriver As New Random()
            Dim CpmSpeedMin As Integer = LinotypoWritingSpeedMin * 5
            Dim CpmSpeedMax As Integer = LinotypoWritingSpeedMax * 5
            Dim MaxCharacters As Integer = ((Console.WindowWidth - 2) / LinotypoTextColumns) - 2
            Dim CurrentColumn As Integer = 1
            Dim CurrentColumnRowConsole As Integer = Console.CursorLeft
            Dim ColumnRowConsoleThreshold As Integer = Console.WindowWidth / LinotypoTextColumns

            'Strikes
            Dim Strikes As New List(Of String) From {"q`12wsa", "r43edfgt5", "u76yhjki8", "p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa "}
            Dim CapStrikes As New List(Of String) From {"Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:""{_+}|", "?"":> ", "M<LKJN ", "VBHGFC ", "ZXDSA "}
            Dim CapSymbols As String = "~!@$#%&^*)(:""{_+}|?><"
            Dim LinotypeLayout(,) As String = {{"e", "t", "a", "o", "i", "n", " "}, {"s", "h", "r", "d", "l", "u", " "},
                                               {"c", "m", "f", "w", "y", "p", " "}, {"v", "b", "g", "k", "q", "j", " "},
                                               {"x", "z", " ", " ", " ", " ", " "}, {" ", " ", " ", " ", " ", " ", " "}}

            'Logic. As always.
            Do While True
                SleepNoBlock(LinotypoDelay, Linotypo)
                If Linotypo.CancellationPending = True Then
                    Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg("I", "All clean. Linotypo screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    'Variables
                    Dim CountingCharacters As Boolean
                    Dim CharacterCounter As Integer
                    Dim EtaoinMode As Boolean
                    Dim CappedEtaoin As Boolean
                    Dim LinotypeWrite As String = LinotypoWrite

                    'Linotypo can also deal with files written on the field that is used for storing text, so check to see if the path exists.
                    If TryParsePath(LinotypoWrite) AndAlso File.Exists(LinotypoWrite) Then
                        'File found! Now, write the contents of it to the local variable that stores the actual written text.
                        LinotypeWrite = File.ReadAllText(LinotypoWrite)
                    End If

                    'For each line, write four spaces, and extra two spaces if paragraph starts.
                    For Each Paragraph As String In LinotypeWrite.SplitNewLines
                        If Linotypo.CancellationPending Then Exit For

                        'Sometimes, a paragraph could consist of nothing, but prints its new line, so honor this by checking to see if we need to
                        'clear screen or advance to the next column so that we don't mess up the display by them
                        HandleNextColumn(CurrentColumn, CurrentColumnRowConsole, ColumnRowConsoleThreshold)

                        'We need to make sure that we indent spaces for each new paragraph.
                        If CurrentColumn = 1 Then
                            Console.WriteLine()
                        Else
                            Console.SetCursorPosition(CurrentColumnRowConsole, Console.CursorTop + 1)
                        End If
                        Console.Write("    ")
                        Dim NewLineDone As Boolean = True

                        'Split the paragraph into sentences that have the length of maximum characters that can be printed for each column
                        'in various terminal sizes. This enables us to easily tell if we're going to re-write the line after a typo and the
                        'line completion that consists of "Etaoin shrdlu" and other nonsense written sometimes on newspapers or ads back in
                        'the early 20th century.
                        Dim IncompleteSentences As New List(Of String)
                        Dim IncompleteSentenceBuilder As New StringBuilder
                        Dim CharactersParsed As Integer = 0

                        'This reserved characters count tells us how many spaces are used for indenting the paragraph. This is only four for
                        'the first time and will be reverted back to zero after the incomplete sentence is formed.
                        Dim ReservedCharacters As Integer = 4
                        For Each ParagraphChar As Char In Paragraph
                            If Linotypo.CancellationPending Then Exit For

                            'Append the character into the incomplete sentence builder.
                            IncompleteSentenceBuilder.Append(ParagraphChar)
                            CharactersParsed += 1

                            'Check to see if we're at the maximum character number
                            If IncompleteSentenceBuilder.Length = MaxCharacters - ReservedCharacters Or Paragraph.Length = CharactersParsed Then
                                'We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                                IncompleteSentences.Add(IncompleteSentenceBuilder.ToString)

                                'Clean everything up
                                IncompleteSentenceBuilder.Clear()
                                ReservedCharacters = 0
                            End If
                        Next

                        'Get struck character and write it
                        For IncompleteSentenceIndex As Integer = 0 To IncompleteSentences.Count - 1
                            Dim IncompleteSentence As String = IncompleteSentences(IncompleteSentenceIndex)
                            If Linotypo.CancellationPending Then Exit For

                            'Check if we need to indent a sentence
                            If Not NewLineDone Then
                                If CurrentColumn = 1 Then
                                    Console.WriteLine()
                                Else
                                    Console.SetCursorPosition(CurrentColumnRowConsole, Console.CursorTop + 1)
                                End If
                            End If
                            Console.Write("  ")

                            'We need to store which column and which key from the linotype keyboard layout is taken.
                            Dim LinotypeColumnIndex As Integer = 0
                            Dim LinotypeKeyIndex As Integer = 0
                            Dim LinotypeMaxColumnIndex As Integer = 5

                            'Process the incomplete sentences
                            For StruckCharIndex As Integer = 0 To IncompleteSentence.Length - 1
                                If Linotypo.CancellationPending Then Exit For

                                'Sometimes, typing error can be made in the last line and the line is repeated on the first line in the different
                                'column, but it ruins the overall beautiful look of the paragraphs, considering how it is split in columns. We
                                'need to re-indent the sentence.
                                If Console.CursorTop = 0 Then
                                    If CurrentColumn = 1 Then
                                        Console.WriteLine()
                                    Else
                                        Console.SetCursorPosition(CurrentColumnRowConsole, Console.CursorTop + 1)
                                    End If
                                    Console.Write("  ")
                                    If IncompleteSentenceIndex = 0 Then Console.Write("    ")
                                End If

                                'Select a character
                                Dim StruckChar As Char = IncompleteSentence(StruckCharIndex)

                                'Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                                Dim SelectedCpm As Integer = RandomDriver.Next(CpmSpeedMin, CpmSpeedMax)
                                Dim WriteMs As Integer = (60 / SelectedCpm) * 1000

                                'Choose a character depending on the current mode
                                If EtaoinMode Then
                                    'Doing this in linotype machines after spotting an error usually triggers a speed boost, because the authors
                                    'that used this machine back then considered it as a quick way to fill the faulty line.
                                    WriteMs /= 1 + RandomDriver.NextDouble

                                    'Get the character
                                    StruckChar = LinotypeLayout(LinotypeColumnIndex, LinotypeKeyIndex)
                                    If CappedEtaoin Then StruckChar = Char.ToUpper(StruckChar)

                                    'Advance the indexes of column and key, depending on their values, and get the character
                                    Select Case LinotypoEtaoinType
                                        Case FillType.EtaoinComplete, FillType.EtaoinPattern
                                            If LinotypoEtaoinType = FillType.EtaoinPattern Then LinotypeMaxColumnIndex = 1

                                            'Increment the key (and optionally column) index. If both exceed the max limit, reset both to zero.
                                            LinotypeKeyIndex += 1
                                            If LinotypeKeyIndex = 7 Then
                                                LinotypeKeyIndex = 0
                                                LinotypeColumnIndex += 1
                                            End If
                                            If LinotypeColumnIndex = LinotypeMaxColumnIndex + 1 Then
                                                LinotypeColumnIndex = 0
                                                LinotypeKeyIndex = 0
                                            End If
                                        Case FillType.RandomChars
                                            'Randomly select the linotype indexes
                                            LinotypeColumnIndex = RandomDriver.Next(0, 5)
                                            LinotypeKeyIndex = RandomDriver.Next(0, 6)
                                    End Select
                                Else
                                    'See if the typo is guaranteed
                                    Dim Probability As Double = If(LinotypoMissStrikePossibility >= 5, 5, LinotypoMissStrikePossibility) / 100
                                    Dim LinotypoGuaranteed As Boolean = Rnd() < Probability
                                    If LinotypoGuaranteed Then
                                        'Sometimes, a typo is generated by missing a character.
                                        Dim MissProbability As Double = If(LinotypoMissPossibility >= 10, 10, LinotypoMissPossibility) / 100
                                        Dim MissGuaranteed As Boolean = Rnd() < MissProbability
                                        If MissGuaranteed Then
                                            'Miss is guaranteed. Simulate the missed character
                                            StruckChar = ""
                                        Else
                                            'Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                                            Dim StrikeCharsIndex As Integer
                                            Dim StruckFound As Boolean = False
                                            Dim CappedStrike As Boolean = False
                                            Dim StrikesString As String = ""
                                            Do Until StruckFound
                                                StrikeCharsIndex = RandomDriver.Next(0, Strikes.Count - 1)
                                                CappedStrike = Char.IsUpper(StruckChar) Or CapSymbols.Contains(StruckChar)
                                                StrikesString = If(CappedStrike, CapStrikes(StrikeCharsIndex), Strikes(StrikeCharsIndex))
                                                StruckFound = Not String.IsNullOrEmpty(StrikesString) AndAlso StrikesString.Contains(StruckChar)
                                            Loop

                                            'Select a random character that is a typo from the selected strike index
                                            Dim RandomStrikeIndex As Integer = RandomDriver.Next(0, StrikesString.Length - 1)
                                            Dim MistypedChar As Char = StrikesString(RandomStrikeIndex)
                                            If "`-=\][';/.,".Contains(MistypedChar) And CappedStrike Then
                                                'The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                                                MistypedChar = CapStrikes(StrikeCharsIndex)(RandomStrikeIndex)
                                            End If
                                            StruckChar = MistypedChar
                                        End If

                                        'Randomly select whether or not to turn on the capped Etaoin
                                        Dim CappingProbability As Double = If(LinotypoEtaoinCappingPossibility >= 10, 10, LinotypoEtaoinCappingPossibility) / 100
                                        CappedEtaoin = Rnd() < CappingProbability

                                        'Trigger character counter mode
                                        CountingCharacters = True
                                    End If
                                End If

                                'Write the final character to the console and wait
                                If Not StruckChar = vbNullChar Then Console.Write(StruckChar)
                                SleepNoBlock(WriteMs, Linotypo)

                                'If we're on the character counter mode, increment this for every character until the "line fill" mode starts
                                If CountingCharacters Then
                                    CharacterCounter += 1
                                    If CharacterCounter > LinotypoEtaoinThreshold Then
                                        'We've reached the Etaoin threshold. Turn on that mode and stop counting characters.
                                        EtaoinMode = True
                                        CountingCharacters = False
                                        CharacterCounter = 0
                                    End If
                                End If

                                'If we're on the Etaoin mode and we've reached the end of incomplete sentence, reset the index to 0 and do the
                                'necessary changes.
                                If EtaoinMode And (StruckCharIndex = MaxCharacters - 1 Or StruckCharIndex = IncompleteSentence.Length - 1) Then
                                    StruckCharIndex = -1
                                    EtaoinMode = False
                                    If Console.CursorTop >= Console.WindowHeight - 2 Then
                                        HandleNextColumn(CurrentColumn, CurrentColumnRowConsole, ColumnRowConsoleThreshold)
                                    Else
                                        If CurrentColumn = 1 Then
                                            Console.WriteLine()
                                        Else
                                            Console.SetCursorPosition(CurrentColumnRowConsole, Console.CursorTop + 1)
                                        End If
                                        Console.Write("  ")
                                    End If
                                End If
                            Next

                            'Let the next sentence generate a new line
                            NewLineDone = False

                            'Clean things up
                            LinotypeColumnIndex = 0
                            LinotypeKeyIndex = 0

                            'It's possible that the writer might have made an error on writing a line on the very end of it where the threshold is
                            'lower than the partial sentence being written, so don't do the Etaoin pattern in this case, but re-write the text as
                            'if the error is being made.
                            If CountingCharacters Then
                                CountingCharacters = False
                                CharacterCounter = 0
                                IncompleteSentenceIndex -= 1
                            End If

                            'See if the current cursor is on the bottom so we can make the second column, if we have more than a column assigned
                            HandleNextColumn(CurrentColumn, CurrentColumnRowConsole, ColumnRowConsoleThreshold)
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
            Wdbg("I", "All clean. Linotypo screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

    ''' <summary>
    ''' Instructs the Linotypo screensaver to go to the next column
    ''' </summary>
    ''' <param name="CurrentColumn"></param>
    ''' <param name="CurrentColumnRowConsole"></param>
    ''' <param name="ColumnRowConsoleThreshold"></param>
    Sub HandleNextColumn(ByRef CurrentColumn As Integer, ByRef CurrentColumnRowConsole As Integer, ByVal ColumnRowConsoleThreshold As Integer)
        If LinotypoTextColumns > 1 Then
            If Console.CursorTop >= Console.WindowHeight - 2 Then
                'We're on the bottom, so...
                If CurrentColumn >= LinotypoTextColumns Then
                    '...wait until retry
                    Console.WriteLine()
                    SleepNoBlock(LinotypoNewScreenDelay, Linotypo)

                    '...and make a new screen
                    Console.Clear()
                    CurrentColumn = 1
                    CurrentColumnRowConsole = Console.CursorLeft
                Else
                    '...we're moving to the next column
                    CurrentColumn += 1
                    CurrentColumnRowConsole += ColumnRowConsoleThreshold
                    Console.SetCursorPosition(CurrentColumnRowConsole, 0)
                End If
            End If
        ElseIf LinotypoTextColumns = 1 And Console.CursorTop >= Console.WindowHeight - 2 Then
            'We're on the bottom, so wait until retry...
            Console.WriteLine()
            SleepNoBlock(LinotypoNewScreenDelay, Linotypo)

            '...and make a new screen
            Console.Clear()
            CurrentColumn = 1
            CurrentColumnRowConsole = Console.CursorLeft
        End If
    End Sub

    Public Enum FillType
        ''' <summary>
        ''' Prints the known pattern of etaoin characters, such as: etaoin shrdlu
        ''' </summary>
        EtaoinPattern
        ''' <summary>
        ''' Prints the complete pattern of etaoin characters, such as: etaoin shrdlu cmfwyp vbgkqj xz
        ''' </summary>
        EtaoinComplete
        ''' <summary>
        ''' Prints the random set of characters to rapidly fill in the line
        ''' </summary>
        RandomChars
    End Enum

End Module
