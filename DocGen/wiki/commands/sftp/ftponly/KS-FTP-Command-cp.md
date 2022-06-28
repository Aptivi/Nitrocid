## cp FTP command

### Summary

Copies a file or directory to another destination in the server

### Description

If you manage the FTP server and wanted to copy a file or a directory from a remote directory to another remote directory, use this command. It might take a few minutes depending on the server, because it downloads the file to a temporary directory and uploads the file to another destination.

This is because FluentFTP doesn't support `.CopyFile(Source, Destination)` yet.

### Command usage

* `cp <source> <destination>`

### Examples

* `cp Linux Systems`: Copies the Linux folder to the Systems folder
* `cp shanghai.png Cities`: Copies the shanghai.png to the Cities folder