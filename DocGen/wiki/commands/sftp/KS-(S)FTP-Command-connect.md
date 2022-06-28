## connect (S)FTP command

### Summary

Connects your (S)FTP client to any (S)FTP server that is valid

### Description

This command must be executed before running any interactive (S)FTP server commands, like download, upload, cdl, cdr, etc.

This command opens a new session to connect your (S)FTP client to any (S)FTP server that is open to the public, and valid. It then asks for your credentials. Try with anonymous first, then usernames.

### Command usage

* `connect <address>`

### Examples

* `connect ftp.fabrikam.com`: Connects your FTP client to the server named "ftp.fabrikam.com"
* `connect sftp.fabrikam.com`: Connects your SFTP client to the server named "ftp.fabrikam.com"