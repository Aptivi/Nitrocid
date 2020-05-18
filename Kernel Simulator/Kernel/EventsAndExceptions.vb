
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Public Class EventsAndExceptions

    'These events are fired by their Raise<EventName>() subs
    Public Event KernelStarted()
    Public Event PreLogin()
    Public Event PostLogin()
    Public Event ShellInitialized()
    Public Event PreExecuteCommand()
    Public Event PostExecuteCommand()
    Public Event KernelError()
    Public Event ContKernelError()
    Public Event PreShutdown()
    Public Event PostShutdown()
    Public Event PreReboot()
    Public Event PostReboot()
    Public Event PreShowScreensaver()
    Public Event PostShowScreensaver() 'After a key is pressed after screensaver is shown
    Public Event PreUnlock()
    Public Event PostUnlock()
    Public Event CommandError()
    Public Event PreReloadConfig()
    Public Event PostReloadConfig()
    Public Event PreFetchNetworks()
    Public Event PostFetchNetworks()
    Public Event PlaceholderParsing()
    Public Event PlaceholderParsed()
    Public Event GarbageCollected()
    Public Event FTPShellInitialized()
    Public Event FTPPreExecuteCommand()
    Public Event FTPPostExecuteCommand()
    Public Event FTPCommandError()

    ''' <summary>
    ''' Makes the mod respond to the event of kernel start
    ''' </summary>
    Public Sub RespondStartKernel() Handles Me.KernelStarted
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event KernelStarted()...", script.Name, script.Version)
            script.InitEvents("KernelStarted")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-login
    ''' </summary>
    Public Sub RespondPreLogin() Handles Me.PreLogin
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreLogin()...", script.Name, script.Version)
            script.InitEvents("PreLogin")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-login
    ''' </summary>
    Public Sub RespondPostLogin() Handles Me.PostLogin
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostLogin()...", script.Name, script.Version)
            script.InitEvents("PostLogin")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of the shell being initialized
    ''' </summary>
    Public Sub RespondShellInitialized() Handles Me.ShellInitialized
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event ShellInitialized()...", script.Name, script.Version)
            script.InitEvents("ShellInitialized")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-execute command
    ''' </summary>
    Public Sub RespondPreExecuteCommand() Handles Me.PreExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("PreExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-execute command
    ''' </summary>
    Public Sub RespondPostExecuteCommand() Handles Me.PostExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("PostExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of kernel error
    ''' </summary>
    Public Sub RespondKernelError() Handles Me.KernelError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event KernelError()...", script.Name, script.Version)
            script.InitEvents("KernelError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of continuable kernel error
    ''' </summary>
    Public Sub RespondContKernelError() Handles Me.ContKernelError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event ContKernelError()...", script.Name, script.Version)
            script.InitEvents("ContKernelError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-shutdown
    ''' </summary>
    Public Sub RespondPreShutdown() Handles Me.PreShutdown
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreShutdown()...", script.Name, script.Version)
            script.InitEvents("PreShutdown")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-shutdown
    ''' </summary>
    Public Sub RespondPostShutdown() Handles Me.PostShutdown
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostShutdown()...", script.Name, script.Version)
            script.InitEvents("PostShutdown")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-reboot
    ''' </summary>
    Public Sub RespondPreReboot() Handles Me.PreReboot
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreReboot()...", script.Name, script.Version)
            script.InitEvents("PreReboot")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-reboot
    ''' </summary>
    Public Sub RespondPostReboot() Handles Me.PostReboot
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostReboot()...", script.Name, script.Version)
            script.InitEvents("PostReboot")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-screensaver show
    ''' </summary>
    Public Sub RespondPreShowScreensaver() Handles Me.PreShowScreensaver
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreShowScreensaver()...", script.Name, script.Version)
            script.InitEvents("PreShowScreensaver")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-screensaver show
    ''' </summary>
    Public Sub RespondPostShowScreensaver() Handles Me.PostShowScreensaver
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostShowScreensaver()...", script.Name, script.Version)
            script.InitEvents("PostShowScreensaver")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-unlock
    ''' </summary>
    Public Sub RespondPreUnlock() Handles Me.PreUnlock
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreUnlock()...", script.Name, script.Version)
            script.InitEvents("PreUnlock")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-unlock
    ''' </summary>
    Public Sub RespondPostUnlock() Handles Me.PostUnlock
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostUnlock()...", script.Name, script.Version)
            script.InitEvents("PostUnlock")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of command error
    ''' </summary>
    Public Sub RespondCommandError() Handles Me.CommandError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event CommandError()...", script.Name, script.Version)
            script.InitEvents("CommandError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-reload config
    ''' </summary>
    Public Sub RespondPreReloadConfig() Handles Me.PreReloadConfig
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreReloadConfig()...", script.Name, script.Version)
            script.InitEvents("PreReloadConfig")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-reload config
    ''' </summary>
    Public Sub RespondPostReloadConfig() Handles Me.PostReloadConfig
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostReloadConfig()...", script.Name, script.Version)
            script.InitEvents("PostReloadConfig")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-fetch networks
    ''' </summary>
    Public Sub RespondPreFetchNetworks() Handles Me.PreFetchNetworks
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PreFetchNetworks()...", script.Name, script.Version)
            script.InitEvents("PreFetchNetworks")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-fetch networks
    ''' </summary>
    Public Sub RespondPostFetchNetworks() Handles Me.PostFetchNetworks
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PostFetchNetworks()...", script.Name, script.Version)
            script.InitEvents("PostFetchNetworks")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of a placeholder being parsed
    ''' </summary>
    Public Sub RespondPlaceholderParsing() Handles Me.PlaceholderParsing
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PlaceholderParsing()...", script.Name, script.Version)
            script.InitEvents("PlaceholderParsing")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of a parsed placeholder
    ''' </summary>
    Public Sub RespondPlaceholderParsed() Handles Me.PlaceholderParsed
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event PlaceholderParsed()...", script.Name, script.Version)
            script.InitEvents("PlaceholderParsed")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of garbage collection finish
    ''' </summary>
    Public Sub RespondGarbageCollected() Handles Me.GarbageCollected
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event GarbageCollected()...", script.Name, script.Version)
            script.InitEvents("GarbageCollected")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP shell initialized
    ''' </summary>
    Public Sub RespondFTPShellInitialized() Handles Me.FTPShellInitialized
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPShellInitialized()...", script.Name, script.Version)
            script.InitEvents("FTPShellInitialized")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-command execution
    ''' </summary>
    Public Sub RespondFTPPreExecuteCommand() Handles Me.FTPPreExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPPreExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("FTPPreExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-command execution
    ''' </summary>
    Public Sub RespondFTPPostExecuteCommand() Handles Me.FTPPostExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPPostExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("FTPPostExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP command error
    ''' </summary>
    Public Sub RespondFTPCommandError() Handles Me.FTPCommandError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPCommandError()...", script.Name, script.Version)
            script.InitEvents("FTPCommandError")
        Next
    End Sub

    'These subs are for raising events
    ''' <summary>
    ''' Raise an event of kernel start
    ''' </summary>
    Public Sub RaiseStartKernel()
        Wdbg("I", "Raising event KernelStarted() and responding in RespondStartKernel()...")
        RaiseEvent KernelStarted()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-login
    ''' </summary>
    Public Sub RaisePreLogin()
        Wdbg("I", "Raising event PreLogin() and responding in RespondPreLogin()...")
        RaiseEvent PreLogin()
    End Sub
    ''' <summary>
    ''' Raise an event of post-login
    ''' </summary>
    Public Sub RaisePostLogin()
        Wdbg("I", "Raising event PostLogin() and responding in RespondPostLogin()...")
        RaiseEvent PostLogin()
    End Sub
    ''' <summary>
    ''' Raise an event of shell initialized
    ''' </summary>
    Public Sub RaiseShellInitialized()
        Wdbg("I", "Raising event ShellInitialized() and responding in RespondShellInitialized()...")
        RaiseEvent ShellInitialized()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-command execution
    ''' </summary>
    Public Sub RaisePreExecuteCommand()
        Wdbg("I", "Raising event PreExecuteCommand() and responding in RespondPreExecuteCommand()...")
        RaiseEvent PreExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of post-command execution
    ''' </summary>
    Public Sub RaisePostExecuteCommand()
        Wdbg("I", "Raising event PostExecuteCommand() and responding in RespondPostExecuteCommand()...")
        RaiseEvent PostExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of kernel error
    ''' </summary>
    Public Sub RaiseKernelError()
        Wdbg("I", "Raising event KernelError() and responding in RespondKernelError()...")
        RaiseEvent KernelError()
    End Sub
    ''' <summary>
    ''' Raise an event of continuable kernel error
    ''' </summary>
    Public Sub RaiseContKernelError()
        Wdbg("I", "Raising event ContKernelError() and responding in RespondContKernelError()...")
        RaiseEvent ContKernelError()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-shutdown
    ''' </summary>
    Public Sub RaisePreShutdown()
        Wdbg("I", "Raising event PreShutdown() and responding in RespondPreShutdown()...")
        RaiseEvent PreShutdown()
    End Sub
    ''' <summary>
    ''' Raise an event of post-shutdown
    ''' </summary>
    Public Sub RaisePostShutdown()
        Wdbg("I", "Raising event PostShutdown() and responding in RespondPostShutdown()...")
        RaiseEvent PostShutdown()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-reboot
    ''' </summary>
    Public Sub RaisePreReboot()
        Wdbg("I", "Raising event PreReboot() and responding in RespondPreReboot()...")
        RaiseEvent PreReboot()
    End Sub
    ''' <summary>
    ''' Raise an event of post-reboot
    ''' </summary>
    Public Sub RaisePostReboot()
        Wdbg("I", "Raising event PostReboot() and responding in RespondPostReboot()...")
        RaiseEvent PostReboot()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-show screensaver
    ''' </summary>
    Public Sub RaisePreShowScreensaver()
        Wdbg("I", "Raising event PreShowScreensaver() and responding in RespondPreShowScreensaver()...")
        RaiseEvent PreShowScreensaver()
    End Sub
    ''' <summary>
    ''' Raise an event of post-show screensaver
    ''' </summary>
    Public Sub RaisePostShowScreensaver()
        Wdbg("I", "Raising event PostShowScreensaver() and responding in RespondPostShowScreensaver()...")
        RaiseEvent PostShowScreensaver()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-unlock
    ''' </summary>
    Public Sub RaisePreUnlock()
        Wdbg("I", "Raising event PreUnlock() and responding in RespondPreUnlock()...")
        RaiseEvent PreUnlock()
    End Sub
    ''' <summary>
    ''' Raise an event of post-unlock
    ''' </summary>
    Public Sub RaisePostUnlock()
        Wdbg("I", "Raising event PostUnlock() and responding in RespondPostUnlock()...")
        RaiseEvent PostUnlock()
    End Sub
    ''' <summary>
    ''' Raise an event of command error
    ''' </summary>
    Public Sub RaiseCommandError()
        Wdbg("I", "Raising event CommandError() and responding in RespondCommandError()...")
        RaiseEvent CommandError()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-reload config
    ''' </summary>
    Public Sub RaisePreReloadConfig()
        Wdbg("I", "Raising event PreReloadConfig() and responding in RespondPreReloadConfig()...")
        RaiseEvent PreReloadConfig()
    End Sub
    ''' <summary>
    ''' Raise an event of post-reload config
    ''' </summary>
    Public Sub RaisePostReloadConfig()
        Wdbg("I", "Raising event PostReloadConfig() and responding in RespondPostReloadConfig()...")
        RaiseEvent PostReloadConfig()
    End Sub
    ''' <summary>
    ''' Raise an event of pre-fetch networks
    ''' </summary>
    Public Sub RaisePreFetchNetworks()
        Wdbg("I", "Raising event PreFetchNetworks() and responding in RespondPreFetchNetworks()...")
        RaiseEvent PreFetchNetworks()
    End Sub
    ''' <summary>
    ''' Raise an event of post-fetch networks
    ''' </summary>
    Public Sub RaisePostFetchNetworks()
        Wdbg("I", "Raising event PostFetchNetworks() and responding in RespondPostFetchNetworks()...")
        RaiseEvent PostFetchNetworks()
    End Sub
    ''' <summary>
    ''' Raise an event of placeholders being parsed
    ''' </summary>
    Public Sub RaisePlaceholderParsing()
        Wdbg("I", "Raising event PlaceholderParsing() and responding in RespondPlaceholderParsing()...")
        RaiseEvent PlaceholderParsing()
    End Sub
    ''' <summary>
    ''' Raise an event of a parsed placeholder
    ''' </summary>
    Public Sub RaisePlaceholderParsed()
        Wdbg("I", "Raising event PlaceholderParsed() and responding in RespondPlaceholderParsed()...")
        RaiseEvent PlaceholderParsed()
    End Sub
    ''' <summary>
    ''' Raise an event of garbage collection finish
    ''' </summary>
    Public Sub RaiseGarbageCollected()
        Wdbg("I", "Raising event GarbageCollected() and responding in RespondGarbageCollected()...")
        RaiseEvent GarbageCollected()
    End Sub
    ''' <summary>
    ''' Raise an event of FTP shell initialized
    ''' </summary>
    Public Sub RaiseFTPShellInitialized()
        Wdbg("I", "Raising event FTPShellInitialized() and responding in RespondFTPShellInitialized()...")
        RaiseEvent FTPShellInitialized()
    End Sub
    ''' <summary>
    ''' Raise an event of FTP pre-execute command
    ''' </summary>
    Public Sub RaiseFTPPreExecuteCommand()
        Wdbg("I", "Raising event FTPPreExecuteCommand() and responding in RespondFTPPreExecuteCommand()...")
        RaiseEvent FTPPreExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of FTP post-execute command
    ''' </summary>
    Public Sub RaiseFTPPostExecuteCommand()
        Wdbg("I", "Raising event FTPPostExecuteCommand() and responding in RespondFTPPostExecuteCommand()...")
        RaiseEvent FTPPostExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of FTP command error
    ''' </summary>
    Public Sub RaiseFTPCommandError()
        Wdbg("I", "Raising event FTPCommandError() and responding in RespondFTPCommandError()...")
        RaiseEvent FTPCommandError()
    End Sub

    'These classes are for exceptions. (For developers of mods: Only use if your mod is an extension to the kernel)
    ''' <summary>
    ''' There are no more users remaining
    ''' </summary>
    Public Class NullUsersException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class

    ''' <summary>
    ''' Not enough command arguments supplied in shell
    ''' </summary>
    Public Class NotEnoughArgumentsException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class

    ''' <summary>
    ''' Not enough command arguments supplied in FTP shell
    ''' </summary>
    <Obsolete>
    Public Class FTPNotEnoughArgumentsException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class

    ''' <summary>
    ''' Nothing in JSON
    ''' </summary>
    <Obsolete>
    Public Class JsonNullException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class

    ''' <summary>
    ''' Manual page truncated
    ''' </summary>
    <Obsolete>
    Public Class TruncatedManpageException
        Inherits Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class

End Class
