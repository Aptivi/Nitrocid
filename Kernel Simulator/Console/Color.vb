
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
    Public inputColor As ConsoleColors = ConsoleColors.White
    Public licenseColor As ConsoleColors = ConsoleColors.White
    Public contKernelErrorColor As ConsoleColors = ConsoleColors.Yellow
    Public uncontKernelErrorColor As ConsoleColors = ConsoleColors.Red
    Public hostNameShellColor As ConsoleColors = ConsoleColors.DarkGreen
    Public userNameShellColor As ConsoleColors = ConsoleColors.Green
    Public backgroundColor As ConsoleColors = ConsoleColors.Black
    Public neutralTextColor As ConsoleColors = ConsoleColors.Gray
    Public cmdListColor As ConsoleColors = ConsoleColors.DarkYellow
    Public cmdDefColor As ConsoleColors = ConsoleColors.DarkGray

    'Templates array (available ones)
    Public colorTemplates() As String = {"Default", "RedConsole", "Bluespire", "Hacker", "Ubuntu", "YellowFG", "YellowBG", "Windows95", "GTASA", "GrayOnYellow", "BlackOnWhite", "LinuxUncolored", "LinuxColoredDef"}

    'Variables for the "Default" theme
    Public inputColorDef As ConsoleColors = inputColor
    Public licenseColorDef As ConsoleColors = licenseColor
    Public contKernelErrorColorDef As ConsoleColors = contKernelErrorColor
    Public uncontKernelErrorColorDef As ConsoleColors = uncontKernelErrorColor
    Public hostNameShellColorDef As ConsoleColors = hostNameShellColor
    Public userNameShellColorDef As ConsoleColors = userNameShellColor
    Public backgroundColorDef As ConsoleColors = backgroundColor
    Public neutralTextColorDef As ConsoleColors = neutralTextColor
    Public cmdListColorDef As ConsoleColors = cmdListColor
    Public cmdDefColorDef As ConsoleColors = cmdDefColor

    'Variables for the "RedConsole" theme
    Public inputColorRC As ConsoleColors = ConsoleColors.Red
    Public licenseColorRC As ConsoleColors = ConsoleColors.Red
    Public contKernelErrorColorRC As ConsoleColors = ConsoleColors.Red
    Public uncontKernelErrorColorRC As ConsoleColors = ConsoleColors.DarkRed
    Public hostNameShellColorRC As ConsoleColors = ConsoleColors.DarkRed
    Public userNameShellColorRC As ConsoleColors = ConsoleColors.Red
    Public backgroundColorRC As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorRC As ConsoleColors = ConsoleColors.Red
    Public cmdListColorRC As ConsoleColors = ConsoleColors.Red
    Public cmdDefColorRC As ConsoleColors = ConsoleColors.DarkRed

    'Variables for the "Bluespire" theme
    Public inputColorBS As ConsoleColors = ConsoleColors.Cyan
    Public licenseColorBS As ConsoleColors = ConsoleColors.Cyan
    Public contKernelErrorColorBS As ConsoleColors = ConsoleColors.Blue
    Public uncontKernelErrorColorBS As ConsoleColors = ConsoleColors.Blue
    Public hostNameShellColorBS As ConsoleColors = ConsoleColors.Blue
    Public userNameShellColorBS As ConsoleColors = ConsoleColors.Blue
    Public backgroundColorBS As ConsoleColors = ConsoleColors.DarkCyan
    Public neutralTextColorBS As ConsoleColors = ConsoleColors.Cyan
    Public cmdListColorBS As ConsoleColors = ConsoleColors.Cyan
    Public cmdDefColorBS As ConsoleColors = ConsoleColors.Blue

    'Variables for the "Hacker" theme
    Public inputColorHckr As ConsoleColors = ConsoleColors.Green
    Public licenseColorHckr As ConsoleColors = ConsoleColors.Green
    Public contKernelErrorColorHckr As ConsoleColors = ConsoleColors.Green
    Public uncontKernelErrorColorHckr As ConsoleColors = ConsoleColors.Green
    Public hostNameShellColorHckr As ConsoleColors = ConsoleColors.Green
    Public userNameShellColorHckr As ConsoleColors = ConsoleColors.Green
    Public backgroundColorHckr As ConsoleColors = ConsoleColors.DarkGray
    Public neutralTextColorHckr As ConsoleColors = ConsoleColors.Green
    Public cmdListColorHckr As ConsoleColors = ConsoleColors.DarkGreen
    Public cmdDefColorHckr As ConsoleColors = ConsoleColors.Green

    'Variables for the "Ubuntu" theme
    Public inputColorU As ConsoleColors = ConsoleColors.White
    Public licenseColorU As ConsoleColors = ConsoleColors.White
    Public contKernelErrorColorU As ConsoleColors = ConsoleColors.White
    Public uncontKernelErrorColorU As ConsoleColors = ConsoleColors.White
    Public hostNameShellColorU As ConsoleColors = ConsoleColors.Gray
    Public userNameShellColorU As ConsoleColors = ConsoleColors.Gray
    Public backgroundColorU As ConsoleColors = ConsoleColors.Purple3
    Public neutralTextColorU As ConsoleColors = ConsoleColors.White
    Public cmdListColorU As ConsoleColors = ConsoleColors.White
    Public cmdDefColorU As ConsoleColors = ConsoleColors.White

    'Variables for the "YellowFG" theme
    Public inputColorYFG As ConsoleColors = ConsoleColors.Yellow
    Public licenseColorYFG As ConsoleColors = ConsoleColors.DarkYellow
    Public contKernelErrorColorYFG As ConsoleColors = ConsoleColors.Yellow
    Public uncontKernelErrorColorYFG As ConsoleColors = ConsoleColors.Yellow
    Public hostNameShellColorYFG As ConsoleColors = ConsoleColors.DarkYellow
    Public userNameShellColorYFG As ConsoleColors = ConsoleColors.DarkYellow
    Public backgroundColorYFG As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorYFG As ConsoleColors = ConsoleColors.Yellow
    Public cmdListColorYFG As ConsoleColors = ConsoleColors.Yellow
    Public cmdDefColorYFG As ConsoleColors = ConsoleColors.DarkYellow

    'Variables for the "YellowBG" theme
    Public inputColorYBG As ConsoleColors = ConsoleColors.Black
    Public licenseColorYBG As ConsoleColors = ConsoleColors.Black
    Public contKernelErrorColorYBG As ConsoleColors = ConsoleColors.Black
    Public uncontKernelErrorColorYBG As ConsoleColors = ConsoleColors.Black
    Public hostNameShellColorYBG As ConsoleColors = ConsoleColors.Black
    Public userNameShellColorYBG As ConsoleColors = ConsoleColors.Black
    Public backgroundColorYBG As ConsoleColors = ConsoleColors.DarkYellow
    Public neutralTextColorYBG As ConsoleColors = ConsoleColors.Black
    Public cmdListColorYBG As ConsoleColors = ConsoleColors.Black
    Public cmdDefColorYBG As ConsoleColors = ConsoleColors.Black

    'Variables for the "Windows95" theme
    Public inputColor95 As ConsoleColors = ConsoleColors.White
    Public licenseColor95 As ConsoleColors = ConsoleColors.White
    Public contKernelErrorColor95 As ConsoleColors = ConsoleColors.White
    Public uncontKernelErrorColor95 As ConsoleColors = ConsoleColors.White
    Public hostNameShellColor95 As ConsoleColors = ConsoleColors.White
    Public userNameShellColor95 As ConsoleColors = ConsoleColors.White
    Public backgroundColor95 As ConsoleColors = ConsoleColors.DarkCyan
    Public neutralTextColor95 As ConsoleColors = ConsoleColors.White
    Public cmdListColor95 As ConsoleColors = ConsoleColors.White
    Public cmdDefColor95 As ConsoleColors = ConsoleColors.White

    'Variables for the "GTASA" theme
    Public inputColorSA As ConsoleColors = ConsoleColors.Yellow
    Public licenseColorSA As ConsoleColors = ConsoleColors.Yellow
    Public contKernelErrorColorSA As ConsoleColors = ConsoleColors.DarkYellow
    Public uncontKernelErrorColorSA As ConsoleColors = ConsoleColors.DarkYellow
    Public hostNameShellColorSA As ConsoleColors = ConsoleColors.Yellow
    Public userNameShellColorSA As ConsoleColors = ConsoleColors.DarkYellow
    Public backgroundColorSA As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorSA As ConsoleColors = ConsoleColors.White
    Public cmdListColorSA As ConsoleColors = ConsoleColors.Yellow
    Public cmdDefColorSA As ConsoleColors = ConsoleColors.DarkYellow

    'Variables for the "GrayOnYellow" theme
    Public inputColorGY As ConsoleColors = ConsoleColors.Gray
    Public licenseColorGY As ConsoleColors = ConsoleColors.Gray
    Public contKernelErrorColorGY As ConsoleColors = ConsoleColors.Gray
    Public uncontKernelErrorColorGY As ConsoleColors = ConsoleColors.Gray
    Public hostNameShellColorGY As ConsoleColors = ConsoleColors.Gray
    Public userNameShellColorGY As ConsoleColors = ConsoleColors.Gray
    Public backgroundColorGY As ConsoleColors = ConsoleColors.DarkYellow
    Public neutralTextColorGY As ConsoleColors = ConsoleColors.Gray
    Public cmdListColorGY As ConsoleColors = ConsoleColors.Gray
    Public cmdDefColorGY As ConsoleColors = ConsoleColors.Gray

    'Variables for the "BlackOnWhite" theme
    Public inputColorBY As ConsoleColors = ConsoleColors.Black
    Public licenseColorBY As ConsoleColors = ConsoleColors.Black
    Public contKernelErrorColorBY As ConsoleColors = ConsoleColors.Black
    Public uncontKernelErrorColorBY As ConsoleColors = ConsoleColors.Black
    Public hostNameShellColorBY As ConsoleColors = ConsoleColors.Black
    Public userNameShellColorBY As ConsoleColors = ConsoleColors.Black
    Public backgroundColorBY As ConsoleColors = ConsoleColors.White
    Public neutralTextColorBY As ConsoleColors = ConsoleColors.Black
    Public cmdListColorBY As ConsoleColors = ConsoleColors.Black
    Public cmdDefColorBY As ConsoleColors = ConsoleColors.Black

    'Variables for the "LinuxUncolored" theme
    Public inputColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public licenseColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public contKernelErrorColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public uncontKernelErrorColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public hostNameShellColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public userNameShellColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public backgroundColorLUnc As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public cmdListColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public cmdDefColorLUnc As ConsoleColors = ConsoleColors.Gray

    'Variables for the "LinuxColoredDef" theme
    'If there is a mistake in colors, please fix it.
    Public inputColorLcDef As ConsoleColors = ConsoleColors.Gray
    Public licenseColorLcDef As ConsoleColors = ConsoleColors.Gray
    Public contKernelErrorColorLcDef As ConsoleColors = ConsoleColors.Gray
    Public uncontKernelErrorColorLcDef As ConsoleColors = ConsoleColors.Gray
    Public hostNameShellColorLcDef As ConsoleColors = ConsoleColors.Blue
    Public userNameShellColorLcDef As ConsoleColors = ConsoleColors.Blue
    Public backgroundColorLcDef As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorLcDef As ConsoleColors = ConsoleColors.Gray
    Public cmdListColorLcDef As ConsoleColors = ConsoleColors.White
    Public cmdDefColorLcDef As ConsoleColors = ConsoleColors.Gray

    'Variables
    Public currentTheme As String

    Public Sub ResetColors()
        inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorDef), ConsoleColors)
        licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorDef), ConsoleColors)
        contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorDef), ConsoleColors)
        uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorDef), ConsoleColors)
        hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorDef), ConsoleColors)
        userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorDef), ConsoleColors)
        backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorDef), ConsoleColors)
        neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorDef), ConsoleColors)
        cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorDef), ConsoleColors)
        cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorDef), ConsoleColors)
        Load()
    End Sub
    Public Sub Load()
        Dim esc As Char = GetEsc()
        Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        Console.Clear()
    End Sub
    Public Sub TemplateSet(ByVal theme As String)
        If colorTemplates.Contains(theme) = True Then
            If theme = "Default" Then
                ResetColors()
            ElseIf theme = "RedConsole" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorRC), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorRC), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorRC), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorRC), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorRC), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorRC), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorRC), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorRC), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorRC), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorRC), ConsoleColors)
                Load()
            ElseIf theme = "Bluespire" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorBS), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorBS), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorBS), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorBS), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorBS), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorBS), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorBS), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorBS), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorBS), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorBS), ConsoleColors)
                Load()
            ElseIf theme = "Hacker" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorHckr), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorHckr), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorHckr), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorHckr), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorHckr), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorHckr), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorHckr), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorHckr), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorHckr), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorHckr), ConsoleColors)
                Load()
            ElseIf theme = "Ubuntu" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorU), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorU), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorU), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorU), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorU), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorU), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorU), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorU), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorU), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorU), ConsoleColors)
                Load()
            ElseIf theme = "YellowFG" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorYFG), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorYFG), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorYFG), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorYFG), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorYFG), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorYFG), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorYFG), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorYFG), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorYFG), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorYFG), ConsoleColors)
                Load()
            ElseIf theme = "YellowBG" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorYBG), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorYBG), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorYBG), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorYBG), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorYBG), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorYBG), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorYBG), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorYBG), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorYBG), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorYBG), ConsoleColors)
                Load()
            ElseIf theme = "Windows95" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColor95), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColor95), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColor95), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColor95), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColor95), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColor95), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColor95), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColor95), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColor95), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColor95), ConsoleColors)
                Load()
            ElseIf theme = "GTASA" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorSA), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorSA), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorSA), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorSA), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorSA), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorSA), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorSA), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorSA), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorSA), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorSA), ConsoleColors)
                Load()
            ElseIf theme = "GrayOnYellow" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorGY), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorGY), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorGY), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorGY), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorGY), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorGY), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorGY), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorGY), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorGY), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorGY), ConsoleColors)
                Load()
            ElseIf theme = "BlackOnWhite" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorBY), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorBY), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorBY), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorBY), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorBY), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorBY), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorBY), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorBY), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorBY), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorBY), ConsoleColors)
                Load()
            ElseIf theme = "LinuxUncolored" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorLUnc), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorLUnc), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorLUnc), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorLUnc), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorLUnc), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorLUnc), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorLUnc), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorLUnc), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorLUnc), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorLUnc), ConsoleColors)
                Load()
            ElseIf theme = "LinuxColoredDef" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorLcDef), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorLcDef), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorLcDef), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorLcDef), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorLcDef), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorLcDef), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorLcDef), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorLcDef), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorLcDef), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorLcDef), ConsoleColors)
                Load()
            End If
            MakePermanent()
            ParseCurrentTheme()
        Else
            W(DoTranslation("Invalid color template {0}", currentLang), True, ColTypes.Neutral, theme)
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
        If ksconf.Sections("Colors").Keys("User Name Shell Color").Value = ConsoleColors.DarkGreen.ToString And
           ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = ConsoleColors.Green.ToString And
           ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = ConsoleColors.Yellow.ToString And
           ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = ConsoleColors.Red.ToString And
           ksconf.Sections("Colors").Keys("Text Color").Value = ConsoleColors.Gray.ToString And
           ksconf.Sections("Colors").Keys("License Color").Value = ConsoleColors.White.ToString And
           ksconf.Sections("Colors").Keys("Background Color").Value = ConsoleColors.Black.ToString And
           ksconf.Sections("Colors").Keys("Input Color").Value = ConsoleColors.White.ToString And
           ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = ConsoleColors.DarkYellow.ToString And
           ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = ConsoleColors.DarkGray.ToString Then
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
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorYFG.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorYFG.ToString Then
            currentTheme = "YellowFG"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorYBG.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorYBG.ToString Then
            currentTheme = "YellowBG"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColor95.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColor95.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColor95.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColor95.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColor95.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColor95.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColor95.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColor95.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColor95.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColor95.ToString Then
            currentTheme = "Windows95"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorSA.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorSA.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorSA.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorSA.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorSA.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorSA.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorSA.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorSA.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorSA.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorSA.ToString Then
            currentTheme = "GTASA"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorGY.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorGY.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorGY.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorGY.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorGY.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorGY.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorGY.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorGY.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorGY.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorGY.ToString Then
            currentTheme = "GrayOnYellow"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorBY.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorBY.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorBY.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorBY.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorBY.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorBY.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorBY.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorBY.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorBY.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorBY.ToString Then
            currentTheme = "BlackOnWhite"
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
