
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

Public Module Color

    'Variables for colors used by previous versions of Kernel.
    Public inputColor As Object = ConsoleColor.White
    Public licenseColor As Object = ConsoleColor.White
    Public contKernelErrorColor As Object = ConsoleColor.Yellow
    Public uncontKernelErrorColor As Object = ConsoleColor.Red
    Public hostNameShellColor As Object = ConsoleColor.DarkGreen
    Public userNameShellColor As Object = ConsoleColor.Green
    Public backgroundColor As Object = ConsoleColor.Black
    Public neutralTextColor As Object = ConsoleColor.Gray
    Public cmdListColor As Object = ConsoleColor.DarkYellow
    Public cmdDefColor As Object = ConsoleColor.DarkGray

    'Array for available colors
    Public availableColors() As String = {"White", "Black", "Gray", "DarkGray", "DarkRed", "Red", "DarkYellow", "Yellow", "DarkGreen", "Green",
                                          "DarkCyan", "Cyan", "DarkBlue", "Blue", "DarkMagenta", "Magenta", "RESET", "THEME"}

    'Templates array (available ones)
    Public colorTemplates() As String = {"Default", "RedConsole", "Bluespire", "Hacker", "Ubuntu", "LinuxUncolored", "LinuxColoredDef"}

    'Variables for the "Default" theme
    Public inputColorDef As Object = inputColor
    Public licenseColorDef As Object = licenseColor
    Public contKernelErrorColorDef As Object = contKernelErrorColor
    Public uncontKernelErrorColorDef As Object = uncontKernelErrorColor
    Public hostNameShellColorDef As Object = hostNameShellColor
    Public userNameShellColorDef As Object = userNameShellColor
    Public backgroundColorDef As Object = backgroundColor
    Public neutralTextColorDef As Object = neutralTextColor
    Public cmdListColorDef As Object = cmdListColor
    Public cmdDefColorDef As Object = cmdDefColor

    'Variables for the "RedConsole" theme
    Public inputColorRC As Object = ConsoleColor.Red
    Public licenseColorRC As Object = ConsoleColor.Red
    Public contKernelErrorColorRC As Object = ConsoleColor.Red
    Public uncontKernelErrorColorRC As Object = ConsoleColor.DarkRed
    Public hostNameShellColorRC As Object = ConsoleColor.DarkRed
    Public userNameShellColorRC As Object = ConsoleColor.Red
    Public backgroundColorRC As Object = ConsoleColor.Black
    Public neutralTextColorRC As Object = ConsoleColor.Red
    Public cmdListColorRC As Object = ConsoleColor.Red
    Public cmdDefColorRC As Object = ConsoleColor.DarkRed

    'Variables for the "Bluespire" theme
    Public inputColorBS As Object = ConsoleColor.Cyan
    Public licenseColorBS As Object = ConsoleColor.Cyan
    Public contKernelErrorColorBS As Object = ConsoleColor.Blue
    Public uncontKernelErrorColorBS As Object = ConsoleColor.Blue
    Public hostNameShellColorBS As Object = ConsoleColor.Blue
    Public userNameShellColorBS As Object = ConsoleColor.Blue
    Public backgroundColorBS As Object = ConsoleColor.DarkCyan
    Public neutralTextColorBS As Object = ConsoleColor.Cyan
    Public cmdListColorBS As Object = ConsoleColor.Cyan
    Public cmdDefColorBS As Object = ConsoleColor.Blue

    'Variables for the "Hacker" theme
    Public inputColorHckr As Object = ConsoleColor.Green
    Public licenseColorHckr As Object = ConsoleColor.Green
    Public contKernelErrorColorHckr As Object = ConsoleColor.Green
    Public uncontKernelErrorColorHckr As Object = ConsoleColor.Green
    Public hostNameShellColorHckr As Object = ConsoleColor.Green
    Public userNameShellColorHckr As Object = ConsoleColor.Green
    Public backgroundColorHckr As Object = ConsoleColor.DarkGray
    Public neutralTextColorHckr As Object = ConsoleColor.Green
    Public cmdListColorHckr As Object = ConsoleColor.DarkGreen
    Public cmdDefColorHckr As Object = ConsoleColor.Green

    'Variables for the "Ubuntu" theme
    Public inputColorU As Object = ConsoleColor.White
    Public licenseColorU As Object = ConsoleColor.White
    Public contKernelErrorColorU As Object = ConsoleColor.White
    Public uncontKernelErrorColorU As Object = ConsoleColor.White
    Public hostNameShellColorU As Object = ConsoleColor.Gray
    Public userNameShellColorU As Object = ConsoleColor.Gray
    Public backgroundColorU As Object = ConsoleColor.DarkMagenta
    Public neutralTextColorU As Object = ConsoleColor.White
    Public cmdListColorU As Object = ConsoleColor.White
    Public cmdDefColorU As Object = ConsoleColor.White

    'Variables for the "LinuxUncolored" theme
    Public inputColorLUnc As Object = ConsoleColor.Gray
    Public licenseColorLUnc As Object = ConsoleColor.Gray
    Public contKernelErrorColorLUnc As Object = ConsoleColor.Gray
    Public uncontKernelErrorColorLUnc As Object = ConsoleColor.Gray
    Public hostNameShellColorLUnc As Object = ConsoleColor.Gray
    Public userNameShellColorLUnc As Object = ConsoleColor.Gray
    Public backgroundColorLUnc As Object = ConsoleColor.Black
    Public neutralTextColorLUnc As Object = ConsoleColor.Gray
    Public cmdListColorLUnc As Object = ConsoleColor.Gray
    Public cmdDefColorLUnc As Object = ConsoleColor.Gray

    'Variables for the "LinuxColoredDef" theme
    'If there is a mistake in colors, please fix it.
    Public inputColorLcDef As Object = ConsoleColor.Gray
    Public licenseColorLcDef As Object = ConsoleColor.Gray
    Public contKernelErrorColorLcDef As Object = ConsoleColor.Gray
    Public uncontKernelErrorColorLcDef As Object = ConsoleColor.Gray
    Public hostNameShellColorLcDef As Object = ConsoleColor.Blue
    Public userNameShellColorLcDef As Object = ConsoleColor.Blue
    Public backgroundColorLcDef As Object = ConsoleColor.Black
    Public neutralTextColorLcDef As Object = ConsoleColor.Gray
    Public cmdListColorLcDef As Object = ConsoleColor.White
    Public cmdDefColorLcDef As Object = ConsoleColor.Gray

    'Variables
    Public currentTheme As String

    Public Sub ResetColors()
        inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorDef), ConsoleColor)
        licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorDef), ConsoleColor)
        contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorDef), ConsoleColor)
        uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorDef), ConsoleColor)
        hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorDef), ConsoleColor)
        userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorDef), ConsoleColor)
        backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorDef), ConsoleColor)
        neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorDef), ConsoleColor)
        cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorDef), ConsoleColor)
        cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorDef), ConsoleColor)
        Load()
    End Sub
    Public Sub Load()
        Console.BackgroundColor = backgroundColor
        Console.Clear()
    End Sub
    Public Sub TemplateSet(ByVal theme As String)
        If colorTemplates.Contains(theme) = True Then
            If theme = "Default" Then
                ResetColors()
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
                Load()
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
                Load()
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
                Load()
            ElseIf theme = "Ubuntu" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorU), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorU), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorU), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorU), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorU), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorU), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorU), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorU), ConsoleColor)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorU), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorU), ConsoleColor)
                Load()
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
                Load()
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
                Load()
            End If
            MakePermanent()
            ParseCurrentTheme()
        Else
            Wln(DoTranslation("Invalid color template {0}", currentLang), "neutralText", theme)
        End If
    End Sub

    Public Sub MakePermanent()
        Dim ksconf As New IniFile()
        Dim configPath As String = paths("Configuration")
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
        Dim configPath As String = paths("Configuration")
        ksconf.Load(configPath)
        If ksconf.Sections("Colors").Keys("User Name Shell Color").Value = ConsoleColor.DarkGreen.ToString And
           ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = ConsoleColor.Green.ToString And
           ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = ConsoleColor.Yellow.ToString And
           ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = ConsoleColor.Red.ToString And
           ksconf.Sections("Colors").Keys("Text Color").Value = ConsoleColor.Gray.ToString And
           ksconf.Sections("Colors").Keys("License Color").Value = ConsoleColor.White.ToString And
           ksconf.Sections("Colors").Keys("Background Color").Value = ConsoleColor.Black.ToString And
           ksconf.Sections("Colors").Keys("Input Color").Value = ConsoleColor.White.ToString And
           ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = ConsoleColor.DarkYellow.ToString And
           ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = ConsoleColor.DarkGray.ToString Then
            currentTheme = "Default"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorRC.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorRC.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorRC.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorRC.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorRC.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorRC.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorRC.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorRC.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorRC.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorRC.ToString Then
            currentTheme = "RedConsole"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorBS.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorBS.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorBS.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorBS.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorBS.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorBS.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorBS.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorBS.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorBS.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorBS.ToString Then
            currentTheme = "BlueSpire"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorHckr.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorHckr.ToString Then
            currentTheme = "Hacker"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorU.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorU.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorU.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorU.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorU.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorU.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorU.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorU.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorU.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorU.ToString Then
            currentTheme = "Ubuntu"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorLUnc.ToString Then
            currentTheme = "LinuxUncolored"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorLcDef.ToString Then
            currentTheme = "LinuxColoredDef"
        Else
            currentTheme = "Custom"
        End If
    End Sub

End Module
