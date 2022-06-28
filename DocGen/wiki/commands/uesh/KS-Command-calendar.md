## calendar command

### Summary

Manages your calendar

### Description

This is a master application for the calendar that not only it shows you the calendar, but also shows and manages the events and reminders.

### Command usage

* `calendar <show> [year] [month]`
* `calendar <event> <add> <date> <title>`
* `calendar <event> <remove> <eventid>`
* `calendar <event> <list>`
* `calendar <event> <saveall>`
* `calendar <reminder> <add> <dateandtime> <title>`
* `calendar <reminder> <remove> <reminderid>`
* `calendar <reminder> <list>`
* `calendar <reminder> <saveall>`

### Examples

* `calendar show`: Shows the current calendar in the current year and month
* `calendar show 2018 2`: Shows the calendar of February 2018
* `calendar event add 5/21/2022 "Elaine's 20th birthday"`: Adds the event of "Elaine's 20th birthday" and sets it to May 21, 2022.
* `calendar remove 1`: Removes the first event
* `calendar reminder add "7/1/2022 6:30 PM" "Meet Agustin"`: Adds the reminder of "Meet Agustin" and sets it to July 1, 2022 at 6:30 PM
