
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
    Friend _SelectedOptionColor As New Color(OptionColor)
    ''' <summary>
    ''' Selected banner color for new theme
    ''' </summary>
    Friend SelectedBannerColor As New Color(BannerColor)
    ''' <summary>
    ''' Selected error color for new theme
    ''' </summary>
    Friend SelectedNotificationTitleColor As New Color(ErrorColor)
    ''' <summary>
    ''' Selected warning color for new theme
    ''' </summary>
    Friend SelectedNotificationDescriptionColor As New Color(WarningColor)
    ''' <summary>
    ''' Selected option color for new theme
    ''' </summary>
    Friend SelectedNotificationProgressColor As New Color(OptionColor)
    ''' <summary>
    ''' Selected banner color for new theme
    ''' </summary>
    Friend SelectedNotificationFailureColor As New Color(BannerColor)
    ''' <summary>
    ''' Selected error color for new theme
    ''' </summary>
    Friend SelectedQuestionColor As New Color(ErrorColor)
    ''' <summary>
    ''' Selected warning color for new theme
    ''' </summary>
    Friend SelectedSuccessColor As New Color(WarningColor)
    ''' <summary>
    ''' Selected option color for new theme
    ''' </summary>
    Friend SelectedUserDollarColor As New Color(OptionColor)
    ''' <summary>
    ''' Selected banner color for new theme
    ''' </summary>
    Friend SelectedTipColor As New Color(BannerColor)
    ''' <summary>
    ''' Selected error color for new theme
    ''' </summary>
    Friend SelectedSeparatorTextColor As New Color(ErrorColor)
    ''' <summary>
    ''' Selected warning color for new theme
    ''' </summary>
    Friend SelectedSeparatorColor As New Color(WarningColor)
    ''' <summary>
    ''' Selected option color for new theme
    ''' </summary>
    Friend SelectedListTitleColor As New Color(OptionColor)
    ''' <summary>
    ''' Selected banner color for new theme
    ''' </summary>
    Friend SelectedDevelopmentWarningColor As New Color(BannerColor)
    ''' <summary>
    ''' Selected warning color for new theme
    ''' </summary>
    Friend SelectedStageTimeColor As New Color(WarningColor)
    ''' <summary>
    ''' Selected option color for new theme
    ''' </summary>
    Friend SelectedProgressColor As New Color(OptionColor)
    ''' <summary>
    ''' Selected banner color for new theme
    ''' </summary>
    Friend SelectedBackOptionColor As New Color(BannerColor)
    ''' <summary>
    ''' Selected low priority notification border color for new theme
    ''' </summary>
    Friend SelectedLowPriorityBorderColor As New Color(LowPriorityBorderColor)
    ''' <summary>
    ''' Selected medium priority notification border color for new theme
    ''' </summary>
    Friend SelectedMediumPriorityBorderColor As New Color(MediumPriorityBorderColor)
    ''' <summary>
    ''' Selected high priority notification border color for new theme
    ''' </summary>
    Friend SelectedHighPriorityBorderColor As New Color(HighPriorityBorderColor)
    ''' <summary>
    ''' Selected Table separator color for new theme
    ''' </summary>
    Friend SelectedTableSeparatorColor As New Color(TableSeparatorColor)
    ''' <summary>
    ''' Selected Table header color for new theme
    ''' </summary>
    Friend SelectedTableHeaderColor As New Color(TableHeaderColor)
    ''' <summary>
    ''' Selected Table value color for new theme
    ''' </summary>
    Friend SelectedTableValueColor As New Color(TableValueColor)
    ''' <summary>
    ''' Selected selected option color for new theme
    ''' </summary>
    Friend SelectedSelectedOptionColor As New Color(SelectedOptionColor)

    ''' <summary>
    ''' Saves theme to current directory under "<paramref name="Theme"/>.json."
    ''' </summary>
    ''' <param name="Theme">Theme name</param>
    Sub SaveThemeToCurrentDirectory(Theme As String)
        Dim ThemeJson As JObject = GetThemeJson()
        File.WriteAllText(NeutralizePath(Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Saves theme to another directory under "<paramref name="Theme"/>.json."
    ''' </summary>
    ''' <param name="Theme">Theme name</param>
    ''' <param name="Path">Path name. Neutralized by <see cref="NeutralizePath(String, Boolean)"/></param>
    Sub SaveThemeToAnotherDirectory(Theme As String, Path As String)
        Dim ThemeJson As JObject = GetThemeJson()
        File.WriteAllText(NeutralizePath(Path + "/" + Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Loads theme from resource and places it to the studio
    ''' </summary>
    ''' <param name="Theme">A theme name</param>
    Sub LoadThemeFromResource(Theme As String)
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
        _SelectedOptionColor = ThemeInfo.ThemeOptionColor
        SelectedBannerColor = ThemeInfo.ThemeBannerColor
        SelectedNotificationTitleColor = ThemeInfo.ThemeNotificationTitleColor
        SelectedNotificationDescriptionColor = ThemeInfo.ThemeNotificationDescriptionColor
        SelectedNotificationProgressColor = ThemeInfo.ThemeNotificationProgressColor
        SelectedNotificationFailureColor = ThemeInfo.ThemeNotificationFailureColor
        SelectedQuestionColor = ThemeInfo.ThemeQuestionColor
        SelectedSuccessColor = ThemeInfo.ThemeSuccessColor
        SelectedUserDollarColor = ThemeInfo.ThemeUserDollarColor
        SelectedTipColor = ThemeInfo.ThemeTipColor
        SelectedSeparatorTextColor = ThemeInfo.ThemeSeparatorTextColor
        SelectedSeparatorColor = ThemeInfo.ThemeSeparatorColor
        SelectedListTitleColor = ThemeInfo.ThemeListTitleColor
        SelectedDevelopmentWarningColor = ThemeInfo.ThemeDevelopmentWarningColor
        SelectedStageTimeColor = ThemeInfo.ThemeStageTimeColor
        SelectedProgressColor = ThemeInfo.ThemeProgressColor
        SelectedBackOptionColor = ThemeInfo.ThemeBackOptionColor
        SelectedLowPriorityBorderColor = ThemeInfo.ThemeLowPriorityBorderColor
        SelectedMediumPriorityBorderColor = ThemeInfo.ThemeMediumPriorityBorderColor
        SelectedHighPriorityBorderColor = ThemeInfo.ThemeHighPriorityBorderColor
        SelectedTableSeparatorColor = ThemeInfo.ThemeTableSeparatorColor
        SelectedTableHeaderColor = ThemeInfo.ThemeTableHeaderColor
        SelectedTableValueColor = ThemeInfo.ThemeTableValueColor
        SelectedSelectedOptionColor = ThemeInfo.ThemeSelectedOptionColor
    End Sub

    ''' <summary>
    ''' Loads theme from resource and places it to the studio
    ''' </summary>
    ''' <param name="Theme">A theme name</param>
    Sub LoadThemeFromFile(Theme As String)
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
        _SelectedOptionColor = ThemeInfo.ThemeOptionColor
        SelectedBannerColor = ThemeInfo.ThemeBannerColor
        SelectedNotificationTitleColor = ThemeInfo.ThemeNotificationTitleColor
        SelectedNotificationDescriptionColor = ThemeInfo.ThemeNotificationDescriptionColor
        SelectedNotificationProgressColor = ThemeInfo.ThemeNotificationProgressColor
        SelectedNotificationFailureColor = ThemeInfo.ThemeNotificationFailureColor
        SelectedQuestionColor = ThemeInfo.ThemeQuestionColor
        SelectedSuccessColor = ThemeInfo.ThemeSuccessColor
        SelectedUserDollarColor = ThemeInfo.ThemeUserDollarColor
        SelectedTipColor = ThemeInfo.ThemeTipColor
        SelectedSeparatorTextColor = ThemeInfo.ThemeSeparatorTextColor
        SelectedSeparatorColor = ThemeInfo.ThemeSeparatorColor
        SelectedListTitleColor = ThemeInfo.ThemeListTitleColor
        SelectedDevelopmentWarningColor = ThemeInfo.ThemeDevelopmentWarningColor
        SelectedStageTimeColor = ThemeInfo.ThemeStageTimeColor
        SelectedProgressColor = ThemeInfo.ThemeProgressColor
        SelectedBackOptionColor = ThemeInfo.ThemeBackOptionColor
        SelectedLowPriorityBorderColor = ThemeInfo.ThemeLowPriorityBorderColor
        SelectedMediumPriorityBorderColor = ThemeInfo.ThemeMediumPriorityBorderColor
        SelectedHighPriorityBorderColor = ThemeInfo.ThemeHighPriorityBorderColor
        SelectedTableSeparatorColor = ThemeInfo.ThemeTableSeparatorColor
        SelectedTableHeaderColor = ThemeInfo.ThemeTableHeaderColor
        SelectedTableValueColor = ThemeInfo.ThemeTableValueColor
        SelectedSelectedOptionColor = ThemeInfo.ThemeSelectedOptionColor
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
                           New JProperty("OptionColor", _SelectedOptionColor.PlainSequence),
                           New JProperty("BannerColor", SelectedBannerColor.PlainSequence),
                           New JProperty("NotificationTitleColor", SelectedNotificationTitleColor.PlainSequence),
                           New JProperty("NotificationDescriptionColor", SelectedNotificationDescriptionColor.PlainSequence),
                           New JProperty("NotificationProgressColor", SelectedNotificationProgressColor.PlainSequence),
                           New JProperty("NotificationFailureColor", SelectedNotificationFailureColor.PlainSequence),
                           New JProperty("QuestionColor", SelectedQuestionColor.PlainSequence),
                           New JProperty("SuccessColor", SelectedSuccessColor.PlainSequence),
                           New JProperty("UserDollarColor", SelectedUserDollarColor.PlainSequence),
                           New JProperty("TipColor", SelectedTipColor.PlainSequence),
                           New JProperty("SeparatorTextColor", SelectedSeparatorTextColor.PlainSequence),
                           New JProperty("SeparatorColor", SelectedSeparatorColor.PlainSequence),
                           New JProperty("ListTitleColor", SelectedListTitleColor.PlainSequence),
                           New JProperty("DevelopmentWarningColor", SelectedDevelopmentWarningColor.PlainSequence),
                           New JProperty("StageTimeColor", SelectedStageTimeColor.PlainSequence),
                           New JProperty("ProgressColor", SelectedProgressColor.PlainSequence),
                           New JProperty("BackOptionColor", SelectedBackOptionColor.PlainSequence),
                           New JProperty("LowPriorityBorderColor", SelectedLowPriorityBorderColor.PlainSequence),
                           New JProperty("MediumPriorityBorderColor", SelectedMediumPriorityBorderColor.PlainSequence),
                           New JProperty("HighPriorityBorderColor", SelectedHighPriorityBorderColor.PlainSequence),
                           New JProperty("TableSeparatorColor", SelectedTableSeparatorColor.PlainSequence),
                           New JProperty("TableHeaderColor", SelectedTableHeaderColor.PlainSequence),
                           New JProperty("TableValueColor", SelectedTableValueColor.PlainSequence),
                           New JProperty("SelectedOptionColor", SelectedSelectedOptionColor.PlainSequence))
    End Function

    ''' <summary>
    ''' Prepares the preview
    ''' </summary>
    Sub PreparePreview()
        Console.Clear()
        Write(DoTranslation("Here's how your theme will look like:") + vbNewLine, True, ColTypes.Neutral)

        'Print every possibility of color types
        'Input color
        Write("*) " + DoTranslation("Input color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedInputColor)

        'License color
        Write("*) " + DoTranslation("License color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedLicenseColor)

        'Continuable kernel error color
        Write("*) " + DoTranslation("Continuable kernel error color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedContKernelErrorColor)

        'Uncontinuable kernel error color
        Write("*) " + DoTranslation("Uncontinuable kernel error color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedUncontKernelErrorColor)

        'Host name color
        Write("*) " + DoTranslation("Host name color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedHostNameShellColor)

        'User name color
        Write("*) " + DoTranslation("User name color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedUserNameShellColor)

        'Background color
        Write("*) " + DoTranslation("Background color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedBackgroundColor)

        'Neutral text color
        Write("*) " + DoTranslation("Neutral text color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedNeutralTextColor)

        'List entry color
        Write("*) " + DoTranslation("List entry color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedListEntryColor)

        'List value color
        Write("*) " + DoTranslation("List value color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedListValueColor)

        'Stage color
        Write("*) " + DoTranslation("Stage color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedStageColor)

        'Error color
        Write("*) " + DoTranslation("Error color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedErrorColor)

        'Warning color
        Write("*) " + DoTranslation("Warning color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedWarningColor)

        'Option color
        Write("*) " + DoTranslation("Option color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, _SelectedOptionColor)

        'Banner color
        Write("*) " + DoTranslation("Banner color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedBannerColor)

        'Notification title color
        Write("*) " + DoTranslation("Notification title color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedNotificationTitleColor)

        'Notification description color
        Write("*) " + DoTranslation("Notification description color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedNotificationDescriptionColor)

        'Notification progress color
        Write("*) " + DoTranslation("Notification progress color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedNotificationProgressColor)

        'Notification failure color
        Write("*) " + DoTranslation("Notification failure color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedNotificationFailureColor)

        'Question color
        Write("*) " + DoTranslation("Question color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedQuestionColor)

        'Success color
        Write("*) " + DoTranslation("Success color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedSuccessColor)

        'User dollar color
        Write("*) " + DoTranslation("User dollar color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedUserDollarColor)

        'Tip color
        Write("*) " + DoTranslation("Tip color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedTipColor)

        'Separator text color
        Write("*) " + DoTranslation("Separator text color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedSeparatorTextColor)

        'Separator color
        Write("*) " + DoTranslation("Separator color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedSeparatorColor)

        'List title color
        Write("*) " + DoTranslation("List title color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedListTitleColor)

        'Development warning color
        Write("*) " + DoTranslation("Development warning color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedDevelopmentWarningColor)

        'Stage time color
        Write("*) " + DoTranslation("Stage time color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedStageTimeColor)

        'Progress color
        Write("*) " + DoTranslation("Progress color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedProgressColor)

        'Back option color
        Write("*) " + DoTranslation("Back option color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedBackOptionColor)

        'Low priority border color
        Write("*) " + DoTranslation("Low priority border color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedLowPriorityBorderColor)

        'Medium priority border color
        Write("*) " + DoTranslation("Medium priority border color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedMediumPriorityBorderColor)

        'High priority border color
        Write("*) " + DoTranslation("High priority border color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedHighPriorityBorderColor)

        'Table separator color
        Write("*) " + DoTranslation("Table separator color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedTableSeparatorColor)

        'Table header color
        Write("*) " + DoTranslation("Table header color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedTableHeaderColor)

        'Table value color
        Write("*) " + DoTranslation("Table value color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedTableValueColor)

        'Selected option color
        Write("*) " + DoTranslation("Selected option color") + ": ", False, ColTypes.Option)
        Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, SelectedTableValueColor)

        'Pause until a key is pressed
        Write(vbNewLine + DoTranslation("Press any key to go back."), True, ColTypes.Neutral)
        Console.ReadKey()
    End Sub

End Module
