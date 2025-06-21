//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Shell.Shells;
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
using Textify.Tools.Placeholder;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Power;
using Terminaux.Base.Checks;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Threading;
using Nitrocid.Network.Types.RSS;
using Nitrocid.Shell.Homepage;
using Terminaux.Base;
using Nitrocid.Misc.Audio;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Shell.Arguments.Base;

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

        internal static void EntryPoint(string[]? args)
        {
            // Initialize very important components
            KernelInitializers.InitializeCritical();

            // Check for kernel command-line arguments
            ArgumentParse.ParseArguments(args, KernelArguments.AvailableCMDLineArgs);

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

            // Play the startup sound
            if (Config.MainConfig.EnableStartupSounds)
                AudioCuesTools.PlayAudioCue(AudioCueType.Startup);

            // Show the closing screen
            SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME"), 100);
            SplashManager.CloseSplash(SplashContext.StartingUp);
            SplashReport._KernelBooted = true;
            if (!Config.MainConfig.EnableSplash)
                TextWriterRaw.Write();

            // If this is the first time, run the first run presentation
            if (FirstTime)
            {
                FirstTime = false;
                KernelFirstRun.PresentFirstRunIntro();
            }

            // Show the license infobox
            if (Config.MainConfig.ShowLicenseInfoBox && Config.MainConfig.EnableSplash)
            {
                InfoBoxNonModalColor.WriteInfoBox(
                    WelcomeMessage.GetLicenseString(), new InfoBoxSettings()
                    {
                        Title = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_WELCOMEMESSAGE_LICENSE_TITLE"),
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.License)
                    }
                );
                ConsoleWrapper.CursorVisible = false;
                ThreadManager.SleepUntilInput(15000);
                ThemeColorsTools.LoadBackground();
            }

            // Start the main loop
            DebugWriter.WriteDebug(DebugLevel.I, "Main Loop start.");
            MainLoop();
            DebugWriter.WriteDebug(DebugLevel.I, "Main Loop end.");

            // Load splash for reboot or shutdown
            SplashReport._KernelBooted = false;
            ThemeColorsTools.LoadBackground();
            if (!PowerManager.KernelShutdown)
                SplashManager.OpenSplash(SplashContext.Rebooting);
            else
                SplashManager.OpenSplash(SplashContext.ShuttingDown);
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded splash for reboot or shutdown.");
            ShellManager.PurgeShells();

            // Play the shutdown sound
            if (Config.MainConfig.EnableShutdownSounds)
            {
                SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_KERNEL_PLAYINGSHUTDOWNSOUND"));
                AudioCuesTools.PlayAudioCue(AudioCueType.Shutdown, false);
            }
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
                throw new KernelErrorException(LanguageTools.GetLocalized("NKS_KERNEL_ENTRY_EXCEPTION_KERNELERROR"), exception, exception?.Message ?? "");
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
                    continue;

                // Initialize shell
                DebugWriter.WriteDebug(DebugLevel.I, "Shell is being initialized.");
                while (!Login.LogoutRequested)
                {
                    HomepageTools.OpenHomepage();
                    if (Login.LogoutRequested)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Requested log out: {0}", vars: [Login.LogoutRequested]);
                        break;
                    }

                    // Show MAL
                    BaseLoginHandler.ShowMOTDOnceFlag = true;
                    if (Config.MainConfig.ShowMAL)
                    {
                        TextWriters.Write(PlaceParse.ProbePlaces(MalParse.MalMessage), true, ThemeColorType.Banner);
                        MalParse.ProcessDynamicMal();
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded MAL.");

                    // Show current time
                    if (Config.MainConfig.ShowCurrentTimeBeforeLogin)
                        TimeDateMiscRenderers.ShowCurrentTimes();
                    TextWriterRaw.Write();

                    // Show headline
                    RSSTools.ShowHeadlineLogin();
                    DebugWriter.WriteDebug(DebugLevel.I, "Loaded headline.");

                    // Show the tip
                    if (WelcomeMessage.ShowTip)
                        WelcomeMessage.ShowRandomTip();

                    // Show a tip telling users to see license information
                    TextWriters.Write("* " + LanguageTools.GetLocalized("NKS_KERNEL_LICENSEINFO"), ThemeColorType.Tip);

                    // Show another tip for release window
                    KernelReleaseInfo.NotifyReleaseSupportWindow();

                    // Start the shell
                    ShellManager.StartShell("Shell");
                }
                Login.LoggedIn = false;
                Login.LogoutRequested = false;
            }
        }
    }
}
