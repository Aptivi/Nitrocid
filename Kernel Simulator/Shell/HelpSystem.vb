
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

    Public moddefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Shows the help of a command, or command list if nothing is specified
    ''' </summary>
    ''' <param name="command">A specified command</param>
    Public Sub ShowHelp(Optional ByVal command As String = "")
        'Populate screensaver files
        Dim ScreensaverFiles As New List(Of String)
        ScreensaverFiles.AddRange(Directory.GetFiles(paths("Mods"), "*.ss.vb", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))
        ScreensaverFiles.AddRange(Directory.GetFiles(paths("Mods"), "*.ss.cs", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))

        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(command) And Commands.ContainsKey(command) Then
            Dim HelpDefinition As String = Commands(command).GetTranslatedHelpEntry
            Dim UsageLength As Integer = DoTranslation("Usage:").Length
            Select Case command
                Case "adduser"
                    W(DoTranslation("Usage:") + " adduser <userName> [password] [confirm]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "alias"
                    W(DoTranslation("Usage:") + " alias <rem/add> <{0}> <alias> <cmd>: " + HelpDefinition, True, ColTypes.Neutral, String.Join("/", [Enum].GetNames(GetType(AliasType))))
                Case "arginj"
                    W(DoTranslation("Usage:") + " arginj [Arguments separated by spaces]: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("where arguments will be {0}"), True, ColTypes.Neutral, String.Join(", ", AvailableArgs))
                Case "beep"
                    W(DoTranslation("Usage:") + " beep <37-32767 Hz> <milliseconds>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "blockdbgdev"
                    W(DoTranslation("Usage:") + " blockdbgdev <ipaddress>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "cat"
                    W(DoTranslation("Usage:") + " cat <file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "calc"
                    W(DoTranslation("Usage:") + " calc <expression>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "cdbglog"
                    W(DoTranslation("Usage:") + " cdbglog: " + HelpDefinition, True, ColTypes.Neutral)
                Case "chattr"
                    W(DoTranslation("Usage:") + " chattr <file> +/-<attributes>: " + HelpDefinition, True, ColTypes.Neutral)
                    W(DoTranslation("where <attributes> is one of the following:") + vbNewLine, True, ColTypes.Neutral)
                    W("- Normal: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a normal file"), True, ColTypes.ListValue)                   'Normal   = 128
                    W("- ReadOnly: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a read-only file"), True, ColTypes.ListValue)              'ReadOnly = 1
                    W("- Hidden: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a hidden file"), True, ColTypes.ListValue)                   'Hidden   = 2
                    W("- Archive: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is an archive. Used for backups."), True, ColTypes.ListValue)  'Archive  = 32
                Case "chdir"
                    W(DoTranslation("Usage:") + " chdir <directory/..>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "chhostname"
                    W(DoTranslation("Usage:") + " chhostname <HostName>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "chlang"
                    W(DoTranslation("Usage:") + " chlang <language>: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + " <language>: " + String.Join("/", Languages.Keys), True, ColTypes.Neutral)
                Case "chmotd"
                    W(DoTranslation("Usage:") + " chmotd [Message]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "chmal"
                    W(DoTranslation("Usage:") + " chmal [Message]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "choice"
                    W(DoTranslation("Usage:") + " choice [-o|-t|-m] <$variable> <answers> <input>: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("where <$variable> is any variable that will be used to store response") + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), True, ColTypes.Neutral)
                Case "chpwd"
                    W(DoTranslation("Usage:") + " chpwd <Username> <UserPass> <newPass> <confirm>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "chusrname"
                    W(DoTranslation("Usage:") + " chusrname <oldUserName> <newUserName>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "cls"
                    W(DoTranslation("Usage:") + " cls: " + HelpDefinition, True, ColTypes.Neutral)
                Case "copy"
                    W(DoTranslation("Usage:") + " copy <source> <target>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "dirinfo"
                    W(DoTranslation("Usage:") + " dirinfo <directory>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "dismissnotif"
                    W(DoTranslation("Usage:") + " dismissnotif <notificationNumber>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "disconndbgdev"
                    W(DoTranslation("Usage:") + " disconndbgdev <ip>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "echo"
                    W(DoTranslation("Usage:") + " echo <text>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "edit"
                    W(DoTranslation("Usage:") + " edit <file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "fileinfo"
                    W(DoTranslation("Usage:") + " fileinfo <file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "firedevents"
                    W(DoTranslation("Usage:") + " firedevents: " + HelpDefinition, True, ColTypes.Neutral)
                Case "ftp"
                    W(DoTranslation("Usage:") + " ftp <server>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "gettimeinfo"
                    W(DoTranslation("Usage:") + " gettimeinfo <date>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "get"
                    W(DoTranslation("Usage:") + " get <URL> [username]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "hwinfo"
                    W(DoTranslation("Usage:") + " hwinfo <HardwareType>: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("where HardwareType will be") + " HDD, LogicalParts, CPU, GPU, Sound, Network, System, Machine, BIOS, RAM, all.", True, ColTypes.Neutral)
                Case "input"
                    W(DoTranslation("Usage:") + " input <$variable> <question>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "jsonbeautify"
                    W(DoTranslation("Usage:") + " jsonbeautify <jsonfile> [output]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "jsonminify"
                    W(DoTranslation("Usage:") + " jsonminify <jsonfile> [output]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "list"
                    W(DoTranslation("Usage:") + " list [oneDirectory]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "loteresp"
                    W(DoTranslation("Usage:") + " loteresp: " + HelpDefinition, True, ColTypes.Neutral)
                Case "lsdbgdev"
                    W(DoTranslation("Usage:") + " lsdbgdev: " + HelpDefinition, True, ColTypes.Neutral)
                Case "lsmail"
                    W(DoTranslation("Usage:") + " lsmail [emailAddress]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "lockscreen"
                    W(DoTranslation("Usage:") + " lockscreen: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("Friends of") + " lockscreen: savescreen", True, ColTypes.Neutral)
                Case "logout"
                    W(DoTranslation("Usage:") + " logout: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("Friends of") + " logout: reboot, shutdown", True, ColTypes.Neutral)
                Case "mathbee"
                    W(DoTranslation("Usage:") + " mathbee: " + HelpDefinition, True, ColTypes.Neutral)
                Case "md"
                    W(DoTranslation("Usage:") + " md <anything>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "mkfile"
                    W(DoTranslation("Usage:") + " mkfile <anything>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "mktheme"
                    W(DoTranslation("Usage:") + " mktheme <themeName>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "modinfo"
                    W(DoTranslation("Usage:") + " modinfo <mod>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "modman"
                    W(DoTranslation("Usage:") + " modman <start/stop/info/reload> <modfilename> " + vbNewLine +
                      " ".Repeat(UsageLength) + " modman <list/reloadall/stopall/startall>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "move"
                    W(DoTranslation("Usage:") + " move <source> <target>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "netinfo"
                    W(DoTranslation("Usage:") + " netinfo: " + HelpDefinition, True, ColTypes.Neutral)
                Case "perm"
                    W(DoTranslation("Usage:") + " perm <userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "ping"
                    W(DoTranslation("Usage:") + " ping [times] <Address1> <Address2> ...: " + HelpDefinition, True, ColTypes.Neutral)
                Case "put"
                    W(DoTranslation("Usage:") + " put <FileName> <URL> [username]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rm"
                    W(DoTranslation("Usage:") + " rm <directory/file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rdebug"
                    W(DoTranslation("Usage:") + " rdebug: " + HelpDefinition, True, ColTypes.Neutral)
                Case "reboot"
                    W(DoTranslation("Usage:") + " reboot [ip] [port]: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("Friends of") + " reboot: shutdown, logout", True, ColTypes.Neutral)
                Case "reloadmods"
                    W(DoTranslation("Usage:") + " reloadmods: " + HelpDefinition, True, ColTypes.Neutral)
                Case "reloadconfig"
                    W(DoTranslation("Usage:") + " reloadconfig: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("Colors doesn't require a restart, but most of the settings require you to restart."), True, ColTypes.Neutral)
                Case "reloadsaver"
                    W(DoTranslation("Usage:") + " reloadsaver <customsaver>: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", ScreensaverFiles))
                Case "reportbug"
                    W(DoTranslation("Usage:") + " reportbug: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rexec"
                    W(DoTranslation("Usage:") + " rexec <address> [port] <command>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rmuser"
                    W(DoTranslation("Usage:") + " rmuser <Username>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rss"
                    W(DoTranslation("Usage:") + " rss [feedlink]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "savecurrdir"
                    W(DoTranslation("Usage:") + " savecurrdir: " + HelpDefinition, True, ColTypes.Neutral)
                Case "savescreen"
                    W(DoTranslation("Usage:") + " savescreen [saver]: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("Friends of") + " savescreen: lockscreen", True, ColTypes.Neutral)
                Case "search"
                    W(DoTranslation("Usage:") + " search <Regexp> <File>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "searchword"
                    W(DoTranslation("Usage:") + " search <StringEnclosedInDoubleQuotes> <File>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "setsaver"
                    W(DoTranslation("Usage:") + " setsaver <customsaver/{0}>: " + HelpDefinition, True, ColTypes.Neutral, String.Join("/", ScrnSvrdb.Keys))
                    If CSvrdb.Count > 0 Then
                        W(" ".Repeat(UsageLength) + " " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", CSvrdb.Keys))
                    End If
                Case "setthemes"
                    W(DoTranslation("Usage:") + " setthemes <Theme>: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + "<Theme>: ThemeName.json, " + String.Join(", ", colorTemplates.Keys), True, ColTypes.Neutral)
                Case "settings"
                    W(DoTranslation("Usage:") + " settings: " + HelpDefinition, True, ColTypes.Neutral)
                Case "set"
                    W(DoTranslation("Usage:") + " set <$variable> <value>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "sftp"
                    W(DoTranslation("Usage:") + " sftp <server>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "shownotifs"
                    W(DoTranslation("Usage:") + " shownotifs: " + HelpDefinition, True, ColTypes.Neutral)
                Case "showtd"
                    W(DoTranslation("Usage:") + " showtd: " + HelpDefinition, True, ColTypes.Neutral)
                Case "showtdzone"
                    W(DoTranslation("Usage:") + " showtdzone <timezone/all>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "shutdown"
                    W(DoTranslation("Usage:") + " shutdown [ip] [port]: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("Friends of") + " shutdown: reboot, logout", True, ColTypes.Neutral)
                Case "speedpress"
                    W(DoTranslation("Usage:") + " speedpress <e/m/h>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "spellbee"
                    W(DoTranslation("Usage:") + " spellbee: " + HelpDefinition, True, ColTypes.Neutral)
                Case "sshell"
                    W(DoTranslation("Usage:") + " sshell <address:port> <username>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "sshcmd"
                    W(DoTranslation("Usage:") + " sshcmd <address:port> <username> ""<command>"": " + HelpDefinition, True, ColTypes.Neutral)
                Case "sumfile"
                    W(DoTranslation("Usage:") + " sumfile <MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "sumfiles"
                    W(DoTranslation("Usage:") + " sumfiles <MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "sysinfo"
                    W(DoTranslation("Usage:") + " sysinfo: " + HelpDefinition, True, ColTypes.Neutral)
                Case "unblockdbgdev"
                    W(DoTranslation("Usage:") + " unblockdbgdev <ipaddress>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "unzip"
                    W(DoTranslation("Usage:") + " unzip <zipfile> [path] [-createdir]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "update"
                    W(DoTranslation("Usage:") + " update: " + HelpDefinition, True, ColTypes.Neutral)
                Case "usermanual"
                    W(DoTranslation("Usage:") + " usermanual: " + HelpDefinition, True, ColTypes.Neutral)
                Case "verify"
                    W(DoTranslation("Usage:") + " verify <MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "weather"
                    W(DoTranslation("Usage:") + " weather <CityID/CityName/listcities>: " + HelpDefinition + vbNewLine +
                      " ".Repeat(UsageLength) + " " + DoTranslation("You can always consult http://bulk.openweathermap.org/sample/city.list.json.gz for the list of cities with their IDs.") + " " + DoTranslation("Or, pass ""listcities"" to this command."), True, ColTypes.Neutral)
                Case "wrap"
                    'Get wrappable commands
                    Dim WrappableCmds As New ArrayList
                    For Each CommandInfo As CommandInfo In Commands.Values
                        If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
                    Next

                    'Print them along with help description
                    W(DoTranslation("Usage:") + " wrap <command>: " + HelpDefinition, True, ColTypes.Neutral)
                    W(" ".Repeat(UsageLength) + " " + DoTranslation("Wrappable commands:") + " {0}", True, ColTypes.Neutral, String.Join(", ", WrappableCmds.ToArray))
                Case "zip"
                    W(DoTranslation("Usage:") + " zip <zipfile> <path> [-fast/-nocomp] [-nobasedir]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "zipshell"
                    W(DoTranslation("Usage:") + " zipshell <zipfile>: " + HelpDefinition, True, ColTypes.Neutral)
            End Select
        ElseIf String.IsNullOrWhiteSpace(command) Then
            'List the available commands
            If simHelp = False Then
                'The built-in commands
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In Commands.Keys
                    If (Not Commands(cmd).Strict) Or (Commands(cmd).Strict And adminList(CurrentUser)) Then
                        W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, Commands(cmd).GetTranslatedHelpEntry)
                    End If
                Next

                'The mod commands
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If moddefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In moddefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, moddefs(cmd))
                Next

                'The alias commands
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If Aliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In Aliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, Commands(Aliases(cmd)).GetTranslatedHelpEntry)
                Next

                'A tip for you all
                W(vbNewLine + DoTranslation("* You can use multiple commands using the colon between commands."), True, ColTypes.Neutral)
            Else
                'The built-in commands
                For Each cmd As String In Commands.Keys
                    If (Not Commands(cmd).Strict) Or (Commands(cmd).Strict And adminList(CurrentUser)) Then
                        W("{0}, ", False, ColTypes.ListEntry, cmd)
                    End If
                Next

                'The mod commands
                For Each cmd As String In moddefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next

                'The alias commands
                W(String.Join(", ", Aliases.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
        End If
    End Sub

End Module
