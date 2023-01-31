## settings command

### Summary

Lets you change kernel settings

### Description

This command starts up the Settings application, which allows you to change the kernel settings available to you. It's the successor to the defunct `Nitrocid KS Configuration Tool` application, and is native to the kernel.

It starts with the list of sections to start from. Once the user selects one, they'll be greeted with various options that are configurable. When they choose one, they'll be able to change the setting there.

If you just want to try out a setting without saving to the configuration file, you can change a setting and exit it immediately. It only survives the current session until you decide to save the changes to the configuration file.

Some settings allow you to specify a string, a number, or by the usage of another API, like the `ColorWheel()` tool.

In the string or long string values, if you used the `/clear` value, it will blank out the value. In some settings, if you just pressed ENTER, it'll use the same value that the kernel uses at the moment.

We've made sure that this application is user-friendly.

For the screensaver and splashes, refer to the command usage below.

| Switches | Description
|:---------|:------------
| -saver   | Opens the screensaver settings
| -splash  | Opens the splash settings

### Command usage

* `settings [-saver|-splash]`

### Examples

* `settings`: Opens the kernel settings
* `settings -saver`: Opens the screensaver settings
* `settings -splash`: Opens the splash settings
