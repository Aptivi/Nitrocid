//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System;
using System.IO;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Scripting;
using Nitrocid.Users.Login;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Kernel.Threading;
using Nitrocid.Shell.ShellBase.Aliases;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Misc.Screensaver;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Misc.Reflection;
using Nitrocid.Files.Operations;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Files.Folders;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;
using Nitrocid.Security.Privacy;
using Nitrocid.Modifications;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Time.Alarm;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Extensions;
using Nitrocid.Kernel.Journaling;
using Nitrocid.Files.Operations.Querying;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Kernel.Power;
using Nitrocid.Kernel.Threading.Watchdog;
using Terminaux.Colors;
using Nitrocid.ConsoleBase.Writers.MiscWriters;
using Terminaux.Base.Checks;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Network.Types.RPC;
using Nitrocid.Network.SpeedDial;
using Nitrocid.Network.Connections;
using Terminaux.Base.Extensions;

namespace Nitrocid.Kernel.Starting
{
    internal static class KernelInitializers
    {
        internal static void InitializeCritical()
        {
            try
            {
                // Check for terminal
                ConsoleChecker.CheckConsole();

                // Initialize crucial things
                if (!KernelPlatform.IsOnUnix())
                {
                    // Initialize the VT sequences
                    if (!ConsolePositioning.InitializeSequences())
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Can not initialize VT sequences for your Windows terminal. Make sure that you're running Windows 10 or later."));
                        InputTools.DetectKeypress();
                    }
                }

                // Load the assembly resolver
                AppDomain.CurrentDomain.AssemblyResolve += AssemblyLookup.LoadFromAssemblySearchPaths;

                // Check to see if we have an appdata folder for KS
                if (!Checking.FolderExists(PathsManagement.AppDataPath))
                    Making.MakeDirectory(PathsManagement.AppDataPath, false);

                // Set the first time run variable
                if (Listing.GetFilesystemEntries(PathsManagement.AppDataPath).Length == 0)
                    KernelEntry.FirstTime = true;

                // Initialize debug path
                DebugWriter.InitializeDebugPath();

                // Power signal handlers
                PowerSignalHandlers.RegisterHandlers();

                // Resize handler
                ConsoleResizeHandler.StartHandler();
                InputTools.InitializeTerminauxWrappers();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Failed to start critical components") + $": {ex}");
                throw;
            }
        }

        internal static void InitializeEssential()
        {
            try
            {
                // Load alternative buffer (only supported on Linux, because Windows doesn't seem to respect CursorVisible = false on alt buffers)
                if (!KernelPlatform.IsOnWindows() && ConsoleTools.UseAltBuffer)
                {
                    ConsoleMisc.ShowAltBuffer();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded alternative buffer.");
                }

                // A title
                ConsoleMisc.SetTitle(KernelReleaseInfo.ConsoleTitle);

                // Initialize pre-boot splash (if enabled)
                if (KernelEntry.PrebootSplash)
                    SplashManager.OpenSplash(SplashContext.Preboot);

                // Initialize watchdog
                ThreadWatchdog.StartWatchdog();

                // Show initializing
                if (KernelEntry.TalkativePreboot)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Welcome!"));
                    SplashReport.ReportProgress(Translate.DoTranslation("Starting Nitrocid..."));
                }

                // Turn on safe mode in unusual environments
                if (!KernelPlatform.IsOnUsualEnvironment())
                    KernelEntry.SafeMode = true;

                // Initialize journal path
                JournalManager.JournalPath = Getting.GetNumberedFileName(Path.GetDirectoryName(PathsManagement.GetKernelPath(KernelPathType.Journaling)), PathsManagement.GetKernelPath(KernelPathType.Journaling));

                // Initialize custom languages
                if (KernelEntry.TalkativePreboot)
                    SplashReport.ReportProgress(Translate.DoTranslation("Loading custom languages..."));
                LanguageManager.InstallCustomLanguages();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded custom languages.");

                // Initialize addons
                if (KernelEntry.TalkativePreboot)
                    SplashReport.ReportProgress(Translate.DoTranslation("Loading important kernel addons..."));
                AddonTools.ProcessAddons(ModLoadPriority.Important);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded important kernel addons.");

                // Stop the splash prior to loading config
                if (KernelEntry.PrebootSplash)
                    SplashManager.CloseSplash(SplashContext.Preboot);

                // Create config file and then read it
                if (KernelEntry.TalkativePreboot)
                    SplashReport.ReportProgress(Translate.DoTranslation("Loading configuration..."));
                if (!KernelEntry.SafeMode)
                    Config.InitializeConfig();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded configuration.");

                // Read privacy consents
                PrivacyConsentTools.LoadConsents();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded privacy consents.");

                // Load background
                ColorTools.LoadBack();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded background.");

                // Load splash
                SplashManager.OpenSplash(SplashContext.StartingUp);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded splash.");

                // Initialize important mods
                if (ModManager.StartKernelMods)
                {
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgress(Translate.DoTranslation("Loading important mods..."));
                    ModManager.StartMods(ModLoadPriority.Important);
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded important mods.");
                }

                // Populate debug devices
                RemoteDebugTools.LoadAllDevices();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded remote debug devices.");

                // Show first-time color calibration for first-time run
                if (KernelEntry.FirstTime)
                    ConsoleTools.ShowColorRampAndSet();
            }
            catch (Exception ex)
            {
                SplashManager.BeginSplashOut(SplashContext.StartingUp);
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to initialize essential components! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBox(
                    Translate.DoTranslation("The kernel failed to initialize some of the essential components. The kernel will not work properly at this point.") + "\n\n" +
                    Translate.DoTranslation("Error information:") + $" {ex.Message}"
                );
                SplashManager.EndSplashOut(SplashContext.StartingUp);
            }
        }

        internal static void InitializeWelcomeMessages()
        {
            // Show welcome message.
            WelcomeMessage.WriteMessage();

            // Some information
            if (WelcomeMessage.ShowAppInfoOnBoot & !SplashManager.EnableSplash)
            {
                TextFancyWriters.WriteSeparator(Translate.DoTranslation("Kernel environment information"), KernelColorType.Stage);
                TextWriterColor.Write("  OS: " + Translate.DoTranslation("Running on {0}"), System.Environment.OSVersion.ToString());
                TextWriterColor.Write("  KSAPI: " + $"v{KernelMain.ApiVersion}");
            }
        }

        internal static void InitializeOptional()
        {
            try
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

                // Load system env vars and convert them
                UESHVariables.ConvertSystemEnvironmentVariables();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded environment variables.");

                // Initialize alarm listener
                AlarmListener.StartListener();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded alarm listener.");

                // Finalize addons
                AddonTools.ProcessAddons(ModLoadPriority.Optional);
                AddonTools.FinalizeAddons();
                DebugWriter.WriteDebug(DebugLevel.I, "Finalized addons.");

                // If the two files are not found, create two MOTD files with current config and load them.
                MotdParse.ReadMotd();
                MalParse.ReadMal();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded MOTD and MAL.");

                // Load shell command histories
                ShellManager.LoadHistories();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded shell command histories.");

                // Load extension handlers
                ExtensionHandlerTools.LoadAllHandlers();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded extension handlers.");
            }
            catch (Exception ex)
            {
                SplashManager.BeginSplashOut(SplashContext.StartingUp);
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to initialize optional components! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBox(
                    Translate.DoTranslation("The kernel failed to initialize some of the optional components. If it's trying to read a configuration file, make sure that it's formatted correctly.") + "\n\n" +
                    Translate.DoTranslation("Error information:") + $" {ex.Message}"
                );
                SplashManager.EndSplashOut(SplashContext.StartingUp);
            }
        }

        internal static void ResetEverything()
        {
            var context = !PowerManager.KernelShutdown ? SplashContext.Rebooting : SplashContext.ShuttingDown;
            try
            {
                // Reset every variable below
                SplashReport._Progress = 0;
                SplashReport._ProgressText = "";
                SplashReport._KernelBooted = false;
                DebugWriter.WriteDebug(DebugLevel.I, "General variables reset");
                SplashReport.ReportProgress(Translate.DoTranslation("General variables reset"));

                // Save shell command histories
                ShellManager.SaveHistories();
                DebugWriter.WriteDebug(DebugLevel.I, "Saved shell command histories.");
                SplashReport.ReportProgress(Translate.DoTranslation("Saved shell command histories."));

                // Save privacy consents
                PrivacyConsentTools.SaveConsents();
                DebugWriter.WriteDebug(DebugLevel.I, "Saved privacy consents.");

                // Disconnect all hosts from remote debugger
                RemoteDebugger.StopRDebugThread();
                DebugWriter.WriteDebug(DebugLevel.I, "Remote debugger stopped");

                // Reset languages
                SplashManager.BeginSplashOut(context);
                LanguageManager.SetLangDry(LanguageManager.CurrentLanguage);
                LanguageManager.currentUserLanguage = LanguageManager.Languages[LanguageManager.CurrentLanguage];
                SplashManager.EndSplashOut(context);

                // Save extension handlers
                ExtensionHandlerTools.SaveAllHandlers();
                DebugWriter.WriteDebug(DebugLevel.I, "Extension handlers saved");

                // Stop alarm listener
                AlarmListener.StopListener();
                DebugWriter.WriteDebug(DebugLevel.I, "Stopped alarm listener.");

                // Save all settings
                Config.CreateConfig();
                DebugWriter.WriteDebug(DebugLevel.I, "Config saved");
                SplashReport.ReportProgress(Translate.DoTranslation("Config saved."));

                // Stop all mods
                ModManager.StopMods();
                DebugWriter.WriteDebug(DebugLevel.I, "Mods stopped");

                // Stop all addons and their registered components
                AddonTools.UnloadAddons();
                ScreensaverManager.AddonSavers.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Addons and their registered components stopped");
                SplashReport.ReportProgress(Translate.DoTranslation("Extra kernel functions and their registered components stopped."));

                // Stop RPC
                RemoteProcedure.StopRPC();
                DebugWriter.WriteDebug(DebugLevel.I, "RPC stopped");

                // Disconnect all connections
                NetworkConnectionTools.CloseAllConnections();
                DebugWriter.WriteDebug(DebugLevel.I, "Closed all connections");

                // Disable safe mode
                KernelEntry.SafeMode = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Safe mode disabled");

                // Stop the time/date change thread
                TimeDateTopRight.TimeTopRightChange.Stop();
                DebugWriter.WriteDebug(DebugLevel.I, "Time/date corner stopped");

                // Stop screensaver timeout
                ScreensaverManager.StopTimeout();
                DebugWriter.WriteDebug(DebugLevel.I, "Screensaver timeout stopped");

                // Reset the boot log
                SplashReport.logBuffer.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Boot log buffer reset");

                // Disable Debugger
                if (KernelEntry.DebugMode)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Shutting down debugger");
                    KernelEntry.DebugMode = false;
                    DebugWriter.DebugStreamWriter.Close();
                    DebugWriter.DebugStreamWriter.Dispose();
                    DebugWriter.isDisposed = true;
                }

                // Clear all active threads as we're rebooting
                ThreadManager.StopAllThreads();
            }
            catch (Exception ex)
            {
                // We could fail with the debugger enabled
                ColorTools.LoadBack();
                SplashManager.BeginSplashOut(context);
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to reset everything! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBox(
                    Translate.DoTranslation("The kernel failed to reset all the configuration to their initial states. Some of the components might have not unloaded correctly. If you're experiencing problems after the reboot, this might be the cause. Please shut down the kernel once rebooted.") + "\n\n" +
                    Translate.DoTranslation("Error information:") + $" {ex.Message}"
                );
                SplashManager.EndSplashOut(context);
            }
            finally
            {
                // Unload all custom splashes
                SplashReport.ReportProgress(Translate.DoTranslation("Goodbye!"));
                SplashManager.CloseSplash(context);
                SplashManager.customSplashes.Clear();

                // Clear remaining lists
                SplashReport.logBuffer.Clear();
                JournalManager.journalEntries.Clear();
                PowerManager.Uptime.Reset();

                // Reset power state
                PowerManager.RebootRequested = false;
                Login.LogoutRequested = false;

                // Reset base lookup paths
                AssemblyLookup.baseAssemblyLookupPaths.Clear();

                // Set modes as appropriate
                KernelEntry.SafeMode = PowerManager.RebootingToSafeMode;
                KernelEntry.Maintenance = PowerManager.RebootingToMaintenanceMode;
                KernelEntry.DebugMode = PowerManager.RebootingToDebugMode;

                // Unload the assembly resolver
                AppDomain.CurrentDomain.AssemblyResolve -= AssemblyLookup.LoadFromAssemblySearchPaths;

                // Reset quiet state
                KernelEntry.QuietKernel = false;
            }
        }
    }
}
