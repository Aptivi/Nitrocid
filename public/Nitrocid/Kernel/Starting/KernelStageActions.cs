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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Hardware;
using Nitrocid.Kernel.Threading.Watchdog;
using Nitrocid.Kernel.Updates;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;
using Nitrocid.Misc.Splash;
using Nitrocid.Users;
using Nitrocid.Users.Groups;
using Nitrocid.Network.Types.RPC;
using Nitrocid.Kernel.Starting.Bootloader.Apps;
using Nitrocid.Kernel.Starting.Bootloader;
using Nitrocid.Kernel.Starting.Environment;
using Nitrocid.Kernel.Power;

#if SPECIFIERREL
using Nitrocid.Files.Paths;
using Nitrocid.Files;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles;
#endif

namespace Nitrocid.Kernel.Starting
{
    internal static class KernelStageActions
    {
        internal static void Stage01SystemInitialization()
        {
            // If running on development version and not consented, interrupt boot and show developer disclaimer.
            if (!Config.MainConfig.DevNoticeConsented)
            {
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
                WelcomeMessage.ShowDevelopmentDisclaimer();
                SplashManager.EndSplashOut(SplashManager.CurrentSplashContext);
            }

            // If running on unusual environment, interrupt boot and show a message.
            if (!KernelPlatform.IsOnUsualEnvironment())
            {
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
                WelcomeMessage.ShowUnusualEnvironmentWarning();
                SplashManager.EndSplashOut(SplashManager.CurrentSplashContext);
            }

            // Now, initialize remote debugger if the kernel is running in debug mode
            if (Config.MainConfig.RDebugAutoStart & KernelEntry.DebugMode)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Starting the remote debugger..."), 3);
                RemoteDebugger.StartRDebugThread();
                if (!RemoteDebugger.RDebugFailed)
                    SplashReport.ReportProgress(Translate.DoTranslation("Debug listening on all addresses using port {0}."), 5, Config.MainConfig.DebugPort);
                else if (RemoteDebugger.RDebugFailedReason is not null)
                    SplashReport.ReportProgressError(Translate.DoTranslation("Remote debug failed to start: {0}"), RemoteDebugger.RDebugFailedReason.Message);
                else
                    SplashReport.ReportProgressError(Translate.DoTranslation("Remote debug failed to start for unknown reasons."));
            }

            // Try to start the remote procedure call server
            SplashReport.ReportProgress(Translate.DoTranslation("Starting RPC..."), 3);
            RemoteProcedure.WrapperStartRPC();
        }

        internal static void Stage02KernelUpdates()
        {
            if (Config.MainConfig.CheckUpdateStart)
                UpdateManager.CheckKernelUpdates();
#if SPECIFIERREL
            string upgradedPath = PathsManagement.TempPath + "/.upgraded";
            if (!FilesystemTools.FileExists(upgradedPath) || FilesystemTools.ReadContents(upgradedPath)[0] != KernelMain.Version?.ToString())
            {
                FilesystemTools.WriteContentsText(upgradedPath, KernelMain.Version?.ToString() ?? "0.0.0.0");
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
                string changes = UpdateManager.FetchCurrentChangelogsFromResources();
                InfoBoxButtonsColor.WriteInfoBoxButtons([
                    new InputChoiceInfo(Translate.DoTranslation("Acknowledged"), Translate.DoTranslation("Acknowledged")),
                ], changes);
                SplashManager.EndSplashOut(SplashManager.CurrentSplashContext);
            }
#endif
        }

        internal static void Stage03HardwareProbe()
        {
            if (!Config.MainConfig.QuietHardwareProbe)
                SplashReport.ReportProgress(Translate.DoTranslation("Please wait while the kernel initializes your hardware..."), 15);
            HardwareProbe.StartProbing();
            if (!Config.MainConfig.EnableSplash & !KernelEntry.QuietKernel)
                HardwareList.ListHardware();
        }

        internal static void Stage04OptionalComponents() =>
            KernelInitializers.InitializeOptional();

        internal static void Stage05UserInitialization()
        {
            UserManagement.InitializeUsers();
            GroupManagement.InitializeGroups();
            SplashReport.ReportProgress(Translate.DoTranslation("Users initialized"), 5);
        }

        internal static void Stage06SysIntegrity()
        {
            SplashReport.ReportProgress(Translate.DoTranslation("Verifying system integrity"), 5);

            // Check for configuration errors
            if (ConfigTools.NotifyConfigError)
            {
                ConfigTools.NotifyConfigError = false;
                SplashReport.ReportProgressError(Translate.DoTranslation("Configuration error will be notified"));
                NotificationManager.NotifySend(
                    new Notification(
                        Translate.DoTranslation("Configuration error"),
                        Translate.DoTranslation("There is a problem with one of the configuration files. Please check its contents."),
                        NotificationPriority.High,
                        NotificationType.Normal
                    )
                );
            }

            // Check for critical threads
            ThreadWatchdog.EnsureAllCriticalThreadsStarted();
        }

        internal static void Stage07Bootables()
        {
            SplashReport.ReportProgress(Translate.DoTranslation("Checking for multiple installed environments"), 5);

            // Check for multiple environments
            if (BootManager.GetBootApps().Count > 1)
            {
                // End the splash temporarily while we load the bootloader
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
                BootloaderMain.StartBootloader();

                // Request reboot if we need to reboot to another environment
                if (EnvironmentTools.environment != EnvironmentTools.mainEnvironment)
                    PowerManager.PowerManage(PowerMode.Reboot);

                // Open the splash
                SplashManager.EndSplashOut(SplashManager.CurrentSplashContext);
            }
        }
    }
}
