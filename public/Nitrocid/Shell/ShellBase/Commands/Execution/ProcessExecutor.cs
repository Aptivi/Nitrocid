
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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Kernel.Events;
using System.Text;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration;

namespace KS.Shell.ShellBase.Commands.Execution
{
    /// <summary>
    /// Process executor module
    /// </summary>
    public static class ProcessExecutor
    {

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
        internal static void ExecuteProcess(ExecuteProcessThreadParameters ThreadParams) =>
            ExecuteProcess(ThreadParams.File, ThreadParams.Args);

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static int ExecuteProcess(string File, string Args) =>
            ExecuteProcess(File, Args, CurrentDirectory.CurrentDir);

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
                bool HasProcessExited = false;
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
                CommandProcess.EnableRaisingEvents = true;
                CommandProcess.OutputDataReceived += ExecutableOutput;
                CommandProcess.ErrorDataReceived += ExecutableOutput;
                CommandProcess.Exited += (sender, args) => HasProcessExited = true;

                // Start the process
                DebugWriter.WriteDebug(DebugLevel.I, "Starting process {0} with working directory {1} and arguments {2}...", File, WorkingDirectory, Args);
                CommandProcess.Start();
                CommandProcess.BeginOutputReadLine();
                CommandProcess.BeginErrorReadLine();

                // Wait for process exit
                while (!HasProcessExited | !KernelFlags.CancelRequested)
                {
                    if (HasProcessExited)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Process exited! Output may not be complete!");
                        CommandProcess.WaitForExit();
                        DebugWriter.WriteDebug(DebugLevel.I, "Flushed as much as possible.");
                        break;
                    }
                    else if (KernelFlags.CancelRequested)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Process killed! Output may not be complete!");
                        CommandProcess.Kill();
                        CommandProcess.WaitForExit();
                        DebugWriter.WriteDebug(DebugLevel.I, "Flushed as much as possible.");
                        break;
                    }
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Process exited with exit code {0}.", CommandProcess.ExitCode);
                return CommandProcess.ExitCode;
            }
            catch (ThreadInterruptedException)
            {
                KernelFlags.CancelRequested = false;
                return default;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ProcessError, File + Args, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message, File);
            }
            return -1;
        }

        /// <summary>
        /// Executes a file with specified arguments and puts the output to the string
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="exitCode">Application exit code. -1 if internal error occurred</param>
        /// <param name="includeStdErr">Include output printed to StdErr</param>
        /// <returns>Output of a command from stdout</returns>
        public static string ExecuteProcessToString(string File, string Args, ref int exitCode, bool includeStdErr) =>
            ExecuteProcessToString(File, Args, CurrentDirectory.CurrentDir, ref exitCode, includeStdErr);

        /// <summary>
        /// Executes a file with specified arguments and puts the output to the string
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="WorkingDirectory">Specifies the working directory</param>
        /// <param name="exitCode">Application exit code. -1 if internal error occurred</param>
        /// <param name="includeStdErr">Include output printed to StdErr</param>
        /// <returns>Output of a command from stdout</returns>
        public static string ExecuteProcessToString(string File, string Args, string WorkingDirectory, ref int exitCode, bool includeStdErr)
        {
            var commandOutputBuilder = new StringBuilder();
            try
            {
                bool HasProcessExited = false;
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = includeStdErr,
                    FileName = File,
                    Arguments = Args,
                    WorkingDirectory = WorkingDirectory,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                };
                CommandProcess.StartInfo = CommandProcessStart;

                // Set events up
                void DataReceivedHandler(object _, DataReceivedEventArgs data)
                {
                    if (data.Data is not null)
                        commandOutputBuilder.Append(data.Data);
                }
                CommandProcess.EnableRaisingEvents = true;
                CommandProcess.OutputDataReceived += DataReceivedHandler;
                if (includeStdErr)
                    CommandProcess.ErrorDataReceived += DataReceivedHandler;
                CommandProcess.Exited += (sender, args) => HasProcessExited = true;

                // Start the process
                DebugWriter.WriteDebug(DebugLevel.I, "Starting process {0} with working directory {1} and arguments {2}...", File, WorkingDirectory, Args);
                CommandProcess.Start();
                CommandProcess.BeginOutputReadLine();
                if (includeStdErr)
                    CommandProcess.BeginErrorReadLine();

                // Wait for process exit
                while (!HasProcessExited | !KernelFlags.CancelRequested)
                {
                    if (HasProcessExited)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Process exited! Output may not be complete!");
                        CommandProcess.WaitForExit();
                        DebugWriter.WriteDebug(DebugLevel.I, "Flushed as much as possible.");
                        break;
                    }
                    else if (KernelFlags.CancelRequested)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Process killed! Output may not be complete!");
                        CommandProcess.Kill();
                        CommandProcess.WaitForExit();
                        DebugWriter.WriteDebug(DebugLevel.I, "Flushed as much as possible.");
                        break;
                    }
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Process exited with exit code {0}.", CommandProcess.ExitCode);
                exitCode = CommandProcess.ExitCode;
            }
            catch (ThreadInterruptedException)
            {
                KernelFlags.CancelRequested = false;
                exitCode = -1;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ProcessError, File + Args, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message, File);
                exitCode = -1;
            }
            return commandOutputBuilder.ToString();
        }

        /// <summary>
        /// Handles executable output
        /// </summary>
        /// <param name="sendingProcess">Sender</param>
        /// <param name="outLine">Output</param>
        private static void ExecutableOutput(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data is null)
                return;
            DebugWriter.WriteDebug(DebugLevel.I, outLine.Data);
            TextWriterColor.Write(outLine.Data);
        }

    }
}
