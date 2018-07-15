
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
    Public answersColor(7) As String

    Sub UseDefaults()
        'Use default settings in current step.
        If (stepCurrent = 0) Then
            answersColor(0) = "White"
        ElseIf (stepCurrent = 1) Then
            answersColor(1) = "White"
        ElseIf (stepCurrent = 2) Then
            answersColor(2) = "Yellow"
        ElseIf (stepCurrent = 3) Then
            answersColor(3) = "Red"
        ElseIf (stepCurrent = 4) Then
            answersColor(4) = "DarkGreen"
        ElseIf (stepCurrent = 5) Then
            answersColor(5) = "Green"
        ElseIf (stepCurrent = 6) Then
            answersColor(6) = "Black"
        ElseIf (stepCurrent = 7) Then
            answersColor(7) = "Gray"
        End If
    End Sub

    Sub SetColorSteps()

        'Set colors of individual things.
        Dim ResetFlag As Boolean = False
        Dim DoneFlag As Boolean = False

        'Actual code
        Try
            For i As Integer = 0 To 8
                If (i = 8) Then
                    'Print summary of what is being changed, and evaluate "Live Mode"
                    inputColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(0)), ConsoleColor)
                    licenseColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(1)), ConsoleColor)
                    contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(2)), ConsoleColor)
                    uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(3)), ConsoleColor)
                    hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(4)), ConsoleColor)
                    userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(5)), ConsoleColor)
                    backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(6)), ConsoleColor)
                    neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), answersColor(7)), ConsoleColor)
                    LoadBackground.Load()
                    Wln("Input Color: {0}" + vbNewLine + "License Color: {1}" + vbNewLine + "Cont. Kernel Error Color: {2}" + vbNewLine + _
                        "Uncont. Kernel Error Color: {3}" + vbNewLine + "Hostname Shell Color: {4}" + vbNewLine + "Username Shell Color: {5}" + vbNewLine + _
                        "Background Color: {6}" + vbNewLine + "Text Color: {7}", "neutralText", answersColor(0), answersColor(1), answersColor(2), _
                        answersColor(3), answersColor(4), answersColor(5), answersColor(6), answersColor(7))
                End If

                'Write current step message
                W(currentStepMessage, "input")
                answerColor = System.Console.ReadLine()

                'Checks the user input if colors exist, and then try to put it into a temporary array. 
                'TODO: "Live Mode" will come in the future.
                If (i <> 8) Then
                    If (answerColor = "RESET") Then
                        'Give a signal to the command that the colors are resetting. 
                        W("Are you sure that you want to reset your colors to the defaults? <y/n> ", "input")
                        Dim answerreset As String = System.Console.ReadKey.KeyChar
                        If (answerreset = "y") Then
                            System.Console.WriteLine()
                            ResetFlag = True
                            Exit For
                        ElseIf (answerreset = "n") Then
                            System.Console.WriteLine()
                        ElseIf (answerreset = "q") Then
                            Wln(vbNewLine + "Color changing has been cancelled.", "neutralText")
                            Exit Sub
                        End If
                    ElseIf (answerColor = "THEME") Then
                        TemplateSet.TemplatePrompt()
                        If (templateSetExitFlag = True) Then
                            Wln("Theme changed.", "neutralText")
                            Exit Sub
                        Else
                            Exit For
                        End If
                    ElseIf (answerColor = "q") Then
                        Wln("Color changing has been cancelled.", "neutralText")
                        Exit Sub
                    ElseIf (availableColors.Contains(answerColor)) Then
                        answersColor(i) = answerColor
                        advanceStep()
                    ElseIf (answerColor = "") Then
                        'Nothing written, use defaults.
                        UseDefaults()
                        advanceStep()
                    Else
                        Wln("The {0} color is not found. Your answers is case-sensitive.", "neutralText", answerColor)
                        UseDefaults()
                        advanceStep()
                    End If
                ElseIf (i = 8) Then
                    If answerColor = "y" Then
                        DoneFlag = True
                        Wln("Colors changed.", "neutralText")
                    ElseIf answerColor = "n" Then
                        ResetColors()
                    Else
                        Wln("System colors are not changed because you wrote an invalid choice.", "neutralText")
                        Exit Sub
                    End If
                End If
            Next
            If (ResetFlag = False And DoneFlag = False) Then
                stepCurrent = 0
                currentStepMessage = "Color for input: "
                SetColorSteps()
            ElseIf (ResetFlag = True) Then
                'Reset every color to their default settings and exit.
                ResetColors()
                Wln("Everything is reset to normal settings.", "neutralText")
            End If
            stepCurrent = 0
            currentStepMessage = "Color for input: "
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("Error {0}: {1}" + vbNewLine + "{2}", "neutralText", Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln("Error {0}: {1}", "neutralText", Err.Number, Err.Description)
            End If
            stepCurrent = 0
            currentStepMessage = "Color for input: "
        End Try

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
            currentStepMessage = "Color for background: "
        ElseIf (stepCurrent = 7) Then
            currentStepMessage = "Color for texts: "
        ElseIf (stepCurrent = 8) Then
            currentStepMessage = "Is this information correct? <y/n> "
        End If

    End Sub

    Sub ResetColors()
        inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorDef), ConsoleColor)
        licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorDef), ConsoleColor)
        contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorDef), ConsoleColor)
        uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorDef), ConsoleColor)
        hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorDef), ConsoleColor)
        userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorDef), ConsoleColor)
        backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorDef), ConsoleColor)
        neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorDef), ConsoleColor)
        LoadBackground.Load()
    End Sub

End Module
