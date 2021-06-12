
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
    Friend SelectedInputColor As New Color(InputColor)
    ''' <summary>
    ''' Selected license color for new theme
    ''' </summary>
    Friend SelectedLicenseColor As New Color(LicenseColor)
    ''' <summary>
    ''' Selected continuable kernel error color for new theme
    ''' </summary>
    Friend SelectedContKernelErrorColor As New Color(ContKernelErrorColor)
    ''' <summary>
    ''' Selected uncontinuable kernel error color for new theme
    ''' </summary>
    Friend SelectedUncontKernelErrorColor As New Color(UncontKernelErrorColor)
    ''' <summary>
    ''' Selected host name shell color for new theme
    ''' </summary>
    Friend SelectedHostNameShellColor As New Color(HostNameShellColor)
    ''' <summary>
    ''' Selected user name shell color for new theme
    ''' </summary>
    Friend SelectedUserNameShellColor As New Color(UserNameShellColor)
    ''' <summary>
    ''' Selected background color for new theme
    ''' </summary>
    Friend SelectedBackgroundColor As New Color(BackgroundColor)
    ''' <summary>
    ''' Selected neutral text color for new theme
    ''' </summary>
    Friend SelectedNeutralTextColor As New Color(NeutralTextColor)
    ''' <summary>
    ''' Selected list entry color for new theme
    ''' </summary>
    Friend SelectedListEntryColor As New Color(ListEntryColor)
    ''' <summary>
    ''' Selected list value color for new theme
    ''' </summary>
    Friend SelectedListValueColor As New Color(ListValueColor)
    ''' <summary>
    ''' Selected stage color for new theme
    ''' </summary>
    Friend SelectedStageColor As New Color(StageColor)
    ''' <summary>
    ''' Selected error color for new theme
    ''' </summary>
    Friend SelectedErrorColor As New Color(ErrorColor)
    ''' <summary>
    ''' Selected warning color for new theme
    ''' </summary>
    Friend SelectedWarningColor As New Color(WarningColor)
    ''' <summary>
    ''' Selected option color for new theme
    ''' </summary>
    Friend SelectedOptionColor As New Color(OptionColor)
    ''' <summary>
    ''' Selected banner color for new theme
    ''' </summary>
    Friend SelectedBannerColor As New Color(BannerColor)

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
    ''' <param name="Path">Path name. Neutralized by <see cref="NeutralizePath(String, Boolean)"/></param>
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
        SelectedListEntryColor = ThemeInfo.ThemeListEntryColor
        SelectedListValueColor = ThemeInfo.ThemeListValueColor
        SelectedStageColor = ThemeInfo.ThemeStageColor
        SelectedErrorColor = ThemeInfo.ThemeErrorColor
        SelectedWarningColor = ThemeInfo.ThemeWarningColor
        SelectedOptionColor = ThemeInfo.ThemeOptionColor
        SelectedBannerColor = ThemeInfo.ThemeBannerColor
    End Sub

    ''' <summary>
    ''' Loads theme from resource and places it to the studio
    ''' </summary>
    ''' <param name="Theme">A theme name</param>
    Sub LoadThemeFromFile(ByVal Theme As String)
        'Populate theme info
        Dim ThemeInfo As New ThemeInfo(New StreamReader(NeutralizePath(Theme)))

        'Place information to the studio
        SelectedInputColor = ThemeInfo.ThemeInputColor
        SelectedLicenseColor = ThemeInfo.ThemeLicenseColor
        SelectedContKernelErrorColor = ThemeInfo.ThemeContKernelErrorColor
        SelectedUncontKernelErrorColor = ThemeInfo.ThemeUncontKernelErrorColor
        SelectedHostNameShellColor = ThemeInfo.ThemeHostNameShellColor
        SelectedUserNameShellColor = ThemeInfo.ThemeUserNameShellColor
        SelectedBackgroundColor = ThemeInfo.ThemeBackgroundColor
        SelectedNeutralTextColor = ThemeInfo.ThemeNeutralTextColor
        SelectedListEntryColor = ThemeInfo.ThemeListEntryColor
        SelectedListValueColor = ThemeInfo.ThemeListValueColor
        SelectedStageColor = ThemeInfo.ThemeStageColor
        SelectedErrorColor = ThemeInfo.ThemeErrorColor
        SelectedWarningColor = ThemeInfo.ThemeWarningColor
        SelectedOptionColor = ThemeInfo.ThemeOptionColor
        SelectedBannerColor = ThemeInfo.ThemeBannerColor
    End Sub

    ''' <summary>
    ''' Gets the full theme JSON object
    ''' </summary>
    ''' <returns>A JSON object</returns>
    Function GetThemeJson() As JObject
        Return New JObject(New JProperty("InputColor", SelectedInputColor.PlainSequence),
                           New JProperty("LicenseColor", SelectedLicenseColor.PlainSequence),
                           New JProperty("ContKernelErrorColor", SelectedContKernelErrorColor.PlainSequence),
                           New JProperty("UncontKernelErrorColor", SelectedUncontKernelErrorColor.PlainSequence),
                           New JProperty("HostNameShellColor", SelectedHostNameShellColor.PlainSequence),
                           New JProperty("UserNameShellColor", SelectedUserNameShellColor.PlainSequence),
                           New JProperty("BackgroundColor", SelectedBackgroundColor.PlainSequence),
                           New JProperty("NeutralTextColor", SelectedNeutralTextColor.PlainSequence),
                           New JProperty("ListEntryColor", SelectedListEntryColor.PlainSequence),
                           New JProperty("ListValueColor", SelectedListValueColor.PlainSequence),
                           New JProperty("StageColor", SelectedStageColor.PlainSequence),
                           New JProperty("ErrorColor", SelectedErrorColor.PlainSequence),
                           New JProperty("WarningColor", SelectedWarningColor.PlainSequence),
                           New JProperty("OptionColor", SelectedOptionColor.PlainSequence),
                           New JProperty("BannerColor", SelectedBannerColor.PlainSequence))
    End Function

    ''' <summary>
    ''' Prepares the preview
    ''' </summary>
    Sub PreparePreview()
        Console.Clear()
        W(DoTranslation("Here's how your theme will look like:") + vbNewLine, True, ColTypes.Neutral)

        'Print every possibility of color types
        'Input color
        W("*) " + DoTranslation("Input color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedInputColor)

        'License color
        W("*) " + DoTranslation("License color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedLicenseColor)

        'Continuable kernel error color
        W("*) " + DoTranslation("Continuable kernel error color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedContKernelErrorColor)

        'Uncontinuable kernel error color
        W("*) " + DoTranslation("Uncontinuable kernel error color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedUncontKernelErrorColor)

        'Host name color
        W("*) " + DoTranslation("Host name color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedHostNameShellColor)

        'User name color
        W("*) " + DoTranslation("User name color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedUserNameShellColor)

        'Background color
        W("*) " + DoTranslation("Background color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedBackgroundColor)

        'Neutral text color
        W("*) " + DoTranslation("Neutral text color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedNeutralTextColor)

        'List entry color
        W("*) " + DoTranslation("List entry color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedListEntryColor)

        'List value color
        W("*) " + DoTranslation("List value color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedListValueColor)

        'Stage color
        W("*) " + DoTranslation("Stage color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedStageColor)

        'Error color
        W("*) " + DoTranslation("Error color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedErrorColor)

        'Warning color
        W("*) " + DoTranslation("Warning color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedWarningColor)

        'Option color
        W("*) " + DoTranslation("Option color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedOptionColor)

        'Banner color
        W("*) " + DoTranslation("Banner color") + ": ", False, ColTypes.Option)
        WriteC("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedBannerColor)

        'Pause until a key is pressed
        W(vbNewLine + DoTranslation("Press any key to go back."), True, ColTypes.Neutral)
        Console.ReadKey()
    End Sub

End Module
