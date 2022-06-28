## mail command

### Summary

Opens the IMAP shell

### Description

This command is an entry point to the IMAP shell that lets you view and list messages. The shell is currently limited, but will eventually get improved in future releases.

If no address is specified, it will prompt you for the address, password, and the mail server (IMAP). Currently, it connects with necessary requirements to ensure successful connection.

List of some IMAP addresses (majority have the port of 993, and is encrypted using SSL):

- imap.gmail.com
- imap-mail.outlook.com
- imap.mail.com
- outlook.office365.com
- imap.mail.yahoo.com

### Command usage

* `mail [emailAddress]`

### Examples

* `mail john.smith@mail.com`: Opens the IMAP shell and tries to log in to john.smith@mail.com
