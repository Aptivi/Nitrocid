
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

'TODO: Use JSON. Users will have to use our converter. This is to ensure that we can port KS to .NET 5.0
Public Module Config

    ''' <summary>
    ''' Creates the kernel configuration file
    ''' </summary>
    ''' <param name="Preserve">Preserves configuration values</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    ''' <exception cref="Exceptions.ConfigException"></exception>
    Public Function CreateConfig(ByVal Preserve As Boolean) As Boolean
        Try
            Dim ksconf As New IniFile()
            Wdbg("I", "Preserve: {0}", Preserve)
            If Preserve Then
                'The General Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "General",
                        New IniKey(ksconf, "Prompt for Arguments on Boot", argsOnBoot),
                        New IniKey(ksconf, "Maintenance Mode", maintenance),
                        New IniKey(ksconf, "Change Root Password", setRootPasswd),
                        New IniKey(ksconf, "Set Root Password to", RootPasswd),
                        New IniKey(ksconf, "Check for Updates on Startup", CheckUpdateStart),
                        New IniKey(ksconf, "Change Culture when Switching Languages", LangChangeCulture),
                        New IniKey(ksconf, "Language")))

                'The Colors Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Colors",
                        New IniKey(ksconf, "User Name Shell Color", If(New Color(UserNameShellColor).Type = ColorType.TrueColor, UserNameShellColor.EncloseByDoubleQuotes, UserNameShellColor)),
                        New IniKey(ksconf, "Host Name Shell Color", If(New Color(HostNameShellColor).Type = ColorType.TrueColor, HostNameShellColor.EncloseByDoubleQuotes, HostNameShellColor)),
                        New IniKey(ksconf, "Continuable Kernel Error Color", If(New Color(ContKernelErrorColor).Type = ColorType.TrueColor, ContKernelErrorColor.EncloseByDoubleQuotes, ContKernelErrorColor)),
                        New IniKey(ksconf, "Uncontinuable Kernel Error Color", If(New Color(UncontKernelErrorColor).Type = ColorType.TrueColor, UncontKernelErrorColor.EncloseByDoubleQuotes, UncontKernelErrorColor)),
                        New IniKey(ksconf, "Text Color", If(New Color(NeutralTextColor).Type = ColorType.TrueColor, NeutralTextColor.EncloseByDoubleQuotes, NeutralTextColor)),
                        New IniKey(ksconf, "License Color", If(New Color(LicenseColor).Type = ColorType.TrueColor, LicenseColor.EncloseByDoubleQuotes, LicenseColor)),
                        New IniKey(ksconf, "Background Color", If(New Color(BackgroundColor).Type = ColorType.TrueColor, BackgroundColor.EncloseByDoubleQuotes, BackgroundColor)),
                        New IniKey(ksconf, "Input Color", If(New Color(InputColor).Type = ColorType.TrueColor, InputColor.EncloseByDoubleQuotes, InputColor)),
                        New IniKey(ksconf, "List Entry Color", If(New Color(ListEntryColor).Type = ColorType.TrueColor, ListEntryColor.EncloseByDoubleQuotes, ListEntryColor)),
                        New IniKey(ksconf, "List Value Color", If(New Color(ListValueColor).Type = ColorType.TrueColor, ListValueColor.EncloseByDoubleQuotes, ListValueColor)),
                        New IniKey(ksconf, "Kernel Stage Color", If(New Color(StageColor).Type = ColorType.TrueColor, StageColor.EncloseByDoubleQuotes, StageColor)),
                        New IniKey(ksconf, "Error Text Color", If(New Color(ErrorColor).Type = ColorType.TrueColor, ErrorColor.EncloseByDoubleQuotes, ErrorColor)),
                        New IniKey(ksconf, "Warning Text Color", If(New Color(WarningColor).Type = ColorType.TrueColor, WarningColor.EncloseByDoubleQuotes, WarningColor)),
                        New IniKey(ksconf, "Option Color", If(New Color(OptionColor).Type = ColorType.TrueColor, OptionColor.EncloseByDoubleQuotes, OptionColor)),
                        New IniKey(ksconf, "Banner Color", If(New Color(BannerColor).Type = ColorType.TrueColor, BannerColor.EncloseByDoubleQuotes, BannerColor))))

                'The Hardware Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Hardware",
                        New IniKey(ksconf, "Quiet Probe", quietProbe),
                        New IniKey(ksconf, "Full Probe", FullProbe)))

                'The Login Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Login",
                        New IniKey(ksconf, "Show MOTD on Log-in", showMOTD),
                        New IniKey(ksconf, "Clear Screen on Log-in", clsOnLogin),
                        New IniKey(ksconf, "Host Name", HName),
                        New IniKey(ksconf, "Show available usernames", ShowAvailableUsers)))

                'The Shell Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Shell",
                        New IniKey(ksconf, "Colored Shell", ColoredShell),
                        New IniKey(ksconf, "Simplified Help Command", simHelp),
                        New IniKey(ksconf, "Current Directory", CurrDir),
                        New IniKey(ksconf, "Lookup Directories", PathsToLookup.EncloseByDoubleQuotes),
                        New IniKey(ksconf, "Prompt Style", ShellPromptStyle),
                        New IniKey(ksconf, "FTP Prompt Style", FTPShellPromptStyle),
                        New IniKey(ksconf, "Mail Prompt Style", MailShellPromptStyle),
                        New IniKey(ksconf, "SFTP Prompt Style", SFTPShellPromptStyle)))

                'The Network Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Network",
                        New IniKey(ksconf, "Debug Port", DebugPort),
                        New IniKey(ksconf, "Download Retry Times", DRetries),
                        New IniKey(ksconf, "Upload Retry Times", URetries),
                        New IniKey(ksconf, "Show progress bar while downloading or uploading from ""get"" or ""put"" command", ShowProgress),
                        New IniKey(ksconf, "Log FTP username", FTPLoggerUsername),
                        New IniKey(ksconf, "Log FTP IP address", FTPLoggerIP),
                        New IniKey(ksconf, "Return only first FTP profile", FTPFirstProfileOnly),
                        New IniKey(ksconf, "Show mail message preview", ShowPreview),
                        New IniKey(ksconf, "Record chat to debug log", RecordChatToDebugLog),
                        New IniKey(ksconf, "Show SSH banner", SSHBanner),
                        New IniKey(ksconf, "Enable RPC", RPCEnabled),
                        New IniKey(ksconf, "RPC Port", RPCPort)))

                'The Screensaver Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Screensaver",
                        New IniKey(ksconf, "Screensaver", defSaverName),
                        New IniKey(ksconf, "Screensaver Timeout in ms", ScrnTimeout),
                        New IniKey(ksconf, "ColorMix - Activate 255 Color Mode", ColorMix255Colors),
                        New IniKey(ksconf, "Disco - Activate 255 Color Mode", Disco255Colors),
                        New IniKey(ksconf, "GlitterColor - Activate 255 Color Mode", GlitterColor255Colors),
                        New IniKey(ksconf, "Lines - Activate 255 Color Mode", Lines255Colors),
                        New IniKey(ksconf, "Dissolve - Activate 255 Color Mode", Dissolve255Colors),
                        New IniKey(ksconf, "BouncingBlock - Activate 255 Color Mode", BouncingBlock255Colors),
                        New IniKey(ksconf, "ProgressClock - Activate 255 Color Mode", ProgressClock255Colors),
                        New IniKey(ksconf, "Lighter - Activate 255 Color Mode", Lighter255Colors),
                        New IniKey(ksconf, "Wipe - Activate 255 Color Mode", Wipe255Colors),
                        New IniKey(ksconf, "ColorMix - Activate True Color Mode", ColorMixTrueColor),
                        New IniKey(ksconf, "Disco - Activate True Color Mode", DiscoTrueColor),
                        New IniKey(ksconf, "GlitterColor - Activate True Color Mode", GlitterColorTrueColor),
                        New IniKey(ksconf, "Lines - Activate True Color Mode", LinesTrueColor),
                        New IniKey(ksconf, "Dissolve - Activate True Color Mode", DissolveTrueColor),
                        New IniKey(ksconf, "BouncingBlock - Activate True Color Mode", BouncingBlockTrueColor),
                        New IniKey(ksconf, "ProgressClock - Activate True Color Mode", ProgressClockTrueColor),
                        New IniKey(ksconf, "Lighter - Activate True Color Mode", LighterTrueColor),
                        New IniKey(ksconf, "Wipe - Activate True Color Mode", WipeTrueColor),
                        New IniKey(ksconf, "Disco - Cycle Colors", DiscoCycleColors),
                        New IniKey(ksconf, "ProgressClock - Cycle Colors", ProgressClockCycleColors),
                        New IniKey(ksconf, "ProgressClock - Ticks to change color", ProgressClockCycleColorsTicks),
                        New IniKey(ksconf, "ProgressClock - Color of Seconds Bar", ProgressClockSecondsProgressColor),
                        New IniKey(ksconf, "ProgressClock - Color of Minutes Bar", ProgressClockMinutesProgressColor),
                        New IniKey(ksconf, "ProgressClock - Color of Hours Bar", ProgressClockHoursProgressColor),
                        New IniKey(ksconf, "ProgressClock - Color of Information", ProgressClockProgressColor),
                        New IniKey(ksconf, "BouncingBlock - Delay in Milliseconds", BouncingBlockDelay),
                        New IniKey(ksconf, "BouncingText - Delay in Milliseconds", BouncingTextDelay),
                        New IniKey(ksconf, "ColorMix - Delay in Milliseconds", ColorMixDelay),
                        New IniKey(ksconf, "Disco - Delay in Milliseconds", DiscoDelay),
                        New IniKey(ksconf, "GlitterColor - Delay in Milliseconds", GlitterColorDelay),
                        New IniKey(ksconf, "GlitterMatrix - Delay in Milliseconds", GlitterMatrixDelay),
                        New IniKey(ksconf, "Lines - Delay in Milliseconds", LinesDelay),
                        New IniKey(ksconf, "Matrix - Delay in Milliseconds", MatrixDelay),
                        New IniKey(ksconf, "Lighter - Delay in Milliseconds", LighterDelay),
                        New IniKey(ksconf, "Fader - Delay in Milliseconds", FaderDelay),
                        New IniKey(ksconf, "Fader - Fade Out Delay in Milliseconds", FaderFadeOutDelay),
                        New IniKey(ksconf, "Typo - Delay in Milliseconds", TypoDelay),
                        New IniKey(ksconf, "Typo - Write Again Delay in Milliseconds", TypoWriteAgainDelay),
                        New IniKey(ksconf, "Wipe - Delay in Milliseconds", WipeDelay),
                        New IniKey(ksconf, "BouncingText - Text Shown", BouncingTextWrite),
                        New IniKey(ksconf, "Fader - Text Shown", FaderWrite),
                        New IniKey(ksconf, "Typo - Text Shown", TypoWrite),
                        New IniKey(ksconf, "Lighter - Max Positions Count", LighterMaxPositions),
                        New IniKey(ksconf, "Fader - Max Fade Steps", FaderMaxSteps),
                        New IniKey(ksconf, "Typo - Minimum writing speed in WPM", TypoWritingSpeedMin),
                        New IniKey(ksconf, "Typo - Maximum writing speed in WPM", TypoWritingSpeedMax),
                        New IniKey(ksconf, "Typo - Probability of typo in percent", TypoMissStrikePossibility),
                        New IniKey(ksconf, "Wipe - Wipes to change direction", WipeWipesNeededToChangeDirection)))

                'Misc Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Misc",
                        New IniKey(ksconf, "Show Time/Date on Upper Right Corner", CornerTD),
                        New IniKey(ksconf, "Debug Size Quota in Bytes", DebugQuota),
                        New IniKey(ksconf, "Size parse mode", FullParseMode),
                        New IniKey(ksconf, "Marquee on startup", StartScroll),
                        New IniKey(ksconf, "Long Time and Date", LongTimeDate),
                        New IniKey(ksconf, "Show Hidden Files", HiddenFiles),
                        New IniKey(ksconf, "Preferred Unit for Temperature", PreferredUnit),
                        New IniKey(ksconf, "Enable text editor autosave", TextEdit_AutoSaveFlag),
                        New IniKey(ksconf, "Text editor autosave interval", TextEdit_AutoSaveInterval),
                        New IniKey(ksconf, "Wrap list outputs", WrapListOutputs),
                        New IniKey(ksconf, "Filesystem sort mode", SortMode.ToString),
                        New IniKey(ksconf, "Filesystem sort direction", SortDirection.ToString),
                        New IniKey(ksconf, "Kernel Version", KernelVersion)))
            Else '----------------------- If [Preserve] value is False, then don't preserve.
                'The General Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "General",
                        New IniKey(ksconf, "Prompt for Arguments on Boot", "False"),
                        New IniKey(ksconf, "Maintenance Mode", "False"),
                        New IniKey(ksconf, "Change Root Password", "False"),
                        New IniKey(ksconf, "Set Root Password to", "toor"),
                        New IniKey(ksconf, "Check for Updates on Startup", "True"),
                        New IniKey(ksconf, "Change Culture when Switching Languages", "False"),
                        New IniKey(ksconf, "Language", "eng")))

                'The Colors Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Colors",
                        New IniKey(ksconf, "User Name Shell Color", If(New Color(UserNameShellColor).Type = ColorType.TrueColor, UserNameShellColor.EncloseByDoubleQuotes, UserNameShellColor)),
                        New IniKey(ksconf, "Host Name Shell Color", If(New Color(HostNameShellColor).Type = ColorType.TrueColor, HostNameShellColor.EncloseByDoubleQuotes, HostNameShellColor)),
                        New IniKey(ksconf, "Continuable Kernel Error Color", If(New Color(ContKernelErrorColor).Type = ColorType.TrueColor, ContKernelErrorColor.EncloseByDoubleQuotes, ContKernelErrorColor)),
                        New IniKey(ksconf, "Uncontinuable Kernel Error Color", If(New Color(UncontKernelErrorColor).Type = ColorType.TrueColor, UncontKernelErrorColor.EncloseByDoubleQuotes, UncontKernelErrorColor)),
                        New IniKey(ksconf, "Text Color", If(New Color(NeutralTextColor).Type = ColorType.TrueColor, NeutralTextColor.EncloseByDoubleQuotes, NeutralTextColor)),
                        New IniKey(ksconf, "License Color", If(New Color(LicenseColor).Type = ColorType.TrueColor, LicenseColor.EncloseByDoubleQuotes, LicenseColor)),
                        New IniKey(ksconf, "Background Color", If(New Color(BackgroundColor).Type = ColorType.TrueColor, BackgroundColor.EncloseByDoubleQuotes, BackgroundColor)),
                        New IniKey(ksconf, "Input Color", If(New Color(InputColor).Type = ColorType.TrueColor, InputColor.EncloseByDoubleQuotes, InputColor)),
                        New IniKey(ksconf, "List Entry Color", If(New Color(ListEntryColor).Type = ColorType.TrueColor, ListEntryColor.EncloseByDoubleQuotes, ListEntryColor)),
                        New IniKey(ksconf, "List Value Color", If(New Color(ListValueColor).Type = ColorType.TrueColor, ListValueColor.EncloseByDoubleQuotes, ListValueColor)),
                        New IniKey(ksconf, "Kernel Stage Color", If(New Color(StageColor).Type = ColorType.TrueColor, StageColor.EncloseByDoubleQuotes, StageColor)),
                        New IniKey(ksconf, "Error Text Color", If(New Color(ErrorColor).Type = ColorType.TrueColor, ErrorColor.EncloseByDoubleQuotes, ErrorColor)),
                        New IniKey(ksconf, "Warning Text Color", If(New Color(WarningColor).Type = ColorType.TrueColor, WarningColor.EncloseByDoubleQuotes, WarningColor)),
                        New IniKey(ksconf, "Option Color", If(New Color(OptionColor).Type = ColorType.TrueColor, OptionColor.EncloseByDoubleQuotes, OptionColor)),
                        New IniKey(ksconf, "Banner Color", If(New Color(BannerColor).Type = ColorType.TrueColor, BannerColor.EncloseByDoubleQuotes, BannerColor))))

                'The Hardware Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Hardware",
                        New IniKey(ksconf, "Quiet Probe", "False"),
                        New IniKey(ksconf, "Full Probe", "True")))

                'The Login Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Login",
                        New IniKey(ksconf, "Show MOTD on Log-in", "True"),
                        New IniKey(ksconf, "Clear Screen on Log-in", "False"),
                        New IniKey(ksconf, "Host Name", "kernel"),
                        New IniKey(ksconf, "Show available usernames", "True")))

                'The Shell Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Shell",
                        New IniKey(ksconf, "Colored Shell", "True"),
                        New IniKey(ksconf, "Simplified Help Command", "False"),
                        New IniKey(ksconf, "Current Directory", paths("Home")),
                        New IniKey(ksconf, "Lookup Directories", Environ("PATH").EncloseByDoubleQuotes),
                        New IniKey(ksconf, "Prompt Style", ""),
                        New IniKey(ksconf, "FTP Prompt Style", ""),
                        New IniKey(ksconf, "Mail Prompt Style", ""),
                        New IniKey(ksconf, "SFTP Prompt Style", "")))

                'The Network Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Network",
                        New IniKey(ksconf, "Debug Port", 3014),
                        New IniKey(ksconf, "Download Retry Times", 3),
                        New IniKey(ksconf, "Upload Retry Times", 3),
                        New IniKey(ksconf, "Show progress bar while downloading or uploading from ""get"" or ""put"" command", "True"),
                        New IniKey(ksconf, "Log FTP username", "False"),
                        New IniKey(ksconf, "Log FTP IP address", "False"),
                        New IniKey(ksconf, "Return only first FTP profile", "False"),
                        New IniKey(ksconf, "Show mail message preview", "False"),
                        New IniKey(ksconf, "Record chat to debug log", "True"),
                        New IniKey(ksconf, "Show SSH banner", "False"),
                        New IniKey(ksconf, "Enable RPC", "True"),
                        New IniKey(ksconf, "RPC Port", 12345)))

                'The Screensaver Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Screensaver",
                        New IniKey(ksconf, "Screensaver", "matrix"),
                        New IniKey(ksconf, "Screensaver Timeout in ms", 300000),
                        New IniKey(ksconf, "ColorMix - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "Disco - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "GlitterColor - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "Lines - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "Dissolve - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "BouncingBlock - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "ProgressClock - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "Lighter - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "Wipe - Activate 255 Color Mode", "False"),
                        New IniKey(ksconf, "ColorMix - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Disco - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "GlitterColor - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Lines - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Dissolve - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "BouncingBlock - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "ProgressClock - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Lighter - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Wipe - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Disco - Cycle Colors", "False"),
                        New IniKey(ksconf, "ProgressClock - Cycle Colors", "True"),
                        New IniKey(ksconf, "ProgressClock - Ticks to change color", 20),
                        New IniKey(ksconf, "ProgressClock - Color of Seconds Bar", 4),
                        New IniKey(ksconf, "ProgressClock - Color of Minutes Bar", 5),
                        New IniKey(ksconf, "ProgressClock - Color of Hours Bar", 6),
                        New IniKey(ksconf, "ProgressClock - Color of Information", 7),
                        New IniKey(ksconf, "BouncingBlock - Delay in Milliseconds", 10),
                        New IniKey(ksconf, "BouncingText - Delay in Milliseconds", 10),
                        New IniKey(ksconf, "ColorMix - Delay in Milliseconds", 1),
                        New IniKey(ksconf, "Disco - Delay in Milliseconds", 100),
                        New IniKey(ksconf, "GlitterColor - Delay in Milliseconds", 1),
                        New IniKey(ksconf, "GlitterMatrix - Delay in Milliseconds", 1),
                        New IniKey(ksconf, "Lines - Delay in Milliseconds", 500),
                        New IniKey(ksconf, "Matrix - Delay in Milliseconds", 1),
                        New IniKey(ksconf, "Lighter - Delay in Milliseconds", 100),
                        New IniKey(ksconf, "Fader - Delay in Milliseconds", 50),
                        New IniKey(ksconf, "Fader - Fade Out Delay in Milliseconds", 3000),
                        New IniKey(ksconf, "Typo - Delay in Milliseconds", 50),
                        New IniKey(ksconf, "Typo - Write Again Delay in Milliseconds", 3000),
                        New IniKey(ksconf, "Wipe - Delay in Milliseconds", 10),
                        New IniKey(ksconf, "BouncingText - Text Shown", "Kernel Simulator"),
                        New IniKey(ksconf, "Fader - Text Shown", "Kernel Simulator"),
                        New IniKey(ksconf, "Typo - Text Shown", "Kernel Simulator"),
                        New IniKey(ksconf, "Lighter - Max Positions Count", 10),
                        New IniKey(ksconf, "Fader - Max Fade Steps", 25),
                        New IniKey(ksconf, "Typo - Minimum writing speed in WPM", 50),
                        New IniKey(ksconf, "Typo - Maximum writing speed in WPM", 80),
                        New IniKey(ksconf, "Typo - Probability of typo in percent", 60),
                        New IniKey(ksconf, "Wipe - Wipes to change direction", 10)))

                'Misc Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Misc",
                        New IniKey(ksconf, "Show Time/Date on Upper Right Corner", "False"),
                        New IniKey(ksconf, "Debug Size Quota in Bytes", 1073741824),
                        New IniKey(ksconf, "Size parse mode", "False"),
                        New IniKey(ksconf, "Marquee on startup", "True"),
                        New IniKey(ksconf, "Long Time and Date", "True"),
                        New IniKey(ksconf, "Show Hidden Files", "False"),
                        New IniKey(ksconf, "Preferred Unit for Temperature", UnitMeasurement.Metric),
                        New IniKey(ksconf, "Enable text editor autosave", "True"),
                        New IniKey(ksconf, "Text editor autosave interval", "60"),
                        New IniKey(ksconf, "Wrap list outputs", "False"),
                        New IniKey(ksconf, "Filesystem sort mode", SortMode.ToString),
                        New IniKey(ksconf, "Filesystem sort direction", SortDirection.ToString),
                        New IniKey(ksconf, "Kernel Version", KernelVersion)))
            End If

            'Put comments before saving. General
            ksconf.Sections("General").TrailingComment.Text = "This section is the general settings for KS. It controls boot settings and regional settings."
            ksconf.Sections("General").Keys("Change Root Password").TrailingComment.Text = "Whether or not to change root password. If it is set to True, it will set the password to a password that will be set in the config entry below."
            ksconf.Sections("General").Keys("Maintenance Mode").TrailingComment.Text = "Whether or not to start the kernel in maintenance mode."
            ksconf.Sections("General").Keys("Prompt for Arguments on Boot").TrailingComment.Text = "Whether or not to prompt for arguments on boot to let you set arguments on the current boot"
            ksconf.Sections("General").Keys("Check for Updates on Startup").TrailingComment.Text = "If set to True, everytime the kernel boots, it will check for new updates."
            ksconf.Sections("General").Keys("Change Culture when Switching Languages").TrailingComment.Text = "Indicate if the kernel is allowed to localize time and date locally."
            ksconf.Sections("General").Keys("Language").TrailingComment.Text = "The three-letter language name should be written. All languages can be found in the chlang command wiki."

            'Colors
            ksconf.Sections("Colors").TrailingComment.Text = "Self-explanatory. You can just write the name of colors as specified in the ConsoleColors enumerator or a plain VT sequence that contains the RGB levels like this: RRR;GGG;BBB."

            'Login
            ksconf.Sections("Login").TrailingComment.Text = "This section is the login settings that lets you control the host name and whether or not it shows MOTD and/or clears screen."
            ksconf.Sections("Login").Keys("Clear Screen on Log-in").TrailingComment.Text = "Whether or not it clears screen on sign-in."
            ksconf.Sections("Login").Keys("Show MOTD on Log-in").TrailingComment.Text = "Whether or not it shows MOTD on sign-in."
            ksconf.Sections("Login").Keys("Host Name").TrailingComment.Text = "Custom host name. It will be used in the future for networking references, but is currently here to customize shell prompt."
            ksconf.Sections("Login").Keys("Show available usernames").TrailingComment.Text = "Whether or not to show available usernames on login"

            'Shell
            ksconf.Sections("Shell").TrailingComment.Text = "This section is the shell settings that lets you control whether or not to enable simplified help command and/or colored shell."
            ksconf.Sections("Shell").Keys("Simplified Help Command").TrailingComment.Text = "Simplifies the ""help"" command so it only shows available commands."
            ksconf.Sections("Shell").Keys("Current Directory").TrailingComment.Text = "Sets the shell's current directory."
            ksconf.Sections("Shell").Keys("Colored Shell").TrailingComment.Text = "Whether or not it supports colored shell."
            ksconf.Sections("Shell").Keys("Lookup Directories").TrailingComment.Text = "Group of paths separated by the colon. It works the same as PATH."
            ksconf.Sections("Shell").Keys("Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the main shell. Placeholders here are parsed."
            ksconf.Sections("Shell").Keys("FTP Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the FTP shell. Placeholders here are parsed."
            ksconf.Sections("Shell").Keys("Mail Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the mail shell. Placeholders here are parsed."
            ksconf.Sections("Shell").Keys("SFTP Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the SFTP shell. Placeholders here are parsed."

            'Hardware
            ksconf.Sections("Hardware").TrailingComment.Text = "This section is the hardware probing settings."
            ksconf.Sections("Hardware").Keys("Quiet Probe").TrailingComment.Text = "Whether or not to quietly probe hardware"
            ksconf.Sections("Hardware").Keys("Full Probe").TrailingComment.Text = "If true, probes all the hardware; else, will only probe the needed hardware."

            'Network
            ksconf.Sections("Network").TrailingComment.Text = "This section is the network settings."
            ksconf.Sections("Network").Keys("Debug Port").TrailingComment.Text = "Specifies the remote debugger port."
            ksconf.Sections("Network").Keys("Download Retry Times").TrailingComment.Text = "How many times does the ""get"" command retry the download before assuming failure?"
            ksconf.Sections("Network").Keys("Upload Retry Times").TrailingComment.Text = "How many times does the ""put"" command retry the upload before assuming failure?"
            ksconf.Sections("Network").Keys("Show progress bar while downloading or uploading from ""get"" or ""put"" command").TrailingComment.Text = "If true, it makes ""get"" or ""put"" show the progress bar while downloading or uploading."
            ksconf.Sections("Network").Keys("Log FTP username").TrailingComment.Text = "Whether or not to log FTP username in the debugger log."
            ksconf.Sections("Network").Keys("Log FTP IP address").TrailingComment.Text = "Whether or not to log FTP IP address in the debugger log."
            ksconf.Sections("Network").Keys("Return only first FTP profile").TrailingComment.Text = "Whether or not to return only first successful FTP profile when polling for profiles."
            ksconf.Sections("Network").Keys("Show mail message preview").TrailingComment.Text = "Whether or not to show mail message preview (body text truncated to 200 characters)."
            ksconf.Sections("Network").Keys("Record chat to debug log").TrailingComment.Text = "Records remote debug chat to debug log."
            ksconf.Sections("Network").Keys("Show SSH banner").TrailingComment.Text = "Shows the SSH server banner on connection."
            ksconf.Sections("Network").Keys("Enable RPC").TrailingComment.Text = "Whether or not to enable RPC."
            ksconf.Sections("Network").Keys("RPC Port").TrailingComment.Text = "The RPC port."

            'Screensaver
            ksconf.Sections("Screensaver").TrailingComment.Text = "This section is the network settings."
            ksconf.Sections("Screensaver").Keys("Screensaver").TrailingComment.Text = "Specifies the current screensaver."
            ksconf.Sections("Screensaver").Keys("Screensaver Timeout in ms").TrailingComment.Text = "After specified milliseconds, the screensaver will launch."

            'Screensaver: Colors
            ksconf.Sections("Screensaver").Keys("ColorMix - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for ColorMix"
            ksconf.Sections("Screensaver").Keys("Disco - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Disco"
            ksconf.Sections("Screensaver").Keys("GlitterColor - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for GlitterColor"
            ksconf.Sections("Screensaver").Keys("Lines - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Lines"
            ksconf.Sections("Screensaver").Keys("Dissolve - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Dissolve"
            ksconf.Sections("Screensaver").Keys("BouncingBlock - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for BouncingBlock"
            ksconf.Sections("Screensaver").Keys("ProgressClock - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for ProgressColor"
            ksconf.Sections("Screensaver").Keys("Lighter - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Lighter"
            ksconf.Sections("Screensaver").Keys("Wipe - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Wipe"
            ksconf.Sections("Screensaver").Keys("ColorMix - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for ColorMix"
            ksconf.Sections("Screensaver").Keys("Disco - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Disco"
            ksconf.Sections("Screensaver").Keys("GlitterColor - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for GlitterColor"
            ksconf.Sections("Screensaver").Keys("Lines - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Lines"
            ksconf.Sections("Screensaver").Keys("Dissolve - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Dissolve"
            ksconf.Sections("Screensaver").Keys("BouncingBlock - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for BouncingBlock"
            ksconf.Sections("Screensaver").Keys("ProgressClock - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for ProgressColor"
            ksconf.Sections("Screensaver").Keys("Lighter - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Lighter"
            ksconf.Sections("Screensaver").Keys("Wipe - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Wipe"
            ksconf.Sections("Screensaver").Keys("Disco - Cycle Colors").TrailingComment.Text = "Disco will cycle colors if it's enabled. Otherwise, select random colors."
            ksconf.Sections("Screensaver").Keys("ProgressClock - Cycle Colors").TrailingComment.Text = "ProgressClock will select random colors if it's enabled. Otherwise, use colors from config."
            ksconf.Sections("Screensaver").Keys("ProgressClock - Color of Seconds Bar").TrailingComment.Text = "The color of seconds progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."
            ksconf.Sections("Screensaver").Keys("ProgressClock - Color of Minutes Bar").TrailingComment.Text = "The color of minutes progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."
            ksconf.Sections("Screensaver").Keys("ProgressClock - Color of Hours Bar").TrailingComment.Text = "The color of hours progress bar. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."
            ksconf.Sections("Screensaver").Keys("ProgressClock - Color of Information").TrailingComment.Text = "The color of date information. It can be 1-16, 1-255, or ""1-255;1-255;1-255""."

            'Screensaver: Delays
            ksconf.Sections("Screensaver").Keys("BouncingBlock - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in BouncingBlock?"
            ksconf.Sections("Screensaver").Keys("BouncingText - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in BouncingText?"
            ksconf.Sections("Screensaver").Keys("ColorMix - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in ColorMix?"
            ksconf.Sections("Screensaver").Keys("Disco - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in Disco?"
            ksconf.Sections("Screensaver").Keys("GlitterColor - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in GlitterColor?"
            ksconf.Sections("Screensaver").Keys("GlitterMatrix - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in GlitterMatrix?"
            ksconf.Sections("Screensaver").Keys("Lines - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in Lines?"
            ksconf.Sections("Screensaver").Keys("Matrix - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in Matrix?"
            ksconf.Sections("Screensaver").Keys("Lighter - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in Lighter?"
            ksconf.Sections("Screensaver").Keys("Fader - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in Fader?"
            ksconf.Sections("Screensaver").Keys("Fader - Fade Out Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before fading out text in Fader?"
            ksconf.Sections("Screensaver").Keys("ProgressClock - Ticks to change color").TrailingComment.Text = "If color cycling is enabled, how many ticks before changing colors in ProgressClock? 1 tick = 0.5 seconds"
            ksconf.Sections("Screensaver").Keys("Typo - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in Fader?"
            ksconf.Sections("Screensaver").Keys("Typo - Write Again Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before writing text again in Fader?"
            ksconf.Sections("Screensaver").Keys("Wipe - Delay in Milliseconds").TrailingComment.Text = "How many milliseconds to wait before making the next write in Wipe?"

            'Screensaver: Texts
            ksconf.Sections("Screensaver").Keys("BouncingText - Text Shown").TrailingComment.Text = "Any text for BouncingText"
            ksconf.Sections("Screensaver").Keys("Fader - Text Shown").TrailingComment.Text = "Any text for Fader"
            ksconf.Sections("Screensaver").Keys("Typo - Text Shown").TrailingComment.Text = "Any text for Typo"

            'Screensaver: Misc
            ksconf.Sections("Screensaver").Keys("Lighter - Max Positions Count").TrailingComment.Text = "How many positions are lit before dimming?"
            ksconf.Sections("Screensaver").Keys("Fader - Max Fade Steps").TrailingComment.Text = "How many fade steps to do?"
            ksconf.Sections("Screensaver").Keys("Typo - Minimum writing speed in WPM").TrailingComment.Text = "Minimum writing speed in WPM"
            ksconf.Sections("Screensaver").Keys("Typo - Maximum writing speed in WPM").TrailingComment.Text = "Maximum writing speed in WPM"
            ksconf.Sections("Screensaver").Keys("Typo - Probability of typo in percent").TrailingComment.Text = "Probability of typo in percent"
            ksconf.Sections("Screensaver").Keys("Wipe - Wipes to change direction").TrailingComment.Text = "How many wipes to do before changing direction randomly?"

            'Misc
            ksconf.Sections("Misc").TrailingComment.Text = "This section is the other settings that are not categorized yet."
            ksconf.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").TrailingComment.Text = "Whether or not it shows time and date on the upper right corner."
            ksconf.Sections("Misc").Keys("Debug Size Quota in Bytes").TrailingComment.Text = "Specifies the maximum log size in bytes. If this was exceeded, it will remove the first 5 lines from the log to free up some space."
            ksconf.Sections("Misc").Keys("Size parse mode").TrailingComment.Text = "Parse whole directory for size. If set to False, it will parse just the surface."
            ksconf.Sections("Misc").Keys("Marquee on startup").TrailingComment.Text = "Whether or not to activate banner animation."
            ksconf.Sections("Misc").Keys("Long Time and Date").TrailingComment.Text = "Whether or not to render time and date using long."
            ksconf.Sections("Misc").Keys("Show Hidden Files").TrailingComment.Text = "Whether or not to list hidden files."
            ksconf.Sections("Misc").Keys("Preferred Unit for Temperature").TrailingComment.Text = "Choose either Kelvin, Celsius, or Fahrenheit for temperature measurement."
            ksconf.Sections("Misc").Keys("Enable text editor autosave").TrailingComment.Text = "Turns on or off the text editor autosave feature."
            ksconf.Sections("Misc").Keys("Text editor autosave interval").TrailingComment.Text = "If autosave is enabled, the text file will be saved for each ""n"" seconds."
            ksconf.Sections("Misc").Keys("Filesystem sort mode").TrailingComment.Text = "Controls how the files will be sorted."
            ksconf.Sections("Misc").Keys("Filesystem sort direction").TrailingComment.Text = "Controls the direction of filesystem sorting whether it's ascending or descending."

            'Save Config
            ksconf.Save(paths("Configuration"))
            EventManager.RaiseConfigSaved()
            Return True
        Catch ex As Exception
            EventManager.RaiseConfigSaveError(ex)
            If DebugMode = True Then
                WStkTrc(ex)
                Throw New Exceptions.ConfigException(DoTranslation("There is an error trying to create configuration: {0}."), ex, ex.Message)
            Else
                Throw New Exceptions.ConfigException(DoTranslation("There is an error trying to create configuration."), ex)
            End If
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Checks the config file for mismatched version and upgrades it
    ''' </summary>
    ''' <returns>True if there are updates, False if unsuccessful.</returns>
    ''' <exception cref="Exceptions.ConfigException"></exception>
    Public Function CheckForUpgrade() As Boolean
        Try
            'Variables
            Dim configUpdater As New IniFile()

            'Load config
            configUpdater.Load(paths("Configuration"))

            'Check to see if the kernel is outdated
            If configUpdater.Sections("Misc").Keys("Kernel Version").Value <> KernelVersion Then
                Wdbg("W", "Kernel version upgraded from {1} to {0}", KernelVersion, configUpdater.Sections("Misc").Keys("Kernel Version").Value)
                Return True
            End If
        Catch ex As Exception
            If DebugMode = True Then
                WStkTrc(ex)
                Throw New Exceptions.ConfigException(DoTranslation("There is an error trying to update configuration: {0}."), ex, ex.Message)
            Else
                Throw New Exceptions.ConfigException(DoTranslation("There is an error trying to update configuration."), ex)
            End If
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Updates the configuration file and reboots the simulated system
    ''' </summary>
    Public Sub UpdateConfig()

        CreateConfig(True)
        PowerManage("reboot")

    End Sub

    ''' <summary>
    ''' Configures the kernel according to the kernel configuration file
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.ConfigException"></exception>
    Public Function ReadConfig() As Boolean
        Try
            '----------------------------- Important configuration -----------------------------
            'Language
            Dim ConfiguredLang As String = configReader.Sections("General").Keys("Language").Value
            Wdbg("I", "Language is {0}", ConfiguredLang)
            If configReader.Sections("General").Keys("Change Culture when Switching Languages").Value = "True" Then LangChangeCulture = True Else LangChangeCulture = False
            SetLang(If(String.IsNullOrWhiteSpace(ConfiguredLang), "eng", ConfiguredLang))

            'Colored Shell
            If configReader.Sections("Shell").Keys("Colored Shell").Value = "False" Then
                Wdbg("W", "Detected uncolored shell. Removing colors...")
                ApplyThemeFromResources("LinuxUncolored")
                ColoredShell = False
            End If

            'Hostname setting
            If Not configReader.Sections("Login").Keys("Host Name").Value = "" Then
                HName = configReader.Sections("Login").Keys("Host Name").Value
            Else
                HName = "kernel"
            End If

            '----------------------------- General configuration -----------------------------
            'Colors Section
            Wdbg("I", "Loading colors...")
            If ColoredShell Then
                'We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                UserNameShellColor = New Color(configReader.Sections("Colors").Keys("User Name Shell Color").Value).PlainSequence
                HostNameShellColor = New Color(configReader.Sections("Colors").Keys("Host Name Shell Color").Value).PlainSequence
                ContKernelErrorColor = New Color(configReader.Sections("Colors").Keys("Continuable Kernel Error Color").Value).PlainSequence
                UncontKernelErrorColor = New Color(configReader.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value).PlainSequence
                NeutralTextColor = New Color(configReader.Sections("Colors").Keys("Text Color").Value).PlainSequence
                LicenseColor = New Color(configReader.Sections("Colors").Keys("License Color").Value).PlainSequence
                BackgroundColor = New Color(configReader.Sections("Colors").Keys("Background Color").Value).PlainSequence
                InputColor = New Color(configReader.Sections("Colors").Keys("Input Color").Value).PlainSequence
                ListEntryColor = New Color(configReader.Sections("Colors").Keys("List Entry Color").Value).PlainSequence
                ListValueColor = New Color(configReader.Sections("Colors").Keys("List Value Color").Value).PlainSequence
                StageColor = New Color(configReader.Sections("Colors").Keys("Kernel Stage Color").Value).PlainSequence
                ErrorColor = New Color(configReader.Sections("Colors").Keys("Error Text Color").Value).PlainSequence
                WarningColor = New Color(configReader.Sections("Colors").Keys("Warning Text Color").Value).PlainSequence
                OptionColor = New Color(configReader.Sections("Colors").Keys("Option Color").Value).PlainSequence
                BannerColor = New Color(configReader.Sections("Colors").Keys("Banner Color").Value).PlainSequence
                LoadBack()
            End If

            'General Section
            Wdbg("I", "Parsing general section...")
            If configReader.Sections("General").Keys("Change Root Password").Value = "True" Then setRootPasswd = True Else setRootPasswd = False
            If setRootPasswd = True Then RootPasswd = configReader.Sections("General").Keys("Set Root Password to").Value
            If configReader.Sections("General").Keys("Maintenance Mode").Value = "True" Then maintenance = True Else maintenance = False
            If configReader.Sections("General").Keys("Prompt for Arguments on Boot").Value = "True" Then argsOnBoot = True Else argsOnBoot = False
            If configReader.Sections("General").Keys("Check for Updates on Startup").Value = "True" Then CheckUpdateStart = True Else CheckUpdateStart = False

            'Login Section
            Wdbg("I", "Parsing login section...")
            If configReader.Sections("Login").Keys("Clear Screen on Log-in").Value = "True" Then clsOnLogin = True Else clsOnLogin = False
            If configReader.Sections("Login").Keys("Show MOTD on Log-in").Value = "True" Then showMOTD = True Else showMOTD = False
            If configReader.Sections("Login").Keys("Show available usernames").Value = "True" Then ShowAvailableUsers = True Else ShowAvailableUsers = False

            'Shell Section
            Wdbg("I", "Parsing shell section...")
            If configReader.Sections("Shell").Keys("Simplified Help Command").Value = "True" Then simHelp = True Else simHelp = False
            CurrDir = configReader.Sections("Shell").Keys("Current Directory").Value
            PathsToLookup = configReader.Sections("Shell").Keys("Lookup Directories").Value.ReleaseDoubleQuotes
            ShellPromptStyle = configReader.Sections("Shell").Keys("Prompt Style").Value
            FTPShellPromptStyle = configReader.Sections("Shell").Keys("FTP Prompt Style").Value
            MailShellPromptStyle = configReader.Sections("Shell").Keys("Mail Prompt Style").Value
            SFTPShellPromptStyle = configReader.Sections("Shell").Keys("SFTP Prompt Style").Value

            'Hardware Section
            Wdbg("I", "Parsing hardware section...")
            If configReader.Sections("Hardware").Keys("Quiet Probe").Value = "True" Then quietProbe = True Else quietProbe = False
            If configReader.Sections("Hardware").Keys("Full Probe").Value = "True" Then FullProbe = True Else FullProbe = False

            'Network Section
            Wdbg("I", "Parsing network section...")
            If Integer.TryParse(configReader.Sections("Network").Keys("Debug Port").Value, 0) Then DebugPort = configReader.Sections("Network").Keys("Debug Port").Value
            If Integer.TryParse(configReader.Sections("Network").Keys("Download Retry Times").Value, 0) Then DRetries = configReader.Sections("Network").Keys("Download Retry Times").Value
            If Integer.TryParse(configReader.Sections("Network").Keys("Upload Retry Times").Value, 0) Then URetries = configReader.Sections("Network").Keys("Upload Retry Times").Value
            ShowProgress = configReader.Sections("Network").Keys("Show progress bar while downloading or uploading from ""get"" or ""put"" command").Value
            FTPLoggerUsername = configReader.Sections("Network").Keys("Log FTP username").Value
            FTPLoggerIP = configReader.Sections("Network").Keys("Log FTP IP address").Value
            FTPFirstProfileOnly = configReader.Sections("Network").Keys("Return only first FTP profile").Value
            ShowPreview = configReader.Sections("Network").Keys("Show mail message preview").Value
            RecordChatToDebugLog = configReader.Sections("Network").Keys("Record chat to debug log").Value
            SSHBanner = configReader.Sections("Network").Keys("Show SSH banner").Value
            RPCEnabled = configReader.Sections("Network").Keys("Enable RPC").Value
            If Integer.TryParse(configReader.Sections("Network").Keys("RPC Port").Value, 0) Then RPCPort = configReader.Sections("Network").Keys("RPC Port").Value

            'Screensaver Section
            defSaverName = configReader.Sections("Screensaver").Keys("Screensaver").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Screensaver Timeout in ms").Value, 0) Then ScrnTimeout = configReader.Sections("Screensaver").Keys("Screensaver Timeout in ms").Value

            'Screensaver: Colors
            ColorMix255Colors = configReader.Sections("Screensaver").Keys("ColorMix - Activate 255 Color Mode").Value
            Disco255Colors = configReader.Sections("Screensaver").Keys("Disco - Activate 255 Color Mode").Value
            GlitterColor255Colors = configReader.Sections("Screensaver").Keys("GlitterColor - Activate 255 Color Mode").Value
            Lines255Colors = configReader.Sections("Screensaver").Keys("Lines - Activate 255 Color Mode").Value
            Dissolve255Colors = configReader.Sections("Screensaver").Keys("Dissolve - Activate 255 Color Mode").Value
            BouncingBlock255Colors = configReader.Sections("Screensaver").Keys("BouncingBlock - Activate 255 Color Mode").Value
            ProgressClock255Colors = configReader.Sections("Screensaver").Keys("ProgressClock - Activate 255 Color Mode").Value
            Lighter255Colors = configReader.Sections("Screensaver").Keys("Lighter - Activate 255 Color Mode").Value
            Wipe255Colors = configReader.Sections("Screensaver").Keys("Wipe - Activate 255 Color Mode").Value
            ColorMixTrueColor = configReader.Sections("Screensaver").Keys("ColorMix - Activate True Color Mode").Value
            DiscoTrueColor = configReader.Sections("Screensaver").Keys("Disco - Activate True Color Mode").Value
            GlitterColorTrueColor = configReader.Sections("Screensaver").Keys("GlitterColor - Activate True Color Mode").Value
            LinesTrueColor = configReader.Sections("Screensaver").Keys("Lines - Activate True Color Mode").Value
            DissolveTrueColor = configReader.Sections("Screensaver").Keys("Dissolve - Activate True Color Mode").Value
            BouncingBlockTrueColor = configReader.Sections("Screensaver").Keys("BouncingBlock - Activate True Color Mode").Value
            ProgressClockTrueColor = configReader.Sections("Screensaver").Keys("ProgressClock - Activate True Color Mode").Value
            LighterTrueColor = configReader.Sections("Screensaver").Keys("Lighter - Activate True Color Mode").Value
            WipeTrueColor = configReader.Sections("Screensaver").Keys("Wipe - Activate True Color Mode").Value
            DiscoCycleColors = configReader.Sections("Screensaver").Keys("Disco - Cycle Colors").Value
            ProgressClockCycleColors = configReader.Sections("Screensaver").Keys("ProgressClock - Cycle Colors").Value
            ProgressClockSecondsProgressColor = configReader.Sections("Screensaver").Keys("ProgressClock - Color of Seconds Bar").Value
            ProgressClockMinutesProgressColor = configReader.Sections("Screensaver").Keys("ProgressClock - Color of Minutes Bar").Value
            ProgressClockHoursProgressColor = configReader.Sections("Screensaver").Keys("ProgressClock - Color of Hours Bar").Value
            ProgressClockProgressColor = configReader.Sections("Screensaver").Keys("ProgressClock - Color of Information").Value

            'Screensaver: Delays
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("BouncingBlock - Delay in Milliseconds").Value, 0) Then BouncingBlockDelay = configReader.Sections("Screensaver").Keys("BouncingBlock - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("BouncingText - Delay in Milliseconds").Value, 0) Then BouncingTextDelay = configReader.Sections("Screensaver").Keys("BouncingText - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("ColorMix - Delay in Milliseconds").Value, 0) Then ColorMixDelay = configReader.Sections("Screensaver").Keys("ColorMix - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Disco - Delay in Milliseconds").Value, 0) Then DiscoDelay = configReader.Sections("Screensaver").Keys("Disco - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("GlitterColor - Delay in Milliseconds").Value, 0) Then GlitterColorDelay = configReader.Sections("Screensaver").Keys("GlitterColor - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("GlitterMatrix - Delay in Milliseconds").Value, 0) Then GlitterMatrixDelay = configReader.Sections("Screensaver").Keys("GlitterMatrix - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Lines - Delay in Milliseconds").Value, 0) Then LinesDelay = configReader.Sections("Screensaver").Keys("Lines - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Matrix - Delay in Milliseconds").Value, 0) Then MatrixDelay = configReader.Sections("Screensaver").Keys("Matrix - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Lighter - Delay in Milliseconds").Value, 0) Then LighterDelay = configReader.Sections("Screensaver").Keys("Lighter - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Fader - Delay in Milliseconds").Value, 0) Then FaderDelay = configReader.Sections("Screensaver").Keys("Fader - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Fader - Fade Out Delay in Milliseconds").Value, 0) Then FaderFadeOutDelay = configReader.Sections("Screensaver").Keys("Fader - Fade Out Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("ProgressClock - Ticks to change color").Value, 0) Then ProgressClockCycleColorsTicks = configReader.Sections("Screensaver").Keys("ProgressClock - Ticks to change color").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Typo - Delay in Milliseconds").Value, 0) Then TypoDelay = configReader.Sections("Screensaver").Keys("Typo - Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Typo - Write Again Delay in Milliseconds").Value, 0) Then TypoWriteAgainDelay = configReader.Sections("Screensaver").Keys("Typo - Write Again Delay in Milliseconds").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Wipe - Delay in Milliseconds").Value, 0) Then WipeDelay = configReader.Sections("Screensaver").Keys("Wipe - Delay in Milliseconds").Value

            'Screensaver: Texts
            BouncingTextWrite = configReader.Sections("Screensaver").Keys("BouncingText - Text Shown").Value
            FaderWrite = configReader.Sections("Screensaver").Keys("Fader - Text Shown").Value
            TypoWrite = configReader.Sections("Screensaver").Keys("Typo - Text Shown").Value

            'Screensaver: Misc
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Lighter - Max Positions Count").Value, 0) Then LighterMaxPositions = configReader.Sections("Screensaver").Keys("Lighter - Max Positions Count").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Fader - Max Fade Steps").Value, 0) Then FaderMaxSteps = configReader.Sections("Screensaver").Keys("Fader - Max Fade Steps").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Typo - Minimum writing speed in WPM").Value, 0) Then TypoWritingSpeedMin = configReader.Sections("Screensaver").Keys("Typo - Minimum writing speed in WPM").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Typo - Maximum writing speed in WPM").Value, 0) Then TypoWritingSpeedMax = configReader.Sections("Screensaver").Keys("Typo - Maximum writing speed in WPM").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Typo - Probability of typo in percent").Value, 0) Then TypoMissStrikePossibility = configReader.Sections("Screensaver").Keys("Typo - Probability of typo in percent").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Wipe - Wipes to change direction").Value, 0) Then WipeWipesNeededToChangeDirection = configReader.Sections("Screensaver").Keys("Wipe - Wipes to change direction").Value

            'Misc Section
            Wdbg("I", "Parsing misc section...")
            If configReader.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").Value = "True" Then CornerTD = True Else CornerTD = False
            If Integer.TryParse(configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value, 0) Then DebugQuota = configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value
            If configReader.Sections("Misc").Keys("Size parse mode").Value = "True" Then FullParseMode = True Else FullParseMode = False
            If configReader.Sections("Misc").Keys("Marquee on startup").Value = "True" Then StartScroll = True Else StartScroll = False
            If configReader.Sections("Misc").Keys("Long Time and Date").Value = "True" Then LongTimeDate = True Else LongTimeDate = False
            If configReader.Sections("Misc").Keys("Show Hidden Files").Value = "True" Then HiddenFiles = True Else HiddenFiles = False
            PreferredUnit = configReader.Sections("Misc").Keys("Preferred Unit for Temperature").Value
            If configReader.Sections("Misc").Keys("Enable text editor autosave").Value = "True" Then TextEdit_AutoSaveFlag = True Else TextEdit_AutoSaveFlag = False
            If Integer.TryParse(configReader.Sections("Misc").Keys("Text editor autosave interval").Value, 0) Then TextEdit_AutoSaveInterval = configReader.Sections("Misc").Keys("Text editor autosave interval").Value
            If configReader.Sections("Misc").Keys("Wrap list outputs").Value = "True" Then WrapListOutputs = True Else WrapListOutputs = False
            SortMode = [Enum].Parse(GetType(FilesystemSortOptions), configReader.Sections("Misc").Keys("Filesystem sort mode").Value)
            SortDirection = [Enum].Parse(GetType(FilesystemSortDirection), configReader.Sections("Misc").Keys("Filesystem sort direction").Value)

            'Raise event and return true
            EventManager.RaiseConfigRead()
            Return True
        Catch nre As NullReferenceException
            'Old config file being read. It is not appropriate to let KS crash on startup when the old version is read, so convert.
            Wdbg("W", "Detected incompatible/old version of config. Renewing...")
            UpgradeConfig()
        Catch ex As Exception
            EventManager.RaiseConfigReadError(ex)
            WStkTrc(ex)
            NotifyConfigError = True
            Throw New Exceptions.ConfigException(DoTranslation("There is an error trying to read configuration: {0}."), ex, ex.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Reloads config
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function ReloadConfig() As Boolean
        Try
            EventManager.RaisePreReloadConfig()
            InitializeConfig()
            EventManager.RaisePostReloadConfig()
            Return True
        Catch ex As Exception
            Wdbg("E", "Failed to reload config: {0}", ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Main loader for configuration file
    ''' </summary>
    Sub InitializeConfig()
        'Make a config file if not found
        Dim pathConfig As String = paths("Configuration")
        If Not File.Exists(pathConfig) Then
            Wdbg("E", "No config file found. Creating...")
            CreateConfig(False)
        End If

        'Load and read config
        Try
            configReader.Load(pathConfig)
            Wdbg("I", "Config loaded with {0} sections", configReader.Sections.Count)
            ReadConfig()
        Catch cex As Exceptions.ConfigException
            W(cex.Message, True, ColTypes.Error)
            WStkTrc(cex)
        End Try

        'Check for updates for config
        If CheckForUpgrade() Then
            W(DoTranslation("An upgrade to {0} is detected. Updating configuration..."), True, ColTypes.Neutral, KernelVersion)
            UpdateConfig()
        End If
    End Sub

End Module
