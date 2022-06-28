## choice command

### Summary

Lets the user make a choice

### Description

This command can be used in scripting file that end in .uesh file extension. It lets the user choose the correct answers when answering this question and passes the chosen answer to the specified variable.

| Switches | Description
|:----------|:------------
| -multiple | The output can be more than a character
| -single   | The output can be only one character
| -o        | One line choice style
| -t        | Two lines choice style
| -m        | Modern choice style
| -a        | Table choie style

### Command usage

* `choice [-o|-t|-m|-a] [-multiple|-single] <$variable> <a/n/s/w/e/r/s> <input> [answertitle1] [answertitle2] ...`

### Examples

* `choice $answer y/n/a Do you want to answer this question?`: Lets the user answer the specified question, with three answers, and passes the correct answer into $answer.