# Second-generation KS releases

Because the API was having mixed code of between refactored and unrefactored, we would like to release the second-generation Kernel Simulator to celebrate the end of Alpha stage of this kernel, as well as the new "beta" version of our kernel.

Please note that the API that the second-generation KS has will not be backwards-compatible with the first-generation API, so we urge all mod and screensaver developers to update their mod to fully support second-generation and optionally provide the first-generation version, following the [Compatibility Notes for Second Generation KS](Compatibility-Notes-for-Second-Generation-KS.md).

Meanwhile, we plan to keep supporting the first generation KS until 2024.

## KS 0.0.20.x series

### KS 0.0.20.0 (2/22/2022)

1. Added high customization
2. Added new commands
3. Added new screensavers
4. Added new languages
5. Added new themes
6. Added notification borders
7. Added manual page viewer for mods
8. Added UTC support
9. Added mod blacklist support
10. Added support for POP3 (experimental)
11. Added switches for commands
12. Updated libraries
13. Updated the icon for Chocolatey installations
14. Changed mail command name from `lsmail` to `mail` for relevancy
15. Removed obsolete commands
16. Removed AptErrorSim and HackUserFromAD screensavers
17. Many API changes and improvements
18. General improvements and bug fixes
19. And many surprises...

### KS 0.0.20.1 (3/2/2022)

1. Fixed bugs in update facility
2. Fixed debugger not accepting messages in Linux hosts
3. General improvements and bug fixes

### KS 0.0.20.2 (3/13/2022)

1. General improvements

### KS 0.0.20.3 (3/19/2022)

1. SSH will automatically disconnect if exited
2. General improvements and bug fixes

### KS 0.0.20.4 (4/5/2022)

1. Updated the debug symbol downloader to point to new link

### KS 0.0.20.5 (4/14/2022)

1. Fixed race condition in kernel threads causing screensavers to stop with an error

### KS 0.0.20.6 (5/5/2022)

1. Backported fixes and improvements from 0.0.21.0

### KS 0.0.20.7 (6/10/2022)

1. Backported fixes from 0.0.22.0

### KS 0.0.20.8 (7/8/2022)

1. Backported fixes

## KS 0.0.21.x series

### KS 0.0.21.0 (4/28/2022)

1. Updated libraries
2. Added new screensavers
3. Added custom notifications API
4. Added hex shell
5. You can select feed by country!
6. Line parsing behavior should be consistent in all shells
7. MathBee is now Solver
8. General improvements and bug fixes

### KS 0.0.21.1 (5/1/2022)

1. Improvements to the RSS feed selector
2. General improvements and bug fixes

### KS 0.0.21.2 (5/4/2022)

1. Fixed fatal crash on startup on ARM devices

### KS 0.0.21.3 (5/8/2022)

1. Fixed crash on startup in some Linux systems
2. Fixed crash on startup when a non-screensaver file is encountered
3. Fixed crash on startup if there is a kernel crash during boot
4. Fixed `hwinfo` not displaying hardware information

### KS 0.0.21.4 (5/10/2022)

1. General improvements and bug fixes

### KS 0.0.21.5 (5/16/2022)

1. General improvements and bug fixes

### KS 0.0.21.6 (6/10/2022)

1. Backported fixes from 0.0.22.0

### KS 0.0.21.7 (7/8/2022)

1. Backported fixes

## KS 0.0.22.x series

### KS 0.0.22.0 (6/12/2022)

1. Added dependencies support for mods
2. Added new commands
3. Added new screensavers
4. Added new themes
5. Added support for .NET 6.0
6. Used [ReadLine.Reboot](https://github.com/EoflaOE/ReadLine.Reboot) to read inputs. This fixes many issues regarding reading inputs on Linux systems.
7. Fixed variety of crashes
8. Removed unnecessary dependency, MadMilkman.Ini, from the main app
9. You can now run Kernel Simulator from Android directly (UserLAnd)!
9. General improvements and bug fixes

### KS 0.0.22.1 (6/12/2022)

1. Improved accuracy of remaining time
2. Fixed problems related to screensavers

### KS 0.0.22.2 (6/13/2022)

1. Fixed custom screensavers breaking when `SleepNoBlock(milliseconds, Custom)` is used
2. General improvements and bug fixes

### KS 0.0.22.3 (6/15/2022)

1. General compatibility improvements for .NET 6.0

### KS 0.0.22.4 (6/17/2022)

1. General improvements and bug fixes

### KS 0.0.22.5 (7/8/2022)

1. Backported fixes

## KS 0.0.23.x series

### KS 0.0.23.0 (7/12/2022)

1. Updated libraries
2. Added "No APM" simulator
3. Improved name generation process
4. No more stack overflow when double panic fails
5. Added new screensavers (Glitch, FallingLine)
6. Added new language (Gangsta, Playa - pla)
7. Added new splash (Fader)
8. Screensaver improvements
9. General compatibility improvements for .NET 6.0
10. General improvements and bug fixes

### KS 0.0.23.1 (7/12/2022)

1. Fixed crash on startup due to an Inxi.NET bug

### KS 0.0.23.2 (7/13/2022)

1. Fixed dictionary defining only one meaning for words like `fine`
2. Fixed file handles being stuck open

# Second-generation, Revision 1

## KS 0.0.24.x series

### KS 0.0.24.0 (WIP)

This version is WIP as of 7/14/2022.

1. Removed obsolete Mod and Screensaver APIs
2. Added command autocompletion
3. Fixed wrong character for lower left corner in Ramp when the config is being created
4. General API improvements
5. General improvements and bug fixes