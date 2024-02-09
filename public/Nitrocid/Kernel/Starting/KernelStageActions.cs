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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Hardware;
using Nitrocid.Kernel.Threading.Watchdog;
using Nitrocid.Kernel.Updates;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;
using Nitrocid.Misc.Splash;
using Nitrocid.Modifications;
using Nitrocid.Users;
using Nitrocid.Users.Groups;
using Nitrocid.ConsoleBase.Writers.MiscWriters;
using Nitrocid.Network.Types.RPC;

namespace Nitrocid.Kernel.Starting
{
    internal static class KernelStageActions
    {
        internal static void Stage01SystemInitialization()
        {
            // If running on development version and not consented, interrupt boot and show developer disclaimer.
            if (!WelcomeMessage.DevNoticeConsented)
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
            if (RemoteDebugger.RDebugAutoStart & KernelEntry.DebugMode)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Starting the remote debugger..."), 3);
                RemoteDebugger.StartRDebugThread();
                if (!RemoteDebugger.RDebugFailed)
                    SplashReport.ReportProgress(Translate.DoTranslation("Debug listening on all addresses using port {0}."), 5, RemoteDebugger.DebugPort);
                else
                    SplashReport.ReportProgressError(Translate.DoTranslation("Remote debug failed to start: {0}"), RemoteDebugger.RDebugFailedReason.Message);
            }

            // Try to start the remote procedure call server
            SplashReport.ReportProgress(Translate.DoTranslation("Starting RPC..."), 3);
            RemoteProcedure.WrapperStartRPC();
        }

        internal static void Stage02KernelUpdates()
        {
            if (UpdateManager.CheckUpdateStart)
                UpdateManager.CheckKernelUpdates();
        }

        internal static void Stage03HardwareProbe()
        {
            if (!HardwareProbe.QuietHardwareProbe)
                SplashReport.ReportProgress(Translate.DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), 15);
            HardwareProbe.StartProbing();
            if (!SplashManager.EnableSplash & !KernelEntry.QuietKernel)
                HardwareList.ListHardware();
        }

        internal static void Stage04KernelModifications()
        {
            if (ModManager.StartKernelMods)
                ModManager.StartMods();
        }

        internal static void Stage05OptionalComponents() =>
            KernelInitializers.InitializeOptional();

        internal static void Stage06UserInitialization()
        {
            UserManagement.InitializeUsers();
            GroupManagement.InitializeGroups();
            SplashReport.ReportProgress(Translate.DoTranslation("Users initialized"), 5);
        }

        internal static void Stage07SysIntegrity()
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
    }
}
