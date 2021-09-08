
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

Public Module HelpSystem

    ''' <summary>
    ''' Shows the help of a command, or command list if nothing is specified
    ''' </summary>
    ''' <param name="CommandType">A specified command type</param>
    Public Sub ShowHelp(CommandType As ShellCommandType)
        ShowHelp("", CommandType)
    End Sub

    ''' <summary>
    ''' Shows the help of a command, or command list if nothing is specified
    ''' </summary>
    ''' <param name="command">A specified command</param>
    Public Sub ShowHelp(command As String)
        ShowHelp(command, ShellCommandType.Shell)
    End Sub

    ''' <summary>
    ''' Shows the help of a command, or command list if nothing is specified
    ''' </summary>
    ''' <param name="command">A specified command</param>
    ''' <param name="CommandType">A specified command type</param>
    Public Sub ShowHelp(command As String, CommandType As ShellCommandType)
        'Populate screensaver files
        Dim ScreensaverFiles As New List(Of String)
        ScreensaverFiles.AddRange(Directory.GetFiles(GetKernelPath(KernelPathType.Mods), "*.ss.vb", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))
        ScreensaverFiles.AddRange(Directory.GetFiles(GetKernelPath(KernelPathType.Mods), "*.ss.cs", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))

        'Determine command type
        Dim CommandList As Dictionary(Of String, CommandInfo) = Commands
        Dim ModCommandList As Dictionary(Of String, String) = ModDefs
        Dim AliasedCommandList As Dictionary(Of String, String) = Aliases
        Select Case CommandType
            Case ShellCommandType.FTPShell
                CommandList = FTPCommands
                ModCommandList = FTPModDefs
                AliasedCommandList = FTPShellAliases
            Case ShellCommandType.MailShell
                CommandList = MailCommands
                ModCommandList = MailModDefs
                AliasedCommandList = MailShellAliases
            Case ShellCommandType.RSSShell
                CommandList = RSSCommands
                ModCommandList = RSSModDefs
                AliasedCommandList = RSSShellAliases
            Case ShellCommandType.SFTPShell
                CommandList = SFTPCommands
                ModCommandList = SFTPModDefs
                AliasedCommandList = SFTPShellAliases
            Case ShellCommandType.TestShell
                CommandList = Test_Commands
                ModCommandList = TestModDefs
                AliasedCommandList = TestShellAliases
            Case ShellCommandType.TextShell
                CommandList = TextEdit_Commands
                ModCommandList = TextEdit_ModHelpEntries
                AliasedCommandList = TextShellAliases
            Case ShellCommandType.ZIPShell
                CommandList = ZipShell_Commands
                ModCommandList = ZipShell_ModHelpEntries
                AliasedCommandList = ZIPShellAliases
        End Select

        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(command) And CommandList.ContainsKey(command) Then
            Dim HelpDefinition As String = CommandList(command).GetTranslatedHelpEntry
            Dim HelpUsage As String = CommandList(command).HelpUsage
            Dim UsageLength As Integer = DoTranslation("Usage:").Length

            'Print usage information
            W(DoTranslation("Usage:") + $" {command} {HelpUsage}: {HelpDefinition}", True, ColTypes.Neutral)

            'Extra information for specific commands to be printed
            If CommandType = ShellCommandType.Shell Then
                Select Case command
                    Case "arginj"
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("where arguments will be {0}"), True, ColTypes.Neutral, String.Join(", ", AvailableArgs))
                    Case "chattr"
                        W(DoTranslation("where <attributes> is one of the following:") + vbNewLine, True, ColTypes.Neutral)
                        W("- Normal: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a normal file"), True, ColTypes.ListValue)                   'Normal   = 128
                        W("- ReadOnly: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a read-only file"), True, ColTypes.ListValue)              'ReadOnly = 1
                        W("- Hidden: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a hidden file"), True, ColTypes.ListValue)                   'Hidden   = 2
                        W("- Archive: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is an archive. Used for backups."), True, ColTypes.ListValue)  'Archive  = 32
                    Case "chlang"
                        W(" ".Repeat(UsageLength) + " " + " <language>: " + String.Join("/", Languages.Keys), True, ColTypes.Neutral)
                    Case "choice"
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("where <$variable> is any variable that will be used to store response") + vbNewLine +
                          " ".Repeat(UsageLength) + " " + DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), True, ColTypes.Neutral)
                    Case "hwinfo"
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("where HardwareType will be") + " HDD, LogicalParts, CPU, GPU, Sound, Network, System, Machine, BIOS, RAM, all.", True, ColTypes.Neutral)
                    Case "reloadconfig"
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("Colors doesn't require a restart, but most of the settings require you to restart."), True, ColTypes.Neutral)
                    Case "reloadsaver"
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", ScreensaverFiles))
                    Case "setsaver"
                        If CSvrdb.Count > 0 Then
                            W(" ".Repeat(UsageLength) + " " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", CSvrdb.Keys))
                        End If
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("where builtinsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", ScrnSvrdb.Keys))
                    Case "setthemes"
                        W(" ".Repeat(UsageLength) + "<Theme>: ThemeName.json, " + String.Join(", ", colorTemplates.Keys), True, ColTypes.Neutral)
                    Case "weather"
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("You can always consult http://bulk.openweathermap.org/sample/city.list.json.gz for the list of cities with their IDs.") + " " + DoTranslation("Or, pass ""listcities"" to this command."), True, ColTypes.Neutral)
                    Case "wrap"
                        'Get wrappable commands
                        Dim WrappableCmds As New ArrayList
                        For Each CommandInfo As CommandInfo In Commands.Values
                            If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
                        Next

                        'Print them along with help description
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("Wrappable commands:") + " {0}", True, ColTypes.Neutral, String.Join(", ", WrappableCmds.ToArray))
                End Select
            End If
        ElseIf String.IsNullOrWhiteSpace(command) Then
            'List the available commands
            If Not simHelp Then
                'The built-in commands
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In CommandList.Keys
                    If (Not CommandList(cmd).Strict) Or (CommandList(cmd).Strict And HasPermission(CurrentUser, PermissionType.Administrator)) Then
                        W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, CommandList(cmd).GetTranslatedHelpEntry)
                    End If
                Next

                'The mod commands
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If ModCommandList.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In ModCommandList.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, ModCommandList(cmd))
                Next

                'The alias commands
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If AliasedCommandList.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In AliasedCommandList.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, CommandList(AliasedCommandList(cmd)).GetTranslatedHelpEntry)
                Next

                'A tip for you all
                W(vbNewLine + DoTranslation("* You can use multiple commands using the colon between commands."), True, ColTypes.Neutral)
            Else
                'The built-in commands
                For Each cmd As String In CommandList.Keys
                    If (Not CommandList(cmd).Strict) Or (CommandList(cmd).Strict And HasPermission(CurrentUser, PermissionType.Administrator)) Then
                        W("{0}, ", False, ColTypes.ListEntry, cmd)
                    End If
                Next

                'The mod commands
                For Each cmd As String In ModCommandList.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next

                'The alias commands
                W(String.Join(", ", AliasedCommandList.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
        End If
    End Sub

End Module
