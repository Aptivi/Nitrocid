## del (S)FTP command

### Summary

Removes files or folders

### Description

If you have logged in to a user that has administrative privileges, you can remove unwanted files, or extra folders, from the server.

If you deleted a file while there is transmissions going on in the server, people who tries to get the deleted file will never be able to download it again after their download fails.

### Command usage

* `del <file/directory>`

### Examples

* `del unwanted.txt`: Delete the file named "unwanted.txt"
* `del InactiveDir`: Delete the folder named "InactiveDir"