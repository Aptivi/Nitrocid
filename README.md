# |---+-> Kernel Simulator <-+---|

AppVeyor: [![Build status](https://ci.appveyor.com/api/projects/status/9anm0jc0x9raoy8x/branch/master?svg=true)](https://ci.appveyor.com/project/EoflaOE/kernel-simulator/branch/master) 

TravisCI: [![Build Status](https://travis-ci.org/EoflaOE/Kernel-Simulator.svg?branch=master)](https://travis-ci.org/EoflaOE/Kernel-Simulator)

This kernel simulator simulates our **future** kernel that is planned by us and is not a final planned version of Kernel, since it was minimal.

## |-----+--> _Notes_ <--+-----|

- This kernel simulator _will_ continue to be developed, even if we made the real PC version of Kernel.

- It can only be Console at the moment, while we are developing a GUI for this simulator.

- We took out Windows XP support in favor of NuGet and .NET Framework 4.6.1.

## |-----+--> _Prerequisites_ <--+-----|

1. For Windows systems

- Windows 7 or higher

- Microsoft .NET Framework 4.6.1 or higher

2. For Unix systems

- Any Unix system that contains the latest version of [Mono Runtime](http://www.mono-project.com/docs/about-mono/languages/visualbasic/). MonoDevelop(32-bit or 64-bit)/JetBrains Rider(64-bit) is required to build from source.

## |-----+--> _Build Instructions_ <--+-----|

- For Windows systems

1. Install Microsoft Visual Studio 2017, or higher.

2. After installation, extract the source code, open VS2017, and click on **Open Project...**

3. Go to the source directory, and double-click the solution file

4. Right click on the project on the right, and select **Properties**

5. Go to **Compile**, click **Browse...** on **Build output path:**, and select your build path. When you're finished, click on **OK** button.

6. Click on the **Build** menu bar, and click on **Build Kernel Simulator**

7. In **Windows Explorer**, go to the build directory and then double-click on the executable file. 

- For Unix systems (MonoDevelop)

1. Install [Mono Runtime](http://www.mono-project.com/docs/about-mono/languages/visualbasic/) and MonoDevelop.

2. After installation, extrace the source code, and open MonoDevelop, and click on **Open...**

3. Go to the source directory, and double-click the solution file

4. Add the following lines to **Kernel Simulator.vbproj**:

	<AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
	
5. Change the output directory if you are building using **Release**.

6. Click on the **Build** menu bar, and click on build button to compile.

7. In **your file manager**, go to the build directory and then double-click on the executable file.

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

## |-----+--> _Manual pages_ <--+-----|

The documentations can be found in source code of kernel simulator in `Kernel Simulator/Documentation`

**Documentation - main page:** Information about Kernel Simulator, this page

**Documentation - faq:** Frequently Asked Questions for Kernel Simulator

**Documentation - contributing rules:** Conditions for contributing to Kernel Simulator

**Documentation - troubleshooting:** List of known and user-reported problems

* The text files will be moved to wiki section.

## |-----+--> _Contributors_ <--+-----|

**EoflaOE:** Owner of Kernel Simulator

**Paomedia:** Icon creator

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

