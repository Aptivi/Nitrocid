## sumfile command

### Summary

Calculates the sum of a file

### Description

Calculating the hash sum of files is important, because it lets users verify if the file is corrupt or not. It calculates the sum of a file using either the MD5, SHA1, SHA256, or SHA512 algorithms.

### Command usage

* `sumfile <algorithm> <file> [outputFile]`

### Examples

* `sumfile MD5 DefenseShield.png`: Calculates the MD5 sum of an image that has the defensive shield.
* `sumfile SHA256 plasma-desktop_5.16.5-0ubuntu1_amd64.deb plasma.txt`: Calculates the SHA256 sum of the Ubuntu/Debian package of KDE Plasma Desktop and outputs it to plasma.txt in the current working directory