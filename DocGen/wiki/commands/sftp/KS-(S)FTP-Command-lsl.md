## lsl (S)FTP command

### Summary

Lists the contents of the current folder or the folder provided

### Description

You can see the list of the files and sub-directories contained in the current working directory if no directories are specified, or in the specified directory, if specified.

You can also see the list of the files and sub-directories contained in the previous directory of your current position.

| Switches | Description
|:------------------|:------------
| -showdetails      | Shows the details of the files and folders
| -suppressmessages | Suppresses the "unauthorized" messages

### Command usage

* `lsl [-showdetails|-suppressmessages] [folder/..]`

### Examples

* `lsl`: Lists the contents in the working directory
* `lsl ..`: Lists the contents in the previous working directory
* `lsl cities`: Lists the contents of "cities" folder
