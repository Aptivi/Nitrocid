### Breaking API changes (Third generation)

#### 0.1.0 notes

> [!WARNING]
> When upgrading your mods to support 0.1.0, you must follow the compatibility notes to ensure that your mod works with 0.1.0. If you want to support both the first-generation and the second-generation KS, you must separate your mod codebase to three parts: one for the first-gen, the other for the second-gen, and the other for the third-gen. They can't coexist with each other in your KSMods directory.

##### Moved events to KS.Kernel.Events

All of the event-related code files are moved to `KS.Kernel.Events` to separate these from the actual kernel code. However, because events are part of the kernel, we prefer to put it on `KS.Kernel` namespace rather than `KS.Misc`.

##### Moved PlatformDetector to KS.Kernel.KernelPlatform

It has come to the conclusion that `PlatformDetector` is now promoted to the Kernel namespace under the name `KernelPlatform` so you can access it under the `KS.Kernel` namespace.

* Affected classes:
  - PlatformDetector -> KernelPlatform

##### Separated the MOTD and MAL parsers

MOTD and MAL parsers were unified since early versions of Kernel Simulator. However, it didn't occur to us that we need to separate them for a very long time. Now is the time to separate them, effectively removing `MessageType` enumeration.

##### Moved GetTerminal* to KernelPlatform

These two functions are actually part of the terminal, and they make use of the $TERM_PROGRAM and $TERM environment variables, which are dependent on the terminal.

Since these usually are undefined in Windows, we put these functions to KernelPlatform to accomodate the change as platform-dependent, but we don't actually check for Linux to execute these functions, because some terminal emulators in Windows actually define these variables.

##### Moved shell common folders to KS.Shell.Shells

This is to separate the shell code for each tool from their folders to a unified namespace, `KS.Shell.Shells`. It houses every shell for every tool. This is to make creation of built-in shells easier.

This means that `*ShellCommon` modules are moved to `KS.Shell.Shells.*` and every call to that module should be redirected to that namespace. The tools, however, stays intact.

##### Removed MakePermanent()

This function is now removed as a result of recent configuration improvements.

##### Graduated `Configuration` to `KS.Kernel`

This configuration logic is smart enough to be graduated, since it's the core function in the kernel. It's used everywhere, including the `settings` command, which is an application that lets you adjust settings on the go.

##### Graduated `FindSetting` and `CheckSettingsVariables`

These two functions are probably unrelated to the settings app, but one of them is what `KS.Misc.Settings.SettingsApp` uses, and one of them is renamed to accomodate with the graduation.

* Renamed functions:
  - CheckSettingsVariables() -> CheckConfigVariables()

##### Moved power management functions to `KS.Kernel.Power`

These power management functions were there in `KernelTools` since the earliest version of KS. Now, they're relocated to `KS.Kernel.Power`.

##### Removed `InitPaths` in favor of properties

Now, we don't have to initialize paths everytime we make an internal app that depends on Kernel Simulator's paths. The call to the function that gets the path, `GetKernelPath`, however won't be removed because it's widely used and is unaffected. This reduces the `NullReferenceException` bugs regarding paths.

##### Moved kernel update code to `Kernel.Updates`

Same story as in power management.

##### Removed `ListArgs()` from `ICommand` and `IArgument`

It seems that `ListArgs()` is now no longer a reliable way to check for arguments, as it could be `null` when no argument is provided. While it contains a collection of switches and arguments, it's sequential. We have separated between switches and arguments as demonstrated in both the `ListArgsOnly()` and `ListSwitchesOnly()` arrays.

##### Moved `GetEsc()` to `Misc.Text.CharManager`

`GetEsc()` is now widely used for color manipulation, but we need to move it to a better place as migration to `ColorSeq` is done. We have deleted `Color255` from `KS.ConsoleBase` as a result of this migration.

##### Removed FullArgumentList

Following the removal of `ListArgs()`, we can safely remove this property.

##### Removed `ConsoleWriters.*.Write*Plain` in favor of the plain writers

The plain writer interfaces feel like they're a great addition to control the console writers.

##### Updated debug and notifications namespaces

We felt that moving debug-related functions to `KS.Kernel` would be more convenient, so we moved these functions to it. Also, we've renamed the notifications namespace.

* Affected namespaces:
  - `KS.Misc.Writers.DebugWriters` -> `KS.Debugging`
  - `KS.Network.RemoteDebug` -> `KS.Debugging.RemoteDebug`
  - `KS.Misc.Notifiers` -> `KS.Misc.Notifications`

##### Merged `ParseCmd` with `ExecuteCommand`

It has come to the conclusion that `ParseCmd` is now very similar to `ExecuteCommand` with treating the remote debug shell specially. This is no longer needed as `ProvidedCommandArgumentsInfo` has been provided the IP address of the target device, making `ParseCmd` redundant.

##### Removed `ExecuteRDAlias()`

This function is not needed to execute aliases since there has been recent improvements to the executor in both 0.0.20.0 and 0.0.24.0.

##### Renamed debug writer function names

We needed to do the same thing as we've renamed `W()` to `Write()`, so we renamed the following:
  - Wdbg() -> WriteDebug()
  - WdbgConditional() -> WriteDebugConditional()
  - WdbgDevicesOnly() -> WriteDebugDevicesOnly()
  - WStkTrcConditional() -> WriteDebugStackTraceConditional()
  - WStkTrc() -> WriteDebugStackTrace

##### Moved MAL and MOTD message to Misc.Probers.Motd

These have no relationship with the kernel directly.

##### Moved HostName from Kernel to NetworkTools

It has no relationship with the kernel either.

##### Renamed `permissions` to `groups`

Renamed `permissions` to `groups` as it has been incorrectly named after the permissions. It could have been more accurately described as user groups.

##### Made abstractions regarding the color management class

The color management used to define so many variables for just one color type. Now, it has been simplified to ease the making of the new color type. Color types are renamed to match all files that mention the color type.

As a result, we have removed all separate variables for each color type and merged them to simplify the declaration.

Also, we have added `GetColor()` and `SetColor()` functions to ColorTools to perform operations on these color types.

##### Moved ConfigCategory outside Config class

We have moved ConfigCategory outside the Config class to better organize the enumerations relating to the configuration.

##### Implemented interface for encryptors

This will make implementing future encryptors easier for mods, since it'll now use our own encryption algorithm manager instead of relying on the mod's algorithm management.

An interface, IEncryptor, was created to help you implement the encryptor faster. You can use Encryption in KS.Drivers.Encryption to use helper functions.

However, this is only the initial stage of the implementation.

##### Use CommandArgumentInfo in arguments

Using `CommandArgumentInfo` allows you to define argument information for commands. However, it's a powerful class for managing arguments for commands and kernel arguments themselves.

As a result, we've used `CommandArgumentInfo` in `ArgumentInfo`.

##### Removed *SetColors as they're no longer used

We have improved the color tools module, so SetColors is no longer used. We've removed it for this reason. Use `SetColor()` instead.

##### Changed algorithm enum to EncryptionAlgorithms

As part of an ongoing change to the encryption driver, we've changed the algorithm enumeration to EncryptionAlgorithms outside the Encryption module.

##### Renamed two classes related to shell

We have renamed the below two shell-related classes to the ones that suit the narrative of the classes.

* Affected classes:
  - ShellInfo -> ShellExecuteInfo
  - ShellExecutor -> BaseShell

##### Moved TextLocation to Misc.Text

This is a text-related enum of text vertical location (top, bottom).

##### Moved GetCommands to CommandManager

This sub is more of a command management routine than the "getting command" module work itself.

##### Changed the entire event system

We have moved all the events to its own dedicated array containing all the available events that the kernel introduced, removing the giant Events class and its variable, `KernelEventManager`, in the Kernel entry point class. This will reduce the need of importing the Kernel namespace and class everytime we need to directly manage the events.

Event firing and response functions are moved to the EventsManager class in one function, `FireEvent`.

This reduces the amount of lines by more than 3500 lines.

##### Moved NewLine from Kernel to CharManager

This is actually a character management function and not a function directly to the kernel.

##### Moved KS.Login to KS.Users.Login

This has to do with the user management namespace, so we moved it to that namespace.

##### Moved Kernel[Api]Version to KernelTools

We don't want the `Kernel` class to be publicly accessible, since it has been planned back at 0.0.1 as the class responsible for being an entry point.

These variables, `KernelVersion` and `KernelApiVersion`, are successfully moved to `KernelTools`.

##### Removed `ColoredShell`

The colors are now an essential part of KS, so we decided to take out support for uncolored shell.

##### Finally condensed `TableColor`

We noticed that the code is repetitive for making tables, and we don't want to update six locations every bug fix, so we decided to condense it to a single code.

As a side-effect, we've changed the color signatures from foreground and background pairs to header, value, separator, and background colors. Consequently, we've removed the `ConsoleColor` version of `TableColor` as we found it irrelevant thanks to the enhanced `Color` class found in the ColorSeq library.

##### Tried to balance color support for writers

Migration from `ConsoleColor` to `ConsoleColors` is complete. This means that all the writers in the `KS.Misc.*Writers` now have the `ConsoleColors` support.

The latest ColorSeq version, 1.0.2, will be used to make it easier to achieve.

##### Added last argument support to auto completer

This is to aid in trying to autocomplete subjects starting from the last argument

##### Renamed command executor and base to reduce confusion

Base command class had the name of `CommandExecutor`. However, it behaved like the base class for your mod commands, so we renamed it to `BaseCommand`. This caused us to rename the command execution class to `CommandExecutor`.

Affected classes:
  * `CommandExecutor` -> `BaseCommand`
  * `GetCommand` -> `CommandExecutor`

##### Renamed ConsoleSanityChecker to ConsoleChecker

This class will be filled by many console checks, so renamed it according to the purpose.

Affected classes:
  * `ConsoleSanityChecker` -> `ConsoleChecker`

##### WriteWherePlain from TextWriterWhereColor renamed

This change is necessary to fit in with the rest of the ConsoleWriters

Affected functions:
  * `WriteWherePlain` -> `WriteWhere`

##### Removed Screensaver.colors

This is to remove support for 255 colors in screensavers.

##### Moved KS.Network classes to KS.Network.Base

These classes are believed to be the base classes for the networking. These classes are used for general networking purposes.

##### Condensed speed dial to KS.Network.SpeedDial

The speed dial API wasn't touched for a long time, so we decided to condense the speed dial API to a single namespace to ease the addition of the speed dial feature to all the networking shells.

In consequence, the speed dial format has changed.

Old format:
```json
  "ftp.riken.jp": {
    "Address": "ftp.riken.jp",
    "Port": 21,
    "User": "anonymous",
    "Type": 0,
    "FTP Encryption Mode": 0
  },
```

New format:
```json
  "ftp.us.debian.org": {
    "Address": "ftp.us.debian.org",
    "Port": 21,
    "Type": 0,
    "Options": [
      "anonymous",
      0
    ]
  }
```

The username and FTP encryption mode is now moved to Options, which is an array of options that the networking shells use. To convert your speed dial entries, you have to manually open all FTP and SFTP speed dial JSON files and make changes to transition from the old format to the new format.

##### Moved network transfer functions to KS.Network.Base.Transfer

We've done that to the base network classes, so why not do the same to the network transfer functions?
