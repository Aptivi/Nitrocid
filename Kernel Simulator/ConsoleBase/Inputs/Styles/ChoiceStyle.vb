
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
    Public Module ChoiceStyle

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
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="OutputType">Output type of choices</param>
        ''' <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        Public Function PromptChoice(Question As String, AnswersStr As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False) As String
            Return PromptChoice(Question, AnswersStr, Array.Empty(Of String)(), OutputType, PressEnter)
        End Function

        ''' <summary>
        ''' Prompts user for choice
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        ''' <param name="OutputType">Output type of choices</param>
        ''' <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        Public Function PromptChoice(Question As String, AnswersStr As String, AnswersTitles() As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False) As String
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
                        Dim AnswerTitleLeft As Integer = answers.Max(Function(x) $" {x}) ".Length)
                        If AnswerTitleLeft >= Console.WindowWidth Then AnswerTitleLeft = 0
                        For AnswerIndex As Integer = 0 To answers.Length - 1
                            Dim AnswerInstance As String = answers(AnswerIndex)
                            Dim AnswerTitle As String = AnswersTitles(AnswerIndex)
                            If AnswerTitleLeft > 0 Then
                                Write($" {AnswerInstance}) ", False, ColTypes.Option)
                                WriteWhere(AnswerTitle, AnswerTitleLeft, Console.CursorTop, False, ColTypes.Option)
                                Write("", True, ColTypes.Option)
                            Else
                                Write($" {AnswerInstance}) {AnswerTitle}", True, ColTypes.Option)
                            End If
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
                    Return answer
                End If
            End While
        End Function

    End Module
End Namespace
