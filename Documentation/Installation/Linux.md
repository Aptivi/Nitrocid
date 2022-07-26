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
