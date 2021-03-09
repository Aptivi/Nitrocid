
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
    Sub StartThemeStudio(ByVal ThemeName As String)
        'Inform user that we're on the studio
        Wdbg("I", "Starting theme studio with theme name {0}", ThemeName)
        Dim Response As String
        Dim MaximumOptions As Integer = 17
        Dim StudioExiting As Boolean

        While Not StudioExiting
            Wdbg("I", "Studio not exiting yet. Populating {0} options...", MaximumOptions)
            Console.Clear()
            W(DoTranslation("Making a new theme ""{0}"".", currentLang) + vbNewLine, True, ColTypes.Neutral, ThemeName)

            'List options
            W("1) " + DoTranslation("Input color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedInputColor)
            W("2) " + DoTranslation("License color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedLicenseColor)
            W("3) " + DoTranslation("Continuable kernel error color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedContKernelErrorColor)
            W("4) " + DoTranslation("Uncontinuable kernel error color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedUncontKernelErrorColor)
            W("5) " + DoTranslation("Host name color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedHostNameShellColor)
            W("6) " + DoTranslation("User name color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedUserNameShellColor)
            W("7) " + DoTranslation("Background color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedBackgroundColor)
            W("8) " + DoTranslation("Neutral text color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedNeutralTextColor)
            W("9) " + DoTranslation("Command list color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedCmdListColor)
            W("10) " + DoTranslation("Command definition color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedCmdDefColor)
            W("11) " + DoTranslation("Stage color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedStageColor)
            W("12) " + DoTranslation("Error color", currentLang) + ": [{0}] " + vbNewLine, True, ColTypes.HelpCmd, SelectedErrorColor)
            W("13) " + DoTranslation("Warning color", currentLang) + ": [{0}] ", True, ColTypes.HelpCmd, SelectedWarningColor)
            W("14) " + DoTranslation("Option color", currentLang) + ": [{0}] " + vbNewLine, True, ColTypes.HelpCmd, SelectedOptionColor)

            'List saving options
            W("15) " + DoTranslation("Save Theme to Current Directory", currentLang), True, ColTypes.HelpCmd)
            W("16) " + DoTranslation("Save Theme to Another Directory", currentLang), True, ColTypes.HelpCmd)
            W("17) " + DoTranslation("Exit", currentLang) + vbNewLine, True, ColTypes.HelpCmd)

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
                If NumericResponse >= 1 And NumericResponse <= 17 Then
                    Wdbg("I", "Numeric response {0} is >= 1 and <= 17.", NumericResponse)
                    Select Case NumericResponse
                        Case 1 'Input color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedInputColor = ColorWheelReturn
                            End If
                        Case 2 'License color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedLicenseColor = ColorWheelReturn
                            End If
                        Case 3 'Continuable kernel error color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedContKernelErrorColor = ColorWheelReturn
                            End If
                        Case 4 'Uncontinuable kernel error color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedUncontKernelErrorColor = ColorWheelReturn
                            End If
                        Case 5 'Host name color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedHostNameShellColor = ColorWheelReturn
                            End If
                        Case 6 'User name color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedUserNameShellColor = ColorWheelReturn
                            End If
                        Case 7 'Background color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedBackgroundColor = ColorWheelReturn
                            End If
                        Case 8 'Neutral text color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedNeutralTextColor = ColorWheelReturn
                            End If
                        Case 9 'Command list color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedCmdListColor = ColorWheelReturn
                            End If
                        Case 10 'Command definition color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedCmdDefColor = ColorWheelReturn
                            End If
                        Case 11 'Stage color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedStageColor = ColorWheelReturn
                            End If
                        Case 12 'Error color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedErrorColor = ColorWheelReturn
                            End If
                        Case 13 'Warning color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedWarningColor = ColorWheelReturn
                            End If
                        Case 14 'Option color
                            Dim ColorWheelReturn As Integer = ColorWheel()
                            If ColorWheelReturn >= 0 And ColorWheelReturn <= 255 Then
                                SelectedOptionColor = ColorWheelReturn
                            End If
                        Case 15 'Save theme to current directory
                            SaveThemeToCurrentDirectory(ThemeName)
                        Case 16 'Save theme to another directory
                            Wdbg("I", "Prompting user for directory name...")
                            W(DoTranslation("Specify directory to save theme to:", currentLang) + " [{0}] ", False, ColTypes.Input, CurrDir)
                            Dim DirectoryName As String = Console.ReadLine
                            Wdbg("I", "Got directory name {0}.", DirectoryName)
                            SaveThemeToAnotherDirectory(ThemeName, DirectoryName)
                        Case 17 'Exit
                            Wdbg("I", "Exiting studio...")
                            StudioExiting = True
                    End Select
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid.", currentLang), True, ColTypes.Err, NumericResponse)
                    W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric.", currentLang), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <returns></returns>
    Function ColorWheel() As Integer
        Dim CurrentColor As ConsoleColors = ConsoleColors.White
        Dim ColorWheelExiting As Boolean
        Console.CursorVisible = False
        While Not ColorWheelExiting
            Console.Clear()
            W(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Press ENTER to quit.", currentLang), True, ColTypes.Neutral)
            W(vbNewLine + " <", False, ColTypes.Gray)
            WriteWhereC(CurrentColor.ToString, (Console.CursorLeft + 30 - CurrentColor.ToString.Length) / 2, Console.CursorTop, CurrentColor)
            WriteWhere(">", Console.CursorLeft + 27, Console.CursorTop, ColTypes.Gray)
            Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey
            If ConsoleResponse.Key = ConsoleKey.LeftArrow Then
                If CurrentColor = 0 Then
                    CurrentColor = 255
                Else
                    CurrentColor -= 1
                End If
            ElseIf ConsoleResponse.Key = ConsoleKey.RightArrow Then
                If CurrentColor = 255 Then
                    CurrentColor = 0
                Else
                    CurrentColor += 1
                End If
            ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                ColorWheelExiting = True
            End If
        End While
        Return CurrentColor
    End Function

End Module
