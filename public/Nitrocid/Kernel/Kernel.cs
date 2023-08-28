
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

using System;
using System.Linq;
using System.Threading;
using KS.Arguments.ArgumentBase;
using KS.ConsoleBase;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Splash;
using KS.Kernel.Debugging;
using KS.Users.Login;
using KS.Users;
using KS.Misc.Text;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Colors;
using KS.Kernel.Power;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Starting;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    internal static class Kernel
    {

        /// <summary>
        /// Entry point
        /// </summary>
        internal static void Main(string[] Args)
        {
            // Set main thread name
            Thread.CurrentThread.Name = "Main Nitrocid Kernel Thread";

            // This is a kernel entry point
            while (!Flags.KernelShutdown)
            {
                try
                {
                    KernelInitializers.InitializeCritical();
                    if (Flags.IsEnteringRetroMode)
                    {
                        Flags.IsEnteringRetroMode = false;

                        // Reboot the kernel from RetroKS
                        continue;
                    }

                    // Check for kernel command-line arguments
                    ArgumentParse.ParseArguments(Args.ToList());

                    // Some command-line arguments may request kernel shutdown
                    if (Flags.KernelShutdown)
                        break;

                    // Check for console size
                    if (Flags.CheckingForConsoleSize)
                    {
                        ConsoleChecker.CheckConsoleSize();
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Looks like you're bypassing the console size detection. Things may not work properly on small screens.") + CharManager.NewLine + 
                                              Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, KernelColorType.Warning);
                        Input.DetectKeypress();
                        Flags.CheckingForConsoleSize = true;
                    }

                    // Initialize important components
                    KernelTools.StageTimer.Start();
                    PowerManager.Uptime.Start();
                    KernelInitializers.InitializeEssential();
                    KernelInitializers.InitializeWelcomeMessages();
                    KernelTools.CheckErrored();

                    // Notify user of errors if appropriate
                    KernelPanic.NotifyBootFailure();

                    // Iterate through available stages
                    for (int i = 1; i <= KernelStageTools.Stages.Count + 1; i++)
                        KernelStageTools.RunKernelStage(i);

                    // Show the closing screen
                    SplashReport.ReportProgress(Translate.DoTranslation("Welcome!"), 100);
                    SplashManager.CloseSplash();
                    SplashReport._KernelBooted = true;
                    if (!Flags.EnableSplash)
                        TextWriterColor.Write();

                    // If this is the first time, run the first run presentation
                    if (Flags.FirstTime)
                    {
                        Flags.FirstTime = false;
                        KernelFirstRun.PresentFirstRun();
                    }

                    // Initialize login prompt
                    if (!Flags.Maintenance)
                        Login.LoginPrompt();
                    else
                        Login.PromptMaintenanceLogin();

                    // Clear all active threads as we're rebooting
                    ThreadManager.StopAllThreads();
                    PowerManager.Uptime.Reset();
                }
                catch (KernelException icde) when (icde.ExceptionType == KernelExceptionType.InsaneConsoleDetected)
                {
                    ConsoleWrapper.WriteLine(icde.Message);
                    Flags.KernelShutdown = true;
                }
                catch (KernelErrorException kee)
                {
                    DebugWriter.WriteDebugStackTrace(kee);
                    Flags.SafeMode = false;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    SplashManager.BeginSplashOut();
                    KernelPanic.KernelError(KernelErrorLevel.U, true, 5L, Translate.DoTranslation("Kernel Error while booting: {0}"), ex, ex.Message);
                }
                finally
                {
                    // Reset everything to their initial state
                    KernelInitializers.ResetEverything();

                    // Clear the console and reset the colors
                    ConsoleWrapper.ResetColor();
                    ConsoleWrapper.Clear();
                }
            }

            // If "No APM" is enabled, simply print the text
            if (Flags.SimulateNoAPM)
            {
                ConsoleWrapper.WriteLine(Translate.DoTranslation("It's now safe to turn off your computer."));
                Input.DetectKeypress();
            }

            // Load main buffer
            if (!KernelPlatform.IsOnWindows() && Flags.UseAltBuffer)
            {
                TextWriterColor.Write("\u001b[?1049l");
                ConsoleWrapper.Clear();
            }

            // Reset cursor state
            ConsoleWrapper.CursorVisible = true;
        }

    }
}
