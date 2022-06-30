## put command

### Summary

Upload a file

### Description

This command uploads a file from the website to a file, preserving the file name. This is currently very basic, but it will be expanded in future releases.

### Command usage

* `put <file> <URL>`

### Examples

* `put shanghai.png http://127.0.0.1/Files/images/ user1`: Uploads an image from the current working directory after authentication with `user1`.
* `put "ShotStone OS Overview.mp4" http://ftp.fabrikam.org/Files/videos/`: Downloads the video file to the target without authentication