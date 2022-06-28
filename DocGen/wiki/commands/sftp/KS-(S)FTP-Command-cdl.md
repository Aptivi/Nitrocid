## cdl (S)FTP command

### Summary

Changes your local directory

### Description

This command lets you change your local directory in your hard drives to another directory that exists in the subdirectory. However, when specifying .., it goes backwards.

### Command usage

* `cdl <directory/..>`

### Examples

* cdl C:\Users\Owner\Desktop: This will change your local directory to C:\Users\Owner\Desktop
* cdl Stuff: This will change your local directory to Stuff in your current working directory
* cdl ..: This will go to a parent directory for the current working directory (go back)