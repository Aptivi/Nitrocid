
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
using System.Collections.Generic;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Events;
using System.Linq;
using KS.Drivers.Console.Consoles;
using KS.Drivers.Console;
using KS.Drivers;
using KS.Misc.Threading;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command executor module
    /// </summary>
    public static class CommandExecutor
    {

        /// <summary>
        /// Parameters to pass to <see cref="ExecuteCommand(ExecuteCommandParameters)"/>
        /// </summary>
        internal class ExecuteCommandParameters
        {
            /// <summary>
            /// The requested command with arguments
            /// </summary>
            internal string RequestedCommand;
            /// <summary>
            /// The shell type
            /// </summary>
            internal string ShellType;
            /// <summary>
            /// Is the command the custom command?
            /// </summary>
            internal bool CustomCommand;
            /// <summary>
            /// Mod commands
            /// </summary>
            internal Dictionary<string, CommandInfo> ModCommands;

            internal ExecuteCommandParameters(string RequestedCommand, ShellType ShellType) : 
                this(RequestedCommand, ShellManager.GetShellTypeName(ShellType)) 
            { }

            internal ExecuteCommandParameters(string RequestedCommand, string ShellType)
            {
                this.RequestedCommand = RequestedCommand;
                this.ShellType = ShellType;
            }
        }

        internal static void StartCommandThread(ExecuteCommandParameters ThreadParams)
        {
            // Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
            // is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
            // shell can do their job.
            var ShellInstance = ShellStart.ShellStack[^1];
            var StartCommandThread = ShellInstance.ShellCommandThread;
            bool CommandThreadValid = true;
            if (StartCommandThread.IsAlive)
            {
                if (ShellInstance.AltCommandThreads.Count > 0)
                {
                    StartCommandThread = ShellInstance.AltCommandThreads[^1];
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: Alt command threads are not there.");
                    CommandThreadValid = false;
                }
            }
            if (CommandThreadValid)
            {
                StartCommandThread.Start(ThreadParams);
                StartCommandThread.Wait();
                StartCommandThread.Stop();
            }
        }

        internal static void ExecuteCommand(ExecuteCommandParameters ThreadParams) =>
            ExecuteCommand(ThreadParams, ThreadParams.ModCommands is not null ? ThreadParams.ModCommands : CommandManager.GetCommands(ThreadParams.ShellType));

        private static void ExecuteCommand(ExecuteCommandParameters ThreadParams, Dictionary<string, CommandInfo> TargetCommands)
        {
            string RequestedCommand = ThreadParams.RequestedCommand;
            string ShellType = ThreadParams.ShellType;
            try
            {
                // Variables
                var ArgumentInfo = new ProvidedCommandArgumentsInfo(RequestedCommand, ShellType);
                string Command = ArgumentInfo.Command;
                var Args = ArgumentInfo.ArgumentsList;
                var Switches = ArgumentInfo.SwitchesList;
                string StrArgs = ArgumentInfo.ArgumentsText;
                bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;
                bool RequiredSwitchesProvided = ArgumentInfo.RequiredSwitchesProvided;
                bool RequiredSwitchArgumentsProvided = ArgumentInfo.RequiredSwitchArgumentsProvided;

                // Check to see if a requested command is obsolete
                if (TargetCommands[Command].Flags.HasFlag(CommandFlags.Obsolete))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "The command requested {0} is obsolete", Command);
                    TextWriterColor.Write(Translate.DoTranslation("This command is obsolete and will be removed in a future release."));
                }

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                var ArgInfo = TargetCommands[Command].CommandArgumentInfo;
                bool argSatisfied = true;
                if (ArgInfo is not null)
                {
                    // Check for required arguments
                    if (!RequiredArgumentsProvided && ArgInfo.ArgumentsRequired)
                    {
                        argSatisfied = false;
                        DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", Command);
                        TextWriterColor.Write(Translate.DoTranslation("Required arguments are not provided. See below for usage:"));
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                    
                    // Check for required switches
                    if (!RequiredSwitchesProvided && ArgInfo.Switches.Any((@switch) => @switch.IsRequired))
                    {
                        argSatisfied = false;
                        DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough switches for {0}", Command);
                        TextWriterColor.Write(Translate.DoTranslation("Required switches are not provided. See below for usage:"));
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                    
                    // Check for required switch arguments
                    if (!RequiredSwitchArgumentsProvided && ArgInfo.Switches.Any((@switch) => @switch.ArgumentsRequired))
                    {
                        argSatisfied = false;
                        DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided a value for one of the switches for {0}", Command);
                        TextWriterColor.Write(Translate.DoTranslation("One of the switches requires a value that is not provided. See below for usage:"));
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                    
                    // Check for unknown switches
                    if (ArgumentInfo.unknownSwitchesList.Length > 0)
                    {
                        argSatisfied = false;
                        DebugWriter.WriteDebug(DebugLevel.W, "User has provided unknown switches {0}", Command);
                        TextWriterColor.Write(Translate.DoTranslation("Switches that are listed below are unknown."));
                        ListWriterColor.WriteList(ArgumentInfo.unknownSwitchesList);
                        TextWriterColor.Write(Translate.DoTranslation("See below for usage:"));
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                    
                    // Check for conflicting switches
                    if (ArgumentInfo.conflictingSwitchesList.Length > 0)
                    {
                        argSatisfied = false;
                        DebugWriter.WriteDebug(DebugLevel.W, "User has provided conflicting switches for {0}", Command);
                        TextWriterColor.Write(Translate.DoTranslation("Switches that are listed below conflict with each other."));
                        ListWriterColor.WriteList(ArgumentInfo.conflictingSwitchesList);
                        TextWriterColor.Write(Translate.DoTranslation("See below for usage:"));
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                }

                // Execute the command
                if (argSatisfied)
                {
                    var CommandBase = TargetCommands[Command].CommandBase;
                    CommandBase.Execute(StrArgs, Args, Switches);
                }
            }
            catch (ThreadInterruptedException)
            {
                Flags.CancelRequested = false;
                return;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.CommandError, ShellType, RequestedCommand, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message, RequestedCommand);
            }
        }

        /// <summary>
        /// Executes a command in a wrapped mode (must be run from a separate command execution entry point, <see cref="BaseCommand.Execute(string, string[], string[])"/>.)
        /// </summary>
        /// <param name="Command">Requested command with its arguments and switches</param>
        public static void ExecuteCommandWrapped(string Command)
        {
            var currentShell = ShellStart.ShellStack[^1];
            var currentType = currentShell.ShellType;
            var StartCommandThread = currentShell.ShellCommandThread;
            var argumentInfo = new ProvidedCommandArgumentsInfo(Command, currentType);
            string CommandToBeWrapped = argumentInfo.Command;

            // Check to see if the command is found
            if (!CommandManager.IsCommandFound(CommandToBeWrapped, currentType))
            {
                TextWriterColor.Write(Translate.DoTranslation("The wrappable command is not found."), true, KernelColorType.Error);
                return;
            }

            // Check to see if we can start an alternative thread
            if (!StartCommandThread.IsAlive)
            {
                TextWriterColor.Write(Translate.DoTranslation("You must not directly execute this command in a wrapped mode."), true, KernelColorType.Error);
                return;
            }

            // Now, check to see if the command is wrappable
            if (!CommandManager.GetCommand(CommandToBeWrapped, currentType).Flags.HasFlag(CommandFlags.Wrappable))
            {
                var WrappableCmds = GetWrappableCommands(currentType);
                TextWriterColor.Write(Translate.DoTranslation("The command is not wrappable. These commands are wrappable:"), true, KernelColorType.Error);
                ListWriterColor.WriteList(WrappableCmds);
                return;
            }

            bool buffered = false;
            try
            {
                // First, initialize the alternative command thread
                var AltThreads = ShellStart.ShellStack[^1].AltCommandThreads;
                if (AltThreads.Count == 0 || AltThreads[^1].IsAlive)
                {
                    var WrappedCommand = new KernelThread($"Wrapped Shell Command Thread", false, (cmdThreadParams) => ExecuteCommand((ExecuteCommandParameters)cmdThreadParams));
                    ShellStart.ShellStack[^1].AltCommandThreads.Add(WrappedCommand);
                }

                // Then, initialize the buffered writer and execute the commands
                DriverHandler.BeginLocalDriver<IConsoleDriver>("Buffered");
                ShellManager.GetLine(Command, "", currentType, false);
                buffered = true;

                // Extract the buffer and then end the local driver
                var wrapBuffer = ((Buffered)DriverHandler.CurrentConsoleDriverLocal).consoleBuffer;
                var wrapOutput = wrapBuffer.ToString();
                wrapBuffer.Clear();
                DriverHandler.EndLocalDriver<IConsoleDriver>();

                // Now, print the output
                TextWriterWrappedColor.WriteWrapped(wrapOutput, false, KernelColorType.NeutralText);
                if (!wrapOutput.EndsWith(CharManager.NewLine))
                    TextWriterColor.Write();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to wrap command {0}: {1}", CommandToBeWrapped, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("An error occurred while trying to wrap a command output") + ": {0}", true, KernelColorType.Error, ex.Message);
            }

            // In case error happens
            if (!buffered)
                DriverHandler.EndLocalDriver<IConsoleDriver>();
        }

        /// <summary>
        /// Gets the wrappable commands
        /// </summary>
        /// <param name="shellType">Shell type</param>
        /// <returns>List of commands that one of their flags contains <see cref="CommandFlags.Wrappable"/></returns>
        public static string[] GetWrappableCommands(ShellType shellType) =>
            GetWrappableCommands(ShellManager.GetShellTypeName(shellType));

        /// <summary>
        /// Gets the wrappable commands
        /// </summary>
        /// <param name="shellType">Shell type</param>
        /// <returns>List of commands that one of their flags contains <see cref="CommandFlags.Wrappable"/></returns>
        public static string[] GetWrappableCommands(string shellType)
        {
            // Get wrappable commands
            var WrappableCmds = ShellManager.GetShellInfo(shellType).Commands.Values
                .Where(CommandInfo => CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                .Select(CommandInfo => CommandInfo.Command)
                .ToArray();

            return WrappableCmds;
        }

    }
}
