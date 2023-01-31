
# JetBrains Rider

*How to build with **JetBrains 64-bit**.*

<br>

## Requirements

*Install the following items:*

<br>

- `libmono-microsoft-visualbasic10.0-cil`

- **JetBrains Rider**

- **Mono Runtime**

- **Git**

<br>
<br>

## Configuration

<br>

1.  Open **JetBrains Rider**

    <br>

2.  In the main menu, choose `Check out from Version Control` » `Git`

    <br>

3.  Insert the following for the URL:

    ```
    https://github.com/Aptivi/NitrocidKS.git
    ```
    
    Press  <kbd>  Test  </kbd>  to verify your connectivity.

<br>
<br>

## Project Setup

<br>

1.  Press  <kbd>  Clone  </kbd>  to have git download the <br>
    repository and automatically open up Rider.

    *This might take a few minutes depending* <br>
    *on the speed of your Internet Connection.*
    
    <br>

2.  Ensure you're building `Nitrocid KS.sln`.

    *`KS.DotNetSdk.sln` is not ready yet*
    
    <br>

3.  Click on the 

    -   **Hammer** to build
    
    -   **Bug** to build with breakpoints enabled
    
    -   **Run** to run with breakpoints disabled
    
        <kbd>  CTRL  +  F5  </kbd>  in Visual Studio
        
    <br>

4.  When the Edit configuration screen appears, <br>
    tick the `Use External Console` checkbox.
    
    <br>

5.  If you build the project, open your file <br>
    explorer, go to the build directory and <br>
    double-click on the executable file.

<br>
