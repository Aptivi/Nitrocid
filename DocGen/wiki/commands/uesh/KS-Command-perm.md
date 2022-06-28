## perm command

### Summary

Manages permissions for your user or other users

### Description

If you want to manage permissions for your user or other users, or if you want to prevent the user from being logged on, use this command.

This command lets you manage permissions whether the administrative privileges are on or off, or if the user is disabled or not.

### Command usage

* `perm <userName> <Administrator/Disabled> <Allow/Disallow>`

### Examples

* `perm joe Administrator Allow`: It enables "joe" to access administrative commands
* `perm joe Disabled Allow`: It causes "joe" to be suspended
* `perm joe Administrator Disallow`: It disables "joe" to access administrative commands
* `perm joe Disabled Disallow`: It enables "joe" to be logged on