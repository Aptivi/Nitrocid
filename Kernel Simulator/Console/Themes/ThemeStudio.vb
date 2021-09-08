
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
        Wdbg("I", "Starting theme studio with theme name {0}", ThemeName)
        Dim Response As String
        Dim MaximumOptions As Integer = 23
        Dim StudioExiting As Boolean

        While Not StudioExiting
            Wdbg("I", "Studio not exiting yet. Populating {0} options...", MaximumOptions)
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
            W("14) " + DoTranslation("Option color") + ": [{0}] ", True, ColTypes.Option, SelectedOptionColor.PlainSequence)
            W("15) " + DoTranslation("Banner color") + ": [{0}] " + vbNewLine, True, ColTypes.Option, SelectedBannerColor.PlainSequence)

            'List saving and loading options
            W("16) " + DoTranslation("Save Theme to Current Directory"), True, ColTypes.Option)
            W("17) " + DoTranslation("Save Theme to Another Directory..."), True, ColTypes.Option)
            W("18) " + DoTranslation("Save Theme to Current Directory as..."), True, ColTypes.Option)
            W("19) " + DoTranslation("Save Theme to Another Directory as..."), True, ColTypes.Option)
            W("20) " + DoTranslation("Load Theme From File..."), True, ColTypes.Option)
            W("21) " + DoTranslation("Load Theme From Prebuilt Themes..."), True, ColTypes.Option)
            W("22) " + DoTranslation("Preview..."), True, ColTypes.Option)
            W("23) " + DoTranslation("Exit") + vbNewLine, True, ColTypes.Option)

            'Prompt user
            Wdbg("I", "Waiting for user input...")
            W("> ", False, ColTypes.Input)
            Response = Console.ReadLine
            Wdbg("I", "Got response: {0}", Response)

            'Check for response integrity
            If IsNumeric(Response) Then
                Wdbg("I", "Response is numeric.")
                Dim NumericResponse As Integer = Response
                Wdbg("I", "Checking response...")
                If NumericResponse >= 1 And NumericResponse <= MaximumOptions Then
                    Wdbg("I", "Numeric response {0} is >= 1 and <= {0}.", NumericResponse, MaximumOptions)
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
                            Dim ColorWheelReturn As String = ColorWheel(SelectedOptionColor.Type = ColorType.TrueColor, If(SelectedOptionColor.Type = ColorType._255Color, SelectedOptionColor.PlainSequence, ConsoleColors.White), SelectedOptionColor.R, SelectedOptionColor.G, SelectedOptionColor.B)
                            SelectedOptionColor = New Color(ColorWheelReturn)
                        Case 15 'Banner color
                            Dim ColorWheelReturn As String = ColorWheel(SelectedBannerColor.Type = ColorType.TrueColor, If(SelectedBannerColor.Type = ColorType._255Color, SelectedBannerColor.PlainSequence, ConsoleColors.White), SelectedBannerColor.R, SelectedBannerColor.G, SelectedBannerColor.B)
                            SelectedBannerColor = New Color(ColorWheelReturn)
                        Case 16 'Save theme to current directory
                            SaveThemeToCurrentDirectory(ThemeName)
                        Case 17 'Save theme to another directory...
                            Wdbg("I", "Prompting user for directory name...")
                            W(DoTranslation("Specify directory to save theme to:") + " [{0}] ", False, ColTypes.Input, CurrDir)
                            Dim DirectoryName As String = Console.ReadLine
                            DirectoryName = If(String.IsNullOrWhiteSpace(DirectoryName), CurrDir, DirectoryName)
                            Wdbg("I", "Got directory name {0}.", DirectoryName)
                            SaveThemeToAnotherDirectory(ThemeName, DirectoryName)
                        Case 18 'Save theme to current directory as...
                            Wdbg("I", "Prompting user for theme name...")
                            W(DoTranslation("Specify theme name:") + " [{0}] ", False, ColTypes.Input, ThemeName)
                            Dim AltThemeName As String = Console.ReadLine
                            AltThemeName = If(String.IsNullOrWhiteSpace(AltThemeName), ThemeName, AltThemeName)
                            Wdbg("I", "Got theme name {0}.", AltThemeName)
                            SaveThemeToCurrentDirectory(AltThemeName)
                        Case 19 'Save theme to another directory as...
                            Wdbg("I", "Prompting user for theme and directory name...")
                            W(DoTranslation("Specify directory to save theme to:") + " [{0}] ", False, ColTypes.Input, CurrDir)
                            Dim DirectoryName As String = Console.ReadLine
                            DirectoryName = If(String.IsNullOrWhiteSpace(DirectoryName), CurrDir, DirectoryName)
                            Wdbg("I", "Got directory name {0}.", DirectoryName)
                            Wdbg("I", "Prompting user for theme name...")
                            W(DoTranslation("Specify theme name:") + " [{0}] ", False, ColTypes.Input, ThemeName)
                            Dim AltThemeName As String = Console.ReadLine
                            AltThemeName = If(String.IsNullOrWhiteSpace(AltThemeName), ThemeName, AltThemeName)
                            Wdbg("I", "Got theme name {0}.", AltThemeName)
                            SaveThemeToAnotherDirectory(AltThemeName, DirectoryName)
                        Case 20 'Load Theme From File...
                            Wdbg("I", "Prompting user for theme name...")
                            W(DoTranslation("Specify theme file name wihout the .json extension:") + " ", False, ColTypes.Input)
                            Dim AltThemeName As String = Console.ReadLine + ".json"
                            Wdbg("I", "Got theme name {0}.", AltThemeName)
                            LoadThemeFromFile(AltThemeName)
                        Case 21 'Load Theme From Prebuilt Themes...
                            Wdbg("I", "Prompting user for theme name...")
                            W(DoTranslation("Specify theme name:") + " ", False, ColTypes.Input)
                            Dim AltThemeName As String = Console.ReadLine
                            Wdbg("I", "Got theme name {0}.", AltThemeName)
                            LoadThemeFromResource(AltThemeName)
                        Case 22 'Preview...
                            Wdbg("I", "Printing text with colors of theme...")
                            PreparePreview()
                        Case 23 'Exit
                            Wdbg("I", "Exiting studio...")
                            StudioExiting = True
                    End Select
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, NumericResponse)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End While

        'Raise event
        EventManager.RaiseThemeStudioExit()
    End Sub

End Module
