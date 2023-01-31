# Kernel arguments for KS

## How to run arguments? and how do I make the prompt appear?

An argument to the kernel is the option of which the kernel or parts of it will behave differently based on the options you have chosen. It will be run on the start of the kernel, and if no arguments were specified, the kernel will run normally.

You can run arguments on the next reboot by using the `arginj` command when on shell.

You can make the prompt appear in two methods:

1. By changing config option `Prompt for Arguments on Boot` to True (For more information about config, see [Kernel Configuration](../config/Configuration-for-KS.md))
2. By running Nitrocid KS with the promptArgs cmdline argument (eg. `"Nitrocid KS.exe" promptArgs`)

You can separate multiple argument with commas without spaces. For example, `quiet,safe`

### Useful arguments

| Argument        | Description
|:----------------|:------------
| safe            | Disables custom mods and screensavers. Can be used to check if the mods are causing trouble.
| quiet           | Don't let the kernel say anything on boot
| cmdinject       | Auto-run commands when you're logged in. You can separate multiple commands using a colon with spaces (Ex. `cmdinject setthemes Bluespire : arginj debug`)
| debug           | Enables the diagnostic messages on the kernel to allow debugging. You can use it to send a bug report to us. It saves to a file.
| maintenance     | Same as safe mode, except that it's restricted to the root user only.
| testInteractive | Enters the test shell
