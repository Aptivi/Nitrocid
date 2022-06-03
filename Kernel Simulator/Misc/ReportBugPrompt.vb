
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

Namespace Misc
    Module ReportBugPrompt

        ''' <summary>
        ''' Prompts user for brief bug report details
        ''' </summary>
        Sub PromptForBug()
            Dim [Step] As Integer = 1
            Dim AnswerKind As Integer
            Dim AnswerFeature As String = ""
            Dim AnswerRequest As String = ""

            'First, select what kind of bug you're reporting
            While [Step] = 1
                Write(DoTranslation("Thank you for raising a ticket to us! Select what kind of request do you have.") + NewLine, True, ColTypes.Neutral)
                Write(" 1) " + DoTranslation("A problem"), True, ColTypes.Option)
                Write(" 2) " + DoTranslation("A feature request"), True, ColTypes.Option)
                Write(" 3) " + DoTranslation("A question") + NewLine, True, ColTypes.Option)
                Write(">> ", False, ColTypes.Input)
                If Integer.TryParse(ReadLine(), AnswerKind) Then
                    Wdbg(DebugLevel.I, "Answer: {0}", AnswerKind)
                    Select Case AnswerKind
                        Case 1, 2, 3
                            [Step] += 1
                        Case Else '???
                            Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                            Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerKind)
                            Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                            Console.ReadKey()
                    End Select
                ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                    Wdbg(DebugLevel.W, "Answer is not numeric.")
                    Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            End While

            'Second, type what feature you need to raise a ticket on
            While [Step] = 2
                Write(DoTranslation("Type a feature that you want to raise a ticket on.") + NewLine, True, ColTypes.Neutral)
                Write(">> ", False, ColTypes.Input)
                AnswerFeature = ReadLine()
                Wdbg(DebugLevel.I, "Answer: {0}", AnswerFeature)
                If String.IsNullOrWhiteSpace(AnswerFeature) And ReadLineReboot.ReadLine.ReadRanToCompletion Then
                    Wdbg(DebugLevel.W, "Text written is not valid. Returning...")
                    Write(DoTranslation("You must specify a feature."), True, ColTypes.Error)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                Else
                    [Step] += 1
                End If
            End While

            'Third, type your idea, question or problem
            While [Step] = 3
                Write(DoTranslation("Ask a question, jot your idea, or report a problem.") + NewLine, True, ColTypes.Neutral)
                Write(">> ", False, ColTypes.Input)
                AnswerRequest = ReadLine()
                Wdbg(DebugLevel.I, "Answer: {0}", AnswerRequest)
                If String.IsNullOrWhiteSpace(AnswerRequest) And ReadLineReboot.ReadLine.ReadRanToCompletion Then
                    Wdbg(DebugLevel.W, "Text written is not valid. Returning...")
                    Write(DoTranslation("You must write your request."), True, ColTypes.Error)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                Else
                    Exit While
                End If
            End While

            'Finally, pass these answers to a URL
            Dim TargetURL As String = "https://github.com/EoflaOE/Kernel-Simulator/issues/new?assignees=&labels="
            Select Case AnswerKind
                Case 1 'A problem
                    TargetURL += $"&template=report-an-issue.md&title=%5BBUG%5D+{AnswerFeature}+-+{AnswerRequest}"
                    Wdbg(DebugLevel.I, "Target URL: {0}", TargetURL)
                Case 2 'A feature request
                    TargetURL += $"&template=request-a-feature.md&title=%5BADD%5D+{AnswerFeature}+-+{AnswerRequest}"
                    Wdbg(DebugLevel.I, "Target URL: {0}", TargetURL)
                Case 3 'A question
                    TargetURL += $"&template=ask-a-question.md&title=%5BQ%26A%5D+{AnswerFeature}+-+{AnswerRequest}"
                    Wdbg(DebugLevel.I, "Target URL: {0}", TargetURL)
            End Select
            Process.Start(TargetURL)
        End Sub

    End Module
End Namespace
