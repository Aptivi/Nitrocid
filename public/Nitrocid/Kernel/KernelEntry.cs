//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Splash;
using KS.Users.Login;
using KS.Misc.Text;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Colors;
using KS.Kernel.Power;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Starting;
using KS.Arguments;
using KS.ConsoleBase.Writers.MiscWriters;
using KS.Kernel.Time.Renderers;
using KS.Kernel.Debugging;
using KS.Misc.Text.Probers.Placeholder;
using KS.Network.RSS;
using KS.Shell.ShellBase.Shells;
using KS.Misc.Text.Probers.Motd;
using KS.Kernel.Time;
using KS.Users.Login.Handlers;

namespace KS.Kernel
{
    internal static class KernelEntry
    {
        internal static bool FirstTime;
        internal static bool DebugMode;
        internal static bool SafeMode;
        internal static bool Maintenance;
        internal static bool QuietKernel;
        internal static bool TalkativePreboot;
        internal static bool PrebootSplash;

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
            if (ConsoleChecker.CheckingForConsoleSize)
            {
                ConsoleChecker.CheckConsoleSize();
            }
            else
            {
                TextWriterColor.WriteKernelColor(
                    Translate.DoTranslation("Looks like you're bypassing the console size detection. Things may not work properly on small screens.") + CharManager.NewLine +
                    Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, KernelColorType.Warning
                );
                Input.DetectKeypress();
                ConsoleChecker.CheckingForConsoleSize = true;
            }

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

            // Start the main loop
            DebugWriter.WriteDebug(DebugLevel.I, "Main Loop start.");
            MainLoop();
            ShellManager.PurgeShells();
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

                // Show license information
                WelcomeMessage.WriteLicense();

                // Show current time
                if (TimeDateTools.ShowCurrentTimeBeforeLogin)
                {
                    TimeDateMiscRenderers.ShowCurrentTimes();
                    TextWriterColor.Write();
                }

                // Show the tip
                if (WelcomeMessage.ShowTip)
                    WelcomeMessage.ShowRandomTip();

                // Show MOTD
                BaseLoginHandler.ShowMOTDOnceFlag = true;
                if (BaseLoginHandler.ShowMAL)
                {
                    TextWriterColor.WriteKernelColor(PlaceParse.ProbePlaces(MalParse.MalMessage), true, KernelColorType.Banner);
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
            SplashManager.OpenSplash(SplashContext.ShuttingDown);
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded splash.");
        }
    }
}
