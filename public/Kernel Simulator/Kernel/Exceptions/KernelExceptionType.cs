
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace KS.Kernel.Exceptions
{
    /// <summary>
    /// Kernel exception types
    /// </summary>
    public enum KernelExceptionType
    {
        /// <summary>
        /// Unknown kernel error.
        /// </summary>
        Unknown,
        /// <summary>
        /// The specified alias already exists. Make sure that you select a different alias.
        /// </summary>
        AliasAlreadyExists,
        /// <summary>
        /// The alias system encountered an invalid operation trying to process your request.
        /// </summary>
        AliasInvalidOperation,
        /// <summary>
        /// The specified alias isn't found.
        /// </summary>
        AliasNoSuchAlias,
        /// <summary>
        /// The specified command isn't found to alias to.
        /// </summary>
        AliasNoSuchCommand,
        /// <summary>
        /// The specified alias type isn't found.
        /// </summary>
        AliasNoSuchType,
        /// <summary>
        /// An error occurred while trying to process the color request. Common mistakes include specifying the wrong color specifier.
        /// </summary>
        Color,
        /// <summary>
        /// A kernel configuration error occurred.
        /// </summary>
        Config,
        /// <summary>
        /// User didn't provide any input in a timely fashion.
        /// </summary>
        ConsoleReadTimeout,
        /// <summary>
        /// A general filesystem error occurred. Check to make sure that the path specified is correct, and that the file or directory actually exists.
        /// </summary>
        Filesystem,
        /// <summary>
        /// A general FTP filesystem error occurred. Check to make sure that the path specified is correct, and that the file or directory actually exists.
        /// </summary>
        FTPFilesystem,
        /// <summary>
        /// A general FTP network error occurred. Check to make sure that your internet connection is working.
        /// </summary>
        FTPNetwork,
        /// <summary>
        /// A general FTP shell error occurred.
        /// </summary>
        FTPShell,
        /// <summary>
        /// A group management error occurred.
        /// </summary>
        GroupManagement,
        /// <summary>
        /// Hostname error.
        /// </summary>
        Hostname,
        /// <summary>
        /// A general HTTP shell error occurred.
        /// </summary>
        HTTPShell,
        /// <summary>
        /// Insane console has been detected. Kernel Simulator can't continue running, because your console is incompatible with our requirements.
        /// </summary>
        InsaneConsoleDetected,
        /// <summary>
        /// Invalid RSS feed.
        /// </summary>
        InvalidFeed,
        /// <summary>
        /// Invalid RSS feed link.
        /// </summary>
        InvalidFeedLink,
        /// <summary>
        /// Invalid RSS feed type.
        /// </summary>
        InvalidFeedType,
        /// <summary>
        /// Invalid hash algorithm. If you're using a custom hash sum driver, check to make sure that the driver is loaded.
        /// </summary>
        InvalidHashAlgorithm,
        /// <summary>
        /// Invalid hash sum. Check to make sure that you copied and pasted the hash sum correctly. If you're using a custom hash sum driver, check to make sure that the driver is loaded and that it works as expected.
        /// </summary>
        InvalidHash,
        /// <summary>
        /// Invalid kernel path.
        /// </summary>
        InvalidKernelPath,
        /// <summary>
        /// Invalid manual page. Check to make sure that the mod containing the valid manual pages is loaded.
        /// </summary>
        InvalidManpage,
        /// <summary>
        /// Invalid mod. If the mod is a .NET Framework mod, make sure that you're using the .NET Framework version of Kernel Simulator. The same applies to .NET CoreCLR version. If your mod is a valid .NET assembly, check to make sure that it actually implements the interface that is necessary to start the mod.
        /// </summary>
        InvalidMod,
        /// <summary>
        /// Invalid filesystem path. Check to make sure that it's written correctly.
        /// </summary>
        InvalidPath,
        /// <summary>
        /// Invalid placeholder. Consult the kernel documentation for more information.
        /// </summary>
        InvalidPlaceholder,
        /// <summary>
        /// The custom language failed to install. Check to make sure that the language file is correctly formed. Usually, the locale generator does a good job in making the localization information for your custom language.
        /// </summary>
        LanguageInstall,
        /// <summary>
        /// The custom language failed to load. Check to make sure that the language file is correctly formed. Usually, the locale generator does a good job in making the localization information for your custom language.
        /// </summary>
        LanguageParse,
        /// <summary>
        /// The custom language failed to uninstall.
        /// </summary>
        LanguageUninstall,
        /// <summary>
        /// The mail system encountered an error.
        /// </summary>
        Mail,
        /// <summary>
        /// The mod failed to install. Check to make sure that it's a valid mod file. If the mod is a .NET Framework mod, make sure that you're using the .NET Framework version of Kernel Simulator. The same applies to .NET CoreCLR version. If your mod is a valid .NET assembly, check to make sure that it actually implements the interface that is necessary to start the mod.
        /// </summary>
        ModInstall,
        /// <summary>
        /// The mod contains no parts. Consult with the mod vendor for a new copy.
        /// </summary>
        ModNoParts,
        /// <summary>
        /// The mod uninstallation has failed.
        /// </summary>
        ModUninstall,
        /// <summary>
        /// An error occurred trying to read or set the message of the day.
        /// </summary>
        MOTD,
        /// <summary>
        /// There is no event by this name.
        /// </summary>
        NoSuchEvent,
        /// <summary>
        /// There is no language by this name.
        /// </summary>
        NoSuchLanguage,
        /// <summary>
        /// There is no mail directory by this name.
        /// </summary>
        NoSuchMailDirectory,
        /// <summary>
        /// There is no mod by this name.
        /// </summary>
        NoSuchMod,
        /// <summary>
        /// There is no reflection property by this name.
        /// </summary>
        NoSuchReflectionProperty,
        /// <summary>
        /// There is no reflection variable by this name.
        /// </summary>
        NoSuchReflectionVariable,
        /// <summary>
        /// There is no screensaver by this name.
        /// </summary>
        NoSuchScreensaver,
        /// <summary>
        /// There is no shell preset by this name.
        /// </summary>
        NoSuchShellPreset,
        /// <summary>
        /// There is no theme by this name.
        /// </summary>
        NoSuchTheme,
        /// <summary>
        /// There are zero users in the entire kernel. We've reached an extremely rare situation. The kernel can't continue.
        /// </summary>
        NullUsers,
        /// <summary>
        /// An error occurred trying to manage permissions
        /// </summary>
        PermissionManagement,
        /// <summary>
        /// The remote debug device already exists.
        /// </summary>
        RemoteDebugDeviceAlreadyExists,
        /// <summary>
        /// The remote debug device is not found. Check to make sure that you've written an IP address of the device right.
        /// </summary>
        RemoteDebugDeviceNotFound,
        /// <summary>
        /// The remote debug device operation failed.
        /// </summary>
        RemoteDebugDeviceOperation,
        /// <summary>
        /// The RSS network error occurred. Check to make sure that your internet connection is working.
        /// </summary>
        RSSNetwork,
        /// <summary>
        /// The RSS shell error occurred.
        /// </summary>
        RSSShell,
        /// <summary>
        /// The screensaver management error occurred during the operation.
        /// </summary>
        ScreensaverManagement,
        /// <summary>
        /// A general SFTP filesystem error occurred. Check to make sure that the path specified is correct, and that the file or directory actually exists.
        /// </summary>
        SFTPFilesystem,
        /// <summary>
        /// A general SFTP network error occurred. Check to make sure that your internet connection is working.
        /// </summary>
        SFTPNetwork,
        /// <summary>
        /// A general SFTP shell error occurred.
        /// </summary>
        SFTPShell,
        /// <summary>
        /// Condition parsing failed. Please check your syntax to meet the operator's expected design.
        /// </summary>
        UESHConditionParse,
        /// <summary>
        /// An error occurred while trying to parse the UESH script file. Check your script file for errors and correct them.
        /// </summary>
        UESHScript,
        /// <summary>
        /// A user creation error occurred.
        /// </summary>
        UserCreation,
        /// <summary>
        /// A user management error occurred.
        /// </summary>
        UserManagement,
        /// <summary>
        /// A general network error occurred. Should you be trying to connect to a remote end, make sure that you're connected to the Internet or to your local network.
        /// </summary>
        Network,
        /// <summary>
        /// An unsupported console has been detected.
        /// </summary>
        UnsupportedConsole,
        /// <summary>
        /// An assertion failure has been detected in the kernel! This is most likely a bug in the kernel module that should be fixed on our end.
        /// </summary>
        AssertionFailure,
        /// <summary>
        /// Your network needs to be online before being able to perform operations related to networking. Connect your network adapter and try again.
        /// </summary>
        NetworkOffline
    }
}
