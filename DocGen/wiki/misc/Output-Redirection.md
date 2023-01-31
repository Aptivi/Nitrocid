# Output redirection

## How it works

Output redirection allows you to pipe the output of a command that needs to be saved to a file so you can use it later. For example, if you wanted to save an output for `ls -l` command in Linux, you would type it like this: `ls -l > file.txt`.

In Unix systems, not only it allows you to redirect an output of a command to a file, but it also allows you to redirect it to a device like COM1, COM2, and so on. For example, there is a `/dev` blob that allows you to play sound just by piping the contents of a sound file to the device, and can be done in the `root` user like this: `cat sound.wav > /dev/dsp`. It can be done if the Linux kernel supports that device blob.

## In Nitrocid KS

Currently, you can pipe the output of the command to a file like this: `list >>>list.txt`. It only supports `Console.Out`, so it doesn't work when piping an output coming from `Console.Err`.

> [!CAUTION]
> You can't use this on some of the commands that require user interaction or commands that never generate output. If you did it to an interactive command like `mail`, `ftp`, and so on, you may find that it requires an input, though the fields don't print to the console, causing you to go to the file that holds console output, read the required field, and write the answer down. It gets even worse when the command lets you enter a separate shell for its functions like `edit`, `ftp`, `mail`, and so on.

Here are two ways to pipe the output of a command to the file:

| Syntax      | Description
|:------------|:------------
| ` >> file`  | It overwrites the output file and stores the output of the command to it
| ` >>> file` | It appends the output to the output file
| ` |SILENT|` | It throws all output away
