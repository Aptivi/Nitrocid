
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Namespace ManPages
    Public Module PageViewer

        ''' <summary>
        ''' Manual page information style
        ''' </summary>
        Public ManpageInfoStyle As String = ""

        ''' <summary>
        ''' Previews the manual page
        ''' </summary>
        ''' <param name="ManualTitle">A manual title</param>
        Public Sub ViewPage(ManualTitle As String)
            If Pages.ContainsKey(ManualTitle) Then
                'Variables
                Dim InfoPlace As Integer

                'Get the bottom place
                InfoPlace = Console.WindowHeight - 1
                Wdbg(DebugLevel.I, "Bottom info height is {0}", InfoPlace)

                'If there is any To-do, write them to the console
                Wdbg(DebugLevel.I, "Todo count for ""{0}"": {1}", ManualTitle, Pages(ManualTitle).Todos.Count.ToString)
                If Pages(ManualTitle).Todos.Count <> 0 Then
                    Wdbg(DebugLevel.I, "Todos are found in manpage.")
                    Write(DoTranslation("This manual page needs work for:"), True, ColTypes.Warning)
                    WriteList(Pages(ManualTitle).Todos, True)
                    Write(NewLine + DoTranslation("Press any key to read the manual page..."), False, ColTypes.Warning)
                    Console.ReadKey()
                End If

                'Clear screen for readability
                Console.Clear()

                'Write the information to the console
                If Not String.IsNullOrWhiteSpace(ManpageInfoStyle) Then
                    WriteWhere(ProbePlaces(ManpageInfoStyle), Console.CursorLeft, InfoPlace, True, BackgroundColor, NeutralTextColor, Pages(ManualTitle).Title, Pages(ManualTitle).Revision)
                Else
                    WriteWhere(" {0} [v{1}] ", Console.CursorLeft, InfoPlace, True, BackgroundColor, NeutralTextColor, Pages(ManualTitle).Title, Pages(ManualTitle).Revision)
                End If

                'Disable blinking cursor
                Console.CursorVisible = False

                'Split the sentences to parts to deal with sentence lengths that are longer than the console window width
                Dim IncompleteSentences As New List(Of String)
                Dim IncompleteSentenceBuilder As New StringBuilder
                Dim CharactersParsed As Integer
                Dim EscapeCharacters As Integer
                Dim EscapeCharactersCountInside As Integer
                Dim InEsc As Boolean
                For Each line As String In Pages(ManualTitle).Body.ToString.SplitNewLines
                    CharactersParsed = 0
                    EscapeCharacters = 0
                    EscapeCharactersCountInside = 0

                    'Deal with empty lines
                    If String.IsNullOrEmpty(line) Then
                        IncompleteSentences.Add("")
                    End If

                    'Now, enumerate through each character in the string
                    For Each LineChar As Char In line
                        'If the character is Escape, run through the color change sequence until we reach "m"
                        If LineChar = GetEsc() Then InEsc = True
                        If InEsc Then EscapeCharactersCountInside += 1
                        If InEsc And (EscapeCharactersCountInside > 19 Or LineChar = "m") Then
                            EscapeCharacters += 1
                            InEsc = False
                        End If

                        'Append the character into the incomplete sentence builder.
                        IncompleteSentenceBuilder.Append(LineChar)
                        CharactersParsed += 1

                        'Check to see if we're at the maximum character number
                        If Not InEsc Then
                            If IncompleteSentenceBuilder.Length - EscapeCharacters = Console.WindowWidth - Convert.ToInt32(IsOnUnix) Or line.Length = CharactersParsed Then
                                'We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                                Wdbg(DebugLevel.I, "Adding {0} to the list... Incomplete sentences: {1}", IncompleteSentenceBuilder.ToString, IncompleteSentences.Count)
                                IncompleteSentences.Add(IncompleteSentenceBuilder.ToString)

                                'Clean everything up
                                IncompleteSentenceBuilder.Clear()
                            End If
                        Else
                            EscapeCharacters += 1
                        End If
                    Next
                Next

                'Write the body
                For Each line As String In IncompleteSentences
                    'Write the line
                    Dim OldTop As Integer = Console.CursorTop + 1
                    Write(line, True, ColTypes.Neutral)
                    If OldTop <> Console.CursorTop Then Console.CursorTop = OldTop

                    'Check to see if we're at the end
                    If Console.CursorTop = InfoPlace - 1 Then
                        Dim PressedKey As ConsoleKeyInfo = Console.ReadKey()
                        If PressedKey.Key = ConsoleKey.Escape Then
                            Console.Clear()
                            Exit Sub
                        Else
                            Console.Clear()
                            If Not String.IsNullOrWhiteSpace(ManpageInfoStyle) Then
                                WriteWhere(ProbePlaces(ManpageInfoStyle), Console.CursorLeft, InfoPlace, True, BackgroundColor, NeutralTextColor, Pages(ManualTitle).Title, Pages(ManualTitle).Revision)
                            Else
                                WriteWhere(" {0} (v{1}) ", Console.CursorLeft, InfoPlace, True, BackgroundColor, NeutralTextColor, Pages(ManualTitle).Title, Pages(ManualTitle).Revision)
                            End If
                        End If
                    End If
                Next

                'Stop on last page
                Wdbg(DebugLevel.I, "We're on the last page.")
                Console.ReadKey()

                'Clean up
                Wdbg(DebugLevel.I, "Exiting...")
                Console.Clear()
            Else
                Write(DoTranslation("Manual page {0} not found."), True, ColTypes.Neutral, ManualTitle)
            End If
        End Sub

    End Module
End Namespace
