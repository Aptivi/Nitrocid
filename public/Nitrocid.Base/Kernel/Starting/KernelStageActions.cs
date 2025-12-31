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

extern alias TextifyDep;

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Users.Groups;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Kernel.Starting.Bootloader;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Kernel.Updates;
using Nitrocid.Base.Kernel.Hardware;
using Nitrocid.Base.Kernel.Threading.Watchdog;
using Nitrocid.Base.Kernel.Starting.Bootloader.Apps;
using Nitrocid.Base.Network.Types.RPC;
using Nitrocid.Core.Environment;

#if SPECIFIERREL
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Files;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles;
#endif

namespace Nitrocid.Base.Kernel.Starting
{
    internal static class KernelStageActions
    {
        internal static void Stage01SystemInitialization()
        {
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
                SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE1_RDEBUGSTARTING"), 3);
                RemoteDebugger.StartRDebugThread();
                if (!RemoteDebugger.RDebugFailed)
                    SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE1_RDEBUGSTARTED"), 5, Config.MainConfig.DebugPort);
                else if (RemoteDebugger.RDebugFailedReason is not null)
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE1_RDEBUGFAILED"), RemoteDebugger.RDebugFailedReason.Message);
                else
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE1_RDEBUGFAILEDUNKNOWN"));
            }

            // Try to start the remote procedure call server
            SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE1_RPCSTARTED"), 3);
            RemoteProcedure.WrapperStartRPC();
        }

        internal static void Stage02KernelUpdates()
        {
            if (Config.MainConfig.CheckUpdateStart)
                UpdateManager.CheckKernelUpdates();
#if SPECIFIERREL
            string upgradedPath = PathsManagement.TempPath + "/.upgraded";
            if (!FilesystemTools.FileExists(upgradedPath) || FilesystemTools.ReadContents(upgradedPath)[0] != KernelReleaseInfo.Version?.ToString())
            {
                FilesystemTools.WriteContentsText(upgradedPath, KernelReleaseInfo.Version?.ToString() ?? "0.0.0.0");
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
                string changes = UpdateManager.FetchCurrentChangelogsFromResources();
                InfoBoxButtonsColor.WriteInfoBoxButtons([
                    new InputChoiceInfo(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_DEVDISMISS_ACKED"), LanguageTools.GetLocalized("NKS_KERNEL_STARTING_DEVDISMISS_ACKED")),
                ], changes);
                SplashManager.EndSplashOut(SplashManager.CurrentSplashContext);
            }
#endif
        }

        internal static void Stage03HardwareProbe()
        {
            if (!Config.MainConfig.QuietHardwareProbe)
                SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE3_HWINIT"), 15);
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
            SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE5_USERSINITIALIZED"), 5);
        }

        internal static void Stage06SysIntegrity()
        {
            SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE6_INTEGRITY"), 5);

            // Check for configuration errors
            if (ConfigTools.NotifyConfigError)
            {
                ConfigTools.NotifyConfigError = false;
                SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE6_INTEGRITY_CONFIGERROR"));
                NotificationManager.NotifySend(
                    new Notification(
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE6_INTEGRITY_CONFIGERROR_NOTIFTITLE"),
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE6_INTEGRITY_CONFIGERROR_NOTIFDESC"),
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
            SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_STAGE7_CHECKINGENVS"), 5);

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
