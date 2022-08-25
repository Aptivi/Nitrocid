
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

using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;

namespace KS.Arguments
{
    public static class ArgumentPrompt
    {

        // Variables
        internal static List<string> EnteredArguments = new();

        /// <summary>
        /// Prompts user for arguments
        /// </summary>
        /// <param name="InjMode">Argument injection mode (usually by "arginj" command)</param>
        public static void PromptArgs(bool InjMode = false)
        {
            // Checks if the arguments are injected
            string AnswerArgs = "";

            // Shows available arguments
            TextWriterColor.Write(Translate.DoTranslation("Available kernel arguments:"), true, ColorTools.ColTypes.ListTitle);
            ArgumentHelpSystem.ShowArgsHelp(ArgumentType.KernelArgs);
            ConsoleBase.ConsoleWrapper.WriteLine();
            TextWriterColor.Write("* " + Translate.DoTranslation("Press \"q\" if you're done."), true, ColorTools.ColTypes.Tip);
            TextWriterColor.Write("* " + Translate.DoTranslation("Multiple kernel arguments can be separated with commas without spaces, for example:") + " \"debug,safe\"", true, ColorTools.ColTypes.Tip);
            TextWriterColor.Write("* " + Translate.DoTranslation("Multiple injected commands can be separated with colons with spaces, for example:") + " cmdinject \"beep 100 500 : echo Hello!\"", true, ColorTools.ColTypes.Tip);

            // Prompts for the arguments
            while (!(AnswerArgs == "q"))
            {
                TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                AnswerArgs = Input.ReadLine();

                // Add an argument to the entered arguments list
                if (AnswerArgs != "q")
                {
                    foreach (string AnswerArg in AnswerArgs.Split(','))
                    {
                        if (ArgumentParse.AvailableArgs.ContainsKey(AnswerArg.Split(' ')[0]))
                        {
                            EnteredArguments.Add(AnswerArg);
                        }
                        else if (!string.IsNullOrWhiteSpace(AnswerArg.Split(' ')[0]))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("The requested argument {0} is not found."), true, ColorTools.ColTypes.Error, AnswerArg.Split(' ')[0]);
                        }
                    }
                }
                else if (InjMode)
                {
                    Flags.ArgsInjected = true;
                    Kernel.Kernel.KernelEventManager.RaiseArgumentsInjected(EnteredArguments);
                    TextWriterColor.Write(Translate.DoTranslation("Injected arguments will be scheduled to run at next reboot."), true, ColorTools.ColTypes.Neutral);
                }
                else if (EnteredArguments.Count != 0)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Starting the kernel with:") + " {0}", true, ColorTools.ColTypes.Neutral, string.Join(", ", EnteredArguments));
                    ArgumentParse.ParseArguments(EnteredArguments, ArgumentType.KernelArgs);
                }
            }
        }

    }
}