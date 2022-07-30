
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
    Public Module InputStyle

        ''' <summary>
        ''' Prompts user for input (answer the question with your own answers)
        ''' </summary>
        Public Function PromptInput(Question As String) As String
            While True
                'Variables
                Dim Answer As String
                Wdbg(DebugLevel.I, "Question: {0}", Question)

                'Ask a question
                Write(Question, False, ColTypes.Question)
                SetConsoleColor(InputColor)

                'Wait for an answer
                Answer = ReadLine()
                Wdbg(DebugLevel.I, "Answer: {0}", Answer)

                Return Answer
            End While
        End Function

    End Module
End Namespace
