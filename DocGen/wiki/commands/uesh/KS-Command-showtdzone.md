## showtdzone command

### Summary

Shows current time and date in another timezone

### Description

If you need to know what time is it on another city or country, you can use this tool to tell you the current time and date in another country or city.

This command is multi-platform, and uses the IANA timezones on Unix systems and the Windows timezone system on Windows.

For example, if you need to use "Asia/Damascus" on the Unix systems, you will write `showtdzone Asia/Damascus`." However on Windows 10, assuming we're on the summer season, you write `showtdzone "Syria Daylight Time"`

| Switches | Description
|:---------|:------------
| -all     | Displays all timezones and their times and dates

### Command usage

* `showtdzone [-all] <TimeZone>`

### Examples

* `showtdzone "Egypt Standard Time"`: Displays the current time and date in Egypt on Windows systems
* `showtdzone Asia/Hong_Kong`: Displays the current time and date in Hong Kong on Linux systems
* `showtdzone -all`: Displays all timezones and their times and dates