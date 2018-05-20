
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

Module TemplateSet

    Sub TemplatePrompt()

        W("Available templates: {0}" + vbNewLine + "Template: ", "input", String.Join(", ", colorTemplates))
        Dim answertemplate As String = System.Console.ReadLine()
        templateSet(answertemplate)

    End Sub

    Friend Sub templateSet(ByVal theme As String)

        If colorTemplates.Contains(theme) = True Then
            If theme = "Default" Then
                ColorSet.ResetColors()
                templateSetExitFlag = True
            ElseIf theme = "RedConsole" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorRC), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorRC), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorRC), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorRC), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorRC), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorRC), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorRC), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorRC), ConsoleColor)
                LoadBackground.Load()
                templateSetExitFlag = True
            ElseIf theme = "Bluespire" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorBS), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorBS), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorBS), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorBS), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorBS), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorBS), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorBS), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorBS), ConsoleColor)
                LoadBackground.Load()
                templateSetExitFlag = True
            ElseIf theme = "Hacker" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorHckr), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorHckr), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorHckr), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorHckr), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorHckr), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorHckr), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorHckr), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorHckr), ConsoleColor)
                LoadBackground.Load()
                templateSetExitFlag = True
            ElseIf theme = "LinuxUncolored" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorLUnc), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorLUnc), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorLUnc), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorLUnc), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorLUnc), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorLUnc), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorLUnc), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorLUnc), ConsoleColor)
                LoadBackground.Load()
                templateSetExitFlag = True
            ElseIf theme = "LinuxColoredDef" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorLcDef), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorLcDef), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorLcDef), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorLcDef), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorLcDef), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorLcDef), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorLcDef), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorLcDef), ConsoleColor)
                LoadBackground.Load()
                templateSetExitFlag = True
            End If
        Else
            Wln("Invalid color template {0}", "neutralText", theme)
        End If

    End Sub

End Module
