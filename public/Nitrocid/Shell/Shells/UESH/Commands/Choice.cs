
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
using System.Linq;
using KS.ConsoleBase.Inputs.Styles;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Scripting.Interaction;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lets the user make a choice
    /// </summary>
    /// <remarks>
    /// This command can be used in scripting file that end in .uesh file extension. It lets the user choose the correct answers when answering this question and passes the chosen answer to the specified variable.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-multiple</term>
    /// <description>The output can be more than a character</description>
    /// </item>
    /// <item>
    /// <term>-single</term>
    /// <description>The output can be only one character</description>
    /// </item>
    /// <item>
    /// <term>-o</term>
    /// <description>One line choice style</description>
    /// </item>
    /// <item>
    /// <term>-t</term>
    /// <description>Two lines choice style</description>
    /// </item>
    /// <item>
    /// <term>-m</term>
    /// <description>Modern choice style</description>
    /// </item>
    /// <item>
    /// <term>-a</term>
    /// <description>Table choice style</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ChoiceCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Titles = new List<string>();
            var PressEnter = false;
            var OutputType = ChoiceStyle.DefaultChoiceOutputType;
            if (ListSwitchesOnly.Contains("-multiple"))
                PressEnter = true;
            if (ListSwitchesOnly.Contains("-single"))
                PressEnter = false;

            // Add the provided working titles
            if (ListArgsOnly.Length > 3)
            {
                Titles.AddRange(ListArgsOnly.Skip(3));
            }

            // Check for output type switches
            if (ListSwitchesOnly.Length > 0)
            {
                if (ListSwitchesOnly[0] == "-o")
                    OutputType = ChoiceStyle.ChoiceOutputType.OneLine;
                if (ListSwitchesOnly[0] == "-t")
                    OutputType = ChoiceStyle.ChoiceOutputType.TwoLines;
                if (ListSwitchesOnly[0] == "-m")
                    OutputType = ChoiceStyle.ChoiceOutputType.Modern;
                if (ListSwitchesOnly[0] == "-a")
                    OutputType = ChoiceStyle.ChoiceOutputType.Table;
            }

            // Prompt for choice
            UESHCommands.PromptChoiceAndSet(ListArgsOnly[2], ListArgsOnly[0], ListArgsOnly[1], Titles.ToArray(), OutputType, PressEnter);
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(Translate.DoTranslation("where <$variable> is any variable that will be used to store response") + CharManager.NewLine +
                                  Translate.DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"));

    }
}
