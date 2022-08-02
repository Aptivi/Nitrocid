
# Linux

*How to install the **Kernel Simulator**.*

<br>

## Recommended

*The following steps are for **Ubuntu**.*

<br>

1.  Open a new terminal.

    <br>

2.  Add the **PPA** with:

    ```shell
    sudo add-apt-repository ppa:eofla/kernel-sim
    ```
    
    <br>

2.  Confirm the addition of the **PPA**.

    *It should update your package cache.*

    <br>

3.  Install the package with:

    ```shell
    sudo apt install kernel-simulator
    ```
    
    <br>

4.  Start the simulator with:

    ```shell
    ks
    ```

<br>
<br>

## Alternative

*The commands given are meant* <br>
*for **Debian** related distributions.*

<br>

1.  **[Download]** the Kernel Simulator binaries.

    <br>

2.  Unzip the archive to any directory

    <br>

3.  Install the following programs:
    
    <br>
   
    -   `Microsoft.VisualBasic.dll` 10.0
    
        ```shell
        sudo apt install libmono-microsoft-visualbasic10.0-cil
        ```
        
        <br>
   
    -   **mono-vbnc**
    
        ```shell
        sudo apt install mono-vbnc
        ```
        
        <br>
   
    -   **libcpanel-json-xs-perl**
        
        ```shell
        sudo apt install libcpanel-json-xs-perl
        ```
        
        <br>
   
    -   **Inxi Application**
    
        *For hard drive probation*
        
        ```shell
        sudo apt install inxi libcpanel-json-xs-perl
        ```
    
    <br>

4.  Open a new terminal in the directory <br>
    that contains **KS**, and start it with:

    ```shell
    mono "Kernel Simulator.exe"
    ```

<br>


<!----------------------------------------------------------------------------->

[Download]: https://github.com/Aptivi/Kernel-Simulator/releases
