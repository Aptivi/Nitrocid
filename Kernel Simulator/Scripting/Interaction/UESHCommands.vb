
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

Public Module UESHCommands

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
    ''' <param name="ScriptVariable">An $variable</param>
    ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
    ''' <param name="OutputType">Output type of choices</param>
    Public Sub PromptChoice(Question As String, ScriptVariable As String, AnswersStr As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine)
        While True
            'Variables
            Dim answers As String() = AnswersStr.Split("/")
            Dim answer As String

            'Ask a question
            Select Case OutputType
                Case ChoiceOutputType.OneLine
                    W(Question, False, ColTypes.Question)
                    W(" <{0}> ", False, ColTypes.Input, AnswersStr)
                Case ChoiceOutputType.TwoLines
                    W(Question, True, ColTypes.Question)
                    W("<{0}> ", False, ColTypes.Input, AnswersStr)
                Case ChoiceOutputType.Modern
                    W(Question + vbNewLine, True, ColTypes.Question)
                    For Each AnswerInstance As String In answers
                        W($"{AnswerInstance})", True, ColTypes.Option)
                    Next
                    W(vbNewLine + ">> ", False, ColTypes.Input)
                Case ChoiceOutputType.Table
                    Dim ChoiceHeader As String() = {DoTranslation("Possible answers")}
                    Dim ChoiceData(answers.Length - 1, 0) As String
                    W(Question, True, ColTypes.Question)
                    For AnswerIndex As Integer = 0 To answers.Length - 1
                        ChoiceData(AnswerIndex, 0) = answers(AnswerIndex)
                    Next
                    WriteTable(ChoiceHeader, ChoiceData, 2)
                    W(vbNewLine + ">> ", False, ColTypes.Input)
            End Select

            'Wait for an answer
            answer = Console.ReadKey.KeyChar
            Console.WriteLine()

            'Check if answer if correct.
            If answers.Contains(answer) Then
                SetVariable(ScriptVariable, answer)
                Exit While
            End If
        End While
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
            W(Question, False, ColTypes.Question)
            SetConsoleColor(New Color(InputColor))

            'Wait for an answer
            Answer = Console.ReadLine
            Wdbg(DebugLevel.I, "Answer: {0}", Answer)

            Wdbg(DebugLevel.I, "Setting {0} to {1}...", ScriptVariable, Answer)
            SetVariable(ScriptVariable, Answer)
            Exit While
        End While
    End Sub

End Module
