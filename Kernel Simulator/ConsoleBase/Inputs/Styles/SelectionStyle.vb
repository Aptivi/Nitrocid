
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

Namespace ConsoleBase.Inputs.Styles
    Public Module SelectionStyle

        ''' <summary>
        ''' Prompts user for selection
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        Public Function PromptSelection(Question As String, AnswersStr As String) As Integer
            Return PromptSelection(Question, AnswersStr, Array.Empty(Of String)())
        End Function

        ''' <summary>
        ''' Prompts user for Selection
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        Public Function PromptSelection(Question As String, AnswersStr As String, AnswersTitles() As String) As Integer
            Dim HighlightedAnswer As Integer = 1
            While True
                'Variables
                Dim answers As String() = AnswersStr.Split("/")
                Dim Answer As ConsoleKeyInfo
                Console.Clear()

                'Check to see if the answer titles are the same
                If answers.Length <> AnswersTitles.Length Then
                    ReDim Preserve AnswersTitles(answers.Length - 1)
                End If

                'Ask a question
                Write(Question + NewLine, True, ColTypes.Question)
                For AnswerIndex As Integer = 0 To answers.Length - 1
                    Dim AnswerInstance As String = answers(AnswerIndex)
                    Dim AnswerTitle As String = AnswersTitles(AnswerIndex)
                    Write($" {AnswerInstance}) {AnswerTitle}", True, If(AnswerIndex + 1 = HighlightedAnswer, ColTypes.SelectedOption, ColTypes.Option))
                Next

                'Wait for an answer
                Answer = Console.ReadKey(True)
                Console.WriteLine()

                'Check the answer
                Select Case Answer.Key
                    Case ConsoleKey.UpArrow
                        HighlightedAnswer -= 1
                        If HighlightedAnswer = 0 Then
                            HighlightedAnswer = answers.Length
                        End If
                    Case ConsoleKey.DownArrow
                        If HighlightedAnswer = answers.Length Then
                            HighlightedAnswer = 0
                        End If
                        HighlightedAnswer += 1
                    Case ConsoleKey.Enter
                        Return HighlightedAnswer
                    Case ConsoleKey.Escape
                        Return -1
                End Select
            End While
            Return -1
        End Function

    End Module
End Namespace
