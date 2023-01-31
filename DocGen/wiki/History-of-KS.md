# History of KS

## 2018

### Pre-release

Before the release of 0.0.1.0, we were working on the PowerPoint OS collection almost 2 years ago (as of 2019), and that used VBA (VB6) macros on Office 2007. While we were working on it, the macro wasn't maintainable thanks to the restrictions of Visual Basic 6, so we have cancelled it.

In that way, we have learned how to develop VB.NET, and started to think about the kernels, and because it's impossible to make a bootable kernel except that if we have Cosmos, we have done the simulator that does the equivalent way of booting.

We wrote the kernel booting procedure, log-in, and the shell which includes commands, making that four source code files compared to now. When we finished, we have initialized our Git repository sitting around the Aptivi account on GitHub as our coding management system, because it isn't right to release the GNU GPL software WITHOUT the source code.

### Big releases

Big releases are releases that has 12+ exact changes or more changes. When the change amount in the version is 24+ or more, it's considered as "Huge" update. If possible, every 4 releases could be a Big update, or a Huge update. But these updates might dramatically increase the size of the executable, depending on the number of the striking changes made.

To consider it as a big or huge release, the third part of the version that is divided by 4 should be the number that doesn't have the decimal point, the first release which meets the third version part rule may claim this title, and the striking changes should be more than 12.

### 5/23/2018

We published the 0.0.4.1 version publicly in case there is an HDD failure.

After that, we are trying to learn more about kernels, and how are they built, using old Linux CD-ROMs, and the one happened to be Mandrake Linux, was downloading. On our Android tablet, the Internet connection slowed down, so we went to pause a download, hoping that it goes fine.

Whenever we paused the game, we went into the computer, and saw that the download paused, like usual. We saw the HDD light always being on, and also the WiFi light. We tried to move the mouse but we can't, so we did the hard reboot and now our development environment started to get bluescreens during the startup.

We then tried the Hiren's Boot CD's Mini Windows XP, and that booted successfully. Yet when it tried to access the faulty hard drive, we're hearing clicks and at the same time, MiniXP hangs.

We managed to open the CMD and we have run the chkdsk program with /F.

We saw that it has scanned for errors and fixed them, and when we restarted again, everything went fine.

See more at [https://www.bleepingcomputer.com/forums/t/677922/windows-xp-sp3-reboot-cycle-crashes-often-on-boot/](https://www.bleepingcomputer.com/forums/t/677922/windows-xp-sp3-reboot-cycle-crashes-often-on-boot/) (Thread is hot - According to bleepingcomputer.com, it says that the topic is created at 5/22/2018 when the failure happened at 5/23/2018.)

### 5/24/2018 to 5/27/2018

We tried again by going to our OS, but things messed up after installing and uninstalling AVIRA Antivirus because of its instability with the non-SSE2 processors. The boot up slowed down, the hard drive clicking noise appeared when we're trying to access Firefox, and we got the blue screen showing that it crashed because the critical process is killed.

We went into MiniXP again, and we tried to get the SMART info using gsmartcontrol latest version, but it reported that it didn't have the dwmapi.dll. The dwmapi.dll is for Windows Vista and up.

We tried again using CrystalDiskInfo, but it didn't print the information properly.

We quickly backed up all of the important data to another working drive that is connected in the past 2-3 years from the spare computer that failed to boot.

We also tried to test the drive by using the Western Digital diagnostic utility, but the quick test failed immediately, so it suggests that the hard drive failure is imminent. We tried the long test, it found bad sectors! We tried to repair it, but we got an error saying that bad sectors didn't get fixed, so the drive is failing.

On the final day of the HDD that has the super-customized Windows XP OS, the drive failed completely after that, and all the portion of the OS and programs, including the XPize dark theme (Royale?), Windows Sidebar for XP, Visual Basic 2010 (old), Firefox, and all of the programs we tried hard to get and customize, including the OS font settings like Ubuntu, Segoe UI, Source Sans, etc., sound settings, mouse settings, the custom bootscreen, and so on, are lost forever, because the BIOS won't detect that drive. This is an indication that the HDD has died. It reached the expected 5 years of death.

See more at: [https://www.bleepingcomputer.com/forums/t/678191/windows-xp-sp3-slow-booting-unusual-hdd-activity-with-noise-then-bsod/](https://www.bleepingcomputer.com/forums/t/678191/windows-xp-sp3-slow-booting-unusual-hdd-activity-with-noise-then-bsod/) (Thread is hot)

### 5/28/2018

After the drive has died, we decided to unplug the dead drive from the computer, and set the working drive settings to Master, we went into Hiren's BootCD's Partition Magic, and set up appropriate settings so that we have 2 partitions without wiping our data drive.

After that, we installed Windows XP and configured everything as appropriate, as long as we didn't wipe everything on the data partition, because if it was wiped, then all of the hard work are lost forever.

We set up VB2010 as appropriate.

### Use of libraries

We tried to make everything as simple as possible, but the config one turns out to have all the writers.

We decided to download the MadMilkman.Ini library and install it into KS, and we re-wrote the config writers to the library ones.

The 0.0.5.5 version didn't use NuGet to get libraries, because from the 0.0.1, we're using the VB2010 on Windows XP development environment.

But, there is no working NuGet package manager for VB2010 because support has ended, and instead, they provide downloads for VS2015, and VS2017, at the time of writing.

However, 0.0.5.5 still supports Windows XP.

### The support for Unix systems

When we're developing KS, we stumbled upon the Mono project, and it said that it has support for Windows, Mac OS X, and Linux. MonoDevelop doesn't check for mistakes on the code when you're coding, but does check when you're building.

We recommend JetBrains Rider for Linux 64-bit systems, because it can check for code for errors LIVE like Visual Studio for Windows, Visual Studio Code for Windows, Mac, and Linux, and unlike MonoDevelop. Rider can also check for spelling mistakes in strings, while VS2017, VS Code, and MonoDevelop can't.

The .NET for Mono project however is experimental at this time, but both MonoDevelop and Rider use it.

So, we have loaded the project into MonoDevelop, but when we built the project, we got 3 warnings. We fixed them all, but the hardware probing step crashed, because there is no WMI for Linux or Mac, because WMI is only meant for Windows.

After we removed the hardware initialization statement from Kernel.vb, the kernel started up, but there's another problem.

Although Mono's Console.Write(Line) can parse arguments correctly, W(ln) can't. However, it's fixed in later versions.

We made everything as necessary to fix this problem, and it succeeded. We fixed other crashing or exception (AmbiguousMatchException) problems.

### The new development looks

We began development of 0.0.5.9 in VB2010 Express and Windows XP, and used the MadMilkman.Ini, FluentFTP and Newtonsoft.Json libraries by getting in normal way.

Meanwhile, one of our libraries were severely outdated and should be updated to track the progress bar of upload and download progress. But we couldn't get the NuGet package manager for VB2010 because the download was failing although the Internet was working and can access sites fine.

We have dual-booted Windows XP with Windows 7 and tried to install VS2017, but it appears that the Installer crashed (Not the installer where it downloads 60 MB of data and unpacking them, but used for installing VS2017). The installer was called "Visual Studio Installer" and you can use it to install VS2017, modify it, or uninstall it at your own needs without having to re-run the setup tool.

We then suspected it was vs_installer.exe trying to access illegal instructions in our non-SSE2 AMD Athlon XP 1500+, so we have tried VS2015, and it worked.

The NuGet package manager worked successfully. We have moved our KS 0.0.5.9 source code to Windows 7 before accessing them in VS2015.

We removed all old libraries using the outdated way and began using NuGet to put dependency libraries into KS, and we got the latest version of FluentFTP. What's good is that NuGet can update packages.

We completed the coding, and we tried to put it to GitHub using the GitHub extension in VS Community, but it failed to work because the devenv.exe tried to access SSE2 instructions, so VS2015 crashed.

After several crashes, we got a laptop running Windows 10 on it, and moved all the KS files into it, but 3-4 files were lost, and one of them is GetCommand.vb, which parses commands.

We re-wrote these 3-4 files, and they worked successfully like the last time we extended them to have 0.0.5.9 features.

## 2019

### Truth about status probing

When we were developing KS 0.0.4.5, we have added Status for RAM and for HDD to indicate that they're OK, or if they're failing. But, the readings for the RAM were always "Unknown", and we thought that the HDD status probing was a bit redundant, so we have removed them in this release. We thought that this was an accidental addition, and due to it being so early, so the status was redundant and will likely be added in the future.

### Linux probing

We are using the /proc/cpuinfo file to get information about the processor (name and clock), the /proc/meminfo file to get total memory that is installed in the system, and inxi application to get information about hard drives.

But, not all distros install inxi by default, so if you don't have it installed, probing will fail. In order to install, follow below:

Debian, and its derivatives:

```
# apt update
# apt install inxi
```

Red Hat, and its derivatives:

```
# dnf/yum install epel-release (?)
# dnf/yum install inxi (?)
```

OpenSUSE, and its derivatives:

```
# zypper install inxi (?)
```

Arch Linux, and its derivatives:

```
# pacman -Syu (?)
# pacman -i inxi (?)
```

The rest, build from source and put the executable file into /usr/bin since the prober only looks in that directory, or use your package managers to install it.

(?): Untested. If anybody tested, send a PR to edit this manual page with the correct commands.

### KS is now inside GitHub Package Registry Beta

Congratulations to us because we got access to GitHub Package Registry Beta. Now, we have three sources instead of two, which are the following: Chocolatey Gallery, NuGet, and GitHub Package Registry.

## 2020

### Truth about sound libraries

Several releases ago, we have released `speak` and `soundtest` commands using NAudio as the library to handle sounds. It has worked fine in our test environment that's running Windows. We have assumed that everything is fine, until we've done the following:

- Testing these commands for Linux compatibility

Apparently, because NAudio is only for Windows for its core functions such as playing audio, it doesn't work for Linux because it tries to locate mfplat.dll which is non-existent for non-Windows systems.

So, we've changed the library that handles sound to VLC in 0.0.8, and it appears to work normally in Windows until it's time to test its compatibility on Linux systems. It has proven that it only worked on one command, `soundtest`, and didn't work on the other, `speak`. We didn't know the reason behind this error until we've found out that a console app, which is part of Linux's VLC, can play sounds, so we've used that and tested it, and it worked fine.

However, when we went to test it on Windows systems whilst developing 0.0.11, it stopped working on the `speak` command. Because there is an instability while the core tries to load the native library for Windows systems, which causes the `You haven't installed the package which provides the native library...` error message to appear, even when installed. We've double-checked that we have that native library for Windows systems.

Today, we didn't believe that both commands can be stable, so we've decided to shut down those commands as part of the 0.0.11 changes, which resulted in us not having to ship two types of binary packages: Full, and Isolated. We're back to one type: Full! Just before the 0.0.8 packaging method!

As a result, the build times are faster and doesn't use too much I/O during the build, because of NuGet packaging task during it.

### The Truth of SSH.NET 2020.0.0 Beta 1

In development versions of 0.0.12.0, we've upgraded SSH.NET to 2020.0.0-beta1 hoping that it would someday have a full release for the SSH library that Nitrocid KS uses. When we've reached the final version of 0.0.12, we've seen that SSH.NET is still on 2020.0.0-beta1 instead of continuing development, so we've left it.

After we've released 0.0.12.1, we've tested installing KS from NuGet, and we've seen an error message about SSH.NET, so we've installed 2020.0.0-beta1 first, then installed KS, and found that NuGet installed it successfully. Because this brings extra steps and we wanted simplicity, we've decided to downgrade SSH.NET to 2016.1.0 as its latest stable version.

### Goodbye, directory structure parsing

We have let KS parse the entire directory structure since the actual file system was implemented on 0.0.5.14, which is a pre-release version of 0.0.6. The `InitFS()` (previously `Init()`) function involved initializing the file structure by parsing the entire Home directory and all its subdirectories and files.

We thought this is good, but over time, its usage (not the entire FS, but its structures) starts to decrease. Examples are listed below:

* You have `SetCurrDir()` that doesn't use the FS structure list to see if the folder exists or not
* You have `copy` command that uses `{Directory|File}.Exists()` to check for directory or file existence
* You have `md` command that uses `Directory.Exists()` to check for directory existence
* You have `read` command that uses `File.Exists()` to check for file existence

Also, the filesystem structures are initialized every startup and every directory change. This causes additional load on the hard drive, especially on the first cold startup, making startup times longer than usual. The problem gets worse if your filesystem is too big and contains many directories and files, as it scans the entire directory, files, the subdirectories, and what's inside them. If this happens, then it's like you're telling the hard drive to read the entire directory structure.

We have fears of hard drives thrashing, especially the SSDs that they have read and write limits and if exceeded, would no longer work properly, so we have to put an end to the directory structure parsing function. If this happens, the below happens:

* Hard drive usage will be decreased
* Faster start-up times
* Faster directory change
* Increases the lifespan of both hard drives and SSDs

#### Removal history

* Removed filesystem parsing according to commit [a9fc2de6](https://github.com/Aptivi/NitrocidKS/commit/a9fc2de6cfa79c4580331fa5bea5c7e5ad9326fe)
