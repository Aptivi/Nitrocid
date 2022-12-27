
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Kernel.Administration.Journalling;
using KS.Kernel.Debugging;
using KS.Kernel.Events;
using KS.Kernel.Power;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.TimeDate;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace KS.Kernel.Exceptions
{
    internal class KernelPanic
    {
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
        internal static void KernelError(KernelErrorLevel ErrorType, bool Reboot, long RebootTime, string Description, Exception Exc, params object[] Variables)
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
                            TextWriterColor.Write(Translate.DoTranslation("DOUBLE PANIC: Reboot Time exceeds maximum allowed {0} error reboot time. You found a kernel bug."), true, KernelColorType.UncontKernelError, ((int)ErrorType).ToString());
                            return;
                        }
                        else if (!Reboot)
                        {
                            // If the error type is unrecoverable, or double, and the rebooting is false where it should
                            // not be false, then it can deal with this issue by enabling reboot.
                            DebugWriter.WriteDebug(DebugLevel.W, "Errors that have type {0} enforced Reboot = True.", ErrorType);
                            TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Reboot enabled due to error level being {0}."), true, KernelColorType.UncontKernelError, ErrorType);
                            Reboot = true;
                        }
                    }
                    if (RebootTime > 3600L)
                    {
                        // If the reboot time exceeds 1 hour, then it will set the time to 1 minute.
                        DebugWriter.WriteDebug(DebugLevel.W, "RebootTime shouldn't exceed 1 hour. Was {0} seconds", RebootTime);
                        TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Time to reboot: {1} seconds, exceeds 1 hour. It is set to 1 minute."), true, KernelColorType.UncontKernelError, ErrorType, RebootTime.ToString());
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
                EventsManager.FireEvent(EventType.KernelError, ErrorType, Reboot, RebootTime, Description, Exc, Variables);

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
                                TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: Reboot disabled due to error level being {0}."), true, KernelColorType.Warning, ErrorType);
                            }
                            // Print normally
                            EventsManager.FireEvent(EventType.ContKernelError, ErrorType, Reboot, RebootTime, Description, Exc, Variables);
                            TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Press any key to continue using the kernel."), true, KernelColorType.ContKernelError, ErrorType, Description);
                            if (Flags.ShowStackTraceOnKernelError & Exc is not null)
                                TextWriterColor.Write(Exc.StackTrace, true, KernelColorType.ContKernelError);
                            Input.DetectKeypress();
                            break;
                        }

                    default:
                        {
                            if (Reboot)
                            {
                                // Offer the user to wait for the set time interval before the kernel reboots.
                                DebugWriter.WriteDebug(DebugLevel.F, "Kernel panic initiated with reboot time: {0} seconds, Error Type: {1}", RebootTime, ErrorType);
                                TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Rebooting in {2} seconds..."), true, KernelColorType.UncontKernelError, ErrorType, Description, RebootTime.ToString());
                                if (Flags.ShowStackTraceOnKernelError & Exc is not null)
                                    TextWriterColor.Write(Exc.StackTrace, true, KernelColorType.UncontKernelError);
                                Thread.Sleep((int)(RebootTime * 1000L));
                                PowerManager.PowerManage(PowerMode.Reboot);
                            }
                            else
                            {
                                // If rebooting is disabled, offer the user to shutdown the kernel
                                DebugWriter.WriteDebug(DebugLevel.W, "Reboot is False, ErrorType is not double or continuable.");
                                TextWriterColor.Write(Translate.DoTranslation("[{0}] panic: {1} -- Press any key to shutdown."), true, KernelColorType.UncontKernelError, ErrorType, Description);
                                if (Flags.ShowStackTraceOnKernelError & Exc is not null)
                                    TextWriterColor.Write(Exc.StackTrace, true, KernelColorType.UncontKernelError);
                                Input.DetectKeypress();
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
                TextWriterColor.Write("[D] dpanic: " + Translate.DoTranslation("{0} -- Rebooting in {1} seconds..."), true, KernelColorType.UncontKernelError, Description, 5);
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
        internal static void GeneratePanicDump(string Description, KernelErrorLevel ErrorType, Exception Exc)
        {
            try
            {
                // Open a file stream for dump
                var Dump = new StreamWriter($"{Paths.AppDataPath}/dmp_{TimeDateRenderers.RenderDate(TimeDate.TimeDate.FormatType.Short).Replace("/", "-")}_{TimeDateRenderers.RenderTime(TimeDate.TimeDate.FormatType.Long).Replace(":", "-")}.txt");
                DebugWriter.WriteDebug(DebugLevel.I, "Opened file stream in home directory, saved as dmp_{0}.txt", $"{TimeDateRenderers.RenderDate(TimeDate.TimeDate.FormatType.Short).Replace("/", "-")}_{TimeDateRenderers.RenderTime(TimeDate.TimeDate.FormatType.Long).Replace(":", "-")}");

                // Write info (Header)
                Dump.AutoFlush = true;
                Dump.WriteLine(Translate.DoTranslation("----------------------------- Kernel panic dump -----------------------------") + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation(">> Panic information <<") + CharManager.NewLine + Translate.DoTranslation("> Description: {0}") + CharManager.NewLine + Translate.DoTranslation("> Error type: {1}") + CharManager.NewLine + Translate.DoTranslation("> Date and Time: {2}") + CharManager.NewLine, Description, ErrorType.ToString(), TimeDateRenderers.Render());

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
                TextWriterColor.Write(Translate.DoTranslation("Dump information gatherer crashed when trying to get information about {0}: {1}"), true, KernelColorType.Error, Exc.GetType().FullName, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }
    }
}
