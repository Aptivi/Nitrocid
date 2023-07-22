
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

using System;
using System.Diagnostics;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Hardware;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Power;
using KS.Languages;
using KS.Misc.Calendar.Events;
using KS.Misc.Calendar.Reminders;
using KS.Misc.Notifications;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Network.RPC;
using KS.Scripting;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;
using System.Reflection;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Inputs;
using KS.Misc.Contacts;
using KS.Network.Base.Connections;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel tools module
    /// </summary>
    public static class KernelTools
    {

        /// <summary>
        /// Kernel version
        /// </summary>
        public readonly static Version KernelVersion = Assembly.GetExecutingAssembly().GetName().Version;
        /// <summary>
        /// Kernel API version
        /// </summary>
        // Refer to NitrocidModAPIVersion in the project file.
        public readonly static Version KernelApiVersion = new(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

        internal static Stopwatch StageTimer = new();
        internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (object arg) => PowerManager.PowerManage((PowerMode)arg)) { isCritical = true };
        internal static string bannerFigletFont = "Banner";

        // #ifdef'd variables...
        // Release specifiers (SPECIFIER: REL, RC, or DEV | MILESTONESPECIFIER: ALPHA, BETA, or NONE | None satisfied: Unsupported Release)
#if SPECIFIERREL
        internal readonly static string ReleaseSpecifier = $"Final";
#elif SPECIFIERRC
        internal readonly static string ReleaseSpecifier = $"Release Candidate";
#elif SPECIFIERDEV
#if MILESTONESPECIFIERALPHA
        internal readonly static string ReleaseSpecifier = $"Alpha 1";
#elif MILESTONESPECIFIERBETA
        internal readonly static string ReleaseSpecifier = $"Beta 1";
#else
        internal readonly static string ReleaseSpecifier = $"Developer Preview";
#endif
#else
        internal readonly static string ReleaseSpecifier = $"- UNSUPPORTED -";
#endif

        // Final console window title
#if SPECIFIERREL
        internal readonly static string ConsoleTitle = $"Nitrocid Kernel v{KernelVersion} (API v{KernelApiVersion})";
#else
        internal readonly static string ConsoleTitle = $"Nitrocid Kernel v{KernelVersion} {ReleaseSpecifier} (API v{KernelApiVersion})";
#endif

        /// <summary>
        /// Reset everything for the next restart
        /// </summary>
        internal static void ResetEverything()
        {
            // Reset every variable below
            ReminderManager.Reminders.Clear();
            EventManager.CalendarEvents.Clear();
            Flags.SafeMode = false;
            Flags.QuietKernel = false;
            Flags.Maintenance = false;
            SplashReport._Progress = 0;
            SplashReport._ProgressText = "";
            SplashReport._KernelBooted = false;
            DebugWriter.WriteDebug(DebugLevel.I, "General variables reset");

            // Reset hardware info
            HardwareProbe.HardwareInfo = null;
            DebugWriter.WriteDebug(DebugLevel.I, "Hardware info reset.");

            // Disconnect all hosts from remote debugger
            RemoteDebugger.StopRDebugThread();
            DebugWriter.WriteDebug(DebugLevel.I, "Remote debugger stopped");

            // Stop all mods
            ModManager.StopMods();
            DebugWriter.WriteDebug(DebugLevel.I, "Mods stopped");

            // Disable Debugger
            if (Flags.DebugMode)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Shutting down debugger");
                Flags.DebugMode = false;
                DebugWriter.DebugStreamWriter.Close();
                DebugWriter.DebugStreamWriter.Dispose();
            }

            // Stop RPC
            RemoteProcedure.StopRPC();

            // Disconnect all connections
            NetworkConnectionTools.CloseAllConnections();

            // Unload all splashes
            SplashManager.UnloadSplashes();

            // Disable safe mode
            Flags.SafeMode = false;

            // Unload all contacts
            ContactsManager.RemoveContacts(false);

            // Stop the time/date change thread
            TimeDateTopRight.TimeTopRightChange.Stop();
        }

        /// <summary>
        /// Initializes everything
        /// </summary>
        internal static void InitEverything()
        {
            // Load alternative buffer (only supported on Linux, because Windows doesn't seem to respect CursorVisible = false on alt buffers)
            if (!KernelPlatform.IsOnWindows() && Flags.UseAltBuffer)
            {
                TextWriterColor.Write("\u001b[?1049h");
                ConsoleWrapper.SetCursorPosition(0, 0);
                ConsoleWrapper.CursorVisible = false;
            }

            // Initialize console wrappers for TermRead
            Input.InitializeInputWrappers();

            // Show initializing
            TextWriterColor.Write(Translate.DoTranslation("Starting Nitrocid..."));

            // Initialize notifications
            if (!NotificationManager.NotifThread.IsAlive)
                NotificationManager.NotifThread.Start();

            // Initialize events and reminders
            if (!ReminderManager.ReminderThread.IsAlive)
                ReminderManager.ReminderThread.Start();
            if (!EventManager.EventThread.IsAlive)
                EventManager.EventThread.Start();

            // Initialize console resize listener
            ConsoleResizeListener.StartResizeListener();

            // Install cancellation handler
            if (!Flags.CancellationHandlerInstalled)
            {
                Console.CancelKeyPress += CancellationHandlers.CancelCommand;
                Flags.CancellationHandlerInstalled = true;
            }

            // Initialize aliases
            AliasManager.InitAliases();

            // Initialize custom languages
            LanguageManager.InstallCustomLanguages();

            // Initialize splashes
            TextWriterColor.Write(Translate.DoTranslation("Loading custom splashes..."));
            SplashManager.LoadSplashes();

            // Create config file and then read it
            TextWriterColor.Write(Translate.DoTranslation("Loading configuration..."));
            if (!Flags.SafeMode)
                Config.InitializeConfig();

            // Load background
            ColorTools.LoadBack();

            // Initialize top right date
            TimeDateTopRight.InitTopRightDate();

            // Show welcome message.
            WelcomeMessage.WriteMessage();

            // Some information
            if (Flags.ShowAppInfoOnBoot & !Flags.EnableSplash)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Kernel environment information"), true, KernelColorType.Stage);
                TextWriterColor.Write("  OS: " + Translate.DoTranslation("Running on {0}"), Environment.OSVersion.ToString());
                TextWriterColor.Write("  KS: " + Translate.DoTranslation("Running from GRILO?") + $" {KernelPlatform.IsRunningFromGrilo()}");
                TextWriterColor.Write("  KSAPI: " + $"v{KernelApiVersion}");
            }

            // Load splash
            SplashManager.OpenSplash();

            // Populate ban list for debug devices
            RemoteDebugTools.PopulateBlockedDevices();

            // Start screensaver timeout
            if (!Screensaver.Timeout.IsAlive)
                Screensaver.Timeout.Start();

            // Load all events and reminders
            EventManager.LoadEvents();
            ReminderManager.LoadReminders();

            // Load system env vars and convert them
            UESHVariables.ConvertSystemEnvironmentVariables();
        }

    }
}
