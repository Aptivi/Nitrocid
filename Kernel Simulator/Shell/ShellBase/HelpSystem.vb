
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

Imports System.IO
Imports KS.Misc.JsonShell
Imports KS.Misc.TextEdit
Imports KS.Misc.HexEdit
Imports KS.Misc.Writers.MiscWriters
Imports KS.Misc.ZipFile
Imports KS.Network.FTP
Imports KS.Network.HTTP
Imports KS.Network.Mail
Imports KS.Network.RemoteDebug
Imports KS.Network.RSS
Imports KS.Network.SFTP
Imports KS.TestShell

Namespace Shell.ShellBase
    Public Module HelpSystem

        'Mod definitions
        Public ModDefs As New Dictionary(Of String, String)
        Public TestModDefs As New Dictionary(Of String, String)
        Public SFTPModDefs As New Dictionary(Of String, String)
        Public RSSModDefs As New Dictionary(Of String, String)
        Public RDebugModDefs As New Dictionary(Of String, String)
        Public MailModDefs As New Dictionary(Of String, String)
        Public FTPModDefs As New Dictionary(Of String, String)
        Public ZipShell_ModHelpEntries As New Dictionary(Of String, String)
        Public TextEdit_ModHelpEntries As New Dictionary(Of String, String)
        Public JsonShell_ModDefs As New Dictionary(Of String, String)
        Public HTTPModDefs As New Dictionary(Of String, String)
        Public HexEdit_ModHelpEntries As New Dictionary(Of String, String)

        ''' <summary>
        ''' Shows the help of a command, or command list if nothing is specified
        ''' </summary>
        ''' <param name="CommandType">A specified command type</param>
        Public Sub ShowHelp(CommandType As ShellType)
            ShowHelp("", CommandType)
        End Sub

        ''' <summary>
        ''' Shows the help of a command, or command list if nothing is specified
        ''' </summary>
        ''' <param name="command">A specified command</param>
        Public Sub ShowHelp(command As String)
            ShowHelp(command, ShellType.Shell)
        End Sub

        ''' <summary>
        ''' Shows the help of a command, or command list if nothing is specified
        ''' </summary>
        ''' <param name="command">A specified command</param>
        ''' <param name="CommandType">A specified command type</param>
        ''' <param name="DebugDeviceSocket">Only for remote debug shell. Specifies the debug device socket.</param>
        Public Sub ShowHelp(command As String, CommandType As ShellType, Optional DebugDeviceSocket As StreamWriter = Nothing)
            'Determine command type
            Dim CommandList As Dictionary(Of String, CommandInfo) = Shell.Commands
            Dim ModCommandList As Dictionary(Of String, String) = ModDefs
            Dim AliasedCommandList As Dictionary(Of String, String) = Aliases
            Select Case CommandType
                Case ShellType.Shell
                    CommandList = Shell.Commands
                    ModCommandList = ModDefs
                    AliasedCommandList = Aliases
                Case ShellType.FTPShell
                    CommandList = FTPCommands
                    ModCommandList = FTPModDefs
                    AliasedCommandList = FTPShellAliases
                Case ShellType.MailShell
                    CommandList = MailCommands
                    ModCommandList = MailModDefs
                    AliasedCommandList = MailShellAliases
                Case ShellType.RSSShell
                    CommandList = RSSCommands
                    ModCommandList = RSSModDefs
                    AliasedCommandList = RSSShellAliases
                Case ShellType.SFTPShell
                    CommandList = SFTPCommands
                    ModCommandList = SFTPModDefs
                    AliasedCommandList = SFTPShellAliases
                Case ShellType.TestShell
                    CommandList = Test_Commands
                    ModCommandList = TestModDefs
                    AliasedCommandList = TestShellAliases
                Case ShellType.TextShell
                    CommandList = TextEdit_Commands
                    ModCommandList = TextEdit_ModHelpEntries
                    AliasedCommandList = TextShellAliases
                Case ShellType.ZIPShell
                    CommandList = ZipShell_Commands
                    ModCommandList = ZipShell_ModHelpEntries
                    AliasedCommandList = ZIPShellAliases
                Case ShellType.RemoteDebugShell
                    CommandList = DebugCommands
                    ModCommandList = RDebugModDefs
                    AliasedCommandList = RemoteDebugAliases
                Case ShellType.JsonShell
                    CommandList = JsonShell_Commands
                    ModCommandList = JsonShell_ModDefs
                    AliasedCommandList = JsonShellAliases
                Case ShellType.HTTPShell
                    CommandList = HTTPCommands
                    ModCommandList = HTTPModDefs
                    AliasedCommandList = HTTPShellAliases
                Case ShellType.HexShell
                    CommandList = HexEdit_Commands
                    ModCommandList = HexEdit_ModHelpEntries
                    AliasedCommandList = HexShellAliases
            End Select

            'Check to see if command exists
            If Not String.IsNullOrWhiteSpace(command) And (CommandList.ContainsKey(command) Or AliasedCommandList.ContainsKey(command)) Then
                'Found!
                Dim FinalCommand As String = If(AliasedCommandList.ContainsKey(command), AliasedCommandList(command), command)
                Dim HelpDefinition As String = CommandList(FinalCommand).GetTranslatedHelpEntry
                Dim UsageLength As Integer = DoTranslation("Usage:").Length
                Dim HelpUsages() As String = CommandList(FinalCommand).HelpUsages

                'Print usage information
                If HelpUsages.Length <> 0 Then
                    'Print the usage information holder
                    Dim Indent As Boolean
                    DecisiveWrite(CommandType, DebugDeviceSocket, DoTranslation("Usage:"), False, ColTypes.ListEntry)

                    'If remote debug, set the command to be prepended by the slash
                    If CommandType = ShellType.RemoteDebugShell Then FinalCommand = $"/{FinalCommand}"

                    'Enumerate through the available help usages
                    For Each HelpUsage As String In HelpUsages
                        'Indent, if necessary
                        If Indent Then DecisiveWrite(CommandType, DebugDeviceSocket, " ".Repeat(UsageLength), False, ColTypes.ListEntry)
                        DecisiveWrite(CommandType, DebugDeviceSocket, $" {FinalCommand} {HelpUsage}", True, ColTypes.ListEntry)
                        Indent = True
                    Next
                End If

                'Write the description now
                DecisiveWrite(CommandType, DebugDeviceSocket, DoTranslation("Description:") + $" {HelpDefinition}", True, ColTypes.ListValue)

                'Extra help action for some commands
                CommandList(FinalCommand).CommandBase.HelpHelper()
            ElseIf String.IsNullOrWhiteSpace(command) Then
                'List the available commands
                If Not SimHelp Then
                    'The built-in commands
                    DecisiveWrite(CommandType, DebugDeviceSocket, DoTranslation("General commands:") + If(ShowCommandsCount And ShowShellCommandsCount, " [{0}]", ""), True, ColTypes.ListTitle, CommandList.Count)

                    'Check the command list count and print not implemented. This is an extremely rare situation.
                    If CommandList.Count = 0 Then DecisiveWrite(CommandType, DebugDeviceSocket, "- " + DoTranslation("Shell commands not implemented!!!"), True, ColTypes.Warning)
                    For Each cmd As String In CommandList.Keys
                        If ((Not CommandList(cmd).Strict) Or (CommandList(cmd).Strict And HasPermission(CurrentUser?.Username, PermissionType.Administrator))) And
                            (Maintenance And Not CommandList(cmd).NoMaintenance Or Not Maintenance) Then
                            DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", False, ColTypes.ListEntry, cmd)
                            DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", True, ColTypes.ListValue, CommandList(cmd).GetTranslatedHelpEntry)
                        End If
                    Next

                    'The mod commands
                    DecisiveWrite(CommandType, DebugDeviceSocket, NewLine + DoTranslation("Mod commands:") + If(ShowCommandsCount And ShowModCommandsCount, " [{0}]", ""), True, ColTypes.ListTitle, ModCommandList.Count)
                    If ModCommandList.Count = 0 Then DecisiveWrite(CommandType, DebugDeviceSocket, "- " + DoTranslation("No mod commands."), True, ColTypes.Warning)
                    For Each cmd As String In ModCommandList.Keys
                        DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", False, ColTypes.ListEntry, cmd)
                        DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", True, ColTypes.ListValue, ModCommandList(cmd))
                    Next

                    'The alias commands
                    DecisiveWrite(CommandType, DebugDeviceSocket, NewLine + DoTranslation("Alias commands:") + If(ShowCommandsCount And ShowShellAliasesCount, " [{0}]", ""), True, ColTypes.ListTitle, AliasedCommandList.Count)
                    If AliasedCommandList.Count = 0 Then DecisiveWrite(CommandType, DebugDeviceSocket, "- " + DoTranslation("No alias commands."), True, ColTypes.Warning)
                    For Each cmd As String In AliasedCommandList.Keys
                        DecisiveWrite(CommandType, DebugDeviceSocket, "- {0}: ", False, ColTypes.ListEntry, cmd)
                        DecisiveWrite(CommandType, DebugDeviceSocket, "{0}", True, ColTypes.ListValue, CommandList(AliasedCommandList(cmd)).GetTranslatedHelpEntry)
                    Next

                    'A tip for you all
                    If CommandType = ShellType.Shell Then
                        DecisiveWrite(CommandType, DebugDeviceSocket, NewLine + DoTranslation("* You can use multiple commands using the colon between commands."), True, ColTypes.Tip)
                    End If
                Else
                    'The built-in commands
                    For Each cmd As String In CommandList.Keys
                        If ((Not CommandList(cmd).Strict) Or (CommandList(cmd).Strict And HasPermission(CurrentUser?.Username, PermissionType.Administrator))) And
                            (Maintenance And Not CommandList(cmd).NoMaintenance Or Not Maintenance) Then
                            DecisiveWrite(CommandType, DebugDeviceSocket, "{0}, ", False, ColTypes.ListEntry, cmd)
                        End If
                    Next

                    'The mod commands
                    For Each cmd As String In ModCommandList.Keys
                        DecisiveWrite(CommandType, DebugDeviceSocket, "{0}, ", False, ColTypes.ListEntry, cmd)
                    Next

                    'The alias commands
                    DecisiveWrite(CommandType, DebugDeviceSocket, String.Join(", ", AliasedCommandList.Keys), True, ColTypes.ListEntry)
                End If
            Else
                DecisiveWrite(CommandType, DebugDeviceSocket, DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
            End If
        End Sub

    End Module
End Namespace
