
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers.MiscWriters;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Debugging.Trace;
using KS.Kernel.Extensions;
using KS.Kernel.Hardware;
using KS.Kernel.Journaling;
using KS.Kernel.Power;
using KS.Kernel.Threading;
using KS.Kernel.Time.Renderers;
using KS.Languages;
using KS.Misc.Calendar.Events;
using KS.Misc.Calendar.Reminders;
using KS.Misc.Contacts;
using KS.Misc.Notifications;
using KS.Misc.Probers.Motd;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Modifications;
using KS.Network.Base.Connections;
using KS.Network.RPC;
using KS.Network.SpeedDial;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Scripting;
using KS.Shell.ShellBase.Shells;
using System;
using System.IO;

namespace KS.Kernel.Starting
{
    internal static class KernelInitializers
    {
        internal static void InitializeCritical()
        {
            // Check for terminal
            ConsoleChecker.CheckConsole();

            // Initialize crucial things
            if (!KernelPlatform.IsOnUnix())
            {
                if (!ConsoleExtensions.InitializeSequences())
                {
                    TextWriterColor.Write("Can not initialize VT sequences for your Windows terminal. Make sure that you're running Windows 10 or later.");
                    Input.DetectKeypress();
                }
            }

            // Load the assembly resolver
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyLookup.LoadFromAssemblySearchPaths;

            // Check to see if we have an appdata folder for KS
            if (!Checking.FolderExists(Paths.AppDataPath))
                Making.MakeDirectory(Paths.AppDataPath, false);

            // Set the first time run variable
            if (!Checking.FileExists(Paths.ConfigurationPath))
                Flags.FirstTime = true;

            // Initialize debug path
            DebugWriter.DebugPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Debugging)), Paths.GetKernelPath(KernelPathType.Debugging));

            // Power signal handlers
            if (!PowerSignalHandlers.initialized)
            {
                PowerSignalHandlers.initialized = true;
                PowerSignalHandlers.RegisterHandlers();
            }
        }

        internal static void InitializeEssential()
        {
            // Load alternative buffer (only supported on Linux, because Windows doesn't seem to respect CursorVisible = false on alt buffers)
            if (!KernelPlatform.IsOnWindows() && Flags.UseAltBuffer)
            {
                TextWriterColor.Write("\u001b[?1049h");
                ConsoleWrapper.SetCursorPosition(0, 0);
                ConsoleWrapper.CursorVisible = false;
                Flags.HasSetAltBuffer = true;
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded alternative buffer.");
            }

            // A title
            ConsoleExtensions.SetTitle(KernelTools.ConsoleTitle);

            // Set the buffer size
            if (Flags.SetBufferSize)
                ConsoleExtensions.SetBufferSize();

            // Initialize console wrappers for TermRead
            Input.InitializeTerminauxWrappers();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded input wrappers.");

            // Show initializing
            if (Flags.TalkativePreboot)
            {
                TextWriterColor.Write(Translate.DoTranslation("Welcome!"));
                TextWriterColor.Write(Translate.DoTranslation("Starting Nitrocid..."));
            }

            // Initialize journal path
            JournalManager.JournalPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Journalling)), Paths.GetKernelPath(KernelPathType.Journalling));

            // Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
            if (Flags.TalkativePreboot)
                TextWriterColor.Write(Translate.DoTranslation("Downloading debug symbols..."));
            DebugSymbolsTools.CheckDebugSymbols();

            // Initialize custom languages
            if (Flags.TalkativePreboot)
                TextWriterColor.Write(Translate.DoTranslation("Loading custom languages..."));
            LanguageManager.InstallCustomLanguages();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded custom languages.");

            // Initialize splashes
            if (Flags.TalkativePreboot)
                TextWriterColor.Write(Translate.DoTranslation("Loading custom splashes..."));
            SplashManager.LoadSplashes();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded custom splashes.");

            // Initialize addons
            if (Flags.TalkativePreboot)
                TextWriterColor.Write(Translate.DoTranslation("Loading kernel addons..."));
            AddonTools.ProcessAddons(AddonType.Important);
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded kernel addons.");

            // Create config file and then read it
            if (Flags.TalkativePreboot)
                TextWriterColor.Write(Translate.DoTranslation("Loading configuration..."));
            if (!Flags.SafeMode)
                Config.InitializeConfig();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded configuration.");

            // Load background
            KernelColorTools.LoadBack();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded background.");

            // Load splash
            SplashManager.OpenSplash();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded splash.");

            // Populate debug devices
            RemoteDebugTools.LoadAllDevices();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded remote debug devices.");
        }

        internal static void InitializeWelcomeMessages()
        {
            // Show welcome message.
            WelcomeMessage.WriteMessage();

            // Some information
            if (Flags.ShowAppInfoOnBoot & !Flags.EnableSplash)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Kernel environment information"), true, KernelColorType.Stage);
                TextWriterColor.Write("  OS: " + Translate.DoTranslation("Running on {0}"), System.Environment.OSVersion.ToString());
                TextWriterColor.Write("  KS: " + Translate.DoTranslation("Running from GRILO?") + $" {KernelPlatform.IsRunningFromGrilo()}");
                TextWriterColor.Write("  KSAPI: " + $"v{KernelTools.KernelApiVersion}");
            }
        }

        internal static void InitializeOptional()
        {
            // Initialize notifications
            if (!NotificationManager.NotifThread.IsAlive)
                NotificationManager.NotifThread.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded notification thread.");

            // Install cancellation handler
            CancellationHandlers.InstallHandler();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded cancellation handler.");

            // Initialize aliases
            AliasManager.InitAliases();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded aliases.");

            // Initialize speed dial
            SpeedDialTools.LoadAll();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded speed dial entries.");

            // Initialize top right date
            TimeDateTopRight.InitTopRightDate();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded top right date.");

            // Start screensaver timeout
            if (!ScreensaverManager.Timeout.IsAlive)
                ScreensaverManager.Timeout.Start();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded screensaver timeout.");

            // Initialize events and reminders
            if (!ReminderManager.ReminderThread.IsAlive)
                ReminderManager.ReminderThread.Start();
            if (!EventManager.EventThread.IsAlive)
                EventManager.EventThread.Start();
            EventManager.LoadEvents();
            ReminderManager.LoadReminders();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded events & reminders.");

            // Load system env vars and convert them
            UESHVariables.ConvertSystemEnvironmentVariables();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded environment variables.");

            // Finalize addons
            AddonTools.ProcessAddons(AddonType.Optional);
            AddonTools.FinalizeAddons();
            DebugWriter.WriteDebug(DebugLevel.I, "Finalized addons.");

            // If the two files are not found, create two MOTD files with current config.
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MOTD)))
                MotdParse.SetMotd(Translate.DoTranslation("Welcome to Nitrocid Kernel!"));
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MAL)))
                MalParse.SetMal(Translate.DoTranslation("Welcome to Nitrocid Kernel") + ", <user>!");

            // Load MOTD and MAL
            MotdParse.ReadMotd();
            MalParse.ReadMal();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded MOTD and MAL.");

            // Load shell command histories
            ShellManager.LoadHistories();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded shell command histories.");
        }

        internal static void ResetEverything()
        {
            // Reset every variable below
            ReminderManager.Reminders.Clear();
            EventManager.CalendarEvents.Clear();
            Flags.SafeMode = false;
            Flags.QuietKernel = false;
            Flags.Maintenance = false;
            Flags.HasSetAltBuffer = false;
            SplashReport._Progress = 0;
            SplashReport._ProgressText = "";
            SplashReport._KernelBooted = false;
            DebugWriter.WriteDebug(DebugLevel.I, "General variables reset");

            // Save shell command histories
            ShellManager.SaveHistories();
            DebugWriter.WriteDebug(DebugLevel.I, "Saved shell command histories.");

            // Reset hardware info
            HardwareProbe.HardwareInfo = null;
            DebugWriter.WriteDebug(DebugLevel.I, "Hardware info reset.");

            // Disconnect all hosts from remote debugger
            RemoteDebugger.StopRDebugThread();
            DebugWriter.WriteDebug(DebugLevel.I, "Remote debugger stopped");

            // Stop all mods
            ModManager.StopMods();
            DebugWriter.WriteDebug(DebugLevel.I, "Mods stopped");

            // Stop all addons
            AddonTools.UnloadAddons();
            DebugWriter.WriteDebug(DebugLevel.I, "Addons stopped");

            // Stop RPC
            RemoteProcedure.StopRPC();
            DebugWriter.WriteDebug(DebugLevel.I, "RPC stopped");

            // Disconnect all connections
            NetworkConnectionTools.CloseAllConnections();
            DebugWriter.WriteDebug(DebugLevel.I, "Closed all connections");

            // Unload all splashes
            SplashManager.UnloadSplashes();
            DebugWriter.WriteDebug(DebugLevel.I, "Unloaded all splashes");

            // Disable safe mode
            Flags.SafeMode = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Safe mode disabled");

            // Unload all contacts
            ContactsManager.RemoveContacts(false);
            DebugWriter.WriteDebug(DebugLevel.I, "Unloaded all contacts");

            // Stop the time/date change thread
            TimeDateTopRight.TimeTopRightChange.Stop();
            DebugWriter.WriteDebug(DebugLevel.I, "Time/date corner stopped");

            // Disable Debugger
            if (Flags.DebugMode)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Shutting down debugger");
                Flags.DebugMode = false;
                DebugWriter.DebugStreamWriter.Close();
                DebugWriter.DebugStreamWriter.Dispose();
                DebugWriter.isDisposed = true;
            }

            // Reset the buffer size
            ConsoleExtensions.RestoreBufferSize();

            // Clear all active threads as we're rebooting
            ThreadManager.StopAllThreads();
            PowerManager.Uptime.Reset();

            // Reset power state
            Flags.RebootRequested = false;
            Flags.LogoutRequested = false;
        }
    }
}
