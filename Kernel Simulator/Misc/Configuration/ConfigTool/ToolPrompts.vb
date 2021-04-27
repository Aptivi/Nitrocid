
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

Imports System.Reflection

Public Module ToolPrompts

    ''' <summary>
    ''' Key type for settings entry
    ''' </summary>
    Enum SettingsKeyType
        SBoolean
        SInt
        SString
        SMultivar
        SSelection
        SMenu
    End Enum

    ''' <summary>
    ''' Main page
    ''' </summary>
    Sub OpenMainPage()
        Dim PromptFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer

        While Not PromptFinished
            Console.Clear()
            'List sections
            W(DoTranslation("Select section:") + vbNewLine, True, ColTypes.Neutral)
            W("1) " + DoTranslation("General Settings..."), True, ColTypes.Option)
            W("2) " + DoTranslation("Hardware Settings..."), True, ColTypes.Option)
            W("3) " + DoTranslation("Login Settings..."), True, ColTypes.Option)
            W("4) " + DoTranslation("Shell Settings..."), True, ColTypes.Option)
            W("5) " + DoTranslation("Network Settings..."), True, ColTypes.Option)
            W("6) " + DoTranslation("Screensaver Settings..."), True, ColTypes.Option)
            W("7) " + DoTranslation("Miscellaneous Settings...") + vbNewLine, True, ColTypes.Option)
            W("8) " + DoTranslation("Save Settings"), True, ColTypes.Option)
            W("9) " + DoTranslation("Exit") + vbNewLine, True, ColTypes.Option)

            'Prompt user and check for input
            W("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg("I", "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg("I", "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg("I", "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= 7 Then
                    Wdbg("I", "Opening section {0}...", AnswerInt)
                    OpenSection(AnswerString)
                ElseIf AnswerInt = 8 Then 'Save Settings
                    Wdbg("I", "Saving settings...")
                    Try
                        CreateConfig(True)
                    Catch ex As Exceptions.ConfigException
                        W(ex.Message, True, ColTypes.Err)
                        WStkTrc(ex)
                    End Try
                ElseIf AnswerInt = 9 Then 'Exit
                    Wdbg("W", "Exiting...")
                    PromptFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Open section
    ''' </summary>
    ''' <param name="SectionNum">Section number</param>
    Sub OpenSection(ByVal SectionNum As String)
        Dim MaxOptions As Integer = 0
        Dim SectionFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer

        While Not SectionFinished
            Console.Clear()
            'List options
            Select Case SectionNum
                Case "1" 'General
                    MaxOptions = 5
                    W("*) " + DoTranslation("General Settings...") + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists all general kernel settings, mainly for maintaining the kernel.") + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Prompt for Arguments on Boot") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(argsOnBoot)))
                    W("2) " + DoTranslation("Maintenance Mode Trigger") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(maintenance)))
                    W("3) " + DoTranslation("Change Root Password..."), True, ColTypes.Option)
                    W("4) " + DoTranslation("Check for Updates on Startup") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(CheckUpdateStart)))
                    W("5) " + DoTranslation("Change Culture when Switching Languages") + " [{0}]" + vbNewLine, True, ColTypes.Option, GetConfigValue(NameOf(LangChangeCulture)))
                Case "2" 'Hardware
                    MaxOptions = 1
                    W("*) " + DoTranslation("Hardware Settings...") + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section changes hardware probe behavior.") + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Quiet Probe") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(quietProbe)))
                Case "3" 'Login
                    MaxOptions = 3
                    W("*) " + DoTranslation("Login Settings...") + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section represents the login settings. Log out of your account for the changes to take effect.") + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Show MOTD on Log-in") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(showMOTD)))
                    W("2) " + DoTranslation("Clear Screen on Log-in") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(clsOnLogin)))
                    W("3) " + DoTranslation("Show available usernames") + " [{0}]" + vbNewLine, True, ColTypes.Option, GetConfigValue(NameOf(ShowAvailableUsers)))
                Case "4" 'Shell
                    MaxOptions = 8
                    W("*) " + DoTranslation("Shell Settings...") + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists the shell settings.") + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Colored Shell") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ColoredShell)))
                    W("2) " + DoTranslation("Simplified Help Command") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(simHelp)))
                    W("3) " + DoTranslation("Current Directory", currentLang) + " [{0}]", True, ColTypes.ListEntry, GetConfigValue(NameOf(CurrDir)))
                    W("4) " + DoTranslation("Prompt Style", currentLang) + " [{0}]", True, ColTypes.ListEntry, GetConfigValue(NameOf(ShellPromptStyle)))
                    W("5) " + DoTranslation("FTP Prompt Style", currentLang) + " [{0}]", True, ColTypes.ListEntry, GetConfigValue(NameOf(FTPShellPromptStyle)))
                    W("6) " + DoTranslation("Mail Prompt Style", currentLang) + " [{0}]", True, ColTypes.ListEntry, GetConfigValue(NameOf(MailShellPromptStyle)))
                    W("7) " + DoTranslation("SFTP Prompt Style", currentLang) + " [{0}]", True, ColTypes.ListEntry, GetConfigValue(NameOf(SFTPShellPromptStyle)))
                    W("8) " + DoTranslation("Custom colors...", currentLang) + vbNewLine, True, ColTypes.ListEntry)
                Case "5" 'Network
                    MaxOptions = 10
                    W("*) " + DoTranslation("Network Settings...") + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists the network settings, like the FTP shell, the network-related command settings, and the remote debug settings.") + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Debug Port") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DebugPort)))
                    W("2) " + DoTranslation("Download Retry Times") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DRetries)))
                    W("3) " + DoTranslation("Upload Retry Times") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(URetries)))
                    W("4) " + DoTranslation("Show progress bar while downloading or uploading from ""get"" or ""put"" command") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ShowProgress)))
                    W("5) " + DoTranslation("Log FTP username") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FTPLoggerUsername)))
                    W("6) " + DoTranslation("Log FTP IP address") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FTPLoggerIP)))
                    W("7) " + DoTranslation("Return only first FTP profile") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FTPFirstProfileOnly)))
                    W("8) " + DoTranslation("Show mail message preview") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ShowPreview)))
                    W("9) " + DoTranslation("Record chat to debug log") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(RecordChatToDebugLog)))
                    W("10) " + DoTranslation("Show SSH banner") + " [{0}]" + vbNewLine, True, ColTypes.Option, GetConfigValue(NameOf(SSHBanner)))
                Case "6" 'Screensaver
                    'TODO: Add support for custom screensavers
                    MaxOptions = 12
                    W("*) " + DoTranslation("Screensaver Settings...") + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists all the screensavers and their available settings.") + vbNewLine, True, ColTypes.Neutral)
                    W("1) ColorMix...", True, ColTypes.Option)
                    W("2) Matrix...", True, ColTypes.Option)
                    W("3) GlitterMatrix...", True, ColTypes.Option)
                    W("4) Disco...", True, ColTypes.Option)
                    W("5) Lines...", True, ColTypes.Option)
                    W("6) GlitterColor...", True, ColTypes.Option)
                    W("7) BouncingText...", True, ColTypes.Option)
                    W("8) Dissolve...", True, ColTypes.Option)
                    W("9) BouncingBlock...", True, ColTypes.Option)
                    W("10) ProgressClock...", True, ColTypes.Option)
                    W("11) Lighter...", True, ColTypes.Option)
                    W("12) " + DoTranslation("Screensaver Timeout in ms") + " [{0}]" + vbNewLine, True, ColTypes.Option, GetConfigValue(NameOf(ScrnTimeout)))
                Case "6.1" 'Screensaver > ColorMix
                    MaxOptions = 3
                    W("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " ColorMix." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ColorMix255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ColorMixTrueColor)))
                    W("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]" + vbNewLine, True, ColTypes.Option, GetConfigValue(NameOf(ColorMixDelay)))
                Case "6.2" 'Screensaver > Matrix
                    MaxOptions = 1
                    W("*) " + DoTranslation("Screensaver Settings...") + " > Matrix" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " Matrix." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(MatrixDelay)))
                Case "6.3" 'Screensaver > GlitterMatrix
                    MaxOptions = 1
                    W("*) " + DoTranslation("Screensaver Settings...") + " > GlitterMatrix" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " GlitterMatrix." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterMatrixDelay)))
                Case "6.4" 'Screensaver > Disco
                    MaxOptions = 4
                    W("*) " + DoTranslation("Screensaver Settings...") + " > Disco" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " Disco." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Disco255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DiscoTrueColor)))
                    W("3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DiscoCycleColors)))
                    W("4) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DiscoDelay)))
                Case "6.5" 'Screensaver > Lines
                    MaxOptions = 3
                    W("*) " + DoTranslation("Screensaver Settings...") + " > Lines" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " Lines." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Lines255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LinesTrueColor)))
                    W("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LinesDelay)))
                Case "6.6" 'Screensaver > GlitterColor
                    MaxOptions = 3
                    W("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " GlitterColor." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterColor255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterColorTrueColor)))
                    W("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(GlitterColorDelay)))
                Case "6.7" 'Screensaver > BouncingText
                    MaxOptions = 2
                    W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " BouncingText." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingTextDelay)))
                    W("2) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingTextWrite)))
                Case "6.8" 'Screensaver > Dissolve
                    MaxOptions = 2
                    W("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " Dissolve." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Dissolve255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DissolveTrueColor)))
                Case "6.9" 'Screensaver > BouncingBlock
                    MaxOptions = 3
                    W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " BouncingBlock." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingBlock255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingBlockTrueColor)))
                    W("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(BouncingBlockDelay)))
                Case "6.10" 'Screensaver > ProgressClock
                    MaxOptions = 8
                    W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " ProgressClock." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClock255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockTrueColor)))
                    W("3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockCycleColors)))
                    W("4) " + DoTranslation("Color of Seconds Bar") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockSecondsProgressColor)))
                    W("5) " + DoTranslation("Color of Minutes Bar") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockMinutesProgressColor)))
                    W("6) " + DoTranslation("Color of Hours Bar") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockHoursProgressColor)))
                    W("7) " + DoTranslation("Color of Information") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockProgressColor)))
                    W("8) " + DoTranslation("Ticks to change color") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(ProgressClockCycleColorsTicks)))
                Case "6.11" 'Screensaver > Lighter
                    MaxOptions = 4
                    W("*) " + DoTranslation("Screensaver Settings...") + " > Lighter" + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("This section lists screensaver settings for") + " Lighter." + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(Lighter255Colors)))
                    W("2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LighterTrueColor)))
                    W("3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LighterDelay)))
                    W("4) " + DoTranslation("Max Positions Count") + " [{0}]" + vbNewLine, True, ColTypes.Option, GetConfigValue(NameOf(LighterMaxPositions)))
                Case "7" 'Misc
                    MaxOptions = 10
                    W("*) " + DoTranslation("Miscellaneous Settings...") + vbNewLine, True, ColTypes.Neutral)
                    W(DoTranslation("Settings that don't fit in their appropriate sections land here.") + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Show Time/Date on Upper Right Corner") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(CornerTD)))
                    W("2) " + DoTranslation("Debug Size Quota in Bytes") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(DebugQuota)))
                    W("3) " + DoTranslation("Size parse mode") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(FullParseMode)))
                    W("4) " + DoTranslation("Marquee on startup") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(StartScroll)))
                    W("5) " + DoTranslation("Long Time and Date") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(LongTimeDate)))
                    W("6) " + DoTranslation("Show Hidden Files") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(HiddenFiles)))
                    W("7) " + DoTranslation("Preferred Unit for Temperature") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(PreferredUnit)))
                    W("8) " + DoTranslation("Enable text editor autosave") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TextEdit_AutoSaveFlag)))
                    W("9) " + DoTranslation("Text editor autosave interval") + " [{0}]", True, ColTypes.Option, GetConfigValue(NameOf(TextEdit_AutoSaveInterval)))
                    W("10) " + DoTranslation("Wrap list outputs") + " [{0}]" + vbNewLine, True, ColTypes.Option, GetConfigValue(NameOf(WrapListOutputs)))
                Case Else 'Invalid section
                    W("*) ???" + vbNewLine, True, ColTypes.Neutral)
                    W("X) " + DoTranslation("Invalid section entered. Please go back.") + vbNewLine, True, ColTypes.Err)
            End Select
            W("{0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.Option, MaxOptions + 1)
            Wdbg("W", "Section {0} has {1} selections.", SectionNum, MaxOptions)

            'Prompt user and check for input
            W("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg("I", "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg("I", "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg("I", "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxOptions Then
                    If AnswerInt = 3 And SectionNum = "1" Then
                        Wdbg("I", "Tried to open special section. Opening section 1.3...")
                        OpenKey("1.3", AnswerInt)
                    ElseIf AnswerInt = 8 And SectionNum = "4" Then
                        Wdbg("I", "Tried to open special section. Opening section 4.8...")
                        OpenKey("4.8", AnswerInt)
                    ElseIf AnswerInt <> MaxOptions And SectionNum = "6" Then
                        Wdbg("I", "Tried to open subsection. Opening section 6.{0}...", AnswerString)
                        OpenSection("6." + AnswerString)
                    Else
                        Wdbg("I", "Opening key {0} from section {1}...", AnswerInt, SectionNum)
                        OpenKey(SectionNum, AnswerInt)
                    End If
                ElseIf AnswerInt = MaxOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    SectionFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Open a key.
    ''' </summary>
    ''' <param name="Section">Section number</param>
    ''' <param name="KeyNumber">Key number</param>
    Sub OpenKey(ByVal Section As String, ByVal KeyNumber As Integer)
        Dim MaxKeyOptions As Integer = 0
        Dim KeyFinished As Boolean
        Dim KeyType As SettingsKeyType
        Dim KeyVar As String = ""
        Dim KeyVars As Dictionary(Of String, Object)
        Dim MultivarCustomAction As String = ""
        Dim AnswerString As String
        Dim AnswerInt As Integer

        While Not KeyFinished
            Console.Clear()
            'List Keys for specified section
            Select Case Section
                Case "1" 'General
                    Select Case KeyNumber
                        Case 1 'Prompt for Arguments on Boot
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(argsOnBoot)
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Prompt for Arguments on Boot") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Sets up the kernel so it prompts you for argument on boot.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Maintenance Mode Trigger
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(maintenance)
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Maintenance Mode Trigger") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Triggers maintenance mode. This disables multiple accounts.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'Change Root Password
                            OpenKey(Section, 1.3)
                        Case 4 'Check for Updates on Startup
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(CheckUpdateStart)
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Check for Updates on Startup") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Each startup, it will check for updates.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 5 'Change Culture when Switching Languages
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LangChangeCulture)
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Culture when Switching Languages") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("When switching languages, change the month names, calendar, etc.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case Else
                            W("*) " + DoTranslation("General Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "1.3" 'General -> Change Root Password
                    Select Case KeyNumber
                        Case 1
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(setRootPasswd)
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > " + DoTranslation("Change Root Password?") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("If the kernel is started, it will set root password.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > " + DoTranslation("Set Root Password...") + vbNewLine, True, ColTypes.Neutral)
                            If GetConfigValue(NameOf(setRootPasswd)) Then
                                KeyType = SettingsKeyType.SString
                                KeyVar = NameOf(RootPasswd)
                                W("*) " + DoTranslation("Write the root password to be set. Don't worry; the password are shown as stars."), True, ColTypes.Neutral)
                            Else
                                W("X) " + DoTranslation("Enable ""Change Root Password"" to use this option. Please go back.") + vbNewLine, True, ColTypes.Err)
                            End If
                        Case 3
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SMenu
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Select option:") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Change Root Password?") + " [{0}]", True, ColTypes.Option, GetConfigValue("setRootPasswd"))
                            W("2) " + DoTranslation("Set Root Password...") + vbNewLine, True, ColTypes.Option)
                        Case Else
                            W("*) " + DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "2" 'Hardware
                    Select Case KeyNumber
                        Case 1 'Quiet Probe
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(quietProbe)
                            W("*) " + DoTranslation("Hardware Settings...") + " > " + DoTranslation("Quiet Probe") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Keep hardware probing messages silent.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case Else
                            W("*) " + DoTranslation("Hardware Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "3" 'Login
                    Select Case KeyNumber
                        Case 1 'Show MOTD on Log-in
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(showMOTD)
                            W("*) " + DoTranslation("Login Settings...") + " > " + DoTranslation("Show MOTD on Log-in") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Show Message of the Day before displaying login screen.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Clear Screen on Log-in
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(clsOnLogin)
                            W("*) " + DoTranslation("Login Settings...") + " > " + DoTranslation("Clear Screen on Log-in") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Clear screen before displaying login screen.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'Show Available Usernames
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowAvailableUsers)
                            W("*) " + DoTranslation("Login Settings...") + " > " + DoTranslation("Show available usernames") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Shows available users if enabled.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case Else
                            W("*) " + DoTranslation("Login Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "4" 'Shell
                    Select Case KeyNumber
                        Case 1 'Colored Shell
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColoredShell)
                            W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Colored Shell") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Gives the kernel color support") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Simplified Help Command
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(simHelp)
                            W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Simplified Help Command") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Simplified help command for all the shells") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'Current Directory
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(CurrDir)
                            W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Current Directory") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Sets the shell's current directory. Write an absolute path to any existing directory."), True, ColTypes.Neutral)
                        Case 4 'Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ShellPromptStyle)
                            W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 5 'FTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(FTPShellPromptStyle)
                            W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("FTP Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 6 'Mail Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(MailShellPromptStyle)
                            W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Mail Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 7 'SFTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(SFTPShellPromptStyle)
                            W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("SFTP Prompt Style") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Shell Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "4.8" 'Shell -> Custom colors
                    'TODO: Configure this to use ColorWheel once it can support true color
                    MaxKeyOptions = 14
                    KeyType = SettingsKeyType.SMultivar
                    KeyVars = New Dictionary(Of String, Object)
                    MultivarCustomAction = "SetColors"
                    W("*) " + DoTranslation("Shell Settings...") + " > " + DoTranslation("Custom colors...") + vbNewLine, True, ColTypes.Neutral)
                    Dim Response As String
                    W("*) " + DoTranslation("Write a color as specified below:"), True, ColTypes.Neutral)
                    W("*) " + String.Join(", ", [Enum].GetNames(GetType(ConsoleColors))) + vbNewLine, True, ColTypes.Neutral)

                    ' Input color
                    W("1) " + DoTranslation("Input color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(InputColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(InputColor))
                    KeyVars.AddOrModify(NameOf(InputColor), Response)

                    ' License color
                    W("2) " + DoTranslation("License color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(LicenseColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(LicenseColor))
                    KeyVars.AddOrModify(NameOf(LicenseColor), Response)

                    ' Continuable kernel error color
                    W("3) " + DoTranslation("Continuable kernel error color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(ContKernelErrorColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(ContKernelErrorColor))
                    KeyVars.AddOrModify(NameOf(ContKernelErrorColor), Response)

                    ' Unontinuable kernel error color
                    W("4) " + DoTranslation("Uncontinuable kernel error color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(UncontKernelErrorColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(UncontKernelErrorColor))
                    KeyVars.AddOrModify(NameOf(UncontKernelErrorColor), Response)

                    ' Host name color
                    W("5) " + DoTranslation("Host name color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(HostNameShellColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(HostNameShellColor))
                    KeyVars.AddOrModify(NameOf(HostNameShellColor), Response)

                    ' User name color
                    W("6) " + DoTranslation("User name color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(UserNameShellColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(UserNameShellColor))
                    KeyVars.AddOrModify(NameOf(UserNameShellColor), Response)

                    ' Background color
                    W("7) " + DoTranslation("Background color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(BackgroundColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(BackgroundColor))
                    KeyVars.AddOrModify(NameOf(BackgroundColor), Response)

                    ' Neutral text color
                    W("8) " + DoTranslation("Neutral text color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(NeutralTextColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(NeutralTextColor))
                    KeyVars.AddOrModify(NameOf(NeutralTextColor), Response)

                    ' List entry color
                    W("9) " + DoTranslation("List entry color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(ListEntryColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(ListEntryColor))
                    KeyVars.AddOrModify(NameOf(ListEntryColor), Response)

                    ' List value color
                    W("10) " + DoTranslation("List value color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(ListValueColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(ListValueColor))
                    KeyVars.AddOrModify(NameOf(ListValueColor), Response)

                    ' Stage color
                    W("11) " + DoTranslation("Stage color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(StageColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(StageColor))
                    KeyVars.AddOrModify(NameOf(StageColor), Response)

                    ' Error color
                    W("12) " + DoTranslation("Error color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(ErrorColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(ErrorColor))
                    KeyVars.AddOrModify(NameOf(ErrorColor), Response)

                    ' Warning color
                    W("13) " + DoTranslation("Warning color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(WarningColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(WarningColor))
                    KeyVars.AddOrModify(NameOf(WarningColor), Response)

                    ' Option color
                    W("14) " + DoTranslation("Option color") + ": [{0}] ", False, ColTypes.Input, GetConfigValue(NameOf(OptionColor)))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetConfigValue(NameOf(OptionColor))
                    KeyVars.AddOrModify(NameOf(OptionColor), Response)
                Case "5" 'Network
                    Select Case KeyNumber
                        Case 1 'Debug Port
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DebugPort)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Debug Port") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port."), True, ColTypes.Neutral)
                        Case 2 'Download Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DRetries)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Download Retry Times") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write how many times the ""get"" command should retry failed downloads. It must be numeric."), True, ColTypes.Neutral)
                        Case 3 'Upload Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(URetries)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Upload Retry Times") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write how many times the ""put"" command should retry failed uploads. It must be numeric."), True, ColTypes.Neutral)
                        Case 4 'Show progress bar while downloading or uploading from "get" or "put" command
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowProgress)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Show progress bar while downloading or uploading from ""get"" or ""put"" command") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("If true, it makes ""get"" or ""put"" show the progress bar while downloading or uploading.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 5 'Log FTP username
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPLoggerUsername)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Log FTP username") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Whether or not to log FTP username.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 6 'Log FTP IP address
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPLoggerIP)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Log FTP IP address") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Whether or not to log FTP IP address.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 7 'Return only first FTP profile
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPFirstProfileOnly)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Return only first FTP profile") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Pick the first profile only when connecting.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 8 'Show mail message preview
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowPreview)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Show mail message preview") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("When listing mail messages, show body preview.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 9 'Record chat to debug log
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RecordChatToDebugLog)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Record chat to debug log") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Records remote debug chat to debug log.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 10 'Show SSH banner
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SSHBanner)
                            W("*) " + DoTranslation("Network Settings...") + DoTranslation("Show SSH banner") + " > " + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Shows the SSH server banner on connection.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case Else
                            W("*) " + DoTranslation("Network Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6" 'Screensaver
                    Select Case KeyNumber
                        Case 12 'Screensaver Timeout in ms
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ScrnTimeout)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Screensaver Timeout in ms") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write when to launch screensaver after specified milliseconds. It must be numeric."), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.1" 'ColorMix
                    Select Case KeyNumber
                        Case 1 'ColorMix: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMix255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for ColorMix.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'ColorMix: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMixTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for ColorMix.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'ColorMix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ColorMix > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.2" 'Matrix
                    Select Case KeyNumber
                        Case 1 'Matrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MatrixDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Matrix > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Matrix > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.3" 'GlitterMatrix
                    Select Case KeyNumber
                        Case 1 'GlitterMatrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterMatrixDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > GlitterMatrix > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > GlitterMatrix > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.4" 'Disco
                    Select Case KeyNumber
                        Case 1 'Disco: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Disco255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for Disco.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Disco: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for Disco.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'Disco: Cycle colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoCycleColors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Cycle colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Disco will cycle colors when enabled. Otherwise, select random colors.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 4 'Disco: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Disco >" + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Disco > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.5" 'Lines
                    Select Case KeyNumber
                        Case 1 'Lines: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lines255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for Lines.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Lines: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LinesTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for Lines.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'Lines: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lines > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.6" 'GlitterColor
                    Select Case KeyNumber
                        Case 1 'GlitterColor: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColor255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for GlitterColor.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'GlitterColor: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColorTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for GlitterColor.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'GlitterColor: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lines > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.7" 'BouncingText
                    Select Case KeyNumber
                        Case 1 'BouncingText: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'BouncingText: Text shown
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(BouncingTextWrite)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Text shown") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write any text you want shown. Shorter is better."), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingText > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.8" 'Dissolve
                    Select Case KeyNumber
                        Case 1 'Dissolve: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Dissolve255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for Dissolve.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Dissolve: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DissolveTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for Dissolve.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Dissolve > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.9" 'BouncingBlock
                    Select Case KeyNumber
                        Case 1 'BouncingBlock: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlock255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for BouncingBlock.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'BouncingBlock: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlockTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for BouncingBlock.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'BouncingBlock: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > BouncingBlock > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.10" 'ProgressClock
                    Select Case KeyNumber
                        Case 1 'ProgressClock: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClock255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for ProgressClock.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'ProgressClock: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for ProgressClock.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'ProgressClock: Cycle colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockCycleColors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Cycle colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("ProgressClock will select random colors if it's enabled. Otherwise, use colors from config.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 4 'ProgressClock: Color of Seconds Bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockSecondsProgressColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Seconds Bar") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("The color of seconds progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 5 'ProgressClock: Color of Minutes Bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockMinutesProgressColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Minutes Bar") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("The color of minutes progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 6 'ProgressClock: Color of Hours Bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockHoursProgressColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Hours Bar") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("The color of hours progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 7 'ProgressClock: Color of Information
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockProgressColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Color of Information") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("The color of date information. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."), True, ColTypes.Neutral)
                        Case 8 'ProgressClock: Ticks to change color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockCycleColorsTicks)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Ticks to change color") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("If color cycling is enabled, how many ticks before changing colors in ProgressClock? 1 tick = 0.5 seconds"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > ProgressClock > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "6.11" 'Lighter
                    Select Case KeyNumber
                        Case 1 'Lighter: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lighter255Colors)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Activate 255 colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates 255 color support for Lighter.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Lighter: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LighterTrueColor)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Activate true colors") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Activates true color support for Lighter.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 3 'Lighter: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterDelay)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Delay in Milliseconds") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Lighter: Max Positions Count
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaxPositions)
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Max Positions Count") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("How many positions are lit before dimming?"), True, ColTypes.Neutral)
                        Case Else
                            W("*) " + DoTranslation("Screensaver Settings...") + " > Lighter > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case "7" 'Misc
                    Select Case KeyNumber
                        Case 1 'Show Time/Date on Upper Right Corner
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(CornerTD)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show Time/Date on Upper Right Corner") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("The time and date will be shown in the upper right corner of the screen") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 2 'Debug Size Quota in Bytes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DebugQuota)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Debug Size Quota in Bytes") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("Write how many bytes can the debug log store. It must be numeric."), True, ColTypes.Neutral)
                        Case 3 'Size parse mode
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FullParseMode)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Size parse mode") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 4 'Marquee on startup
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(StartScroll)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Marquee on startup") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Enables eyecandy on startup") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 5 'Long Time and Date
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LongTimeDate)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Long Time and Date") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("The time and date will be longer, showing full month names, etc.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 6 'Show Hidden Files
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(HiddenFiles)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show Hidden Files") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Shows hidden files.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 7 'Preferred Unit for Temperature
                            MaxKeyOptions = 3
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(PreferredUnit)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Preferred Unit for Temperature") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Select your preferred unit for temperature (this only applies to the ""weather"" command)") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Kelvin"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Metric (Celsius)"), True, ColTypes.Option)
                            W("3) " + DoTranslation("Imperial (Fahrenheit)") + vbNewLine, True, ColTypes.Option)
                        Case 8 'Enable text editor autosave
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(TextEdit_AutoSaveFlag)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Enable text editor autosave") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Turns on or off the text editor autosave feature.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case 9 'Text editor autosave interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TextEdit_AutoSaveInterval)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Text editor autosave interval") + vbNewLine, True, ColTypes.Neutral)
                            W("*) " + DoTranslation("If autosave is enabled, the text file will be saved for each ""n"" seconds."), True, ColTypes.Neutral)
                        Case 10 'Wrap list outputs
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(WrapListOutputs)
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Wrap list outputs") + vbNewLine, True, ColTypes.Neutral)
                            W(DoTranslation("Wraps the list outputs if it seems too long for the current console geometry.") + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable"), True, ColTypes.Option)
                            W("2) " + DoTranslation("Disable") + vbNewLine, True, ColTypes.Option)
                        Case Else
                            W("*) " + DoTranslation("Miscellaneous Settings...") + " > ???" + vbNewLine, True, ColTypes.Neutral)
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.") + vbNewLine, True, ColTypes.Err)
                    End Select
                Case Else
                    W("*) ???" + vbNewLine, True, ColTypes.Neutral)
                    W("X) " + DoTranslation("Invalid section entered. Please go back.") + vbNewLine, True, ColTypes.Err)
            End Select

            'If user is on color selection screen, we'll give a user a confirmation.
            If Section = "4.8" Then
                W(vbNewLine + "*) " + DoTranslation("Do these color choices look OK?"), True, ColTypes.Neutral)
#Disable Warning BC42104
                For Each ColorType As String In KeyVars.Keys
#Enable Warning BC42104
                    W("   - {0}: ", False, ColTypes.ListEntry, ColorType)
                    W(KeyVars(ColorType), True, ColTypes.ListValue)
                Next
                W(vbNewLine + "*) " + DoTranslation("Answer {0} to go back. Otherwise, any answer means yes."), True, ColTypes.Neutral, MaxKeyOptions + 1)
            End If

            'Add an option to go back.
            W("{0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.Option, MaxKeyOptions + 1)
            Wdbg("W", "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions)
            Wdbg("W", "Target variable: {0}, Key Type: {1}", KeyVar, KeyType)

            'Prompt user
            W(If(Section = "4" And KeyNumber = 3, $"[{CurrDir}]", "") + "> ", False, ColTypes.Input)
            If KeyNumber = 2 And Section = "1.3" Then
                AnswerString = ReadLineNoInput("*")
                Console.WriteLine()
            Else
                AnswerString = Console.ReadLine
            End If
            Wdbg("I", "User answered {0}", AnswerString)

            'Check for input
            Wdbg("I", "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SBoolean Then
                Wdbg("I", "Answer is numeric and key is of the Boolean type.")
                If AnswerInt >= 1 And AnswerInt <= MaxKeyOptions Then
                    Wdbg("I", "Translating {0} to the boolean equivalent...", AnswerInt)
                    KeyFinished = True
                    Select Case AnswerInt
                        Case 1 'True
                            Wdbg("I", "Setting to True...")
                            SetConfigValue(KeyVar, True)
                        Case 2 'False
                            Wdbg("I", "Setting to False...")
                            SetConfigValue(KeyVar, False)
                    End Select
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            ElseIf (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SInt) Or
                   (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SSelection) Then
                Wdbg("I", "Answer is numeric and key is of the {0} type.", KeyType)
                If AnswerInt >= 0 Then
                    Wdbg("I", "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                    KeyFinished = True
                    SetConfigValue(KeyVar, AnswerInt)
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("W", "Negative values are disallowed.")
                    W(DoTranslation("The answer may not be negative."), True, ColTypes.Err)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            ElseIf KeyType = SettingsKeyType.SString Then
                Wdbg("I", "Answer is not numeric and key is of the String type. Setting variable...")
                If Section = "4" And KeyNumber = 3 Then 'If user is on Shell > Current Directory
                    If String.IsNullOrWhiteSpace(AnswerString) Then
                        Wdbg("I", "Answer is nothing but user on Shell > Current Directory. Setting to {0}...", CurrDir)
                        AnswerString = CurrDir
                    End If
                End If
                KeyFinished = True
                SetConfigValue(KeyVar, AnswerString)
            ElseIf Section = "1.3" And KeyNumber = 3 Then
                Wdbg("I", "Answer is not numeric and the user is on the special section.")
                If AnswerInt >= 1 And AnswerInt <= 2 Then
                    Wdbg("I", "AnswerInt is {0}. Opening key...", AnswerInt)
                    OpenKey(Section, AnswerInt)
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            ElseIf KeyType = SettingsKeyType.SMultivar And MultivarCustomAction = "SetColors" Then
                Wdbg("I", "Multiple variables, and custom action was {0}.", MultivarCustomAction)
                Wdbg("I", "Answer was {0}", AnswerInt)
                If AnswerInt = 15 Then 'Go Back...
                    Wdbg("W", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("I", "Setting necessary variables...")
                    Wdbg("I", "Variables: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}.", KeyVars(NameOf(InputColor)), KeyVars(NameOf(LicenseColor)), KeyVars(NameOf(ContKernelErrorColor)),
                         KeyVars(NameOf(UncontKernelErrorColor)), KeyVars(NameOf(HostNameShellColor)), KeyVars(NameOf(UserNameShellColor)), KeyVars(NameOf(BackgroundColor)), KeyVars(NameOf(NeutralTextColor)),
                         KeyVars(NameOf(ListEntryColor)), KeyVars(NameOf(ListValueColor)), KeyVars(NameOf(StageColor)), KeyVars(NameOf(ErrorColor)), KeyVars(NameOf(WarningColor)), KeyVars(NameOf(OptionColor)))

                    If SetColors(KeyVars(NameOf(InputColor)), KeyVars(NameOf(LicenseColor)), KeyVars(NameOf(ContKernelErrorColor)), KeyVars(NameOf(UncontKernelErrorColor)), KeyVars(NameOf(HostNameShellColor)),
                                 KeyVars(NameOf(UserNameShellColor)), KeyVars(NameOf(BackgroundColor)), KeyVars(NameOf(NeutralTextColor)), KeyVars(NameOf(ListEntryColor)), KeyVars(NameOf(ListValueColor)),
                                 KeyVars(NameOf(StageColor)), KeyVars(NameOf(ErrorColor)), KeyVars(NameOf(WarningColor)), KeyVars(NameOf(OptionColor))) Then
                        KeyFinished = True
                    End If
                End If
            Else
                Wdbg("W", "Answer is not valid.")
                W(DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Sets the value of a variable to the new value dynamically
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="VariableValue">New value of variable</param>
    Public Sub SetConfigValue(ByVal Variable As String, ByVal VariableValue As Object)
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Set the variable if found
        If Not IsNothing(TargetField) Then
            'The "obj" description says this: "The object whose field value will be set."
            'Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg("I", "Got field {0}. Setting to {1}...", TargetField.Name, VariableValue)
            TargetField.SetValue(Variable, VariableValue)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg("I", "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Err, Variable)
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of a variable dynamically 
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <returns>Value of a variable</returns>
    Public Function GetConfigValue(ByVal Variable As String) As Object
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Get the variable if found
        If Not IsNothing(TargetField) Then
            'The "obj" description says this: "The object whose field value will be returned."
            'Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg("I", "Got field {0}.", TargetField.Name)
            Return TargetField.GetValue(Variable)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg("I", "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Err, Variable)
            Return Nothing
        End If
    End Function

End Module
