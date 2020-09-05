
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
    Public Event PlaceholderParsing()
    Public Event PlaceholderParsed()
    Public Event GarbageCollected()
    Public Event FTPShellInitialized()
    Public Event FTPPreExecuteCommand()
    Public Event FTPPostExecuteCommand()
    Public Event FTPCommandError()
    Public Event FTPPreDownload()
    Public Event FTPPostDownload()
    Public Event FTPPreUpload()
    Public Event FTPPostUpload()
    Public Event IMAPShellInitialized()
    Public Event IMAPPreExecuteCommand()
    Public Event IMAPPostExecuteCommand()
    Public Event IMAPCommandError()
    Public Event RemoteDebugConnectionAccepted()
    Public Event RemoteDebugConnectionDisconnected()
    Public Event RemoteDebugExecuteCommand()
    Public Event RemoteDebugCommandError()
    Public Event RPCCommandSent()
    Public Event RPCCommandReceived()
    Public Event RPCCommandError()
    Public Event SSHConnected()
    Public Event SSHDisconnected()
    Public Event SSHError()
    Public Event UESHPreExecute()
    Public Event UESHPostExecute()
    Public Event UESHError()
    Public Event TextShellInitialized()
    Public Event TextPreExecuteCommand()
    Public Event TextPostExecuteCommand()
    Public Event TextCommandError()

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
    ''' <summary>
    ''' Makes the mod respond to the event of FTP pre-download
    ''' </summary>
    Public Sub RespondFTPPreDownload() Handles Me.FTPPreDownload
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPPreDownload()...", script.Name, script.Version)
            script.InitEvents("FTPPreDownload")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP post-download
    ''' </summary>
    Public Sub RespondFTPPostDownload() Handles Me.FTPPostDownload
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPPostDownload()...", script.Name, script.Version)
            script.InitEvents("FTPPostDownload")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP pre-upload
    ''' </summary>
    Public Sub RespondFTPPreUpload() Handles Me.FTPPreUpload
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPPreUpload()...", script.Name, script.Version)
            script.InitEvents("FTPPreUpload")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP post-upload
    ''' </summary>
    Public Sub RespondFTPPostUpload() Handles Me.FTPPostUpload
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event FTPPostUpload()...", script.Name, script.Version)
            script.InitEvents("FTPPostUpload")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP shell initialized
    ''' </summary>
    Public Sub RespondIMAPShellInitialized() Handles Me.IMAPShellInitialized
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event IMAPShellInitialized()...", script.Name, script.Version)
            script.InitEvents("IMAPShellInitialized")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP pre-command execution
    ''' </summary>
    Public Sub RespondIMAPPreExecuteCommand() Handles Me.IMAPPreExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event IMAPPreExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("IMAPPreExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP post-command execution
    ''' </summary>
    Public Sub RespondIMAPPostExecuteCommand() Handles Me.IMAPPostExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event IMAPPostExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("IMAPPostExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP command error
    ''' </summary>
    Public Sub RespondIMAPCommandError() Handles Me.IMAPCommandError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event IMAPCommandError()...", script.Name, script.Version)
            script.InitEvents("IMAPCommandError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging connection accepted
    ''' </summary>
    Public Sub RespondRemoteDebugConnectionAccepted() Handles Me.RemoteDebugConnectionAccepted
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event RemoteDebugConnectionAccepted()...", script.Name, script.Version)
            script.InitEvents("RemoteDebugConnectionAccepted")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging connection disconnected
    ''' </summary>
    Public Sub RespondRemoteDebugConnectionDisconnected() Handles Me.RemoteDebugConnectionDisconnected
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event RemoteDebugConnectionDisconnected()...", script.Name, script.Version)
            script.InitEvents("RemoteDebugConnectionDisconnected")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging command execution
    ''' </summary>
    Public Sub RespondRemoteDebugExecuteCommand() Handles Me.RemoteDebugExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event RemoteDebugExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("RemoteDebugExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging command error
    ''' </summary>
    Public Sub RespondRemoteDebugCommandError() Handles Me.RemoteDebugCommandError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event RemoteDebugCommandError()...", script.Name, script.Version)
            script.InitEvents("RemoteDebugCommandError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of RPC command sent
    ''' </summary>
    Public Sub RespondRPCCommandSent() Handles Me.RPCCommandSent
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event RPCCommandSent()...", script.Name, script.Version)
            script.InitEvents("RPCCommandSent")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of RPC command received
    ''' </summary>
    Public Sub RespondRPCCommandReceived() Handles Me.RPCCommandReceived
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event RPCCommandReceived()...", script.Name, script.Version)
            script.InitEvents("RPCCommandReceived")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of RPC command error
    ''' </summary>
    Public Sub RespondRPCCommandError() Handles Me.RPCCommandError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event RPCCommandError()...", script.Name, script.Version)
            script.InitEvents("RPCCommandError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SSH being connected
    ''' </summary>
    Public Sub RespondSSHConnected() Handles Me.SSHConnected
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event SSHConnected()...", script.Name, script.Version)
            script.InitEvents("SSHConnected")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SSH being disconnected
    ''' </summary>
    Public Sub RespondSSHDisconnected() Handles Me.SSHDisconnected
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event SSHDisconnected()...", script.Name, script.Version)
            script.InitEvents("SSHDisconnected")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SSH error
    ''' </summary>
    Public Sub RespondSSHError() Handles Me.SSHError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event SSHError()...", script.Name, script.Version)
            script.InitEvents("SSHError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of UESH pre-execute
    ''' </summary>
    Public Sub RespondUESHPreExecute() Handles Me.UESHPreExecute
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event UESHPreExecute()...", script.Name, script.Version)
            script.InitEvents("UESHPreExecute")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of UESH post-execute
    ''' </summary>
    Public Sub RespondUESHPostExecute() Handles Me.UESHPostExecute
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event UESHPostExecute()...", script.Name, script.Version)
            script.InitEvents("UESHPostExecute")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of UESH post-execute
    ''' </summary>
    Public Sub RespondUESHError() Handles Me.UESHError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event UESHError()...", script.Name, script.Version)
            script.InitEvents("UESHError")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text shell initialized
    ''' </summary>
    Public Sub RespondTextShellInitialized() Handles Me.TextShellInitialized
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event TextShellInitialized()...", script.Name, script.Version)
            script.InitEvents("TextShellInitialized")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text pre-command execution
    ''' </summary>
    Public Sub RespondTextPreExecuteCommand() Handles Me.TextPreExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event TextPreExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("TextPreExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text post-command execution
    ''' </summary>
    Public Sub RespondTextPostExecuteCommand() Handles Me.TextPostExecuteCommand
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event TextPostExecuteCommand()...", script.Name, script.Version)
            script.InitEvents("TextPostExecuteCommand")
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text command error
    ''' </summary>
    Public Sub RespondTextCommandError() Handles Me.TextCommandError
        For Each script As IScript In scripts.Values
            Wdbg("I", "Mod {0} v{1} responded to event TextCommandError()...", script.Name, script.Version)
            script.InitEvents("TextCommandError")
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
    ''' <summary>
    ''' Raise an event of FTP pre-download
    ''' </summary>
    Public Sub RaiseFTPPreDownload()
        Wdbg("I", "Raising event FTPPreDownload() and responding in RespondFTPPreDownload()...")
        RaiseEvent FTPPreDownload()
    End Sub
    ''' <summary>
    ''' Raise an event of FTP post-download
    ''' </summary>
    Public Sub RaiseFTPPostDownload()
        Wdbg("I", "Raising event FTPPostDownload() and responding in RespondFTPPostDownload()...")
        RaiseEvent FTPPostDownload()
    End Sub
    ''' <summary>
    ''' Raise an event of FTP pre-upload
    ''' </summary>
    Public Sub RaiseFTPPreUpload()
        Wdbg("I", "Raising event FTPPreUpload() and responding in RespondFTPPreUpload()...")
        RaiseEvent FTPPreUpload()
    End Sub
    ''' <summary>
    ''' Raise an event of FTP post-upload
    ''' </summary>
    Public Sub RaiseFTPPostUpload()
        Wdbg("I", "Raising event FTPPostUpload() and responding in RespondFTPPostUpload()...")
        RaiseEvent FTPPostUpload()
    End Sub
    ''' <summary>
    ''' Raise an event of IMAP shell initialized
    ''' </summary>
    Public Sub RaiseIMAPShellInitialized()
        Wdbg("I", "Raising event IMAPShellInitialized() and responding in RespondIMAPShellInitialized()...")
        RaiseEvent IMAPShellInitialized()
    End Sub
    ''' <summary>
    ''' Raise an event of IMAP pre-command execution
    ''' </summary>
    Public Sub RaiseIMAPPreExecuteCommand()
        Wdbg("I", "Raising event IMAPPreExecuteCommand() and responding in RespondIMAPPreExecuteCommand()...")
        RaiseEvent IMAPPreExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of IMAP post-command execution
    ''' </summary>
    Public Sub RaiseIMAPPostExecuteCommand()
        Wdbg("I", "Raising event IMAPPostExecuteCommand() and responding in RespondIMAPPostExecuteCommand()...")
        RaiseEvent IMAPPostExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of IMAP command error
    ''' </summary>
    Public Sub RaiseIMAPCommandError()
        Wdbg("I", "Raising event IMAPCommandError() and responding in RespondIMAPCommandError()...")
        RaiseEvent IMAPCommandError()
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging connection accepted
    ''' </summary>
    Public Sub RaiseRemoteDebugConnectionAccepted()
        Wdbg("I", "Raising event RemoteDebugConnectionAccepted() and responding in RespondRemoteDebugConnectionAccepted()...")
        RaiseEvent RemoteDebugConnectionAccepted()
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging connection disconnected
    ''' </summary>
    Public Sub RaiseRemoteDebugConnectionDisconnected()
        Wdbg("I", "Raising event RemoteDebugConnectionDisconnected() and responding in RespondRemoteDebugConnectionDisconnected()...")
        RaiseEvent RemoteDebugConnectionDisconnected()
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging command execution
    ''' </summary>
    Public Sub RaiseRemoteDebugExecuteCommand()
        Wdbg("I", "Raising event RemoteDebugExecuteCommand() and responding in RespondRemoteDebugExecuteCommand()...")
        RaiseEvent RemoteDebugExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging command error
    ''' </summary>
    Public Sub RaiseRemoteDebugCommandError()
        Wdbg("I", "Raising event RemoteDebugCommandError() and responding in RespondRemoteDebugCommandError()...")
        RaiseEvent RemoteDebugCommandError()
    End Sub
    ''' <summary>
    ''' Raise an event of RPC command sent
    ''' </summary>
    Public Sub RaiseRPCCommandSent()
        Wdbg("I", "Raising event RPCCommandSent() and responding in RespondRPCCommandSent()...")
        RaiseEvent RPCCommandSent()
    End Sub
    ''' <summary>
    ''' Raise an event of RPC command received
    ''' </summary>
    Public Sub RaiseRPCCommandReceived()
        Wdbg("I", "Raising event RPCCommandReceived() and responding in RespondRPCCommandReceived()...")
        RaiseEvent RPCCommandReceived()
    End Sub
    ''' <summary>
    ''' Raise an event of RPC command error
    ''' </summary>
    Public Sub RaiseRPCCommandError()
        Wdbg("I", "Raising event RPCCommandError() and responding in RespondRPCCommandError()...")
        RaiseEvent RPCCommandError()
    End Sub
    ''' <summary>
    ''' Raise an event of SSH being connected
    ''' </summary>
    Public Sub RaiseSSHConnected()
        Wdbg("I", "Raising event SSHConnected() and responding in RespondSSHConnected()...")
        RaiseEvent SSHConnected()
    End Sub
    ''' <summary>
    ''' Raise an event of SSH being disconnected
    ''' </summary>
    Public Sub RaiseSSHDisconnected()
        Wdbg("I", "Raising event SSHDisconnected() and responding in RespondSSHDisconnected()...")
        RaiseEvent SSHDisconnected()
    End Sub
    ''' <summary>
    ''' Raise an event of SSH error
    ''' </summary>
    Public Sub RaiseSSHError()
        Wdbg("I", "Raising event SSHError() and responding in RespondSSHError()...")
        RaiseEvent SSHError()
    End Sub
    ''' <summary>
    ''' Raise an event of UESH pre-execute
    ''' </summary>
    Public Sub RaiseUESHPreExecute()
        Wdbg("I", "Raising event UESHPreExecute() and responding in RespondUESHPreExecute()...")
        RaiseEvent UESHPreExecute()
    End Sub
    ''' <summary>
    ''' Raise an event of UESH post-execute
    ''' </summary>
    Public Sub RaiseUESHPostExecute()
        Wdbg("I", "Raising event UESHPostExecute() and responding in RespondUESHPostExecute()...")
        RaiseEvent UESHPostExecute()
    End Sub
    ''' <summary>
    ''' Raise an event of UESH error
    ''' </summary>
    Public Sub RaiseUESHError()
        Wdbg("I", "Raising event UESHError() and responding in RespondUESHError()...")
        RaiseEvent UESHError()
    End Sub
    ''' <summary>
    ''' Raise an event of text shell initialized
    ''' </summary>
    Public Sub RaiseTextShellInitialized()
        Wdbg("I", "Raising event TextShellInitialized() and responding in RespondTextShellInitialized()...")
        RaiseEvent TextShellInitialized()
    End Sub
    ''' <summary>
    ''' Raise an event of text pre-command execution
    ''' </summary>
    Public Sub RaiseTextPreExecuteCommand()
        Wdbg("I", "Raising event TextPreExecuteCommand() and responding in RespondTextPreExecuteCommand()...")
        RaiseEvent TextPreExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of text post-command execution
    ''' </summary>
    Public Sub RaiseTextPostExecuteCommand()
        Wdbg("I", "Raising event TextPostExecuteCommand() and responding in RespondTextPostExecuteCommand()...")
        RaiseEvent TextPostExecuteCommand()
    End Sub
    ''' <summary>
    ''' Raise an event of text command error
    ''' </summary>
    Public Sub RaiseTextCommandError()
        Wdbg("I", "Raising event TextCommandError() and responding in RespondTextCommandError()...")
        RaiseEvent TextCommandError()
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
    ''' Not enough command arguments supplied in all shells
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
    ''' Thrown when alias source and destination have the same name
    ''' </summary>
    Public Class AliasInvalidOperationException
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
    ''' Thrown when alias type is nonexistent
    ''' </summary>
    Public Class AliasNoSuchTypeException
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
    ''' Thrown when alias source command is nonexistent
    ''' </summary>
    Public Class AliasNoSuchCommandException
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
    ''' Thrown when alias already exists
    ''' </summary>
    Public Class AliasAlreadyExistsException
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
    ''' Thrown when alias is nonexistent
    ''' </summary>
    Public Class AliasNoSuchAliasException
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
    ''' Thrown when language is nonexistent
    ''' </summary>
    Public Class NoSuchLanguageException
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
    ''' Thrown when synth file is invalid
    ''' </summary>
    Public Class InvalidSynthException
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
    ''' Thrown when screensaver is nonexistent
    ''' </summary>
    Public Class NoSuchScreensaverException
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
    ''' Thrown when there is a config error
    ''' </summary>
    Public Class ConfigException
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
    ''' Thrown when there is a user creation error
    ''' </summary>
    Public Class UserCreationException
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
    ''' Thrown when there is a user management error
    ''' </summary>
    Public Class UserManagementException
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
    ''' Thrown when there is a user management error
    ''' </summary>
    Public Class PermissionManagementException
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
    ''' Thrown when there is a hostname error
    ''' </summary>
    Public Class HostnameException
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
