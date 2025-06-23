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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Events;
using Nitrocid.Kernel.Journaling;
using Nitrocid.Kernel.Power;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Misc.Splash;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Textify.General;
using Terminaux.Inputs;
using Nitrocid.Files;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Kernel.Exceptions
{
    internal class KernelPanic
    {
        internal static Exception? LastKernelErrorException;
        internal static bool KernelErrored;
        internal static bool NotifyKernelError;


        /// <summary>
        /// Indicates that there's something wrong with the kernel.
        /// </summary>
        /// <param name="ErrorType">Specifies the error type.</param>
        /// <param name="Reboot">Specifies whether to reboot on panic or to show the message to press any key to shut down</param>
        /// <param name="RebootTime">Specifies seconds before reboot. 0 is instant. Negative numbers are not allowed.</param>
        /// <param name="Description">Explanation of what happened when it errored.</param>
        /// <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
        /// <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
        internal static void KernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception? Exc, params object[] Variables)
        {
            KernelErrored = true;
            LastKernelErrorException = Exc ??
                new KernelException(KernelExceptionType.Unknown, Description, Variables);
            NotifyKernelError = true;

            try
            {
                // Unquiet
                KernelEntry.QuietKernel = false;
                JournalManager.WriteJournal(Description, JournalStatus.Fatal, Variables);

                // Check error types and its capabilities
                DebugWriter.WriteDebug(DebugLevel.I, "Error type: {0}", vars: [ErrorType]);
                if (Enum.IsDefined(typeof(KernelErrorLevel), ErrorType))
                {
                    if (ErrorType == KernelErrorLevel.U)
                    {
                        if (RebootTime > 5L)
                        {
                            // If the error type is unrecoverable, or double, and the reboot time exceeds 5 seconds, then
                            // generate a second kernel error stating that there is something wrong with the reboot time.
                            DebugWriter.WriteDebug(DebugLevel.W, "Errors that have type {0} shouldn't exceed 5 seconds. RebootTime was {1} seconds", vars: [ErrorType, RebootTime]);
                            KernelErrorDouble(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DOUBLEPANIC_REBOOTTIME"), null, ((int)ErrorType).ToString());
                            return;
                        }
                        else if (!Reboot)
                        {
                            // If the error type is unrecoverable, or double, and the rebooting is false where it should
                            // not be false, then it can deal with this issue by enabling reboot.
                            DebugWriter.WriteDebug(DebugLevel.W, "Errors that have type {0} enforced Reboot = True.", vars: [ErrorType]);
                            SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_REBOOTENABLEDERRORLEVEL"), ErrorType);
                            Reboot = true;
                        }
                    }
                    if (RebootTime > 3600L)
                    {
                        // If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                        DebugWriter.WriteDebug(DebugLevel.W, "RebootTime shouldn't exceed 1 hour. Was {0} seconds", vars: [RebootTime]);
                        SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_REBOOTEXCEEDSONEHOUR"), ErrorType, RebootTime.ToString());
                        RebootTime = 60L;
                    }
                }
                else
                {
                    // If the error type is other than F/U/S, then it will generate a second error.
                    DebugWriter.WriteDebug(DebugLevel.E, "Error type {0} is not valid.", vars: [ErrorType]);
                    KernelErrorDouble(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DOUBLEPANIC_TYPEINVALID"), null, ((int)ErrorType).ToString());
                    return;
                }

                // Format the "Description" string variable
                Description = TextTools.FormatString(Description, Variables);

                // Fire an event
                EventsManager.FireEvent(EventType.KernelError, ErrorType, Reboot, RebootTime, Description, Exc, Variables);

                // Make a dump file
                GeneratePanicDump(Description, ErrorType, Exc);

                // Do the job
                if (Reboot)
                {
                    // Offer the user to wait for the set time interval before the kernel reboots.
                    DebugWriter.WriteDebug(DebugLevel.F, "Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", vars: [RebootTime, ErrorType]);
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_ERRORREBOOT"), Exc, ErrorType, Description, RebootTime);
                    if (Config.MainConfig.ShowStackTraceOnKernelError && Exc is not null && Exc.StackTrace is not null)
                        SplashReport.ReportProgressError(Exc.StackTrace);
                    Thread.Sleep((int)(RebootTime * 1000L));
                    PowerManager.PowerManage(PowerMode.Reboot);
                }
                else
                {
                    // If rebooting is disabled, offer the user to shutdown the kernel
                    DebugWriter.WriteDebug(DebugLevel.W, "Reboot is False, ErrorType is not double or continuable.");
                    DebugWriter.WriteDebug(DebugLevel.F, "Shutdown panic initiated with reboot time: {0} seconds, Error Type: {1}", vars: [RebootTime, ErrorType]);
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_ERRORSHUTDOWN"), Exc, ErrorType, Description);
                    if (Config.MainConfig.ShowStackTraceOnKernelError && Exc is not null && Exc.StackTrace is not null)
                        SplashReport.ReportProgressError(Exc.StackTrace);
                    Input.ReadKey();
                    PowerManager.PowerManage(PowerMode.Shutdown);
                }
            }
            catch (Exception ex)
            {
                // Alright, we have a double panic.
                DebugWriter.WriteDebug(DebugLevel.F, "DOUBLE PANIC: Kernel bug: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelErrorDouble(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DOUBLEPANIC_KERNELBUG"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Indicates that there's a double fault with the kernel.
        /// </summary>
        /// <param name="Description">Explanation of what happened when it errored.</param>
        /// <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
        /// <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
        private static void KernelErrorDouble(string Description, Exception? Exc, params object[] Variables)
        {
            KernelErrored = true;
            LastKernelErrorException = Exc ??
                new KernelException(KernelExceptionType.Unknown);
            NotifyKernelError = true;

            try
            {
                // Unquiet
                KernelEntry.QuietKernel = false;

                // Format the "Description" string variable
                Description = TextTools.FormatString(Description, Variables);

                // Double panic printed and reboot initiated
                DebugWriter.WriteDebug(DebugLevel.F, "Double panic caused by bug in kernel crash.");
                SplashReport.ReportProgressError("[D] dpanic: " + LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DOUBLEPANIC_MESSAGE"), Description, 5);
                Thread.Sleep(5000);
                DebugWriter.WriteDebug(DebugLevel.F, "Rebooting");
                PowerManager.PowerManage(PowerMode.Reboot);
            }
            catch (Exception ex)
            {
                // Trigger triple fault
                Environment.FailFast("TRIPLE FAULT in trying to handle DOUBLE PANIC. Nitrocid can't continue.", ex);
            }
        }

        /// <summary>
        /// Indicates that there's a continuable error with the kernel.
        /// </summary>
        /// <param name="Description">Explanation of what happened when it errored.</param>
        /// <param name="Exc">An exception to get stack traces, etc. Used for dump files currently.</param>
        /// <param name="Variables">Optional. Specifies variables to get on text that will be printed.</param>
        internal static void KernelErrorContinuable(string Description, Exception? Exc, params object[] Variables)
        {
            try
            {
                // Format the "Description" string variable
                Description = TextTools.FormatString(Description, Variables);

                // Let the user know that there is a continuable kernel error
                EventsManager.FireEvent(EventType.ContKernelError, Description, Exc, Variables);
                DebugWriter.WriteDebug(DebugLevel.W, "Continuable kernel error occurred: {0}. {1}.", vars: [Description, Exc?.Message]);
                SplashReport.ReportProgressWarning(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_CONTINUABLE_MESSAGE") + " {0}", Exc, Description);
                if (Config.MainConfig.ShowStackTraceOnKernelError && Exc is not null && Exc.StackTrace is not null)
                    SplashReport.ReportProgressWarning(Exc.StackTrace);
                SplashReport.ReportProgressWarning(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_CONTINUABLE_WARNING"));
            }
            catch (Exception ex)
            {
                // Alright, we have a double panic.
                DebugWriter.WriteDebug(DebugLevel.F, "DOUBLE PANIC: Kernel bug: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelErrorDouble(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DOUBLEPANIC_KERNELBUG"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Generates the stack trace dump file for kernel panics
        /// </summary>
        /// <param name="Description">Error description</param>
        /// <param name="ErrorType">Error type</param>
        /// <param name="Exc">Exception</param>
        internal static void GeneratePanicDump(string Description, KernelErrorLevel ErrorType, Exception? Exc)
        {
            try
            {
                // Local function for writing header in an appropriate style
                static void WriteHeader(StringBuilder dumpBuilder, string text)
                {
                    dumpBuilder.AppendLine(text);
                    dumpBuilder.AppendLine(new string('=', text.Length));
                    dumpBuilder.AppendLine();
                }

                // Local function to write...
                static void WriteInDepthAnalysis(StringBuilder dumpBuilder, Exception? Exc)
                {
                    if (Exc is null)
                        return;

                    // Write an in-depth analysis of the error
                    WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_INDEPTHANALYSIS"));
                    dumpBuilder.AppendLine(
                        LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_INDEPTHANALYSIS_EXCEPTIONTYPE") + $" {Exc.GetType().FullName}" + CharManager.NewLine +
                        LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_INDEPTHANALYSIS_ERRORCODE") + $": {Exc.HResult} [0x{Exc.HResult:X8}]" + CharManager.NewLine +
                        LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_INDEPTHANALYSIS_ERRORSOURCE") + $": {Exc.Source} [{(Exc.TargetSite is not null ? Exc.TargetSite.Name : LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_INDEPTHANALYSIS_NOSOURCE"))}]" + CharManager.NewLine
                    );

                    // Write a description
                    WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORDESC"));
                    dumpBuilder.AppendLine(Exc.Message + CharManager.NewLine);

                    // Write stack trace
                    WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_STACKTRACE"));
                    dumpBuilder.AppendLine(Exc.StackTrace + CharManager.NewLine);
                }

                // Make a string builder for the dump file
                var dumpBuilder = new StringBuilder();

                // Write the summary of the kernel panic
                WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO"));
                dumpBuilder.AppendLine(
                    LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_TIME") + $" {TimeDateRenderers.Render()}" + CharManager.NewLine +
                    LanguageTools.GetLocalized("NKS_KERNEL_EXCEPTIONS_FINALEXCEPTION_ERRORTYPE") + $": {ErrorType}" + CharManager.NewLine +
                    LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_HASEXCEPTION") + $" {Exc is not null}" + CharManager.NewLine
                );

                // Write an error description
                WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_DESC"));
                dumpBuilder.AppendLine(Description + CharManager.NewLine);

                // Write exception information
                if (Exc is not null)
                {
                    // Do the job.
                    WriteInDepthAnalysis(dumpBuilder, Exc);

                    // Write inner exception information
                    int Count = 1;
                    var InnerExc = Exc.InnerException;
                    while (InnerExc is not null)
                    {
                        WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_INNER") + $" {Count}");
                        WriteInDepthAnalysis(dumpBuilder, InnerExc);
                        InnerExc = InnerExc.InnerException;
                        Count += 1;
                    }
                    dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_INNER_LAST") + $" {Count}" + CharManager.NewLine);

                    // Write frame info for further analysis
                    WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_FRAME"));
                    try
                    {
                        var ExcTrace = new StackTrace(Exc, true);
                        int FrameNum = 1;

                        // If there are frames to print the file information, write them down.
                        if (ExcTrace.FrameCount > 0)
                        {
                            // Get the max lengths for rendering
                            var frames = ExcTrace.GetFrames();
                            int maxFileLength = frames.Where((sf) => !string.IsNullOrEmpty(sf.GetFileName())).Select((sf) => sf.GetFileName() ?? "").Max((sf) => sf.Length);
                            int maxFileLineNumber = frames.Max((sf) => sf.GetFileLineNumber().GetDigits());
                            int maxFileColumnNumber = frames.Max((sf) => sf.GetFileColumnNumber().GetDigits());
                            foreach (StackFrame Frame in frames)
                            {
                                // Get information about each stack frame
                                string fileName = Frame.GetFileName() ?? "";
                                int fileLineNumber = Frame.GetFileLineNumber();
                                int fileColumnNumber = Frame.GetFileColumnNumber();

                                // If we have information, go ahead.
                                if (!string.IsNullOrEmpty(fileName) && fileLineNumber != 0 && fileColumnNumber != 0)
                                {
                                    // Render information
                                    string renderedFileName = $"{fileName}{new string(' ', maxFileLength - fileName.Length)}";
                                    string renderedLineNumber = $"{fileLineNumber}{new string(' ', maxFileLineNumber - fileLineNumber.GetDigits())}";
                                    string renderedColumnNumber = $"{fileColumnNumber}{new string(' ', maxFileColumnNumber - fileColumnNumber.GetDigits())}";
                                    dumpBuilder.AppendLine($"[{FrameNum}] {renderedFileName} | {renderedLineNumber}:{renderedColumnNumber}");
                                }
                                FrameNum += 1;
                            }
                        }
                        else
                            dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_FRAME_NOFRAMES"));
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Can't analyze frames: ", vars: [ex.Message]);
                        DebugWriter.WriteDebugStackTrace(ex);
                        dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_FRAME_FRAMESFAILED") + $" {ex.Message}");
                    }
                }
                else
                    dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_ERRORINFO_NOINFO"));
                dumpBuilder.AppendLine();

                // All kernel threads
                WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS"));
                try
                {
                    var threads = ThreadManager.KernelThreads;
                    foreach (var thread in threads)
                    {
                        dumpBuilder.AppendLine($"[{thread.ThreadId}] {thread.Name}:");
                        dumpBuilder.AppendLine($"  - {LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS_ALIVE")}: {thread.IsAlive}");
                        dumpBuilder.AppendLine($"  - {LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS_BG")}: {thread.IsBackground}");
                        dumpBuilder.AppendLine($"  - {LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS_CRITICAL")}: {thread.IsCritical}");
                        dumpBuilder.AppendLine($"  - {LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS_READY")}: {thread.IsReady}");
                        dumpBuilder.AppendLine($"  - {LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS_STOPPING")}: {thread.IsStopping}");
                        dumpBuilder.AppendLine();
                    }
                    dumpBuilder.AppendLine($"{LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS_TOTAL")}: {threads.Count}");
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Can't analyze threads: ", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_KERNELTHREADS_FAILED") + $" {ex.Message}");
                }
                dumpBuilder.AppendLine();

                // All operating system threads
                WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_OSTHREADS"));
                try
                {
                    var threads = ThreadManager.OperatingSystemThreads;
                    foreach (ProcessThread thread in threads)
                        dumpBuilder.AppendLine($"[{thread.Id}] 0x{thread.StartAddress:X16}");
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Can't analyze OS threads: ", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_OSTHREADS_FAILED") + $" {ex.Message}");
                }
                dumpBuilder.AppendLine();

                // All thread backtraces
                WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_BACKTRACES"));
                try
                {
                    Dictionary<string, string[]> result = ThreadManager.GetThreadBacktraces();
                    if (result.Count == 0)
                    {
                        dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_BACKTRACES_EMPTY"));
                        dumpBuilder.AppendLine();
                    }
                    foreach (var trace in result)
                    {
                        string threadAddress = trace.Key;
                        string[] threadTrace = trace.Value;
                        WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_BACKTRACES_STACKTRACE") + $" {threadAddress}");
                        foreach (var traceVal in threadTrace)
                            dumpBuilder.AppendLine(traceVal);
                        dumpBuilder.AppendLine();
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Can't analyze thread backtraces: ", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    dumpBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_BACKTRACES_STACKTRACE_FAILED") + $" {ex.Message}");
                    dumpBuilder.AppendLine();
                }

                // Versions
                WriteHeader(dumpBuilder, LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_VERSIONINFO"));
                dumpBuilder.AppendLine(KernelReleaseInfo.ConsoleTitle);
                dumpBuilder.AppendLine(Environment.OSVersion.ToString());

                // Save the dump file
                string filePath = $"{PathsManagement.AppDataPath}/dmp_{TimeDateRenderers.RenderDate(FormatType.Short).Replace("/", "-")}_{TimeDateRenderers.RenderTime(FormatType.Long).Replace(":", "-")}.txt";
                FilesystemTools.WriteContentsText(filePath, dumpBuilder.ToString());
                DebugWriter.WriteDebug(DebugLevel.I, "Opened file stream in home directory, saved as {0}", vars: [filePath]);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_DUMP_DUMPFAILED") + " {0}: {1}", true, ThemeColorType.Error, Exc?.GetType()?.FullName ?? "<null>", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void NotifyBootFailure()
        {
            if (NotifyKernelError)
            {
                string translated = LanguageTools.GetLocalized("NKS_KERNEL_PANIC_NOTIFY_BOOTFAILED");
                var failureBuilder = new StringBuilder();
                NotifyKernelError = false;
                string finalMessage =
                    LastKernelErrorException is not null ?
                    LastKernelErrorException.Message :
                    LanguageTools.GetLocalized("NKS_KERNEL_PANIC_NOTIFY_UNKNOWN");
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
                failureBuilder.AppendLine(translated);
                failureBuilder.AppendLine(new string('=', translated.Length) + "\n");
                failureBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_NOTIFY_DESCRIPTION") + "\n");
                failureBuilder.AppendLine(finalMessage + "\n");
                failureBuilder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_PANIC_NOTIFY_TIP"));
                InfoBoxModalColor.WriteInfoBoxModal(failureBuilder.ToString(), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
                SplashManager.EndSplashOut(SplashManager.CurrentSplashContext);
            }
        }
    }
}
