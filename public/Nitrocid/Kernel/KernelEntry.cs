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

using Nitrocid.Kernel.Time;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Users.Login;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Arguments;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Misc.Splash;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Starting;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Users.Login.Handlers;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Power;
using Textify.General;
using Nitrocid.ConsoleBase.Writers.MiscWriters;
using Terminaux.Base.Checks;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Threading;
using Nitrocid.Network.Types.RSS;

namespace Nitrocid.Kernel
{
    internal static class KernelEntry
    {
        internal static bool FirstTime;
        internal static bool DebugMode;
        internal static bool SafeMode;
        internal static bool Maintenance;
        internal static bool QuietKernel;
        internal static bool TalkativePreboot;
        internal static bool PrebootSplash = true;

        internal static void EntryPoint(string[] args)
        {
            // Initialize very important components
            KernelInitializers.InitializeCritical();

            // Check for kernel command-line arguments
            ArgumentParse.ParseArguments(args);

            // Some command-line arguments may request kernel shutdown
            if (PowerManager.KernelShutdown)
                return;

            // Check for console size
            ConsoleChecker.CheckConsoleSize();

            // Initialize important components
            KernelStageTools.StageTimer.Start();
            PowerManager.Uptime.Start();
            KernelInitializers.InitializeEssential();
            KernelInitializers.InitializeWelcomeMessages();
            CheckErrored();

            // Notify user of errors if appropriate
            KernelPanic.NotifyBootFailure();

            // Iterate through available stages
            for (int i = 1; i <= KernelStageTools.Stages.Count + 1; i++)
                KernelStageTools.RunKernelStage(i);

            // Show the closing screen
            SplashReport.ReportProgress(Translate.DoTranslation("Welcome!"), 100);
            SplashManager.CloseSplash(SplashContext.StartingUp);
            SplashReport._KernelBooted = true;
            if (!SplashManager.EnableSplash)
                TextWriterColor.Write();

            // If this is the first time, run the first run presentation
            if (FirstTime)
            {
                FirstTime = false;
                KernelFirstRun.PresentFirstRun();
            }

            // Show the license infobox
            if (Config.MainConfig.ShowLicenseInfoBox)
            {
                string rendered = Translate.DoTranslation("License information");
                string dashes = new('=', rendered.Length);
                InfoBoxColor.WriteInfoBoxColor(
                    rendered + CharManager.NewLine +
                    dashes + CharManager.NewLine +
                    WelcomeMessage.GetLicenseString(), false, KernelColorTools.GetColor(KernelColorType.License)
                );
                ThreadManager.SleepUntilInput(3000);
            }

            // Start the main loop
            DebugWriter.WriteDebug(DebugLevel.I, "Main Loop start.");
            MainLoop();
            DebugWriter.WriteDebug(DebugLevel.I, "Main Loop end.");
        }

        /// <summary>
        /// Check to see if KernelError has been called
        /// </summary>
        internal static void CheckErrored()
        {
            if (KernelPanic.KernelErrored)
            {
                KernelPanic.KernelErrored = false;
                var exception = KernelPanic.LastKernelErrorException;
                throw new KernelErrorException(Translate.DoTranslation("Kernel Error while booting: {0}"), exception, exception.Message);
            }
        }

        private static void MainLoop()
        {
            while (!PowerManager.RebootRequested && !PowerManager.KernelShutdown)
            {
                // Initialize login prompt
                if (!Maintenance)
                    Login.LoginPrompt();
                else
                    Login.PromptMaintenanceLogin();
                CheckErrored();

                // Check to see if login handler requested power action
                if (PowerManager.RebootRequested || PowerManager.KernelShutdown)
                    return;

                // Show current time
                if (TimeDateTools.ShowCurrentTimeBeforeLogin)
                {
                    TimeDateMiscRenderers.ShowCurrentTimes();
                    TextWriterColor.Write();
                }

                // Show the tip
                if (WelcomeMessage.ShowTip)
                    WelcomeMessage.ShowRandomTip();

                // Show a tip telling users to see license information
                TextWriters.Write("* " + Translate.DoTranslation("Run 'license' to see the license information.") + CharManager.NewLine, KernelColorType.Tip);

                // Show MOTD
                BaseLoginHandler.ShowMOTDOnceFlag = true;
                if (BaseLoginHandler.ShowMAL)
                {
                    TextWriters.Write(PlaceParse.ProbePlaces(MalParse.MalMessage), true, KernelColorType.Banner);
                    MalParse.ProcessDynamicMal();
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded MAL.");

                // Show headline
                RSSTools.ShowHeadlineLogin();
                DebugWriter.WriteDebug(DebugLevel.I, "Loaded headline.");

                // Initialize shell
                DebugWriter.WriteDebug(DebugLevel.I, "Shell is being initialized.");
                ShellManager.StartShellForced(ShellType.Shell);
            }

            // Load splash
            SplashReport._KernelBooted = false;
            if (!PowerManager.KernelShutdown)
                SplashManager.OpenSplash(SplashContext.Rebooting);
            else
                SplashManager.OpenSplash(SplashContext.ShuttingDown);
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded splash for reboot or shutdown.");
            ShellManager.PurgeShells();
        }
    }
}
