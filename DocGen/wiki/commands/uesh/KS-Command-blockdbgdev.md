## blockdbgdev command

### Summary

You can block an IP address from entering the remote debugger.

### Description

If you wanted to moderate the remote debugger and block a device from joining it because it either causes problems or kept flooding the chat, you may use this command to block such offenders.

This command is available to administrators only. The blocked device can be unblocked using the `unblockdbgdev` command.

### Command usage

* `blockdbgdev <address>`

### Examples

* `blockdbgdev 192.168.1.108`: Blocks a local device whose IP address is 192.168.1.108 from joining the remote debugger.
* `blockdbgdev 166.153.172.123`: Blocks a remote device whose IP address is 166.153.172.123 from joining the remote debugger.