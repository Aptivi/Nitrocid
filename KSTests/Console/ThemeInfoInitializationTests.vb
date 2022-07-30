
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports Extensification.StringExts
Imports KS.ConsoleBase.Themes

<TestFixture> Public Class ThemeInfoInitializationTests

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from KS resources
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeThemeInfoFromResources()
        'Create instance
        Dim ThemeInfoInstance As New ThemeInfo("Hacker")

        'Check for null
        ThemeInfoInstance.ThemeBackgroundColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListValueColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListEntryColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeContKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHostNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeInputColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLicenseColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNeutralTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUncontKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationDescriptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationFailureColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeQuestionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSuccessColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserDollarColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTipColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeDevelopmentWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageTimeColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeBackOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLowPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeMediumPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHighPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTableSeparatorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTableHeaderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTableValueColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSelectedOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeAlternativeOptionColor.ShouldNotBeNull
    End Sub

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from all KS resources
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeThemeInfoFromAllResources()
        For Each ResourceName As String In Themes.Keys

            'Special naming cases
            Dim ThemeName As String = ResourceName.ReplaceAll({"-", " "}, "_")
            Select Case ResourceName
                Case "Default"
                    ThemeName = "_Default"
                Case "3Y-Diamond"
                    ThemeName = "_3Y_Diamond"
            End Select

            'Create instance
            Dim ThemeInfoInstance As New ThemeInfo(ThemeName)

            'Check for null
            ThemeInfoInstance.ThemeBackgroundColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeListValueColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeListEntryColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeContKernelErrorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeErrorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeHostNameShellColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeInputColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeLicenseColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNeutralTextColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeOptionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeStageColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeUncontKernelErrorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeUserNameShellColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeWarningColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationTitleColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationDescriptionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationProgressColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeNotificationFailureColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeQuestionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeSuccessColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeUserDollarColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeTipColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeSeparatorTextColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeSeparatorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeListTitleColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeDevelopmentWarningColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeStageTimeColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeProgressColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeBackOptionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeLowPriorityBorderColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeMediumPriorityBorderColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeHighPriorityBorderColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeTableSeparatorColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeTableHeaderColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeTableValueColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeSelectedOptionColor.ShouldNotBeNull
            ThemeInfoInstance.ThemeAlternativeOptionColor.ShouldNotBeNull
        Next
    End Sub

    ''' <summary>
    ''' Tests initializing an instance of ThemeInfo from file
    ''' </summary>
    <Test, Description("Initialization")> Public Sub TestInitializeThemeInfoFromFile()
        'Create instance
        Dim SourcePath As String = Path.GetFullPath("TestData/Hacker.json")
        Dim ThemeInfoStream As New StreamReader(SourcePath)
        Dim ThemeInfoInstance As New ThemeInfo(ThemeInfoStream)
        ThemeInfoStream.Close()

        'Check for null
        ThemeInfoInstance.ThemeBackgroundColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListValueColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListEntryColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeContKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHostNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeInputColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLicenseColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNeutralTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUncontKernelErrorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserNameShellColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationDescriptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeNotificationFailureColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeQuestionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSuccessColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeUserDollarColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTipColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorTextColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSeparatorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeListTitleColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeDevelopmentWarningColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeStageTimeColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeProgressColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeBackOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeLowPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeMediumPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeHighPriorityBorderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTableSeparatorColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTableHeaderColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeTableValueColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeSelectedOptionColor.ShouldNotBeNull
        ThemeInfoInstance.ThemeAlternativeOptionColor.ShouldNotBeNull
    End Sub

End Class