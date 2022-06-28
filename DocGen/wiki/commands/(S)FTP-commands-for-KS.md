## Commands for (S)FTP client for KS

The commands below are available for the (S)FTP client that is built-in to KS. Some of the commands are available only for FTP shells.

For more information about every command, click on the command.

### Administrative commands

| Command                                                        | Description
|:---------------------------------------------------------------|:------------
| [cp](commands/sftp/ftponly/KS-FTP-Command-cp.md)               | Copies a file to the server
| [del](commands/sftp/KS-(S)FTP-Command-del.md)                  | You can delete unwanted files from the server
| [mv](commands/sftp/ftponly/KS-FTP-Command-mv.md)               | Moves a remote file to another remote directory
| [put](commands/sftp/KS-(S)FTP-Command-put.md)                  | Send your local file to the server
| [putfolder](commands/sftp/ftponly/KS-FTP-Command-putfolder.md) | Send your local folder to the server
| [perm](commands/sftp/ftponly/KS-FTP-Command-perm.md)           | Sets file permissions. This only works on FTP servers that run on Unix.

### Normal user commands

| Command                                                         | Description
|:----------------------------------------------------------------|:------------
| [pwdl](commands/sftp/KS-(S)FTP-Command-pwdl.md)                 | Get your current working local directory
| [pwdr](commands/sftp/KS-(S)FTP-Command-pwdr.md)                 | Get your current working server directory
| [cdl](commands/sftp/KS-(S)FTP-Command-cdl.md)                   | Changes your local directory to another directory that exists
| [cdr](commands/sftp/KS-(S)FTP-Command-cdr.md)                   | Changes your remote directory to another directory that exists in the server
| [connect](commands/sftp/KS-(S)FTP-Command-connect.md)           | Lets you communicate with the server, and with the credentials if specified
| [disconnect](commands/sftp/KS-(S)FTP-Command-disconnect.md)     | When you're finished communicating with the server, use this command to let the server know that you're leaving
| [get](commands/sftp/KS-(S)FTP-Command-get.md)                   | Gets the remote file from the server to your current local working directory
| [getfolder](commands/sftp/ftponly/KS-FTP-Command-getfolder.md)  | Gets the remote folder from the server to your current local working directory
| [lsl](commands/sftp/KS-(S)FTP-Command-lsl.md)                   | Lists the contents of your current working directory, or specified directory
| [lsr](commands/sftp/KS-(S)FTP-Command-lsr.md)                   | Lists the contents of your current working server directory, or specified directory
| [quickconnect](commands/sftp/KS-(S)FTP-Command-quickconnect.md) | Use the speed dial to quickly connect to any FTP server. Automatically filled when connecting to a server for the first time.
| [type](commands/sftp/ftponly/KS-FTP-Command-type.md)            | Changes the data type (ASCII, Binary)
