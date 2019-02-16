
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Public Module TemplateSet

    'Variables
    Public currentTheme As String

    Public Sub templateSet(ByVal theme As String)

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
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorRC), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorRC), ConsoleColor)
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
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorBS), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorBS), ConsoleColor)
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
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorHckr), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorHckr), ConsoleColor)
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
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorLUnc), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorLUnc), ConsoleColor)
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
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorLcDef), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorLcDef), ConsoleColor)
                LoadBackground.Load()
                templateSetExitFlag = True
            End If
            MakePermanent()
            ParseCurrentTheme()
        Else
            Wln(DoTranslation("Invalid color template {0}", currentLang), "neutralText", theme)
        End If

    End Sub

    Public Sub MakePermanent()

        Dim ksconf As New IniFile()
        Dim configPath As String
        If (EnvironmentOSType.Contains("Unix")) Then
            configPath = Environ("HOME") + "/kernelConfig.ini"
        Else
            configPath = Environ("USERPROFILE") + "\kernelConfig.ini"
        End If
        ksconf.Load(configPath)
        ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColor.ToString
        ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColor.ToString
        ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColor.ToString
        ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColor.ToString
        ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColor.ToString
        ksconf.Sections("Colors").Keys("License Color").Value = licenseColor.ToString
        ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColor.ToString
        ksconf.Sections("Colors").Keys("Input Color").Value = inputColor.ToString
        ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColor.ToString
        ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColor.ToString
        ksconf.Save(configPath)

    End Sub

    Sub ParseCurrentTheme()

        Dim ksconf As New IniFile()
        Dim configPath As String
        If (EnvironmentOSType.Contains("Unix")) Then
            configPath = Environ("HOME") + "/kernelConfig.ini"
        Else
            configPath = Environ("USERPROFILE") + "\kernelConfig.ini"
        End If
        ksconf.Load(configPath)
        If (ksconf.Sections("Colors").Keys("User Name Shell Color").Value = ConsoleColor.DarkGreen.ToString And _
            ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = ConsoleColor.Green.ToString And _
            ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = ConsoleColor.Yellow.ToString And _
            ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = ConsoleColor.Red.ToString And _
            ksconf.Sections("Colors").Keys("Text Color").Value = ConsoleColor.Gray.ToString And _
            ksconf.Sections("Colors").Keys("License Color").Value = ConsoleColor.White.ToString And _
            ksconf.Sections("Colors").Keys("Background Color").Value = ConsoleColor.Black.ToString And _
            ksconf.Sections("Colors").Keys("Input Color").Value = ConsoleColor.White.ToString And _
            ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = ConsoleColor.DarkYellow.ToString And _
            ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = ConsoleColor.DarkGray.ToString) Then
            currentTheme = "Default"
        ElseIf (ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorRC.ToString And _
            ksconf.Sections("Colors").Keys("License Color").Value = licenseColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Input Color").Value = inputColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorRC.ToString And _
            ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorRC.ToString) Then
            currentTheme = "RedConsole"
        ElseIf (ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorBS.ToString And _
            ksconf.Sections("Colors").Keys("License Color").Value = licenseColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Input Color").Value = inputColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorBS.ToString And _
            ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorBS.ToString) Then
            currentTheme = "BlueSpire"
        ElseIf (ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("License Color").Value = licenseColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Input Color").Value = inputColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorHckr.ToString And _
            ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorHckr.ToString) Then
            currentTheme = "Hacker"
        ElseIf (ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("License Color").Value = licenseColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Input Color").Value = inputColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorLUnc.ToString And _
            ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorLUnc.ToString) Then
            currentTheme = "LinuxUncolored"
        ElseIf (ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("License Color").Value = licenseColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Input Color").Value = inputColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorLcDef.ToString And _
            ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorLcDef.ToString) Then
            currentTheme = "LinuxColoredDef"
        Else
            currentTheme = "Custom"
        End If

    End Sub

End Module
