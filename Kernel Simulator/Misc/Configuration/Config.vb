
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

    Public Sub CreateConfig(ByVal CmdArg As Boolean, ByVal Preserve As Boolean)
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
                        New IniKey(ksconf, "Create Demo Account", enableDemo),
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
                        New IniKey(ksconf, "Kernel Stage Color", stageColor.ToString)))

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
                        New IniKey(ksconf, "Simplified Help Command", simHelp)))

                'Misc Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Misc",
                        New IniKey(ksconf, "Show Time/Date on Upper Right Corner", CornerTD),
                        New IniKey(ksconf, "Screensaver", defSaverName),
                        New IniKey(ksconf, "Debug Port", DebugPort),
                        New IniKey(ksconf, "Debug Size Quota in Bytes", DebugQuota),
                        New IniKey(ksconf, "Remote Debug Default Nick Prefix", RDebugDNP),
                        New IniKey(ksconf, "Download Retry Times", DRetries),
                        New IniKey(ksconf, "Log FTP username", FTPLoggerUsername),
                        New IniKey(ksconf, "Log FTP IP address", FTPLoggerIP),
                        New IniKey(ksconf, "Size parse mode", FullParseMode),
                        New IniKey(ksconf, "Marquee on startup", StartScroll),
                        New IniKey(ksconf, "Long Time and Date", LongTimeDate),
                        New IniKey(ksconf, "Show Hidden Files", HiddenFiles),
                        New IniKey(ksconf, "Kernel Version", KernelVersion)))
            Else '----------------------- If [Preserve] value is False, then don't preserve.
                'The General Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "General",
                        New IniKey(ksconf, "Prompt for Arguments on Boot", "False"),
                        New IniKey(ksconf, "Maintenance Mode", "False"),
                        New IniKey(ksconf, "Change Root Password", "False"),
                        New IniKey(ksconf, "Set Root Password to", "toor"),
                        New IniKey(ksconf, "Create Demo Account", "True"),
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
                        New IniKey(ksconf, "Kernel Stage Color", stageColor.ToString)))

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
                        New IniKey(ksconf, "Simplified Help Command", "False")))

                'Misc Section
                ksconf.Sections.Add(
                    New IniSection(ksconf, "Misc",
                        New IniKey(ksconf, "Show Time/Date on Upper Right Corner", "False"),
                        New IniKey(ksconf, "Screensaver", "matrix"),
                        New IniKey(ksconf, "Debug Port", 3014),
                        New IniKey(ksconf, "Debug Size Quota in Bytes", 1073741824),
                        New IniKey(ksconf, "Remote Debug Default Nick Prefix", "KSUser"),
                        New IniKey(ksconf, "Download Retry Times", 3),
                        New IniKey(ksconf, "Log FTP username", "False"),   '> Keep them off by default
                        New IniKey(ksconf, "Log FTP IP address", "False"), '> for privacy protection
                        New IniKey(ksconf, "Size parse mode", "False"),
                        New IniKey(ksconf, "Marquee on startup", "True"),
                        New IniKey(ksconf, "Long Time and Date", "True"),
                        New IniKey(ksconf, "Show Hidden Files", "False"),
                        New IniKey(ksconf, "Kernel Version", KernelVersion)))
            End If

            'Put comments before saving. General
            ksconf.Sections("General").TrailingComment.Text = "This section is the general settings for KS. It controls boot settings and regional settings."
            ksconf.Sections("General").Keys("Prompt for Arguments on Boot").TrailingComment.Text = "If set to True, everytime the kernel boots, you'll be prompted for the kernel arguments."
            ksconf.Sections("General").Keys("Create Demo Account").TrailingComment.Text = "If set to True, it creates testing demonstration account to test the log-in system with the permissions. It will be removed in 0.0.8."
            ksconf.Sections("General").Keys("Change Root Password").TrailingComment.Text = "Whether or not to change root password. If it is set to True, it will set the password to a password that will be set in the config entry below."
            ksconf.Sections("General").Keys("Maintenance Mode").TrailingComment.Text = "Whether or not to start the kernel in maintenance mode."
            ksconf.Sections("General").Keys("Prompt for Arguments on Boot").TrailingComment.Text = "Whether or not to prompt for arguments on boot to let you set arguments on the current boot"

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

            'Hardware
            ksconf.Sections("Hardware").TrailingComment.Text = "This section is the hardware probing settings that lets you control whether or not to probe RAM slots and/or quietly probe hardware. This section and the two settings are deprecated."
            ksconf.Sections("Hardware").Keys("Probe Slots").TrailingComment.Text = "Whether or not to probe RAM slots on boot"
            ksconf.Sections("Hardware").Keys("Quiet Probe").TrailingComment.Text = "Whether or not to quietly probe hardware"

            'Misc
            ksconf.Sections("Misc").TrailingComment.Text = "This section is the other settings that are not categorized yet."
            ksconf.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").TrailingComment.Text = "Whether or not it shows time and date on the upper right corner."
            ksconf.Sections("Misc").Keys("Screensaver").TrailingComment.Text = "Specifies the current screensaver."
            ksconf.Sections("Misc").Keys("Debug Port").TrailingComment.Text = "Specifies the remote debugger port."
            ksconf.Sections("Misc").Keys("Debug Size Quota in Bytes").TrailingComment.Text = "Specifies the maximum log size in bytes. If this was exceeded, it will remove the first 5 lines from the log to free up some space."
            ksconf.Sections("Misc").Keys("Remote Debug Default Nick Prefix").TrailingComment.Text = "The name, which will be prepended to the random device ID."
            ksconf.Sections("Misc").Keys("Download Retry Times").TrailingComment.Text = "How many times does the ""get"" command retry the download before assuming failure?"
            ksconf.Sections("Misc").Keys("Log FTP username").TrailingComment.Text = "Whether or not to log FTP username in the debugger log."
            ksconf.Sections("Misc").Keys("Log FTP IP address").TrailingComment.Text = "Whether or not to log FTP IP address in the debugger log."
            ksconf.Sections("Misc").Keys("Size parse mode").TrailingComment.Text = "Parse whole directory for size. If set to False, it will parse just the surface."
            ksconf.Sections("Misc").Keys("Marquee on startup").TrailingComment.Text = "Whether or not to activate banner animation."
            ksconf.Sections("Misc").Keys("Long Time and Date").TrailingComment.Text = "Whether or not to render time and date using long."
            ksconf.Sections("Misc").Keys("Show Hidden Files").TrailingComment.Text = "Whether or not to list hidden files."

            'Save Config
            ksconf.Save(paths("Configuration"))

            'Exit if it is executed using real command-line arguments
            If CmdArg = True Then
                DisposeAll()
                Environment.Exit(0)
            End If
        Catch ex As Exception
            If DebugMode = True Then
                WStkTrc(ex)
                W(DoTranslation("There is an error trying to create configuration: {0}.", currentLang) + vbNewLine + ex.StackTrace, True, ColTypes.Neutral, ex.Message)
            Else
                W(DoTranslation("There is an error trying to create configuration.", currentLang), True, ColTypes.Neutral)
            End If
            If CmdArg = True Then
                DisposeAll()
                Environment.Exit(2)
            End If
        End Try
    End Sub

    Public Sub CheckForUpgrade()
        Try
            'Variables
            Dim configUpdater As New IniFile()

            'Load config
            configUpdater.Load(paths("Configuration"))

            'Check to see if the kernel is outdated
            If configUpdater.Sections("Misc").Keys("Kernel Version").Value <> KernelVersion Then
                Wdbg("W", "Kernel version upgraded from {1} to {0}", KernelVersion, configUpdater.Sections("Misc").Keys("Kernel Version").Value)
                W(DoTranslation("An upgrade from {0} to {1} was detected. Updating configuration...", currentLang), True, ColTypes.Neutral, configUpdater.Sections("Misc").Keys("Kernel Version").Value, KernelVersion)
                UpdateConfig()
            End If
        Catch ex As Exception
            If DebugMode = True Then
                WStkTrc(ex)
                W(DoTranslation("There is an error trying to update configuration: {0}.", currentLang) + vbNewLine + ex.StackTrace, True, ColTypes.Neutral, ex.Message)
            Else
                W(DoTranslation("There is an error trying to update configuration.", currentLang), True, ColTypes.Neutral)
            End If
        End Try
    End Sub

    Public Sub UpdateConfig()

        CreateConfig(False, True)
        PowerManage("reboot")

    End Sub

    Public Sub ReadConfig()
        Try
            '----------------------------- Important configuration -----------------------------
            'Language
            Wdbg("I", "Language is {0}", configReader.Sections("General").Keys("Language").Value)
            SetLang(configReader.Sections("General").Keys("Language").Value, True)

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
            'Colors Section (Not loaded if there are CI arguments passed)
            Wdbg("I", "Loading colors...")
            If Not Environment.GetCommandLineArgs.Contains("CI-") Then
                Wdbg("I", "Not on CI environment.")
                If ColoredShell Then userNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("User Name Shell Color").Value), ConsoleColors)
                If ColoredShell Then hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Host Name Shell Color").Value), ConsoleColors)
                If ColoredShell Then contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Continuable Kernel Error Color").Value), ConsoleColors)
                If ColoredShell Then uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value), ConsoleColors)
                If ColoredShell Then neutralTextColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Text Color").Value), ConsoleColors)
                If ColoredShell Then licenseColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("License Color").Value), ConsoleColors)
                If ColoredShell Then
                    backgroundColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Background Color").Value), ConsoleColors)
                    Load()
                End If
                If ColoredShell Then inputColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Input Color").Value), ConsoleColors)
                If ColoredShell Then cmdListColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Listed command in help Color").Value), ConsoleColors)
                If ColoredShell Then cmdDefColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Definition of command in Help Color").Value), ConsoleColors)
                If ColoredShell Then stageColor = CType([Enum].Parse(GetType(ConsoleColors), configReader.Sections("Colors").Keys("Kernel Stage Color").Value), ConsoleColors)
            Else
                Wdbg("W", "Running on CI environment. Ignoring...")
            End If

            'General Section
            Wdbg("I", "Parsing general section...")
            If configReader.Sections("General").Keys("Create Demo Account").Value = "True" Then enableDemo = True Else enableDemo = False
            If configReader.Sections("General").Keys("Change Root Password").Value = "True" Then setRootPasswd = True Else setRootPasswd = False
            If setRootPasswd = True Then RootPasswd = configReader.Sections("General").Keys("Set Root Password to").Value
            If configReader.Sections("General").Keys("Maintenance Mode").Value = "True" Then maintenance = True Else maintenance = False
            If configReader.Sections("General").Keys("Prompt for Arguments on Boot").Value = "True" Then argsOnBoot = True Else argsOnBoot = False

            'Login Section
            Wdbg("I", "Parsing login section...")
            If configReader.Sections("Login").Keys("Clear Screen on Log-in").Value = "True" Then clsOnLogin = True Else clsOnLogin = False
            If configReader.Sections("Login").Keys("Show MOTD on Log-in").Value = "True" Then showMOTD = True Else showMOTD = False
            If configReader.Sections("Login").Keys("Show available usernames").Value = "True" Then ShowAvailableUsers = True Else ShowAvailableUsers = False

            'Shell Section
            Wdbg("I", "Parsing shell section...")
            If configReader.Sections("Shell").Keys("Simplified Help Command").Value = "True" Then simHelp = True Else simHelp = False

            'Hardware Section
            Wdbg("I", "Parsing hardware section...")
            If configReader.Sections("Hardware").Keys("Probe Slots").Value = "True" Then slotProbe = True Else slotProbe = False
            If configReader.Sections("Hardware").Keys("Quiet Probe").Value = "True" Then quietProbe = True Else quietProbe = False

            'Misc Section
            Wdbg("I", "Parsing misc section...")
            If configReader.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").Value = "True" Then CornerTD = True Else CornerTD = False
            defSaverName = configReader.Sections("Misc").Keys("Screensaver").Value
            If Integer.TryParse(configReader.Sections("Misc").Keys("Debug Port").Value, 0) Then DebugPort = configReader.Sections("Misc").Keys("Debug Port").Value
            If Integer.TryParse(configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value, 0) Then DebugQuota = configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value
            RDebugDNP = configReader.Sections("Misc").Keys("Remote Debug Default Nick Prefix").Value
            If Integer.TryParse(configReader.Sections("Misc").Keys("Download Retry Times").Value, 0) Then DRetries = configReader.Sections("Misc").Keys("Download Retry Times").Value
            FTPLoggerUsername = configReader.Sections("Misc").Keys("Log FTP username").Value
            FTPLoggerIP = configReader.Sections("Misc").Keys("Log FTP IP address").Value
            If configReader.Sections("Misc").Keys("Size parse mode").Value = "True" Then FullParseMode = True Else FullParseMode = False
            If configReader.Sections("Misc").Keys("Marquee on startup").Value = "True" Then StartScroll = True Else StartScroll = False
            If configReader.Sections("Misc").Keys("Long Time and Date").Value = "True" Then LongTimeDate = True Else LongTimeDate = False
            If configReader.Sections("Misc").Keys("Show Hidden Files").Value = "True" Then HiddenFiles = True Else HiddenFiles = False
        Catch nre As NullReferenceException 'Old config file being read. It is not appropriate to let KS crash on startup when the old version is read, so convert.
            Wdbg("W", "Detected incompatible/old version of config. Renewing...")
            UpgradeConfig() 'Upgrades the config if there are any changes.
        Catch ex As Exception
            If DebugMode = True Then
                WStkTrc(ex)
                W(DoTranslation("There is an error trying to read configuration: {0}.", currentLang) + vbNewLine + ex.StackTrace, True, ColTypes.Neutral, ex.Message)
            Else
                W(DoTranslation("There is an error trying to read configuration: {0}.", currentLang), True, ColTypes.Neutral, ex.Message)
            End If
        End Try
    End Sub

    Sub InitializeConfig()
        'Make a config file if not found
        Dim pathConfig As String = paths("Configuration")
        If Not File.Exists(pathConfig) Then
            Wdbg("E", "No config file found. Creating...")
            CreateConfig(False, False)
        End If

        'Load and read config
        configReader.Load(pathConfig)
        Wdbg("I", "Config loaded with {0} sections", configReader.Sections.Count)
        ReadConfig()

        'Check for updates for config
        CheckForUpgrade()
    End Sub

End Module
