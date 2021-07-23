
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

    'This dictionary is the definitions for commands.
    Public definitions As Dictionary(Of String, String)
    Public moddefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the shell help dictionary to reflect the available commands
    ''' </summary>
    Public Sub InitHelp()
        definitions = New Dictionary(Of String, String) From {{"adduser", DoTranslation("Adds users")},
                                                              {"alias", DoTranslation("Adds aliases to commands")},
                                                              {"arginj", DoTranslation("Injects arguments to the kernel (reboot required)")},
                                                              {"beep", DoTranslation("Beep in 'n' Hz and time in 'n' milliseconds")},
                                                              {"blockdbgdev", DoTranslation("Block a debug device by IP address")},
                                                              {"calc", DoTranslation("Calculator to calculate expressions.")},
                                                              {"cat", DoTranslation("Prints content of file to console")},
                                                              {"cdbglog", DoTranslation("Deletes everything in debug log")},
                                                              {"chattr", DoTranslation("Changes attribute of a file")},
                                                              {"chdir", DoTranslation("Changes directory")},
                                                              {"chhostname", DoTranslation("Changes host name")},
                                                              {"chlang", DoTranslation("Changes language")},
                                                              {"chmal", DoTranslation("Changes MAL, the MOTD After Login")},
                                                              {"chmotd", DoTranslation("Changes MOTD, the Message Of The Day")},
                                                              {"choice", DoTranslation("Makes user choices")},
                                                              {"chpwd", DoTranslation("Changes password for current user")},
                                                              {"chusrname", DoTranslation("Changes user name")},
                                                              {"cls", DoTranslation("Clears the screen")},
                                                              {"copy", DoTranslation("Creates another copy of a file under different directory or name.")},
                                                              {"dirinfo", DoTranslation("Provides information about a directory")},
                                                              {"disconndbgdev", DoTranslation("Disconnect a debug device")},
                                                              {"dismissnotif", DoTranslation("Dismisses a notification")},
                                                              {"echo", DoTranslation("Writes text into the console")},
                                                              {"edit", DoTranslation("Edits a text file")},
                                                              {"fileinfo", DoTranslation("Provides information about a file")},
                                                              {"firedevents", DoTranslation("Lists all fired events")},
                                                              {"ftp", DoTranslation("Use an FTP shell to interact with servers")},
                                                              {"gettimeinfo", DoTranslation("Gets the date and time information")},
                                                              {"get", DoTranslation("Downloads a file to current working directory")},
                                                              {"help", DoTranslation("Help page")},
                                                              {"hwinfo", DoTranslation("Prints hardware information")},
                                                              {"input", DoTranslation("Allows user to enter input")},
                                                              {"list", DoTranslation("List file/folder contents in current folder")},
                                                              {"lockscreen", DoTranslation("Locks your screen with a password")},
                                                              {"logout", DoTranslation("Logs you out")},
                                                              {"loteresp", DoTranslation("Respond to love or hate comments.")},
                                                              {"lsdbgdev", DoTranslation("Lists debugging devices connected")},
                                                              {"lsmail", DoTranslation("Lists all mails in the specific user.")},
                                                              {"mathbee", DoTranslation("See if you can solve mathematical equations on time")},
                                                              {"md", DoTranslation("Creates a directory")},
                                                              {"mkfile", DoTranslation("Makes a new file")},
                                                              {"mktheme", DoTranslation("Makes a new theme")},
                                                              {"modinfo", DoTranslation("Gets mod information")},
                                                              {"move", DoTranslation("Moves a file to another directory")},
                                                              {"netinfo", DoTranslation("Lists information about all available interfaces")},
                                                              {"perm", DoTranslation("Manage permissions for users")},
                                                              {"ping", DoTranslation("Pings an address")},
                                                              {"put", DoTranslation("Uploads a file to specified website")},
                                                              {"reboot", DoTranslation("Restarts your computer (WARNING: No syncing, because it is not a final kernel)")},
                                                              {"reloadconfig", DoTranslation("Reloads configuration file that is edited.")},
                                                              {"reloadmods", DoTranslation("Reloads mods.")},
                                                              {"reloadsaver", DoTranslation("Reloads screensaver file in KSMods")},
                                                              {"rexec", DoTranslation("Remotely executes a command to remote PC")},
                                                              {"rm", DoTranslation("Removes a directory or a file")},
                                                              {"rdebug", DoTranslation("Enables or disables remote debugging.")},
                                                              {"reportbug", DoTranslation("A bug reporting prompt.")},
                                                              {"rmuser", DoTranslation("Removes a user from the list")},
                                                              {"rss", DoTranslation("Opens an RSS shell to read the feeds")},
                                                              {"savecurrdir", DoTranslation("Saves the current directory to kernel configuration file")},
                                                              {"savescreen", DoTranslation("Saves your screen from burn outs")},
                                                              {"search", DoTranslation("Searches for specified string in the provided file using regular expressions")},
                                                              {"searchword", DoTranslation("Searches for specified string in the provided file")},
                                                              {"setsaver", DoTranslation("Sets up kernel screensavers")},
                                                              {"setthemes", DoTranslation("Sets up kernel themes")},
                                                              {"settings", DoTranslation("Changes kernel configuration")},
                                                              {"set", DoTranslation("Sets a variable to a value in a script")},
                                                              {"sftp", DoTranslation("Lets you use an SSH FTP server")},
                                                              {"shownotifs", DoTranslation("Shows all received notifications")},
                                                              {"showtd", DoTranslation("Shows date and time")},
                                                              {"showtdzone", DoTranslation("Shows date and time in zones")},
                                                              {"shutdown", DoTranslation("The kernel will be shut down")},
                                                              {"speedpress", DoTranslation("See if you can press a key on time")},
                                                              {"spellbee", DoTranslation("See if you can spell words correctly on time")},
                                                              {"sshell", DoTranslation("Connects to an SSH server.")},
                                                              {"sshcmd", DoTranslation("Connects to an SSH server to execute a command.")},
                                                              {"sumfile", DoTranslation("Calculates file sums.")},
                                                              {"sumfiles", DoTranslation("Calculates sums of files in specified directory.")},
                                                              {"sysinfo", DoTranslation("System information")},
                                                              {"unblockdbgdev", DoTranslation("Unblock a debug device by IP address")},
                                                              {"unzip", DoTranslation("Extracts a ZIP archive")},
                                                              {"update", DoTranslation("System update")},
                                                              {"usermanual", DoTranslation("Takes you to our GitHub Wiki.")},
                                                              {"verify", DoTranslation("Verifies sanity of the file")},
                                                              {"weather", DoTranslation("Shows weather info for specified city. Uses OpenWeatherMap.")},
                                                              {"wrap", DoTranslation("Wraps the console output")},
                                                              {"zip", DoTranslation("Creates a ZIP archive")},
                                                              {"zipshell", DoTranslation("Opens a ZIP archive")}}
    End Sub

    ''' <summary>
    ''' Shows the help of a command, or command list if nothing is specified
    ''' </summary>
    ''' <param name="command">A specified command</param>
    Public Sub ShowHelp(Optional ByVal command As String = "")

        Dim ScreensaverFiles As New List(Of String)
        ScreensaverFiles.AddRange(Directory.GetFiles(paths("Mods"), "*.ss.vb", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))
        ScreensaverFiles.AddRange(Directory.GetFiles(paths("Mods"), "*.ss.cs", SearchOption.TopDirectoryOnly).Select(Function(x) Path.GetFileName(x)))
        If command = "" Then

            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In definitions.Keys
                    If (Not Commands(cmd).Strict) Or (Commands(cmd).Strict And adminList(CurrentUser)) Then
                        W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, definitions(cmd))
                    End If
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If moddefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In moddefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, moddefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If Aliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In Aliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, definitions(Aliases(cmd)))
                Next
                W(vbNewLine + DoTranslation("* You can use multiple commands using the colon between commands."), True, ColTypes.Neutral)
            Else
                For Each cmd As String In Commands.Keys
                    If (Not Commands(cmd).Strict) Or (Commands(cmd).Strict And adminList(CurrentUser)) Then
                        W("{0}, ", False, ColTypes.ListEntry, cmd)
                    End If
                Next
                For Each cmd As String In moddefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", Aliases.Keys), True, ColTypes.ListEntry)
            End If

        ElseIf command = "adduser" Then

            W(DoTranslation("Usage:") + " adduser <userName> [password] [confirm]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "alias" Then

            W(DoTranslation("Usage:") + " alias <rem/add> <{0}> <alias> <cmd>: " + definitions(command), True, ColTypes.Neutral, String.Join("/", [Enum].GetNames(GetType(AliasType))))

        ElseIf command = "arginj" Then

            W(DoTranslation("Usage:") + " arginj [Arguments separated by spaces]: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("where arguments will be {0}"), True, ColTypes.Neutral, String.Join(", ", AvailableArgs))

        ElseIf command = "beep" Then

            W(DoTranslation("Usage:") + " beep <37-32767 Hz> <milliseconds>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "blockdbgdev" Then

            W(DoTranslation("Usage:") + " blockdbgdev <ipaddress>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "cat" Then

            W(DoTranslation("Usage:") + " cat <file>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "calc" Then

            W(DoTranslation("Usage:") + " calc <expression>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "cdbglog" Then

            W(DoTranslation("Usage:") + " cdbglog: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "chattr" Then

            W(DoTranslation("Usage:") + " chattr <file> +/-<attributes>: " + definitions(command), True, ColTypes.Neutral)
            W(DoTranslation("where <attributes> is one of the following:") + vbNewLine, True, ColTypes.Neutral)
            W("- Normal: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a normal file"), True, ColTypes.ListValue)                   'Normal   = 128
            W("- ReadOnly: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a read-only file"), True, ColTypes.ListValue)              'ReadOnly = 1
            W("- Hidden: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is a hidden file"), True, ColTypes.ListValue)                   'Hidden   = 2
            W("- Archive: ", False, ColTypes.ListEntry) : W(DoTranslation("The file is an archive. Used for backups."), True, ColTypes.ListValue)  'Archive  = 32

        ElseIf command = "chdir" Then

            W(DoTranslation("Usage:") + " chdir <directory/..>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "chhostname" Then

            W(DoTranslation("Usage:") + " chhostname <HostName>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "chlang" Then

            W(DoTranslation("Usage:") + " chlang <language>: " + definitions(command) + vbNewLine +
              "<language>: " + String.Join("/", Languages.Keys), True, ColTypes.Neutral)

        ElseIf command = "chmotd" Then

            W(DoTranslation("Usage:") + " chmotd [Message]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "chmal" Then

            W(DoTranslation("Usage:") + " chmal [Message]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "choice" Then

            W(DoTranslation("Usage:") + " choice [-o|-t|-m] <$variable> <answers> <input>: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("where <$variable> is any variable that will be used to store response") + vbNewLine +
              "       " + DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), True, ColTypes.Neutral)

        ElseIf command = "chpwd" Then

            W(DoTranslation("Usage:") + " chpwd <Username> <UserPass> <newPass> <confirm>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "chusrname" Then

            W(DoTranslation("Usage:") + " chusrname <oldUserName> <newUserName>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "cls" Then

            W(DoTranslation("Usage:") + " cls: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "copy" Then

            W(DoTranslation("Usage:") + " copy <source> <target>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "dirinfo" Then

            W(DoTranslation("Usage:") + " dirinfo <directory>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "dismissnotif" Then

            W(DoTranslation("Usage:") + " dismissnotif <notificationNumber>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "disconndbgdev" Then

            W(DoTranslation("Usage:") + " disconndbgdev <ip>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "echo" Then

            W(DoTranslation("Usage:") + " echo <text>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "edit" Then

            W(DoTranslation("Usage:") + " edit <file>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "fileinfo" Then

            W(DoTranslation("Usage:") + " fileinfo <file>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "firedevents" Then

            W(DoTranslation("Usage:") + " firedevents: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "ftp" Then

            W(DoTranslation("Usage:") + " ftp <server>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "gettimeinfo" Then

            W(DoTranslation("Usage:") + " gettimeinfo <date>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "get" Then

            W(DoTranslation("Usage:") + " get <URL> [username]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "hwinfo" Then

            W(DoTranslation("Usage:") + " hwinfo <HardwareType>: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("where HardwareType will be") + " HDD, LogicalParts, CPU, GPU, Sound, Network, System, Machine, BIOS, RAM, all.", True, ColTypes.Neutral)

        ElseIf command = "input" Then

            W(DoTranslation("Usage:") + " input <$variable> <question>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "list" Then

            W(DoTranslation("Usage:") + " list [oneDirectory]: " + definitions(command) + vbNewLine +
              "       list: " + DoTranslation("to get current directory."), True, ColTypes.Neutral)

        ElseIf command = "loteresp" Then

            W(DoTranslation("Usage:") + " loteresp: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "lsdbgdev" Then

            W(DoTranslation("Usage:") + " lsdbgdev: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "lsmail" Then

            W(DoTranslation("Usage:") + " lsmail [emailAddress]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "lockscreen" Then

            W(DoTranslation("Usage:") + " lockscreen: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("Friends of") + " lockscreen: savescreen", True, ColTypes.Neutral)

        ElseIf command = "logout" Then

            W(DoTranslation("Usage:") + " logout: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("Friends of") + " logout: reboot, shutdown", True, ColTypes.Neutral)

        ElseIf command = "mathbee" Then

            W(DoTranslation("Usage:") + " mathbee: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "md" Then

            W(DoTranslation("Usage:") + " md <anything>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "mkfile" Then

            W(DoTranslation("Usage:") + " mkfile <anything>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "mktheme" Then

            W(DoTranslation("Usage:") + " mktheme <themeName>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "modinfo" Then

            W(DoTranslation("Usage:") + " modinfo <mod>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "move" Then

            W(DoTranslation("Usage:") + " move <source> <target>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "netinfo" Then

            W(DoTranslation("Usage:") + " netinfo: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "perm" Then

            W(DoTranslation("Usage:") + " perm <userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "ping" Then

            W(DoTranslation("Usage:") + " ping [times] <Address1> <Address2> ...: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "put" Then

            W(DoTranslation("Usage:") + " put <FileName> <URL> [username]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "rm" Then

            W(DoTranslation("Usage:") + " rm <directory/file>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "rdebug" Then

            W(DoTranslation("Usage:") + " rdebug: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "reboot" Then

            W(DoTranslation("Usage:") + " reboot [ip] [port]: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("Friends of") + " reboot: shutdown, logout", True, ColTypes.Neutral)

        ElseIf command = "reloadmods" Then

            W(DoTranslation("Usage:") + " reloadmods: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "reloadconfig" Then

            W(DoTranslation("Usage:") + " reloadconfig: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("Colors doesn't require a restart, but most of the settings require you to restart."), True, ColTypes.Neutral)

        ElseIf command = "reloadsaver" Then

            W(DoTranslation("Usage:") + " reloadsaver <customsaver>: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", ScreensaverFiles))

        ElseIf command = "reportbug" Then

            W(DoTranslation("Usage:") + " reportbug: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "rexec" Then

            W(DoTranslation("Usage:") + " rexec <address> [port] <command>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "rmuser" Then

            W(DoTranslation("Usage:") + " rmuser <Username>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "rss" Then

            W(DoTranslation("Usage:") + " rss [feedlink]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "savecurrdir" Then

            W(DoTranslation("Usage:") + " savecurrdir: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "savescreen" Then

            W(DoTranslation("Usage:") + " savescreen [saver]: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("Friends of") + " savescreen: lockscreen", True, ColTypes.Neutral)

        ElseIf command = "search" Then

            W(DoTranslation("Usage:") + " search <Regexp> <File>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "searchword" Then

            W(DoTranslation("Usage:") + " search <StringEnclosedInDoubleQuotes> <File>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "setsaver" Then

            W(DoTranslation("Usage:") + " setsaver <customsaver/{0}>: " + definitions(command), True, ColTypes.Neutral, String.Join("/", ScrnSvrdb.Keys))
            If CSvrdb.Count > 0 Then
                W("       " + DoTranslation("where customsaver will be") + " {0}", True, ColTypes.Neutral, String.Join(", ", CSvrdb.Keys))
            End If

        ElseIf command = "setthemes" Then

            W(DoTranslation("Usage:") + " setthemes <Theme>: " + definitions(command) + vbNewLine +
              "       " + "<Theme>: ThemeName.json, " + String.Join(", ", colorTemplates.Keys), True, ColTypes.Neutral)

        ElseIf command = "settings" Then

            W(DoTranslation("Usage:") + " settings: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "set" Then

            W(DoTranslation("Usage:") + " set <$variable> <value>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "sftp" Then

            W(DoTranslation("Usage:") + " sftp <server>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "shownotifs" Then

            W(DoTranslation("Usage:") + " shownotifs: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "showtd" Then

            W(DoTranslation("Usage:") + " showtd: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "showtdzone" Then

            W(DoTranslation("Usage:") + " showtdzone <timezone/all>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "shutdown" Then

            W(DoTranslation("Usage:") + " shutdown [ip] [port]: " + definitions(command) + vbNewLine +
              "       " + DoTranslation("Friends of") + " shutdown: reboot, logout", True, ColTypes.Neutral)

        ElseIf command = "speedpress" Then

            W(DoTranslation("Usage:") + " speedpress <e/m/h>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "spellbee" Then

            W(DoTranslation("Usage:") + " spellbee: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "sshell" Then

            W(DoTranslation("Usage:") + " sshell <address:port> <username>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "sshcmd" Then

            W(DoTranslation("Usage:") + " sshcmd <address:port> <username> ""<command>"": " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "sumfile" Then

            W(DoTranslation("Usage:") + " sumfile <MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "sumfiles" Then

            W(DoTranslation("Usage:") + " sumfiles <MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "sysinfo" Then

            W(DoTranslation("Usage:") + " sysinfo: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "unblockdbgdev" Then

            W(DoTranslation("Usage:") + " unblockdbgdev <ipaddress>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "unzip" Then

            W(DoTranslation("Usage:") + " unzip <zipfile> [path] [-createdir]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "update" Then

            W(DoTranslation("Usage:") + " update: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "usermanual" Then

            W(DoTranslation("Usage:") + " usermanual: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "verify" Then

            W(DoTranslation("Usage:") + " verify <MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "weather" Then

            W(DoTranslation("Usage:") + " weather <CityID/CityName/listcities>: " + definitions(command) + vbNewLine +
              DoTranslation("You can always consult http://bulk.openweathermap.org/sample/city.list.json.gz for the list of cities with their IDs.") + " " + DoTranslation("Or, pass ""listcities"" to this command."), True, ColTypes.Neutral)

        ElseIf command = "wrap" Then

            Dim WrappableCmds As New ArrayList
            For Each CommandInfo As CommandInfo In Commands.Values
                If CommandInfo.Wrappable Then WrappableCmds.Add(CommandInfo.Command)
            Next
            W(DoTranslation("Usage:") + " wrap <command>: " + definitions(command), True, ColTypes.Neutral)
            W("       " + DoTranslation("Wrappable commands:") + " {0}", True, ColTypes.Neutral, String.Join(", ", WrappableCmds.ToArray))

        ElseIf command = "zip" Then

            W(DoTranslation("Usage:") + " zip <zipfile> <path> [-fast/-nocomp] [-nobasedir]: " + definitions(command), True, ColTypes.Neutral)

        ElseIf command = "zipshell" Then

            W(DoTranslation("Usage:") + " zipshell <zipfile>: " + definitions(command), True, ColTypes.Neutral)

        Else

            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)

        End If

    End Sub

End Module
