## setrange command

### Summary

Makes a UESH array and sets their values

### Description

If you want to store a group of values in one variable, you can use this command to create arrays of values. Such variables will have the `[n]` suffix, for example, `$values[1]`.

### Command usage

* `setrange <$variablename> <value1> [value2] [value3] ...`

### Examples

* `setrange $vars One Two Three Four`: Makes an array of `$vars` holding the four values
