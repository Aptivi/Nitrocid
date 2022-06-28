## speedpress command

### Summary

Launches the speed press game

### Description

This game will test your keystroke speed. It will only give you very little time to press a key before moving to the next one.

| Switches | Description
|:---------|:------------
| -e       | Easy difficulty
| -m       | Medium difficulty
| -h       | Hard difficulty
| -v       | Very Hard difficulty
| -c       | Custom difficulty. The timeout should be specified

### Command usage

* `speedpress [-e|-m|-h|-v|-c] [timeout]`

### Examples

* `speedpress`: Starts the speedpress game using default settings
* `speedpress -h`: Starts the speedpress game in Hard difficulty
* `speedpress -c 250`: Starts the speedpress game in custom difficulty with the timeout of 250 milliseconds