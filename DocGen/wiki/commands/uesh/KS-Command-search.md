## search command

### Summary

Searches for a string in a specified file

### Description

Searching for strings in files is a common practice to find messages, unused messages, and hidden messages in files and executables, especially games. The command is found to make this practice much easier to access. It searches for a specified string in a specified file, and returns all matches.

### Command usage

* `search <regexp> <file>`

### Examples

* `search "Debug Menu" "DEBUG NFS.ELF"`: Searches for "Debug Menu" in a file "DEBUG NFS.ELF"
* `search ^.*(\s([a-zA-Z]+\s)+)[a-zA-Z]+\.$ "hello.txt"`: Searches for this regular expression in a file "hello.txt"