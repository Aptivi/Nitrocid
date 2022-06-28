## Compatibility notes

The second-generation versions of Kernel Simulator are not backwards-compatible with the first-generation versions, due to too many structural changes as outlined below. Here are the breaking changes per generation.

### Breaking API changes (Second-generation, Revision 0)

Each breaking change have their own workarounds and description, including the reason why we've removed the API function. You need to provide alternatives for the below deprecated functions or APIs in your mods.

#### 0.0.20.0 notes

When upgrading your mods to support 0.0.20.0, you must follow the compatibility notes to ensure that your mod works with 0.0.20.0. If you want to support the first-generation KS, you must separate your mod codebase to two parts: one for the first-gen and the other for the second-gen. They can't coexist with each other in your KSMods directory.

##### Unified help system to support every shell

We have unified the help system to support every shell provided in KS. This results in us having to do these changes:

* Affected functions:
  - Removed TextEdit_GetHelp()
  - Removed ZipShell_GetHelp()
  - Removed FTPShowHelp()
  - Removed IMAPShowHelp()
  - Removed RSSShowHelp()
  - Removed SFTPShowHelp()
  - Removed TestShowHelp()
  - Removed RDebugShowHelp()
  - Changed constructor of CommandInfo to allow help usage writing

##### Improved naming of injected commands

During the re-write of kernel argument parser, we've done these changes:

* Affected variables:
  - Replaced argcommands and argcmds() with InjectedCommands() in public API

##### Prefixed the FTP shell variables with "Ftp"

We needed to reduce the ambiguity of the FTP shell variables to minimize the risk of bugs, so we have prefixed these variables:

* Affected variables:
  - connected -> FtpConnected
  - initialized -> FtpInitialized
  - currDirect -> FtpCurrentDirectory
  - currentremoteDir -> FtpCurrentRemoteDir
  - user -> FtpUser
  - pass -> FtpPass
  - strcmd -> FtpCommand

##### Relocated Client(S)FTP to their Shell.vb files

* Affected variables:
  - Moved ClientSFTP from SFTPGetCommand.vb to SFTPShell.vb
  - Moved ClientFTP from FTPGetCommand.vb to FTPShell.vb

##### Reworked on how to create notifications

* Affected variables:
  - NotifyCreate() made obsolete
  - Notification class moved to its individual file

##### Made getting kernel paths more secure

paths() used to store all the kernel paths, but it was implemented insecurely, so we decided to replace it with something more secure.

* Affected variables:
  - paths() replaced with GetKernelPath and GetOtherPath

##### Debug now uses the DebugLevel enumerator

We no longer use Chars to indicate the debug level. Instead, we provide the DebugLevel enumerator to accurately represent debug levels that are available.

* Affected functions:
  - Wdbg(), WdbgConditional(), and WdbgDevicesOnly() now take a DebugLevel enum value instead of char

##### Rewritten command handler

As we're moving to the second generation, many API changes needed to be done, so we've rewritten the command parser to divide the command codes into their separate code files.

* Affected functions:
  - ExecuteCommand() from FTPGetCommand has been removed
  - ExecuteCommand() from SFTPGetCommand has been removed

* Affected variables:
  - CommandBase has been implemented. Just pass `null` to it from now in mods.
  - Moved `*CommandThread` thread vars to `ShellStartThreads`. Consider publicizing it later.

##### Moved IsOn* to PlatformDetector

This is a breaking change for platform detection functions. We've moved them to PlatformDetector.

##### Split ICustomSaver to separate codefile

You can now reference the ICustomSaver within the KS namespace.

##### Renamed variables in public API

This renames some of the variables to something more meaningful, and, optionally, honors the Camel-Case standards.

* Affected variables:
  - defSaverName -> DefSaverName
  - CSvrdb -> CustomSavers
  - finalSaver -> FinalSaver
  - ScrnSvrdb -> Screensavers
  - colorTemplates -> ColorTemplates
  - setRootPasswd -> SetRootPassword
  - RootPasswd -> RootPassword
  - maintenance -> Maintenance
  - argsOnBoot -> ArgsOnBoot
  - clsOnLogin -> ClearOnLogin
  - showMOTD -> ShowMOTD
  - simHelp -> SimHelp
  - slotProbe -> SlotProbe
  - CornerTD -> CornerTimeDate
  - instanceChecked -> InstanceChecked
  - HName -> HostName
  - currentLang -> CurrentLanguage
  - dbgWriter -> DebugWriter
  - dbgStackTraces -> DebugStackTraces
  - ftpsite -> FtpSite
  - ftpexit -> FtpExit
  - dbgConns -> DebugConnections
  - sftpsite -> SFTPSite
  - sftpexit -> SFTPExit
  - DRetries -> DownloadRetries
  - URetries -> UploadRetries
  - modcmnds -> ModCommands

##### Made some cleanups regarding MOTD parser

As part of the clean-up process, we've removed MOTDStreamR and MOTDStreamW as accidental public API variables.

* Affected variables:
  - Removed MOTDStreamR
  - Removed MOTDStreamW
  - Renamed ReadMOTDFromFile to ReadMOTD

##### Split GetConnectionInfo

This splits GetConnectionInfo into two separate functions:

* New functions:
  - GetConnectionInfo (added AuthMethods argument)
  - PromptConnectionInfo

This renames GetConnectionInfo to PromptConnectionInfo.

##### Changed how mail listing works

Using StringBuilder to make lists in MailListMessages is disgusting enough to warrant a change.

##### Changed how reading contents API works

Moved the original logic to PrintContents(), causing ReadContents() to consist of a function with the return type of String() that contains the file contents.

* Affected functions:
  - ReadContents()

* New functions:
  - PrintContents() + two overloads

##### Removed NotifyCreate()

The removal of NotifyCreate() is needed because it mimics the constructor, which is unnecessary. We've already made the constructor of the Notification class, and we thought that it's necessary to remove NotifyCreate().

* Affected functions:
  - NotifyCreate()

##### Split the theme-related tools from ColorTools

* Affected functions:
  - ApplyThemeFromResources
  - ApplyThemeFromFile
  - SetColors (theme overload) -> SetColorsTheme
  - ColorTemplates -> Themes

##### Implemented help helpers for commands and args

We saw that providing hard-coded extra help wasn't a good idea, so why not make "help helpers" and get rid of hardcoded and irrelevant varibles once and for all?

But, the "help helpers" aren't on their appropriate interfaces, because, on CommandInfo and ArgumentInfo, the default was null.

* Affected classes:
  - ArgumentInfo
  - CommandInfo

##### Enumerized the reasons for the three events

This makes it easier to distinguish the reasons. It has been set accordingly so that they could be combined.

* Affected events:
  - LoginError
  - ThemeSetError
  - ColorSetError

##### Split the custom screensaver code

All the functions and variables which define the custom screensaver code are affected by this change. Compiler-related functions are moved to CustomSaverCompiler, while the tools are moved to CustomSaverTools.

##### Moved few variables regarding mods

This is to split a few variables from ModParser.

* Affected variables:
  - All mod definitions, like ModDefs, TestModDefs, and so on are moved to HelpSystem.
  - Scripts dictionary has been moved to ModManager
  - IScript moved from ModParser to its individual file

##### Renamed ScreensaverInfo for relevancy

This name was misleading as it only has Screensaver, so you may think that it was a class meant for normal screensavers when it's really for custom screensavers. It's been renamed to CustomSaverInfo.

##### Cleaned some flags up

We see that making StopPanicAndGoToDoublePanic and InstanceChecked flags exposed to the public API (mods) is meaningless, so we've demoted these two to KernelTools as a friend.

##### Implemented HelpUsages() to replace HelpUsage

This gives a better support for commands with multiple usages.

##### Unified the overloads for writing functions

###### Reasons

It has been recently reported that a single-lettered function or sub is not a good way to describe writing your text into the console or even doing anything. For example, if you're going to make a function that adds the two numbers from the two variables and returns the result, you're not going to name that function "A," because, while it provides the shortcut to this function, it also creates problems, especially if you no longer understood what that function did or even meant for. The problem grows even worse when you decided to leave that function undocumented because you would like "to be the only person that can understand it and nobody else." It also may pose confusions, because what if "A" is meant by "AllNumbers," "AddNumbers," "AddTwoNumbers," or, obviously, "Addition?"

Moreover, there was a need to overload all the writing functions by renaming their names to their appropriate "Write" functions so that we wouldn't have to use the separate functions for their colored versions. However, if it was bound to cause problems in later commits, we'll revert this change.

###### Actions

Your mods might break if any of them uses the console writing functions from KS, so change all the `W()` instances to `Write()` and remove any "C" or "C16" suffixes.

###### Side Effects

When writing such functions, you'll discover that the arguments parsing is stricter than the previous, due to how we've implemented the message argument. Make explicit casts while we're testing the new overloads to make sure that everything is the same as before.

##### Actually removed AliasType

It's basically a carbon-copy of ShellCommandType

##### Reworked on the fetch kernel update API

* Affected functions:
  - FetchKernelUpdates() has its return value changed to its dedicated class, KernelUpdate

##### Removed the RGB class

The RGB class was obsolete because the Color class does its job properly.

##### Removed unnecessary OtherPath and its Get function

This kind of path was unnecessary.

##### Reworked on how to use the Color class

All of the kernel color variables will now use the Color type instead of a string. This ensures easier usage of such variables.

However, things will break if your mods still use the old traditional way of how to use such variables.

##### Increased security of the "scripts" variable

We have made the above variable private so that no mod can modify it for malicious purposes.

##### [G|S]etConfig* functions and subs are now obsolete

They are just derivatives of the already existing SetValue, GetValue, and GetPropertyValueInVariable with slight changes. Trying to avoid code repeats by warning.

This will give mod devs a chance to move to the FieldManager versions before being removed in early cycles of RC2.

##### Made IShell and shell stacks to handle shells

This removes all kinds of InitializeShell() in all the shell common files to reduce confusion. Adjustments will be made to finish the implementation.

##### Cleaned up GetLine() so strcommand is first

It feels that ArgsMode was only used for one purpose, and it felt so unnecessary that it shouldn't have been implemented in the first place.

##### Renamed ShellCommandType to ShellType

It was used for many purposes other than the commands.

##### Moved all the GetLine()'s for shells to the master GetLine()

This removes all the GetLine specially crafted for the shells, because they're just repeated code taken from the master GetLine().

##### Moved GetTerminalEmulator() to ConsoleExtensions

It's now part of the console extensions module, because it doesn't change how the kernel works.

##### Split the exceptions to separate codefiles

This will break mods that handle any kernel exceptions. They're now located in the Kernel.Exceptions namespace. However, this required a change to the Kernel class that involves namespacing it from KS.Kernel to KS.Kernel.Kernel.

##### Renamed newline field to NewLine from vbNewLine

Because `vbNewLine` sounded like it came from Visual Basic 6.0 (not .NET), a COM-based Windows-only language which we'll never support, and because of the below namespace changes that causes Microsoft.VisualBasic namespace to break things related to vbNewLine, we decided to change it to just `NewLine`.

##### Namespaced the entire codebase

To further organize the codebase, we decided to namespace each one of them based on the folders in the source code. This way, we'll have the following namespaces:

* Namespaces created:
  - KS.Arguments
  - KS.Arguments.ArgumentBase
  - KS.Arguments.CommandLineArguments
  - KS.Arguments.KernelArguments
  - KS.Arguments.PreBootCommandLineArguments
  - KS.ConsoleBase
  - KS.ConsoleBase.Themes
  - KS.ConsoleBase.Themes.Studio
  - KS.Files
  - KS.Hardware
  - KS.Kernel
  - KS.Kernel.Exceptions
  - KS.Languages
  - KS.Login
  - KS.ManPages
  - KS.Misc
  - KS.Misc.Beautifiers
  - KS.Misc.Calendar
  - KS.Misc.Calendar.Events
  - KS.Misc.Calendar.Reminders
  - KS.Misc.Configuration
  - KS.Misc.Encryption
  - KS.Misc.Execution
  - KS.Misc.Forecast
  - KS.Misc.Games
  - KS.Misc.JsonShell
  - KS.Misc.JsonShell.Commands
  - KS.Misc.Notifiers
  - KS.Misc.Platform
  - KS.Misc.Probers
  - KS.Misc.Reflection
  - KS.Misc.Screensaver
  - KS.Misc.Screensaver.Customized
  - KS.Misc.Screensaver.Displays
  - KS.Misc.Splash
  - KS.Misc.Splash.Splashes
  - KS.Misc.TextEdit
  - KS.Misc.TextEdit.Commands
  - KS.Misc.Threading
  - KS.Misc.Timers
  - KS.Misc.Writers
  - KS.Misc.Writers.ConsoleWriters
  - KS.Misc.Writers.DebugWriters
  - KS.Misc.Writers.FancyWriters
  - KS.Misc.Writers.FancyWriters.Tools
  - KS.Misc.Writers.MiscWriters
  - KS.Misc.ZipFile
  - KS.Misc.ZipFile.Commands
  - KS.Modifications
  - KS.Network
  - KS.Network.FTP
  - KS.Network.FTP.Commands
  - KS.Network.FTP.Filesystem
  - KS.Network.FTP.Transfer
  - KS.Network.HTTP
  - KS.Network.HTTP.Commands
  - KS.Network.Mail
  - KS.Network.Mail.Commands
  - KS.Network.Mail.Directory
  - KS.Network.Mail.PGP
  - KS.Network.Mail.Transfer
  - KS.Network.RemoteDebug
  - KS.Network.RemoteDebug.Commands
  - KS.Network.RemoteDebug.Interface
  - KS.Network.RPC
  - KS.Network.RSS
  - KS.Network.RSS.Commands
  - KS.Network.RSS.Instance
  - KS.Network.SFTP
  - KS.Network.SFTP.Commands
  - KS.Network.SFTP.Filesystem
  - KS.Network.SFTP.Transfer
  - KS.Network.SSH
  - KS.Scripting
  - KS.Scripting.Interaction
  - KS.Shell
  - KS.Shell.Commands
  - KS.Shell.ShellBase
  - KS.Shell.Shells
  - KS.TestShell
  - KS.TestShell.Commands
  - KS.TimeDate

##### Help helpers are now part of the command interface

We see that it's sometimes easy to make mistakes when trying to define the extra help action, so we decided to remove an optional argument from CommandInfo and move the helper sub to the command interface.

* Affected interfaces:
  - ICommand

* Affected classes:
  - CommandExecutor
  
##### Removed built-in string evaluators

For a variety of reasons:

1. It was already done in Extensification, which will be moved to `Extensification.Legacy` soon
2. They're insecure and slow, especially the normal `Evaluate()` function

* Affected classes:
  - StringEvaluators
  
* Affected functions:
  - Evaluate()
  - EvaluateFast()

##### Technical information

These are the technical information about **some** of the above breaking changes:

###### TextEdit_GetHelp()

- Name: TextEdit_GetHelp()
- Source file: TextEditHelpSystem.vb
- Version range: 0.0.11 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### ZipShell_GetHelp()

- Name: ZipShell_GetHelp()
- Source file: ZipHelpSystem.vb
- Version range: 0.0.16 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### FTPShowHelp()

- Name: FTPShowHelp()
- Source file: FTPHelpSystem.vb
- Version range: 0.0.5.5 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### IMAPShowHelp()

- Name: IMAPShowHelp()
- Source file: MailHelpSystem.vb
- Version range: 0.0.11 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### RSSShowHelp()

- Name: RSSShowHelp()
- Source file: RSSHelpSystem.vb
- Version range: 0.0.16 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### SFTPShowHelp()

- Name: SFTPShowHelp()
- Source file: SFTPHelpSystem.vb
- Version range: 0.0.13 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### TestShowHelp()

- Name: TestShowHelp()
- Source file: TestHelpSystem.vb
- Version range: 0.0.16 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### RDebugShowHelp()

- Name: RDebugShowHelp()
- Source file: RemoteDebugHelpSystem.vb
- Version range: 0.0.16 - 0.0.20
- Reason for deletion: Unifying the help systems
- Likeliness to come back: Highly unlikely

###### ExecuteCommand()

- Name: ExecuteCommand()
- Source file: FTPGetCommand.vb
- Version range: 0.0.5.5 - 0.0.20
- Reason for deletion: Rewritten the command handler
- Likeliness to come back: Highly unlikely

###### ExecuteCommand()

- Name: ExecuteCommand()
- Source file: SFTPGetCommand.vb
- Version range: 0.0.13 - 0.0.20
- Reason for deletion: Rewritten the command handler
- Likeliness to come back: Highly unlikely

###### NotifyCreate()

- Name: NotifyCreate()
- Source file: Notifications.vb
- Version range: 0.0.15 - 0.0.20
- Reason for deletion: Mimicking the constructor
- Likeliness to come back: Highly unlikely

#### 0.0.21.0 notes

When upgrading your mods to support 0.0.21.0, you must follow the compatibility notes to ensure that your mod works with 0.0.21.0. If you want to support the first-generation KS, you must separate your mod codebase to two parts: one for the first-gen and the other for the second-gen. They can't coexist with each other in your KSMods directory.

##### [BREAKING] Consolidated the obsolete functions

The three deprecated functions are just wrappers to the reflection routines in Kernel Simulator. The one sub is about finding settings, but since we already have `FindSetting(String, JToken)`, why don't we just go ahead and remove the `FindSetting(String, Boolean)` one, since it doesn't support splashes?

* Affected functions:
  - FindSetting(String, Boolean)
  - SetConfigValueField()
  - GetConfigValueField()
  - GetConfigPropertyValueInVariableField()
  
##### Technical information

These are the technical information about **some** of the above breaking changes:

###### FindSetting(String, Boolean)

- Name: FindSetting(String, Boolean)
- Source file: SettingsApp.vb
- Version range: 0.0.11 - 0.0.21
- Reason for deletion: We already have FindSetting(String, JToken)
- Likeliness to come back: Highly unlikely

###### SetConfigValueField()

- Name: SetConfigValueField()
- Source file: SettingsApp.vb
- Version range: 0.0.16 - 0.0.21
- Reason for deletion: A wrapper function to reflection routine
- Likeliness to come back: Highly unlikely

###### GetConfigValueField()

- Name: GetConfigValueField()
- Source file: SettingsApp.vb
- Version range: 0.0.16 - 0.0.21
- Reason for deletion: A wrapper function to reflection routine
- Likeliness to come back: Highly unlikely

###### GetConfigPropertyValueInVariableField()

- Name: GetConfigPropertyValueInVariableField()
- Source file: SettingsApp.vb
- Version range: 0.0.20 - 0.0.21
- Reason for deletion: A wrapper function to reflection routine
- Likeliness to come back: Highly unlikely

#### 0.0.22.0 notes

When upgrading your mods to support 0.0.22.0, you must follow the compatibility notes to ensure that your mod works with 0.0.22.0. If you want to support the first-generation KS, you must separate your mod codebase to two parts: one for the first-gen and the other for the second-gen. They can't coexist with each other in your KSMods directory.

##### [BREAKING] Separated properties code to PropertyManager

As we feel that properties reflection code should have their own place, we moved it to PropertyManager module.

* Affected functions:
  - GetPropertyValueInVariable()
  - GetProperties()
  - GetPropertiesNoEvaluation()

##### Events and reminders format

As BinarySerializer is being deprecated, we've changed the format to the XML format. They're not compatible with the 0.0.21.0 and 0.0.20.0's event format.

##### Deprecation of `IScript.PerformCmd()`

As we have implemented the fully-fledged `CommandBase.Execute()` function which does the same thing as `IScript.PerformCmd()`, we'll deprecate the function in the interface to take advantage of the `CommandBase.Execute()` routine.

If possible, upgrade your mods to use this routine instead of `IScript.PerformCmd()`, which will be deleted in the next API revision.

##### [BREAKING] Removed `ReadLineLong()`

As ReadLine.Reboot supports long inputs, we've decided to remove the function, `ReadLineLong()`, as it doesn't do anything.

* Affected functions:
  - ReadLineLong()

##### Technical information

These are the technical information about **some** of the above breaking changes:

###### ReadLineLong()

- Name: ReadLineLong()
- Source file: Input.vb
- Version range: TBA - 0.0.22
- Reason for deletion: We already have ReadLine.Reboot
- Likeliness to come back: Highly unlikely