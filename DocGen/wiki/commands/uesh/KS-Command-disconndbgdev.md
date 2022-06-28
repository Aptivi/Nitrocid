## disconndbgdev command

### Summary

Disconnects debug devices

### Description

This command allows you to disconnect debug devices that are no longer needed. This will ensure that the target will not receive further debugging messages, and the debugger will notify other targets that he/she is disconnected.

### Command usage

* `disconndbgdev <ipAddress>`

### Examples

* `disconndbgdev 192.168.1.104`: Disconnects 192.168.1.104 from the debugging server