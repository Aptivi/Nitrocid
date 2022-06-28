## sshcmd command

### Summary

You can interact with the Secure SHell server (SSH) to remotely execute commands on the host of another PC.

### Description

Secure SHell server (SSH) is a type of server which lets another computer connect to it to run commands in it. In the recent iterations, it is bound to support X11 forwarding. Our implementation is pretty basic, and uses the SSH.NET library by Renci.

This command lets you connect to another computer to remotely execute commands.

### Command usage

* `sshcmd <address:port> <username> <command>`

### Examples

* `sshcmd 192.168.1.102 EoflaOE "rm ~/test.txt"`: Connects to 192.168.1.102 with port 22 using EoflaOE to execute command "rm ~/test.txt"
* `sshcmd 127.0.0.1:2233 Michele "touch ~/home.txt"`: Connects to 127.0.0.1 with port 2233 using Michele o execute command "touch ~/home.txt"
