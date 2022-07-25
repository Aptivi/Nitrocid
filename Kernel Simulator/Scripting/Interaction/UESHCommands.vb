
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

Imports KS.ConsoleBase.Inputs.Styles

Namespace Scripting.Interaction
    Public Module UESHCommands

        ''' <summary>
        ''' Prompts user for choice
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="ScriptVariable">A $variable</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="OutputType">Output type of choices</param>
        ''' <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        Public Sub PromptChoiceAndSet(Question As String, ScriptVariable As String, AnswersStr As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False)
            PromptChoiceAndSet(Question, ScriptVariable, AnswersStr, Array.Empty(Of String)(), OutputType, PressEnter)
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
        Public Sub PromptChoiceAndSet(Question As String, ScriptVariable As String, AnswersStr As String, AnswersTitles() As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine, Optional PressEnter As Boolean = False)
            Dim Answer As String = PromptChoice(Question, AnswersStr, AnswersTitles, OutputType, PressEnter)
            SetVariable(ScriptVariable, Answer)
        End Sub

        ''' <summary>
        ''' Prompts user for selection
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="ScriptVariable">A $variable</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        Public Sub PromptSelectionAndSet(Question As String, ScriptVariable As String, AnswersStr As String)
            PromptSelectionAndSet(Question, ScriptVariable, AnswersStr, Array.Empty(Of String)())
        End Sub

        ''' <summary>
        ''' Prompts user for Selection
        ''' </summary>
        ''' <param name="Question">A question</param>
        ''' <param name="ScriptVariable">A $variable</param>
        ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        ''' <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        Public Sub PromptSelectionAndSet(Question As String, ScriptVariable As String, AnswersStr As String, AnswersTitles() As String)
            Dim SelectedAnswer As Integer = PromptSelection(Question, AnswersStr, AnswersTitles)

            'Set the value
            SetVariable(ScriptVariable, SelectedAnswer)
        End Sub

        ''' <summary>
        ''' Prompts user for input (answer the question with your own answers)
        ''' </summary>
        ''' <param name="ScriptVariable">An $variable</param>
        Public Sub PromptInputAndSet(Question As String, ScriptVariable As String)
            'Variables
            Dim Answer As String = PromptInput(Question)
            Wdbg(DebugLevel.I, "Script var: {0} ({1})", ScriptVariable, ShellVariables.ContainsKey(ScriptVariable))
            Wdbg(DebugLevel.I, "Setting to {0}...", Answer)
            SetVariable(ScriptVariable, Answer)
        End Sub

    End Module
End Namespace
