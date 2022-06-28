## convertlineendings command

### Summary

Converts the line endings

### Description

If you have a text file that needs a change for its line endings, you can use this command to convert the line endings to your platform's format, or the format of your choice by using these switches:

| Switches | Description
|:---------|:------------
| -w       | Converts the line endings to the Windows format (CR + LF)
| -u       | Converts the line endings to the Unix format (LF)
| -m       | Converts the line endings to the Mac OS 9 format (CR)

### Command usage

* `convertlineendings <textfile> [-w|-u|-m]`

### Examples

* `convertlineendings text.txt`: Converts the text file to your platform's newline style
* `convertlineendings text.txt -u`: Converts the text file to the Unix format
