## select command

### Summary

Lets the user make a selection

### Description

This command can be used in a scripting file that ends in .uesh file extension. It lets the user select the correct answers when answering this question and passes the chosen answer to the specified variable.

### Command usage

* `select <$variable> <answers> <input> [answertitle1] [answertitle2] ...`

### Examples

* `select $answer y/n "Do you want to answer this question?"`: Lets the user select the specified question, with two answers, and passes the correct answer into $answer.
* `select $answer y/n "Do you want to answer this question?" Yes No`: Lets the user select the specified question, with two answers, and passes the correct answer into $answer. The answer titles are set.
