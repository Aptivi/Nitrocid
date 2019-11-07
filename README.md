# |---+-> Kernel Simulator <-+---|

[![Build status](https://ci.appveyor.com/api/projects/status/9anm0jc0x9raoy8x/branch/master?svg=true)](https://ci.appveyor.com/project/EoflaOE/kernel-simulator/branch/master) [![Build Status](https://travis-ci.org/EoflaOE/Kernel-Simulator.svg?branch=master)](https://travis-ci.org/EoflaOE/Kernel-Simulator)

This kernel simulator simulates our **future** kernel that is planned by us and is not a final planned version of Kernel, since it was minimal.

## |-----+--> _Notes_ <--+-----|

- This kernel simulator _will_ continue to be developed, even if we made the real PC version of Kernel.

- It can only be Console at the moment, while we are developing a GUI for this simulator.

- We took out Windows XP support in 0.0.5.9 in favor of NuGet and .NET Framework 4.7.

## |-----+--> _Virus Warning_ <--+-----|

Unfortunately, some parts of KS have been taken to avoid detection in the source code of a malicious executable file called payslip.exe. It got its entries for the following cybersecurity analyzers:

* Hybrid Analysis (downloadable for researchers): https://www.hybrid-analysis.com/sample/756b94b872cada97c6ebcbc65c47734e3238f171db719d428a42f6ac8bc93e4f

* VirusTotal (downloadable for researchers): https://www.virustotal.com/gui/file/756b94b872cada97c6ebcbc65c47734e3238f171db719d428a42f6ac8bc93e4f/detection

* ANY.RUN ([downloadable for registered users](https://app.any.run/tasks/567f7789-cf49-4602-8a73-0459eb11be49/)): https://any.run/report/fab1bf683a31f5f5249fd685b9b75ce92bd28e3ad2abae10a07407de4d42ad78/567f7789-cf49-4602-8a73-0459eb11be49

For more information, inspect [this wiki page](https://github.com/EoflaOE/Kernel-Simulator/wiki/Studying-payslip-virus).

## |-----+--> _Important announcement_ <--+-----|

This is probably the last release of the 0.0.7.x series. We were bored for 2+ months, so we decided to put an end to this by making 0.0.8 in the next 4 months. That doesn't mean that KS is abandoned, but we will make commits for each feature addition, removal, improvement, fix, etc. instead of making the whole commit as the commit dedicated to each release. During 4 months, there will be no release. If you would like to see the aspects of 0.0.8, build from source. To see more, a page for it will come soon.

## |-----+--> _How to run/install_ <--+-----|

* If you are going to install KS without building from source, either look at the Releases, or use the second link below.

* If you are going to install KS as a NuGet library when modding or integrating, use the first link below.

* NuGet Package: https://www.nuget.org/packages/KS/

* Chocolatey: https://chocolatey.org/packages/KS/ (choco install ks, Run using "Kernel Simulator" on the CMD or on Run.)

## |-----+--> _Prerequisites_ <--+-----|

1. For Windows systems

- Windows 7 or higher

- Microsoft .NET Framework 4.7 or higher

2. For Unix systems

- Any Unix system that contains the latest version of [Mono Runtime](http://www.mono-project.com/docs/about-mono/languages/visualbasic/). MonoDevelop(32-bit or 64-bit)/JetBrains Rider(64-bit) is required to build from source.

## |-----+--> _Build Instructions_ <--+-----|

- For Windows systems

1. Install Microsoft Visual Studio 2017, or higher.

2. After installation, extract the source code, open Visual Studio, and click on **Open Project...**

3. Go to the source directory, and double-click the solution file

4. Right click on the project on the right, and select **Properties**

5. Go to **Compile**, click **Browse...** on **Build output path:**, and select your build path. When you're finished, click on **OK** button.

6. Click on the **Build** menu bar, and click on **Build Kernel Simulator**

7. In **Windows Explorer**, go to the build directory and then double-click on the executable file. 

Notice: You must have **at least** Visual Studio 2017, because of how the syntax is formatted inside the project file, as well as the NuGet properties inside.

- For Unix systems (MonoDevelop)

1. Install [Mono Runtime](http://www.mono-project.com/docs/about-mono/languages/visualbasic/), libmono-microsoft-visualbasic10.0-cil, and MonoDevelop.

2. After installation, extract the source code, open MonoDevelop, and click on **Open...**

3. Go to the source directory, and double-click the solution file
	
4. Change the output directory if you are building using **Release**.

5. Click on the **Build** menu bar, and click on build button to compile.

6. In **your file manager**, go to the build directory and then double-click on the executable file.

* For Unix Systems that can do 64-bit (JetBrains Rider)

1. Install [Mono Runtime](http://www.mono-project.com/docs/about-mono/languages/visualbasic/), Git, and libmono-microsoft-visualbasic10.0-cil. Remember to install Mono Runtime from the website, not your distro's repos. Mono 6.0.0 is at least required.

2. Install JetBrains Rider from their website or snap if you use Ubuntu 64-bit

3. After installation, open JetBrains Rider, and follow the configuration steps

4. When the main menu opens, choose "Check out from Version Control" and then "Git"

5. Write on the URL "https://github.com/EoflaOE/Kernel-Simulator.git" and press "Test" to verify your connectivity

6. Press Clone, and git will download 100+ MB of data (because of archive branch), then Rider will open up. Don't worry if the progress bar stops moving. It's based on the amount of objects, not the size, because Rider and/or Git still hasn't implemented progress bar by repo size yet.

7. You will get some errors about the inability to resolve My.Computer. Ignore these, as they won't interrupt the compilation.

8. Click on the hammer button to build, or the Run button. When the Edit configuration screen appears, tick the checkbox named "Use External Console".

9. If you used the hammer button, then open your file explorer, go to the build directory, and double-click on the executable file.

NOTE: We recommend running builds using the bug button to make breakpoints work. The run button is not like MonoDevelop or Visual Studio. Visual Studio is lighter than Rider, although it's only available for Windows.

## |-----+--> _History_ <--+-----|

Please note that dates mentioned here is for development date changes only. If you want to access the old versions, see `archive` branch.

**2/22/2018 - 0.0.1:** Initial release, normally, for Windows.

**3/16/2018 - 0.0.1.1:** Added "showmotd", changed a message and better checking for integer overflow on Beep Frequency.

**3/31/2018 - 0.0.2:** Code re-design, more commands, implemented basic Internet, argument system, changing password, and more changes.

**4/5/2018 - 0.0.2.1:** Fix bug for "Command Not Found" message, and added forgotten checking for root in "chhostname" and "chmotd".

**4/9/2018 - 0.0.2.2:** Fix bug for network list where double PC names show up on both listing ways, Error handling on listing networks.

**4/11/2018 - 0.0.2.3:** Fix crash on arguments after reboot, fix bugs, and more.

**4/30/2018 - 0.0.3:** Fix bugs, Log-in system rewritten, added commands, added arguments, added permission system, custom colors, and more.

**5/2/2018 - 0.0.3.1:** Shell title edited in preparation for the big release, fix bugs with removing users, fix blank command, and added admin checking.

**5/20/2018 - 0.0.4:** Change of startup text, customizable settings, Themes, Command-line arguments, Command argument and full parsing, Actual directory system (alpha), more commands, calculator, debugging with stack trace, debugging logs (unfinished), no RAM leak, fix bugs, and more.

**5/22/2018 - 0.0.4.1:** Fix bugs in changing directory, Fix bugs in "help chdir", added alias for changing directory named "cd", and config update.

**7/15/2018 - 0.0.4.5:** Fix bugs when any probers failed to probe hardware, added more details in probers, Help system improved, Fix bugs in color prompts, Prompts deprecated, and more.

**7/16/2018 - 0.0.4.6:** Removed extraneous "fed" that stands as the removed command in 0.0.4.5, Preparation for 0.0.5's custom substitutions.

**7/17/2018 - 0.0.4.7:** Better Error Handling for "ping" command, Fixed "unitconv" usage message

**7/21/2018 - 0.0.4.9:** Better Error Handling for "unitconv" command, Added temporary aliases (not final because there is no "showaliases" command), fix some bugs, added time zones ("showtdzone", and show current time zone in "showtd"), Added "alias", "chmal", and "showmal", Made MOTD after login customizable, Allowed special characters on passwords to ensure security, Made Kernel Simulator single-instance to avoid interferences, and more.

**8/1/2018 - 0.0.4.10:** Fused "sysinfo" with "lsdrivers", Improved Help definition (used dictionary for preparation for modding), added "lscomp" which can list all online and offline computers by names only, Added error handler for "lsnet" and "lsnettree", fixed grammatical mistakes in "lsnet" and "lsnettree", added mods (commands not implemented yet - <modname>.m), added screensavers, changed the behavior of showing MOTD, fixed bug where instance checking after reboot of the kernel would say that it has more than one instance and should close, and more.

**8/3/2018 - 0.0.4.11:** Removed pre-defined aliases, Fixed known bug that is mentioned.

**8/16/2018 - 0.0.4.12:** Replaced disco command with a screensaver. It seems like 0.0.5 will be released because it looks stable, but we have some remaining changes before the final release.

**9/4/2018 - 0.0.5.0:** Removed prompts, fixed MAL username probing, added "showaliases", fixed alias parsing, removed the requirement to provide command to remove alias, and implementation of user-made commands in mods

**9/6/2018 - 0.0.5.1:** Follow-up release removed unused code, improved behavior of debugging logs, and improved readability of a debug message while probing mods with commands without definitions.

**9/9/2018 - 0.0.5.2:** Made GPU probing on boot, removing "gpuprobe" argument, changed behavior of updating config

**9/22/2018 - 0.0.5.5:** Re-written config, Forbidden aliases, added missing help entries for "showalises", added more MOTD and MAL placeholders, fixed repeating message of RAM status, and an FTP client has been added, finally!

**10/12/2018 - 0.0.5.6:** Improved probers, username list on log-in, better compatibility with Unix

**10/13/2018 - 0.0.5.7:** Fixed crash when starting when running on a file name that is other than "Kernel Simulator.exe", Better error handling for FTP, Added current directory printing in FTP, removed "version" command, fixed the "Quiet Probe" value being set "Quiet Probe", Expanded "sysinfo", Fixed configuration reader not closing when exiting kernel, (Unix) Fixed a known bug

**11/1/2018 - 0.0.5.8:** Removed beeping when rebooting and shutting down, Removed "beep" command, (Windows) Probers will now continue even if they failed, Disposing memory now no longer uses VB6 method of handling errors

**12/24/2018 - 0.0.5.9:** Mods will now be stopped when shutting down, Mods can have their own name and their own version, fixed debugger not closing properly when rebooting or shutting down, Shell now no longer exit the application when an exception happens, Debugging now more sensitive (except for getting commands), Now debug writer doesn't support writing without new lines, You are finally allowed to include spaces in your hostname as well as hostnames that is less than 4 characters like "joe", Deprecated useless and abusive commands, Removed extra checks for IANA timezones resulting in no more registry way, fixed listing networks, Added currency converter that uses the Internet (If we have added local values, we have to release more updates which is time-consuming, so the Internet way conserves time and updates), we have finally allowed users to view debug logs using KS with the debugging off, we have improved FTP client, for those who don't speak English can now set their own language although the default is English, fixed missing help entry for "lscomp", Added kernel manual pages, took out Windows XP support, fixed NullReferenceException when there are errors trying to compile mods, added testMods cmdline argument, and more...

**2/16/2019 - 0.0.5.10:** Improved readability of manual pages (vbTab is now filtered and will not cause issues), Now the translator prints debug info when a string is not found in the translation list, Hardware Prober: Stop spamming "System.__ComObject" to debugger to allow easy reading, Manpage Parser: Stop filling debug logs with useless "Checking for..." texts and expanded few debug messages, Fixed the BIOS string not showing, Removed unnecessary sleep platform checks, Removed "nohwprobe" kernel argument as hardware probing is important, Removed unnecessary timezone platform checks, Updated FluentFTP and Newtonsoft.Json libraries to their latest stable versions, No stack duplicates except the password part in Login, Fixed bug of MAL and MOTD not refreshing between logins, Fixed bug of sysinfo (the message settings not printing), The kernel now prints environment used on boot, debug, and on sysinfo command, Made writing events obsolete

**2/18/2019 - 0.0.5.11:** Made GPU and BIOS probing `<Obsolete>`, No more COM calls when probing hardware, Removed a useless file that has hard drive data, Fixed the translation of sysinfo when displaying the kernel configuration section, Removed status probing from HDD and RAM (See why on `usermanual History of Kernel Simulator`, section `Truth about status probing`), Fixed the CHS section not appearing if the hard drive has the Manufacturer value, Fixed the translator not returning English value if the translation list doesn't contain such value, Fixed the GPU prober assuming that Microsoft Basic Display Driver is not a basic driver, Made screensavers be probed on boot, Fixed NullReferenceException when trying to load the next screensaver after an error occured on the previous screensaver, Fixed the OS info not translated when starting up a kernel, Fixed language config not preserving when updating, Debug information now prints to VS2017 debug output window (You still have to turn on debugging), Made the loadsaver command reloadsaver, Removed useless and abusive commands (echo, panicsim and choice)

**2/22/2019 - 0.0.5.12:** Now createConf cmdline arg only creates config if the config file isn't found, Some preparations for 0.0.6 (slimming down only), Removed the GPU and BIOS probing, Now older KS config won't be allowed to be updated here (Workaround: You need to remove your old KS config file and re-run the app), Fixed the Environment.OS bug on Windows 10 (10.0) where it returns Windows 8 (6.2) version, Fixed the placeholders not parsing when using showmotd/showmal command, Fixed the simple help not showing mods, Fixed built-in commands not running after you run mod commands or alias commands, Fixed NullReferenceException when debugging, Improved alias listing, Fixed the printing text exception message not translating to current language, Fixed the "/" or "\" appearing before the modname when probing mods and screensavers, Removed unnecessary fixup in translation, Fixed more stack overflows in FTP shell, Fixed the FTP message translation translating "'help'" as the language when it's supposed to be a command, Fixed the command not found message when not entering anything in FTP shell

**4/14/2019 - 0.0.5.13:** More slimming by JetBrains ReSharper for VS2017, Implemented Linux hardware probing (You need to install inxi for HDD probes to work), Increased .NET requirement to 4.7, Removed warning about binding redirects in MonoDevelop, Increased VS version requirement to VS2019, Removed annoying "Naming rule violation" by using suggested option

**6/13/2019 - 0.0.5.14:** Replaced fake file system with real one (access to your files), Fixed the wrong `changedir` help command being shown instead of `chdir`, `cdir,` which shows the current directory, is now obsolete, Fixed crash while rebooting the kernel

**6/19/2019 - 0.0.6:** New icon, Updated FluentFTP and Newtonsoft.Json libs, Removed writing events, Re-written login (Not all, but re-designed), Fixed the chpwd command not changing password if the target doesn't have password, Fixed chpwd not checking if a normal user changes admin password, Fixed adduser not adding users without passwords, Fixed adduser adding users with passwords even if they don't match, Removed cdir, Added config entry for screensaver name, Implemented debugging and dump files for kernel errors, Shipped with .pdb debugging symbols for KS, Fixed reboot not clearing screen, Added Dutch, Finnish, Italian, Malay, Swedish and Turkey languages (switch to a compatible font in console), Countries and currencies are now listed when not providing enough arguments or issuing "help currency", Fixed help list not updating for new language update when rebooting, Added permanent aliases (located under your profile, aliases.csv), The password is now hidden when logging in to maintain security, Fixed users being removed after each reboot

**6/21/2019 - 0.0.6.1:** Removed currency information showing on help (will bring it back later), Users are now required to enter their API Key from apilayer.net to convert currencies (Basic plan, get at http://currencylayer.com/product, untested: couldn't pay for basic plan)

**6/24/2019 - 0.0.6.2:** Fixed debug log show command not working because the path was not found (typo: Debugger -> Debugging), Added a notice in listing PC commands about latest versions of Windows 10, Fixed debug kernel header not writing when run with debug argument on, Fixed the debug log being empty every reboot and start, Allowed clearing debug log in command using cdbglog, Used built-in FtpVerify enumerators, removing our hash check for older versions of FluentFTP, Better debugging experience, Debugging now shows line number and source file if pdb is on the same folder, Allowed modding using C#

**6/24/2019 - 0.0.6.2a:** Added checking for processor instructions (currently used in kernel booting to see if SSE2 is supported)

**6/25/2019 - 0.0.6.2b:** Added a command named `sses` to list all SSE versions

**6/26/2019 - 0.0.6.3:** Fixed `quiet` not being entirely quiet, Fixed messages not appearing after signing in (ex. Adding user message), Allowed changing language using command, Fixed the help text showing after executing `sses`, Added Czech language

**6/28/2019 - 0.0.6.4:** Fixed NullReferenceException when changing language, Fixed massive documentation newlines when trying to parse an empty word that is not on the beginning (Please note that we still have newline issues in the first line), Added Ubuntu theme, Removed unused flag, Removed extra requirement to parse colors on boot (removed greed), Made reading FTP file size human-readable

**7/6/2019 - 0.0.6.4a:** Fixed Linux hardware probing failing even if succeeded, Fixed RAM prober showing MemTotal: prefix, Made message about libcpanel-json-xs-perl clear

**7/7/2019 - 0.0.6.4b:** Made one preparation for 0.0.6.5: Downloading debug symbols on startup if not found

**7/25/2019 - 0.0.6.5:** Fixed dump files being created without extension, Localized dumps and manpages, Upgraded language version to the latest, Fixed some bugs about filesystem, Fixed CPU clock speed showing up twice in latest processors (processors that have clock speed on their internal names, for ex. "Intel(R) Core(TM) i7-8700 CPU @ 3.20GHz"), Fixed progress bar of FTP transfers so it uses new format, Added ETA and speed

**7/26/2019 - 0.0.6.6:** Updated manual page for new commands, Removed currency command (unpaid)

**7/27/2019 - 0.0.6.9:** Removed calculators and unit converters, Unified two printing commands into one, Unified two mod generators into one, Allowed entering FTP server without specifying "ftp://" prefix, Allowed specifying address as the "ftp" command argument, Now the FTP client will disconnect peacefully when exiting, Fixed FTP help descriptions not updating when changing languages, SSE checking by command is now supported on Unix systems, Fixed corner time and date position, Fixed password not working correctly even if the user put the correct password, Removed unused phase, Added debug on each phase, Added operating system placeholder for use with MOTD and MAL, Added newline parser to make MOTD support more than 1 line

**8/8/2019 - 0.0.6.10:** Simplified namespace to KS, Fixed codeblocks for Hindi, Chinese, and 1/2 Czech (See comment on GetCommand.vb), Added missing help entry for "reloadsaver", Added "reloadmods" command, Made KernelVersion and EnvironmentOSType read-only, "promptArgs" cmdline argument removed, Extra stack now not generated when rebooting

**8/10/2019 - 0.0.6.11:** Fixed debug showing password in clear text, Showed changelogs during update, Fixed KeyNotFoundException after updating config on startup

**8/11/2019 - 0.0.6.12:** More codeblock corrections for Czech, Croatian, Dutch, Finnish, French, German, Italian, Malay, Portuguese, Spanish, Swedish, and Turkish manual pages. This version is a result of useless modifications to codeblocks that Google has to make so it feels ugly, translated, uncompilable, and misaligned.

**8/13/2019 - 0.0.6.13:** Improved Time and Date probations (Now two fields, one DateTime, one String, are made into one), MOTD and MAL parsing using files to better support newlines, Fixed `chmal` and `chmotd` only taking one word, Fixed casting issues on kernel error, Removed new line placeholder, Removed MAL and MOTD config entries

**8/16/2019 - 0.0.6.13N:** Now builds for both Chocolatey Gallery and NuGet, Fixed NullReferenceException when reading old KS config files by upgrading it to a new format

**8/25/2019 - 0.0.6.14:** Deprecated the usermanual command by custom message, Made text sections for MOTD and MAL in sysinfo

**8/30/2019 - 0.0.7:** Removed manual code and moved all the English docs to Wiki (reducing size to its initial size before manpages were released), Removed changelog viewing on config upgrade, Removed pinging and listing computers in the network, Added support for FTPS, Made use of Filesystem.List instead of its own listing in FTP, Fixed the list command not supporting directories that have spaces

**8/31/2019 - 0.0.7.1:** Updated FluentFTP, Created `get` command to download something from the Internet, Removed useddeps as the devs are already credited in this README.md (no need to credit them for second time), Config now always creates with the string representation of the colors, Added Indonesian, Polish, Romanian, and Uzbekistan language, Implemented remote debugging support

**9/3/2019 - 0.0.7.11:** Added handler for repeated alias addition, Now `arginj` checks for arguments before putting them to the answer field, `cdbglog` now shows a message when it finished or failed, Added `chdir` error handler and support for spaced folder names, Added `chpwd` user not found error handler, `get` will disallow all addresses starting with a space, `md` now can create directories that have spaces, `netinfo` is tidier, `rd` has an error handler about directories that didn't exist, Fixed `setcolors` not defaulting one of the colors or resetting them, Remnants of showmotd, showmal, and showaliases are removed, Added required arguments into `showtdzone`'s help entry, `showtdzone` can now show time in a specific zone

**9/5/2019 - 0.0.7.12:** Made debug port and download retry count customizable, Fixed `get` not downloading anything containing arguments

**9/15/2019 - 0.0.7.13:** Improved the quiet system so it no longer uses the old-fashioned flag system, Fixed the `NotEnoughArgumentsException` when the arguments specified were invalid, Added the built-in chat in **networked** debugger console (Not stable, Version 0.1)

**9/21/2019 - 0.0.7.14:** Fixed chat system not sending messages properly when multiple users talk

**9/23/2019 - 0.0.7.2:** Updated FluentFTP, Added YellowFG and YellowBG themes, Fixed part of the shell prompt color on yellow light/dark backgrounds, Fixed time/date corner position overlapping existing text, Now time zone offsets are shown in each time zone view, Added Japanese language

**9/29/2019 - 0.0.7.21:** Added a message when specifying non-existent time zone in `showtdzone`, Fixed Japanese language missing latest locale additions, Added missing argument requirements in the help entry for `showtdzone`, Fixed FTP connection not prompting for profile selection (apparently, it's not written yet, but it's now written.)

**10/4/2019 - 0.0.7.3:** Updated NuGet.Build.Tasks.Pack to version 5.3.0, Fixed empty address being accepted in FTP `connect` command, Fixed NullReferenceException when handling an error from socket connection that isn't a socket problem, Added basic command support for debugger (No argument system yet, only stack trace show, help, and exit), Added Arabic transliteration (all English letters)

**10/18/2019 - 0.0.7.4:** Updated FluentFTP, Fixed license not showing in NuGet.org, Moved from the deprecated PackageIconUrl to PackageIcon, Added unit test shell (doesn't cover all functions currently, variables treated as texts), Added debug quota so the debugging logs aren't huge, Fixed debugger not flushing properly to the file after using `cdbglog` command

**10/19/2019 - 0.0.7.41:** Recent tests concluded that the FTP progress bar is now fixed (No duplication), Fixed the purple stain in progress bar writing, The ETA for FTP file transfer is now more clear

**10/24/2019 - 0.0.7.5:** Added a new debugging command named `username` that shows current username, Fixed stack trace history not updating when there's an error in accepting new connections, Remote debug shell and the test shell now complain when the command is not found, Added argument support to the debug command, Stack traces are stored in a list and can be viewed in the remote debugger command `trace`

**10/29/2019 - 0.0.7.6:** Added the experimental naming system for chat in remote debugger (Custom names not implemented yet), Added `lines` and `glitterMatrix` screensaver, Now screensavers have their own debugging messages, FluentFTP debugger messages are now redirected to the KS debugger, Now filesystem actions are debugged, Now `get` doesn't run if the URL is not specified, Added missing `get` help entry

**10/30/2019 - 0.0.7.61:** Fixed mod reload help description not translating, Fixed Google's weirdness about `reloadsaver` help description on several language files, Fixed KS crashing on startup if the mods are inserted, Now cursor won't show up if the custom screensaver runs, Added transliterated Russian language

**11/7/2019 - WIP - 0.0.8:** Added translated versions of Arabic, Japanese, Chinese, Hindi, and Russian, Added `debug` real command line argument, Updated FluentFTP and NewtonSoft.Json, Now the FTP shell lets you adjust logging options for username and IP address, Added `glitterColor`, `hackUserFromAD`, and `aptErrorSim` screensavers, Added `Windows95`, `GTASA`, `GrayOnYellow`, and `BlackOnWhite` themes, Fixed crash when opening second instance of KS, Passwords are encrypted, Now strings needed localization are prepended with ` ! Needs Localization ! `, Some error handling improved, Function names logged in debug log, Added safe mode, Current stage now shows on boot, Settings are now commented, Now custom screensavers have a custom ending and a delay for each write, Now it lets users set a size parse mode, Fixed `chdir`, `rd`, and `list` not working on full directory names, Added `testregexp` test shell command, 

## |-----+--> _Contributors_ <--+-----|

**EoflaOE:** Owner of Kernel Simulator

**Oxygen Team:** Icon creator

## |-----+--> _Open Source Libraries used_ <--+-----|

MadMilkman.Ini

Source code: https://github.com/MarioZ/MadMilkman.Ini

Copyright (c) 2016, Mario Zorica

License (Apache 2.0): https://github.com/MarioZ/MadMilkman.Ini/blob/master/LICENSE

Newtonsoft.Json

Source code: https://github.com/JamesNK/Newtonsoft.Json

Copyright (c) 2007, James Newton-King

License (MIT): https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md

FluentFTP

Source code: https://github.com/robinrodricks/FluentFTP

Copyright (c) 2011-2016, J.P. Trosclair

Copyright (c) 2016-present, Robin Rodricks

License (MIT): https://github.com/robinrodricks/FluentFTP/blob/master/LICENSE.TXT

## |-----+--> _License - GNU GPL_ <--+-----|

    Kernel Simulator - Simulates our future planned Kernel
    Copyright (C) 2018-2019  EoflaOE

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.

