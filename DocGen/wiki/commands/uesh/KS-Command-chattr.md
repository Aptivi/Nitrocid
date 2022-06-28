## chattr command

### Summary

Changes attributes of file

### Description

You can use this command to change attributes of a file. Currently, it only supports these:

- Normal: The file is a normal file
- ReadOnly: The file is a read-only file
- Hidden: The file is a hidden file
- Archive: The file is an archive. Used for backups.

### Command usage

* `chattr <filename> -/+<attribute>`

### Examples

* `chattr ToBeHidden.txt +Hidden`: Adds the hidden attribute to the file.
* `chattr ToUnhide.txt -Hidden`: Removes the hidden attribute from the file.