## zip command

### Summary

Makes a ZIP file

### Description

If you wanted to make a ZIP file containing the contents you want to compress, you can use this command.

| Switches | Description
|:-----------|:------------
| -fast      | Uses fast compression
| -nocomp    | No compression
| -nobasedir | Don't create base directory on the archive

### Command usage

* `zip <zipfile> <path> [-fast|-nocomp|-nobasedir]`

### Examples

* `zip "Operating systems.zip" "Operating systems"`: Makes an archive of "Operating systems" folder to a zip file called "Operating systems.zip", including the base directory.
* `zip Mountains.zip Mountains -nobasedir`: Makes an archive of "Mountains" folder to a zip file called "Mountains.zip", without the base directory.