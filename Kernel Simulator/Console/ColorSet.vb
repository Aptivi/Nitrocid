
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

Module ColorSet

    'Private variables
    Private answerColor As String
    Private currentStepMessage As String = "Color for input: "
    Private stepCurrent As Integer = 0
    Private i As Integer

    'Variables
    Public answersColor(5) As String

    'TODO: Templates like (Default, RedConsole, Bluespire, Hacker, LinuxUncolored, LinuxColoredDef) comes in the future.

    Sub UseDefaults()
        'Use default settings in current step.
        If (i = 0) Then
            answersColor(i) = "White"
        ElseIf (i = 1) Then
            answersColor(i) = "White"
        ElseIf (i = 2) Then
            answersColor(i) = "Yellow"
        ElseIf (i = 3) Then
            answersColor(i) = "Red"
        ElseIf (i = 4) Then
            answersColor(i) = "DarkGreen"
        ElseIf (i = 5) Then
            answersColor(i) = "Green"
        End If
    End Sub

    Sub SetColorSteps()

        'Set colors of individual things.
        Dim ResetFlag As Boolean = False
        Dim DoneFlag As Boolean = False

        'Actual code
        For i As Integer = 0 To 6
            If (i = 6) Then
                'Print summary of what is being changed.
                System.Console.WriteLine("Input Color: {0}" + vbNewLine + _
                                         "License Color: {1}" + vbNewLine + _
                                         "Cont. Kernel Error Color: {2}" + vbNewLine + _
                                         "Uncont. Kernel Error Color: {3}" + vbNewLine + _
                                         "Hostname Shell Color: {4}" + vbNewLine + _
                                         "Username Shell Color: {5}", _
                                         answersColor(0), answersColor(1), answersColor(2), _
                                         answersColor(3), answersColor(4), answersColor(5))
            End If

            'Write current step message
            System.Console.Write(currentStepMessage)
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            answerColor = System.Console.ReadLine()
            System.Console.ResetColor()

            'Checks the user input if colors exist, and then try to put it into a temporary array. 
            'TODO: "Live Mode" will come in the future.
            If (i <> 6) Then
                If (answerColor = "RESET") Then
                    'Give a signal to the command that the colors are resetting. 
                    'TODO: "Confirmation" will also come in the future.
                    ResetFlag = True
                    Exit For
                ElseIf (answerColor = "q") Then
                    System.Console.WriteLine("Color changing has been cancelled.")
                    Exit Sub
                ElseIf (availableColors.Contains(answerColor)) Then
                    answersColor(i) = answerColor
                    advanceStep()
                ElseIf (answerColor = "") Then
                    'Nothing written, use defaults.
                    UseDefaults()
                    advanceStep()
                Else
                    System.Console.WriteLine("The {0} color is not found. Your answers is case-sensitive.", answerColor)
                    UseDefaults()
                    advanceStep()
                End If
            ElseIf (i = 6) Then
                If answerColor = "y" Then
                    'Set all colors into user-written settings (actual)
                    'The CType([Enum].Parse...) code is the only way to change colors in Visual Basic language.
                    DoneFlag = True
                    inputColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(0)), ConsoleColor)
                    licenseColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(1)), ConsoleColor)
                    contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(2)), ConsoleColor)
                    uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(3)), ConsoleColor)
                    hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(4)), ConsoleColor)
                    userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(5)), ConsoleColor)
                    System.Console.WriteLine("Colors changed.")
                ElseIf answerColor = "n" Then
                    stepCurrent = 0
                    currentStepMessage = "Color for input: "
                    Exit For
                Else
                    System.Console.WriteLine("System colors are not changed because you wrote an invalid choice.")
                    Exit Sub
                End If
            End If
        Next
        If (ResetFlag = False And DoneFlag = False) Then
            SetColorSteps()
        ElseIf (ResetFlag = True) Then
            'Reset every color to their default settings and exit.
            inputColor = inputColorDef
            licenseColor = licenseColorDef
            contKernelErrorColor = contKernelErrorColorDef
            uncontKernelErrorColor = uncontKernelErrorColorDef
            hostNameShellColor = hostNameShellColorDef
            userNameShellColor = userNameShellColorDef
            System.Console.WriteLine("Everything is reset to normal settings.")
            stepCurrent = 0
            currentStepMessage = "Color for input: "
        End If

    End Sub

    Sub advanceStep()

        'Advance a step
        stepCurrent = stepCurrent + 1
        If (stepCurrent = 1) Then
            currentStepMessage = "Color for license: "
        ElseIf (stepCurrent = 2) Then
            currentStepMessage = "Color for continuable kernel error: "
        ElseIf (stepCurrent = 3) Then
            currentStepMessage = "Color for uncontinuable kernel error: "
        ElseIf (stepCurrent = 4) Then
            currentStepMessage = "Color for hostname on shell prompt: "
        ElseIf (stepCurrent = 5) Then
            currentStepMessage = "Color for username on shell prompt: "
        ElseIf (stepCurrent = 6) Then
            currentStepMessage = "Is this information correct? <y/n> "
        End If

    End Sub

End Module
