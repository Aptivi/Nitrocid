
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module ArgumentPrompt

    'Variables
    Public answerargs As String                                                     'Input for arguments
    Public argsFlag As Boolean                                                      'A flag for checking for an argument later

    Sub PromptArgs()

        'Shows available arguments
        System.Console.Write("Available arguments: ")
        For Each argsavail As String In AvailableArgs
            System.Console.Write(argsavail + " ")
        Next

        'Prompts user to write arguments
        System.Console.Write(vbNewLine + "Boot arguments: ")
        System.Console.ForegroundColor = ConsoleColor.White
        answerargs = System.Console.ReadLine()
        System.Console.ResetColor()

        'Make a kernel check for arguments later if anything is entered
        If Not (answerargs = Nothing) Then
            argsFlag = True
        End If

    End Sub

End Module
