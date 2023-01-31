# Configuration for KS

## What is the config?

The config is the configuration file for your kernel that stores more kernel options that couldn't be edited in either the arguments or command-line arguments. It provides control on how the kernel or targeted routine in the kernel (network, shell, etc.) behave, like kernel colors, defaults, and so on. Some commands do change the config file. Use the `settings` command to change the kernel settings as the legacy KS Config Tool is obsolete.

There are many sections to make reading the config file easier thanks to the nice hierarchy. Refer to the table below to learn about the kernel settings by section.

## Config entries

Some of the configuration entries support placeholders. Refer to [Placeholders](../misc/Placeholders.md) for more info.

### General

| Name | Type | Values | Description 
|:----------------------------------------|:----------|:----------------------------------------|:---
| Prompt for Arguments on Boot            | `boolean` | `true` or `false`                       | You can force the kernel to give you the argument prompt.
| Maintenance Mode                        | `boolean` | `true` or `false`                       | You can make the kernel not to parse any mods and screensavers when booting, and gives you the opportunity to repair your kernel. It's also known as Safe Mode.
| Change Culture when Switching Languages | `boolean` | `true` or `false`                       | You can make the kernel change the culture based on language.
| Check for Updates on Startup            | `boolean` | `true` or `false`                       | If true, the kernel will check for updates on startup.
| Language                                | `string`  | Three-letter language name (e.g. `eng`) | Localizes Nitrocid KS to your country.
| Culture                                 | `string`  | ISO language format (e.g. `en-US`)      | Culture of the language
| Custom Startup Banner                   | `string`  | Style with placeholder                  | If specified, it will display customized startup banner with placeholder support. You can use {0} for kernel version.
| Show app information during boot        | `boolean` | `true` or `false`                       | Shows brief information about the application on boot
| Parse command-line arguments            | `boolean` | `true` or `false`                       | Parses the command-line arguments on boot.
| Show stage finish times                 | `boolean` | `true` or `false`                       | Shows how much time did the kernel take to finish a stage.
| Start kernel modifications on boot      | `boolean` | `true` or `false`                       | Automatically start the kernel modifications on boot.
| Show current time before login          | `boolean` | `true` or `false`                       | Shows the current time, time zone, and date before logging in.
| Notify for any fault during boot        | `boolean` | `true` or `false`                       | If there is a minor fault during kernel boot, notifies the user about it.
| Show stack trace on kernel error        | `boolean` | `true` or `false`                       | If there is any kernel error, choose whether or not to print the stack trace to the console.
| Check debug quota                       | `boolean` | `true` or `false`                       | Do we check if the debug system needs to check for quota before writing to the debugger? Please note that if this feature is enabled, the debugger will cause performance bottlenecks.
| Automatically download updates          | `boolean` | `true` or `false`                       | If there is any update, the kernel will automatically download it.
| Enable event debugging                  | `boolean` | `true` or `false`                       | Enables debugging for the kernel event system.
| New welcome banner                      | `boolean` | `true` or `false`                       | Shows the new Figlet-rendered welcome banner
| Stylish splash screen                   | `boolean` | `true` or `false`                       | Enables the stylish splash screen on startup. Please note that it will disable argument prompt and test shell pre-boot.
| Splash name                             | `string`  | Valid splash name                       | Splash name from the available splashes implemented in the kernel
| Banner figlet font                      | `string`  | Figlet font name supported by Figgle    | Write a figlet font that is supported by the Figgle library. Consult the library documentation for more information
| Simulate No APM Mode                    | `boolean` | `true` or `false`                       | If enabled, it will show the "It's now safe to turn off your computer" message on kernel shutdown.

### Colors

See ConsoleColor for more information.

| Name | Type | Values | Description
|:------------------------------------|:---------|:----------------------------------|:---
| User Name Shell Color               | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the username displaying part color in the whole prompt.
| Host Name Shell Color               | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the hostname displaying part color in the whole prompt.
| Continuable Kernel Error Color      | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the continuable kernel error text color.
| Uncontinuable Kernel Error Color    | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the fatal error text color.
| Text Color                          | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the general text color.
| License Color                       | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the license text color.
| Background Color                    | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the background color.
| Input Color                         | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the text input color.
| Listed command in Help Color        | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the printed command color in the command list.
| Definition of command in Help Color | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the printed description of the command color in the command list.
| Kernel Stage Color                  | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the kernel stage indicator color.
| Error Text Color                    | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the error text color.
| Warning Text Color                  | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the warning text color.
| Option Color                        | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the option text color.
| Banner Color                        | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the banner text color.
| Notification Title Color            | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the notification title color in the whole prompt.
| Notification Description Color      | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the notification description color in the whole prompt.
| Notification Progress Color         | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the notification progress text color.
| Notification Failure Color          | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the notification failure text color.
| Question Color                      | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the questiom text color.
| Success Color                       | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the success text color.
| User Dollar Color                   | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the user adminship indicator color.
| Tip Color                           | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the tip text color.
| Separator Text Color                | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the separator text color.
| Separator Color                     | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the separator color.
| List Title Color                    | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the list title text color.
| Development Warning Color           | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the development warning text color.
| Stage Time Color                    | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the stage time text color.
| Progress Color                      | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the progress text color.
| Back Option Color                   | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the back option color.
| Low Priority Border Color           | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the low priority notification border color.
| Medium Priority Border Color        | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the medium priority notification border color.
| High Priority Border Color          | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the high priority notification border color.
| Table Separator Color               | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the table separator color.
| Table Header Color                  | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the table header text color.
| Table Value Color                   | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the table value text color.
| Selected Option Color               | `string` | `0-15`, `0-255`, or `RRR;GGG;BBB` | You can change the selected option color.

### Hardware

| Name | Type | Values | Description
|:----------------------------|:----------|:------------------|:---
| Quiet Probe                 | `boolean` | `true` or `false` | You can make hardware probing quiet, by not showing results of probed hardware.
| Full Probe                  | `boolean` | `true` or `false` | Ensures that each hardware is probed.
| Verbose Probe               | `boolean` | `true` or `false` | You can make hardware probing verbose, by showing what probed.

### Login

| Name | Type | Values | Description
|:-------------------------|:----------|:-----------------------|:---
| Show MOTD on Log-in      | `boolean` | `true` or `false`      | You can make the log-in prompt show you the Message of the Day before displaying the prompt. It can be True or False.
| Clear Screen on Log-in   | `boolean` | `true` or `false`      | You can remove screen clutter before log-in if it is set to True. It can be True or False.
| Show available usernames | `boolean` | `true` or `false`      | You can choose whether or not to show available usernames.
| MOTD Path                | `string`  | Path to text file      | Which file is the MOTD text file? Write an absolute path to the text file.
| MAL Path                 | `string`  | Path to text file      | Which file is the MAL text file? Write an absolute path to the text file.
| Host Name                | `string`  | Host name              | You can change the host name of the kernel.
| Username prompt style    | `string`  | Style with placeholder | Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed.
| Password prompt style    | `string`  | Style with placeholder | Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed.
| Show MAL on Log-in       | `boolean` | `true` or `false`      | Shows Message of the Day after displaying login screen.
| Include anonymous users  | `boolean` | `true` or `false`      | Includes the anonymous users in the list
| Include disabled users   | `boolean` | `true` or `false`      | Includes the disabled users in the list

### Shell

| Name | Type | Values | Description
|:-------------------------------------|:----------|:------------------------------|:---
| Colored Shell                        | `boolean` | `true` or `false`             | You can add support for coloring the shell.
| Simplified Help Command              | `boolean` | `true` or `false`             | You can list the commands in the comma-separated form.
| Current Directory                    | `string`  | Existing directory            | Each time the main shell runs, it will be set to this directory. It should exist.
| Lookup Directories                   | `string`  | Paths separated by colon      | When running a common system command (a file) that are located in these paths, ensure that it runs. This works the same as PATH.
| Prompt Style                         | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the main shell. Placeholders here are parsed.
| FTP Prompt Style                     | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the FTP shell. Placeholders here are parsed.
| Mail Prompt Style                    | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the mail shell. Placeholders here are parsed.
| SFTP Prompt Style                    | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the SFTP shell. Placeholders here are parsed.
| RSS Prompt Style                     | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the RSS shell. Placeholders here are parsed.
| Text Edit Prompt Style               | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the text edit shell. Placeholders here are parsed.
| ZIP Shell Prompt Style               | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the ZIP shell. Placeholders here are parsed.
| Test Shell Prompt Style              | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the test shell. Placeholders here are parsed.
| JSON Shell Prompt Style              | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the JSON shell. Placeholders here are parsed.
| Hex Edit Prompt Style                | `string`  | Style with placeholder        | Prompt style. Leave blank to use default style. It only affects the hex edit shell. Placeholders here are parsed.
| Probe injected commands              | `boolean` | `true` or `false`             | Probes the injected commands at the start of the kernel shell.
| Start color wheel in true color mode | `boolean` | `true` or `false`             | Start color wheel in true color mode
| Default choice output type           | `string`  | ChoiceOutputType value string | Default choice output type

### Filesystem

| Name | Type | Values | Description
|:---------------------------------------------|:----------|:-------------------------------------|:---
| Filesystem sort mode                         | `string`  | FilesystemSortOptions value string   | Chooses how to sort files
| Filesystem sort direction                    | `string`  | FilesystemSortDirection value string | Chooses what direction the sort works
| Debug Size Quota in Bytes                    | `double`  | A size in bytes                      | Specifies the maximum log size in bytes. If this was exceeded, it will remove the first 5 lines from the log to free up some space.
| Size parse mode                              | `boolean` | `true` or `false`                    | Parse whole directory for size. If set to False, it will parse just the surface.
| Show Hidden Files                            | `boolean` | `true` or `false`                    | Whether or not to list hidden files.
| Show progress on filesystem operations       | `boolean` | `true` or `false`                    | Shows what file is being processed during the filesystem operations
| Show file details in list                    | `boolean` | `true` or `false`                    | Shows the brief file details while listing files
| Suppress unauthorized messages               | `boolean` | `true` or `false`                    | Hides the annoying message if the listing function tries to open an unauthorized folder
| Print line numbers on printing file contents | `boolean` | `true` or `false`                    | Makes the "cat" command print the file's line numbers.
| Sort the list                                | `boolean` | `true` or `false`                    | Sorts the filesystem list professionally.
| Show total size in list                      | `boolean` | `true` or `false`                    | If enabled, shows the total folder size in list, depending on how to calculate the folder sizes according to the configuration.

### Network

| Name | Type | Values | Description
|:-----------------------------------------------------------------------------|:----------|:---------------------------|:---
| Debug Port                                                                   | `integer` | Any unused port            | Specifies the remote debugger port. Make sure that the selected port is not used.
| Download Retry Times                                                         | `integer` | A number of times          | How many times does the "get" command retry the download before assuming failure?
| Upload Retry Times                                                           | `integer` | A number of times          | How many times does the "put" command retry the upload before assuming failure?
| Record chat to debug log                                                     | `boolean` | `true` or `false`          | Whether or not to log the chat history of all devices to the debug log.
| Log FTP username                                                             | `boolean` | `true` or `false`          | Whether or not to log FTP username in the debugger log.
| Log FTP IP address                                                           | `boolean` | `true` or `false`          | Whether or not to log FTP IP address in the debugger log.
| Return only first FTP profile                                                | `boolean` | `true` or `false`          | If true, uses the first working profile to connect to the FTP server.
| Show progress bar while downloading or uploading from `get` or `put` command | `boolean` | `true` or `false`          | Self-explanatory
| Show mail message preview                                                    | `boolean` | `true` or `false`          | Self-explanatory
| Show SSH banner                                                              | `boolean` | `true` or `false`          | Whether or not to show the SSH banner if the server has one configured.
| Enable RPC                                                                   | `boolean` | `true` or `false`          | Self-explanatory
| RPC Port                                                                     | `integer` | Any unused port            | Specifies the RPC port. Make sure that the selected port is not used.
| Show file details in FTP list                                                | `boolean` | `true` or `false`          | Shows the FTP file details while listing remote directories.
| Username prompt style for FTP                                                | `string`  | Style with placeholder     | Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed.
| Password prompt style for FTP                                                | `string`  | Style with placeholder     | Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed.
| Use first FTP profile                                                        | `boolean` | `true` or `false`          | Uses the first FTP profile to connect to FTP.
| Add new connections to FTP speed dial                                        | `boolean` | `true` or `false`          | If enabled, adds a new connection to the FTP speed dial.
| Try to validate secure FTP certificates                                      | `boolean` | `true` or `false`          | Tries to validate the FTP certificates. Turning it off is not recommended.
| Show FTP MOTD on connection                                                  | `boolean` | `true` or `false`          | Shows the FTP message of the day on login.
| Always accept invalid FTP certificates                                       | `boolean` | `true` or `false`          | Always accept invalid FTP certificates. Turning it on is not recommended as it may pose security risks.
| Username prompt style for mail                                               | `string`  | Style with placeholder     | Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed.
| Password prompt style for mail                                               | `string`  | Style with placeholder     | Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed.
| IMAP prompt style for mail                                                   | `string`  | Style with placeholder     | Write how you want your IMAP server prompt to be. Leave blank to use default style. Placeholders are parsed.
| SMTP prompt style for mail                                                   | `string`  | Style with placeholder     | Write how you want your SMTP server prompt to be. Leave blank to use default style. Placeholders are parsed.
| Automatically detect mail server                                             | `boolean` | `true` or `false`          | Automatically detect the mail server based on the given address.
| Enable mail debug                                                            | `boolean` | `true` or `false`          | Enables mail server debug
| Notify for new mail messages                                                 | `boolean` | `true` or `false`          | Notifies you for any new mail messages.
| GPG password prompt style for mail                                           | `string`  | Style with placeholder     | Write how you want your GPG password prompt to be. Leave blank to use default style. Placeholders are parsed.
| Send IMAP ping interval                                                      | `integer` | Interval in milliseconds   | How many milliseconds to send the IMAP ping?
| Send SMTP ping interval                                                      | `integer` | Interval in milliseconds   | How many milliseconds to send the SMTP ping?
| Mail text format                                                             | `string`  | TextFormat value string    | Controls how the mail text will be shown.
| Automatically start remote debug on startup                                  | `boolean` | `true` or `false`          | If you want remote debug to start on boot, enable this.
| Remote debug message format                                                  | `string`  | Style without placeholder  | Specifies the remote debug message format. {0} for name, {1} for message.
| RSS feed URL prompt style                                                    | `string`  | Style with placeholder     | Write how you want your RSS feed server prompt to be. Leave blank to use default style. Placeholders are parsed.
| Auto refresh RSS feed                                                        | `boolean` | `true` or `false`          | Auto refresh RSS feed
| Auto refresh RSS feed interval                                               | `integer` | Interval in milliseconds   | How many milliseconds to refresh the RSS feed?
| Show file details in SFTP list                                               | `boolean` | `true` or `false`          | Shows the SFTP file details while listing remote directories.
| Username prompt style for SFTP                                               | `string`  | Style with placeholder     | Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed.
| Add new connections to SFTP speed dial                                       | `boolean` | `true` or `false`          | If enabled, adds a new connection to the SFTP speed dial.
| Ping timeout                                                                 | `integer` | Interval in milliseconds   | How many milliseconds to wait before declaring timeout?
| Show extensive adapter info                                                  | `boolean` | `true` or `false`          | Prints the extensive adapter information, such as packet information.
| Show general network information                                             | `boolean` | `true` or `false`          | Shows the general information about network
| Download percentage text                                                     | `string`  | Style with placeholder     | Write how you want your download percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for downloaded size, {1} for target size, {2} for percentage.
| Upload percentage text                                                       | `string`  | Style with placeholder     | Write how you want your upload percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for uploaded size, {1} for target size, {2} for percentage.
| Recursive hashing for FTP                                                    | `boolean` | `true` or `false`          | Whether to recursively hash a directory. Please note that not all the FTP servers support that.
| Maximum number of e-mails in one page                                        | `integer` | Number of e-mails per page | How many e-mails should be shown in one page?
| POP3 prompt style for mail                                                   | `string`  | Style with placeholder     | Write how you want your POP3 server prompt to be. Leave blank to use default style. Placeholders are parsed.
| Send POP3 ping interval                                                      | `integer` | Interval in milliseconds   | How many milliseconds to send the POP3 ping?
| Use POP3                                                                     | `boolean` | `true` or `false`          | Whether to use POP3. Disabling this will use SMTP.
| Show mail transfer progress                                                  | `boolean` | `true` or `false`          | If enabled, the mail shell will show how many bytes transmitted when downloading mail.
| Mail transfer progress                                                       | `string`  | Style with placeholder     | Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size and {1} for total size.
| Mail transfer progress (single)                                              | `string`  | Style with placeholder     | Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size.
| Show notification for download progress                                      | `boolean` | `true` or `false`          | Shows the notification showing the download progress.
| Show notification for upload progress                                        | `boolean` | `true` or `false`          | Shows the notification showing the upload progress.
| RSS feed fetch timeout                                                       | `integer` | Interval in milliseconds   | How many milliseconds to wait before RSS feed fetch timeout?
| Verify retry attempts for FTP transmission                                   | `boolean` | `true` or `false`          | How many times to verify the upload and download and retry if the verification fails before the download fails as a whole?
| FTP connection timeout                                                       | `integer` | Interval in milliseconds   | How many milliseconds to wait before the FTP connection timeout?
| FTP data connection timeout                                                  | `integer` | Interval in milliseconds   | How many milliseconds to wait before the FTP data connection timeout?
| FTP IP versions                                                              | `string`  | FtpIpVersion value string  | Choose the version of Internet Protocol that the FTP server supports and that the FTP client uses.
| Notify on remote debug connection error                                      | `boolean` | `true` or `false`          | If enabled, will use the notification system to notify the host of remote debug connection error. Otherwise, will use the default console writing.

### Screensaver

| Name | Type | Values | Description
|:-------------------------------|:----------|:-----------------------------|:---
| Screensaver                    | `string`  | Screensaver name             | You can choose your screensavers available.
| Screensaver Timeout in ms      | `integer` | Time in milliseconds         | Self-explanatory
| Enable screensaver debugging   | `boolean` | `true` or `false`            | Enables debugging for screensavers. Please note that it may quickly fill the debug log and slightly slow the screensaver down, depending on the screensaver used. Only works if kernel debugging is enabled for diagnostic purposes.
| Ask for password after locking | `boolean` | `true` or `false`            | After locking the screen, ask for password

> [!NOTE]
> These screensavers are built-in directly to Nitrocid KS and are always available. If you want your screensaver to be included by default to Nitrocid KS, let us know. Consult [Screensaver Configuration](Screensaver-settings-for-KS.md) for configuration entries for specific screensavers.

### Misc

| Name | Type | Values | Description
|:-----------------------------------------------------|:----------|:-------------------------------------|:---
| Show Time/Date on Upper Right Corner                 | `boolean` | `true` or `false`                    | You can establish the live time/date banner that's updating, and the position is on the upper-right corner.
| Marquee on startup                                   | `boolean` | `true` or `false`                    | Whether or not to activate banner animation.
| Long Time and Date                                   | `boolean` | `true` or `false`                    | Whether or not to render time and date using long.
| Preferred Unit for Temperature                       | `string`  | UnitMeasurement value string         | Select the preferred unit for temperature. One of Kelvin (1), Metric (2), or Imperial (3) is accepted.
| Enable text editor autosave                          | `boolean` | `true` or `false`                    | Turns on or off the text editor autosave feature.
| Text editor autosave interval                        | `integer` | Time in milliseconds                 | If autosave is enabled, the text file will be saved for each "n" seconds.
| Wrap list outputs                                    | `boolean` | `true` or `false`                    | If enabled, the console will stop printing on wrappable commands until a key is pressed if the printed lines exceed the console window height.
| Draw notification border                             | `boolean` | `true` or `false`                    | Covers the notification with the border.
| Blacklisted mods                                     | `string`  | Mod paths separated by semicolons    | Write the filenames of the mods that will not run on startup. When you're finished, write "q". Write a minus sign next to the path to remove an existing mod.
| Solver minimum number                                | `integer` | Minimum number                       | What is the minimum number to choose?
| Solver maximum number                                | `integer` | Maximum number                       | What is the maximum number to choose?
| Solver show input                                    | `boolean` | `true` or `false`                    | Whether to show what's written in the input prompt.
| Upper left corner character for notification border  | `char`    | A single character                   | A character that resembles the upper left corner.
| Upper right corner character for notification border | `char`    | A single character                   | A character that resembles the upper right corner.
| Lower left corner character for notification border  | `char`    | A single character                   | A character that resembles the lower left corner.
| Lower right corner character for notification border | `char`    | A single character                   | A character that resembles the lower right corner.
| Upper frame character for notification border        | `char`    | A single character                   | A character that resembles the upper frame.
| Lower frame character for notification border        | `char`    | A single character                   | A character that resembles the lower frame.
| Left frame character for notification border         | `char`    | A single character                   | A character that resembles the left frame.
| Right frame character for notification border        | `char`    | A single character                   | A character that resembles the right frame.
| Manual page information style                        | `string`  | Style with placeholder               | Write how you want your manpage information to be. Leave blank to use default style. Placeholders are parsed. {0} for manual title, {1} for revision.
| Default difficulty for SpeedPress                    | `string`  | SpeedPressDifficulty value string    | Select your preferred difficulty
| Keypress timeout for SpeedPress                      | `integer` | Interval in milliseconds             | How many milliseconds to wait for the keypress before the timeout? (In custom difficulty)
| Show latest RSS headline on login                    | `boolean` | `true` or `false`                    | Each login, it will show the latest RSS headline from the selected headline URL.
| RSS headline URL                                     | `string`  | A valid URL to your RSS feed         | RSS headline URL to be used when showing the latest headline. This is usually your favorite feed.
| Save all events and/or reminders destructively       | `boolean` | `true` or `false`                    | If enabled, deletes all events and/or reminders before saving all of them using the calendar command.
| Upper left corner character for RGB color wheel      | `char`    | A single character                   | A character that resembles the upper left corner.
| Upper right corner character for RGB color wheel     | `char`    | A single character                   | A character that resembles the upper right corner.
| Lower left corner character for RGB color wheel      | `char`    | A single character                   | A character that resembles the lower left corner.
| Lower right corner character for RGB color wheel     | `char`    | A single character                   | A character that resembles the lower right corner.
| Upper frame character for RGB color wheel            | `char`    | A single character                   | A character that resembles the upper frame.
| Lower frame character for RGB color wheel            | `char`    | A single character                   | A character that resembles the lower frame.
| Left frame character for RGB color wheel             | `char`    | A single character                   | A character that resembles the left frame.
| Right frame character for RGB color wheel            | `char`    | A single character                   | A character that resembles the right frame.
| Default JSON formatting for JSON shell               | `string`  | Format value string                  | Selects the default JSON formatting (beautified or minified) for the JSON shell to save.
| Enable Figlet for timer                              | `boolean` | `true` or `false`                    | If enabled, will use figlet for timer. Please note that it needs a big console screen in order to render the time properly with Figlet enabled.
| Figlet font for timer                                | `string`  | Figlet font name supported by Figgle | Write a figlet font that is supported by the Figgle library. Consult the library documentation for more information
| Show the commands count on help                      | `boolean` | `true` or `false`                    | Shows the commands count in the command list, controlled by the three count show switches for different kinds of commands.
| Show the shell commands count on help                | `boolean` | `true` or `false`                    | Self-explanatory
| Show the mod commands count on help                  | `boolean` | `true` or `false`                    | Self-explanatory
| Show the aliases count on help                       | `boolean` | `true` or `false`                    | Self-explanatory
| Password mask character                              | `char`    | A single character                   | A character that is placed to enter the password.
| Upper left corner character for progress bars        | `char`    | A single character                   | A character that resembles the upper left corner.
| Upper right corner character for progress bars       | `char`    | A single character                   | A character that resembles the upper right corner.
| Lower left corner character for progress bars        | `char`    | A single character                   | A character that resembles the lower left corner.
| Lower right corner character for progress bars       | `char`    | A single character                   | A character that resembles the lower right corner.
| Upper frame character for progress bars              | `char`    | A single character                   | A character that resembles the upper frame.
| Lower frame character for progress bars              | `char`    | A single character                   | A character that resembles the lower frame.
| Left frame character for progress bars               | `char`    | A single character                   | A character that resembles the left frame.
| Right frame character for progress bars              | `char`    | A single character                   | A character that resembles the right frame.
| Users count for love or hate comments                | `integer` | Interval in milliseconds             | How many user names to generate in LoveHate game?
| Input history enabled                                | `boolean` | `true` or `false`                    | Whether the input history is enabled
| Input clipboard enabled                              | `boolean` | `true` or `false`                    | Whether the input clipboard is enabled. Use `CTRL + Y` to paste or yank the contents back.
| Input undo enabled                                   | `boolean` | `true` or `false`                    | Whether the input undo is enabled
