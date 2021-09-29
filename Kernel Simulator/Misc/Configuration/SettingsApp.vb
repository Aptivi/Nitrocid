
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
        Dim BuiltinSavers As Integer = 22

        'Section-specific variables
        Dim ConfigurableScreensavers As New List(Of String)

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
                    W(" 7) " + DoTranslation("Culture of") + " {0} [{1}]", True, ColTypes.Option, CurrentLanguage, CurrentCult.Name)
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
                    MaxOptions = 3
                    WriteSeparator(DoTranslation("Hardware Settings..."), True)
                    W(vbNewLine + DoTranslation("This section changes hardware probe behavior.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Quiet Probe") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(QuietHardwareProbe)))
                    W(" 2) " + DoTranslation("Full Probe") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FullHardwareProbe)))
                    W(" 3) " + DoTranslation("Verbose Probe") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(VerboseHardwareProbe)))
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
                    MaxOptions = 15
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
                    W(" 15) " + DoTranslation("Custom colors...", CurrentLanguage), True, ColTypes.Option)
                Case "4.15" 'Custom colors...
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
                    MaxOptions = 7
                    WriteSeparator(DoTranslation("Filesystem Settings..."), True)
                    W(vbNewLine + DoTranslation("This section lists the filesystem settings.") + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Filesystem sort mode") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SortMode)))
                    W(" 2) " + DoTranslation("Filesystem sort direction") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SortDirection)))
                    W(" 3) " + DoTranslation("Debug Size Quota in Bytes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DebugQuota)))
                    W(" 4) " + DoTranslation("Show Hidden Files") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(HiddenFiles)))
                    W(" 5) " + DoTranslation("Size parse mode") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FullParseMode)))
                    W(" 6) " + DoTranslation("Show progress on filesystem operations") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowFilesystemProgress)))
                    W(" 7) " + DoTranslation("Show file details in list") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ShowFileDetailsList)))
                Case "6" 'Network
                    MaxOptions = 44
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
                Case "7" 'Screensaver
                    MaxOptions = BuiltinSavers + 3 'Screensavers + Keys
                    WriteSeparator(DoTranslation("Screensaver Settings..."), True)
                    W(vbNewLine + DoTranslation("This section lists all the screensavers and their available settings.") + vbNewLine, True, ColTypes.Neutral)

                    'Populate kernel screensavers
                    W(" 1) ColorMix...", True, ColTypes.Option)
                    W(" 2) Matrix...", True, ColTypes.Option)
                    W(" 3) GlitterMatrix...", True, ColTypes.Option)
                    W(" 4) Disco...", True, ColTypes.Option)
                    W(" 5) Lines...", True, ColTypes.Option)
                    W(" 6) GlitterColor...", True, ColTypes.Option)
                    W(" 7) BouncingText...", True, ColTypes.Option)
                    W(" 8) Dissolve...", True, ColTypes.Option)
                    W(" 9) BouncingBlock...", True, ColTypes.Option)
                    W(" 10) ProgressClock...", True, ColTypes.Option)
                    W(" 11) Lighter...", True, ColTypes.Option)
                    W(" 12) Fader...", True, ColTypes.Option)
                    W(" 13) Typo...", True, ColTypes.Option)
                    W(" 14) Wipe...", True, ColTypes.Option)
                    W(" 15) Marquee...", True, ColTypes.Option)
                    W(" 16) FaderBack...", True, ColTypes.Option)
                    W(" 17) BeatFader...", True, ColTypes.Option)
                    W(" 18) Linotypo...", True, ColTypes.Option)
                    W(" 19) Typewriter...", True, ColTypes.Option)
                    W(" 20) FlashColor...", True, ColTypes.Option)
                    W(" 21) SpotWrite...", True, ColTypes.Option)
                    W(" 22) Ramp...", True, ColTypes.Option)

                    'Populate custom screensavers
                    For Each CustomSaver As String In CustomSavers.Keys
                        If CustomSavers(CustomSaver).Screensaver.SaverSettings?.Count >= 1 Then
                            ConfigurableScreensavers.Add(CustomSaver)
                            W(" {0}) {1}...", True, ColTypes.Option, MaxOptions, CustomSaver)
                            MaxOptions += 1
                        End If
                    Next

                    'Populate general screensaver settings
                    'TODO: Separate between the normal and the screensaver-specific options.
                    W(" {0}) " + DoTranslation("Screensaver Timeout in ms") + " [{1}]", True, ColTypes.Option, MaxOptions - 2, GetConfigValueField(NameOf(ScrnTimeout)))
                    W(" {0}) " + DoTranslation("Enable screensaver debugging") + " [{1}]", True, ColTypes.Option, MaxOptions - 1, GetConfigValueField(NameOf(ScreensaverDebug)))
                    W(" {0}) " + DoTranslation("Ask for password after locking") + " [{1}]", True, ColTypes.Option, MaxOptions, GetConfigValueField(NameOf(PasswordLock)))
                Case "7.1" 'Screensaver > ColorMix
                    MaxOptions = 12
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " ColorMix." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMix255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixDelay)))
                    W(" 4) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixBackgroundColor)))
                    W(" 5) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumRedColorLevel)))
                    W(" 6) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumGreenColorLevel)))
                    W(" 7) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumBlueColorLevel)))
                    W(" 8) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumColorLevel)))
                    W(" 9) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumRedColorLevel)))
                    W(" 10) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumGreenColorLevel)))
                    W(" 11) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumBlueColorLevel)))
                    W(" 12) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumColorLevel)))
                Case "7.2" 'Screensaver > Matrix
                    MaxOptions = 1
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Matrix", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Matrix." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MatrixDelay)))
                Case "7.3" 'Screensaver > GlitterMatrix
                    MaxOptions = 3
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterMatrix", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " GlitterMatrix." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterMatrixDelay)))
                    W(" 2) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterMatrixBackgroundColor)))
                    W(" 3) " + DoTranslation("Foreground color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterMatrixForegroundColor)))
                Case "7.4" 'Screensaver > Disco
                    MaxOptions = 13
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Disco." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Disco255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoTrueColor)))
                    W(" 3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoCycleColors)))
                    W(" 4) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoDelay)))
                    W(" 5) " + DoTranslation("Use Beats Per Minute") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoUseBeatsPerMinute)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumColorLevel)))
                Case "7.5" 'Screensaver > Lines
                    MaxOptions = 13
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Lines." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Lines255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesDelay)))
                    W(" 4) " + DoTranslation("Line character") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesLineChar)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumColorLevel)))
                Case "7.6" 'Screensaver > GlitterColor
                    MaxOptions = 11
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " GlitterColor." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColor255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorDelay)))
                    W(" 4) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumRedColorLevel)))
                    W(" 5) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumGreenColorLevel)))
                    W(" 6) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumBlueColorLevel)))
                    W(" 7) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumColorLevel)))
                    W(" 8) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumRedColorLevel)))
                    W(" 9) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumGreenColorLevel)))
                    W(" 10) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumBlueColorLevel)))
                    W(" 11) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumColorLevel)))
                Case "7.7" 'Screensaver > BouncingText
                    MaxOptions = 14
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " BouncingText." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingText255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextDelay)))
                    W(" 4) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextWrite)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextBackgroundColor)))
                    W(" 6) " + DoTranslation("Foreground color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextForegroundColor)))
                    W(" 7) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumRedColorLevel)))
                    W(" 8) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumGreenColorLevel)))
                    W(" 9) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumBlueColorLevel)))
                    W(" 10) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumColorLevel)))
                    W(" 11) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumRedColorLevel)))
                    W(" 12) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumGreenColorLevel)))
                    W(" 13) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumBlueColorLevel)))
                    W(" 14) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumColorLevel)))
                Case "7.8" 'Screensaver > Dissolve
                    MaxOptions = 11
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Dissolve." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Dissolve255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveTrueColor)))
                    W(" 3) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveBackgroundColor)))
                    W(" 4) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumRedColorLevel)))
                    W(" 5) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumGreenColorLevel)))
                    W(" 6) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumBlueColorLevel)))
                    W(" 7) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumColorLevel)))
                    W(" 8) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumRedColorLevel)))
                    W(" 9) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumGreenColorLevel)))
                    W(" 10) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumBlueColorLevel)))
                    W(" 11) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumColorLevel)))
                Case "7.9" 'Screensaver > BouncingBlock
                    MaxOptions = 13
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " BouncingBlock." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlock255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockDelay)))
                    W(" 4) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockBackgroundColor)))
                    W(" 5) " + DoTranslation("Foreground color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockForegroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumColorLevel)))
                Case "7.10" 'Screensaver > ProgressClock
                    MaxOptions = 68
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " ProgressClock." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClock255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockTrueColor)))
                    W(" 3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockCycleColors)))
                    W(" 4) " + DoTranslation("Color of Seconds Bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockSecondsProgressColor)))
                    W(" 5) " + DoTranslation("Color of Minutes Bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinutesProgressColor)))
                    W(" 6) " + DoTranslation("Color of Hours Bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockHoursProgressColor)))
                    W(" 7) " + DoTranslation("Color of Information") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockProgressColor)))
                    W(" 8) " + DoTranslation("Ticks to change color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockCycleColorsTicks)))
                    W(" 9) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockDelay)))
                    W(" 10) " + DoTranslation("Upper left corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperLeftCornerCharHours)))
                    W(" 11) " + DoTranslation("Upper left corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperLeftCornerCharMinutes)))
                    W(" 12) " + DoTranslation("Upper left corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperLeftCornerCharSeconds)))
                    W(" 13) " + DoTranslation("Lower left corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerLeftCornerCharHours)))
                    W(" 14) " + DoTranslation("Lower left corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerLeftCornerCharMinutes)))
                    W(" 15) " + DoTranslation("Lower left corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerLeftCornerCharSeconds)))
                    W(" 16) " + DoTranslation("Upper right corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperRightCornerCharHours)))
                    W(" 17) " + DoTranslation("Upper right corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperRightCornerCharMinutes)))
                    W(" 18) " + DoTranslation("Upper right corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperRightCornerCharSeconds)))
                    W(" 19) " + DoTranslation("Lower right corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerRightCornerCharHours)))
                    W(" 20) " + DoTranslation("Lower right corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerRightCornerCharMinutes)))
                    W(" 21) " + DoTranslation("Lower right corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerRightCornerCharSeconds)))
                    W(" 22) " + DoTranslation("Upper frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperFrameCharHours)))
                    W(" 23) " + DoTranslation("Upper frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperFrameCharMinutes)))
                    W(" 24) " + DoTranslation("Upper frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperFrameCharSeconds)))
                    W(" 25) " + DoTranslation("Lower frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerFrameCharHours)))
                    W(" 26) " + DoTranslation("Lower frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerFrameCharMinutes)))
                    W(" 27) " + DoTranslation("Lower frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerFrameCharSeconds)))
                    W(" 28) " + DoTranslation("Left frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLeftFrameCharHours)))
                    W(" 29) " + DoTranslation("Left frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLeftFrameCharMinutes)))
                    W(" 30) " + DoTranslation("Left frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLeftFrameCharSeconds)))
                    W(" 31) " + DoTranslation("Right frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockRightFrameCharHours)))
                    W(" 32) " + DoTranslation("Right frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockRightFrameCharMinutes)))
                    W(" 33) " + DoTranslation("Right frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockRightFrameCharSeconds)))
                    W(" 34) " + DoTranslation("Information text for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockInfoTextHours)))
                    W(" 35) " + DoTranslation("Information text for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockInfoTextMinutes)))
                    W(" 36) " + DoTranslation("Information text for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockInfoTextSeconds)))
                    W(" 37) " + DoTranslation("Minimum red color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevelHours)))
                    W(" 38) " + DoTranslation("Minimum green color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevelHours)))
                    W(" 39) " + DoTranslation("Minimum blue color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevelHours)))
                    W(" 40) " + DoTranslation("Minimum color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevelHours)))
                    W(" 41) " + DoTranslation("Maximum red color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevelHours)))
                    W(" 42) " + DoTranslation("Maximum green color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevelHours)))
                    W(" 43) " + DoTranslation("Maximum blue color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevelHours)))
                    W(" 44) " + DoTranslation("Maximum color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevelHours)))
                    W(" 45) " + DoTranslation("Minimum red color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevelMinutes)))
                    W(" 46) " + DoTranslation("Minimum green color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevelMinutes)))
                    W(" 47) " + DoTranslation("Minimum blue color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevelMinutes)))
                    W(" 48) " + DoTranslation("Minimum color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevelMinutes)))
                    W(" 49) " + DoTranslation("Maximum red color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevelMinutes)))
                    W(" 50) " + DoTranslation("Maximum green color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevelMinutes)))
                    W(" 51) " + DoTranslation("Maximum blue color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevelMinutes)))
                    W(" 52) " + DoTranslation("Maximum color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevelMinutes)))
                    W(" 53) " + DoTranslation("Minimum red color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevelSeconds)))
                    W(" 54) " + DoTranslation("Minimum green color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevelSeconds)))
                    W(" 55) " + DoTranslation("Minimum blue color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevelSeconds)))
                    W(" 56) " + DoTranslation("Minimum color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevelSeconds)))
                    W(" 57) " + DoTranslation("Maximum red color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevelSeconds)))
                    W(" 58) " + DoTranslation("Maximum green color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevelSeconds)))
                    W(" 59) " + DoTranslation("Maximum blue color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevelSeconds)))
                    W(" 60) " + DoTranslation("Maximum color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevelSeconds)))
                    W(" 61) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevel)))
                    W(" 62) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevel)))
                    W(" 63) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevel)))
                    W(" 64) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevel)))
                    W(" 65) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevel)))
                    W(" 66) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevel)))
                    W(" 67) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevel)))
                    W(" 68) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevel)))
                Case "7.11" 'Screensaver > Lighter
                    MaxOptions = 13
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Lighter." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Lighter255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterDelay)))
                    W(" 4) " + DoTranslation("Max Positions Count") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaxPositions)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumColorLevel)))
                Case "7.12" 'Screensaver > Fader
                    MaxOptions = 11
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Fader." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderDelay)))
                    W(" 2) " + DoTranslation("Fade Out Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderFadeOutDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderWrite)))
                    W(" 4) " + DoTranslation("Max Fade Steps") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaxSteps)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaximumRedColorLevel)))
                    W(" 10) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaximumGreenColorLevel)))
                    W(" 11) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaximumBlueColorLevel)))
                Case "7.13" 'Screensaver > Typo
                    MaxOptions = 8
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Typo." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoDelay)))
                    W(" 2) " + DoTranslation("Write Again Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWriteAgainDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWrite)))
                    W(" 4) " + DoTranslation("Minimum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWritingSpeedMin)))
                    W(" 5) " + DoTranslation("Maximum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWritingSpeedMax)))
                    W(" 6) " + DoTranslation("Probability of typo in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoMissStrikePossibility)))
                    W(" 7) " + DoTranslation("Probability of miss in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoMissPossibility)))
                    W(" 8) " + DoTranslation("Text color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoTextColor)))
                Case "7.14" 'Screensaver > Wipe
                    MaxOptions = 13
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Wipe." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Wipe255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeDelay)))
                    W(" 4) " + DoTranslation("Wipes to change direction") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeWipesNeededToChangeDirection)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumColorLevel)))
                Case "7.15" 'Screensaver > Marquee
                    MaxOptions = 6
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Marquee." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Marquee255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeDelay)))
                    W(" 4) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeWrite)))
                    W(" 5) " + DoTranslation("Always centered") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeAlwaysCentered)))
                    W(" 6) " + DoTranslation("Use Console API") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeUseConsoleAPI)))
                    W(" 7) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeBackgroundColor)))
                    W(" 8) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumRedColorLevel)))
                    W(" 9) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumGreenColorLevel)))
                    W(" 10) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumBlueColorLevel)))
                    W(" 11) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumColorLevel)))
                    W(" 12) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumRedColorLevel)))
                    W(" 13) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumGreenColorLevel)))
                    W(" 14) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumBlueColorLevel)))
                    W(" 15) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumColorLevel)))
                Case "7.16" 'Screensaver > FaderBack
                    MaxOptions = 9
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " FaderBack." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackDelay)))
                    W(" 2) " + DoTranslation("Fade Out Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackFadeOutDelay)))
                    W(" 3) " + DoTranslation("Max Fade Steps") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaxSteps)))
                    W(" 4) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMinimumRedColorLevel)))
                    W(" 5) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMinimumGreenColorLevel)))
                    W(" 6) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMinimumBlueColorLevel)))
                    W(" 7) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaximumRedColorLevel)))
                    W(" 8) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaximumGreenColorLevel)))
                    W(" 9) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaximumBlueColorLevel)))
                Case "7.17" 'Screensaver > BeatFader
                    MaxOptions = 14
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " BeatFader." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFader255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Beats Per Minute") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderDelay)))
                    W(" 4) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderCycleColors)))
                    W(" 5) " + DoTranslation("Beat Color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderBeatColor)))
                    W(" 6) " + DoTranslation("Max Fade Steps") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaxSteps)))
                    W(" 7) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumRedColorLevel)))
                    W(" 8) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumGreenColorLevel)))
                    W(" 9) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumBlueColorLevel)))
                    W(" 10) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumColorLevel)))
                    W(" 11) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumRedColorLevel)))
                    W(" 12) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumGreenColorLevel)))
                    W(" 13) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumBlueColorLevel)))
                    W(" 14) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumColorLevel)))
                Case "7.18" 'Screensaver > Linotypo
                    MaxOptions = 12
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Linotypo." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoDelay)))
                    W(" 2) " + DoTranslation("New Screen Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoNewScreenDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoWrite)))
                    W(" 4) " + DoTranslation("Minimum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoWritingSpeedMin)))
                    W(" 5) " + DoTranslation("Maximum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoWritingSpeedMax)))
                    W(" 6) " + DoTranslation("Probability of typo in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoMissStrikePossibility)))
                    W(" 7) " + DoTranslation("Column Count") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoTextColumns)))
                    W(" 8) " + DoTranslation("Line Fill Threshold") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoEtaoinThreshold)))
                    W(" 9) " + DoTranslation("Line Fill Capping Probability in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoEtaoinCappingPossibility)))
                    W(" 10) " + DoTranslation("Line Fill Type") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoEtaoinType)))
                    W(" 11) " + DoTranslation("Probability of miss in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoMissPossibility)))
                    W(" 12) " + DoTranslation("Text color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoTextColor)))
                Case "7.19" 'Screensaver > Typewriter
                    MaxOptions = 6
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typewriter", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Typewriter." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterDelay)))
                    W(" 2) " + DoTranslation("New Screen Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterNewScreenDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterWrite)))
                    W(" 4) " + DoTranslation("Minimum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterWritingSpeedMin)))
                    W(" 5) " + DoTranslation("Maximum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterWritingSpeedMax)))
                    W(" 6) " + DoTranslation("Text color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterTextColor)))
                Case "7.20" 'Screensaver > FlashColor
                    MaxOptions = 3
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > FlashColor", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " FlashColor." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColor255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorDelay)))
                Case "7.21" 'Screensaver > SpotWrite
                    MaxOptions = 5
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > SpotWrite", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " SpotWrite." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SpotWriteDelay)))
                    W(" 2) " + DoTranslation("New Screen Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SpotWriteNewScreenDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SpotWriteWrite)))
                Case "7.22" 'Screensaver > Ramp
                    MaxOptions = 4
                    WriteSeparator(DoTranslation("Screensaver Settings...") + " > Ramp", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Ramp." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Ramp255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampDelay)))
                    W(" 4) " + DoTranslation("Next ramp interval") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampNextRampDelay)))
                Case "7." + $"{If(SectionParameters.Length <> 0, SectionParameters(0), $"{BuiltinSavers + 1}")}" 'Screensaver > a custom saver
                    Dim SaverIndex As Integer = SectionParameters(0) - BuiltinSavers - 1
                    Dim Configurables As List(Of String) = SectionParameters(1)
                    Dim OptionNumber As Integer = 1
                    If CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings IsNot Nothing Then
                        MaxOptions = CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings.Count
                        WriteSeparator(DoTranslation("Screensaver Settings...") + " > {0}" + vbNewLine, True, Configurables(SaverIndex))
                        W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " {0}." + vbNewLine, True, ColTypes.Neutral, Configurables(SaverIndex))
                        For Each Setting As String In CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings.Keys
                            W(" {0}) {1} [{2}]", True, ColTypes.Option, OptionNumber, Setting, CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings(Setting))
                            OptionNumber += 1
                        Next
                    End If
                Case "8" 'Misc
                    MaxOptions = 9
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
                    ElseIf AnswerInt = 15 And SectionNum = "4" Then
                        Wdbg(DebugLevel.I, "Tried to open subsection. Opening section 4.15...")
                        OpenSection("4.15")
                    ElseIf AnswerInt <> MaxOptions And SectionNum = "4.15" Then
                        Wdbg(DebugLevel.I, "Tried to open subsection. Opening key {0} in section 4.15...", AnswerString)
                        OpenKey("4.15", AnswerInt)
                    ElseIf AnswerInt <= BuiltinSavers And SectionNum = "7" Then
                        Wdbg(DebugLevel.I, "Tried to open subsection. Opening section 7.{0}...", AnswerString)
                        Wdbg(DebugLevel.I, "Arguments: AnswerInt: {0}, ConfigurableScreensavers: {1}", AnswerInt, ConfigurableScreensavers.Count)
                        OpenSection("7." + AnswerString, AnswerInt, ConfigurableScreensavers)
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
        Dim AnswerString As String = ""
        Dim AnswerInt As Integer
        Dim SectionParts() As String = Section.Split(".")
        Dim ListJoinString As String = ""
        Dim TargetList As IEnumerable(Of Object)
        Dim SelectFrom As IEnumerable(Of Object)
        Dim SelectionEnumZeroBased As Boolean
        Dim NeutralizePaths As Boolean
        Dim NeutralizeRootPath As String = CurrDir
        Dim BuiltinSavers As Integer = 22

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
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("MOTD Path"), True)
                            W(vbNewLine + DoTranslation("Which file is the MOTD text file? Write an absolute path to the text file."), True, ColTypes.Neutral)
                        Case 5 'MAL Path
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(MALFilePath)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("MAL Path"), True)
                            W(vbNewLine + DoTranslation("Which file is the MAL text file? Write an absolute path to the text file."), True, ColTypes.Neutral)
                        Case 6 'Username prompt style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(UsernamePrompt)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Username prompt style"), True)
                            W(vbNewLine + DoTranslation("Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 7 'Password prompt style
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(PasswordPrompt)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Password prompt style"), True)
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
                        Case Else
                            WriteSeparator(DoTranslation("Shell Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "4.15" 'Shell -> Custom colors
                    Select Case KeyNumber
                        Case 1 'Input color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(InputColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(InputColor).Type = ColorType.TrueColor, If(New Color(InputColor).Type = ColorType._255Color, New Color(InputColor).PlainSequence, ConsoleColors.White), New Color(InputColor).R, New Color(InputColor).G, New Color(InputColor).B)
                        Case 2 'License color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LicenseColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LicenseColor).Type = ColorType.TrueColor, If(New Color(LicenseColor).Type = ColorType._255Color, New Color(LicenseColor).PlainSequence, ConsoleColors.White), New Color(LicenseColor).R, New Color(LicenseColor).G, New Color(LicenseColor).B)
                        Case 3 'Continuable kernel error color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ContKernelErrorColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ContKernelErrorColor).Type = ColorType.TrueColor, If(New Color(ContKernelErrorColor).Type = ColorType._255Color, New Color(ContKernelErrorColor).PlainSequence, ConsoleColors.White), New Color(ContKernelErrorColor).R, New Color(ContKernelErrorColor).G, New Color(ContKernelErrorColor).B)
                        Case 4 'Uncontinuable kernel error color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(UncontKernelErrorColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(UncontKernelErrorColor).Type = ColorType.TrueColor, If(New Color(UncontKernelErrorColor).Type = ColorType._255Color, New Color(UncontKernelErrorColor).PlainSequence, ConsoleColors.White), New Color(UncontKernelErrorColor).R, New Color(UncontKernelErrorColor).G, New Color(UncontKernelErrorColor).B)
                        Case 5 'Host name color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(HostNameShellColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(HostNameShellColor).Type = ColorType.TrueColor, If(New Color(HostNameShellColor).Type = ColorType._255Color, New Color(HostNameShellColor).PlainSequence, ConsoleColors.White), New Color(HostNameShellColor).R, New Color(HostNameShellColor).G, New Color(HostNameShellColor).B)
                        Case 6 'User name color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(UserNameShellColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(UserNameShellColor).Type = ColorType.TrueColor, If(New Color(UserNameShellColor).Type = ColorType._255Color, New Color(UserNameShellColor).PlainSequence, ConsoleColors.White), New Color(UserNameShellColor).R, New Color(UserNameShellColor).G, New Color(UserNameShellColor).B)
                        Case 7 'Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BackgroundColor).Type = ColorType.TrueColor, If(New Color(BackgroundColor).Type = ColorType._255Color, New Color(BackgroundColor).PlainSequence, ConsoleColors.White), New Color(BackgroundColor).R, New Color(BackgroundColor).G, New Color(BackgroundColor).B)
                        Case 8 'Neutral text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(NeutralTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(NeutralTextColor).Type = ColorType.TrueColor, If(New Color(NeutralTextColor).Type = ColorType._255Color, New Color(NeutralTextColor).PlainSequence, ConsoleColors.White), New Color(NeutralTextColor).R, New Color(NeutralTextColor).G, New Color(NeutralTextColor).B)
                        Case 9 'List entry color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ListEntryColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ListEntryColor).Type = ColorType.TrueColor, If(New Color(ListEntryColor).Type = ColorType._255Color, New Color(ListEntryColor).PlainSequence, ConsoleColors.White), New Color(ListEntryColor).R, New Color(ListEntryColor).G, New Color(ListEntryColor).B)
                        Case 10 'List value color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ListValueColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ListValueColor).Type = ColorType.TrueColor, If(New Color(ListValueColor).Type = ColorType._255Color, New Color(ListValueColor).PlainSequence, ConsoleColors.White), New Color(ListValueColor).R, New Color(ListValueColor).G, New Color(ListValueColor).B)
                        Case 11 'Stage color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(StageColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(StageColor).Type = ColorType.TrueColor, If(New Color(StageColor).Type = ColorType._255Color, New Color(StageColor).PlainSequence, ConsoleColors.White), New Color(StageColor).R, New Color(StageColor).G, New Color(StageColor).B)
                        Case 12 'Error color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ErrorColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ErrorColor).Type = ColorType.TrueColor, If(New Color(ErrorColor).Type = ColorType._255Color, New Color(ErrorColor).PlainSequence, ConsoleColors.White), New Color(ErrorColor).R, New Color(ErrorColor).G, New Color(ErrorColor).B)
                        Case 13 'Warning color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(WarningColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(WarningColor).Type = ColorType.TrueColor, If(New Color(WarningColor).Type = ColorType._255Color, New Color(WarningColor).PlainSequence, ConsoleColors.White), New Color(WarningColor).R, New Color(WarningColor).G, New Color(WarningColor).B)
                        Case 14 'Option color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(OptionColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(OptionColor).Type = ColorType.TrueColor, If(New Color(OptionColor).Type = ColorType._255Color, New Color(OptionColor).PlainSequence, ConsoleColors.White), New Color(OptionColor).R, New Color(OptionColor).G, New Color(OptionColor).B)
                        Case 15 'Banner color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BannerColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BannerColor).Type = ColorType.TrueColor, If(New Color(BannerColor).Type = ColorType._255Color, New Color(BannerColor).PlainSequence, ConsoleColors.White), New Color(BannerColor).R, New Color(BannerColor).G, New Color(BannerColor).B)
                        Case 16 'Notification title color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(NotificationTitleColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(NotificationTitleColor).Type = ColorType.TrueColor, If(New Color(NotificationTitleColor).Type = ColorType._255Color, New Color(NotificationTitleColor).PlainSequence, ConsoleColors.White), New Color(NotificationTitleColor).R, New Color(NotificationTitleColor).G, New Color(NotificationTitleColor).B)
                        Case 17 'Notification description color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(NotificationDescriptionColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(NotificationDescriptionColor).Type = ColorType.TrueColor, If(New Color(NotificationDescriptionColor).Type = ColorType._255Color, New Color(NotificationDescriptionColor).PlainSequence, ConsoleColors.White), New Color(NotificationDescriptionColor).R, New Color(NotificationDescriptionColor).G, New Color(NotificationDescriptionColor).B)
                        Case 18 'Notification progress color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(NotificationProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(NotificationProgressColor).Type = ColorType.TrueColor, If(New Color(NotificationProgressColor).Type = ColorType._255Color, New Color(NotificationProgressColor).PlainSequence, ConsoleColors.White), New Color(NotificationProgressColor).R, New Color(NotificationProgressColor).G, New Color(NotificationProgressColor).B)
                        Case 19 'Notification failure color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(NotificationFailureColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(NotificationFailureColor).Type = ColorType.TrueColor, If(New Color(NotificationFailureColor).Type = ColorType._255Color, New Color(NotificationFailureColor).PlainSequence, ConsoleColors.White), New Color(NotificationFailureColor).R, New Color(NotificationFailureColor).G, New Color(NotificationFailureColor).B)
                        Case 20 'Question color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(QuestionColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(QuestionColor).Type = ColorType.TrueColor, If(New Color(QuestionColor).Type = ColorType._255Color, New Color(QuestionColor).PlainSequence, ConsoleColors.White), New Color(QuestionColor).R, New Color(QuestionColor).G, New Color(QuestionColor).B)
                        Case 21 'Success color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(SuccessColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(SuccessColor).Type = ColorType.TrueColor, If(New Color(SuccessColor).Type = ColorType._255Color, New Color(SuccessColor).PlainSequence, ConsoleColors.White), New Color(SuccessColor).R, New Color(SuccessColor).G, New Color(SuccessColor).B)
                        Case 22 'User dollar color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(UserDollarColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(UserDollarColor).Type = ColorType.TrueColor, If(New Color(UserDollarColor).Type = ColorType._255Color, New Color(UserDollarColor).PlainSequence, ConsoleColors.White), New Color(UserDollarColor).R, New Color(UserDollarColor).G, New Color(UserDollarColor).B)
                        Case 23 'Tip color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(TipColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(TipColor).Type = ColorType.TrueColor, If(New Color(TipColor).Type = ColorType._255Color, New Color(TipColor).PlainSequence, ConsoleColors.White), New Color(TipColor).R, New Color(TipColor).G, New Color(TipColor).B)
                        Case 24 'Separator text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(SeparatorTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(SeparatorTextColor).Type = ColorType.TrueColor, If(New Color(SeparatorTextColor).Type = ColorType._255Color, New Color(SeparatorTextColor).PlainSequence, ConsoleColors.White), New Color(SeparatorTextColor).R, New Color(SeparatorTextColor).G, New Color(SeparatorTextColor).B)
                        Case 25 'Separator color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(SeparatorColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(SeparatorColor).Type = ColorType.TrueColor, If(New Color(SeparatorColor).Type = ColorType._255Color, New Color(SeparatorColor).PlainSequence, ConsoleColors.White), New Color(SeparatorColor).R, New Color(SeparatorColor).G, New Color(SeparatorColor).B)
                        Case 26 'List title color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ListTitleColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ListTitleColor).Type = ColorType.TrueColor, If(New Color(ListTitleColor).Type = ColorType._255Color, New Color(ListTitleColor).PlainSequence, ConsoleColors.White), New Color(ListTitleColor).R, New Color(ListTitleColor).G, New Color(ListTitleColor).B)
                        Case 27 'Development warning color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(DevelopmentWarningColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(DevelopmentWarningColor).Type = ColorType.TrueColor, If(New Color(DevelopmentWarningColor).Type = ColorType._255Color, New Color(DevelopmentWarningColor).PlainSequence, ConsoleColors.White), New Color(DevelopmentWarningColor).R, New Color(DevelopmentWarningColor).G, New Color(DevelopmentWarningColor).B)
                        Case 28 'Stage time color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(StageTimeColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(StageTimeColor).Type = ColorType.TrueColor, If(New Color(StageTimeColor).Type = ColorType._255Color, New Color(StageTimeColor).PlainSequence, ConsoleColors.White), New Color(StageTimeColor).R, New Color(StageTimeColor).G, New Color(StageTimeColor).B)
                        Case 29 'Progress color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressColor).Type = ColorType._255Color, New Color(ProgressColor).PlainSequence, ConsoleColors.White), New Color(ProgressColor).R, New Color(ProgressColor).G, New Color(ProgressColor).B)
                        Case 30 'Back option color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BackOptionColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BackOptionColor).Type = ColorType.TrueColor, If(New Color(BackOptionColor).Type = ColorType._255Color, New Color(BackOptionColor).PlainSequence, ConsoleColors.White), New Color(BackOptionColor).R, New Color(BackOptionColor).G, New Color(BackOptionColor).B)
                        Case 31 'Low priority border color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LowPriorityBorderColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LowPriorityBorderColor).Type = ColorType.TrueColor, If(New Color(LowPriorityBorderColor).Type = ColorType._255Color, New Color(LowPriorityBorderColor).PlainSequence, ConsoleColors.White), New Color(LowPriorityBorderColor).R, New Color(LowPriorityBorderColor).G, New Color(LowPriorityBorderColor).B)
                        Case 32 'Medium priority border color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(MediumPriorityBorderColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(MediumPriorityBorderColor).Type = ColorType.TrueColor, If(New Color(MediumPriorityBorderColor).Type = ColorType._255Color, New Color(MediumPriorityBorderColor).PlainSequence, ConsoleColors.White), New Color(MediumPriorityBorderColor).R, New Color(MediumPriorityBorderColor).G, New Color(MediumPriorityBorderColor).B)
                        Case 33 'High priority border color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(HighPriorityBorderColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(HighPriorityBorderColor).Type = ColorType.TrueColor, If(New Color(HighPriorityBorderColor).Type = ColorType._255Color, New Color(HighPriorityBorderColor).PlainSequence, ConsoleColors.White), New Color(HighPriorityBorderColor).R, New Color(HighPriorityBorderColor).G, New Color(HighPriorityBorderColor).B)
                    End Select
                Case "5" 'Filesystem
                    Select Case KeyNumber
                        Case 1 'Filesystem sort mode
                            MaxKeyOptions = 5
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(SortMode)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Filesystem sort mode"), True)
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
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Filesystem sort direction"), True)
                            W(vbNewLine + DoTranslation("Controls the direction of filesystem sorting whether it's ascending or descending.") + vbNewLine, True, ColTypes.Neutral)
                            W(" 1) " + DoTranslation("Ascending order"), True, ColTypes.Option)
                            W(" 2) " + DoTranslation("Descending order"), True, ColTypes.Option)
                        Case 3 'Debug Size Quota in Bytes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DebugQuota)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Debug Size Quota in Bytes"), True)
                            W(vbNewLine + DoTranslation("Write how many bytes can the debug log store. It must be numeric."), True, ColTypes.Neutral)
                        Case 4 'Show Hidden Files
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(HiddenFiles)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show Hidden Files"), True)
                            W(vbNewLine + DoTranslation("Shows hidden files."), True, ColTypes.Neutral)
                        Case 5 'Size parse mode
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FullParseMode)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Size parse mode"), True)
                            W(vbNewLine + DoTranslation("If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface."), True, ColTypes.Neutral)
                        Case 6 'Show progress on filesystem operations
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowFilesystemProgress)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show progress on filesystem operations"), True)
                            W(vbNewLine + DoTranslation("Shows what file is being processed during the filesystem operations"), True, ColTypes.Neutral)
                        Case 7 'Show file details in list
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ShowFileDetailsList)
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Show file details in list"), True)
                            W(vbNewLine + DoTranslation("Shows the brief file details while listing files"), True, ColTypes.Neutral)
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
                            KeyVar = NameOf(FtpShowDetailsInList)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Use first FTP profile"), True)
                            W(vbNewLine + DoTranslation("Uses the first FTP profile to connect to FTP."), True, ColTypes.Neutral)
                        Case 17 'Add new connections to FTP speed dial
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpShowDetailsInList)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Add new connections to FTP speed dial"), True)
                            W(vbNewLine + DoTranslation("If enabled, adds a new connection to the FTP speed dial."), True, ColTypes.Neutral)
                        Case 18 'Try to validate secure FTP certificates
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpShowDetailsInList)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Try to validate secure FTP certificates"), True)
                            W(vbNewLine + DoTranslation("Tries to validate the FTP certificates. Turning it off is not recommended."), True, ColTypes.Neutral)
                        Case 19 'Show FTP MOTD on connection
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpShowDetailsInList)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Show FTP MOTD on connection"), True)
                            W(vbNewLine + DoTranslation("Shows the FTP message of the day on login."), True, ColTypes.Neutral)
                        Case 20 'Always accept invalid FTP certificates
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FtpShowDetailsInList)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Always accept invalid FTP certificates"), True)
                            W(vbNewLine + DoTranslation("Always accept invalid FTP certificates. Turning it on is not recommended as it may pose security risks."), True, ColTypes.Neutral)
                        Case 21 'Username prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_UserPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Username prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your username prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 22 'Password prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_PassPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Password prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 23 'IMAP prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_IMAPPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("IMAP prompt style for mail"), True)
                            W(vbNewLine + DoTranslation("Write how you want your IMAP server prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 24 'SMTP prompt style for mail
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(Mail_SMTPPromptStyle)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("SMTP prompt style for mail"), True)
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
                            WriteSeparator(DoTranslation("Miscellaneous Settings...") + " > " + DoTranslation("Mail text format"), True)
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
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Automatically start remote debug on startup"), True)
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
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Auto refresh RSS feed"), True)
                            W(vbNewLine + DoTranslation("Auto refresh RSS feed"), True, ColTypes.Neutral)
                        Case 36 'Auto refresh RSS feed interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RSSRefreshInterval)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Auto refresh RSS feed interval"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to refresh the RSS feed?"), True, ColTypes.Neutral)
                        Case 37 'Show file details in SFTP list
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SFTPShowDetailsInList)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Show file details in SFTP list"), True)
                            W(vbNewLine + DoTranslation("Shows the SFTP file details while listing remote directories."), True, ColTypes.Neutral)
                        Case 38 'Username prompt style for SFTP
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(SFTPUserPromptStyle)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Username prompt style for SFTP"), True)
                            W(vbNewLine + DoTranslation("Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed."), True, ColTypes.Neutral)
                        Case 39 'Add new connections to SFTP speed dial
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(SFTPNewConnectionsToSpeedDial)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Add new connections to SFTP speed dial"), True)
                            W(vbNewLine + DoTranslation("If enabled, adds a new connection to the SFTP speed dial."), True, ColTypes.Neutral)
                        Case 40 'Ping timeout
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(PingTimeout)
                            WriteSeparator(DoTranslation("Network Settings...") + " > " + DoTranslation("Ping timeout"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before declaring timeout?"), True, ColTypes.Neutral)
                        Case 41 'Show extensive adapter info
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ExtensiveAdapterInformation)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Show extensive adapter info"), True)
                            W(vbNewLine + DoTranslation("Prints the extensive adapter information, such as packet information."), True, ColTypes.Neutral)
                        Case 42 'Show general network information
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GeneralNetworkInformation)
                            WriteSeparator(DoTranslation("Shell Settings...") + " > " + DoTranslation("Show general network information"), True)
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
                        Case Else
                            WriteSeparator(DoTranslation("Network Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7" 'Screensaver
                    Select Case KeyNumber
                        Case BuiltinSavers + 1 'Screensaver Timeout in ms
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ScrnTimeout)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Screensaver Timeout in ms"), True)
                            W(vbNewLine + DoTranslation("Write when to launch screensaver after specified milliseconds. It must be numeric."), True, ColTypes.Neutral)
                        Case BuiltinSavers + 2 'Enable screensaver debugging
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ScreensaverDebug)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Enable screensaver debugging"), True)
                            W(vbNewLine + DoTranslation("Enables debugging for screensavers. Please note that it may quickly fill the debug log and slightly slow the screensaver down, depending on the screensaver used. Only works if kernel debugging is enabled for diagnostic purposes."), True, ColTypes.Neutral)
                        Case BuiltinSavers + 3 'Ask for password after locking
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(PasswordLock)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > " + DoTranslation("Ask for password after locking"), True)
                            W(vbNewLine + DoTranslation("After locking the screen, ask for password"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.1" 'ColorMix
                    Select Case KeyNumber
                        Case 1 'ColorMix: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMix255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for ColorMix."), True, ColTypes.Neutral)
                        Case 2 'ColorMix: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMixTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for ColorMix."), True, ColTypes.Neutral)
                        Case 3 'ColorMix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'ColorMix: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ColorMixBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ColorMixBackgroundColor).Type = ColorType.TrueColor, If(New Color(ColorMixBackgroundColor).Type = ColorType._255Color, New Color(ColorMixBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(ColorMixBackgroundColor).R, New Color(ColorMixBackgroundColor).G, New Color(ColorMixBackgroundColor).B)
                        Case 5 'ColorMix: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 6 'ColorMix: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'ColorMix: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'ColorMix: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 9 'ColorMix: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'ColorMix: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'ColorMix: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'ColorMix: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ColorMix > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.2" 'Matrix
                    Select Case KeyNumber
                        Case 1 'Matrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MatrixDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Matrix > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Matrix > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.3" 'GlitterMatrix
                    Select Case KeyNumber
                        Case 1 'GlitterMatrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterMatrixDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterMatrix > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'GlitterMatrix: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(GlitterMatrixBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(GlitterMatrixBackgroundColor).Type = ColorType.TrueColor, If(New Color(GlitterMatrixBackgroundColor).Type = ColorType._255Color, New Color(GlitterMatrixBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(GlitterMatrixBackgroundColor).R, New Color(GlitterMatrixBackgroundColor).G, New Color(GlitterMatrixBackgroundColor).B)
                        Case 3 'GlitterMatrix: Foreground color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(GlitterMatrixForegroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(GlitterMatrixForegroundColor).Type = ColorType.TrueColor, If(New Color(GlitterMatrixForegroundColor).Type = ColorType._255Color, New Color(GlitterMatrixForegroundColor).PlainSequence, ConsoleColors.Green), New Color(GlitterMatrixForegroundColor).R, New Color(GlitterMatrixForegroundColor).G, New Color(GlitterMatrixForegroundColor).B)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterMatrix > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.4" 'Disco
                    Select Case KeyNumber
                        Case 1 'Disco: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Disco255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Disco."), True, ColTypes.Neutral)
                        Case 2 'Disco: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Disco."), True, ColTypes.Neutral)
                        Case 3 'Disco: Cycle colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoCycleColors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Cycle colors"), True)
                            W(vbNewLine + DoTranslation("Disco will cycle colors when enabled. Otherwise, select random colors."), True, ColTypes.Neutral)
                        Case 4 'Disco: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 5 'Disco: Use Beats Per Second
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoUseBeatsPerMinute)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Use Beats Per Minute"), True)
                            W(vbNewLine + DoTranslation("Whether to use the Beats Per Minute unit to write the next color."), True, ColTypes.Neutral)
                        Case 6 'Disco: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Disco: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Disco: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Disco: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Disco: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Disco: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Disco: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Disco: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Disco > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.5" 'Lines
                    Select Case KeyNumber
                        Case 1 'Lines: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lines255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Lines."), True, ColTypes.Neutral)
                        Case 2 'Lines: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LinesTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Lines."), True, ColTypes.Neutral)
                        Case 3 'Lines: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Lines: Line character
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(LinesLineChar)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Line character"), True)
                            W(vbNewLine + DoTranslation("A character to form a line. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 5 'Lines: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LinesBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LinesBackgroundColor).Type = ColorType.TrueColor, If(New Color(LinesBackgroundColor).Type = ColorType._255Color, New Color(LinesBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(LinesBackgroundColor).R, New Color(LinesBackgroundColor).G, New Color(LinesBackgroundColor).B)
                        Case 6 'Lines: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Lines: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Lines: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Lines: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Lines: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Lines: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Lines: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Lines: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.6" 'GlitterColor
                    Select Case KeyNumber
                        Case 1 'GlitterColor: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColor255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for GlitterColor."), True, ColTypes.Neutral)
                        Case 2 'GlitterColor: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColorTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for GlitterColor."), True, ColTypes.Neutral)
                        Case 3 'GlitterColor: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'GlitterColor: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 5 'GlitterColor: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 6 'GlitterColor: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'GlitterColor: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 8 'GlitterColor: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'GlitterColor: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'GlitterColor: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'GlitterColor: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > GlitterColor > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.7" 'BouncingText
                    Select Case KeyNumber
                        Case 1 'BouncingText: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingText255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for BouncingText."), True, ColTypes.Neutral)
                        Case 2 'BouncingText: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingTextTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for BouncingText."), True, ColTypes.Neutral)
                        Case 3 'BouncingText: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'BouncingText: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(BouncingTextWrite)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Shorter is better."), True, ColTypes.Neutral)
                        Case 5 'BouncingText: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingTextBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingTextBackgroundColor).Type = ColorType.TrueColor, If(New Color(BouncingTextBackgroundColor).Type = ColorType._255Color, New Color(BouncingTextBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(BouncingTextBackgroundColor).R, New Color(BouncingTextBackgroundColor).G, New Color(BouncingTextBackgroundColor).B)
                        Case 6 'BouncingText: Foreground color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingTextForegroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingTextForegroundColor).Type = ColorType.TrueColor, If(New Color(BouncingTextForegroundColor).Type = ColorType._255Color, New Color(BouncingTextForegroundColor).PlainSequence, ConsoleColors.White), New Color(BouncingTextForegroundColor).R, New Color(BouncingTextForegroundColor).G, New Color(BouncingTextForegroundColor).B)
                        Case 7 'BouncingText: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'BouncingText: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'BouncingText: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'BouncingText: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 11 'BouncingText: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BouncingText: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'BouncingText: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 14 'BouncingText: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingText > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.8" 'Dissolve
                    Select Case KeyNumber
                        Case 1 'Dissolve: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Dissolve255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Dissolve."), True, ColTypes.Neutral)
                        Case 2 'Dissolve: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DissolveTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Dissolve."), True, ColTypes.Neutral)
                        Case 5 'Dissolve: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(DissolveBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(DissolveBackgroundColor).Type = ColorType.TrueColor, If(New Color(DissolveBackgroundColor).Type = ColorType._255Color, New Color(DissolveBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(DissolveBackgroundColor).R, New Color(DissolveBackgroundColor).G, New Color(DissolveBackgroundColor).B)
                        Case 6 'Dissolve: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Dissolve: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Dissolve: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Dissolve: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Dissolve: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Dissolve: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Dissolve: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Dissolve: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Dissolve > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.9" 'BouncingBlock
                    Select Case KeyNumber
                        Case 1 'BouncingBlock: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlock255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for BouncingBlock."), True, ColTypes.Neutral)
                        Case 2 'BouncingBlock: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlockTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for BouncingBlock."), True, ColTypes.Neutral)
                        Case 3 'BouncingBlock: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'BouncingBlock: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingBlockBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingBlockBackgroundColor).Type = ColorType.TrueColor, If(New Color(BouncingBlockBackgroundColor).Type = ColorType._255Color, New Color(BouncingBlockBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(BouncingBlockBackgroundColor).R, New Color(BouncingBlockBackgroundColor).G, New Color(BouncingBlockBackgroundColor).B)
                        Case 5 'BouncingBlock: Foreground color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingBlockForegroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingBlockForegroundColor).Type = ColorType.TrueColor, If(New Color(BouncingBlockForegroundColor).Type = ColorType._255Color, New Color(BouncingBlockForegroundColor).PlainSequence, ConsoleColors.White), New Color(BouncingBlockForegroundColor).R, New Color(BouncingBlockForegroundColor).G, New Color(BouncingBlockForegroundColor).B)
                        Case 6 'BouncingBlock: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'BouncingBlock: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'BouncingBlock: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'BouncingBlock: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'BouncingBlock: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'BouncingBlock: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BouncingBlock: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'BouncingBlock: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BouncingBlock > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.10" 'ProgressClock
                    Select Case KeyNumber
                        Case 1 'ProgressClock: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClock255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for ProgressClock."), True, ColTypes.Neutral)
                        Case 2 'ProgressClock: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for ProgressClock."), True, ColTypes.Neutral)
                        Case 3 'ProgressClock: Cycle colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockCycleColors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Cycle colors"), True)
                            W(vbNewLine + DoTranslation("ProgressClock will select random colors if it's enabled. Otherwise, use colors from config."), True, ColTypes.Neutral)
                        Case 4 'ProgressClock: Color of Seconds Bar
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockSecondsProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockSecondsProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockSecondsProgressColor).Type = ColorType._255Color, New Color(ProgressClockSecondsProgressColor).PlainSequence, ConsoleColors.DarkBlue), New Color(ProgressClockSecondsProgressColor).R, New Color(ProgressClockSecondsProgressColor).G, New Color(ProgressClockSecondsProgressColor).B)
                        Case 5 'ProgressClock: Color of Minutes Bar
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockMinutesProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockMinutesProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockMinutesProgressColor).Type = ColorType._255Color, New Color(ProgressClockMinutesProgressColor).PlainSequence, ConsoleColors.DarkMagenta), New Color(ProgressClockMinutesProgressColor).R, New Color(ProgressClockMinutesProgressColor).G, New Color(ProgressClockMinutesProgressColor).B)
                        Case 6 'ProgressClock: Color of Hours Bar
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockHoursProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockHoursProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockHoursProgressColor).Type = ColorType._255Color, New Color(ProgressClockHoursProgressColor).PlainSequence, ConsoleColors.DarkCyan), New Color(ProgressClockHoursProgressColor).R, New Color(ProgressClockHoursProgressColor).G, New Color(ProgressClockHoursProgressColor).B)
                        Case 7 'ProgressClock: Color of Information
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockProgressColor).Type = ColorType._255Color, New Color(ProgressClockProgressColor).PlainSequence, ConsoleColors.Gray), New Color(ProgressClockProgressColor).R, New Color(ProgressClockProgressColor).G, New Color(ProgressClockProgressColor).B)
                        Case 8 'ProgressClock: Ticks to change color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockCycleColorsTicks)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Ticks to change color"), True)
                            W(vbNewLine + DoTranslation("If color cycling is enabled, how many ticks before changing colors in ProgressClock? 1 tick = 0.5 seconds"), True, ColTypes.Neutral)
                        Case 9 'ProgressClock: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 10 'ProgressClock: Upper left corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperLeftCornerCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper left corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 11 'ProgressClock: Upper left corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperLeftCornerCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper left corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 12 'ProgressClock: Upper left corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperLeftCornerCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper left corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 13 'ProgressClock: Lower left corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerLeftCornerCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower left corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 14 'ProgressClock: Lower left corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerLeftCornerCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower left corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 15 'ProgressClock: Lower left corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerLeftCornerCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower left corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 16 'ProgressClock: Upper right corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperRightCornerCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper right corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 17 'ProgressClock: Upper right corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperRightCornerCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper right corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 18 'ProgressClock: Upper right corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperRightCornerCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper right corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 19 'ProgressClock: Lower right corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerRightCornerCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower right corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 20 'ProgressClock: Lower right corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerRightCornerCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower right corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 21 'ProgressClock: Lower right corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerRightCornerCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower right corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 22 'ProgressClock: Upper frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperFrameCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 23 'ProgressClock: Upper frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperFrameCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 24 'ProgressClock: Upper frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperFrameCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Upper frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 25 'ProgressClock: Lower frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerFrameCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 26 'ProgressClock: Lower frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerFrameCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 27 'ProgressClock: Lower frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerFrameCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Lower frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 28 'ProgressClock: Left frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLeftFrameCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Left frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 29 'ProgressClock: Left frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLeftFrameCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Left frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 30 'ProgressClock: Left frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLeftFrameCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Left frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 31 'ProgressClock: Right frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockRightFrameCharHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Right frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 32 'ProgressClock: Right frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockRightFrameCharMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Right frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 33 'ProgressClock: Right frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockRightFrameCharSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Right frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 34 'ProgressClock: Information text for hours
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockInfoTextHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Information text for hours"), True)
                            W(vbNewLine + DoTranslation("Write how your information text for the current hour shows. {0} for current hour out of 24 hours."), True, ColTypes.Neutral)
                        Case 35 'ProgressClock: Information text for minutes
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockInfoTextMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Information text for minutes"), True)
                            W(vbNewLine + DoTranslation("Write how your information text for the current minute shows. {0} for current minute out of 60 minutes."), True, ColTypes.Neutral)
                        Case 36 'ProgressClock: Information text for seconds
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockInfoTextSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Information text for seconds"), True)
                            W(vbNewLine + DoTranslation("Write how your information text for the current second shows. {0} for current second out of 60 seconds."), True, ColTypes.Neutral)
                        Case 37 'ProgressClock: Minimum red color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum red color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 38 'ProgressClock: Minimum green color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum green color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 39 'ProgressClock: Minimum blue color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum blue color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 44 'ProgressClock: Minimum color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 41 'ProgressClock: Maximum red color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum red color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 42 'ProgressClock: Maximum green color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum green color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 43 'ProgressClock: Maximum blue color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum blue color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 44 'ProgressClock: Maximum color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevelHours)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 45 'ProgressClock: Minimum red color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum red color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 46 'ProgressClock: Minimum green color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum green color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 47 'ProgressClock: Minimum blue color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum blue color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 48 'ProgressClock: Minimum color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 49 'ProgressClock: Maximum red color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum red color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 50 'ProgressClock: Maximum green color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum green color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 51 'ProgressClock: Maximum blue color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum blue color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 52 'ProgressClock: Maximum color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevelMinutes)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 53 'ProgressClock: Minimum red color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum red color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 54 'ProgressClock: Minimum green color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum green color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 55 'ProgressClock: Minimum blue color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum blue color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 56 'ProgressClock: Minimum color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 57 'ProgressClock: Maximum red color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum red color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 58 'ProgressClock: Maximum green color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum green color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 59 'ProgressClock: Maximum blue color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum blue color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 60 'ProgressClock: Maximum color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevelSeconds)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 61 'ProgressClock: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 62 'ProgressClock: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 63 'ProgressClock: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 64 'ProgressClock: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 65 'ProgressClock: Maximum red color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 66 'ProgressClock: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 67 'ProgressClock: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 68 'ProgressClock: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > ProgressClock > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.11" 'Lighter
                    Select Case KeyNumber
                        Case 1 'Lighter: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lighter255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Lighter."), True, ColTypes.Neutral)
                        Case 2 'Lighter: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LighterTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Lighter."), True, ColTypes.Neutral)
                        Case 3 'Lighter: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Lighter: Max Positions Count
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaxPositions)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Max Positions Count"), True)
                            W(vbNewLine + DoTranslation("How many positions are lit before dimming?"), True, ColTypes.Neutral)
                        Case 5 'Lighter: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LighterBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LighterBackgroundColor).Type = ColorType.TrueColor, If(New Color(LighterBackgroundColor).Type = ColorType._255Color, New Color(LighterBackgroundColor).PlainSequence, ConsoleColors.White), New Color(LighterBackgroundColor).R, New Color(LighterBackgroundColor).G, New Color(LighterBackgroundColor).B)
                        Case 6 'Lighter: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Lighter: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Lighter: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Lighter: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Lighter: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Lighter: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Lighter: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Lighter: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lighter > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.12" 'Fader
                    Select Case KeyNumber
                        Case 1 'Fader: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Fader: Fade Out Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderFadeOutDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Fade Out Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before fading out text?"), True, ColTypes.Neutral)
                        Case 3 'Fader: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(FaderWrite)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Shorter is better."), True, ColTypes.Neutral)
                        Case 4 'Fader: Max Fade Steps
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaxSteps)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Max Fade Steps"), True)
                            W(vbNewLine + DoTranslation("How many fade steps to do?"), True, ColTypes.Neutral)
                        Case 5 'Fader: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(FaderBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(FaderBackgroundColor).Type = ColorType.TrueColor, If(New Color(FaderBackgroundColor).Type = ColorType._255Color, New Color(FaderBackgroundColor).PlainSequence, ConsoleColors.White), New Color(FaderBackgroundColor).R, New Color(FaderBackgroundColor).G, New Color(FaderBackgroundColor).B)
                        Case 6 'Fader: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Fader: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Fader: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Fader: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'Fader: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Fader: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Fader > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.13" 'Typo
                    Select Case KeyNumber
                        Case 1 'Typo: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Typo: Write Again Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWriteAgainDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Write Again Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing text again?"), True, ColTypes.Neutral)
                        Case 3 'Typo: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(TypoWrite)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                        Case 4 'Typo: Minimum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWritingSpeedMin)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Minimum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Minimum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 5 'Typo: Maximum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWritingSpeedMax)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Maximum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Maximum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 6 'Typo: Probability of typo in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoMissStrikePossibility)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Probability of typo in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of typo in percent"), True, ColTypes.Neutral)
                        Case 7 'Typo: Probability of miss in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoMissPossibility)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > " + DoTranslation("Probability of miss in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of miss in percent"), True, ColTypes.Neutral)
                        Case 8 'Typo: Text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(TypoTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(TypoTextColor).Type = ColorType.TrueColor, If(New Color(TypoTextColor).Type = ColorType._255Color, New Color(TypoTextColor).PlainSequence, ConsoleColors.White), New Color(TypoTextColor).R, New Color(TypoTextColor).G, New Color(TypoTextColor).B)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typo > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.14" 'Wipe
                    Select Case KeyNumber
                        Case 1 'Wipe: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Wipe255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Wipe."), True, ColTypes.Neutral)
                        Case 2 'Wipe: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(WipeTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Wipe."), True, ColTypes.Neutral)
                        Case 3 'Wipe: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Wipe: Wipes to change direction
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeWipesNeededToChangeDirection)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Wipes to change direction"), True)
                            W(vbNewLine + DoTranslation("How many wipes to do before changing direction randomly?"), True, ColTypes.Neutral)
                        Case 5 'Wipe: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(WipeBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(WipeBackgroundColor).Type = ColorType.TrueColor, If(New Color(WipeBackgroundColor).Type = ColorType._255Color, New Color(WipeBackgroundColor).PlainSequence, ConsoleColors.White), New Color(WipeBackgroundColor).R, New Color(WipeBackgroundColor).G, New Color(WipeBackgroundColor).B)
                        Case 6 'Wipe: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Wipe: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Wipe: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Wipe: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Wipe: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Wipe: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Wipe: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Wipe: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Wipe > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.15" 'Marquee
                    Select Case KeyNumber
                        Case 1 'Marquee: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Marquee255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Marquee."), True, ColTypes.Neutral)
                        Case 2 'Marquee: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Marquee."), True, ColTypes.Neutral)
                        Case 3 'Marquee: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Marquee: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(MarqueeWrite)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown."), True, ColTypes.Neutral)
                        Case 5 'Marquee: Always centered
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeAlwaysCentered)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Always centered"), True)
                            W(vbNewLine + DoTranslation("Whether the text shown on the marquee is always centered."), True, ColTypes.Neutral)
                        Case 6 'Marquee: Use Console API
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeUseConsoleAPI)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Use Console API"), True)
                            W(vbNewLine + DoTranslation("Whether to use the Console API to clear text or to use the faster line clearing VT sequence. If False, Marquee will use the appropriate VT sequence. Otherwise, it will use the probably slower Console API."), True, ColTypes.Neutral)
                        Case 7 'Marquee: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(MarqueeBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(MarqueeBackgroundColor).Type = ColorType.TrueColor, If(New Color(MarqueeBackgroundColor).Type = ColorType._255Color, New Color(MarqueeBackgroundColor).PlainSequence, ConsoleColors.White), New Color(MarqueeBackgroundColor).R, New Color(MarqueeBackgroundColor).G, New Color(MarqueeBackgroundColor).B)
                        Case 8 'Marquee: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Marquee: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'Marquee: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Marquee: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 12 'Marquee: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Marquee: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 14 'Marquee: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 15 'Marquee: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Marquee > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.16" 'FaderBack
                    Select Case KeyNumber
                        Case 1 'FaderBack: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'FaderBack: Fade Out Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackFadeOutDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Fade Out Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before fading out text?"), True, ColTypes.Neutral)
                        Case 3 'FaderBack: Max Fade Steps
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaxSteps)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Max Fade Steps"), True)
                            W(vbNewLine + DoTranslation("How many fade steps to do?"), True, ColTypes.Neutral)
                        Case 4 'FaderBack: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 5 'FaderBack: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 6 'FaderBack: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'FaderBack: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'FaderBack: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'FaderBack: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FaderBack > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.17" 'BeatFader
                    Select Case KeyNumber
                        Case 1 'BeatFader: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BeatFader255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for BeatFader."), True, ColTypes.Neutral)
                        Case 2 'BeatFader: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BeatFaderTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for BeatFader."), True, ColTypes.Neutral)
                        Case 3 'BeatFader: Delay in Beats Per Minute
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Delay in Beats Per Minute"), True)
                            W(vbNewLine + DoTranslation("How many beats per minute to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'BeatFader: Cycle colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BeatFaderCycleColors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Cycle colors"), True)
                            W(vbNewLine + DoTranslation("BeatFader will select random colors if it's enabled. Otherwise, use colors from config."), True, ColTypes.Neutral)
                        Case 5 'BeatFader: Beat Color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BeatFaderBeatColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BeatFaderBeatColor).Type = ColorType.TrueColor, If(New Color(BeatFaderBeatColor).Type = ColorType._255Color, New Color(BeatFaderBeatColor).PlainSequence, ConsoleColors.NavyBlue), New Color(BeatFaderBeatColor).R, New Color(BeatFaderBeatColor).G, New Color(BeatFaderBeatColor).B)
                        Case 6 'BeatFader: Max Fade Steps
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaxSteps)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Max Fade Steps"), True)
                            W(vbNewLine + DoTranslation("How many fade steps to do?"), True, ColTypes.Neutral)
                        Case 7 'BeatFader: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'BeatFader: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'BeatFader: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'BeatFader: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 11 'BeatFader: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumRedColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BeatFader: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumGreenColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BeatFader: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumBlueColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 14 'BeatFader: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumColorLevel)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > BeatFader > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.18" 'Linotypo
                    Select Case KeyNumber
                        Case 1 'Linotypo: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Linotypo: New Screen Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoNewScreenDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("New Screen Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing the text in the new screen again?"), True, ColTypes.Neutral)
                        Case 3 'Linotypo: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(LinotypoWrite)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                            W(DoTranslation("This screensaver supports written text on file. Pass the complete file path to this field, and the screensaver will display the contents of the file appropriately."), True, ColTypes.Neutral)
                        Case 4 'Linotypo: Minimum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoWritingSpeedMin)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Minimum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Minimum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 5 'Linotypo: Maximum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoWritingSpeedMax)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Maximum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Maximum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 6 'Linotypo: Probability of typo in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoMissStrikePossibility)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Probability of typo in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of typo in percent"), True, ColTypes.Neutral)
                        Case 7 'Linotypo: Column Count
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoTextColumns)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Column Count"), True)
                            W(vbNewLine + DoTranslation("The text columns to be printed."), True, ColTypes.Neutral)
                        Case 8 'Linotypo: Line Fill Threshold
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoEtaoinThreshold)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Line Fill Threshold"), True)
                            W(vbNewLine + DoTranslation("How many characters to write before triggering the ""line fill""?"), True, ColTypes.Neutral)
                        Case 9 'Linotypo: Line Fill Capping Probability in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoEtaoinCappingPossibility)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Line Fill Capping Probability in percent"), True)
                            W(vbNewLine + DoTranslation("Possibility that the line fill pattern will be printed in all caps in percent"), True, ColTypes.Neutral)
                        Case 10 'Linotypo: Line Fill Type
                            MaxKeyOptions = 3
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(LinotypoEtaoinType)
                            SelectionEnumZeroBased = True
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Line Fill Type"), True)
                            W(vbNewLine + DoTranslation("Line fill pattern type"), True, ColTypes.Neutral)
                            W(" 1) " + DoTranslation("Common Pattern"), True, ColTypes.Option)
                            W(" 2) " + DoTranslation("Complete Pattern"), True, ColTypes.Option)
                            W(" 3) " + DoTranslation("Random Pattern"), True, ColTypes.Option)
                        Case 11 'Linotypo: Probability of miss in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoMissPossibility)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > " + DoTranslation("Probability of miss in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of miss in percent"), True, ColTypes.Neutral)
                        Case 12 'Linotypo: Text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LinotypoTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LinotypoTextColor).Type = ColorType.TrueColor, If(New Color(LinotypoTextColor).Type = ColorType._255Color, New Color(LinotypoTextColor).PlainSequence, ConsoleColors.White), New Color(LinotypoTextColor).R, New Color(LinotypoTextColor).G, New Color(LinotypoTextColor).B)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Linotypo > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.19" 'Typewriter
                    Select Case KeyNumber
                        Case 1 'Typewriter: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typewriter > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Typewriter: New Screen Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterNewScreenDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typewriter > " + DoTranslation("New Screen Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing the text in the new screen again?"), True, ColTypes.Neutral)
                        Case 3 'Typewriter: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(TypewriterWrite)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typewriter > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                            W(DoTranslation("This screensaver supports written text on file. Pass the complete file path to this field, and the screensaver will display the contents of the file appropriately."), True, ColTypes.Neutral)
                        Case 4 'Typewriter: Minimum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterWritingSpeedMin)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typewriter > " + DoTranslation("Minimum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Minimum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 5 'Typewriter: Maximum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterWritingSpeedMax)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typewriter > " + DoTranslation("Maximum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Maximum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 6 'Typewriter: Text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(TypewriterTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(TypewriterTextColor).Type = ColorType.TrueColor, If(New Color(TypewriterTextColor).Type = ColorType._255Color, New Color(TypewriterTextColor).PlainSequence, ConsoleColors.White), New Color(TypewriterTextColor).R, New Color(TypewriterTextColor).G, New Color(TypewriterTextColor).B)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Typewriter > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.20" 'FlashColor
                    Select Case KeyNumber
                        Case 1 'FlashColor: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FlashColor255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FlashColor > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for FlashColor."), True, ColTypes.Neutral)
                        Case 2 'FlashColor: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FlashColorTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FlashColor > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for FlashColor."), True, ColTypes.Neutral)
                        Case 3 'FlashColor: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > FlashColor > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7.21" 'SpotWrite
                    Select Case KeyNumber
                        Case 1 'SpotWrite: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(SpotWriteDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > SpotWrite > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'SpotWrite: New Screen Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(SpotWriteNewScreenDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > SpotWrite > " + DoTranslation("New Screen Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing the text in the new screen again?"), True, ColTypes.Neutral)
                        Case 3 'SpotWrite: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(SpotWriteWrite)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > SpotWrite > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                            W(DoTranslation("This screensaver supports written text on file. Pass the complete file path to this field, and the screensaver will display the contents of the file appropriately."), True, ColTypes.Neutral)
                    End Select
                Case "7.22" 'Ramp
                    Select Case KeyNumber
                        Case 1 'Ramp: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Ramp255Colors)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Ramp > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Ramp."), True, ColTypes.Neutral)
                        Case 2 'Ramp: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RampTrueColor)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Ramp > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Ramp."), True, ColTypes.Neutral)
                        Case 3 'Ramp: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Ramp > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Ramp: Next ramp interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampNextRampDelay)
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Ramp > " + DoTranslation("Next ramp interval"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before filling in the next ramp?"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator(DoTranslation("Screensaver Settings...") + " > Lines > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7." + $"{If(SectionParts.Length > 1, SectionParts(1), $"{BuiltinSavers + 1}")}" 'Custom saver
                    Dim SaverIndex As Integer = SectionParts(1) - BuiltinSavers - 1
                    Dim SaverSettings As Dictionary(Of String, Object) = CustomSavers.Values(SaverIndex).Screensaver.SaverSettings
                    Dim KeyIndex As Integer = KeyNumber - 1
                    If KeyIndex <= SaverSettings.Count - 1 Then
                        KeyType = SettingsKeyType.SVariant
                        KeyVar = CustomSavers.Values(SaverIndex).Screensaver.SaverSettings.Keys(KeyIndex)
                        WriteSeparator(DoTranslation("Screensaver Settings...") + " > {0} > {1}" + vbNewLine, True, CustomSavers.Keys(SaverIndex), SaverSettings.Keys(KeyIndex))
                        W(vbNewLine + DoTranslation("Consult the screensaver manual or source code for information."), True, ColTypes.Neutral)
                    Else
                        WriteSeparator(DoTranslation("Screensaver Settings...") + " > {0} > ???" + vbNewLine, True, CustomSavers.Keys(SaverIndex))
                        W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End If
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
            ElseIf Not KeyType = SettingsKeyType.SVariant Then
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
            ElseIf SectionParts.Length > 1 Then
                If Section = "7." + SectionParts(1) And SectionParts(1) > BuiltinSavers And KeyType = SettingsKeyType.SVariant Then
                    Dim SaverIndex As Integer = SectionParts(1) - BuiltinSavers - 1
                    Dim SaverSettings As Dictionary(Of String, Object) = CustomSavers.Values(SaverIndex).Screensaver.SaverSettings
                    SaverSettings(KeyVar) = VariantValue
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf KeyType = SettingsKeyType.SVariant Then
                    SetConfigValueField(KeyVar, VariantValue)
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                End If
            ElseIf KeyType = SettingsKeyType.SVariant Then
                SetConfigValueField(KeyVar, VariantValue)
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

End Module
