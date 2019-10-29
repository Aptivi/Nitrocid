
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
                        New IniKey(ksconf, "Definition of command in Help Color", cmdDefColor.ToString)))

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
                        New IniKey(ksconf, "Host Name", HName)))

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
                        New IniKey(ksconf, "Definition of command in Help Color", cmdDefColor.ToString)))

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
                        New IniKey(ksconf, "Host Name", "kernel")))

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
                        New IniKey(ksconf, "Kernel Version", KernelVersion)))
            End If

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
                W(DoTranslation("There is an error trying to create configuration: {0}.", currentLang) + vbNewLine + ex.StackTrace, True, ColTypes.Neutral, Err.Description)
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
                Wdbg("Kernel version upgraded to {0} from {1}", KernelVersion, configUpdater.Sections("Misc").Keys("Kernel Version").Value)
                W(DoTranslation("An upgrade from {0} to {1} was detected. Updating configuration...", currentLang), True, ColTypes.Neutral, configUpdater.Sections("Misc").Keys("Kernel Version").Value, KernelVersion)
                UpdateConfig()
            End If
        Catch ex As Exception
            If DebugMode = True Then
                WStkTrc(ex)
                W(DoTranslation("There is an error trying to update configuration: {0}.", currentLang) + vbNewLine + ex.StackTrace, True, ColTypes.Neutral, Err.Description)
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
            SetLang(configReader.Sections("General").Keys("Language").Value)

            'Colored Shell
            If configReader.Sections("Shell").Keys("Colored Shell").Value = "False" Then
                TemplateSet("LinuxUncolored")
                ColoredShell = False
            End If

            '----------------------------- General configuration -----------------------------
            'Colors Section
            If ColoredShell Then userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("User Name Shell Color").Value), ConsoleColor)
            If ColoredShell Then hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Host Name Shell Color").Value), ConsoleColor)
            If ColoredShell Then contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Continuable Kernel Error Color").Value), ConsoleColor)
            If ColoredShell Then uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Uncontinuable Kernel Error Color").Value), ConsoleColor)
            If ColoredShell Then neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Text Color").Value), ConsoleColor)
            If ColoredShell Then licenseColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("License Color").Value), ConsoleColor)
            If ColoredShell Then
                backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Background Color").Value), ConsoleColor)
                Load()
            End If
            If ColoredShell Then inputColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Input Color").Value), ConsoleColor)
            If ColoredShell Then cmdListColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Listed command in help Color").Value), ConsoleColor)
            If ColoredShell Then cmdDefColor = CType([Enum].Parse(GetType(ConsoleColor), configReader.Sections("Colors").Keys("Definition of command in Help Color").Value), ConsoleColor)

            'General Section
            If configReader.Sections("General").Keys("Create Demo Account").Value = "True" Then enableDemo = True Else enableDemo = False
            If configReader.Sections("General").Keys("Change Root Password").Value = "True" Then setRootPasswd = True Else setRootPasswd = False
            If setRootPasswd = True Then RootPasswd = configReader.Sections("General").Keys("Set Root Password to").Value
            If configReader.Sections("General").Keys("Maintenance Mode").Value = "True" Then maintenance = True Else maintenance = False
            If configReader.Sections("General").Keys("Prompt for Arguments on Boot").Value = "True" Then argsOnBoot = True Else argsOnBoot = False

            'Login Section
            If configReader.Sections("Login").Keys("Clear Screen on Log-in").Value = "True" Then clsOnLogin = True Else clsOnLogin = False
            If configReader.Sections("Login").Keys("Show MOTD on Log-in").Value = "True" Then showMOTD = True Else showMOTD = False
            HName = configReader.Sections("Login").Keys("Host Name").Value

            'Shell Section
            If configReader.Sections("Shell").Keys("Simplified Help Command").Value = "True" Then simHelp = True Else simHelp = False

            'Hardware Section
            If configReader.Sections("Hardware").Keys("Probe Slots").Value = "True" Then slotProbe = True Else slotProbe = False
            If configReader.Sections("Hardware").Keys("Quiet Probe").Value = "True" Then quietProbe = True Else quietProbe = False

            'Misc Section
            If configReader.Sections("Misc").Keys("Show Time/Date on Upper Right Corner").Value = "True" Then CornerTD = True Else CornerTD = False
            defSaverName = configReader.Sections("Misc").Keys("Screensaver").Value
            If Integer.TryParse(configReader.Sections("Misc").Keys("Debug Port").Value, 0) Then DebugPort = configReader.Sections("Misc").Keys("Debug Port").Value
            If Integer.TryParse(configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value, 0) Then DebugQuota = configReader.Sections("Misc").Keys("Debug Size Quota in Bytes").Value
            RDebugDNP = configReader.Sections("Misc").Keys("Remote Debug Default Nick Prefix").Value
            If Integer.TryParse(configReader.Sections("Misc").Keys("Download Retry Times").Value, 0) Then DRetries = configReader.Sections("Misc").Keys("Download Retry Times").Value
        Catch nre As NullReferenceException 'Old config file being read. It is not appropriate to let KS crash on startup when the old version is read, so convert.
            UpgradeConfig() 'Upgrades the config if there are any changes.
        Catch ex As Exception
            If DebugMode = True Then
                WStkTrc(ex)
                W(DoTranslation("There is an error trying to read configuration: {0}.", currentLang) + vbNewLine + ex.StackTrace, True, ColTypes.Neutral, Err.Description)
            Else
                W(DoTranslation("There is an error trying to read configuration.", currentLang), True, ColTypes.Neutral)
            End If
        End Try
    End Sub

    Sub InitializeConfig()
        Dim pathConfig As String = paths("Configuration")
        If Not File.Exists(pathConfig) Then createConfig(False, False)
        configReader.Load(pathConfig)
        ReadConfig()
        checkForUpgrade()
    End Sub

End Module
