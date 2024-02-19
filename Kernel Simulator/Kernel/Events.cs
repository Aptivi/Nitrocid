﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Themes;
using KS.Login;
using KS.Misc.Notifiers;
using KS.Misc.Writers.DebugWriters;
using KS.Modifications;

namespace KS.Kernel
{
    public class Events
    {

        /// <summary>
        /// Recently fired events
        /// </summary>
        internal Dictionary<string, object[]> FiredEvents { get; set; } = [];

        // These events are fired by their Raise<EventName>() subs and are responded by their Respond<EventName>() subs.
        public event KernelStartedEventHandler KernelStarted;

        public delegate void KernelStartedEventHandler();
        public event PreLoginEventHandler PreLogin;

        public delegate void PreLoginEventHandler();
        public event PostLoginEventHandler PostLogin;

        public delegate void PostLoginEventHandler(string Username);
        public event LoginErrorEventHandler LoginError;

        public delegate void LoginErrorEventHandler(string Username, LoginErrorReasons Reason);
        public event ShellInitializedEventHandler ShellInitialized;

        public delegate void ShellInitializedEventHandler();
        public event PreExecuteCommandEventHandler PreExecuteCommand;

        public delegate void PreExecuteCommandEventHandler(string Command);
        public event PostExecuteCommandEventHandler PostExecuteCommand;

        public delegate void PostExecuteCommandEventHandler(string Command);
        public event KernelErrorEventHandler KernelError;

        public delegate void KernelErrorEventHandler(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, object[] Variables);
        public event ContKernelErrorEventHandler ContKernelError;

        public delegate void ContKernelErrorEventHandler(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, object[] Variables);
        public event PreShutdownEventHandler PreShutdown;

        public delegate void PreShutdownEventHandler();
        public event PostShutdownEventHandler PostShutdown;

        public delegate void PostShutdownEventHandler();
        public event PreRebootEventHandler PreReboot;

        public delegate void PreRebootEventHandler();
        public event PostRebootEventHandler PostReboot;

        public delegate void PostRebootEventHandler();
        public event PreShowScreensaverEventHandler PreShowScreensaver;

        public delegate void PreShowScreensaverEventHandler(string Screensaver);
        public event PostShowScreensaverEventHandler PostShowScreensaver;

        public delegate void PostShowScreensaverEventHandler(string Screensaver); // After a key is pressed after screensaver is shown
        public event PreUnlockEventHandler PreUnlock;

        public delegate void PreUnlockEventHandler(string Screensaver);
        public event PostUnlockEventHandler PostUnlock;

        public delegate void PostUnlockEventHandler(string Screensaver);
        public event CommandErrorEventHandler CommandError;

        public delegate void CommandErrorEventHandler(string Command, Exception Exception);
        public event PreReloadConfigEventHandler PreReloadConfig;

        public delegate void PreReloadConfigEventHandler();
        public event PostReloadConfigEventHandler PostReloadConfig;

        public delegate void PostReloadConfigEventHandler();
        public event PlaceholderParsingEventHandler PlaceholderParsing;

        public delegate void PlaceholderParsingEventHandler(string Target);
        public event PlaceholderParsedEventHandler PlaceholderParsed;

        public delegate void PlaceholderParsedEventHandler(string Target);
        public event PlaceholderParseErrorEventHandler PlaceholderParseError;

        public delegate void PlaceholderParseErrorEventHandler(string Target, Exception Exception);
        public event GarbageCollectedEventHandler GarbageCollected;

        public delegate void GarbageCollectedEventHandler();
        public event FTPShellInitializedEventHandler FTPShellInitialized;

        public delegate void FTPShellInitializedEventHandler();
        public event FTPPreExecuteCommandEventHandler FTPPreExecuteCommand;

        public delegate void FTPPreExecuteCommandEventHandler(string Command);
        public event FTPPostExecuteCommandEventHandler FTPPostExecuteCommand;

        public delegate void FTPPostExecuteCommandEventHandler(string Command);
        public event FTPCommandErrorEventHandler FTPCommandError;

        public delegate void FTPCommandErrorEventHandler(string Command, Exception Exception);
        public event FTPPreDownloadEventHandler FTPPreDownload;

        public delegate void FTPPreDownloadEventHandler(string File);
        public event FTPPostDownloadEventHandler FTPPostDownload;

        public delegate void FTPPostDownloadEventHandler(string File, bool Success);
        public event FTPPreUploadEventHandler FTPPreUpload;

        public delegate void FTPPreUploadEventHandler(string File);
        public event FTPPostUploadEventHandler FTPPostUpload;

        public delegate void FTPPostUploadEventHandler(string File, bool Success);
        public event IMAPShellInitializedEventHandler IMAPShellInitialized;

        public delegate void IMAPShellInitializedEventHandler();
        public event IMAPPreExecuteCommandEventHandler IMAPPreExecuteCommand;

        public delegate void IMAPPreExecuteCommandEventHandler(string Command);
        public event IMAPPostExecuteCommandEventHandler IMAPPostExecuteCommand;

        public delegate void IMAPPostExecuteCommandEventHandler(string Command);
        public event IMAPCommandErrorEventHandler IMAPCommandError;

        public delegate void IMAPCommandErrorEventHandler(string Command, Exception Exception);
        public event RemoteDebugConnectionAcceptedEventHandler RemoteDebugConnectionAccepted;

        public delegate void RemoteDebugConnectionAcceptedEventHandler(string IP);
        public event RemoteDebugConnectionDisconnectedEventHandler RemoteDebugConnectionDisconnected;

        public delegate void RemoteDebugConnectionDisconnectedEventHandler(string IP);
        public event RemoteDebugExecuteCommandEventHandler RemoteDebugExecuteCommand;

        public delegate void RemoteDebugExecuteCommandEventHandler(string IP, string Command);
        public event RemoteDebugCommandErrorEventHandler RemoteDebugCommandError;

        public delegate void RemoteDebugCommandErrorEventHandler(string IP, string Command, Exception Exception);
        public event RPCCommandSentEventHandler RPCCommandSent;

        public delegate void RPCCommandSentEventHandler(string Command, string Argument, string IP, int Port);
        public event RPCCommandReceivedEventHandler RPCCommandReceived;

        public delegate void RPCCommandReceivedEventHandler(string Command, string IP, int Port);
        public event RPCCommandErrorEventHandler RPCCommandError;

        public delegate void RPCCommandErrorEventHandler(string Command, Exception Exception, string IP, int Port);
        public event RSSShellInitializedEventHandler RSSShellInitialized;

        public delegate void RSSShellInitializedEventHandler(string FeedUrl);
        public event RSSPreExecuteCommandEventHandler RSSPreExecuteCommand;

        public delegate void RSSPreExecuteCommandEventHandler(string FeedUrl, string Command);
        public event RSSPostExecuteCommandEventHandler RSSPostExecuteCommand;

        public delegate void RSSPostExecuteCommandEventHandler(string FeedUrl, string Command);
        public event RSSCommandErrorEventHandler RSSCommandError;

        public delegate void RSSCommandErrorEventHandler(string FeedUrl, string Command, Exception Exception);
        public event SFTPShellInitializedEventHandler SFTPShellInitialized;

        public delegate void SFTPShellInitializedEventHandler();
        public event SFTPPreExecuteCommandEventHandler SFTPPreExecuteCommand;

        public delegate void SFTPPreExecuteCommandEventHandler(string Command);
        public event SFTPPostExecuteCommandEventHandler SFTPPostExecuteCommand;

        public delegate void SFTPPostExecuteCommandEventHandler(string Command);
        public event SFTPCommandErrorEventHandler SFTPCommandError;

        public delegate void SFTPCommandErrorEventHandler(string Command, Exception Exception);
        public event SFTPPreDownloadEventHandler SFTPPreDownload;

        public delegate void SFTPPreDownloadEventHandler(string File);
        public event SFTPPostDownloadEventHandler SFTPPostDownload;

        public delegate void SFTPPostDownloadEventHandler(string File);
        public event SFTPDownloadErrorEventHandler SFTPDownloadError;

        public delegate void SFTPDownloadErrorEventHandler(string File, Exception Exception);
        public event SFTPPreUploadEventHandler SFTPPreUpload;

        public delegate void SFTPPreUploadEventHandler(string File);
        public event SFTPPostUploadEventHandler SFTPPostUpload;

        public delegate void SFTPPostUploadEventHandler(string File);
        public event SFTPUploadErrorEventHandler SFTPUploadError;

        public delegate void SFTPUploadErrorEventHandler(string File, Exception Exception);
        public event SSHConnectedEventHandler SSHConnected;

        public delegate void SSHConnectedEventHandler(string Target);
        public event SSHDisconnectedEventHandler SSHDisconnected;

        public delegate void SSHDisconnectedEventHandler();
        public event SSHPreExecuteCommandEventHandler SSHPreExecuteCommand;

        public delegate void SSHPreExecuteCommandEventHandler(string Target, string Command);
        public event SSHPostExecuteCommandEventHandler SSHPostExecuteCommand;

        public delegate void SSHPostExecuteCommandEventHandler(string Target, string Command);
        public event SSHCommandErrorEventHandler SSHCommandError;

        public delegate void SSHCommandErrorEventHandler(string Target, string Command, Exception Exception);
        public event SSHErrorEventHandler SSHError;

        public delegate void SSHErrorEventHandler(Exception Exception);
        public event UESHPreExecuteEventHandler UESHPreExecute;

        public delegate void UESHPreExecuteEventHandler(string Command, string Arguments);
        public event UESHPostExecuteEventHandler UESHPostExecute;

        public delegate void UESHPostExecuteEventHandler(string Command, string Arguments);
        public event UESHErrorEventHandler UESHError;

        public delegate void UESHErrorEventHandler(string Command, string Arguments, Exception Exception);
        public event TextShellInitializedEventHandler TextShellInitialized;

        public delegate void TextShellInitializedEventHandler();
        public event TextPreExecuteCommandEventHandler TextPreExecuteCommand;

        public delegate void TextPreExecuteCommandEventHandler(string Command);
        public event TextPostExecuteCommandEventHandler TextPostExecuteCommand;

        public delegate void TextPostExecuteCommandEventHandler(string Command);
        public event TextCommandErrorEventHandler TextCommandError;

        public delegate void TextCommandErrorEventHandler(string Command, Exception Exception);
        public event NotificationSentEventHandler NotificationSent;

        public delegate void NotificationSentEventHandler(Notification Notification);
        public event NotificationsSentEventHandler NotificationsSent;

        public delegate void NotificationsSentEventHandler(List<Notification> Notifications);
        public event NotificationReceivedEventHandler NotificationReceived;

        public delegate void NotificationReceivedEventHandler(Notification Notification);
        public event NotificationsReceivedEventHandler NotificationsReceived;

        public delegate void NotificationsReceivedEventHandler(List<Notification> Notifications);
        public event NotificationDismissedEventHandler NotificationDismissed;

        public delegate void NotificationDismissedEventHandler();
        public event ConfigSavedEventHandler ConfigSaved;

        public delegate void ConfigSavedEventHandler();
        public event ConfigSaveErrorEventHandler ConfigSaveError;

        public delegate void ConfigSaveErrorEventHandler(Exception Exception);
        public event ConfigReadEventHandler ConfigRead;

        public delegate void ConfigReadEventHandler();
        public event ConfigReadErrorEventHandler ConfigReadError;

        public delegate void ConfigReadErrorEventHandler(Exception Exception);
        public event PreExecuteModCommandEventHandler PreExecuteModCommand;

        public delegate void PreExecuteModCommandEventHandler(string Command);
        public event PostExecuteModCommandEventHandler PostExecuteModCommand;

        public delegate void PostExecuteModCommandEventHandler(string Command);
        public event ModParsedEventHandler ModParsed;

        public delegate void ModParsedEventHandler(string ModFileName);
        public event ModParseErrorEventHandler ModParseError;

        public delegate void ModParseErrorEventHandler(string ModFileName);
        public event ModFinalizedEventHandler ModFinalized;

        public delegate void ModFinalizedEventHandler(string ModFileName);
        public event ModFinalizationFailedEventHandler ModFinalizationFailed;

        public delegate void ModFinalizationFailedEventHandler(string ModFileName, string Reason);
        public event UserAddedEventHandler UserAdded;

        public delegate void UserAddedEventHandler(string Username);
        public event UserRemovedEventHandler UserRemoved;

        public delegate void UserRemovedEventHandler(string Username);
        public event UsernameChangedEventHandler UsernameChanged;

        public delegate void UsernameChangedEventHandler(string OldUsername, string NewUsername);
        public event UserPasswordChangedEventHandler UserPasswordChanged;

        public delegate void UserPasswordChangedEventHandler(string Username);
        public event HardwareProbingEventHandler HardwareProbing;

        public delegate void HardwareProbingEventHandler();
        public event HardwareProbedEventHandler HardwareProbed;

        public delegate void HardwareProbedEventHandler();
        public event CurrentDirectoryChangedEventHandler CurrentDirectoryChanged;

        public delegate void CurrentDirectoryChangedEventHandler();
        public event FileCreatedEventHandler FileCreated;

        public delegate void FileCreatedEventHandler(string File);
        public event DirectoryCreatedEventHandler DirectoryCreated;

        public delegate void DirectoryCreatedEventHandler(string Directory);
        public event FileCopiedEventHandler FileCopied;

        public delegate void FileCopiedEventHandler(string Source, string Destination);
        public event DirectoryCopiedEventHandler DirectoryCopied;

        public delegate void DirectoryCopiedEventHandler(string Source, string Destination);
        public event FileMovedEventHandler FileMoved;

        public delegate void FileMovedEventHandler(string Source, string Destination);
        public event DirectoryMovedEventHandler DirectoryMoved;

        public delegate void DirectoryMovedEventHandler(string Source, string Destination);
        public event FileRemovedEventHandler FileRemoved;

        public delegate void FileRemovedEventHandler(string File);
        public event DirectoryRemovedEventHandler DirectoryRemoved;

        public delegate void DirectoryRemovedEventHandler(string Directory);
        public event FileAttributeAddedEventHandler FileAttributeAdded;

        public delegate void FileAttributeAddedEventHandler(string File, FileAttributes Attributes);
        public event FileAttributeRemovedEventHandler FileAttributeRemoved;

        public delegate void FileAttributeRemovedEventHandler(string File, FileAttributes Attributes);
        public event ColorResetEventHandler ColorReset;

        public delegate void ColorResetEventHandler();
        public event ThemeSetEventHandler ThemeSet;

        public delegate void ThemeSetEventHandler(string Theme);
        public event ThemeSetErrorEventHandler ThemeSetError;

        public delegate void ThemeSetErrorEventHandler(string Theme, ThemeSetErrorReasons Reason);
        public event ColorSetEventHandler ColorSet;

        public delegate void ColorSetEventHandler();
        public event ColorSetErrorEventHandler ColorSetError;

        public delegate void ColorSetErrorEventHandler(ColorSetErrorReasons Reason);
        public event ThemeStudioStartedEventHandler ThemeStudioStarted;

        public delegate void ThemeStudioStartedEventHandler();
        public event ThemeStudioExitEventHandler ThemeStudioExit;

        public delegate void ThemeStudioExitEventHandler();
        public event ArgumentsInjectedEventHandler ArgumentsInjected;

        public delegate void ArgumentsInjectedEventHandler(List<string> InjectedArguments);
        public event ZipShellInitializedEventHandler ZipShellInitialized;

        public delegate void ZipShellInitializedEventHandler();
        public event ZipPreExecuteCommandEventHandler ZipPreExecuteCommand;

        public delegate void ZipPreExecuteCommandEventHandler(string Command);
        public event ZipPostExecuteCommandEventHandler ZipPostExecuteCommand;

        public delegate void ZipPostExecuteCommandEventHandler(string Command);
        public event ZipCommandErrorEventHandler ZipCommandError;

        public delegate void ZipCommandErrorEventHandler(string Command, Exception Exception);
        public event HTTPShellInitializedEventHandler HTTPShellInitialized;

        public delegate void HTTPShellInitializedEventHandler();
        public event HTTPPreExecuteCommandEventHandler HTTPPreExecuteCommand;

        public delegate void HTTPPreExecuteCommandEventHandler(string Command);
        public event HTTPPostExecuteCommandEventHandler HTTPPostExecuteCommand;

        public delegate void HTTPPostExecuteCommandEventHandler(string Command);
        public event HTTPCommandErrorEventHandler HTTPCommandError;

        public delegate void HTTPCommandErrorEventHandler(string Command, Exception Exception);
        public event ProcessErrorEventHandler ProcessError;

        public delegate void ProcessErrorEventHandler(string Process, Exception Exception);
        public event LanguageInstalledEventHandler LanguageInstalled;

        public delegate void LanguageInstalledEventHandler(string Language);
        public event LanguageUninstalledEventHandler LanguageUninstalled;

        public delegate void LanguageUninstalledEventHandler(string Language);
        public event LanguageInstallErrorEventHandler LanguageInstallError;

        public delegate void LanguageInstallErrorEventHandler(string Language, Exception Exception);
        public event LanguageUninstallErrorEventHandler LanguageUninstallError;

        public delegate void LanguageUninstallErrorEventHandler(string Language, Exception Exception);
        public event LanguagesInstalledEventHandler LanguagesInstalled;

        public delegate void LanguagesInstalledEventHandler();
        public event LanguagesUninstalledEventHandler LanguagesUninstalled;

        public delegate void LanguagesUninstalledEventHandler();
        public event LanguagesInstallErrorEventHandler LanguagesInstallError;

        public delegate void LanguagesInstallErrorEventHandler(Exception Exception);
        public event LanguagesUninstallErrorEventHandler LanguagesUninstallError;

        public delegate void LanguagesUninstallErrorEventHandler(Exception Exception);
        public event HexShellInitializedEventHandler HexShellInitialized;

        public delegate void HexShellInitializedEventHandler();
        public event HexPreExecuteCommandEventHandler HexPreExecuteCommand;

        public delegate void HexPreExecuteCommandEventHandler(string Command);
        public event HexPostExecuteCommandEventHandler HexPostExecuteCommand;

        public delegate void HexPostExecuteCommandEventHandler(string Command);
        public event HexCommandErrorEventHandler HexCommandError;

        public delegate void HexCommandErrorEventHandler(string Command, Exception Exception);
        public event TestShellInitializedEventHandler TestShellInitialized;

        public delegate void TestShellInitializedEventHandler();
        public event TestPreExecuteCommandEventHandler TestPreExecuteCommand;

        public delegate void TestPreExecuteCommandEventHandler(string Command);
        public event TestPostExecuteCommandEventHandler TestPostExecuteCommand;

        public delegate void TestPostExecuteCommandEventHandler(string Command);
        public event TestCommandErrorEventHandler TestCommandError;

        public delegate void TestCommandErrorEventHandler(string Command, Exception Exception);
        public event JsonShellInitializedEventHandler JsonShellInitialized;

        public delegate void JsonShellInitializedEventHandler();
        public event JsonPreExecuteCommandEventHandler JsonPreExecuteCommand;

        public delegate void JsonPreExecuteCommandEventHandler(string Command);
        public event JsonPostExecuteCommandEventHandler JsonPostExecuteCommand;

        public delegate void JsonPostExecuteCommandEventHandler(string Command);
        public event JsonCommandErrorEventHandler JsonCommandError;

        public delegate void JsonCommandErrorEventHandler(string Command, Exception Exception);
        public event RarShellInitializedEventHandler RarShellInitialized;

        public delegate void RarShellInitializedEventHandler();
        public event RarPreExecuteCommandEventHandler RarPreExecuteCommand;

        public delegate void RarPreExecuteCommandEventHandler(string Command);
        public event RarPostExecuteCommandEventHandler RarPostExecuteCommand;

        public delegate void RarPostExecuteCommandEventHandler(string Command);
        public event RarCommandErrorEventHandler RarCommandError;

        public delegate void RarCommandErrorEventHandler(string Command, Exception Exception);

        public Events()
        {
            KernelStarted += RespondStartKernel;
            PreLogin += RespondPreLogin;
            PostLogin += RespondPostLogin;
            LoginError += RespondLoginError;
            ShellInitialized += RespondShellInitialized;
            PreExecuteCommand += RespondPreExecuteCommand;
            PostExecuteCommand += RespondPostExecuteCommand;
            KernelError += RespondKernelError;
            ContKernelError += RespondContKernelError;
            PreShutdown += RespondPreShutdown;
            PostShutdown += RespondPostShutdown;
            PreReboot += RespondPreReboot;
            PostReboot += RespondPostReboot;
            PreShowScreensaver += RespondPreShowScreensaver;
            PostShowScreensaver += RespondPostShowScreensaver;
            PreUnlock += RespondPreUnlock;
            PostUnlock += RespondPostUnlock;
            CommandError += RespondCommandError;
            PreReloadConfig += RespondPreReloadConfig;
            PostReloadConfig += RespondPostReloadConfig;
            PlaceholderParsing += RespondPlaceholderParsing;
            PlaceholderParsed += RespondPlaceholderParsed;
            PlaceholderParseError += RespondPlaceholderParseError;
            GarbageCollected += RespondGarbageCollected;
            FTPShellInitialized += RespondFTPShellInitialized;
            FTPPreExecuteCommand += RespondFTPPreExecuteCommand;
            FTPPostExecuteCommand += RespondFTPPostExecuteCommand;
            FTPCommandError += RespondFTPCommandError;
            FTPPreDownload += RespondFTPPreDownload;
            FTPPostDownload += RespondFTPPostDownload;
            FTPPreUpload += RespondFTPPreUpload;
            FTPPostUpload += RespondFTPPostUpload;
            IMAPShellInitialized += RespondIMAPShellInitialized;
            IMAPPreExecuteCommand += RespondIMAPPreExecuteCommand;
            IMAPPostExecuteCommand += RespondIMAPPostExecuteCommand;
            IMAPCommandError += RespondIMAPCommandError;
            RemoteDebugConnectionAccepted += RespondRemoteDebugConnectionAccepted;
            RemoteDebugConnectionDisconnected += RespondRemoteDebugConnectionDisconnected;
            RemoteDebugExecuteCommand += RespondRemoteDebugExecuteCommand;
            RemoteDebugCommandError += RespondRemoteDebugCommandError;
            RPCCommandSent += RespondRPCCommandSent;
            RPCCommandReceived += RespondRPCCommandReceived;
            RPCCommandError += RespondRPCCommandError;
            RSSShellInitialized += RespondRSSShellInitialized;
            RSSPreExecuteCommand += RespondRSSPreExecuteCommand;
            RSSPostExecuteCommand += RespondRSSPostExecuteCommand;
            RSSCommandError += RespondRSSCommandError;
            SFTPShellInitialized += RespondSFTPShellInitialized;
            SFTPPreExecuteCommand += RespondSFTPPreExecuteCommand;
            SFTPPostExecuteCommand += RespondSFTPPostExecuteCommand;
            SFTPCommandError += RespondSFTPCommandError;
            SFTPPreDownload += RespondSFTPPreDownload;
            SFTPPostDownload += RespondSFTPPostDownload;
            SFTPDownloadError += RespondSFTPDownloadError;
            SFTPPreUpload += RespondSFTPPreUpload;
            SFTPPostUpload += RespondSFTPPostUpload;
            SFTPUploadError += RespondSFTPUploadError;
            SSHConnected += RespondSSHConnected;
            SSHDisconnected += RespondSSHDisconnected;
            SSHPreExecuteCommand += RespondSSHPreExecuteCommand;
            SSHPostExecuteCommand += RespondSSHPostExecuteCommand;
            SSHCommandError += RespondSSHCommandError;
            SSHError += RespondSSHError;
            UESHPreExecute += RespondUESHPreExecute;
            UESHPostExecute += RespondUESHPostExecute;
            UESHError += RespondUESHError;
            TextShellInitialized += RespondTextShellInitialized;
            TextPreExecuteCommand += RespondTextPreExecuteCommand;
            TextPostExecuteCommand += RespondTextPostExecuteCommand;
            TextCommandError += RespondTextCommandError;
            NotificationSent += RespondNotificationSent;
            NotificationsSent += RespondNotificationsSent;
            NotificationReceived += RespondNotificationReceived;
            NotificationsReceived += RespondNotificationsReceived;
            NotificationDismissed += RespondNotificationDismissed;
            ConfigSaved += RespondConfigSaved;
            ConfigSaveError += RespondConfigSaveError;
            ConfigRead += RespondConfigRead;
            ConfigReadError += RespondConfigReadError;
            PreExecuteModCommand += RespondPreExecuteModCommand;
            PostExecuteModCommand += RespondPostExecuteModCommand;
            ModParsed += RespondModParsed;
            ModParseError += RespondModParseError;
            ModFinalized += RespondModFinalized;
            ModFinalizationFailed += RespondModFinalizationFailed;
            UserAdded += RespondUserAdded;
            UserRemoved += RespondUserRemoved;
            UsernameChanged += RespondUsernameChanged;
            UserPasswordChanged += RespondUserPasswordChanged;
            HardwareProbing += RespondHardwareProbing;
            HardwareProbed += RespondHardwareProbed;
            CurrentDirectoryChanged += RespondCurrentDirectoryChanged;
            FileCreated += RespondFileCreated;
            DirectoryCreated += RespondDirectoryCreated;
            FileCopied += RespondFileCopied;
            DirectoryCopied += RespondDirectoryCopied;
            FileMoved += RespondFileMoved;
            DirectoryMoved += RespondDirectoryMoved;
            FileRemoved += RespondFileRemoved;
            DirectoryRemoved += RespondDirectoryRemoved;
            FileAttributeAdded += RespondFileAttributeAdded;
            FileAttributeRemoved += RespondFileAttributeRemoved;
            ColorReset += RespondColorReset;
            ThemeSet += RespondThemeSet;
            ThemeSetError += RespondThemeSetError;
            ColorSet += RespondColorSet;
            ColorSetError += RespondColorSetError;
            ThemeStudioStarted += RespondThemeStudioStarted;
            ThemeStudioExit += RespondThemeStudioExit;
            ArgumentsInjected += RespondArgumentsInjected;
            ZipShellInitialized += RespondZipShellInitialized;
            ZipPreExecuteCommand += RespondZipPreExecuteCommand;
            ZipPostExecuteCommand += RespondZipPostExecuteCommand;
            ZipCommandError += RespondZipCommandError;
            HTTPShellInitialized += RespondHTTPShellInitialized;
            HTTPPreExecuteCommand += RespondHTTPPreExecuteCommand;
            HTTPPostExecuteCommand += RespondHTTPPostExecuteCommand;
            HTTPCommandError += RespondHTTPCommandError;
            ProcessError += RespondProcessError;
            LanguageInstalled += RespondLanguageInstalled;
            LanguageUninstalled += RespondLanguageUninstalled;
            LanguageInstallError += RespondLanguageInstallError;
            LanguageUninstallError += RespondLanguageUninstallError;
            LanguagesInstalled += RespondLanguagesInstalled;
            LanguagesUninstalled += RespondLanguagesUninstalled;
            LanguagesInstallError += RespondLanguagesInstallError;
            LanguagesUninstallError += RespondLanguagesUninstallError;
            HexShellInitialized += RespondHexShellInitialized;
            HexPreExecuteCommand += RespondHexPreExecuteCommand;
            HexPostExecuteCommand += RespondHexPostExecuteCommand;
            HexCommandError += RespondHexCommandError;
            JsonShellInitialized += RespondJsonShellInitialized;
            JsonPreExecuteCommand += RespondJsonPreExecuteCommand;
            JsonPostExecuteCommand += RespondJsonPostExecuteCommand;
            JsonCommandError += RespondJsonCommandError;
            TestShellInitialized += RespondTestShellInitialized;
            TestPreExecuteCommand += RespondTestPreExecuteCommand;
            TestPostExecuteCommand += RespondTestPostExecuteCommand;
            TestCommandError += RespondTestCommandError;
            RarShellInitialized += RespondRarShellInitialized;
            RarPreExecuteCommand += RespondRarPreExecuteCommand;
            RarPostExecuteCommand += RespondRarPostExecuteCommand;
            RarCommandError += RespondRarCommandError;
        }

        /// <summary>
        /// Makes the mod respond to the event of kernel start
        /// </summary>
        public void RespondStartKernel()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event KernelStarted()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("KernelStarted");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-login
        /// </summary>
        public void RespondPreLogin()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreLogin()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreLogin");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-login
        /// </summary>
        public void RespondPostLogin(string Username)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostLogin()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostLogin", Username);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of login error
        /// </summary>
        public void RespondLoginError(string Username, LoginErrorReasons Reason)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LoginError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LoginError", Username, Reason);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of the shell being initialized
        /// </summary>
        public void RespondShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-execute command
        /// </summary>
        public void RespondPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-execute command
        /// </summary>
        public void RespondPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of kernel error
        /// </summary>
        public void RespondKernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, object[] Variables)
        {
            foreach (ModInfo ModPart in ModManager.Mods?.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event KernelError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("KernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of continuable kernel error
        /// </summary>
        public void RespondContKernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, object[] Variables)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ContKernelError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ContKernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-shutdown
        /// </summary>
        public void RespondPreShutdown()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreShutdown()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreShutdown");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-shutdown
        /// </summary>
        public void RespondPostShutdown()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostShutdown()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostShutdown");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-reboot
        /// </summary>
        public void RespondPreReboot()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreReboot()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreReboot");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-reboot
        /// </summary>
        public void RespondPostReboot()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostReboot()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostReboot");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-screensaver show
        /// </summary>
        public void RespondPreShowScreensaver(string Screensaver)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreShowScreensaver()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreShowScreensaver", Screensaver);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-screensaver show
        /// </summary>
        public void RespondPostShowScreensaver(string Screensaver)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostShowScreensaver()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostShowScreensaver", Screensaver);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-unlock
        /// </summary>
        public void RespondPreUnlock(string Screensaver)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreUnlock()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreUnlock", Screensaver);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-unlock
        /// </summary>
        public void RespondPostUnlock(string Screensaver)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostUnlock()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostUnlock", Screensaver);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of command error
        /// </summary>
        public void RespondCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event CommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("CommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-reload config
        /// </summary>
        public void RespondPreReloadConfig()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreReloadConfig()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreReloadConfig");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-reload config
        /// </summary>
        public void RespondPostReloadConfig()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostReloadConfig()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostReloadConfig");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of a placeholder being parsed
        /// </summary>
        public void RespondPlaceholderParsing(string Target)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PlaceholderParsing()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PlaceholderParsing", Target);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of a parsed placeholder
        /// </summary>
        public void RespondPlaceholderParsed(string Target)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PlaceholderParsed()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PlaceholderParsed", Target);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of a placeholder parse error
        /// </summary>
        public void RespondPlaceholderParseError(string Target, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PlaceholderParseError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PlaceholderParseError", Target, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of garbage collection finish
        /// </summary>
        public void RespondGarbageCollected()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event GarbageCollected()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("GarbageCollected");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of FTP shell initialized
        /// </summary>
        public void RespondFTPShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-command execution
        /// </summary>
        public void RespondFTPPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-command execution
        /// </summary>
        public void RespondFTPPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of FTP command error
        /// </summary>
        public void RespondFTPCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of FTP pre-download
        /// </summary>
        public void RespondFTPPreDownload(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPreDownload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPPreDownload", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of FTP post-download
        /// </summary>
        public void RespondFTPPostDownload(string File, bool Success)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPostDownload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPPostDownload", File, Success);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of FTP pre-upload
        /// </summary>
        public void RespondFTPPreUpload(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPreUpload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPPreUpload", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of FTP post-upload
        /// </summary>
        public void RespondFTPPostUpload(string File, bool Success)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FTPPostUpload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FTPPostUpload", File, Success);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of IMAP shell initialized
        /// </summary>
        public void RespondIMAPShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("IMAPShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of IMAP pre-command execution
        /// </summary>
        public void RespondIMAPPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("IMAPPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of IMAP post-command execution
        /// </summary>
        public void RespondIMAPPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("IMAPPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of IMAP command error
        /// </summary>
        public void RespondIMAPCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event IMAPCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("IMAPCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of remote debugging connection accepted
        /// </summary>
        public void RespondRemoteDebugConnectionAccepted(string IP)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugConnectionAccepted()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RemoteDebugConnectionAccepted", IP);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of remote debugging connection disconnected
        /// </summary>
        public void RespondRemoteDebugConnectionDisconnected(string IP)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugConnectionDisconnected()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RemoteDebugConnectionDisconnected", IP);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of remote debugging command execution
        /// </summary>
        public void RespondRemoteDebugExecuteCommand(string IP, string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RemoteDebugExecuteCommand", IP, Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of remote debugging command error
        /// </summary>
        public void RespondRemoteDebugCommandError(string IP, string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RemoteDebugCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RemoteDebugCommandError", IP, Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of RPC command sent
        /// </summary>
        public void RespondRPCCommandSent(string Command, string Argument, string IP, int Port)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RPCCommandSent()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RPCCommandSent", Command, Argument, IP, Port);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of RPC command received
        /// </summary>
        public void RespondRPCCommandReceived(string Command, string IP, int Port)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RPCCommandReceived()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RPCCommandReceived", Command, IP, Port);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of RPC command error
        /// </summary>
        public void RespondRPCCommandError(string Command, Exception Exception, string IP, int Port)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RPCCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RPCCommandError", Command, Exception, IP, Port);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of RSS shell initialized
        /// </summary>
        public void RespondRSSShellInitialized(string FeedUrl)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RSSShellInitialized", FeedUrl);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-command execution
        /// </summary>
        public void RespondRSSPreExecuteCommand(string FeedUrl, string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RSSPreExecuteCommand", FeedUrl, Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-command execution
        /// </summary>
        public void RespondRSSPostExecuteCommand(string FeedUrl, string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RSSPostExecuteCommand", FeedUrl, Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of RSS command error
        /// </summary>
        public void RespondRSSCommandError(string FeedUrl, string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RSSCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RSSCommandError", FeedUrl, Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP shell initialized
        /// </summary>
        public void RespondSFTPShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-command execution
        /// </summary>
        public void RespondSFTPPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-command execution
        /// </summary>
        public void RespondSFTPPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP command error
        /// </summary>
        public void RespondSFTPCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP pre-download
        /// </summary>
        public void RespondSFTPPreDownload(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPreDownload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPPreDownload", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP post-download
        /// </summary>
        public void RespondSFTPPostDownload(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPostDownload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPPostDownload", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP download error
        /// </summary>
        public void RespondSFTPDownloadError(string File, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPDownloadError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPDownloadError", File, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP pre-upload
        /// </summary>
        public void RespondSFTPPreUpload(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPreUpload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPPreUpload", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP post-upload
        /// </summary>
        public void RespondSFTPPostUpload(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPPostUpload()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPPostUpload", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SFTP download error
        /// </summary>
        public void RespondSFTPUploadError(string File, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SFTPUploadError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SFTPUploadError", File, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SSH being connected
        /// </summary>
        public void RespondSSHConnected(string Target)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHConnected()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SSHConnected", Target);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SSH being disconnected
        /// </summary>
        public void RespondSSHDisconnected()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHDisconnected()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SSHDisconnected");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-command execution
        /// </summary>
        public void RespondSSHPreExecuteCommand(string Target, string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SSHPreExecuteCommand", Target, Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-command execution
        /// </summary>
        public void RespondSSHPostExecuteCommand(string Target, string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SSHPostExecuteCommand", Target, Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SSH command error
        /// </summary>
        public void RespondSSHCommandError(string Target, string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SSHCommandError", Target, Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of SSH error
        /// </summary>
        public void RespondSSHError(Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event SSHError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("SSHError", Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of UESH pre-execute
        /// </summary>
        public void RespondUESHPreExecute(string Command, string Arguments)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UESHPreExecute()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("UESHPreExecute", Command, Arguments);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of UESH post-execute
        /// </summary>
        public void RespondUESHPostExecute(string Command, string Arguments)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UESHPostExecute()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("UESHPostExecute", Command, Arguments);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of UESH post-execute
        /// </summary>
        public void RespondUESHError(string Command, string Arguments, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UESHError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("UESHError", Command, Arguments, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of text shell initialized
        /// </summary>
        public void RespondTextShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TextShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of text pre-command execution
        /// </summary>
        public void RespondTextPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TextPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of text post-command execution
        /// </summary>
        public void RespondTextPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TextPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of text command error
        /// </summary>
        public void RespondTextCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TextCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TextCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of notification being sent
        /// </summary>
        public void RespondNotificationSent(Notification Notification)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationSent()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("NotificationSent", Notification);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of notifications being sent
        /// </summary>
        public void RespondNotificationsSent(List<Notification> Notifications)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationsSent()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("NotificationsSent", Notifications);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of notification being received
        /// </summary>
        public void RespondNotificationReceived(Notification Notification)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationReceived()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("NotificationReceived", Notification);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of notifications being received
        /// </summary>
        public void RespondNotificationsReceived(List<Notification> Notifications)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationsReceived()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("NotificationsReceived", Notifications);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of notification being dismissed
        /// </summary>
        public void RespondNotificationDismissed()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event NotificationDismissed()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("NotificationDismissed");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of config being saved
        /// </summary>
        public void RespondConfigSaved()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigSaved()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ConfigSaved");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of config having problems saving
        /// </summary>
        public void RespondConfigSaveError(Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigSaveError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ConfigSaveError", Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of config being read
        /// </summary>
        public void RespondConfigRead()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigRead()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ConfigRead");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of config having problems reading
        /// </summary>
        public void RespondConfigReadError(Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ConfigReadError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ConfigReadError", Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of mod command pre-execution
        /// </summary>
        public void RespondPreExecuteModCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PreExecuteModCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PreExecuteModCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of mod command post-execution
        /// </summary>
        public void RespondPostExecuteModCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event PostExecuteModCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("PostExecuteModCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of mod being parsed
        /// </summary>
        public void RespondModParsed(string ModFileName)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModParsed()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ModParsed", ModFileName);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of mod having problems parsing
        /// </summary>
        public void RespondModParseError(string ModFileName)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModParseError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ModParseError", ModFileName);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of mod being finalized
        /// </summary>
        public void RespondModFinalized(string ModFileName)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModFinalized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ModFinalized", ModFileName);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of mod having problems finalizing
        /// </summary>
        public void RespondModFinalizationFailed(string ModFileName, string Reason)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ModFinalizationFailed()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ModFinalizationFailed", ModFileName, Reason);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of user being added
        /// </summary>
        public void RespondUserAdded(string Username)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UserAdded()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("UserAdded", Username);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of user being removed
        /// </summary>
        public void RespondUserRemoved(string Username)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UserRemoved()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("UserRemoved", Username);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of username being changed
        /// </summary>
        public void RespondUsernameChanged(string OldUsername, string NewUsername)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UsernameChanged()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("UsernameChanged", OldUsername, NewUsername);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of user password being changed
        /// </summary>
        public void RespondUserPasswordChanged(string Username)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event UserPasswordChanged()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("UserPasswordChanged", Username);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of hardware probing
        /// </summary>
        public void RespondHardwareProbing()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HardwareProbing()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HardwareProbing");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of hardware being probed
        /// </summary>
        public void RespondHardwareProbed()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HardwareProbed()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HardwareProbed");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of current directory being changed
        /// </summary>
        public void RespondCurrentDirectoryChanged()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event CurrentDirectoryChanged()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("CurrentDirectoryChanged");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of file creation
        /// </summary>
        public void RespondFileCreated(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileCreated()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FileCreated", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of directory creation
        /// </summary>
        public void RespondDirectoryCreated(string Directory)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryCreated()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("DirectoryCreated", Directory);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of file copying process
        /// </summary>
        public void RespondFileCopied(string Source, string Destination)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileCopied()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FileCopied", Source, Destination);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of directory copying process
        /// </summary>
        public void RespondDirectoryCopied(string Source, string Destination)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryCopied()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("DirectoryCopied", Source, Destination);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of file moving process
        /// </summary>
        public void RespondFileMoved(string Source, string Destination)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileMoved()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FileMoved", Source, Destination);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of directory moving process
        /// </summary>
        public void RespondDirectoryMoved(string Source, string Destination)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryMoved()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("DirectoryMoved", Source, Destination);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of file removal
        /// </summary>
        public void RespondFileRemoved(string File)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileRemoved()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FileRemoved", File);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of directory removal
        /// </summary>
        public void RespondDirectoryRemoved(string Directory)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event DirectoryRemoved()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("DirectoryRemoved", Directory);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of file attribute addition
        /// </summary>
        public void RespondFileAttributeAdded(string File, FileAttributes Attributes)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileAttributeAdded()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FileAttributeAdded", File, Attributes);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of file attribute removal
        /// </summary>
        public void RespondFileAttributeRemoved(string File, FileAttributes Attributes)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event FileAttributeRemoved()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("FileAttributeRemoved", File, Attributes);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of console colors being reset
        /// </summary>
        public void RespondColorReset()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ColorReset()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ColorReset");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of theme setting
        /// </summary>
        public void RespondThemeSet(string Theme)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeSet()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ThemeSet", Theme);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of theme setting problem
        /// </summary>
        public void RespondThemeSetError(string Theme, ThemeSetErrorReasons Reason)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeSetError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ThemeSetError", Theme, Reason);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of console colors being set
        /// </summary>
        public void RespondColorSet()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ColorSet()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ColorSet");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of console colors having problems being set
        /// </summary>
        public void RespondColorSetError(ColorSetErrorReasons Reason)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ColorSetError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ColorSetError", Reason);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of theme studio start
        /// </summary>
        public void RespondThemeStudioStarted()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeStudioStarted()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ThemeStudioStarted");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of theme studio exit
        /// </summary>
        public void RespondThemeStudioExit()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ThemeStudioExit()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ThemeStudioExit");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of console colors having problems being set
        /// </summary>
        public void RespondArgumentsInjected(List<string> InjectedArguments)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ArgumentsInjected()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ArgumentsInjected", InjectedArguments);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP shell initialized
        /// </summary>
        public void RespondZipShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ZipShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP pre-command execution
        /// </summary>
        public void RespondZipPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ZipPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP post-command execution
        /// </summary>
        public void RespondZipPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ZipPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP command error
        /// </summary>
        public void RespondZipCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ZipCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ZipCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of HTTP shell initialized
        /// </summary>
        public void RespondHTTPShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HTTPShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of pre-command execution
        /// </summary>
        public void RespondHTTPPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HTTPPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of post-command execution
        /// </summary>
        public void RespondHTTPPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HTTPPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of HTTP command error
        /// </summary>
        public void RespondHTTPCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HTTPCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HTTPCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of process error
        /// </summary>
        public void RespondProcessError(string Process, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event ProcessError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("ProcessError", Process, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of a custom language installation
        /// </summary>
        public void RespondLanguageInstalled(string Language)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageInstalled()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguageInstalled", Language);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of a custom language uninstallation
        /// </summary>
        public void RespondLanguageUninstalled(string Language)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageUninstalled()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguageUninstalled", Language);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of custom language install error
        /// </summary>
        public void RespondLanguageInstallError(string Language, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageInstallError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguageInstallError", Language, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of custom language uninstall error
        /// </summary>
        public void RespondLanguageUninstallError(string Language, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguageUninstallError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguageUninstallError", Language, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of custom languages installation
        /// </summary>
        public void RespondLanguagesInstalled()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesInstalled()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguagesInstalled");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of custom languages uninstallation
        /// </summary>
        public void RespondLanguagesUninstalled()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesUninstalled()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguagesUninstalled");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of custom languages install error
        /// </summary>
        public void RespondLanguagesInstallError(Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesInstallError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguagesInstallError", Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of custom languages uninstall error
        /// </summary>
        public void RespondLanguagesUninstallError(Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event LanguagesUninstallError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("LanguagesUninstallError", Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Hex shell initialized
        /// </summary>
        public void RespondHexShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HexShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Hex pre-command execution
        /// </summary>
        public void RespondHexPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HexPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Hex post-command execution
        /// </summary>
        public void RespondHexPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HexPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Hex command error
        /// </summary>
        public void RespondHexCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event HexCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("HexCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Json shell initialized
        /// </summary>
        public void RespondJsonShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("JsonShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Json pre-command execution
        /// </summary>
        public void RespondJsonPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("JsonPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Json post-command execution
        /// </summary>
        public void RespondJsonPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("JsonPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Json command error
        /// </summary>
        public void RespondJsonCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event JsonCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("JsonCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Test shell initialized
        /// </summary>
        public void RespondTestShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TestShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Test pre-command execution
        /// </summary>
        public void RespondTestPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TestPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Test post-command execution
        /// </summary>
        public void RespondTestPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TestPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of Test command error
        /// </summary>
        public void RespondTestCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event TestCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("TestCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP shell initialized
        /// </summary>
        public void RespondRarShellInitialized()
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RarShellInitialized()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RarShellInitialized");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP pre-command execution
        /// </summary>
        public void RespondRarPreExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RarPreExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RarPreExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP post-command execution
        /// </summary>
        public void RespondRarPostExecuteCommand(string Command)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RarPostExecuteCommand()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RarPostExecuteCommand", Command);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }
        /// <summary>
        /// Makes the mod respond to the event of ZIP command error
        /// </summary>
        public void RespondRarCommandError(string Command, Exception Exception)
        {
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event RarCommandError()...", script.ModPart, script.Name, script.Version);
                        script.InitEvents("RarCommandError", Command, Exception);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WStkTrcConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }

        // These subs are for raising events
        /// <summary>
        /// Raise an event of kernel start
        /// </summary>
        public void RaiseStartKernel()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event KernelStarted() and responding in RespondStartKernel()...");
            FiredEvents.Add("KernelStarted (" + FiredEvents.Count.ToString() + ")", []);
            KernelStarted?.Invoke();
        }
        /// <summary>
        /// Raise an event of pre-login
        /// </summary>
        public void RaisePreLogin()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreLogin() and responding in RespondPreLogin()...");
            FiredEvents.Add("PreLogin (" + FiredEvents.Count.ToString() + ")", []);
            PreLogin?.Invoke();
        }
        /// <summary>
        /// Raise an event of post-login
        /// </summary>
        public void RaisePostLogin(string Username)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostLogin() and responding in RespondPostLogin()...");
            FiredEvents.Add("PostLogin (" + FiredEvents.Count.ToString() + ")", [Username]);
            PostLogin?.Invoke(Username);
        }
        /// <summary>
        /// Raise an event of login error
        /// </summary>
        public void RaiseLoginError(string Username, LoginErrorReasons Reason)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LoginError() and responding in RespondLoginError()...");
            FiredEvents.Add("LoginError (" + FiredEvents.Count.ToString() + ")", [Username, Reason]);
            LoginError?.Invoke(Username, Reason);
        }
        /// <summary>
        /// Raise an event of shell initialized
        /// </summary>
        public void RaiseShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ShellInitialized() and responding in RespondShellInitialized()...");
            FiredEvents.Add("ShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            ShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of pre-command execution
        /// </summary>
        public void RaisePreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreExecuteCommand() and responding in RespondPreExecuteCommand()...");
            FiredEvents.Add("PreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            PreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of post-command execution
        /// </summary>
        public void RaisePostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostExecuteCommand() and responding in RespondPostExecuteCommand()...");
            FiredEvents.Add("PostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            PostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of kernel error
        /// </summary>
        public void RaiseKernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, object[] Variables)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event KernelError() and responding in RespondKernelError()...");
            FiredEvents.Add("KernelError (" + FiredEvents.Count.ToString() + ")", [ErrorType, Reboot, (object)RebootTime, Description, Exc, Variables]);
            KernelError?.Invoke(ErrorType, Reboot, RebootTime, Description, Exc, Variables);
        }
        /// <summary>
        /// Raise an event of continuable kernel error
        /// </summary>
        public void RaiseContKernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, object[] Variables)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ContKernelError() and responding in RespondContKernelError()...");
            FiredEvents.Add("ContKernelError (" + FiredEvents.Count.ToString() + ")", [ErrorType, Reboot, (object)RebootTime, Description, Exc, Variables]);
            ContKernelError?.Invoke(ErrorType, Reboot, RebootTime, Description, Exc, Variables);
        }
        /// <summary>
        /// Raise an event of pre-shutdown
        /// </summary>
        public void RaisePreShutdown()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreShutdown() and responding in RespondPreShutdown()...");
            FiredEvents.Add("PreShutdown (" + FiredEvents.Count.ToString() + ")", []);
            PreShutdown?.Invoke();
        }
        /// <summary>
        /// Raise an event of post-shutdown
        /// </summary>
        public void RaisePostShutdown()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostShutdown() and responding in RespondPostShutdown()...");
            FiredEvents.Add("PostShutdown (" + FiredEvents.Count.ToString() + ")", []);
            PostShutdown?.Invoke();
        }
        /// <summary>
        /// Raise an event of pre-reboot
        /// </summary>
        public void RaisePreReboot()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreReboot() and responding in RespondPreReboot()...");
            FiredEvents.Add("PreReboot (" + FiredEvents.Count.ToString() + ")", []);
            PreReboot?.Invoke();
        }
        /// <summary>
        /// Raise an event of post-reboot
        /// </summary>
        public void RaisePostReboot()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostReboot() and responding in RespondPostReboot()...");
            FiredEvents.Add("PostReboot (" + FiredEvents.Count.ToString() + ")", []);
            PostReboot?.Invoke();
        }
        /// <summary>
        /// Raise an event of pre-show screensaver
        /// </summary>
        public void RaisePreShowScreensaver(string Screensaver)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreShowScreensaver() and responding in RespondPreShowScreensaver()...");
            FiredEvents.Add("PreShowScreensaver (" + FiredEvents.Count.ToString() + ")", [Screensaver]);
            PreShowScreensaver?.Invoke(Screensaver);
        }
        /// <summary>
        /// Raise an event of post-show screensaver
        /// </summary>
        public void RaisePostShowScreensaver(string Screensaver)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostShowScreensaver() and responding in RespondPostShowScreensaver()...");
            FiredEvents.Add("PostShowScreensaver (" + FiredEvents.Count.ToString() + ")", [Screensaver]);
            PostShowScreensaver?.Invoke(Screensaver);
        }
        /// <summary>
        /// Raise an event of pre-unlock
        /// </summary>
        public void RaisePreUnlock(string Screensaver)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreUnlock() and responding in RespondPreUnlock()...");
            FiredEvents.Add("PreUnlock (" + FiredEvents.Count.ToString() + ")", [Screensaver]);
            PreUnlock?.Invoke(Screensaver);
        }
        /// <summary>
        /// Raise an event of post-unlock
        /// </summary>
        public void RaisePostUnlock(string Screensaver)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostUnlock() and responding in RespondPostUnlock()...");
            FiredEvents.Add("PostUnlock (" + FiredEvents.Count.ToString() + ")", [Screensaver]);
            PostUnlock?.Invoke(Screensaver);
        }
        /// <summary>
        /// Raise an event of command error
        /// </summary>
        public void RaiseCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event CommandError() and responding in RespondCommandError()...");
            FiredEvents.Add("CommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            CommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of pre-reload config
        /// </summary>
        public void RaisePreReloadConfig()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreReloadConfig() and responding in RespondPreReloadConfig()...");
            FiredEvents.Add("PreReloadConfig (" + FiredEvents.Count.ToString() + ")", []);
            PreReloadConfig?.Invoke();
        }
        /// <summary>
        /// Raise an event of post-reload config
        /// </summary>
        public void RaisePostReloadConfig()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostReloadConfig() and responding in RespondPostReloadConfig()...");
            FiredEvents.Add("PostReloadConfig (" + FiredEvents.Count.ToString() + ")", []);
            PostReloadConfig?.Invoke();
        }
        /// <summary>
        /// Raise an event of placeholders being parsed
        /// </summary>
        public void RaisePlaceholderParsing(string Target)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PlaceholderParsing() and responding in RespondPlaceholderParsing()...");
            FiredEvents.Add("PlaceholderParsing (" + FiredEvents.Count.ToString() + ")", [Target]);
            PlaceholderParsing?.Invoke(Target);
        }
        /// <summary>
        /// Raise an event of a parsed placeholder
        /// </summary>
        public void RaisePlaceholderParsed(string Target)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PlaceholderParsed() and responding in RespondPlaceholderParsed()...");
            FiredEvents.Add("PlaceholderParsed (" + FiredEvents.Count.ToString() + ")", [Target]);
            PlaceholderParsed?.Invoke(Target);
        }
        /// <summary>
        /// Raise an event of a placeholder parse error
        /// </summary>
        public void RaisePlaceholderParseError(string Target, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PlaceholderParseError() and responding in RespondPlaceholderParseError()...");
            FiredEvents.Add("PlaceholderParseError (" + FiredEvents.Count.ToString() + ")", [Target, Exception]);
            PlaceholderParseError?.Invoke(Target, Exception);
        }
        /// <summary>
        /// Raise an event of garbage collection finish
        /// </summary>
        public void RaiseGarbageCollected()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event GarbageCollected() and responding in RespondGarbageCollected()...");
            FiredEvents.Add("GarbageCollected (" + FiredEvents.Count.ToString() + ")", []);
            GarbageCollected?.Invoke();
        }
        /// <summary>
        /// Raise an event of FTP shell initialized
        /// </summary>
        public void RaiseFTPShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPShellInitialized() and responding in RespondFTPShellInitialized()...");
            FiredEvents.Add("FTPShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            FTPShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of FTP pre-execute command
        /// </summary>
        public void RaiseFTPPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPPreExecuteCommand() and responding in RespondFTPPreExecuteCommand()...");
            FiredEvents.Add("FTPPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            FTPPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of FTP post-execute command
        /// </summary>
        public void RaiseFTPPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPPostExecuteCommand() and responding in RespondFTPPostExecuteCommand()...");
            FiredEvents.Add("FTPPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            FTPPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of FTP command error
        /// </summary>
        public void RaiseFTPCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPCommandError() and responding in RespondFTPCommandError()...");
            FiredEvents.Add("FTPCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            FTPCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of FTP pre-download
        /// </summary>
        public void RaiseFTPPreDownload(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPPreDownload() and responding in RespondFTPPreDownload()...");
            FiredEvents.Add("FTPPreDownload (" + FiredEvents.Count.ToString() + ")", [File]);
            FTPPreDownload?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of FTP post-download
        /// </summary>
        public void RaiseFTPPostDownload(string File, bool Success)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPPostDownload() and responding in RespondFTPPostDownload()...");
            FiredEvents.Add("FTPPostDownload (" + FiredEvents.Count.ToString() + ")", [File, (object)Success]);
            FTPPostDownload?.Invoke(File, Success);
        }
        /// <summary>
        /// Raise an event of FTP pre-upload
        /// </summary>
        public void RaiseFTPPreUpload(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPPreUpload() and responding in RespondFTPPreUpload()...");
            FiredEvents.Add("FTPPreUpload (" + FiredEvents.Count.ToString() + ")", [File]);
            FTPPreUpload?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of FTP post-upload
        /// </summary>
        public void RaiseFTPPostUpload(string File, bool Success)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FTPPostUpload() and responding in RespondFTPPostUpload()...");
            FiredEvents.Add("FTPPostUpload (" + FiredEvents.Count.ToString() + ")", [File, (object)Success]);
            FTPPostUpload?.Invoke(File, Success);
        }
        /// <summary>
        /// Raise an event of IMAP shell initialized
        /// </summary>
        public void RaiseIMAPShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event IMAPShellInitialized() and responding in RespondIMAPShellInitialized()...");
            FiredEvents.Add("IMAPShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            IMAPShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of IMAP pre-command execution
        /// </summary>
        public void RaiseIMAPPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event IMAPPreExecuteCommand() and responding in RespondIMAPPreExecuteCommand()...");
            FiredEvents.Add("IMAPPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            IMAPPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of IMAP post-command execution
        /// </summary>
        public void RaiseIMAPPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event IMAPPostExecuteCommand() and responding in RespondIMAPPostExecuteCommand()...");
            FiredEvents.Add("IMAPPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            IMAPPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of IMAP command error
        /// </summary>
        public void RaiseIMAPCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event IMAPCommandError() and responding in RespondIMAPCommandError()...");
            FiredEvents.Add("IMAPCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            IMAPCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of remote debugging connection accepted
        /// </summary>
        public void RaiseRemoteDebugConnectionAccepted(string IP)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RemoteDebugConnectionAccepted() and responding in RespondRemoteDebugConnectionAccepted()...");
            FiredEvents.Add("RemoteDebugConnectionAccepted (" + FiredEvents.Count.ToString() + ")", [IP]);
            RemoteDebugConnectionAccepted?.Invoke(IP);
        }
        /// <summary>
        /// Raise an event of remote debugging connection disconnected
        /// </summary>
        public void RaiseRemoteDebugConnectionDisconnected(string IP)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RemoteDebugConnectionDisconnected() and responding in RespondRemoteDebugConnectionDisconnected()...");
            FiredEvents.Add("RemoteDebugConnectionDisconnected (" + FiredEvents.Count.ToString() + ")", [IP]);
            RemoteDebugConnectionDisconnected?.Invoke(IP);
        }
        /// <summary>
        /// Raise an event of remote debugging command execution
        /// </summary>
        public void RaiseRemoteDebugExecuteCommand(string IP, string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RemoteDebugExecuteCommand() and responding in RespondRemoteDebugExecuteCommand()...");
            FiredEvents.Add("RemoteDebugExecuteCommand (" + FiredEvents.Count.ToString() + ")", [IP, Command]);
            RemoteDebugExecuteCommand?.Invoke(IP, Command);
        }
        /// <summary>
        /// Raise an event of remote debugging command error
        /// </summary>
        public void RaiseRemoteDebugCommandError(string IP, string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RemoteDebugCommandError() and responding in RespondRemoteDebugCommandError()...");
            FiredEvents.Add("RemoteDebugCommandError (" + FiredEvents.Count.ToString() + ")", [IP, Command, Exception]);
            RemoteDebugCommandError?.Invoke(IP, Command, Exception);
        }
        /// <summary>
        /// Raise an event of RPC command sent
        /// </summary>
        public void RaiseRPCCommandSent(string Command, string Argument, string IP, int Port)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RPCCommandSent() and responding in RespondRPCCommandSent()...");
            FiredEvents.Add("RPCCommandSent (" + FiredEvents.Count.ToString() + ")", [Command]);
            RPCCommandSent?.Invoke(Command, Argument, IP, Port);
        }
        /// <summary>
        /// Raise an event of RPC command received
        /// </summary>
        public void RaiseRPCCommandReceived(string Command, string IP, int Port)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RPCCommandReceived() and responding in RespondRPCCommandReceived()...");
            FiredEvents.Add("RPCCommandReceived (" + FiredEvents.Count.ToString() + ")", [Command]);
            RPCCommandReceived?.Invoke(Command, IP, Port);
        }
        /// <summary>
        /// Raise an event of RPC command error
        /// </summary>
        public void RaiseRPCCommandError(string Command, Exception Exception, string IP, int Port)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RPCCommandError() and responding in RespondRPCCommandError()...");
            FiredEvents.Add("RPCCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            RPCCommandError?.Invoke(Command, Exception, IP, Port);
        }
        /// <summary>
        /// Raise an event of RSS shell initialized
        /// </summary>
        public void RaiseRSSShellInitialized(string FeedUrl)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RSSShellInitialized() and responding in RespondRSSShellInitialized()...");
            FiredEvents.Add("RSSShellInitialized (" + FiredEvents.Count.ToString() + ")", [FeedUrl]);
            RSSShellInitialized?.Invoke(FeedUrl);
        }
        /// <summary>
        /// Raise an event of RSS pre-execute command
        /// </summary>
        public void RaiseRSSPreExecuteCommand(string FeedUrl, string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RSSPreExecuteCommand() and responding in RespondRSSPreExecuteCommand()...");
            FiredEvents.Add("RSSPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [FeedUrl, Command]);
            RSSPreExecuteCommand?.Invoke(FeedUrl, Command);
        }
        /// <summary>
        /// Raise an event of RSS post-execute command
        /// </summary>
        public void RaiseRSSPostExecuteCommand(string FeedUrl, string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RSSPostExecuteCommand() and responding in RespondRSSPostExecuteCommand()...");
            FiredEvents.Add("RSSPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [FeedUrl, Command]);
            RSSPostExecuteCommand?.Invoke(FeedUrl, Command);
        }
        /// <summary>
        /// Raise an event of RSS command error
        /// </summary>
        public void RaiseRSSCommandError(string FeedUrl, string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RSSCommandError() and responding in RespondRSSCommandError()...");
            FiredEvents.Add("RSSCommandError (" + FiredEvents.Count.ToString() + ")", [FeedUrl, Command, Exception]);
            RSSCommandError?.Invoke(FeedUrl, Command, Exception);
        }
        /// <summary>
        /// Raise an event of SFTP shell initialized
        /// </summary>
        public void RaiseSFTPShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPShellInitialized() and responding in RespondSFTPShellInitialized()...");
            FiredEvents.Add("SFTPShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            SFTPShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of SFTP pre-execute command
        /// </summary>
        public void RaiseSFTPPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPPreExecuteCommand() and responding in RespondSFTPPreExecuteCommand()...");
            FiredEvents.Add("SFTPPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            SFTPPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of SFTP post-execute command
        /// </summary>
        public void RaiseSFTPPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPPostExecuteCommand() and responding in RespondSFTPPostExecuteCommand()...");
            FiredEvents.Add("SFTPPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            SFTPPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of SFTP command error
        /// </summary>
        public void RaiseSFTPCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPCommandError() and responding in RespondSFTPCommandError()...");
            FiredEvents.Add("SFTPCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            SFTPCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of SFTP pre-download
        /// </summary>
        public void RaiseSFTPPreDownload(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPPreDownload() and responding in RespondSFTPPreDownload()...");
            FiredEvents.Add("SFTPPreDownload (" + FiredEvents.Count.ToString() + ")", [File]);
            SFTPPreDownload?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of SFTP post-download
        /// </summary>
        public void RaiseSFTPPostDownload(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPPostDownload() and responding in RespondSFTPPostDownload()...");
            FiredEvents.Add("SFTPPostDownload (" + FiredEvents.Count.ToString() + ")", [File]);
            SFTPPostDownload?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of SFTP download error
        /// </summary>
        public void RaiseSFTPDownloadError(string File, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPDownloadError() and responding in RespondSFTPDownloadError()...");
            FiredEvents.Add("SFTPDownloadError (" + FiredEvents.Count.ToString() + ")", [File, Exception]);
            SFTPDownloadError?.Invoke(File, Exception);
        }
        /// <summary>
        /// Raise an event of SFTP pre-upload
        /// </summary>
        public void RaiseSFTPPreUpload(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPPreUpload() and responding in RespondSFTPPreUpload()...");
            FiredEvents.Add("SFTPPreUpload (" + FiredEvents.Count.ToString() + ")", [File]);
            SFTPPreUpload?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of SFTP post-upload
        /// </summary>
        public void RaiseSFTPPostUpload(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPPostUpload() and responding in RespondSFTPPostUpload()...");
            FiredEvents.Add("SFTPPostUpload (" + FiredEvents.Count.ToString() + ")", [File]);
            SFTPPostUpload?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of SFTP upload error
        /// </summary>
        public void RaiseSFTPUploadError(string File, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SFTPUploadError() and responding in RespondSFTPUploadError()...");
            FiredEvents.Add("SFTPUploadError (" + FiredEvents.Count.ToString() + ")", [File, Exception]);
            SFTPUploadError?.Invoke(File, Exception);
        }
        /// <summary>
        /// Raise an event of SSH being connected
        /// </summary>
        public void RaiseSSHConnected(string Target)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SSHConnected() and responding in RespondSSHConnected()...");
            FiredEvents.Add("SSHConnected (" + FiredEvents.Count.ToString() + ")", [Target]);
            SSHConnected?.Invoke(Target);
        }
        /// <summary>
        /// Raise an event of SSH being disconnected
        /// </summary>
        public void RaiseSSHDisconnected()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SSHDisconnected() and responding in RespondSSHDisconnected()...");
            FiredEvents.Add("SSHDisconnected (" + FiredEvents.Count.ToString() + ")", []);
            SSHDisconnected?.Invoke();
        }
        /// <summary>
        /// Raise an event of SSH pre-execute command
        /// </summary>
        public void RaiseSSHPreExecuteCommand(string Target, string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SSHPreExecuteCommand() and responding in RespondSSHPreExecuteCommand()...");
            FiredEvents.Add("SSHPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            SSHPreExecuteCommand?.Invoke(Target, Command);
        }
        /// <summary>
        /// Raise an event of SSH post-execute command
        /// </summary>
        public void RaiseSSHPostExecuteCommand(string Target, string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SSHPostExecuteCommand() and responding in RespondSSHPostExecuteCommand()...");
            FiredEvents.Add("SSHPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            SSHPostExecuteCommand?.Invoke(Target, Command);
        }
        /// <summary>
        /// Raise an event of SSH command error
        /// </summary>
        public void RaiseSSHCommandError(string Target, string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SSHCommandError() and responding in RespondSSHCommandError()...");
            FiredEvents.Add("SSHCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            SSHCommandError?.Invoke(Target, Command, Exception);
        }
        /// <summary>
        /// Raise an event of SSH error
        /// </summary>
        public void RaiseSSHError(Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event SSHError() and responding in RespondSSHError()...");
            FiredEvents.Add("SSHError (" + FiredEvents.Count.ToString() + ")", [Exception]);
            SSHError?.Invoke(Exception);
        }
        /// <summary>
        /// Raise an event of UESH pre-execute
        /// </summary>
        public void RaiseUESHPreExecute(string Command, string Arguments)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event UESHPreExecute() and responding in RespondUESHPreExecute()...");
            FiredEvents.Add("UESHPreExecute (" + FiredEvents.Count.ToString() + ")", [Command, Arguments]);
            UESHPreExecute?.Invoke(Command, Arguments);
        }
        /// <summary>
        /// Raise an event of UESH post-execute
        /// </summary>
        public void RaiseUESHPostExecute(string Command, string Arguments)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event UESHPostExecute() and responding in RespondUESHPostExecute()...");
            FiredEvents.Add("UESHPostExecute (" + FiredEvents.Count.ToString() + ")", [Command, Arguments]);
            UESHPostExecute?.Invoke(Command, Arguments);
        }
        /// <summary>
        /// Raise an event of UESH error
        /// </summary>
        public void RaiseUESHError(string Command, string Arguments, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event UESHError() and responding in RespondUESHError()...");
            FiredEvents.Add("UESHError (" + FiredEvents.Count.ToString() + ")", [Command, Arguments, Exception]);
            UESHError?.Invoke(Command, Arguments, Exception);
        }
        /// <summary>
        /// Raise an event of text shell initialized
        /// </summary>
        public void RaiseTextShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TextShellInitialized() and responding in RespondTextShellInitialized()...");
            FiredEvents.Add("TextShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            TextShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of text pre-command execution
        /// </summary>
        public void RaiseTextPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TextPreExecuteCommand() and responding in RespondTextPreExecuteCommand()...");
            FiredEvents.Add("TextPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            TextPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of text post-command execution
        /// </summary>
        public void RaiseTextPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TextPostExecuteCommand() and responding in RespondTextPostExecuteCommand()...");
            FiredEvents.Add("TextPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            TextPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of text command error
        /// </summary>
        public void RaiseTextCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TextCommandError() and responding in RespondTextCommandError()...");
            FiredEvents.Add("TextCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            TextCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of notification being sent
        /// </summary>
        public void RaiseNotificationSent(Notification Notification)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event NotificationSent() and responding in RespondNotificationSent()...");
            FiredEvents.Add("NotificationSent (" + FiredEvents.Count.ToString() + ")", [Notification]);
            NotificationSent?.Invoke(Notification);
        }
        /// <summary>
        /// Raise an event of notifications being sent
        /// </summary>
        public void RaiseNotificationsSent(List<Notification> Notifications)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event NotificationsSent() and responding in RespondNotificationsSent()...");
            FiredEvents.Add("NotificationsSent (" + FiredEvents.Count.ToString() + ")", [Notifications]);
            NotificationsSent?.Invoke(Notifications);
        }
        /// <summary>
        /// Raise an event of notification being received
        /// </summary>
        public void RaiseNotificationReceived(Notification Notification)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event NotificationReceived() and responding in RespondNotificationReceived()...");
            FiredEvents.Add("NotificationReceived (" + FiredEvents.Count.ToString() + ")", [Notification]);
            NotificationReceived?.Invoke(Notification);
        }
        /// <summary>
        /// Raise an event of notifications being received
        /// </summary>
        public void RaiseNotificationsReceived(List<Notification> Notifications)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event NotificationsReceived() and responding in RespondNotificationsReceived()...");
            FiredEvents.Add("NotificationsReceived (" + FiredEvents.Count.ToString() + ")", [Notifications]);
            NotificationsReceived?.Invoke(Notifications);
        }
        /// <summary>
        /// Raise an event of notification being dismissed
        /// </summary>
        public void RaiseNotificationDismissed()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event NotificationDismissed() and responding in RespondNotificationDismissed()...");
            FiredEvents.Add("NotificationDismissed (" + FiredEvents.Count.ToString() + ")", []);
            NotificationDismissed?.Invoke();
        }
        /// <summary>
        /// Raise an event of config being saved
        /// </summary>
        public void RaiseConfigSaved()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ConfigSaved() and responding in RespondConfigSaved()...");
            FiredEvents.Add("ConfigSaved (" + FiredEvents.Count.ToString() + ")", []);
            ConfigSaved?.Invoke();
        }
        /// <summary>
        /// Raise an event of config having problems saving
        /// </summary>
        public void RaiseConfigSaveError(Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ConfigSaveError() and responding in RespondConfigSaveError()...");
            FiredEvents.Add("ConfigSaveError (" + FiredEvents.Count.ToString() + ")", [Exception]);
            ConfigSaveError?.Invoke(Exception);
        }
        /// <summary>
        /// Raise an event of config being read
        /// </summary>
        public void RaiseConfigRead()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ConfigRead() and responding in RespondConfigRead()...");
            FiredEvents.Add("ConfigRead (" + FiredEvents.Count.ToString() + ")", []);
            ConfigRead?.Invoke();
        }
        /// <summary>
        /// Raise an event of config having problems reading
        /// </summary>
        public void RaiseConfigReadError(Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ConfigReadError() and responding in RespondConfigReadError()...");
            FiredEvents.Add("ConfigReadError (" + FiredEvents.Count.ToString() + ")", [Exception]);
            ConfigReadError?.Invoke(Exception);
        }
        /// <summary>
        /// Raise an event of mod command pre-execution
        /// </summary>
        public void RaisePreExecuteModCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PreExecuteModCommand() and responding in RespondPreExecuteModCommand()...");
            FiredEvents.Add("PreExecuteModCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            PreExecuteModCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of mod command post-execution
        /// </summary>
        public void RaisePostExecuteModCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event PostExecuteModCommand() and responding in RespondPostExecuteModCommand()...");
            FiredEvents.Add("PostExecuteModCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            PostExecuteModCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of mod being parsed
        /// </summary>
        public void RaiseModParsed(string ModFileName)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ModParsed() and responding in RespondModParsed()...");
            FiredEvents.Add("ModParsed (" + FiredEvents.Count.ToString() + ")", [ModFileName]);
            ModParsed?.Invoke(ModFileName);
        }
        /// <summary>
        /// Raise an event of mod having problems parsing
        /// </summary>
        public void RaiseModParseError(string ModFileName)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ModParseError() and responding in RespondModParseError()...");
            FiredEvents.Add("ModParseError (" + FiredEvents.Count.ToString() + ")", [ModFileName]);
            ModParseError?.Invoke(ModFileName);
        }
        /// <summary>
        /// Raise an event of mod being finalized
        /// </summary>
        public void RaiseModFinalized(string ModFileName)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ModFinalized() and responding in RespondModFinalized()...");
            FiredEvents.Add("ModFinalized (" + FiredEvents.Count.ToString() + ")", [ModFileName]);
            ModFinalized?.Invoke(ModFileName);
        }
        /// <summary>
        /// Raise an event of mod having problems finalizing
        /// </summary>
        public void RaiseModFinalizationFailed(string ModFileName, string Reason)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ModFinalizationFailed() and responding in RespondModFinalizationFailed()...");
            FiredEvents.Add("ModFinalizationFailed (" + FiredEvents.Count.ToString() + ")", [ModFileName, Reason]);
            ModFinalizationFailed?.Invoke(ModFileName, Reason);
        }
        /// <summary>
        /// Raise an event of user being added
        /// </summary>
        public void RaiseUserAdded(string Username)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event UserAdded() and responding in RespondUserAdded()...");
            FiredEvents.Add("UserAdded (" + FiredEvents.Count.ToString() + ")", [Username]);
            UserAdded?.Invoke(Username);
        }
        /// <summary>
        /// Raise an event of user being removed
        /// </summary>
        public void RaiseUserRemoved(string Username)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event UserRemoved() and responding in RespondUserRemoved()...");
            FiredEvents.Add("UserRemoved (" + FiredEvents.Count.ToString() + ")", [Username]);
            UserRemoved?.Invoke(Username);
        }
        /// <summary>
        /// Raise an event of username being changed
        /// </summary>
        public void RaiseUsernameChanged(string OldUsername, string NewUsername)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event UsernameChanged() and responding in RespondUsernameChanged()...");
            FiredEvents.Add("UsernameChanged (" + FiredEvents.Count.ToString() + ")", [OldUsername, NewUsername]);
            UsernameChanged?.Invoke(OldUsername, NewUsername);
        }
        /// <summary>
        /// Raise an event of user password being changed
        /// </summary>
        public void RaiseUserPasswordChanged(string Username)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event UserPasswordChanged() and responding in RespondUserPasswordChanged()...");
            FiredEvents.Add("UserPasswordChanged (" + FiredEvents.Count.ToString() + ")", [Username]);
            UserPasswordChanged?.Invoke(Username);
        }
        /// <summary>
        /// Raise an event of hardware probing
        /// </summary>
        public void RaiseHardwareProbing()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HardwareProbing() and responding in RespondHardwareProbing()...");
            FiredEvents.Add("HardwareProbing (" + FiredEvents.Count.ToString() + ")", []);
            HardwareProbing?.Invoke();
        }
        /// <summary>
        /// Raise an event of hardware being probed
        /// </summary>
        public void RaiseHardwareProbed()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HardwareProbed() and responding in RespondHardwareProbed()...");
            FiredEvents.Add("HardwareProbed (" + FiredEvents.Count.ToString() + ")", []);
            HardwareProbed?.Invoke();
        }
        /// <summary>
        /// Raise an event of current directory being changed
        /// </summary>
        public void RaiseCurrentDirectoryChanged()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event CurrentDirectoryChanged() and responding in RespondCurrentDirectoryChanged()...");
            FiredEvents.Add("CurrentDirectoryChanged (" + FiredEvents.Count.ToString() + ")", []);
            CurrentDirectoryChanged?.Invoke();
        }
        /// <summary>
        /// Raise an event of file creation
        /// </summary>
        public void RaiseFileCreated(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FileCreated() and responding in RespondFileCreated()...");
            FiredEvents.Add("FileCreated (" + FiredEvents.Count.ToString() + ")", [File]);
            FileCreated?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of directory creation
        /// </summary>
        public void RaiseDirectoryCreated(string Directory)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event DirectoryCreated() and responding in RespondDirectoryCreated()...");
            FiredEvents.Add("DirectoryCreated (" + FiredEvents.Count.ToString() + ")", [Directory]);
            DirectoryCreated?.Invoke(Directory);
        }
        /// <summary>
        /// Raise an event of file copying process
        /// </summary>
        public void RaiseFileCopied(string Source, string Destination)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FileCopied() and responding in RespondFileCopied()...");
            FiredEvents.Add("FileCopied (" + FiredEvents.Count.ToString() + ")", [Source, Destination]);
            FileCopied?.Invoke(Source, Destination);
        }
        /// <summary>
        /// Raise an event of directory copying process
        /// </summary>
        public void RaiseDirectoryCopied(string Source, string Destination)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event DirectoryCopied() and responding in RespondDirectoryCopied()...");
            FiredEvents.Add("DirectoryCopied (" + FiredEvents.Count.ToString() + ")", [Source, Destination]);
            DirectoryCopied?.Invoke(Source, Destination);
        }
        /// <summary>
        /// Raise an event of file moving process
        /// </summary>
        public void RaiseFileMoved(string Source, string Destination)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FileMoved() and responding in RespondFileMoved()...");
            FiredEvents.Add("FileMoved (" + FiredEvents.Count.ToString() + ")", [Source, Destination]);
            FileMoved?.Invoke(Source, Destination);
        }
        /// <summary>
        /// Raise an event of directory moving process
        /// </summary>
        public void RaiseDirectoryMoved(string Source, string Destination)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event DirectoryMoved() and responding in RespondDirectoryMoved()...");
            FiredEvents.Add("DirectoryMoved (" + FiredEvents.Count.ToString() + ")", [Source, Destination]);
            DirectoryMoved?.Invoke(Source, Destination);
        }
        /// <summary>
        /// Raise an event of file removal
        /// </summary>
        public void RaiseFileRemoved(string File)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FileRemoved() and responding in RespondFileRemoved()...");
            FiredEvents.Add("FileRemoved (" + FiredEvents.Count.ToString() + ")", [File]);
            FileRemoved?.Invoke(File);
        }
        /// <summary>
        /// Raise an event of directory removal
        /// </summary>
        public void RaiseDirectoryRemoved(string Directory)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event DirectoryRemoved() and responding in RespondDirectoryRemoved()...");
            FiredEvents.Add("DirectoryRemoved (" + FiredEvents.Count.ToString() + ")", [Directory]);
            DirectoryRemoved?.Invoke(Directory);
        }
        /// <summary>
        /// Raise an event of file attribute addition
        /// </summary>
        public void RaiseFileAttributeAdded(string File, FileAttributes Attributes)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FileAttributeAdded() and responding in RespondFileAttributeAdded()...");
            FiredEvents.Add("FileAttributeAdded (" + FiredEvents.Count.ToString() + ")", [File, Attributes]);
            FileAttributeAdded?.Invoke(File, Attributes);
        }
        /// <summary>
        /// Raise an event of file attribute removal
        /// </summary>
        public void RaiseFileAttributeRemoved(string File, FileAttributes Attributes)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event FileAttributeRemoved() and responding in RespondFileAttributeRemoved()...");
            FiredEvents.Add("FileAttributeRemoved (" + FiredEvents.Count.ToString() + ")", [File, Attributes]);
            FileAttributeRemoved?.Invoke(File, Attributes);
        }
        /// <summary>
        /// Raise an event of console colors being reset
        /// </summary>
        public void RaiseColorReset()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ColorReset() and responding in RespondColorReset()...");
            FiredEvents.Add("ColorReset (" + FiredEvents.Count.ToString() + ")", []);
            ColorReset?.Invoke();
        }
        /// <summary>
        /// Raise an event of theme setting
        /// </summary>
        public void RaiseThemeSet(string Theme)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ThemeSet() and responding in RespondThemeSet()...");
            FiredEvents.Add("ThemeSet (" + FiredEvents.Count.ToString() + ")", [Theme]);
            ThemeSet?.Invoke(Theme);
        }
        /// <summary>
        /// Raise an event of theme setting problem
        /// </summary>
        public void RaiseThemeSetError(string Theme, ThemeSetErrorReasons Reason)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ThemeSetError() and responding in RespondThemeSetError()...");
            FiredEvents.Add("ThemeSetError (" + FiredEvents.Count.ToString() + ")", [Theme, Reason]);
            ThemeSetError?.Invoke(Theme, Reason);
        }
        /// <summary>
        /// Raise an event of console colors being set
        /// </summary>
        public void RaiseColorSet()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ColorSet() and responding in RespondColorSet()...");
            FiredEvents.Add("ColorSet (" + FiredEvents.Count.ToString() + ")", []);
            ColorSet?.Invoke();
        }
        /// <summary>
        /// Raise an event of console colors having problems being set
        /// </summary>
        public void RaiseColorSetError(ColorSetErrorReasons Reason)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ColorSetError() and responding in RespondColorSetError()...");
            FiredEvents.Add("ColorSetError (" + FiredEvents.Count.ToString() + ")", [Reason]);
            ColorSetError?.Invoke(Reason);
        }
        /// <summary>
        /// Raise an event of theme studio start
        /// </summary>
        public void RaiseThemeStudioStarted()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ThemeStudioStarted() and responding in RespondThemeStudioStarted()...");
            FiredEvents.Add("ThemeStudioStarted (" + FiredEvents.Count.ToString() + ")", []);
            ThemeStudioStarted?.Invoke();
        }
        /// <summary>
        /// Raise an event of theme studio exit
        /// </summary>
        public void RaiseThemeStudioExit()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ThemeStudioExit() and responding in RespondThemeStudioExit()...");
            FiredEvents.Add("ThemeStudioExit (" + FiredEvents.Count.ToString() + ")", []);
            ThemeStudioExit?.Invoke();
        }
        /// <summary>
        /// Raise an event of arguments being injected
        /// </summary>
        public void RaiseArgumentsInjected(List<string> InjectedArguments)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ArgumentsInjected() and responding in RespondArgumentsInjected()...");
            FiredEvents.Add("ArgumentsInjected (" + FiredEvents.Count.ToString() + ")", [InjectedArguments]);
            ArgumentsInjected?.Invoke(InjectedArguments);
        }
        /// <summary>
        /// Raise an event of ZIP shell initialized
        /// </summary>
        public void RaiseZipShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ZipShellInitialized() and responding in RespondZipShellInitialized()...");
            FiredEvents.Add("ZipShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            ZipShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of ZIP pre-command execution
        /// </summary>
        public void RaiseZipPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ZipPreExecuteCommand() and responding in RespondZipPreExecuteCommand()...");
            FiredEvents.Add("ZipPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            ZipPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of ZIP post-command execution
        /// </summary>
        public void RaiseZipPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ZipPostExecuteCommand() and responding in RespondZipPostExecuteCommand()...");
            FiredEvents.Add("ZipPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            ZipPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of ZIP command error
        /// </summary>
        public void RaiseZipCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ZipCommandError() and responding in RespondZipCommandError()...");
            FiredEvents.Add("ZipCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            ZipCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of HTTP shell initialized
        /// </summary>
        public void RaiseHTTPShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HTTPShellInitialized() and responding in RespondHTTPShellInitialized()...");
            FiredEvents.Add("HTTPShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            HTTPShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of HTTP pre-execute command
        /// </summary>
        public void RaiseHTTPPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HTTPPreExecuteCommand() and responding in RespondHTTPPreExecuteCommand()...");
            FiredEvents.Add("HTTPPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            HTTPPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of HTTP post-execute command
        /// </summary>
        public void RaiseHTTPPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HTTPPostExecuteCommand() and responding in RespondHTTPPostExecuteCommand()...");
            FiredEvents.Add("HTTPPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            HTTPPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of HTTP command error
        /// </summary>
        public void RaiseHTTPCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HTTPCommandError() and responding in RespondHTTPCommandError()...");
            FiredEvents.Add("HTTPCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            HTTPCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of process error
        /// </summary>
        public void RaiseProcessError(string Process, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event ProcessError() and responding in RespondProcessError()...");
            FiredEvents.Add("ProcessError (" + FiredEvents.Count.ToString() + ")", [Process, Exception]);
            ProcessError?.Invoke(Process, Exception);
        }
        /// <summary>
        /// Raise an event of a custom language installation
        /// </summary>
        public void RaiseLanguageInstalled(string Language)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguageInstalled() and responding in RespondLanguageInstalled()...");
            FiredEvents.Add("LanguageInstalled (" + FiredEvents.Count.ToString() + ")", []);
            LanguageInstalled?.Invoke(Language);
        }
        /// <summary>
        /// Raise an event of a custom language uninstallation
        /// </summary>
        public void RaiseLanguageUninstalled(string Language)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguageUninstalled() and responding in RespondLanguageUninstalled()...");
            FiredEvents.Add("LanguageUninstalled (" + FiredEvents.Count.ToString() + ")", []);
            LanguageUninstalled?.Invoke(Language);
        }
        /// <summary>
        /// Raise an event of custom language install error
        /// </summary>
        public void RaiseLanguageInstallError(string Language, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguageInstallError() and responding in RespondLanguageInstallError()...");
            FiredEvents.Add("LanguageInstallError (" + FiredEvents.Count.ToString() + ")", []);
            LanguageInstallError?.Invoke(Language, Exception);
        }
        /// <summary>
        /// Raise an event of custom language uninstall error
        /// </summary>
        public void RaiseLanguageUninstallError(string Language, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguageUninstallError() and responding in RespondLanguageUninstallError()...");
            FiredEvents.Add("LanguageUninstallError (" + FiredEvents.Count.ToString() + ")", []);
            LanguageUninstallError?.Invoke(Language, Exception);
        }
        /// <summary>
        /// Raise an event of custom languages installation
        /// </summary>
        public void RaiseLanguagesInstalled()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguagesInstalled() and responding in RespondLanguagesInstalled()...");
            FiredEvents.Add("LanguagesInstalled (" + FiredEvents.Count.ToString() + ")", []);
            LanguagesInstalled?.Invoke();
        }
        /// <summary>
        /// Raise an event of custom languages uninstallation
        /// </summary>
        public void RaiseLanguagesUninstalled()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguagesUninstalled() and responding in RespondLanguagesUninstalled()...");
            FiredEvents.Add("LanguagesUninstalled (" + FiredEvents.Count.ToString() + ")", []);
            LanguagesUninstalled?.Invoke();
        }
        /// <summary>
        /// Raise an event of custom languages install error
        /// </summary>
        public void RaiseLanguagesInstallError(Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguagesInstallError() and responding in RespondLanguagesInstallError()...");
            FiredEvents.Add("LanguagesInstallError (" + FiredEvents.Count.ToString() + ")", []);
            LanguagesInstallError?.Invoke(Exception);
        }
        /// <summary>
        /// Raise an event of custom languages uninstall error
        /// </summary>
        public void RaiseLanguagesUninstallError(Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event LanguagesUninstallError() and responding in RespondLanguagesUninstallError()...");
            FiredEvents.Add("LanguagesUninstallError (" + FiredEvents.Count.ToString() + ")", []);
            LanguagesUninstallError?.Invoke(Exception);
        }
        /// <summary>
        /// Raise an event of Hex shell initialized
        /// </summary>
        public void RaiseHexShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HexShellInitialized() and responding in RespondHexShellInitialized()...");
            FiredEvents.Add("HexShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            HexShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of Hex pre-command execution
        /// </summary>
        public void RaiseHexPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HexPreExecuteCommand() and responding in RespondHexPreExecuteCommand()...");
            FiredEvents.Add("HexPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            HexPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of Hex post-command execution
        /// </summary>
        public void RaiseHexPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HexPostExecuteCommand() and responding in RespondHexPostExecuteCommand()...");
            FiredEvents.Add("HexPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            HexPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of Hex command error
        /// </summary>
        public void RaiseHexCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event HexCommandError() and responding in RespondHexCommandError()...");
            FiredEvents.Add("HexCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            HexCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of Json shell initialized
        /// </summary>
        public void RaiseJsonShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event JsonShellInitialized() and responding in RespondJsonShellInitialized()...");
            FiredEvents.Add("JsonShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            JsonShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of Json pre-command execution
        /// </summary>
        public void RaiseJsonPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event JsonPreExecuteCommand() and responding in RespondJsonPreExecuteCommand()...");
            FiredEvents.Add("JsonPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            JsonPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of Json post-command execution
        /// </summary>
        public void RaiseJsonPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event JsonPostExecuteCommand() and responding in RespondJsonPostExecuteCommand()...");
            FiredEvents.Add("JsonPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            JsonPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of Json command error
        /// </summary>
        public void RaiseJsonCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event JsonCommandError() and responding in RespondJsonCommandError()...");
            FiredEvents.Add("JsonCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            JsonCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of Test shell initialized
        /// </summary>
        public void RaiseTestShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TestShellInitialized() and responding in RespondTestShellInitialized()...");
            FiredEvents.Add("TestShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            TestShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of Test pre-command execution
        /// </summary>
        public void RaiseTestPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TestPreExecuteCommand() and responding in RespondTestPreExecuteCommand()...");
            FiredEvents.Add("TestPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            TestPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of Test post-command execution
        /// </summary>
        public void RaiseTestPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TestPostExecuteCommand() and responding in RespondTestPostExecuteCommand()...");
            FiredEvents.Add("TestPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            TestPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of Test command error
        /// </summary>
        public void RaiseTestCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event TestCommandError() and responding in RespondTestCommandError()...");
            FiredEvents.Add("TestCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            TestCommandError?.Invoke(Command, Exception);
        }
        /// <summary>
        /// Raise an event of ZIP shell initialized
        /// </summary>
        public void RaiseRarShellInitialized()
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RarShellInitialized() and responding in RespondRarShellInitialized()...");
            FiredEvents.Add("RarShellInitialized (" + FiredEvents.Count.ToString() + ")", []);
            RarShellInitialized?.Invoke();
        }
        /// <summary>
        /// Raise an event of ZIP pre-command execution
        /// </summary>
        public void RaiseRarPreExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RarPreExecuteCommand() and responding in RespondRarPreExecuteCommand()...");
            FiredEvents.Add("RarPreExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            RarPreExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of ZIP post-command execution
        /// </summary>
        public void RaiseRarPostExecuteCommand(string Command)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RarPostExecuteCommand() and responding in RespondRarPostExecuteCommand()...");
            FiredEvents.Add("RarPostExecuteCommand (" + FiredEvents.Count.ToString() + ")", [Command]);
            RarPostExecuteCommand?.Invoke(Command);
        }
        /// <summary>
        /// Raise an event of ZIP command error
        /// </summary>
        public void RaiseRarCommandError(string Command, Exception Exception)
        {
            DebugWriter.WdbgConditional(ref Flags.EventDebug, DebugLevel.I, "Raising event RarCommandError() and responding in RespondRarCommandError()...");
            FiredEvents.Add("RarCommandError (" + FiredEvents.Count.ToString() + ")", [Command, Exception]);
            RarCommandError?.Invoke(Command, Exception);
        }

    }
}