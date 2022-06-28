## chhostname command

### Summary

You can change your host name to another name

### Description

If you're planning to change your host name to another name, this command is for you.

This command used to change your host name and resets it everytime you reboot the kernel, but now it stores it in the config file as soon as you change your host name.

This version of the kernel finally allows hostnames that is less than 4 characters.

This command is also useful if you're identifying multiple computers/servers, so you won't forget them.

### Command usage

* `chhostname <name>`

### Examples

* `chhostname joe`: It changes your host name to "joe"
* `chhostname comp-1`: It changes your host name to "comp-1"