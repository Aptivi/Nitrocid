
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
    Public ReadOnly Property ThemeInputColor As ConsoleColors
    ''' <summary>
    ''' License color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeLicenseColor As ConsoleColors
    ''' <summary>
    ''' Continuable kernel error color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeContKernelErrorColor As ConsoleColors
    ''' <summary>
    ''' Uncontinuable kernel error color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeUncontKernelErrorColor As ConsoleColors
    ''' <summary>
    ''' Host name color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeHostNameShellColor As ConsoleColors
    ''' <summary>
    ''' User name color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeUserNameShellColor As ConsoleColors
    ''' <summary>
    ''' Background color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeBackgroundColor As ConsoleColors
    ''' <summary>
    ''' Neutral text color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeNeutralTextColor As ConsoleColors
    ''' <summary>
    ''' Command list color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeCmdListColor As ConsoleColors
    ''' <summary>
    ''' Command definition color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeCmdDefColor As ConsoleColors
    ''' <summary>
    ''' Stage color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeStageColor As ConsoleColors
    ''' <summary>
    ''' General error color set by theme
    ''' </summary>
    Public ReadOnly Property ThemeErrorColor As ConsoleColors

    ''' <summary>
    ''' Generates a new theme info from KS resources
    ''' </summary>
    ''' <param name="ThemeResourceName">Theme name (must match resource name)</param>
    Public Sub New(ByVal ThemeResourceName As String)
        Dim ThemeResourceJson As JToken = JToken.Parse(Evaluate("KS.My.Resources." + ThemeResourceName))
        ThemeInputColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("InputColor").ToString)
        ThemeLicenseColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("LicenseColor").ToString)
        ThemeContKernelErrorColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("ContKernelErrorColor").ToString)
        ThemeUncontKernelErrorColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("UncontKernelErrorColor").ToString)
        ThemeHostNameShellColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("HostNameShellColor").ToString)
        ThemeUserNameShellColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("UserNameShellColor").ToString)
        ThemeBackgroundColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("BackgroundColor").ToString)
        ThemeNeutralTextColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("NeutralTextColor").ToString)
        ThemeCmdListColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("CmdListColor").ToString)
        ThemeCmdDefColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("CmdDefColor").ToString)
        ThemeStageColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("StageColor").ToString)
        ThemeErrorColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("ErrorColor").ToString)
    End Sub

    ''' <summary>
    ''' Generates a new theme info from file stream
    ''' </summary>
    ''' <param name="ThemeFileStream">Theme file stream reader</param>
    Public Sub New(ByVal ThemeFileStream As StreamReader)
        Dim ThemeResourceJson As JToken = JToken.Parse(ThemeFileStream.ReadToEnd)
        ThemeInputColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("InputColor").ToString)
        ThemeLicenseColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("LicenseColor").ToString)
        ThemeContKernelErrorColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("ContKernelErrorColor").ToString)
        ThemeUncontKernelErrorColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("UncontKernelErrorColor").ToString)
        ThemeHostNameShellColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("HostNameShellColor").ToString)
        ThemeUserNameShellColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("UserNameShellColor").ToString)
        ThemeBackgroundColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("BackgroundColor").ToString)
        ThemeNeutralTextColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("NeutralTextColor").ToString)
        ThemeCmdListColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("CmdListColor").ToString)
        ThemeCmdDefColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("CmdDefColor").ToString)
        ThemeStageColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("StageColor").ToString)
        ThemeErrorColor = [Enum].Parse(GetType(ConsoleColors), ThemeResourceJson.SelectToken("ErrorColor").ToString)
    End Sub

End Class
