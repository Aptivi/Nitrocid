
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
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Module Config

    ''' <summary>
    ''' Creates the kernel configuration file
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    ''' <exception cref="Exceptions.ConfigException"></exception>
    Public Function CreateConfig() As Boolean
        Try
            Dim ConfigurationObject As New JObject

            'The General Section
            Dim GeneralConfig As New JObject From {
                    {"Prompt for Arguments on Boot", argsOnBoot},
                    {"Maintenance Mode", maintenance},
                    {"Change Root Password", setRootPasswd},
                    {"Set Root Password to", RootPasswd},
                    {"Check for Updates on Startup", CheckUpdateStart},
                    {"Change Culture when Switching Languages", LangChangeCulture},
                    {"Language", currentLang},
                    {"Culture", CurrentCult.Name}
            }
            ConfigurationObject.Add("General", GeneralConfig)

            'The Colors Section
            Dim ColorConfig As New JObject From {
                    {"User Name Shell Color", If(New Color(UserNameShellColor).Type = ColorType.TrueColor, UserNameShellColor.EncloseByDoubleQuotes, UserNameShellColor)},
                    {"Host Name Shell Color", If(New Color(HostNameShellColor).Type = ColorType.TrueColor, HostNameShellColor.EncloseByDoubleQuotes, HostNameShellColor)},
                    {"Continuable Kernel Error Color", If(New Color(ContKernelErrorColor).Type = ColorType.TrueColor, ContKernelErrorColor.EncloseByDoubleQuotes, ContKernelErrorColor)},
                    {"Uncontinuable Kernel Error Color", If(New Color(UncontKernelErrorColor).Type = ColorType.TrueColor, UncontKernelErrorColor.EncloseByDoubleQuotes, UncontKernelErrorColor)},
                    {"Text Color", If(New Color(NeutralTextColor).Type = ColorType.TrueColor, NeutralTextColor.EncloseByDoubleQuotes, NeutralTextColor)},
                    {"License Color", If(New Color(LicenseColor).Type = ColorType.TrueColor, LicenseColor.EncloseByDoubleQuotes, LicenseColor)},
                    {"Background Color", If(New Color(BackgroundColor).Type = ColorType.TrueColor, BackgroundColor.EncloseByDoubleQuotes, BackgroundColor)},
                    {"Input Color", If(New Color(InputColor).Type = ColorType.TrueColor, InputColor.EncloseByDoubleQuotes, InputColor)},
                    {"List Entry Color", If(New Color(ListEntryColor).Type = ColorType.TrueColor, ListEntryColor.EncloseByDoubleQuotes, ListEntryColor)},
                    {"List Value Color", If(New Color(ListValueColor).Type = ColorType.TrueColor, ListValueColor.EncloseByDoubleQuotes, ListValueColor)},
                    {"Kernel Stage Color", If(New Color(StageColor).Type = ColorType.TrueColor, StageColor.EncloseByDoubleQuotes, StageColor)},
                    {"Error Text Color", If(New Color(ErrorColor).Type = ColorType.TrueColor, ErrorColor.EncloseByDoubleQuotes, ErrorColor)},
                    {"Warning Text Color", If(New Color(WarningColor).Type = ColorType.TrueColor, WarningColor.EncloseByDoubleQuotes, WarningColor)},
                    {"Option Color", If(New Color(OptionColor).Type = ColorType.TrueColor, OptionColor.EncloseByDoubleQuotes, OptionColor)},
                    {"Banner Color", If(New Color(BannerColor).Type = ColorType.TrueColor, BannerColor.EncloseByDoubleQuotes, BannerColor)}
            }
            ConfigurationObject.Add("Colors", ColorConfig)

            'The Hardware Section
            Dim HardwareConfig As New JObject From {
                    {"Quiet Probe", quietProbe},
                    {"Full Probe", FullProbe}
            }
            ConfigurationObject.Add("Hardware", HardwareConfig)

            'The Login Section
            Dim LoginConfig As New JObject From {
                    {"Show MOTD on Log-in", showMOTD},
                    {"Clear Screen on Log-in", clsOnLogin},
                    {"Host Name", HName},
                    {"Show available usernames", ShowAvailableUsers}
            }
            ConfigurationObject.Add("Login", LoginConfig)

            'The Shell Section
            Dim ShellConfig As New JObject From {
                    {"Colored Shell", ColoredShell},
                    {"Simplified Help Command", simHelp},
                    {"Current Directory", CurrDir},
                    {"Lookup Directories", PathsToLookup.EncloseByDoubleQuotes},
                    {"Prompt Style", ShellPromptStyle},
                    {"FTP Prompt Style", FTPShellPromptStyle},
                    {"Mail Prompt Style", MailShellPromptStyle},
                    {"SFTP Prompt Style", SFTPShellPromptStyle}
            }
            ConfigurationObject.Add("Shell", ShellConfig)

            'The Filesystem Section
            Dim FilesystemConfig As New JObject From {
                    {"Filesystem sort mode", SortMode.ToString},
                    {"Filesystem sort direction", SortDirection.ToString},
                    {"Debug Size Quota in Bytes", DebugQuota},
                    {"Show Hidden Files", HiddenFiles},
                    {"Size parse mode", FullParseMode}
            }
            ConfigurationObject.Add("Filesystem", FilesystemConfig)

            'The Network Section
            Dim NetworkConfig As New JObject From {
                    {"Debug Port", DebugPort},
                    {"Download Retry Times", DRetries},
                    {"Upload Retry Times", URetries},
                    {"Show progress bar while downloading or uploading from ""get"" or ""put"" command", ShowProgress},
                    {"Log FTP username", FTPLoggerUsername},
                    {"Log FTP IP address", FTPLoggerIP},
                    {"Return only first FTP profile", FTPFirstProfileOnly},
                    {"Show mail message preview", ShowPreview},
                    {"Record chat to debug log", RecordChatToDebugLog},
                    {"Show SSH banner", SSHBanner},
                    {"Enable RPC", RPCEnabled},
                    {"RPC Port", RPCPort}
            }
            ConfigurationObject.Add("Network", NetworkConfig)

            'The Screensaver Section
            Dim ScreensaverConfig As New JObject From {
                    {"Screensaver", defSaverName},
                    {"Screensaver Timeout in ms", ScrnTimeout}
            }

            'ColorMix config json object
            Dim ColorMixConfig As New JObject From {
                    {"Activate 255 Color Mode", ColorMix255Colors},
                    {"Activate True Color Mode", ColorMixTrueColor},
                    {"Delay in Milliseconds", ColorMixDelay}
            }
            ScreensaverConfig.Add("ColorMix", ColorMixConfig)

            'Disco config json object
            Dim DiscoConfig As New JObject From {
                    {"Activate 255 Color Mode", Disco255Colors},
                    {"Activate True Color Mode", DiscoTrueColor},
                    {"Delay in Milliseconds", DiscoDelay},
                    {"Cycle Colors", DiscoCycleColors}
            }
            ScreensaverConfig.Add("Disco", DiscoConfig)

            'GlitterColor config json object
            Dim GlitterColorConfig As New JObject From {
                    {"Activate 255 Color Mode", GlitterColor255Colors},
                    {"Activate True Color Mode", GlitterColorTrueColor},
                    {"Delay in Milliseconds", GlitterColorDelay}
            }
            ScreensaverConfig.Add("GlitterColor", GlitterColorConfig)

            'Lines config json object
            Dim LinesConfig As New JObject From {
                    {"Activate 255 Color Mode", Lines255Colors},
                    {"Activate True Color Mode", LinesTrueColor},
                    {"Delay in Milliseconds", LinesDelay}
            }
            ScreensaverConfig.Add("Lines", LinesConfig)

            'Dissolve config json object
            Dim DissolveConfig As New JObject From {
                    {"Activate 255 Color Mode", Dissolve255Colors},
                    {"Activate True Color Mode", DissolveTrueColor}
            }
            ScreensaverConfig.Add("Dissolve", DissolveConfig)

            'BouncingBlock config json object
            Dim BouncingBlockConfig As New JObject From {
                    {"Activate 255 Color Mode", BouncingBlock255Colors},
                    {"Activate True Color Mode", BouncingBlockTrueColor},
                    {"Delay in Milliseconds", BouncingBlockDelay}
            }
            ScreensaverConfig.Add("BouncingBlock", BouncingBlockConfig)

            'ProgressClock config json object
            Dim ProgressClockConfig As New JObject From {
                    {"Activate 255 Color Mode", ProgressClock255Colors},
                    {"Activate True Color Mode", ProgressClockTrueColor},
                    {"Cycle Colors", ProgressClockCycleColors},
                    {"Ticks to change color", ProgressClockCycleColorsTicks},
                    {"Color of Seconds Bar", ProgressClockSecondsProgressColor},
                    {"Color of Minutes Bar", ProgressClockMinutesProgressColor},
                    {"Color of Hours Bar", ProgressClockHoursProgressColor},
                    {"Color of Information", ProgressClockProgressColor}
            }
            ScreensaverConfig.Add("ProgressClock", ProgressClockConfig)

            'Lighter config json object
            Dim LighterConfig As New JObject From {
                    {"Activate 255 Color Mode", Lighter255Colors},
                    {"Activate True Color Mode", LighterTrueColor},
                    {"Delay in Milliseconds", LighterDelay},
                    {"Max Positions Count", LighterMaxPositions}
            }
            ScreensaverConfig.Add("Lighter", LighterConfig)

            'Wipe config json object
            Dim WipeConfig As New JObject From {
                    {"Activate 255 Color Mode", Wipe255Colors},
                    {"Activate True Color Mode", WipeTrueColor},
                    {"Delay in Milliseconds", WipeDelay},
                    {"Wipes to change direction", WipeWipesNeededToChangeDirection}
            }
            ScreensaverConfig.Add("Wipe", WipeConfig)

            'Matrix config json object
            Dim MatrixConfig As New JObject From {
                    {"Delay in Milliseconds", MatrixDelay}
            }
            ScreensaverConfig.Add("Matrix", MatrixConfig)

            'GlitterMatrix config json object
            Dim GlitterMatrixConfig As New JObject From {
                    {"Delay in Milliseconds", GlitterMatrixDelay}
            }
            ScreensaverConfig.Add("GlitterMatrix", GlitterMatrixConfig)

            'BouncingText config json object
            Dim BouncingTextConfig As New JObject From {
                    {"Activate 255 Color Mode", BouncingText255Colors},
                    {"Activate True Color Mode", BouncingTextTrueColor},
                    {"Delay in Milliseconds", BouncingTextDelay},
                    {"Text Shown", BouncingTextWrite}
            }
            ScreensaverConfig.Add("BouncingText", BouncingTextConfig)

            'Fader config json object
            Dim FaderConfig As New JObject From {
                    {"Delay in Milliseconds", FaderDelay},
                    {"Fade Out Delay in Milliseconds", FaderFadeOutDelay},
                    {"Text Shown", FaderWrite},
                    {"Max Fade Steps", FaderMaxSteps}
            }
            ScreensaverConfig.Add("Fader", FaderConfig)

            'Typo config json object
            Dim TypoConfig As New JObject From {
                    {"Delay in Milliseconds", TypoDelay},
                    {"Write Again Delay in Milliseconds", TypoWriteAgainDelay},
                    {"Text Shown", TypoWrite},
                    {"Minimum writing speed in WPM", TypoWritingSpeedMin},
                    {"Maximum writing speed in WPM", TypoWritingSpeedMax},
                    {"Probability of typo in percent", TypoMissStrikePossibility}
            }
            ScreensaverConfig.Add("Typo", TypoConfig)

            'Add a screensaver config json object to Screensaver section
            ConfigurationObject.Add("Screensaver", ScreensaverConfig)

            'Misc Section
            Dim MiscConfig As New JObject From {
                    {"Show Time/Date on Upper Right Corner", CornerTD},
                    {"Marquee on startup", StartScroll},
                    {"Long Time and Date", LongTimeDate},
                    {"Preferred Unit for Temperature", PreferredUnit},
                    {"Enable text editor autosave", TextEdit_AutoSaveFlag},
                    {"Text editor autosave interval", TextEdit_AutoSaveInterval},
                    {"Wrap list outputs", WrapListOutputs}
            }
            ConfigurationObject.Add("Misc", MiscConfig)

            'Save Config
            File.WriteAllText(paths("Configuration"), JsonConvert.SerializeObject(ConfigurationObject, Formatting.Indented))
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
    ''' Configures the kernel according to the kernel configuration file
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.ConfigException"></exception>
    Public Function ReadConfig() As Boolean
        Try
            'Parse configuration. NOTE: Question marks between parentheses are for nullable types.
            ConfigToken = JObject.Parse(File.ReadAllText(paths("Configuration")))
            Wdbg("I", "Config loaded with {0} sections", ConfigToken.Count)

            '----------------------------- Important configuration -----------------------------
            'Language
            LangChangeCulture = If(ConfigToken("General")?("Change Culture when Switching Languages"), False)
            If LangChangeCulture Then CurrentCult = New CultureInfo(If(ConfigToken("General")?("Culture") IsNot Nothing, ConfigToken("General")("Culture").ToString, "en-US"))
            SetLang(If(ConfigToken("General")?("Language"), "eng"))

            'Colored Shell
            Dim UncoloredDetected As Boolean = ConfigToken("Shell")?("Colored Shell") IsNot Nothing AndAlso Not ConfigToken("Shell")("Colored Shell").ToObject(Of Boolean)
            If UncoloredDetected Then
                Wdbg("W", "Detected uncolored shell. Removing colors...")
                ApplyThemeFromResources("LinuxUncolored")
                ColoredShell = False
            End If

            '----------------------------- General configuration -----------------------------
            'Colors Section
            Wdbg("I", "Loading colors...")
            If ColoredShell Then
                'We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                UserNameShellColor = New Color(If(ConfigToken("Colors")?("User Name Shell Color"), ConsoleColors.Green)).PlainSequence
                HostNameShellColor = New Color(If(ConfigToken("Colors")?("Host Name Shell Color"), ConsoleColors.DarkGreen)).PlainSequence
                ContKernelErrorColor = New Color(If(ConfigToken("Colors")?("Continuable Kernel Error Color"), ConsoleColors.Yellow)).PlainSequence
                UncontKernelErrorColor = New Color(If(ConfigToken("Colors")?("Uncontinuable Kernel Error Color"), ConsoleColors.Red)).PlainSequence
                NeutralTextColor = New Color(If(ConfigToken("Colors")?("Text Color"), ConsoleColors.Gray)).PlainSequence
                LicenseColor = New Color(If(ConfigToken("Colors")?("License Color"), ConsoleColors.White)).PlainSequence
                BackgroundColor = New Color(If(ConfigToken("Colors")?("Background Color"), ConsoleColors.Black)).PlainSequence
                InputColor = New Color(If(ConfigToken("Colors")?("Input Color"), ConsoleColors.White)).PlainSequence
                ListEntryColor = New Color(If(ConfigToken("Colors")?("List Entry Color"), ConsoleColors.DarkYellow)).PlainSequence
                ListValueColor = New Color(If(ConfigToken("Colors")?("List Value Color"), ConsoleColors.DarkGray)).PlainSequence
                StageColor = New Color(If(ConfigToken("Colors")?("Kernel Stage Color"), ConsoleColors.Green)).PlainSequence
                ErrorColor = New Color(If(ConfigToken("Colors")?("Error Text Color"), ConsoleColors.Red)).PlainSequence
                WarningColor = New Color(If(ConfigToken("Colors")?("Warning Text Color"), ConsoleColors.Yellow)).PlainSequence
                OptionColor = New Color(If(ConfigToken("Colors")?("Option Color"), ConsoleColors.DarkYellow)).PlainSequence
                BannerColor = New Color(If(ConfigToken("Colors")?("Banner Color"), ConsoleColors.Green)).PlainSequence
                LoadBack()
            End If

            'General Section
            Wdbg("I", "Parsing general section...")
            setRootPasswd = If(ConfigToken("General")?("Change Root Password"), False)
            If setRootPasswd = True Then RootPasswd = ConfigToken("General")?("Set Root Password to")
            maintenance = If(ConfigToken("General")?("Maintenance Mode"), False)
            argsOnBoot = If(ConfigToken("General")?("Prompt for Arguments on Boot"), False)
            CheckUpdateStart = If(ConfigToken("General")?("Check for Updates on Startup"), True)

            'Login Section
            Wdbg("I", "Parsing login section...")
            clsOnLogin = If(ConfigToken("Login")?("Clear Screen on Log-in"), False)
            showMOTD = If(ConfigToken("Login")?("Show MOTD on Log-in"), True)
            ShowAvailableUsers = If(ConfigToken("Login")?("Show available usernames"), True)
            If Not String.IsNullOrWhiteSpace(ConfigToken("Login")?("Host Name")) Then
                HName = ConfigToken("Login")?("Host Name")
            Else
                HName = "kernel"
            End If

            'Shell Section
            Wdbg("I", "Parsing shell section...")
            simHelp = If(ConfigToken("Shell")?("Simplified Help Command"), False)
            CurrDir = If(ConfigToken("Shell")?("Current Directory"), paths("Home"))
            PathsToLookup = If(Not String.IsNullOrEmpty(ConfigToken("Shell")?("Lookup Directories")), ConfigToken("Shell")?("Lookup Directories").ToString.ReleaseDoubleQuotes, Environ("PATH"))
            ShellPromptStyle = If(ConfigToken("Shell")?("Prompt Style"), "")
            FTPShellPromptStyle = If(ConfigToken("Shell")?("FTP Prompt Style"), "")
            MailShellPromptStyle = If(ConfigToken("Shell")?("Mail Prompt Style"), "")
            SFTPShellPromptStyle = If(ConfigToken("Shell")?("SFTP Prompt Style"), "")

            'Filesystem Section
            Wdbg("I", "Parsing filesystem section...")
            DebugQuota = If(Integer.TryParse(ConfigToken("Filesystem")?("Debug Size Quota in Bytes"), 0), ConfigToken("Filesystem")?("Debug Size Quota in Bytes"), 1073741824)
            FullParseMode = If(ConfigToken("Filesystem")?("Size parse mode"), False)
            HiddenFiles = If(ConfigToken("Filesystem")?("Show Hidden Files"), False)
            SortMode = If(ConfigToken("Filesystem")?("Filesystem sort mode") IsNot Nothing, If([Enum].TryParse(ConfigToken("Filesystem")?("Filesystem sort mode"), SortMode), SortMode, FilesystemSortOptions.FullName), FilesystemSortOptions.FullName)
            SortDirection = If(ConfigToken("Filesystem")?("Filesystem sort direction") IsNot Nothing, If([Enum].TryParse(ConfigToken("Filesystem")?("Filesystem sort direction"), SortDirection), SortDirection, FilesystemSortDirection.Ascending), FilesystemSortDirection.Ascending)

            'Hardware Section
            Wdbg("I", "Parsing hardware section...")
            quietProbe = If(ConfigToken("Hardware")?("Quiet Probe"), False)
            FullProbe = If(ConfigToken("Hardware")?("Full Probe"), True)

            'Network Section
            Wdbg("I", "Parsing network section...")
            DebugPort = If(Integer.TryParse(ConfigToken("Network")?("Debug Port"), 0), ConfigToken("Network")?("Debug Port"), 3014)
            DRetries = If(Integer.TryParse(ConfigToken("Network")?("Download Retry Times"), 0), ConfigToken("Network")?("Download Retry Times"), 3)
            URetries = If(Integer.TryParse(ConfigToken("Network")?("Upload Retry Times"), 0), ConfigToken("Network")?("Upload Retry Times"), 3)
            ShowProgress = If(ConfigToken("Network")?("Show progress bar while downloading or uploading from ""get"" or ""put"" command"), True)
            FTPLoggerUsername = If(ConfigToken("Network")?("Log FTP username"), False)
            FTPLoggerIP = If(ConfigToken("Network")?("Log FTP IP address"), False)
            FTPFirstProfileOnly = If(ConfigToken("Network")?("Return only first FTP profile"), False)
            ShowPreview = If(ConfigToken("Network")?("Show mail message preview"), False)
            RecordChatToDebugLog = If(ConfigToken("Network")?("Record chat to debug log"), True)
            SSHBanner = If(ConfigToken("Network")?("Show SSH banner"), False)
            RPCEnabled = If(ConfigToken("Network")?("Enable RPC"), True)
            RPCPort = If(Integer.TryParse(ConfigToken("Network")?("RPC Port"), 0), ConfigToken("Network")?("RPC Port"), 12345)

            'Screensaver Section
            defSaverName = If(ConfigToken("Screensaver")?("Screensaver"), "matrix")
            ScrnTimeout = If(Integer.TryParse(ConfigToken("Screensaver")?("Screensaver Timeout in ms"), 0), ConfigToken("Screensaver")?("Screensaver Timeout in ms"), 300000)

            'Screensaver: Colors
            ColorMix255Colors = If(ConfigToken("Screensaver")?("ColorMix")?("Activate 255 Color Mode"), False)
            Disco255Colors = If(ConfigToken("Screensaver")?("Disco")?("Activate 255 Color Mode"), False)
            GlitterColor255Colors = If(ConfigToken("Screensaver")?("GlitterColor")?("Activate 255 Color Mode"), False)
            Lines255Colors = If(ConfigToken("Screensaver")?("Lines")?("Activate 255 Color Mode"), False)
            Dissolve255Colors = If(ConfigToken("Screensaver")?("Dissolve")?("Activate 255 Color Mode"), False)
            BouncingBlock255Colors = If(ConfigToken("Screensaver")?("BouncingBlock")?("Activate 255 Color Mode"), False)
            BouncingText255Colors = If(ConfigToken("Screensaver")?("BouncingText")?("Activate 255 Color Mode"), False)
            ProgressClock255Colors = If(ConfigToken("Screensaver")?("ProgressClock")?("Activate 255 Color Mode"), False)
            Lighter255Colors = If(ConfigToken("Screensaver")?("Lighter")?("Activate 255 Color Mode"), False)
            Wipe255Colors = If(ConfigToken("Screensaver")?("Wipe")?("Activate 255 Color Mode"), False)
            ColorMixTrueColor = If(ConfigToken("Screensaver")?("ColorMix")?("Activate True Color Mode"), True)
            DiscoTrueColor = If(ConfigToken("Screensaver")?("Disco")?("Activate True Color Mode"), True)
            GlitterColorTrueColor = If(ConfigToken("Screensaver")?("GlitterColor")?("Activate True Color Mode"), True)
            LinesTrueColor = If(ConfigToken("Screensaver")?("Lines")?("Activate True Color Mode"), True)
            DissolveTrueColor = If(ConfigToken("Screensaver")?("Dissolve")?("Activate True Color Mode"), True)
            BouncingBlockTrueColor = If(ConfigToken("Screensaver")?("BouncingBlock")?("Activate True Color Mode"), True)
            BouncingTextTrueColor = If(ConfigToken("Screensaver")?("BouncingText")?("Activate True Color Mode"), True)
            ProgressClockTrueColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Activate True Color Mode"), True)
            LighterTrueColor = If(ConfigToken("Screensaver")?("Lighter")?("Activate True Color Mode"), True)
            WipeTrueColor = If(ConfigToken("Screensaver")?("Wipe")?("Activate True Color Mode"), True)
            DiscoCycleColors = If(ConfigToken("Screensaver")?("Disco")?("Cycle Colors"), False)
            ProgressClockCycleColors = If(ConfigToken("Screensaver")?("ProgressClock")?("Cycle Colors"), True)
            ProgressClockSecondsProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Seconds Bar"), 4)
            ProgressClockMinutesProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Minutes Bar"), 5)
            ProgressClockHoursProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Hours Bar"), 6)
            ProgressClockProgressColor = If(ConfigToken("Screensaver")?("ProgressClock")?("Color of Information"), 7)

            'Screensaver: Delays
            BouncingBlockDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingBlock")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("BouncingBlock")?("Delay in Milliseconds"), 10)
            BouncingTextDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("BouncingText")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("BouncingText")?("Delay in Milliseconds"), 10)
            ColorMixDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("ColorMix")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("ColorMix")?("Delay in Milliseconds"), 1)
            DiscoDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Disco")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Disco")?("Delay in Milliseconds"), 100)
            GlitterColorDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterColor")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("GlitterColor")?("Delay in Milliseconds"), 1)
            GlitterMatrixDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("GlitterMatrix")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("GlitterMatrix")?("Delay in Milliseconds"), 1)
            LinesDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Lines")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Lines")?("Delay in Milliseconds"), 500)
            MatrixDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Matrix")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Matrix")?("Delay in Milliseconds"), 1)
            LighterDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Lighter")?("Delay in Milliseconds"), 100)
            FaderDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Fader")?("Delay in Milliseconds"), 50)
            FaderFadeOutDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Fade Out Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Fader")?("Fade Out Delay in Milliseconds"), 3000)
            ProgressClockCycleColorsTicks = If(Integer.TryParse(ConfigToken("Screensaver")?("ProgressClock")?("Ticks to change color"), 0), ConfigToken("Screensaver")?("ProgressClock")?("Ticks to change color"), 20)
            TypoDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Typo")?("Delay in Milliseconds"), 50)
            TypoWriteAgainDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Write Again Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Typo")?("Write Again Delay in Milliseconds"), 3000)
            WipeDelay = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Delay in Milliseconds"), 0), ConfigToken("Screensaver")?("Wipe")?("Delay in Milliseconds"), 10)

            'Screensaver: Texts
            BouncingTextWrite = If(ConfigToken("Screensaver")?("BouncingText")?("Text Shown"), "Kernel Simulator")
            FaderWrite = If(ConfigToken("Screensaver")?("Fader")?("Text Shown"), "Kernel Simulator")
            TypoWrite = If(ConfigToken("Screensaver")?("Typo")?("Text Shown"), "Kernel Simulator")

            'Screensaver: Misc
            LighterMaxPositions = If(Integer.TryParse(ConfigToken("Screensaver")?("Lighter")?("Max Positions Count"), 0), ConfigToken("Screensaver")?("Lighter")?("Max Positions Count"), 10)
            FaderMaxSteps = If(Integer.TryParse(ConfigToken("Screensaver")?("Fader")?("Max Fade Steps"), 0), ConfigToken("Screensaver")?("Fader")?("Max Fade Steps"), 25)
            TypoWritingSpeedMin = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Minimum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Typo")?("Minimum writing speed in WPM"), 50)
            TypoWritingSpeedMax = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Maximum writing speed in WPM"), 0), ConfigToken("Screensaver")?("Typo")?("Maximum writing speed in WPM"), 80)
            TypoMissStrikePossibility = If(Integer.TryParse(ConfigToken("Screensaver")?("Typo")?("Probability of typo in percent"), 0), ConfigToken("Screensaver")?("Typo")?("Probability of typo in percent"), 60)
            WipeWipesNeededToChangeDirection = If(Integer.TryParse(ConfigToken("Screensaver")?("Wipe")?("Wipes to change direction"), 0), ConfigToken("Screensaver")?("Wipe")?("Wipes to change direction"), 10)

            'Misc Section
            Wdbg("I", "Parsing misc section...")
            CornerTD = If(ConfigToken("Misc")?("Show Time/Date on Upper Right Corner"), False)
            StartScroll = If(ConfigToken("Misc")?("Marquee on startup"), True)
            LongTimeDate = If(ConfigToken("Misc")?("Long Time and Date"), True)
            PreferredUnit = If(ConfigToken("Misc")?("Preferred Unit for Temperature") IsNot Nothing, If([Enum].TryParse(ConfigToken("Misc")?("Preferred Unit for Temperature"), PreferredUnit), PreferredUnit, UnitMeasurement.Metric), UnitMeasurement.Metric)
            TextEdit_AutoSaveFlag = If(ConfigToken("Misc")?("Enable text editor autosave"), True)
            TextEdit_AutoSaveInterval = If(Integer.TryParse(ConfigToken("Misc")?("Text editor autosave interval"), 0), ConfigToken("Misc")?("Text editor autosave interval"), 60)
            WrapListOutputs = If(ConfigToken("Misc")?("Wrap list outputs"), False)

            'Check to see if the config needs fixes
            RepairConfig()

            'Raise event and return true
            EventManager.RaiseConfigRead()
            Return True
        Catch nre As NullReferenceException
            'Rare, but repair config if an NRE is caught.
            Wdbg("E", "Error trying to read config: {0}", nre.Message)
            RepairConfig()
        Catch ex As Exception
            EventManager.RaiseConfigReadError(ex)
            WStkTrc(ex)
            NotifyConfigError = True
            Wdbg("E", "Error trying to read config: {0}", ex.Message)
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
        If Not File.Exists(paths("Configuration")) Then
            Wdbg("E", "No config file found. Creating...")
            CreateConfig()
        End If

        'Load and read config
        Try
            ReadConfig()
        Catch cex As Exceptions.ConfigException
            W(cex.Message, True, ColTypes.Error)
            WStkTrc(cex)
        End Try
    End Sub

    ''' <summary>
    ''' Checks to see if the config needs repair and repairs it as necessary.
    ''' </summary>
    ''' <returns>True if the config is repaired; False if no repairs done; Throws exceptions if unsuccessful.</returns>
    Public Function RepairConfig() As Boolean
        'Variables
        Dim FixesNeeded As Boolean

        'Check for missing sections
        If ConfigToken.Count <> 9 Then
            Wdbg("W", "Missing sections. Config fix needed set to true.")
            FixesNeeded = True
        End If
        If ConfigToken("Screensaver") IsNot Nothing Then
            If ConfigToken("Screensaver").Count <> 14 + 2 Then 'Screensavers + Keys
                Wdbg("W", "Missing sections and/or keys in Screensaver. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If

        'Now, check for missing keys in each section that ARE available.
        If ConfigToken("General") IsNot Nothing Then
            If ConfigToken("General").Count <> 8 Then
                Wdbg("W", "Missing keys in General. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Colors") IsNot Nothing Then
            If ConfigToken("Colors").Count <> 15 Then
                Wdbg("W", "Missing keys in Colors. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Hardware") IsNot Nothing Then
            If ConfigToken("Hardware").Count <> 2 Then
                Wdbg("W", "Missing keys in Hardware. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Login") IsNot Nothing Then
            If ConfigToken("Login").Count <> 4 Then
                Wdbg("W", "Missing keys in Login. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Shell") IsNot Nothing Then
            If ConfigToken("Shell").Count <> 8 Then
                Wdbg("W", "Missing keys in Shell. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Filesystem") IsNot Nothing Then
            If ConfigToken("Filesystem").Count <> 5 Then
                Wdbg("W", "Missing keys in Filesystem. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Network") IsNot Nothing Then
            If ConfigToken("Network").Count <> 12 Then
                Wdbg("W", "Missing keys in Network. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If
        If ConfigToken("Screensaver") IsNot Nothing Then
            If ConfigToken("Screensaver")("ColorMix") IsNot Nothing Then
                If ConfigToken("Screensaver")("ColorMix").Count <> 3 Then
                    Wdbg("W", "Missing keys in Screensaver > ColorMix. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Disco") IsNot Nothing Then
                If ConfigToken("Screensaver")("Disco").Count <> 4 Then
                    Wdbg("W", "Missing keys in Screensaver > Disco. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("GlitterColor") IsNot Nothing Then
                If ConfigToken("Screensaver")("GlitterColor").Count <> 3 Then
                    Wdbg("W", "Missing keys in Screensaver > GlitterColor. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Lines") IsNot Nothing Then
                If ConfigToken("Screensaver")("Lines").Count <> 3 Then
                    Wdbg("W", "Missing keys in Screensaver > Lines. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Dissolve") IsNot Nothing Then
                If ConfigToken("Screensaver")("Dissolve").Count <> 2 Then
                    Wdbg("W", "Missing keys in Screensaver > Dissolve. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("BouncingBlock") IsNot Nothing Then
                If ConfigToken("Screensaver")("BouncingBlock").Count <> 3 Then
                    Wdbg("W", "Missing keys in Screensaver > BouncingBlock. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("BouncingText") IsNot Nothing Then
                If ConfigToken("Screensaver")("BouncingText").Count <> 4 Then
                    Wdbg("W", "Missing keys in Screensaver > BouncingText. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("ProgressClock") IsNot Nothing Then
                If ConfigToken("Screensaver")("ProgressClock").Count <> 8 Then
                    Wdbg("W", "Missing keys in Screensaver > ProgressClock. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Lighter") IsNot Nothing Then
                If ConfigToken("Screensaver")("Lighter").Count <> 4 Then
                    Wdbg("W", "Missing keys in Screensaver > Lighter. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Wipe") IsNot Nothing Then
                If ConfigToken("Screensaver")("Wipe").Count <> 4 Then
                    Wdbg("W", "Missing keys in Screensaver > Wipe. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Matrix") IsNot Nothing Then
                If ConfigToken("Screensaver")("Matrix").Count <> 1 Then
                    Wdbg("W", "Missing keys in Screensaver > Matrix. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("GlitterMatrix") IsNot Nothing Then
                If ConfigToken("Screensaver")("GlitterMatrix").Count <> 1 Then
                    Wdbg("W", "Missing keys in Screensaver > GlitterMatrix. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Fader") IsNot Nothing Then
                If ConfigToken("Screensaver")("Fader").Count <> 4 Then
                    Wdbg("W", "Missing keys in Screensaver > Fader. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
            If ConfigToken("Screensaver")("Typo") IsNot Nothing Then
                If ConfigToken("Screensaver")("Typo").Count <> 6 Then
                    Wdbg("W", "Missing keys in Screensaver > Typo. Config fix needed set to true.")
                    FixesNeeded = True
                End If
            End If
        End If
        If ConfigToken("Misc") IsNot Nothing Then
            If ConfigToken("Misc").Count <> 7 Then
                Wdbg("W", "Missing keys in Misc. Config fix needed set to true.")
                FixesNeeded = True
            End If
        End If

        'If the fixes are needed, try to remake config with parsed values
        If FixesNeeded Then CreateConfig()
        Return FixesNeeded
    End Function

End Module
