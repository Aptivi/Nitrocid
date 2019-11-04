
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

    'TODO: Make W() with the "def" color write the text in black if it encountered a light background so the shell prompt is readable
    'Variables for colors used by previous versions of Kernel.
    Public inputColor As ConsoleColor = ConsoleColor.White
    Public licenseColor As ConsoleColor = ConsoleColor.White
    Public contKernelErrorColor As ConsoleColor = ConsoleColor.Yellow
    Public uncontKernelErrorColor As ConsoleColor = ConsoleColor.Red
    Public hostNameShellColor As ConsoleColor = ConsoleColor.DarkGreen
    Public userNameShellColor As ConsoleColor = ConsoleColor.Green
    Public backgroundColor As ConsoleColor = ConsoleColor.Black
    Public neutralTextColor As ConsoleColor = ConsoleColor.Gray
    Public cmdListColor As ConsoleColor = ConsoleColor.DarkYellow
    Public cmdDefColor As ConsoleColor = ConsoleColor.DarkGray

    'Array for available colors
    Public availableColors() As String = {"White", "Black", "Gray", "DarkGray", "DarkRed", "Red", "DarkYellow", "Yellow", "DarkGreen", "Green",
                                          "DarkCyan", "Cyan", "DarkBlue", "Blue", "DarkMagenta", "Magenta", "RESET"}

    'Templates array (available ones)
    Public colorTemplates() As String = {"Default", "RedConsole", "Bluespire", "Hacker", "Ubuntu", "YellowFG", "YellowBG", "Windows95", "GTASA", "GrayOnYellow", "BlackOnWhite", "LinuxUncolored", "LinuxColoredDef"}

    'Variables for the "Default" theme
    Public inputColorDef As ConsoleColor = inputColor
    Public licenseColorDef As ConsoleColor = licenseColor
    Public contKernelErrorColorDef As ConsoleColor = contKernelErrorColor
    Public uncontKernelErrorColorDef As ConsoleColor = uncontKernelErrorColor
    Public hostNameShellColorDef As ConsoleColor = hostNameShellColor
    Public userNameShellColorDef As ConsoleColor = userNameShellColor
    Public backgroundColorDef As ConsoleColor = backgroundColor
    Public neutralTextColorDef As ConsoleColor = neutralTextColor
    Public cmdListColorDef As ConsoleColor = cmdListColor
    Public cmdDefColorDef As ConsoleColor = cmdDefColor

    'Variables for the "RedConsole" theme
    Public inputColorRC As ConsoleColor = ConsoleColor.Red
    Public licenseColorRC As ConsoleColor = ConsoleColor.Red
    Public contKernelErrorColorRC As ConsoleColor = ConsoleColor.Red
    Public uncontKernelErrorColorRC As ConsoleColor = ConsoleColor.DarkRed
    Public hostNameShellColorRC As ConsoleColor = ConsoleColor.DarkRed
    Public userNameShellColorRC As ConsoleColor = ConsoleColor.Red
    Public backgroundColorRC As ConsoleColor = ConsoleColor.Black
    Public neutralTextColorRC As ConsoleColor = ConsoleColor.Red
    Public cmdListColorRC As ConsoleColor = ConsoleColor.Red
    Public cmdDefColorRC As ConsoleColor = ConsoleColor.DarkRed

    'Variables for the "Bluespire" theme
    Public inputColorBS As ConsoleColor = ConsoleColor.Cyan
    Public licenseColorBS As ConsoleColor = ConsoleColor.Cyan
    Public contKernelErrorColorBS As ConsoleColor = ConsoleColor.Blue
    Public uncontKernelErrorColorBS As ConsoleColor = ConsoleColor.Blue
    Public hostNameShellColorBS As ConsoleColor = ConsoleColor.Blue
    Public userNameShellColorBS As ConsoleColor = ConsoleColor.Blue
    Public backgroundColorBS As ConsoleColor = ConsoleColor.DarkCyan
    Public neutralTextColorBS As ConsoleColor = ConsoleColor.Cyan
    Public cmdListColorBS As ConsoleColor = ConsoleColor.Cyan
    Public cmdDefColorBS As ConsoleColor = ConsoleColor.Blue

    'Variables for the "Hacker" theme
    Public inputColorHckr As ConsoleColor = ConsoleColor.Green
    Public licenseColorHckr As ConsoleColor = ConsoleColor.Green
    Public contKernelErrorColorHckr As ConsoleColor = ConsoleColor.Green
    Public uncontKernelErrorColorHckr As ConsoleColor = ConsoleColor.Green
    Public hostNameShellColorHckr As ConsoleColor = ConsoleColor.Green
    Public userNameShellColorHckr As ConsoleColor = ConsoleColor.Green
    Public backgroundColorHckr As ConsoleColor = ConsoleColor.DarkGray
    Public neutralTextColorHckr As ConsoleColor = ConsoleColor.Green
    Public cmdListColorHckr As ConsoleColor = ConsoleColor.DarkGreen
    Public cmdDefColorHckr As ConsoleColor = ConsoleColor.Green

    'Variables for the "Ubuntu" theme
    Public inputColorU As ConsoleColor = ConsoleColor.White
    Public licenseColorU As ConsoleColor = ConsoleColor.White
    Public contKernelErrorColorU As ConsoleColor = ConsoleColor.White
    Public uncontKernelErrorColorU As ConsoleColor = ConsoleColor.White
    Public hostNameShellColorU As ConsoleColor = ConsoleColor.Gray
    Public userNameShellColorU As ConsoleColor = ConsoleColor.Gray
    Public backgroundColorU As ConsoleColor = ConsoleColor.DarkMagenta
    Public neutralTextColorU As ConsoleColor = ConsoleColor.White
    Public cmdListColorU As ConsoleColor = ConsoleColor.White
    Public cmdDefColorU As ConsoleColor = ConsoleColor.White

    'Variables for the "YellowFG" theme
    Public inputColorYFG As ConsoleColor = ConsoleColor.Yellow
    Public licenseColorYFG As ConsoleColor = ConsoleColor.DarkYellow
    Public contKernelErrorColorYFG As ConsoleColor = ConsoleColor.Yellow
    Public uncontKernelErrorColorYFG As ConsoleColor = ConsoleColor.Yellow
    Public hostNameShellColorYFG As ConsoleColor = ConsoleColor.DarkYellow
    Public userNameShellColorYFG As ConsoleColor = ConsoleColor.DarkYellow
    Public backgroundColorYFG As ConsoleColor = ConsoleColor.Black
    Public neutralTextColorYFG As ConsoleColor = ConsoleColor.Yellow
    Public cmdListColorYFG As ConsoleColor = ConsoleColor.Yellow
    Public cmdDefColorYFG As ConsoleColor = ConsoleColor.DarkYellow

    'Variables for the "YellowBG" theme
    Public inputColorYBG As ConsoleColor = ConsoleColor.Black
    Public licenseColorYBG As ConsoleColor = ConsoleColor.Black
    Public contKernelErrorColorYBG As ConsoleColor = ConsoleColor.Black
    Public uncontKernelErrorColorYBG As ConsoleColor = ConsoleColor.Black
    Public hostNameShellColorYBG As ConsoleColor = ConsoleColor.Black
    Public userNameShellColorYBG As ConsoleColor = ConsoleColor.Black
    Public backgroundColorYBG As ConsoleColor = ConsoleColor.DarkYellow
    Public neutralTextColorYBG As ConsoleColor = ConsoleColor.Black
    Public cmdListColorYBG As ConsoleColor = ConsoleColor.Black
    Public cmdDefColorYBG As ConsoleColor = ConsoleColor.Black

    'Variables for the "Windows95" theme
    Public inputColor95 As ConsoleColor = ConsoleColor.White
    Public licenseColor95 As ConsoleColor = ConsoleColor.White
    Public contKernelErrorColor95 As ConsoleColor = ConsoleColor.White
    Public uncontKernelErrorColor95 As ConsoleColor = ConsoleColor.White
    Public hostNameShellColor95 As ConsoleColor = ConsoleColor.White
    Public userNameShellColor95 As ConsoleColor = ConsoleColor.White
    Public backgroundColor95 As ConsoleColor = ConsoleColor.DarkCyan
    Public neutralTextColor95 As ConsoleColor = ConsoleColor.White
    Public cmdListColor95 As ConsoleColor = ConsoleColor.White
    Public cmdDefColor95 As ConsoleColor = ConsoleColor.White

    'Variables for the "GTASA" theme
    Public inputColorSA As ConsoleColor = ConsoleColor.Yellow
    Public licenseColorSA As ConsoleColor = ConsoleColor.Yellow
    Public contKernelErrorColorSA As ConsoleColor = ConsoleColor.DarkYellow
    Public uncontKernelErrorColorSA As ConsoleColor = ConsoleColor.DarkYellow
    Public hostNameShellColorSA As ConsoleColor = ConsoleColor.Yellow
    Public userNameShellColorSA As ConsoleColor = ConsoleColor.DarkYellow
    Public backgroundColorSA As ConsoleColor = ConsoleColor.Black
    Public neutralTextColorSA As ConsoleColor = ConsoleColor.White
    Public cmdListColorSA As ConsoleColor = ConsoleColor.Yellow
    Public cmdDefColorSA As ConsoleColor = ConsoleColor.DarkYellow

    'Variables for the "GrayOnYellow" theme
    Public inputColorGY As ConsoleColor = ConsoleColor.Gray
    Public licenseColorGY As ConsoleColor = ConsoleColor.Gray
    Public contKernelErrorColorGY As ConsoleColor = ConsoleColor.Gray
    Public uncontKernelErrorColorGY As ConsoleColor = ConsoleColor.Gray
    Public hostNameShellColorGY As ConsoleColor = ConsoleColor.Gray
    Public userNameShellColorGY As ConsoleColor = ConsoleColor.Gray
    Public backgroundColorGY As ConsoleColor = ConsoleColor.DarkYellow
    Public neutralTextColorGY As ConsoleColor = ConsoleColor.Gray
    Public cmdListColorGY As ConsoleColor = ConsoleColor.Gray
    Public cmdDefColorGY As ConsoleColor = ConsoleColor.Gray

    'Variables for the "BlackOnWhite" theme
    Public inputColorBY As ConsoleColor = ConsoleColor.Black
    Public licenseColorBY As ConsoleColor = ConsoleColor.Black
    Public contKernelErrorColorBY As ConsoleColor = ConsoleColor.Black
    Public uncontKernelErrorColorBY As ConsoleColor = ConsoleColor.Black
    Public hostNameShellColorBY As ConsoleColor = ConsoleColor.Black
    Public userNameShellColorBY As ConsoleColor = ConsoleColor.Black
    Public backgroundColorBY As ConsoleColor = ConsoleColor.White
    Public neutralTextColorBY As ConsoleColor = ConsoleColor.Black
    Public cmdListColorBY As ConsoleColor = ConsoleColor.Black
    Public cmdDefColorBY As ConsoleColor = ConsoleColor.Black

    'Variables for the "LinuxUncolored" theme
    Public inputColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public licenseColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public contKernelErrorColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public uncontKernelErrorColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public hostNameShellColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public userNameShellColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public backgroundColorLUnc As ConsoleColor = ConsoleColor.Black
    Public neutralTextColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public cmdListColorLUnc As ConsoleColor = ConsoleColor.Gray
    Public cmdDefColorLUnc As ConsoleColor = ConsoleColor.Gray

    'Variables for the "LinuxColoredDef" theme
    'If there is a mistake in colors, please fix it.
    Public inputColorLcDef As ConsoleColor = ConsoleColor.Gray
    Public licenseColorLcDef As ConsoleColor = ConsoleColor.Gray
    Public contKernelErrorColorLcDef As ConsoleColor = ConsoleColor.Gray
    Public uncontKernelErrorColorLcDef As ConsoleColor = ConsoleColor.Gray
    Public hostNameShellColorLcDef As ConsoleColor = ConsoleColor.Blue
    Public userNameShellColorLcDef As ConsoleColor = ConsoleColor.Blue
    Public backgroundColorLcDef As ConsoleColor = ConsoleColor.Black
    Public neutralTextColorLcDef As ConsoleColor = ConsoleColor.Gray
    Public cmdListColorLcDef As ConsoleColor = ConsoleColor.White
    Public cmdDefColorLcDef As ConsoleColor = ConsoleColor.Gray

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
            ElseIf theme = "YellowFG" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorYFG), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorYFG), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorYFG), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorYFG), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorYFG), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorYFG), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorYFG), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorYFG), ConsoleColor)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorYFG), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorYFG), ConsoleColor)
                Load()
            ElseIf theme = "YellowBG" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorYBG), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorYBG), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorYBG), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorYBG), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorYBG), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorYBG), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorYBG), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorYBG), ConsoleColor)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorYBG), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorYBG), ConsoleColor)
                Load()
            ElseIf theme = "Windows95" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColor95), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColor95), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColor95), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColor95), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColor95), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColor95), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColor95), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColor95), ConsoleColor)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColor95), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColor95), ConsoleColor)
                Load()
            ElseIf theme = "GTASA" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorSA), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorSA), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorSA), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorSA), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorSA), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorSA), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorSA), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorSA), ConsoleColor)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorSA), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorSA), ConsoleColor)
                Load()
            ElseIf theme = "GrayOnYellow" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorGY), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorGY), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorGY), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorGY), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorGY), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorGY), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorGY), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorGY), ConsoleColor)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorGY), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorGY), ConsoleColor)
                Load()
            ElseIf theme = "BlackOnWhite" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColor), inputColorBY), ConsoleColor)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColor), licenseColorBY), ConsoleColor)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), contKernelErrorColorBY), ConsoleColor)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), uncontKernelErrorColorBY), ConsoleColor)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), hostNameShellColorBY), ConsoleColor)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), userNameShellColorBY), ConsoleColor)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), backgroundColorBY), ConsoleColor)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), neutralTextColorBY), ConsoleColor)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), cmdListColorBY), ConsoleColor)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), cmdDefColorBY), ConsoleColor)
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
