## Brief overview of mail shell

The mail shell, invoked by `mail`, is one of the shells available in the kernel that provides you basic mail functions, such as reading mail, reading encrypted mail, sending mail, listing mail, and so on. It supports folder management.

For more information about every command, click the command.

### All mail commands

| Command                                             | Description
|:----------------------------------------------------|:------------
| [cd](commands/mail/KS-Mail-Command-cd.md)           | Changes directory to another mail folder.
| [exit](commands/mail/KS-Mail-Command-exit.md)       | Exits the mail shell and returns to shell.
| [list](commands/mail/KS-Mail-Command-list.md)       | Lists messages. You can use page number to navigate your messages.
| [lsdirs](commands/mail/KS-Mail-Command-lsdirs.md)   | Lists directories.
| [mkdir](commands/mail/KS-Mail-Command-mkdir.md)     | Makes a mail directory
| [mv](commands/mail/KS-Mail-Command-mv.md)           | Moves a message to another directory.
| [mvall](commands/mail/KS-Mail-Command-mvall.md)     | Moves all messages from a specified sender to another directory.
| [read](commands/mail/KS-Mail-Command-read.md)       | Reads a message.
| [readenc](commands/mail/KS-Mail-Command-readenc.md) | Reads an encrypted message. You should have the sender's GPG public key.
| [ren](commands/mail/KS-Mail-Command-ren.md)         | Renames a directory.
| [rm](commands/mail/KS-Mail-Command-rm.md)           | Removes a message.
| [rmall](commands/mail/KS-Mail-Command-rmall.md)     | Removes all messages from a specified sender.
| [rmdir](commands/mail/KS-Mail-Command-rmdir.md)     | Removes a mail directory
| [send](commands/mail/KS-Mail-Command-send.md)       | Sends mail.
| [sendenc](commands/mail/KS-Mail-Command-sendenc.md) | Sends encrypted mail. You should have your own private key, and the recipient should have your public key.
