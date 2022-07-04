
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports KS.Misc.Notifications
Imports KS.Modifications

Namespace Kernel
    Public Class Events

        ''' <summary>
        ''' Recently fired events
        ''' </summary>
        Friend Property FiredEvents As New Dictionary(Of String, Object())

        'These events are fired by their Raise<EventName>() subs and are responded by their Respond<EventName>() subs.
        Public Event KernelStarted()
        Public Event PreLogin()
        Public Event PostLogin(Username As String)
        Public Event LoginError(Username As String, Reason As LoginErrorReasons)
        Public Event ShellInitialized()
        Public Event PreExecuteCommand(Command As String)
        Public Event PostExecuteCommand(Command As String)
        Public Event KernelError(ErrorType As KernelErrorLevel, Reboot As Boolean, RebootTime As Long, Description As String, Exc As Exception, Variables() As Object)
        Public Event ContKernelError(ErrorType As KernelErrorLevel, Reboot As Boolean, RebootTime As Long, Description As String, Exc As Exception, Variables() As Object)
        Public Event PreShutdown()
        Public Event PostShutdown()
        Public Event PreReboot()
        Public Event PostReboot()
        Public Event PreShowScreensaver(Screensaver As String)
        Public Event PostShowScreensaver(Screensaver As String) 'After a key is pressed after screensaver is shown
        Public Event PreUnlock(Screensaver As String)
        Public Event PostUnlock(Screensaver As String)
        Public Event CommandError(Command As String, Exception As Exception)
        Public Event PreReloadConfig()
        Public Event PostReloadConfig()
        Public Event PlaceholderParsing(Target As String)
        Public Event PlaceholderParsed(Target As String)
        Public Event PlaceholderParseError(Target As String, Exception As Exception)
        Public Event GarbageCollected()
        Public Event FTPShellInitialized()
        Public Event FTPPreExecuteCommand(Command As String)
        Public Event FTPPostExecuteCommand(Command As String)
        Public Event FTPCommandError(Command As String, Exception As Exception)
        Public Event FTPPreDownload(File As String)
        Public Event FTPPostDownload(File As String, Success As Boolean)
        Public Event FTPPreUpload(File As String)
        Public Event FTPPostUpload(File As String, Success As Boolean)
        Public Event IMAPShellInitialized()
        Public Event IMAPPreExecuteCommand(Command As String)
        Public Event IMAPPostExecuteCommand(Command As String)
        Public Event IMAPCommandError(Command As String, Exception As Exception)
        Public Event RemoteDebugConnectionAccepted(IP As String)
        Public Event RemoteDebugConnectionDisconnected(IP As String)
        Public Event RemoteDebugExecuteCommand(IP As String, Command As String)
        Public Event RemoteDebugCommandError(IP As String, Command As String, Exception As Exception)
        Public Event RPCCommandSent(Command As String, Argument As String, IP As String, Port As Integer)
        Public Event RPCCommandReceived(Command As String, IP As String, Port As Integer)
        Public Event RPCCommandError(Command As String, Exception As Exception, IP As String, Port As Integer)
        Public Event RSSShellInitialized(FeedUrl As String)
        Public Event RSSPreExecuteCommand(FeedUrl As String, Command As String)
        Public Event RSSPostExecuteCommand(FeedUrl As String, Command As String)
        Public Event RSSCommandError(FeedUrl As String, Command As String, Exception As Exception)
        Public Event SFTPShellInitialized()
        Public Event SFTPPreExecuteCommand(Command As String)
        Public Event SFTPPostExecuteCommand(Command As String)
        Public Event SFTPCommandError(Command As String, Exception As Exception)
        Public Event SFTPPreDownload(File As String)
        Public Event SFTPPostDownload(File As String)
        Public Event SFTPDownloadError(File As String, Exception As Exception)
        Public Event SFTPPreUpload(File As String)
        Public Event SFTPPostUpload(File As String)
        Public Event SFTPUploadError(File As String, Exception As Exception)
        Public Event SSHConnected(Target As String)
        Public Event SSHDisconnected()
        Public Event SSHPreExecuteCommand(Target As String, Command As String)
        Public Event SSHPostExecuteCommand(Target As String, Command As String)
        Public Event SSHCommandError(Target As String, Command As String, Exception As Exception)
        Public Event SSHError(Exception As Exception)
        Public Event UESHPreExecute(Command As String, Arguments As String)
        Public Event UESHPostExecute(Command As String, Arguments As String)
        Public Event UESHError(Command As String, Arguments As String, Exception As Exception)
        Public Event TextShellInitialized()
        Public Event TextPreExecuteCommand(Command As String)
        Public Event TextPostExecuteCommand(Command As String)
        Public Event TextCommandError(Command As String, Exception As Exception)
        Public Event NotificationSent(Notification As Notification)
        Public Event NotificationsSent(Notifications As List(Of Notification))
        Public Event NotificationReceived(Notification As Notification)
        Public Event NotificationsReceived(Notifications As List(Of Notification))
        Public Event NotificationDismissed()
        Public Event ConfigSaved()
        Public Event ConfigSaveError(Exception As Exception)
        Public Event ConfigRead()
        Public Event ConfigReadError(Exception As Exception)
        Public Event PreExecuteModCommand(Command As String)
        Public Event PostExecuteModCommand(Command As String)
        Public Event ModParsed(ModFileName As String)
        Public Event ModParseError(ModFileName As String)
        Public Event ModFinalized(ModFileName As String)
        Public Event ModFinalizationFailed(ModFileName As String, Reason As String)
        Public Event UserAdded(Username As String)
        Public Event UserRemoved(Username As String)
        Public Event UsernameChanged(OldUsername As String, NewUsername As String)
        Public Event UserPasswordChanged(Username As String)
        Public Event HardwareProbing()
        Public Event HardwareProbed()
        Public Event CurrentDirectoryChanged()
        Public Event FileCreated(File As String)
        Public Event DirectoryCreated(Directory As String)
        Public Event FileCopied(Source As String, Destination As String)
        Public Event DirectoryCopied(Source As String, Destination As String)
        Public Event FileMoved(Source As String, Destination As String)
        Public Event DirectoryMoved(Source As String, Destination As String)
        Public Event FileRemoved(File As String)
        Public Event DirectoryRemoved(Directory As String)
        Public Event FileAttributeAdded(File As String, Attributes As FileAttributes)
        Public Event FileAttributeRemoved(File As String, Attributes As FileAttributes)
        Public Event ColorReset()
        Public Event ThemeSet(Theme As String)
        Public Event ThemeSetError(Theme As String, Reason As ThemeSetErrorReasons)
        Public Event ColorSet()
        Public Event ColorSetError(Reason As ColorSetErrorReasons)
        Public Event ThemeStudioStarted()
        Public Event ThemeStudioExit()
        Public Event ArgumentsInjected(InjectedArguments As List(Of String))
        Public Event ZipShellInitialized()
        Public Event ZipPreExecuteCommand(Command As String)
        Public Event ZipPostExecuteCommand(Command As String)
        Public Event ZipCommandError(Command As String, Exception As Exception)
        Public Event HTTPShellInitialized()
        Public Event HTTPPreExecuteCommand(Command As String)
        Public Event HTTPPostExecuteCommand(Command As String)
        Public Event HTTPCommandError(Command As String, Exception As Exception)
        Public Event ProcessError(Process As String, Exception As Exception)
        Public Event LanguageInstalled(Language As String)
        Public Event LanguageUninstalled(Language As String)
        Public Event LanguageInstallError(Language As String, Exception As Exception)
        Public Event LanguageUninstallError(Language As String, Exception As Exception)
        Public Event LanguagesInstalled()
        Public Event LanguagesUninstalled()
        Public Event LanguagesInstallError(Exception As Exception)
        Public Event LanguagesUninstallError(Exception As Exception)
        Public Event HexShellInitialized()
        Public Event HexPreExecuteCommand(Command As String)
        Public Event HexPostExecuteCommand(Command As String)
        Public Event HexCommandError(Command As String, Exception As Exception)
        Public Event TestShellInitialized()
        Public Event TestPreExecuteCommand(Command As String)
        Public Event TestPostExecuteCommand(Command As String)
        Public Event TestCommandError(Command As String, Exception As Exception)
        Public Event JsonShellInitialized()
        Public Event JsonPreExecuteCommand(Command As String)
        Public Event JsonPostExecuteCommand(Command As String)
        Public Event JsonCommandError(Command As String, Exception As Exception)

        ''' <summary>
        ''' Makes the mod respond to the event of kernel start
        ''' </summary>
        Public Sub RespondStartKernel() Handles Me.KernelStarted
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event KernelStarted()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("KernelStarted")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-login
        ''' </summary>
        Public Sub RespondPreLogin() Handles Me.PreLogin
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreLogin()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreLogin")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-login
        ''' </summary>
        Public Sub RespondPostLogin(Username As String) Handles Me.PostLogin
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostLogin()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostLogin", Username)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of login error
        ''' </summary>
        Public Sub RespondLoginError(Username As String, Reason As String) Handles Me.LoginError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LoginError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LoginError", Username, Reason)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of the shell being initialized
        ''' </summary>
        Public Sub RespondShellInitialized() Handles Me.ShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-execute command
        ''' </summary>
        Public Sub RespondPreExecuteCommand(Command As String) Handles Me.PreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-execute command
        ''' </summary>
        Public Sub RespondPostExecuteCommand(Command As String) Handles Me.PostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of kernel error
        ''' </summary>
        Public Sub RespondKernelError(ErrorType As KernelErrorLevel, Reboot As Boolean, RebootTime As Long, Description As String, Exc As Exception, Variables() As Object) Handles Me.KernelError
            For Each ModPart As ModInfo In Mods?.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event KernelError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("KernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of continuable kernel error
        ''' </summary>
        Public Sub RespondContKernelError(ErrorType As KernelErrorLevel, Reboot As Boolean, RebootTime As Long, Description As String, Exc As Exception, Variables() As Object) Handles Me.ContKernelError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ContKernelError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ContKernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-shutdown
        ''' </summary>
        Public Sub RespondPreShutdown() Handles Me.PreShutdown
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreShutdown()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreShutdown")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-shutdown
        ''' </summary>
        Public Sub RespondPostShutdown() Handles Me.PostShutdown
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostShutdown()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostShutdown")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-reboot
        ''' </summary>
        Public Sub RespondPreReboot() Handles Me.PreReboot
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreReboot()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreReboot")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-reboot
        ''' </summary>
        Public Sub RespondPostReboot() Handles Me.PostReboot
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostReboot()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostReboot")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-screensaver show
        ''' </summary>
        Public Sub RespondPreShowScreensaver(Screensaver As String) Handles Me.PreShowScreensaver
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreShowScreensaver()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreShowScreensaver", Screensaver)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-screensaver show
        ''' </summary>
        Public Sub RespondPostShowScreensaver(Screensaver As String) Handles Me.PostShowScreensaver
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostShowScreensaver()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostShowScreensaver", Screensaver)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-unlock
        ''' </summary>
        Public Sub RespondPreUnlock(Screensaver As String) Handles Me.PreUnlock
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreUnlock()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreUnlock", Screensaver)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-unlock
        ''' </summary>
        Public Sub RespondPostUnlock(Screensaver As String) Handles Me.PostUnlock
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostUnlock()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostUnlock", Screensaver)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of command error
        ''' </summary>
        Public Sub RespondCommandError(Command As String, Exception As Exception) Handles Me.CommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event CommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("CommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-reload config
        ''' </summary>
        Public Sub RespondPreReloadConfig() Handles Me.PreReloadConfig
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreReloadConfig()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreReloadConfig")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-reload config
        ''' </summary>
        Public Sub RespondPostReloadConfig() Handles Me.PostReloadConfig
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostReloadConfig()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostReloadConfig")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of a placeholder being parsed
        ''' </summary>
        Public Sub RespondPlaceholderParsing(Target As String) Handles Me.PlaceholderParsing
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PlaceholderParsing()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PlaceholderParsing", Target)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of a parsed placeholder
        ''' </summary>
        Public Sub RespondPlaceholderParsed(Target As String) Handles Me.PlaceholderParsed
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PlaceholderParsed()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PlaceholderParsed", Target)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of a placeholder parse error
        ''' </summary>
        Public Sub RespondPlaceholderParseError(Target As String, Exception As Exception) Handles Me.PlaceholderParseError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PlaceholderParseError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PlaceholderParseError", Target, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of garbage collection finish
        ''' </summary>
        Public Sub RespondGarbageCollected() Handles Me.GarbageCollected
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event GarbageCollected()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("GarbageCollected")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of FTP shell initialized
        ''' </summary>
        Public Sub RespondFTPShellInitialized() Handles Me.FTPShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-command execution
        ''' </summary>
        Public Sub RespondFTPPreExecuteCommand(Command As String) Handles Me.FTPPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-command execution
        ''' </summary>
        Public Sub RespondFTPPostExecuteCommand(Command As String) Handles Me.FTPPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of FTP command error
        ''' </summary>
        Public Sub RespondFTPCommandError(Command As String, Exception As Exception) Handles Me.FTPCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of FTP pre-download
        ''' </summary>
        Public Sub RespondFTPPreDownload(File As String) Handles Me.FTPPreDownload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPreDownload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPPreDownload", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of FTP post-download
        ''' </summary>
        Public Sub RespondFTPPostDownload(File As String, Success As Boolean) Handles Me.FTPPostDownload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPostDownload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPPostDownload", File, Success)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of FTP pre-upload
        ''' </summary>
        Public Sub RespondFTPPreUpload(File As String) Handles Me.FTPPreUpload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPreUpload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPPreUpload", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of FTP post-upload
        ''' </summary>
        Public Sub RespondFTPPostUpload(File As String, Success As Boolean) Handles Me.FTPPostUpload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPostUpload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FTPPostUpload", File, Success)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of IMAP shell initialized
        ''' </summary>
        Public Sub RespondIMAPShellInitialized() Handles Me.IMAPShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("IMAPShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of IMAP pre-command execution
        ''' </summary>
        Public Sub RespondIMAPPreExecuteCommand(Command As String) Handles Me.IMAPPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("IMAPPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of IMAP post-command execution
        ''' </summary>
        Public Sub RespondIMAPPostExecuteCommand(Command As String) Handles Me.IMAPPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("IMAPPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of IMAP command error
        ''' </summary>
        Public Sub RespondIMAPCommandError(Command As String, Exception As Exception) Handles Me.IMAPCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("IMAPCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of remote debugging connection accepted
        ''' </summary>
        Public Sub RespondRemoteDebugConnectionAccepted(IP As String) Handles Me.RemoteDebugConnectionAccepted
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugConnectionAccepted()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RemoteDebugConnectionAccepted", IP)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of remote debugging connection disconnected
        ''' </summary>
        Public Sub RespondRemoteDebugConnectionDisconnected(IP As String) Handles Me.RemoteDebugConnectionDisconnected
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugConnectionDisconnected()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RemoteDebugConnectionDisconnected", IP)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of remote debugging command execution
        ''' </summary>
        Public Sub RespondRemoteDebugExecuteCommand(IP As String, Command As String) Handles Me.RemoteDebugExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RemoteDebugExecuteCommand", IP, Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of remote debugging command error
        ''' </summary>
        Public Sub RespondRemoteDebugCommandError(IP As String, Command As String, Exception As Exception) Handles Me.RemoteDebugCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RemoteDebugCommandError", IP, Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of RPC command sent
        ''' </summary>
        Public Sub RespondRPCCommandSent(Command As String, Argument As String, IP As String, Port As Integer) Handles Me.RPCCommandSent
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RPCCommandSent()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RPCCommandSent", Command, Argument, IP, Port)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of RPC command received
        ''' </summary>
        Public Sub RespondRPCCommandReceived(Command As String, IP As String, Port As Integer) Handles Me.RPCCommandReceived
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RPCCommandReceived()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RPCCommandReceived", Command, IP, Port)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of RPC command error
        ''' </summary>
        Public Sub RespondRPCCommandError(Command As String, Exception As Exception, IP As String, Port As Integer) Handles Me.RPCCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RPCCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RPCCommandError", Command, Exception, IP, Port)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of RSS shell initialized
        ''' </summary>
        Public Sub RespondRSSShellInitialized(FeedUrl As String) Handles Me.RSSShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RSSShellInitialized", FeedUrl)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-command execution
        ''' </summary>
        Public Sub RespondRSSPreExecuteCommand(FeedUrl As String, Command As String) Handles Me.RSSPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RSSPreExecuteCommand", FeedUrl, Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-command execution
        ''' </summary>
        Public Sub RespondRSSPostExecuteCommand(FeedUrl As String, Command As String) Handles Me.RSSPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RSSPostExecuteCommand", FeedUrl, Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of RSS command error
        ''' </summary>
        Public Sub RespondRSSCommandError(FeedUrl As String, Command As String, Exception As Exception) Handles Me.RSSCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("RSSCommandError", FeedUrl, Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP shell initialized
        ''' </summary>
        Public Sub RespondSFTPShellInitialized() Handles Me.SFTPShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-command execution
        ''' </summary>
        Public Sub RespondSFTPPreExecuteCommand(Command As String) Handles Me.SFTPPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-command execution
        ''' </summary>
        Public Sub RespondSFTPPostExecuteCommand(Command As String) Handles Me.SFTPPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP command error
        ''' </summary>
        Public Sub RespondSFTPCommandError(Command As String, Exception As Exception) Handles Me.SFTPCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP pre-download
        ''' </summary>
        Public Sub RespondSFTPPreDownload(File As String) Handles Me.SFTPPreDownload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPreDownload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPPreDownload", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP post-download
        ''' </summary>
        Public Sub RespondSFTPPostDownload(File As String) Handles Me.SFTPPostDownload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPostDownload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPPostDownload", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP download error
        ''' </summary>
        Public Sub RespondSFTPDownloadError(File As String, Exception As Exception) Handles Me.SFTPDownloadError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPDownloadError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPDownloadError", File, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP pre-upload
        ''' </summary>
        Public Sub RespondSFTPPreUpload(File As String) Handles Me.SFTPPreUpload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPreUpload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPPreUpload", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP post-upload
        ''' </summary>
        Public Sub RespondSFTPPostUpload(File As String) Handles Me.SFTPPostUpload
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPostUpload()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPPostUpload", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SFTP download error
        ''' </summary>
        Public Sub RespondSFTPUploadError(File As String, Exception As Exception) Handles Me.SFTPUploadError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPUploadError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SFTPUploadError", File, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SSH being connected
        ''' </summary>
        Public Sub RespondSSHConnected(Target As String) Handles Me.SSHConnected
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHConnected()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SSHConnected", Target)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SSH being disconnected
        ''' </summary>
        Public Sub RespondSSHDisconnected() Handles Me.SSHDisconnected
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHDisconnected()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SSHDisconnected")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-command execution
        ''' </summary>
        Public Sub RespondSSHPreExecuteCommand(Target As String, Command As String) Handles Me.SSHPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SSHPreExecuteCommand", Target, Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-command execution
        ''' </summary>
        Public Sub RespondSSHPostExecuteCommand(Target As String, Command As String) Handles Me.SSHPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SSHPostExecuteCommand", Target, Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SSH command error
        ''' </summary>
        Public Sub RespondSSHCommandError(Target As String, Command As String, Exception As Exception) Handles Me.SSHCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SSHCommandError", Target, Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of SSH error
        ''' </summary>
        Public Sub RespondSSHError(Exception As Exception) Handles Me.SSHError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("SSHError", Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of UESH pre-execute
        ''' </summary>
        Public Sub RespondUESHPreExecute(Command As String, Arguments As String) Handles Me.UESHPreExecute
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UESHPreExecute()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("UESHPreExecute", Command, Arguments)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of UESH post-execute
        ''' </summary>
        Public Sub RespondUESHPostExecute(Command As String, Arguments As String) Handles Me.UESHPostExecute
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UESHPostExecute()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("UESHPostExecute", Command, Arguments)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of UESH post-execute
        ''' </summary>
        Public Sub RespondUESHError(Command As String, Arguments As String, Exception As Exception) Handles Me.UESHError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UESHError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("UESHError", Command, Arguments, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of text shell initialized
        ''' </summary>
        Public Sub RespondTextShellInitialized() Handles Me.TextShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TextShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of text pre-command execution
        ''' </summary>
        Public Sub RespondTextPreExecuteCommand(Command As String) Handles Me.TextPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TextPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of text post-command execution
        ''' </summary>
        Public Sub RespondTextPostExecuteCommand(Command As String) Handles Me.TextPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TextPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of text command error
        ''' </summary>
        Public Sub RespondTextCommandError(Command As String, Exception As Exception) Handles Me.TextCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TextCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of notification being sent
        ''' </summary>
        Public Sub RespondNotificationSent(Notification As Notification) Handles Me.NotificationSent
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationSent()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("NotificationSent", Notification)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of notifications being sent
        ''' </summary>
        Public Sub RespondNotificationsSent(Notifications As List(Of Notification)) Handles Me.NotificationsSent
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationsSent()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("NotificationsSent", Notifications)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of notification being received
        ''' </summary>
        Public Sub RespondNotificationReceived(Notification As Notification) Handles Me.NotificationReceived
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationReceived()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("NotificationReceived", Notification)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of notifications being received
        ''' </summary>
        Public Sub RespondNotificationsReceived(Notifications As List(Of Notification)) Handles Me.NotificationsReceived
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationsReceived()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("NotificationsReceived", Notifications)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of notification being dismissed
        ''' </summary>
        Public Sub RespondNotificationDismissed() Handles Me.NotificationDismissed
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationDismissed()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("NotificationDismissed")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of config being saved
        ''' </summary>
        Public Sub RespondConfigSaved() Handles Me.ConfigSaved
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigSaved()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ConfigSaved")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of config having problems saving
        ''' </summary>
        Public Sub RespondConfigSaveError(Exception As Exception) Handles Me.ConfigSaveError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigSaveError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ConfigSaveError", Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of config being read
        ''' </summary>
        Public Sub RespondConfigRead() Handles Me.ConfigRead
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigRead()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ConfigRead")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of config having problems reading
        ''' </summary>
        Public Sub RespondConfigReadError(Exception As Exception) Handles Me.ConfigReadError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigReadError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ConfigReadError", Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of mod command pre-execution
        ''' </summary>
        Public Sub RespondPreExecuteModCommand(Command As String) Handles Me.PreExecuteModCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreExecuteModCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PreExecuteModCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of mod command post-execution
        ''' </summary>
        Public Sub RespondPostExecuteModCommand(Command As String) Handles Me.PostExecuteModCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostExecuteModCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("PostExecuteModCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of mod being parsed
        ''' </summary>
        Public Sub RespondModParsed(ModFileName As String) Handles Me.ModParsed
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModParsed()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ModParsed", ModFileName)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of mod having problems parsing
        ''' </summary>
        Public Sub RespondModParseError(ModFileName As String) Handles Me.ModParseError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModParseError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ModParseError", ModFileName)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of mod being finalized
        ''' </summary>
        Public Sub RespondModFinalized(ModFileName As String) Handles Me.ModFinalized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModFinalized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ModFinalized", ModFileName)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of mod having problems finalizing
        ''' </summary>
        Public Sub RespondModFinalizationFailed(ModFileName As String, Reason As String) Handles Me.ModFinalizationFailed
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModFinalizationFailed()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ModFinalizationFailed", ModFileName, Reason)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of user being added
        ''' </summary>
        Public Sub RespondUserAdded(Username As String) Handles Me.UserAdded
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UserAdded()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("UserAdded", Username)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of user being removed
        ''' </summary>
        Public Sub RespondUserRemoved(Username As String) Handles Me.UserRemoved
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UserRemoved()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("UserRemoved", Username)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of username being changed
        ''' </summary>
        Public Sub RespondUsernameChanged(OldUsername As String, NewUsername As String) Handles Me.UsernameChanged
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UsernameChanged()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("UsernameChanged", OldUsername, NewUsername)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of user password being changed
        ''' </summary>
        Public Sub RespondUserPasswordChanged(Username As String) Handles Me.UserPasswordChanged
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UserPasswordChanged()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("UserPasswordChanged", Username)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of hardware probing
        ''' </summary>
        Public Sub RespondHardwareProbing() Handles Me.HardwareProbing
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HardwareProbing()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HardwareProbing")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of hardware being probed
        ''' </summary>
        Public Sub RespondHardwareProbed() Handles Me.HardwareProbed
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HardwareProbed()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HardwareProbed")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of current directory being changed
        ''' </summary>
        Public Sub RespondCurrentDirectoryChanged() Handles Me.CurrentDirectoryChanged
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event CurrentDirectoryChanged()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("CurrentDirectoryChanged")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of file creation
        ''' </summary>
        Public Sub RespondFileCreated(File As String) Handles Me.FileCreated
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileCreated()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FileCreated", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of directory creation
        ''' </summary>
        Public Sub RespondDirectoryCreated(Directory As String) Handles Me.DirectoryCreated
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryCreated()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("DirectoryCreated", Directory)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of file copying process
        ''' </summary>
        Public Sub RespondFileCopied(Source As String, Destination As String) Handles Me.FileCopied
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileCopied()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FileCopied", Source, Destination)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of directory copying process
        ''' </summary>
        Public Sub RespondDirectoryCopied(Source As String, Destination As String) Handles Me.DirectoryCopied
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryCopied()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("DirectoryCopied", Source, Destination)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of file moving process
        ''' </summary>
        Public Sub RespondFileMoved(Source As String, Destination As String) Handles Me.FileMoved
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileMoved()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FileMoved", Source, Destination)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of directory moving process
        ''' </summary>
        Public Sub RespondDirectoryMoved(Source As String, Destination As String) Handles Me.DirectoryMoved
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryMoved()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("DirectoryMoved", Source, Destination)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of file removal
        ''' </summary>
        Public Sub RespondFileRemoved(File As String) Handles Me.FileRemoved
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileRemoved()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FileRemoved", File)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of directory removal
        ''' </summary>
        Public Sub RespondDirectoryRemoved(Directory As String) Handles Me.DirectoryRemoved
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryRemoved()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("DirectoryRemoved", Directory)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of file attribute addition
        ''' </summary>
        Public Sub RespondFileAttributeAdded(File As String, Attributes As FileAttributes) Handles Me.FileAttributeAdded
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileAttributeAdded()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FileAttributeAdded", File, Attributes)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of file attribute removal
        ''' </summary>
        Public Sub RespondFileAttributeRemoved(File As String, Attributes As FileAttributes) Handles Me.FileAttributeRemoved
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileAttributeRemoved()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("FileAttributeRemoved", File, Attributes)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of console colors being reset
        ''' </summary>
        Public Sub RespondColorReset() Handles Me.ColorReset
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ColorReset()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ColorReset")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of theme setting
        ''' </summary>
        Public Sub RespondThemeSet(Theme As String) Handles Me.ThemeSet
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeSet()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ThemeSet", Theme)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of theme setting problem
        ''' </summary>
        Public Sub RespondThemeSetError(Theme As String, Reason As String) Handles Me.ThemeSetError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeSetError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ThemeSetError", Theme, Reason)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of console colors being set
        ''' </summary>
        Public Sub RespondColorSet() Handles Me.ColorSet
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ColorSet()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ColorSet")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of console colors having problems being set
        ''' </summary>
        Public Sub RespondColorSetError(Reason As String) Handles Me.ColorSetError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ColorSetError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ColorSetError", Reason)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of theme studio start
        ''' </summary>
        Public Sub RespondThemeStudioStarted() Handles Me.ThemeStudioStarted
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeStudioStarted()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ThemeStudioStarted")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of theme studio exit
        ''' </summary>
        Public Sub RespondThemeStudioExit() Handles Me.ThemeStudioExit
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeStudioExit()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ThemeStudioExit")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of console colors having problems being set
        ''' </summary>
        Public Sub RespondArgumentsInjected(InjectedArguments As List(Of String)) Handles Me.ArgumentsInjected
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ArgumentsInjected()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ArgumentsInjected", InjectedArguments)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of ZIP shell initialized
        ''' </summary>
        Public Sub RespondZipShellInitialized() Handles Me.ZipShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ZipShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of ZIP pre-command execution
        ''' </summary>
        Public Sub RespondZipPreExecuteCommand(Command As String) Handles Me.ZipPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ZipPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of ZIP post-command execution
        ''' </summary>
        Public Sub RespondZipPostExecuteCommand(Command As String) Handles Me.ZipPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ZipPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of ZIP command error
        ''' </summary>
        Public Sub RespondZipCommandError(Command As String, Exception As Exception) Handles Me.ZipCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ZipCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of HTTP shell initialized
        ''' </summary>
        Public Sub RespondHTTPShellInitialized() Handles Me.HTTPShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HTTPShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of pre-command execution
        ''' </summary>
        Public Sub RespondHTTPPreExecuteCommand(Command As String) Handles Me.HTTPPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HTTPPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of post-command execution
        ''' </summary>
        Public Sub RespondHTTPPostExecuteCommand(Command As String) Handles Me.HTTPPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HTTPPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of HTTP command error
        ''' </summary>
        Public Sub RespondHTTPCommandError(Command As String, Exception As Exception) Handles Me.HTTPCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HTTPCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of process error
        ''' </summary>
        Public Sub RespondProcessError(Process As String, Exception As Exception) Handles Me.ProcessError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ProcessError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("ProcessError", Process, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of a custom language installation
        ''' </summary>
        Public Sub RespondLanguageInstalled(Language As String) Handles Me.LanguageInstalled
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageInstalled()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguageInstalled", Language)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of a custom language uninstallation
        ''' </summary>
        Public Sub RespondLanguageUninstalled(Language As String) Handles Me.LanguageUninstalled
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageUninstalled()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguageUninstalled", Language)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of custom language install error
        ''' </summary>
        Public Sub RespondLanguageInstallError(Language As String, Exception As Exception) Handles Me.LanguageInstallError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageInstallError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguageInstallError", Language, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of custom language uninstall error
        ''' </summary>
        Public Sub RespondLanguageUninstallError(Language As String, Exception As Exception) Handles Me.LanguageUninstallError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageUninstallError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguageUninstallError", Language, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of custom languages installation
        ''' </summary>
        Public Sub RespondLanguagesInstalled() Handles Me.LanguagesInstalled
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesInstalled()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguagesInstalled")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of custom languages uninstallation
        ''' </summary>
        Public Sub RespondLanguagesUninstalled() Handles Me.LanguagesUninstalled
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesUninstalled()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguagesUninstalled")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of custom languages install error
        ''' </summary>
        Public Sub RespondLanguagesInstallError(Exception As Exception) Handles Me.LanguagesInstallError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesInstallError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguagesInstallError", Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of custom languages uninstall error
        ''' </summary>
        Public Sub RespondLanguagesUninstallError(Exception As Exception) Handles Me.LanguagesUninstallError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesUninstallError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("LanguagesUninstallError", Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Hex shell initialized
        ''' </summary>
        Public Sub RespondHexShellInitialized() Handles Me.HexShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HexShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Hex pre-command execution
        ''' </summary>
        Public Sub RespondHexPreExecuteCommand(Command As String) Handles Me.HexPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HexPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Hex post-command execution
        ''' </summary>
        Public Sub RespondHexPostExecuteCommand(Command As String) Handles Me.HexPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HexPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Hex command error
        ''' </summary>
        Public Sub RespondHexCommandError(Command As String, Exception As Exception) Handles Me.HexCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("HexCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Json shell initialized
        ''' </summary>
        Public Sub RespondJsonShellInitialized() Handles Me.JsonShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("JsonShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Json pre-command execution
        ''' </summary>
        Public Sub RespondJsonPreExecuteCommand(Command As String) Handles Me.JsonPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("JsonPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Json post-command execution
        ''' </summary>
        Public Sub RespondJsonPostExecuteCommand(Command As String) Handles Me.JsonPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("JsonPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Json command error
        ''' </summary>
        Public Sub RespondJsonCommandError(Command As String, Exception As Exception) Handles Me.JsonCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("JsonCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Test shell initialized
        ''' </summary>
        Public Sub RespondTestShellInitialized() Handles Me.TestShellInitialized
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestShellInitialized()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TestShellInitialized")
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Test pre-command execution
        ''' </summary>
        Public Sub RespondTestPreExecuteCommand(Command As String) Handles Me.TestPreExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestPreExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TestPreExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Test post-command execution
        ''' </summary>
        Public Sub RespondTestPostExecuteCommand(Command As String) Handles Me.TestPostExecuteCommand
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestPostExecuteCommand()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TestPostExecuteCommand", Command)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub
        ''' <summary>
        ''' Makes the mod respond to the event of Test command error
        ''' </summary>
        Public Sub RespondTestCommandError(Command As String, Exception As Exception) Handles Me.TestCommandError
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Try
                        Dim script As IScript = PartInfo.PartScript
                        WdbgConditional(EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestCommandError()...", script.ModPart, script.Name, script.Version)
                        script.InitEvents("TestCommandError", Command, Exception)
                    Catch ex As Exception
                        WdbgConditional(EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message)
                        WStkTrcConditional(EventDebug, ex)
                    End Try
                Next
            Next
        End Sub

        'These subs are for raising events
        ''' <summary>
        ''' Raise an event of kernel start
        ''' </summary>
        Public Sub RaiseStartKernel()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event KernelStarted() and responding in RespondStartKernel()...")
            FiredEvents.Add("KernelStarted (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent KernelStarted()
        End Sub
        ''' <summary>
        ''' Raise an event of pre-login
        ''' </summary>
        Public Sub RaisePreLogin()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreLogin() and responding in RespondPreLogin()...")
            FiredEvents.Add("PreLogin (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent PreLogin()
        End Sub
        ''' <summary>
        ''' Raise an event of post-login
        ''' </summary>
        Public Sub RaisePostLogin(Username As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostLogin() and responding in RespondPostLogin()...")
            FiredEvents.Add("PostLogin (" + CStr(FiredEvents.Count) + ")", {Username})
            RaiseEvent PostLogin(Username)
        End Sub
        ''' <summary>
        ''' Raise an event of login error
        ''' </summary>
        Public Sub RaiseLoginError(Username As String, Reason As LoginErrorReasons)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LoginError() and responding in RespondLoginError()...")
            FiredEvents.Add("LoginError (" + CStr(FiredEvents.Count) + ")", {Username, Reason})
            RaiseEvent LoginError(Username, Reason)
        End Sub
        ''' <summary>
        ''' Raise an event of shell initialized
        ''' </summary>
        Public Sub RaiseShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ShellInitialized() and responding in RespondShellInitialized()...")
            FiredEvents.Add("ShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of pre-command execution
        ''' </summary>
        Public Sub RaisePreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreExecuteCommand() and responding in RespondPreExecuteCommand()...")
            FiredEvents.Add("PreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent PreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of post-command execution
        ''' </summary>
        Public Sub RaisePostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostExecuteCommand() and responding in RespondPostExecuteCommand()...")
            FiredEvents.Add("PostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent PostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of kernel error
        ''' </summary>
        Public Sub RaiseKernelError(ErrorType As KernelErrorLevel, Reboot As Boolean, RebootTime As Long, Description As String, Exc As Exception, Variables() As Object)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event KernelError() and responding in RespondKernelError()...")
            FiredEvents.Add("KernelError (" + CStr(FiredEvents.Count) + ")", {ErrorType, Reboot, RebootTime, Description, Exc, Variables})
            RaiseEvent KernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)
        End Sub
        ''' <summary>
        ''' Raise an event of continuable kernel error
        ''' </summary>
        Public Sub RaiseContKernelError(ErrorType As KernelErrorLevel, Reboot As Boolean, RebootTime As Long, Description As String, Exc As Exception, Variables() As Object)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ContKernelError() and responding in RespondContKernelError()...")
            FiredEvents.Add("ContKernelError (" + CStr(FiredEvents.Count) + ")", {ErrorType, Reboot, RebootTime, Description, Exc, Variables})
            RaiseEvent ContKernelError(ErrorType, Reboot, RebootTime, Description, Exc, Variables)
        End Sub
        ''' <summary>
        ''' Raise an event of pre-shutdown
        ''' </summary>
        Public Sub RaisePreShutdown()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreShutdown() and responding in RespondPreShutdown()...")
            FiredEvents.Add("PreShutdown (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent PreShutdown()
        End Sub
        ''' <summary>
        ''' Raise an event of post-shutdown
        ''' </summary>
        Public Sub RaisePostShutdown()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostShutdown() and responding in RespondPostShutdown()...")
            FiredEvents.Add("PostShutdown (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent PostShutdown()
        End Sub
        ''' <summary>
        ''' Raise an event of pre-reboot
        ''' </summary>
        Public Sub RaisePreReboot()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreReboot() and responding in RespondPreReboot()...")
            FiredEvents.Add("PreReboot (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent PreReboot()
        End Sub
        ''' <summary>
        ''' Raise an event of post-reboot
        ''' </summary>
        Public Sub RaisePostReboot()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostReboot() and responding in RespondPostReboot()...")
            FiredEvents.Add("PostReboot (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent PostReboot()
        End Sub
        ''' <summary>
        ''' Raise an event of pre-show screensaver
        ''' </summary>
        Public Sub RaisePreShowScreensaver(Screensaver As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreShowScreensaver() and responding in RespondPreShowScreensaver()...")
            FiredEvents.Add("PreShowScreensaver (" + CStr(FiredEvents.Count) + ")", {Screensaver})
            RaiseEvent PreShowScreensaver(Screensaver)
        End Sub
        ''' <summary>
        ''' Raise an event of post-show screensaver
        ''' </summary>
        Public Sub RaisePostShowScreensaver(Screensaver As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostShowScreensaver() and responding in RespondPostShowScreensaver()...")
            FiredEvents.Add("PostShowScreensaver (" + CStr(FiredEvents.Count) + ")", {Screensaver})
            RaiseEvent PostShowScreensaver(Screensaver)
        End Sub
        ''' <summary>
        ''' Raise an event of pre-unlock
        ''' </summary>
        Public Sub RaisePreUnlock(Screensaver As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreUnlock() and responding in RespondPreUnlock()...")
            FiredEvents.Add("PreUnlock (" + CStr(FiredEvents.Count) + ")", {Screensaver})
            RaiseEvent PreUnlock(Screensaver)
        End Sub
        ''' <summary>
        ''' Raise an event of post-unlock
        ''' </summary>
        Public Sub RaisePostUnlock(Screensaver As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostUnlock() and responding in RespondPostUnlock()...")
            FiredEvents.Add("PostUnlock (" + CStr(FiredEvents.Count) + ")", {Screensaver})
            RaiseEvent PostUnlock(Screensaver)
        End Sub
        ''' <summary>
        ''' Raise an event of command error
        ''' </summary>
        Public Sub RaiseCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event CommandError() and responding in RespondCommandError()...")
            FiredEvents.Add("CommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent CommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of pre-reload config
        ''' </summary>
        Public Sub RaisePreReloadConfig()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreReloadConfig() and responding in RespondPreReloadConfig()...")
            FiredEvents.Add("PreReloadConfig (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent PreReloadConfig()
        End Sub
        ''' <summary>
        ''' Raise an event of post-reload config
        ''' </summary>
        Public Sub RaisePostReloadConfig()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostReloadConfig() and responding in RespondPostReloadConfig()...")
            FiredEvents.Add("PostReloadConfig (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent PostReloadConfig()
        End Sub
        ''' <summary>
        ''' Raise an event of placeholders being parsed
        ''' </summary>
        Public Sub RaisePlaceholderParsing(Target As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PlaceholderParsing() and responding in RespondPlaceholderParsing()...")
            FiredEvents.Add("PlaceholderParsing (" + CStr(FiredEvents.Count) + ")", {Target})
            RaiseEvent PlaceholderParsing(Target)
        End Sub
        ''' <summary>
        ''' Raise an event of a parsed placeholder
        ''' </summary>
        Public Sub RaisePlaceholderParsed(Target As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PlaceholderParsed() and responding in RespondPlaceholderParsed()...")
            FiredEvents.Add("PlaceholderParsed (" + CStr(FiredEvents.Count) + ")", {Target})
            RaiseEvent PlaceholderParsed(Target)
        End Sub
        ''' <summary>
        ''' Raise an event of a placeholder parse error
        ''' </summary>
        Public Sub RaisePlaceholderParseError(Target As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PlaceholderParseError() and responding in RespondPlaceholderParseError()...")
            FiredEvents.Add("PlaceholderParseError (" + CStr(FiredEvents.Count) + ")", {Target, Exception})
            RaiseEvent PlaceholderParseError(Target, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of garbage collection finish
        ''' </summary>
        Public Sub RaiseGarbageCollected()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event GarbageCollected() and responding in RespondGarbageCollected()...")
            FiredEvents.Add("GarbageCollected (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent GarbageCollected()
        End Sub
        ''' <summary>
        ''' Raise an event of FTP shell initialized
        ''' </summary>
        Public Sub RaiseFTPShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPShellInitialized() and responding in RespondFTPShellInitialized()...")
            FiredEvents.Add("FTPShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent FTPShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of FTP pre-execute command
        ''' </summary>
        Public Sub RaiseFTPPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPPreExecuteCommand() and responding in RespondFTPPreExecuteCommand()...")
            FiredEvents.Add("FTPPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent FTPPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of FTP post-execute command
        ''' </summary>
        Public Sub RaiseFTPPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPPostExecuteCommand() and responding in RespondFTPPostExecuteCommand()...")
            FiredEvents.Add("FTPPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent FTPPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of FTP command error
        ''' </summary>
        Public Sub RaiseFTPCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPCommandError() and responding in RespondFTPCommandError()...")
            FiredEvents.Add("FTPCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent FTPCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of FTP pre-download
        ''' </summary>
        Public Sub RaiseFTPPreDownload(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPPreDownload() and responding in RespondFTPPreDownload()...")
            FiredEvents.Add("FTPPreDownload (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent FTPPreDownload(File)
        End Sub
        ''' <summary>
        ''' Raise an event of FTP post-download
        ''' </summary>
        Public Sub RaiseFTPPostDownload(File As String, Success As Boolean)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPPostDownload() and responding in RespondFTPPostDownload()...")
            FiredEvents.Add("FTPPostDownload (" + CStr(FiredEvents.Count) + ")", {File, Success})
            RaiseEvent FTPPostDownload(File, Success)
        End Sub
        ''' <summary>
        ''' Raise an event of FTP pre-upload
        ''' </summary>
        Public Sub RaiseFTPPreUpload(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPPreUpload() and responding in RespondFTPPreUpload()...")
            FiredEvents.Add("FTPPreUpload (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent FTPPreUpload(File)
        End Sub
        ''' <summary>
        ''' Raise an event of FTP post-upload
        ''' </summary>
        Public Sub RaiseFTPPostUpload(File As String, Success As Boolean)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FTPPostUpload() and responding in RespondFTPPostUpload()...")
            FiredEvents.Add("FTPPostUpload (" + CStr(FiredEvents.Count) + ")", {File, Success})
            RaiseEvent FTPPostUpload(File, Success)
        End Sub
        ''' <summary>
        ''' Raise an event of IMAP shell initialized
        ''' </summary>
        Public Sub RaiseIMAPShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event IMAPShellInitialized() and responding in RespondIMAPShellInitialized()...")
            FiredEvents.Add("IMAPShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent IMAPShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of IMAP pre-command execution
        ''' </summary>
        Public Sub RaiseIMAPPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event IMAPPreExecuteCommand() and responding in RespondIMAPPreExecuteCommand()...")
            FiredEvents.Add("IMAPPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent IMAPPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of IMAP post-command execution
        ''' </summary>
        Public Sub RaiseIMAPPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event IMAPPostExecuteCommand() and responding in RespondIMAPPostExecuteCommand()...")
            FiredEvents.Add("IMAPPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent IMAPPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of IMAP command error
        ''' </summary>
        Public Sub RaiseIMAPCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event IMAPCommandError() and responding in RespondIMAPCommandError()...")
            FiredEvents.Add("IMAPCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent IMAPCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of remote debugging connection accepted
        ''' </summary>
        Public Sub RaiseRemoteDebugConnectionAccepted(IP As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RemoteDebugConnectionAccepted() and responding in RespondRemoteDebugConnectionAccepted()...")
            FiredEvents.Add("RemoteDebugConnectionAccepted (" + CStr(FiredEvents.Count) + ")", {IP})
            RaiseEvent RemoteDebugConnectionAccepted(IP)
        End Sub
        ''' <summary>
        ''' Raise an event of remote debugging connection disconnected
        ''' </summary>
        Public Sub RaiseRemoteDebugConnectionDisconnected(IP As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RemoteDebugConnectionDisconnected() and responding in RespondRemoteDebugConnectionDisconnected()...")
            FiredEvents.Add("RemoteDebugConnectionDisconnected (" + CStr(FiredEvents.Count) + ")", {IP})
            RaiseEvent RemoteDebugConnectionDisconnected(IP)
        End Sub
        ''' <summary>
        ''' Raise an event of remote debugging command execution
        ''' </summary>
        Public Sub RaiseRemoteDebugExecuteCommand(IP As String, Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RemoteDebugExecuteCommand() and responding in RespondRemoteDebugExecuteCommand()...")
            FiredEvents.Add("RemoteDebugExecuteCommand (" + CStr(FiredEvents.Count) + ")", {IP, Command})
            RaiseEvent RemoteDebugExecuteCommand(IP, Command)
        End Sub
        ''' <summary>
        ''' Raise an event of remote debugging command error
        ''' </summary>
        Public Sub RaiseRemoteDebugCommandError(IP As String, Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RemoteDebugCommandError() and responding in RespondRemoteDebugCommandError()...")
            FiredEvents.Add("RemoteDebugCommandError (" + CStr(FiredEvents.Count) + ")", {IP, Command, Exception})
            RaiseEvent RemoteDebugCommandError(IP, Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of RPC command sent
        ''' </summary>
        Public Sub RaiseRPCCommandSent(Command As String, Argument As String, IP As String, Port As Integer)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RPCCommandSent() and responding in RespondRPCCommandSent()...")
            FiredEvents.Add("RPCCommandSent (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent RPCCommandSent(Command, Argument, IP, Port)
        End Sub
        ''' <summary>
        ''' Raise an event of RPC command received
        ''' </summary>
        Public Sub RaiseRPCCommandReceived(Command As String, IP As String, Port As Integer)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RPCCommandReceived() and responding in RespondRPCCommandReceived()...")
            FiredEvents.Add("RPCCommandReceived (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent RPCCommandReceived(Command, IP, Port)
        End Sub
        ''' <summary>
        ''' Raise an event of RPC command error
        ''' </summary>
        Public Sub RaiseRPCCommandError(Command As String, Exception As Exception, IP As String, Port As Integer)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RPCCommandError() and responding in RespondRPCCommandError()...")
            FiredEvents.Add("RPCCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent RPCCommandError(Command, Exception, IP, Port)
        End Sub
        ''' <summary>
        ''' Raise an event of RSS shell initialized
        ''' </summary>
        Public Sub RaiseRSSShellInitialized(FeedUrl As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RSSShellInitialized() and responding in RespondRSSShellInitialized()...")
            FiredEvents.Add("RSSShellInitialized (" + CStr(FiredEvents.Count) + ")", {FeedUrl})
            RaiseEvent RSSShellInitialized(FeedUrl)
        End Sub
        ''' <summary>
        ''' Raise an event of RSS pre-execute command
        ''' </summary>
        Public Sub RaiseRSSPreExecuteCommand(FeedUrl As String, Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RSSPreExecuteCommand() and responding in RespondRSSPreExecuteCommand()...")
            FiredEvents.Add("RSSPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {FeedUrl, Command})
            RaiseEvent RSSPreExecuteCommand(FeedUrl, Command)
        End Sub
        ''' <summary>
        ''' Raise an event of RSS post-execute command
        ''' </summary>
        Public Sub RaiseRSSPostExecuteCommand(FeedUrl As String, Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RSSPostExecuteCommand() and responding in RespondRSSPostExecuteCommand()...")
            FiredEvents.Add("RSSPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {FeedUrl, Command})
            RaiseEvent RSSPostExecuteCommand(FeedUrl, Command)
        End Sub
        ''' <summary>
        ''' Raise an event of RSS command error
        ''' </summary>
        Public Sub RaiseRSSCommandError(FeedUrl As String, Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event RSSCommandError() and responding in RespondRSSCommandError()...")
            FiredEvents.Add("RSSCommandError (" + CStr(FiredEvents.Count) + ")", {FeedUrl, Command, Exception})
            RaiseEvent RSSCommandError(FeedUrl, Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP shell initialized
        ''' </summary>
        Public Sub RaiseSFTPShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPShellInitialized() and responding in RespondSFTPShellInitialized()...")
            FiredEvents.Add("SFTPShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent SFTPShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP pre-execute command
        ''' </summary>
        Public Sub RaiseSFTPPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPPreExecuteCommand() and responding in RespondSFTPPreExecuteCommand()...")
            FiredEvents.Add("SFTPPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent SFTPPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP post-execute command
        ''' </summary>
        Public Sub RaiseSFTPPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPPostExecuteCommand() and responding in RespondSFTPPostExecuteCommand()...")
            FiredEvents.Add("SFTPPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent SFTPPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP command error
        ''' </summary>
        Public Sub RaiseSFTPCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPCommandError() and responding in RespondSFTPCommandError()...")
            FiredEvents.Add("SFTPCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent SFTPCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP pre-download
        ''' </summary>
        Public Sub RaiseSFTPPreDownload(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPPreDownload() and responding in RespondSFTPPreDownload()...")
            FiredEvents.Add("SFTPPreDownload (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent SFTPPreDownload(File)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP post-download
        ''' </summary>
        Public Sub RaiseSFTPPostDownload(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPPostDownload() and responding in RespondSFTPPostDownload()...")
            FiredEvents.Add("SFTPPostDownload (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent SFTPPostDownload(File)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP download error
        ''' </summary>
        Public Sub RaiseSFTPDownloadError(File As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPDownloadError() and responding in RespondSFTPDownloadError()...")
            FiredEvents.Add("SFTPDownloadError (" + CStr(FiredEvents.Count) + ")", {File, Exception})
            RaiseEvent SFTPDownloadError(File, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP pre-upload
        ''' </summary>
        Public Sub RaiseSFTPPreUpload(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPPreUpload() and responding in RespondSFTPPreUpload()...")
            FiredEvents.Add("SFTPPreUpload (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent SFTPPreUpload(File)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP post-upload
        ''' </summary>
        Public Sub RaiseSFTPPostUpload(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPPostUpload() and responding in RespondSFTPPostUpload()...")
            FiredEvents.Add("SFTPPostUpload (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent SFTPPostUpload(File)
        End Sub
        ''' <summary>
        ''' Raise an event of SFTP upload error
        ''' </summary>
        Public Sub RaiseSFTPUploadError(File As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SFTPUploadError() and responding in RespondSFTPUploadError()...")
            FiredEvents.Add("SFTPUploadError (" + CStr(FiredEvents.Count) + ")", {File, Exception})
            RaiseEvent SFTPUploadError(File, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of SSH being connected
        ''' </summary>
        Public Sub RaiseSSHConnected(Target As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SSHConnected() and responding in RespondSSHConnected()...")
            FiredEvents.Add("SSHConnected (" + CStr(FiredEvents.Count) + ")", {Target})
            RaiseEvent SSHConnected(Target)
        End Sub
        ''' <summary>
        ''' Raise an event of SSH being disconnected
        ''' </summary>
        Public Sub RaiseSSHDisconnected()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SSHDisconnected() and responding in RespondSSHDisconnected()...")
            FiredEvents.Add("SSHDisconnected (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent SSHDisconnected()
        End Sub
        ''' <summary>
        ''' Raise an event of SSH pre-execute command
        ''' </summary>
        Public Sub RaiseSSHPreExecuteCommand(Target As String, Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SSHPreExecuteCommand() and responding in RespondSSHPreExecuteCommand()...")
            FiredEvents.Add("SSHPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent SSHPreExecuteCommand(Target, Command)
        End Sub
        ''' <summary>
        ''' Raise an event of SSH post-execute command
        ''' </summary>
        Public Sub RaiseSSHPostExecuteCommand(Target As String, Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SSHPostExecuteCommand() and responding in RespondSSHPostExecuteCommand()...")
            FiredEvents.Add("SSHPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent SSHPostExecuteCommand(Target, Command)
        End Sub
        ''' <summary>
        ''' Raise an event of SSH command error
        ''' </summary>
        Public Sub RaiseSSHCommandError(Target As String, Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SSHCommandError() and responding in RespondSSHCommandError()...")
            FiredEvents.Add("SSHCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent SSHCommandError(Target, Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of SSH error
        ''' </summary>
        Public Sub RaiseSSHError(Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event SSHError() and responding in RespondSSHError()...")
            FiredEvents.Add("SSHError (" + CStr(FiredEvents.Count) + ")", {Exception})
            RaiseEvent SSHError(Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of UESH pre-execute
        ''' </summary>
        Public Sub RaiseUESHPreExecute(Command As String, Arguments As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event UESHPreExecute() and responding in RespondUESHPreExecute()...")
            FiredEvents.Add("UESHPreExecute (" + CStr(FiredEvents.Count) + ")", {Command, Arguments})
            RaiseEvent UESHPreExecute(Command, Arguments)
        End Sub
        ''' <summary>
        ''' Raise an event of UESH post-execute
        ''' </summary>
        Public Sub RaiseUESHPostExecute(Command As String, Arguments As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event UESHPostExecute() and responding in RespondUESHPostExecute()...")
            FiredEvents.Add("UESHPostExecute (" + CStr(FiredEvents.Count) + ")", {Command, Arguments})
            RaiseEvent UESHPostExecute(Command, Arguments)
        End Sub
        ''' <summary>
        ''' Raise an event of UESH error
        ''' </summary>
        Public Sub RaiseUESHError(Command As String, Arguments As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event UESHError() and responding in RespondUESHError()...")
            FiredEvents.Add("UESHError (" + CStr(FiredEvents.Count) + ")", {Command, Arguments, Exception})
            RaiseEvent UESHError(Command, Arguments, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of text shell initialized
        ''' </summary>
        Public Sub RaiseTextShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TextShellInitialized() and responding in RespondTextShellInitialized()...")
            FiredEvents.Add("TextShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent TextShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of text pre-command execution
        ''' </summary>
        Public Sub RaiseTextPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TextPreExecuteCommand() and responding in RespondTextPreExecuteCommand()...")
            FiredEvents.Add("TextPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent TextPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of text post-command execution
        ''' </summary>
        Public Sub RaiseTextPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TextPostExecuteCommand() and responding in RespondTextPostExecuteCommand()...")
            FiredEvents.Add("TextPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent TextPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of text command error
        ''' </summary>
        Public Sub RaiseTextCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TextCommandError() and responding in RespondTextCommandError()...")
            FiredEvents.Add("TextCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent TextCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of notification being sent
        ''' </summary>
        Public Sub RaiseNotificationSent(Notification As Notification)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event NotificationSent() and responding in RespondNotificationSent()...")
            FiredEvents.Add("NotificationSent (" + CStr(FiredEvents.Count) + ")", {Notification})
            RaiseEvent NotificationSent(Notification)
        End Sub
        ''' <summary>
        ''' Raise an event of notifications being sent
        ''' </summary>
        Public Sub RaiseNotificationsSent(Notifications As List(Of Notification))
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event NotificationsSent() and responding in RespondNotificationsSent()...")
            FiredEvents.Add("NotificationsSent (" + CStr(FiredEvents.Count) + ")", {Notifications})
            RaiseEvent NotificationsSent(Notifications)
        End Sub
        ''' <summary>
        ''' Raise an event of notification being received
        ''' </summary>
        Public Sub RaiseNotificationReceived(Notification As Notification)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event NotificationReceived() and responding in RespondNotificationReceived()...")
            FiredEvents.Add("NotificationReceived (" + CStr(FiredEvents.Count) + ")", {Notification})
            RaiseEvent NotificationReceived(Notification)
        End Sub
        ''' <summary>
        ''' Raise an event of notifications being received
        ''' </summary>
        Public Sub RaiseNotificationsReceived(Notifications As List(Of Notification))
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event NotificationsReceived() and responding in RespondNotificationsReceived()...")
            FiredEvents.Add("NotificationsReceived (" + CStr(FiredEvents.Count) + ")", {Notifications})
            RaiseEvent NotificationsReceived(Notifications)
        End Sub
        ''' <summary>
        ''' Raise an event of notification being dismissed
        ''' </summary>
        Public Sub RaiseNotificationDismissed()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event NotificationDismissed() and responding in RespondNotificationDismissed()...")
            FiredEvents.Add("NotificationDismissed (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent NotificationDismissed()
        End Sub
        ''' <summary>
        ''' Raise an event of config being saved
        ''' </summary>
        Public Sub RaiseConfigSaved()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ConfigSaved() and responding in RespondConfigSaved()...")
            FiredEvents.Add("ConfigSaved (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ConfigSaved()
        End Sub
        ''' <summary>
        ''' Raise an event of config having problems saving
        ''' </summary>
        Public Sub RaiseConfigSaveError(Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ConfigSaveError() and responding in RespondConfigSaveError()...")
            FiredEvents.Add("ConfigSaveError (" + CStr(FiredEvents.Count) + ")", {Exception})
            RaiseEvent ConfigSaveError(Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of config being read
        ''' </summary>
        Public Sub RaiseConfigRead()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ConfigRead() and responding in RespondConfigRead()...")
            FiredEvents.Add("ConfigRead (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ConfigRead()
        End Sub
        ''' <summary>
        ''' Raise an event of config having problems reading
        ''' </summary>
        Public Sub RaiseConfigReadError(Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ConfigReadError() and responding in RespondConfigReadError()...")
            FiredEvents.Add("ConfigReadError (" + CStr(FiredEvents.Count) + ")", {Exception})
            RaiseEvent ConfigReadError(Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of mod command pre-execution
        ''' </summary>
        Public Sub RaisePreExecuteModCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PreExecuteModCommand() and responding in RespondPreExecuteModCommand()...")
            FiredEvents.Add("PreExecuteModCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent PreExecuteModCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of mod command post-execution
        ''' </summary>
        Public Sub RaisePostExecuteModCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event PostExecuteModCommand() and responding in RespondPostExecuteModCommand()...")
            FiredEvents.Add("PostExecuteModCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent PostExecuteModCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of mod being parsed
        ''' </summary>
        Public Sub RaiseModParsed(ModFileName As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ModParsed() and responding in RespondModParsed()...")
            FiredEvents.Add("ModParsed (" + CStr(FiredEvents.Count) + ")", {ModFileName})
            RaiseEvent ModParsed(ModFileName)
        End Sub
        ''' <summary>
        ''' Raise an event of mod having problems parsing
        ''' </summary>
        Public Sub RaiseModParseError(ModFileName As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ModParseError() and responding in RespondModParseError()...")
            FiredEvents.Add("ModParseError (" + CStr(FiredEvents.Count) + ")", {ModFileName})
            RaiseEvent ModParseError(ModFileName)
        End Sub
        ''' <summary>
        ''' Raise an event of mod being finalized
        ''' </summary>
        Public Sub RaiseModFinalized(ModFileName As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ModFinalized() and responding in RespondModFinalized()...")
            FiredEvents.Add("ModFinalized (" + CStr(FiredEvents.Count) + ")", {ModFileName})
            RaiseEvent ModFinalized(ModFileName)
        End Sub
        ''' <summary>
        ''' Raise an event of mod having problems finalizing
        ''' </summary>
        Public Sub RaiseModFinalizationFailed(ModFileName As String, Reason As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ModFinalizationFailed() and responding in RespondModFinalizationFailed()...")
            FiredEvents.Add("ModFinalizationFailed (" + CStr(FiredEvents.Count) + ")", {ModFileName, Reason})
            RaiseEvent ModFinalizationFailed(ModFileName, Reason)
        End Sub
        ''' <summary>
        ''' Raise an event of user being added
        ''' </summary>
        Public Sub RaiseUserAdded(Username As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event UserAdded() and responding in RespondUserAdded()...")
            FiredEvents.Add("UserAdded (" + CStr(FiredEvents.Count) + ")", {Username})
            RaiseEvent UserAdded(Username)
        End Sub
        ''' <summary>
        ''' Raise an event of user being removed
        ''' </summary>
        Public Sub RaiseUserRemoved(Username As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event UserRemoved() and responding in RespondUserRemoved()...")
            FiredEvents.Add("UserRemoved (" + CStr(FiredEvents.Count) + ")", {Username})
            RaiseEvent UserRemoved(Username)
        End Sub
        ''' <summary>
        ''' Raise an event of username being changed
        ''' </summary>
        Public Sub RaiseUsernameChanged(OldUsername As String, NewUsername As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event UsernameChanged() and responding in RespondUsernameChanged()...")
            FiredEvents.Add("UsernameChanged (" + CStr(FiredEvents.Count) + ")", {OldUsername, NewUsername})
            RaiseEvent UsernameChanged(OldUsername, NewUsername)
        End Sub
        ''' <summary>
        ''' Raise an event of user password being changed
        ''' </summary>
        Public Sub RaiseUserPasswordChanged(Username As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event UserPasswordChanged() and responding in RespondUserPasswordChanged()...")
            FiredEvents.Add("UserPasswordChanged (" + CStr(FiredEvents.Count) + ")", {Username})
            RaiseEvent UserPasswordChanged(Username)
        End Sub
        ''' <summary>
        ''' Raise an event of hardware probing
        ''' </summary>
        Public Sub RaiseHardwareProbing()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HardwareProbing() and responding in RespondHardwareProbing()...")
            FiredEvents.Add("HardwareProbing (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent HardwareProbing()
        End Sub
        ''' <summary>
        ''' Raise an event of hardware being probed
        ''' </summary>
        Public Sub RaiseHardwareProbed()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HardwareProbed() and responding in RespondHardwareProbed()...")
            FiredEvents.Add("HardwareProbed (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent HardwareProbed()
        End Sub
        ''' <summary>
        ''' Raise an event of current directory being changed
        ''' </summary>
        Public Sub RaiseCurrentDirectoryChanged()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event CurrentDirectoryChanged() and responding in RespondCurrentDirectoryChanged()...")
            FiredEvents.Add("CurrentDirectoryChanged (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent CurrentDirectoryChanged()
        End Sub
        ''' <summary>
        ''' Raise an event of file creation
        ''' </summary>
        Public Sub RaiseFileCreated(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FileCreated() and responding in RespondFileCreated()...")
            FiredEvents.Add("FileCreated (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent FileCreated(File)
        End Sub
        ''' <summary>
        ''' Raise an event of directory creation
        ''' </summary>
        Public Sub RaiseDirectoryCreated(Directory As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event DirectoryCreated() and responding in RespondDirectoryCreated()...")
            FiredEvents.Add("DirectoryCreated (" + CStr(FiredEvents.Count) + ")", {Directory})
            RaiseEvent DirectoryCreated(Directory)
        End Sub
        ''' <summary>
        ''' Raise an event of file copying process
        ''' </summary>
        Public Sub RaiseFileCopied(Source As String, Destination As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FileCopied() and responding in RespondFileCopied()...")
            FiredEvents.Add("FileCopied (" + CStr(FiredEvents.Count) + ")", {Source, Destination})
            RaiseEvent FileCopied(Source, Destination)
        End Sub
        ''' <summary>
        ''' Raise an event of directory copying process
        ''' </summary>
        Public Sub RaiseDirectoryCopied(Source As String, Destination As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event DirectoryCopied() and responding in RespondDirectoryCopied()...")
            FiredEvents.Add("DirectoryCopied (" + CStr(FiredEvents.Count) + ")", {Source, Destination})
            RaiseEvent DirectoryCopied(Source, Destination)
        End Sub
        ''' <summary>
        ''' Raise an event of file moving process
        ''' </summary>
        Public Sub RaiseFileMoved(Source As String, Destination As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FileMoved() and responding in RespondFileMoved()...")
            FiredEvents.Add("FileMoved (" + CStr(FiredEvents.Count) + ")", {Source, Destination})
            RaiseEvent FileMoved(Source, Destination)
        End Sub
        ''' <summary>
        ''' Raise an event of directory moving process
        ''' </summary>
        Public Sub RaiseDirectoryMoved(Source As String, Destination As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event DirectoryMoved() and responding in RespondDirectoryMoved()...")
            FiredEvents.Add("DirectoryMoved (" + CStr(FiredEvents.Count) + ")", {Source, Destination})
            RaiseEvent DirectoryMoved(Source, Destination)
        End Sub
        ''' <summary>
        ''' Raise an event of file removal
        ''' </summary>
        Public Sub RaiseFileRemoved(File As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FileRemoved() and responding in RespondFileRemoved()...")
            FiredEvents.Add("FileRemoved (" + CStr(FiredEvents.Count) + ")", {File})
            RaiseEvent FileRemoved(File)
        End Sub
        ''' <summary>
        ''' Raise an event of directory removal
        ''' </summary>
        Public Sub RaiseDirectoryRemoved(Directory As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event DirectoryRemoved() and responding in RespondDirectoryRemoved()...")
            FiredEvents.Add("DirectoryRemoved (" + CStr(FiredEvents.Count) + ")", {Directory})
            RaiseEvent DirectoryRemoved(Directory)
        End Sub
        ''' <summary>
        ''' Raise an event of file attribute addition
        ''' </summary>
        Public Sub RaiseFileAttributeAdded(File As String, Attributes As FileAttributes)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FileAttributeAdded() and responding in RespondFileAttributeAdded()...")
            FiredEvents.Add("FileAttributeAdded (" + CStr(FiredEvents.Count) + ")", {File, Attributes})
            RaiseEvent FileAttributeAdded(File, Attributes)
        End Sub
        ''' <summary>
        ''' Raise an event of file attribute removal
        ''' </summary>
        Public Sub RaiseFileAttributeRemoved(File As String, Attributes As FileAttributes)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event FileAttributeRemoved() and responding in RespondFileAttributeRemoved()...")
            FiredEvents.Add("FileAttributeRemoved (" + CStr(FiredEvents.Count) + ")", {File, Attributes})
            RaiseEvent FileAttributeRemoved(File, Attributes)
        End Sub
        ''' <summary>
        ''' Raise an event of console colors being reset
        ''' </summary>
        Public Sub RaiseColorReset()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ColorReset() and responding in RespondColorReset()...")
            FiredEvents.Add("ColorReset (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ColorReset()
        End Sub
        ''' <summary>
        ''' Raise an event of theme setting
        ''' </summary>
        Public Sub RaiseThemeSet(Theme As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ThemeSet() and responding in RespondThemeSet()...")
            FiredEvents.Add("ThemeSet (" + CStr(FiredEvents.Count) + ")", {Theme})
            RaiseEvent ThemeSet(Theme)
        End Sub
        ''' <summary>
        ''' Raise an event of theme setting problem
        ''' </summary>
        Public Sub RaiseThemeSetError(Theme As String, Reason As ThemeSetErrorReasons)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ThemeSetError() and responding in RespondThemeSetError()...")
            FiredEvents.Add("ThemeSetError (" + CStr(FiredEvents.Count) + ")", {Theme, Reason})
            RaiseEvent ThemeSetError(Theme, Reason)
        End Sub
        ''' <summary>
        ''' Raise an event of console colors being set
        ''' </summary>
        Public Sub RaiseColorSet()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ColorSet() and responding in RespondColorSet()...")
            FiredEvents.Add("ColorSet (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ColorSet()
        End Sub
        ''' <summary>
        ''' Raise an event of console colors having problems being set
        ''' </summary>
        Public Sub RaiseColorSetError(Reason As ColorSetErrorReasons)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ColorSetError() and responding in RespondColorSetError()...")
            FiredEvents.Add("ColorSetError (" + CStr(FiredEvents.Count) + ")", {Reason})
            RaiseEvent ColorSetError(Reason)
        End Sub
        ''' <summary>
        ''' Raise an event of theme studio start
        ''' </summary>
        Public Sub RaiseThemeStudioStarted()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ThemeStudioStarted() and responding in RespondThemeStudioStarted()...")
            FiredEvents.Add("ThemeStudioStarted (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ThemeStudioStarted()
        End Sub
        ''' <summary>
        ''' Raise an event of theme studio exit
        ''' </summary>
        Public Sub RaiseThemeStudioExit()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ThemeStudioExit() and responding in RespondThemeStudioExit()...")
            FiredEvents.Add("ThemeStudioExit (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ThemeStudioExit()
        End Sub
        ''' <summary>
        ''' Raise an event of arguments being injected
        ''' </summary>
        Public Sub RaiseArgumentsInjected(InjectedArguments As List(Of String))
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ArgumentsInjected() and responding in RespondArgumentsInjected()...")
            FiredEvents.Add("ArgumentsInjected (" + CStr(FiredEvents.Count) + ")", {InjectedArguments})
            RaiseEvent ArgumentsInjected(InjectedArguments)
        End Sub
        ''' <summary>
        ''' Raise an event of ZIP shell initialized
        ''' </summary>
        Public Sub RaiseZipShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ZipShellInitialized() and responding in RespondZipShellInitialized()...")
            FiredEvents.Add("ZipShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent ZipShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of ZIP pre-command execution
        ''' </summary>
        Public Sub RaiseZipPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ZipPreExecuteCommand() and responding in RespondZipPreExecuteCommand()...")
            FiredEvents.Add("ZipPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent ZipPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of ZIP post-command execution
        ''' </summary>
        Public Sub RaiseZipPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ZipPostExecuteCommand() and responding in RespondZipPostExecuteCommand()...")
            FiredEvents.Add("ZipPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent ZipPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of ZIP command error
        ''' </summary>
        Public Sub RaiseZipCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ZipCommandError() and responding in RespondZipCommandError()...")
            FiredEvents.Add("ZipCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent ZipCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of HTTP shell initialized
        ''' </summary>
        Public Sub RaiseHTTPShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HTTPShellInitialized() and responding in RespondHTTPShellInitialized()...")
            FiredEvents.Add("HTTPShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent HTTPShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of HTTP pre-execute command
        ''' </summary>
        Public Sub RaiseHTTPPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HTTPPreExecuteCommand() and responding in RespondHTTPPreExecuteCommand()...")
            FiredEvents.Add("HTTPPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent HTTPPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of HTTP post-execute command
        ''' </summary>
        Public Sub RaiseHTTPPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HTTPPostExecuteCommand() and responding in RespondHTTPPostExecuteCommand()...")
            FiredEvents.Add("HTTPPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent HTTPPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of HTTP command error
        ''' </summary>
        Public Sub RaiseHTTPCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HTTPCommandError() and responding in RespondHTTPCommandError()...")
            FiredEvents.Add("HTTPCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent HTTPCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of process error
        ''' </summary>
        Public Sub RaiseProcessError(Process As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event ProcessError() and responding in RespondProcessError()...")
            FiredEvents.Add("ProcessError (" + CStr(FiredEvents.Count) + ")", {Process, Exception})
            RaiseEvent ProcessError(Process, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of a custom language installation
        ''' </summary>
        Public Sub RaiseLanguageInstalled(Language As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguageInstalled() and responding in RespondLanguageInstalled()...")
            FiredEvents.Add("LanguageInstalled (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguageInstalled(Language)
        End Sub
        ''' <summary>
        ''' Raise an event of a custom language uninstallation
        ''' </summary>
        Public Sub RaiseLanguageUninstalled(Language As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguageUninstalled() and responding in RespondLanguageUninstalled()...")
            FiredEvents.Add("LanguageUninstalled (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguageUninstalled(Language)
        End Sub
        ''' <summary>
        ''' Raise an event of custom language install error
        ''' </summary>
        Public Sub RaiseLanguageInstallError(Language As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguageInstallError() and responding in RespondLanguageInstallError()...")
            FiredEvents.Add("LanguageInstallError (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguageInstallError(Language, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of custom language uninstall error
        ''' </summary>
        Public Sub RaiseLanguageUninstallError(Language As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguageUninstallError() and responding in RespondLanguageUninstallError()...")
            FiredEvents.Add("LanguageUninstallError (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguageUninstallError(Language, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of custom languages installation
        ''' </summary>
        Public Sub RaiseLanguagesInstalled()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguagesInstalled() and responding in RespondLanguagesInstalled()...")
            FiredEvents.Add("LanguagesInstalled (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguagesInstalled()
        End Sub
        ''' <summary>
        ''' Raise an event of custom languages uninstallation
        ''' </summary>
        Public Sub RaiseLanguagesUninstalled()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguagesUninstalled() and responding in RespondLanguagesUninstalled()...")
            FiredEvents.Add("LanguagesUninstalled (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguagesUninstalled()
        End Sub
        ''' <summary>
        ''' Raise an event of custom languages install error
        ''' </summary>
        Public Sub RaiseLanguagesInstallError(Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguagesInstallError() and responding in RespondLanguagesInstallError()...")
            FiredEvents.Add("LanguagesInstallError (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguagesInstallError(Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of custom languages uninstall error
        ''' </summary>
        Public Sub RaiseLanguagesUninstallError(Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event LanguagesUninstallError() and responding in RespondLanguagesUninstallError()...")
            FiredEvents.Add("LanguagesUninstallError (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent LanguagesUninstallError(Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of Hex shell initialized
        ''' </summary>
        Public Sub RaiseHexShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HexShellInitialized() and responding in RespondHexShellInitialized()...")
            FiredEvents.Add("HexShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent HexShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of Hex pre-command execution
        ''' </summary>
        Public Sub RaiseHexPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HexPreExecuteCommand() and responding in RespondHexPreExecuteCommand()...")
            FiredEvents.Add("HexPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent HexPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of Hex post-command execution
        ''' </summary>
        Public Sub RaiseHexPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HexPostExecuteCommand() and responding in RespondHexPostExecuteCommand()...")
            FiredEvents.Add("HexPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent HexPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of Hex command error
        ''' </summary>
        Public Sub RaiseHexCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event HexCommandError() and responding in RespondHexCommandError()...")
            FiredEvents.Add("HexCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent HexCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of Json shell initialized
        ''' </summary>
        Public Sub RaiseJsonShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event JsonShellInitialized() and responding in RespondJsonShellInitialized()...")
            FiredEvents.Add("JsonShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent JsonShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of Json pre-command execution
        ''' </summary>
        Public Sub RaiseJsonPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event JsonPreExecuteCommand() and responding in RespondJsonPreExecuteCommand()...")
            FiredEvents.Add("JsonPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent JsonPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of Json post-command execution
        ''' </summary>
        Public Sub RaiseJsonPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event JsonPostExecuteCommand() and responding in RespondJsonPostExecuteCommand()...")
            FiredEvents.Add("JsonPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent JsonPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of Json command error
        ''' </summary>
        Public Sub RaiseJsonCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event JsonCommandError() and responding in RespondJsonCommandError()...")
            FiredEvents.Add("JsonCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent JsonCommandError(Command, Exception)
        End Sub
        ''' <summary>
        ''' Raise an event of Test shell initialized
        ''' </summary>
        Public Sub RaiseTestShellInitialized()
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TestShellInitialized() and responding in RespondTestShellInitialized()...")
            FiredEvents.Add("TestShellInitialized (" + CStr(FiredEvents.Count) + ")", Array.Empty(Of Object))
            RaiseEvent TestShellInitialized()
        End Sub
        ''' <summary>
        ''' Raise an event of Test pre-command execution
        ''' </summary>
        Public Sub RaiseTestPreExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TestPreExecuteCommand() and responding in RespondTestPreExecuteCommand()...")
            FiredEvents.Add("TestPreExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent TestPreExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of Test post-command execution
        ''' </summary>
        Public Sub RaiseTestPostExecuteCommand(Command As String)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TestPostExecuteCommand() and responding in RespondTestPostExecuteCommand()...")
            FiredEvents.Add("TestPostExecuteCommand (" + CStr(FiredEvents.Count) + ")", {Command})
            RaiseEvent TestPostExecuteCommand(Command)
        End Sub
        ''' <summary>
        ''' Raise an event of Test command error
        ''' </summary>
        Public Sub RaiseTestCommandError(Command As String, Exception As Exception)
            WdbgConditional(EventDebug, DebugLevel.I, "Raising event TestCommandError() and responding in RespondTestCommandError()...")
            FiredEvents.Add("TestCommandError (" + CStr(FiredEvents.Count) + ")", {Command, Exception})
            RaiseEvent TestCommandError(Command, Exception)
        End Sub

    End Class
End Namespace