## Commands for KS

The commands in the entirety of the kernel are programs that allow you to do what they're supposed to do. They can be either implemented internally (which is what we call the normal commands), implemented as an alias (an alias to normal commands or mods), or created by mods.

All the shells (either normal or custom) share a common command input parser, such as arguments, switches, and so on. Some commands have arguments (either required, for which you should provide to make the command work, or optional, for which you can omit), and some don't provide any.

The switches are special arguments that have the dash in front of them. They change how the commands work, but must be implemented before they actually work.

## General commands

These commands that are mentioned below are available in all of the shells.

 | Command                                  | Description
 |:-----------------------------------------|:------------
 | [exit](unified/KS-Command-exit.md)       | Exits the subshell
 | [presets](unified/KS-Command-presets.md) | Changes the shell preset

## UESH shell commands

The below commands for Nitrocid KS can be used in the normal UESH shell, the one that is started after you log in to your account.

For more information about every command, click the command.

### Administrative commands

> [!IMPORTANT]
> The user must have at least the administrative privileges before they can run the below commands.

| Command                                           | Description
|:--------------------------------------------------|:------------
| [adduser](uesh/KS-Command-adduser.md)             | You can add the user's name whenever you need, with the password if required. However, passwords are required to ensure security.
| [alias](uesh/KS-Command-alias.md)                 | You can manage your aliases to commands so you don't have to type long commands.
| [arginj](uesh/KS-Command-arginj.md)               | You can inject arguments into the kernel so that when you reboot, the arguments that are injected will be run.
| [blockdbgdev](uesh/KS-Command-blockdbgdev.md)     | You can block an IP address of a debug device to prevent it from entering remote debug until it's unblocked.
| [cdbglog](uesh/KS-Command-cdbglog.md)             | You can clear debug log, resetting the size to 0
| [chhostname](uesh/KS-Command-chhostname.md)       | You can change your hostname of your kernel to personalize things. It has an argument of `chhostname <AnyHostName>.`
| [chlang](uesh/KS-Command-chlang.md)               | Changes your language
| [chmal](uesh/KS-Command-chmal.md)                 | You can change your message of the day after login, and it supports the same placeholders.
| [chmotd](uesh/KS-Command-chmotd.md)               | You can change your message of the day, and you can include your own placeholders, including `<user>` which stands for username, `<shortdate>` for the short date in "MM/DD/YYYY" format, `<longdate>` for the long date that looks like "Saturday, December 1, 2018", `<shorttime>` for the short time in "HH:MM" format, `<longtime>` for the long time in "HH:MM:SS AM/PM" format, `<timezone>` for the standard time zone (eg. Egypt Standard Time), `<summertimezone>` for the daylight time zone name (eg. Syria Daylight Time).
| [chpwd](uesh/KS-Command-chpwd.md)                 | You can change your password, or someone else's password.
| [chusrname](uesh/KS-Command-chusrname.md)         | You can change your username, or someone else's name, although if you changed your own username to new name, you'll be signed out immediately.
| [disconndbgdev](uesh/KS-Command-disconndbgdev.md) | Disconnects a debug device
| [langman](uesh/KS-Command-langman.md)             | Manage languages
| [lsdbgdev](uesh/KS-Command-lsdbgdev.md)           | Lists all debug devices that are connected
| [modman](uesh/KS-Command-modman.md)               | Manages your mods
| [netinfo](uesh/KS-Command-netinfo.md)             | You can check your network status and network interface information, including WiFi support. You can also use this for troubleshooting problems with the network, and you can look at the packets that has an error.
| [perm](uesh/KS-Command-perm.md)                   | You can manage user's permission settings.
| [rmuser](uesh/KS-Command-rmuser.md)               | You can remove usernames, but you can't remove yours, if the specified user doesn't want to use the computer, or is uninvited, or is redundant.
| [rdebug](uesh/KS-Command-rdebug.md)               | Enables or disables remote debugging functionality
| [reloadconfig](uesh/KS-Command-reloadconfig.md)   | You can reload the configuration file to read the new changes, but the changes will be applied after you restart the kernel.
| [reloadsaver](uesh/KS-Command-reloadsaver.md)     | Reloads the specified screensaver mod file
| [rexec](uesh/KS-Command-rexec.md)                 | Remotely executes a command in another kernel instance (other PC)
| [savecurrdir](uesh/KS-Command-savecurrdir.md)     | Saves the current directory information to kernel config.
| [setsaver](uesh/KS-Command-setsaver.md)           | You can set your screensaver of your choice or your customized one as the default one, and if you plan to use customized screensavers, you should name your extension as `<ScreensaverName>SS.m` to be recognized as a screensaver, not as an extension.
| [settings](uesh/KS-Command-settings.md)           | Changes kernel settings.
| [testshell](uesh/KS-Command-testshell.md)         | Opens a test shell
| [unblockdbgdev](uesh/KS-Command-unblockdbgdev.md) | You can unblock an IP address so it can enter remote debug again.
| [update](uesh/KS-Command-update.md)               | Checks for updates, and if it found one, it tells you.

### Normal user commands

| Command                                                     | Description
|:------------------------------------------------------------|:------------
| [chattr](uesh/KS-Command-chattr.md)                         | Changes the attributes of a file
| [chdir](uesh/KS-Command-chdir.md)                           | You can change your working directory.
| [cls](uesh/KS-Command-cls.md)                               | To clear your screen from text.
| [calc](uesh/KS-Command-calc.md)                             | It's back! The Calculator calculates the formulas like `4 / 2`.
| [calendar](uesh/KS-Command-calendar.md)                     | Simple calendar
| [clearfiredevents](uesh/KS-Command-clearfiredevents.md)     | Clears the fired events
| [copy](uesh/KS-Command-copy.md)                             | Copies the source file to the destination
| [colorhextorgb](uesh/KS-Command-colorhextorgb.md)           | Converts the hexadecimal representation of the color to RGB numbers.
| [colorhextorgbks](uesh/KS-Command-colorhextorgbks.md)       | Converts the hexadecimal representation of the color to RGB numbers in KS format.
| [colorrgbtohex](uesh/KS-Command-colorrgbtohex.md)           | Converts the color RGB numbers to hex.
| [combine](uesh/KS-Command-combine.md)                       | Combines the two text files or more into the output file
| [convertlineendings](uesh/KS-Command-convertlineendings.md) | Converts the line endings in text files
| [dict](uesh/KS-Command-dict.md)                             | The English Dictionary
| [dismissnotif](uesh/KS-Command-dismissnotif.md)             | Dismisses a specific notification.
| [dismissnotifs](uesh/KS-Command-dismissnotifs.md)           | Dismisses all notifications
| [edit](uesh/KS-Command-edit.md)                             | Opens the text editor shell to an existing text file.
| [fileinfo](uesh/KS-Command-fileinfo.md)                     | Gets file information
| [find](uesh/KS-Command-find.md)                             | Finds a specified file
| [firedevents](uesh/KS-Command-firedevents.md)               | Lists all fired events
| [ftp](uesh/KS-Command-ftp.md)                               | You can transfer files from/to an FTP server, and interact with the servers.
| [genname](uesh/KS-Command-genname.md)                       | Name generator
| [get](uesh/KS-Command-get.md)                               | Downloads a file from the specified URL.
| [gettimeinfo](uesh/KS-Command-gettimeinfo.md)               | Gets the time information for the specified time
| [hexedit](uesh/KS-Command-hexedit.md)                       | Opens a binary file to the hex editor
| [http](uesh/KS-Command-http.md)                             | The HTTP Shell
| [hwinfo](uesh/KS-Command-hwinfo.md)                         | Shows hardware information
| [jsonbeautify](uesh/KS-Command-jsonbeautify.md)             | Beautifies the JSON file
| [jsonminify](uesh/KS-Command-jsonminify.md)                 | Minifies the JSON file
| [jsonshell](uesh/KS-Command-jsonshell.md)                   | The JSON Shell
| [keyinfo](uesh/KS-Command-keyinfo.md)                       | Gets the key information
| [list](uesh/KS-Command-list.md)                             | You can list your current working directory, or another directory.
| [listunits](uesh/KS-Command-listunits.md)                   | Lists all units
| [lockscreen](uesh/KS-Command-lockscreen.md)                 | You can lock your screen and show your default screensaver set by you or by the kernel. Default screensaver is Matrix.
| [lovehate](uesh/KS-Command-lovehate.md)                     | Starts the Love/Hate comment responder game
| [logout](uesh/KS-Command-logout.md)                         | You can log off your account when you're finished working.
| [lsvars](uesh/KS-Command-lsvars.md)                         | Lists variables
| [mail](uesh/KS-Command-mail.md)                             | Opens the mail shell to your mail account.
| [md](uesh/KS-Command-md.md)                                 | You can make your directory on the root directory.
| [meteor](uesh/KS-Command-meteor.md)                         | You're a spaceship and the meteors are destroying you.
| [mkfile](uesh/KS-Command-mkfile.md)                         | You can create your file under any name.
| [mktheme](uesh/KS-Command-mktheme.md)                       | Makes a new theme.
| [modmanual](uesh/KS-Command-modmanual.md)                   | Mod manual
| [move](uesh/KS-Command-move.md)                             | Moves the source file to the destination
| [open](uesh/KS-Command-open.md)                             | Opens a URL
| [ping](uesh/KS-Command-ping.md)                             | Pings addresses.
| [put](uesh/KS-Command-put.md)                               | Uploads a file to the URL using a file.
| [rarshell](uesh/KS-Command-rarshell.md)                     | Opens a RAR shell to the specified rar file
| [rm](uesh/KS-Command-rm.md)                                 | You can remove a directory or file.
| [reboot](uesh/KS-Command-reboot.md)                         | You can restart your kernel if you have made manual or tool configuration changes for them to be reflected, or if you want to see the boot sequence again.
| [reportbug](uesh/KS-Command-reportbug.md)                   | Opens a prompt to let you file a bug report.
| [retroks](uesh/KS-Command-retroks.md)                       | Retro Nitrocid KS based on 0.0.4.1
| [rss](uesh/KS-Command-rss.md)                               | Opens an RSS shell.
| [savescreen](uesh/KS-Command-savescreen.md)                 | You can show the screensaver to prevent screen burn-outs.
| [search](uesh/KS-Command-search.md)                         | Searches for a specific string in a specific file using regular expressions.
| [searchword](uesh/KS-Command-searchword.md)                 | Searches for a specific string in a specified file using text.
| [setthemes](uesh/KS-Command-setthemes.md)                   | You can set the color set for your kernel, as known as themes.
| [sftp](uesh/KS-Command-sftp.md)                             | You can transfer files from/to an SFTP server, and interact with the servers.
| [shownotifs](uesh/KS-Command-shownotifs.md)                 | Shows the notifications.
| [showtd](uesh/KS-Command-showtd.md)                         | You can show your current time and date, as well as your timezone.
| [showtdzone](uesh/KS-Command-showtdzone.md)                 | You can show the time and date of the timezone, or you can show all of the dates and times of the timezones in the current time and date.
| [shutdown](uesh/KS-Command-shutdown.md)                     | You can shut down your computer (the kernel, not the actual PC)
| [snaker](uesh/KS-Command-snaker.md)                         | Starts the snake game
| [solver](uesh/KS-Command-solver.md)                         | Starts the math solver game
| [speedpress](uesh/KS-Command-speedpress.md)                 | Initializes the speedpress game
| [spellbee](uesh/KS-Command-spellbee.md)                     | Plays the spelling bee game
| [sshell](uesh/KS-Command-sshell.md)                         | Opens the SSH connection. Press ESC to disconnect when in session.
| [sshcmd](uesh/KS-Command-sshcmd.md)                         | Opens the SSH connection to send a command.
| [stopwatch](uesh/KS-Command-stopwatch.md)                   | A simple stopwatch
| [sumfile](uesh/KS-Command-sumfile.md)                       | Calculates the MD5, SHA1, SHA256, or SHA512 sum of a specific file.
| [sumfiles](uesh/KS-Command-sumfiles.md)                     | Calculates the MD5, SHA1, SHA256, or SHA512 sums of the files in the specified directory.
| [timer](uesh/KS-Command-timer.md)                           | A simple timer
| [unitconv](uesh/KS-Command-unitconv.md)                     | Unit conversion
| [unzip](uesh/KS-Command-unzip.md)                           | Extracts a zip file
| [usermanual](uesh/KS-Command-usermanual.md)                 | Opens the Nitrocid KS wiki
| [verify](uesh/KS-Command-verify.md)                         | Verifies a file.
| [weather](uesh/KS-Command-weather.md)                       | Gets weather information for a city.
| [wrap](uesh/KS-Command-wrap.md)                             | Wraps a command
| [zip](uesh/KS-Command-zip.md)                               | Makes a zip file
| [zipshell](uesh/KS-Command-zipshell.md)                     | Opens a ZIP shell to the specified zip file

### Scripting commands

These commands can be used in shell and in scripting, though it works better in scripting.

| Command                                 | Description
|:----------------------------------------|:------------
| [beep](uesh/KS-Command-beep.md)         | Makes your PC speaker beep in specified n Hz in n ms.
| [cat](uesh/KS-Command-cat.md)           | Prints the content of a specific file to console
| [choice](uesh/KS-Command-choice.md)     | Makes user choices
| [echo](uesh/KS-Command-echo.md)         | Prints written strings
| [if](uesh/KS-Command-if.md)             | Satisfies the condition and then executes the command
| [input](uesh/KS-Command-input.md)       | Makes user input
| [set](uesh/KS-Command-set.md)           | Sets a variable to a specified value.
| [setrange](uesh/KS-Command-setrange.md) | Makes an array of variable with values
| [select](uesh/KS-Command-select.md)     | Makes user selection
