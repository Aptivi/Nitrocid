
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

Module ThemeStudio

    ''' <summary>
    ''' Starts the theme studio
    ''' </summary>
    ''' <param name="ThemeName">Theme name</param>
    Sub StartThemeStudio(ThemeName As String)
        'Inform user that we're on the studio
        EventManager.RaiseThemeStudioStarted()
        Wdbg(DebugLevel.I, "Starting theme studio with theme name {0}", ThemeName)
        Dim Response As String
        Dim MaximumOptions As Integer = 37 + 8 'Colors + options
        Dim StudioExiting As Boolean

        While Not StudioExiting
            Wdbg(DebugLevel.I, "Studio not exiting yet. Populating {0} options...", MaximumOptions)
            Console.Clear()
            W(DoTranslation("Making a new theme ""{0}"".") + vbNewLine, True, ColTypes.Neutral, ThemeName)

            'List options
            W("1) " + DoTranslation("Input color") + ": [{0}] ", True, ColTypes.Option, SelectedInputColor.PlainSequence)
            W("2) " + DoTranslation("License color") + ": [{0}] ", True, ColTypes.Option, SelectedLicenseColor.PlainSequence)
            W("3) " + DoTranslation("Continuable kernel error color") + ": [{0}] ", True, ColTypes.Option, SelectedContKernelErrorColor.PlainSequence)
            W("4) " + DoTranslation("Uncontinuable kernel error color") + ": [{0}] ", True, ColTypes.Option, SelectedUncontKernelErrorColor.PlainSequence)
            W("5) " + DoTranslation("Host name color") + ": [{0}] ", True, ColTypes.Option, SelectedHostNameShellColor.PlainSequence)
            W("6) " + DoTranslation("User name color") + ": [{0}] ", True, ColTypes.Option, SelectedUserNameShellColor.PlainSequence)
            W("7) " + DoTranslation("Background color") + ": [{0}] ", True, ColTypes.Option, SelectedBackgroundColor.PlainSequence)
            W("8) " + DoTranslation("Neutral text color") + ": [{0}] ", True, ColTypes.Option, SelectedNeutralTextColor.PlainSequence)
            W("9) " + DoTranslation("List entry color") + ": [{0}] ", True, ColTypes.Option, SelectedListEntryColor.PlainSequence)
            W("10) " + DoTranslation("List value color") + ": [{0}] ", True, ColTypes.Option, SelectedListValueColor.PlainSequence)
            W("11) " + DoTranslation("Stage color") + ": [{0}] ", True, ColTypes.Option, SelectedStageColor.PlainSequence)
            W("12) " + DoTranslation("Error color") + ": [{0}] ", True, ColTypes.Option, SelectedErrorColor.PlainSequence)
            W("13) " + DoTranslation("Warning color") + ": [{0}] ", True, ColTypes.Option, SelectedWarningColor.PlainSequence)
            W("14) " + DoTranslation("Option color") + ": [{0}] ", True, ColTypes.Option, _SelectedOptionColor.PlainSequence)
            W("15) " + DoTranslation("Banner color") + ": [{0}] ", True, ColTypes.Option, SelectedBannerColor.PlainSequence)
            W("16) " + DoTranslation("Notification title color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationTitleColor.PlainSequence)
            W("17) " + DoTranslation("Notification description color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationDescriptionColor.PlainSequence)
            W("18) " + DoTranslation("Notification progress color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationProgressColor.PlainSequence)
            W("19) " + DoTranslation("Notification failure color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationFailureColor.PlainSequence)
            W("20) " + DoTranslation("Question color") + ": [{0}] ", True, ColTypes.Option, SelectedQuestionColor.PlainSequence)
            W("21) " + DoTranslation("Success color") + ": [{0}] ", True, ColTypes.Option, SelectedSuccessColor.PlainSequence)
            W("22) " + DoTranslation("User dollar color") + ": [{0}] ", True, ColTypes.Option, SelectedUserDollarColor.PlainSequence)
            W("23) " + DoTranslation("Tip color") + ": [{0}] ", True, ColTypes.Option, SelectedTipColor.PlainSequence)
            W("24) " + DoTranslation("Separator text color") + ": [{0}] ", True, ColTypes.Option, SelectedSeparatorTextColor.PlainSequence)
            W("25) " + DoTranslation("Separator color") + ": [{0}] ", True, ColTypes.Option, SelectedSeparatorColor.PlainSequence)
            W("26) " + DoTranslation("List title color") + ": [{0}] ", True, ColTypes.Option, SelectedListTitleColor.PlainSequence)
            W("27) " + DoTranslation("Development warning color") + ": [{0}] ", True, ColTypes.Option, SelectedDevelopmentWarningColor.PlainSequence)
            W("28) " + DoTranslation("Stage time color") + ": [{0}] ", True, ColTypes.Option, SelectedStageTimeColor.PlainSequence)
            W("29) " + DoTranslation("Progress color") + ": [{0}] ", True, ColTypes.Option, SelectedProgressColor.PlainSequence)
            W("30) " + DoTranslation("Back option color") + ": [{0}] ", True, ColTypes.Option, SelectedBackOptionColor.PlainSequence)
            W("31) " + DoTranslation("Low priority border color") + ": [{0}] ", True, ColTypes.Option, SelectedLowPriorityBorderColor.PlainSequence)
            W("32) " + DoTranslation("Medium priority border color") + ": [{0}] ", True, ColTypes.Option, SelectedMediumPriorityBorderColor.PlainSequence)
            W("33) " + DoTranslation("High priority border color") + ": [{0}] ", True, ColTypes.Option, SelectedHighPriorityBorderColor.PlainSequence)
            W("34) " + DoTranslation("Table separator color") + ": [{0}] ", True, ColTypes.Option, SelectedTableSeparatorColor.PlainSequence)
            W("35) " + DoTranslation("Table header color") + ": [{0}] ", True, ColTypes.Option, SelectedTableHeaderColor.PlainSequence)
            W("36) " + DoTranslation("Table value color") + ": [{0}] ", True, ColTypes.Option, SelectedTableValueColor.PlainSequence)
            W("37) " + DoTranslation("Selected option color") + ": [{0}] ", True, ColTypes.Option, SelectedSelectedOptionColor.PlainSequence)
            Console.WriteLine()

            'List saving and loading options
            W("38) " + DoTranslation("Save Theme to Current Directory"), True, ColTypes.Option)
            W("39) " + DoTranslation("Save Theme to Another Directory..."), True, ColTypes.Option)
            W("40) " + DoTranslation("Save Theme to Current Directory as..."), True, ColTypes.Option)
            W("41) " + DoTranslation("Save Theme to Another Directory as..."), True, ColTypes.Option)
            W("42) " + DoTranslation("Load Theme From File..."), True, ColTypes.Option)
            W("43) " + DoTranslation("Load Theme From Prebuilt Themes..."), True, ColTypes.Option)
            W("44) " + DoTranslation("Preview..."), True, ColTypes.Option)
            W("45) " + DoTranslation("Exit"), True, ColTypes.Option)
            Console.WriteLine()

            'Prompt user
            Wdbg(DebugLevel.I, "Waiting for user input...")
            W("> ", False, ColTypes.Input)
            Response = Console.ReadLine
            Wdbg(DebugLevel.I, "Got response: {0}", Response)

            'Check for response integrity
            If IsNumeric(Response) Then
                Wdbg(DebugLevel.I, "Response is numeric.")
                Dim NumericResponse As Integer = Response
                Wdbg(DebugLevel.I, "Checking response...")
                If NumericResponse >= 1 And NumericResponse <= MaximumOptions Then
                    Wdbg(DebugLevel.I, "Numeric response {0} is >= 1 and <= {0}.", NumericResponse, MaximumOptions)
                    Select Case NumericResponse
                        Case 1 'Input color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedInputColor.Type = ColorType.TrueColor, If(SelectedInputColor.Type = ColorType._255Color, SelectedInputColor.PlainSequence, ConsoleColors.White), SelectedInputColor.R, SelectedInputColor.G, SelectedInputColor.B)
                            SelectedInputColor = New Color(ColorWheelReturn)
                        Case 2 'License color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedLicenseColor.Type = ColorType.TrueColor, If(SelectedLicenseColor.Type = ColorType._255Color, SelectedLicenseColor.PlainSequence, ConsoleColors.White), SelectedLicenseColor.R, SelectedLicenseColor.G, SelectedLicenseColor.B)
                            SelectedLicenseColor = New Color(ColorWheelReturn)
                        Case 3 'Continuable kernel error color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedContKernelErrorColor.Type = ColorType.TrueColor, If(SelectedContKernelErrorColor.Type = ColorType._255Color, SelectedContKernelErrorColor.PlainSequence, ConsoleColors.White), SelectedContKernelErrorColor.R, SelectedContKernelErrorColor.G, SelectedContKernelErrorColor.B)
                            SelectedContKernelErrorColor = New Color(ColorWheelReturn)
                        Case 4 'Uncontinuable kernel error color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedUncontKernelErrorColor.Type = ColorType.TrueColor, If(SelectedUncontKernelErrorColor.Type = ColorType._255Color, SelectedUncontKernelErrorColor.PlainSequence, ConsoleColors.White), SelectedUncontKernelErrorColor.R, SelectedUncontKernelErrorColor.G, SelectedUncontKernelErrorColor.B)
                            SelectedUncontKernelErrorColor = New Color(ColorWheelReturn)
                        Case 5 'Host name color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedHostNameShellColor.Type = ColorType.TrueColor, If(SelectedHostNameShellColor.Type = ColorType._255Color, SelectedHostNameShellColor.PlainSequence, ConsoleColors.White), SelectedHostNameShellColor.R, SelectedHostNameShellColor.G, SelectedHostNameShellColor.B)
                            SelectedHostNameShellColor = New Color(ColorWheelReturn)
                        Case 6 'User name color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedUserNameShellColor.Type = ColorType.TrueColor, If(SelectedUserNameShellColor.Type = ColorType._255Color, SelectedUserNameShellColor.PlainSequence, ConsoleColors.White), SelectedUserNameShellColor.R, SelectedUserNameShellColor.G, SelectedUserNameShellColor.B)
                            SelectedUserNameShellColor = New Color(ColorWheelReturn)
                        Case 7 'Background color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedBackgroundColor.Type = ColorType.TrueColor, If(SelectedBackgroundColor.Type = ColorType._255Color, SelectedBackgroundColor.PlainSequence, ConsoleColors.White), SelectedBackgroundColor.R, SelectedBackgroundColor.G, SelectedBackgroundColor.B)
                            SelectedBackgroundColor = New Color(ColorWheelReturn)
                        Case 8 'Neutral text color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedNeutralTextColor.Type = ColorType.TrueColor, If(SelectedNeutralTextColor.Type = ColorType._255Color, SelectedNeutralTextColor.PlainSequence, ConsoleColors.White), SelectedNeutralTextColor.R, SelectedNeutralTextColor.G, SelectedNeutralTextColor.B)
                            SelectedNeutralTextColor = New Color(ColorWheelReturn)
                        Case 9 'list entry color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedListEntryColor.Type = ColorType.TrueColor, If(SelectedListEntryColor.Type = ColorType._255Color, SelectedListEntryColor.PlainSequence, ConsoleColors.White), SelectedListEntryColor.R, SelectedListEntryColor.G, SelectedListEntryColor.B)
                            SelectedListEntryColor = New Color(ColorWheelReturn)
                        Case 10 'list value color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedListValueColor.Type = ColorType.TrueColor, If(SelectedListValueColor.Type = ColorType._255Color, SelectedListValueColor.PlainSequence, ConsoleColors.White), SelectedListValueColor.R, SelectedListValueColor.G, SelectedListValueColor.B)
                            SelectedListValueColor = New Color(ColorWheelReturn)
                        Case 11 'Stage color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedStageColor.Type = ColorType.TrueColor, If(SelectedStageColor.Type = ColorType._255Color, SelectedStageColor.PlainSequence, ConsoleColors.White), SelectedStageColor.R, SelectedStageColor.G, SelectedStageColor.B)
                            SelectedStageColor = New Color(ColorWheelReturn)
                        Case 12 'Error color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedErrorColor.Type = ColorType.TrueColor, If(SelectedErrorColor.Type = ColorType._255Color, SelectedErrorColor.PlainSequence, ConsoleColors.White), SelectedErrorColor.R, SelectedErrorColor.G, SelectedErrorColor.B)
                            SelectedErrorColor = New Color(ColorWheelReturn)
                        Case 13 'Warning color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedWarningColor.Type = ColorType.TrueColor, If(SelectedWarningColor.Type = ColorType._255Color, SelectedWarningColor.PlainSequence, ConsoleColors.White), SelectedWarningColor.R, SelectedWarningColor.G, SelectedWarningColor.B)
                            SelectedWarningColor = New Color(ColorWheelReturn)
                        Case 14 'Option color
                            Dim ColorWheelReturn As String = ColorWheel(_SelectedOptionColor.Type = ColorType.TrueColor, If(_SelectedOptionColor.Type = ColorType._255Color, _SelectedOptionColor.PlainSequence, ConsoleColors.White), _SelectedOptionColor.R, _SelectedOptionColor.G, _SelectedOptionColor.B)
                            _SelectedOptionColor = New Color(ColorWheelReturn)
                        Case 15 'Banner color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedBannerColor.Type = ColorType.TrueColor, If(SelectedBannerColor.Type = ColorType._255Color, SelectedBannerColor.PlainSequence, ConsoleColors.White), SelectedBannerColor.R, SelectedBannerColor.G, SelectedBannerColor.B)
                            SelectedBannerColor = New Color(ColorWheelReturn)
                        Case 16 'Notification title color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedNotificationTitleColor.Type = ColorType.TrueColor, If(SelectedNotificationTitleColor.Type = ColorType._255Color, SelectedNotificationTitleColor.PlainSequence, ConsoleColors.White), SelectedNotificationTitleColor.R, SelectedNotificationTitleColor.G, SelectedNotificationTitleColor.B)
                            SelectedNotificationTitleColor = New Color(ColorWheelReturn)
                        Case 17 'Notification description color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedNotificationDescriptionColor.Type = ColorType.TrueColor, If(SelectedNotificationDescriptionColor.Type = ColorType._255Color, SelectedNotificationDescriptionColor.PlainSequence, ConsoleColors.White), SelectedNotificationDescriptionColor.R, SelectedNotificationDescriptionColor.G, SelectedNotificationDescriptionColor.B)
                            SelectedNotificationDescriptionColor = New Color(ColorWheelReturn)
                        Case 18 'Notification progress color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedNotificationProgressColor.Type = ColorType.TrueColor, If(SelectedNotificationProgressColor.Type = ColorType._255Color, SelectedNotificationProgressColor.PlainSequence, ConsoleColors.White), SelectedNotificationProgressColor.R, SelectedNotificationProgressColor.G, SelectedNotificationProgressColor.B)
                            SelectedNotificationProgressColor = New Color(ColorWheelReturn)
                        Case 19 'Notification failure color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedNotificationFailureColor.Type = ColorType.TrueColor, If(SelectedNotificationFailureColor.Type = ColorType._255Color, SelectedNotificationFailureColor.PlainSequence, ConsoleColors.White), SelectedNotificationFailureColor.R, SelectedNotificationFailureColor.G, SelectedNotificationFailureColor.B)
                            SelectedNotificationFailureColor = New Color(ColorWheelReturn)
                        Case 20 'Question color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedQuestionColor.Type = ColorType.TrueColor, If(SelectedQuestionColor.Type = ColorType._255Color, SelectedQuestionColor.PlainSequence, ConsoleColors.White), SelectedQuestionColor.R, SelectedQuestionColor.G, SelectedQuestionColor.B)
                            SelectedQuestionColor = New Color(ColorWheelReturn)
                        Case 21 'Success color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedSuccessColor.Type = ColorType.TrueColor, If(SelectedSuccessColor.Type = ColorType._255Color, SelectedSuccessColor.PlainSequence, ConsoleColors.White), SelectedSuccessColor.R, SelectedSuccessColor.G, SelectedSuccessColor.B)
                            SelectedSuccessColor = New Color(ColorWheelReturn)
                        Case 22 'User dollar color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedUserDollarColor.Type = ColorType.TrueColor, If(SelectedUserDollarColor.Type = ColorType._255Color, SelectedUserDollarColor.PlainSequence, ConsoleColors.White), SelectedUserDollarColor.R, SelectedUserDollarColor.G, SelectedUserDollarColor.B)
                            SelectedUserDollarColor = New Color(ColorWheelReturn)
                        Case 23 'Tip color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedTipColor.Type = ColorType.TrueColor, If(SelectedTipColor.Type = ColorType._255Color, SelectedTipColor.PlainSequence, ConsoleColors.White), SelectedTipColor.R, SelectedTipColor.G, SelectedTipColor.B)
                            SelectedTipColor = New Color(ColorWheelReturn)
                        Case 24 'Separator text color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedSeparatorTextColor.Type = ColorType.TrueColor, If(SelectedSeparatorTextColor.Type = ColorType._255Color, SelectedSeparatorTextColor.PlainSequence, ConsoleColors.White), SelectedSeparatorTextColor.R, SelectedSeparatorTextColor.G, SelectedSeparatorTextColor.B)
                            SelectedSeparatorTextColor = New Color(ColorWheelReturn)
                        Case 25 'Separator color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedSeparatorColor.Type = ColorType.TrueColor, If(SelectedSeparatorColor.Type = ColorType._255Color, SelectedSeparatorColor.PlainSequence, ConsoleColors.White), SelectedSeparatorColor.R, SelectedSeparatorColor.G, SelectedSeparatorColor.B)
                            SelectedSeparatorColor = New Color(ColorWheelReturn)
                        Case 26 'List title color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedListTitleColor.Type = ColorType.TrueColor, If(SelectedListTitleColor.Type = ColorType._255Color, SelectedListTitleColor.PlainSequence, ConsoleColors.White), SelectedListTitleColor.R, SelectedListTitleColor.G, SelectedListTitleColor.B)
                            SelectedListTitleColor = New Color(ColorWheelReturn)
                        Case 27 'Development warning color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedDevelopmentWarningColor.Type = ColorType.TrueColor, If(SelectedDevelopmentWarningColor.Type = ColorType._255Color, SelectedDevelopmentWarningColor.PlainSequence, ConsoleColors.White), SelectedDevelopmentWarningColor.R, SelectedDevelopmentWarningColor.G, SelectedDevelopmentWarningColor.B)
                            SelectedDevelopmentWarningColor = New Color(ColorWheelReturn)
                        Case 28 'Stage time color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedStageTimeColor.Type = ColorType.TrueColor, If(SelectedStageTimeColor.Type = ColorType._255Color, SelectedStageTimeColor.PlainSequence, ConsoleColors.White), SelectedStageTimeColor.R, SelectedStageTimeColor.G, SelectedStageTimeColor.B)
                            SelectedStageTimeColor = New Color(ColorWheelReturn)
                        Case 29 'Progress color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedProgressColor.Type = ColorType.TrueColor, If(SelectedProgressColor.Type = ColorType._255Color, SelectedProgressColor.PlainSequence, ConsoleColors.White), SelectedProgressColor.R, SelectedProgressColor.G, SelectedProgressColor.B)
                            SelectedProgressColor = New Color(ColorWheelReturn)
                        Case 30 'Back option color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedBackOptionColor.Type = ColorType.TrueColor, If(SelectedBackOptionColor.Type = ColorType._255Color, SelectedBackOptionColor.PlainSequence, ConsoleColors.White), SelectedBackOptionColor.R, SelectedBackOptionColor.G, SelectedBackOptionColor.B)
                            SelectedBackOptionColor = New Color(ColorWheelReturn)
                        Case 31 'Low priority border color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedLowPriorityBorderColor.Type = ColorType.TrueColor, If(SelectedLowPriorityBorderColor.Type = ColorType._255Color, SelectedLowPriorityBorderColor.PlainSequence, ConsoleColors.White), SelectedLowPriorityBorderColor.R, SelectedLowPriorityBorderColor.G, SelectedLowPriorityBorderColor.B)
                            SelectedLowPriorityBorderColor = New Color(ColorWheelReturn)
                        Case 32 'Medium priority border color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedMediumPriorityBorderColor.Type = ColorType.TrueColor, If(SelectedMediumPriorityBorderColor.Type = ColorType._255Color, SelectedMediumPriorityBorderColor.PlainSequence, ConsoleColors.White), SelectedMediumPriorityBorderColor.R, SelectedMediumPriorityBorderColor.G, SelectedMediumPriorityBorderColor.B)
                            SelectedMediumPriorityBorderColor = New Color(ColorWheelReturn)
                        Case 33 'High priority border color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedHighPriorityBorderColor.Type = ColorType.TrueColor, If(SelectedHighPriorityBorderColor.Type = ColorType._255Color, SelectedHighPriorityBorderColor.PlainSequence, ConsoleColors.White), SelectedHighPriorityBorderColor.R, SelectedHighPriorityBorderColor.G, SelectedHighPriorityBorderColor.B)
                            SelectedHighPriorityBorderColor = New Color(ColorWheelReturn)
                        Case 34 'Table separator color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedTableSeparatorColor.Type = ColorType.TrueColor, If(SelectedTableSeparatorColor.Type = ColorType._255Color, SelectedTableSeparatorColor.PlainSequence, ConsoleColors.White), SelectedTableSeparatorColor.R, SelectedTableSeparatorColor.G, SelectedTableSeparatorColor.B)
                            SelectedTableSeparatorColor = New Color(ColorWheelReturn)
                        Case 35 'Table header color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedTableHeaderColor.Type = ColorType.TrueColor, If(SelectedTableHeaderColor.Type = ColorType._255Color, SelectedTableHeaderColor.PlainSequence, ConsoleColors.White), SelectedTableHeaderColor.R, SelectedTableHeaderColor.G, SelectedTableHeaderColor.B)
                            SelectedTableHeaderColor = New Color(ColorWheelReturn)
                        Case 36 'Table value color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedTableValueColor.Type = ColorType.TrueColor, If(SelectedTableValueColor.Type = ColorType._255Color, SelectedTableValueColor.PlainSequence, ConsoleColors.White), SelectedTableValueColor.R, SelectedTableValueColor.G, SelectedTableValueColor.B)
                            SelectedTableValueColor = New Color(ColorWheelReturn)
                        Case 37 'Selected option color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedSelectedOptionColor.Type = ColorType.TrueColor, If(SelectedSelectedOptionColor.Type = ColorType._255Color, SelectedSelectedOptionColor.PlainSequence, ConsoleColors.White), SelectedSelectedOptionColor.R, SelectedSelectedOptionColor.G, SelectedSelectedOptionColor.B)
                            SelectedSelectedOptionColor = New Color(ColorWheelReturn)
                        Case 38 'Save theme to current directory
                            SaveThemeToCurrentDirectory(ThemeName)
                        Case 39 'Save theme to another directory...
                            Wdbg(DebugLevel.I, "Prompting user for directory name...")
                            W(DoTranslation("Specify directory to save theme to:") + " [{0}] ", False, ColTypes.Input, CurrDir)
                            Dim DirectoryName As String = Console.ReadLine
                            DirectoryName = If(String.IsNullOrWhiteSpace(DirectoryName), CurrDir, DirectoryName)
                            Wdbg(DebugLevel.I, "Got directory name {0}.", DirectoryName)
                            SaveThemeToAnotherDirectory(ThemeName, DirectoryName)
                        Case 40 'Save theme to current directory as...
                            Wdbg(DebugLevel.I, "Prompting user for theme name...")
                            W(DoTranslation("Specify theme name:") + " [{0}] ", False, ColTypes.Input, ThemeName)
                            Dim AltThemeName As String = Console.ReadLine
                            AltThemeName = If(String.IsNullOrWhiteSpace(AltThemeName), ThemeName, AltThemeName)
                            Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                            SaveThemeToCurrentDirectory(AltThemeName)
                        Case 41 'Save theme to another directory as...
                            Wdbg(DebugLevel.I, "Prompting user for theme and directory name...")
                            W(DoTranslation("Specify directory to save theme to:") + " [{0}] ", False, ColTypes.Input, CurrDir)
                            Dim DirectoryName As String = Console.ReadLine
                            DirectoryName = If(String.IsNullOrWhiteSpace(DirectoryName), CurrDir, DirectoryName)
                            Wdbg(DebugLevel.I, "Got directory name {0}.", DirectoryName)
                            Wdbg(DebugLevel.I, "Prompting user for theme name...")
                            W(DoTranslation("Specify theme name:") + " [{0}] ", False, ColTypes.Input, ThemeName)
                            Dim AltThemeName As String = Console.ReadLine
                            AltThemeName = If(String.IsNullOrWhiteSpace(AltThemeName), ThemeName, AltThemeName)
                            Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                            SaveThemeToAnotherDirectory(AltThemeName, DirectoryName)
                        Case 42 'Load Theme From File...
                            Wdbg(DebugLevel.I, "Prompting user for theme name...")
                            W(DoTranslation("Specify theme file name wihout the .json extension:") + " ", False, ColTypes.Input)
                            Dim AltThemeName As String = Console.ReadLine + ".json"
                            Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                            LoadThemeFromFile(AltThemeName)
                        Case 43 'Load Theme From Prebuilt Themes...
                            Wdbg(DebugLevel.I, "Prompting user for theme name...")
                            W(DoTranslation("Specify theme name:") + " ", False, ColTypes.Input)
                            Dim AltThemeName As String = Console.ReadLine
                            Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                            LoadThemeFromResource(AltThemeName)
                        Case 44 'Preview...
                            Wdbg(DebugLevel.I, "Printing text with colors of theme...")
                            PreparePreview()
                        Case 45 'Exit
                            Wdbg(DebugLevel.I, "Exiting studio...")
                            StudioExiting = True
                    End Select
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, NumericResponse)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            Else
                Wdbg(DebugLevel.W, "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End While

        'Raise event
        EventManager.RaiseThemeStudioExit()
    End Sub

End Module
