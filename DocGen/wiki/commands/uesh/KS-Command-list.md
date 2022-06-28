## list command

### Summary

You can list contents inside the current directory, or specified folder

### Description

If you don't know what's in the directory, or in the current directory, you can use this command to list folder contents in the colorful way.

| Switches | Description
|:------------------|:------------
| -showdetails      | Shows the details of the files and folders
| -suppressmessages | Suppresses the "unauthorized" messages

### Command usage

* `list [-showdetails|-suppressmessages] [directory]`

### Examples

* `list`: This command lists the current working directory contents
* `list ..`: This command lists the parent folder's contents from the current working directory
* `list boot`: This command lists the "boot" folder from the current working directory, assuming the working directory is /