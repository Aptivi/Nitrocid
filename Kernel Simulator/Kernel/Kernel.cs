using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Reflection.Assembly;
using System.Threading;
using ColorSeq;
using Extensification.StringExts;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Arguments.ArgumentBase;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Querying;
using KS.Hardware;
using KS.Kernel.Configuration;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Login;
using KS.Misc.Notifications;
using KS.Misc.Platform;
using KS.Misc.Probers.Motd;
using KS.Misc.Reflection;
using KS.Misc.Splash;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Network.RemoteDebug;
using KS.Network.RPC;
using ReadLineReboot;

#if SPECIFIERREL
Imports KS.Network
Imports KS.Network.Transfer
Imports KS.Kernel.Updates
#endif

namespace KS.Kernel
{
    public static class Kernel
    {

        // Variables
        public static string MOTDMessage, MAL;
        public static string HostName = "kernel";
        public readonly static string KernelVersion = GetExecutingAssembly().GetName().Version.ToString();
        public readonly static string NewLine = Environment.NewLine;
        public readonly static Events.Events KernelEventManager = new Events.Events();
        public readonly static string ExecutableDir = Environment.CurrentDirectory;
        internal static Stopwatch StageTimer = new Stopwatch();
        internal static TextWriter DefConsoleOut;

        // #ifdef'd variables ... Framework monikers
#if NETCOREAPP
        public const string KernelSimulatorMoniker = ".NET CoreCLR";
#else
        public const string KernelSimulatorMoniker = ".NET Framework";
#endif
        // Release specifiers (SPECIFIER: REL, RC, or DEV | MILESTONESPECIFIER: ALPHA, BETA, DELTA, GAMMA, NONE | None satisfied: Unsupported Release)
#if SPECIFIERREL
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker}";
#elif SPECIFIERRC
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Release Candidate";
#elif SPECIFIERDEV
#if MILESTONESPECIFIERALPHA
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Milestone 2";
#elif MILESTONESPECIFIERBETA
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Beta 1";
#elif MILESTONESPECIFIERDELTA
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Delta 1";
#elif MILESTONESPECIFIERGAMMA
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview - Gamma 1";
#else
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Developer Preview";
#endif
#else
        public readonly static string ConsoleTitle = $"Kernel Simulator v{KernelVersion} - {KernelSimulatorMoniker} - Unsupported Release";
#endif

        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] Args)
        {
            // Set main thread name
            Thread.CurrentThread.Name = "Main Kernel Thread";

            // This is a kernel entry point
            while (!Flags.KernelShutdown)
            {
                try
                {
                    // A title
                    ConsoleExtensions.SetTitle(ConsoleTitle);

                    // Initial ReadLine settings
                    ReadLine.CtrlCEnabled = true;
                    Flags.InputHistoryEnabled = true;
                    ReadLine.PrewriteDefaultValue = true;
                    ReadLine.AutoCompletionEnabled = true;

                    // Check for terminal
                    ConsoleSanityChecker.CheckConsole();

                    // Initialize crucial things
                    if (Flags.SafeMode)
                        Config.ReadFailsafeConfig();
                    if (!KernelPlatform.IsOnUnix())
                        Color255.Initialize255();
                    AppDomain.CurrentDomain.AssemblyResolve += AssemblyLookup.LoadFromAssemblySearchPaths;

                    // Check for pre-boot arguments
                    ArgumentParse.ParseArguments(Args.ToList(), ArgumentType.PreBootCommandLineArgs);

                    // Download debug symbols if not found (loads automatically, useful for debugging problems and stack traces)
                    // TODO: Move this to a separate function
#if SPECIFIERREL
					if (!NetworkAvailable)
					{
						NotifySend(new Notification(DoTranslation("No network while downloading debug data"), DoTranslation("Check your internet connection and try again."), NotifPriority.Medium, NotifType.Normal));
					}
					if (NetworkAvailable)
					{
						//Check to see if we're running from Ubuntu PPA
						bool PPASpotted = ExecPath.StartsWith("/usr/lib/ks");
						if (ExecPath.StartsWith("/usr/lib/ks"))
						{
							ReportProgress(DoTranslation("Use apt to update Kernel Simulator."), 10, ColTypes.Error);
						}

						//Download debug symbols
						if ((FileExists(GetExecutingAssembly.Location.Replace(".exe", ".pdb")) & !PPASpotted) == 0)
						{
							try
							{
#if NETCOREAPP
								DownloadFile($"https://github.com/Aptivi/Kernel-Simulator/releases/download/v{KernelVersion}-beta/{KernelVersion}-dotnet.pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"));
#else
								DownloadFile($"https://github.com/Aptivi/Kernel-Simulator/releases/download/v{KernelVersion}-beta/{KernelVersion}.pdb", GetExecutingAssembly.Location.Replace(".exe", ".pdb"));
#endif
							}
							catch (Exception ex)
							{
								NotifySend(new Notification(DoTranslation("Error downloading debug data"), DoTranslation("There is an error while downloading debug data. Check your internet connection."), NotifPriority.Medium, NotifType.Normal));
							}
						}
					}
#endif

                    // Check for console size
                    if (Flags.CheckingForConsoleSize)
                    {
                        // Check for the minimum console window requirements (80x24)
                        while (Console.WindowWidth < 80 | Console.WindowHeight < 24)
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Your console is too small to run properly:") + " {0}x{1}", true, ColorTools.ColTypes.Warning, Console.WindowWidth, Console.WindowHeight);
                            TextWriterColor.Write(Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, ColorTools.ColTypes.Warning);
                            Console.ReadKey(true);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Looks like you're bypassing the console size detection. Things may not work properly on small screens.") + NewLine + Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, ColorTools.ColTypes.Warning);
                        Console.ReadKey(true);
                        Flags.CheckingForConsoleSize = true;
                    }

                    // Initialize everything
                    StageTimer.Start();
                    KernelTools.InitEverything(Args);
                    CheckErrored();

                    // Stage 1: Initialize the system
                    KernelTools.ReportNewStage(1, Translate.DoTranslation("- Stage 1: System initialization"));
                    if (RemoteDebugger.RDebugAutoStart & Flags.DebugMode)
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Starting the remote debugger..."), 3, ColorTools.ColTypes.Neutral);
                        RemoteDebugger.StartRDebugThread();
                        if (!RemoteDebugger.RDebugFailed)
                        {
                            SplashReport.ReportProgress(Translate.DoTranslation("Debug listening on all addresses using port {0}.").FormatString(RemoteDebugger.DebugPort), 5, ColorTools.ColTypes.Neutral);
                        }
                        else
                        {
                            SplashReport.ReportProgress(Translate.DoTranslation("Remote debug failed to start: {0}").FormatString(RemoteDebugger.RDebugFailedReason.Message), 5, ColorTools.ColTypes.Error);
                        }
                    }
                    SplashReport.ReportProgress(Translate.DoTranslation("Starting RPC..."), 3, ColorTools.ColTypes.Neutral);
                    RemoteProcedure.WrapperStartRPC();

                    // If the two files are not found, create two MOTD files with current config.
                    if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MOTD)))
                    {
                        MotdParse.SetMotd(Translate.DoTranslation("Welcome to Kernel!"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Generated default MOTD."), 3, ColorTools.ColTypes.Neutral);
                    }
                    if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.MAL)))
                    {
                        MalParse.SetMal(Translate.DoTranslation("Logged in successfully as <user>"));
                        SplashReport.ReportProgress(Translate.DoTranslation("Generated default MAL."), 3, ColorTools.ColTypes.Neutral);
                    }

                    // Check for kernel updates
#if SPECIFIERREL
                    if (CheckUpdateStart)
                        CheckKernelUpdates()
#endif

                    // Phase 2: Probe hardware
                    KernelTools.ReportNewStage(2, Translate.DoTranslation("- Stage 2: Hardware detection"));
                    if (!Flags.QuietHardwareProbe)
                        SplashReport.ReportProgress(Translate.DoTranslation("hwprobe: Your hardware will be probed. Please wait..."), 15, ColorTools.ColTypes.Progress);
                    HardwareProbe.StartProbing();
                    if (!Flags.EnableSplash & !Flags.QuietKernel)
                        HardwareList.ListHardware();
                    CheckErrored();

                    // Phase 3: Parse Mods and Screensavers
                    KernelTools.ReportNewStage(3, Translate.DoTranslation("- Stage 3: Mods and screensavers detection"));
                    DebugWriter.Wdbg(DebugLevel.I, "Safe mode flag is set to {0}", Flags.SafeMode);
                    if (!Flags.SafeMode)
                    {
                        if (Flags.StartKernelMods)
                            ModManager.StartMods();
                    }
                    else
                    {
                        SplashReport.ReportProgress(Translate.DoTranslation("Running in safe mode. Skipping stage..."), 0, ColorTools.ColTypes.Neutral);
                    }
                    KernelEventManager.RaiseStartKernel();

                    // Phase 4: Log-in
                    KernelTools.ReportNewStage(4, Translate.DoTranslation("- Stage 4: Log in"));
                    UserManagement.InitializeSystemAccount();
                    SplashReport.ReportProgress(Translate.DoTranslation("System account initialized"), 5, ColorTools.ColTypes.Neutral);
                    UserManagement.InitializeUsers();
                    SplashReport.ReportProgress(Translate.DoTranslation("Users initialized"), 5, ColorTools.ColTypes.Neutral);
                    PermissionManagement.LoadPermissions();
                    SplashReport.ReportProgress(Translate.DoTranslation("Permissions loaded"), 5, ColorTools.ColTypes.Neutral);

                    // Reset console state and stop stage timer
                    KernelTools.ReportNewStage(5, "");

                    // Show the closing screen
                    SplashReport.ReportProgress(Translate.DoTranslation("Welcome!"), 100, ColorTools.ColTypes.Success);
                    SplashManager.CloseSplash();

                    // Show current time
                    if (Flags.ShowCurrentTimeBeforeLogin)
                        TimeDate.TimeDate.ShowCurrentTimes();

                    // Notify user of errors if appropriate
                    if (Flags.NotifyKernelError)
                    {
                        Flags.NotifyKernelError = false;
                        Notifications.NotifySend(new Notification(Translate.DoTranslation("Previous boot failed"), KernelTools.LastKernelErrorException.Message, Notifications.NotifPriority.High, Notifications.NotifType.Normal));
                    }

                    // Show license if new style used
                    if (Flags.NewWelcomeStyle | Flags.EnableSplash)
                    {
                        Console.WriteLine();
                        SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true, ColorTools.ColTypes.Stage);
                        WelcomeMessage.WriteLicense(false);
                    }

                    // Initialize login prompt
                    if (!Flags.Maintenance)
                    {
                        Login.Login.LoginPrompt();
                    }
                    else
                    {
                        MotdParse.ReadMotd();
                        MalParse.ReadMal();
                        TextWriterColor.Write(Translate.DoTranslation("Enter the admin password for maintenance."), true, ColorTools.ColTypes.Neutral);
                        if (Login.Login.Users.ContainsKey("root"))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Root account found. Prompting for password...");
                            Login.Login.ShowPasswordPrompt("root");
                        }
                        else
                        {
                            // Some malicious mod removed the root account, or rare situation happened and it was gone.
                            DebugWriter.Wdbg(DebugLevel.W, "Root account not found for maintenance. Initializing it...");
                            UserManagement.InitializeSystemAccount();
                            Login.Login.ShowPasswordPrompt("root");
                        }
                    }

                    // Clear all active threads as we're rebooting
                    ThreadManager.StopAllThreads();
                }
                catch (InsaneConsoleDetectedException icde)
                {
                    Console.WriteLine(icde.Message);
                    Console.WriteLine(icde.InsanityReason);
                    Flags.KernelShutdown = true;
                }
                catch (KernelErrorException kee)
                {
                    DebugWriter.WStkTrc(kee);
                    Flags.KernelErrored = false;
                    Flags.RebootRequested = false;
                    Flags.LogoutRequested = false;
                    Flags.SafeMode = false;
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.U, true, 5L, Translate.DoTranslation("Kernel Error while booting: {0}"), ex, ex.Message);
                }
            }

            // Clear the console and reset the colors
            Console.ResetColor();
            Console.Clear();

            // If "No APM" is enabled, simply print the text
            if (Flags.SimulateNoAPM)
            {
                Console.WriteLine(Translate.DoTranslation("It's now safe to turn off your computer."));
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Check to see if KernelError has been called
        /// </summary>
        public static void CheckErrored()
        {
            if (Flags.KernelErrored)
                throw new KernelErrorException(Translate.DoTranslation("Kernel Error while booting: {0}"), KernelTools.LastKernelErrorException, KernelTools.LastKernelErrorException.Message);
        }

    }
}
