
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
    ''' Generates a new theme info from KS resources
    ''' </summary>
    ''' <param name="ThemeResourceName">Theme name (must match resource name)</param>
    Public Sub New(ThemeResourceName As String)
        Dim ThemeResourceJson As JToken = JToken.Parse(My.Resources.ResourceManager.GetString(ThemeResourceName))
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
    End Sub

    ''' <summary>
    ''' Generates a new theme info from file stream
    ''' </summary>
    ''' <param name="ThemeFileStream">Theme file stream reader</param>
    Public Sub New(ThemeFileStream As StreamReader)
        Dim ThemeResourceJson As JToken = JToken.Parse(ThemeFileStream.ReadToEnd)
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
    End Sub

End Class
