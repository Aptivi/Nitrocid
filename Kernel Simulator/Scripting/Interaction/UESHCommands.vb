
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

Namespace Scripting.Interaction
    Public Module UESHCommands

        Public DefaultChoiceOutputType As ChoiceOutputType = ChoiceOutputType.Modern

        ''' <summary>
        ''' The enumeration for the choice command output type
        ''' </summary>
        Public Enum ChoiceOutputType
            ''' <summary>
            ''' A question and a set of answers in one line
            ''' </summary>
            OneLine
            ''' <summary>
            ''' A question in a line and a set of answers in another line
            ''' </summary>
            TwoLines
            ''' <summary>
            ''' The modern way of listing choices
            ''' </summary>
            Modern
            ''' <summary>
            ''' The table of choices
            ''' </summary>
            Table
        End Enum

        ''' <summary>
        ''' Prompts user for choice
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="ScriptVariable">A $variable</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="OutputType">Output type of choices</param>
        ''' <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        Public Sub PromptChoice(Question As String, ScriptVariable As String, AnswersStr As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False)
            PromptChoice(Question, ScriptVariable, AnswersStr, Array.Empty(Of String)(), OutputType, PressEnter)
        End Sub

        ''' <summary>
        ''' Prompts user for choice
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="ScriptVariable">A $variable</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        ''' <param name="OutputType">Output type of choices</param>
        ''' <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        Public Sub PromptChoice(Question As String, ScriptVariable As String, AnswersStr As String, AnswersTitles() As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False)
            While True
                'Variables
                Dim answers As String() = AnswersStr.Split("/")
                Dim answer As String

                'Check to see if the answer titles are the same
                If answers.Length <> AnswersTitles.Length Then
                    ReDim Preserve AnswersTitles(answers.Length - 1)
                End If

                'Ask a question
                Select Case OutputType
                    Case ChoiceOutputType.OneLine
                        Write(Question, False, ColTypes.Question)
                        Write(" <{0}> ", False, ColTypes.Input, AnswersStr)
                    Case ChoiceOutputType.TwoLines
                        Write(Question, True, ColTypes.Question)
                        Write("<{0}> ", False, ColTypes.Input, AnswersStr)
                    Case ChoiceOutputType.Modern
                        Write(Question + NewLine, True, ColTypes.Question)
                        For AnswerIndex As Integer = 0 To answers.Length - 1
                            Dim AnswerInstance As String = answers(AnswerIndex)
                            Dim AnswerTitle As String = AnswersTitles(AnswerIndex)
                            Write($" {AnswerInstance}) {AnswerTitle}", True, ColTypes.Option)
                        Next
                        Write(NewLine + ">> ", False, ColTypes.Input)
                    Case ChoiceOutputType.Table
                        Dim ChoiceHeader As String() = {DoTranslation("Possible answers"), DoTranslation("Answer description")}
                        Dim ChoiceData(answers.Length - 1, 1) As String
                        Write(Question, True, ColTypes.Question)
                        For AnswerIndex As Integer = 0 To answers.Length - 1
                            ChoiceData(AnswerIndex, 0) = answers(AnswerIndex)
                            ChoiceData(AnswerIndex, 1) = AnswersTitles(AnswerIndex)
                        Next
                        WriteTable(ChoiceHeader, ChoiceData, 2)
                        Write(NewLine + ">> ", False, ColTypes.Input)
                End Select

                'Wait for an answer
                If PressEnter Then
                    answer = ReadLine()
                Else
                    answer = Console.ReadKey.KeyChar
                    Console.WriteLine()
                End If

                'Check if answer is correct.
                If answers.Contains(answer) Then
                    SetVariable(ScriptVariable, answer)
                    Exit While
                End If
            End While
        End Sub

        ''' <summary>
        ''' Prompts user for selection
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="ScriptVariable">A $variable</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        Public Sub PromptSelection(Question As String, ScriptVariable As String, AnswersStr As String)
            PromptSelection(Question, ScriptVariable, AnswersStr, Array.Empty(Of String)())
        End Sub

        ''' <summary>
        ''' Prompts user for Selection
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="ScriptVariable">A $variable</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        Public Sub PromptSelection(Question As String, ScriptVariable As String, AnswersStr As String, AnswersTitles() As String)
            Dim HighlightedAnswer As Integer = 1
            Dim SelectedAnswer As Integer
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
                        SelectedAnswer = HighlightedAnswer
                        Exit While
                    Case ConsoleKey.Escape
                        Exit Sub
                End Select
            End While

            'Set the value
            SetVariable(ScriptVariable, SelectedAnswer)
        End Sub

        ''' <summary>
        ''' Prompts user for input (answer the question with your own answers)
        ''' </summary>
        ''' <param name="ScriptVariable">An $variable</param>
        Public Sub PromptInput(Question As String, ScriptVariable As String)
            While True
                'Variables
                Dim Answer As String
                Wdbg(DebugLevel.I, "Script var: {0} ({1}), Question: {2}", ScriptVariable, ShellVariables.ContainsKey(ScriptVariable), Question)

                'Ask a question
                Write(Question, False, ColTypes.Question)
                SetConsoleColor(InputColor)

                'Wait for an answer
                Answer = ReadLine()
                Wdbg(DebugLevel.I, "Answer: {0}", Answer)

                Wdbg(DebugLevel.I, "Setting {0} to {1}...", ScriptVariable, Answer)
                SetVariable(ScriptVariable, Answer)
                Exit While
            End While
        End Sub

    End Module
End Namespace
