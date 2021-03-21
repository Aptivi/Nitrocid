
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.IO

Public Class Events

    'These events are fired by their Raise<EventName>() subs and are responded by their Respond<EventName>() subs.
    Public Event KernelStarted()
    Public Event PreLogin()
    Public Event PostLogin(ByVal Username As String)
    Public Event LoginError(ByVal Username As String, ByVal Reason As String)
    Public Event ShellInitialized()
    Public Event PreExecuteCommand(ByVal Command As String)
    Public Event PostExecuteCommand(ByVal Command As String)
    Public Event KernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal Variables() As Object)
    Public Event ContKernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal Variables() As Object)
    Public Event PreShutdown()
    Public Event PostShutdown()
    Public Event PreReboot()
    Public Event PostReboot()
    Public Event PreShowScreensaver(ByVal Screensaver As String)
    Public Event PostShowScreensaver(ByVal Screensaver As String) 'After a key is pressed after screensaver is shown
    Public Event PreUnlock(ByVal Screensaver As String)
    Public Event PostUnlock(ByVal Screensaver As String)
    Public Event CommandError(ByVal Command As String, ByVal Exception As Exception)
    Public Event PreReloadConfig()
    Public Event PostReloadConfig()
    Public Event PlaceholderParsing(ByVal Target As String)
    Public Event PlaceholderParsed(ByVal Target As String)
    Public Event GarbageCollected()
    Public Event FTPShellInitialized()
    Public Event FTPPreExecuteCommand(ByVal Command As String)
    Public Event FTPPostExecuteCommand(ByVal Command As String)
    Public Event FTPCommandError(ByVal Command As String, ByVal Exception As Exception)
    Public Event FTPPreDownload(ByVal File As String)
    Public Event FTPPostDownload(ByVal File As String, ByVal Success As Boolean)
    Public Event FTPPreUpload(ByVal File As String)
    Public Event FTPPostUpload(ByVal File As String, ByVal Success As Boolean)
    Public Event IMAPShellInitialized()
    Public Event IMAPPreExecuteCommand(ByVal Command As String)
    Public Event IMAPPostExecuteCommand(ByVal Command As String)
    Public Event IMAPCommandError(ByVal Command As String, ByVal Exception As Exception)
    Public Event RemoteDebugConnectionAccepted(ByVal IP As String)
    Public Event RemoteDebugConnectionDisconnected(ByVal IP As String)
    Public Event RemoteDebugExecuteCommand(ByVal IP As String, ByVal Command As String)
    Public Event RemoteDebugCommandError(ByVal IP As String, ByVal Command As String, ByVal Exception As Exception)
    Public Event RPCCommandSent(ByVal Command As String)
    Public Event RPCCommandReceived(ByVal Command As String)
    Public Event RPCCommandError(ByVal Command As String, ByVal Exception As Exception)
    Public Event SFTPShellInitialized()
    Public Event SFTPPreExecuteCommand(ByVal Command As String)
    Public Event SFTPPostExecuteCommand(ByVal Command As String)
    Public Event SFTPCommandError(ByVal Command As String, ByVal Exception As Exception)
    Public Event SFTPPreDownload(ByVal File As String)
    Public Event SFTPPostDownload(ByVal File As String)
    Public Event SFTPDownloadError(ByVal File As String, ByVal Exception As Exception)
    Public Event SFTPPreUpload(ByVal File As String)
    Public Event SFTPPostUpload(ByVal File As String)
    Public Event SFTPUploadError(ByVal File As String, ByVal Exception As Exception)
    Public Event SSHConnected(ByVal Target As String)
    Public Event SSHDisconnected()
    Public Event SSHError(ByVal Exception As Exception)
    Public Event UESHPreExecute(ByVal Command As String)
    Public Event UESHPostExecute(ByVal Command As String)
    Public Event UESHError(ByVal Command As String, ByVal Exception As Exception)
    Public Event TextShellInitialized()
    Public Event TextPreExecuteCommand(ByVal Command As String)
    Public Event TextPostExecuteCommand(ByVal Command As String)
    Public Event TextCommandError(ByVal Command As String, ByVal Exception As Exception)
    Public Event NotificationSent(ByVal Notification As Notification)
    Public Event NotificationReceived(ByVal Notification As Notification)
    Public Event NotificationDismissed()
    Public Event ConfigSaved()
    Public Event ConfigSaveError(ByVal Exception As Exception)
    Public Event ConfigRead()
    Public Event ConfigReadError(ByVal Exception As Exception)
    Public Event PreExecuteModCommand(ByVal Command As String)
    Public Event PostExecuteModCommand(ByVal Command As String)
    Public Event ModParsed(ByVal Starting As Boolean, ByVal ModFileName As String)
    Public Event ModParseError(ByVal ModFileName As String)
    Public Event ModFinalized(ByVal Starting As Boolean, ByVal ModFileName As String)
    Public Event ModFinalizationFailed(ByVal ModFileName As String, ByVal Reason As String)
    Public Event UserAdded(ByVal Username As String)
    Public Event UserRemoved(ByVal Username As String)
    Public Event UsernameChanged(ByVal OldUsername As String, ByVal NewUsername As String)
    Public Event UserPasswordChanged(ByVal Username As String)
    Public Event HardwareProbing()
    Public Event HardwareProbed()
    Public Event CurrentDirectoryChanged()
    Public Event FileCreated(ByVal File As String)
    Public Event DirectoryCreated(ByVal Directory As String)
    Public Event FileCopied(ByVal Source As String, ByVal Destination As String)
    Public Event DirectoryCopied(ByVal Source As String, ByVal Destination As String)
    Public Event FileMoved(ByVal Source As String, ByVal Destination As String)
    Public Event DirectoryMoved(ByVal Source As String, ByVal Destination As String)
    Public Event FileRemoved(ByVal File As String)
    Public Event DirectoryRemoved(ByVal Directory As String)
    Public Event FileAttributeAdded(ByVal File As String, ByVal Attributes As FileAttributes)
    Public Event FileAttributeRemoved(ByVal File As String, ByVal Attributes As FileAttributes)
    Public Event ColorReset()
    Public Event ThemeSet(ByVal Theme As String)
    Public Event ThemeSetError(ByVal Theme As String, ByVal Reason As String)
    Public Event ColorSet()
    Public Event ColorSetError(ByVal Reason As String)
    Public Event ThemeStudioStarted()
    Public Event ThemeStudioExit()
    Public Event ArgumentsInjected(ByVal InjectedArguments As String)

    ''' <summary>
    ''' Makes the mod respond to the event of kernel start
    ''' </summary>
    Public Sub RespondStartKernel() Handles Me.KernelStarted
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event KernelStarted()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("KernelStarted")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-login
    ''' </summary>
    Public Sub RespondPreLogin() Handles Me.PreLogin
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreLogin()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreLogin")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-login
    ''' </summary>
    Public Sub RespondPostLogin(ByVal Username As String) Handles Me.PostLogin
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostLogin()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostLogin", Username)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of login error
    ''' </summary>
    Public Sub RespondLoginError(ByVal Username As String, ByVal Reason As String) Handles Me.LoginError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event LoginError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("LoginError", Username, Reason)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of the shell being initialized
    ''' </summary>
    Public Sub RespondShellInitialized() Handles Me.ShellInitialized
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ShellInitialized()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ShellInitialized")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-execute command
    ''' </summary>
    Public Sub RespondPreExecuteCommand(ByVal Command As String) Handles Me.PreExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-execute command
    ''' </summary>
    Public Sub RespondPostExecuteCommand(ByVal Command As String) Handles Me.PostExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of kernel error
    ''' </summary>
    Public Sub RespondKernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal Variables() As Object) Handles Me.KernelError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event KernelError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("KernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of continuable kernel error
    ''' </summary>
    Public Sub RespondContKernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal Variables() As Object) Handles Me.ContKernelError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ContKernelError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ContKernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-shutdown
    ''' </summary>
    Public Sub RespondPreShutdown() Handles Me.PreShutdown
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreShutdown()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreShutdown")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-shutdown
    ''' </summary>
    Public Sub RespondPostShutdown() Handles Me.PostShutdown
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostShutdown()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostShutdown")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-reboot
    ''' </summary>
    Public Sub RespondPreReboot() Handles Me.PreReboot
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreReboot()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreReboot")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-reboot
    ''' </summary>
    Public Sub RespondPostReboot() Handles Me.PostReboot
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostReboot()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostReboot")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-screensaver show
    ''' </summary>
    Public Sub RespondPreShowScreensaver(ByVal Screensaver As String) Handles Me.PreShowScreensaver
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreShowScreensaver()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreShowScreensaver", Screensaver)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-screensaver show
    ''' </summary>
    Public Sub RespondPostShowScreensaver(ByVal Screensaver As String) Handles Me.PostShowScreensaver
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostShowScreensaver()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostShowScreensaver", Screensaver)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-unlock
    ''' </summary>
    Public Sub RespondPreUnlock(ByVal Screensaver As String) Handles Me.PreUnlock
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreUnlock()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreUnlock", Screensaver)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-unlock
    ''' </summary>
    Public Sub RespondPostUnlock(ByVal Screensaver As String) Handles Me.PostUnlock
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostUnlock()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostUnlock", Screensaver)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of command error
    ''' </summary>
    Public Sub RespondCommandError(ByVal Command As String, ByVal Exception As Exception) Handles Me.CommandError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event CommandError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("CommandError", Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-reload config
    ''' </summary>
    Public Sub RespondPreReloadConfig() Handles Me.PreReloadConfig
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreReloadConfig()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreReloadConfig")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-reload config
    ''' </summary>
    Public Sub RespondPostReloadConfig() Handles Me.PostReloadConfig
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostReloadConfig()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostReloadConfig")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of a placeholder being parsed
    ''' </summary>
    Public Sub RespondPlaceholderParsing(ByVal Target As String) Handles Me.PlaceholderParsing
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PlaceholderParsing()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PlaceholderParsing", Target)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of a parsed placeholder
    ''' </summary>
    Public Sub RespondPlaceholderParsed(ByVal Target As String) Handles Me.PlaceholderParsed
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PlaceholderParsed()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PlaceholderParsed", Target)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of garbage collection finish
    ''' </summary>
    Public Sub RespondGarbageCollected() Handles Me.GarbageCollected
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event GarbageCollected()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("GarbageCollected")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP shell initialized
    ''' </summary>
    Public Sub RespondFTPShellInitialized() Handles Me.FTPShellInitialized
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPShellInitialized()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPShellInitialized")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-command execution
    ''' </summary>
    Public Sub RespondFTPPreExecuteCommand(ByVal Command As String) Handles Me.FTPPreExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPPreExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-command execution
    ''' </summary>
    Public Sub RespondFTPPostExecuteCommand(ByVal Command As String) Handles Me.FTPPostExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPPostExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP command error
    ''' </summary>
    Public Sub RespondFTPCommandError(ByVal Command As String, ByVal Exception As Exception) Handles Me.FTPCommandError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPCommandError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPCommandError", Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP pre-download
    ''' </summary>
    Public Sub RespondFTPPreDownload(ByVal File As String) Handles Me.FTPPreDownload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPPreDownload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPPreDownload", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP post-download
    ''' </summary>
    Public Sub RespondFTPPostDownload(ByVal File As String, ByVal Success As Boolean) Handles Me.FTPPostDownload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPPostDownload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPPostDownload", File, Success)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP pre-upload
    ''' </summary>
    Public Sub RespondFTPPreUpload(ByVal File As String) Handles Me.FTPPreUpload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPPreUpload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPPreUpload", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of FTP post-upload
    ''' </summary>
    Public Sub RespondFTPPostUpload(ByVal File As String, ByVal Success As Boolean) Handles Me.FTPPostUpload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FTPPostUpload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FTPPostUpload", File, Success)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP shell initialized
    ''' </summary>
    Public Sub RespondIMAPShellInitialized() Handles Me.IMAPShellInitialized
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event IMAPShellInitialized()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("IMAPShellInitialized")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP pre-command execution
    ''' </summary>
    Public Sub RespondIMAPPreExecuteCommand(ByVal Command As String) Handles Me.IMAPPreExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event IMAPPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("IMAPPreExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP post-command execution
    ''' </summary>
    Public Sub RespondIMAPPostExecuteCommand(ByVal Command As String) Handles Me.IMAPPostExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event IMAPPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("IMAPPostExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of IMAP command error
    ''' </summary>
    Public Sub RespondIMAPCommandError(ByVal Command As String, ByVal Exception As Exception) Handles Me.IMAPCommandError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event IMAPCommandError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("IMAPCommandError", Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging connection accepted
    ''' </summary>
    Public Sub RespondRemoteDebugConnectionAccepted(ByVal IP As String) Handles Me.RemoteDebugConnectionAccepted
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event RemoteDebugConnectionAccepted()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("RemoteDebugConnectionAccepted", IP)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging connection disconnected
    ''' </summary>
    Public Sub RespondRemoteDebugConnectionDisconnected(ByVal IP As String) Handles Me.RemoteDebugConnectionDisconnected
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event RemoteDebugConnectionDisconnected()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("RemoteDebugConnectionDisconnected", IP)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging command execution
    ''' </summary>
    Public Sub RespondRemoteDebugExecuteCommand(ByVal IP As String, ByVal Command As String) Handles Me.RemoteDebugExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event RemoteDebugExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("RemoteDebugExecuteCommand", IP, Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of remote debugging command error
    ''' </summary>
    Public Sub RespondRemoteDebugCommandError(ByVal IP As String, ByVal Command As String, ByVal Exception As Exception) Handles Me.RemoteDebugCommandError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event RemoteDebugCommandError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("RemoteDebugCommandError", IP, Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of RPC command sent
    ''' </summary>
    Public Sub RespondRPCCommandSent(ByVal Command As String) Handles Me.RPCCommandSent
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event RPCCommandSent()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("RPCCommandSent", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of RPC command received
    ''' </summary>
    Public Sub RespondRPCCommandReceived(ByVal Command As String) Handles Me.RPCCommandReceived
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event RPCCommandReceived()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("RPCCommandReceived", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of RPC command error
    ''' </summary>
    Public Sub RespondRPCCommandError(ByVal Command As String, ByVal Exception As Exception) Handles Me.RPCCommandError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event RPCCommandError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("RPCCommandError", Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP shell initialized
    ''' </summary>
    Public Sub RespondSFTPShellInitialized() Handles Me.SFTPShellInitialized
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPShellInitialized()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPShellInitialized")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of pre-command execution
    ''' </summary>
    Public Sub RespondSFTPPreExecuteCommand(ByVal Command As String) Handles Me.SFTPPreExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPPreExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of post-command execution
    ''' </summary>
    Public Sub RespondSFTPPostExecuteCommand(ByVal Command As String) Handles Me.SFTPPostExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPPostExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP command error
    ''' </summary>
    Public Sub RespondSFTPCommandError(ByVal Command As String, ByVal Exception As Exception) Handles Me.SFTPCommandError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPCommandError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPCommandError", Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP pre-download
    ''' </summary>
    Public Sub RespondSFTPPreDownload(ByVal File As String) Handles Me.SFTPPreDownload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPPreDownload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPPreDownload", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP post-download
    ''' </summary>
    Public Sub RespondSFTPPostDownload(ByVal File As String) Handles Me.SFTPPostDownload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPPostDownload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPPostDownload", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP download error
    ''' </summary>
    Public Sub RespondSFTPDownloadError(ByVal File As String, ByVal Exception As Exception) Handles Me.SFTPDownloadError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPDownloadError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPDownloadError", File, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP pre-upload
    ''' </summary>
    Public Sub RespondSFTPPreUpload(ByVal File As String) Handles Me.SFTPPreUpload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPPreUpload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPPreUpload", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP post-upload
    ''' </summary>
    Public Sub RespondSFTPPostUpload(ByVal File As String) Handles Me.SFTPPostUpload
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPPostUpload()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPPostUpload", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SFTP download error
    ''' </summary>
    Public Sub RespondSFTPUploadError(ByVal File As String, ByVal Exception As Exception) Handles Me.SFTPUploadError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SFTPUploadError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SFTPUploadError", File, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SSH being connected
    ''' </summary>
    Public Sub RespondSSHConnected(ByVal Target As String) Handles Me.SSHConnected
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SSHConnected()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SSHConnected", Target)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SSH being disconnected
    ''' </summary>
    Public Sub RespondSSHDisconnected() Handles Me.SSHDisconnected
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SSHDisconnected()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SSHDisconnected")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of SSH error
    ''' </summary>
    Public Sub RespondSSHError(ByVal Exception As Exception) Handles Me.SSHError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event SSHError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("SSHError", Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of UESH pre-execute
    ''' </summary>
    Public Sub RespondUESHPreExecute(ByVal Command As String) Handles Me.UESHPreExecute
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event UESHPreExecute()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("UESHPreExecute", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of UESH post-execute
    ''' </summary>
    Public Sub RespondUESHPostExecute(ByVal Command As String) Handles Me.UESHPostExecute
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event UESHPostExecute()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("UESHPostExecute", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of UESH post-execute
    ''' </summary>
    Public Sub RespondUESHError(ByVal Command As String, ByVal Exception As Exception) Handles Me.UESHError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event UESHError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("UESHError", Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text shell initialized
    ''' </summary>
    Public Sub RespondTextShellInitialized() Handles Me.TextShellInitialized
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event TextShellInitialized()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("TextShellInitialized")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text pre-command execution
    ''' </summary>
    Public Sub RespondTextPreExecuteCommand(ByVal Command As String) Handles Me.TextPreExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event TextPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("TextPreExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text post-command execution
    ''' </summary>
    Public Sub RespondTextPostExecuteCommand(ByVal Command As String) Handles Me.TextPostExecuteCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event TextPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("TextPostExecuteCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of text command error
    ''' </summary>
    Public Sub RespondTextCommandError(ByVal Command As String, ByVal Exception As Exception) Handles Me.TextCommandError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event TextCommandError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("TextCommandError", Command, Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of notification being sent
    ''' </summary>
    Public Sub RespondNotificationSent(ByVal Notification As Notification) Handles Me.NotificationSent
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event NotificationSent()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("NotificationSent", Notification)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of notification being received
    ''' </summary>
    Public Sub RespondNotificationReceived(ByVal Notification As Notification) Handles Me.NotificationReceived
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event NotificationReceived()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("NotificationReceived", Notification)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of notification being dismissed
    ''' </summary>
    Public Sub RespondNotificationDismissed() Handles Me.NotificationDismissed
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event NotificationDismissed()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("NotificationDismissed")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of config being saved
    ''' </summary>
    Public Sub RespondConfigSaved() Handles Me.ConfigSaved
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ConfigSaved()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ConfigSaved")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of config having problems saving
    ''' </summary>
    Public Sub RespondConfigSaveError(ByVal Exception As Exception) Handles Me.ConfigSaveError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ConfigSaveError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ConfigSaveError", Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of config being read
    ''' </summary>
    Public Sub RespondConfigRead() Handles Me.ConfigRead
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ConfigRead()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ConfigRead")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of config having problems reading
    ''' </summary>
    Public Sub RespondConfigReadError(ByVal Exception As Exception) Handles Me.ConfigReadError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ConfigReadError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ConfigReadError", Exception)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of mod command pre-execution
    ''' </summary>
    Public Sub RespondPreExecuteModCommand(ByVal Command As String) Handles Me.PreExecuteModCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PreExecuteModCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PreExecuteModCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of mod command post-execution
    ''' </summary>
    Public Sub RespondPostExecuteModCommand(ByVal Command As String) Handles Me.PostExecuteModCommand
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event PostExecuteModCommand()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("PostExecuteModCommand", Command)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of mod being parsed
    ''' </summary>
    Public Sub RespondModParsed(ByVal Starting As Boolean, ByVal ModFileName As String) Handles Me.ModParsed
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ModParsed()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ModParsed", Starting, ModFileName)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of mod having problems parsing
    ''' </summary>
    Public Sub RespondModParseError(ByVal ModFileName As String) Handles Me.ModParseError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ModParseError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ModParseError", ModFileName)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of mod being finalized
    ''' </summary>
    Public Sub RespondModFinalized(ByVal Starting As Boolean, ByVal ModFileName As String) Handles Me.ModFinalized
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ModFinalized()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ModFinalized", Starting, ModFileName)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of mod having problems finalizing
    ''' </summary>
    Public Sub RespondModFinalizationFailed(ByVal ModFileName As String, ByVal Reason As String) Handles Me.ModFinalizationFailed
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ModFinalizationFailed()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ModFinalizationFailed", ModFileName, Reason)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of user being added
    ''' </summary>
    Public Sub RespondUserAdded(ByVal Username As String) Handles Me.UserAdded
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event UserAdded()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("UserAdded", Username)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of user being removed
    ''' </summary>
    Public Sub RespondUserRemoved(ByVal Username As String) Handles Me.UserRemoved
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event UserRemoved()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("UserRemoved", Username)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of username being changed
    ''' </summary>
    Public Sub RespondUsernameChanged(ByVal OldUsername As String, ByVal NewUsername As String) Handles Me.UsernameChanged
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event UsernameChanged()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("UsernameChanged", OldUsername, NewUsername)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of user password being changed
    ''' </summary>
    Public Sub RespondUserPasswordChanged(ByVal Username As String) Handles Me.UserPasswordChanged
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event UserPasswordChanged()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("UserPasswordChanged", Username)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of hardware probing
    ''' </summary>
    Public Sub RespondHardwareProbing() Handles Me.HardwareProbing
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event HardwareProbing()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("HardwareProbing")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of hardware being probed
    ''' </summary>
    Public Sub RespondHardwareProbed() Handles Me.HardwareProbed
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event HardwareProbed()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("HardwareProbed")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of current directory being changed
    ''' </summary>
    Public Sub RespondCurrentDirectoryChanged() Handles Me.CurrentDirectoryChanged
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event CurrentDirectoryChanged()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("CurrentDirectoryChanged")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of file creation
    ''' </summary>
    Public Sub RespondFileCreated(ByVal File As String) Handles Me.FileCreated
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FileCreated()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FileCreated", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of directory creation
    ''' </summary>
    Public Sub RespondDirectoryCreated(ByVal Directory As String) Handles Me.DirectoryCreated
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event DirectoryCreated()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("DirectoryCreated", Directory)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of file copying process
    ''' </summary>
    Public Sub RespondFileCopied(ByVal Source As String, ByVal Destination As String) Handles Me.FileCopied
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FileCopied()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FileCopied", Source, Destination)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of directory copying process
    ''' </summary>
    Public Sub RespondDirectoryCopied(ByVal Source As String, ByVal Destination As String) Handles Me.DirectoryCopied
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event DirectoryCopied()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("DirectoryCopied", Source, Destination)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of file moving process
    ''' </summary>
    Public Sub RespondFileMoved(ByVal Source As String, ByVal Destination As String) Handles Me.FileMoved
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FileMoved()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FileMoved", Source, Destination)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of directory moving process
    ''' </summary>
    Public Sub RespondDirectoryMoved(ByVal Source As String, ByVal Destination As String) Handles Me.DirectoryMoved
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event DirectoryMoved()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("DirectoryMoved", Source, Destination)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of file removal
    ''' </summary>
    Public Sub RespondFileRemoved(ByVal File As String) Handles Me.FileRemoved
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FileRemoved()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FileRemoved", File)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of directory removal
    ''' </summary>
    Public Sub RespondDirectoryRemoved(ByVal Directory As String) Handles Me.DirectoryRemoved
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event DirectoryRemoved()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("DirectoryRemoved", Directory)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of file attribute addition
    ''' </summary>
    Public Sub RespondFileAttributeAdded(ByVal File As String, ByVal Attributes As FileAttributes) Handles Me.FileAttributeAdded
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FileAttributeAdded()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FileAttributeAdded", File, Attributes)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of file attribute removal
    ''' </summary>
    Public Sub RespondFileAttributeRemoved(ByVal File As String, ByVal Attributes As FileAttributes) Handles Me.FileAttributeRemoved
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event FileAttributeRemoved()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("FileAttributeRemoved", File, Attributes)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of console colors being reset
    ''' </summary>
    Public Sub RespondColorReset() Handles Me.ColorReset
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ColorReset()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ColorReset")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of theme setting
    ''' </summary>
    Public Sub RespondThemeSet(ByVal Theme As String) Handles Me.ThemeSet
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ThemeSet()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ThemeSet", Theme)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of theme setting problem
    ''' </summary>
    Public Sub RespondThemeSetError(ByVal Theme As String, ByVal Reason As String) Handles Me.ThemeSetError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ThemeSetError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ThemeSetError", Theme, Reason)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of console colors being set
    ''' </summary>
    Public Sub RespondColorSet() Handles Me.ColorSet
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ColorSet()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ColorSet")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of console colors having problems being set
    ''' </summary>
    Public Sub RespondColorSetError(ByVal Reason As String) Handles Me.ColorSetError
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ColorSetError()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ColorSetError", Reason)
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of theme studio start
    ''' </summary>
    Public Sub RespondThemeStudioStarted() Handles Me.ThemeStudioStarted
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ThemeStudioStarted()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ThemeStudioStarted")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of theme studio exit
    ''' </summary>
    Public Sub RespondThemeStudioExit() Handles Me.ThemeStudioExit
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ThemeStudioExit()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ThemeStudioExit")
            Next
        Next
    End Sub
    ''' <summary>
    ''' Makes the mod respond to the event of console colors having problems being set
    ''' </summary>
    Public Sub RespondArgumentsInjected(ByVal InjectedArguments As String) Handles Me.ArgumentsInjected
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                Wdbg("I", "{0} in mod {1} v{2} responded to event ArgumentsInjected()...", script.ModPart, script.Name, script.Version)
                script.InitEvents("ArgumentsInjected", InjectedArguments)
            Next
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
    Public Sub RaisePostLogin(ByVal Username As String)
        Wdbg("I", "Raising event PostLogin() and responding in RespondPostLogin()...")
        RaiseEvent PostLogin(Username)
    End Sub
    ''' <summary>
    ''' Raise an event of login error
    ''' </summary>
    Public Sub RaiseLoginError(ByVal Username As String, ByVal Reason As String)
        Wdbg("I", "Raising event LoginError() and responding in RespondLoginError()...")
        RaiseEvent LoginError(Username, Reason)
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
    Public Sub RaisePreExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event PreExecuteCommand() and responding in RespondPreExecuteCommand()...")
        RaiseEvent PreExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of post-command execution
    ''' </summary>
    Public Sub RaisePostExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event PostExecuteCommand() and responding in RespondPostExecuteCommand()...")
        RaiseEvent PostExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of kernel error
    ''' </summary>
    Public Sub RaiseKernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal Variables() As Object)
        Wdbg("I", "Raising event KernelError() and responding in RespondKernelError()...")
        RaiseEvent KernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)
    End Sub
    ''' <summary>
    ''' Raise an event of continuable kernel error
    ''' </summary>
    Public Sub RaiseContKernelError(ByVal ErrorType As Char, ByVal Reboot As Boolean, ByVal RebootTime As Long, ByVal Description As String, ByVal Exc As Exception, ByVal Variables() As Object)
        Wdbg("I", "Raising event ContKernelError() and responding in RespondContKernelError()...")
        RaiseEvent ContKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)
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
    Public Sub RaisePreShowScreensaver(ByVal Screensaver As String)
        Wdbg("I", "Raising event PreShowScreensaver() and responding in RespondPreShowScreensaver()...")
        RaiseEvent PreShowScreensaver(Screensaver)
    End Sub
    ''' <summary>
    ''' Raise an event of post-show screensaver
    ''' </summary>
    Public Sub RaisePostShowScreensaver(ByVal Screensaver As String)
        Wdbg("I", "Raising event PostShowScreensaver() and responding in RespondPostShowScreensaver()...")
        RaiseEvent PostShowScreensaver(Screensaver)
    End Sub
    ''' <summary>
    ''' Raise an event of pre-unlock
    ''' </summary>
    Public Sub RaisePreUnlock(ByVal Screensaver As String)
        Wdbg("I", "Raising event PreUnlock() and responding in RespondPreUnlock()...")
        RaiseEvent PreUnlock(Screensaver)
    End Sub
    ''' <summary>
    ''' Raise an event of post-unlock
    ''' </summary>
    Public Sub RaisePostUnlock(ByVal Screensaver As String)
        Wdbg("I", "Raising event PostUnlock() and responding in RespondPostUnlock()...")
        RaiseEvent PostUnlock(Screensaver)
    End Sub
    ''' <summary>
    ''' Raise an event of command error
    ''' </summary>
    Public Sub RaiseCommandError(ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event CommandError() and responding in RespondCommandError()...")
        RaiseEvent CommandError(Command, Exception)
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
    Public Sub RaisePlaceholderParsing(ByVal Target As String)
        Wdbg("I", "Raising event PlaceholderParsing() and responding in RespondPlaceholderParsing()...")
        RaiseEvent PlaceholderParsing(Target)
    End Sub
    ''' <summary>
    ''' Raise an event of a parsed placeholder
    ''' </summary>
    Public Sub RaisePlaceholderParsed(ByVal Target As String)
        Wdbg("I", "Raising event PlaceholderParsed() and responding in RespondPlaceholderParsed()...")
        RaiseEvent PlaceholderParsed(Target)
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
    Public Sub RaiseFTPPreExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event FTPPreExecuteCommand() and responding in RespondFTPPreExecuteCommand()...")
        RaiseEvent FTPPreExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of FTP post-execute command
    ''' </summary>
    Public Sub RaiseFTPPostExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event FTPPostExecuteCommand() and responding in RespondFTPPostExecuteCommand()...")
        RaiseEvent FTPPostExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of FTP command error
    ''' </summary>
    Public Sub RaiseFTPCommandError(ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event FTPCommandError() and responding in RespondFTPCommandError()...")
        RaiseEvent FTPCommandError(Command, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of FTP pre-download
    ''' </summary>
    Public Sub RaiseFTPPreDownload(ByVal File As String)
        Wdbg("I", "Raising event FTPPreDownload() and responding in RespondFTPPreDownload()...")
        RaiseEvent FTPPreDownload(File)
    End Sub
    ''' <summary>
    ''' Raise an event of FTP post-download
    ''' </summary>
    Public Sub RaiseFTPPostDownload(ByVal File As String, ByVal Success As Boolean)
        Wdbg("I", "Raising event FTPPostDownload() and responding in RespondFTPPostDownload()...")
        RaiseEvent FTPPostDownload(File, Success)
    End Sub
    ''' <summary>
    ''' Raise an event of FTP pre-upload
    ''' </summary>
    Public Sub RaiseFTPPreUpload(ByVal File As String)
        Wdbg("I", "Raising event FTPPreUpload() and responding in RespondFTPPreUpload()...")
        RaiseEvent FTPPreUpload(File)
    End Sub
    ''' <summary>
    ''' Raise an event of FTP post-upload
    ''' </summary>
    Public Sub RaiseFTPPostUpload(ByVal File As String, ByVal Success As Boolean)
        Wdbg("I", "Raising event FTPPostUpload() and responding in RespondFTPPostUpload()...")
        RaiseEvent FTPPostUpload(File, Success)
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
    Public Sub RaiseIMAPPreExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event IMAPPreExecuteCommand() and responding in RespondIMAPPreExecuteCommand()...")
        RaiseEvent IMAPPreExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of IMAP post-command execution
    ''' </summary>
    Public Sub RaiseIMAPPostExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event IMAPPostExecuteCommand() and responding in RespondIMAPPostExecuteCommand()...")
        RaiseEvent IMAPPostExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of IMAP command error
    ''' </summary>
    Public Sub RaiseIMAPCommandError(ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event IMAPCommandError() and responding in RespondIMAPCommandError()...")
        RaiseEvent IMAPCommandError(Command, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging connection accepted
    ''' </summary>
    Public Sub RaiseRemoteDebugConnectionAccepted(ByVal IP As String)
        Wdbg("I", "Raising event RemoteDebugConnectionAccepted() and responding in RespondRemoteDebugConnectionAccepted()...")
        RaiseEvent RemoteDebugConnectionAccepted(IP)
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging connection disconnected
    ''' </summary>
    Public Sub RaiseRemoteDebugConnectionDisconnected(ByVal IP As String)
        Wdbg("I", "Raising event RemoteDebugConnectionDisconnected() and responding in RespondRemoteDebugConnectionDisconnected()...")
        RaiseEvent RemoteDebugConnectionDisconnected(IP)
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging command execution
    ''' </summary>
    Public Sub RaiseRemoteDebugExecuteCommand(ByVal IP As String, ByVal Command As String)
        Wdbg("I", "Raising event RemoteDebugExecuteCommand() and responding in RespondRemoteDebugExecuteCommand()...")
        RaiseEvent RemoteDebugExecuteCommand(IP, Command)
    End Sub
    ''' <summary>
    ''' Raise an event of remote debugging command error
    ''' </summary>
    Public Sub RaiseRemoteDebugCommandError(ByVal IP As String, ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event RemoteDebugCommandError() and responding in RespondRemoteDebugCommandError()...")
        RaiseEvent RemoteDebugCommandError(IP, Command, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of RPC command sent
    ''' </summary>
    Public Sub RaiseRPCCommandSent(ByVal Command As String)
        Wdbg("I", "Raising event RPCCommandSent() and responding in RespondRPCCommandSent()...")
        RaiseEvent RPCCommandSent(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of RPC command received
    ''' </summary>
    Public Sub RaiseRPCCommandReceived(ByVal Command As String)
        Wdbg("I", "Raising event RPCCommandReceived() and responding in RespondRPCCommandReceived()...")
        RaiseEvent RPCCommandReceived(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of RPC command error
    ''' </summary>
    Public Sub RaiseRPCCommandError(ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event RPCCommandError() and responding in RespondRPCCommandError()...")
        RaiseEvent RPCCommandError(Command, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP shell initialized
    ''' </summary>
    Public Sub RaiseSFTPShellInitialized()
        Wdbg("I", "Raising event SFTPShellInitialized() and responding in RespondSFTPShellInitialized()...")
        RaiseEvent SFTPShellInitialized()
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP pre-execute command
    ''' </summary>
    Public Sub RaiseSFTPPreExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event SFTPPreExecuteCommand() and responding in RespondSFTPPreExecuteCommand()...")
        RaiseEvent SFTPPreExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP post-execute command
    ''' </summary>
    Public Sub RaiseSFTPPostExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event SFTPPostExecuteCommand() and responding in RespondSFTPPostExecuteCommand()...")
        RaiseEvent SFTPPostExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP command error
    ''' </summary>
    Public Sub RaiseSFTPCommandError(ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event SFTPCommandError() and responding in RespondSFTPCommandError()...")
        RaiseEvent SFTPCommandError(Command, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP pre-download
    ''' </summary>
    Public Sub RaiseSFTPPreDownload(ByVal File As String)
        Wdbg("I", "Raising event SFTPPreDownload() and responding in RespondSFTPPreDownload()...")
        RaiseEvent SFTPPreDownload(File)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP post-download
    ''' </summary>
    Public Sub RaiseSFTPPostDownload(ByVal File As String)
        Wdbg("I", "Raising event SFTPPostDownload() and responding in RespondSFTPPostDownload()...")
        RaiseEvent SFTPPostDownload(File)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP download error
    ''' </summary>
    Public Sub RaiseSFTPDownloadError(ByVal File As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event SFTPDownloadError() and responding in RespondSFTPDownloadError()...")
        RaiseEvent SFTPDownloadError(File, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP pre-upload
    ''' </summary>
    Public Sub RaiseSFTPPreUpload(ByVal File As String)
        Wdbg("I", "Raising event SFTPPreUpload() and responding in RespondSFTPPreUpload()...")
        RaiseEvent SFTPPreUpload(File)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP post-upload
    ''' </summary>
    Public Sub RaiseSFTPPostUpload(ByVal File As String)
        Wdbg("I", "Raising event SFTPPostUpload() and responding in RespondSFTPPostUpload()...")
        RaiseEvent SFTPPostUpload(File)
    End Sub
    ''' <summary>
    ''' Raise an event of SFTP upload error
    ''' </summary>
    Public Sub RaiseSFTPUploadError(ByVal File As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event SFTPUploadError() and responding in RespondSFTPUploadError()...")
        RaiseEvent SFTPUploadError(File, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of SSH being connected
    ''' </summary>
    Public Sub RaiseSSHConnected(ByVal Target As String)
        Wdbg("I", "Raising event SSHConnected() and responding in RespondSSHConnected()...")
        RaiseEvent SSHConnected(Target)
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
    Public Sub RaiseSSHError(ByVal Exception As Exception)
        Wdbg("I", "Raising event SSHError() and responding in RespondSSHError()...")
        RaiseEvent SSHError(Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of UESH pre-execute
    ''' </summary>
    Public Sub RaiseUESHPreExecute(ByVal Command As String)
        Wdbg("I", "Raising event UESHPreExecute() and responding in RespondUESHPreExecute()...")
        RaiseEvent UESHPreExecute(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of UESH post-execute
    ''' </summary>
    Public Sub RaiseUESHPostExecute(ByVal Command As String)
        Wdbg("I", "Raising event UESHPostExecute() and responding in RespondUESHPostExecute()...")
        RaiseEvent UESHPostExecute(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of UESH error
    ''' </summary>
    Public Sub RaiseUESHError(ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event UESHError() and responding in RespondUESHError()...")
        RaiseEvent UESHError(Command, Exception)
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
    Public Sub RaiseTextPreExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event TextPreExecuteCommand() and responding in RespondTextPreExecuteCommand()...")
        RaiseEvent TextPreExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of text post-command execution
    ''' </summary>
    Public Sub RaiseTextPostExecuteCommand(ByVal Command As String)
        Wdbg("I", "Raising event TextPostExecuteCommand() and responding in RespondTextPostExecuteCommand()...")
        RaiseEvent TextPostExecuteCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of text command error
    ''' </summary>
    Public Sub RaiseTextCommandError(ByVal Command As String, ByVal Exception As Exception)
        Wdbg("I", "Raising event TextCommandError() and responding in RespondTextCommandError()...")
        RaiseEvent TextCommandError(Command, Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of notification being sent
    ''' </summary>
    Public Sub RaiseNotificationSent(ByVal Notification As Notification)
        Wdbg("I", "Raising event NotificationSent() and responding in RespondNotificationSent()...")
        RaiseEvent NotificationSent(Notification)
    End Sub
    ''' <summary>
    ''' Raise an event of notification being received
    ''' </summary>
    Public Sub RaiseNotificationReceived(ByVal Notification As Notification)
        Wdbg("I", "Raising event NotificationReceived() and responding in RespondNotificationReceived()...")
        RaiseEvent NotificationReceived(Notification)
    End Sub
    ''' <summary>
    ''' Raise an event of notification being dismissed
    ''' </summary>
    Public Sub RaiseNotificationDismissed()
        Wdbg("I", "Raising event NotificationDismissed() and responding in RespondNotificationDismissed()...")
        RaiseEvent NotificationDismissed()
    End Sub
    ''' <summary>
    ''' Raise an event of config being saved
    ''' </summary>
    Public Sub RaiseConfigSaved()
        Wdbg("I", "Raising event ConfigSaved() and responding in RespondConfigSaved()...")
        RaiseEvent ConfigSaved()
    End Sub
    ''' <summary>
    ''' Raise an event of config having problems saving
    ''' </summary>
    Public Sub RaiseConfigSaveError(ByVal Exception As Exception)
        Wdbg("I", "Raising event ConfigSaveError() and responding in RespondConfigSaveError()...")
        RaiseEvent ConfigSaveError(Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of config being read
    ''' </summary>
    Public Sub RaiseConfigRead()
        Wdbg("I", "Raising event ConfigRead() and responding in RespondConfigRead()...")
        RaiseEvent ConfigRead()
    End Sub
    ''' <summary>
    ''' Raise an event of config having problems reading
    ''' </summary>
    Public Sub RaiseConfigReadError(ByVal Exception As Exception)
        Wdbg("I", "Raising event ConfigReadError() and responding in RespondConfigReadError()...")
        RaiseEvent ConfigReadError(Exception)
    End Sub
    ''' <summary>
    ''' Raise an event of mod command pre-execution
    ''' </summary>
    Public Sub RaisePreExecuteModCommand(ByVal Command As String)
        Wdbg("I", "Raising event PreExecuteModCommand() and responding in RespondPreExecuteModCommand()...")
        RaiseEvent PreExecuteModCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of mod command post-execution
    ''' </summary>
    Public Sub RaisePostExecuteModCommand(ByVal Command As String)
        Wdbg("I", "Raising event PostExecuteModCommand() and responding in RespondPostExecuteModCommand()...")
        RaiseEvent PostExecuteModCommand(Command)
    End Sub
    ''' <summary>
    ''' Raise an event of mod being parsed
    ''' </summary>
    Public Sub RaiseModParsed(ByVal Starting As Boolean, ByVal ModFileName As String)
        Wdbg("I", "Raising event ModParsed() and responding in RespondModParsed()...")
        RaiseEvent ModParsed(Starting, ModFileName)
    End Sub
    ''' <summary>
    ''' Raise an event of mod having problems parsing
    ''' </summary>
    Public Sub RaiseModParseError(ByVal ModFileName As String)
        Wdbg("I", "Raising event ModParseError() and responding in RespondModParseError()...")
        RaiseEvent ModParseError(ModFileName)
    End Sub
    ''' <summary>
    ''' Raise an event of mod being finalized
    ''' </summary>
    Public Sub RaiseModFinalized(ByVal Starting As Boolean, ByVal ModFileName As String)
        Wdbg("I", "Raising event ModFinalized() and responding in RespondModFinalized()...")
        RaiseEvent ModFinalized(Starting, ModFileName)
    End Sub
    ''' <summary>
    ''' Raise an event of mod having problems finalizing
    ''' </summary>
    Public Sub RaiseModFinalizationFailed(ByVal ModFileName As String, ByVal Reason As String)
        Wdbg("I", "Raising event ModFinalizationFailed() and responding in RespondModFinalizationFailed()...")
        RaiseEvent ModFinalizationFailed(ModFileName, Reason)
    End Sub
    ''' <summary>
    ''' Raise an event of user being added
    ''' </summary>
    Public Sub RaiseUserAdded(ByVal Username As String)
        Wdbg("I", "Raising event UserAdded() and responding in RespondUserAdded()...")
        RaiseEvent UserAdded(Username)
    End Sub
    ''' <summary>
    ''' Raise an event of user being removed
    ''' </summary>
    Public Sub RaiseUserRemoved(ByVal Username As String)
        Wdbg("I", "Raising event UserRemoved() and responding in RespondUserRemoved()...")
        RaiseEvent UserRemoved(Username)
    End Sub
    ''' <summary>
    ''' Raise an event of username being changed
    ''' </summary>
    Public Sub RaiseUsernameChanged(ByVal OldUsername As String, ByVal NewUsername As String)
        Wdbg("I", "Raising event UsernameChanged() and responding in RespondUsernameChanged()...")
        RaiseEvent UsernameChanged(OldUsername, NewUsername)
    End Sub
    ''' <summary>
    ''' Raise an event of user password being changed
    ''' </summary>
    Public Sub RaiseUserPasswordChanged(ByVal Username As String)
        Wdbg("I", "Raising event UserPasswordChanged() and responding in RespondUserPasswordChanged()...")
        RaiseEvent UserPasswordChanged(Username)
    End Sub
    ''' <summary>
    ''' Raise an event of hardware probing
    ''' </summary>
    Public Sub RaiseHardwareProbing()
        Wdbg("I", "Raising event HardwareProbing() and responding in RespondHardwareProbing()...")
        RaiseEvent HardwareProbing()
    End Sub
    ''' <summary>
    ''' Raise an event of hardware being probed
    ''' </summary>
    Public Sub RaiseHardwareProbed()
        Wdbg("I", "Raising event HardwareProbed() and responding in RespondHardwareProbed()...")
        RaiseEvent HardwareProbed()
    End Sub
    ''' <summary>
    ''' Raise an event of current directory being changed
    ''' </summary>
    Public Sub RaiseCurrentDirectoryChanged()
        Wdbg("I", "Raising event CurrentDirectoryChanged() and responding in RespondCurrentDirectoryChanged()...")
        RaiseEvent CurrentDirectoryChanged()
    End Sub
    ''' <summary>
    ''' Raise an event of file creation
    ''' </summary>
    Public Sub RaiseFileCreated(ByVal File As String)
        Wdbg("I", "Raising event FileCreated() and responding in RespondFileCreated()...")
        RaiseEvent FileCreated(File)
    End Sub
    ''' <summary>
    ''' Raise an event of directory creation
    ''' </summary>
    Public Sub RaiseDirectoryCreated(ByVal Directory As String)
        Wdbg("I", "Raising event DirectoryCreated() and responding in RespondDirectoryCreated()...")
        RaiseEvent DirectoryCreated(Directory)
    End Sub
    ''' <summary>
    ''' Raise an event of file copying process
    ''' </summary>
    Public Sub RaiseFileCopied(ByVal Source As String, ByVal Destination As String)
        Wdbg("I", "Raising event FileCopied() and responding in RespondFileCopied()...")
        RaiseEvent FileCopied(Source, Destination)
    End Sub
    ''' <summary>
    ''' Raise an event of directory copying process
    ''' </summary>
    Public Sub RaiseDirectoryCopied(ByVal Source As String, ByVal Destination As String)
        Wdbg("I", "Raising event DirectoryCopied() and responding in RespondDirectoryCopied()...")
        RaiseEvent DirectoryCopied(Source, Destination)
    End Sub
    ''' <summary>
    ''' Raise an event of file moving process
    ''' </summary>
    Public Sub RaiseFileMoved(ByVal Source As String, ByVal Destination As String)
        Wdbg("I", "Raising event FileMoved() and responding in RespondFileMoved()...")
        RaiseEvent FileMoved(Source, Destination)
    End Sub
    ''' <summary>
    ''' Raise an event of directory moving process
    ''' </summary>
    Public Sub RaiseDirectoryMoved(ByVal Source As String, ByVal Destination As String)
        Wdbg("I", "Raising event DirectoryMoved() and responding in RespondDirectoryMoved()...")
        RaiseEvent DirectoryMoved(Source, Destination)
    End Sub
    ''' <summary>
    ''' Raise an event of file removal
    ''' </summary>
    Public Sub RaiseFileRemoved(ByVal File As String)
        Wdbg("I", "Raising event FileRemoved() and responding in RespondFileRemoved()...")
        RaiseEvent FileRemoved(File)
    End Sub
    ''' <summary>
    ''' Raise an event of directory removal
    ''' </summary>
    Public Sub RaiseDirectoryRemoved(ByVal Directory As String)
        Wdbg("I", "Raising event DirectoryRemoved() and responding in RespondDirectoryRemoved()...")
        RaiseEvent DirectoryRemoved(Directory)
    End Sub
    ''' <summary>
    ''' Raise an event of file attribute addition
    ''' </summary>
    Public Sub RaiseFileAttributeAdded(ByVal File As String, ByVal Attributes As FileAttributes)
        Wdbg("I", "Raising event FileAttributeAdded() and responding in RespondFileAttributeAdded()...")
        RaiseEvent FileAttributeAdded(File, Attributes)
    End Sub
    ''' <summary>
    ''' Raise an event of file attribute removal
    ''' </summary>
    Public Sub RaiseFileAttributeRemoved(ByVal File As String, ByVal Attributes As FileAttributes)
        Wdbg("I", "Raising event FileAttributeRemoved() and responding in RespondFileAttributeRemoved()...")
        RaiseEvent FileAttributeRemoved(File, Attributes)
    End Sub
    ''' <summary>
    ''' Raise an event of console colors being reset
    ''' </summary>
    Public Sub RaiseColorReset()
        Wdbg("I", "Raising event ColorReset() and responding in RespondColorReset()...")
        RaiseEvent ColorReset()
    End Sub
    ''' <summary>
    ''' Raise an event of theme setting
    ''' </summary>
    Public Sub RaiseThemeSet(ByVal Theme As String)
        Wdbg("I", "Raising event ThemeSet() and responding in RespondThemeSet()...")
        RaiseEvent ThemeSet(Theme)
    End Sub
    ''' <summary>
    ''' Raise an event of theme setting problem
    ''' </summary>
    Public Sub RaiseThemeSetError(ByVal Theme As String, ByVal Reason As String)
        Wdbg("I", "Raising event ThemeSetError() and responding in RespondThemeSetError()...")
        RaiseEvent ThemeSetError(Theme, Reason)
    End Sub
    ''' <summary>
    ''' Raise an event of console colors being set
    ''' </summary>
    Public Sub RaiseColorSet()
        Wdbg("I", "Raising event ColorSet() and responding in RespondColorSet()...")
        RaiseEvent ColorSet()
    End Sub
    ''' <summary>
    ''' Raise an event of console colors having problems being set
    ''' </summary>
    Public Sub RaiseColorSetError(ByVal Reason As String)
        Wdbg("I", "Raising event ColorSetError() and responding in RespondColorSetError()...")
        RaiseEvent ColorSetError(Reason)
    End Sub
    ''' <summary>
    ''' Raise an event of theme studio start
    ''' </summary>
    Public Sub RaiseThemeStudioStarted()
        Wdbg("I", "Raising event ThemeStudioStarted() and responding in RespondThemeStudioStarted()...")
        RaiseEvent ThemeStudioStarted()
    End Sub
    ''' <summary>
    ''' Raise an event of theme studio exit
    ''' </summary>
    Public Sub RaiseThemeStudioExit()
        Wdbg("I", "Raising event ThemeStudioExit() and responding in RespondThemeStudioExit()...")
        RaiseEvent ThemeStudioExit()
    End Sub
    ''' <summary>
    ''' Raise an event of console colors having problems being set
    ''' </summary>
    Public Sub RaiseArgumentsInjected(ByVal InjectedArguments As String)
        Wdbg("I", "Raising event ArgumentsInjected() and responding in RespondArgumentsInjected()...")
        RaiseEvent ArgumentsInjected(InjectedArguments)
    End Sub

End Class
