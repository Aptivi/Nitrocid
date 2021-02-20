# Kernel Simulator

![Kernel startup](https://i.imgur.com/KO1v1Kg.png)

![GitHub repo size](https://img.shields.io/github/repo-size/EoflaOE/Kernel-Simulator?color=purple&label=size) [![GitHub All Releases](https://img.shields.io/github/downloads/EoflaOE/Kernel-Simulator/total?color=purple&label=d/l)](https://github.com/EoflaOE/Kernel-Simulator/releases) [![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/EoflaOE/Kernel-Simulator?color=purple&include_prereleases&label=github)](https://github.com/EoflaOE/Kernel-Simulator/releases/latest) [![Chocolatey Version (including pre-releases)](https://img.shields.io/chocolatey/v/ks?color=purple&include_prereleases)](https://chocolatey.org/packages/KS/) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/KS?color=purple)](https://www.nuget.org/packages/KS/)

Kernel Simulator simulates the future of our planned kernel that's coming as soon as it's finished. It consists of the kernel, the built-in shell known as UESH, and the built-in applications.

Not only it has some basic commands, but it also provides script support, network support, and tons of awesome things.

## Build Status

Here are all the CI build status for all KS branches.

| Branch      | AppVeyor | Travis CI
|-------------|----------|------------
| master      | [![Build status](https://ci.appveyor.com/api/projects/status/9anm0jc0x9raoy8x/branch/master?svg=true)](https://ci.appveyor.com/project/EoflaOE/kernel-simulator/branch/master) | [![Build Status](https://travis-ci.org/EoflaOE/Kernel-Simulator.svg?branch=master)](https://travis-ci.org/EoflaOE/Kernel-Simulator)
| servicing   | [![Build status](https://ci.appveyor.com/api/projects/status/9anm0jc0x9raoy8x/branch/servicing?svg=true)](https://ci.appveyor.com/project/EoflaOE/kernel-simulator/branch/servicing) | [![Build Status](https://travis-ci.org/EoflaOE/Kernel-Simulator.svg?branch=servicing)](https://travis-ci.org/EoflaOE/Kernel-Simulator)

## System Requirements

This section covers what you need to run Kernel Simulator. Please scroll down to your system below.

### Windows systems

1. Windows 7 or later (Windows 10 recommended)
2. .NET Framework 4.8
3. Internet Connection
4. Appropriate console that supports escape sequences and custom monotyped fonts for languages

### Linux systems

1. Mono 5.10 or higher (6.0 or higher is recommended)
2. Internet Connection
3. Appropriate console that supports escape sequences and custom monotyped fonts for languages (Konsole, GNOME Terminal, ...)

## How to install

This section covers how to install Kernel Simulator on your system. Please scroll down to your system below.

### Windows systems (First method)

1. Download the Kernel Simulator binary files [here](https://github.com/EoflaOE/Kernel-Simulator/releases).
2. Unzip the file to any directory
3. Run it by double-clicking `Kernel Simulator.exe`

### Windows systems (Second method)

1. Install Chocolatey [here](https://chocolatey.org/install).
2. Press the `chocolatey` button
3. Follow the steps to install Kernel Simulator
4. Once installed, open PowerShell and execute `Kernel Simulator`

### Linux systems (First method)

1. Download the Kernel Simulator binary files [here](https://github.com/EoflaOE/Kernel-Simulator/releases).
2. Unzip the file to any directory
3. Install the following programs:
   - Microsoft.VisualBasic.dll 10.0 (Debian and its derivatives: `sudo apt install libmono-microsoft-visualbasic10.0-cil`)
   - Inxi application (For hard drive probation) (Debian and its derivatives: `sudo apt install inxi libcpanel-json-xs-perl`)
4. Open terminal to the directory that contains KS, and run it using `mono "Kernel Simulator.exe"`

### Linux systems (Second method - Ubuntu)

1. Open the terminal, and execute `sudo add-apt-repository ppa:eofla/kernel-sim`
2. Confirm the addition of the PPA. It should update your package cache
3. Execute `sudo apt install kernel-simulator`
4. Execute `ks`

## How to Build

This section covers how to build Kernel Simulator on your system. Please scroll down to your platform below.

### Visual Studio 2017+

1. Open Visual Studio
2. Press `Clone a repository`
3. In Repository Location, enter `https://github.com/EoflaOE/Kernel-Simulator.git`
4. Wait until it clones. It might take a few minutes depending on your Internet connection.
5. Press `Solution Explorer`, then press `Switch Views`
6. Click on `Kernel Simulator.sln`
7. Press `Start` or press `Build > Build Solution`
8. Open your file explorer, go to the build directory, and double-click on the executable file.

### JetBrains Rider (64-bit)

1. Install Mono Runtime, Git, and `libmono-microsoft-visualbasic10.0-cil`.
2. Install JetBrains Rider.
3. After installation, open JetBrains Rider, and follow the configuration steps.
4. When the main menu opens, choose `Check out from Version Control` and then `Git`.
5. Write on the URL `https://github.com/EoflaOE/Kernel-Simulator.git` and press `Test` to verify your connectivity.
6. Press Clone, and git will download the repo, then Rider will open up. It might take a few minutes depending on your Internet connection.
7. You will get some errors about the inability to resolve `My.Computer`. Ignore these, as they won't interrupt the compilation.
8. Click on the hammer button to build, the bug button (breakpoints enabled), or the Run button (breakpoints disabled - CTRL+F5 on VS). When the Edit configuration screen appears, tick the checkbox named `Use External Console`.
9. If you used the hammer button, then open your file explorer, go to the build directory, and double-click on the executable file.

### MonoDevelop

1. Install Mono Runtime, `libmono-microsoft-visualbasic10.0-cil`, and MonoDevelop.
2. After installation, extract the source code, open MonoDevelop, and click on `Open...`
3. Click on the `Build` menu bar, and click on build button to compile.
4. In your file manager, go to the build directory and then double-click on the executable file.

## Credits

**EoflaOE:** Owner of Kernel Simulator

**Oxygen Team:** Icon creator

**OpenWeatherMap:** Weather API

**All contributors:** Contribution

## Open Source Libraries

Below entries are the open source libraries that is used by KS. They are required for execution.

### MadMilkman.Ini

Source code: https://github.com/MarioZ/MadMilkman.Ini

Copyright (c) 2016, Mario Zorica

License (Apache 2.0): https://github.com/MarioZ/MadMilkman.Ini/blob/master/LICENSE

### Newtonsoft.Json

Source code: https://github.com/JamesNK/Newtonsoft.Json

Copyright (c) 2007, James Newton-King

License (MIT): https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md

### FluentFTP

Source code: https://github.com/robinrodricks/FluentFTP

Copyright (c) 2011-2016, J.P. Trosclair

Copyright (c) 2016-present, Robin Rodricks

License (MIT): https://github.com/robinrodricks/FluentFTP/blob/master/LICENSE.TXT

### MailKit

Source code: https://github.com/jstedfast/MailKit/

Copyright (c) 2013-present, .NET Foundation and Contributors

License (MIT): https://github.com/jstedfast/MailKit/blob/master/LICENSE

### SSH.NET

Source code: https://github.com/sshnet/SSH.NET/

Copyright (c) Renci

License (MIT): https://github.com/sshnet/SSH.NET/blob/develop/LICENSE

### Extensification

Source code: https://github.com/EoflaOE/Extensification/

Copyright (c) 2020-present EoflaOE and its companies

License (GNU GPL 3.0 or later): https://github.com/EoflaOE/Extensification/blob/master/LICENSE

### Inxi.NET

Source code: https://github.com/EoflaOE/Inxi.NET/

Copyriht (c) 2020-present EoflaOE and its companies

License (GNU GPL 3.0 or later): https://github.com/EoflaOE/Inxi.NET/blob/master/LICENSE

### Nuget.Build.Tasks.Pack (build dependency)

Source code: https://github.com/NuGet/NuGet.Client

Copyright (c) .NET Foundation. All rights reserved.

License (Apache 2.0): https://github.com/NuGet/NuGet.Client/blob/dev/LICENSE.txt

## License

    Kernel Simulator - Simulates our future planned Kernel
    Copyright (C) 2018-2021  EoflaOE

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

