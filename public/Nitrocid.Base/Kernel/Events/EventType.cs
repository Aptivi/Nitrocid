//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

namespace Nitrocid.Base.Kernel.Events
{
    /// <summary>
    /// Kernel event type
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// When the kernel starts
        /// </summary>
        StartKernel,
        /// <summary>
        /// When the kernel is started
        /// </summary>
        KernelStarted,
        /// <summary>
        /// Fired before login
        /// </summary>
        PreLogin,
        /// <summary>
        /// Fired after login
        /// </summary>
        PostLogin,
        /// <summary>
        /// Fired on login error
        /// </summary>
        LoginError,
        /// <summary>
        /// Fired on kernel error
        /// </summary>
        KernelError,
        /// <summary>
        /// Fired on continuable kernel error
        /// </summary>
        ContKernelError,
        /// <summary>
        /// Fired before shutdown
        /// </summary>
        PreShutdown,
        /// <summary>
        /// Fired after shutdown
        /// </summary>
        PostShutdown,
        /// <summary>
        /// Fired before reboot
        /// </summary>
        PreReboot,
        /// <summary>
        /// Fired after reboot
        /// </summary>
        PostReboot,
        /// <summary>
        /// Fired on screensaver start
        /// </summary>
        PreShowScreensaver,
        /// <summary>
        /// Fired on screensaver end
        /// </summary>
        PostShowScreensaver,
        /// <summary>
        /// Fired before unlocking
        /// </summary>
        PreUnlock,
        /// <summary>
        /// Fired after unlocking
        /// </summary>
        PostUnlock,
        /// <summary>
        /// Fired on command error
        /// </summary>
        CommandError,
        /// <summary>
        /// Fired before config reload
        /// </summary>
        PreReloadConfig,
        /// <summary>
        /// Fired after config reload
        /// </summary>
        PostReloadConfig,
        /// <summary>
        /// Fired while the placeholders are being parsed
        /// </summary>
        PlaceholderParsing,
        /// <summary>
        /// Fired after parsing placeholders
        /// </summary>
        PlaceholderParsed,
        /// <summary>
        /// Fired when parsing placeholders failed
        /// </summary>
        PlaceholderParseError,
        /// <summary>
        /// Fired after garbage collection
        /// </summary>
        GarbageCollected,
        /// <summary>
        /// Fired before an FTP file is transferred locally (download)
        /// </summary>
        FTPPreDownload,
        /// <summary>
        /// Fired after an FTP file is transferred locally (download)
        /// </summary>
        FTPPostDownload,
        /// <summary>
        /// Fired before an FTP file is transferred remotely (upload)
        /// </summary>
        FTPPreUpload,
        /// <summary>
        /// Fired after an FTP file is transferred remotely (upload)
        /// </summary>
        FTPPostUpload,
        /// <summary>
        /// A connection is accepted
        /// </summary>
        RemoteDebugConnectionAccepted,
        /// <summary>
        /// A connection is disconnected
        /// </summary>
        RemoteDebugConnectionDisconnected,
        /// <summary>
        /// Fired on remote debug command execution
        /// </summary>
        RemoteDebugExecuteCommand,
        /// <summary>
        /// Fired on remote debug command error
        /// </summary>
        RemoteDebugCommandError,
        /// <summary>
        /// RPC command is sent
        /// </summary>
        RPCCommandSent,
        /// <summary>
        /// RPC command is received
        /// </summary>
        RPCCommandReceived,
        /// <summary>
        /// Fired on RPC command error
        /// </summary>
        RPCCommandError,
        /// <summary>
        /// Fired before an SFTP file is downloaded
        /// </summary>
        SFTPPreDownload,
        /// <summary>
        /// Fired after an SFTP file is downloaded
        /// </summary>
        SFTPPostDownload,
        /// <summary>
        /// Fired on SFTP download error
        /// </summary>
        SFTPDownloadError,
        /// <summary>
        /// Fired before an SFTP file is uploaded
        /// </summary>
        SFTPPreUpload,
        /// <summary>
        /// Fired after an SFTP file is uploaded
        /// </summary>
        SFTPPostUpload,
        /// <summary>
        /// Fired on SFTP upload error
        /// </summary>
        SFTPUploadError,
        /// <summary>
        /// Connected to SSH
        /// </summary>
        SSHConnected,
        /// <summary>
        /// Disconnected from SSH
        /// </summary>
        SSHDisconnected,
        /// <summary>
        /// Fired before SSH command execution
        /// </summary>
        SSHPreExecuteCommand,
        /// <summary>
        /// Fired after SSH command execution
        /// </summary>
        SSHPostExecuteCommand,
        /// <summary>
        /// Fired on SSH command error
        /// </summary>
        SSHCommandError,
        /// <summary>
        /// Fired on SSH error
        /// </summary>
        SSHError,
        /// <summary>
        /// Fired on notification being sent
        /// </summary>
        NotificationSent,
        /// <summary>
        /// Fired on notifications being sent
        /// </summary>
        NotificationsSent,
        /// <summary>
        /// Fired on notification being received
        /// </summary>
        NotificationReceived,
        /// <summary>
        /// Fired on notifications being received
        /// </summary>
        NotificationsReceived,
        /// <summary>
        /// Fired on notification being dismissed
        /// </summary>
        NotificationDismissed,
        /// <summary>
        /// Fired when config is saved
        /// </summary>
        ConfigSaved,
        /// <summary>
        /// Fired on config save error
        /// </summary>
        ConfigSaveError,
        /// <summary>
        /// Fired when config is read
        /// </summary>
        ConfigRead,
        /// <summary>
        /// Fired on config read error
        /// </summary>
        ConfigReadError,
        /// <summary>
        /// Fired before mod command execution
        /// </summary>
        PreExecuteModCommand,
        /// <summary>
        /// Fired after mod command execution
        /// </summary>
        PostExecuteModCommand,
        /// <summary>
        /// Fired when the mod is parsed
        /// </summary>
        ModParsed,
        /// <summary>
        /// Fired when the mod failed to parse
        /// </summary>
        ModParseError,
        /// <summary>
        /// Fired when the mod is finalized
        /// </summary>
        ModFinalized,
        /// <summary>
        /// Fired when the mod failed to finalize
        /// </summary>
        ModFinalizationFailed,
        /// <summary>
        /// Fired when a new user arrived
        /// </summary>
        UserAdded,
        /// <summary>
        /// Fired when a user is removed
        /// </summary>
        UserRemoved,
        /// <summary>
        /// Fired when a user changed their name
        /// </summary>
        UsernameChanged,
        /// <summary>
        /// Fired when a user changed their password
        /// </summary>
        UserPasswordChanged,
        /// <summary>
        /// Fired when the hardware is probing
        /// </summary>
        HardwareProbing,
        /// <summary>
        /// Fired when the hardware is probed
        /// </summary>
        HardwareProbed,
        /// <summary>
        /// Fired when the current diectory is changed
        /// </summary>
        CurrentDirectoryChanged,
        /// <summary>
        /// Fired when a new file is created
        /// </summary>
        FileCreated,
        /// <summary>
        /// Fired when a new folder is created
        /// </summary>
        DirectoryCreated,
        /// <summary>
        /// Fired when a new file is copied
        /// </summary>
        FileCopied,
        /// <summary>
        /// Fired when a new folder is copied
        /// </summary>
        DirectoryCopied,
        /// <summary>
        /// Fired when a new file is moved
        /// </summary>
        FileMoved,
        /// <summary>
        /// Fired when a new folder is moved
        /// </summary>
        DirectoryMoved,
        /// <summary>
        /// Fired when a new file is removed
        /// </summary>
        FileRemoved,
        /// <summary>
        /// Fired when a new folder is removed
        /// </summary>
        DirectoryRemoved,
        /// <summary>
        /// Fired when a file attribute is added
        /// </summary>
        FileAttributeAdded,
        /// <summary>
        /// Fired when a file attribute is removed
        /// </summary>
        FileAttributeRemoved,
        /// <summary>
        /// Fired when the colors are reset
        /// </summary>
        ColorReset,
        /// <summary>
        /// Fired when a theme is set
        /// </summary>
        ThemeSet,
        /// <summary>
        /// Fired when a theme is not set due to an error
        /// </summary>
        ThemeSetError,
        /// <summary>
        /// Fired when a color is set
        /// </summary>
        ColorSet,
        /// <summary>
        /// Fired when a color is not set due to an error
        /// </summary>
        ColorSetError,
        /// <summary>
        /// Fired when the theme studio is started
        /// </summary>
        ThemeStudioStarted,
        /// <summary>
        /// Fired when the theme studio is exited
        /// </summary>
        ThemeStudioExit,
        /// <summary>
        /// Fired when the arguments are injected
        /// </summary>
        ArgumentsInjected,
        /// <summary>
        /// Fired on process error
        /// </summary>
        ProcessError,
        /// <summary>
        /// Fired on language installed
        /// </summary>
        LanguageInstalled,
        /// <summary>
        /// Fired on language uninstalled
        /// </summary>
        LanguageUninstalled,
        /// <summary>
        /// Fired on language install error
        /// </summary>
        LanguageInstallError,
        /// <summary>
        /// Fired on language uninstall error
        /// </summary>
        LanguageUninstallError,
        /// <summary>
        /// Fired on languages installed
        /// </summary>
        LanguagesInstalled,
        /// <summary>
        /// Fired on languages uninstalled
        /// </summary>
        LanguagesUninstalled,
        /// <summary>
        /// Fired on languages install error
        /// </summary>
        LanguagesInstallError,
        /// <summary>
        /// Fired on languages uninstall error
        /// </summary>
        LanguagesUninstallError,
        /// <summary>
        /// Fired on console resize
        /// </summary>
        ResizeDetected
    }
}
