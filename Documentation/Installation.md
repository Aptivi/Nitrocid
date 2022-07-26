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
