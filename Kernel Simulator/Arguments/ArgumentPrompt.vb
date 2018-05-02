
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
    Public argsInjected As Boolean                                                  'A flag for checking for an argument on reboot

    'TODO: Add "debug" argument to increase verbosity on the kernel, and possibly include the line and file when there is a serious error.

    Sub PromptArgs(Optional ByVal InjMode As Boolean = False)

        'Checks if the arguments are injected
        If argsInjected = True Then
            argsInjected = False
            ArgumentParse.ParseArguments()
        Else
            'Shows available arguments
            System.Console.Write("Available arguments: {0}", String.Join(", ", AvailableArgs))

            'Prompts user to write arguments
            System.Console.Write(vbNewLine + "Boot arguments: ")
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            answerargs = System.Console.ReadLine()
            System.Console.ResetColor()

            'Make a kernel check for arguments later if anything is entered
            If (answerargs <> Nothing And InjMode = False) Then
                argsFlag = True
            ElseIf (answerargs <> Nothing And InjMode = True) Then
                argsInjected = True
                System.Console.WriteLine("Injected arguments will be scheduled to run at next reboot.")
            ElseIf (answerargs = "q" And InjMode = True) Then
                System.Console.WriteLine("Argument Injection has been cancelled.")
            End If
        End If

    End Sub

End Module
