## alias command

### Summary

You can set an alternative shortcut to the command if you want to use shorter words for long commands.

### Description

Some commands in this kernel are long, and some people doesn't write fast on computers. The alias command fixes this problem by providing the shorter terms for long commands.

You can also use this command if you plan to make scripts if the real file system will be added in the future, or if you are rushing for something and you don't have time to execute the long command.

You can add or remove the alias to the long command.

### Command usage

* `alias add <aliastype> <alias> <cmd>`
* `alias rem <aliastype> <alias>`

### Examples

* `alias add Shell stdz showtdzone`: This will create a shortcut of "showtdzone" by the name of "stdz"
* `alias add Shell turnoff shutdown`: This will create a shortcut of "shutdown" by the name of "turnoff"
* `alias add FTPShell ? help`: This will create a shortcut of "help" with the symbol of "help"
* `alias rem Shell signout`: This will remove a shortcut that is named "signout" by the command "logout"
* `alias rem Shell savescrn`: This will remove a shortcut that is named "savescrn" by the command "savescreen"
* `alias rem Shell clearscrn`: This will remove a shortcut that is named "clearscrn" by the command "cls"
* `alias rem Shell lckscrnsvr`: This will remove a shortcut that is named "lckscrnsvr" by the command "lockscreen"