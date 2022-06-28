## unzip command

### Summary

Extracts a ZIP file

### Description

If you wanted to extract the contents of a ZIP file, you can use this command to gain access to the compressed files stored inside it.

| Switches | Description
|:-----------|:------------
| -createdir | Extracts the archive to the new directory that has the same name as the archive

### Command usage

* `unzip <zipfile> [path] [-createdir]`

### Examples

* `unzip "Operating systems.zip"`: Extracts the contents of "Operating systems.zip" to the current working directory
* `unzip Mountains.zip -createdir`: Extracts the contents of Mountains.zip to a directory called Mountains in the current working directory