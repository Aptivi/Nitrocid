
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
    Public stageColor As ConsoleColors = ConsoleColors.Green
    Public errorColor As ConsoleColors = ConsoleColors.Red

    'Templates array (available ones)
    Public colorTemplates() As String = {"Default", "RedConsole", "Bluespire", "Hacker", "Ubuntu", "YellowFG", "YellowBG", "Windows95", "GTASA", "GrayOnYellow",
                                         "BlackOnWhite", "Debian", "NFSHP-Cop", "NFSHP-Racer", "TealerOS", "BedOS", "3Y-Diamond", "LinuxUncolored", "LinuxColoredDef"}

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
    Public stageColorDef As ConsoleColors = stageColor
    Public errorColorDef As ConsoleColors = errorColor

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
    Public stageColorRC As ConsoleColors = ConsoleColors.Red
    Public errorColorRC As ConsoleColors = ConsoleColors.DarkRed

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
    Public stageColorBS As ConsoleColors = ConsoleColors.Cyan
    Public errorColorBS As ConsoleColors = ConsoleColors.Blue

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
    Public stageColorHckr As ConsoleColors = ConsoleColors.DarkGreen
    Public errorColorHckr As ConsoleColors = ConsoleColors.DarkGreen

    'Variables for the "Ubuntu" theme
    Public inputColorU As ConsoleColors = ConsoleColors.White
    Public licenseColorU As ConsoleColors = ConsoleColors.White
    Public contKernelErrorColorU As ConsoleColors = ConsoleColors.White
    Public uncontKernelErrorColorU As ConsoleColors = ConsoleColors.White
    Public hostNameShellColorU As ConsoleColors = ConsoleColors.Gray
    Public userNameShellColorU As ConsoleColors = ConsoleColors.Gray
    Public backgroundColorU As ConsoleColors = ConsoleColors.DeepPink4
    Public neutralTextColorU As ConsoleColors = ConsoleColors.White
    Public cmdListColorU As ConsoleColors = ConsoleColors.White
    Public cmdDefColorU As ConsoleColors = ConsoleColors.White
    Public stageColorU As ConsoleColors = ConsoleColors.White
    Public errorColorU As ConsoleColors = ConsoleColors.Gray

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
    Public stageColorYFG As ConsoleColors = ConsoleColors.Yellow
    Public errorColorYFG As ConsoleColors = ConsoleColors.DarkYellow

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
    Public stageColorYBG As ConsoleColors = ConsoleColors.Black
    Public errorColorYBG As ConsoleColors = ConsoleColors.Black

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
    Public stageColor95 As ConsoleColors = ConsoleColors.White
    Public errorColor95 As ConsoleColors = ConsoleColors.White

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
    Public stageColorSA As ConsoleColors = ConsoleColors.Yellow
    Public errorColorSA As ConsoleColors = ConsoleColors.DarkYellow

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
    Public stageColorGY As ConsoleColors = ConsoleColors.Gray
    Public errorColorGY As ConsoleColors = ConsoleColors.Gray

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
    Public stageColorBY As ConsoleColors = ConsoleColors.Black
    Public errorColorBY As ConsoleColors = ConsoleColors.Black

    'Variables for the "Debian" theme
    Public inputColorD As ConsoleColors = ConsoleColors.White
    Public licenseColorD As ConsoleColors = ConsoleColors.White
    Public contKernelErrorColorD As ConsoleColors = ConsoleColors.White
    Public uncontKernelErrorColorD As ConsoleColors = ConsoleColors.White
    Public hostNameShellColorD As ConsoleColors = ConsoleColors.Gray
    Public userNameShellColorD As ConsoleColors = ConsoleColors.Gray
    Public backgroundColorD As ConsoleColors = ConsoleColors.DeepPink3
    Public neutralTextColorD As ConsoleColors = ConsoleColors.White
    Public cmdListColorD As ConsoleColors = ConsoleColors.White
    Public cmdDefColorD As ConsoleColors = ConsoleColors.White
    Public stageColorD As ConsoleColors = ConsoleColors.White
    Public errorColorD As ConsoleColors = ConsoleColors.Gray

    'Variables for the "NFSHP-Cop" theme
    Public inputColorNFSHPC As ConsoleColors = ConsoleColors.Red
    Public licenseColorNFSHPC As ConsoleColors = ConsoleColors.Blue3
    Public contKernelErrorColorNFSHPC As ConsoleColors = ConsoleColors.DarkBlue
    Public uncontKernelErrorColorNFSHPC As ConsoleColors = ConsoleColors.DarkRed_870000
    Public hostNameShellColorNFSHPC As ConsoleColors = ConsoleColors.DarkBlue
    Public userNameShellColorNFSHPC As ConsoleColors = ConsoleColors.Red
    Public backgroundColorNFSHPC As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorNFSHPC As ConsoleColors = ConsoleColors.DarkBlue
    Public cmdListColorNFSHPC As ConsoleColors = ConsoleColors.DarkBlue
    Public cmdDefColorNFSHPC As ConsoleColors = ConsoleColors.Red
    Public stageColorNFSHPC As ConsoleColors = ConsoleColors.Red
    Public errorColorNFSHPC As ConsoleColors = ConsoleColors.DarkRed_870000

    'Variables for the "NFSHP-Racer" theme
    Public inputColorNFSHPR As ConsoleColors = ConsoleColors.White
    Public licenseColorNFSHPR As ConsoleColors = ConsoleColors.White
    Public contKernelErrorColorNFSHPR As ConsoleColors = ConsoleColors.Orange4_875f00
    Public uncontKernelErrorColorNFSHPR As ConsoleColors = ConsoleColors.DarkRed_870000
    Public hostNameShellColorNFSHPR As ConsoleColors = ConsoleColors.Orange4_875f00
    Public userNameShellColorNFSHPR As ConsoleColors = ConsoleColors.Orange3
    Public backgroundColorNFSHPR As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorNFSHPR As ConsoleColors = ConsoleColors.Gold3_d7af00
    Public cmdListColorNFSHPR As ConsoleColors = ConsoleColors.Orange4_875f00
    Public cmdDefColorNFSHPR As ConsoleColors = ConsoleColors.White
    Public stageColorNFSHPR As ConsoleColors = ConsoleColors.Yellow3_d7d700
    Public errorColorNFSHPR As ConsoleColors = ConsoleColors.DarkRed_870000

    'Variables for the "TealerOS" theme
    Public inputColorTOS As ConsoleColors = ConsoleColors.SkyBlue3
    Public licenseColorTOS As ConsoleColors = ConsoleColors.SkyBlue3
    Public contKernelErrorColorTOS As ConsoleColors = ConsoleColors.DeepSkyBlue4_005f5f
    Public uncontKernelErrorColorTOS As ConsoleColors = ConsoleColors.DeepSkyBlue4_005f5f
    Public hostNameShellColorTOS As ConsoleColors = ConsoleColors.DeepSkyBlue4_005f5f
    Public userNameShellColorTOS As ConsoleColors = ConsoleColors.SkyBlue3
    Public backgroundColorTOS As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorTOS As ConsoleColors = ConsoleColors.SkyBlue3
    Public cmdListColorTOS As ConsoleColors = ConsoleColors.DeepSkyBlue4_005f5f
    Public cmdDefColorTOS As ConsoleColors = ConsoleColors.SkyBlue3
    Public stageColorTOS As ConsoleColors = ConsoleColors.MediumTurquoise
    Public errorColorTOS As ConsoleColors = ConsoleColors.DeepSkyBlue4_005f5f

    'Variables for the "BedOS" theme
    Public inputColorBOS As ConsoleColors = ConsoleColors.SkyBlue3
    Public licenseColorBOS As ConsoleColors = ConsoleColors.Violet
    Public contKernelErrorColorBOS As ConsoleColors = ConsoleColors.OrangeRed1
    Public uncontKernelErrorColorBOS As ConsoleColors = ConsoleColors.OrangeRed1
    Public hostNameShellColorBOS As ConsoleColors = ConsoleColors.DarkOrange3
    Public userNameShellColorBOS As ConsoleColors = ConsoleColors.OrangeRed1
    Public backgroundColorBOS As ConsoleColors = ConsoleColors.Black
    Public neutralTextColorBOS As ConsoleColors = ConsoleColors.Violet
    Public cmdListColorBOS As ConsoleColors = ConsoleColors.Blue
    Public cmdDefColorBOS As ConsoleColors = ConsoleColors.MediumTurquoise
    Public stageColorBOS As ConsoleColors = ConsoleColors.MediumTurquoise
    Public errorColorBOS As ConsoleColors = ConsoleColors.OrangeRed1

    'Variables for the "3Y-Diamond" theme
    Public inputColor3YD As ConsoleColors = ConsoleColors.MediumPurple4
    Public licenseColor3YD As ConsoleColors = ConsoleColors.DarkOliveGreen3_87d75f
    Public contKernelErrorColor3YD As ConsoleColors = ConsoleColors.LightSteelBlue3
    Public uncontKernelErrorColor3YD As ConsoleColors = ConsoleColors.LightSteelBlue3
    Public hostNameShellColor3YD As ConsoleColors = ConsoleColors.MediumPurple4
    Public userNameShellColor3YD As ConsoleColors = ConsoleColors.SpringGreen3_00af5f
    Public backgroundColor3YD As ConsoleColors = ConsoleColors.Black
    Public neutralTextColor3YD As ConsoleColors = ConsoleColors.DarkOliveGreen3_87d75f
    Public cmdListColor3YD As ConsoleColors = ConsoleColors.DarkOliveGreen3_87d75f
    Public cmdDefColor3YD As ConsoleColors = ConsoleColors.SpringGreen3_00af5f
    Public stageColor3YD As ConsoleColors = ConsoleColors.LightSteelBlue3
    Public errorColor3YD As ConsoleColors = ConsoleColors.LightSteelBlue3

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
    Public stageColorLUnc As ConsoleColors = ConsoleColors.Gray
    Public errorColorLUnc As ConsoleColors = ConsoleColors.Gray

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
    Public stageColorLcDef As ConsoleColors = ConsoleColors.White
    Public errorColorLcDef As ConsoleColors = ConsoleColors.Gray

    'Variables
    Public currentTheme As String

    ''' <summary>
    ''' Resets all colors to default
    ''' </summary>
    Public Sub ResetColors()
        Wdbg("I", "Resetting colors")
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
        stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorDef), ConsoleColors)
        errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorDef), ConsoleColors)
        LoadBack()
    End Sub

    ''' <summary>
    ''' Loads the background
    ''' </summary>
    Public Sub LoadBack()
        Try
            Wdbg("I", "Filling background with background color")
            Dim esc As Char = GetEsc()
            Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            Console.Clear()
        Catch ex As Exception
            Wdbg("E", "Failed to set background: {0}", ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Sets system colors according to the programmed templates
    ''' </summary>
    ''' <param name="theme">A specified theme</param>
    Public Sub TemplateSet(ByVal theme As String)
        Wdbg("I", "Theme: {0}", theme)
        If colorTemplates.Contains(theme) Then
            Wdbg("I", "Theme found.")
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorRC), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorRC), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorBS), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorBS), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorHckr), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorHckr), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorU), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorU), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorYFG), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorYFG), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorYBG), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorYBG), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColor95), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), stageColor95), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorSA), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorSA), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorGY), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorGY), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorBY), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorBY), ConsoleColors)
                LoadBack()
            ElseIf theme = "Debian" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorD), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorD), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorD), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorD), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorD), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorD), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorD), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorD), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorD), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorD), ConsoleColors)
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorD), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorD), ConsoleColors)
                LoadBack()
            ElseIf theme = "NFSHP-Cop" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorNFSHPC), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorNFSHPC), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorNFSHPC), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorNFSHPC), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorNFSHPC), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorNFSHPC), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorNFSHPC), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorNFSHPC), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorNFSHPC), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorNFSHPC), ConsoleColors)
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorNFSHPC), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorNFSHPC), ConsoleColors)
                LoadBack()
            ElseIf theme = "NFSHP-Racer" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorNFSHPR), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorNFSHPR), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorNFSHPR), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorNFSHPR), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorNFSHPR), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorNFSHPR), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorNFSHPR), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorNFSHPR), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorNFSHPR), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorNFSHPR), ConsoleColors)
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorNFSHPR), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorNFSHPR), ConsoleColors)
                LoadBack()
            ElseIf theme = "TealerOS" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorTOS), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorTOS), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorTOS), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorTOS), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorTOS), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorTOS), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorTOS), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorTOS), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorTOS), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorTOS), ConsoleColors)
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorTOS), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorTOS), ConsoleColors)
                LoadBack()
            ElseIf theme = "BedOS" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColorBOS), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColorBOS), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColorBOS), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColorBOS), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColorBOS), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColorBOS), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColorBOS), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColorBOS), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColorBOS), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColorBOS), ConsoleColors)
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorBOS), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorBOS), ConsoleColors)
                LoadBack()
            ElseIf theme = "3Y-Diamond" Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), inputColor3YD), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), licenseColor3YD), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), contKernelErrorColor3YD), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), uncontKernelErrorColor3YD), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), hostNameShellColor3YD), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), userNameShellColor3YD), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), backgroundColor3YD), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), neutralTextColor3YD), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), cmdListColor3YD), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), cmdDefColor3YD), ConsoleColors)
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColor3YD), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColor3YD), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorLUnc), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorLUnc), ConsoleColors)
                LoadBack()
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
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), stageColorLcDef), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), errorColorLcDef), ConsoleColors)
                LoadBack()
            End If
            Wdbg("I", "Saving theme")
            MakePermanent()
            Wdbg("I", "System information changing for theme")
            ParseCurrentTheme()
        Else
            W(DoTranslation("Invalid color template {0}", currentLang), True, ColTypes.Err, theme)
            Wdbg("E", "Theme not found.")
        End If
    End Sub

    ''' <summary>
    ''' Makes the color configuration permanent
    ''' </summary>
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
        ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColor.ToString
        ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColor.ToString
        ksconf.Save(configPath)
    End Sub

    ''' <summary>
    ''' Parses the current theme for sysinfo
    ''' </summary>
    Public Sub ParseCurrentTheme()
        Dim ksconf As New IniFile()
        Dim configPath As String = paths("Configuration")
        ksconf.Load(configPath)
        If ksconf.Sections("Colors").Keys("User Name Shell Color").Value = ConsoleColors.Green.ToString And
           ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = ConsoleColors.DarkGreen.ToString And
           ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = ConsoleColors.Yellow.ToString And
           ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = ConsoleColors.Red.ToString And
           ksconf.Sections("Colors").Keys("Text Color").Value = ConsoleColors.Gray.ToString And
           ksconf.Sections("Colors").Keys("License Color").Value = ConsoleColors.White.ToString And
           ksconf.Sections("Colors").Keys("Background Color").Value = ConsoleColors.Black.ToString And
           ksconf.Sections("Colors").Keys("Input Color").Value = ConsoleColors.White.ToString And
           ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = ConsoleColors.DarkYellow.ToString And
           ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = ConsoleColors.DarkGray.ToString And
           ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = ConsoleColors.Green.ToString And
           ksconf.Sections("Colors").Keys("Error Text Color").Value = ConsoleColors.Red.ToString Then
            Wdbg("I", "Theme set to Default")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorRC.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorRC.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorRC.ToString Then
            Wdbg("I", "Theme set to RedConsole")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorBS.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorBS.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorBS.ToString Then
            Wdbg("I", "Theme set to Bluespire")
            currentTheme = "Bluespire"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorHckr.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorHckr.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorHckr.ToString Then
            Wdbg("I", "Theme set to Hacker")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorU.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorU.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorU.ToString Then
            Wdbg("I", "Theme set to Ubuntu")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorYFG.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorYFG.ToString Then
            Wdbg("I", "Theme set to YellowFG")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorYBG.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorYBG.ToString Then
            Wdbg("I", "Theme set to YellowBG")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColor95.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColor95.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColor95.ToString Then
            Wdbg("I", "Theme set to Windows 95")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorSA.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorSA.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorSA.ToString Then
            Wdbg("I", "Theme set to GTA: SA")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorGY.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorGY.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorGY.ToString Then
            Wdbg("I", "Theme set to Gray on Yellow")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorBY.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorBY.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorBY.ToString Then
            Wdbg("I", "Theme set to Black On White")
            currentTheme = "BlackOnWhite"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorD.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorD.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorD.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorD.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorD.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorD.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorD.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorD.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorD.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorD.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorD.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorD.ToString Then
            Wdbg("I", "Theme set to Debian")
            currentTheme = "Debian"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorNFSHPC.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorNFSHPC.ToString Then
            Wdbg("I", "Theme set to Need for Speed: Hot Pursuit (Cop)")
            currentTheme = "NFSHP-Cop"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorNFSHPR.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorNFSHPR.ToString Then
            Wdbg("I", "Theme set to Need for Speed: Hot Pursuit (Racer)")
            currentTheme = "NFSHP-Racer"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorTOS.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorTOS.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorTOS.ToString Then
            Wdbg("I", "Theme set to TealerOS")
            currentTheme = "TealerOS"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorBOS.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorBOS.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorBOS.ToString Then
            Wdbg("I", "Theme set to BedOS")
            currentTheme = "BedOS"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColor3YD.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColor3YD.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColor3YD.ToString Then
            Wdbg("I", "Theme set to 3Y-Diamond OS")
            currentTheme = "3Y-Diamond"
        ElseIf ksconf.Sections("Colors").Keys("User Name Shell Color").Value = userNameShellColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = hostNameShellColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = contKernelErrorColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = uncontKernelErrorColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Text Color").Value = neutralTextColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("License Color").Value = licenseColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Background Color").Value = backgroundColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Input Color").Value = inputColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Listed command in Help Color").Value = cmdListColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorLUnc.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorLUnc.ToString Then
            Wdbg("I", "Theme set to LinuxUncolored")
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
               ksconf.Sections("Colors").Keys("Definition of command in Help Color").Value = cmdDefColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = stageColorLcDef.ToString And
               ksconf.Sections("Colors").Keys("Error Text Color").Value = errorColorLcDef.ToString Then
            Wdbg("I", "Theme set to LinuxColoredDef")
            currentTheme = "LinuxColoredDef"
        Else
            Wdbg("I", "No match, set to Custom.")
            currentTheme = "Custom"
        End If
    End Sub

    ''' <summary>
    ''' Sets custom colors. It only works if colored shell is enabled.
    ''' </summary>
    ''' <param name="InputC">Input color</param>
    ''' <param name="LicenseC">License color</param>
    ''' <param name="ContKernelErrorC">Continuable kernel error color</param>
    ''' <param name="UncontKernelErrorC">Uncontinuable kernel error color</param>
    ''' <param name="HostNameC">Host name color</param>
    ''' <param name="UserNameC">User name color</param>
    ''' <param name="BackC">Background color</param>
    ''' <param name="NeutralTextC">Neutral text color</param>
    ''' <param name="CmdListC">Command list color</param>
    ''' <param name="CmdDefC">Command definition color</param>
    ''' <param name="StageC">Stage color</param>
    ''' <param name="ErrorC">Error color</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="EventsAndExceptions.ColorException"></exception>
    Public Function SetColors(InputC As ConsoleColors, LicenseC As ConsoleColors, ContKernelErrorC As ConsoleColors,
                              UncontKernelErrorC As ConsoleColors, HostNameC As ConsoleColors, UserNameC As ConsoleColors,
                              BackC As ConsoleColors, NeutralTextC As ConsoleColors, CmdListC As ConsoleColors,
                              CmdDefC As ConsoleColors, StageC As ConsoleColors, ErrorC As ConsoleColors) As Boolean
        If ColoredShell = True Then
            If InputC = ConsoleColors.def Then
                InputC = ConsoleColors.White
            End If
            If LicenseC = ConsoleColors.def Then
                LicenseC = ConsoleColors.White
            End If
            If ContKernelErrorC = ConsoleColors.def Then
                ContKernelErrorC = ConsoleColors.Yellow
            End If
            If UncontKernelErrorC = ConsoleColors.def Then
                UncontKernelErrorC = ConsoleColors.Red
            End If
            If HostNameC = ConsoleColors.def Then
                HostNameC = ConsoleColors.DarkGreen
            End If
            If UserNameC = ConsoleColors.def Then
                UserNameC = ConsoleColors.Green
            End If
            If BackC = ConsoleColors.def Then
                BackC = ConsoleColors.Black
                LoadBack()
            End If
            If NeutralTextC = ConsoleColors.def Then
                NeutralTextC = ConsoleColors.Gray
            End If
            If CmdListC = ConsoleColors.def Then
                CmdListC = ConsoleColors.DarkYellow
            End If
            If CmdDefC = ConsoleColors.def Then
                CmdDefC = ConsoleColors.DarkGray
            End If
            If StageC = ConsoleColors.def Then
                StageC = ConsoleColors.Green
            End If
            If ErrorC = ConsoleColors.def Then
                ErrorC = ConsoleColors.Red
            End If
            If [Enum].IsDefined(GetType(ConsoleColors), InputC) And [Enum].IsDefined(GetType(ConsoleColors), LicenseC) And [Enum].IsDefined(GetType(ConsoleColors), ContKernelErrorC) And
               [Enum].IsDefined(GetType(ConsoleColors), UncontKernelErrorC) And [Enum].IsDefined(GetType(ConsoleColors), HostNameC) And [Enum].IsDefined(GetType(ConsoleColors), UserNameC) And
               [Enum].IsDefined(GetType(ConsoleColors), BackC) And [Enum].IsDefined(GetType(ConsoleColors), NeutralTextC) And [Enum].IsDefined(GetType(ConsoleColors), CmdListC) And
               [Enum].IsDefined(GetType(ConsoleColors), CmdDefC) And [Enum].IsDefined(GetType(ConsoleColors), StageC) And [Enum].IsDefined(GetType(ConsoleColors), ErrorC) Then
                inputColor = CType([Enum].Parse(GetType(ConsoleColors), InputC), ConsoleColors)
                licenseColor = CType([Enum].Parse(GetType(ConsoleColors), LicenseC), ConsoleColors)
                contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), ContKernelErrorC), ConsoleColors)
                uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), UncontKernelErrorC), ConsoleColors)
                hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), HostNameC), ConsoleColors)
                userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), UserNameC), ConsoleColors)
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), BackC), ConsoleColors)
                neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), NeutralTextC), ConsoleColors)
                cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), CmdListC), ConsoleColors)
                cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), CmdDefC), ConsoleColors)
                stageColor = CType([Enum].Parse(GetType(ConsoleColors), StageC), ConsoleColors)
                errorColor = CType([Enum].Parse(GetType(ConsoleColors), ErrorC), ConsoleColors)
                LoadBack()
                MakePermanent()
                Return True
            Else
                Throw New EventsAndExceptions.ColorException(DoTranslation("One or more of the colors is invalid.", currentLang))
            End If
        Else
            Throw New InvalidOperationException(DoTranslation("Colors are not available. Turn on colored shell in the kernel config.", currentLang))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Sets input color (only use for shells)
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SetInputColor() As Boolean
        Dim esc As Char = GetEsc()
        If ColoredShell = True Then
            Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
            Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            Return True
        End If
        Return False
    End Function

End Module
