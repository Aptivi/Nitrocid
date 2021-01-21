
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

    Enum SettingsKeyType
        SBoolean
        SInt
        SString
        SMultivar
        SSelection
        SMenu
    End Enum

    Sub OpenMainPage()
        Dim PromptFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer

        While Not PromptFinished
            Console.Clear()
            'List sections
            W(DoTranslation("Select section:", currentLang) + vbNewLine, True, ColTypes.Neutral)
            W("1) " + DoTranslation("General Settings...", currentLang), True, ColTypes.Neutral)
            W("2) " + DoTranslation("Hardware Settings...", currentLang), True, ColTypes.Neutral)
            W("3) " + DoTranslation("Login Settings...", currentLang), True, ColTypes.Neutral)
            W("4) " + DoTranslation("Shell Settings...", currentLang), True, ColTypes.Neutral)
            W("5) " + DoTranslation("Network Settings...", currentLang), True, ColTypes.Neutral)
            W("6) " + DoTranslation("Screensaver Settings...", currentLang), True, ColTypes.Neutral)
            W("7) " + DoTranslation("Miscellaneous Settings...", currentLang) + vbNewLine, True, ColTypes.Neutral)
            W("8) " + DoTranslation("Save Settings", currentLang), True, ColTypes.Neutral)
            W("9) " + DoTranslation("Exit", currentLang) + vbNewLine, True, ColTypes.Neutral)

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
                    OpenSection(AnswerInt)
                ElseIf AnswerInt = 8 Then 'Save Settings
                    Wdbg("I", "Saving settings...")
                    Try
                        CreateConfig(True)
                    Catch ex As EventsAndExceptions.ConfigException
                        W(ex.Message, True, ColTypes.Err)
                        WStkTrc(ex)
                    End Try
                ElseIf AnswerInt = 9 Then 'Exit
                    Wdbg("W", "Exiting...")
                    PromptFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid.", currentLang), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric.", currentLang), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                Console.ReadKey()
            End If
        End While
    End Sub

    Sub OpenSection(ByVal SectionNum As Integer)
        Dim MaxOptions As Integer = 0
        Dim SectionFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer

        While Not SectionFinished
            Console.Clear()
            'List options
            W(DoTranslation("Select option:", currentLang) + vbNewLine, True, ColTypes.Neutral)
            Select Case SectionNum
                Case 1 'General
                    MaxOptions = 5
                    W(DoTranslation("This section lists all general kernel settings, mainly for maintaining the kernel.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Prompt for Arguments on Boot", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("argsOnBoot"))
                    W("2) " + DoTranslation("Maintenance Mode Trigger", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("maintenance"))
                    W("3) " + DoTranslation("Change Root Password...", currentLang), True, ColTypes.Neutral)
                    W("4) " + DoTranslation("Check for Updates on Startup", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("CheckUpdateStart"))
                    W("5) " + DoTranslation("Change Culture when Switching Languages", currentLang) + " [{0}]" + vbNewLine, True, ColTypes.Neutral, GetValue("LangChangeCulture"))
                Case 2 'Hardware
                    MaxOptions = 1
                    W(DoTranslation("This section changes hardware probe behavior.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Quiet Probe", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("quietProbe"))
                Case 3 'Login
                    MaxOptions = 3
                    W(DoTranslation("This section represents the login settings. Log out of your account for the changes to take effect.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Show MOTD on Log-in", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("showMOTD"))
                    W("2) " + DoTranslation("Clear Screen on Log-in", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("clsOnLogin"))
                    W("3) " + DoTranslation("Show available usernames", currentLang) + " [{0}]" + vbNewLine, True, ColTypes.Neutral, GetValue("ShowAvailableUsers"))
                Case 4 'Shell
                    MaxOptions = 7
                    W(DoTranslation("This section lists the shell settings.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Colored Shell", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("ColoredShell"))
                    W("2) " + DoTranslation("Simplified Help Command", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("simHelp"))
                    W("3) " + DoTranslation("Prompt Style", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("ShellPromptStyle"))
                    W("4) " + DoTranslation("FTP Prompt Style", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("FTPShellPromptStyle"))
                    W("5) " + DoTranslation("Mail Prompt Style", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("MailShellPromptStyle"))
                    W("6) " + DoTranslation("SFTP Prompt Style", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("SFTPShellPromptStyle"))
                    W("7) " + DoTranslation("Custom colors...", currentLang) + vbNewLine, True, ColTypes.Neutral)
                Case 5 'Network
                    MaxOptions = 9
                    W(DoTranslation("This section lists the network settings, like the FTP shell, the network-related command settings, and the remote debug settings.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Debug Port", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("DebugPort"))
                    W("2) " + DoTranslation("Remote Debug Default Nick Prefix", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("RDebugDNP"))
                    W("3) " + DoTranslation("Download Retry Times", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("DRetries"))
                    W("4) " + DoTranslation("Upload Retry Times", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("URetries"))
                    W("5) " + DoTranslation("Show progress bar while downloading or uploading from ""get"" or ""put"" command", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("ShowProgress"))
                    W("6) " + DoTranslation("Log FTP username", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("FTPLoggerUsername"))
                    W("7) " + DoTranslation("Log FTP IP address", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("FTPLoggerIP"))
                    W("8) " + DoTranslation("Return only first FTP profile", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("FTPFirstProfileOnly"))
                    W("9) " + DoTranslation("Show mail message preview", currentLang) + " [{0}]" + vbNewLine, True, ColTypes.Neutral, GetValue("ShowPreview"))
                Case 6 'Screensaver
                    MaxOptions = 13
                    W(DoTranslation("This section lists all the screensavers and their available settings.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Screensaver Timeout in ms", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("ScrnTimeout"))
                    W("2) [ColorMix] " + DoTranslation("Activate 255 colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("ColorMix255Colors"))
                    W("3) [Disco] " + DoTranslation("Activate 255 colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("Disco255Colors"))
                    W("4) [GlitterColor] " + DoTranslation("Activate 255 colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("GlitterColor255Colors"))
                    W("5) [Lines] " + DoTranslation("Activate 255 colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("Lines255Colors"))
                    W("6) [Dissolve] " + DoTranslation("Activate 255 colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("Dissolve255Colors"))
                    W("7) [ColorMix] " + DoTranslation("Activate true colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("ColorMixTrueColor"))
                    W("8) [Disco] " + DoTranslation("Activate true colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("DiscoTrueColor"))
                    W("9) [GlitterColor] " + DoTranslation("Activate true colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("GlitterColorTrueColor"))
                    W("10) [Lines] " + DoTranslation("Activate true colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("LinesTrueColor"))
                    W("11) [Dissolve] " + DoTranslation("Activate true colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("DissolveTrueColor"))
                    W("12) [Disco] " + DoTranslation("Cycle colors", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("DiscoCycleColors"))
                    W("13) [BouncingText] " + DoTranslation("Text shown", currentLang) + " [{0}]" + vbNewLine, True, ColTypes.Neutral, GetValue("BouncingTextWrite"))
                Case 7 'Misc
                    MaxOptions = 7
                    W(DoTranslation("Settings that don't fit in their appropriate sections land here.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                    W("1) " + DoTranslation("Show Time/Date on Upper Right Corner", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("CornerTD"))
                    W("2) " + DoTranslation("Debug Size Quota in Bytes", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("DebugQuota"))
                    W("3) " + DoTranslation("Size parse mode", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("FullParseMode"))
                    W("4) " + DoTranslation("Marquee on startup", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("StartScroll"))
                    W("5) " + DoTranslation("Long Time and Date", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("LongTimeDate"))
                    W("6) " + DoTranslation("Show Hidden Files", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("HiddenFiles"))
                    W("7) " + DoTranslation("Preferred Unit for Temperature", currentLang) + " [{0}]" + vbNewLine, True, ColTypes.Neutral, GetValue("PreferredUnit"))
                Case Else 'Invalid section
                    W("X) " + DoTranslation("Invalid section entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
            End Select
            W("{0}) " + DoTranslation("Go Back...", currentLang) + vbNewLine, True, ColTypes.Neutral, MaxOptions + 1)
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
                    If AnswerInt = 3 And SectionNum = 1 Then
                        Wdbg("I", "Tried to open special section. Opening section 1.3...")
                        OpenKey(1.3, AnswerInt)
                    ElseIf AnswerInt = 7 And SectionNum = 4 Then
                        Wdbg("I", "Tried to open special section. Opening section 4.7...")
                        OpenKey(4.7, AnswerInt)
                    Else
                        Wdbg("I", "Opening key {0} from section {1}...", AnswerInt, SectionNum)
                        OpenKey(SectionNum, AnswerInt)
                    End If
                ElseIf AnswerInt = MaxOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    SectionFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid.", currentLang), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            Else
                Wdbg("W", "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric.", currentLang), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                Console.ReadKey()
            End If
        End While
    End Sub

    Sub OpenKey(ByVal Section As Double, ByVal KeyNumber As Integer)
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
                Case 1 'General
                    Select Case KeyNumber
                        Case 1 'Prompt for Arguments on Boot
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "argsOnBoot"
                            W(DoTranslation("Sets up the kernel so it prompts you for argument on boot.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 2 'Maintenance Mode Trigger
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "maintenance"
                            W(DoTranslation("Triggers maintenance mode. This disables multiple accounts.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 3 'Change Root Password
                            OpenKey(Section, 1.3)
                        Case 4 'Check for Updates on Startup
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "CheckUpdateStart"
                            W(DoTranslation("Each startup, it will check for updates.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 5 'Change Culture when Switching Languages
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "LangChangeCulture"
                            W(DoTranslation("When switching languages, change the month names, calendar, etc.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case 1.3 'General -> Change Root Password
                    Select Case KeyNumber
                        Case 1
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "setRootPasswd"
                            W(DoTranslation("If the kernel is started, it will set root password.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 2
                            If GetValue("setRootPasswd") Then
                                KeyType = SettingsKeyType.SString
                                KeyVar = "RootPasswd"
                                W("*) " + DoTranslation("Write the root password to be set. Don't worry; the password are shown as stars.", currentLang), True, ColTypes.Neutral)
                            Else
                                W("X) " + DoTranslation("Enable ""Change Root Password"" to use this option. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                            End If
                        Case 3
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SMenu
                            W(DoTranslation("Select option:", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Change Root Password?", currentLang) + " [{0}]", True, ColTypes.Neutral, GetValue("setRootPasswd"))
                            W("2) " + DoTranslation("Set Root Password...", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case 2 'Hardware
                    Select Case KeyNumber
                        Case 1 'Quiet Probe
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "quietProbe"
                            W(DoTranslation("Keep hardware probing messages silent.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case 3 'Login
                    Select Case KeyNumber
                        Case 1 'Show MOTD on Log-in
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "showMOTD"
                            W(DoTranslation("Show Message of the Day before displaying login screen.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 2 'Clear Screen on Log-in
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "clsOnLogin"
                            W(DoTranslation("Clear screen before displaying login screen.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 3 'Show Available Usernames
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "ShowAvailableUsers"
                            W(DoTranslation("Shows available users if enabled.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case 4 'Shell
                    Select Case KeyNumber
                        Case 1 'Colored Shell
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "ColoredShell"
                            W(DoTranslation("Gives the kernel color support", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 2 'Simplified Help Command
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "simHelp"
                            W(DoTranslation("Simplified help command for all the shells", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 3 'Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = "ShellPromptStyle"
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed.", currentLang), True, ColTypes.Neutral)
                        Case 4 'FTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = "FTPShellPromptStyle"
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed.", currentLang), True, ColTypes.Neutral)
                        Case 5 'Mail Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = "MailShellPromptStyle"
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed.", currentLang), True, ColTypes.Neutral)
                        Case 6 'SFTP Prompt Style
                            KeyType = SettingsKeyType.SString
                            KeyVar = "SFTPShellPromptStyle"
                            W("*) " + DoTranslation("Write how you want your shell prompt to be. Leave blank to use default style. Placeholders are parsed.", currentLang), True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case 4.7 'Shell -> Custom colors
                    MaxKeyOptions = 12
                    KeyType = SettingsKeyType.SMultivar
                    KeyVars = New Dictionary(Of String, Object)
                    MultivarCustomAction = "SetColors"
                    Dim Response As String
                    W("*) " + DoTranslation("Write a color as specified below:", currentLang), True, ColTypes.Neutral)
                    W("*) " + String.Join(", ", [Enum].GetNames(GetType(ConsoleColors))) + vbNewLine, True, ColTypes.Neutral)

                    ' Input color
                    W("1) " + DoTranslation("Input color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("inputColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("inputColor")
                    KeyVars.AddOrModify("inputColor", Response)

                    ' License color
                    W("2) " + DoTranslation("License color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("licenseColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("licenseColor")
                    KeyVars.AddOrModify("licenseColor", Response)

                    ' Continuable kernel error color
                    W("3) " + DoTranslation("Continuable kernel error color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("contKernelErrorColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("contKernelErrorColor")
                    KeyVars.AddOrModify("contKernelErrorColor", Response)

                    ' Unontinuable kernel error color
                    W("4) " + DoTranslation("Uncontinuable kernel error color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("uncontKernelErrorColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("uncontKernelErrorColor")
                    KeyVars.AddOrModify("uncontKernelErrorColor", Response)

                    ' Host name color
                    W("5) " + DoTranslation("Host name color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("hostNameShellColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("hostNameShellColor")
                    KeyVars.AddOrModify("hostNameShellColor", Response)

                    ' User name color
                    W("6) " + DoTranslation("User name color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("userNameShellColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("userNameShellColor")
                    KeyVars.AddOrModify("userNameShellColor", Response)

                    ' Background color
                    W("7) " + DoTranslation("Background color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("backgroundColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("backgroundColor")
                    KeyVars.AddOrModify("backgroundColor", Response)

                    ' Neutral text color
                    W("8) " + DoTranslation("Neutral text color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("neutralTextColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("neutralTextColor")
                    KeyVars.AddOrModify("neutralTextColor", Response)

                    ' Command list color
                    W("9) " + DoTranslation("Command list color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("cmdListColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("cmdListColor")
                    KeyVars.AddOrModify("cmdListColor", Response)

                    ' Command definition color
                    W("10) " + DoTranslation("Command definition color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("cmdDefColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("cmdDefColor")
                    KeyVars.AddOrModify("cmdDefColor", Response)

                    ' Stage color
                    W("11) " + DoTranslation("Stage color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("stageColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("stageColor")
                    KeyVars.AddOrModify("stageColor", Response)

                    ' Error color
                    W("12) " + DoTranslation("Error color", currentLang) + ": [{0}] ", False, ColTypes.Input, GetValue("errorColor"))
                    Response = Console.ReadLine
                    If String.IsNullOrWhiteSpace(Response) Then Response = GetValue("errorColor")
                    KeyVars.AddOrModify("errorColor", Response)
                Case 5 'Network
                    Select Case KeyNumber
                        Case 1 'Debug Port
                            KeyType = SettingsKeyType.SInt
                            KeyVar = "DebugPort"
                            W("*) " + DoTranslation("Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port.", currentLang), True, ColTypes.Neutral)
                        Case 2 'Remote Debug Default Nick Prefix
                            KeyType = SettingsKeyType.SString
                            KeyVar = "RDebugDNP"
                            W("*) " + DoTranslation("Write the default remote debug nickname prefix.", currentLang), True, ColTypes.Neutral)
                        Case 3 'Download Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = "DRetries"
                            W("*) " + DoTranslation("Write how many times the ""get"" command should retry failed downloads. It must be numeric.", currentLang), True, ColTypes.Neutral)
                        Case 4 'Upload Retry Times
                            KeyType = SettingsKeyType.SInt
                            KeyVar = "URetries"
                            W("*) " + DoTranslation("Write how many times the ""put"" command should retry failed uploads. It must be numeric.", currentLang), True, ColTypes.Neutral)
                        Case 5 'Show progress bar while downloading or uploading from "get" or "put" command
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "ShowProgress"
                            W(DoTranslation("If true, it makes ""get"" or ""put"" show the progress bar while downloading or uploading.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 6 'Log FTP username
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "FTPLoggerUsername"
                            W(DoTranslation("Whether or not to log FTP username.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 7 'Log FTP IP address
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "FTPLoggerIP"
                            W(DoTranslation("Whether or not to log FTP IP address.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 8 'Return only first FTP profile
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "FTPFirstProfileOnly"
                            W(DoTranslation("Pick the first profile only when connecting.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 9 'Show mail message preview
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "ShowPreview"
                            W(DoTranslation("When listing mail messages, show body preview.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case 6 'Screensaver
                    Select Case KeyNumber
                        Case 1 'Screensaver Timeout in ms
                            KeyType = SettingsKeyType.SInt
                            KeyVar = "ScrnTimeout"
                            W("*) " + DoTranslation("Write when to launch screensaver after specified milliseconds. It must be numeric.", currentLang), True, ColTypes.Neutral)
                        Case 2 'ColorMix: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "ColorMix255Colors"
                            W(DoTranslation("Activates 255 color support for ColorMix.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 3 'Disco: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "Disco255Colors"
                            W(DoTranslation("Activates 255 color support for Disco.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 4 'GlitterColor: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "GlitterColor255Colors"
                            W(DoTranslation("Activates 255 color support for GlitterColor.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 5 'Lines: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "Lines255Colors"
                            W(DoTranslation("Activates 255 color support for Lines.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 6 'Dissolve: Activate 255 colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "Dissolve255Colors"
                            W(DoTranslation("Activates 255 color support for Dissolve.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 7 'ColorMix: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "ColorMixTrueColor"
                            W(DoTranslation("Activates true color support for ColorMix.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 8 'Disco: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "DiscoTrueColor"
                            W(DoTranslation("Activates true color support for Disco.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 9 'GlitterColor: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "GlitterColorTrueColor"
                            W(DoTranslation("Activates true color support for GlitterColor.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 10 'Lines: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "LinesTrueColor"
                            W(DoTranslation("Activates true color support for Lines.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 11 'Dissolve: Activate true colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "DissolveTrueColor"
                            W(DoTranslation("Activates true color support for Dissolve.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 12 'Disco: Cycle colors
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "DiscoCycleColors"
                            W(DoTranslation("Disco will cycle colors when enabled. Otherwise, select random colors.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 13 'Text shown
                            KeyType = SettingsKeyType.SString
                            KeyVar = "BouncingTextWrite"
                            W("*) " + DoTranslation("Write any text you want shown. Shorter is better.", currentLang), True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case 7 'Misc
                    Select Case KeyNumber
                        Case 1 'Show Time/Date on Upper Right Corner
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "CornerTD"
                            W(DoTranslation("The time and date will be shown in the upper right corner of the screen", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 2 'Debug Size Quota in Bytes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = "DebugQuota"
                            W("*) " + DoTranslation("Write how many bytes can the debug log store. It must be numeric.", currentLang), True, ColTypes.Neutral)
                        Case 3 'Size parse mode
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "FullParseMode"
                            W(DoTranslation("If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 4 'Marquee on startup
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "StartScroll"
                            W(DoTranslation("Enables eyecandy on startup", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 5 'Long Time and Date
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "LongTimeDate"
                            W(DoTranslation("The time and date will be longer, showing full month names, etc.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 6 'Show Hidden Files
                            MaxKeyOptions = 2
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = "HiddenFiles"
                            W(DoTranslation("Shows hidden files.", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Enable", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Disable", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case 7 'Preferred Unit for Temperature
                            MaxKeyOptions = 3
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = "PreferredUnit"
                            W(DoTranslation("Select your preferred unit for temperature (this only applies to the ""weather"" command)", currentLang) + vbNewLine, True, ColTypes.Neutral)
                            W("1) " + DoTranslation("Kelvin", currentLang), True, ColTypes.Neutral)
                            W("2) " + DoTranslation("Metric (Celsius)", currentLang), True, ColTypes.Neutral)
                            W("3) " + DoTranslation("Imperial (Fahrenheit)", currentLang) + vbNewLine, True, ColTypes.Neutral)
                        Case Else
                            W("X) " + DoTranslation("Invalid key number entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
                    End Select
                Case Else
                    W("X) " + DoTranslation("Invalid section entered. Please go back.", currentLang) + vbNewLine, True, ColTypes.Err)
            End Select

            'If user is on color selection screen, we'll give a user a confirmation.
            If Section = 4.7 Then
                W(vbNewLine + "*) " + DoTranslation("Do these color choices look OK?", currentLang), True, ColTypes.Neutral)
                For Each ColorType As String In KeyVars.Keys
                    W("   - {0}: ", False, ColTypes.HelpCmd, ColorType)
                    W(KeyVars(ColorType), True, ColTypes.HelpDef)
                Next
                W(vbNewLine + "*) " + DoTranslation("Answer {0} to go back. Otherwise, any answer means yes.", currentLang), True, ColTypes.Neutral, MaxKeyOptions + 1)
            End If

            'Add an option to go back.
            W("{0}) " + DoTranslation("Go Back...", currentLang) + vbNewLine, True, ColTypes.Neutral, MaxKeyOptions + 1)
            Wdbg("W", "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions)
            Wdbg("W", "Target variable: {0}, Key Type: {1}", KeyVar, KeyType)

            'Prompt user
            W("> ", False, ColTypes.Input)
            If KeyNumber = 2 And Section = 1.3 Then
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
                            SetValue(KeyVar, True)
                        Case 2 'False
                            Wdbg("I", "Setting to False...")
                            SetValue(KeyVar, False)
                    End Select
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid.", currentLang), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            ElseIf (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SInt) Or
                   (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SSelection) Then
                Wdbg("I", "Answer is numeric and key is of the {0} type.", KeyType)
                If AnswerInt >= 0 Then
                    Wdbg("I", "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                    KeyFinished = True
                    SetValue(KeyVar, AnswerInt)
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("W", "Negative values are disallowed.")
                    W(DoTranslation("The answer may not be negative.", currentLang), True, ColTypes.Err)
                    W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            ElseIf KeyType = SettingsKeyType.SString Then
                Wdbg("I", "Answer is not numeric and key is of the String type. Setting variable...")
                KeyFinished = True
                SetValue(KeyVar, AnswerString)
            ElseIf Section = 1.3 And KeyNumber = 3 Then
                Wdbg("I", "Answer is not numeric and the user is on the special section.")
                If AnswerInt >= 1 And AnswerInt <= 2 Then
                    Wdbg("I", "AnswerInt is {0}. Opening key...", AnswerInt)
                    OpenKey(Section, AnswerInt)
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg("I", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid.", currentLang), True, ColTypes.Err, AnswerInt)
                    W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                    Console.ReadKey()
                End If
            ElseIf KeyType = SettingsKeyType.SMultivar And MultivarCustomAction = "SetColors" Then
                Wdbg("I", "Multiple variables, and custom action was {0}.", MultivarCustomAction)
                Wdbg("I", "Answer was {0}", AnswerInt)
                If AnswerInt = 13 Then 'Go Back...
                    Wdbg("W", "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg("I", "Setting necessary variables...")
                    Wdbg("I", "Variables: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}.", KeyVars("inputColor"), KeyVars("licenseColor"), KeyVars("contKernelErrorColor"),
                         KeyVars("uncontKernelErrorColor"), KeyVars("hostNameShellColor"), KeyVars("userNameShellColor"), KeyVars("backgroundColor"), KeyVars("neutralTextColor"), KeyVars("cmdListColor"),
                         KeyVars("cmdDefColor"), KeyVars("stageColor"), KeyVars("errorColor"))

                    'This is cumbersome. This is worth an Extensification for [Enum].
                    If SetColors([Enum].Parse(GetType(ConsoleColors), KeyVars("inputColor")), [Enum].Parse(GetType(ConsoleColors), KeyVars("licenseColor")),
                                 [Enum].Parse(GetType(ConsoleColors), KeyVars("contKernelErrorColor")), [Enum].Parse(GetType(ConsoleColors), KeyVars("uncontKernelErrorColor")),
                                 [Enum].Parse(GetType(ConsoleColors), KeyVars("hostNameShellColor")), [Enum].Parse(GetType(ConsoleColors), KeyVars("userNameShellColor")),
                                 [Enum].Parse(GetType(ConsoleColors), KeyVars("backgroundColor")), [Enum].Parse(GetType(ConsoleColors), KeyVars("neutralTextColor")),
                                 [Enum].Parse(GetType(ConsoleColors), KeyVars("cmdListColor")), [Enum].Parse(GetType(ConsoleColors), KeyVars("cmdDefColor")),
                                 [Enum].Parse(GetType(ConsoleColors), KeyVars("stageColor")), [Enum].Parse(GetType(ConsoleColors), KeyVars("errorColor"))) Then
                        KeyFinished = True
                    End If
                End If
            Else
                Wdbg("W", "Answer is not valid.")
                W(DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers.", currentLang), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back.", currentLang), True, ColTypes.Err)
                Console.ReadKey()
            End If
        End While
    End Sub

    Public Sub SetValue(ByVal Variable As String, ByVal VariableValue As Object)
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
            W(DoTranslation("Variable {0} is not found on any of the modules.", currentLang), True, ColTypes.Err, Variable)
        End If
    End Sub

    Public Function GetValue(ByVal Variable As String) As Object
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
            W(DoTranslation("Variable {0} is not found on any of the modules.", currentLang), True, ColTypes.Err, Variable)
            Return Nothing
        End If
    End Function

    Public Function GetField(ByVal Variable As String) As FieldInfo
        'Get types of possible flag locations
        Dim TypeOfFlags As Type = GetType(Flags)
        Dim TypeOfKernel As Type = GetType(Kernel)
        Dim TypeOfShell As Type = GetType(Shell)
        Dim TypeOfFTPShell As Type = GetType(FTPShell)
        Dim TypeOfMailShell As Type = GetType(MailShell)
        Dim TypeOfSFTPShell As Type = GetType(SFTPShell)
        Dim TypeOfRDebugger As Type = GetType(RemoteDebugger)
        Dim TypeOfDebugWriters As Type = GetType(DebugWriters)
        Dim TypeOfNetworkTools As Type = GetType(NetworkTools)
        Dim TypeOfScreensaverSettings As Type = GetType(ScreensaverSettings)
        Dim TypeOfForecast As Type = GetType(Forecast)
        Dim TypeOfMailManager As Type = GetType(MailManager)
        Dim TypeOfColors As Type = GetType(Color)

        'Get fields of flag modules
        Dim FieldFlags As FieldInfo = TypeOfFlags.GetField(Variable)
        Dim FieldKernel As FieldInfo = TypeOfKernel.GetField(Variable)
        Dim FieldShell As FieldInfo = TypeOfShell.GetField(Variable)
        Dim FieldFTPShell As FieldInfo = TypeOfFTPShell.GetField(Variable)
        Dim FieldMailShell As FieldInfo = TypeOfMailShell.GetField(Variable)
        Dim FieldSFTPShell As FieldInfo = TypeOfSFTPShell.GetField(Variable)
        Dim FieldRDebugger As FieldInfo = TypeOfRDebugger.GetField(Variable)
        Dim FieldDebugWriters As FieldInfo = TypeOfDebugWriters.GetField(Variable)
        Dim FieldNetworkTools As FieldInfo = TypeOfNetworkTools.GetField(Variable)
        Dim FieldScreensaverSettings As FieldInfo = TypeOfScreensaverSettings.GetField(Variable)
        Dim FieldForecast As FieldInfo = TypeOfForecast.GetField(Variable)
        Dim FieldMailManager As FieldInfo = TypeOfMailManager.GetField(Variable)
        Dim FieldColors As FieldInfo = TypeOfColors.GetField(Variable)

        'Check if any of them contains the specified variable
        If Not IsNothing(FieldFlags) Then
            Return FieldFlags
        ElseIf Not IsNothing(FieldKernel) Then
            Return FieldKernel
        ElseIf Not IsNothing(FieldShell) Then
            Return FieldShell
        ElseIf Not IsNothing(FieldFTPShell) Then
            Return FieldFTPShell
        ElseIf Not IsNothing(FieldMailShell) Then
            Return FieldMailShell
        ElseIf Not IsNothing(FieldSFTPShell) Then
            Return FieldSFTPShell
        ElseIf Not IsNothing(FieldRDebugger) Then
            Return FieldRDebugger
        ElseIf Not IsNothing(FieldDebugWriters) Then
            Return FieldDebugWriters
        ElseIf Not IsNothing(FieldNetworkTools) Then
            Return FieldNetworkTools
        ElseIf Not IsNothing(FieldScreensaverSettings) Then
            Return FieldScreensaverSettings
        ElseIf Not IsNothing(FieldForecast) Then
            Return FieldForecast
        ElseIf Not IsNothing(FieldMailManager) Then
            Return FieldMailManager
        ElseIf Not IsNothing(FieldColors) Then
            Return FieldColors
        End If
    End Function

End Module
