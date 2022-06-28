## List of events that KS fires

KS fires an event after its own actions which can be seen below:

|  Event                          |  When                                                    |  Arguments
|:--------------------------------|:---------------------------------------------------------|:----
|KernelStarted                    |When the kernel is started                                |-
|PreLogin                         |Fired before login                                        |-
|PostLogin                        |Fired after login                                         |Username
|LoginError                       |Fired on login error                                      |Username, Reason
|ShellInitialized                 |When the shell is initialized                             |-
|PreExecuteCommand                |Fired before command execution                            |Command
|PostExecuteCommand               |Fired after command execution                             |Command
|KernelError                      |Fired on kernel error                                     |Error Type, Reboot?, Reboot Time, Description, Exception, Arguments
|ContKernelError                  |Fired on continuable kernel error                         |Error Type, Reboot?, Reboot Time, Description, Exception, Arguments
|PreShutdown                      |Fired before shutdown                                     |-
|PostShutdown                     |Fired after shutdown                                      |-
|PreReboot                        |Fired before reboot                                       |-
|PostReboot                       |Fired after reboot                                        |-
|PreShowScreensaver               |Fired on screensaver start                                |Screensaver
|PostShowScreensaver              |Fired on screensaver finish                               |Screensaver
|PreUnlock                        |Fired before unlocking                                    |Screensaver
|PostUnlock                       |Fired after unlocking                                     |Screensaver
|CommandError                     |Fired on command error                                    |Command, Exception
|PreReloadConfig                  |Fired before config reload                                |-
|PostReloadConfig                 |Fired after config reload                                 |-
|PlaceholderParsing               |Fired while the placeholders are being parsed             |Target String
|PlaceholderParsed                |Fired after parsing placeholders                          |Target String
|GarbageCollected                 |Fired after garbage collection                            |-
|FTPShellInitialized              |When the FTP shell is initialized                         |-
|FTPPreExecuteCommand             |Fired before FTP command execution                        |Command
|FTPPostExecuteCommand            |Fired after FTP command execution                         |Command
|FTPCommandError                  |Fired on FTP command error                                |Command, Exception
|FTPPreDownload                   |Fired before an FTP file is transferred locally (download)|File
|FTPPostDownload                  |Fired after an FTP file is transferred locally (download) |File, Succeeded?
|FTPPreUpload                     |Fired before an FTP file is transferred remotely (upload) |File
|FTPPostUpload                    |Fired after an FTP file is transferred remotely (upload)  |File, Succeeded?
|IMAPShellInitialized             |When the IMAP shell is initialized                        |-
|IMAPPreExecuteCommand            |Fired before IMAP command execution                       |Command
|IMAPPostExecuteCommand           |Fired after IMAP command execution                        |Command
|IMAPCommandError                 |Fired on IMAP command error                               |Command, Exception
|RemoteDebugConnectionAccepted    |A connection is accepted                                  |Target
|RemoteDebugConnectionDisconnected|A connection is disconnected                              |Target
|RemoteDebugExecuteCommand        |Fired on remote debug command execution                   |Target, Command
|RemoteDebugCommandError          |Fired on remote debug command error                       |Target, Command, Exception
|RPCCommandSent                   |RPC command is sent                                       |Command, Argument, IP, Port
|RPCCommandReceived               |RPC command is received                                   |Command, IP, Port
|RPCCommandError                  |Fired on RPC command error                                |Command, Exception, IP, Port
|RSSShellInitialized              |When the RSS shell is initialized                         |FeedUrl
|RSSPreExecuteCommand             |Fired before RSS command execution                        |FeedUrl, Command
|RSSPostExecuteCommand            |Fired after RSS command execution                         |FeedUrl, Command
|RSSCommandError                  |Fired on RSS command error                                |FeedUrl, Command, Exception
|SFTPShellInitialized             |When the SFTP shell is initialized                        |-
|SFTPPreExecuteCommand            |Fired before SFTP command execution                       |Command
|SFTPPostExecuteCommand           |Fired after SFTP command execution                        |Command
|SFTPCommandError                 |Fired on SFTP command error                               |Command, Exception
|SFTPPreDownload                  |Fired before an SFTP file is downloaded                   |File
|SFTPPostDownload                 |Fired after an SFTP file is downloaded                    |File
|SFTPDownloadError                |Fired on SFTP download error                              |File, Exception
|SFTPPreUpload                    |Fired before an SFTP file is uploaded                     |File
|SFTPPostUpload                   |Fired after an SFTP file is uploaded                      |File
|SFTPUploadError                  |Fired on SFTP upload error                                |File, Exception
|SSHConnected                     |Connected to SSH                                          |Target
|SSHDisconnected                  |Disconnected from SSH                                     |-
|SSHError                         |Fired on SSH error                                        |Exception
|SSHPreExecuteCommand             |Fired before SSH command execution                        |Target, Command
|SSHPostExecuteCommand            |Fired after SSH command execution                         |Target, Command
|SSHCommandError                  |Fired on SSH command error                                |Target, Command, Exception
|UESHPreExecute                   |Fired vefore UESH script execution                        |Command, Arguments
|UESHPostExecute                  |Fired after UESH script execution                         |Command, Arguments
|UESHError                        |Fired on UESH error                                       |Command, Arguments, Exception
|TextShellInitialized             |When the text edit shell is initialized                   |-
|TextPreExecuteCommand            |Fired before text edit command execution                  |Command
|TextPostExecuteCommand           |Fired after text edit command execution                   |Command
|TextCommandError                 |Fired on text edit command error                          |Command, Exception
|NotificationSent                 |Fired on notification being sent                          |Notification
|NotificationsSent                |Fired on notifications being sent                         |Notification
|NotificationReceived             |Fired on notification being received                      |Notification
|NotificationsReceived            |Fired on notifications being received                     |Notification
|NotificationDismissed            |Fired on notification being dismissed                     |-
|ConfigSaved                      |Fired when config is saved                                |-
|ConfigRead                       |Fired when config is read                                 |-
|ConfigSaveError                  |Fired on config save error                                |Exception
|ConfigReadError                  |Fired on config read error                                |Exception
|PreExecuteModCommand             |Fired before mod command execution                        |Command
|PostExecuteModCommand            |Fired after mod command execution                         |Command
|ModParsed                        |Fired when the mod is parsed                              |Starting, ModFileName
|ModParseError                    |Fired when the mod failed to parse                        |ModFileName
|ModFinalized                     |Fired when the mod is finalized                           |Starting, ModFileName
|ModFinalizationFailed            |Fired when the mod failed to finalize itself              |ModFileName, Reason
|UserAdded                        |Fired when a new user arrived                             |Username
|UserRemoved                      |Fired when a user is removed                              |Username
|UsernameChanged                  |Fired when a user changed their name                      |OldUsername, NewUsername
|UserPasswordChanged              |Fired when a user changed his/her password                |Username
|HardwareProbing                  |Fired when the hardware is probing                        |-
|HardwareProbed                   |Fired when the hardware is probed                         |-
|CurrentDirectoryChanged          |Fired when the current diectory is changed                |-
|FileCreated                      |Fired when a new file is created                          |File
|DirectoryCreated                 |Fired when a new directory is created                     |Directory
|FileCopied                       |Fired when a file is copied                               |Source, Destination
|DirectoryCopied                  |Fired when a directory is copied                          |Source, Destination
|FileMoved                        |Fired when a file is moved                                |Source, Destination
|DirectoryMoved                   |Fired when a directory is moved                           |Source, Destination
|FileRemoved                      |Fired when a file is removed                              |File
|DirectoryRemoved                 |Fired when a directory is removed                         |Directory
|FileAttributeAdded               |Fired when a file attribute is added                      |File, Attributes
|FileAttributeRemoved             |Fired when a file attribute is removed                    |File, Attributes
|ColorReset                       |Fired when the colors are reset                           |-
|ThemeSet                         |Fired when a theme is set                                 |Theme
|ThemeSetError                    |Fired when a theme is not set due to an error             |Theme, Reason
|ColorSet                         |Fired when a color is set                                 |-
|ColorSetError                    |Fired when a color is not set due to an error             |Reason
|ThemeStudioStarted               |Fired when a theme studio is started                      |-
|ThemeStudioExit                  |Fired when a theme studio is exited                       |-
|ArgumentsInjected                |Fired when the arguments are injected                     |InjectedArguments
|ZipShellInitialized              |When the ZIP shell is initialized                         |-
|ZipPreExecuteCommand             |Fired before ZIP command execution                        |Command
|ZipPostExecuteCommand            |Fired after ZIP command execution                         |Command
|ZipCommandError                  |Fired on ZIP command error                                |Command, Exception
|HTTPShellInitialized             |When the HTTP shell is initialized                        |-
|HTTPPreExecuteCommand            |Fired before HTTP command execution                       |Command
|HTTPPostExecuteCommand           |Fired after HTTP command execution                        |Command
|HTTPCommandError                 |Fired on HTTP command error                               |Command, Exception
|ProcessError                     |Fired on process error                                    |Process, Exception
|LanguageInstalled                |Fired on language installed                               |Language
|LanguageUninstalled              |Fired on language uninstalled                             |Language
|LanguageInstallError             |Fired on language install error                           |Language, Exception
|LanguageUninstallError           |Fired on language uninstall error                         |Language, Exception
|LanguagesInstalled               |Fired on languages installed                              |-
|LanguagesUninstalled             |Fired on languages uninstalled                            |-
|LanguagesInstallError            |Fired on languages install error                          |Exception
|LanguagesUninstallError          |Fired on languages uninstall error                        |Exception
