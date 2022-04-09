
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

Imports System.IO
Imports KS.Misc.Configuration

Namespace ConsoleBase
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
            ''' <summary>
            ''' Alternative option
            ''' </summary>
            AlternativeOption
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

        'Variables for colors used by previous versions of the kernel.
        Public InputColor As New Color(ConsoleColors.White)
        Public LicenseColor As New Color(ConsoleColors.White)
        Public ContKernelErrorColor As New Color(ConsoleColors.Yellow)
        Public UncontKernelErrorColor As New Color(ConsoleColors.Red)
        Public HostNameShellColor As New Color(ConsoleColors.DarkGreen)
        Public UserNameShellColor As New Color(ConsoleColors.Green)
        Public BackgroundColor As New Color(ConsoleColors.Black)
        Public NeutralTextColor As New Color(ConsoleColors.Gray)
        Public ListEntryColor As New Color(ConsoleColors.DarkYellow)
        Public ListValueColor As New Color(ConsoleColors.DarkGray)
        Public StageColor As New Color(ConsoleColors.Green)
        Public ErrorColor As New Color(ConsoleColors.Red)
        Public WarningColor As New Color(ConsoleColors.Yellow)
        Public OptionColor As New Color(ConsoleColors.DarkYellow)
        Public BannerColor As New Color(ConsoleColors.Green)
        Public NotificationTitleColor As New Color(ConsoleColors.White)
        Public NotificationDescriptionColor As New Color(ConsoleColors.Gray)
        Public NotificationProgressColor As New Color(ConsoleColors.DarkYellow)
        Public NotificationFailureColor As New Color(ConsoleColors.Red)
        Public QuestionColor As New Color(ConsoleColors.Yellow)
        Public SuccessColor As New Color(ConsoleColors.Green)
        Public UserDollarColor As New Color(ConsoleColors.Gray)
        Public TipColor As New Color(ConsoleColors.Gray)
        Public SeparatorTextColor As New Color(ConsoleColors.White)
        Public SeparatorColor As New Color(ConsoleColors.Gray)
        Public ListTitleColor As New Color(ConsoleColors.White)
        Public DevelopmentWarningColor As New Color(ConsoleColors.Yellow)
        Public StageTimeColor As New Color(ConsoleColors.Gray)
        Public ProgressColor As New Color(ConsoleColors.DarkYellow)
        Public BackOptionColor As New Color(ConsoleColors.DarkRed)
        Public LowPriorityBorderColor As New Color(ConsoleColors.White)
        Public MediumPriorityBorderColor As New Color(ConsoleColors.Yellow)
        Public HighPriorityBorderColor As New Color(ConsoleColors.Red)
        Public TableSeparatorColor As New Color(ConsoleColors.DarkGray)
        Public TableHeaderColor As New Color(ConsoleColors.White)
        Public TableValueColor As New Color(ConsoleColors.Gray)
        Public SelectedOptionColor As New Color(ConsoleColors.Yellow)
        Public AlternativeOptionColor As New Color(ConsoleColors.DarkGreen)

        ''' <summary>
        ''' Resets all colors to default
        ''' </summary>
        Public Sub ResetColors()
            Wdbg(DebugLevel.I, "Resetting colors")
            Dim DefInfo As New ThemeInfo("_Default")
            InputColor = DefInfo.ThemeInputColor
            LicenseColor = DefInfo.ThemeLicenseColor
            ContKernelErrorColor = DefInfo.ThemeContKernelErrorColor
            UncontKernelErrorColor = DefInfo.ThemeUncontKernelErrorColor
            HostNameShellColor = DefInfo.ThemeHostNameShellColor
            UserNameShellColor = DefInfo.ThemeUserNameShellColor
            BackgroundColor = DefInfo.ThemeBackgroundColor
            NeutralTextColor = DefInfo.ThemeNeutralTextColor
            ListEntryColor = DefInfo.ThemeListEntryColor
            ListValueColor = DefInfo.ThemeListValueColor
            StageColor = DefInfo.ThemeStageColor
            ErrorColor = DefInfo.ThemeErrorColor
            WarningColor = DefInfo.ThemeWarningColor
            OptionColor = DefInfo.ThemeOptionColor
            BannerColor = DefInfo.ThemeBannerColor
            NotificationTitleColor = DefInfo.ThemeNotificationTitleColor
            NotificationDescriptionColor = DefInfo.ThemeNotificationDescriptionColor
            NotificationProgressColor = DefInfo.ThemeNotificationProgressColor
            NotificationFailureColor = DefInfo.ThemeNotificationFailureColor
            QuestionColor = DefInfo.ThemeQuestionColor
            SuccessColor = DefInfo.ThemeSuccessColor
            UserDollarColor = DefInfo.ThemeUserDollarColor
            TipColor = DefInfo.ThemeTipColor
            SeparatorTextColor = DefInfo.ThemeSeparatorTextColor
            SeparatorColor = DefInfo.ThemeSeparatorColor
            ListTitleColor = DefInfo.ThemeListTitleColor
            DevelopmentWarningColor = DefInfo.ThemeDevelopmentWarningColor
            StageTimeColor = DefInfo.ThemeStageTimeColor
            ProgressColor = DefInfo.ThemeProgressColor
            BackOptionColor = DefInfo.ThemeBackOptionColor
            LowPriorityBorderColor = DefInfo.ThemeLowPriorityBorderColor
            MediumPriorityBorderColor = DefInfo.ThemeMediumPriorityBorderColor
            HighPriorityBorderColor = DefInfo.ThemeHighPriorityBorderColor
            TableSeparatorColor = DefInfo.ThemeTableSeparatorColor
            TableHeaderColor = DefInfo.ThemeTableHeaderColor
            TableValueColor = DefInfo.ThemeTableValueColor
            SelectedOptionColor = DefInfo.ThemeSelectedOptionColor
            AlternativeOptionColor = DefInfo.ThemeAlternativeOptionColor
            LoadBack()

            'Raise event
            KernelEventManager.RaiseColorReset()
        End Sub

        ''' <summary>
        ''' Loads the background
        ''' </summary>
        Public Sub LoadBack()
            Try
                Wdbg(DebugLevel.I, "Filling background with background color")
                SetConsoleColor(BackgroundColor, True)
                Console.Clear()
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to set background: {0}", ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Makes the color configuration permanent
        ''' </summary>
        Public Sub MakePermanent()
            ConfigToken("Colors")("User Name Shell Color") = UserNameShellColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Host Name Shell Color") = HostNameShellColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Continuable Kernel Error Color") = ContKernelErrorColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Uncontinuable Kernel Error Color") = UncontKernelErrorColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Text Color") = NeutralTextColor.PlainSequenceEnclosed
            ConfigToken("Colors")("License Color") = LicenseColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Background Color") = BackgroundColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Input Color") = InputColor.PlainSequenceEnclosed
            ConfigToken("Colors")("List Entry Color") = ListEntryColor.PlainSequenceEnclosed
            ConfigToken("Colors")("List Value Color") = ListValueColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Kernel Stage Color") = StageColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Error Text Color") = ErrorColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Warning Text Color") = WarningColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Option Color") = OptionColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Banner Color") = BannerColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Notification Title Color") = NotificationTitleColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Notification Description Color") = NotificationDescriptionColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Notification Progress Color") = NotificationProgressColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Notification Failure Color") = NotificationFailureColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Question Color") = QuestionColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Success Color") = SuccessColor.PlainSequenceEnclosed
            ConfigToken("Colors")("User Dollar Color") = UserDollarColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Tip Color") = TipColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Separator Text Color") = SeparatorTextColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Separator Color") = SeparatorColor.PlainSequenceEnclosed
            ConfigToken("Colors")("List Title Color") = ListTitleColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Development Warning Color") = DevelopmentWarningColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Stage Time Color") = StageTimeColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Progress Color") = ProgressColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Back Option Color") = BackOptionColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Low Priority Border Color") = LowPriorityBorderColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Medium Priority Border Color") = MediumPriorityBorderColor.PlainSequenceEnclosed
            ConfigToken("Colors")("High Priority Border Color") = HighPriorityBorderColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Table Separator Color") = TableSeparatorColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Table Header Color") = TableHeaderColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Table Value Color") = TableValueColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Selected Option Color") = SelectedOptionColor.PlainSequenceEnclosed
            ConfigToken("Colors")("Alternative Option Color") = AlternativeOptionColor.PlainSequenceEnclosed
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
        ''' <param name="AlternativeOptionColor">Alternative option color</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="InvalidOperationException"></exception>
        ''' <exception cref="Exceptions.ColorException"></exception>
        Public Function SetColors(InputColor As String, LicenseColor As String, ContKernelErrorColor As String, UncontKernelErrorColor As String, HostNameShellColor As String, UserNameShellColor As String,
                              BackgroundColor As String, NeutralTextColor As String, ListEntryColor As String, ListValueColor As String, StageColor As String, ErrorColor As String, WarningColor As String,
                              OptionColor As String, BannerColor As String, NotificationTitleColor As String, NotificationDescriptionColor As String, NotificationProgressColor As String,
                              NotificationFailureColor As String, QuestionColor As String, SuccessColor As String, UserDollarColor As String, TipColor As String, SeparatorTextColor As String,
                              SeparatorColor As String, ListTitleColor As String, DevelopmentWarningColor As String, StageTimeColor As String, ProgressColor As String, BackOptionColor As String,
                              LowPriorityBorderColor As String, MediumPriorityBorderColor As String, HighPriorityBorderColor As String, TableSeparatorColor As String, TableHeaderColor As String,
                              TableValueColor As String, SelectedOptionColor As String, AlternativeOptionColor As String) As Boolean
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
            If String.IsNullOrEmpty(AlternativeOptionColor) Then AlternativeOptionColor = "def"

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
                If SelectedOptionColor = "def" Then SelectedOptionColor = New Color(ConsoleColors.Yellow).PlainSequence
                If AlternativeOptionColor = "def" Then AlternativeOptionColor = New Color(ConsoleColors.DarkGreen).PlainSequence
                If BackgroundColor = "def" Then
                    BackgroundColor = New Color(ConsoleColors.Black).PlainSequence
                    LoadBack()
                End If

                'Set the colors
                Try
                    ColorTools.InputColor = New Color(InputColor)
                    ColorTools.LicenseColor = New Color(LicenseColor)
                    ColorTools.ContKernelErrorColor = New Color(ContKernelErrorColor)
                    ColorTools.UncontKernelErrorColor = New Color(UncontKernelErrorColor)
                    ColorTools.HostNameShellColor = New Color(HostNameShellColor)
                    ColorTools.UserNameShellColor = New Color(UserNameShellColor)
                    ColorTools.BackgroundColor = New Color(BackgroundColor)
                    ColorTools.NeutralTextColor = New Color(NeutralTextColor)
                    ColorTools.ListEntryColor = New Color(ListEntryColor)
                    ColorTools.ListValueColor = New Color(ListValueColor)
                    ColorTools.StageColor = New Color(StageColor)
                    ColorTools.ErrorColor = New Color(ErrorColor)
                    ColorTools.WarningColor = New Color(WarningColor)
                    ColorTools.OptionColor = New Color(OptionColor)
                    ColorTools.BannerColor = New Color(BannerColor)
                    ColorTools.NotificationTitleColor = New Color(NotificationTitleColor)
                    ColorTools.NotificationDescriptionColor = New Color(NotificationDescriptionColor)
                    ColorTools.NotificationProgressColor = New Color(NotificationProgressColor)
                    ColorTools.NotificationFailureColor = New Color(NotificationFailureColor)
                    ColorTools.QuestionColor = New Color(QuestionColor)
                    ColorTools.SuccessColor = New Color(SuccessColor)
                    ColorTools.UserDollarColor = New Color(UserDollarColor)
                    ColorTools.TipColor = New Color(TipColor)
                    ColorTools.SeparatorTextColor = New Color(SeparatorTextColor)
                    ColorTools.SeparatorColor = New Color(SeparatorColor)
                    ColorTools.ListTitleColor = New Color(ListTitleColor)
                    ColorTools.DevelopmentWarningColor = New Color(DevelopmentWarningColor)
                    ColorTools.StageTimeColor = New Color(StageTimeColor)
                    ColorTools.ProgressColor = New Color(ProgressColor)
                    ColorTools.BackOptionColor = New Color(BackOptionColor)
                    ColorTools.LowPriorityBorderColor = New Color(LowPriorityBorderColor)
                    ColorTools.MediumPriorityBorderColor = New Color(MediumPriorityBorderColor)
                    ColorTools.HighPriorityBorderColor = New Color(HighPriorityBorderColor)
                    ColorTools.TableSeparatorColor = New Color(TableSeparatorColor)
                    ColorTools.TableHeaderColor = New Color(TableHeaderColor)
                    ColorTools.TableValueColor = New Color(TableValueColor)
                    ColorTools.SelectedOptionColor = New Color(SelectedOptionColor)
                    ColorTools.AlternativeOptionColor = New Color(AlternativeOptionColor)
                    LoadBack()
                    MakePermanent()

                    'Raise event
                    KernelEventManager.RaiseColorSet()
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.InvalidColors)
                    Throw New Exceptions.ColorException(DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message)
                End Try
            Else
                KernelEventManager.RaiseColorSetError(ColorSetErrorReasons.NoColors)
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
                SetConsoleColor(InputColor)
                SetConsoleColor(BackgroundColor, True)
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
            Return SetConsoleColor(colorType, False)
        End Function

        ''' <summary>
        ''' Sets the console color
        ''' </summary>
        ''' <param name="colorType">A type of colors that will be changed.</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function SetConsoleColor(colorType As ColTypes, Background As Boolean) As Boolean
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                Select Case colorType
                    Case ColTypes.Neutral
                        SetConsoleColor(NeutralTextColor, Background)
                    Case ColTypes.Continuable
                        SetConsoleColor(ContKernelErrorColor, Background)
                    Case ColTypes.Uncontinuable
                        SetConsoleColor(UncontKernelErrorColor, Background)
                    Case ColTypes.HostName
                        SetConsoleColor(HostNameShellColor, Background)
                    Case ColTypes.UserName
                        SetConsoleColor(UserNameShellColor, Background)
                    Case ColTypes.License
                        SetConsoleColor(LicenseColor, Background)
                    Case ColTypes.Gray
                        If BackgroundColor.IsBright Then
                            SetConsoleColor(NeutralTextColor, Background)
                        Else
                            SetConsoleColor(New Color(ConsoleColors.Gray), Background)
                        End If
                    Case ColTypes.ListValue
                        SetConsoleColor(ListValueColor, Background)
                    Case ColTypes.ListEntry
                        SetConsoleColor(ListEntryColor, Background)
                    Case ColTypes.Stage
                        SetConsoleColor(StageColor, Background)
                    Case ColTypes.Error
                        SetConsoleColor(ErrorColor, Background)
                    Case ColTypes.Warning
                        SetConsoleColor(WarningColor, Background)
                    Case ColTypes.Option
                        SetConsoleColor(OptionColor, Background)
                    Case ColTypes.Banner
                        SetConsoleColor(BannerColor, Background)
                    Case ColTypes.NotificationTitle
                        SetConsoleColor(NotificationTitleColor, Background)
                    Case ColTypes.NotificationDescription
                        SetConsoleColor(NotificationDescriptionColor, Background)
                    Case ColTypes.NotificationProgress
                        SetConsoleColor(NotificationProgressColor, Background)
                    Case ColTypes.NotificationFailure
                        SetConsoleColor(NotificationFailureColor, Background)
                    Case ColTypes.Question, ColTypes.Input
                        SetConsoleColor(QuestionColor, Background)
                    Case ColTypes.Success
                        SetConsoleColor(SuccessColor, Background)
                    Case ColTypes.UserDollarSign
                        SetConsoleColor(UserDollarColor, Background)
                    Case ColTypes.Tip
                        SetConsoleColor(TipColor, Background)
                    Case ColTypes.SeparatorText
                        SetConsoleColor(SeparatorTextColor, Background)
                    Case ColTypes.Separator
                        SetConsoleColor(SeparatorColor, Background)
                    Case ColTypes.ListTitle
                        SetConsoleColor(ListTitleColor, Background)
                    Case ColTypes.DevelopmentWarning
                        SetConsoleColor(DevelopmentWarningColor, Background)
                    Case ColTypes.StageTime
                        SetConsoleColor(StageTimeColor, Background)
                    Case ColTypes.Progress
                        SetConsoleColor(ProgressColor, Background)
                    Case ColTypes.BackOption
                        SetConsoleColor(BackOptionColor, Background)
                    Case ColTypes.LowPriorityBorder
                        SetConsoleColor(LowPriorityBorderColor, Background)
                    Case ColTypes.MediumPriorityBorder
                        SetConsoleColor(MediumPriorityBorderColor, Background)
                    Case ColTypes.HighPriorityBorder
                        SetConsoleColor(HighPriorityBorderColor, Background)
                    Case ColTypes.TableSeparator
                        SetConsoleColor(TableSeparatorColor, Background)
                    Case ColTypes.TableHeader
                        SetConsoleColor(TableHeaderColor, Background)
                    Case ColTypes.TableValue
                        SetConsoleColor(TableValueColor, Background)
                    Case ColTypes.SelectedOption
                        SetConsoleColor(SelectedOptionColor, Background)
                    Case ColTypes.AlternativeOption
                        SetConsoleColor(AlternativeOptionColor, Background)
                    Case Else
                        Exit Select
                End Select
                If Not Background Then SetConsoleColor(BackgroundColor, True)
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
        ''' Tries parsing the color from the specifier string
        ''' </summary>
        ''' <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        ''' <returns>True if successful; False if failed</returns>
        Public Function TryParseColor(ColorSpecifier As String) As Boolean
            Try
                Dim ColorInstance As New Color(ColorSpecifier)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Tries parsing the color from the specifier string
        ''' </summary>
        ''' <param name="ColorNum">The color number</param>
        ''' <returns>True if successful; False if failed</returns>
        Public Function TryParseColor(ColorNum As Integer) As Boolean
            Try
                Dim ColorInstance As New Color(ColorNum)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Tries parsing the color from the specifier string
        ''' </summary>
        ''' <param name="R">The red level</param>
        ''' <param name="G">The green level</param>
        ''' <param name="B">The blue level</param>
        ''' <returns>True if successful; False if failed</returns>
        Public Function TryParseColor(R As Integer, G As Integer, B As Integer) As Boolean
            Try
                Dim ColorInstance As New Color(R, G, B)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Converts from the hexadecimal representation of a color to the RGB sequence
        ''' </summary>
        ''' <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        ''' <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        Public Function ConvertFromHexToRGB(Hex As String) As String
            If Hex.StartsWith("#") Then
                Dim ColorDecimal As Integer = Convert.ToInt32(Hex.RemoveLetter(0), 16)
                Dim R As Integer = CByte((ColorDecimal And &HFF0000) >> &H10)
                Dim G As Integer = CByte((ColorDecimal And &HFF00) >> 8)
                Dim B As Integer = CByte(ColorDecimal And &HFF)
                Return $"{R};{G};{B}"
            Else
                Throw New Exceptions.ColorException(DoTranslation("Invalid hex color specifier."))
            End If
        End Function

        ''' <summary>
        ''' Converts from the RGB sequence of a color to the hexadecimal representation
        ''' </summary>
        ''' <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        ''' <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        Public Function ConvertFromRGBToHex(RGBSequence As String) As String
            If RGBSequence.Contains(";") Then
                'Split the VT sequence into three parts
                Dim ColorSpecifierArray() As String = RGBSequence.Split(";")
                If ColorSpecifierArray.Length = 3 Then
                    Dim R As Integer = ColorSpecifierArray(0)
                    Dim G As Integer = ColorSpecifierArray(1)
                    Dim B As Integer = ColorSpecifierArray(2)
                    Return $"#{R:X2}{G:X2}{B:X2}"
                Else
                    Throw New Exceptions.ColorException(DoTranslation("Invalid RGB color specifier."))
                End If
            Else
                Throw New Exceptions.ColorException(DoTranslation("Invalid RGB color specifier."))
            End If
        End Function

    End Module
End Namespace