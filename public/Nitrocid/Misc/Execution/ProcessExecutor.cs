
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
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Kernel.Events;

namespace KS.Misc.Execution
{
    /// <summary>
    /// Process executor module
    /// </summary>
    public static class ProcessExecutor
    {

        internal static string ProcessData = "";
        internal static bool NewDataSpotted;
        internal static KernelThread NewDataDetector = new("New data detection for process", false, DetectNewData);

        /// <summary>
        /// Thread parameters for ExecuteProcess()
        /// </summary>
        internal class ExecuteProcessThreadParameters
        {
            /// <summary>
            /// Full path to file
            /// </summary>
            internal string File;
            /// <summary>
            /// Arguments, if any
            /// </summary>
            internal string Args;

            internal ExecuteProcessThreadParameters(string File, string Args)
            {
                this.File = File;
                this.Args = Args;
            }
        }

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        internal static void ExecuteProcess(ExecuteProcessThreadParameters ThreadParams) => ExecuteProcess(ThreadParams.File, ThreadParams.Args);

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static int ExecuteProcess(string File, string Args) => ExecuteProcess(File, Args, CurrentDirectory.CurrentDir);

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="WorkingDirectory">Specifies the working directory</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static int ExecuteProcess(string File, string Args, string WorkingDirectory)
        {
            try
            {
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = File,
                    Arguments = Args,
                    WorkingDirectory = WorkingDirectory,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                };
                CommandProcess.StartInfo = CommandProcessStart;
                CommandProcess.OutputDataReceived += ExecutableOutput;
                CommandProcess.ErrorDataReceived += ExecutableOutput;

                // Start the process
                DebugWriter.WriteDebug(DebugLevel.I, "Starting...");
                CommandProcess.Start();
                CommandProcess.BeginOutputReadLine();
                CommandProcess.BeginErrorReadLine();
                NewDataDetector.Start();

                // Wait for process exit
                while (!CommandProcess.HasExited | !Flags.CancelRequested)
                {
                    if (CommandProcess.HasExited)
                    {
                        break;
                    }
                    else if (Flags.CancelRequested)
                    {
                        CommandProcess.Kill();
                        break;
                    }
                }

                // Wait until no more data is entered
                while (NewDataSpotted)
                {
                }

                // Stop the new data detector
                NewDataDetector.Stop();
                ProcessData = "";

                // Assume that we've spotted new data. This is to avoid race conditions happening sometimes if the processes are exited while output is still going.
                // This is a workaround for some commands like netstat.exe that don't work with normal workarounds shown below.
                NewDataSpotted = true;
                return CommandProcess.ExitCode;
            }
            catch (ThreadInterruptedException)
            {
                Flags.CancelRequested = false;
                return default;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ProcessError, File + Args, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message, File);
            }
            return -1;
        }

        /// <summary>
        /// Handles executable output
        /// </summary>
        /// <param name="sendingProcess">Sender</param>
        /// <param name="outLine">Output</param>
        private static void ExecutableOutput(object sendingProcess, DataReceivedEventArgs outLine)
        {
            NewDataSpotted = true;
            DebugWriter.WriteDebug(DebugLevel.I, outLine.Data);
            TextWriterColor.Write(outLine.Data);
            ProcessData += outLine.Data;
        }

        /// <summary>
        /// Detects new data
        /// </summary>
        private static void DetectNewData()
        {
            try
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    if (NewDataSpotted)
                    {
                        long OldLength = ProcessData.LongCount();
                        Thread.Sleep(50);
                        if (OldLength == ProcessData.LongCount())
                            NewDataSpotted = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is not ThreadInterruptedException)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Error processing data: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

    }
}
