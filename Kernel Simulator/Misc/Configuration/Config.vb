
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

Public Module Config

    ''' <summary>
    ''' Creates the kernel configuration file
    ''' </summary>
    ''' <param name="Preserve">Preserves configuration values</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    ''' <exception cref="EventsAndExceptions.ConfigException"></exception>
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
                        New IniKey(ksconf, "Language", currentLang)))

                'The Colors Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Colors",
                        New IniKey(ksconf, "User Name Shell Color", userNameShellColor.ToString),
                        New IniKey(ksconf, "Host Name Shell Color", hostNameShellColor.ToString),
                        New IniKey(ksconf, "Continuable Kernel Error Color", contKernelErrorColor.ToString),
                        New IniKey(ksconf, "Uncontinuable Kernel Error Color", uncontKernelErrorColor.ToString),
                        New IniKey(ksconf, "Text Color", neutralTextColor.ToString),
                        New IniKey(ksconf, "License Color", licenseColor.ToString),
                        New IniKey(ksconf, "Background Color", backgroundColor.ToString),
                        New IniKey(ksconf, "Input Color", inputColor.ToString),
                        New IniKey(ksconf, "Listed command in Help Color", cmdListColor.ToString),
                        New IniKey(ksconf, "Definition of command in Help Color", cmdDefColor.ToString),
                        New IniKey(ksconf, "Kernel Stage Color", stageColor.ToString),
                        New IniKey(ksconf, "Error Text Color", errorColor.ToString)))

                'The Hardware Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Hardware",
                        New IniKey(ksconf, "Quiet Probe", quietProbe),
                        New IniKey(ksconf, "Probe Slots", slotProbe)))

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
                        New IniKey(ksconf, "Prompt Style", ShellPromptStyle),
                        New IniKey(ksconf, "FTP Prompt Style", FTPShellPromptStyle),
                        New IniKey(ksconf, "Mail Prompt Style", MailShellPromptStyle),
                        New IniKey(ksconf, "SFTP Prompt Style", SFTPShellPromptStyle)))

                'The Network Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Network",
                        New IniKey(ksconf, "Debug Port", DebugPort),
                        New IniKey(ksconf, "Remote Debug Default Nick Prefix", RDebugDNP),
                        New IniKey(ksconf, "Download Retry Times", DRetries),
                        New IniKey(ksconf, "Upload Retry Times", URetries),
                        New IniKey(ksconf, "Show progress bar while downloading or uploading from ""get"" or ""put"" command", ShowProgress),
                        New IniKey(ksconf, "Log FTP username", FTPLoggerUsername),
                        New IniKey(ksconf, "Log FTP IP address", FTPLoggerIP),
                        New IniKey(ksconf, "Return only first FTP profile", FTPFirstProfileOnly),
                        New IniKey(ksconf, "Show mail message preview", ShowPreview)))

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
                        New IniKey(ksconf, "ColorMix - Activate True Color Mode", ColorMixTrueColor),
                        New IniKey(ksconf, "Disco - Activate True Color Mode", DiscoTrueColor),
                        New IniKey(ksconf, "GlitterColor - Activate True Color Mode", GlitterColorTrueColor),
                        New IniKey(ksconf, "Lines - Activate True Color Mode", LinesTrueColor),
                        New IniKey(ksconf, "Dissolve - Activate True Color Mode", DissolveTrueColor),
                        New IniKey(ksconf, "Disco - Cycle Colors", DiscoCycleColors),
                        New IniKey(ksconf, "BouncingText - Text Shown", BouncingTextWrite)))

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
                        New IniKey(ksconf, "User Name Shell Color", userNameShellColor.ToString),
                        New IniKey(ksconf, "Host Name Shell Color", hostNameShellColor.ToString),
                        New IniKey(ksconf, "Continuable Kernel Error Color", contKernelErrorColor.ToString),
                        New IniKey(ksconf, "Uncontinuable Kernel Error Color", uncontKernelErrorColor.ToString),
                        New IniKey(ksconf, "Text Color", neutralTextColor.ToString),
                        New IniKey(ksconf, "License Color", licenseColor.ToString),
                        New IniKey(ksconf, "Background Color", backgroundColor.ToString),
                        New IniKey(ksconf, "Input Color", inputColor.ToString),
                        New IniKey(ksconf, "Listed command in Help Color", cmdListColor.ToString),
                        New IniKey(ksconf, "Definition of command in Help Color", cmdDefColor.ToString),
                        New IniKey(ksconf, "Kernel Stage Color", stageColor.ToString),
                        New IniKey(ksconf, "Error Text Color", errorColor.ToString)))

                'The Hardware Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Hardware",
                        New IniKey(ksconf, "Quiet Probe", "False"),
                        New IniKey(ksconf, "Probe Slots", "True")))

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
                        New IniKey(ksconf, "Prompt Style", ""),
                        New IniKey(ksconf, "FTP Prompt Style", ""),
                        New IniKey(ksconf, "Mail Prompt Style", ""),
                        New IniKey(ksconf, "SFTP Prompt Style", "")))

                'The Network Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Network",
                        New IniKey(ksconf, "Debug Port", 3014),
                        New IniKey(ksconf, "Remote Debug Default Nick Prefix", "KSUser"),
                        New IniKey(ksconf, "Download Retry Times", 3),
                        New IniKey(ksconf, "Upload Retry Times", 3),
                        New IniKey(ksconf, "Show progress bar while downloading or uploading from ""get"" or ""put"" command", "True"),
                        New IniKey(ksconf, "Log FTP username", "False"),
                        New IniKey(ksconf, "Log FTP IP address", "False"),
                        New IniKey(ksconf, "Return only first FTP profile", "False"),
                        New IniKey(ksconf, "Show mail message preview", "False")))

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
                        New IniKey(ksconf, "ColorMix - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Disco - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "GlitterColor - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Lines - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Dissolve - Activate True Color Mode", "True"),
                        New IniKey(ksconf, "Disco - Cycle Colors", "False"),
                        New IniKey(ksconf, "BouncingText - Text Shown", "Kernel Simulator")))

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
            ksconf.Sections("Colors").TrailingComment.Text = "Self-explanatory. You can just write the name of colors as specified in the ConsoleColors enumerator."

            'Login
            ksconf.Sections("Login").TrailingComment.Text = "This section is the login settings that lets you control the host name and whether or not it shows MOTD and/or clears screen."
            ksconf.Sections("Login").Keys("Clear Screen on Log-in").TrailingComment.Text = "Whether or not it clears screen on sign-in."
            ksconf.Sections("Login").Keys("Show MOTD on Log-in").TrailingComment.Text = "Whether or not it shows MOTD on sign-in."
            ksconf.Sections("Login").Keys("Host Name").TrailingComment.Text = "Custom host name. It will be used in the future for networking references, but is currently here to customize shell prompt."
            ksconf.Sections("Login").Keys("Show available usernames").TrailingComment.Text = "Whether or not to show available usernames on login"

            'Shell
            ksconf.Sections("Shell").TrailingComment.Text = "This section is the shell settings that lets you control whether or not to enable simplified help command and/or colored shell."
            ksconf.Sections("Shell").Keys("Simplified Help Command").TrailingComment.Text = "Simplifies the ""help"" command so it only shows available commands."
            ksconf.Sections("Shell").Keys("Colored Shell").TrailingComment.Text = "Whether or not it supports colored shell."
            ksconf.Sections("Shell").Keys("Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the main shell. Placeholders here are parsed."
            ksconf.Sections("Shell").Keys("FTP Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the FTP shell. Placeholders here are parsed."
            ksconf.Sections("Shell").Keys("Mail Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the mail shell. Placeholders here are parsed."
            ksconf.Sections("Shell").Keys("SFTP Prompt Style").TrailingComment.Text = "Prompt style. Leave blank to use default style. It only affects the SFTP shell. Placeholders here are parsed."

            'Hardware
            ksconf.Sections("Hardware").TrailingComment.Text = "This section is the hardware probing settings that lets you control whether or not to probe RAM slots and/or quietly probe hardware. This section and the two settings are deprecated."
            ksconf.Sections("Hardware").Keys("Probe Slots").TrailingComment.Text = "Whether or not to probe RAM slots on boot"
            ksconf.Sections("Hardware").Keys("Quiet Probe").TrailingComment.Text = "Whether or not to quietly probe hardware"

            'Network
            ksconf.Sections("Network").TrailingComment.Text = "This section is the network settings."
            ksconf.Sections("Network").Keys("Debug Port").TrailingComment.Text = "Specifies the remote debugger port."
            ksconf.Sections("Network").Keys("Remote Debug Default Nick Prefix").TrailingComment.Text = "The name, which will be prepended to the random device ID."
            ksconf.Sections("Network").Keys("Download Retry Times").TrailingComment.Text = "How many times does the ""get"" command retry the download before assuming failure?"
            ksconf.Sections("Network").Keys("Upload Retry Times").TrailingComment.Text = "How many times does the ""put"" command retry the upload before assuming failure?"
            ksconf.Sections("Network").Keys("Show progress bar while downloading or uploading from ""get"" or ""put"" command").TrailingComment.Text = "If true, it makes ""get"" or ""put"" show the progress bar while downloading or uploading."
            ksconf.Sections("Network").Keys("Log FTP username").TrailingComment.Text = "Whether or not to log FTP username in the debugger log."
            ksconf.Sections("Network").Keys("Log FTP IP address").TrailingComment.Text = "Whether or not to log FTP IP address in the debugger log."
            ksconf.Sections("Network").Keys("Return only first FTP profile").TrailingComment.Text = "Whether or not to return only first successful FTP profile when polling for profiles."
            ksconf.Sections("Network").Keys("Show mail message preview").TrailingComment.Text = "Whether or not to show mail message preview (body text truncated to 200 characters)."

            'Screensaver
            ksconf.Sections("Screensaver").TrailingComment.Text = "This section is the network settings."
            ksconf.Sections("Screensaver").Keys("Screensaver").TrailingComment.Text = "Specifies the current screensaver."
            ksconf.Sections("Screensaver").Keys("Screensaver Timeout in ms").TrailingComment.Text = "After specified milliseconds, the screensaver will launch."
            ksconf.Sections("Screensaver").Keys("ColorMix - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for ColorMix"
            ksconf.Sections("Screensaver").Keys("Disco - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Disco"
            ksconf.Sections("Screensaver").Keys("GlitterColor - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for GlitterColor"
            ksconf.Sections("Screensaver").Keys("Lines - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Lines"
            ksconf.Sections("Screensaver").Keys("Dissolve - Activate 255 Color Mode").TrailingComment.Text = "Activates the 255 color mode for Dissolve"
            ksconf.Sections("Screensaver").Keys("ColorMix - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for ColorMix"
            ksconf.Sections("Screensaver").Keys("Disco - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Disco"
            ksconf.Sections("Screensaver").Keys("GlitterColor - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for GlitterColor"
            ksconf.Sections("Screensaver").Keys("Lines - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Lines"
            ksconf.Sections("Screensaver").Keys("Dissolve - Activate True Color Mode").TrailingComment.Text = "Activates the true color mode for Dissolve"
            ksconf.Sections("Screensaver").Keys("Disco - Cycle Colors").TrailingComment.Text = "Disco will cycle colors if it's enabled. Otherwise, select random colors."
            ksconf.Sections("Screensaver").Keys("BouncingText - Text Shown").TrailingComment.Text = "Any text for BouncingText"

            'Misc
            ksconf.Sections("Misc").TrailingComment.Text = "This section is the other settings that are not categorized yet."
            ksconf.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").TrailingComment.Text = "Whether or not it shows time and date on the upper right corner."
            ksconf.Sections("Misc").Keys("Debug Size Quota in Bytes").TrailingComment.Text = "Specifies the maximum log size in bytes. If this was exceeded, it will remove the first 5 lines from the log to free up some space."
            ksconf.Sections("Misc").Keys("Size parse mode").TrailingComment.Text = "Parse whole directory for size. If set to False, it will parse just the surface."
            ksconf.Sections("Misc").Keys("Marquee on startup").TrailingComment.Text = "Whether or not to activate banner animation."
            ksconf.Sections("Misc").Keys("Long Time and Date").TrailingComment.Text = "Whether or not to render time and date using long."
            ksconf.Sections("Misc").Keys("Show Hidden Files").TrailingComment.Text = "Whether or not to list hidden files."
            ksconf.Sections("Misc").Keys("Preferred Unit for Temperature").TrailingComment.Text = "Choose either Kelvin, Celsius, or Fahrenheit for temperature measurement."

            'Save Config
            ksconf.Save(paths("Configuration"))
            Return True
        Catch ex As Exception
            If DebugMode = True Then
                WStkTrc(ex)
                Throw New EventsAndExceptions.ConfigException(DoTranslation("There is an error trying to create configuration: {0}.", currentLang).FormatString(ex.Message))
            Else
                Throw New EventsAndExceptions.ConfigException(DoTranslation("There is an error trying to create configuration.", currentLang))
            End If
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Checks the config file for mismatched version and upgrades it
    ''' </summary>
    ''' <returns>True if there are updates, False if unsuccessful.</returns>
    ''' <exception cref="EventsAndExceptions.ConfigException"></exception>
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
                Throw New EventsAndExceptions.ConfigException(DoTranslation("There is an error trying to update configuration: {0}.", currentLang).FormatString(ex.Message))
            Else
                Throw New EventsAndExceptions.ConfigException(DoTranslation("There is an error trying to update configuration.", currentLang))
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
    ''' <exception cref="EventsAndExceptions.ConfigException"></exception>
    Public Function ReadConfig() As Boolean
        Try
            '----------------------------- Important configuration -----------------------------
            'Language
            Wdbg("I", "Language is {0}", configReader.Sections("General").Keys("Language").Value)
            If configReader.Sections("General").Keys("Change Culture when Switching Languages").Value = "True" Then LangChangeCulture = True Else LangChangeCulture = False
            SetLang(configReader.Sections("General").Keys("Language").Value)

            'Colored Shell
            If configReader.Sections("Shell").Keys("Colored Shell").Value = "False" Then
                Wdbg("W", "Detected uncolored shell. Removing colors...")
                TemplateSet("LinuxUncolored")
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
            If ColoredShell Then userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("User Name Shell Color").Value), ConsoleColors)
            If ColoredShell Then hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Host Name Shell Color").Value), ConsoleColors)
            If ColoredShell Then contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Continuable Kernel Error Color").Value), ConsoleColors)
            If ColoredShell Then uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value), ConsoleColors)
            If ColoredShell Then neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Text Color").Value), ConsoleColors)
            If ColoredShell Then licenseColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("License Color").Value), ConsoleColors)
            If ColoredShell Then
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Background Color").Value), ConsoleColors)
                LoadBack()
            End If
            If ColoredShell Then inputColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Input Color").Value), ConsoleColors)
            If ColoredShell Then cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Listed command in help Color").Value), ConsoleColors)
            If ColoredShell Then cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Definition of command in Help Color").Value), ConsoleColors)
            If ColoredShell Then stageColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Kernel Stage Color").Value), ConsoleColors)
            If ColoredShell Then errorColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Error Text Color").Value), ConsoleColors)

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
            ShellPromptStyle = configReader.Sections("Shell").Keys("Prompt Style").Value
            FTPShellPromptStyle = configReader.Sections("Shell").Keys("FTP Prompt Style").Value
            MailShellPromptStyle = configReader.Sections("Shell").Keys("Mail Prompt Style").Value
            SFTPShellPromptStyle = configReader.Sections("Shell").Keys("SFTP Prompt Style").Value

            'Hardware Section
            Wdbg("I", "Parsing hardware section...")
            If configReader.Sections("Hardware").Keys("Probe Slots").Value = "True" Then slotProbe = True Else slotProbe = False
            If configReader.Sections("Hardware").Keys("Quiet Probe").Value = "True" Then quietProbe = True Else quietProbe = False

            'Network Section
            Wdbg("I", "Parsing network section...")
            If Integer.TryParse(configReader.Sections("Network").Keys("Debug Port").Value, 0) Then DebugPort = configReader.Sections("Network").Keys("Debug Port").Value
            RDebugDNP = configReader.Sections("Network").Keys("Remote Debug Default Nick Prefix").Value
            If Integer.TryParse(configReader.Sections("Network").Keys("Download Retry Times").Value, 0) Then DRetries = configReader.Sections("Network").Keys("Download Retry Times").Value
            If Integer.TryParse(configReader.Sections("Network").Keys("Upload Retry Times").Value, 0) Then URetries = configReader.Sections("Network").Keys("Upload Retry Times").Value
            ShowProgress = configReader.Sections("Network").Keys("Show progress bar while downloading or uploading from ""get"" or ""put"" command").Value
            FTPLoggerUsername = configReader.Sections("Network").Keys("Log FTP username").Value
            FTPLoggerIP = configReader.Sections("Network").Keys("Log FTP IP address").Value
            FTPFirstProfileOnly = configReader.Sections("Network").Keys("Return only first FTP profile").Value
            ShowPreview = configReader.Sections("Network").Keys("Show mail message preview").Value

            'Screensaver Section
            defSaverName = configReader.Sections("Screensaver").Keys("Screensaver").Value
            If Integer.TryParse(configReader.Sections("Screensaver").Keys("Screensaver Timeout in ms").Value, 0) Then ScrnTimeout = configReader.Sections("Screensaver").Keys("Screensaver Timeout in ms").Value
            ColorMix255Colors = configReader.Sections("Screensaver").Keys("ColorMix - Activate 255 Color Mode").Value
            Disco255Colors = configReader.Sections("Screensaver").Keys("Disco - Activate 255 Color Mode").Value
            GlitterColor255Colors = configReader.Sections("Screensaver").Keys("GlitterColor - Activate 255 Color Mode").Value
            Lines255Colors = configReader.Sections("Screensaver").Keys("Lines - Activate 255 Color Mode").Value
            Dissolve255Colors = configReader.Sections("Screensaver").Keys("Dissolve - Activate 255 Color Mode").Value
            ColorMixTrueColor = configReader.Sections("Screensaver").Keys("ColorMix - Activate True Color Mode").Value
            DiscoTrueColor = configReader.Sections("Screensaver").Keys("Disco - Activate True Color Mode").Value
            GlitterColorTrueColor = configReader.Sections("Screensaver").Keys("GlitterColor - Activate True Color Mode").Value
            LinesTrueColor = configReader.Sections("Screensaver").Keys("Lines - Activate True Color Mode").Value
            DissolveTrueColor = configReader.Sections("Screensaver").Keys("Dissolve - Activate True Color Mode").Value
            DiscoCycleColors = configReader.Sections("Screensaver").Keys("Disco - Cycle Colors").Value
            BouncingTextWrite = configReader.Sections("Screensaver").Keys("BouncingText - Text Shown").Value

            'Misc Section
            Wdbg("I", "Parsing misc section...")
            If configReader.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").Value = "True" Then CornerTD = True Else CornerTD = False
            If Integer.TryParse(configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value, 0) Then DebugQuota = configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value
            If configReader.Sections("Misc").Keys("Size parse mode").Value = "True" Then FullParseMode = True Else FullParseMode = False
            If configReader.Sections("Misc").Keys("Marquee on startup").Value = "True" Then StartScroll = True Else StartScroll = False
            If configReader.Sections("Misc").Keys("Long Time and Date").Value = "True" Then LongTimeDate = True Else LongTimeDate = False
            If configReader.Sections("Misc").Keys("Show Hidden Files").Value = "True" Then HiddenFiles = True Else HiddenFiles = False
            PreferredUnit = configReader.Sections("Misc").Keys("Preferred Unit for Temperature").Value

            Return True
        Catch nre As NullReferenceException 'Old config file being read. It is not appropriate to let KS crash on startup when the old version is read, so convert.
            Wdbg("W", "Detected incompatible/old version of config. Renewing...")
            UpgradeConfig() 'Upgrades the config if there are any changes.
        Catch ex As Exception
            WStkTrc(ex)
            NotifyConfigError = True
            Throw New EventsAndExceptions.ConfigException(DoTranslation("There is an error trying to read configuration: {0}.", currentLang).FormatString(ex.Message))
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
        Catch cex As EventsAndExceptions.ConfigException
            W(cex.Message, True, ColTypes.Err)
            WStkTrc(cex)
        End Try

        'Check for updates for config
        If CheckForUpgrade() Then
            W(DoTranslation("An upgrade to {0} is detected. Updating configuration...", currentLang), True, ColTypes.Neutral, KernelVersion)
            UpdateConfig()
        End If
    End Sub

End Module
