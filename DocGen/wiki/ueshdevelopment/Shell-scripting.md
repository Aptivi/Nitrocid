## Shell scripting

Since 0.0.10, the shell scripting support has been added to automate the sets of steps used. It can handle variables and comments and are treated as such. It can also be used to make scripts ranging from the Hello World script to the most complicated one.

> [!WARNING]
> It doesn't support making functions yet, but it will be supported in the near future. Meanwhile, here's the basic guide on how to start the UESH scripting.

## How to make scripts?

1. Create an empty text file, but make the extension `.uesh`
2. Open it using any text editor
3. Write any valid KS command. Make sure that you separate between commands using the newlines for readability.
4. If you need to put comments, put a `#` in the beginning of each line.
5. Save the file. Now, run KS, and write the file name with `.uesh`
6. Your script should run.

## Scripting format

Any normal command that you would normally execute in the shell of Nitrocid KS can be written in the file as if you're writing a command. It supports the arguments. It can be written like below:

```
echo Hello World!
```

However, if you need to comment on something, it should be written in the newline as in the below form:

```
# A script that prints "Hello World"
```

Also, every line that starts with the space or with the `#` is considered as a comment, and they will not be executed as commands.
