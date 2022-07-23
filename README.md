# Kernel Simulator

![image](https://user-images.githubusercontent.com/15963131/154856234-bcbdfbb2-7c37-4e65-a6bf-43fbb8fbb949.png)

![GitHub repo size](https://img.shields.io/github/repo-size/EoflaOE/Kernel-Simulator?color=purple&label=size) [![GitHub All Releases](https://img.shields.io/github/downloads/EoflaOE/Kernel-Simulator/total?color=purple&label=d/l)](https://github.com/EoflaOE/Kernel-Simulator/releases) [![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/EoflaOE/Kernel-Simulator?color=purple&include_prereleases&label=github)](https://github.com/EoflaOE/Kernel-Simulator/releases/latest) [![Chocolatey Version (including pre-releases)](https://img.shields.io/chocolatey/v/ks?color=purple&include_prereleases)](https://chocolatey.org/packages/KS/) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/KS?color=purple)](https://www.nuget.org/packages/KS/)

Kernel Simulator simulates the future of our planned kernel that's coming as soon as it's finished. It consists of the kernel, the built-in shell known as UESH, and the built-in applications.

Not only it has some basic commands, but it also provides script support, network support, and tons of awesome things.

WARNING: Second-generation versions of KS are not backwards-compatible with the first-generation versions. Upgrade your mods to support the latest API changes.

WARNING: The gangsta language contains strong language that may make you feel uncomfortable reading it.

## Build Status

Here are all the CI build status for all the active KS branches.

| Branch    | AppVeyor 
|-----------|----------
| master    | [![Build status](https://ci.appveyor.com/api/projects/status/9anm0jc0x9raoy8x/branch/master?svg=true)](https://ci.appveyor.com/project/EoflaOE/kernel-simulator/branch/master)
| servicing | [![Build status](https://ci.appveyor.com/api/projects/status/9anm0jc0x9raoy8x/branch/servicing?svg=true)](https://ci.appveyor.com/project/EoflaOE/kernel-simulator/branch/servicing)

## System Requirements

This section covers what you need to run Kernel Simulator. Please refer to the table below:

### Minimum requirements

| System  | System version     | Framework version                      | Terminal Emulator                   | Internet
|---------|--------------------|----------------------------------------|-------------------------------------|----------
| Windows | Windows 7 or later | .NET Framework 4.8 or .NET Runtime 6.0 | Improved cmd.exe, ConEmu            | Required
| Linux   | Supported distros  | Mono 5.10 or later or .NET Runtime 6.0 | Konsole, GNOME Terminal             | Required
| macOS   | macOS Catalina     | Mono Runtime or .NET Runtime 6.0       | iTerm2 (Terminal.app not supported) | Required

### Recommended requirements

| System  | System version     | Framework version                      | Terminal Emulator                   | Internet
|---------|--------------------|----------------------------------------|-------------------------------------|----------
| Windows | Windows 10 or 11   | .NET Framework 4.8 or .NET Runtime 6.0 | Improved cmd.exe, ConEmu            | Required
| Linux   | Supported distros  | Mono 6.0 or later or .NET Runtime 6.0  | Konsole, GNOME Terminal             | Required
| macOS   | macOS Catalina     | Mono Runtime or .NET Runtime 6.0       | iTerm2 (Terminal.app not supported) | Required

### Notes

* Terminal.app has broken support for 255 and true colors. We discourage using it.
* Download .NET Runtime 6.0 from [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Download .NET Framework 4.8 from [here](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-web-installer)
* Download Mono from [here](https://www.mono-project.com/download/stable/)

## How to install

This section covers how to install Kernel Simulator on your system. Please scroll down to your system below.

### Windows systems 

#### Recommended method

1. Install Chocolatey [here](https://chocolatey.org/install).
2. Press the `chocolatey` button
3. Follow the steps to install Kernel Simulator
4. Once installed, open PowerShell and execute `ks`

#### Alternative method

1. Download the Kernel Simulator binary files [here](https://github.com/EoflaOE/Kernel-Simulator/releases).
2. Unzip the file to any directory
3. Run it by double-clicking `Kernel Simulator.exe`

### Linux systems

#### Recommended method (Ubuntu)

1. Open the terminal, and execute `sudo add-apt-repository ppa:eofla/kernel-sim`
2. Confirm the addition of the PPA. It should update your package cache
3. Execute `sudo apt install kernel-simulator`
4. Execute `ks`

#### Alternative method

1. Download the Kernel Simulator binary files [here](https://github.com/EoflaOE/Kernel-Simulator/releases).
2. Unzip the file to any directory
3. Install the following programs:
   - Microsoft.VisualBasic.dll 10.0 (Debian and its derivatives: `sudo apt install libmono-microsoft-visualbasic10.0-cil`)
   - mono-vbnc (Debian and its derivatives: `sudo apt install mono-vbnc`)
   - libcpanel-json-xs-perl (Debian and its derivatives: `sudo apt install libcpanel-json-xs-perl`)
   - Inxi application (For hard drive probation) (Debian and its derivatives: `sudo apt install inxi libcpanel-json-xs-perl`)
4. Open terminal to the directory that contains KS, and run it using `mono "Kernel Simulator.exe"`

### macOS systems

#### Recommended method

1. Download the Kernel Simulator binary files [here](https://github.com/EoflaOE/Kernel-Simulator/releases).
2. Unzip the file to any directory
3. Install the following programs:
   - [Mono Runtime](https://www.mono-project.com/download/stable/#download-mac)
   - [iTerm2](https://iterm2.com/downloads.html)
4. Open terminal to the directory that contains KS, and run it using `mono "Kernel Simulator.exe"`

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
7. Make sure that you're building `Kernel Simulator.sln` as `KS.DotNetSdk.sln` is not ready yet
8. Click on the hammer button to build, the bug button (breakpoints enabled), or the Run button (breakpoints disabled - CTRL+F5 on VS). When the Edit configuration screen appears, tick the checkbox named `Use External Console`.
9. If you used the hammer button, then open your file explorer, go to the build directory, and double-click on the executable file.

### MonoDevelop

1. Install Mono Runtime, `libmono-microsoft-visualbasic10.0-cil`, and MonoDevelop.
2. After installation, extract the source code, open MonoDevelop, and click on `Open...` to navigate to `Kernel Simulator.sln`
3. Click on the `Build` menu bar, and click on build button to compile.
4. In your file manager, go to the build directory and then double-click on the executable file.

## Packing for distribution

The packing and distribution procedures are now easier by executing this script below on the command line.

1. Open the terminal to the root directory of KS
2. Execute `./buildandpack.sh` if you're using Linux or execute `buildandpack` on `cmd` if running on Windows

## Credits

| Credits to           | For
|----------------------|--------------------
| EoflaOE              | Owner of Kernel Simulator
| OpenWeatherMap       | Weather API
| jonasjacek           | [Console color data](https://jonasjacek.github.io/colors/)
| sindresorhus         | Word list
| ayu-theme            | Ayu Theme
| Ethan Schoonover     | Solarized Theme
| Fabian Neuschmidt    | [Breezy Theme](https://github.com/fneu/breezy)
| TechRepublic         | Articles RSS feed URL
| EoflaOE              | [Name databases](https://github.com/EoflaOE/NamesList)
| smashew              | [Name databases (just in case)](https://github.com/smashew/NameDatabases)
| yavuz                | [RSS feed list by country](https://github.com/yavuz/news-feed-list-of-countries/)
| All VIM theme makers | for VIM themes, such as [Darcula](https://github.com/doums/darcula), [Melange](https://github.com/savq/melange), [Papercolor](https://github.com/NLKNguyen/papercolor-theme), [SpaceCamp](https://github.com/jaredgorski/SpaceCamp), etc.
| All contributors     | Contribution

## Open Source Libraries

Below entries are the open source libraries that are used by KS and are required for execution.

### Addresstigator

Source code: https://github.com/EoflaOE/Addresstigator/

Copyright (c) 2022-present EoflaOE and its companies

License (MIT): https://github.com/EoflaOE/Addresstigator/blob/main/LICENSE.txt

### CRC32.NET

Source code: https://github.com/force-net/CRC32.NET

Copyright (c) 2017, force

License (MIT): https://github.com/force-net/Crc32.NET/blob/develop/LICENSE

### Extensification

Source code: https://github.com/EoflaOE/Extensification/

Copyright (c) 2020-present EoflaOE and its companies

License (GNU GPL 3.0 or later): https://github.com/EoflaOE/Extensification/blob/master/LICENSE

### Figgle

Source code: https://github.com/drewnoakes/figgle

Copyright (c) 2017-2021 drewnoakes

License (Apache License 2.0): https://github.com/drewnoakes/figgle/blob/master/LICENSE

### FluentFTP

Source code: https://github.com/robinrodricks/FluentFTP

Copyright (c) 2011-2016, J.P. Trosclair

Copyright (c) 2016-present, Robin Rodricks

License (MIT): https://github.com/robinrodricks/FluentFTP/blob/master/LICENSE.TXT

### HtmlAgilityPack

Source code: https://github.com/zzzprojects/html-agility-pack/

Copyright (c) ZZZ Projects Inc. 2014 - 2021. All rights reserved.

License (MIT): https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE

### Inxi.NET

Source code: https://github.com/EoflaOE/Inxi.NET/

Copyright (c) 2020-present EoflaOE and its companies

License (GNU GPL 3.0 or later): https://github.com/EoflaOE/Inxi.NET/blob/master/LICENSE

### MailKit

Source code: https://github.com/jstedfast/MailKit/

Copyright (c) 2013-present, .NET Foundation and Contributors

License (MIT): https://github.com/jstedfast/MailKit/blob/master/LICENSE

### ManagedWeatherMap

Source code: https://github.com/EoflaOE/ManagedWeatherMap/

Copyright (c) 2021-present EoflaOE and its companies

License (MIT): https://github.com/EoflaOE/ManagedWeatherMap/blob/main/LICENSE.txt

### Microsoft.AspNet.WebApi.Client

Source code: https://github.com/aspnet/aspnetwebstack

Copyright (c) .NET Foundation. All rights reserved.

License (Apache License 2.0): https://github.com/aspnet/AspNetWebStack/blob/main/LICENSE.txt

### Newtonsoft.Json

Source code: https://github.com/JamesNK/Newtonsoft.Json

Copyright (c) 2007, James Newton-King

License (MIT): https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md

### Nuget.Build.Tasks.Pack (build dependency)

Source code: https://github.com/NuGet/NuGet.Client

Copyright (c) .NET Foundation. All rights reserved.

License (Apache 2.0): https://github.com/NuGet/NuGet.Client/blob/dev/LICENSE.txt

### ReadLine.Reboot

Source code: https://github.com/EoflaOE/ReadLine.Reboot/

Copyright (c) 2017 Toni Solarin-Sodara

Copyright (c) 2022-present EoflaOE and its companies

License (MIT): https://github.com/EoflaOE/ReadLine.Reboot/blob/master/LICENSE

### SSH.NET

Source code: https://github.com/sshnet/SSH.NET/

Copyright (c) Renci

License (MIT): https://github.com/sshnet/SSH.NET/blob/develop/LICENSE

### StringMath

Source code: https://github.com/miroiu/string-math

Copyright (c) Miroiu Emanuel

License (MIT): https://github.com/miroiu/string-math/blob/dev/LICENSE

## License

    Kernel Simulator - Simulates our future planned Kernel
    Copyright (C) 2018-2022  EoflaOE

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

