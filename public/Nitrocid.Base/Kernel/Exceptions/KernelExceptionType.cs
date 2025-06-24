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

namespace Nitrocid.Base.Kernel.Exceptions
{
    /// <summary>
    /// Kernel exception types
    /// </summary>
    public enum KernelExceptionType
    {
        /// <summary>
        /// There was an unknown kernel error, possibly due to either the kernel exception type not being specified or something was wrong.
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
        /// The kernel networking has reported an error for the hostname component.
        /// </summary>
        Hostname,
        /// <summary>
        /// A general HTTP shell error occurred.
        /// </summary>
        HTTPShell,
        /// <summary>
        /// Invalid RSS feed. Ensure that you've referenced the feed correctly and try again.
        /// </summary>
        InvalidFeed,
        /// <summary>
        /// Invalid RSS feed link. Ensure that you've entered the link correctly and try again.
        /// </summary>
        InvalidFeedLink,
        /// <summary>
        /// Invalid RSS feed type. Ensure that you've entered the feed type correctly and try again.
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
        /// Invalid mod. If your mod is a valid .NET assembly, check to make sure that it actually implements the interface that is necessary to start the mod.
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
        /// The mod failed to install. Check to make sure that it's a valid mod file. If your mod is a valid .NET assembly, check to make sure that it actually implements the interface that is necessary to start the mod.
        /// </summary>
        ModInstall,
        /// <summary>
        /// The mod contains no mod instance. Consult with the mod vendor for a new copy.
        /// </summary>
        ModWithoutMod,
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
        /// An assertion failure has been detected in the kernel! This is most likely a bug in the kernel module that should be fixed on our end.
        /// </summary>
        AssertionFailure,
        /// <summary>
        /// Your network needs to be online before being able to perform operations related to networking. Connect your network adapter and try again.
        /// </summary>
        NetworkOffline,
        /// <summary>
        /// Permission denied trying to perform an operation. You'll need to log in as a user that has the necessary permissions in order to be able to perform this operation.
        /// </summary>
        PermissionDenied,
        /// <summary>
        /// User doesn't exist. Check to make sure that you've written the user correctly.
        /// </summary>
        NoSuchUser,
        /// <summary>
        /// Driver doesn't exist. Check to make sure that you've written the driver name correctly, and that the driver is registered properly.
        /// </summary>
        NoSuchDriver,
        /// <summary>
        /// The thread is not ready yet. The user code might have forgotten to regenerate the kernel thread after stopping it manually.
        /// </summary>
        ThreadNotReadyYet,
        /// <summary>
        /// A thread operation is invalid in the current state. Refer to the additional message the kernel thread manager gave you for additional information.
        /// </summary>
        ThreadOperation,
        /// <summary>
        /// An invalid shell operation is being attempted.
        /// </summary>
        ShellOperation,
        /// <summary>
        /// This function or feature is not implemented yet.
        /// </summary>
        NotImplementedYet,
        /// <summary>
        /// An operation was attempted on the remote procedure system that is invalid.
        /// </summary>
        RemoteProcedure,
        /// <summary>
        /// An invalid encryption operation is being performed.
        /// </summary>
        Encryption,
        /// <summary>
        /// An invalid debugging operation is being performed.
        /// </summary>
        Debug,
        /// <summary>
        /// An invalid archive operation is being performed.
        /// </summary>
        Archive,
        /// <summary>
        /// An invalid hex editor operation is being performed.
        /// </summary>
        HexEditor,
        /// <summary>
        /// An invalid JSON editor operation is being performed.
        /// </summary>
        JsonEditor,
        /// <summary>
        /// An invalid text editor operation is being performed.
        /// </summary>
        TextEditor,
        /// <summary>
        /// When the kernel tried to load the specified mod, it requested loading "Kernel Simulator". Since the main application is renamed to Nitrocid KS, this mod can't be run safely. We advice you to upgrade the mod.
        /// </summary>
        OldModDetected,
        /// <summary>
        /// A regular expression error happened while the text is being processed. Check your regular expression syntax and try again. If the time-out occurred, ensure that you don't recurse too much (don't be greedy).
        /// </summary>
        RegularExpression,
        /// <summary>
        /// A contacts manager error occurred when trying to perform the requested operation. Make sure that the contact file exists in the correct folder and that it's a valid vCard 2.1, 3.0, or 4.0 contact file.
        /// </summary>
        Contacts,
        /// <summary>
        /// An invalid SQL editor operation is being performed.
        /// </summary>
        SqlEditor,
        /// <summary>
        /// Group doesn't exist. Check to make sure that you've written the group name correctly.
        /// </summary>
        NoSuchGroup,
        /// <summary>
        /// An error occurred in the custom settings manager for users.
        /// </summary>
        CustomSettings,
        /// <summary>
        /// An error occurred in the network connection manager.
        /// </summary>
        NetworkConnection,
        /// <summary>
        /// A general HTTP network error occurred. Check to make sure that your internet connection is working.
        /// </summary>
        HTTPNetwork,
        /// <summary>
        /// A general command manager error occurred. Check to make sure that you've written the command or the shell type correctly.
        /// </summary>
        CommandManager,
        /// <summary>
        /// Locale generator tool returned an error while trying to generate JSON files for languages.
        /// </summary>
        LocaleGen,
        /// <summary>
        /// An error occurred in the time and date module. Check to make sure that you've specified the time and the date correctly, and that the time zone exists.
        /// </summary>
        TimeDate,
        /// <summary>
        /// An error occurred in the mod manual parser. Please ensure that your manual file is valid and that the mod is started.
        /// </summary>
        ModManual,
        /// <summary>
        /// An error occurred in the calendar manager.
        /// </summary>
        Calendar,
        /// <summary>
        /// There was an error in the notification management.
        /// </summary>
        NotificationManagement,
        /// <summary>
        /// There was an error in the language management.
        /// </summary>
        LanguageManagement,
        /// <summary>
        /// There was an error in the mod management.
        /// </summary>
        ModManagement,
        /// <summary>
        /// There was an error in the reflection system.
        /// </summary>
        Reflection,
        /// <summary>
        /// There was an error when trying to perform an operation for the kernel event management.
        /// </summary>
        EventManagement,
        /// <summary>
        /// There was an error when trying to perform an operation for the kernel addon management.
        /// </summary>
        AddonManagement,
        /// <summary>
        /// There was an error when trying to perform an operation for the note management.
        /// </summary>
        NoteManagement,
        /// <summary>
        /// Hardware component manager failed.
        /// </summary>
        Hardware,
        /// <summary>
        /// Login handler failed. Please ensure that it's registered properly and that it does its job as expected.
        /// </summary>
        LoginHandler,
        /// <summary>
        /// Encoding has failed. Check to make sure that your encoding driver works correctly and fix any problems if found, then try again.
        /// </summary>
        Encoding,
        /// <summary>
        /// Invalid privacy consent operation. Please make sure that the consent is correct and try again.
        /// </summary>
        PrivacyConsent,
        /// <summary>
        /// Splash manager has failed to perform your requested operation. Please check the splash name and try again.
        /// </summary>
        Splash,
        /// <summary>
        /// Text tools failed to process your request.
        /// </summary>
        Text,
        /// <summary>
        /// Invalid placeholder action. Consult the kernel documentation for more information.
        /// </summary>
        InvalidPlaceholderAction,
        /// <summary>
        /// The driver handler failed to perform this action because you might have supplied the parameters wrong. If you're sure that they're specified correctly, make sure that you've provided the right driver name and type.
        /// </summary>
        DriverHandler,
        /// <summary>
        /// The progress handler has failed to perform the requested operation.
        /// </summary>
        ProgressHandler,
        /// <summary>
        /// The console operation failed to perform the required task.
        /// </summary>
        Console,
        /// <summary>
        /// The kernel journaling operation failed to perform the required task.
        /// </summary>
        Journaling,
        /// <summary>
        /// The system docking operation failed to perform the required task.
        /// </summary>
        Docking,
        /// <summary>
        /// The security operation failed to perform the required task.
        /// </summary>
        Security,
        /// <summary>
        /// There was an error when trying to perform an operation for the kernel driver management.
        /// </summary>
        DriverManagement,
        /// <summary>
        /// There was an error when trying to perform an operation for the environment management.
        /// </summary>
        Environment,
        /// <summary>
        /// There was an error when trying to process a bootloader operation.
        /// </summary>
        Bootloader,
        /// <summary>
        /// There was an error when trying to process an alarm system operation.
        /// </summary>
        Alarm,
        /// <summary>
        /// There was an error when trying to process a widget system operation. If you're sure that this widget is registered properly, please make sure that you've written the widget class name properly.
        /// </summary>
        Widget,
        /// <summary>
        /// The homepage tools has encountered an error when trying to process your request. Please make sure that you've entered all the necessary data correctly.
        /// </summary>
        Homepage,
        /// <summary>
        /// There is no culture by this name.
        /// </summary>
        NoSuchCulture,
        /// <summary>
        /// There was an error when processing your audio cue action.
        /// </summary>
        AudioCue,
    }
}
