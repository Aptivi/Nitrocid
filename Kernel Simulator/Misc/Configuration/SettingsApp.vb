
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

Imports System.Globalization
Imports System.Reflection

'TODO: This and SaverSettingsApp are huge; consider downsizing it and jsonifying all the configuration parameters.
Public Module SettingsApp

    ''' <summary>
    ''' Key type for settings entry
    ''' </summary>
    Enum SettingsKeyType
        SUnknown
        SBoolean
        SInt
        SString
        SLongString
        SSelection
        SList
        SVariant
        SColor
    End Enum

    ''' <summary>
    ''' Main page
    ''' </summary>
    Sub OpenMainPage()
        Dim PromptFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer
        Dim MaxSections As Integer = 8

        While Not PromptFinished
            Console.Clear()
            'List sections
            WriteSeparator(DoTranslation("Welcome to Settings!"), True)
            W(vbNewLine + DoTranslation("Select section:") + vbNewLine, True, ColTypes.Neutral)
            W(" 1) " + DoTranslation("General Settings..."), True, ColTypes.Option)
            W(" 2) " + DoTranslation("Hardware Settings..."), True, ColTypes.Option)
            W(" 3) " + DoTranslation("Login Settings..."), True, ColTypes.Option)
            W(" 4) " + DoTranslation("Shell Settings..."), True, ColTypes.Option)
            W(" 5) " + DoTranslation("Filesystem Settings..."), True, ColTypes.Option)
            W(" 6) " + DoTranslation("Network Settings..."), True, ColTypes.Option)
            W(" 7) " + DoTranslation("Screensaver Settings..."), True, ColTypes.Option)
            W(" 8) " + DoTranslation("Miscellaneous Settings...") + vbNewLine, True, ColTypes.Option)
            W(" 9) " + DoTranslation("Save Settings"), True, ColTypes.BackOption)
            W(" 10) " + DoTranslation("Exit"), True, ColTypes.BackOption)

            'Prompt user and check for input
            Console.WriteLine()
            W("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxSections Then
                    Wdbg(DebugLevel.I, "Opening section {0}...", AnswerInt)
                    OpenSection(AnswerString)
                ElseIf AnswerInt = MaxSections + 1 Then 'Save Settings
                    Wdbg(DebugLevel.I, "Saving settings...")
                    Try
                        CreateConfig()
                        SaveCustomSaverSettings()
                    Catch ex As Exception
                        W(ex.Message, True, ColTypes.Error)
                        WStkTrc(ex)
                        Console.ReadKey()
                    End Try
                ElseIf AnswerInt = MaxSections + 2 Then 'Exit
                    Wdbg(DebugLevel.W, "Exiting...")
                    PromptFinished = True
                    Console.Clear()
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
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
    End Sub

    ''' <summary>
    ''' Open section
    ''' </summary>
    ''' <param name="SectionNum">Section number</param>
    Sub OpenSection(SectionNum As String, ParamArray SectionParameters() As Object)
        'General variables
        Dim MaxOptions As Integer = 0
        Dim SectionFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer

        While Not SectionFinished
            Console.Clear()

            'List options
            Select Case SectionNum
                Case "1" 'General
                    MaxOptions = 13
                    WriteSeparator(DoTranslation("General Settings..."), True)
                    W(vbNewLine + DoTranslation("This section lists all general kernel settings, mainly for maintaining the kernel.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Prompt for Arguments on Boot") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ArgsOnBoot)))
                    W(" 2) " + DoTranslation("Maintenance Mode Trigger") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Maintenance)))
                    W(" 3) " + DoTranslation("Change Root Password..."), True, ColTypes.Option)
                    W(" 4) " + DoTranslation("Check for Updates on Startup") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(CheckUpdateStart)))
                    W(" 5) " + DoTranslation("Custom Startup Banner") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(CustomBanner)))
                    W(" 6) " + DoTranslation("Change Culture when Switching Languages") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LangChangeCulture)))
                    W(" 7) " + DoTranslation("Culture of") + " {0} [{1}]", True, ColTypes.Option, CurrentLanguage, GetConfigPropertyValueInVariableField(NameOf(CurrentCult), NameOf(CurrentCult.Name)))
                    W(" 8) " + DoTranslation("Show app information during boot") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowAppInfoOnBoot)))
                    W(" 9) " + DoTranslation("Parse command-line arguments") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ParseCommandLineArguments)))
                    W(" 10) " + DoTranslation("Show stage finish times") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowStageFinishTimes)))
                    W(" 11) " + DoTranslation("Start kernel modifications on boot") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(StartKernelMods)))
                    W(" 12) " + DoTranslation("Show current time before login") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowCurrentTimeBeforeLogin)))
                    W(" 13) " + DoTranslation("Notify for any fault during boot") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyFaultsBoot)))
                Case "1.3" 'Change Root Password...
                    MaxOptions = 2
                    WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password..."), True)
                    W(vbNewLine + DoTranslation("This section lets you manage root password creation.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Change Root Password?") + " [{0}]", True, ColTypes.Option, GetConfigValueField("setRootPasswd"))
                    W(" 2) " + DoTranslation("Set Root Password..."), True, ColTypes.Option)
                Case "2" 'Hardware
                    MaxOptions = 4
                    WriteSeparator(DoTranslation("Hardware Settings..."), True)
                    W(vbNewLine + DoTranslation("This section changes hardware probe behavior.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Quiet Probe") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(QuietHardwareProbe)))
                    W(" 2) " + DoTranslation("Full Probe") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FullHardwareProbe)))
                    W(" 3) " + DoTranslation("Verbose Probe") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(VerboseHardwareProbe)))
                    W(" 4) " + DoTranslation("Use legacy hardware listing") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(UseLegacyHardwareListing)))
                Case "3" 'Login
                    MaxOptions = 8
                    WriteSeparator(DoTranslation("Login Settings..."), True)
                    W(vbNewLine + DoTranslation("This section represents the login settings. Log out of your account for the changes to take effect.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Show MOTD on Log-in") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowMOTD)))
                    W(" 2) " + DoTranslation("Clear Screen on Log-in") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ClearOnLogin)))
                    W(" 3) " + DoTranslation("Show available usernames") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowAvailableUsers)))
                    W(" 4) " + DoTranslation("MOTD path") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MOTDFilePath)))
                    W(" 5) " + DoTranslation("MAL path") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MALFilePath)))
                    W(" 6) " + DoTranslation("Username prompt style") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(UsernamePrompt)))
                    W(" 7) " + DoTranslation("Password prompt style") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(PasswordPrompt)))
                    W(" 8) " + DoTranslation("Show MAL on log-in") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowMAL)))
                Case "4" 'Shell
                    MaxOptions = 16
                    WriteSeparator(DoTranslation("Shell Settings..."), True)
                    W(vbNewLine + DoTranslation("This section lists the shell settings.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Colored Shell") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColoredShell)))
                    W(" 2) " + DoTranslation("Simplified Help Command") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SimHelp)))
                    W(" 3) " + DoTranslation("Current Directory", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(CurrDir)))
                    W(" 4) " + DoTranslation("Lookup Directories", CurrentLanguage) + " [{0}]", True, ColTypes.Option, PathsToLookup.Split(PathLookupDelimiter).Length)
                    W(" 5) " + DoTranslation("Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShellPromptStyle)))
                    W(" 6) " + DoTranslation("FTP Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FTPShellPromptStyle)))
                    W(" 7) " + DoTranslation("Mail Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MailShellPromptStyle)))
                    W(" 8) " + DoTranslation("SFTP Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SFTPShellPromptStyle)))
                    W(" 9) " + DoTranslation("RSS Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RSSShellPromptStyle)))
                    W(" 10) " + DoTranslation("Text Edit Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TextEdit_PromptStyle)))
                    W(" 11) " + DoTranslation("Zip Shell Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ZipShell_PromptStyle)))
                    W(" 12) " + DoTranslation("Test Shell Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Test_PromptStyle)))
                    W(" 13) " + DoTranslation("JSON Shell Prompt Style", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(JsonShell_PromptStyle)))
                    W(" 14) " + DoTranslation("Probe injected commands", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProbeInjectedCommands)))
                    W(" 15) " + DoTranslation("Start color wheel in true color mode", CurrentLanguage) + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorWheelTrueColor)))
                    W(" 16) " + DoTranslation("Custom colors...", CurrentLanguage), True, ColTypes.Option)
                Case "4.16" 'Custom colors...
                    MaxOptions = 33
                    WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Custom colors..."), True)
                    W(vbNewLine + DoTranslation("This section lets you choose what type of color do you want to change.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Input color") + " [{0}]", True, ColTypes.Option, InputColor)
                    W(" 2) " + DoTranslation("License color") + " [{0}]", True, ColTypes.Option, LicenseColor)
                    W(" 3) " + DoTranslation("Continuable kernel error color") + " [{0}]", True, ColTypes.Option, ContKernelErrorColor)
                    W(" 4) " + DoTranslation("Uncontinuable kernel error color") + " [{0}]", True, ColTypes.Option, UncontKernelErrorColor)
                    W(" 5) " + DoTranslation("Host name color") + " [{0}]", True, ColTypes.Option, HostNameShellColor)
                    W(" 6) " + DoTranslation("User name color") + " [{0}]", True, ColTypes.Option, UserNameShellColor)
                    W(" 7) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, BackgroundColor)
                    W(" 8) " + DoTranslation("Neutral text color") + " [{0}]", True, ColTypes.Option, NeutralTextColor)
                    W(" 9) " + DoTranslation("List entry color") + " [{0}]", True, ColTypes.Option, ListEntryColor)
                    W(" 10) " + DoTranslation("List value color") + " [{0}]", True, ColTypes.Option, ListValueColor)
                    W(" 11) " + DoTranslation("Stage color") + " [{0}]", True, ColTypes.Option, StageColor)
                    W(" 12) " + DoTranslation("Error color") + " [{0}]", True, ColTypes.Option, ErrorColor)
                    W(" 13) " + DoTranslation("Warning color") + " [{0}]", True, ColTypes.Option, WarningColor)
                    W(" 14) " + DoTranslation("Option color") + " [{0}]", True, ColTypes.Option, OptionColor)
                    W(" 15) " + DoTranslation("Banner color") + " [{0}]", True, ColTypes.Option, BannerColor)
                    W(" 16) " + DoTranslation("Notification title color") + " [{0}] ", True, ColTypes.Option, NotificationTitleColor)
                    W(" 17) " + DoTranslation("Notification description color") + " [{0}] ", True, ColTypes.Option, NotificationDescriptionColor)
                    W(" 18) " + DoTranslation("Notification progress color") + " [{0}] ", True, ColTypes.Option, NotificationProgressColor)
                    W(" 19) " + DoTranslation("Notification failure color") + " [{0}] ", True, ColTypes.Option, NotificationFailureColor)
                    W(" 20) " + DoTranslation("Question color") + " [{0}] ", True, ColTypes.Option, QuestionColor)
                    W(" 21) " + DoTranslation("Success color") + " [{0}] ", True, ColTypes.Option, SuccessColor)
                    W(" 22) " + DoTranslation("User dollar color") + " [{0}] ", True, ColTypes.Option, UserDollarColor)
                    W(" 23) " + DoTranslation("Tip color") + " [{0}] ", True, ColTypes.Option, TipColor)
                    W(" 24) " + DoTranslation("Separator text color") + " [{0}] ", True, ColTypes.Option, SeparatorTextColor)
                    W(" 25) " + DoTranslation("Separator color") + " [{0}] ", True, ColTypes.Option, SeparatorColor)
                    W(" 26) " + DoTranslation("List title color") + " [{0}] ", True, ColTypes.Option, ListTitleColor)
                    W(" 27) " + DoTranslation("Development warning color") + " [{0}] ", True, ColTypes.Option, DevelopmentWarningColor)
                    W(" 28) " + DoTranslation("Stage time color") + " [{0}] ", True, ColTypes.Option, StageTimeColor)
                    W(" 29) " + DoTranslation("Progress color") + " [{0}] ", True, ColTypes.Option, ProgressColor)
                    W(" 30) " + DoTranslation("Back option color") + " [{0}] ", True, ColTypes.Option, BackOptionColor)
                    W(" 31) " + DoTranslation("Low priority border color") + " [{0}] ", True, ColTypes.Option, LowPriorityBorderColor)
                    W(" 32) " + DoTranslation("Medium priority border color") + " [{0}] ", True, ColTypes.Option, MediumPriorityBorderColor)
                    W(" 33) " + DoTranslation("High priority border color") + " [{0}] ", True, ColTypes.Option, HighPriorityBorderColor)
                Case "5" 'Filesystem
                    MaxOptions = 8
                    WriteSeparator(DoTranslation("Filesystem Settings..."), True)
                    W(vbNewLine + DoTranslation("This section lists the filesystem settings.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Filesystem sort mode") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SortMode)))
                    W(" 2) " + DoTranslation("Filesystem sort direction") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SortDirection)))
                    W(" 3) " + DoTranslation("Debug Size Quota in Bytes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DebugQuota)))
                    W(" 4) " + DoTranslation("Show Hidden Files") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(HiddenFiles)))
                    W(" 5) " + DoTranslation("Size parse mode") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FullParseMode)))
                    W(" 6) " + DoTranslation("Show progress on filesystem operations") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowFilesystemProgress)))
                    W(" 7) " + DoTranslation("Show file details in list") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowFileDetailsList)))
                    W(" 8) " + DoTranslation("Suppress unauthorized messages") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SuppressUnauthorizedMessages)))
                Case "6" 'Network
                    MaxOptions = 46
                    WriteSeparator(DoTranslation("Network Settings..."), True)
                    W(vbNewLine + DoTranslation("This section lists the network settings, like the FTP shell, the network-related command settings, and the remote debug settings.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Debug Port") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DebugPort)))
                    W(" 2) " + DoTranslation("Download Retry Times") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DownloadRetries)))
                    W(" 3) " + DoTranslation("Upload Retry Times") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(UploadRetries)))
                    W(" 4) " + DoTranslation("Show progress bar while downloading or uploading from ""get"" or ""put"" command") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowProgress)))
                    W(" 5) " + DoTranslation("Log FTP username") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FTPLoggerUsername)))
                    W(" 6) " + DoTranslation("Log FTP IP address") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FTPLoggerIP)))
                    W(" 7) " + DoTranslation("Return only first FTP profile") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FTPFirstProfileOnly)))
                    W(" 8) " + DoTranslation("Show mail message preview") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowPreview)))
                    W(" 9) " + DoTranslation("Record chat to debug log") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RecordChatToDebugLog)))
                    W(" 10) " + DoTranslation("Show SSH banner") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SSHBanner)))
                    W(" 11) " + DoTranslation("Enable RPC") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RPCEnabled)))
                    W(" 12) " + DoTranslation("RPC Port") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RPCPort)))
                    W(" 13) " + DoTranslation("Show file details in FTP list") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpShowDetailsInList)))
                    W(" 14) " + DoTranslation("Username prompt style for FTP") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpUserPromptStyle)))
                    W(" 15) " + DoTranslation("Password prompt style for FTP") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpPassPromptStyle)))
                    W(" 16) " + DoTranslation("Use first FTP profile") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpUseFirstProfile)))
                    W(" 17) " + DoTranslation("Add new connections to FTP speed dial") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpNewConnectionsToSpeedDial)))
                    W(" 18) " + DoTranslation("Try to validate secure FTP certificates") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpTryToValidateCertificate)))
                    W(" 19) " + DoTranslation("Show FTP MOTD on connection") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpShowMotd)))
                    W(" 20) " + DoTranslation("Always accept invalid FTP certificates") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpAlwaysAcceptInvalidCerts)))
                    W(" 21) " + DoTranslation("Username prompt style for mail") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_UserPromptStyle)))
                    W(" 22) " + DoTranslation("Password prompt style for mail") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_PassPromptStyle)))
                    W(" 23) " + DoTranslation("IMAP prompt style for mail") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_IMAPPromptStyle)))
                    W(" 24) " + DoTranslation("SMTP prompt style for mail") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_SMTPPromptStyle)))
                    W(" 25) " + DoTranslation("Automatically detect mail server") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_AutoDetectServer)))
                    W(" 26) " + DoTranslation("Enable mail debug") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_Debug)))
                    W(" 27) " + DoTranslation("Notify for new mail messages") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_NotifyNewMail)))
                    W(" 28) " + DoTranslation("GPG password prompt style for mail") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_GPGPromptStyle)))
                    W(" 29) " + DoTranslation("Send IMAP ping interval") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_ImapPingInterval)))
                    W(" 30) " + DoTranslation("Send SMTP ping interval") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_SmtpPingInterval)))
                    W(" 31) " + DoTranslation("Mail text format") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_TextFormat)))
                    W(" 32) " + DoTranslation("Automatically start remote debug on startup") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RDebugAutoStart)))
                    W(" 33) " + DoTranslation("Remote debug message format") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RDebugMessageFormat)))
                    W(" 34) " + DoTranslation("RSS feed URL prompt style") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RSSFeedUrlPromptStyle)))
                    W(" 35) " + DoTranslation("Auto refresh RSS feed") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RSSRefreshFeeds)))
                    W(" 36) " + DoTranslation("Auto refresh RSS feed interval") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RSSRefreshInterval)))
                    W(" 37) " + DoTranslation("Show file details in SFTP list") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SFTPShowDetailsInList)))
                    W(" 38) " + DoTranslation("Username prompt style for SFTP") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SFTPUserPromptStyle)))
                    W(" 39) " + DoTranslation("Add new connections to SFTP speed dial") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SFTPNewConnectionsToSpeedDial)))
                    W(" 40) " + DoTranslation("Ping timeout") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(PingTimeout)))
                    W(" 41) " + DoTranslation("Show extensive adapter info") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ExtensiveAdapterInformation)))
                    W(" 42) " + DoTranslation("Show general network information") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GeneralNetworkInformation)))
                    W(" 43) " + DoTranslation("Download percentage text") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DownloadPercentagePrint)))
                    W(" 44) " + DoTranslation("Upload percentage text") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(UploadPercentagePrint)))
                    W(" 45) " + DoTranslation("Recursive hashing for FTP") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FtpRecursiveHashing)))
                    W(" 46) " + DoTranslation("Maximum number of e-mails in one page") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Mail_MaxMessagesInPage)))
                Case "7" 'Screensaver
                    MaxOptions = 3
                    WriteSeparator(DoTranslation("Screensaver Settings..."), True)
                    W(vbNewLine + DoTranslation("This section lists the general screensaver settings."), True, ColTypes.Neutral)
                    W(DoTranslation("For individual screensaver settings, exit Settings and open Screensaver Settings using this command:") + " saversettings" + vbNewLine, True, ColTypes.Tip)
                    W(" 1) " + DoTranslation("Screensaver Timeout in ms") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ScrnTimeout)))
                    W(" 2) " + DoTranslation("Enable screensaver debugging") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ScreensaverDebug)))
                    W(" 3) " + DoTranslation("Ask for password after locking") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(PasswordLock)))
                Case "8" 'Misc
                    MaxOptions = 21
                    WriteSeparator(DoTranslation("Miscellaneous Settings..."), True)
                    W(vbNewLine + DoTranslation("Settings that don't fit in their appropriate sections land here.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Show Time/Date on Upper Right Corner") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(CornerTimeDate)))
                    W(" 2) " + DoTranslation("Marquee on startup") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(StartScroll)))
                    W(" 3) " + DoTranslation("Long Time and Date") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LongTimeDate)))
                    W(" 4) " + DoTranslation("Preferred Unit for Temperature") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(PreferredUnit)))
                    W(" 5) " + DoTranslation("Enable text editor autosave") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TextEdit_AutoSaveFlag)))
                    W(" 6) " + DoTranslation("Text editor autosave interval") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TextEdit_AutoSaveInterval)))
                    W(" 7) " + DoTranslation("Wrap list outputs") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WrapListOutputs)))
                    W(" 8) " + DoTranslation("Draw notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DrawBorderNotification)))
                    W(" 9) " + DoTranslation("Blacklisted mods") + " [{0}]", True, ColTypes.Option, BlacklistedModsString.Split(";").Length)
                    W(" 10) " + DoTranslation("Solver minimum number") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SolverMinimumNumber)))
                    W(" 11) " + DoTranslation("Solver maximum number") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SolverMaximumNumber)))
                    W(" 12) " + DoTranslation("Solver show input") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SolverShowInput)))
                    W(" 13) " + DoTranslation("Upper left corner character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyUpperLeftCornerChar)))
                    W(" 14) " + DoTranslation("Lower left corner character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyUpperRightCornerChar)))
                    W(" 15) " + DoTranslation("Upper right corner character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyLowerLeftCornerChar)))
                    W(" 16) " + DoTranslation("Lower right corner character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyLowerRightCornerChar)))
                    W(" 17) " + DoTranslation("Upper frame character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyUpperFrameChar)))
                    W(" 18) " + DoTranslation("Lower frame character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyLowerFrameChar)))
                    W(" 19) " + DoTranslation("Left frame character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyLeftFrameChar)))
                    W(" 20) " + DoTranslation("Right frame character for notification border") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(NotifyRightFrameChar)))
                    W(" 21) " + DoTranslation("Manual page information style") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ManpageInfoStyle)))
                Case Else 'Invalid section
                    WriteSeparator("*) ???", True)
                    W(vbNewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            End Select
            Console.WriteLine()
            W(" {0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.BackOption, MaxOptions + 1)
            Wdbg(DebugLevel.W, "Section {0} has {1} selections.", SectionNum, MaxOptions)

            'Prompt user and check for input
            W("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxOptions Then
                    If AnswerInt = 3 And SectionNum = "1" Then
                        Wdbg(DebugLevel.I, "Tried to open special section. Opening section 1.3...")
                        OpenSection("1.3")
                    ElseIf AnswerInt <> MaxOptions And SectionNum = "1.3" Then
                        Wdbg(DebugLevel.I, "Tried to open special section. Opening key {0} in section 1.3...", AnswerString)
                        OpenKey("1.3", AnswerInt)
                    ElseIf AnswerInt = 16 And SectionNum = "4" Then
                        Wdbg(DebugLevel.I, "Tried to open subsection. Opening section 4.16...")
                        OpenSection("4.16")
                    ElseIf AnswerInt <> MaxOptions And SectionNum = "4.16" Then
                        Wdbg(DebugLevel.I, "Tried to open subsection. Opening key {0} in section 4.16...", AnswerString)
                        OpenKey("4.16", AnswerInt)
                    Else
                        Wdbg(DebugLevel.I, "Opening key {0} from section {1}...", AnswerInt, SectionNum)
                        OpenKey(SectionNum, AnswerInt)
                    End If
                ElseIf AnswerInt = MaxOptions + 1 Then 'Go Back...
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    SectionFinished = True
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
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
    End Sub

    ''' <summary>
    ''' Open a key.
    ''' </summary>
    ''' <param name="Section">Section number</param>
    ''' <param name="KeyNumber">Key number</param>
    Sub OpenKey(Section As String, KeyNumber As Integer)
        Dim MaxKeyOptions As Integer = 0
        Dim KeyFinished As Boolean
        Dim KeyType As SettingsKeyType = SettingsKeyType.SUnknown
        Dim KeyVar As String = ""
        Dim KeyValue As Object = ""
        Dim VariantValue As Object = ""
        Dim VariantValueFromExternalPrompt As Boolean
        Dim ColorValue As Object = ""
        Dim AnswerString As String = ""
        Dim AnswerInt As Integer
        Dim SectionParts() As String = Section.Split(".")
        Dim ListJoinString As String = ""
        Dim TargetList As IEnumerable(Of Object)
        Dim SelectFrom As IEnumerable(Of Object)
        Dim SelectionEnumZeroBased As Boolean
        Dim NeutralizePaths As Boolean
        Dim NeutralizeRootPath As String = CurrDir

        While Not KeyFinished
            Console.Clear()
            'List Keys for specified section
            Select Case Section
                Case "1" 'General
                    Select Case KeyNumber
                        Case 1 'Prompt for Arguments on Boot
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ArgsOnBoot)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Prompt for Arguments on Boot"), True)
                            W(vbNewLine + DoTranslation("Sets up the kernel so it prompts you for argument on boot."), True, ColTypes.Neutral)
                        Case 2 'Maintenance Mode Trigger
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Maintenance)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Maintenance Mode Trigger"), True)
                            W(vbNewLine + DoTranslation("Triggers maintenance mode. This disables multiple accounts."), True, ColTypes.Neutral)
                        Case 3 'Change Root Password
                            OpenKey(Section, 1.3)
                        Case 4 'Check for Updates on Startup
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(CheckUpdateStart)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Check for Updates on Startup"), True)
                            W(vbNewLine + DoTranslation("Each startup, it will check for updates."), True, ColTypes.Neutral)
                        Case 5 'Custom Startup Banner
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(CustomBanner)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Custom Startup Banner"), True)
                            W(vbNewLine + DoTranslation("If specified, it will display customized startup banner with placeholder support. You can use this phrase for kernel version:") + " {0}", True, ColTypes.Neutral)
                        Case 6 'Change Culture when Switching Languages
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LangChangeCulture)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Change Culture when Switching Languages"), True)
                            W(vbNewLine + DoTranslation("When switching languages, change the month names, calendar, etc."), True, ColTypes.Neutral)
                        Case 7 'Culture of current language
                            MaxKeyOptions = 0
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(CurrentCult)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Culture of") + " {0}" + vbNewLine, True, CurrentLanguage)
                            W(vbNewLine + DoTranslation("Which variant of {0} is being used to change the month names, calendar, etc.?"), True, ColTypes.Neutral, CurrentLanguage)
                            SelectFrom = GetCulturesFromCurrentLang()
                            If SelectFrom.Count > 0 Then
                                For Each Cult As CultureInfo In SelectFrom
                                    MaxKeyOptions += 1
                                    W(" {0}) {1} ({2})", True, ColTypes.Option, MaxKeyOptions, Cult.Name, Cult.EnglishName)
                                Next
                            Else
                                SelectFrom = New List(Of CultureInfo) From {New CultureInfo("en-US")}
                                MaxKeyOptions = 1
                                W(" 1) en-US (English (United States))", True, ColTypes.Option)
                            End If
                        Case 8 'Show app information during boot
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowAppInfoOnBoot)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Show app information during boot"), True)
                            W(vbNewLine + DoTranslation("Shows brief information about the application on boot."), True, ColTypes.Neutral)
                        Case 9 'Parse command-line arguments
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ParseCommandLineArguments)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Parse command-line arguments"), True)
                            W(vbNewLine + DoTranslation("Parses the command-line arguments on boot."), True, ColTypes.Neutral)
                        Case 10 'Show stage finish times
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowStageFinishTimes)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Show stage finish times"), True)
                            W(vbNewLine + DoTranslation("Shows how much time did the kernel take to finish a stage."), True, ColTypes.Neutral)
                        Case 11 'Start kernel modifications on boot
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(StartKernelMods)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Start kernel modifications on boot"), True)
                            W(vbNewLine + DoTranslation("Automatically start the kernel modifications on boot."), True, ColTypes.Neutral)
                        Case 12 'Show current time before login
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowCurrentTimeBeforeLogin)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Show current time before login"), True)
                            W(vbNewLine + DoTranslation("Shows the current time, time zone, and date before logging in."), True, ColTypes.Neutral)
                        Case 13 'Notify for any fault during boot
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(NotifyFaultsBoot)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Notify for any fault during boot"), True)
                            W(vbNewLine + DoTranslation("If there is a minor fault during kernel boot, notifies the user about it."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("General Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "1.3" 'General -> Change Root Password
                    Select Case KeyNumber
                        Case 1 'Change Root Password?
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SetRootPassword)
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > " + DoTranslation("Change Root Password?"), True)
                            W(vbNewLine + DoTranslation("If the kernel is started, it will set root password."), True, ColTypes.Neutral)
                        Case 2 'Set Root Password...
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > " + DoTranslation("Set Root Password..."), True)
                            If GetConfigValueField(NameOf(SetRootPassword)) Then
                                KeyType = SettingsKeyType.SString
                                KeyVar = NameOf(RootPassword)
                                WriteSeparator(DoTranslation("Write the root password to be set. Don't worry; the password are shown as stars."), True, ColTypes.Neutral)
                            Else
                                W(vbNewLine + "X) " + DoTranslation("Enable ""Change Root Password"" to use this option. Please go back."), True, ColTypes.Error)
                            End If
                        Case Else
                            WriteSeparator(DoTranslation("General Settings...") + " > " + DoTranslation("Change Root Password...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "2" 'Hardware
                    Select Case KeyNumber
                        Case 1 'Quiet Probe
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(QuietHardwareProbe)
                            WriteSeparator(DoTranslation("Hardware Settings...") + " > " + DoTranslation("Quiet Probe"), True)
                            W(vbNewLine + DoTranslation("Keep hardware probing messages silent."), True, ColTypes.Neutral)
                        Case 2 'Full Probe
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FullHardwareProbe)
                            WriteSeparator(DoTranslation("Hardware Settings...") + " > " + DoTranslation("Full Probe"), True)
                            W(vbNewLine + DoTranslation("If true, probes all the hardware; else, will only probe the needed hardware."), True, ColTypes.Neutral)
                        Case 3 'Verbose Probe
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(VerboseHardwareProbe)
                            WriteSeparator(DoTranslation("Hardware Settings...") + " > " + DoTranslation("Verbose Probe"), True)
                            W(vbNewLine + DoTranslation("Make hardware probing messages a bit talkative."), True, ColTypes.Neutral)
                        Case 4 'Use legacy hardware listing
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(UseLegacyHardwareListing)
                            WriteSeparator(DoTranslation("Hardware Settings...") + " > " + DoTranslation("Use legacy hardware listing"), True)
                            W(vbNewLine + DoTranslation("Uses the legacy way of listing the hardware on startup."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Hardware Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "3" 'Login
                    Select Case KeyNumber
                        Case 1 'Show MOTD on Log-in
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowMOTD)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("Show MOTD on Log-in"), True)
                            W(vbNewLine + DoTranslation("Show Message of the Day before displaying login screen."), True, ColTypes.Neutral)
                        Case 2 'Clear Screen on Log-in
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ClearOnLogin)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("Clear Screen on Log-in"), True)
                            W(vbNewLine + DoTranslation("Clear screen before displaying login screen."), True, ColTypes.Neutral)
                        Case 3 'Show Available Usernames
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowAvailableUsers)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("Show available usernames"), True)
                            W(vbNewLine + DoTranslation("Shows available users if enabled."), True, ColTypes.Neutral)
                        Case 4 'MOTD Path
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(MOTDFilePath)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("MOTD Path"), True)
                            W(vbNewLine + DoTranslation("Which file is the MOTD text file? Write an absolute path to the text file."), True, ColTypes.Neutral)
                        Case 5 'MAL Path
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(MALFilePath)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("MAL Path"), True)
                            W(vbNewLine + DoTranslation("Which file is the MAL text file? Write an absolute path to the text file."), True, ColTypes.Neutral)
                        Case 6 'Username prompt style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(UsernamePrompt)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("Username prompt style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 7 'Password prompt style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(PasswordPrompt)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("Password prompt style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 8 'Show MAL on Log-in
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowMAL)
                            WriteSeparator(DoTranslation("Login Settings...") + " > " + DoTranslation("Show MAL on Log-in"), True)
                            W(vbNewLine + DoTranslation("Shows Message of the Day after displaying login screen."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Login Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "4" 'Shell
                    Select Case KeyNumber
                        Case 1 'Colored Shell
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColoredShell)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Colored Shell"), True)
                            W(vbNewLine + DoTranslation("Gives the kernel color support"), True, ColTypes.Neutral)
                        Case 2 'Simplified Help Command
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SimHelp)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Simplified Help Command"), True)
                            W(vbNewLine + DoTranslation("Simplified help command for all the shells"), True, ColTypes.Neutral)
                        Case 3 'Current Directory
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(CurrDir)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Current Directory"), True)
                            W(vbNewLine + DoTranslation("Sets the shell's current directory. Write an absolute path to any existing directory."), True, ColTypes.Neutral)
                        Case 4 'Lookup Directories
                            KeyType = SettingsKeyType.SList
                            KeyVar = NameOf(PathsToLookup)
                            ListJoinString = PathLookupDelimiter
                            TargetList = GetPathList()
                            NeutralizePaths = True
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Lookup Directories"), True)
                            W(vbNewLine + DoTranslation("Group of paths separated by the colon. It works the same as PATH. Write a full path to a folder or a folder name. When you're finished, write ""q"". Write a minus sign next to the path to remove an existing directory."), True, ColTypes.Neutral)
                        Case 5 'Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ShellPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 6 'FTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(FTPShellPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("FTP Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 7 'Mail Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(MailShellPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Mail Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 8 'SFTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(SFTPShellPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("SFTP Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 9 'RSS Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RSSShellPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("RSS Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 10 'Text Edit Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(TextEdit_PromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Text Edit Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 11 'Zip Shell Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ZipShell_PromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Zip Shell Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 12 'Test Shell Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Test_PromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Test Shell Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 13 'JSON Shell Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(JsonShell_PromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("JSON Shell Prompt Style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 14 'Probe injected commands
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProbeInjectedCommands)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Probe injected commands"), True)
                            W(vbNewLine + DoTranslation("Probes the injected commands at the start of the kernel shell."), True, ColTypes.Neutral)
                        Case 15 'Start color wheel in true color mode
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorWheelTrueColor)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Start color wheel in true color mode"), True)
                            W(vbNewLine + DoTranslation("Start color wheel in true color mode"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Shell Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "4.16" 'Shell -> Custom colors
                    Select Case KeyNumber
                        Case 1 'Input color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(InputColor)
                            ColorValue = ColorWheel(New Color(InputColor).Type = ColorType.TrueColor, If(New Color(InputColor).Type = ColorType._255Color, New Color(InputColor).PlainSequence, ConsoleColors.White), New Color(InputColor).R, New Color(InputColor).G, New Color(InputColor).B)
                        Case 2 'License color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(LicenseColor)
                            ColorValue = ColorWheel(New Color(LicenseColor).Type = ColorType.TrueColor, If(New Color(LicenseColor).Type = ColorType._255Color, New Color(LicenseColor).PlainSequence, ConsoleColors.White), New Color(LicenseColor).R, New Color(LicenseColor).G, New Color(LicenseColor).B)
                        Case 3 'Continuable kernel error color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(ContKernelErrorColor)
                            ColorValue = ColorWheel(New Color(ContKernelErrorColor).Type = ColorType.TrueColor, If(New Color(ContKernelErrorColor).Type = ColorType._255Color, New Color(ContKernelErrorColor).PlainSequence, ConsoleColors.White), New Color(ContKernelErrorColor).R, New Color(ContKernelErrorColor).G, New Color(ContKernelErrorColor).B)
                        Case 4 'Uncontinuable kernel error color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(UncontKernelErrorColor)
                            ColorValue = ColorWheel(New Color(UncontKernelErrorColor).Type = ColorType.TrueColor, If(New Color(UncontKernelErrorColor).Type = ColorType._255Color, New Color(UncontKernelErrorColor).PlainSequence, ConsoleColors.White), New Color(UncontKernelErrorColor).R, New Color(UncontKernelErrorColor).G, New Color(UncontKernelErrorColor).B)
                        Case 5 'Host name color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(HostNameShellColor)
                            ColorValue = ColorWheel(New Color(HostNameShellColor).Type = ColorType.TrueColor, If(New Color(HostNameShellColor).Type = ColorType._255Color, New Color(HostNameShellColor).PlainSequence, ConsoleColors.White), New Color(HostNameShellColor).R, New Color(HostNameShellColor).G, New Color(HostNameShellColor).B)
                        Case 6 'User name color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(UserNameShellColor)
                            ColorValue = ColorWheel(New Color(UserNameShellColor).Type = ColorType.TrueColor, If(New Color(UserNameShellColor).Type = ColorType._255Color, New Color(UserNameShellColor).PlainSequence, ConsoleColors.White), New Color(UserNameShellColor).R, New Color(UserNameShellColor).G, New Color(UserNameShellColor).B)
                        Case 7 'Background color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(BackgroundColor)
                            ColorValue = ColorWheel(New Color(BackgroundColor).Type = ColorType.TrueColor, If(New Color(BackgroundColor).Type = ColorType._255Color, New Color(BackgroundColor).PlainSequence, ConsoleColors.White), New Color(BackgroundColor).R, New Color(BackgroundColor).G, New Color(BackgroundColor).B)
                        Case 8 'Neutral text color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(NeutralTextColor)
                            ColorValue = ColorWheel(New Color(NeutralTextColor).Type = ColorType.TrueColor, If(New Color(NeutralTextColor).Type = ColorType._255Color, New Color(NeutralTextColor).PlainSequence, ConsoleColors.White), New Color(NeutralTextColor).R, New Color(NeutralTextColor).G, New Color(NeutralTextColor).B)
                        Case 9 'List entry color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(ListEntryColor)
                            ColorValue = ColorWheel(New Color(ListEntryColor).Type = ColorType.TrueColor, If(New Color(ListEntryColor).Type = ColorType._255Color, New Color(ListEntryColor).PlainSequence, ConsoleColors.White), New Color(ListEntryColor).R, New Color(ListEntryColor).G, New Color(ListEntryColor).B)
                        Case 10 'List value color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(ListValueColor)
                            ColorValue = ColorWheel(New Color(ListValueColor).Type = ColorType.TrueColor, If(New Color(ListValueColor).Type = ColorType._255Color, New Color(ListValueColor).PlainSequence, ConsoleColors.White), New Color(ListValueColor).R, New Color(ListValueColor).G, New Color(ListValueColor).B)
                        Case 11 'Stage color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(StageColor)
                            ColorValue = ColorWheel(New Color(StageColor).Type = ColorType.TrueColor, If(New Color(StageColor).Type = ColorType._255Color, New Color(StageColor).PlainSequence, ConsoleColors.White), New Color(StageColor).R, New Color(StageColor).G, New Color(StageColor).B)
                        Case 12 'Error color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(ErrorColor)
                            ColorValue = ColorWheel(New Color(ErrorColor).Type = ColorType.TrueColor, If(New Color(ErrorColor).Type = ColorType._255Color, New Color(ErrorColor).PlainSequence, ConsoleColors.White), New Color(ErrorColor).R, New Color(ErrorColor).G, New Color(ErrorColor).B)
                        Case 13 'Warning color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(WarningColor)
                            ColorValue = ColorWheel(New Color(WarningColor).Type = ColorType.TrueColor, If(New Color(WarningColor).Type = ColorType._255Color, New Color(WarningColor).PlainSequence, ConsoleColors.White), New Color(WarningColor).R, New Color(WarningColor).G, New Color(WarningColor).B)
                        Case 14 'Option color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(OptionColor)
                            ColorValue = ColorWheel(New Color(OptionColor).Type = ColorType.TrueColor, If(New Color(OptionColor).Type = ColorType._255Color, New Color(OptionColor).PlainSequence, ConsoleColors.White), New Color(OptionColor).R, New Color(OptionColor).G, New Color(OptionColor).B)
                        Case 15 'Banner color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(BannerColor)
                            ColorValue = ColorWheel(New Color(BannerColor).Type = ColorType.TrueColor, If(New Color(BannerColor).Type = ColorType._255Color, New Color(BannerColor).PlainSequence, ConsoleColors.White), New Color(BannerColor).R, New Color(BannerColor).G, New Color(BannerColor).B)
                        Case 16 'Notification title color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(NotificationTitleColor)
                            ColorValue = ColorWheel(New Color(NotificationTitleColor).Type = ColorType.TrueColor, If(New Color(NotificationTitleColor).Type = ColorType._255Color, New Color(NotificationTitleColor).PlainSequence, ConsoleColors.White), New Color(NotificationTitleColor).R, New Color(NotificationTitleColor).G, New Color(NotificationTitleColor).B)
                        Case 17 'Notification description color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(NotificationDescriptionColor)
                            ColorValue = ColorWheel(New Color(NotificationDescriptionColor).Type = ColorType.TrueColor, If(New Color(NotificationDescriptionColor).Type = ColorType._255Color, New Color(NotificationDescriptionColor).PlainSequence, ConsoleColors.White), New Color(NotificationDescriptionColor).R, New Color(NotificationDescriptionColor).G, New Color(NotificationDescriptionColor).B)
                        Case 18 'Notification progress color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(NotificationProgressColor)
                            ColorValue = ColorWheel(New Color(NotificationProgressColor).Type = ColorType.TrueColor, If(New Color(NotificationProgressColor).Type = ColorType._255Color, New Color(NotificationProgressColor).PlainSequence, ConsoleColors.White), New Color(NotificationProgressColor).R, New Color(NotificationProgressColor).G, New Color(NotificationProgressColor).B)
                        Case 19 'Notification failure color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(NotificationFailureColor)
                            ColorValue = ColorWheel(New Color(NotificationFailureColor).Type = ColorType.TrueColor, If(New Color(NotificationFailureColor).Type = ColorType._255Color, New Color(NotificationFailureColor).PlainSequence, ConsoleColors.White), New Color(NotificationFailureColor).R, New Color(NotificationFailureColor).G, New Color(NotificationFailureColor).B)
                        Case 20 'Question color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(QuestionColor)
                            ColorValue = ColorWheel(New Color(QuestionColor).Type = ColorType.TrueColor, If(New Color(QuestionColor).Type = ColorType._255Color, New Color(QuestionColor).PlainSequence, ConsoleColors.White), New Color(QuestionColor).R, New Color(QuestionColor).G, New Color(QuestionColor).B)
                        Case 21 'Success color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(SuccessColor)
                            ColorValue = ColorWheel(New Color(SuccessColor).Type = ColorType.TrueColor, If(New Color(SuccessColor).Type = ColorType._255Color, New Color(SuccessColor).PlainSequence, ConsoleColors.White), New Color(SuccessColor).R, New Color(SuccessColor).G, New Color(SuccessColor).B)
                        Case 22 'User dollar color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(UserDollarColor)
                            ColorValue = ColorWheel(New Color(UserDollarColor).Type = ColorType.TrueColor, If(New Color(UserDollarColor).Type = ColorType._255Color, New Color(UserDollarColor).PlainSequence, ConsoleColors.White), New Color(UserDollarColor).R, New Color(UserDollarColor).G, New Color(UserDollarColor).B)
                        Case 23 'Tip color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(TipColor)
                            ColorValue = ColorWheel(New Color(TipColor).Type = ColorType.TrueColor, If(New Color(TipColor).Type = ColorType._255Color, New Color(TipColor).PlainSequence, ConsoleColors.White), New Color(TipColor).R, New Color(TipColor).G, New Color(TipColor).B)
                        Case 24 'Separator text color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(SeparatorTextColor)
                            ColorValue = ColorWheel(New Color(SeparatorTextColor).Type = ColorType.TrueColor, If(New Color(SeparatorTextColor).Type = ColorType._255Color, New Color(SeparatorTextColor).PlainSequence, ConsoleColors.White), New Color(SeparatorTextColor).R, New Color(SeparatorTextColor).G, New Color(SeparatorTextColor).B)
                        Case 25 'Separator color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(SeparatorColor)
                            ColorValue = ColorWheel(New Color(SeparatorColor).Type = ColorType.TrueColor, If(New Color(SeparatorColor).Type = ColorType._255Color, New Color(SeparatorColor).PlainSequence, ConsoleColors.White), New Color(SeparatorColor).R, New Color(SeparatorColor).G, New Color(SeparatorColor).B)
                        Case 26 'List title color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(ListTitleColor)
                            ColorValue = ColorWheel(New Color(ListTitleColor).Type = ColorType.TrueColor, If(New Color(ListTitleColor).Type = ColorType._255Color, New Color(ListTitleColor).PlainSequence, ConsoleColors.White), New Color(ListTitleColor).R, New Color(ListTitleColor).G, New Color(ListTitleColor).B)
                        Case 27 'Development warning color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(DevelopmentWarningColor)
                            ColorValue = ColorWheel(New Color(DevelopmentWarningColor).Type = ColorType.TrueColor, If(New Color(DevelopmentWarningColor).Type = ColorType._255Color, New Color(DevelopmentWarningColor).PlainSequence, ConsoleColors.White), New Color(DevelopmentWarningColor).R, New Color(DevelopmentWarningColor).G, New Color(DevelopmentWarningColor).B)
                        Case 28 'Stage time color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(StageTimeColor)
                            ColorValue = ColorWheel(New Color(StageTimeColor).Type = ColorType.TrueColor, If(New Color(StageTimeColor).Type = ColorType._255Color, New Color(StageTimeColor).PlainSequence, ConsoleColors.White), New Color(StageTimeColor).R, New Color(StageTimeColor).G, New Color(StageTimeColor).B)
                        Case 29 'Progress color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(ProgressColor)
                            ColorValue = ColorWheel(New Color(ProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressColor).Type = ColorType._255Color, New Color(ProgressColor).PlainSequence, ConsoleColors.White), New Color(ProgressColor).R, New Color(ProgressColor).G, New Color(ProgressColor).B)
                        Case 30 'Back option color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(BackOptionColor)
                            ColorValue = ColorWheel(New Color(BackOptionColor).Type = ColorType.TrueColor, If(New Color(BackOptionColor).Type = ColorType._255Color, New Color(BackOptionColor).PlainSequence, ConsoleColors.White), New Color(BackOptionColor).R, New Color(BackOptionColor).G, New Color(BackOptionColor).B)
                        Case 31 'Low priority border color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(LowPriorityBorderColor)
                            ColorValue = ColorWheel(New Color(LowPriorityBorderColor).Type = ColorType.TrueColor, If(New Color(LowPriorityBorderColor).Type = ColorType._255Color, New Color(LowPriorityBorderColor).PlainSequence, ConsoleColors.White), New Color(LowPriorityBorderColor).R, New Color(LowPriorityBorderColor).G, New Color(LowPriorityBorderColor).B)
                        Case 32 'Medium priority border color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(MediumPriorityBorderColor)
                            ColorValue = ColorWheel(New Color(MediumPriorityBorderColor).Type = ColorType.TrueColor, If(New Color(MediumPriorityBorderColor).Type = ColorType._255Color, New Color(MediumPriorityBorderColor).PlainSequence, ConsoleColors.White), New Color(MediumPriorityBorderColor).R, New Color(MediumPriorityBorderColor).G, New Color(MediumPriorityBorderColor).B)
                        Case 33 'High priority border color
                            KeyType = SettingsKeyType.SColor
                            KeyVar = NameOf(HighPriorityBorderColor)
                            ColorValue = ColorWheel(New Color(HighPriorityBorderColor).Type = ColorType.TrueColor, If(New Color(HighPriorityBorderColor).Type = ColorType._255Color, New Color(HighPriorityBorderColor).PlainSequence, ConsoleColors.White), New Color(HighPriorityBorderColor).R, New Color(HighPriorityBorderColor).G, New Color(HighPriorityBorderColor).B)
                    End Select
                Case "5" 'Filesystem
                    Select Case KeyNumber
                        Case 1 'Filesystem sort mode
                            MaxKeyOptions = 5
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(SortMode)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Filesystem sort mode"), True)
                            W(vbNewLine + DoTranslation("Controls how the files will be sorted.") + vbNewLine, True, ColTypes.Neutral)
                            W(" 1) " + DoTranslation("Full name"), True, ColTypes.Option)
                            W(" 2) " + DoTranslation("File size"), True, ColTypes.Option)
                            W(" 3) " + DoTranslation("Creation time"), True, ColTypes.Option)
                            W(" 4) " + DoTranslation("Last write time"), True, ColTypes.Option)
                            W(" 5) " + DoTranslation("Last access time"), True, ColTypes.Option)
                        Case 2 'Filesystem sort direction
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(SortDirection)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Filesystem sort direction"), True)
                            W(vbNewLine + DoTranslation("Controls the direction of filesystem sorting whether it's ascending or descending.") + vbNewLine, True, ColTypes.Neutral)
                            W(" 1) " + DoTranslation("Ascending order"), True, ColTypes.Option)
                            W(" 2) " + DoTranslation("Descending order"), True, ColTypes.Option)
                        Case 3 'Debug Size Quota in Bytes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DebugQuota)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Debug Size Quota in Bytes"), True)
                            W(vbNewLine + DoTranslation("Write how many bytes can the debug log store. It must be numeric."), True, ColTypes.Neutral)
                        Case 4 'Show Hidden Files
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(HiddenFiles)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Show Hidden Files"), True)
                            W(vbNewLine + DoTranslation("Shows hidden files."), True, ColTypes.Neutral)
                        Case 5 'Size parse mode
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FullParseMode)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Size parse mode"), True)
                            W(vbNewLine + DoTranslation("If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface."), True, ColTypes.Neutral)
                        Case 6 'Show progress on filesystem operations
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowFilesystemProgress)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Show progress on filesystem operations"), True)
                            W(vbNewLine + DoTranslation("Shows what file is being processed during the filesystem operations"), True, ColTypes.Neutral)
                        Case 7 'Show file details in list
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowFileDetailsList)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Show file details in list"), True)
                            W(vbNewLine + DoTranslation("Shows the brief file details while listing files"), True, ColTypes.Neutral)
                        Case 8 'Suppress unauthorized messages
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SuppressUnauthorizedMessages)
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > " + DoTranslation("Suppress unauthorized messages"), True)
                            W(vbNewLine + DoTranslation("Hides the annoying message if the listing function tries to open an unauthorized folder"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Filesystem Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "6" 'Network
                    Select Case KeyNumber
                        Case 1 'Debug Port
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DebugPort)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Debug Port"), True)
                            W(vbNewLine + DoTranslation("Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port."), True, ColTypes.Neutral)
                        Case 2 'Download Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DownloadRetries)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Download Retry Times"), True)
                            W(vbNewLine + DoTranslation("Write how many times the ""get"" command should retry failed downloads. It must be numeric."), True, ColTypes.Neutral)
                        Case 3 'Upload Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(UploadRetries)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Upload Retry Times"), True)
                            W(vbNewLine + DoTranslation("Write how many times the ""put"" command should retry failed uploads. It must be numeric."), True, ColTypes.Neutral)
                        Case 4 'Show progress bar while downloading or uploading from "get" or "put" command
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowProgress)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show progress bar while downloading or uploading from ""get"" or ""put"" command"), True)
                            W(vbNewLine + DoTranslation("If true, it makes ""get"" or ""put"" show the progress bar while downloading or uploading."), True, ColTypes.Neutral)
                        Case 5 'Log FTP username
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPLoggerUsername)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Log FTP username"), True)
                            W(vbNewLine + DoTranslation("Whether or not to log FTP username."), True, ColTypes.Neutral)
                        Case 6 'Log FTP IP address
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPLoggerIP)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Log FTP IP address"), True)
                            W(vbNewLine + DoTranslation("Whether or not to log FTP IP address."), True, ColTypes.Neutral)
                        Case 7 'Return only first FTP profile
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FTPFirstProfileOnly)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Return only first FTP profile"), True)
                            W(vbNewLine + DoTranslation("Pick the first profile only when connecting."), True, ColTypes.Neutral)
                        Case 8 'Show mail message preview
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowPreview)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show mail message preview"), True)
                            W(vbNewLine + DoTranslation("When listing mail messages, show body preview."), True, ColTypes.Neutral)
                        Case 9 'Record chat to debug log
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RecordChatToDebugLog)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Record chat to debug log"), True)
                            W(vbNewLine + DoTranslation("Records remote debug chat to debug log."), True, ColTypes.Neutral)
                        Case 10 'Show SSH banner
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SSHBanner)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show SSH banner"), True)
                            W(vbNewLine + DoTranslation("Shows the SSH server banner on connection."), True, ColTypes.Neutral)
                        Case 11 'Enable RPC
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RPCEnabled)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Enable RPC"), True)
                            W(vbNewLine + DoTranslation("Whether or not to enable RPC."), True, ColTypes.Neutral)
                        Case 12 'RPC Port
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RPCPort)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("RPC Port"), True)
                            W(vbNewLine + DoTranslation("Write an RPC port. It must be numeric, and must not be already used. Otherwise, RPC will fail to open the port."), True, ColTypes.Neutral)
                        Case 13 'Show file details in FTP list
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpShowDetailsInList)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show file details in FTP list"), True)
                            W(vbNewLine + DoTranslation("Shows the FTP file details while listing remote directories."), True, ColTypes.Neutral)
                        Case 14 'Username prompt style for FTP
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(FtpUserPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Username prompt style for FTP"), True)
                            W(vbNewLine + DoTranslation("Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 15 'Password prompt style for FTP
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(FtpPassPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Password prompt style for FTP"), True)
                            W(vbNewLine + DoTranslation("Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 16 'Use first FTP profile
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpUseFirstProfile)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Use first FTP profile"), True)
                            W(vbNewLine + DoTranslation("Uses the first FTP profile to connect to FTP."), True, ColTypes.Neutral)
                        Case 17 'Add new connections to FTP speed dial
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpNewConnectionsToSpeedDial)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Add new connections to FTP speed dial"), True)
                            W(vbNewLine + DoTranslation("If enabled, adds a new connection to the FTP speed dial."), True, ColTypes.Neutral)
                        Case 18 'Try to validate secure FTP certificates
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpTryToValidateCertificate)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Try to validate secure FTP certificates"), True)
                            W(vbNewLine + DoTranslation("Tries to validate the FTP certificates. Turning it off is not recommended."), True, ColTypes.Neutral)
                        Case 19 'Show FTP MOTD on connection
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpShowMotd)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show FTP MOTD on connection"), True)
                            W(vbNewLine + DoTranslation("Shows the FTP message of the day on login."), True, ColTypes.Neutral)
                        Case 20 'Always accept invalid FTP certificates
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpAlwaysAcceptInvalidCerts)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Always accept invalid FTP certificates"), True)
                            W(vbNewLine + DoTranslation("Always accept invalid FTP certificates. Turning it on is not recommended as it may pose security risks."), True, ColTypes.Neutral)
                        Case 21 'Username prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_UserPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Username prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your username prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 22 'Password prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_PassPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Password prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 23 'IMAP prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_IMAPPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("IMAP prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your IMAP server prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 24 'SMTP prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_SMTPPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("SMTP prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your SMTP server prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 25 'Automatically detect mail server
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Mail_AutoDetectServer)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Automatically detect mail server"), True)
                            W(vbNewLine + DoTranslation("Automatically detect the mail server based on the given address."), True, ColTypes.Neutral)
                        Case 26 'Enable mail debug
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Mail_Debug)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Enable mail debug"), True)
                            W(vbNewLine + DoTranslation("Enables mail server debug."), True, ColTypes.Neutral)
                        Case 27 'Notify for new mail messages
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Mail_NotifyNewMail)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Notify for new mail messages"), True)
                            W(vbNewLine + DoTranslation("Notifies you for any new mail messages."), True, ColTypes.Neutral)
                        Case 28 'GPG password prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_GPGPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("GPG password prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your GPG password prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 29 'Send IMAP ping interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(Mail_ImapPingInterval)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Send IMAP ping interval"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to send the IMAP ping?"), True, ColTypes.Neutral)
                        Case 30 'Send SMTP ping interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(Mail_SmtpPingInterval)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Send SMTP ping interval"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to send the SMTP ping?"), True, ColTypes.Neutral)
                        Case 31 'Mail text format
                            MaxKeyOptions = 6
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(Mail_TextFormat)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Mail text format"), True)
                            W(vbNewLine + DoTranslation("Controls how the mail text will be shown.") + vbNewLine, True, ColTypes.Neutral)
                            W(" 1) " + DoTranslation("Plain text"), True, ColTypes.Option)
                            W(" 2) " + DoTranslation("Flowed text"), True, ColTypes.Option)
                            W(" 3) " + DoTranslation("HTML text"), True, ColTypes.Option)
                            W(" 4) " + DoTranslation("Enriched text"), True, ColTypes.Option)
                            W(" 5) " + DoTranslation("Compressed rich text"), True, ColTypes.Option)
                            W(" 6) " + DoTranslation("Rich text"), True, ColTypes.Option)
                        Case 32 'Automatically start remote debug on startup
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RDebugAutoStart)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Automatically start remote debug on startup"), True)
                            W(vbNewLine + DoTranslation("If you want remote debug to start on boot, enable this."), True, ColTypes.Neutral)
                        Case 33 'Remote debug message format
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RDebugMessageFormat)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Remote debug message format"), True)
                            W(vbNewLine + DoTranslation("Specifies the remote debug message format. {0} for name, {1} for message."), True, ColTypes.Neutral)
                        Case 34 'RSS feed URL prompt style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RSSFeedUrlPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("RSS feed URL prompt style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your RSS feed server prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 35 'Auto refresh RSS feed
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RSSRefreshFeeds)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Auto refresh RSS feed"), True)
                            W(vbNewLine + DoTranslation("Auto refresh RSS feed"), True, ColTypes.Neutral)
                        Case 36 'Auto refresh RSS feed interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RSSRefreshInterval)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Auto refresh RSS feed interval"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to refresh the RSS feed?"), True, ColTypes.Neutral)
                        Case 37 'Show file details in SFTP list
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SFTPShowDetailsInList)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show file details in SFTP list"), True)
                            W(vbNewLine + DoTranslation("Shows the SFTP file details while listing remote directories."), True, ColTypes.Neutral)
                        Case 38 'Username prompt style for SFTP
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(SFTPUserPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Username prompt style for SFTP"), True)
                            W(vbNewLine + DoTranslation("Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 39 'Add new connections to SFTP speed dial
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SFTPNewConnectionsToSpeedDial)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Add new connections to SFTP speed dial"), True)
                            W(vbNewLine + DoTranslation("If enabled, adds a new connection to the SFTP speed dial."), True, ColTypes.Neutral)
                        Case 40 'Ping timeout
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(PingTimeout)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Ping timeout"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before declaring timeout?"), True, ColTypes.Neutral)
                        Case 41 'Show extensive adapter info
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ExtensiveAdapterInformation)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show extensive adapter info"), True)
                            W(vbNewLine + DoTranslation("Prints the extensive adapter information, such as packet information."), True, ColTypes.Neutral)
                        Case 42 'Show general network information
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GeneralNetworkInformation)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show general network information"), True)
                            W(vbNewLine + DoTranslation("Shows the general information about network."), True, ColTypes.Neutral)
                        Case 43 'Download percentage text
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(DownloadPercentagePrint)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Download percentage text"), True)
                            W(vbNewLine + DoTranslation("Write how you want your download percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for downloaded size, {1} for target size, {2} for percentage."), True, ColTypes.Neutral)
                        Case 44 'Upload percentage text
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(UploadPercentagePrint)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Upload percentage text"), True)
                            W(vbNewLine + DoTranslation("Write how you want your upload percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for uploaded size, {1} for target size, {2} for percentage."), True, ColTypes.Neutral)
                        Case 45 'Recursive hashing for FTP
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpRecursiveHashing)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Recursive hashing for FTP"), True)
                            W(vbNewLine + DoTranslation("Whether to recursively hash a directory. Please note that not all the FTP servers support that."), True, ColTypes.Neutral)
                        Case 46 'Maximum number of e-mails in one page
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(Mail_MaxMessagesInPage)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Maximum number of e-mails in one page"), True)
                            W(vbNewLine + DoTranslation("Maximum number of e-mails in one page"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Network Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7" 'Screensaver
                    Select Case KeyNumber
                        Case 1 'Screensaver Timeout in ms
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ScrnTimeout)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Screensaver Timeout in ms"), True)
                            W(vbNewLine + DoTranslation("Write when to launch screensaver after specified milliseconds. It must be numeric."), True, ColTypes.Neutral)
                        Case 2 'Enable screensaver debugging
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ScreensaverDebug)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Enable screensaver debugging"), True)
                            W(vbNewLine + DoTranslation("Enables debugging for screensavers. Please note that it may quickly fill the debug log and slightly slow the screensaver down, depending on the screensaver used. Only works if kernel debugging is enabled for diagnostic purposes."), True, ColTypes.Neutral)
                        Case 3 'Ask for password after locking
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(PasswordLock)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Ask for password after locking"), True)
                            W(vbNewLine + DoTranslation("After locking the screen, ask for password"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "8" 'Misc
                    Select Case KeyNumber
                        Case 1 'Show Time/Date on Upper Right Corner
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(CornerTimeDate)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show Time/Date on Upper Right Corner"), True)
                            W(vbNewLine + DoTranslation("The time and date will be shown in the upper right corner of the screen"), True, ColTypes.Neutral)
                        Case 2 'Marquee on startup
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(StartScroll)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Marquee on startup"), True)
                            W(vbNewLine + DoTranslation("Enables eyecandy on startup"), True, ColTypes.Neutral)
                        Case 3 'Long Time and Date
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LongTimeDate)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Long Time and Date"), True)
                            W(vbNewLine + DoTranslation("The time and date will be longer, showing full month names, etc."), True, ColTypes.Neutral)
                        Case 4 'Preferred Unit for Temperature
                            MaxKeyOptions = 3
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(PreferredUnit)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Preferred Unit for Temperature"), True)
                            W(vbNewLine + DoTranslation("Select your preferred unit for temperature (this only applies to the ""weather"" command)") + vbNewLine, True, ColTypes.Neutral)
                            W(" 1) " + DoTranslation("Kelvin"), True, ColTypes.Option)
                            W(" 2) " + DoTranslation("Metric (Celsius)"), True, ColTypes.Option)
                            W(" 3) " + DoTranslation("Imperial (Fahrenheit)"), True, ColTypes.Option)
                        Case 5 'Enable text editor autosave
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(TextEdit_AutoSaveFlag)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Enable text editor autosave"), True)
                            W(vbNewLine + DoTranslation("Turns on or off the text editor autosave feature."), True, ColTypes.Neutral)
                        Case 6 'Text editor autosave interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TextEdit_AutoSaveInterval)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Text editor autosave interval"), True)
                            W(vbNewLine + DoTranslation("If autosave is enabled, the text file will be saved for each ""n"" seconds."), True, ColTypes.Neutral)
                        Case 7 'Wrap list outputs
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(WrapListOutputs)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Wrap list outputs"), True)
                            W(vbNewLine + DoTranslation("Wraps the list outputs if it seems too long for the current console geometry."), True, ColTypes.Neutral)
                        Case 8 'Draw notification border
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DrawBorderNotification)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Draw notification border"), True)
                            W(vbNewLine + DoTranslation("Covers the notification with the border."), True, ColTypes.Neutral)
                        Case 9 'Blacklisted mods
                            KeyType = SettingsKeyType.SList
                            KeyVar = NameOf(BlacklistedModsString)
                            ListJoinString = ";"
                            TargetList = GetBlacklistedMods()
                            NeutralizePaths = True
                            NeutralizeRootPath = GetKernelPath(KernelPathType.Mods)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Blacklisted mods"), True)
                            W(vbNewLine + DoTranslation("Write the filenames of the mods that will not run on startup. When you're finished, write ""q"". Write a minus sign next to the path to remove an existing mod."), True, ColTypes.Neutral)
                        Case 10 'Solver minimum number
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(SolverMinimumNumber)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Solver minimum number"), True)
                            W(vbNewLine + DoTranslation("What is the minimum number to choose?"), True, ColTypes.Neutral)
                        Case 11 'Solver maximum number
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(SolverMaximumNumber)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Solver maximum number"), True)
                            W(vbNewLine + DoTranslation("What is the maximum number to choose?"), True, ColTypes.Neutral)
                        Case 12 'Solver show input
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SolverShowInput)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Solver show input"), True)
                            W(vbNewLine + DoTranslation("Whether to show what's written in the input prompt."), True, ColTypes.Neutral)
                        Case 13 'Upper left corner character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyUpperLeftCornerChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Upper left corner character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 14 'Lower left corner character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyLowerLeftCornerChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Lower left corner character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 15 'Upper right corner character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyUpperRightCornerChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Upper right corner character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper right corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 16 'Lower right corner character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyLowerRightCornerChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Lower right corner character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower right corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 17 'Upper frame character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyUpperFrameChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Upper frame character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 18 'Lower frame character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyLowerFrameChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Lower frame character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 19 'Left frame character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyLeftFrameChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Left frame character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 20 'Right frame character for notification border
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(NotifyRightFrameChar)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Right frame character for notification border"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 21 'Manual page information style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ManpageInfoStyle)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Manual page information style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your manpage information to be. Leave blank to use default style. Placeholders are parsed. {0} for manual title, {1} for revision."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case Else
                    WriteSeparator("*) ???", True)
                    W(vbNewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            End Select

#Disable Warning BC42104
            'If the type is boolean, write the two options
            If KeyType = SettingsKeyType.SBoolean Then
                Console.WriteLine()
                MaxKeyOptions = 2
                W(" 1) " + DoTranslation("Enable"), True, ColTypes.Option)
                W(" 2) " + DoTranslation("Disable"), True, ColTypes.Option)
            End If
            Console.WriteLine()

            'Add an option to go back.
            If Not KeyType = SettingsKeyType.SVariant And Not KeyType = SettingsKeyType.SInt And Not KeyType = SettingsKeyType.SLongString And Not KeyType = SettingsKeyType.SString And Not KeyType = SettingsKeyType.SList Then
                W(" {0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.BackOption, MaxKeyOptions + 1)
            ElseIf KeyType = SettingsKeyType.SList Then
                W(DoTranslation("Current items:"), True, ColTypes.ListTitle)
                WriteList(TargetList)
                W(vbNewLine + " q) " + DoTranslation("Save Changes...") + vbNewLine, True, ColTypes.Option, MaxKeyOptions + 1)
            End If

            'Get key value
            If Not KeyType = SettingsKeyType.SUnknown Then KeyValue = GetConfigValueField(KeyVar)

            'Print debugging info
            Wdbg(DebugLevel.W, "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions)
            Wdbg(DebugLevel.W, "Target variable: {0}, Key Type: {1}, Key value: {2}, Variant Value: {3}", KeyVar, KeyType, KeyValue, VariantValue)

            'Prompt user
            If KeyNumber = 2 And Section = "1.3" And Not KeyType = SettingsKeyType.SUnknown Then
                W("> ", False, ColTypes.Input)
                AnswerString = ReadLineNoInput("*")
                Console.WriteLine()
            ElseIf KeyType = SettingsKeyType.SVariant And Not VariantValueFromExternalPrompt Then
                W("> ", False, ColTypes.Input)
                VariantValue = Console.ReadLine
                If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                Wdbg(DebugLevel.I, "User answered {0}", VariantValue)
            ElseIf Not KeyType = SettingsKeyType.SVariant And Not KeyType = SettingsKeyType.SColor Then
                If KeyType = SettingsKeyType.SList Then
                    W("> ", False, ColTypes.Input)
                    Do Until AnswerString = "q"
                        AnswerString = Console.ReadLine
                        If Not AnswerString = "q" Then
                            If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                            If Not AnswerString.StartsWith("-") Then
                                'We're not removing an item!
                                TargetList = Enumerable.Append(TargetList, AnswerString)
                            Else
                                'We're removing an item.
                                Dim DeletedItems As IEnumerable(Of Object) = Enumerable.Empty(Of Object)
                                DeletedItems = Enumerable.Append(DeletedItems, AnswerString.Substring(1))
                                TargetList = Enumerable.Except(TargetList, DeletedItems)
                            End If
                            Wdbg(DebugLevel.I, "Added answer {0} to list.", AnswerString)
                            W("> ", False, ColTypes.Input)
                        End If
                    Loop
                Else
                    W(If(KeyType = SettingsKeyType.SUnknown, "> ", "[{0}] > "), False, ColTypes.Input, KeyValue)
                    If KeyType = SettingsKeyType.SLongString Then
                        AnswerString = ReadLineLong()
                    Else
                        AnswerString = Console.ReadLine
                    End If
                    If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                    Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
                End If
            End If

            'Check for input
            Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SBoolean Then
                Wdbg(DebugLevel.I, "Answer is numeric and key is of the Boolean type.")
                If AnswerInt >= 1 And AnswerInt <= MaxKeyOptions Then
                    Wdbg(DebugLevel.I, "Translating {0} to the boolean equivalent...", AnswerInt)
                    KeyFinished = True
                    Select Case AnswerInt
                        Case 1 'True
                            Wdbg(DebugLevel.I, "Setting to True...")
                            SetConfigValueField(KeyVar, True)
                        Case 2 'False
                            Wdbg(DebugLevel.I, "Setting to False...")
                            SetConfigValueField(KeyVar, False)
                    End Select
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            ElseIf (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SInt) Or
                   (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SSelection) Then
                Wdbg(DebugLevel.I, "Answer is numeric and key is of the {0} type.", KeyType)
                If AnswerInt = MaxKeyOptions + 1 And KeyType = SettingsKeyType.SSelection Then 'Go Back...
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf KeyType = SettingsKeyType.SSelection And AnswerInt > 0 And SelectFrom IsNot Nothing Then
                    Wdbg(DebugLevel.I, "Setting variable {0} to item index {1}...", KeyVar, AnswerInt - 1)
                    KeyFinished = True
                    SetConfigValueField(KeyVar, SelectFrom(AnswerInt - 1))
                ElseIf (KeyType = SettingsKeyType.SSelection And AnswerInt > 0) Or
                       (KeyType = SettingsKeyType.SInt And AnswerInt >= 0) Then
                    If (KeyType = SettingsKeyType.SSelection And Not AnswerInt > MaxKeyOptions) Or KeyType = SettingsKeyType.SInt Then
                        If SelectionEnumZeroBased Then AnswerInt -= 1
                        Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                        KeyFinished = True
                        SetConfigValueField(KeyVar, AnswerInt)
                    ElseIf KeyType = SettingsKeyType.SSelection Then
                        Wdbg(DebugLevel.W, "Answer is not valid.")
                        W(DoTranslation("The answer may not exceed the entries shown."), True, ColTypes.Error)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                ElseIf AnswerInt = 0 Then
                    Wdbg(DebugLevel.W, "Zero is not allowed.")
                    W(DoTranslation("The answer may not be zero."), True, ColTypes.Error)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                Else
                    Wdbg(DebugLevel.W, "Negative values are disallowed.")
                    W(DoTranslation("The answer may not be negative."), True, ColTypes.Error)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            ElseIf KeyType = SettingsKeyType.SUnknown Then
                Wdbg(DebugLevel.I, "User requested exit. Returning...")
                KeyFinished = True
            ElseIf KeyType = SettingsKeyType.SString Or KeyType = SettingsKeyType.SLongString Then
                Wdbg(DebugLevel.I, "Answer is not numeric and key is of the String type. Setting variable...")

                'Check to see if written answer is empty
                If String.IsNullOrWhiteSpace(AnswerString) Then
                    Wdbg(DebugLevel.I, "Answer is nothing. Setting to {0}...", KeyValue)
                    AnswerString = KeyValue
                End If

                'Check to see if the user intended to clear the variable to make it consist of nothing
                If AnswerString.ToLower = "/clear" Then
                    Wdbg(DebugLevel.I, "User requested clear.")
                    AnswerString = ""
                End If

                'Set the value
                KeyFinished = True
                SetConfigValueField(KeyVar, AnswerString)
            ElseIf KeyType = SettingsKeyType.SList Then
                Wdbg(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...")
                KeyFinished = True
                SetConfigValueField(KeyVar, String.Join(ListJoinString, TargetList))
            ElseIf KeyType = SettingsKeyType.SVariant Then
                SetConfigValueField(KeyVar, VariantValue)
                Wdbg(DebugLevel.I, "User requested exit. Returning...")
                KeyFinished = True
            ElseIf KeyType = SettingsKeyType.SColor Then
                SetConfigValueField(KeyVar, New Color(ColorValue).PlainSequence)
                Wdbg(DebugLevel.I, "User requested exit. Returning...")
                KeyFinished = True
            Else
                Wdbg(DebugLevel.W, "Answer is not valid.")
                W(DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
#Enable Warning BC42104
        End While
    End Sub

    ''' <summary>
    ''' Sets the value of a variable to the new value dynamically
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="VariableValue">New value of variable</param>
    Public Sub SetConfigValueField(Variable As String, VariableValue As Object)
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Set the variable if found
        If TargetField IsNot Nothing Then
            'The "obj" description says this: "The object whose field value will be set."
            'Apparently, SetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg(DebugLevel.I, "Got field {0}. Setting to {1}...", TargetField.Name, VariableValue)
            TargetField.SetValue(Variable, VariableValue)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg(DebugLevel.I, "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
        End If
    End Sub

    ''' <summary>
    ''' Gets the value of a variable dynamically 
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <returns>Value of a variable</returns>
    Public Function GetConfigValueField(Variable As String) As Object
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Get the variable if found
        If TargetField IsNot Nothing Then
            'The "obj" description says this: "The object whose field value will be returned."
            'Apparently, GetValue works on modules if you specify a variable name as an object (first argument). Not only classes.
            'Unfortunately, there are no examples on the MSDN that showcase such situations; classes are being used.
            Wdbg(DebugLevel.I, "Got field {0}.", TargetField.Name)
            Return TargetField.GetValue(Variable)
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg(DebugLevel.I, "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Gets the value of a property in the type of a variable dynamically
    ''' </summary>
    ''' <param name="Variable">Variable name. Use operator NameOf to get name.</param>
    ''' <param name="Property">Property name from within the variable type</param>
    ''' <returns>Value of a property</returns>
    Public Function GetConfigPropertyValueInVariableField(Variable As String, [Property] As String) As Object
        'Get field for specified variable
        Dim TargetField As FieldInfo = GetField(Variable)

        'Get the variable if found
        If TargetField IsNot Nothing Then
            'Now, get the property
            Wdbg(DebugLevel.I, "Got field {0}.", TargetField.Name)
            Dim TargetProperty As PropertyInfo = TargetField.FieldType.GetProperty([Property])

            'Get the property value if found
            If TargetProperty IsNot Nothing Then
                Return TargetProperty.GetValue(GetConfigValueField(Variable))
            Else
                'Property not found on any of the "flag" modules.
                Wdbg(DebugLevel.I, "Property {0} not found.", [Property])
                W(DoTranslation("Property {0} is not found on any of the modules."), True, ColTypes.Error, [Property])
                Return Nothing
            End If
        Else
            'Variable not found on any of the "flag" modules.
            Wdbg(DebugLevel.I, "Field {0} not found.", Variable)
            W(DoTranslation("Variable {0} is not found on any of the modules."), True, ColTypes.Error, Variable)
            Return Nothing
        End If
    End Function

End Module
