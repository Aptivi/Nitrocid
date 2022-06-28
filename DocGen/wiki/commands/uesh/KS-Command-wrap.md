## wrap command

### Summary

Wraps a command

### Description

You can wrap a command so it stops outputting until you press a key if the console has printed lines that exceed the console window height. Only the commands that are explicitly set to be wrappable can be used with this command.

### Command usage

* `wrap <command>`

### Examples

* wrap "cat Long.txt": Wraps an output of "cat Long.txt"
* wrap ftp: Counterexample for wrap command, which shows "command unwrappable."