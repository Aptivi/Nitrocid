﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.Linq;
using KS.Misc.Text;
using KS.Misc.Writers.DebugWriters;

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

using KS.Modifications;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands
{
    public class ProvidedCommandArgumentsInfo
    {

        /// <summary>
        /// Target command that the user executed in shell
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Text version of the provided arguments and switches
        /// </summary>
        public string ArgumentsText { get; private set; }
        /// <summary>
        /// List version of the provided arguments and switches
        /// </summary>
        public string[] FullArgumentsList { get; private set; }
        /// <summary>
        /// List version of the provided arguments
        /// </summary>
        public string[] ArgumentsList { get; private set; }
        /// <summary>
        /// List version of the provided switches
        /// </summary>
        public string[] SwitchesList { get; private set; }
        /// <summary>
        /// Checks to see if the required arguments are provided
        /// </summary>
        public bool RequiredArgumentsProvided { get; private set; }

        /// <summary>
        /// Makes a new instance of the command argument info with the user-provided command text
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="CommandType">Shell command type. Consult the <see cref="ShellType"/> enum for information about supported shells.</param>
        internal ProvidedCommandArgumentsInfo(string CommandText, ShellType CommandType)
        {
            string Command;
            bool RequiredArgumentsProvided = true;
            Dictionary<string, CommandInfo> ShellCommands;
            Dictionary<string, CommandInfo> ModCommands;

            // Change the available commands list according to command type
            ShellCommands = GetCommand.GetCommands(CommandType);
            ModCommands = ModManager.ListModCommands(CommandType);

            // Get the index of the first space (Used for step 3)
            int index = CommandText.IndexOf(" ");
            if (index == -1)
                index = CommandText.Length;
            DebugWriter.Wdbg(DebugLevel.I, "Index: {0}", index);

            // Split the requested command string into words
            string[] words = CommandText.Split([' ']);
            for (int i = 0, loopTo = words.Length - 1; i <= loopTo; i++)
                DebugWriter.Wdbg(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Command = words[0];

            // Get the string of arguments
            string strArgs = CommandText.Substring(index);
            DebugWriter.Wdbg(DebugLevel.I, "Prototype strArgs: {0}", strArgs);
            if (!(index == CommandText.Length))
                strArgs = strArgs.Substring(1);
            DebugWriter.Wdbg(DebugLevel.I, "Finished strArgs: {0}", strArgs);

            // Split the arguments with enclosed quotes and set the required boolean variable
            var CommandInfo = ModCommands.ContainsKey(Command) ? ModCommands[Command] : ShellCommands[Command];
            string[] EnclosedArgs = strArgs.SplitEncloseDoubleQuotes();
            if (string.IsNullOrWhiteSpace(strArgs))
                EnclosedArgs = null;
            if (CommandInfo.CommandArgumentInfo is not null)
            {
                if (EnclosedArgs is not null)
                {
                    RequiredArgumentsProvided = (bool)(CommandInfo.CommandArgumentInfo.MinimumArguments is var arg1 && (EnclosedArgs?.Count()) is { } arg2 ? arg2 >= arg1 : (bool?)null);
                }
                else if (CommandInfo.CommandArgumentInfo.ArgumentsRequired & EnclosedArgs is null)
                {
                    RequiredArgumentsProvided = false;
                }
            }
            else
            {
                RequiredArgumentsProvided = true;
            }
            if (EnclosedArgs is not null)
                DebugWriter.Wdbg(DebugLevel.I, "Arguments parsed: " + string.Join(", ", EnclosedArgs));

            // Separate the arguments from the switches
            var FinalArgs = new List<string>();
            var FinalSwitches = new List<string>();
            if (EnclosedArgs is not null)
            {
                foreach (string EnclosedArg in EnclosedArgs)
                {
                    if (EnclosedArg.StartsWith("-"))
                    {
                        FinalSwitches.Add(EnclosedArg);
                    }
                    else
                    {
                        FinalArgs.Add(EnclosedArg);
                    }
                }
            }

            // Install the parsed values to the new class instance
            FullArgumentsList = EnclosedArgs;
            ArgumentsList = [.. FinalArgs];
            SwitchesList = [.. FinalSwitches];
            ArgumentsText = strArgs;
            this.Command = Command;
            this.RequiredArgumentsProvided = RequiredArgumentsProvided;
        }

    }
}