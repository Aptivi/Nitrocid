
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

Imports System.IO
Imports Newtonsoft.Json.Linq

Module ThemeStudioTools

    ''' <summary>
    ''' Selected input color for new theme
    ''' </summary>
    Friend SelectedInputColor As ConsoleColors = inputColor
    ''' <summary>
    ''' Selected license color for new theme
    ''' </summary>
    Friend SelectedLicenseColor As ConsoleColors = licenseColor
    ''' <summary>
    ''' Selected continuable kernel error color for new theme
    ''' </summary>
    Friend SelectedContKernelErrorColor As ConsoleColors = contKernelErrorColor
    ''' <summary>
    ''' Selected uncontinuable kernel error color for new theme
    ''' </summary>
    Friend SelectedUncontKernelErrorColor As ConsoleColors = uncontKernelErrorColor
    ''' <summary>
    ''' Selected host name shell color for new theme
    ''' </summary>
    Friend SelectedHostNameShellColor As ConsoleColors = hostNameShellColor
    ''' <summary>
    ''' Selected user name shell color for new theme
    ''' </summary>
    Friend SelectedUserNameShellColor As ConsoleColors = userNameShellColor
    ''' <summary>
    ''' Selected background color for new theme
    ''' </summary>
    Friend SelectedBackgroundColor As ConsoleColors = backgroundColor
    ''' <summary>
    ''' Selected neutral text color for new theme
    ''' </summary>
    Friend SelectedNeutralTextColor As ConsoleColors = NeutralTextColor
    ''' <summary>
    ''' Selected list entry color for new theme
    ''' </summary>
    Friend SelectedListEntryColor As ConsoleColors = ListEntryColor
    ''' <summary>
    ''' Selected list value color for new theme
    ''' </summary>
    Friend SelectedListValueColor As ConsoleColors = ListValueColor
    ''' <summary>
    ''' Selected stage color for new theme
    ''' </summary>
    Friend SelectedStageColor As ConsoleColors = stageColor
    ''' <summary>
    ''' Selected error color for new theme
    ''' </summary>
    Friend SelectedErrorColor As ConsoleColors = errorColor
    ''' <summary>
    ''' Selected warning color for new theme
    ''' </summary>
    Friend SelectedWarningColor As ConsoleColors = WarningColor
    ''' <summary>
    ''' Selected option color for new theme
    ''' </summary>
    Friend SelectedOptionColor As ConsoleColors = OptionColor

    ''' <summary>
    ''' Saves theme to current directory under "<paramref name="Theme"/>.json."
    ''' </summary>
    ''' <param name="Theme">Theme name</param>
    Sub SaveThemeToCurrentDirectory(ByVal Theme As String)
        Dim ThemeJson As JObject = GetThemeJson()
        File.WriteAllText(NeutralizePath(Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Saves theme to another directory under "<paramref name="Theme"/>.json."
    ''' </summary>
    ''' <param name="Theme">Theme name</param>
    ''' <param name="Path">Path name. Neutralized by <see cref="NeutralizePath(String)"/></param>
    Sub SaveThemeToAnotherDirectory(ByVal Theme As String, ByVal Path As String)
        Dim ThemeJson As JObject = GetThemeJson()
        File.WriteAllText(NeutralizePath(Path + "/" + Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Loads theme from resource and places it to the studio
    ''' </summary>
    ''' <param name="Theme">A theme name</param>
    Sub LoadThemeFromResource(ByVal Theme As String)
        'Populate theme info
        Dim ThemeInfo As ThemeInfo
        If Theme = "Default" Then
            ThemeInfo = New ThemeInfo("_Default")
        ElseIf Theme = "NFSHP-Cop" Then
            ThemeInfo = New ThemeInfo("NFSHP_Cop")
        ElseIf Theme = "NFSHP-Racer" Then
            ThemeInfo = New ThemeInfo("NFSHP_Racer")
        ElseIf Theme = "3Y-Diamond" Then
            ThemeInfo = New ThemeInfo("_3Y_Diamond")
        Else
            ThemeInfo = New ThemeInfo(Theme)
        End If

        'Place information to the studio
        SelectedInputColor = ThemeInfo.ThemeInputColor
        SelectedLicenseColor = ThemeInfo.ThemeLicenseColor
        SelectedContKernelErrorColor = ThemeInfo.ThemeContKernelErrorColor
        SelectedUncontKernelErrorColor = ThemeInfo.ThemeUncontKernelErrorColor
        SelectedHostNameShellColor = ThemeInfo.ThemeHostNameShellColor
        SelectedUserNameShellColor = ThemeInfo.ThemeUserNameShellColor
        SelectedBackgroundColor = ThemeInfo.ThemeBackgroundColor
        SelectedNeutralTextColor = ThemeInfo.ThemeNeutralTextColor
        SelectedListEntryColor = ThemeInfo.ThemeCmdListColor
        SelectedListValueColor = ThemeInfo.ThemeCmdDefColor
        SelectedStageColor = ThemeInfo.ThemeStageColor
        SelectedErrorColor = ThemeInfo.ThemeErrorColor
        SelectedWarningColor = ThemeInfo.ThemeWarningColor
        SelectedOptionColor = ThemeInfo.ThemeOptionColor
    End Sub

    ''' <summary>
    ''' Loads theme from resource and places it to the studio
    ''' </summary>
    ''' <param name="Theme">A theme name</param>
    Sub LoadThemeFromFile(ByVal Theme As String)
        'Populate theme info
        Dim ThemeInfo As New ThemeInfo(New StreamReader(NeutralizePath(Theme).ToString))

        'Place information to the studio
        SelectedInputColor = ThemeInfo.ThemeInputColor
        SelectedLicenseColor = ThemeInfo.ThemeLicenseColor
        SelectedContKernelErrorColor = ThemeInfo.ThemeContKernelErrorColor
        SelectedUncontKernelErrorColor = ThemeInfo.ThemeUncontKernelErrorColor
        SelectedHostNameShellColor = ThemeInfo.ThemeHostNameShellColor
        SelectedUserNameShellColor = ThemeInfo.ThemeUserNameShellColor
        SelectedBackgroundColor = ThemeInfo.ThemeBackgroundColor
        SelectedNeutralTextColor = ThemeInfo.ThemeNeutralTextColor
        SelectedListEntryColor = ThemeInfo.ThemeCmdListColor
        SelectedListValueColor = ThemeInfo.ThemeCmdDefColor
        SelectedStageColor = ThemeInfo.ThemeStageColor
        SelectedErrorColor = ThemeInfo.ThemeErrorColor
        SelectedWarningColor = ThemeInfo.ThemeWarningColor
        SelectedOptionColor = ThemeInfo.ThemeOptionColor
    End Sub

    ''' <summary>
    ''' Gets the full theme JSON object
    ''' </summary>
    ''' <returns>A JSON object</returns>
    Function GetThemeJson() As JObject
        Return New JObject(New JProperty("InputColor", SelectedInputColor.ToString),
                           New JProperty("LicenseColor", SelectedLicenseColor.ToString),
                           New JProperty("ContKernelErrorColor", SelectedContKernelErrorColor.ToString),
                           New JProperty("UncontKernelErrorColor", SelectedUncontKernelErrorColor.ToString),
                           New JProperty("HostNameShellColor", SelectedHostNameShellColor.ToString),
                           New JProperty("UserNameShellColor", SelectedUserNameShellColor.ToString),
                           New JProperty("BackgroundColor", SelectedBackgroundColor.ToString),
                           New JProperty("NeutralTextColor", SelectedNeutralTextColor.ToString),
                           New JProperty("CmdListColor", SelectedListEntryColor.ToString),
                           New JProperty("CmdDefColor", SelectedListValueColor.ToString),
                           New JProperty("StageColor", SelectedStageColor.ToString),
                           New JProperty("ErrorColor", SelectedErrorColor.ToString),
                           New JProperty("WarningColor", SelectedWarningColor.ToString),
                           New JProperty("OptionColor", SelectedOptionColor.ToString))
    End Function

End Module
