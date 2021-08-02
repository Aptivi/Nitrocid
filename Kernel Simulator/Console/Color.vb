
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

Public Class Color

    ''' <summary>
    ''' Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PlainSequence As String
    ''' <summary>
    ''' Parsable VT sequence (Foreground)
    ''' </summary>
    Public ReadOnly Property VTSequenceForeground As String
    ''' <summary>
    ''' Parsable VT sequence (Background)
    ''' </summary>
    Public ReadOnly Property VTSequenceBackground As String
    ''' <summary>
    ''' The red color value
    ''' </summary>
    Public ReadOnly Property R As Integer
    ''' <summary>
    ''' The green color value
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property G As Integer
    ''' <summary>
    ''' The blue color value
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property B As Integer
    ''' <summary>
    ''' Color type
    ''' </summary>
    Public ReadOnly Property Type As ColorType
    ''' <summary>
    ''' Is the color bright?
    ''' </summary>
    Public ReadOnly Property IsBright As Boolean

    ''' <summary>
    ''' Makes a new instance of color class from specifier.
    ''' </summary>
    ''' <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Sub New(ByVal ColorSpecifier As String)
        If ColorSpecifier.Contains(";") Then
            ColorSpecifier = ColorSpecifier.Replace("""", "")
            Dim ColorSpecifierArray() As String = ColorSpecifier.Split(";")
            If ColorSpecifierArray.Length = 3 Then
                PlainSequence = $"{ColorSpecifierArray(0)};{ColorSpecifierArray(1)};{ColorSpecifierArray(2)}"
                VTSequenceForeground = GetEsc() + $"[38;2;{PlainSequence}m"
                VTSequenceBackground = GetEsc() + $"[48;2;{PlainSequence}m"
                Type = ColorType.TrueColor
                IsBright = ColorSpecifierArray(0) + 0.2126 + ColorSpecifierArray(1) + 0.7152 + ColorSpecifierArray(2) + 0.0722 > 255 / 2
                R = ColorSpecifierArray(0)
                G = ColorSpecifierArray(1)
                B = ColorSpecifierArray(2)
            Else
                Throw New Exceptions.ColorException(DoTranslation("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows:") + " <R>;<G>;<B>")
            End If
        ElseIf IsNumeric(ColorSpecifier) Then
            ColorSpecifier = ColorSpecifier.Replace("""", "")
            Dim ColorsInfo As New ConsoleColorsInfo(ColorSpecifier)
            PlainSequence = ColorSpecifier
            VTSequenceForeground = GetEsc() + $"[38;5;{PlainSequence}m"
            VTSequenceBackground = GetEsc() + $"[48;5;{PlainSequence}m"
            Type = ColorType._255Color
            IsBright = ColorsInfo.IsBright
            R = ColorsInfo.R
            G = ColorsInfo.G
            B = ColorsInfo.B
        Else
            Throw New Exceptions.ColorException(DoTranslation("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows:") + " <R>;<G>;<B>")
        End If
    End Sub

End Class

Public Module ColorTools

    ''' <summary>
    ''' Enumeration for color types
    ''' </summary>
    Public Enum ColTypes As Integer
        ''' <summary>
        ''' Neutral text (for general purposes)
        ''' </summary>
        Neutral
        ''' <summary>
        ''' Input text
        ''' </summary>
        Input
        ''' <summary>
        ''' Continuable kernel panic text (usually sync'd with Warning)
        ''' </summary>
        Continuable
        ''' <summary>
        ''' Uncontinuable kernel panic text (usually sync'd with Error)
        ''' </summary>
        Uncontinuable
        ''' <summary>
        ''' Host name color
        ''' </summary>
        HostName
        ''' <summary>
        ''' User name color
        ''' </summary>
        UserName
        ''' <summary>
        ''' License color
        ''' </summary>
        License
        ''' <summary>
        ''' Gray color (for special purposes)
        ''' </summary>
        Gray
        ''' <summary>
        ''' List value text
        ''' </summary>
        ListValue
        ''' <summary>
        ''' List entry text
        ''' </summary>
        ListEntry
        ''' <summary>
        ''' Stage text
        ''' </summary>
        Stage
        ''' <summary>
        ''' Error text
        ''' </summary>
        [Error]
        ''' <summary>
        ''' Warning text
        ''' </summary>
        Warning
        ''' <summary>
        ''' Option text
        ''' </summary>
        [Option]
        ''' <summary>
        ''' Banner text
        ''' </summary>
        Banner
    End Enum

    ''' <summary>
    ''' Color type enumeration
    ''' </summary>
    Public Enum ColorType
        ''' <summary>
        ''' Color is a true color
        ''' </summary>
        TrueColor
        ''' <summary>
        ''' Color is a 256-bit color
        ''' </summary>
        _255Color
    End Enum

    'Variables for colors used by previous versions of Kernel.
    Public InputColor As String = New Color(ConsoleColors.White).PlainSequence
    Public LicenseColor As String = New Color(ConsoleColors.White).PlainSequence
    Public ContKernelErrorColor As String = New Color(ConsoleColors.Yellow).PlainSequence
    Public UncontKernelErrorColor As String = New Color(ConsoleColors.Red).PlainSequence
    Public HostNameShellColor As String = New Color(ConsoleColors.DarkGreen).PlainSequence
    Public UserNameShellColor As String = New Color(ConsoleColors.Green).PlainSequence
    Public BackgroundColor As String = New Color(ConsoleColors.Black).PlainSequence
    Public NeutralTextColor As String = New Color(ConsoleColors.Gray).PlainSequence
    Public ListEntryColor As String = New Color(ConsoleColors.DarkYellow).PlainSequence
    Public ListValueColor As String = New Color(ConsoleColors.DarkGray).PlainSequence
    Public StageColor As String = New Color(ConsoleColors.Green).PlainSequence
    Public ErrorColor As String = New Color(ConsoleColors.Red).PlainSequence
    Public WarningColor As String = New Color(ConsoleColors.Yellow).PlainSequence
    Public OptionColor As String = New Color(ConsoleColors.DarkYellow).PlainSequence
    Public BannerColor As String = New Color(ConsoleColors.Green).PlainSequence

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
                                                                        {"AyuDark", New ThemeInfo("AyuDark")},
                                                                        {"AyuLight", New ThemeInfo("AyuLight")},
                                                                        {"AyuMirage", New ThemeInfo("AyuMirage")},
                                                                        {"BrandingBlue", New ThemeInfo("BrandingBlue")},
                                                                        {"BrandingPurple", New ThemeInfo("BrandingPurple")},
                                                                        {"TrafficLight", New ThemeInfo("TrafficLight")},
                                                                        {"BreezeDark", New ThemeInfo("BreezeDark")},
                                                                        {"Breeze", New ThemeInfo("Breeze")},
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
        InputColor = DefInfo.ThemeInputColor.PlainSequence
        LicenseColor = DefInfo.ThemeLicenseColor.PlainSequence
        ContKernelErrorColor = DefInfo.ThemeContKernelErrorColor.PlainSequence
        UncontKernelErrorColor = DefInfo.ThemeUncontKernelErrorColor.PlainSequence
        HostNameShellColor = DefInfo.ThemeHostNameShellColor.PlainSequence
        UserNameShellColor = DefInfo.ThemeUserNameShellColor.PlainSequence
        BackgroundColor = DefInfo.ThemeBackgroundColor.PlainSequence
        NeutralTextColor = DefInfo.ThemeNeutralTextColor.PlainSequence
        ListEntryColor = DefInfo.ThemeListEntryColor.PlainSequence
        ListValueColor = DefInfo.ThemeListValueColor.PlainSequence
        StageColor = DefInfo.ThemeStageColor.PlainSequence
        ErrorColor = DefInfo.ThemeErrorColor.PlainSequence
        WarningColor = DefInfo.ThemeWarningColor.PlainSequence
        OptionColor = DefInfo.ThemeOptionColor.PlainSequence
        BannerColor = DefInfo.ThemeBannerColor.PlainSequence
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
            SetConsoleColor(New Color(BackgroundColor), True)
            Console.Clear()
        Catch ex As Exception
            Wdbg("E", "Failed to set background: {0}", ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Sets system colors according to the programmed templates
    ''' </summary>
    ''' <param name="theme">A specified theme</param>
    Public Sub ApplyThemeFromResources(ByVal theme As String)
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
                SetColors(ThemeInfo.ThemeInputColor.PlainSequence, ThemeInfo.ThemeLicenseColor.PlainSequence, ThemeInfo.ThemeContKernelErrorColor.PlainSequence,
                          ThemeInfo.ThemeUncontKernelErrorColor.PlainSequence, ThemeInfo.ThemeHostNameShellColor.PlainSequence, ThemeInfo.ThemeUserNameShellColor.PlainSequence,
                          ThemeInfo.ThemeBackgroundColor.PlainSequence, ThemeInfo.ThemeNeutralTextColor.PlainSequence, ThemeInfo.ThemeListEntryColor.PlainSequence,
                          ThemeInfo.ThemeListValueColor.PlainSequence, ThemeInfo.ThemeStageColor.PlainSequence, ThemeInfo.ThemeErrorColor.PlainSequence,
                          ThemeInfo.ThemeWarningColor.PlainSequence, ThemeInfo.ThemeOptionColor.PlainSequence, ThemeInfo.ThemeBannerColor.PlainSequence)
#Enable Warning BC42104
            End If

            'Raise event
            EventManager.RaiseThemeSet(theme)
        Else
            W(DoTranslation("Invalid color template {0}"), True, ColTypes.Error, theme)
            Wdbg("E", "Theme not found.")

            'Raise event
            EventManager.RaiseThemeSetError(theme, "notfound")
        End If
    End Sub

    ''' <summary>
    ''' Sets system colors according to the template file
    ''' </summary>
    ''' <param name="ThemeFile">Theme file</param>
    Public Sub ApplyThemeFromFile(ByVal ThemeFile As String)
        Try
            Wdbg("I", "Theme file name: {0}", ThemeFile)
            ThemeFile = NeutralizePath(ThemeFile, True)
            Wdbg("I", "Theme file path: {0}", ThemeFile)

            'Populate theme info
            Dim ThemeInfo As New ThemeInfo(New StreamReader(ThemeFile))

            If Not ThemeFile = "Default" Then
                'Set colors as appropriate
                SetColors(ThemeInfo.ThemeInputColor.PlainSequence, ThemeInfo.ThemeLicenseColor.PlainSequence, ThemeInfo.ThemeContKernelErrorColor.PlainSequence,
                          ThemeInfo.ThemeUncontKernelErrorColor.PlainSequence, ThemeInfo.ThemeHostNameShellColor.PlainSequence, ThemeInfo.ThemeUserNameShellColor.PlainSequence,
                          ThemeInfo.ThemeBackgroundColor.PlainSequence, ThemeInfo.ThemeNeutralTextColor.PlainSequence, ThemeInfo.ThemeListEntryColor.PlainSequence,
                          ThemeInfo.ThemeListValueColor.PlainSequence, ThemeInfo.ThemeStageColor.PlainSequence, ThemeInfo.ThemeErrorColor.PlainSequence,
                          ThemeInfo.ThemeWarningColor.PlainSequence, ThemeInfo.ThemeOptionColor.PlainSequence, ThemeInfo.ThemeBannerColor.PlainSequence)
            End If

            'Raise event
            EventManager.RaiseThemeSet(ThemeFile)
        Catch ex As Exception
            W(DoTranslation("Invalid color template {0}"), True, ColTypes.Error, ThemeFile)
            Wdbg("E", "Theme not found.")

            'Raise event
            EventManager.RaiseThemeSetError(ThemeFile, "notfound")
        End Try
    End Sub

    ''' <summary>
    ''' Makes the color configuration permanent
    ''' </summary>
    Public Sub MakePermanent()
        'We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
        ConfigToken("Colors")("User Name Shell Color") = If(New Color(UserNameShellColor).Type = ColorType.TrueColor, UserNameShellColor.EncloseByDoubleQuotes, UserNameShellColor)
        ConfigToken("Colors")("Host Name Shell Color") = If(New Color(HostNameShellColor).Type = ColorType.TrueColor, HostNameShellColor.EncloseByDoubleQuotes, HostNameShellColor)
        ConfigToken("Colors")("Continuable Kernel Error Color") = If(New Color(ContKernelErrorColor).Type = ColorType.TrueColor, ContKernelErrorColor.EncloseByDoubleQuotes, ContKernelErrorColor)
        ConfigToken("Colors")("Uncontinuable Kernel Error Color") = If(New Color(UncontKernelErrorColor).Type = ColorType.TrueColor, UncontKernelErrorColor.EncloseByDoubleQuotes, UncontKernelErrorColor)
        ConfigToken("Colors")("Text Color") = If(New Color(NeutralTextColor).Type = ColorType.TrueColor, NeutralTextColor.EncloseByDoubleQuotes, NeutralTextColor)
        ConfigToken("Colors")("License Color") = If(New Color(LicenseColor).Type = ColorType.TrueColor, LicenseColor.EncloseByDoubleQuotes, LicenseColor)
        ConfigToken("Colors")("Background Color") = If(New Color(BackgroundColor).Type = ColorType.TrueColor, BackgroundColor.EncloseByDoubleQuotes, BackgroundColor)
        ConfigToken("Colors")("Input Color") = If(New Color(InputColor).Type = ColorType.TrueColor, InputColor.EncloseByDoubleQuotes, InputColor)
        ConfigToken("Colors")("List Entry Color") = If(New Color(ListEntryColor).Type = ColorType.TrueColor, ListEntryColor.EncloseByDoubleQuotes, ListEntryColor)
        ConfigToken("Colors")("List Value Color") = If(New Color(ListValueColor).Type = ColorType.TrueColor, ListValueColor.EncloseByDoubleQuotes, ListValueColor)
        ConfigToken("Colors")("Kernel Stage Color") = If(New Color(StageColor).Type = ColorType.TrueColor, StageColor.EncloseByDoubleQuotes, StageColor)
        ConfigToken("Colors")("Error Text Color") = If(New Color(ErrorColor).Type = ColorType.TrueColor, ErrorColor.EncloseByDoubleQuotes, ErrorColor)
        ConfigToken("Colors")("Warning Text Color") = If(New Color(WarningColor).Type = ColorType.TrueColor, WarningColor.EncloseByDoubleQuotes, WarningColor)
        ConfigToken("Colors")("Option Color") = If(New Color(OptionColor).Type = ColorType.TrueColor, OptionColor.EncloseByDoubleQuotes, OptionColor)
        ConfigToken("Colors")("Banner Color") = If(New Color(BannerColor).Type = ColorType.TrueColor, BannerColor.EncloseByDoubleQuotes, BannerColor)
        File.WriteAllText(paths("Configuration"), JsonConvert.SerializeObject(ConfigToken, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Sets custom colors. It only works if colored shell is enabled.
    ''' </summary>
    ''' <param name="InputColor">Input color</param>
    ''' <param name="LicenseColor">License color</param>
    ''' <param name="ContKernelErrorColor">Continuable kernel error color</param>
    ''' <param name="UncontKernelErrorColor">Uncontinuable kernel error color</param>
    ''' <param name="HostNameShellColor">Host name color</param>
    ''' <param name="UserNameShellColor">User name color</param>
    ''' <param name="BackgroundColor">Background color</param>
    ''' <param name="NeutralTextColor">Neutral text color</param>
    ''' <param name="ListEntryColor">Command list color</param>
    ''' <param name="ListValueColor">Command definition color</param>
    ''' <param name="StageColor">Stage color</param>
    ''' <param name="ErrorColor">Error color</param>
    ''' <param name="OptionColor">Option color</param>
    ''' <param name="BannerColor">Banner color</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Function SetColors(InputColor As String, LicenseColor As String, ContKernelErrorColor As String, UncontKernelErrorColor As String, HostNameShellColor As String, UserNameShellColor As String,
                              BackgroundColor As String, NeutralTextColor As String, ListEntryColor As String, ListValueColor As String, StageColor As String, ErrorColor As String, WarningColor As String,
                              OptionColor As String, BannerColor As String) As Boolean
        'Check colors for null and set them to "def" if found
        If String.IsNullOrEmpty(OptionColor) Then OptionColor = "def"
        If String.IsNullOrEmpty(WarningColor) Then WarningColor = "def"
        If String.IsNullOrEmpty(ErrorColor) Then ErrorColor = "def"
        If String.IsNullOrEmpty(StageColor) Then StageColor = "def"
        If String.IsNullOrEmpty(ListValueColor) Then ListValueColor = "def"
        If String.IsNullOrEmpty(ListEntryColor) Then ListEntryColor = "def"
        If String.IsNullOrEmpty(NeutralTextColor) Then NeutralTextColor = "def"
        If String.IsNullOrEmpty(BackgroundColor) Then BackgroundColor = "def"
        If String.IsNullOrEmpty(UserNameShellColor) Then UserNameShellColor = "def"
        If String.IsNullOrEmpty(HostNameShellColor) Then HostNameShellColor = "def"
        If String.IsNullOrEmpty(UncontKernelErrorColor) Then UncontKernelErrorColor = "def"
        If String.IsNullOrEmpty(ContKernelErrorColor) Then ContKernelErrorColor = "def"
        If String.IsNullOrEmpty(LicenseColor) Then LicenseColor = "def"
        If String.IsNullOrEmpty(InputColor) Then InputColor = "def"
        If String.IsNullOrEmpty(BannerColor) Then BannerColor = "def"

        'Set colors
        If ColoredShell = True Then
            'Check for defaults
            'We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
            If InputColor = "def" Then InputColor = New Color(ConsoleColors.White).PlainSequence
            If LicenseColor = "def" Then LicenseColor = New Color(ConsoleColors.White).PlainSequence
            If ContKernelErrorColor = "def" Then ContKernelErrorColor = New Color(ConsoleColors.Yellow).PlainSequence
            If UncontKernelErrorColor = "def" Then UncontKernelErrorColor = New Color(ConsoleColors.Red).PlainSequence
            If HostNameShellColor = "def" Then HostNameShellColor = New Color(ConsoleColors.DarkGreen).PlainSequence
            If UserNameShellColor = "def" Then UserNameShellColor = New Color(ConsoleColors.Green).PlainSequence
            If NeutralTextColor = "def" Then NeutralTextColor = New Color(ConsoleColors.Gray).PlainSequence
            If ListEntryColor = "def" Then ListEntryColor = New Color(ConsoleColors.DarkYellow).PlainSequence
            If ListValueColor = "def" Then ListValueColor = New Color(ConsoleColors.DarkGray).PlainSequence
            If StageColor = "def" Then StageColor = New Color(ConsoleColors.Green).PlainSequence
            If ErrorColor = "def" Then ErrorColor = New Color(ConsoleColors.Red).PlainSequence
            If WarningColor = "def" Then WarningColor = New Color(ConsoleColors.Yellow).PlainSequence
            If OptionColor = "def" Then OptionColor = New Color(ConsoleColors.DarkYellow).PlainSequence
            If BannerColor = "def" Then OptionColor = New Color(ConsoleColors.Green).PlainSequence
            If BackgroundColor = "def" Then
                BackgroundColor = New Color(ConsoleColors.Black).PlainSequence
                LoadBack()
            End If

            'Set the colors
            Try
                ColorTools.InputColor = New Color(InputColor).PlainSequence
                ColorTools.LicenseColor = New Color(LicenseColor).PlainSequence
                ColorTools.ContKernelErrorColor = New Color(ContKernelErrorColor).PlainSequence
                ColorTools.UncontKernelErrorColor = New Color(UncontKernelErrorColor).PlainSequence
                ColorTools.HostNameShellColor = New Color(HostNameShellColor).PlainSequence
                ColorTools.UserNameShellColor = New Color(UserNameShellColor).PlainSequence
                ColorTools.BackgroundColor = New Color(BackgroundColor).PlainSequence
                ColorTools.NeutralTextColor = New Color(NeutralTextColor).PlainSequence
                ColorTools.ListEntryColor = New Color(ListEntryColor).PlainSequence
                ColorTools.ListValueColor = New Color(ListValueColor).PlainSequence
                ColorTools.StageColor = New Color(StageColor).PlainSequence
                ColorTools.ErrorColor = New Color(ErrorColor).PlainSequence
                ColorTools.WarningColor = New Color(WarningColor).PlainSequence
                ColorTools.OptionColor = New Color(OptionColor).PlainSequence
                ColorTools.BannerColor = New Color(BannerColor).PlainSequence
                LoadBack()
                MakePermanent()

                'Raise event
                EventManager.RaiseColorSet()
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                EventManager.RaiseColorSetError("invalidcolors")
                Throw New Exceptions.ColorException(DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message)
            End Try
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
        If ColoredShell = True Then
            SetConsoleColor(New Color(InputColor))
            SetConsoleColor(New Color(BackgroundColor), True)
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Sets the console color
    ''' </summary>
    ''' <param name="ColorSequence">The color instance</param>
    ''' <param name="Background">Whether to set background or not</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SetConsoleColor(ByVal ColorSequence As Color, Optional ByVal Background As Boolean = False) As Boolean
        If ColoredShell Then
            If ColorSequence Is Nothing Then Throw New ArgumentNullException(NameOf(ColorSequence))
            Dim OldLeft As Integer = Console.CursorLeft
            Dim OldTop As Integer = Console.CursorTop
            If Background Then
                Console.Write(ColorSequence.VTSequenceBackground)
                If IsOnUnix() Then
                    'Restore the CursorLeft value to its correct value in Mono. This is a workaround to fix incorrect Console.CursorLeft value.
                    Console.SetCursorPosition(OldLeft, OldTop)
                End If
            Else
                Console.Write(ColorSequence.VTSequenceForeground)
                If IsOnUnix() Then
                    'Restore the CursorLeft value to its correct value in Mono. This is a workaround to fix incorrect Console.CursorLeft value.
                    Console.SetCursorPosition(OldLeft, OldTop)
                End If
            End If
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    Public Function ColorWheel(ByVal TrueColor As Boolean) As String
        Return ColorWheel(TrueColor, ConsoleColors.White, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColor">The default 255-color to use</param>
    Public Function ColorWheel(ByVal TrueColor As Boolean, ByVal DefaultColor As ConsoleColors) As String
        Return ColorWheel(TrueColor, DefaultColor, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColorR">The default red color range of 0-255 to use</param>
    ''' <param name="DefaultColorG">The default green color range of 0-255 to use</param>
    ''' <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
    Public Function ColorWheel(ByVal TrueColor As Boolean, ByVal DefaultColorR As Integer, ByVal DefaultColorG As Integer, ByVal DefaultColorB As Integer) As String
        Return ColorWheel(TrueColor, ConsoleColors.White, DefaultColorR, DefaultColorG, DefaultColorB)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColor">The default 255-color to use</param>
    ''' <param name="DefaultColorR">The default red color range of 0-255 to use</param>
    ''' <param name="DefaultColorG">The default green color range of 0-255 to use</param>
    ''' <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
    Public Function ColorWheel(ByVal TrueColor As Boolean, ByVal DefaultColor As ConsoleColors, ByVal DefaultColorR As Integer, ByVal DefaultColorG As Integer, ByVal DefaultColorB As Integer) As String
        Dim CurrentColor As ConsoleColors = DefaultColor
        Dim CurrentColorR As Integer = DefaultColorR
        Dim CurrentColorG As Integer = DefaultColorG
        Dim CurrentColorB As Integer = DefaultColorB
        Dim CurrentRange As Char = "R"
        Dim ColorWheelExiting As Boolean
        Console.CursorVisible = False
        While Not ColorWheelExiting
            Console.Clear()
            If TrueColor Then
                W(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Neutral)
                W(DoTranslation("Press ""t"" to switch to 255 color mode."), True, ColTypes.Neutral)

                'The red color level
                W(vbNewLine + " <", False, If(CurrentRange = "R", ColTypes.Gray, ColTypes.Neutral))
                WriteWhereC("R: {0}", (Console.CursorLeft + 30 - $"R: {CurrentColorR}".Length) / 2, Console.CursorTop, True, New Color($"{CurrentColorR};0;0"), CurrentColorR)
                WriteWhere(">" + vbNewLine, Console.CursorLeft + 27, Console.CursorTop, False, If(CurrentRange = "R", ColTypes.Gray, ColTypes.Neutral))

                'The green color level
                W(vbNewLine + " <", False, If(CurrentRange = "G", ColTypes.Gray, ColTypes.Neutral))
                WriteWhereC("G: {0}", (Console.CursorLeft + 30 - $"G: {CurrentColorG}".Length) / 2, Console.CursorTop, True, New Color($"0;{CurrentColorG};0"), CurrentColorG)
                WriteWhere(">" + vbNewLine, Console.CursorLeft + 27, Console.CursorTop, False, If(CurrentRange = "G", ColTypes.Gray, ColTypes.Neutral))

                'The blue color level
                W(vbNewLine + " <", False, If(CurrentRange = "B", ColTypes.Gray, ColTypes.Neutral))
                WriteWhereC("B: {0}", (Console.CursorLeft + 30 - $"B: {CurrentColorB}".Length) / 2, Console.CursorTop, True, New Color($"0;0;{CurrentColorB}"), CurrentColorB)
                WriteWhere(">" + vbNewLine, Console.CursorLeft + 27, Console.CursorTop, False, If(CurrentRange = "B", ColTypes.Gray, ColTypes.Neutral))

                'Show example
                WriteC(vbNewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, New Color($"{CurrentColorR};{CurrentColorG};{CurrentColorB}"))

                'Read and get response
                Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey(True)
                If ConsoleResponse.Key = ConsoleKey.LeftArrow Then
                    Select Case CurrentRange
                        Case "R"
                            If CurrentColorR = 0 Then
                                CurrentColorR = 255
                            Else
                                CurrentColorR -= 1
                            End If
                        Case "G"
                            If CurrentColorG = 0 Then
                                CurrentColorG = 255
                            Else
                                CurrentColorG -= 1
                            End If
                        Case "B"
                            If CurrentColorB = 0 Then
                                CurrentColorB = 255
                            Else
                                CurrentColorB -= 1
                            End If
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.RightArrow Then
                    Select Case CurrentRange
                        Case "R"
                            If CurrentColorR = 255 Then
                                CurrentColorR = 0
                            Else
                                CurrentColorR += 1
                            End If
                        Case "G"
                            If CurrentColorG = 255 Then
                                CurrentColorG = 0
                            Else
                                CurrentColorG += 1
                            End If
                        Case "B"
                            If CurrentColorB = 255 Then
                                CurrentColorB = 0
                            Else
                                CurrentColorB += 1
                            End If
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.UpArrow Then
                    Select Case CurrentRange
                        Case "R"
                            CurrentRange = "B"
                        Case "G"
                            CurrentRange = "R"
                        Case "B"
                            CurrentRange = "G"
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.DownArrow Then
                    Select Case CurrentRange
                        Case "R"
                            CurrentRange = "G"
                        Case "G"
                            CurrentRange = "B"
                        Case "B"
                            CurrentRange = "R"
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.I Then
                    Dim _DefaultColor As Integer
                    Select Case CurrentRange
                        Case "R"
                            _DefaultColor = CurrentColorR
                        Case "G"
                            _DefaultColor = CurrentColorG
                        Case "B"
                            _DefaultColor = CurrentColorB
                    End Select
                    WriteWhere(DoTranslation("Enter color number from 0 to 255:") + " [{0}] ", 0, Console.WindowHeight - 1, False, ColTypes.Input, _DefaultColor)
                    Dim ColorNum As String = Console.ReadLine
                    If IsNumeric(ColorNum) Then
                        If ColorNum >= 0 And ColorNum <= 255 Then
                            Select Case CurrentRange
                                Case "R"
                                    CurrentColorR = ColorNum
                                Case "G"
                                    CurrentColorG = ColorNum
                                Case "B"
                                    CurrentColorB = ColorNum
                            End Select
                        End If
                    End If
                ElseIf ConsoleResponse.Key = ConsoleKey.T Then
                    TrueColor = False
                ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                    ColorWheelExiting = True
                End If
            Else
                W(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Use arrow up and arrow down keys to select between color ranges. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Neutral)
                W(DoTranslation("Press ""t"" to switch to true color mode."), True, ColTypes.Neutral)

                'The color selection
                W(vbNewLine + " <", False, ColTypes.Gray)
                WriteWhereC(CurrentColor.ToString, (Console.CursorLeft + 30 - CurrentColor.ToString.Length) / 2, Console.CursorTop, True, New Color(CurrentColor))
                WriteWhere(">", Console.CursorLeft + 27, Console.CursorTop, False, ColTypes.Gray)

                'Show prompt
                WriteC(vbNewLine + vbNewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, New Color(CurrentColor))

                'Read and get response
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
                ElseIf ConsoleResponse.Key = ConsoleKey.T Then
                    TrueColor = True
                ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                    ColorWheelExiting = True
                End If
            End If
        End While

        If TrueColor Then
            Return $"{CurrentColorR};{CurrentColorG};{CurrentColorB}"
        Else
            Return CurrentColor
        End If
    End Function

End Module
