
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

Public Module Color

    ''' <summary>
    ''' Enumeration for color types
    ''' </summary>
    Public Enum ColTypes As Integer
        Neutral = 1
        Input = 2
        Continuable = 3
        Uncontinuable = 4
        HostName = 5
        UserName = 6
        License = 7
        Gray = 8
        ListValue = 9
        ListEntry = 10
        Stage = 11
        Err = 12
        Warning = 13
        [Option] = 14
    End Enum

    'Variables for colors used by previous versions of Kernel.
    Public InputColor As ConsoleColors = ConsoleColors.White
    Public LicenseColor As ConsoleColors = ConsoleColors.White
    Public ContKernelErrorColor As ConsoleColors = ConsoleColors.Yellow
    Public UncontKernelErrorColor As ConsoleColors = ConsoleColors.Red
    Public HostNameShellColor As ConsoleColors = ConsoleColors.DarkGreen
    Public UserNameShellColor As ConsoleColors = ConsoleColors.Green
    Public BackgroundColor As ConsoleColors = ConsoleColors.Black
    Public NeutralTextColor As ConsoleColors = ConsoleColors.Gray
    Public ListEntryColor As ConsoleColors = ConsoleColors.DarkYellow
    Public ListValueColor As ConsoleColors = ConsoleColors.DarkGray
    Public StageColor As ConsoleColors = ConsoleColors.Green
    Public ErrorColor As ConsoleColors = ConsoleColors.Red
    Public WarningColor As ConsoleColors = ConsoleColors.Yellow
    Public OptionColor As ConsoleColors = ConsoleColors.DarkYellow

    'Templates array (available ones)
    Public colorTemplates As New Dictionary(Of String, ThemeInfo) From {{"Default", New ThemeInfo("_Default")},
                                                                        {"RedConsole", New ThemeInfo("RedConsole")},
                                                                        {"Bluespire", New ThemeInfo("Bluespire")},
                                                                        {"Hacker", New ThemeInfo("Hacker")},
                                                                        {"Ubuntu", New ThemeInfo("Ubuntu")},
                                                                        {"YellowFG", New ThemeInfo("YellowFG")},
                                                                        {"YellowBG", New ThemeInfo("YellowBG")},
                                                                        {"SolarizedDark", New ThemeInfo("SolarizedDark")},
                                                                        {"SolarizedLight", New ThemeInfo("SolarizedLight")},
                                                                        {"NeonBreeze", New ThemeInfo("NeonBreeze")},
                                                                        {"TrafficLight", New ThemeInfo("TrafficLight")},
                                                                        {"Windows95", New ThemeInfo("Windows95")},
                                                                        {"GTASA", New ThemeInfo("GTASA")},
                                                                        {"GrayOnYellow", New ThemeInfo("GrayOnYellow")},
                                                                        {"BlackOnWhite", New ThemeInfo("BlackOnWhite")},
                                                                        {"Debian", New ThemeInfo("Debian")},
                                                                        {"NFSHP-Cop", New ThemeInfo("NFSHP_Cop")},
                                                                        {"NFSHP-Racer", New ThemeInfo("NFSHP_Racer")},
                                                                        {"TealerOS", New ThemeInfo("TealerOS")},
                                                                        {"BedOS", New ThemeInfo("BedOS")},
                                                                        {"3Y-Diamond", New ThemeInfo("_3Y_Diamond")},
                                                                        {"LinuxUncolored", New ThemeInfo("LinuxUncolored")},
                                                                        {"LinuxColoredDef", New ThemeInfo("LinuxColoredDef")}}

    ''' <summary>
    ''' Resets all colors to default
    ''' </summary>
    Public Sub ResetColors()
        Wdbg("I", "Resetting colors")
        Dim DefInfo As New ThemeInfo("_Default")
        InputColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeInputColor), ConsoleColors)
        LicenseColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeLicenseColor), ConsoleColors)
        ContKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeContKernelErrorColor), ConsoleColors)
        UncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeUncontKernelErrorColor), ConsoleColors)
        HostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeHostNameShellColor), ConsoleColors)
        UserNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeUserNameShellColor), ConsoleColors)
        BackgroundColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeBackgroundColor), ConsoleColors)
        NeutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeNeutralTextColor), ConsoleColors)
        ListEntryColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeCmdListColor), ConsoleColors)
        ListValueColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeCmdDefColor), ConsoleColors)
        StageColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeStageColor), ConsoleColors)
        ErrorColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeErrorColor), ConsoleColors)
        WarningColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeWarningColor), ConsoleColors)
        OptionColor = CType([Enum].Parse(GetType(ConsoleColors), DefInfo.ThemeOptionColor), ConsoleColors)
        LoadBack()

        'Raise event
        EventManager.RaiseColorReset()
    End Sub

    ''' <summary>
    ''' Loads the background
    ''' </summary>
    Public Sub LoadBack()
        Try
            Wdbg("I", "Filling background with background color")
            Dim esc As Char = GetEsc()
            Console.Write(esc + "[48;5;" + CStr(BackgroundColor) + "m")
            Console.Clear()
        Catch ex As Exception
            Wdbg("E", "Failed to set background: {0}", ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Sets system colors according to the programmed templates
    ''' </summary>
    ''' <param name="theme">A specified theme</param>
    Public Sub TemplateSet(ByVal theme As String)
        Wdbg("I", "Theme: {0}", theme)
        If colorTemplates.ContainsKey(theme) Then
            Wdbg("I", "Theme found.")

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
                InputColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeInputColor), ConsoleColors)
                LicenseColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeLicenseColor), ConsoleColors)
                ContKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeContKernelErrorColor), ConsoleColors)
                UncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeUncontKernelErrorColor), ConsoleColors)
                HostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeHostNameShellColor), ConsoleColors)
                UserNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeUserNameShellColor), ConsoleColors)
                BackgroundColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeBackgroundColor), ConsoleColors)
                NeutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeNeutralTextColor), ConsoleColors)
                ListEntryColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeCmdListColor), ConsoleColors)
                ListValueColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeCmdDefColor), ConsoleColors)
                StageColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeStageColor), ConsoleColors)
                ErrorColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeErrorColor), ConsoleColors)
                WarningColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeWarningColor), ConsoleColors)
                OptionColor = CType([Enum].Parse(GetType(ConsoleColors), ThemeInfo.ThemeOptionColor), ConsoleColors)
#Enable Warning BC42104

                'Load background
                LoadBack()
            End If

            'Save and parse theme
            Wdbg("I", "Saving theme")
            MakePermanent()

            'Raise event
            EventManager.RaiseThemeSet(theme)
        Else
            W(DoTranslation("Invalid color template {0}"), True, ColTypes.Err, theme)
            Wdbg("E", "Theme not found.")

            'Raise event
            EventManager.RaiseThemeSetError(theme, "notfound")
        End If
    End Sub

    ''' <summary>
    ''' Makes the color configuration permanent
    ''' </summary>
    Public Sub MakePermanent()
        Dim ksconf As New IniFile()
        Dim configPath As String = paths("Configuration")
        ksconf.Load(configPath)
        ksconf.Sections("Colors").Keys("User Name Shell Color").Value = UserNameShellColor.ToString
        ksconf.Sections("Colors").Keys("Host Name Shell Color").Value = HostNameShellColor.ToString
        ksconf.Sections("Colors").Keys("Continuable Kernel Error Color").Value = ContKernelErrorColor.ToString
        ksconf.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value = UncontKernelErrorColor.ToString
        ksconf.Sections("Colors").Keys("Text Color").Value = NeutralTextColor.ToString
        ksconf.Sections("Colors").Keys("License Color").Value = LicenseColor.ToString
        ksconf.Sections("Colors").Keys("Background Color").Value = BackgroundColor.ToString
        ksconf.Sections("Colors").Keys("Input Color").Value = InputColor.ToString
        ksconf.Sections("Colors").Keys("List Entry Color").Value = ListEntryColor.ToString
        ksconf.Sections("Colors").Keys("List Value Color").Value = ListValueColor.ToString
        ksconf.Sections("Colors").Keys("Kernel Stage Color").Value = StageColor.ToString
        ksconf.Sections("Colors").Keys("Error Text Color").Value = ErrorColor.ToString
        ksconf.Sections("Colors").Keys("Warning Text Color").Value = WarningColor.ToString
        ksconf.Sections("Colors").Keys("Option Color").Value = OptionColor.ToString
        ksconf.Save(configPath)
    End Sub

    ''' <summary>
    ''' Sets custom colors. It only works if colored shell is enabled.
    ''' </summary>
    ''' <param name="InputC">Input color</param>
    ''' <param name="LicenseC">License color</param>
    ''' <param name="ContKernelErrorC">Continuable kernel error color</param>
    ''' <param name="UncontKernelErrorC">Uncontinuable kernel error color</param>
    ''' <param name="HostNameC">Host name color</param>
    ''' <param name="UserNameC">User name color</param>
    ''' <param name="BackC">Background color</param>
    ''' <param name="NeutralTextC">Neutral text color</param>
    ''' <param name="CmdListC">Command list color</param>
    ''' <param name="CmdDefC">Command definition color</param>
    ''' <param name="StageC">Stage color</param>
    ''' <param name="ErrorC">Error color</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Function SetColors(InputC As ConsoleColors, LicenseC As ConsoleColors, ContKernelErrorC As ConsoleColors,
                              UncontKernelErrorC As ConsoleColors, HostNameC As ConsoleColors, UserNameC As ConsoleColors,
                              BackC As ConsoleColors, NeutralTextC As ConsoleColors, CmdListC As ConsoleColors,
                              CmdDefC As ConsoleColors, StageC As ConsoleColors, ErrorC As ConsoleColors, WarningC As ConsoleColors,
                              OptionC As ConsoleColors) As Boolean
        If ColoredShell = True Then
            If InputC = ConsoleColors.def Then
                InputC = ConsoleColors.White
            End If
            If LicenseC = ConsoleColors.def Then
                LicenseC = ConsoleColors.White
            End If
            If ContKernelErrorC = ConsoleColors.def Then
                ContKernelErrorC = ConsoleColors.Yellow
            End If
            If UncontKernelErrorC = ConsoleColors.def Then
                UncontKernelErrorC = ConsoleColors.Red
            End If
            If HostNameC = ConsoleColors.def Then
                HostNameC = ConsoleColors.DarkGreen
            End If
            If UserNameC = ConsoleColors.def Then
                UserNameC = ConsoleColors.Green
            End If
            If BackC = ConsoleColors.def Then
                BackC = ConsoleColors.Black
                LoadBack()
            End If
            If NeutralTextC = ConsoleColors.def Then
                NeutralTextC = ConsoleColors.Gray
            End If
            If CmdListC = ConsoleColors.def Then
                CmdListC = ConsoleColors.DarkYellow
            End If
            If CmdDefC = ConsoleColors.def Then
                CmdDefC = ConsoleColors.DarkGray
            End If
            If StageC = ConsoleColors.def Then
                StageC = ConsoleColors.Green
            End If
            If ErrorC = ConsoleColors.def Then
                ErrorC = ConsoleColors.Red
            End If
            If WarningC = ConsoleColors.def Then
                WarningC = ConsoleColors.Yellow
            End If
            If OptionC = ConsoleColors.def Then
                OptionC = ConsoleColors.DarkYellow
            End If
            If [Enum].IsDefined(GetType(ConsoleColors), InputC) And [Enum].IsDefined(GetType(ConsoleColors), LicenseC) And [Enum].IsDefined(GetType(ConsoleColors), ContKernelErrorC) And
               [Enum].IsDefined(GetType(ConsoleColors), UncontKernelErrorC) And [Enum].IsDefined(GetType(ConsoleColors), HostNameC) And [Enum].IsDefined(GetType(ConsoleColors), UserNameC) And
               [Enum].IsDefined(GetType(ConsoleColors), BackC) And [Enum].IsDefined(GetType(ConsoleColors), NeutralTextC) And [Enum].IsDefined(GetType(ConsoleColors), CmdListC) And
               [Enum].IsDefined(GetType(ConsoleColors), CmdDefC) And [Enum].IsDefined(GetType(ConsoleColors), StageC) And [Enum].IsDefined(GetType(ConsoleColors), ErrorC) And
               [Enum].IsDefined(GetType(ConsoleColors), WarningC) And [Enum].IsDefined(GetType(ConsoleColors), OptionC) Then
                InputColor = CType([Enum].Parse(GetType(ConsoleColors), InputC), ConsoleColors)
                LicenseColor = CType([Enum].Parse(GetType(ConsoleColors), LicenseC), ConsoleColors)
                ContKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), ContKernelErrorC), ConsoleColors)
                UncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), UncontKernelErrorC), ConsoleColors)
                HostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), HostNameC), ConsoleColors)
                UserNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), UserNameC), ConsoleColors)
                BackgroundColor = CType([Enum].Parse(GetType(ConsoleColors), BackC), ConsoleColors)
                NeutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), NeutralTextC), ConsoleColors)
                ListEntryColor = CType([Enum].Parse(GetType(ConsoleColors), CmdListC), ConsoleColors)
                ListValueColor = CType([Enum].Parse(GetType(ConsoleColors), CmdDefC), ConsoleColors)
                StageColor = CType([Enum].Parse(GetType(ConsoleColors), StageC), ConsoleColors)
                ErrorColor = CType([Enum].Parse(GetType(ConsoleColors), ErrorC), ConsoleColors)
                WarningColor = CType([Enum].Parse(GetType(ConsoleColors), WarningC), ConsoleColors)
                OptionColor = CType([Enum].Parse(GetType(ConsoleColors), OptionC), ConsoleColors)
                LoadBack()
                MakePermanent()

                'Raise event
                EventManager.RaiseColorSet()
                Return True
            Else
                EventManager.RaiseColorSetError("invalidcolors")
                Throw New Exceptions.ColorException(DoTranslation("One or more of the colors is invalid."))
            End If
        Else
            EventManager.RaiseColorSetError("nocolors")
            Throw New InvalidOperationException(DoTranslation("Colors are not available. Turn on colored shell in the kernel config."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Sets input color
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SetInputColor() As Boolean
        Dim esc As Char = GetEsc()
        If ColoredShell = True Then
            Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
            Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <returns></returns>
    Public Function ColorWheel() As Integer
        Dim CurrentColor As ConsoleColors = ConsoleColors.White
        Dim ColorWheelExiting As Boolean
        Console.CursorVisible = False
        While Not ColorWheelExiting
            Console.Clear()
            W(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Neutral)
            W(vbNewLine + " <", False, ColTypes.Gray)
            WriteWhereC(CurrentColor.ToString, (Console.CursorLeft + 30 - CurrentColor.ToString.Length) / 2, Console.CursorTop, True, CurrentColor)
            WriteWhere(">", Console.CursorLeft + 27, Console.CursorTop, True, ColTypes.Gray)
            WriteC(vbNewLine + vbNewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, CurrentColor)
            Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey(True)
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
            ElseIf ConsoleResponse.Key = ConsoleKey.I Then
                WriteWhere(DoTranslation("Enter color number from 0 to 255:") + " [{0}] ", 0, Console.WindowHeight - 1, False, ColTypes.Input, CInt(CurrentColor))
                Dim ColorNum As String = Console.ReadLine
                If IsNumeric(ColorNum) Then
                    If ColorNum >= 0 And ColorNum <= 255 Then
                        CurrentColor = ColorNum
                    End If
                End If
            ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                ColorWheelExiting = True
            End If
        End While
        Return CurrentColor
    End Function

End Module
