## update command

### Summary

Checks for the kernel update

### Description

It checks for the kernel update by fetching the release information from [GitHub](http://github.com/Aptivi/NitrocidKS), taking the latest release group, and comparing between versions. It will not download the update automatically, only checks for it.

To download the update, download it using the link provided in this command, shutdown the kernel using `shutdown` command, and extract it to the executable directory, overwriting the old version with the latest version.

### Command usage

* `update`