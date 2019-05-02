
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

Module PageViewer

    'Variables
    Private InfoPlace As Integer
    Private OriginalBG As ConsoleColor
    Private OriginalFG_INFO As ConsoleColor

    'Preview the manual page
    Public Sub ViewPage(ByVal title As String)
        If (AvailablePages.Contains(title)) Then
            'Variables
            Dim writtenLines As Integer
            Dim oldTop As Integer
            Dim vbNewLineRequired As Boolean

            'Get the bottom place
            InfoPlace = Console.WindowHeight - 1
            Wdbg("Bottom informational height is {0}", InfoPlace)

            'If there is any To-do, write them to the console
            Wdbg("Todo count for ""{0}"": {1}", title, Pages(title).Todos.Count.ToString)
            If (Pages(title).Todos.Count <> 0) Then
                Wdbg("Todos are found in manpage. List of todos:")
                Wln(DoTranslation("This manual page is incomplete for the following reasons:", currentLang) + vbNewLine, "neutralText")
                For Each Todo As String In Pages(title).Todos
                    Wdbg("TODO: {0}", Todo)
                    Wln(Todo, "neutralText")
                Next
                W(vbNewLine + DoTranslation("Press any key to continue...", currentLang), "neutralText")
                Console.ReadKey()
            End If

            'Clear screen for readability, and backup the values of FG and BG for console
            Console.Clear()
            OriginalBG = Console.BackgroundColor
            OriginalFG_INFO = Console.ForegroundColor
            Wdbg("BG was {0}, FG was {1}", OriginalBG, OriginalFG_INFO)

            'Write the information to the console
            WriteInfo(title)

            'Split the keys, and make another dictionary that explains what each word is its color
            Dim splitWords_COLORS_coll As Object = Pages(title).Colors.Keys
            Dim splitWords_COLORS_dict As New Dictionary(Of String, ConsoleColor)
            For Each Word As String In splitWords_COLORS_coll
                Dim splitWord As String() = Word.Split(" ")
                For Each Wrd As String In splitWord
                    If Word.Contains(Wrd) And Not splitWords_COLORS_dict.ContainsKey(Wrd) Then
                        Wdbg("{0} is being colored {1}...", Wrd, Pages(title).Colors(Word))
                        splitWords_COLORS_dict.Add(Wrd, Pages(title).Colors(Word))
                    End If
                Next
            Next

            'Disable blinking cursor
            Console.CursorVisible = False

            'Write the body
            For Each line As String In Pages(title).Body.ToString.Replace(Chr(13), "").Split(Chr(10))
                Dim MkNewLineNec As Boolean = True

                'Check for line that starts with a space
                If (line.StartsWith(" ") Or line.StartsWith(vbTab)) Then
                    MkNewLineNec = False 'Make making new line unnecessary
                End If

                'Prepare the view
                If (line <> "") Then
                    vbNewLineRequired = True
                    For Each word As String In line.Split({" "c})
                        'Manage lines
                        If oldTop = InfoPlace - 1 Then
                            Wdbg("oldTop ({0}) = InfoPlace - 1 ({1})", oldTop, InfoPlace - 1)
                            Console.ReadKey()
                            Console.Clear()
                            writtenLines = 0
                            WriteInfo(title)
                        ElseIf Console.CursorTop > oldTop Then
                            Wdbg("CursorTop ({0}) > oldTop ({1})", Console.CursorTop, oldTop)
                            writtenLines += 1
                        ElseIf (Console.CursorTop > oldTop And writtenLines <> oldTop) Or vbNewLineRequired = True Then
                            Wdbg("CursorTop ({0}) > oldTop ({1}) and writtenLines ({2}) <> oldTop ({1}) || vbNewLineRequired ({3})", Console.CursorTop, oldTop, writtenLines, vbNewLineRequired)
                            writtenLines += 1
                            Console.WriteLine()
                            If (vbNewLineRequired = True) Then vbNewLineRequired = False
                        End If

                        'Store the old height
                        oldTop = Console.CursorTop

                        'Check the word is there is color
                        For Each Word_color As String In splitWords_COLORS_dict.Keys
                            If (Word_color = word.Replace(vbTab, "")) Then 'If the word in the dictionary matches one in word in the body, set the color
                                Console.ForegroundColor = splitWords_COLORS_dict(Word_color)
                                Wdbg("{0} has a color entry.", Word_color)
                                Exit For
                            End If
                        Next

                        'Parse for sections
                        For Each Line_sect As String In Pages(title).Sections.Keys
                            If (Line_sect = line And line.StartsWith(word)) Then
                                Dim sec_Num As Integer = Pages(title).Sections(Line_sect).Substring(2)
                                For times = 1 To sec_Num
                                    Console.Write("-")
                                Next
                                Console.Write(" ")
                                Exit For
                            End If
                        Next

                        'Check for "" on word variable, and make newline if it's necessary.
                        If (word = "" And MkNewLineNec = True) Then
                            Wdbg("Making newline... They are necessary.")
                            word = vbNewLine
                        End If

                        'Write words
                        Console.Write(word + " ")
                        Console.ForegroundColor = OriginalFG_INFO
                    Next
                Else
                    writtenLines += 1
                    Console.WriteLine()
                    oldTop = Console.CursorTop
                End If
            Next

            'Stop on last page
            Wdbg("We're on the last page.")
            Console.ReadKey()

            'Clean up
            Wdbg("Exiting...")
            Console.Clear()
        Else
            Wln(DoTranslation("It seems that the manual page {0} is not found.", currentLang), "neutralText", title)
        End If

    End Sub

    Private Sub WriteInfo(ByVal title As String)
        Console.SetCursorPosition(Console.CursorLeft, InfoPlace)
        Console.BackgroundColor = ConsoleColor.White : Console.ForegroundColor = ConsoleColor.Black
        Console.Write(title + DoTranslation(" | {0} words | Revision v{1} | {2} sections", currentLang), Pages(title).Body.ToString.Split(" ").Length.ToString,
                                                                                                         Pages(title).ManualRevision, Pages(title).Sections.Count.ToString)
        Console.BackgroundColor = OriginalBG : Console.ForegroundColor = OriginalFG_INFO
        Console.SetCursorPosition(0, 0)
    End Sub

End Module
