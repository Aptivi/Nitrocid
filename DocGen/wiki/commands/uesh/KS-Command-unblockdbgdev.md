## unblockdbgdev command

### Summary

Lets you unblock a blocked debug device

### Description

If you wanted to let a device whose IP address is blocked join the remote debugging again, you can unblock it using this command.

### Command usage

* `unblockdbgdev <address>`

### Examples

* `unblockdbgdev 192.168.1.108`: Allows a local device whose IP address is 192.168.1.108 to join the remote debugger.
* `unblockdbgdev 166.153.172.123`: Allows a remote device whose IP address is 166.153.172.123 to join the remote debugger.