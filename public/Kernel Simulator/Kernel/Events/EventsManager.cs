
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

using KS.Kernel.Debugging;
using KS.Languages;
using KS.Modifications;
using KS.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KS.Kernel.Events
{
    /// <summary>
    /// Kernel event management module
    /// </summary>
    public static class EventsManager
    {

        private static readonly string[] events = 
        {
            "StartKernel",
            "KernelStarted",
            "PreLogin",
            "PostLogin",
            "LoginError",
            "ShellInitialized",
            "PreExecuteCommand",
            "PostExecuteCommand",
            "KernelError",
            "ContKernelError",
            "PreShutdown",
            "PostShutdown",
            "PreReboot",
            "PostReboot",
            "PreShowScreensaver",
            "PostShowScreensaver",
            "PreUnlock",
            "PostUnlock",
            "CommandError",
            "PreReloadConfig",
            "PostReloadConfig",
            "PlaceholderParsing",
            "PlaceholderParsed",
            "PlaceholderParseError",
            "GarbageCollected",
            "FTPPreDownload",
            "FTPPostDownload",
            "FTPPreUpload",
            "FTPPostUpload",
            "RemoteDebugConnectionAccepted",
            "RemoteDebugConnectionDisconnected",
            "RemoteDebugExecuteCommand",
            "RemoteDebugCommandError",
            "RPCCommandSent",
            "RPCCommandReceived",
            "RPCCommandError",
            "SFTPPreDownload",
            "SFTPPostDownload",
            "SFTPDownloadError",
            "SFTPPreUpload",
            "SFTPPostUpload",
            "SFTPUploadError",
            "SSHConnected",
            "SSHDisconnected",
            "SSHPreExecuteCommand",
            "SSHPostExecuteCommand",
            "SSHCommandError",
            "SSHError",
            "UESHPreExecute",
            "UESHPostExecute",
            "UESHError",
            "NotificationSent",
            "NotificationsSent",
            "NotificationReceived",
            "NotificationsReceived",
            "NotificationDismissed",
            "ConfigSaved",
            "ConfigSaveError",
            "ConfigRead",
            "ConfigReadError",
            "PreExecuteModCommand",
            "PostExecuteModCommand",
            "ModParsed",
            "ModParseError",
            "ModFinalized",
            "ModFinalizationFailed",
            "UserAdded",
            "UserRemoved",
            "UsernameChanged",
            "UserPasswordChanged",
            "HardwareProbing",
            "HardwareProbed",
            "CurrentDirectoryChanged",
            "FileCreated",
            "DirectoryCreated",
            "FileCopied",
            "DirectoryCopied",
            "FileMoved",
            "DirectoryMoved",
            "FileRemoved",
            "DirectoryRemoved",
            "FileAttributeAdded",
            "FileAttributeRemoved",
            "ColorReset",
            "ThemeSet",
            "ThemeSetError",
            "ColorSet",
            "ColorSetError",
            "ThemeStudioStarted",
            "ThemeStudioExit",
            "ArgumentsInjected",
            "ProcessError",
            "LanguageInstalled",
            "LanguageUninstalled",
            "LanguageInstallError",
            "LanguageUninstallError",
            "LanguagesInstalled",
            "LanguagesUninstalled",
            "LanguagesInstallError",
            "LanguagesUninstallError",
            "ResizeDetected"
        };

        /// <summary>
        /// Recently fired events
        /// </summary>
        internal static Dictionary<string, object[]> FiredEvents { get; set; } = new Dictionary<string, object[]>();

        /// <summary>
        /// Lists all the fired events with arguments
        /// </summary>
        public static Dictionary<string, object[]> ListAllFiredEvents() => ListAllFiredEvents("");

        /// <summary>
        /// Lists all the fired events with arguments
        /// </summary>
        /// <param name="SearchTerm">The search term</param>
        public static Dictionary<string, object[]> ListAllFiredEvents(string SearchTerm)
        {
            var Events = new Dictionary<string, object[]>();

            // Enumerate all the fired events
            foreach (string FiredEvent in FiredEvents.Keys)
            {
                if (FiredEvent.Contains(SearchTerm))
                {
                    var EventArguments = FiredEvents[FiredEvent];
                    Events.Add(FiredEvent, EventArguments);
                }
            }
            return Events;
        }

        /// <summary>
        /// Clears all the fired events
        /// </summary>
        public static void ClearAllFiredEvents() => FiredEvents.Clear();

        /// <summary>
        /// Fires a kernel event
        /// </summary>
        /// <param name="EventName">Event name. Refer to <see cref="events"/>.</param>
        /// <param name="Params">Parameters for event</param>
        /// <exception cref="Exceptions.NoSuchEventException"></exception>
        public static void FireEvent(string EventName, params object[] Params)
        {
            // Check to see if event exists
            if (!events.Contains(EventName))
                throw new Exceptions.NoSuchEventException(Translate.DoTranslation("Event {0} not found."), EventName);

            // Add fired event to the list
            DebugWriter.WriteDebugConditional(ref Flags.EventDebug, DebugLevel.I, $"Raising event {EventName}...");
            FiredEvents.Add($"[{FiredEvents.Count}] {EventName}", Params);

            // Now, respond to the event
            foreach (ModInfo ModPart in ModManager.Mods.Values)
            {
                foreach (PartInfo PartInfo in ModPart.ModParts.Values)
                {
                    try
                    {
                        var script = PartInfo.PartScript;
                        DebugWriter.WriteDebugConditional(ref Flags.EventDebug, DebugLevel.I, "{0} in mod {1} v{2} responded to event {3}...", script.ModPart, script.Name, script.Version, EventName);
                        script.InitEvents(EventName, Params);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebugConditional(ref Flags.EventDebug, DebugLevel.E, "Error in event handler: {0}", ex.Message);
                        DebugWriter.WriteDebugStackTraceConditional(ref Flags.EventDebug, ex);
                    }
                }
            }
        }

    }
}
