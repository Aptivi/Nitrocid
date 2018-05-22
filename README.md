# |---+-> Kernel Simulator <-+---|

The build is currently [![Build status](https://ci.appveyor.com/api/projects/status/9anm0jc0x9raoy8x/branch/master?svg=true)](https://ci.appveyor.com/project/EoflaOE/kernel-simulator/branch/master)

INFO: This kernel simulator simulates our **future** kernel that is planned by us.

NOTE: This kernel simulator _will_ continue to be developed, even if we made the real PC version of Kernel.

INFO: This kernel simulator is not a final planned version of Kernel, since it was minimal.

NOTE: It can only be Console at the moment, while we are developing a GUI for this simulator.

Kernel Simulator lets you simulate a _very_ early access for our **future** Kernel. It provides the built-in hardware
detector, log-in manager, and the shell.

Cannot log-in to Kernel Simulator on **root** account? The password is the _backwards_ of **root**.

## |-----+--> _0.0.3 Changes_ <--+-----|

We have removed _bin_ folder and made the source code store directly in the _root_ folder of Kernel Simulator files to manage _releases_.

## |-----+--> _Download_ <--+-----|

You can download the binary and the source code here: https://github.com/EoflaOE/Kernel-Simulator/releases

## |-----+--> _Prerequisites_ <--+-----|

[Microsoft .NET Framework 4.0](https://download.microsoft.com/download/1/B/E/1BE39E79-7E39-46A3-96FF-047F95396215/dotNetFx40_Full_setup.exe) is **important and required** for Kernel Simulator to work fully. If you have Windows 8 or later, you might already have this version of Microsoft .NET Framework 4.0.

## |-----+--> _Build Instructions_ <--+-----|

1. Install [Microsoft Visual Basic Express 2010](https://visual-basic-express.soft32.com/old-version/386190/2010.express/) or [Visual Studio 2010](https://www.visualstudio.com/vs/older-downloads/ "Sign-in required"), or higher.

2. After installation, extract the source code, and open Microsoft Visual Basic / Studio 2010, and click on **Open Project...**

3. Go to the source directory, and double-click the solution file

4. Right click on the project on the right, and select **Properties**

5. Go to **Compile**, click **Browse...** on **Build output path:**, and select your build path. When you're finished, click on **OK** button.

6. Click on the **Build** menu bar, and click on **Build Kernel Simulator**

7. In **Windows Explorer**, go to the build directory and then double-click on the executable file.

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

## |-----+--> _Manual pages_ <--+-----|

The documentations can be found in source code of kernel simulator in `Kernel Simulator/Documentation`

**Documentation - main page:** Information about Kernel Simulator, this page

**Documentation - faq:** Frequently Asked Questions for Kernel Simulator

**Documentation - contributing rules:** Conditions for contributing Kernel Simulator

**Documentation - troubleshooting:** List of known and user-reported problems

## |-----+--> _Contributors_ <--+-----|

**EoflaOE:** Owner of Kernel Simulator

**Paomedia:** Icon creator

## |-----+--> _License - GNU GPL_ <--+-----|

    Kernel Simulator - Simulates our future planned Kernel
    Copyright (C) 2018  EoflaOE

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

