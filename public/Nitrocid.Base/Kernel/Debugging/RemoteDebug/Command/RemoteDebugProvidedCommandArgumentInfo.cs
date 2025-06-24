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

using System.Collections.Generic;
using Textify.General;

namespace Nitrocid.Base.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// Remote debug - provided command arguments information class
    /// </summary>
    public class RemoteDebugProvidedCommandArgumentInfo
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
        /// List version of the provided arguments
        /// </summary>
        public string[] ArgumentsList { get; private set; }
        /// <summary>
        /// Text version of the provided arguments and switches (original)
        /// </summary>
        public string ArgumentsTextOrig { get; private set; }
        /// <summary>
        /// List version of the provided arguments (original)
        /// </summary>
        public string[] ArgumentsListOrig { get; private set; }
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
        internal RemoteDebugProvidedCommandArgumentInfo(string CommandText)
        {
            string Command;
            bool RequiredArgumentsProvided = true;

            // Get the index of the first space (Used for step 3)
            int index = CommandText.IndexOf(" ");
            if (index == -1)
                index = CommandText.Length;
            DebugWriter.WriteDebug(DebugLevel.I, "Index: {0}", vars: [index]);

            // Split the requested command string into words
            var words = CommandText.Split([' ']);
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", vars: [i + 1, words[i]]);
            Command = words[0];

            // Get the string of arguments
            string strArgs = CommandText[index..];
            DebugWriter.WriteDebug(DebugLevel.I, "Prototype strArgs: {0}", vars: [strArgs]);
            if (index != CommandText.Length)
                strArgs = strArgs[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgs: {0}", vars: [strArgs]);
            ArgumentsTextOrig = strArgs;

            // Split the arguments with enclosed quotes
            var EnclosedArgs = strArgs.SplitEncloseDoubleQuotes();
            if (string.IsNullOrWhiteSpace(strArgs))
                EnclosedArgs = [];
            DebugWriter.WriteDebug(DebugLevel.I, "Arguments parsed: " + string.Join(", ", EnclosedArgs));
            ArgumentsListOrig = EnclosedArgs;

            // Check to see if the caller has provided required number of arguments
            var CommandInfo = RemoteDebugCommandExecutor.RemoteDebugCommands.TryGetValue(Command, out RemoteDebugCommandInfo? rdci) ? rdci : null;
            if (CommandInfo?.CommandArgumentInfo is not null)
                if (EnclosedArgs is not null)
                    RequiredArgumentsProvided = EnclosedArgs?.Length >= CommandInfo.CommandArgumentInfo.MinimumArguments;
                else if (CommandInfo.CommandArgumentInfo.ArgumentsRequired & EnclosedArgs is null)
                    RequiredArgumentsProvided = false;
                else
                    RequiredArgumentsProvided = true;

            // Separate the arguments from the switches
            var FinalArgs = new List<string>();
            var FinalSwitches = new List<string>();
            if (EnclosedArgs is not null)
            {
                foreach (string EnclosedArg in EnclosedArgs)
                {
                    if (EnclosedArg.StartsWith("-"))
                        FinalSwitches.Add(EnclosedArg);
                    else
                        FinalArgs.Add(EnclosedArg);
                }
            }

            // Install the parsed values to the new class instance
            ArgumentsList = [.. FinalArgs];
            SwitchesList = [.. FinalSwitches];
            ArgumentsText = strArgs;
            this.Command = Command;
            this.RequiredArgumentsProvided = RequiredArgumentsProvided;
        }

    }
}
