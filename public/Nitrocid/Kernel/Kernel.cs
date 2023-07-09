
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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.IO;
using KS.Arguments.ArgumentBase;
using KS.ConsoleBase;
using KS.Files;
using KS.Files.Querying;
using KS.Hardware;
using KS.Kernel.Exceptions;
using KS.Kernel.Updates;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Probers.Motd;
using KS.Misc.Reflection;
using KS.Misc.Splash;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Modifications;
using KS.Network.RPC;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Users.Login;
using KS.Users;
using KS.Kernel.Events;
using KS.Misc.Text;
using KS.Kernel.Administration.Journalling;
using KS.Files.Operations;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.MiscWriters;
using KS.Misc.Screensaver.Customized;
using KS.Kernel.Power;
using KS.Users.Groups;

namespace KS.Kernel
{
    /// <summary>
    /// Kernel main class
    /// </summary>
    internal static class Kernel
    {

        internal static Stopwatch StageTimer = new();

        // #ifdef'd variables ... Release specifiers (SPECIFIER: REL, RC, or DEV | MILESTONESPECIFIER: ALPHA, BETA, NONE | None satisfied: Unsupported Release)
#if SPECIFIERREL
        internal readonly static string ReleaseSpecifier = $"Final";
#elif SPECIFIERRC
        internal readonly static string ReleaseSpecifier = $"Release Candidate";
#elif SPECIFIERDEV
#if MILESTONESPECIFIERALPHA
        internal readonly static string ReleaseSpecifier = $"Milestone 1";
#elif MILESTONESPECIFIERBETA
        internal readonly static string ReleaseSpecifier = $"Beta 1";
#else
        internal readonly static string ReleaseSpecifier = $"Developer Preview";
#endif
#else
        internal readonly static string ReleaseSpecifier = $"- UNSUPPORTED -";
#endif

        // Final console window title
#if SPECIFIERREL
        internal readonly static string ConsoleTitle = $"Nitrocid Kernel v{KernelTools.KernelVersion} (API v{KernelTools.KernelApiVersion})";
#else
        internal readonly static string ConsoleTitle = $"Nitrocid Kernel v{KernelTools.KernelVersion} {ReleaseSpecifier} (API v{KernelTools.KernelApiVersion})";
#endif

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
                    // Check for terminal
                    ConsoleChecker.CheckConsole();

                    // Initialize crucial things
                    if (!KernelPlatform.IsOnUnix())
                        ConsoleExtensions.InitializeSequences();
                    AppDomain.CurrentDomain.AssemblyResolve += AssemblyLookup.LoadFromAssemblySearchPaths;

                    // A title
                    ConsoleExtensions.SetTitle(ConsoleTitle);

                    // Check to see if we have an appdata folder for KS
                    if (!Checking.FolderExists(Paths.AppDataPath))
                        Making.MakeDirectory(Paths.AppDataPath, false);

                    // Set the first time run variable
                    if (!Checking.FileExists(Paths.ConfigurationPath))
                        Flags.FirstTime = true;

                    // Initialize debug path
                    DebugWriter.DebugPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Debugging)), Paths.GetKernelPath(KernelPathType.Debugging));

                    // Check for kernel command-line arguments
                    ArgumentParse.ParseArguments(Args.ToList());

                    // Initialize journal path
                    JournalManager.JournalPath = Getting.GetNumberedFileName(Path.GetDirectoryName(Paths.GetKernelPath(KernelPathType.Journalling)), Paths.GetKernelPath(KernelPathType.Journalling));

                    // Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
                    KernelTools.CheckDebugSymbols();

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

                    // Initialize everything
                    StageTimer.Start();
                    PowerManager.Uptime.Start();
                    KernelTools.InitEverything();
                    CheckErrored();

                    // Stage 1: Initialize the system
                    KernelTools.ReportNewStage(1, Translate.DoTranslation("- Stage 1: System initialization"));
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

                    // If the two files are not found, create two MOTD files with current config.
                    if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MOTD)))
                    {
                        MotdParse.SetMotd(Translate.DoTranslation("Welcome to Nitrocid Kernel!"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Generated default MOTD."), 3);
                    }
                    if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MAL)))
                    {
                        MalParse.SetMal(Translate.DoTranslation("Logged in successfully as <user>"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Generated default MAL."), 3);
                    }

                    // Check for kernel updates
                    if (Flags.CheckUpdateStart)
                        UpdateManager.CheckKernelUpdates();

                    // Phase 2: Probe hardware
                    KernelTools.ReportNewStage(2, Translate.DoTranslation("- Stage 2: Hardware detection"));
                    if (!Flags.QuietHardwareProbe)
                        SplashReport.ReportProgress(Translate.DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), 15);
                    HardwareProbe.StartProbing();
                    if (!Flags.EnableSplash & !Flags.QuietKernel)
                        HardwareList.ListHardware();
                    CheckErrored();

                    // Phase 3: Parse Mods and Screensavers
                    KernelTools.ReportNewStage(3, Translate.DoTranslation("- Stage 3: Mods and screensavers detection"));
                    DebugWriter.WriteDebug(DebugLevel.I, "Safe mode flag is set to {0}", Flags.SafeMode);
                    if (!Flags.SafeMode)
                    {
                        if (Flags.StartKernelMods)
                            ModManager.StartMods();
                        if (Flags.StartCustomScreensavers)
                            CustomSaverParser.ParseCustomSavers();
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Running in safe mode. Skipping stage..."), 0);
                    }
                    CheckErrored();
                    EventsManager.FireEvent(EventType.StartKernel);

                    // Phase 4: Log-in
                    KernelTools.ReportNewStage(4, Translate.DoTranslation("- Stage 4: Log in"));
                    UserManagement.InitializeUsers();
                    GroupManagement.InitializeGroups();
                    SplashReport.ReportProgress(Translate.DoTranslation("Users initialized"), 5);
                    MotdParse.ReadMotd();
                    MalParse.ReadMal();
                    CheckErrored();

                    // Reset console state and stop stage timer
                    KernelTools.ReportNewStage(5, "");

                    // Show the closing screen
                    SplashReport.ReportProgress(Translate.DoTranslation("Welcome!"), 100);
                    SplashManager.CloseSplash();
                    if (!Flags.EnableSplash)
                        TextWriterColor.Write();

                    // Show current time
                    SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Welcome!"), true, KernelColorType.Stage);
                    if (Flags.ShowCurrentTimeBeforeLogin)
                        TimeDate.TimeDateTools.ShowCurrentTimes();

                    // Notify user of errors if appropriate
                    if (Flags.NotifyKernelError)
                    {
                        Flags.NotifyKernelError = false;
                        NotificationManager.NotifySend(new Notification(Translate.DoTranslation("Previous boot failed"), KernelPanic.LastKernelErrorException.Message, NotificationManager.NotifPriority.High, NotificationManager.NotifType.Normal));
                    }

                    // Show license if splash is enabled
                    if (Flags.EnableSplash)
                    {
                        TextWriterColor.Write();
                        WelcomeMessage.WriteLicense();
                    }

                    // If this is the first time, run the first run presentation
                    KernelFirstRun.FirstRun();

                    // Show development disclaimer
                    KernelTools.ShowDevelopmentDisclaimer();

                    // Initialize login prompt
                    if (!Flags.Maintenance)
                    {
                        Login.LoginPrompt();
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Enter the admin password for maintenance."));
                        if (UserManagement.UserExists("root"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Root account found. Prompting for password...");
                            Login.ShowPasswordPrompt("root");
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
                    KernelPanic.KernelError(KernelErrorLevel.U, true, 5L, Translate.DoTranslation("Kernel Error while booting: {0}"), ex, ex.Message);
                }
            }

            // Clear the console and reset the colors
            ConsoleWrapper.ResetColor();
            ConsoleWrapper.Clear();

            // If "No APM" is enabled, simply print the text
            if (Flags.SimulateNoAPM)
            {
                ConsoleWrapper.WriteLine(Translate.DoTranslation("It's now safe to turn off your computer."));
                Input.DetectKeypress();
            }

            // Load main buffer
            if (!KernelPlatform.IsOnWindows() && Flags.UseAltBuffer)
                TextWriterColor.Write("\u001b[?1049l");
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
