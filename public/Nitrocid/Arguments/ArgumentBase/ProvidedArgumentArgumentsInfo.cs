
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

using System.Collections.Generic;
using System.Linq;
using KS.Kernel.Debugging;
using KS.Misc.Text;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Provided argument parameters information class
    /// </summary>
    public class ProvidedArgumentArgumentsInfo
    {

        /// <summary>
        /// Target kernel argument that the user executed in shell
        /// </summary>
        public string Argument { get; private set; }
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
        /// Makes a new instance of the kernel argument argument info with the user-provided command text
        /// </summary>
        /// <param name="ArgumentText">Kernel argument text that the user provided</param>
        internal ProvidedArgumentArgumentsInfo(string ArgumentText)
        {
            string Argument;
            bool RequiredArgumentsProvided = true;
            var KernelArguments = ArgumentParse.AvailableCMDLineArgs;

            // Get the index of the first space (Used for step 3)
            int index = ArgumentText.IndexOf(" ");
            if (index == -1)
                index = ArgumentText.Length;
            DebugWriter.WriteDebug(DebugLevel.I, "Index: {0}", index);

            // Split the requested command string into words
            var words = ArgumentText.Split(new[] { ' ' });
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Argument = words[0];

            // Get the string of arguments
            string strArgs = ArgumentText[index..];
            DebugWriter.WriteDebug(DebugLevel.I, "Prototype strArgs: {0}", strArgs);
            if (!(index == ArgumentText.Length))
                strArgs = strArgs[1..];
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgs: {0}", strArgs);

            // Split the arguments with enclosed quotes and set the required boolean variable
            var EnclosedArgs = strArgs.SplitEncloseDoubleQuotes();
            if (EnclosedArgs is not null)
            {
                RequiredArgumentsProvided = (bool)(KernelArguments[Argument].ArgArgumentInfo.MinimumArguments is var arg2 && (EnclosedArgs.Length) is { } arg1 ? arg1 >= arg2 : (bool?)null);
            }
            else if (KernelArguments[Argument].ArgArgumentInfo.ArgumentsRequired & EnclosedArgs is null)
            {
                RequiredArgumentsProvided = false;
            }
            if (EnclosedArgs is not null)
                DebugWriter.WriteDebug(DebugLevel.I, "Arguments parsed: " + string.Join(", ", EnclosedArgs));

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
            ArgumentsList = FinalArgs.ToArray();
            SwitchesList = FinalSwitches.ToArray();
            ArgumentsText = strArgs;
            this.Argument = Argument;
            this.RequiredArgumentsProvided = RequiredArgumentsProvided;
        }

    }
}
