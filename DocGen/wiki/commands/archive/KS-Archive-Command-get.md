## get Archive command

### Summary

Extract a file from an archive

### Description

If you want to get a single file from an archive, you can use this command to extract such file to the current working directory, or a specified directory.

| Switches | Description
|:----------|:------------
| -absolute | Uses the full target path

### Command usage

* `get <entry> [where] [-absolute]`

### Examples

* `get Linux/KNOPPIX7.iso`: Extracts Linux/KNOPPIX7.iso
* `get Windows/Windows10.iso "Windows 10"`: Extracts Windows/Windows10.iso to a folder called "Windows 10"
