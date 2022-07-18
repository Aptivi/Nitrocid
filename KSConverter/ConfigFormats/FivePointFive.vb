
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

#If Not NETCOREAPP Then
Imports System.Globalization
Imports KS.ConsoleBase
Imports KS.Languages
Imports KS.Kernel
Imports KS.Misc.Forecast
Imports KS.Misc.Screensaver
Imports KS.Misc.Screensaver.Displays
Imports KS.Misc.Editors.TextEdit
Imports KS.Misc.Writers.DebugWriters
Imports KS.Misc.Writers.ConsoleWriters
Imports KS.Network.FTP
Imports KS.Network.Mail.Directory
Imports KS.Network.Mail
Imports KS.Network.RemoteDebug
Imports KS.Network.RPC
Imports KS.Network.SFTP
Imports KS.Network.SSH
Imports KS.Network
Imports KS.Shell
Imports MadMilkman.Ini
Imports Extensification.StringExts
Imports ManagedWeatherMap.Core

Module FivePointFive

    ''' <summary>
    ''' Takes configuration values and installs them to appropriate variables.
    ''' </summary>
    ''' <param name="PathToConfig">Path to 0.0.5.5+ config (kernelConfig.ini)</param>
    Function ReadFivePointFiveConfig(PathToConfig As String) As Boolean
        Try
            Dim ConfigReader As New IniFile()
            Dim ValidFormat As Boolean
            Debug.WriteLine("Reading post-0.0.5.5 config...")
            ConfigReader.Load(PathToConfig)

            'Check for sections
            If ConfigReader.Sections.Contains("General") And ConfigReader.Sections.Contains("Colors") And ConfigReader.Sections.Contains("Hardware") And ConfigReader.Sections.Contains("Login") And ConfigReader.Sections.Contains("Shell") And ConfigReader.Sections.Contains("Misc") Then
                Debug.WriteLine("Valid config!")
                ValidFormat = True
            End If

            'Now, install the values - General section
            If ConfigReader.Sections("General").Keys.Contains("Maintenance Mode") Then
                If ConfigReader.Sections("General").Keys("Maintenance Mode").Value = "True" Then Maintenance = True Else Maintenance = False
            End If
            If ConfigReader.Sections("General").Keys.Contains("Prompt for Arguments on Boot") Then
                If ConfigReader.Sections("General").Keys("Prompt for Arguments on Boot").Value = "True" Then ArgsOnBoot = True Else ArgsOnBoot = False
            End If
            If ConfigReader.Sections("General").Keys.Contains("Check for Updates on Startup") Then
                If ConfigReader.Sections("General").Keys("Check for Updates on Startup").Value = "True" Then CheckUpdateStart = True Else CheckUpdateStart = False
            End If
            If ConfigReader.Sections("General").Keys.Contains("Change Culture when Switching Languages") Then
                If ConfigReader.Sections("General").Keys("Change Culture when Switching Languages").Value = "True" Then LangChangeCulture = True Else LangChangeCulture = False
            End If
            If ConfigReader.Sections("General").Keys.Contains("Language") Then
                Dim ConfiguredLang As String = ConfigReader.Sections("General").Keys("Language").Value
                SetLang(If(String.IsNullOrWhiteSpace(ConfiguredLang), "eng", ConfiguredLang))
            End If
            If ConfigReader.Sections("General").Keys.Contains("Culture") Then
                If LangChangeCulture Then CurrentCult = New CultureInfo(ConfigReader.Sections("General").Keys("Culture").Value)
            End If

            'Colors section
            If ColoredShell Then
                'We use New Color() to parse entered color. This is to ensure that the kernel can use the correct VT sequence.
                If ConfigReader.Sections("Colors").Keys.Contains("User Name Shell Color") Then UserNameShellColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("User Name Shell Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Host Name Shell Color") Then HostNameShellColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Host Name Shell Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Continuable Kernel Error Color") Then ContKernelErrorColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Continuable Kernel Error Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Uncontinuable Kernel Error Color") Then UncontKernelErrorColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Text Color") Then NeutralTextColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Text Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("License Color") Then LicenseColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("License Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Background Color") Then BackgroundColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Background Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Input Color") Then InputColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Input Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("List Entry Color") Then ListEntryColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("List Entry Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("List Value Color") Then ListValueColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("List Value Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Kernel Stage Color") Then StageColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Kernel Stage Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Error Text Color") Then ErrorColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Error Text Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Warning Text Color") Then WarningColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Warning Text Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Option Color") Then OptionColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Option Color").Value)))
                If ConfigReader.Sections("Colors").Keys.Contains("Banner Color") Then BannerColor = New Color(Convert.ToInt32([Enum].Parse(GetType(ConsoleColors), ConfigReader.Sections("Colors").Keys("Banner Color").Value)))
            End If

            'Login section
            If ConfigReader.Sections("Login").Keys.Contains("Clear Screen on Log-in") Then
                If ConfigReader.Sections("Login").Keys("Clear Screen on Log-in").Value = "True" Then ClearOnLogin = True Else ClearOnLogin = False
            End If
            If ConfigReader.Sections("Login").Keys.Contains("Show MOTD on Log-in") Then
                If ConfigReader.Sections("Login").Keys("Show MOTD on Log-in").Value = "True" Then ShowMOTD = True Else ShowMOTD = False
            End If
            If ConfigReader.Sections("Login").Keys.Contains("Show available usernames") Then
                If ConfigReader.Sections("Login").Keys("Show available usernames").Value = "True" Then ShowAvailableUsers = True Else ShowAvailableUsers = False
            End If
            If ConfigReader.Sections("Login").Keys.Contains("Host Name") Then
                If Not ConfigReader.Sections("Login").Keys("Host Name").Value = "" Then
                    HostName = ConfigReader.Sections("Login").Keys("Host Name").Value
                Else
                    HostName = "kernel"
                End If
            End If

            'Shell section
            If ConfigReader.Sections("Shell").Keys.Contains("Simplified Help Command") Then
                If ConfigReader.Sections("Shell").Keys("Simplified Help Command").Value = "True" Then SimHelp = True Else SimHelp = False
            End If
            If ConfigReader.Sections("Shell").Keys.Contains("Colored Shell") Then
                If ConfigReader.Sections("Shell").Keys("Colored Shell").Value = "True" Then ColoredShell = True Else ColoredShell = False
            End If
            If ConfigReader.Sections("Shell").Keys.Contains("Current Directory") Then
                CurrDir = ConfigReader.Sections("Shell").Keys("Current Directory").Value
            End If
            If ConfigReader.Sections("Shell").Keys.Contains("Lookup Directories") Then
                PathsToLookup = ConfigReader.Sections("Shell").Keys("Lookup Directories").Value.ReleaseDoubleQuotes
            End If
            If ConfigReader.Sections("Shell").Keys.Contains("Prompt Style") Then
                ShellPromptStyle = ConfigReader.Sections("Shell").Keys("Prompt Style").Value
            End If
            If ConfigReader.Sections("Shell").Keys.Contains("FTP Prompt Style") Then
                FTPShellPromptStyle = ConfigReader.Sections("Shell").Keys("FTP Prompt Style").Value
            End If
            If ConfigReader.Sections("Shell").Keys.Contains("Mail Prompt Style") Then
                MailShellPromptStyle = ConfigReader.Sections("Shell").Keys("Mail Prompt Style").Value
            End If
            If ConfigReader.Sections("Shell").Keys.Contains("SFTP Prompt Style") Then
                SFTPShellPromptStyle = ConfigReader.Sections("Shell").Keys("SFTP Prompt Style").Value
            End If

            'Hardware section
            If ConfigReader.Sections("Hardware").Keys.Contains("Quiet Probe") Then
                If ConfigReader.Sections("Hardware").Keys("Quiet Probe").Value = "True" Then QuietHardwareProbe = True Else QuietHardwareProbe = False
            End If
            If ConfigReader.Sections("Hardware").Keys.Contains("Full Probe") Then
                If ConfigReader.Sections("Hardware").Keys("Full Probe").Value = "True" Then FullHardwareProbe = True Else FullHardwareProbe = False
            End If

            'Network section
            If ConfigReader.Sections("Network")?.Keys?.Contains("Debug Port") Then
                If Integer.TryParse(ConfigReader.Sections("Network").Keys("Debug Port").Value, 0) Then DebugPort = ConfigReader.Sections("Network").Keys("Debug Port").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Download Retry Times") Then
                If Integer.TryParse(ConfigReader.Sections("Network").Keys("Download Retry Times").Value, 0) Then DownloadRetries = ConfigReader.Sections("Network").Keys("Download Retry Times").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Upload Retry Times") Then
                If Integer.TryParse(ConfigReader.Sections("Network").Keys("Upload Retry Times").Value, 0) Then UploadRetries = ConfigReader.Sections("Network").Keys("Upload Retry Times").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Show progress bar while downloading or uploading from ""get"" or ""put"" command") Then
                ShowProgress = ConfigReader.Sections("Network").Keys("Show progress bar while downloading or uploading from ""get"" or ""put"" command").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Log FTP username") Then
                FTPLoggerUsername = ConfigReader.Sections("Network").Keys("Log FTP username").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Log FTP IP address") Then
                FTPLoggerIP = ConfigReader.Sections("Network").Keys("Log FTP IP address").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Return only first FTP profile") Then
                FTPFirstProfileOnly = ConfigReader.Sections("Network").Keys("Return only first FTP profile").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Show mail message preview") Then
                ShowPreview = ConfigReader.Sections("Network").Keys("Show mail message preview").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Record chat to debug log") Then
                RecordChatToDebugLog = ConfigReader.Sections("Network").Keys("Record chat to debug log").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Show SSH banner") Then
                SSHBanner = ConfigReader.Sections("Network").Keys("Show SSH banner").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("Enable RPC") Then
                RPCEnabled = ConfigReader.Sections("Network").Keys("Enable RPC").Value
            End If
            If ConfigReader.Sections("Network")?.Keys?.Contains("RPC Port") Then
                If Integer.TryParse(ConfigReader.Sections("Network").Keys("RPC Port").Value, 0) Then RPCPort = ConfigReader.Sections("Network").Keys("RPC Port").Value
            End If

            'Screensaver section
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Screensaver") Then
                DefSaverName = ConfigReader.Sections("Screensaver").Keys("Screensaver").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Screensaver Timeout in ms") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Screensaver Timeout in ms").Value, 0) Then ScrnTimeout = ConfigReader.Sections("Screensaver").Keys("Screensaver Timeout in ms").Value
            End If

            'Screensaver: Colors
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ColorMix - Activate 255 Color Mode") Then
                ColorMix255Colors = ConfigReader.Sections("Screensaver").Keys("ColorMix - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Disco - Activate 255 Color Mode") Then
                Disco255Colors = ConfigReader.Sections("Screensaver").Keys("Disco - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("GlitterColor - Activate 255 Color Mode") Then
                GlitterColor255Colors = ConfigReader.Sections("Screensaver").Keys("GlitterColor - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Lines - Activate 255 Color Mode") Then
                Lines255Colors = ConfigReader.Sections("Screensaver").Keys("Lines - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Dissolve - Activate 255 Color Mode") Then
                Dissolve255Colors = ConfigReader.Sections("Screensaver").Keys("Dissolve - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("BouncingBlock - Activate 255 Color Mode") Then
                BouncingBlock255Colors = ConfigReader.Sections("Screensaver").Keys("BouncingBlock - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Activate 255 Color Mode") Then
                ProgressClock255Colors = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Lighter - Activate 255 Color Mode") Then
                Lighter255Colors = ConfigReader.Sections("Screensaver").Keys("Lighter - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Wipe - Activate 255 Color Mode") Then
                Wipe255Colors = ConfigReader.Sections("Screensaver").Keys("Wipe - Activate 255 Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ColorMix - Activate True Color Mode") Then
                ColorMixTrueColor = ConfigReader.Sections("Screensaver").Keys("ColorMix - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Disco - Activate True Color Mode") Then
                DiscoTrueColor = ConfigReader.Sections("Screensaver").Keys("Disco - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("GlitterColor - Activate True Color Mode") Then
                GlitterColorTrueColor = ConfigReader.Sections("Screensaver").Keys("GlitterColor - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Lines - Activate True Color Mode") Then
                LinesTrueColor = ConfigReader.Sections("Screensaver").Keys("Lines - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Dissolve - Activate True Color Mode") Then
                DissolveTrueColor = ConfigReader.Sections("Screensaver").Keys("Dissolve - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("BouncingBlock - Activate True Color Mode") Then
                BouncingBlockTrueColor = ConfigReader.Sections("Screensaver").Keys("BouncingBlock - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Activate True Color Mode") Then
                ProgressClockTrueColor = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Lighter - Activate True Color Mode") Then
                LighterTrueColor = ConfigReader.Sections("Screensaver").Keys("Lighter - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Wipe - Activate True Color Mode") Then
                WipeTrueColor = ConfigReader.Sections("Screensaver").Keys("Wipe - Activate True Color Mode").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Disco - Cycle Colors") Then
                DiscoCycleColors = ConfigReader.Sections("Screensaver").Keys("Disco - Cycle Colors").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Cycle Colors") Then
                ProgressClockCycleColors = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Cycle Colors").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Color of Seconds Bar") Then
                ProgressClockSecondsProgressColor = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Color of Seconds Bar").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Color of Minutes Bar") Then
                ProgressClockMinutesProgressColor = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Color of Minutes Bar").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Color of Hours Bar") Then
                ProgressClockHoursProgressColor = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Color of Hours Bar").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Color of Information") Then
                ProgressClockProgressColor = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Color of Information").Value
            End If

            'Screensaver: Delays
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("BouncingBlock - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("BouncingBlock - Delay in Milliseconds").Value, 0) Then BouncingBlockDelay = ConfigReader.Sections("Screensaver").Keys("BouncingBlock - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("BouncingText - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("BouncingText - Delay in Milliseconds").Value, 0) Then BouncingTextDelay = ConfigReader.Sections("Screensaver").Keys("BouncingText - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ColorMix - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("ColorMix - Delay in Milliseconds").Value, 0) Then ColorMixDelay = ConfigReader.Sections("Screensaver").Keys("ColorMix - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Disco - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Disco - Delay in Milliseconds").Value, 0) Then DiscoDelay = ConfigReader.Sections("Screensaver").Keys("Disco - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("GlitterColor - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("GlitterColor - Delay in Milliseconds").Value, 0) Then GlitterColorDelay = ConfigReader.Sections("Screensaver").Keys("GlitterColor - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("GlitterMatrix - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("GlitterMatrix - Delay in Milliseconds").Value, 0) Then GlitterMatrixDelay = ConfigReader.Sections("Screensaver").Keys("GlitterMatrix - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Lines - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Lines - Delay in Milliseconds").Value, 0) Then LinesDelay = ConfigReader.Sections("Screensaver").Keys("Lines - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Matrix - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Matrix - Delay in Milliseconds").Value, 0) Then MatrixDelay = ConfigReader.Sections("Screensaver").Keys("Matrix - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Lighter - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Lighter - Delay in Milliseconds").Value, 0) Then LighterDelay = ConfigReader.Sections("Screensaver").Keys("Lighter - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Fader - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Fader - Delay in Milliseconds").Value, 0) Then FaderDelay = ConfigReader.Sections("Screensaver").Keys("Fader - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Fader - Fade Out Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Fader - Fade Out Delay in Milliseconds").Value, 0) Then FaderFadeOutDelay = ConfigReader.Sections("Screensaver").Keys("Fader - Fade Out Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("ProgressClock - Ticks to change color") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("ProgressClock - Ticks to change color").Value, 0) Then ProgressClockCycleColorsTicks = ConfigReader.Sections("Screensaver").Keys("ProgressClock - Ticks to change color").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Typo - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Typo - Delay in Milliseconds").Value, 0) Then TypoDelay = ConfigReader.Sections("Screensaver").Keys("Typo - Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Typo - Write Again Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Typo - Write Again Delay in Milliseconds").Value, 0) Then TypoWriteAgainDelay = ConfigReader.Sections("Screensaver").Keys("Typo - Write Again Delay in Milliseconds").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Wipe - Delay in Milliseconds") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Wipe - Delay in Milliseconds").Value, 0) Then WipeDelay = ConfigReader.Sections("Screensaver").Keys("Wipe - Delay in Milliseconds").Value
            End If

            'Screensaver: Texts
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("BouncingText - Text Shown") Then
                BouncingTextWrite = ConfigReader.Sections("Screensaver").Keys("BouncingText - Text Shown").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Fader - Text Shown") Then
                FaderWrite = ConfigReader.Sections("Screensaver").Keys("Fader - Text Shown").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Typo - Text Shown") Then
                TypoWrite = ConfigReader.Sections("Screensaver").Keys("Typo - Text Shown").Value
            End If

            'Screensaver: Misc
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Lighter - Max Positions Count") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Lighter - Max Positions Count").Value, 0) Then LighterMaxPositions = ConfigReader.Sections("Screensaver").Keys("Lighter - Max Positions Count").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Fader - Max Fade Steps") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Fader - Max Fade Steps").Value, 0) Then FaderMaxSteps = ConfigReader.Sections("Screensaver").Keys("Fader - Max Fade Steps").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Typo - Minimum writing speed in WPM") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Typo - Minimum writing speed in WPM").Value, 0) Then TypoWritingSpeedMin = ConfigReader.Sections("Screensaver").Keys("Typo - Minimum writing speed in WPM").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Typo - Maximum writing speed in WPM") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Typo - Maximum writing speed in WPM").Value, 0) Then TypoWritingSpeedMax = ConfigReader.Sections("Screensaver").Keys("Typo - Maximum writing speed in WPM").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Typo - Probability of typo in percent") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Typo - Probability of typo in percent").Value, 0) Then TypoMissStrikePossibility = ConfigReader.Sections("Screensaver").Keys("Typo - Probability of typo in percent").Value
            End If
            If ConfigReader.Sections("Screensaver")?.Keys?.Contains("Wipe - Wipes to change direction") Then
                If Integer.TryParse(ConfigReader.Sections("Screensaver").Keys("Wipe - Wipes to change direction").Value, 0) Then WipeWipesNeededToChangeDirection = ConfigReader.Sections("Screensaver").Keys("Wipe - Wipes to change direction").Value
            End If

            'Misc section
            If ConfigReader.Sections("Misc").Keys.Contains("Show Time/Date on Upper Right Corner") Then
                If ConfigReader.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").Value = "True" Then CornerTimeDate = True Else CornerTimeDate = False
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Debug Size Quota in Bytes") Then
                If Integer.TryParse(ConfigReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value, 0) Then DebugQuota = ConfigReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Size parse mode") Then
                If ConfigReader.Sections("Misc").Keys("Size parse mode").Value = "True" Then FullParseMode = True Else FullParseMode = False
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Marquee on startup") Then
                If ConfigReader.Sections("Misc").Keys("Marquee on startup").Value = "True" Then StartScroll = True Else StartScroll = False
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Long Time and Date") Then
                If ConfigReader.Sections("Misc").Keys("Long Time and Date").Value = "True" Then LongTimeDate = True Else LongTimeDate = False
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Show Hidden Files") Then
                If ConfigReader.Sections("Misc").Keys("Show Hidden Files").Value = "True" Then HiddenFiles = True Else HiddenFiles = False
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Preferred Unit for Temperature") Then
                PreferredUnit = [Enum].Parse(GetType(UnitMeasurement), ConfigReader.Sections("Misc").Keys("Preferred Unit for Temperature").Value)
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Enable text editor autosave") Then
                If ConfigReader.Sections("Misc").Keys("Enable text editor autosave").Value = "True" Then TextEdit_AutoSaveFlag = True Else TextEdit_AutoSaveFlag = False
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Text editor autosave interval") Then
                If Integer.TryParse(ConfigReader.Sections("Misc").Keys("Text editor autosave interval").Value, 0) Then TextEdit_AutoSaveInterval = ConfigReader.Sections("Misc").Keys("Text editor autosave interval").Value
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Wrap list outputs") Then
                If ConfigReader.Sections("Misc").Keys("Wrap list outputs").Value = "True" Then WrapListOutputs = True Else WrapListOutputs = False
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Filesystem sort mode") Then
                SortMode = [Enum].Parse(GetType(FilesystemSortOptions), ConfigReader.Sections("Misc").Keys("Filesystem sort mode").Value)
            End If
            If ConfigReader.Sections("Misc").Keys.Contains("Filesystem sort direction") Then
                SortDirection = [Enum].Parse(GetType(FilesystemSortDirection), ConfigReader.Sections("Misc").Keys("Filesystem sort direction").Value)
            End If

            'Return valid format
            Debug.WriteLine($"Returning ValidFormat as {ValidFormat}...")
            Return ValidFormat
        Catch ex As Exception
            Debug.WriteLine($"Error while converting config! {ex.Message}")
            Write("  - Warning: Failed to completely convert config. Some of the configurations might not be fully migrated.", True, ColTypes.Warning)
            Return False
        End Try
    End Function

End Module
#End If
