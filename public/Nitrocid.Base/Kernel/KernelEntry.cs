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
using Nitrocid.Base.Arguments;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Kernel.Starting;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Audio;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Network.Types.RSS;
using Nitrocid.Base.Shell.Homepage;
using Nitrocid.Base.Users.Login;
using Nitrocid.Base.Users.Login.Handlers;
using Nitrocid.Base.Users.Login.Motd;
using Nitrocid.Base.Users.Windows;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Shell.Arguments.Base;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools.Placeholder;

namespace Nitrocid.Base.Kernel
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
        internal static bool UseAltBuffer = true;
        internal static bool inShell = false;
        internal static bool enteredBase = false;
        internal static bool devMessageAck = false;

        internal static void EntryPoint(string[]? args)
        {
            try
            {
                // Show help / version prior to starting the kernel if help / version is passed
                ArgumentParse.ParseArguments(args, KernelArguments.outArgs);

                // Show development notice
                if (!PowerManager.KernelShutdown && !devMessageAck)
                {
                    // Show the message
#if !SPECIFIERREL
                    string message =
#if SPECIFIERDEV
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_DEVMESSAGE")
#elif SPECIFIERRC
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_RCMESSAGE")
#elif SPECIFIERALPHA
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_ALPHAMESSAGE")
#elif SPECIFIERBETA
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BETAMESSAGE")
#else
                        LanguageTools.GetLocalized("NKS_KERNEL_STARTING_UNSUPPORTED")
#endif
                    ;
                    TextWriterColor.Write(message + "\n", ThemeColorType.Warning);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COMMON_ANYKEY"), ThemeColorType.Warning);
                    Input.ReadKey();
                    devMessageAck = true;
#endif
                }
                else if (PowerManager.KernelShutdown)
                    return;

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
            catch (KernelErrorException kee)
            {
                DebugWriter.WriteDebugStackTrace(kee);
                SafeMode = false;
            }
            catch (Exception ex)
            {
                KernelPanic.KernelError(KernelErrorLevel.U, true, 5, ex.Message, ex);
            }
        }

        internal static void ExitPoint()
        {
            // Reset everything to their initial state
            if (!PowerManager.hardShutdown)
            {
                KernelInitializers.ResetEverything();
                PowerSignalHandlers.DisposeHandlers();

                // If "No APM" is enabled, simply print the text
                if (Config.MainConfig.SimulateNoAPM)
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_NOAPMSIMULATION"), new InfoBoxSettings()
                    {
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Success)
                    });

                // Load main buffer
                if (!KernelPlatform.IsOnWindows() && UseAltBuffer && ConsoleMisc.IsOnAltBuffer && !PowerManager.hardShutdown)
                    ConsoleMisc.ShowMainBuffer();

                // Reset colors and clear the console
                if (!PowerManager.hardShutdown)
                    ConsoleClearing.ResetAll();
                else
                    ThemeColorsTools.ResetColors();

                // Reset cursor state
                ConsoleWrapper.CursorVisible = true;

                // Check to see if we're restarting Nitrocid with elevated permissions
                if (PowerManager.elevating && KernelPlatform.IsOnWindows() && !WindowsUserTools.IsAdministrator())
                    PowerManager.ElevateSelf();
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
                        TextWriterColor.Write(PlaceParse.ProbePlaces(MalParse.MalMessage), true, ThemeColorType.Banner);
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
                    TextWriterColor.Write("* " + LanguageTools.GetLocalized("NKS_KERNEL_LICENSEINFO"), ThemeColorType.Tip);

                    // Show a tip telling users to write 'help' to get started
                    TextWriterColor.Write("* " + LanguageTools.GetLocalized("NKS_KERNEL_SHELLTIP"), ThemeColorType.Tip);

                    // Show another tip for release window
                    KernelReleaseInfo.NotifyReleaseSupportWindow();

                    // Start the shell
                    inShell = true;
                    ShellManager.StartShell("Shell");
                    inShell = false;
                }
                Login.LoggedIn = false;
                Login.LogoutRequested = false;
            }
        }
    }
}
