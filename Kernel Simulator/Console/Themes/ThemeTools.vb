
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

Public Module ThemeTools

    ''' <summary>
    ''' All the available built-in themes
    ''' </summary>
    Public ReadOnly Themes As New Dictionary(Of String, ThemeInfo) From {{"Default", New ThemeInfo("_Default")},
                                                                         {"3Y-Diamond", New ThemeInfo("_3Y_Diamond")},
                                                                         {"AyuDark", New ThemeInfo("AyuDark")},
                                                                         {"AyuLight", New ThemeInfo("AyuLight")},
                                                                         {"AyuMirage", New ThemeInfo("AyuMirage")},
                                                                         {"BlackOnWhite", New ThemeInfo("BlackOnWhite")},
                                                                         {"BedOS", New ThemeInfo("BedOS")},
                                                                         {"Bluespire", New ThemeInfo("Bluespire")},
                                                                         {"BrandingBlue", New ThemeInfo("BrandingBlue")},
                                                                         {"BrandingPurple", New ThemeInfo("BrandingPurple")},
                                                                         {"BreezeDark", New ThemeInfo("BreezeDark")},
                                                                         {"Breeze", New ThemeInfo("Breeze")},
                                                                         {"Debian", New ThemeInfo("Debian")},
                                                                         {"GTASA", New ThemeInfo("GTASA")},
                                                                         {"GrayOnYellow", New ThemeInfo("GrayOnYellow")},
                                                                         {"Hacker", New ThemeInfo("Hacker")},
                                                                         {"LinuxColoredDef", New ThemeInfo("LinuxColoredDef")},
                                                                         {"LinuxUncolored", New ThemeInfo("LinuxUncolored")},
                                                                         {"Metallic", New ThemeInfo("Metallic")},
                                                                         {"NeonBreeze", New ThemeInfo("NeonBreeze")},
                                                                         {"NFSHP-Cop", New ThemeInfo("NFSHP_Cop")},
                                                                         {"NFSHP-Racer", New ThemeInfo("NFSHP_Racer")},
                                                                         {"RedConsole", New ThemeInfo("RedConsole")},
                                                                         {"SolarizedDark", New ThemeInfo("SolarizedDark")},
                                                                         {"SolarizedLight", New ThemeInfo("SolarizedLight")},
                                                                         {"TealerOS", New ThemeInfo("TealerOS")},
                                                                         {"TrafficLight", New ThemeInfo("TrafficLight")},
                                                                         {"Ubuntu", New ThemeInfo("Ubuntu")},
                                                                         {"YellowBG", New ThemeInfo("YellowBG")},
                                                                         {"YellowFG", New ThemeInfo("YellowFG")},
                                                                         {"Windows11", New ThemeInfo("Windows11")},
                                                                         {"Windows11Light", New ThemeInfo("Windows11Light")},
                                                                         {"Windows95", New ThemeInfo("Windows95")},
                                                                         {"Wood", New ThemeInfo("Wood")}}

    ''' <summary>
    ''' Sets system colors according to the programmed templates
    ''' </summary>
    ''' <param name="theme">A specified theme</param>
    Public Sub ApplyThemeFromResources(theme As String)
        Wdbg(DebugLevel.I, "Theme: {0}", theme)
        If Themes.ContainsKey(theme) Then
            Wdbg(DebugLevel.I, "Theme found.")

            'Populate theme info
            Dim ThemeInfo As ThemeInfo
            If theme = "Default" Then
                ResetColors()
            ElseIf theme = "NFSHP-Cop" Then
                ThemeInfo = New ThemeInfo("NFSHP_Cop")
            ElseIf theme = "NFSHP-Racer" Then
                ThemeInfo = New ThemeInfo("NFSHP_Racer")
            ElseIf theme = "3Y-Diamond" Then
                ThemeInfo = New ThemeInfo("_3Y_Diamond")
            Else
                ThemeInfo = New ThemeInfo(theme)
            End If

            If Not theme = "Default" Then
#Disable Warning BC42104
                'Set colors as appropriate
                SetColorsTheme(ThemeInfo)
#Enable Warning BC42104
            End If

            'Raise event
            EventManager.RaiseThemeSet(theme)
        Else
            W(DoTranslation("Invalid color template {0}"), True, ColTypes.Error, theme)
            Wdbg(DebugLevel.E, "Theme not found.")

            'Raise event
            EventManager.RaiseThemeSetError(theme, ThemeSetErrorReasons.NotFound)
        End If
    End Sub

    ''' <summary>
    ''' Sets system colors according to the template file
    ''' </summary>
    ''' <param name="ThemeFile">Theme file</param>
    Public Sub ApplyThemeFromFile(ThemeFile As String)
        Try
            Wdbg(DebugLevel.I, "Theme file name: {0}", ThemeFile)
            ThemeFile = NeutralizePath(ThemeFile, True)
            Wdbg(DebugLevel.I, "Theme file path: {0}", ThemeFile)

            'Populate theme info
            Dim ThemeInfo As New ThemeInfo(New StreamReader(ThemeFile))

            If Not ThemeFile = "Default" Then
                'Set colors as appropriate
                SetColorsTheme(ThemeInfo)
            End If

            'Raise event
            EventManager.RaiseThemeSet(ThemeFile)
        Catch ex As Exception
            W(DoTranslation("Invalid color template {0}"), True, ColTypes.Error, ThemeFile)
            Wdbg(DebugLevel.E, "Theme not found.")

            'Raise event
            EventManager.RaiseThemeSetError(ThemeFile, ThemeSetErrorReasons.NotFound)
        End Try
    End Sub

    ''' <summary>
    ''' Sets custom colors. It only works if colored shell is enabled.
    ''' </summary>
    ''' <param name="ThemeInfo">Theme information</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Function SetColorsTheme(ThemeInfo As ThemeInfo) As Boolean
        If ThemeInfo Is Nothing Then Throw New ArgumentNullException(NameOf(ThemeInfo))

        'Set the colors
        If ColoredShell = True Then
            Try
                InputColor = ThemeInfo.ThemeInputColor.PlainSequence
                LicenseColor = ThemeInfo.ThemeLicenseColor.PlainSequence
                ContKernelErrorColor = ThemeInfo.ThemeContKernelErrorColor.PlainSequence
                UncontKernelErrorColor = ThemeInfo.ThemeUncontKernelErrorColor.PlainSequence
                HostNameShellColor = ThemeInfo.ThemeHostNameShellColor.PlainSequence
                UserNameShellColor = ThemeInfo.ThemeUserNameShellColor.PlainSequence
                BackgroundColor = ThemeInfo.ThemeBackgroundColor.PlainSequence
                NeutralTextColor = ThemeInfo.ThemeNeutralTextColor.PlainSequence
                ListEntryColor = ThemeInfo.ThemeListEntryColor.PlainSequence
                ListValueColor = ThemeInfo.ThemeListValueColor.PlainSequence
                StageColor = ThemeInfo.ThemeStageColor.PlainSequence
                ErrorColor = ThemeInfo.ThemeErrorColor.PlainSequence
                WarningColor = ThemeInfo.ThemeWarningColor.PlainSequence
                OptionColor = ThemeInfo.ThemeOptionColor.PlainSequence
                BannerColor = ThemeInfo.ThemeBannerColor.PlainSequence
                NotificationTitleColor = ThemeInfo.ThemeNotificationTitleColor.PlainSequence
                NotificationDescriptionColor = ThemeInfo.ThemeNotificationDescriptionColor.PlainSequence
                NotificationProgressColor = ThemeInfo.ThemeNotificationProgressColor.PlainSequence
                NotificationFailureColor = ThemeInfo.ThemeNotificationFailureColor.PlainSequence
                QuestionColor = ThemeInfo.ThemeQuestionColor.PlainSequence
                SuccessColor = ThemeInfo.ThemeSuccessColor.PlainSequence
                UserDollarColor = ThemeInfo.ThemeUserDollarColor.PlainSequence
                TipColor = ThemeInfo.ThemeTipColor.PlainSequence
                SeparatorTextColor = ThemeInfo.ThemeSeparatorTextColor.PlainSequence
                SeparatorColor = ThemeInfo.ThemeSeparatorColor.PlainSequence
                ListTitleColor = ThemeInfo.ThemeListTitleColor.PlainSequence
                DevelopmentWarningColor = ThemeInfo.ThemeDevelopmentWarningColor.PlainSequence
                StageTimeColor = ThemeInfo.ThemeStageTimeColor.PlainSequence
                ProgressColor = ThemeInfo.ThemeProgressColor.PlainSequence
                BackOptionColor = ThemeInfo.ThemeBackOptionColor.PlainSequence
                LowPriorityBorderColor = ThemeInfo.ThemeLowPriorityBorderColor.PlainSequence
                MediumPriorityBorderColor = ThemeInfo.ThemeMediumPriorityBorderColor.PlainSequence
                HighPriorityBorderColor = ThemeInfo.ThemeHighPriorityBorderColor.PlainSequence
                TableSeparatorColor = ThemeInfo.ThemeTableSeparatorColor.PlainSequence
                TableHeaderColor = ThemeInfo.ThemeTableHeaderColor.PlainSequence
                TableValueColor = ThemeInfo.ThemeTableValueColor.PlainSequence
                LoadBack()
                MakePermanent()

                'Raise event
                EventManager.RaiseColorSet()
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                EventManager.RaiseColorSetError(ColorSetErrorReasons.InvalidColors)
                Throw New Exceptions.ColorException(DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message)
            End Try
        Else
            EventManager.RaiseColorSetError(ColorSetErrorReasons.NoColors)
            Throw New InvalidOperationException(DoTranslation("Colors are not available. Turn on colored shell in the kernel config."))
        End If
        Return False
    End Function

End Module
