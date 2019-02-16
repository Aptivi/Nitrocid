
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Imports System.ComponentModel

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
    <Obsolete> Public Event PreWriteToDebugger() 'Writing event - Deprecated
    <Obsolete> Public Event PostWriteToDebugger() 'Writing event - Deprecated
    <Obsolete> Public Event PreWriteToConsole() 'Writing event - Deprecated
    <Obsolete> Public Event PostWriteToConsole() 'Writing event - Deprecated
    Public Event GarbageCollected()
    Public Event FTPShellInitialized()
    Public Event FTPPreExecuteCommand()
    Public Event FTPPostExecuteCommand()
    Public Event FTPCommandError()

    'For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
    'TODO: Writing events are considered to be removed in the final release of 0.0.6.
    'These subs handle events
    Public Sub RespondStartKernel() Handles Me.KernelStarted
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event KernelStarted()...", script.Name, script.Version)
            script.InitEvents("KernelStarted")
        Next
    End Sub
    Public Sub RespondPreLogin() Handles Me.PreLogin
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreLogin()...", script.Name, script.Version)
            script.InitEvents("PreLogin")
        Next
    End Sub
    Public Sub RespondPostLogin() Handles Me.PostLogin
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostLogin()...", script.Name, script.Version)
            script.InitEvents("PostLogin")
        Next
    End Sub
    Public Sub RespondShellInitialized() Handles Me.ShellInitialized
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event ShellInitialized()...", script.Name, script.Version)
            script.InitEvents("ShellInitialized")
        Next
    End Sub
    Public Sub RespondPreExecuteCommand() Handles Me.PreExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("PreExecuteCommand")
        Next
    End Sub
    Public Sub RespondPostExecuteCommand() Handles Me.PostExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("PostExecuteCommand")
        Next
    End Sub
    Public Sub RespondKernelError() Handles Me.KernelError
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event KernelError()...", script.Name, script.Version)
            script.InitEvents("KernelError")
        Next
    End Sub
    Public Sub RespondContKernelError() Handles Me.ContKernelError
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event ContKernelError()...", script.Name, script.Version)
            script.InitEvents("ContKernelError")
        Next
    End Sub
    Public Sub RespondPreShutdown() Handles Me.PreShutdown
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreShutdown()...", script.Name, script.Version)
            script.InitEvents("PreShutdown")
        Next
    End Sub
    Public Sub RespondPostShutdown() Handles Me.PostShutdown
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostShutdown()...", script.Name, script.Version)
            script.InitEvents("PostShutdown")
        Next
    End Sub
    Public Sub RespondPreReboot() Handles Me.PreReboot
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreReboot()...", script.Name, script.Version)
            script.InitEvents("PreReboot")
        Next
    End Sub
    Public Sub RespondPostReboot() Handles Me.PostReboot
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostReboot()...", script.Name, script.Version)
            script.InitEvents("PostReboot")
        Next
    End Sub
    Public Sub RespondPreShowScreensaver() Handles Me.PreShowScreensaver
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreShowScreensaver()...", script.Name, script.Version)
            script.InitEvents("PreShowScreensaver")
        Next
    End Sub
    Public Sub RespondPostShowScreensaver() Handles Me.PostShowScreensaver
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostShowScreensaver()...", script.Name, script.Version)
            script.InitEvents("PostShowScreensaver")
        Next
    End Sub
    Public Sub RespondPreUnlock() Handles Me.PreUnlock
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreUnlock()...", script.Name, script.Version)
            script.InitEvents("PreUnlock")
        Next
    End Sub
    Public Sub RespondPostUnlock() Handles Me.PostUnlock
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostUnlock()...", script.Name, script.Version)
            script.InitEvents("PostUnlock")
        Next
    End Sub
    Public Sub RespondCommandError() Handles Me.CommandError
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event CommandError()...", script.Name, script.Version)
            script.InitEvents("CommandError")
        Next
    End Sub
    Public Sub RespondPreReloadConfig() Handles Me.PreReloadConfig
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreReloadConfig()...", script.Name, script.Version)
            script.InitEvents("PreReloadConfig")
        Next
    End Sub
    Public Sub RespondPostReloadConfig() Handles Me.PostReloadConfig
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostReloadConfig()...", script.Name, script.Version)
            script.InitEvents("PostReloadConfig")
        Next
    End Sub
    Public Sub RespondPreFetchNetworks() Handles Me.PreFetchNetworks
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PreFetchNetworks()...", script.Name, script.Version)
            script.InitEvents("PreFetchNetworks")
        Next
    End Sub
    Public Sub RespondPostFetchNetworks() Handles Me.PostFetchNetworks
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PostFetchNetworks()...", script.Name, script.Version)
            script.InitEvents("PostFetchNetworks")
        Next
    End Sub
    Public Sub RespondPlaceholderParsing() Handles Me.PlaceholderParsing
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PlaceholderParsing()...", script.Name, script.Version)
            script.InitEvents("PlaceholderParsing")
        Next
    End Sub
    Public Sub RespondPlaceholderParsed() Handles Me.PlaceholderParsed
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event PlaceholderParsed()...", script.Name, script.Version)
            script.InitEvents("PlaceholderParsed")
        Next
    End Sub
    <Obsolete> Public Sub RespondPreWriteToDebugger() Handles Me.PreWriteToDebugger
        For Each script As IScript In scripts.Values
            script.InitEvents("PreWriteToDebugger")
        Next
    End Sub
    <Obsolete> Public Sub RespondPostWriteToDebugger() Handles Me.PostWriteToDebugger
        For Each script As IScript In scripts.Values
            script.InitEvents("PostWriteToDebugger")
        Next
    End Sub
    <Obsolete> Public Sub RespondPreWriteToConsole() Handles Me.PreWriteToConsole
        For Each script As IScript In scripts.Values
            script.InitEvents("PreWriteToConsole")
        Next
    End Sub
    <Obsolete> Public Sub RespondPostWriteToConsole() Handles Me.PostWriteToConsole
        For Each script As IScript In scripts.Values
            script.InitEvents("PostWriteToConsole")
        Next
    End Sub
    Public Sub RespondGarbageCollected() Handles Me.GarbageCollected
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event GarbageCollected()...", script.Name, script.Version)
            script.InitEvents("GarbageCollected")
        Next
    End Sub
    Public Sub RespondFTPShellInitialized() Handles Me.FTPShellInitialized
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event FTPShellInitialized()...", script.Name, script.Version)
            script.InitEvents("FTPShellInitialized")
        Next
    End Sub
    Public Sub RespondFTPPreExecuteCommand() Handles Me.FTPPreExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event FTPPreExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("FTPPreExecuteCommand")
        Next
    End Sub
    Public Sub RespondFTPPostExecuteCommand() Handles Me.FTPPostExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event FTPPostExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("FTPPostExecuteCommand")
        Next
    End Sub
    Public Sub RespondFTPCommandError() Handles Me.FTPCommandError
        For Each script As IScript In scripts.Values
            Wdbg("Mod {0} v{1} responded to event FTPCommandError()...", script.Name, script.Version)
            script.InitEvents("FTPCommandError")
        Next
    End Sub

    'These subs are for raising events
    Public Sub RaiseStartKernel()
        Wdbg("Raising event KernelStarted() and responding in RespondStartKernel()...")
        RaiseEvent KernelStarted()
    End Sub
    Public Sub RaisePreLogin()
        Wdbg("Raising event PreLogin() and responding in RespondPreLogin()...")
        RaiseEvent PreLogin()
    End Sub
    Public Sub RaisePostLogin()
        Wdbg("Raising event PostLogin() and responding in RespondPostLogin()...")
        RaiseEvent PostLogin()
    End Sub
    Public Sub RaiseShellInitialized()
        Wdbg("Raising event ShellInitialized() and responding in RespondShellInitialized()...")
        RaiseEvent ShellInitialized()
    End Sub
    Public Sub RaisePreExecuteCommand()
        Wdbg("Raising event PreExecuteCommand() and responding in RespondPreExecuteCommand()...")
        RaiseEvent PreExecuteCommand()
    End Sub
    Public Sub RaisePostExecuteCommand()
        Wdbg("Raising event PostExecuteCommand() and responding in RespondPostExecuteCommand()...")
        RaiseEvent PostExecuteCommand()
    End Sub
    Public Sub RaiseKernelError()
        Wdbg("Raising event KernelError() and responding in RespondKernelError()...")
        RaiseEvent KernelError()
    End Sub
    Public Sub RaiseContKernelError()
        Wdbg("Raising event ContKernelError() and responding in RespondContKernelError()...")
        RaiseEvent ContKernelError()
    End Sub
    Public Sub RaisePreShutdown()
        Wdbg("Raising event PreShutdown() and responding in RespondPreShutdown()...")
        RaiseEvent PreShutdown()
    End Sub
    Public Sub RaisePostShutdown()
        Wdbg("Raising event PostShutdown() and responding in RespondPostShutdown()...")
        RaiseEvent PostShutdown()
    End Sub
    Public Sub RaisePreReboot()
        Wdbg("Raising event PreReboot() and responding in RespondPreReboot()...")
        RaiseEvent PreReboot()
    End Sub
    Public Sub RaisePostReboot()
        Wdbg("Raising event PostReboot() and responding in RespondPostReboot()...")
        RaiseEvent PostReboot()
    End Sub
    Public Sub RaisePreShowScreensaver()
        Wdbg("Raising event PreShowScreensaver() and responding in RespondPreShowScreensaver()...")
        RaiseEvent PreShowScreensaver()
    End Sub
    Public Sub RaisePostShowScreensaver()
        Wdbg("Raising event PostShowScreensaver() and responding in RespondPostShowScreensaver()...")
        RaiseEvent PostShowScreensaver()
    End Sub
    Public Sub RaisePreUnlock()
        Wdbg("Raising event PreUnlock() and responding in RespondPreUnlock()...")
        RaiseEvent PreUnlock()
    End Sub
    Public Sub RaisePostUnlock()
        Wdbg("Raising event PostUnlock() and responding in RespondPostUnlock()...")
        RaiseEvent PostUnlock()
    End Sub
    Public Sub RaiseCommandError()
        Wdbg("Raising event CommandError() and responding in RespondCommandError()...")
        RaiseEvent CommandError()
    End Sub
    Public Sub RaisePreReloadConfig()
        Wdbg("Raising event PreReloadConfig() and responding in RespondPreReloadConfig()...")
        RaiseEvent PreReloadConfig()
    End Sub
    Public Sub RaisePostReloadConfig()
        Wdbg("Raising event PostReloadConfig() and responding in RespondPostReloadConfig()...")
        RaiseEvent PostReloadConfig()
    End Sub
    Public Sub RaisePreFetchNetworks()
        Wdbg("Raising event PreFetchNetworks() and responding in RespondPreFetchNetworks()...")
        RaiseEvent PreFetchNetworks()
    End Sub
    Public Sub RaisePostFetchNetworks()
        Wdbg("Raising event PostFetchNetworks() and responding in RespondPostFetchNetworks()...")
        RaiseEvent PostFetchNetworks()
    End Sub
    Public Sub RaisePlaceholderParsing()
        Wdbg("Raising event PlaceholderParsing() and responding in RespondPlaceholderParsing()...")
        RaiseEvent PlaceholderParsing()
    End Sub
    Public Sub RaisePlaceholderParsed()
        Wdbg("Raising event PlaceholderParsed() and responding in RespondPlaceholderParsed()...")
        RaiseEvent PlaceholderParsed()
    End Sub
    <Obsolete> Public Sub RaisePreWriteToDebugger()
        RaiseEvent PreWriteToDebugger()
    End Sub
    <Obsolete> Public Sub RaisePostWriteToDebugger()
        RaiseEvent PostWriteToDebugger()
    End Sub
    <Obsolete> Public Sub RaisePreWriteToConsole()
        RaiseEvent PreWriteToConsole()
    End Sub
    <Obsolete> Public Sub RaisePostWriteToConsole()
        RaiseEvent PostWriteToConsole()
    End Sub
    Public Sub RaiseGarbageCollected()
        Wdbg("Raising event GarbageCollected() and responding in RespondGarbageCollected()...")
        RaiseEvent GarbageCollected()
    End Sub
    Public Sub RaiseFTPShellInitialized()
        Wdbg("Raising event FTPShellInitialized() and responding in RespondFTPShellInitialized()...")
        RaiseEvent FTPShellInitialized()
    End Sub
    Public Sub RaiseFTPPreExecuteCommand()
        Wdbg("Raising event FTPPreExecuteCommand() and responding in RespondFTPPreExecuteCommand()...")
        RaiseEvent FTPPreExecuteCommand()
    End Sub
    Public Sub RaiseFTPPostExecuteCommand()
        Wdbg("Raising event FTPPostExecuteCommand() and responding in RespondFTPPostExecuteCommand()...")
        RaiseEvent FTPPostExecuteCommand()
    End Sub
    Public Sub RaiseFTPCommandError()
        Wdbg("Raising event FTPCommandError() and responding in RespondFTPCommandError()...")
        RaiseEvent FTPCommandError()
    End Sub

    'These classes are for exceptions. (For developers of mods: Only use if your mod is an extension to the kernel)
    Public Class NullUsersException
        Inherits Exception
        'This constructor without arguments
        Public Sub New()
            MyBase.New()
        End Sub
        'This constructor with a message argument
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        'This constructor with a message and an exception argument
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class
    Public Class NotEnoughArgumentsException
        Inherits Exception
        'This constructor without arguments
        Public Sub New()
            MyBase.New()
        End Sub
        'This constructor with a message argument
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        'This constructor with a message and an exception argument
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class
    Public Class FTPNotEnoughArgumentsException
        Inherits Exception
        'This constructor without arguments
        Public Sub New()
            MyBase.New()
        End Sub
        'This constructor with a message argument
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        'This constructor with a message and an exception argument
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class
    Public Class JsonNullException
        Inherits Exception
        'This constructor without arguments
        Public Sub New()
            MyBase.New()
        End Sub
        'This constructor with a message argument
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        'This constructor with a message and an exception argument
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class
    Public Class TruncatedManpageException
        Inherits Exception
        'This constructor without arguments
        Public Sub New()
            MyBase.New()
        End Sub
        'This constructor with a message argument
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        'This constructor with a message and an exception argument
        Public Sub New(ByVal message As String, ByVal e As Exception)
            MyBase.New(message, e)
        End Sub
    End Class

End Class
