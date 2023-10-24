
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
using KS.Drivers;
using KS.Drivers.HardwareProber;
using KS.Files;
using KS.Files.Extensions;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Debugging.Trace;
using KS.Kernel.Extensions;
using KS.Kernel.Journaling;
using KS.Kernel.Power;
using KS.Kernel.Threading;
using KS.Kernel.Threading.Watchdog;
using KS.Kernel.Time.Renderers;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Misc.Text.Probers.Motd;
using KS.Modifications;
using KS.Network.Base.Connections;
using KS.Network.RPC;
using KS.Network.SpeedDial;
using KS.Security.Privacy;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Scripting;
using KS.Shell.ShellBase.Shells;
using KS.Users.Login;
using System;
using System.IO;

namespace KS.Kernel.Starting
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
                    KernelEntry.FirstTime = true;

                // Initialize debug path
                DebugWriter.DebugPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Debugging)), Paths.GetKernelPath(KernelPathType.Debugging));

                // Power signal handlers
                PowerSignalHandlers.RegisterHandlers();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write($"{ex}");
                throw;
            }
        }

        internal static void InitializeEssential()
        {
            try
            {
                // Load alternative buffer (only supported on Linux, because Windows doesn't seem to respect CursorVisible = false on alt buffers)
                if (!KernelPlatform.IsOnWindows() && ConsoleExtensions.UseAltBuffer)
                {
                    TextWriterColor.Write("\u001b[?1049h");
                    ConsoleWrapper.SetCursorPosition(0, 0);
                    ConsoleWrapper.CursorVisible = false;
                    ConsoleExtensions.HasSetAltBuffer = true;
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded alternative buffer.");
                }

                // A title
                ConsoleExtensions.SetTitle(KernelReleaseInfo.ConsoleTitle);

                // Set the buffer size
                if (ConsoleExtensions.setBufferSize)
                    ConsoleExtensions.SetBufferSize();

                // Initialize console wrappers for TermRead
                Input.InitializeTerminauxWrappers();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded input wrappers.");

                // Initialize watchdog
                ThreadWatchdog.StartWatchdog();

                // Show initializing
                if (KernelEntry.TalkativePreboot)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Welcome!"));
                    TextWriterColor.Write(Translate.DoTranslation("Starting Nitrocid..."));
                }

                // Initialize journal path
                JournalManager.JournalPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Journaling)), Paths.GetKernelPath(KernelPathType.Journaling));

                // Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
                if (KernelEntry.TalkativePreboot)
                    TextWriterColor.Write(Translate.DoTranslation("Downloading debug symbols..."));
                DebugSymbolsTools.CheckDebugSymbols();

                // Initialize custom languages
                if (KernelEntry.TalkativePreboot)
                    TextWriterColor.Write(Translate.DoTranslation("Loading custom languages..."));
                LanguageManager.InstallCustomLanguages();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded custom languages.");

                // Initialize splashes
                if (KernelEntry.TalkativePreboot)
                    TextWriterColor.Write(Translate.DoTranslation("Loading custom splashes..."));
                SplashManager.LoadSplashes();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded custom splashes.");

                // Initialize addons
                if (KernelEntry.TalkativePreboot)
                    TextWriterColor.Write(Translate.DoTranslation("Loading kernel addons..."));
                AddonTools.ProcessAddons(AddonType.Important);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded kernel addons.");

                // Create config file and then read it
                if (KernelEntry.TalkativePreboot)
                    TextWriterColor.Write(Translate.DoTranslation("Loading configuration..."));
                if (!KernelEntry.SafeMode)
                    Config.InitializeConfig();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded configuration.");

                // Read privacy consents
                PrivacyConsentTools.LoadConsents();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded privacy consents.");

                // Load background
                KernelColorTools.LoadBack();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded background.");

                // Load splash
                SplashManager.OpenSplash(SplashContext.StartingUp);
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded splash.");

                // Populate debug devices
                RemoteDebugTools.LoadAllDevices();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded remote debug devices.");
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
                SeparatorWriterColor.WriteSeparatorKernelColor(Translate.DoTranslation("Kernel environment information"), true, KernelColorType.Stage);
                TextWriterColor.Write("  OS: " + Translate.DoTranslation("Running on {0}"), System.Environment.OSVersion.ToString());
                TextWriterColor.Write("  KS: " + Translate.DoTranslation("Running from GRILO?") + $" {KernelPlatform.IsRunningFromGrilo()}");
                TextWriterColor.Write("  KSAPI: " + $"v{KernelTools.KernelApiVersion}");
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

                // Start screensaver timeout
                if (!ScreensaverManager.Timeout.IsAlive)
                    ScreensaverManager.Timeout.Start();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded screensaver timeout.");

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
                    MalParse.SetMal(Translate.DoTranslation("Enjoy your day") + ", <user>!");

                // Load MOTD and MAL
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
            try
            {
                // Reset every variable below
                KernelEntry.SafeMode = false;
                KernelEntry.QuietKernel = false;
                KernelEntry.Maintenance = false;
                ConsoleExtensions.HasSetAltBuffer = false;
                SplashReport._Progress = 0;
                SplashReport._ProgressText = "";
                SplashReport._KernelBooted = false;
                JournalManager.journalEntries.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "General variables reset");
                SplashReport.ReportProgress(Translate.DoTranslation("General variables reset"));

                // Save shell command histories
                ShellManager.SaveHistories();
                DebugWriter.WriteDebug(DebugLevel.I, "Saved shell command histories.");
                SplashReport.ReportProgress(Translate.DoTranslation("Saved shell command histories."));

                // Save privacy consents
                PrivacyConsentTools.SaveConsents();
                DebugWriter.WriteDebug(DebugLevel.I, "Saved privacy consents.");

                // Reset hardware info
                var baseProber = DriverHandler.GetFallbackDriver<IHardwareProberDriver>() as BaseHardwareProberDriver;
                baseProber.processors = null;
                baseProber.graphics = null;
                baseProber.hardDrive = null;
                baseProber.pcMemory = null;
                DebugWriter.WriteDebug(DebugLevel.I, "Hardware info reset.");

                // Disconnect all hosts from remote debugger
                RemoteDebugger.StopRDebugThread();
                DebugWriter.WriteDebug(DebugLevel.I, "Remote debugger stopped");

                // Reset languages
                LanguageManager.SetLangDry(LanguageManager.CurrentLanguage);
                LanguageManager.currentUserLanguage = LanguageManager.Languages[LanguageManager.CurrentLanguage];

                // Save extension handlers
                ExtensionHandlerTools.SaveAllHandlers();
                DebugWriter.WriteDebug(DebugLevel.I, "Extension handlers saved");

                // Save all settings
                Config.CreateConfig();
                DebugWriter.WriteDebug(DebugLevel.I, "Config saved");
                SplashReport.ReportProgress(Translate.DoTranslation("Config saved."));

                // Stop all mods
                ModManager.StopMods();
                DebugWriter.WriteDebug(DebugLevel.I, "Mods stopped");

                // Stop all addons
                AddonTools.UnloadAddons();
                DebugWriter.WriteDebug(DebugLevel.I, "Addons stopped");
                SplashReport.ReportProgress(Translate.DoTranslation("Extra kernel functions stopped."));

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

                // Reset the buffer size
                ConsoleExtensions.RestoreBufferSize();

                // Clear all active threads as we're rebooting
                ThreadManager.StopAllThreads();
            }
            catch (Exception ex)
            {
                // We could fail with the debugger enabled
                KernelColorTools.LoadBack();
                SplashManager.BeginSplashOut(SplashContext.ShuttingDown);
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to reset everything! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBox(
                    Translate.DoTranslation("The kernel failed to reset all the configuration to their initial states. Some of the components might have not unloaded correctly. If you're experiencing problems after the reboot, this might be the cause. Please shut down the kernel once rebooted.") + "\n\n" +
                    Translate.DoTranslation("Error information:") + $" {ex.Message}"
                );
                SplashManager.EndSplashOut(SplashContext.ShuttingDown);
            }
            finally
            {
                // Unload all splashes
                SplashReport.ReportProgress(Translate.DoTranslation("Goodbye!"));
                SplashManager.CloseSplash(SplashContext.ShuttingDown);
                SplashManager.UnloadSplashes();
                DebugWriter.WriteDebug(DebugLevel.I, "Unloaded all splashes");
                SplashReport.logBuffer.Clear();
                PowerManager.Uptime.Reset();

                // Reset power state
                PowerManager.RebootRequested = false;
                Login.LogoutRequested = false;
            }
        }
    }
}
