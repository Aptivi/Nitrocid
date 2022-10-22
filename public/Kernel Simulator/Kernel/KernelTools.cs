
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using KS.Arguments;
using KS.Arguments.ArgumentBase;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Hardware;
using KS.Kernel.Administration.Journalling;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Power;
using KS.Languages;
using KS.Misc.Calendar.Events;
using KS.Misc.Calendar.Reminders;
using KS.Misc.Notifications;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.MiscWriters;
using KS.Modifications;
using KS.Network.Mail;
using KS.Network.RPC;
using KS.Scripting;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;
using KS.Users;
using KS.Users.Groups;
using static System.Net.Mime.MediaTypeNames;

#if SPECIFIERREL
using static KS.ConsoleBase.Colors.ColorTools;
using static KS.Misc.Notifications.Notifications;
using KS.Network;
using KS.Network.Transfer;
using System.Reflection;
#endif

namespace KS.Kernel
{
    /// <summary>
    /// Kernel tools module
    /// </summary>
    public static class KernelTools
    {

        /// <summary>
        /// Current banner figlet font
        /// </summary>
        public static string BannerFigletFont = "Banner";
        internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (object arg) => PowerManager.PowerManage((PowerMode)arg));
        internal static Exception LastKernelErrorException;

        // ----------------------------------------------- Kernel errors -----------------------------------------------

        /// <summary>
        /// Indicates that there's something wrong with the kernel.
        /// </summary>
        /// <param name="ErrorType">Specifies the error type.</param>
        /// <param name="Reboot">Specifies whether to reboot on panic or to show the message to press any key to shut down</param>
        /// <param name="RebootTime">Specifies seconds before reboot. 0 is instant. Negative numbers are not allowed.</param>
        /// <param name="Description">Explanation of what happened when it errored.</param>
        /// <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
        /// <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
        public static void KernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, params object[] Variables)
        {
            Flags.KernelErrored = true;
            LastKernelErrorException = Exc;
            Flags.NotifyKernelError = true;

            try
            {
                // Unquiet
                Flags.QuietKernel = false;
                JournalManager.WriteJournal(Description, JournalStatus.Fatal, Variables);

                // Check error types and its capabilities
                DebugWriter.WriteDebug(DebugLevel.I, "Error type: {0}", ErrorType);
                if (Enum.IsDefined(typeof(KernelErrorLevel), ErrorType))
                {
                    if (ErrorType == KernelErrorLevel.U)
                    {
                        if (RebootTime > 5L)
                        {
                            // If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                            // generate a second kernel error stating that there is something wrong with the reboot time.
                            DebugWriter.WriteDebug(DebugLevel.W, "Errors that have type {0} shouldn't exceed 5 seconds. RebootTime was {1} seconds", ErrorType, RebootTime);
                            TextWriterColor.Write(Translate.DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug."), true, ColorTools.ColTypes.UncontKernelError, ((int)ErrorType).ToString());
                            return;
                        }
                        else if (!Reboot)
                        {
                            // If the error type is unrecoverable, or double, and the rebooting is false where it should
                            // not be false, then it can deal with this issue by enabling reboot.
                            DebugWriter.WriteDebug(DebugLevel.W, "Errors that have type {0} enforced Reboot = True.", ErrorType);
                            TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}."), true, ColorTools.ColTypes.UncontKernelError, ErrorType);
                            Reboot = true;
                        }
                    }
                    if (RebootTime > 3600L)
                    {
                        // If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                        DebugWriter.WriteDebug(DebugLevel.W, "RebootTime shouldn't exceed 1 hour. Was {0} seconds", RebootTime);
                        TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute."), true, ColorTools.ColTypes.UncontKernelError, ErrorType, RebootTime.ToString());
                        RebootTime = 60L;
                    }
                }
                else
                {
                    // If the error type is other than D/F/C/U/S, then it will generate a second error.
                    DebugWriter.WriteDebug(DebugLevel.E, "Error type {0} is not valid.", ErrorType);
                    KernelErrorDouble(Translate.DoTranslation("DOUBLE PANIC: Error Type {0} invalid."), null, ((int)ErrorType).ToString());
                    return;
                }

                // Format the "Description" string variable
                Description = StringManipulate.FormatString(Description, Variables);

                // Fire an event
                Events.EventsManager.FireEvent("KernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables);

                // Make a dump file
                GeneratePanicDump(Description, ErrorType, Exc);

                // Check error type
                switch (ErrorType)
                {
                    case KernelErrorLevel.C:
                        {
                            if (Reboot)
                            {
                                // Continuable kernel errors shouldn't cause the kernel to reboot.
                                DebugWriter.WriteDebug(DebugLevel.W, "Continuable kernel errors shouldn't have Reboot = True.");
                                TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}."), true, ColorTools.ColTypes.Warning, ErrorType);
                            }
                            // Print normally
                            Events.EventsManager.FireEvent("ContKernelError", ErrorType, Reboot, RebootTime, Description, Exc, Variables);
                            TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), true, ColorTools.ColTypes.ContKernelError, ErrorType, Description);
                            if (Flags.ShowStackTraceOnKernelError & Exc is not null)
                                TextWriterColor.Write(Exc.StackTrace, true, ColorTools.ColTypes.ContKernelError);
                            ConsoleWrapper.ReadKey();
                            break;
                        }

                    default:
                        {
                            if (Reboot)
                            {
                                // Offer the user to wait for the set time interval before the kernel reboots.
                                DebugWriter.WriteDebug(DebugLevel.F, "Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", RebootTime, ErrorType);
                                TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds..."), true, ColorTools.ColTypes.UncontKernelError, ErrorType, Description, RebootTime.ToString());
                                if (Flags.ShowStackTraceOnKernelError & Exc is not null)
                                    TextWriterColor.Write(Exc.StackTrace, true, ColorTools.ColTypes.UncontKernelError);
                                Thread.Sleep((int)(RebootTime * 1000L));
                                PowerManager.PowerManage(PowerMode.Reboot);
                            }
                            else
                            {
                                // If rebooting is disabled, offer the user to shutdown the kernel
                                DebugWriter.WriteDebug(DebugLevel.W, "Reboot is False, ErrorType is not double or continuable.");
                                TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Press any key to shutdown."), true, ColorTools.ColTypes.UncontKernelError, ErrorType, Description);
                                if (Flags.ShowStackTraceOnKernelError & Exc is not null)
                                    TextWriterColor.Write(Exc.StackTrace, true, ColorTools.ColTypes.UncontKernelError);
                                ConsoleWrapper.ReadKey();
                                PowerManager.PowerManage(PowerMode.Shutdown);
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                // Alright, we have a double panic.
                DebugWriter.WriteDebug(DebugLevel.F, "DOUBLE PANIC: Kernel bug: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelErrorDouble(Translate.DoTranslation("DOUBLE PANIC: Kernel bug: {0}"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Indicates that there's a double fault with the kernel.
        /// </summary>
        /// <param name="Description">Explanation of what happened when it errored.</param>
        /// <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
        /// <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
        private static void KernelErrorDouble(string Description, Exception Exc, params object[] Variables)
        {
            Flags.KernelErrored = true;
            LastKernelErrorException = Exc;
            Flags.NotifyKernelError = true;

            try
            {
                // Unquiet
                Flags.QuietKernel = false;

                // Format the "Description" string variable
                Description = StringManipulate.FormatString(Description, Variables);

                // Double panic printed and reboot initiated
                DebugWriter.WriteDebug(DebugLevel.F, "Double panic caused by bug in kernel crash.");
                TextWriterColor.Write("[D] dpanic: " + Translate.DoTranslation("{0} -- Rebooting in {1} seconds..."), true, ColorTools.ColTypes.UncontKernelError, Description, 5);
                Thread.Sleep(5000);
                DebugWriter.WriteDebug(DebugLevel.F, "Rebooting");
                PowerManager.PowerManage(PowerMode.Reboot);
            }
            catch (Exception ex)
            {
                // Trigger triple fault
                Environment.FailFast("TRIPLE FAULT in trying to handle DOUBLE PANIC. KS can't continue.", ex);
            }
        }

        /// <summary>
        /// Generates the stack trace dump file for kernel panics
        /// </summary>
        /// <param name="Description">Error description</param>
        /// <param name="ErrorType">Error type</param>
        /// <param name="Exc">Exception</param>
        public static void GeneratePanicDump(string Description, KernelErrorLevel ErrorType, Exception Exc)
        {
            try
            {
                // Open a file stream for dump
                var Dump = new StreamWriter($"{Paths.AppDataPath}/dmp_{TimeDateRenderers.RenderDate(TimeDate.TimeDate.FormatType.Short).Replace("/", "-")}_{TimeDateRenderers.RenderTime(TimeDate.TimeDate.FormatType.Long).Replace(":", "-")}.txt");
                DebugWriter.WriteDebug(DebugLevel.I, "Opened file stream in home directory, saved as dmp_{0}.txt", $"{TimeDateRenderers.RenderDate(TimeDate.TimeDate.FormatType.Short).Replace("/", "-")}_{TimeDateRenderers.RenderTime(TimeDate.TimeDate.FormatType.Long).Replace(":", "-")}");

                // Write info (Header)
                Dump.AutoFlush = true;
                Dump.WriteLine(Translate.DoTranslation("----------------------------- Kernel panic dump -----------------------------") + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation(">> Panic information <<") + CharManager.NewLine + Translate.DoTranslation("> Description: {0}") + CharManager.NewLine + Translate.DoTranslation("> Error type: {1}") + CharManager.NewLine + Translate.DoTranslation("> Date and Time: {2}") + CharManager.NewLine + Translate.DoTranslation("> Framework Type: {3}") + CharManager.NewLine, Description, ErrorType.ToString(), TimeDateRenderers.Render(), Kernel.KernelSimulatorMoniker);

                // Write Info (Exception)
                if (Exc is not null)
                {
                    int Count = 1;
                    Dump.WriteLine(Translate.DoTranslation(">> Exception information <<") + CharManager.NewLine + Translate.DoTranslation("> Exception: {0}") + CharManager.NewLine + Translate.DoTranslation("> Description: {1}") + CharManager.NewLine + Translate.DoTranslation("> HRESULT: {2}") + CharManager.NewLine + Translate.DoTranslation("> Source: {3}") + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("> Stack trace <") + CharManager.NewLine + CharManager.NewLine + Exc.StackTrace + CharManager.NewLine + CharManager.NewLine, Exc.GetType().FullName, Exc.Message, Exc.HResult, Exc.Source);
                    Dump.WriteLine(Translate.DoTranslation(">> Inner exception {0} information <<"), Count);

                    // Write info (Inner exceptions)
                    var InnerExc = Exc.InnerException;
                    while (InnerExc is not null)
                    {
                        Dump.WriteLine(Translate.DoTranslation("> Exception: {0}") + CharManager.NewLine + Translate.DoTranslation("> Description: {1}") + CharManager.NewLine + Translate.DoTranslation("> HRESULT: {2}") + CharManager.NewLine + Translate.DoTranslation("> Source: {3}") + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("> Stack trace <") + CharManager.NewLine + CharManager.NewLine + InnerExc.StackTrace + CharManager.NewLine, InnerExc.GetType().FullName, InnerExc.Message, InnerExc.HResult, InnerExc.Source);
                        InnerExc = InnerExc.InnerException;
                        if (InnerExc is not null)
                        {
                            Dump.WriteLine(Translate.DoTranslation(">> Inner exception {0} information <<"), Count);
                        }
                        else
                        {
                            Dump.WriteLine(Translate.DoTranslation(">> Exception {0} is the root cause <<"), Count - 1);
                        }
                        Count += 1;
                    }
                    Dump.WriteLine();
                }
                else
                {
                    Dump.WriteLine(Translate.DoTranslation(">> No exception; might be a kernel error. <<") + CharManager.NewLine);
                }

                // Write info (Frames)
                Dump.WriteLine(Translate.DoTranslation(">> Frames, files, lines, and columns <<"));
                try
                {
                    var ExcTrace = new StackTrace(Exc, true);
                    int FrameNo = 1;

                    // If there are frames to print the file information, write them down to the dump file.
                    if (ExcTrace.FrameCount != 0)
                    {
                        foreach (StackFrame Frame in ExcTrace.GetFrames())
                        {
                            if (!(string.IsNullOrEmpty(Frame.GetFileName()) & Frame.GetFileLineNumber() == 0 & Frame.GetFileColumnNumber() == 0))
                            {
                                Dump.WriteLine(Translate.DoTranslation("> Frame {0}: File: {1} | Line: {2} | Column: {3}"), FrameNo, Frame.GetFileName(), Frame.GetFileLineNumber(), Frame.GetFileColumnNumber());
                            }
                            FrameNo += 1;
                        }
                    }
                    else
                    {
                        Dump.WriteLine(Translate.DoTranslation("> There are no information about frames."));
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    Dump.WriteLine(Translate.DoTranslation("> There is an error when trying to get frame information. {0}: {1}"), ex.GetType().FullName, ex.Message.Replace(CharManager.NewLine, " | "));
                }

                // Close stream
                DebugWriter.WriteDebug(DebugLevel.I, "Closing file stream for dump...");
                Dump.Flush();
                Dump.Close();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Dump information gatherer crashed when trying to get information about {0}: {1}"), true, ColorTools.ColTypes.Error, Exc.GetType().FullName, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        // ----------------------------------------------- Init and reset -----------------------------------------------
        /// <summary>
        /// Reset everything for the next restart
        /// </summary>
        public static void ResetEverything()
        {
            // Reset every variable below
            if (Flags.ArgsInjected == false)
                ArgumentPrompt.EnteredArguments.Clear();
            GroupManagement.UserGroups.Clear();
            ReminderManager.Reminders.Clear();
            EventManager.CalendarEvents.Clear();
            Flags.ArgsOnBoot = false;
            Flags.SafeMode = false;
            Flags.QuietKernel = false;
            Flags.Maintenance = false;
            SplashReport._Progress = 0;
            SplashReport._ProgressText = "";
            SplashReport._KernelBooted = false;
            DebugWriter.WriteDebug(DebugLevel.I, "General variables reset");

            // Reset hardware info
            HardwareProbe.HardwareInfo = null;
            DebugWriter.WriteDebug(DebugLevel.I, "Hardware info reset.");

            // Disconnect all hosts from remote debugger
            RemoteDebugger.StopRDebugThread();
            DebugWriter.WriteDebug(DebugLevel.I, "Remote debugger stopped");

            // Stop all mods
            ModManager.StopMods();
            DebugWriter.WriteDebug(DebugLevel.I, "Mods stopped");

            // Disable Debugger
            if (Flags.DebugMode)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Shutting down debugger");
                Flags.DebugMode = false;
                DebugWriter.DebugStreamWriter.Close();
                DebugWriter.DebugStreamWriter.Dispose();
            }

            // Stop RPC
            RemoteProcedure.StopRPC();

            // Disconnect from mail
            MailLogin.IMAP_Client.Disconnect(true);
            MailLogin.SMTP_Client.Disconnect(true);

            // Unload all splashes
            SplashManager.UnloadSplashes();

            // Disable safe mode
            Flags.SafeMode = false;

            // Stop the time/date change thread
            TimeDateTopRight.TimeTopRightChange.Stop();
        }

        /// <summary>
        /// Initializes everything
        /// </summary>
        public static void InitEverything()
        {
            // Initialize notifications
            if (!Notifications.NotifThread.IsAlive)
                Notifications.NotifThread.Start();

            // Initialize events and reminders
            if (!ReminderManager.ReminderThread.IsAlive)
                ReminderManager.ReminderThread.Start();
            if (!EventManager.EventThread.IsAlive)
                EventManager.EventThread.Start();

            // Initialize console resize listener
            ConsoleResizeListener.StartResizeListener();

            // Install cancellation handler
            if (!Flags.CancellationHandlerInstalled)
                Console.CancelKeyPress += CancellationHandlers.CancelCommand;

            // Initialize aliases
            AliasManager.InitAliases();

            // Initialize custom languages
            LanguageManager.InstallCustomLanguages();

            // Initialize splashes
            SplashManager.LoadSplashes();

            // Read failsafe config
            if (Flags.SafeMode)
                Config.ReadFailsafeConfig();

            // Create config file and then read it
            Config.InitializeConfig();

            // Load background
            ColorTools.LoadBack();

            // Initialize top right date
            TimeDateTopRight.InitTopRightDate();

            // Load user token
            UserManagement.LoadUserToken();

            // Show welcome message.
            WelcomeMessage.WriteMessage();

            // Some information
            if (Flags.ShowAppInfoOnBoot & !Flags.EnableSplash)
            {
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("App information"), true, ColorTools.ColTypes.Stage);
                TextWriterColor.Write("OS: " + Translate.DoTranslation("Running on {0}"), true, ColorTools.ColTypes.NeutralText, Environment.OSVersion.ToString());
                TextWriterColor.Write("KS: " + Translate.DoTranslation("Running from GRILO?") + " {0}", true, ColorTools.ColTypes.NeutralText, KernelPlatform.IsRunningFromGrilo());
            }

            // Check to see if running on macOS, since we no longer support it.
            // WARNING: Never localize the message as it's most likely to be removed.
            if (KernelPlatform.IsOnMacOS())
                TextWriterColor.Write("* You're running on macOS. This is not supported and may or may no longer work starting on January 1st, 2023. Until further notice, all support coming from macOS are denied.", true, ColorTools.ColTypes.Warning);

            // Check arguments
            if (Flags.ArgsOnBoot)
            {
                Kernel.StageTimer.Stop();
                ArgumentPrompt.PromptArgs();
                Kernel.StageTimer.Start();
            }
            if (Flags.ArgsInjected)
            {
                Flags.ArgsInjected = false;
                ArgumentParse.ParseArguments(ArgumentPrompt.EnteredArguments, ArgumentType.KernelArgs);
            }

            // Load splash
            SplashManager.OpenSplash();

            // Populate ban list for debug devices
            RemoteDebugTools.PopulateBlockedDevices();

            // Start screensaver timeout
            if (!Screensaver.Timeout.IsAlive)
                Screensaver.Timeout.Start();

            // Load all events and reminders
            EventManager.LoadEvents();
            ReminderManager.LoadReminders();

            // Load system env vars and convert them
            UESHVariables.ConvertSystemEnvironmentVariables();
        }

        // ----------------------------------------------- Misc -----------------------------------------------

        /// <summary>
        /// Removes all configuration files
        /// </summary>
        public static void FactoryReset()
        {
            // Delete every single thing found in KernelPaths
            foreach (string PathName in Enum.GetNames(typeof(KernelPathType)))
            {
                string TargetPath = Paths.GetKernelPath((KernelPathType)Convert.ToInt32(PathName));
                if (Checking.FileExists(TargetPath))
                {
                    File.Delete(TargetPath);
                }
                else
                {
                    Directory.Delete(TargetPath, true);
                }
            }

            // Exit now.
            Environment.Exit(0);
        }

        /// <summary>
        /// Reports the new kernel stage
        /// </summary>
        /// <param name="StageNumber">The stage number</param>
        /// <param name="StageText">The stage text</param>
        public static void ReportNewStage(int StageNumber, string StageText)
        {
            // Show the stage finish times
            if (StageNumber <= 1)
            {
                if (Flags.ShowStageFinishTimes)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Internal initialization finished in") + $" {Kernel.StageTimer.Elapsed}", 0, ColorTools.ColTypes.StageTime);
                    Kernel.StageTimer.Restart();
                }
            }
            else if (StageNumber >= 5)
            {
                if (Flags.ShowStageFinishTimes)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {Kernel.StageTimer.Elapsed}", 10, ColorTools.ColTypes.StageTime);
                    Kernel.StageTimer.Reset();
                    TextWriterColor.Write();
                }
            }
            else if (Flags.ShowStageFinishTimes)
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Stage finished in") + $" {Kernel.StageTimer.Elapsed}", 10, ColorTools.ColTypes.StageTime);
                Kernel.StageTimer.Restart();
            }

            // Actually report the stage
            if (StageNumber >= 1 & StageNumber <= 4)
            {
                if (!Flags.EnableSplash & !Flags.QuietKernel)
                {
                    TextWriterColor.Write();
                    SeparatorWriterColor.WriteSeparator(StageText, false, ColorTools.ColTypes.Stage);
                }
                DebugWriter.WriteDebug(DebugLevel.I, $"- Kernel stage {StageNumber} | Text: {StageText}");
            }
        }

        /// <summary>
        /// Gets the used compiler variables for building Kernel Simulator
        /// </summary>
        /// <returns>An array containing used compiler variables</returns>
        public static string[] GetCompilerVars()
        {
            var CompilerVars = new List<string>();

            // Determine the compiler vars used to build KS using conditional checks
#if SPECIFIERDEV
            CompilerVars.Add("SPECIFIER = \"DEV\"");
#if MILESTONESPECIFIERALPHA
            CompilerVars.Add("MILESTONESPECIFIERALPHA");
#elif MILESTONESPECIFIERBETA
            CompilerVars.Add("MILESTONESPECIFIERBETA");
#endif
#elif SPECIFIERRC
            CompilerVars.Add("SPECIFIER = \"RC\"");
#elif SPECIFIERREL
            CompilerVars.Add("SPECIFIER = \"REL\"");
#endif

            // Return the compiler vars
            return CompilerVars.ToArray();
        }

        /// <summary>
        /// Checks for debug symbols and downloads it if not found. It'll be auto-loaded upon download.
        /// </summary>
        public static void CheckDebugSymbols()
        {
#if SPECIFIERREL
			if (!NetworkTools.NetworkAvailable)
			{
				NotifySend(new Notification(Translate.DoTranslation("No network while downloading debug data"), Translate.DoTranslation("Check your internet connection and try again."), NotifPriority.Medium, NotifType.Normal));
			}
			if (NetworkTools.NetworkAvailable)
			{
				//Check to see if we're running from Ubuntu PPA
				bool PPASpotted = Paths.ExecPath.StartsWith("/usr/lib/ks");
				if (PPASpotted)
					SplashReport.ReportProgressError(Translate.DoTranslation("Use apt to update Kernel Simulator."));

				//Download debug symbols
				if (!Checking.FileExists(Assembly.GetExecutingAssembly().Location.Replace(".exe", ".pdb")) & !PPASpotted)
				{
					try
					{
#if NETCOREAPP
						NetworkTransfer.DownloadFile($"https://github.com/Aptivi/Kernel-Simulator/releases/download/v{Kernel.KernelVersion}-beta/{Kernel.KernelVersion}-dotnet.pdb", Assembly.GetExecutingAssembly().Location.Replace(".exe", ".pdb"));
#else
                        NetworkTransfer.DownloadFile($"https://github.com/Aptivi/Kernel-Simulator/releases/download/v{Kernel.KernelVersion}-beta/{Kernel.KernelVersion}.pdb", Assembly.GetExecutingAssembly().Location.Replace(".exe", ".pdb"));
#endif
					}
					catch (Exception)
					{
						NotifySend(new Notification(Translate.DoTranslation("Error downloading debug data"), Translate.DoTranslation("There is an error while downloading debug data. Check your internet connection."), NotifPriority.Medium, NotifType.Normal));
					}
				}
			}
#endif
        }

    }
}
