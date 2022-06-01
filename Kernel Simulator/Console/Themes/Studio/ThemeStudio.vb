
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

Imports KS.Misc.Reflection

Namespace ConsoleBase.Themes.Studio
    Module ThemeStudio

        ''' <summary>
        ''' Starts the theme studio
        ''' </summary>
        ''' <param name="ThemeName">Theme name</param>
        Sub StartThemeStudio(ThemeName As String)
            'Inform user that we're on the studio
            KernelEventManager.RaiseThemeStudioStarted()
            Wdbg(DebugLevel.I, "Starting theme studio with theme name {0}", ThemeName)
            Dim Response As String
            Dim MaximumOptions As Integer = 38 + 9 'Colors + options
            Dim StudioExiting As Boolean

            While Not StudioExiting
                Wdbg(DebugLevel.I, "Studio not exiting yet. Populating {0} options...", MaximumOptions)
                Console.Clear()
                Write(DoTranslation("Making a new theme ""{0}"".") + NewLine, True, ColTypes.Neutral, ThemeName)

                'List options
                Write("1) " + DoTranslation("Input color") + ": [{0}] ", True, ColTypes.Option, SelectedInputColor.PlainSequence)
                Write("2) " + DoTranslation("License color") + ": [{0}] ", True, ColTypes.Option, SelectedLicenseColor.PlainSequence)
                Write("3) " + DoTranslation("Continuable kernel error color") + ": [{0}] ", True, ColTypes.Option, SelectedContKernelErrorColor.PlainSequence)
                Write("4) " + DoTranslation("Uncontinuable kernel error color") + ": [{0}] ", True, ColTypes.Option, SelectedUncontKernelErrorColor.PlainSequence)
                Write("5) " + DoTranslation("Host name color") + ": [{0}] ", True, ColTypes.Option, SelectedHostNameShellColor.PlainSequence)
                Write("6) " + DoTranslation("User name color") + ": [{0}] ", True, ColTypes.Option, SelectedUserNameShellColor.PlainSequence)
                Write("7) " + DoTranslation("Background color") + ": [{0}] ", True, ColTypes.Option, SelectedBackgroundColor.PlainSequence)
                Write("8) " + DoTranslation("Neutral text color") + ": [{0}] ", True, ColTypes.Option, SelectedNeutralTextColor.PlainSequence)
                Write("9) " + DoTranslation("List entry color") + ": [{0}] ", True, ColTypes.Option, SelectedListEntryColor.PlainSequence)
                Write("10) " + DoTranslation("List value color") + ": [{0}] ", True, ColTypes.Option, SelectedListValueColor.PlainSequence)
                Write("11) " + DoTranslation("Stage color") + ": [{0}] ", True, ColTypes.Option, SelectedStageColor.PlainSequence)
                Write("12) " + DoTranslation("Error color") + ": [{0}] ", True, ColTypes.Option, SelectedErrorColor.PlainSequence)
                Write("13) " + DoTranslation("Warning color") + ": [{0}] ", True, ColTypes.Option, SelectedWarningColor.PlainSequence)
                Write("14) " + DoTranslation("Option color") + ": [{0}] ", True, ColTypes.Option, _SelectedOptionColor.PlainSequence)
                Write("15) " + DoTranslation("Banner color") + ": [{0}] ", True, ColTypes.Option, SelectedBannerColor.PlainSequence)
                Write("16) " + DoTranslation("Notification title color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationTitleColor.PlainSequence)
                Write("17) " + DoTranslation("Notification description color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationDescriptionColor.PlainSequence)
                Write("18) " + DoTranslation("Notification progress color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationProgressColor.PlainSequence)
                Write("19) " + DoTranslation("Notification failure color") + ": [{0}] ", True, ColTypes.Option, SelectedNotificationFailureColor.PlainSequence)
                Write("20) " + DoTranslation("Question color") + ": [{0}] ", True, ColTypes.Option, SelectedQuestionColor.PlainSequence)
                Write("21) " + DoTranslation("Success color") + ": [{0}] ", True, ColTypes.Option, SelectedSuccessColor.PlainSequence)
                Write("22) " + DoTranslation("User dollar color") + ": [{0}] ", True, ColTypes.Option, SelectedUserDollarColor.PlainSequence)
                Write("23) " + DoTranslation("Tip color") + ": [{0}] ", True, ColTypes.Option, SelectedTipColor.PlainSequence)
                Write("24) " + DoTranslation("Separator text color") + ": [{0}] ", True, ColTypes.Option, SelectedSeparatorTextColor.PlainSequence)
                Write("25) " + DoTranslation("Separator color") + ": [{0}] ", True, ColTypes.Option, SelectedSeparatorColor.PlainSequence)
                Write("26) " + DoTranslation("List title color") + ": [{0}] ", True, ColTypes.Option, SelectedListTitleColor.PlainSequence)
                Write("27) " + DoTranslation("Development warning color") + ": [{0}] ", True, ColTypes.Option, SelectedDevelopmentWarningColor.PlainSequence)
                Write("28) " + DoTranslation("Stage time color") + ": [{0}] ", True, ColTypes.Option, SelectedStageTimeColor.PlainSequence)
                Write("29) " + DoTranslation("Progress color") + ": [{0}] ", True, ColTypes.Option, SelectedProgressColor.PlainSequence)
                Write("30) " + DoTranslation("Back option color") + ": [{0}] ", True, ColTypes.Option, SelectedBackOptionColor.PlainSequence)
                Write("31) " + DoTranslation("Low priority border color") + ": [{0}] ", True, ColTypes.Option, SelectedLowPriorityBorderColor.PlainSequence)
                Write("32) " + DoTranslation("Medium priority border color") + ": [{0}] ", True, ColTypes.Option, SelectedMediumPriorityBorderColor.PlainSequence)
                Write("33) " + DoTranslation("High priority border color") + ": [{0}] ", True, ColTypes.Option, SelectedHighPriorityBorderColor.PlainSequence)
                Write("34) " + DoTranslation("Table separator color") + ": [{0}] ", True, ColTypes.Option, SelectedTableSeparatorColor.PlainSequence)
                Write("35) " + DoTranslation("Table header color") + ": [{0}] ", True, ColTypes.Option, SelectedTableHeaderColor.PlainSequence)
                Write("36) " + DoTranslation("Table value color") + ": [{0}] ", True, ColTypes.Option, SelectedTableValueColor.PlainSequence)
                Write("37) " + DoTranslation("Selected option color") + ": [{0}] ", True, ColTypes.Option, SelectedSelectedOptionColor.PlainSequence)
                Write("38) " + DoTranslation("Alternative option color") + ": [{0}] ", True, ColTypes.Option, SelectedAlternativeOptionColor.PlainSequence)
                Console.WriteLine()

                'List saving and loading options
                Write("39) " + DoTranslation("Save Theme to Current Directory"), True, ColTypes.AlternativeOption)
                Write("40) " + DoTranslation("Save Theme to Another Directory..."), True, ColTypes.AlternativeOption)
                Write("41) " + DoTranslation("Save Theme to Current Directory as..."), True, ColTypes.AlternativeOption)
                Write("42) " + DoTranslation("Save Theme to Another Directory as..."), True, ColTypes.AlternativeOption)
                Write("43) " + DoTranslation("Load Theme From File..."), True, ColTypes.AlternativeOption)
                Write("44) " + DoTranslation("Load Theme From Prebuilt Themes..."), True, ColTypes.AlternativeOption)
                Write("45) " + DoTranslation("Load Current Colors"), True, ColTypes.AlternativeOption)
                Write("46) " + DoTranslation("Preview..."), True, ColTypes.AlternativeOption)
                Write("47) " + DoTranslation("Exit"), True, ColTypes.AlternativeOption)
                Console.WriteLine()

                'Prompt user
                Wdbg(DebugLevel.I, "Waiting for user input...")
                Write("> ", False, ColTypes.Input)
                Response = ReadLine()
                Wdbg(DebugLevel.I, "Got response: {0}", Response)

                'Check for response integrity
                If IsStringNumeric(Response) Then
                    Wdbg(DebugLevel.I, "Response is numeric.")
                    Dim NumericResponse As Integer = Response
                    Wdbg(DebugLevel.I, "Checking response...")
                    If NumericResponse >= 1 And NumericResponse <= MaximumOptions Then
                        Wdbg(DebugLevel.I, "Numeric response {0} is >= 1 and <= {0}.", NumericResponse, MaximumOptions)
                        Dim SelectedColorInstance As Color
                        Select Case NumericResponse
                            Case 1 'Input color
                                SelectedColorInstance = SelectedInputColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedInputColor = New Color(ColorWheelReturn)
                            Case 2 'License color
                                SelectedColorInstance = SelectedLicenseColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedLicenseColor = New Color(ColorWheelReturn)
                            Case 3 'Continuable kernel error color
                                SelectedColorInstance = SelectedContKernelErrorColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedContKernelErrorColor = New Color(ColorWheelReturn)
                            Case 4 'Uncontinuable kernel error color
                                SelectedColorInstance = SelectedUncontKernelErrorColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedUncontKernelErrorColor = New Color(ColorWheelReturn)
                            Case 5 'Host name color
                                SelectedColorInstance = SelectedHostNameShellColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedHostNameShellColor = New Color(ColorWheelReturn)
                            Case 6 'User name color
                                SelectedColorInstance = SelectedUserNameShellColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedUserNameShellColor = New Color(ColorWheelReturn)
                            Case 7 'Background color
                                SelectedColorInstance = SelectedBackgroundColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedBackgroundColor = New Color(ColorWheelReturn)
                            Case 8 'Neutral text color
                                SelectedColorInstance = SelectedNeutralTextColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedNeutralTextColor = New Color(ColorWheelReturn)
                            Case 9 'list entry color
                                SelectedColorInstance = SelectedListEntryColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedListEntryColor = New Color(ColorWheelReturn)
                            Case 10 'list value color
                                SelectedColorInstance = SelectedListValueColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedListValueColor = New Color(ColorWheelReturn)
                            Case 11 'Stage color
                                SelectedColorInstance = SelectedStageColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedStageColor = New Color(ColorWheelReturn)
                            Case 12 'Error color
                                SelectedColorInstance = SelectedErrorColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedErrorColor = New Color(ColorWheelReturn)
                            Case 13 'Warning color
                                SelectedColorInstance = SelectedWarningColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedWarningColor = New Color(ColorWheelReturn)
                            Case 14 'Option color
                                SelectedColorInstance = _SelectedOptionColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                _SelectedOptionColor = New Color(ColorWheelReturn)
                            Case 15 'Banner color
                                SelectedColorInstance = SelectedBannerColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedBannerColor = New Color(ColorWheelReturn)
                            Case 16 'Notification title color
                                SelectedColorInstance = SelectedNotificationTitleColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedNotificationTitleColor = New Color(ColorWheelReturn)
                            Case 17 'Notification description color
                                SelectedColorInstance = SelectedNotificationDescriptionColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedNotificationDescriptionColor = New Color(ColorWheelReturn)
                            Case 18 'Notification progress color
                                SelectedColorInstance = SelectedNotificationProgressColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedNotificationProgressColor = New Color(ColorWheelReturn)
                            Case 19 'Notification failure color
                                SelectedColorInstance = SelectedNotificationFailureColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedNotificationFailureColor = New Color(ColorWheelReturn)
                            Case 20 'Question color
                                SelectedColorInstance = SelectedQuestionColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedQuestionColor = New Color(ColorWheelReturn)
                            Case 21 'Success color
                                SelectedColorInstance = SelectedSuccessColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedSuccessColor = New Color(ColorWheelReturn)
                            Case 22 'User dollar color
                                SelectedColorInstance = SelectedUserDollarColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedUserDollarColor = New Color(ColorWheelReturn)
                            Case 23 'Tip color
                                SelectedColorInstance = SelectedTipColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedTipColor = New Color(ColorWheelReturn)
                            Case 24 'Separator text color
                                SelectedColorInstance = SelectedSeparatorTextColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedSeparatorTextColor = New Color(ColorWheelReturn)
                            Case 25 'Separator color
                                SelectedColorInstance = SelectedSeparatorColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedSeparatorColor = New Color(ColorWheelReturn)
                            Case 26 'List title color
                                SelectedColorInstance = SelectedListTitleColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedListTitleColor = New Color(ColorWheelReturn)
                            Case 27 'Development warning color
                                SelectedColorInstance = SelectedDevelopmentWarningColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedDevelopmentWarningColor = New Color(ColorWheelReturn)
                            Case 28 'Stage time color
                                SelectedColorInstance = SelectedStageTimeColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedStageTimeColor = New Color(ColorWheelReturn)
                            Case 29 'Progress color
                                SelectedColorInstance = SelectedProgressColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedProgressColor = New Color(ColorWheelReturn)
                            Case 30 'Back option color
                                SelectedColorInstance = SelectedBackOptionColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedBackOptionColor = New Color(ColorWheelReturn)
                            Case 31 'Low priority border color
                                SelectedColorInstance = SelectedLowPriorityBorderColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedLowPriorityBorderColor = New Color(ColorWheelReturn)
                            Case 32 'Medium priority border color
                                SelectedColorInstance = SelectedMediumPriorityBorderColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedMediumPriorityBorderColor = New Color(ColorWheelReturn)
                            Case 33 'High priority border color
                                SelectedColorInstance = SelectedHighPriorityBorderColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedHighPriorityBorderColor = New Color(ColorWheelReturn)
                            Case 34 'Table separator color
                                SelectedColorInstance = SelectedTableSeparatorColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedTableSeparatorColor = New Color(ColorWheelReturn)
                            Case 35 'Table header color
                                SelectedColorInstance = SelectedTableHeaderColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedTableHeaderColor = New Color(ColorWheelReturn)
                            Case 36 'Table value color
                                SelectedColorInstance = SelectedTableValueColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedTableValueColor = New Color(ColorWheelReturn)
                            Case 37 'Selected option color
                                SelectedColorInstance = SelectedSelectedOptionColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedSelectedOptionColor = New Color(ColorWheelReturn)
                            Case 38 'Selected option color
                                SelectedColorInstance = SelectedAlternativeOptionColor
                                Dim ColorWheelReturn As String = ColorWheel(SelectedColorInstance.Type = ColorType.TrueColor, If(SelectedColorInstance.Type = ColorType._255Color, SelectedColorInstance.PlainSequence, ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B)
                                SelectedAlternativeOptionColor = New Color(ColorWheelReturn)
                            Case 39 'Save theme to current directory
                                SaveThemeToCurrentDirectory(ThemeName)
                            Case 40 'Save theme to another directory...
                                Wdbg(DebugLevel.I, "Prompting user for directory name...")
                                Write(DoTranslation("Specify directory to save theme to:") + " [{0}] ", False, ColTypes.Input, CurrDir)
                                Dim DirectoryName As String = ReadLine()
                                DirectoryName = If(String.IsNullOrWhiteSpace(DirectoryName), CurrDir, DirectoryName)
                                Wdbg(DebugLevel.I, "Got directory name {0}.", DirectoryName)
                                SaveThemeToAnotherDirectory(ThemeName, DirectoryName)
                            Case 41 'Save theme to current directory as...
                                Wdbg(DebugLevel.I, "Prompting user for theme name...")
                                Write(DoTranslation("Specify theme name:") + " [{0}] ", False, ColTypes.Input, ThemeName)
                                Dim AltThemeName As String = ReadLine()
                                AltThemeName = If(String.IsNullOrWhiteSpace(AltThemeName), ThemeName, AltThemeName)
                                Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                                SaveThemeToCurrentDirectory(AltThemeName)
                            Case 42 'Save theme to another directory as...
                                Wdbg(DebugLevel.I, "Prompting user for theme and directory name...")
                                Write(DoTranslation("Specify directory to save theme to:") + " [{0}] ", False, ColTypes.Input, CurrDir)
                                Dim DirectoryName As String = ReadLine()
                                DirectoryName = If(String.IsNullOrWhiteSpace(DirectoryName), CurrDir, DirectoryName)
                                Wdbg(DebugLevel.I, "Got directory name {0}.", DirectoryName)
                                Wdbg(DebugLevel.I, "Prompting user for theme name...")
                                Write(DoTranslation("Specify theme name:") + " [{0}] ", False, ColTypes.Input, ThemeName)
                                Dim AltThemeName As String = ReadLine()
                                AltThemeName = If(String.IsNullOrWhiteSpace(AltThemeName), ThemeName, AltThemeName)
                                Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                                SaveThemeToAnotherDirectory(AltThemeName, DirectoryName)
                            Case 43 'Load Theme From File...
                                Wdbg(DebugLevel.I, "Prompting user for theme name...")
                                Write(DoTranslation("Specify theme file name wihout the .json extension:") + " ", False, ColTypes.Input)
                                Dim AltThemeName As String = ReadLine() + ".json"
                                Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                                LoadThemeFromFile(AltThemeName)
                            Case 44 'Load Theme From Prebuilt Themes...
                                Wdbg(DebugLevel.I, "Prompting user for theme name...")
                                Write(DoTranslation("Specify theme name:") + " ", False, ColTypes.Input)
                                Dim AltThemeName As String = ReadLine()
                                Wdbg(DebugLevel.I, "Got theme name {0}.", AltThemeName)
                                LoadThemeFromResource(AltThemeName)
                            Case 45 'Load Current Colors
                                Wdbg(DebugLevel.I, "Loading current colors...")
                                LoadThemeFromCurrentColors()
                            Case 46 'Preview...
                                Wdbg(DebugLevel.I, "Printing text with colors of theme...")
                                PreparePreview()
                            Case 47 'Exit
                                Wdbg(DebugLevel.I, "Exiting studio...")
                                StudioExiting = True
                        End Select
                    Else
                        Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                        Write(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, NumericResponse)
                        Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                Else
                    Wdbg(DebugLevel.W, "Answer is not numeric.")
                    Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                    Write(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            End While

            'Raise event
            KernelEventManager.RaiseThemeStudioExit()
        End Sub

    End Module
End Namespace