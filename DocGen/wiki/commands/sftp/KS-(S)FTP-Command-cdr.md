## cdr (S)FTP command

### Summary

Changes your remote directory

### Description

This command lets you change your remote directory in your connected FTP server to another directory that exists in the subdirectory. However, when specifying .., it goes backwards.

### Command usage

* `cdr <directory/..>`

### Examples

* cdr X11/GNOME: This will change your remote directory to X11/GNOME
* cdr pub: This will change your remote directory to pub in your current working directory
* cdr ..: This will go to a parent directory for the current working directory (go back)