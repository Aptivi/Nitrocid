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
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Reflection;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Folders;
using Nitrocid.Languages;
using Nitrocid.Kernel.Events;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Shell.ShellBase.Commands.ProcessExecution
{
    /// <summary>
    /// Process executor module
    /// </summary>
    public static class ProcessExecutor
    {

        /// <summary>
        /// Executes a file with specified arguments
        /// </summary>
        internal static void ExecuteProcess(ExecuteProcessThreadParameters? ThreadParams)
        {
            if (ThreadParams is null)
                throw new KernelException(KernelExceptionType.ShellOperation, Translate.DoTranslation("Can't get thread parameters for process execution."));
            ExecuteProcess(ThreadParams.File, ThreadParams.Args);
        }

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
                while (!HasProcessExited | !CancellationHandlers.CancelRequested)
                {
                    if (HasProcessExited)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Process exited! Output may not be complete!");
                        CommandProcess.WaitForExit();
                        DebugWriter.WriteDebug(DebugLevel.I, "Flushed as much as possible.");
                        break;
                    }
                    else if (CancellationHandlers.CancelRequested)
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
                CancellationHandlers.CancelRequested = false;
                return default;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ProcessError, File + Args, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message, File);
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
                while (!HasProcessExited | !CancellationHandlers.CancelRequested)
                {
                    if (HasProcessExited)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Process exited! Output may not be complete!");
                        CommandProcess.WaitForExit();
                        DebugWriter.WriteDebug(DebugLevel.I, "Flushed as much as possible.");
                        break;
                    }
                    else if (CancellationHandlers.CancelRequested)
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
                CancellationHandlers.CancelRequested = false;
                exitCode = -1;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ProcessError, File + Args, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message, File);
                exitCode = -1;
            }
            return commandOutputBuilder.ToString();
        }

        /// <summary>
        /// Executes a file with specified arguments to a separate window. Doesn't block.
        /// </summary>
        internal static void ExecuteProcessForked(ExecuteProcessThreadParameters ThreadParams) =>
            ExecuteProcessForked(ThreadParams.File, ThreadParams.Args);

        /// <summary>
        /// Executes a file with specified arguments to a separate window. Doesn't block.
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static void ExecuteProcessForked(string File, string Args) =>
            ExecuteProcessForked(File, Args, CurrentDirectory.CurrentDir);

        /// <summary>
        /// Executes a file with specified arguments to a separate window. Doesn't block.
        /// </summary>
        /// <param name="File">Full path to file</param>
        /// <param name="Args">Arguments, if any</param>
        /// <param name="WorkingDirectory">Specifies the working directory</param>
        /// <returns>Application exit code. -1 if internal error occurred.</returns>
        public static void ExecuteProcessForked(string File, string Args, string WorkingDirectory)
        {
            try
            {
                var CommandProcess = new Process();
                var CommandProcessStart = new ProcessStartInfo()
                {
                    FileName = File,
                    Arguments = Args,
                    WorkingDirectory = WorkingDirectory,
                    UseShellExecute = true,
                };
                CommandProcess.StartInfo = StripEnvironmentVariables(CommandProcessStart);

                // Start the process
                DebugWriter.WriteDebug(DebugLevel.I, "Starting process {0} with working directory {1} and arguments {2}...", File, WorkingDirectory, Args);
                CommandProcess.Start();
            }
            catch (ThreadInterruptedException)
            {
                CancellationHandlers.CancelRequested = false;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ProcessError, File + Args, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Process error for {0}, {1}, {2}: {3}.", File, WorkingDirectory, Args, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message, File);
            }
        }

        internal static ProcessStartInfo StripEnvironmentVariables(ProcessStartInfo processStartInfo)
        {
            // --- UseShellExecute and the Environment property population Hack ---
            //
            // We need UseShellExecute to be able to use the runas verb, but it looks like that we can't start the process with the VS debugger,
            // because the StartInfo always populates the _environmentVariables field once the Environment property is populated.
            // _environmentVariables is not a public field.
            //
            // .NET expects _environmentVariables to be null when trying to start the process with the UseShellExecute being set to true,
            // but when calling Start(), .NET calls StartWithShellExecuteEx() and checks to see if that variable is null, so executing the
            // process in this way is basically impossible after evaluating the Environment property without having to somehow nullify this
            // _environmentVariables field using private reflection after evaluating the Environment property.
            //
            // if (startInfo._environmentVariables != null)
            //     throw new InvalidOperationException(SR.CantUseEnvVars);
            //
            // Please DO NOT even try to evaluate selfProcess.StartInfo.Environment in your debugger even if hovering over selfProcess.StartInfo,
            // because that would undo all the changes that we've made to the _environmentVariables and causes us to lose all the changes made
            // to this instance of StartInfo.
            //
            // if (_environmentVariables == null)
            // {
            //     IDictionary envVars = System.Environment.GetEnvironmentVariables();
            //     _environmentVariables = new DictionaryWrapper(new Dictionary<string, string?>(
            //     (...)
            // }
            //
            // This hack is only applicable to developers debugging the StartInfo instance of this specific process using VS. Nitrocid should
            // be able to restart itself as elevated normally if no debugger is attached.
            //
            // References:
            //   - https://github.com/dotnet/runtime/blob/release/8.0/src/libraries/System.Diagnostics.Process/src/System/Diagnostics/Process.Win32.cs#L47
            //   - https://github.com/dotnet/runtime/blob/release/8.0/src/libraries/System.Diagnostics.Process/src/System/Diagnostics/ProcessStartInfo.cs#L91
            //
            // Issue report: https://github.com/dotnet/runtime/issues/94338
            var privateReflection = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField;
            var startInfoType = processStartInfo.GetType();
            var envVarsField = startInfoType.GetField("_environmentVariables", privateReflection) ??
                throw new KernelException(KernelExceptionType.ShellOperation, Translate.DoTranslation("Can't get internal field for environment variables"));
            envVarsField.SetValue(processStartInfo, null);
            // 
            // --- UseShellExecute and the Environment property population Hack End ---
            return processStartInfo;
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
