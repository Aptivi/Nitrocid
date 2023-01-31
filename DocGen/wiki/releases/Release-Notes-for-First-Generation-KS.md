# First-generation KS releases

## KS 0.0.1.x series

### KS 0.0.1.0 (2/22/2018)

In the release of 0.0.1.0, it originally included the prober and their codes are in the `Kernel.vb` code file, but that seemed irrelevant and messy. It also included the original log-in and shell, but it seems that on the booting procedure, it has generated the following output:

```
(1) usrmgr: System Usernames: root useradd created.
(2) usrmgr: User root has been successfully loaded.
(3) usrmgr: User useradd has been successfully loaded.
(4) usrmgr: demo not found. Creating...
(5) usrmgr: Username demo created.
(6) usrmgr: User root has been successfully loaded.
(7) usrmgr: User useradd has been successfully loaded.
(8) usrmgr: User demo has been successfully loaded.
(9) usrmgr: User demo has been successfully loaded.
```

On `(1)`, it told us that these administrators, root, and useradd (test account with admin rights), has been created but not yet initialized.

On `(2-3)`, it told us that they have been loaded.

On `(4-5)`, the test account has been created according to the manager and to the kernel.

On `(6-9)`, the two admins have been loaded twice, like on `(2-3)` (bug), and on `(8-9)`, the test account loaded TWICE, not once. It's a bug.

Also, it has made 2 unnecessary newlines between (9) and `Welcome to Kernel!`

The `Username:` prompt is `login:`, meaning that it's difficult for users that haven't used Linux yet to understand what did "login" mean. However, the `password:` prompt is easy to understand for both the starters and experts.

After you log-in, your prompt looks like this: `[root@kernel] #: `. The colon seems irrelevant.

As soon as you enter "help", it will have a list of commands. However, it's uncolored.

Some commands have their own aliases, and some commands report `This kernel is not final.` At almost every command executed, it looks like this:

```
(1) 
(2) Output
(3)
```

Seems that in both `(1)` and `(3)`, these newlines are completely unnecessary. Also, the output of `lsdrivers` is somehow colored white when this version of kernel isn't supposed to have colors on outputs.

It has these 2 screensavers, Disco, and BW Disco `future-eyes-destroyer`

As soon as you run Disco, it will show the amazing disco effect, but it has a bug when you exit when the BG color is other than black that when you press ENTER, the BG color stays and prints the shell prompt then clears the BG color without clearing the screen, and when you type in commands, it has a black box around the written characters. Same thing as the output. When you reboot, everything will be fine.

However, BW Disco will destroy your eyes (really) and has the same bug.

Listing files has a fake output, although the kernel is not final. It lists boot, bin, dev, etc, lib, proc, usr, var and says "Total number: 8 folders, 0 files." The directory set looks like the Linux dirset.

You can't have arguments added to the command, or KS will assume that the command didn't exist although it exists, saying that for ex. you're trying to execute "ls bin", and it says that "Shell message: The requested command ls bin is not found." Somehow, the message didn't have any unnecessary newline problem.

In `Login.vb`, it contains many unnecessary variables. It has 5 variables for username and password, each being from different sources or declaration:

```vb
Public usernamelist As New List(Of String)           (Username list from Login.vb)
Public passwordlist As New List(Of String)           (Password list from Login.vb)
Public userword As New Dictionary(Of String, String) (Dictionary stores Username and Password)
My.Settings.Passwords                                (Password list from My.Settings)
My.Settings.Usernames                                (Username list from My.Settings)
```

Unnecessary variables. compared to the 0.0.3.0 version:

```vb
Public userword As New Dictionary(Of String, String)         (Dictionary stores Username and Password)
```

### KS 0.0.1.1 (3/16/2018)

The changelogs at "Documentation - main page.txt" on src. code claim that it "added "showmotd", changed a message and better checking for integer overflow on Beep Frequency."

But which message? According to Meld (file comparison software), it says that the MOTD change done message is changed:

* Before: `Done!\nPlease log-out for the changes to take effect.`
* After: `Done!\nPlease log-out, or use 'showmotd' to see the changes`

Sadly, this version has the same exact bugs as the first version.

## KS 0.0.2.x series

### KS 0.0.2.0 (3/31/2018)

The changelogs said that its code is re-designed, has more commands, implemented basic Internet, argument system, changing password, and more changes.

Although it has the same shutdown/restart delaying bug and the no newline after the shell message bug, here's how exactly the changes go:

1. The "login:" prompt has changed to "Username:" for easier understanding
2. There is a kernel argument system, and they're "motd, nohwprobe, chkn=1, preadduser, hostname, and quiet." Let's see what do each argument do:
   * motd: It tells you to write your message. (It worked)
   * nohwprobe: It tells the kernel not to detect the hardware. (It worked)
   * chkn=1: It checks for connectivity, but asks for address first. (It worked)
   * preadduser: It adds user right on startup. (It worked)
   * hostname: It changes your hostname temporarily. (It worked)
   * quiet: It makes the kernel silent. (It worked)
   * But there is a bug that when you try to execute arguments after rebooting the kernel which also has arguments executed, it will crash.
3. Everything is colored now.
4. Fixed unnecessary newlines between the output
5. chpwd, hwprobe, lsnet, lsnettree, ping, showtd and sysinfo are added
6. Fixed the disco background issue
7. Everything that is mentioned in the changelog

### KS 0.0.2.1 (4/5/2018)

The changelog says that it fixed a bug for "Command Not Found" message, and added forgotten checking for root in "chhostname" and "chmotd".

There are no extra changes.

### KS 0.0.2.2 (4/9/2018)

The changelog says that it fixed a bug for network list where double PC names show up on both listing ways, Error handling on listing networks.

It also added an error handler for the kernel error.

### KS 0.0.2.3 (4/11/2018)

The changelog says that it fixed crash on arguments after reboot, fix bugs, and more. What exactly is "fix bugs, and more?"

Reason for crash: The simulator tried to initialize the time and date on each reboot.

1. It has fixed newline issues when using frequency that is over 2048 Hz after answering question.
   * Before: `Are you sure that you want to beep at this frequency, 2049? (y/N) yBeep Time in seconds that has the limit of 1-3600:`
   * After: `Are you sure that you want to beep at this frequency, 2049? (y/N) y\nBeep Time in seconds that has the limit of 1-3600:`
2. It has fixed newline issues when answering the question wrong on choice command. 
   * Before: `Do we have this bug? <Y/N> y[root@kernel] #`
   * After: `Do we have this bug? <Y/N> y\n[root@kernel] #`

## KS 0.0.3.x series

### KS 0.0.3.0 (4/30/2018)

The changelog says that it fixed bugs, Log-in system rewritten, added commands, added arguments, added permission system, custom colors, and more.

This version allowed you to use colors of your choice. Let's be specific:

1. Added the error handler when parsing arguments
2. Added the gpuprobe argument (probes graphics card every startup)
3. Added the check for network tools
4. Added casting for showing time and date
5. Added resetting variables function (was function, now sub)
6. Added error handling when starting up kernel

Please note that the error handling code blocks at this release is not the Try...Catch block, but the standard VB6 error handler code snippet below (On the Console application):

```vb
On Error GoTo HandlerLabel

Dim IntNumber As Integer = 50 / 0
Exit Sub

:HandlerLabel
Console.WriteLine("Divide by Zero")
Console.ReadKey
```

### KS 0.0.3.1 (5/2/2018)

The changelog says that it has edited the shell title in preparation for 0.0.4.0, fix bugs with removing users, fix blank command, and added admin checking. 

The big release was scheduled to release in May 20, 2018, which is 3 days before the hard drive failed in such a horrible way. 

In anyway, in the exact form:

1. In Login.vb, the management subs were moved into UserManagement.vb, leading to only these subs remaining:
   * `Sub LoginPrompt()`
   * `Sub showPasswordPrompt(ByVal usernamerequested As String)`
   * `Sub signIn(ByVal signedInUser As String)`
2. Added the "(Admins Only)" flag on chusrname, editperm, and rmuser
3. Added the restricted command array
4. Every command that starts with the space is considered as a comment
5. Now default permissions is set everytime a user is created

## KS 0.0.4.x series

### KS 0.0.4.0 (5/20/2018)

This is the big release of KS. The changelog say that it has changed of startup text, customizable settings, Themes, Command-line arguments, Command argument and full parsing, Actual directory system (alpha), more commands, calculator, debugging with stack trace, debugging logs (unfinished), no RAM leak, fix bugs, and more.

In the exact form:

1. Added command-line arguments and boot argument arguments
2. Added debugging (Wdbg)
3. Added the writing system (W and Wln) so that we will not use the loop of:
   ```vb
   System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
   answerargs = System.Console.ReadLine()
   System.Console.ResetColor()
   ```
4. Added the real command-line arguments (Running CMD on the KS root folder and running the executable file with arguments like "KS.exe createConf")
   * createConf: Makes the kernel create the config file then exit
   * promptArgs: Forces the kernel to prompt you for arguments
5. Added themes
6. Added error handler for setting permission
7. Converted all error handlers to Try...Catch
8. Added the option to show MOTD
9. Added maintenance mode
10. Added settings
11. Added calculator
12. No RAM leak
13. Added setthemes, netinfo, calc, scical, unitconv, md, mkdir, rd, rmdir and debuglog commands
14. Slowed down the time date change repeat rate from 100 ms to 500 ms
15. Pinging target address can now be repeated
16. Hardware probing is now extended
17. Moved every sub that is in Kernel.vb to files that are appropriate

It's really big!

### KS 0.0.4.1 (5/22/2018)

The changelog says that it has fixed bugs in changing directory, fixed bugs in "help chdir", added alias for changing directory named "cd", and config update. But, how exactly?

Before the HDD failure incident that lasted 5 days, this version was released as a quick backup release, called Failure Followup Release.

1. We forgot to add the license header for Flags.vb
2. Added the config upgrade system
3. Added missing entry for "cd"
4. Added the statement `(And currDir = "/")`
5. Added the reloadconfig command (this command lets you reload config)
6. Edited the help entry for changedir so it reflects "cd"
7. Added the help entry for added command

This was released in 5/22/2018, 1 day before the HDD failure in the development environment.

### KS 0.0.4.5 (7/15/2018)

The changelog says that it has fixed bugs when any probers failed to probe hardware, added more details in probers, Help system improved, Fix bugs in color prompts, Prompts deprecated, and more, but also says that it has been delayed for 2 months due to the hard drive failure incident.

1. Made changing MOTD and hostname permanent
2. You can split when injecting commands using colons with spaces between them.
3. Added maintenance mode argument
4. Changed the tip for help argument
5. Added error handler when setting colors
6. No more bugs when any probers failed to get information about hardware
7. Extended probers to contain more information
8. Added async time/date on corner
9. Changed the welcome banner
10. Now the hardware probing error handling is more friendly
11. Support for variables for Kernel Error
12. Completely abandoned My.Settings MOTD and Hostname variables
13. Added more temperature units
14. Added config entries for Corner Time/Date, MOTD, and HostName
15. Removed the BW Disco
16. Prompts are deprecated
17. Organized the help system
18. We slowed down changing time/date from 500 ms to 1000 ms
19. Removed unnecessary sub ShowTimeQuiet()

### KS 0.0.4.6 (7/16/2018)

The changelog says that it has removed extraneous "fed" that stands as the removed command in 0.0.4.5 and preparation for 0.0.5's custom substitutions

The custom substitutions preparation is exactly informing the user that the pre-defined aliases will be removed when running commands that has their own aliases, and when running the help command

However the extraneous "fed" command is removed from the availableCommands() array.

### KS 0.0.4.7 (7/17/2018)

The changelog says that it has better Error Handling for "ping" command and fixed "unitconv" usage message.

1. Removed the message "net: Network available." when chkn=1 is injected.
2. Converted the rest of VB6 error handlers in Network.vb to Try...Catch blocks
3. Ping attempts can now be repeated when there is an error
4. The unitconv help message when the user didn't enter enough arguments is now updated to reflect the new units.

### KS 0.0.4.8 (4/30/2021)

1. Removed panicsim
2. Fixed time and date not showing on reboot
3. Now help displays "no help" when command is not found
4. Fixed help usage showing if the command and arguments are written

## KS 0.0.5.0 beta versions

### KS 0.0.4.9 (7/21/2018)

The changelog says that it has better Error Handling for "unitconv" command, Added temporary aliases (not final because there is no "showaliases" command), fix some bugs, added time zones ("showtdzone", and show current time zone in "showtd"), Added "alias", "chmal", and "showmal", Made MOTD after login customizable, Allowed special characters on passwords to ensure security, Made Nitrocid KS single-instance to avoid interferences, and more.

1. Multiple-instance check has been added using mutex to avoid interference with two or more KS processes that interfere with each other.
2. Added the MOTD After Login
3. Passwords can now have the symbols to ensure more security
4. Fixed the error message that when removing users, it says "Error trying to add username."
5. Conversion of units are now case-insensitive
6. Fixed unitconv showing results even if an error message appeared
7. Better behavior when updating config
8. You can manage aliases
9. Changed the prompt removing message
10. Changed from showing help as string to HelpSystem.ShowHelp(words(0))
11. Added alias, chmal, showmal and showtdzone commands
12. Made the shell parse aliases
13. Made the writer in the CMD prompt write sub one-line
14. Added support for timezones

### KS 0.0.4.10 (8/1/2018)

The changelog says that it has fused "sysinfo" with "lsdrivers", Improved Help definition (used dictionary for preparation for modding), added "lscomp" which can list all online and offline computers by names only, Added error handler for "lsnet" and "lsnettree", fixed grammatical mistakes in "lsnet" and "lsnettree", added mods (commands not implemented yet - `<modname>.m`), added screensavers, changed the behavior of showing MOTD, fixed bug where instance checking after reboot of the kernel would say that it has more than one instance and should close, and more.

1. Made every sub and every module public for modifications
2. Added modification support
3. Fixed the instance checking every reboot (The instanceChecked flag has been added)
4. Made every if statement that is one-statement one-line
5. Now MOTD will only be shown once
6. Added screensavers
7. Added support for listing all computers, even if they're offline
8. Added VB6 error handler for lscomp, lsnet, and lsnettree
9. Running Disco Effect now makes the cursor invisible, while if exiting, the background will be reset and the cursor will be visible.
10. Reduced the number of times the code block below is run in GetCommand.vb:
    ```vb
    Dim words = requestedCommand.Split({" "c})
    Dim c As Integer
    For arg = 1 To words.Count - 1
        c = c + words(arg).Count + 1
    Next
    Dim strArgs As String = requestedCommand.Substring(requestedCommand.IndexOf(" "), c)
    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
    ```
    * by moving it to the top with some modifications, resulting in:
    ```vb
    Dim index As Integer = requestedCommand.IndexOf(" ")
    If (index = -1) Then index = requestedCommand.Length
    Dim words = requestedCommand.Split({" "c})
    Dim strArgs As String = requestedCommand.Substring(index)
    If Not (index = requestedCommand.Length) Then strArgs = strArgs.Substring(1)
    Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
    ```
11. Changed the redundant alias from beep to annoying-sound
12. Added loadsaver, lockscreen, setsaver and savescreen commands
13. Replaced lsdrivers with lscomp, and the output of lsdrivers is moved into sysinfo
14. Now, ExecuteCommand() sub's error handler can say which command failed to execute.
15. The help definitions are now converted into dictionary. This is also a preparation for translated kernels.
16. Fixed the messy command listing in ShowHelp()
17. ueshversion variable is removed because it isn't updated since 0.0.4, and the UESH version is now inherited from the version of the kernel

### KS 0.0.4.11 (8/3/2018)

The changelog says that it has removed pre-defined aliases, and fixed known bug that is mentioned.

Let's see what is our known bug that is lying around until 0.0.4.11.

The troubleshooting documentation on the source code says that when setting "Colored Shell" to False, you will see that input color is white instead of gray.

The root cause? On TextWriterColor.vb, on W() sub, the code looks like this (one-line, was three lines):

```vb
If (colorType = "input") Then ForegroundColor = inputColor
```

It didn't check if the ColoredShell variable was true, so it always tries to change the console foreground color to inputColor, even if the Colored Shell setting was set to false.

Because this bug is fixed, the code now looks like this:

```vb
If (colorType = "input" And ColoredShell = True) Then ForegroundColor = inputColor
```

However, this change didn't happen on Wln() sub because all prompts use W() to properly initialize the prompts.

Now, the exact changes:

1. Changed the error handler for W() and Wln() subs from VB6 method to Try...Catch blocks
2. Now, when the template is LinuxUncoloredDef, the colored shell variable gets set to False, because everything is uncolored in this template.
3. Removed the pre-defined aliases for commands
4. Edited the help definition so it says that the disco command will be removed and the effect will take place in screensavers.

### KS 0.0.4.12 (8/16/2018)

The changelog only says "Replaced disco command with a screensaver."

## KS 0.0.5.x series

### KS 0.0.5.0 (9/4/2018)

The changelog says "Removed prompts, fixed MAL username probing, added "showaliases", fixed alias parsing, removed the requirement to provide command to remove alias, and implementation of user-made commands in mods"

This release is the final 2 releases to be released without any library dependencies.

1. Removed prompts for commands and arguments, resulting in the slight file-size reduction (0.0.4.12: 306KB -> 0.0.5: 292KB)
2. Added 2 color variables for storing help command list and definitions
3. Added Black to available colors
4. Setting colors are now permanent
5. Removed motd, chkn=1, preadduser and hostname arguments
6. Now the kernel creates a demo test account only if the "Create demo account" setting is set to True
7. Now the beep command prevents frequencies that is more than 2048 Hz
8. Now mods can have commands, and can have the dictionary modlist
9. The manageAlias() sub values are all required
10. Added command showaliases, although there is no help definition for it
11. Fixed the alias parser that when the command line contains one of the words in the alias list, the alias will get executed before the command.

### KS 0.0.5.1 (9/6/2018)

The changelog says "Follow-up release removed unused code, improved behavior of debugging logs, and improved readability of a debug message while probing mods with commands without definitions."

1. Changed the startup banner again
2. It parses real command-line arguments on startup
3. Improved behavior of debugging logs
4. Improved readability of a debug message while probing mods with commands without definitions.

### KS 0.0.5.2 (9/9/2018)

The changelog says "Made GPU probing on boot, removing "gpuprobe" argument, changed behavior of updating config"

1. Now, GPU will always be probed since hardware probing is important, resulting in a removal of the gpuprobe argument
2. Removed the GPUProbeFlag variable
3. Changed the behavior of updating config so double entry bug doesn't appear

### KS 0.0.5.5 (9/22/2018)

This version now uses the MadMilkman.Ini library that you can view in useddeps command.

The changelog said "Re-written config, Forbidden aliases, added missing help entries for "showaliases", added more MOTD and MAL placeholders, fixed repeating message of RAM status, and an FTP client has been added, finally!"

This version is also the final version not to work properly under Unix systems.

The shutdown and reboot command will no longer delay, but still makes beeping sound

1. All of the config are re-written, using the new library, MadMilkman.Ini .NET 4.0
2. Probing status for hardware is now deprecated
3. The repeating message of RAM status has been fixed.
4. Added more MOTD and MAL placeholders, resulting in user, shortdate, longdate, shorttime, longtime, timezone and summertimezone placeholders being added
5. Now when the integer overflow exception is thrown, the message is more descriptive.
6. Added the FTP client, and added ftp command
7. Moved pinging tools to NetworkTools.vb
8. Now aliasing forbidden commands are not allowed anymore
9. Added noaliases and useddeps commands
10. Added missing help entries for showaliases
11. Help entries for loadsaver and setsaver now lists available screensaver files

### KS 0.0.5.6 (10/12/2018)

The changelog says "Improved probers, username list on log-in, better compatibility with Unix"

1. Since Unix has different environment variables, we added a check for each code trying to access user's profile directory
2. Simplified the hardware probing process by making classes and lists for each hardware
3. Now it shows you the compile date in the title
4. It lists usernames on log-in
5. Moved the debug writer into TextWriterColor.vb
6. Pre-parse variables before writing
7. Updated config updaters (You can see what happened on the Config.vb on the source code of 0.0.5.6 and 0.0.5.5)
8. Take out support for all listing computer commands for Unix
9. Made reloadconfig read important config entry
10. Now initTimesInZones() is executed everytime showtdzone is executed, instead of updating every second
11. Now "help arginj" lists arguments
12. Now Unix will get timezone support

### KS 0.0.5.7 (10/13/2018)

The changelog says "Fixed crash when starting when running on a file name that is other than "Nitrocid KS.exe", Better error handling for FTP, Added current directory printing in FTP, removed "version" command, fixed the "Quiet Probe" value being set "Quiet Probe", Expanded "sysinfo", Fixed configuration reader not closing when exiting kernel, (Unix) Fixed a known bug"

1. Now KS can get current theme name
2. Fixed the known bug "Any error messages that is handled will crash the handler with a continuable kernel error."
3. Now it checks for Unix system on all the Sleep functions
4. Fixed the configuration file for Quiet Probe being set "Quiet Probe"
5. On FTP, commands can have their own aliases
6. Made the error messages on the FTP client more descriptive
7. Fixed the shell message appearing when you just press ENTER without typing commands (FTP Shell)
8. Extended the sysinfo command to show more information about the kernel
9. Removed the version command

### KS 0.0.5.8 (11/1/2018)

The changelog says "Removed beeping when rebooting and shutting down, Removed "beep" command, (Windows) Probers will now continue even if they failed, Disposing memory now no longer uses VB6 method of handling errors"

This release is the last release to support Windows XP and to be built on Visual Basic 2010 Express Edition. You can find more information about the transition to VS2017 and the usage of the alternative laptop to build.

We know very well that the laptops are slower than computers for something. If you need more information, go to section "The new development looks"

1. Now the hardware probing error messages are extended and more understandable
2. Now probers will show information anyway, even if hardware probing has failed
3. Changed the error handler in DisposeAll() from VB6 method to Try...Catch blocks
4. Removed the beep command
5. Now shutdown and reboot command doesn't beep
6. Added the usermanual command, but it's work in progress
7. Now only administrators can use reloadconfig and alias commands

## KS 0.0.6.0 beta versions

### KS 0.0.5.9 (12/24/2018)

The changelog says "Mods will now be stopped when shutting down, Mods can have their own name and their own version, fixed debugger not closing properly when rebooting or shutting down, Shell now no longer exit the application when an exception happens, Debugging now more sensitive (except for getting commands), Now debugger can debug better if the new line isn't going to be added after the end of the log, You are finally allowed to include spaces in your hostname as well as hostnames that is less than 4 characters like "joe", Deprecated useless and abusive commands, Removed extra checks for IANA time zones resulting in no more registry way, fixed listing networks, Added currency converter that uses the Internet (If we have added local values, we have to release more updates which is time-consuming, so the Internet way conserves time and updates), we have finally allowed users to view debug logs using KS with the debugging off, we have improved FTP client, for those who don't speak English can now set their own language although the default is English, fixed missing help entry for "lscomp", Added kernel manual pages, took out Windows XP support, and more..."

1. When probing GPU, if the prober can't get video RAM for the GPU, it will return 0
2. GPU memory variable is changed to UInt64
3. Added events and exceptions (great for mods)
4. Removed the initializeMainUsers sub because KS only makes a root account now.
5. If there is no user in the username list, kernel will crash
6. Now mods will be stopped on shutdown or reboot
7. Moved every kernel tool to KernelTools.vb
8. All strings can now be translated (Currently, you can only change language by changing config file or KS Config Tool)
9. Added manual pages
10. Now you can convert currencies (The Internet connection is required)
11. Now debug writer will not support writing without newlines
12. The garbage collector debugs when it's executed
13. Now everything will be debugged
14. Removed binary, passive, ssl, and text FTP commands
15. Added rename FTP command
16. Now FTP client uses the FluentFTP for easier management
17. Now when you upload or download files, you will see the progress bar
18. Now everything will be transferred in appropriate modes
19. CWD injection is not required anymore
20. Some namespace calls are simplified
21. Added currency converter
22. Added missing entry for lscomp
23. Now aliases are shown through help command, removing showaliases command
24. No more stack overflows when typing the large number of commands that exceeds the max stack amount
25. Now Time/Date on the corner will be shown correctly on resized consoles
26. Removed the registry way on TimeZones
27. Now NuGet is used to manage packages (libraries)

### KS 0.0.5.10 (2/16/2019)

1. Improved readability of manual pages (vbTab is now filtered and will not cause issues)
2. Now the translator prints debug info when a string is not found in the translation list
3. Hardware Prober: Stop spamming "System.__ComObject" to debugger to allow easy reading
4. Manpage Parser: Stop filling debug logs with useless "Checking for..." texts and expanded few debug messages
5. Fixed the BIOS string not showing
6. Removed unnecessary sleep platform checks
7. Removed "nohwprobe" kernel argument as hardware probing is important
8. Removed unnecessary timezone platform checks
9. Updated FluentFTP and Newtonsoft.Json libraries to their latest stable versions (Yes, this is why we're using NuGet! NuGet is better than manually updating the external libraries.)
10. No stack duplicates except the password part in Login
11. Fixed bug of MAL and MOTD not refreshing between logins
12. Fixed bug of sysinfo (the message settings not printing)
13. The kernel now prints environment used on boot, debug, and on sysinfo command
14. Made writing events obsolete

### KS 0.0.5.11 (2/18/2019)

This version was released as the hotfix for 0.0.5.10, removing more obsolete things and slimming before 0.0.6. However, this version's slimming process has removed the status probing for RAM and for HDD.

1. Made GPU and BIOS probing `<Obsolete>`
2. No more COM calls when probing hardware
3. Removed a useless file that has hard drive data
4. Fixed the translation of sysinfo when displaying the kernel configuration section
5. Removed status probing from HDD and RAM
6. Fixed the CHS section not appearing if the hard drive has the Manufacturer value
7. Fixed the translator not returning English value if the translation list doesn't contain such value
8. Fixed the GPU prober assuming that Microsoft Basic Display Driver is not a basic driver
9. Made screensavers be probed on boot
10. Fixed NullReferenceException when trying to load the next screensaver after an error occured on the previous screensaver
11. Fixed the OS info not translated when starting up a kernel
12. Fixed language config not preserving when updating
13. Debug information now prints to VS2017 debug output window (You still have to turn on debugging)
14. Made the loadsaver command reloadsaver
15. Removed useless and abusive commands (echo, panicsim and choice)

### KS 0.0.5.12 (2/22/2019)

This version was released to fix more bugs and do more improvements, including the fix for Environment.OS for Windows 10. This release is a 1-year anniversary for KS

1. Now createConf cmdline arg only creates config if the config file isn't found
2. Some preparations for 0.0.6 (slimming down only)
3. Removed the GPU and BIOS probing
4. Now older KS config won't be allowed to be updated here (Workaround: You need to remove your old KS config file and re-run the app)
5. Fixed the Environment.OS bug on Windows 10 (10.0) where it returns Windows 8 (6.2) version
6. Fixed the placeholders not parsing when using showmotd/showmal command
7. Fixed the simple help not showing mods
8. Fixed built-in commands not running after you run mod commands or alias commands
9. Fixed NullReferenceException when debugging
10. Improved alias listing
11. Fixed the printing text exception message not translating to current language
12. Fixed the "/" or "\" appearing before the modname when probing mods and screensavers
13. Removed unnecessary fixup in translation
14. Fixed more stack overflows in FTP shell
15. Fixed the FTP message translation translating "'help'" as the language when it's supposed to be a command
16. Fixed the command not found message when not entering anything in FTP shell

### KS 0.0.5.13 (4/14/2019)

We have simplified even more things, with the changes below:

1. More slimming by JetBrains ReSharper for VS2017
2. Implemented Linux hardware probing (You need to install inxi for HDD probes to work. Instructions in the "Linux probing" section of this manual.)
3. Increased .NET requirement to 4.7 (Every user need to have 4.7 instead of 4.6 installed.)
4. Removed warning about binding redirects in MonoDevelop (MonoDevelop is unaware...)
5. Increased VS version requirement to VS2019 (To test features in it.)
6. Removed annoying "Naming rule violation" by using suggested option

### KS 0.0.5.14 (6/13/2019)

After we got a new PC, the update has come. It has the following changes:

1. Replaced fake file system with real one (access to your files)
2. Fixed the wrong "changedir" help command being shown instead of "chdir"
3. "cdir," which shows the current directory, is now obsolete
4. Fixed crash while rebooting the kernel

## KS 0.0.6.x series

### KS 0.0.6.0 (6/19/2019)

This update is as big as 0.0.4, but we have released this update to fix more problems. It is a service pack for 0.0.5.5 with the following changes:

1. New icon
2. Updated FluentFTP and Newtonsoft.Json libs
3. Removed writing events
4. Re-written login (Not all, but re-designed)
5. Fixed the chpwd command not changing password if the target doesn't have password
6. Fixed chpwd not checking if a normal user changes admin password
7. Fixed adduser not adding users without passwords
8. Fixed adduser adding users with passwords even if they don't match
9. Removed cdir
10. Added config entry for screensaver name
11. Implemented debugging and dump files for kernel errors
12. Shipped with .pdb debugging symbols for KS
13. Fixed reboot not clearing screen
14. Added Dutch, Finnish, Italian, Malay, Swedish and Turkey languages (switch to a compatible font in console)
15. Countries and currencies are now listed when not providing enough arguments or issuing "help currency"
16. Fixed help list not updating for new language update when rebooting
17. Added permanent aliases (located under your profile, aliases.csv)
18. The password is now hidden when logging in to maintain security
19. Fixed users being removed after each reboot

### KS 0.0.6.1 (6/21/2019)

1. Removed currency information showing on help (will bring it back later)
2. Users are now required to enter their API Key from apilayer.net to convert currencies (Basic plan, get at http://currencylayer.com/product, untested: couldn't pay for basic plan)

### KS 0.0.6.2 (6/24/2019)

1. Fixed debug log show command not working because the path was not found (typo: Debugger -> Debugging)
2. Added a notice in listing PC commands about latest versions of Windows 10
3. Fixed debug kernel header not writing when run with debug argument on
4. Fixed the debug log being empty every reboot and start
5. Allowed clearing debug log in command using cdbglog
6. Used built-in FtpVerify enumerators, removing our hash check for older versions of FluentFTP
7. Better debugging experience
8. Debugging now shows line number and source file if pdb is on the same folder
9. Allowed modding using C#

### KS 0.0.6.2a (6/24/2019)

This is the smallest update ever, although contains one important thing. The assembly version is 0.0.6.2, but this is the newer version, because Visual Studio still couldn't allow us to use letters on versions.

1. Added checking for processor instructions (currently used in kernel booting to see if SSE2 is supported)

### KS 0.0.6.2b (6/25/2019)

1. Added a command named `sses` to list all SSE versions

### KS 0.0.6.3 (6/26/2019)

1. Fixed `quiet` not being entirely quiet
2. Fixed messages not appearing after signing in (ex. Adding user message)
3. Allowed changing language using command
4. Fixed the help text showing after executing `sses`
5. Added Czech language

### KS 0.0.6.4 (6/28/2019)

1. Fixed NullReferenceException when changing language
2. Fixed massive documentation newlines when trying to parse an empty word that is not on the beginning (Please note that we still have newline issues in the first line)
3. Added Ubuntu Theme
4. Removed unused flag
5. Removed extra requirement to parse colors on boot (removed greed)
6. Made reading FTP file size human-readable

### KS 0.0.6.4a (7/6/2019)

1. Fixed Linux hardware probing failing even if succeeded
2. Fixed RAM prober showing MemTotal: prefix
3. Made message about libcpanel-json-xs-perl clear

### KS 0.0.6.4b (7/7/2019)

1. Made one preparation for 0.0.6.5: Downloading debug symbols on startup if not found

### KS 0.0.6.5 (7/25/2019)

1. Fixed dump files being created without extension
2. Localized dumps and manual pages
3. Upgraded language version to the latest
4. Fixed some bugs about filesystem
5. Fixed CPU clock speed showing up twice in latest processors (processors that have clock speed on their internal names, for ex. "Intel(R) Core(TM) i7-8700 CPU @ 3.20GHz")
6. Fixed progress bar of FTP transfers so it uses new format
7. Added ETA and speed

### KS 0.0.6.6 (7/26/2019)

1. Updated manual page for new commands
2. Removed currency command (unpaid)

## KS 0.0.7.0 beta versions

### KS 0.0.6.9 (7/27/2019)

1. Removed calculators and unit converters
2. Unified two printing commands into one
3. Unified two mod generators into one
4. Allowed entering FTP server without specifying "ftp://" prefix
5. Allowed specifying address as the "ftp" command argument
6. Now the FTP client will disconnect peacefully when exiting
7. Fixed FTP help descriptions not updating when changing languages
8. SSE checking by command is now supported on Unix systems
9. Fixed corner time and date position
10. Fixed password not working correctly even if the user put the correct password
11. Removed unused phase
12. Added debug on each phase
13. Added operating system placeholder for use with MOTD and MAL
14. Added newline parser to make MOTD support more than 1 line

### KS 0.0.6.10 (8/8/2019)

1. Simplified namespace to KS
2. Fixed codeblocks for Hindi, Chinese, and 1/2 Czech (See comment on GetCommand.vb)
3. Added missing help entry for "reloadsaver"
4. Added "reloadmods" command
5. Made KernelVersion and EnvironmentOSType read-only
6. "promptArgs" cmdline argument removed
7. Extra stack now not generated when rebooting

### KS 0.0.6.11 (8/10/2019)

1. Fixed debug showing password in clear text
2. Showed changelogs during update
3. Fixed KeyNotFoundException after updating config on startup

### KS 0.0.6.12 (8/11/2019)

1. More codeblock corrections for Czech, Croatian, Dutch, Finnish, French, German, Italian, Malay, Portuguese, Spanish, Swedish, and Turkish manual pages. 

NOTE: This version is a result of useless modifications to codeblocks that Google has to make so it feels ugly, translated, uncompilable, and misaligned.

### KS 0.0.6.13 (8/13/2019, 0.0.6.13N: 8/16/2019)

1. Improved Time and Date probers (Now two fields, one DateTime, one String, are made into one)
2. MOTD and MAL parsing using files to better support newlines
3. Fixed `chmal` and `chmotd` only taking one word
4. Fixed casting issues on kernel error
5. Removed new line placeholder
6. Removed MAL and MOTD config entries
7. Now builds for both Chocolatey Gallery and NuGet in "N" edition.
8. Fixed NullReferenceException when reading old KS config files by upgrading it to a new format

### KS 0.0.6.14 (8/25/2019)

1. Deprecated the usermanual command by custom message
2. Made text sections for MOTD and MAL in sysinfo

## KS 0.0.7.x series

### KS 0.0.7.0 (8/30/2019)

This release is not as big as all the major releases.

1. Removed manual code and moved all the English docs to Wiki (reducing size to its initial size before manpages were released)
2. Removed changelog viewing on config upgrade
3. Removed pinging and listing computers in the network
4. Added support for FTPS
5. Made use of Filesystem.List instead of its own listing in FTP
6. Fixed the list command not supporting directories that have spaces

### KS 0.0.7.1 (8/31/2019)

This release is the feature pack to the 0.0.7 series. A version like 0.0.7.11, 0.0.7.12, or 0.0.7.13 is a bug fix update.

1. Updated FluentFTP
2. Created `get` command to download something from the Internet
3. Removed useddeps as the devs are already credited in this README.md (no need to credit them for second time)
4. Config now always creates with the string representation of the colors
5. Added Indonesian, Polish, Romanian, and Uzbekistan language
6. Implemented remote debugging support

### KS 0.0.7.11 (9/3/2019)

1. Added handler for repeated alias addition
2. Now `arginj` checks for arguments before putting them to the answer field
3. `cdbglog` now shows a message when it finished or failed
4. Added `chdir` error handler and support for spaced folder names
5. Added `chpwd` user not found error handler
6. `get` will disallow all addresses starting with a space
7. `md` now can create directories that have spaces
8. `netinfo` is tidier
9. `rd` has an error handler about directories that didn't exist
10. Fixed `setcolors` not defaulting one of the colors or resetting them
11. Remnants of showmotd, showmal, and showaliases are removed
12. Added required arguments into `showtdzone`'s help entry
13. `showtdzone` can now show time in a specific zone

### KS 0.0.7.12 (9/5/2019)

1. Made debug port and download retry count customizable
2. Fixed `get` not downloading anything containing arguments

### KS 0.0.7.13 (9/15/2019)

Chat system for remote debugger is added, but rather unstable, because it is "taking turns".

1. Improved the quiet system so it no longer uses the old-fashioned flag system
2. Fixed the `NotEnoughArgumentsException` when the arguments specified were invalid
3. Added the built-in chat in **networked** debugger console (Not stable, Version 0.1)

### KS 0.0.7.14 (9/21/2019)

This release fixes the chat system "taking turns", so the chat is upgraded to version 0.1.1. Commands for chat might come soon in future releases.

### KS 0.0.7.2 (9/23/2019)

1. Updated FluentFTP
2. Added YellowFG and YellowBG themes
3. Fixed part of the shell prompt color on yellow light/dark backgrounds
4. Fixed time/date corner position overlapping existing text
5. Now time zone offsets are shown in each time zone view
6. Added Japanese language

### KS 0.0.7.21 (9/29/2019)

1. Added a message when specifying non-existent time zone in `showtdzone`
2. Fixed Japanese language missing latest locale additions
3. Added missing argument requirements in the help entry for `showtdzone`
4. Fixed FTP connection not prompting for profile selection (apparently, it's not written yet, but it's now written.)

### KS 0.0.7.3 (10/4/2019)

1. Updated NuGet.Build.Tasks.Pack to version 5.3.0
2. Fixed empty address being accepted in FTP connect command
3. Fixed NullReferenceException when handling an error from socket connection that isn't a socket problem
4. Added basic command support for debugger (No argument system yet, only stack trace show, help, and exit)
5. Added Arabic transliteration (all English letters)

### KS 0.0.7.4 (10/18/2019)

1. Updated FluentFTP
2. Fixed license not showing in NuGet.org
3. Moved from the deprecated PackageIconUrl to PackageIcon
4. Added unit test shell (doesn't cover all functions currently, variables treated as texts)
5. Added debug quota so the debugging logs aren't huge
6. Fixed debugger not flushing properly to the file after using `cdbglog` command

### KS 0.0.7.41 (10/19/2019)

1. Recent tests concluded that the FTP progress bar is now fixed (No duplication)
2. Fixed the purple stain in progress bar writing
3. The ETA for FTP file transfer is now more clear

### KS 0.0.7.5 (10/24/2019)

1. Added a new debugging command named `username` that shows current username
2. Fixed stack trace history not updating when there's an error in accepting new connections
3. Remote debug shell and the test shell now complain when the command is not found
4. Added argument support to the debug command
5. Stack traces are stored in a list and can be viewed in the remote debugger command `trace`

### KS 0.0.7.6 (10/29/2019)

1. Added the experimental naming system for chat in remote debugger (Custom names not implemented yet)
2. Added `lines` and `glitterMatrix` screensaver
3. Now screensavers have their own debugging messages
4. FluentFTP debugger messages are now redirected to the KS debugger
5. Now filesystem actions are debugged
6. Now `get` doesn't run if the URL is not specified
7. Added missing `get` help entry

### KS 0.0.7.61 (10/30/2019)

1. Fixed mod reload help description not translating
2. Fixed Google's weirdness about reloadsaver help description on several language files
3. Fixed KS crashing on startup if the mods are inserted
4. Now cursor won't show up if the custom screensaver runs
5. Added transliterated Russian language

# First-generation, Revision 1

## KS 0.0.8.x series

### KS 0.0.8.0, a big release (2/22/2020)

After 4 development months, we are so excited to announce that this version is released as a stable version found in Releases. A full binary (with LibVLC Windows dependencies) and a stripped-down binary (isolated, without LibVLC Windows dependencies) will be provided starting from this release.

1. Added new commands
2. Added new languages
3. Added screensavers
4. Added stage counter
5. Added eyecandy on startup
6. Removed `sses` and `noaliases` commands
7. Added 255 color support (truecolor will be done on 0.0.9)
8. Added beep synth
9. Added RPC and SSH
10. Now codepages will change accordingly when setting languages
11. Listing partitions and drives now added
12. Added new placeholders
13. Added global IPv6 and IPv4 properties
14. Updated libraries
15. Debug will have their error levels (commits 362d004f and 06d3fefb)
16. Targeted .NET Framework 4.8
17. Restored calculator
18. Fixed copy and move commands working incorrectly
19. Bug and crash fixes

There are many more to discover.

### KS 0.0.8.1 (3/16/2020)

This release is an update to 0.0.8 that fixes bugs, and clarifies the help command usage of select commands that have arguments.

1. Removed remaining `sses` command
2. Updated FluentFTP and LibVLCSharp libraries
3. Now, the `ftp` command no longer reports an error if you have appended `ftp://` or `ftps://` before the IP address or hostname, for ex. `ftp://ftp.us.debian.org`
4. Added `rexec` arguments to command help
5. Updated `setcolors` and `setsaver` help commands to their latest versions

### KS 0.0.8.5 (3/22/2020)

1. Updated libraries
2. Removed filesystem structure parsing
3. Permanent list of usernames and passwords
4. Now, "Command defined by" part will update everytime a user wants to change language
5. Fixed CPU usage being high at all times; the Notifications will be listened to every 10 milliseconds instead of instantly
6. Added 24-bit true color testing test command. Use `testtruecolor <R;G;B>` to test, implying that R, G, and B isn't greater than 255 or less than 0.
7. You can see passwords as stars
8. Added Bengali and Punjabi languages
9. Added `args` cmdline argument in case config can't be used
10. Made `search` show line number on every match
11. Added forgotten `search` help command.
12. Added SHA1 algorithm to `sumfile` and test shell
13. Added `sumfiles` command

### KS 0.0.8.6 (7/14/2021)

1. Removed `speak` command
2. Backported improvements to login and screensaver from 0.0.16.0

### KS 0.0.8.7 (8/22/2021)

1. Backported improvements

### KS 0.0.8.8 (8/24/2021)

1. Fixed parsing user line created in later versions of KS (<=0.0.15)
2. Improved the output of list command
3. Reading contents now not blocking

### KS 0.0.8.9 (2/8/2022)

1. Updated the kernel update check for second-gen KS

### KS 0.0.8.10 (3/3/2022)

1. Backported a fix from 0.0.20.1

### KS 0.0.8.11 (4/5/2022)

1. Updated the debug symbol downloader to point to new link

### KS 0.0.8.12 (5/11/2022)

1. Backported improvements from 0.0.21.0

### KS 0.0.8.13 (6/10/2022)

1. Backported fixes from 0.0.22.0

### KS 0.0.18.14 (7/9/2022)

1. Backported fixes

## KS 0.0.9.x series

### KS 0.0.9.0 (4/23/2020)

This release is probably the smallest major release because we have time constraints and the volume of work. We promise that the next major release, 0.0.10, will bring many features, bug fixes, and more.

1. Updated libraries
2. Added IMAP Shell
3. Fixed kernel crash if remote debugger failed to start
4. Added Slovak language
5. Added comment for `Languages` in config

### KS 0.0.9.1 (5/6/2020)

1. Updated FluentFTP
2. Fixed `list` crashing if no page number is specified

## KS 0.0.10.x series

### KS 0.0.10.0 (5/19/2020)

1. Updated libraries
2. Fixed mods not being able to access the Encryption class
3. Simplified encryption code for mods
4. Improved list of filesystems
5. Fixed FTP crash when faced with multiple profiles
6. Now FTP addresses starting with `ftpes://` will also be accepted
7. Made FTP profile listing more clear
8. Now the IMAP client will tell you if there's an attachment when reading
9. Fixed misleading message when reading the first message
10. Added scripting
11. Fixed IMAP shell crash when idle
12. Now error messages have their own color
13. Restored `echo`, `beep`, and `choice` commands from 0.0.1 for script implementation
14. Added `input` command
15. Now IMAP client shows newest messages first
16. Fixed unexpected kernel crash when the IMAP shell crashed
17. Added debugging messages in IMAP shell
18. Added other folder support to IMAP client
19. Added code documentation for modders and devs using Visual Studio
20. Added usage commands support for FTP
21. Added new events for mods

### KS 0.0.10.1 (7/8/2020)

1. Fixed OutOfMemory exception hashing large files

### KS 0.0.10.2 (4/14/2021)

1. Fixed NullReferenceException when trying to get symlink target (backported)

## KS 0.0.11.x series

### KS 0.0.11.0 (7/25/2020)

According to the changelogs that README.md provides, this is what is changed:

1. Added command cancellation when CTRL+C is pressed implementation
2. Added `shutdown` command to test shell
3. Made `exit` exit the test shell and start the kernel
4. Implemented Speed Dial for FTP shell for faster connection
5. Fixed FTPS using wrong encryption type
6. Added simple send message to mail client (IMAP shell changed to mail shell)
7. Fixed port parsing in mail shell
8. Made screensaver timeouts customizable
9. Added `mkfile` and `edit` commands
10. Added basic text editor
11. Removed `speak` command and `soundtest` test command due to instabilities on sound library
12. Removed demo account creation
13. Permission management for mods is now easier to use
14. Fixed `read` command not working with spaced filenames
15. Fixed invalid `Not enough arguments` message when providing more than enough arguments in some commands
16. Fixed `chusrname` not changing username permanently
17. Now in mail shell when you don't provide enough arguments it will automatically open a help page for the specified command
18. Now remote debugging user messages reflects the current kernel language used
19. Added temporary blocking/unblocking IP addresses for remote debugger (will be permanent in 0.0.12)
20. Fixed notifications showing when dismissing them
21. Added Serbian language
22. Added console output redirection support for main shell commands

### KS 0.0.11.1 (8/6/2020)

We can't support terrorism, so we've removed Turkish language as part of the changes.

1. Fixed messup on console output redirection
2. Added notification when config fails to be read

### KS 0.0.11.2 (7/4/2021)

1. Fixed NullReferenceException when trying to get symlink target (Backported)

# First-generation, Revision 2

## KS 0.0.12.x series

### KS 0.0.12.0, a huge release (11/6/2020)

This release is a huge release, so what's new is written below:

1. Updated libraries
2. Added built-in configuration tool
3. Added permanent permission support
4. Removed CI command-line args
5. Added settings support for screensavers
6. Improved script execution logic
7. Now users can keep connected to their e-mail upon exiting the mail shell
8. Fixed invalid `Argument not provided` on list and send commands
9. Now `listpart` can take drive numbers and not indexes
10. Date and time are now localizable
11. Terminal title now changes accordingly when executing commands
12. Fixed not being able to access files and folders outside current drives
13. Added bouncing text and dissolve screensaver
14. Made more API-friendly
15. Reimplemented unit tests (for Visual Studio devs)
16. Added append mode to output command redirection
17. Removed `read` command
18. Fixed some bugs, including the inability to adapt root account to `users.csv` on cold startup
19. Improved compatibility with mods
20. Now kernel can reset itself to factory settings by running KS with `reset` command line argument
21. Added ability to move mails
22. Added Catalan, Somali, Azerbaijani, Maltese, and Filipino languages
23. Added weather command that allows you to see the current state of weather in specified city. Uses OpenWeatherMap
24. Added mod support for FTP shell, text shell, and mail shell
25. Now events can be adapted with arguments
26. Now mods can cooperate with each other.
27. Some improvements
28. Mail shell will send notification when new mails arrive (currently only on inbox)
29. Hard drive partitions will be shown on boot on Windows systems
30. Added file deletion
31. Added file attribute setting
32. Improved kernel update logic
33. Added "replaceinline" text editor command
34. The user can optionally write the line number in `print` to print specified line number in the text document.
35. Implemented autosave for text editor shell
36. Added console writers for Truecolor. It allows mods and screensavers to use even more colors. A VT-compatible terminal is needed.
37. Allows you to specify multiple files and dirs in `rm`, `list`, `fileinfo`, and `dirinfo` commands
38. Added permanent blocked devices list
39. Added color cycling setting to Disco
40. Added attachments support for sending mail
41. Added dynamic mail address detection (by dictionary)
42. Added mail encryption/decryption support
43. Fixed downloads never starting after previous completed/failed download session
44. Fixed kernel panic on kernel config update
45. Added `put` command that does the exact opposite of `get` command
46. Restored the `ping` command that got removed some versions back
47. Fixed mail not being able to connect once you disconnect
48. Added Debian, NFSHP-Cop, and NFSHP-Racer color themes
49. Notifications are now suppressed on screensavers
50. chmotd and chmal now open text editor shell when no arguments were provided.
51. Partially fixed timed screensaver being messed up when trying to exit it.
52. Time/date in corner now respects culture settings
53. Credentials can be provided to `get` and `put` commands
54. Fixed nasty bug which lets you change `chpwd` when you're not an administrator.
55. Fixed high CPU usage when remote debugger is on
56. Added new commands

### KS 0.0.12.1 (11/22/2020)

1. Fixed variables not being parsed in translated versions of Punjabi messages
2. Fixed some strings not being translated
3. Fixed SSH connection error even after successful connection
4. Fixed high CPU usage when connected to an SSH server

### KS 0.0.12.2 (11/29/2020)

1. Reverted to SSH.NET version 2016.1.0 as it fixes NuGet installation error under normal circumstances. For more info, consult "The truth of SSH.NET 2020.0.0 Beta 1"
2. Bug fixes and general improvements
3. Fixed shell error trying to use multi commands in some cases
4. Fixed multi command colon not working for mods and aliases
5. Fixed injected commands failing with a shell error
6. Fixed newline on input line in cmdinject argument
7. Added help page for real cmdline args for KS

### KS 0.0.12.3 (12/8/2020)

1. Fixed artifacts in Swahili
2. Fixed inconsistencies regarding rexec command
3. Fixed RPC commands not listening

### KS 0.0.12.4 (8/19/2021)

1. Backported fixes from 0.0.16.0 and 0.0.19.0

### KS 0.0.12.5 (2/5/2022)

1. Updated the kernel update check for second-gen KS

### KS 0.0.12.6 (3/3/2022)

1. Backported a fix from 0.0.20.1

### KS 0.0.12.7 (4/5/2022)

1. Updated the debug symbol downloader to point to new link

### KS 0.0.12.8 (5/11/2022)

1. Backported improvements from 0.0.21.0

### KS 0.0.12.9 (6/10/2022)

1. Backported fixes from 0.0.22.0

### KS 0.0.12.10 (7/9/2022)

1. Backported fixes

### KS 0.0.12.11 (8/5/2022)

1. Backported fixes

## KS 0.0.13.x series

### KS 0.0.13.0 (12/20/2020)

1. Added SHA512 encryption algorithm
2. Added SFTP
3. Fixed console formats in FTP and SFTP
4. Added new string placeholder for host name
5. Added custom shell prompt style
6. Fixed password field being accessible to mods

### KS 0.0.13.1 (12/24/2020)

1. Added current directory placeholder

### KS 0.0.13.2 (8/19/2021)

1. Backported fixes from 0.0.16.0 and 0.0.19.0

## KS 0.0.14.x series

### KS 0.0.14.0 (1/21/2021)

1. Added executable file running
2. Fixed NullReferenceException when parsing Nothing in all printing commands in TextWriterColor
3. Updated libraries
4. Used Inxi.NET for hardware probing
5. Added mail message preview implementation
6. Fixed inconsistencies in network section of settings command
7. Removed `listdrives` and `listparts` in favor of `sysinfo` showing hard drive information
8. Added color change to Settings, removing `setcolors`
9. Added a way to get the target link in SFTP directory listing
10. Fixed NullReferenceException when trying to change remote SFTP directory
11. Fixed local SFTP directory not changing
12. Added custom shell input prompt support
13. Removed Slot Probe in favor of rewritten hardware probe
14. Added Bouncing Block Screensaver

### KS 0.0.14.1 (1/29/2021)

1. Fixed wrong UESH message if command is not found
2. DoCalc is obsolete
3. Now bouncing block settings are customizable

### KS 0.0.14.2 (2/1/2021)

1. Added mitigation for Windows 10 NTFS corruption and BSOD

### KS 0.0.14.3 (8/29/2021)

1. Backported fixes from 0.0.19.0

## KS 0.0.15.x series

### KS 0.0.15.0 (2/22/2021)

1. Added BedOS, 3Y-Diamond, and TealerOS colors
2. Allowed specifying city name in weather command
3. Made `calc` use string evaluator
4. Fixed NullReferenceException when running maintenance mode
5. Added Irish and Welsh languages
6. Now time offsets can display a plus sign
7. General improvements
8. Added new text edit commands, including `querychar`
9. Fixed new instances of text editor not "opening" files after closing the first session
10. Added progress implementation to notification system
11. Fixed console output messup in certain situations
12. Users are free to turn on/off text edit autosave and set interval
13. Added text changed indicator
14. Added conflict detection to mod parser
15. Now you're free to choose to record remote debug chat logs or not
16. Fixed crash on startup in Linux
17. Ping continues even if one device can't be pinged
18. Screensaver timeouts are now customizable except for AptErrorSim, HackUserFromAD, and Dissolve

### KS 0.0.15.1 (2/23/2021)

1. Fixed unlocalized strings in 0.0.15.0
2. Added missing string for SSH FTP help entry

### KS 0.0.15.2 (3/4/2021)

1. Use vt100 terminal (nano is still flaky)
2. Fixed crash on next reboot when hardware probing fails
3. Implemented percentage indicator for notification

### KS 0.0.15.3 (3/9/2021)

1. Added category indicator to settings
2. Improved sysinfo viewing in languages other than English
3. Disabled cached hardware probing

### KS 0.0.15.4 (3/12/2021)

1. Removed listdrives and listparts help commands
2. Now help shows message when invalid command is entered

### KS 0.0.15.5 (3/12/2021)

1. General improvements
2. Fixed profile listing being offset by 1
3. Use ftp:// instead of ftps:// by default

### KS 0.0.15.6 (3/17/2021)

1. General improvements
2. Now profile answer takes more than one digit.

### KS 0.0.15.7 (3/21/2021)

1. Fixed percentage of notification not clearing
2. Added option to set current directory from settings

### KS 0.0.15.8 (4/14/2021)

1. Fixed NullReferenceException when trying to get symlink target

### KS 0.0.15.9 (8/29/2021)

1. Backported fixes from 0.0.19.0

# First-generation, Revision 3

## KS 0.0.16.x series

### KS 0.0.16.0 (6/12/2021)

This release is a huge release, so what's new is written below:

1. Added new screensavers
2. Dynamic themes support
3. Added number of times optional argument to ping
4. Added two color types (Warning and Option)
5. Added four new color themes
6. Added new commands
7. Restored GPU probing
8. General improvements and bug fixes
9. Added name support to remote debugger
10. Fixed some filesystem API functions not neutralizing path
11. Now the weather command lets you list cities
12. All configuration files migrated to JSON for easier access
13. Added speed dial support for SFTP
14. Fixed issues connecting to any FTP server using quickconnect
15. Fixed profile selection in speed dials that contain more than 9 profiles
16. Added fast string evaluation
17. User will be notified if there was an error downloading debug symbols for KS
18. Added new events
19. Added recently fired event list
20. Removed beep synth and debuglog commands
21. Added new console writers and other features to the API
22. Fixed various bugs
23. Added two new RPC commands
24. Added the Anonymous permission type
25. Fixed regression about adduser
26. Improved help command on remote debug
27. Added dll support for mods and screensavers
28. Added support for truecolor in KS
29. SSH banner will be shown
30. Added authentication to SSH by private key
31. Added new languages
32. Added first-user trigger OOBE
33. Now providing port is optional in sshell and sshcmd
34. Added changing port and enabling/disabling RPC
35. Added cancellation support for executable processes
36. Enhanced the test shell
37. And many more changes that will surprise you...

### KS 0.0.16.2 (6/12/2021)

1. Fixed NullReferenceException when loading a mod command if one of the loaded mods didn't have any command
2. Added support for `.message` MOTD for FTP
3. Fader and Wipe now exit instantly

### KS 0.0.16.3 (6/14/2021)

1. Fixed DLL-based screensavers not loading
2. Fixed external programs (ffmpeg, ...) not using current working directory

### KS 0.0.16.4 (6/18/2021)

1. Added support for spaces in commands for mods and aliases
2. Better mod conflict detection

### KS 0.0.16.5 (7/25/2021)

1. Added support for complete string formatting in console writers (Backported)
2. Enhanced a default for Typo (Backported)

### KS 0.0.16.6 (8/2/2021)

1. Backported a fix from version 0.0.18.0

### KS 0.0.16.7 (8/19/2021)

1. Backported fixes from 0.0.19.0

### KS 0.0.16.8 (2/5/2022)

1. Updated the kernel update check for second-gen KS

### KS 0.0.16.9 (2/28/2022)

1. Backported fixes from 0.0.20.0

### KS 0.0.16.10 (3/3/2022)

1. Backported a fix from 0.0.20.1

### KS 0.0.16.11 (3/4/2022)

1. Fixed crash on startup when trying to probe hardware

### KS 0.0.16.12 (4/5/2022)

1. Updated the debug symbol downloader to point to new link

### KS 0.0.16.13 (5/11/2022)

1. Backported improvements from 0.0.21.0

### KS 0.0.16.14 (6/10/2022)

1. Backported fixes from 0.0.22.0

### KS 0.0.16.15 (7/9/2022)

1. Backported fixes

### KS 0.0.16.16 (8/5/2022)

1. Backported fixes

## KS 0.0.17.x series

### KS 0.0.17.0 (7/4/2021)

1. Fixed the sudden "Invalid path" when trying to execute a non-existent mod/alias that contains the space in their command.
2. Added FaderBack
3. Fade-ins in Fader and FaderBack are now smoother
4. WebAlerts are now handled in mail shell
5. Extended information in modinfo
6. Added custom prompt styles for RSS, Text Edit, and ZIP Shell
7. Improvements regarding parsing RSS feeds
8. General improvements and bug fixes

### KS 0.0.17.1 (7/17/2021)

1. Better handling for empty folders in List

### KS 0.0.17.2 (7/25/2021)

1. Added support for complete string formatting in console writers (Backported)
2. Enhanced a default for Typo

### KS 0.0.17.3 (8/2/2021)

1. Backported two fixes from version 0.0.18.0

### KS 0.0.17.4 (8/25/2021)

1. Backported fixes from 0.0.19.0

### KS 0.0.17.5 (2/5/2022)

1. Updated the kernel update check for second-gen KS

### KS 0.0.17.6 (4/5/2022)

1. Updated the debug symbol downloader to point to new link

### KS 0.0.17.7 (5/11/2022)

1. Backported improvements from 0.0.21.0

### KS 0.0.17.8 (6/10/2022)

1. Backported fixes from 0.0.22.0

## KS 0.0.18.x series

### KS 0.0.18.0 (8/2/2021)

1. Added SHA384 support
2. Added support for output files in `sumfile`
3. Now `No help for command <cmd>` shows up on all shells
4. Added support for complete string formatting in console writers
5. Added new commands
6. Flipped the date and time on upper right corner
7. Added the GetConfigCategory and SetConfigValueAndWrite APIs
8. Added beats per minute implementation to disco
9. Added BeatFader screensaver
10. Added new placeholders
11. Custom startup banner support
12. Added `/clear` to clear the string variable in Settings
13. General improvements and bug fixes

### KS 0.0.18.1 (8/9/2021)

1. Fixed BeatFader taking time to exit
2. Fixed extra newline in separator if location is two lines lower

### KS 0.0.18.2 (8/24/2021)

1. Backported fixes from 0.0.19.0

### KS 0.0.18.3 (2/5/2022)

1. Updated the kernel update check for second-gen KS

### KS 0.0.18.4 (4/5/2022)

1. Updated the debug symbol downloader to point to new link

### KS 0.0.18.5 (5/11/2022)

1. Backported improvements from 0.0.21.0

### KS 0.0.18.6 (6/10/2022)

1. Backported fixes from 0.0.22.0

### KS 0.0.18.7 (7/9/2022)

1. Backported fixes

## KS 0.0.19.x series

### KS 0.0.19.0 (8/24/2021)

1. Added folder download/upload to FTP
2. Added option to write full color code in ColorWheel
3. Added JSON minifier
4. Added optional output support for jsonbeautify/jsonminify
5. Added the Kazakh and Yoruba languages
6. Added the Linotypo and Typewriter screensavers
7. Improvements in Settings
8. Added reading text files support in supported screensavers (only Linotypo for now)
9. Added the mod manager
10. Added missed character support in typo screensavers
11. Added debugging support to screensaver
12. Restored Turkish language
13. Fixed cancellation not working properly in some situations
14. General improvements and bug fixes

### KS 0.0.19.1 (8/26/2021)

1. Made a shortcut to KS for Windows systems (Chocolatey)
2. Added the FlashColor screensaver
3. Added the Gujarati language

### KS 0.0.19.2 (2/5/2022)

1. Updated the kernel update check for second-gen KS

### KS 0.0.19.3 (3/3/2022)

1. Backported a fix from 0.0.20.1

### KS 0.0.19.4 (4/5/2022)

1. Updated the debug symbol downloader to point to new link

### KS 0.0.19.5 (5/11/2022)

1. Backported improvements from 0.0.21.0

### KS 0.0.19.6 (6/10/2022)

1. Backported fixes from 0.0.22.0

### KS 0.0.19.7 (7/9/2022)

1. Backported fixes

### KS 0.0.19.8 (8/5/2022)

1. Backported fixes
