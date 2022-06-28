## disconnect (S)FTP command

### Summary

Disconnects from the current working server

### Description

This command sends the quit command to the (S)FTP server so the server knows that you're going away. It basically disconnects you from the server to connect to the server again or, in the FTP shell, re-connect to the last server connected.

| Switches | Description
|:---------|:------------
| -f       | Forces disconnection (FTP only)

### Command usage

* `disconnect [-f]`