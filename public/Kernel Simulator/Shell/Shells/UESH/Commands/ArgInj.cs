
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

using System.Collections.Generic;
using KS.Arguments;
using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can set the arguments to launch at reboot.
    /// </summary>
    /// <remarks>
    /// If you need to reboot your kernel to run the debugger, or if you want to disable hardware probing to save time when booting, then this command is for you. It allows you to set arguments so they will be run once at each reboot.
    /// <br></br>
    /// You can use this command if you need to inject arguments while on the kernel. You can also separate many arguments by spaces so you don't have to run arguments one by one to conserve reboots.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ArgInjCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var FinalArgs = new List<string>();
            foreach (string arg in ListArgsOnly)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing argument {0}...", arg);
                if (ArgumentParse.AvailableArgs.ContainsKey(arg))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding argument {0}...", arg);
                    FinalArgs.Add(arg);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Argument {0} not found.", arg);
                    TextWriterColor.Write(Translate.DoTranslation("Argument {0} not found to inject."), true, ColorTools.ColTypes.Warning, arg);
                }
            }
            if (FinalArgs.Count == 0)
            {
                TextWriterColor.Write(Translate.DoTranslation("No arguments specified. Hint: Specify multiple arguments separated by spaces"), true, ColorTools.ColTypes.Error);
            }
            else
            {
                ArgumentPrompt.EnteredArguments = new List<string>(FinalArgs);
                Flags.ArgsInjected = true;
                TextWriterColor.Write(Translate.DoTranslation("Injected arguments, {0}, will be scheduled to run at next reboot."), string.Join(", ", ArgumentPrompt.EnteredArguments));
            }
        }

        public override void HelpHelper() => TextWriterColor.Write(Translate.DoTranslation("where arguments will be {0}"), string.Join(", ", ArgumentParse.AvailableArgs.Keys));

    }
}