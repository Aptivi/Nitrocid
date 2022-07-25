
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports KS.ConsoleBase.Colors
Imports Newtonsoft.Json.Linq

Namespace ConsoleBase.Themes
    Public Class ThemeInfo

        ''' <summary>
        ''' Input color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeInputColor As Color
        ''' <summary>
        ''' License color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeLicenseColor As Color
        ''' <summary>
        ''' Continuable kernel error color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeContKernelErrorColor As Color
        ''' <summary>
        ''' Uncontinuable kernel error color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeUncontKernelErrorColor As Color
        ''' <summary>
        ''' Host name color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeHostNameShellColor As Color
        ''' <summary>
        ''' User name color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeUserNameShellColor As Color
        ''' <summary>
        ''' Background color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeBackgroundColor As Color
        ''' <summary>
        ''' Neutral text color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeNeutralTextColor As Color
        ''' <summary>
        ''' List entry color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeListEntryColor As Color
        ''' <summary>
        ''' List value color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeListValueColor As Color
        ''' <summary>
        ''' Stage color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeStageColor As Color
        ''' <summary>
        ''' General error color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeErrorColor As Color
        ''' <summary>
        ''' General warning color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeWarningColor As Color
        ''' <summary>
        ''' Option color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeOptionColor As Color
        ''' <summary>
        ''' Banner color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeBannerColor As Color
        ''' <summary>
        ''' Input color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeNotificationTitleColor As Color
        ''' <summary>
        ''' License color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeNotificationDescriptionColor As Color
        ''' <summary>
        ''' Continuable kernel error color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeNotificationProgressColor As Color
        ''' <summary>
        ''' Uncontinuable kernel error color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeNotificationFailureColor As Color
        ''' <summary>
        ''' Host name color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeQuestionColor As Color
        ''' <summary>
        ''' User name color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeSuccessColor As Color
        ''' <summary>
        ''' Background color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeUserDollarColor As Color
        ''' <summary>
        ''' Neutral text color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeTipColor As Color
        ''' <summary>
        ''' List entry color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeSeparatorTextColor As Color
        ''' <summary>
        ''' List value color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeSeparatorColor As Color
        ''' <summary>
        ''' Stage color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeListTitleColor As Color
        ''' <summary>
        ''' General error color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeDevelopmentWarningColor As Color
        ''' <summary>
        ''' General warning color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeStageTimeColor As Color
        ''' <summary>
        ''' Option color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeProgressColor As Color
        ''' <summary>
        ''' Banner color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeBackOptionColor As Color
        ''' <summary>
        ''' Low priority notification border color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeLowPriorityBorderColor As Color
        ''' <summary>
        ''' Medium priority notification border color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeMediumPriorityBorderColor As Color
        ''' <summary>
        ''' High priority notification border color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeHighPriorityBorderColor As Color
        ''' <summary>
        ''' Table separator color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeTableSeparatorColor As Color
        ''' <summary>
        ''' Table header color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeTableHeaderColor As Color
        ''' <summary>
        ''' Table value color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeTableValueColor As Color
        ''' <summary>
        ''' Selected option color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeSelectedOptionColor As Color
        ''' <summary>
        ''' Alternative option color set by theme
        ''' </summary>
        Public ReadOnly Property ThemeAlternativeOptionColor As Color

        ''' <summary>
        ''' Generates a new theme info from KS resources
        ''' </summary>
        ''' <param name="ThemeResourceName">Theme name (must match resource name)</param>
        Public Sub New(ThemeResourceName As String)
            Me.New(JToken.Parse(My.Resources.ResourceManager.GetString(ThemeResourceName)))
        End Sub

        ''' <summary>
        ''' Generates a new theme info from file stream
        ''' </summary>
        ''' <param name="ThemeFileStream">Theme file stream reader</param>
        Public Sub New(ThemeFileStream As StreamReader)
            Me.New(JToken.Parse(ThemeFileStream.ReadToEnd))
        End Sub

        ''' <summary>
        ''' Generates a new theme info from theme resource JSON
        ''' </summary>
        ''' <param name="ThemeResourceJson">Theme resource JSON</param>
        Protected Sub New(ThemeResourceJson As JToken)
            ThemeInputColor = New Color(ThemeResourceJson.SelectToken("InputColor").ToString)
            ThemeLicenseColor = New Color(ThemeResourceJson.SelectToken("LicenseColor").ToString)
            ThemeContKernelErrorColor = New Color(ThemeResourceJson.SelectToken("ContKernelErrorColor").ToString)
            ThemeUncontKernelErrorColor = New Color(ThemeResourceJson.SelectToken("UncontKernelErrorColor").ToString)
            ThemeHostNameShellColor = New Color(ThemeResourceJson.SelectToken("HostNameShellColor").ToString)
            ThemeUserNameShellColor = New Color(ThemeResourceJson.SelectToken("UserNameShellColor").ToString)
            ThemeBackgroundColor = New Color(ThemeResourceJson.SelectToken("BackgroundColor").ToString)
            ThemeNeutralTextColor = New Color(ThemeResourceJson.SelectToken("NeutralTextColor").ToString)
            ThemeListEntryColor = New Color(ThemeResourceJson.SelectToken("ListEntryColor").ToString)
            ThemeListValueColor = New Color(ThemeResourceJson.SelectToken("ListValueColor").ToString)
            ThemeStageColor = New Color(ThemeResourceJson.SelectToken("StageColor").ToString)
            ThemeErrorColor = New Color(ThemeResourceJson.SelectToken("ErrorColor").ToString)
            ThemeWarningColor = New Color(ThemeResourceJson.SelectToken("WarningColor").ToString)
            ThemeOptionColor = New Color(ThemeResourceJson.SelectToken("OptionColor").ToString)
            ThemeBannerColor = New Color(ThemeResourceJson.SelectToken("BannerColor").ToString)
            ThemeNotificationTitleColor = New Color(ThemeResourceJson.SelectToken("NotificationTitleColor").ToString)
            ThemeNotificationDescriptionColor = New Color(ThemeResourceJson.SelectToken("NotificationDescriptionColor").ToString)
            ThemeNotificationProgressColor = New Color(ThemeResourceJson.SelectToken("NotificationProgressColor").ToString)
            ThemeNotificationFailureColor = New Color(ThemeResourceJson.SelectToken("NotificationFailureColor").ToString)
            ThemeQuestionColor = New Color(ThemeResourceJson.SelectToken("QuestionColor").ToString)
            ThemeSuccessColor = New Color(ThemeResourceJson.SelectToken("SuccessColor").ToString)
            ThemeUserDollarColor = New Color(ThemeResourceJson.SelectToken("UserDollarColor").ToString)
            ThemeTipColor = New Color(ThemeResourceJson.SelectToken("TipColor").ToString)
            ThemeSeparatorTextColor = New Color(ThemeResourceJson.SelectToken("SeparatorTextColor").ToString)
            ThemeSeparatorColor = New Color(ThemeResourceJson.SelectToken("SeparatorColor").ToString)
            ThemeListTitleColor = New Color(ThemeResourceJson.SelectToken("ListTitleColor").ToString)
            ThemeDevelopmentWarningColor = New Color(ThemeResourceJson.SelectToken("DevelopmentWarningColor").ToString)
            ThemeStageTimeColor = New Color(ThemeResourceJson.SelectToken("StageTimeColor").ToString)
            ThemeProgressColor = New Color(ThemeResourceJson.SelectToken("ProgressColor").ToString)
            ThemeBackOptionColor = New Color(ThemeResourceJson.SelectToken("BackOptionColor").ToString)
            ThemeLowPriorityBorderColor = New Color(ThemeResourceJson.SelectToken("LowPriorityBorderColor").ToString)
            ThemeMediumPriorityBorderColor = New Color(ThemeResourceJson.SelectToken("MediumPriorityBorderColor").ToString)
            ThemeHighPriorityBorderColor = New Color(ThemeResourceJson.SelectToken("HighPriorityBorderColor").ToString)
            ThemeTableSeparatorColor = New Color(ThemeResourceJson.SelectToken("TableSeparatorColor").ToString)
            ThemeTableHeaderColor = New Color(ThemeResourceJson.SelectToken("TableHeaderColor").ToString)
            ThemeTableValueColor = New Color(ThemeResourceJson.SelectToken("TableValueColor").ToString)
            ThemeSelectedOptionColor = New Color(ThemeResourceJson.SelectToken("SelectedOptionColor").ToString)
            ThemeAlternativeOptionColor = New Color(ThemeResourceJson.SelectToken("AlternativeOptionColor").ToString)
        End Sub

    End Class
End Namespace