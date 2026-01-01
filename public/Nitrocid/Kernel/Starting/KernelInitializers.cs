//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Shell.Commands;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Scripting;
using Nitrocid.Users.Login;
using Nitrocid.Kernel.Debugging;
using Terminaux.Shell.Shells;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Kernel.Threading;
using Terminaux.Shell.Aliases;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Misc.Reflection;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;
using Nitrocid.Security.Privacy;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Time.Alarm;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Extensions;
using Nitrocid.Kernel.Journaling;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Kernel.Power;
using Nitrocid.Kernel.Threading.Watchdog;
using Terminaux.Base.Checks;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Network.Types.RPC;
using Nitrocid.Network.SpeedDial;
using Nitrocid.Network.Connections;
using Terminaux.Base.Extensions;
using System.Collections.Generic;
using System.Text;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files;
using Textify.Tools.Placeholder;
using Nitrocid.Users;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Timezones;
using Nitrocid.Shell.Shells.UESH;
using Nitrocid.Shell.Shells.Text;
using Nitrocid.Shell.Shells.Hex;
using Nitrocid.Shell.Shells.Admin;
using Nitrocid.Shell.Shells.Debug;
using Nitrocid.Shell.Shells;
using Terminaux.Colors;

namespace Nitrocid.Kernel.Starting
{
    internal static class KernelInitializers
    {
        internal static readonly List<PlaceInfo> placeholders =
        [
            new PlaceInfo("user", (_) => UserManagement.CurrentUser.Username),
            new PlaceInfo("host", (_) => Config.MainConfig.HostName),
            new PlaceInfo("currentdirectory", (_) => FilesystemTools.CurrentDir),
            new PlaceInfo("currentdirectoryname", (_) => !string.IsNullOrEmpty(FilesystemTools.CurrentDir) ? new DirectoryInfo(FilesystemTools.CurrentDir).Name : ""),
            new PlaceInfo("shortdate", (_) => TimeDateRenderers.RenderDate(FormatType.Short)),
            new PlaceInfo("longdate", (_) => TimeDateRenderers.RenderDate(FormatType.Long)),
            new PlaceInfo("shorttime", (_) => TimeDateRenderers.RenderTime(FormatType.Short)),
            new PlaceInfo("longtime", (_) => TimeDateRenderers.RenderTime(FormatType.Long)),
            new PlaceInfo("date", (_) => TimeDateRenderers.RenderDate()),
            new PlaceInfo("time", (_) => TimeDateRenderers.RenderTime()),
            new PlaceInfo("timezone", (_) => TimeZones.GetCurrentZoneInfo().StandardName),
            new PlaceInfo("summertimezone", (_) => TimeZones.GetCurrentZoneInfo().DaylightName),
            new PlaceInfo("dollar", (_) => UserManagement.GetUserDollarSign()),
            new PlaceInfo("randomfile", (_) => FilesystemTools.GetRandomFileName()),
            new PlaceInfo("randomfolder", (_) => FilesystemTools.GetRandomFolderName()),
            new PlaceInfo("rid", (_) => KernelPlatform.GetCurrentRid()),
            new PlaceInfo("uptime", (_) => PowerManager.KernelUptime),
        ];

        internal static void InitializeCritical()
        {
            try
            {
                // Initialize crucial things
                if (!KernelPlatform.IsOnUnix())
                {
                    // Initialize the VT sequences
                    if (!ConsoleMisc.InitializeSequences())
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Can not initialize VT sequences for your Windows terminal. Make sure that you're running Windows 10 or later."));
                        InputTools.DetectKeypress();
                    }
                }

                // Load the assembly resolver
                AppDomain.CurrentDomain.AssemblyResolve += AssemblyLookup.LoadFromAssemblySearchPaths;

                // Check to see if we have an appdata folder for KS
                if (!FilesystemTools.FolderExists(PathsManagement.AppDataPath))
                    FilesystemTools.MakeDirectory(PathsManagement.AppDataPath, false);

                // Set the first time run variable
                if (FilesystemTools.GetFilesystemEntries(PathsManagement.AppDataPath).Length == 0)
                    KernelEntry.FirstTime = true;

                // Initialize debug
                DebugWriter.InitializeDebug();

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
            List<Exception> exceptions = [];
            KernelEntry.enteredBase = true;
            try
            {
                // Load alternative buffer (only supported on Linux, because Windows doesn't seem to respect CursorVisible = false on alt buffers)
                if (!KernelPlatform.IsOnWindows() && KernelEntry.UseAltBuffer)
                {
                    ConsoleMisc.ShowAltBuffer();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded alternative buffer.");
                }

                // A title
                ConsoleMisc.SetTitle(KernelReleaseInfo.ConsoleTitle);
                ShellManager.InitialTitle = KernelReleaseInfo.ConsoleTitle;

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
                JournalManager.JournalPath = FilesystemTools.GetNumberedFileName(Path.GetDirectoryName(PathsManagement.GetKernelPath(KernelPathType.Journaling)), PathsManagement.GetKernelPath(KernelPathType.Journaling));

                // Add the main shells
                SplashReport.ResetProgressReportArea();
                if (!ShellManager.ShellTypeExists("Shell"))
                    ShellManager.RegisterShell("Shell", new UESHShellInfo());
                if (!ShellManager.ShellTypeExists("TextShell"))
                    ShellManager.RegisterShell("TextShell", new TextShellInfo());
                if (!ShellManager.ShellTypeExists("HexShell"))
                    ShellManager.RegisterShell("HexShell", new HexShellInfo());
                if (!ShellManager.ShellTypeExists("AdminShell"))
                    ShellManager.RegisterShell("AdminShell", new AdminShellInfo());
                if (!ShellManager.ShellTypeExists("DebugShell"))
                    ShellManager.RegisterShell("DebugShell", new DebugShellInfo());

                // Add the placeholders
                foreach (var placeholder in placeholders)
                    PlaceParse.RegisterCustomPlaceholder(placeholder.Placeholder, placeholder.PlaceholderAction);

                // Add the shell completions
                ShellCommon.RegisterCompletions();

                // Initialize custom languages
                try
                {
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgress(Translate.DoTranslation("Loading custom languages..."));
                    LanguageManager.InstallCustomLanguages();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded custom languages.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load custom languages");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load custom languages") + $": {exc.Message}");
                }

                // Initialize addons
                try
                {
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgress(Translate.DoTranslation("Loading important kernel addons..."));
                    AddonTools.ProcessAddons(ModLoadPriority.Important);
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded important kernel addons.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load important kernel addons");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load important kernel addons") + $": {exc.Message}");
                }

                // Stop the splash prior to loading config
                if (KernelEntry.PrebootSplash)
                    SplashManager.CloseSplash(SplashContext.Preboot);

                // Create config file and then read it
                try
                {
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgress(Translate.DoTranslation("Loading configuration..."));
                    if (!KernelEntry.SafeMode)
                        Config.InitializeConfig();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded configuration.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load important kernel addons");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load important kernel addons") + $": {exc.Message}");
                }

                // Read privacy consents
                try
                {
                    PrivacyConsentTools.LoadConsents();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded privacy consents.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load privacy consents");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load privacy consents") + $": {exc.Message}");
                }

                // Load background
                KernelColorTools.LoadBackground();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded background.");

                // Load splash
                try
                {
                    SplashManager.OpenSplash(SplashContext.StartingUp);
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded splash.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load splash");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load splash") + $": {exc.Message}");
                }

                // Initialize important mods
                if (AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods)) is not null)
                {
                    var modSettingsInstance = Config.baseConfigurations["ModsConfig"];
                    var modEnableKey = ConfigTools.GetSettingsKey(modSettingsInstance, "StartKernelMods");
                    bool startMods = (bool)(ConfigTools.GetValueFromEntry(modEnableKey, modSettingsInstance) ?? false);
                    var modManagerType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasMods, "Nitrocid.Extras.Mods.Modifications.ModManager");
                    if (startMods)
                    {
                        try
                        {
                            // Check for kernel mod addon
                            if (KernelEntry.TalkativePreboot)
                                SplashReport.ReportProgress(Translate.DoTranslation("Loading important mods..."));
                            InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "StartMods", modManagerType, ModLoadPriority.Important);
                            DebugWriter.WriteDebug(DebugLevel.I, "Loaded important mods.");
                        }
                        catch (Exception exc)
                        {
                            exceptions.Add(exc);
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to load important mods");
                            DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                            DebugWriter.WriteDebugStackTrace(exc);
                            if (KernelEntry.TalkativePreboot)
                                SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load important mods") + $": {exc.Message}");
                        }
                    }
                }

                // Populate debug devices
                try
                {
                    RemoteDebugTools.LoadAllDevices();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded remote debug devices.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load remote debug devices");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load remote debug devices") + $": {exc.Message}");
                }

                // Show first-time color calibration for first-time run
                if (KernelEntry.FirstTime)
                    ColorTools.DetermineTrueColorFromUser();

                // Check for errors
                if (exceptions.Count > 0)
                    throw new KernelException(KernelExceptionType.Environment, Translate.DoTranslation("There were errors when trying to initialize essential components."));
            }
            catch (Exception ex)
            {
                SplashManager.BeginSplashOut(SplashContext.StartingUp);
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to initialize essential components! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxModalColor.WriteInfoBoxModal(
                    Translate.DoTranslation("The kernel failed to initialize some of the essential components. The kernel will not work properly at this point.") + "\n\n" +
                    Translate.DoTranslation("Error information:") + $" {ex.Message}\n\n" +
                    PopulateExceptionText(exceptions)
                );
                SplashManager.EndSplashOut(SplashContext.StartingUp);
            }
        }

        internal static void InitializeWelcomeMessages()
        {
            // Show welcome message.
            WelcomeMessage.WriteMessage();

            // Some information
            if (Config.MainConfig.ShowAppInfoOnBoot & !Config.MainConfig.EnableSplash)
            {
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Kernel environment information"), KernelColorTools.GetColor(KernelColorType.Stage));
                TextWriterColor.Write("OS: " + Translate.DoTranslation("Running on {0}"), System.Environment.OSVersion.ToString());
                TextWriterColor.Write("KSAPI: " + $"v{KernelMain.ApiVersion}");
            }
        }

        internal static void InitializeOptional()
        {
            List<Exception> exceptions = [];
            try
            {
                try
                {
                    // Initialize notifications
                    if (!NotificationManager.NotifThread.IsAlive)
                        NotificationManager.NotifThread.Start();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded notification thread.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load notification thread");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load notification thread") + $": {exc.Message}");
                }

                try
                {
                    // Install cancellation handler
                    CancellationHandlers.InstallHandler();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded cancellation handler.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load cancellation handler");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load cancellation handler") + $": {exc.Message}");
                }

                try
                {
                    // Initialize speed dial
                    SpeedDialTools.LoadAll();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded speed dial entries.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load speed dial entries");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load speed dial entries") + $": {exc.Message}");
                }

                try
                {
                    // Load system env vars and convert them
                    MESHVariables.ConvertSystemEnvironmentVariables();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded environment variables.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load environment variables");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load environment variables") + $": {exc.Message}");
                }

                try
                {
                    // Initialize alarm listener
                    AlarmListener.StartListener();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded alarm listener.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load alarm listener");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load alarm listener") + $": {exc.Message}");
                }

                try
                {
                    // Finalize addons
                    AddonTools.ProcessAddons(ModLoadPriority.Optional);
                    AddonTools.FinalizeAddons();
                    DebugWriter.WriteDebug(DebugLevel.I, "Finalized addons.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to finalize addons");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to finalize addons") + $": {exc.Message}");
                }

                try
                {
                    // If the two files are not found, create two MOTD files with current config and load them.
                    MotdParse.ReadMotd();
                    MalParse.ReadMal();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded MOTD and MAL.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load MOTD and MAL");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load MOTD and MAL") + $": {exc.Message}");
                }

                try
                {
                    // Load extension handlers
                    ExtensionHandlerTools.LoadAllHandlers();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded extension handlers.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to load extension handlers");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    if (KernelEntry.TalkativePreboot)
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load extension handlers") + $": {exc.Message}");
                }

                // Initialize mods
                if (AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods)) is not null)
                {
                    var modSettingsInstance = Config.baseConfigurations["ModsConfig"];
                    var modEnableKey = ConfigTools.GetSettingsKey(modSettingsInstance, "StartKernelMods");
                    bool startMods = (bool)(ConfigTools.GetValueFromEntry(modEnableKey, modSettingsInstance) ?? false);
                    var modManagerType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasMods, "Nitrocid.Extras.Mods.Modifications.ModManager");
                    if (startMods)
                    {
                        try
                        {
                            // Check for kernel mod addon
                            if (KernelEntry.TalkativePreboot)
                                SplashReport.ReportProgress(Translate.DoTranslation("Loading mods..."));
                            InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "StartMods", modManagerType, ModLoadPriority.Optional);
                            DebugWriter.WriteDebug(DebugLevel.I, "Loaded mods.");
                        }
                        catch (Exception exc)
                        {
                            exceptions.Add(exc);
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to load mods");
                            DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                            DebugWriter.WriteDebugStackTrace(exc);
                            if (KernelEntry.TalkativePreboot)
                                SplashReport.ReportProgressError(Translate.DoTranslation("Failed to load mods") + $": {exc.Message}");
                        }
                    }
                }

                // Check for errors
                if (exceptions.Count > 0)
                    throw new KernelException(KernelExceptionType.Environment, Translate.DoTranslation("There were errors when trying to initialize optional components."));
            }
            catch (Exception ex)
            {
                SplashManager.BeginSplashOut(SplashContext.StartingUp);
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to initialize optional components! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxModalColor.WriteInfoBoxModal(
                    Translate.DoTranslation("The kernel failed to initialize some of the optional components. If it's trying to read a configuration file, make sure that it's formatted correctly.") + "\n\n" +
                    Translate.DoTranslation("Error information:") + $" {ex.Message}\n\n" +
                    PopulateExceptionText(exceptions)
                );
                SplashManager.EndSplashOut(SplashContext.StartingUp);
            }
        }

        internal static void ResetEverything()
        {
            var context = !PowerManager.KernelShutdown ? SplashContext.Rebooting : SplashContext.ShuttingDown;
            List<Exception> exceptions = [];
            try
            {
                try
                {
                    // Reset every variable below
                    SplashReport._Progress = 0;
                    SplashReport._ProgressText = "";
                    SplashReport._KernelBooted = false;
                    DebugWriter.WriteDebug(DebugLevel.I, "General variables reset");
                    SplashReport.ReportProgress(Translate.DoTranslation("General variables reset"));
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to reset general variables");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to reset general variables") + $": {exc.Message}");
                }

                try
                {
                    // Save privacy consents
                    PrivacyConsentTools.SaveConsents();
                    DebugWriter.WriteDebug(DebugLevel.I, "Saved privacy consents.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to save privacy consents");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to save privacy consents") + $": {exc.Message}");
                }

                try
                {
                    // Disconnect all hosts from remote debugger
                    RemoteDebugger.StopRDebugThread();
                    DebugWriter.WriteDebug(DebugLevel.I, "Remote debugger stopped");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop remote debugger");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop remote debugger") + $": {exc.Message}");
                }

                try
                {
                    // Reset languages and cultures
                    SplashManager.BeginSplashOut(context);
                    LanguageManager.SetLangDry(Config.MainConfig.CurrentLanguage);
                    LanguageManager.currentUserLanguage = LanguageManager.Languages[Config.MainConfig.CurrentLanguage];
                    CultureManager.currentUserCulture = CultureManager.GetCulturesDictionary()[Config.MainConfig.CurrentCultureName];
                    SplashManager.EndSplashOut(context);
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to reset languages");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to reset languages") + $": {exc.Message}");
                }

                try
                {
                    // Save extension handlers
                    ExtensionHandlerTools.SaveAllHandlers();
                    DebugWriter.WriteDebug(DebugLevel.I, "Extension handlers saved");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to save extension handlers");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to save extension handlers") + $": {exc.Message}");
                }

                try
                {
                    // Stop alarm listener
                    AlarmListener.StopListener();
                    DebugWriter.WriteDebug(DebugLevel.I, "Stopped alarm listener.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop alarm listener");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop alarm listener") + $": {exc.Message}");
                }

                if (KernelEntry.enteredBase)
                {
                    try
                    {
                        // Save all settings
                        Config.CreateConfig();
                        DebugWriter.WriteDebug(DebugLevel.I, "Config saved");
                        SplashReport.ReportProgress(Translate.DoTranslation("Config saved."));
                    }
                    catch (Exception exc)
                    {
                        exceptions.Add(exc);
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to save configuration");
                        DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                        DebugWriter.WriteDebugStackTrace(exc);
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to save configuration") + $": {exc.Message}");
                    }
                }

                if (AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMods)) is not null)
                {
                    try
                    {
                        // Stop all mods
                        var modManagerType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasMods, "Nitrocid.Extras.Mods.Modifications.ModManager");
                        InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "StopMods", modManagerType);
                        DebugWriter.WriteDebug(DebugLevel.I, "Mods stopped");
                    }
                    catch (Exception exc)
                    {
                        exceptions.Add(exc);
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop mods");
                        DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                        DebugWriter.WriteDebugStackTrace(exc);
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop mods") + $": {exc.Message}");
                    }
                }

                try
                {
                    // Stop all addons and their registered components
                    AddonTools.UnloadAddons();
                    ScreensaverManager.AddonSavers.Clear();
                    DebugWriter.WriteDebug(DebugLevel.I, "Addons and their registered components stopped");
                    SplashReport.ReportProgress(Translate.DoTranslation("Extra kernel functions and their registered components stopped."));
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop addons");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop addons") + $": {exc.Message}");
                }

                try
                {
                    // Stop RPC
                    RemoteProcedure.StopRPC();
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC stopped");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop RPC");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop RPC") + $": {exc.Message}");
                }

                try
                {
                    // Disconnect all connections
                    NetworkConnectionTools.CloseAllConnections();
                    DebugWriter.WriteDebug(DebugLevel.I, "Closed all connections");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to close all connections");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to close all connections") + $": {exc.Message}");
                }

                // Disable safe mode
                KernelEntry.SafeMode = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Safe mode disabled");

                try
                {
                    // Stop screensaver timeout
                    ScreensaverManager.StopTimeout();
                    DebugWriter.WriteDebug(DebugLevel.I, "Screensaver timeout stopped");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop screensaver timeout");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop screensaver timeout") + $": {exc.Message}");
                }

                try
                {
                    // Reset the boot log
                    SplashReport.logBuffer.Clear();
                    DebugWriter.WriteDebug(DebugLevel.I, "Boot log buffer reset");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to reset boot log buffer");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to reset boot log buffer") + $": {exc.Message}");
                }

                try
                {
                    // Stop cursor handler
                    ConsolePointerHandler.StopHandler();
                    DebugWriter.WriteDebug(DebugLevel.I, "Stopped the cursor handler.");
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop the cursor handler");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop the cursor handler") + $": {exc.Message}");
                }

                // Disable Debugger
                if (KernelEntry.DebugMode)
                {
                    try
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Shutting down debugger");
                        KernelEntry.DebugMode = false;
                    }
                    catch (Exception exc)
                    {
                        exceptions.Add(exc);
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop the debugger");
                        DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                        DebugWriter.WriteDebugStackTrace(exc);
                        SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop the debugger") + $": {exc.Message}");
                    }
                }

                try
                {
                    // Clear all active threads as we're rebooting
                    ThreadManager.StopAllThreads();
                }
                catch (Exception exc)
                {
                    exceptions.Add(exc);
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to stop all kernel threads");
                    DebugWriter.WriteDebug(DebugLevel.E, exc.Message);
                    DebugWriter.WriteDebugStackTrace(exc);
                    SplashReport.ReportProgressError(Translate.DoTranslation("Failed to stop all kernel threads") + $": {exc.Message}");
                }

                // Remove the main shells
                SplashReport.ResetProgressReportArea();
                if (!ShellManager.ShellTypeExists("Shell"))
                    ShellManager.UnregisterShell("Shell");
                if (!ShellManager.ShellTypeExists("TextShell"))
                    ShellManager.UnregisterShell("TextShell");
                if (!ShellManager.ShellTypeExists("HexShell"))
                    ShellManager.UnregisterShell("HexShell");
                if (!ShellManager.ShellTypeExists("AdminShell"))
                    ShellManager.UnregisterShell("AdminShell");
                if (!ShellManager.ShellTypeExists("DebugShell"))
                    ShellManager.UnregisterShell("DebugShell");

                // Remove the placeholders
                foreach (var placeholder in placeholders)
                    if (PlaceParse.IsPlaceholderRegistered($"<{placeholder.Placeholder}>"))
                        PlaceParse.UnregisterCustomPlaceholder($"<{placeholder.Placeholder}>");

                // Remove the shell completions
                ShellCommon.UnregisterCompletions();

                // Check for errors
                if (exceptions.Count > 0)
                    throw new KernelException(KernelExceptionType.Environment, Translate.DoTranslation("There were errors when trying to reset components."));
            }
            catch (Exception ex)
            {
                // We could fail with the debugger enabled
                KernelColorTools.LoadBackground();
                SplashManager.BeginSplashOut(context);
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to reset everything! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxModalColor.WriteInfoBoxModal(
                    Translate.DoTranslation("The kernel failed to reset all the configuration to their initial states. Some of the components might have not unloaded correctly. If you're experiencing problems after the reboot, this might be the cause. Please shut down the kernel once rebooted.") + "\n\n" +
                    Translate.DoTranslation("Error information:") + $" {ex.Message}\n\n" +
                    PopulateExceptionText(exceptions)
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

                // Reset base state
                KernelEntry.enteredBase = false;
            }
        }

        private static string PopulateExceptionText(List<Exception> exceptions)
        {
            var exceptionsBuilder = new StringBuilder("\n\n");
            for (int i = 0; i < exceptions.Count; i++)
            {
                Exception exception = exceptions[i];

                // Write the exception header
                string exceptionHeader = $"{Translate.DoTranslation("Exception")} {i + 1}/{exceptions.Count}";
                exceptionsBuilder.AppendLine(exceptionHeader);
                exceptionsBuilder.AppendLine(new string('=', ConsoleChar.EstimateCellWidth(exceptionHeader)));

                // Now, write the exception itself
                exceptionsBuilder.AppendLine($"{exception.GetType().Name}: {exception.Message}");
                if (KernelEntry.DebugMode)
                    exceptionsBuilder.AppendLine(exception.StackTrace);

                if (i < exceptions.Count - 1)
                    exceptionsBuilder.AppendLine("\n\n");
            }
            if (exceptions.Count == 0)
                exceptionsBuilder.AppendLine(Translate.DoTranslation("Consult the kernel debug logs for more info."));
            return exceptionsBuilder.ToString();
        }
    }
}
