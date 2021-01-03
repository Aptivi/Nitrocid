
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

Public Module Commands

    ''' <summary>
    ''' Prompts user for choice
    ''' </summary>
    ''' <param name="Question">A question</param>
    ''' <param name="ScriptVariable">An $variable</param>
    ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
    Public Sub PromptChoice(ByVal Question As String, ByVal ScriptVariable As String, ByVal AnswersStr As String)
        While True
            'Variables
            Question = Question.Replace(ScriptVariable + " " + AnswersStr + " ", "")
            Dim answers As String()
            Dim answer As String
            Wdbg("I", "Removed ""{0} {1} "" from strArgs. Result: {2}", ScriptVariable, AnswersStr, Question)

            'Ask a question
            W(Question, True, ColTypes.Neutral)
            W("<{0}> ", False, ColTypes.Input, AnswersStr)
            answers = AnswersStr.Split("/")

            'Wait for an answer
            answer = Console.ReadKey.KeyChar
            Console.WriteLine()

            'Check if script variable is initialized. If not, exits the program.
            If ScriptVariables.ContainsKey(ScriptVariable) Then
                'Check if answer if correct.
                If answers.Contains(answer) Then
                    SetVariable(ScriptVariable, answer)
                    Exit While
                End If
            Else
                Exit While
            End If
        End While
    End Sub

    ''' <summary>
    ''' Prompts user for input (answer the question with your own answers)
    ''' </summary>
    ''' <param name="ScriptVariable">An $variable</param>
    Public Sub PromptInput(ByVal Question As String, ByVal ScriptVariable As String)
        While True
            'Variables
            Dim Answer As String
            Wdbg("I", "Script var: {0} ({1}), Question: {2}", ScriptVariable, ScriptVariables.ContainsKey(ScriptVariable), Question)

            'Ask a question
            W(Question, False, ColTypes.Input)

            'Wait for an answer
            Answer = Console.ReadLine
            Wdbg("I", "Answer: {0}", Answer)

            'Check if script variable is initialized. If not, exits the program.
            If ScriptVariables.ContainsKey(ScriptVariable) Then
                Wdbg("I", "Variable found. Setting {0} to {1}...", ScriptVariable, Answer)
                SetVariable(ScriptVariable, Answer)
                Exit While
            Else
                Wdbg("W", "Variable {0} not found.", ScriptVariable)
                Exit While
            End If
        End While
    End Sub

End Module
