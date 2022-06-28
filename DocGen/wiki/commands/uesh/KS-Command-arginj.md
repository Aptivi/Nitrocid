## arginj command

### Summary

You can set the arguments to launch at reboot.

### Description

If you need to reboot your kernel to run the debugger, or if you want to disable hardware probing to save time when booting, then this command is for you. It allows you to set arguments so they will be run once at each reboot.

You can use this command if you need to inject arguments while on the kernel. You can also separate many arguments by spaces so you don't have to run arguments one by one to conserve reboots.

### Command usage

* `arginj [Arguments separated by spaces]`

### Examples

* `arginj debug`: Turn on debug mode at the next reboot
* `arginj nohwprobe`: Turn off hardware probing at the next reboot
* `arginj quiet nohwprobe`: Turn off kernel messages and hardware probing at the next reboot