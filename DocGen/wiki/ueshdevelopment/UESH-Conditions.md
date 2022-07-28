# UESH conditions

## What are the UESH conditions?

UESH conditions are expressions that check to see if the target variables satisfy them, and if they do, will return the test as successful.

For example, take "$count eq 5". If the $count variable equals 5, the condition is satisfies. If it doesn't, it isn't satisfied.

> [!WARNING]
> Currently, only the `if` command supports this feature.

### Available conditions

| Condition  | Syntax                  | Description
|:-----------|:------------------------|:------------
| eq         | `$var eq $var2`         | The first variable equals the second
| neq        | `$var neq $var2`        | The first variable doesn't equal the second
| les        | `$var les $var2`        | The first variable is less than the second
| gre        | `$var gre $var2`        | The first variable is greater than the second
| lesoreq    | `$var lesoreq $var2`    | The first variable is less than or equal to the second
| greoreq    | `$var greoreq $var2`    | The first variable is greater than or equal to the second
| fileex     | `fileex $path`          | The file exists
| filenex    | `filenex $path`         | The file doesn't exist
| direx      | `direx $path`           | The directory exists
| dirnex     | `dirnex $path`          | The directory doesn't exist
| has        | `$var has $path`        | The first variable contains the second
| hasno      | `$var hasno $path`      | The first variable doesn't contain the second
| ispath     | `$path ispath`          | The first variable is a path
| isnotpath  | `$path isnotpath`       | The first variable is not a path
| isfname    | `$path isfname`         | The first variable is a filename
| isnotfname | `$path isnotfname`      | The first variable is not a filename
| sane       | `$string1 $hash sane`   | The first variable's SHA256 hash is the same as the specified hash
| insane     | `$string1 $hash insane` | The first variable's SHA256 hash is not the same as the specified hash
| fsane      | `$path $hash fsane`     | The file's SHA256 hash is the same as the specified hash
| finsane    | `$path $hash finsane`   | The file's SHA256 hash is not the same as the specified hash
