
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
using KS.Network.Mail;
using KS.Network.RPC;
using KS.Scripting;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;
using System.Reflection;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using KS.ConsoleBase.Inputs;
using KS.Misc.Contacts;

#if SPECIFIERREL
using static KS.Misc.Notifications.NotificationManager;
using KS.Files;
using KS.Files.Querying;
using KS.Network.Base;
using KS.Network.Base.Transfer;
#endif

namespace KS.Kernel
{
    /// <summary>
    /// Kernel tools module
    /// </summary>
    public static class KernelTools
    {

        internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (object arg) => PowerManager.PowerManage((PowerMode)arg)) { isCritical = true };
        internal static string bannerFigletFont = "Banner";

        /// <summary>
        /// Kernel version
        /// </summary>
        public readonly static Version KernelVersion = Assembly.GetExecutingAssembly().GetName().Version;
        /// <summary>
        /// Kernel API version
        /// </summary>
        // Refer to NitrocidModAPIVersion in the project file.
        public readonly static Version KernelApiVersion = new(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
        /// <summary>
        /// Current banner figlet font
        /// </summary>
        public static string BannerFigletFont =>
            Config.MainConfig.BannerFigletFont;

        // ----------------------------------------------- Init and reset -----------------------------------------------
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
            Config.MainConfig.Maintenance = false;
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

            // Disconnect from mail
            MailLogin.IMAP_Client.Disconnect(true);
            MailLogin.SMTP_Client.Disconnect(true);

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

        // ----------------------------------------------- Misc -----------------------------------------------

        /// <summary>
        /// Reports the new kernel stage
        /// </summary>
        /// <param name="StageNumber">The stage number</param>
        /// <param name="StageText">The stage text</param>
        internal static void ReportNewStage(int StageNumber, string StageText)
        {
            // Show the stage finish times
            if (StageNumber <= 1)
            {
                if (Flags.ShowStageFinishTimes)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Internal initialization finished in") + $" {Kernel.StageTimer.Elapsed}", 0);
                    Kernel.StageTimer.Restart();
                }
            }
            else if (StageNumber >= 5)
            {
                if (Flags.ShowStageFinishTimes)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {Kernel.StageTimer.Elapsed}", 10);
                    Kernel.StageTimer.Reset();
                    TextWriterColor.Write();
                }
            }
            else if (Flags.ShowStageFinishTimes)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {Kernel.StageTimer.Elapsed}", 10);
                Kernel.StageTimer.Restart();
            }

            // Actually report the stage
            if (StageNumber >= 1 & StageNumber <= 4)
            {
                if (!Flags.EnableSplash & !Flags.QuietKernel)
                {
                    TextWriterColor.Write();
                    SeparatorWriterColor.WriteSeparator(StageText, false, KernelColorType.Stage);
                }
                DebugWriter.WriteDebug(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}");
            }
        }

        /// <summary>
        /// Checks for debug symbols and downloads it if not found. It'll be auto-loaded upon download.
        /// </summary>
        internal static void CheckDebugSymbols()
        {
#if SPECIFIERREL
			if (!NetworkTools.NetworkAvailable)
			{
				NotifySend(new Notification(Translate.DoTranslation("No network while downloading debug data"), Translate.DoTranslation("Check your internet connection and try again."), NotifPriority.Medium, NotifType.Normal));
			}
			if (NetworkTools.NetworkAvailable)
			{
				//Check to see if we're running from Ubuntu PPA
				bool PPASpotted = Paths.ExecPath.StartsWith("/usr/lib/ks");
				if (PPASpotted)
					SplashReport.ReportProgressError(Translate.DoTranslation("Use apt to update Nitrocid KS."));

				//Download debug symbols
				if (!Checking.FileExists(Assembly.GetExecutingAssembly().Location.Replace(".exe", ".pdb")) & !PPASpotted)
				{
					try
					{
						NetworkTransfer.DownloadFile($"https://github.com/Aptivi/NitrocidKS/releases/download/v{KernelVersion}-beta/{KernelVersion}.pdb", Assembly.GetExecutingAssembly().Location.Replace(".exe", ".pdb"));
					}
					catch (Exception)
					{
						NotifySend(new Notification(Translate.DoTranslation("Error downloading debug data"), Translate.DoTranslation("There is an error while downloading debug data. Check your internet connection."), NotifPriority.Medium, NotifType.Normal));
					}
				}
			}
#endif
        }

        internal static void ShowDevelopmentDisclaimer()
        {
            // Show development disclaimer
#if SPECIFIERDEV
            TextWriterColor.Write();
            TextWriterColor.Write("* " + Translate.DoTranslation("You're running the development version of the kernel. While you can experience upcoming features which may exist in the final release, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible."), true, KernelColorType.DevelopmentWarning);
#elif SPECIFIERRC
            TextWriterColor.Write();
            TextWriterColor.Write("* " + Translate.DoTranslation("You're running the release candidate version of the kernel. While you can experience the final touches, you may run into bugs, instabilities, or even data loss. We recommend using the stable version, if possible."), true, KernelColorType.DevelopmentWarning);
#elif SPECIFIERREL == false
            TextWriterColor.Write();
            TextWriterColor.Write("* " + Translate.DoTranslation("We recommend against running this version of the kernel, because it is unsupported. If you have downloaded this kernel from unknown sources, this message may appear. Please download from our official downloads page."), true, KernelColorType.DevelopmentWarning);
#endif
        }

    }
}
