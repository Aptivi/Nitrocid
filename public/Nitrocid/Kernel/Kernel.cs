
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
using KS.Kernel.Updates;
using KS.Languages;
using KS.Misc.Splash;
using KS.Modifications;
using KS.Network.RPC;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Users.Login;
using KS.Users;
using KS.Kernel.Events;
using KS.Misc.Text;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Colors;
using KS.Kernel.Power;
using KS.Users.Groups;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Hardware;
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
                    CheckErrored();

                    // Stage 1: Initialize the system
                    SplashReport.ReportNewStage(1, Translate.DoTranslation("- Stage 1: System initialization"));
                    if (RemoteDebugger.RDebugAutoStart & Flags.DebugMode)
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Starting the remote debugger..."), 3);
                        RemoteDebugger.StartRDebugThread();
                        if (!RemoteDebugger.RDebugFailed)
                        {
                            SplashReport.ReportProgress(Translate.DoTranslation("Debug listening on all addresses using port {0}."), 5, RemoteDebugger.DebugPort);
                        }
                        else
                        {
                            SplashReport.ReportProgressError(Translate.DoTranslation("Remote debug failed to start: {0}"), RemoteDebugger.RDebugFailedReason.Message);
                        }
                    }
                    SplashReport.ReportProgress(Translate.DoTranslation("Starting RPC..."), 3);
                    RemoteProcedure.WrapperStartRPC();
                    CheckErrored();

                    // Check for kernel updates
                    if (Flags.CheckUpdateStart)
                        UpdateManager.CheckKernelUpdates();

                    // Phase 2: Probe hardware
                    SplashReport.ReportNewStage(2, Translate.DoTranslation("- Stage 2: Hardware detection"));
                    if (!Flags.QuietHardwareProbe)
                        SplashReport.ReportProgress(Translate.DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), 15);
                    HardwareProbe.StartProbing();
                    if (!Flags.EnableSplash & !Flags.QuietKernel)
                        HardwareList.ListHardware();
                    CheckErrored();

                    // Notify user of errors if appropriate
                    KernelPanic.NotifyBootFailure();

                    // Phase 3: Parse Mods and Screensavers
                    SplashReport.ReportNewStage(3, Translate.DoTranslation("- Stage 3: Mods and screensavers detection"));
                    DebugWriter.WriteDebug(DebugLevel.I, "Safe mode flag is set to {0}", Flags.SafeMode);
                    if (!Flags.SafeMode)
                    {
                        if (Flags.StartKernelMods)
                            ModManager.StartMods();
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Running in safe mode. Skipping stage..."), 0);
                    }
                    CheckErrored();

                    // Phase 4: Load everything
                    SplashReport.ReportNewStage(4, Translate.DoTranslation("- Stage 4: Loading optional kernel components"));
                    if (!Flags.SafeMode)
                        KernelInitializers.InitializeOptional();
                    else
                        SplashReport.ReportProgress(Translate.DoTranslation("Running in safe mode. Skipping stage..."), 0);
                    CheckErrored();
                    EventsManager.FireEvent(EventType.StartKernel);

                    // Phase 5: Log-in
                    SplashReport.ReportNewStage(5, Translate.DoTranslation("- Stage 5: Log in"));
                    UserManagement.InitializeUsers();
                    GroupManagement.InitializeGroups();
                    SplashReport.ReportProgress(Translate.DoTranslation("Users initialized"), 5);
                    CheckErrored();

                    // Reset console state and stop stage timer
                    SplashReport.ReportNewStage(6, "");

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
                    {
                        Login.LoginPrompt();
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Enter the admin password for maintenance."));
                        string user = "root";
                        if (UserManagement.UserExists(user))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Root account found. Prompting for password...");
                            for (int tries = 0; tries < 3; tries++)
                            {
                                if (Login.ShowPasswordPrompt(user))
                                    Login.SignIn(user);
                                else
                                {
                                    TextWriterColor.Write(Translate.DoTranslation("Incorrect admin password. You have {0} tries."), 3 - (tries + 1), true, KernelColorType.Error);
                                    if (tries == 2)
                                        TextWriterColor.Write(Translate.DoTranslation("Out of chances. Rebooting..."), true, KernelColorType.Error);
                                }
                            }
                        }
                        else
                        {
                            // Some malicious mod removed the root account, or rare situation happened and it was gone.
                            DebugWriter.WriteDebug(DebugLevel.F, "Root account not found for maintenance.");
                            throw new KernelException(KernelExceptionType.NoSuchUser, Translate.DoTranslation("Some malicious mod removed the root account, or rare situation happened and it was gone."));
                        }
                    }

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
                    Flags.RebootRequested = false;
                    Flags.LogoutRequested = false;
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

        /// <summary>
        /// Check to see if KernelError has been called
        /// </summary>
        private static void CheckErrored()
        {
            if (Flags.KernelErrored)
            {
                Flags.KernelErrored = false;
                throw new KernelErrorException(Translate.DoTranslation("Kernel Error while booting: {0}"), KernelPanic.LastKernelErrorException, KernelPanic.LastKernelErrorException.Message);
            }
        }

    }
}
