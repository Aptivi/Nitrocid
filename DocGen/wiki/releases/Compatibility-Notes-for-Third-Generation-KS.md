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
