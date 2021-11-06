
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
        ''' <summary>
        ''' Notification title text
        ''' </summary>
        NotificationTitle
        ''' <summary>
        ''' Notification description text
        ''' </summary>
        NotificationDescription
        ''' <summary>
        ''' Notification progress text
        ''' </summary>
        NotificationProgress
        ''' <summary>
        ''' Notification failure text
        ''' </summary>
        NotificationFailure
        ''' <summary>
        ''' Question text
        ''' </summary>
        Question
        ''' <summary>
        ''' Success text
        ''' </summary>
        Success
        ''' <summary>
        ''' User dollar sign on shell text
        ''' </summary>
        UserDollarSign
        ''' <summary>
        ''' Tip text
        ''' </summary>
        Tip
        ''' <summary>
        ''' Separator text
        ''' </summary>
        SeparatorText
        ''' <summary>
        ''' Separator color
        ''' </summary>
        Separator
        ''' <summary>
        ''' List title text
        ''' </summary>
        ListTitle
        ''' <summary>
        ''' Development warning text
        ''' </summary>
        DevelopmentWarning
        ''' <summary>
        ''' Stage time text
        ''' </summary>
        StageTime
        ''' <summary>
        ''' General progress text
        ''' </summary>
        Progress
        ''' <summary>
        ''' Back option text
        ''' </summary>
        BackOption
        ''' <summary>
        ''' Low priority notification border color
        ''' </summary>
        LowPriorityBorder
        ''' <summary>
        ''' Medium priority notification border color
        ''' </summary>
        MediumPriorityBorder
        ''' <summary>
        ''' High priority notification border color
        ''' </summary>
        HighPriorityBorder
        ''' <summary>
        ''' Table separator
        ''' </summary>
        TableSeparator
        ''' <summary>
        ''' Table header
        ''' </summary>
        TableHeader
        ''' <summary>
        ''' Table value
        ''' </summary>
        TableValue
        ''' <summary>
        ''' Selected option
        ''' </summary>
        SelectedOption
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
    Public NotificationTitleColor As String = New Color(ConsoleColors.White).PlainSequence
    Public NotificationDescriptionColor As String = New Color(ConsoleColors.Gray).PlainSequence
    Public NotificationProgressColor As String = New Color(ConsoleColors.DarkYellow).PlainSequence
    Public NotificationFailureColor As String = New Color(ConsoleColors.Red).PlainSequence
    Public QuestionColor As String = New Color(ConsoleColors.Yellow).PlainSequence
    Public SuccessColor As String = New Color(ConsoleColors.Green).PlainSequence
    Public UserDollarColor As String = New Color(ConsoleColors.Gray).PlainSequence
    Public TipColor As String = New Color(ConsoleColors.Gray).PlainSequence
    Public SeparatorTextColor As String = New Color(ConsoleColors.White).PlainSequence
    Public SeparatorColor As String = New Color(ConsoleColors.Gray).PlainSequence
    Public ListTitleColor As String = New Color(ConsoleColors.White).PlainSequence
    Public DevelopmentWarningColor As String = New Color(ConsoleColors.Yellow).PlainSequence
    Public StageTimeColor As String = New Color(ConsoleColors.Gray).PlainSequence
    Public ProgressColor As String = New Color(ConsoleColors.DarkYellow).PlainSequence
    Public BackOptionColor As String = New Color(ConsoleColors.DarkRed).PlainSequence
    Public LowPriorityBorderColor As String = New Color(ConsoleColors.White).PlainSequence
    Public MediumPriorityBorderColor As String = New Color(ConsoleColors.Yellow).PlainSequence
    Public HighPriorityBorderColor As String = New Color(ConsoleColors.Red).PlainSequence
    Public TableSeparatorColor As String = New Color(ConsoleColors.DarkGray).PlainSequence
    Public TableHeaderColor As String = New Color(ConsoleColors.White).PlainSequence
    Public TableValueColor As String = New Color(ConsoleColors.Gray).PlainSequence
    Public SelectedOptionColor As String = New Color(ConsoleColors.Yellow).PlainSequence

    ''' <summary>
    ''' Resets all colors to default
    ''' </summary>
    Public Sub ResetColors()
        Wdbg(DebugLevel.I, "Resetting colors")
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
        NotificationTitleColor = DefInfo.ThemeNotificationTitleColor.PlainSequence
        NotificationDescriptionColor = DefInfo.ThemeNotificationDescriptionColor.PlainSequence
        NotificationProgressColor = DefInfo.ThemeNotificationProgressColor.PlainSequence
        NotificationFailureColor = DefInfo.ThemeNotificationFailureColor.PlainSequence
        QuestionColor = DefInfo.ThemeQuestionColor.PlainSequence
        SuccessColor = DefInfo.ThemeSuccessColor.PlainSequence
        UserDollarColor = DefInfo.ThemeUserDollarColor.PlainSequence
        TipColor = DefInfo.ThemeTipColor.PlainSequence
        SeparatorTextColor = DefInfo.ThemeSeparatorTextColor.PlainSequence
        SeparatorColor = DefInfo.ThemeSeparatorColor.PlainSequence
        ListTitleColor = DefInfo.ThemeListTitleColor.PlainSequence
        DevelopmentWarningColor = DefInfo.ThemeDevelopmentWarningColor.PlainSequence
        StageTimeColor = DefInfo.ThemeStageTimeColor.PlainSequence
        ProgressColor = DefInfo.ThemeProgressColor.PlainSequence
        BackOptionColor = DefInfo.ThemeBackOptionColor.PlainSequence
        LowPriorityBorderColor = DefInfo.ThemeLowPriorityBorderColor.PlainSequence
        MediumPriorityBorderColor = DefInfo.ThemeMediumPriorityBorderColor.PlainSequence
        HighPriorityBorderColor = DefInfo.ThemeHighPriorityBorderColor.PlainSequence
        TableSeparatorColor = DefInfo.ThemeTableSeparatorColor.PlainSequence
        TableHeaderColor = DefInfo.ThemeTableHeaderColor.PlainSequence
        TableValueColor = DefInfo.ThemeTableValueColor.PlainSequence
        SelectedOptionColor = DefInfo.ThemeSelectedOptionColor.PlainSequence
        LoadBack()

        'Raise event
        Kernel.EventManager.RaiseColorReset()
    End Sub

    ''' <summary>
    ''' Loads the background
    ''' </summary>
    Public Sub LoadBack()
        Try
            Wdbg(DebugLevel.I, "Filling background with background color")
            SetConsoleColor(New Color(BackgroundColor), True)
            Console.Clear()
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Failed to set background: {0}", ex.Message)
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
        ConfigToken("Colors")("Notification Title Color") = If(New Color(NotificationTitleColor).Type = ColorType.TrueColor, NotificationTitleColor.EncloseByDoubleQuotes, NotificationTitleColor)
        ConfigToken("Colors")("Notification Description Color") = If(New Color(NotificationDescriptionColor).Type = ColorType.TrueColor, NotificationDescriptionColor.EncloseByDoubleQuotes, NotificationDescriptionColor)
        ConfigToken("Colors")("Notification Progress Color") = If(New Color(NotificationProgressColor).Type = ColorType.TrueColor, NotificationProgressColor.EncloseByDoubleQuotes, NotificationProgressColor)
        ConfigToken("Colors")("Notification Failure Color") = If(New Color(NotificationFailureColor).Type = ColorType.TrueColor, NotificationFailureColor.EncloseByDoubleQuotes, NotificationFailureColor)
        ConfigToken("Colors")("Question Color") = If(New Color(QuestionColor).Type = ColorType.TrueColor, QuestionColor.EncloseByDoubleQuotes, QuestionColor)
        ConfigToken("Colors")("Success Color") = If(New Color(SuccessColor).Type = ColorType.TrueColor, SuccessColor.EncloseByDoubleQuotes, SuccessColor)
        ConfigToken("Colors")("User Dollar Color") = If(New Color(UserDollarColor).Type = ColorType.TrueColor, UserDollarColor.EncloseByDoubleQuotes, UserDollarColor)
        ConfigToken("Colors")("Tip Color") = If(New Color(TipColor).Type = ColorType.TrueColor, TipColor.EncloseByDoubleQuotes, TipColor)
        ConfigToken("Colors")("Separator Text Color") = If(New Color(SeparatorTextColor).Type = ColorType.TrueColor, SeparatorTextColor.EncloseByDoubleQuotes, SeparatorTextColor)
        ConfigToken("Colors")("Separator Color") = If(New Color(SeparatorColor).Type = ColorType.TrueColor, SeparatorColor.EncloseByDoubleQuotes, SeparatorColor)
        ConfigToken("Colors")("List Title Color") = If(New Color(ListTitleColor).Type = ColorType.TrueColor, ListTitleColor.EncloseByDoubleQuotes, ListTitleColor)
        ConfigToken("Colors")("Development Warning Color") = If(New Color(DevelopmentWarningColor).Type = ColorType.TrueColor, DevelopmentWarningColor.EncloseByDoubleQuotes, DevelopmentWarningColor)
        ConfigToken("Colors")("Stage Time Color") = If(New Color(StageTimeColor).Type = ColorType.TrueColor, StageTimeColor.EncloseByDoubleQuotes, StageTimeColor)
        ConfigToken("Colors")("Progress Color") = If(New Color(ProgressColor).Type = ColorType.TrueColor, ProgressColor.EncloseByDoubleQuotes, ProgressColor)
        ConfigToken("Colors")("Back Option Color") = If(New Color(BackOptionColor).Type = ColorType.TrueColor, BackOptionColor.EncloseByDoubleQuotes, BackOptionColor)
        ConfigToken("Colors")("Low Priority Border Color") = If(New Color(LowPriorityBorderColor).Type = ColorType.TrueColor, LowPriorityBorderColor.EncloseByDoubleQuotes, LowPriorityBorderColor)
        ConfigToken("Colors")("Medium Priority Border Color") = If(New Color(MediumPriorityBorderColor).Type = ColorType.TrueColor, MediumPriorityBorderColor.EncloseByDoubleQuotes, MediumPriorityBorderColor)
        ConfigToken("Colors")("High Priority Border Color") = If(New Color(HighPriorityBorderColor).Type = ColorType.TrueColor, HighPriorityBorderColor.EncloseByDoubleQuotes, HighPriorityBorderColor)
        ConfigToken("Colors")("Table Separator Color") = If(New Color(TableSeparatorColor).Type = ColorType.TrueColor, TableSeparatorColor.EncloseByDoubleQuotes, TableSeparatorColor)
        ConfigToken("Colors")("Table Header Color") = If(New Color(TableHeaderColor).Type = ColorType.TrueColor, TableHeaderColor.EncloseByDoubleQuotes, TableHeaderColor)
        ConfigToken("Colors")("Table Value Color") = If(New Color(TableValueColor).Type = ColorType.TrueColor, TableValueColor.EncloseByDoubleQuotes, TableValueColor)
        ConfigToken("Colors")("Selected Option Color") = If(New Color(SelectedOptionColor).Type = ColorType.TrueColor, SelectedOptionColor.EncloseByDoubleQuotes, SelectedOptionColor)
        File.WriteAllText(GetKernelPath(KernelPathType.Configuration), JsonConvert.SerializeObject(ConfigToken, Formatting.Indented))
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
    ''' <param name="WarningColor">Warning color</param>
    ''' <param name="OptionColor">Option color</param>
    ''' <param name="BannerColor">Banner color</param>
    ''' <param name="NotificationTitleColor">Notification title color</param>
    ''' <param name="NotificationDescriptionColor">Notification description color</param>
    ''' <param name="NotificationProgressColor">Notification progress color</param>
    ''' <param name="NotificationFailureColor">Notification failure color</param>
    ''' <param name="QuestionColor">Question color</param>
    ''' <param name="SuccessColor">Success text color</param>
    ''' <param name="UserDollarColor">User dollar color</param>
    ''' <param name="TipColor">Tip color</param>
    ''' <param name="SeparatorTextColor">Separator text color</param>
    ''' <param name="SeparatorColor">Separator color</param>
    ''' <param name="ListTitleColor">List title color</param>
    ''' <param name="DevelopmentWarningColor">Development warning color</param>
    ''' <param name="StageTimeColor">Stage time color</param>
    ''' <param name="ProgressColor">Progress color</param>
    ''' <param name="BackOptionColor">Back option color</param>
    ''' <param name="LowPriorityBorderColor">Low priority notification border color</param>
    ''' <param name="MediumPriorityBorderColor">Medium priority notification border color</param>
    ''' <param name="HighPriorityBorderColor">High priority notification border color</param>
    ''' <param name="TableSeparatorColor">Table separator color</param>
    ''' <param name="TableHeaderColor">Table header color</param>
    ''' <param name="TableValueColor">Table value color</param>
    ''' <param name="SelectedOptionColor">Selected option color</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Function SetColors(InputColor As String, LicenseColor As String, ContKernelErrorColor As String, UncontKernelErrorColor As String, HostNameShellColor As String, UserNameShellColor As String,
                              BackgroundColor As String, NeutralTextColor As String, ListEntryColor As String, ListValueColor As String, StageColor As String, ErrorColor As String, WarningColor As String,
                              OptionColor As String, BannerColor As String, NotificationTitleColor As String, NotificationDescriptionColor As String, NotificationProgressColor As String,
                              NotificationFailureColor As String, QuestionColor As String, SuccessColor As String, UserDollarColor As String, TipColor As String, SeparatorTextColor As String,
                              SeparatorColor As String, ListTitleColor As String, DevelopmentWarningColor As String, StageTimeColor As String, ProgressColor As String, BackOptionColor As String,
                              LowPriorityBorderColor As String, MediumPriorityBorderColor As String, HighPriorityBorderColor As String, TableSeparatorColor As String, TableHeaderColor As String,
                              TableValueColor As String, SelectedOptionColor As String) As Boolean
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
        If String.IsNullOrEmpty(NotificationTitleColor) Then NotificationTitleColor = "def"
        If String.IsNullOrEmpty(NotificationDescriptionColor) Then NotificationDescriptionColor = "def"
        If String.IsNullOrEmpty(NotificationProgressColor) Then NotificationProgressColor = "def"
        If String.IsNullOrEmpty(NotificationFailureColor) Then NotificationFailureColor = "def"
        If String.IsNullOrEmpty(QuestionColor) Then QuestionColor = "def"
        If String.IsNullOrEmpty(SuccessColor) Then SuccessColor = "def"
        If String.IsNullOrEmpty(UserDollarColor) Then UserDollarColor = "def"
        If String.IsNullOrEmpty(TipColor) Then TipColor = "def"
        If String.IsNullOrEmpty(SeparatorTextColor) Then SeparatorTextColor = "def"
        If String.IsNullOrEmpty(SeparatorColor) Then SeparatorColor = "def"
        If String.IsNullOrEmpty(ListTitleColor) Then ListTitleColor = "def"
        If String.IsNullOrEmpty(DevelopmentWarningColor) Then DevelopmentWarningColor = "def"
        If String.IsNullOrEmpty(StageTimeColor) Then StageTimeColor = "def"
        If String.IsNullOrEmpty(ProgressColor) Then ProgressColor = "def"
        If String.IsNullOrEmpty(BackOptionColor) Then BackOptionColor = "def"
        If String.IsNullOrEmpty(LowPriorityBorderColor) Then LowPriorityBorderColor = "def"
        If String.IsNullOrEmpty(MediumPriorityBorderColor) Then MediumPriorityBorderColor = "def"
        If String.IsNullOrEmpty(HighPriorityBorderColor) Then HighPriorityBorderColor = "def"
        If String.IsNullOrEmpty(TableSeparatorColor) Then TableSeparatorColor = "def"
        If String.IsNullOrEmpty(TableHeaderColor) Then TableHeaderColor = "def"
        If String.IsNullOrEmpty(TableValueColor) Then TableValueColor = "def"
        If String.IsNullOrEmpty(SelectedOptionColor) Then SelectedOptionColor = "def"

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
            If NotificationTitleColor = "def" Then NotificationTitleColor = New Color(ConsoleColors.White).PlainSequence
            If NotificationDescriptionColor = "def" Then NotificationDescriptionColor = New Color(ConsoleColors.Gray).PlainSequence
            If NotificationProgressColor = "def" Then NotificationProgressColor = New Color(ConsoleColors.DarkYellow).PlainSequence
            If NotificationFailureColor = "def" Then NotificationFailureColor = New Color(ConsoleColors.Red).PlainSequence
            If QuestionColor = "def" Then QuestionColor = New Color(ConsoleColors.Yellow).PlainSequence
            If SuccessColor = "def" Then SuccessColor = New Color(ConsoleColors.Green).PlainSequence
            If UserDollarColor = "def" Then UserDollarColor = New Color(ConsoleColors.Gray).PlainSequence
            If TipColor = "def" Then TipColor = New Color(ConsoleColors.Gray).PlainSequence
            If SeparatorTextColor = "def" Then SeparatorTextColor = New Color(ConsoleColors.White).PlainSequence
            If SeparatorColor = "def" Then SeparatorColor = New Color(ConsoleColors.Gray).PlainSequence
            If ListTitleColor = "def" Then ListTitleColor = New Color(ConsoleColors.White).PlainSequence
            If DevelopmentWarningColor = "def" Then DevelopmentWarningColor = New Color(ConsoleColors.Yellow).PlainSequence
            If StageTimeColor = "def" Then StageTimeColor = New Color(ConsoleColors.Gray).PlainSequence
            If ProgressColor = "def" Then ProgressColor = New Color(ConsoleColors.DarkYellow).PlainSequence
            If BackOptionColor = "def" Then BackOptionColor = New Color(ConsoleColors.DarkRed).PlainSequence
            If LowPriorityBorderColor = "def" Then LowPriorityBorderColor = New Color(ConsoleColors.White).PlainSequence
            If MediumPriorityBorderColor = "def" Then MediumPriorityBorderColor = New Color(ConsoleColors.Yellow).PlainSequence
            If HighPriorityBorderColor = "def" Then HighPriorityBorderColor = New Color(ConsoleColors.Red).PlainSequence
            If TableSeparatorColor = "def" Then TableSeparatorColor = New Color(ConsoleColors.DarkGray).PlainSequence
            If TableHeaderColor = "def" Then TableHeaderColor = New Color(ConsoleColors.White).PlainSequence
            If TableValueColor = "def" Then TableValueColor = New Color(ConsoleColors.Gray).PlainSequence
            If SelectedOptionColor = "def" Then OptionColor = New Color(ConsoleColors.Yellow).PlainSequence
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
                ColorTools.NotificationTitleColor = New Color(NotificationTitleColor).PlainSequence
                ColorTools.NotificationDescriptionColor = New Color(NotificationDescriptionColor).PlainSequence
                ColorTools.NotificationProgressColor = New Color(NotificationProgressColor).PlainSequence
                ColorTools.NotificationFailureColor = New Color(NotificationFailureColor).PlainSequence
                ColorTools.QuestionColor = New Color(QuestionColor).PlainSequence
                ColorTools.SuccessColor = New Color(SuccessColor).PlainSequence
                ColorTools.UserDollarColor = New Color(UserDollarColor).PlainSequence
                ColorTools.TipColor = New Color(TipColor).PlainSequence
                ColorTools.SeparatorTextColor = New Color(SeparatorTextColor).PlainSequence
                ColorTools.SeparatorColor = New Color(SeparatorColor).PlainSequence
                ColorTools.ListTitleColor = New Color(ListTitleColor).PlainSequence
                ColorTools.DevelopmentWarningColor = New Color(DevelopmentWarningColor).PlainSequence
                ColorTools.StageTimeColor = New Color(StageTimeColor).PlainSequence
                ColorTools.ProgressColor = New Color(ProgressColor).PlainSequence
                ColorTools.BackOptionColor = New Color(BackOptionColor).PlainSequence
                ColorTools.LowPriorityBorderColor = New Color(LowPriorityBorderColor).PlainSequence
                ColorTools.MediumPriorityBorderColor = New Color(MediumPriorityBorderColor).PlainSequence
                ColorTools.HighPriorityBorderColor = New Color(HighPriorityBorderColor).PlainSequence
                ColorTools.TableSeparatorColor = New Color(TableSeparatorColor).PlainSequence
                ColorTools.TableHeaderColor = New Color(TableHeaderColor).PlainSequence
                ColorTools.TableValueColor = New Color(TableValueColor).PlainSequence
                ColorTools.SelectedOptionColor = New Color(SelectedOptionColor).PlainSequence
                LoadBack()
                MakePermanent()

                'Raise event
                Kernel.EventManager.RaiseColorSet()
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Kernel.EventManager.RaiseColorSetError(ColorSetErrorReasons.InvalidColors)
                Throw New Exceptions.ColorException(DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message)
            End Try
        Else
            Kernel.EventManager.RaiseColorSetError(ColorSetErrorReasons.NoColors)
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
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SetConsoleColor(colorType As ColTypes) As Boolean
        If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
            Select Case colorType
                Case ColTypes.Neutral
                    SetConsoleColor(New Color(NeutralTextColor))
                Case ColTypes.Continuable
                    SetConsoleColor(New Color(ContKernelErrorColor))
                Case ColTypes.Uncontinuable
                    SetConsoleColor(New Color(UncontKernelErrorColor))
                Case ColTypes.HostName
                    SetConsoleColor(New Color(HostNameShellColor))
                Case ColTypes.UserName
                    SetConsoleColor(New Color(UserNameShellColor))
                Case ColTypes.License
                    SetConsoleColor(New Color(LicenseColor))
                Case ColTypes.Gray
                    If New Color(BackgroundColor).IsBright Then
                        SetConsoleColor(New Color(NeutralTextColor))
                    Else
                        SetConsoleColor(New Color(ConsoleColors.Gray))
                    End If
                Case ColTypes.ListValue
                    SetConsoleColor(New Color(ListValueColor))
                Case ColTypes.ListEntry
                    SetConsoleColor(New Color(ListEntryColor))
                Case ColTypes.Stage
                    SetConsoleColor(New Color(StageColor))
                Case ColTypes.Error
                    SetConsoleColor(New Color(ErrorColor))
                Case ColTypes.Warning
                    SetConsoleColor(New Color(WarningColor))
                Case ColTypes.Option
                    SetConsoleColor(New Color(OptionColor))
                Case ColTypes.Banner
                    SetConsoleColor(New Color(BannerColor))
                Case ColTypes.NotificationTitle
                    SetConsoleColor(New Color(NotificationTitleColor))
                Case ColTypes.NotificationDescription
                    SetConsoleColor(New Color(NotificationDescriptionColor))
                Case ColTypes.NotificationProgress
                    SetConsoleColor(New Color(NotificationProgressColor))
                Case ColTypes.NotificationFailure
                    SetConsoleColor(New Color(NotificationFailureColor))
                Case ColTypes.Question, ColTypes.Input
                    SetConsoleColor(New Color(QuestionColor))
                Case ColTypes.Success
                    SetConsoleColor(New Color(SuccessColor))
                Case ColTypes.UserDollarSign
                    SetConsoleColor(New Color(UserDollarColor))
                Case ColTypes.Tip
                    SetConsoleColor(New Color(TipColor))
                Case ColTypes.SeparatorText
                    SetConsoleColor(New Color(SeparatorTextColor))
                Case ColTypes.Separator
                    SetConsoleColor(New Color(SeparatorColor))
                Case ColTypes.ListTitle
                    SetConsoleColor(New Color(ListTitleColor))
                Case ColTypes.DevelopmentWarning
                    SetConsoleColor(New Color(DevelopmentWarningColor))
                Case ColTypes.StageTime
                    SetConsoleColor(New Color(StageTimeColor))
                Case ColTypes.Progress
                    SetConsoleColor(New Color(ProgressColor))
                Case ColTypes.BackOption
                    SetConsoleColor(New Color(BackOptionColor))
                Case ColTypes.LowPriorityBorder
                    SetConsoleColor(New Color(LowPriorityBorderColor))
                Case ColTypes.MediumPriorityBorder
                    SetConsoleColor(New Color(MediumPriorityBorderColor))
                Case ColTypes.HighPriorityBorder
                    SetConsoleColor(New Color(HighPriorityBorderColor))
                Case ColTypes.TableSeparator
                    SetConsoleColor(New Color(TableSeparatorColor))
                Case ColTypes.TableHeader
                    SetConsoleColor(New Color(TableHeaderColor))
                Case ColTypes.TableValue
                    SetConsoleColor(New Color(TableValueColor))
                Case ColTypes.SelectedOption
                    SetConsoleColor(New Color(SelectedOptionColor))
                Case Else
                    Exit Select
            End Select
            SetConsoleColor(New Color(BackgroundColor), True)
        End If
        Return True
    End Function

    ''' <summary>
    ''' Sets the console color
    ''' </summary>
    ''' <param name="ColorSequence">The color instance</param>
    ''' <param name="Background">Whether to set background or not</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function SetConsoleColor(ColorSequence As Color, Optional Background As Boolean = False) As Boolean
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
    Public Function ColorWheel() As String
        Return ColorWheel(False, ConsoleColors.White, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    Public Function ColorWheel(TrueColor As Boolean) As String
        Return ColorWheel(TrueColor, ConsoleColors.White, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColor">The default 255-color to use</param>
    Public Function ColorWheel(TrueColor As Boolean, DefaultColor As ConsoleColors) As String
        Return ColorWheel(TrueColor, DefaultColor, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColorR">The default red color range of 0-255 to use</param>
    ''' <param name="DefaultColorG">The default green color range of 0-255 to use</param>
    ''' <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
    Public Function ColorWheel(TrueColor As Boolean, DefaultColorR As Integer, DefaultColorG As Integer, DefaultColorB As Integer) As String
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
    Public Function ColorWheel(TrueColor As Boolean, DefaultColor As ConsoleColors, DefaultColorR As Integer, DefaultColorG As Integer, DefaultColorB As Integer) As String
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
                Write(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Tip)
                Write(DoTranslation("Press ""t"" to switch to 255 color mode."), True, ColTypes.Tip)
                Write(DoTranslation("Press ""c"" to write full color code."), True, ColTypes.Tip)

                'The red color level
                Dim RedForeground As Color = If(CurrentRange = "R", New Color(ConsoleColors.Black), New Color("255;0;0"))
                Dim RedBackground As Color = If(CurrentRange = "R", New Color("255;0;0"), New Color(ConsoleColors.Black))
                Write(vbNewLine + "  ", False, ColTypes.Neutral)
                Write(" < ", False, RedForeground, RedBackground)
                WriteWhere("R: {0}", (Console.CursorLeft + 35 - $"R: {CurrentColorR}".Length) / 2, Console.CursorTop, True, New Color($"{CurrentColorR};0;0"), CurrentColorR)
                WriteWhere(" > " + vbNewLine, Console.CursorLeft + 32, Console.CursorTop, False, RedForeground, RedBackground)

                'The green color level
                Dim GreenForeground As Color = If(CurrentRange = "G", New Color(ConsoleColors.Black), New Color("0;255;0"))
                Dim GreenBackground As Color = If(CurrentRange = "G", New Color("0;255;0"), New Color(ConsoleColors.Black))
                Write(vbNewLine + "  ", False, ColTypes.Neutral)
                Write(" < ", False, GreenForeground, GreenBackground)
                WriteWhere("G: {0}", (Console.CursorLeft + 35 - $"G: {CurrentColorG}".Length) / 2, Console.CursorTop, True, New Color($"0;{CurrentColorG};0"), CurrentColorG)
                WriteWhere(" > " + vbNewLine, Console.CursorLeft + 32, Console.CursorTop, False, GreenForeground, GreenBackground)

                'The blue color level
                Dim BlueForeground As Color = If(CurrentRange = "B", New Color(ConsoleColors.Black), New Color("0;0;255"))
                Dim BlueBackground As Color = If(CurrentRange = "B", New Color("0;0;255"), New Color(ConsoleColors.Black))
                Write(vbNewLine + "  ", False, ColTypes.Neutral)
                Write(" < ", False, BlueForeground, BlueBackground)
                WriteWhere("B: {0}", (Console.CursorLeft + 35 - $"B: {CurrentColorB}".Length) / 2, Console.CursorTop, True, New Color($"0;0;{CurrentColorB}"), CurrentColorB)
                WriteWhere(" > " + vbNewLine, Console.CursorLeft + 32, Console.CursorTop, False, BlueForeground, BlueBackground)

                'Show example
                Write(vbNewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, New Color($"{CurrentColorR};{CurrentColorG};{CurrentColorB}"))

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
                    Console.CursorVisible = True
                    Dim ColorNum As String = Console.ReadLine
                    Console.CursorVisible = False
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
                ElseIf ConsoleResponse.Key = ConsoleKey.C Then
                    WriteWhere(DoTranslation("Enter color code that satisfies these formats:") + " ""RRR;GGG;BBB"" / 0-255 [{0}] ", 0, Console.WindowHeight - 1, False, ColTypes.Input, $"{CurrentColorR};{CurrentColorG};{CurrentColorB}")
                    Console.CursorVisible = True
                    Dim ColorSequence As String = Console.ReadLine
                    Console.CursorVisible = False
                    Try
                        Dim ParsedColor As New Color(ColorSequence)
                        CurrentColorR = ParsedColor.R
                        CurrentColorG = ParsedColor.G
                        CurrentColorB = ParsedColor.B
                    Catch ex As Exception
                        WStkTrc(ex)
                        Wdbg(DebugLevel.E, "Possible input error: {0} ({1})", ColorSequence, ex.Message)
                    End Try
                ElseIf ConsoleResponse.Key = ConsoleKey.T Then
                    TrueColor = False
                ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                    ColorWheelExiting = True
                End If
            Else
                Write(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Use arrow up and arrow down keys to select between color ranges. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Tip)
                Write(DoTranslation("Press ""t"" to switch to true color mode."), True, ColTypes.Tip)

                'The color selection
                Write(vbNewLine + "   < ", False, ColTypes.Gray)
                WriteWhere($"{CurrentColor} [{Convert.ToInt32(CurrentColor)}]", (Console.CursorLeft + 38 - $"{CurrentColor} [{Convert.ToInt32(CurrentColor)}]".Length) / 2, Console.CursorTop, True, New Color(CurrentColor))
                WriteWhere(" >", Console.CursorLeft + 32, Console.CursorTop, False, ColTypes.Gray)

                'Show prompt
                Write(vbNewLine + vbNewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, New Color(CurrentColor))

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
                    Console.CursorVisible = True
                    Dim ColorNum As String = Console.ReadLine
                    Console.CursorVisible = False
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

        Console.CursorVisible = True
        If TrueColor Then
            Return $"{CurrentColorR};{CurrentColorG};{CurrentColorB}"
        Else
            Return CurrentColor
        End If
    End Function

End Module
