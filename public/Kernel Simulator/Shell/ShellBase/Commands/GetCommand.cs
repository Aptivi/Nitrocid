
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
using System.IO;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug.Interface;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.MiscWriters;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Command parser module
    /// </summary>
    public static class GetCommand
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
            /// The debug device stream writer
            /// </summary>
            internal StreamWriter DebugDeviceSocket;
            /// <summary>
            /// Remote debug device address
            /// </summary>
            internal string Address;

            internal ExecuteCommandParameters(string RequestedCommand, ShellType ShellType, StreamWriter DebugDeviceSocket = null, string Address = "") : 
                this(RequestedCommand, Shell.GetShellTypeName(ShellType), DebugDeviceSocket, Address) 
            { }

            internal ExecuteCommandParameters(string RequestedCommand, string ShellType, StreamWriter DebugDeviceSocket = null, string Address = "")
            {
                this.RequestedCommand = RequestedCommand;
                this.ShellType = ShellType;
                this.DebugDeviceSocket = DebugDeviceSocket;
                this.Address = Address;
            }
        }

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="ThreadParams">Thread parameters for ExecuteCommand.</param>
        internal static void ExecuteCommand(ExecuteCommandParameters ThreadParams)
        {
            string RequestedCommand = ThreadParams.RequestedCommand;
            string ShellType = ThreadParams.ShellType;
            var DebugDeviceSocket = ThreadParams.DebugDeviceSocket;
            string DebugDeviceAddress = ThreadParams.Address;
            try
            {
                // Variables
                var ArgumentInfo = new ProvidedCommandArgumentsInfo(RequestedCommand, ShellType);
                string Command = ArgumentInfo.Command;
                var Args = ArgumentInfo.ArgumentsList;
                var Switches = ArgumentInfo.SwitchesList;
                string StrArgs = ArgumentInfo.ArgumentsText;
                bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;
                var TargetCommands = CommandManager.GetCommands(ShellType);

                // Check to see if a requested command is obsolete
                if (TargetCommands[Command].Flags.HasFlag(CommandFlags.Obsolete))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "The command requested {0} is obsolete", Command);
                    Decisive.DecisiveWrite(ShellType, DebugDeviceSocket, Translate.DoTranslation("This command is obsolete and will be removed in a future release."), true, ColorTools.ColTypes.NeutralText);
                }

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                if (TargetCommands[Command].CommandArgumentInfo is not null)
                {
                    var ArgInfo = TargetCommands[Command].CommandArgumentInfo;
                    if (ArgInfo.ArgumentsRequired & RequiredArgumentsProvided | !ArgInfo.ArgumentsRequired)
                    {
                        var CommandBase = TargetCommands[Command].CommandBase;
                        if ((CommandBase is RemoteDebugCommandExecutor || CommandBase.GetType().GetInterface(typeof(IRemoteDebugCommand).Name) != null) && DebugDeviceSocket != null)
                            ((IRemoteDebugCommand)CommandBase).Execute(StrArgs, Args, Switches, DebugDeviceSocket, DebugDeviceAddress);
                        else
                            CommandBase.Execute(StrArgs, Args, Switches);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", Command);
                        Decisive.DecisiveWrite(ShellType, DebugDeviceSocket, Translate.DoTranslation("There was not enough arguments. See below for usage:"), true, ColorTools.ColTypes.NeutralText);
                        HelpSystem.ShowHelp(Command, ShellType);
                    }
                }
                else
                {
                    var CommandBase = TargetCommands[Command].CommandBase;
                    if ((CommandBase is RemoteDebugCommandExecutor || CommandBase.GetType().GetInterface(typeof(IRemoteDebugCommand).Name) != null) && DebugDeviceSocket != null)
                        ((IRemoteDebugCommand)CommandBase).Execute(StrArgs, Args, Switches, DebugDeviceSocket, DebugDeviceAddress);
                    else
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
                Kernel.Events.EventsManager.FireEvent("CommandError", ShellType, RequestedCommand, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                Decisive.DecisiveWrite(ShellType, DebugDeviceSocket, Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, ColorTools.ColTypes.Error, ex.GetType().FullName, ex.Message, RequestedCommand);
            }
        }

    }
}
