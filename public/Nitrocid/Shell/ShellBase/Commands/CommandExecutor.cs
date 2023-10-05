
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
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Events;
using System.Linq;
using KS.Drivers.Console;
using KS.Drivers;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Threading;
using KS.Shell.ShellBase.Scripting;
using KS.Shell.ShellBase.Aliases;
using KS.Kernel.Configuration;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Switches;
using KS.Drivers.Console.Bases;
using KS.Shell.ShellBase.Help;
using System.Runtime;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command executor module
    /// </summary>
    public static class CommandExecutor
    {

        internal static void StartCommandThread(CommandExecutorParameters ThreadParams)
        {
            // Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
            // is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
            // shell can do their job.
            var ShellInstance = ThreadParams.ShellInstance;
            var StartCommandThread = ShellInstance.ShellCommandThread;
            bool CommandThreadValid = true;
            if (StartCommandThread.IsAlive)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't make another main command thread. Using alternatives...");
                if (ShellInstance.AltCommandThreads.Count > 0)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Using last alt command thread...");
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
                DebugWriter.WriteDebug(DebugLevel.I, "Starting command thread...");
                StartCommandThread.Start(ThreadParams);
                StartCommandThread.Wait();
                StartCommandThread.Stop();
            }
        }

        internal static void ExecuteCommand(CommandExecutorParameters ThreadParams) =>
            ExecuteCommand(ThreadParams, CommandManager.GetCommands(ThreadParams.ShellType));

        private static void ExecuteCommand(CommandExecutorParameters ThreadParams, Dictionary<string, CommandInfo> TargetCommands)
        {
            string RequestedCommand = ThreadParams.RequestedCommand;
            string ShellType = ThreadParams.ShellType;
            var ShellInstance = ThreadParams.ShellInstance;
            try
            {
                // Variables
                var ArgumentInfo = ArgumentsParser.ParseShellCommandArguments(RequestedCommand, ShellType);
                string Command = ArgumentInfo.Command;
                var Args = ArgumentInfo.ArgumentsList;
                var ArgsOrig = ArgumentInfo.ArgumentsListOrig;
                var Switches = ArgumentInfo.SwitchesList;
                string StrArgs = ArgumentInfo.ArgumentsText;
                string StrArgsOrig = ArgumentInfo.ArgumentsTextOrig;
                bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;
                bool RequiredSwitchesProvided = ArgumentInfo.RequiredSwitchesProvided;
                bool RequiredSwitchArgumentsProvided = ArgumentInfo.RequiredSwitchArgumentsProvided;
                bool containsSetSwitch = SwitchManager.ContainsSwitch(Switches, "-set");
                string variable = "";

                // Check to see if a requested command is obsolete
                if (TargetCommands[Command].Flags.HasFlag(CommandFlags.Obsolete))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "The command requested {0} is obsolete", Command);
                    TextWriterColor.Write(Translate.DoTranslation("This command is obsolete and will be removed in a future release."));
                }

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                var ArgInfos = TargetCommands[Command].CommandArgumentInfo;
                bool argSatisfied = true;
                for (int i = 0; i < ArgInfos.Length; i++)
                {
                    argSatisfied = true;
                    CommandArgumentInfo ArgInfo = ArgInfos[i];
                    bool isLast = i == ArgInfos.Length - 1;
                    if (ArgInfo is not null)
                    {
                        // Trim the -set switch
                        if (containsSetSwitch)
                        {
                            // First, work on the string
                            string setValue = $"-set={SwitchManager.GetSwitchValue(Switches, "-set")}";

                            // Work on the list
                            if (Switches.Contains(setValue))
                            {
                                for (int j = 0; j < Switches.Length; j++)
                                {
                                    string @switch = Switches[j];
                                    if (@switch == setValue && ArgInfo.AcceptsSet)
                                    {
                                        variable = SwitchManager.GetSwitchValue(Switches, "-set");
                                        Switches = Switches.Except(new[] { setValue }).ToArray();
                                        break;
                                    }
                                }
                            }
                        }

                        // Check for required arguments
                        if (!RequiredArgumentsProvided && ArgInfo.ArgumentsRequired && isLast)
                        {
                            argSatisfied = false;
                            DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", Command);
                            TextWriterColor.Write(Translate.DoTranslation("Required arguments are not provided."));
                        }
                    
                        // Check for required switches
                        if (!RequiredSwitchesProvided && ArgInfo.Switches.Any((@switch) => @switch.IsRequired) && isLast)
                        {
                            argSatisfied = false;
                            DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough switches for {0}", Command);
                            TextWriterColor.Write(Translate.DoTranslation("Required switches are not provided."));
                        }
                    
                        // Check for required switch arguments
                        if (!RequiredSwitchArgumentsProvided && ArgInfo.Switches.Any((@switch) => @switch.ArgumentsRequired) && isLast)
                        {
                            argSatisfied = false;
                            DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided a value for one of the switches for {0}", Command);
                            TextWriterColor.Write(Translate.DoTranslation("One of the switches requires a value that is not provided."));
                        }
                    
                        // Check for unknown switches
                        if (ArgumentInfo.UnknownSwitchesList.Length > 0 && isLast)
                        {
                            argSatisfied = false;
                            DebugWriter.WriteDebug(DebugLevel.W, "User has provided unknown switches {0}", Command);
                            TextWriterColor.Write(Translate.DoTranslation("Switches that are listed below are unknown."));
                            ListWriterColor.WriteList(ArgumentInfo.UnknownSwitchesList);
                        }
                    
                        // Check for conflicting switches
                        if (ArgumentInfo.ConflictingSwitchesList.Length > 0 && isLast)
                        {
                            argSatisfied = false;
                            DebugWriter.WriteDebug(DebugLevel.W, "User has provided conflicting switches for {0}", Command);
                            TextWriterColor.Write(Translate.DoTranslation("Switches that are listed below conflict with each other."));
                            ListWriterColor.WriteList(ArgumentInfo.ConflictingSwitchesList);
                        }
                    
                        // Check for switches that don't accept values
                        if (ArgumentInfo.NoValueSwitchesList.Length > 0 && isLast)
                        {
                            argSatisfied = false;
                            DebugWriter.WriteDebug(DebugLevel.W, "User has provided switches that don't accept values for {0}", Command);
                            TextWriterColor.Write(Translate.DoTranslation("The below switches don't accept values."));
                            ListWriterColor.WriteList(ArgumentInfo.NoValueSwitchesList);
                        }
                    }
                }

                // Execute the command
                if (argSatisfied)
                {
                    // Prepare the command parameter instance
                    var parameters = new CommandParameters(StrArgs, Args, StrArgsOrig, ArgsOrig, Switches);

                    // Now, get the base command and execute it
                    DebugWriter.WriteDebug(DebugLevel.I, "Really executing command {0} with args {1}", Command, StrArgs);
                    var CommandBase = TargetCommands[Command].CommandBase;
                    string value = "";
#if NET7_0
#pragma warning disable SYSLIB0046
                    CancellationHandlers.cts = new CancellationTokenSource();

                    // TODO: Actually use interactive methods to notify cancellation, as ControlledExecution behaves like Thread.Abort() in .NET Framework.
                    try
                    {
                        ControlledExecution.Run(() =>
                        {
                            if (DriverHandler.CurrentConsoleDriverLocal.IsDumb)
                                ShellInstance.LastErrorCode = CommandBase.ExecuteDumb(parameters, ref value);
                            else
                                ShellInstance.LastErrorCode = CommandBase.Execute(parameters, ref value);
                        }, CancellationHandlers.cts.Token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Command aborted in the .NET Framework way. This is currently not supported as it may corrupt the state. Any weird behavior logged below is most likely from this.");
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriterColor.Write(Translate.DoTranslation("Command has been aborted."), true, KernelColorType.Error);
                    }
#pragma warning restore SYSLIB0046
#else
                    if (DriverHandler.CurrentConsoleDriverLocal.IsDumb)
                        ShellInstance.LastErrorCode = CommandBase.ExecuteDumb(parameters, ref value);
                    else
                        ShellInstance.LastErrorCode = CommandBase.Execute(parameters, ref value);
#endif

                    // Set the error code and set the UESH variable as appropriate
                    DebugWriter.WriteDebug(DebugLevel.I, "Error code is {0}", ShellInstance.LastErrorCode);
                    if (containsSetSwitch)
                    {
                        // Check to see if the value contains newlines
                        if (value.Contains('\n'))
                        {
                            // Assume that we're setting an array.
                            DebugWriter.WriteDebug(DebugLevel.I, "Array variable to set is {0}", variable);
                            string[] values = value.Replace((char)13, default).Split('\n');
                            UESHVariables.SetVariables(variable, values);
                        }
                        else if (value.StartsWith('[') && value.EndsWith(']'))
                        {
                            // Assume that we're setting an array
                            DebugWriter.WriteDebug(DebugLevel.I, "Array variable to set is {0}", variable);
                            value = value[1..(value.Length - 1)];
                            string[] values = value.Split(", ");
                            UESHVariables.SetVariables(variable, values);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Variable to set {0} is {1}", value, variable);
                            UESHVariables.SetVariable(variable, value);
                        }
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Arguments not satisfied.");
                    TextWriterColor.Write(Translate.DoTranslation("See below for usage:"));
                    HelpPrint.ShowHelp(Command, ShellType);
                    ShellInstance.LastErrorCode = -6;
                }
            }
            catch (ThreadInterruptedException)
            {
                KernelFlags.CancelRequested = false;
                ShellInstance.LastErrorCode = -5;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.CommandError, ShellType, RequestedCommand, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to execute command {0} from type {1}: {2}", RequestedCommand, ShellType.ToString(), ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message, RequestedCommand);
                ShellInstance.LastErrorCode = ex.GetHashCode();
            }
        }

        /// <summary>
        /// Executes a command in a wrapped mode (must be run from a separate command execution entry point, <see cref="BaseCommand.Execute(CommandParameters, ref string)"/>.)
        /// </summary>
        /// <param name="Command">Requested command with its arguments and switches</param>
        public static void ExecuteCommandWrapped(string Command)
        {
            var currentShell = ShellStart.ShellStack[^1];
            var currentType = currentShell.ShellType;
            var StartCommandThread = currentShell.ShellCommandThread;
            var argumentInfo = ArgumentsParser.ParseShellCommandArguments(Command, currentType);
            string CommandToBeWrapped = argumentInfo.Command;

            // Check to see if the command is found
            if (!CommandManager.IsCommandFound(CommandToBeWrapped, currentType))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Wrappable command {0} not found", Command);
                TextWriterColor.Write(Translate.DoTranslation("The wrappable command is not found."), true, KernelColorType.Error);
                return;
            }

            // Check to see if we can start an alternative thread
            if (!StartCommandThread.IsAlive)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Can't directly execute command {0} in wrapped mode.", Command);
                TextWriterColor.Write(Translate.DoTranslation("You must not directly execute this command in a wrapped mode."), true, KernelColorType.Error);
                return;
            }

            // Now, check to see if the command is wrappable
            if (!CommandManager.GetCommand(CommandToBeWrapped, currentType).Flags.HasFlag(CommandFlags.Wrappable))
            {
                var WrappableCmds = GetWrappableCommands(currentType);
                DebugWriter.WriteDebug(DebugLevel.E, "Unwrappable command {0}! Wrappable commands: [{1}]", Command, string.Join(", ", WrappableCmds));
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
                    DebugWriter.WriteDebug(DebugLevel.I, "Making alt thread for wrapped command {0}...", Command);
                    var WrappedCommand = new KernelThread($"Wrapped Shell Command Thread", false, (cmdThreadParams) => ExecuteCommand((CommandExecutorParameters)cmdThreadParams));
                    ShellStart.ShellStack[^1].AltCommandThreads.Add(WrappedCommand);
                }

                // Then, initialize the buffered writer and execute the commands
                DriverHandler.BeginLocalDriver<IConsoleDriver>("Buffered");
                DebugWriter.WriteDebug(DebugLevel.I, "Buffering...");
                ShellManager.GetLine(Command, "", currentType, false);
                buffered = true;

                // Extract the buffer and then end the local driver
                var wrapBuffer = ((Buffered)DriverHandler.CurrentConsoleDriverLocal).consoleBuffer;
                var wrapOutput = wrapBuffer.ToString();
                wrapBuffer.Clear();
                DriverHandler.EndLocalDriver<IConsoleDriver>();

                // Now, print the output
                DebugWriter.WriteDebug(DebugLevel.I, "Printing...");
                TextWriterWrappedColor.WriteWrapped(wrapOutput, false, KernelColorType.NeutralText);
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
            // Get shell info
            var shellInfo = ShellManager.GetShellInfo(shellType);

            // Get wrappable commands
            var WrappableCmds = shellInfo.Commands.Values
                .Where(CommandInfo => CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                .Select(CommandInfo => CommandInfo.Command)
                .ToArray();
            var WrappableUnified = ShellManager.UnifiedCommands.Values
                .Where(CommandInfo => CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                .Select(CommandInfo => CommandInfo.Command)
                .ToArray();
            var WrappableAliases = AliasManager.GetAliasesListFromType(shellType)
                .Where((info) => WrappableCmds.Contains(info.Command) || WrappableUnified.Contains(info.Command))
                .Select((info) => info.Alias)
                .ToArray();
            var finalWrappables = WrappableCmds
                .Union(WrappableAliases)
                .Union(WrappableUnified)
                .ToArray();

            return finalWrappables;
        }

    }
}
