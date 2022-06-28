## rexec command

### Summary

Remote execution functionality

### Description

You can remotely execute a command to another instance of KS running in another PC. They must maintain a stable Internet connection in order to be able to send/receive requests.

Another KS instance in another PC must listen for requests running on port 12345.

### Command usage

* `rexec <IPAddress> <command>`

### Examples

* `rexec 192.168.1.100 update`: It lets KS that is on 192.168.1.100 address run the command `update`.
* `rexec 192.168.1.104 help`: It lets KS that is on 192.168.1.104 address run the command `help`