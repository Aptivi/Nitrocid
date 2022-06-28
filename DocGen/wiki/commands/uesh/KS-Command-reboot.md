## reboot command

### Summary

Restarts the kernel

### Description

This command restarts your simulated kernel and reloads all the config that are not loaded using reloadconfig. This is especially useful if you want to change colors of text, set arguments into the kernel, inject arguments, and so on.

WARNING: There is no file system syncing because the current kernel version doesn't have the real file system to sync, and the kernel is not final.

### Command usage

* `reboot [safe/computerIP]`

### Examples

* `reboot`: Initiates a normal reboot locally
* `reboot safe`: Initiates a reboot to safe mode locally
* `reboot 192.168.1.101`: Initiates a reboot on 192.168.1.101, assuming that the target is running KS.