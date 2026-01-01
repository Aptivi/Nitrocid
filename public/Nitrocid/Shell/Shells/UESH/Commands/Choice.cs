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
using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
    /// </list>
    /// <br></br>
    /// </remarks>
    class ChoiceCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var Titles = new List<(string, string)>();
            var PressEnter = false;
            var OutputType = (ChoiceOutputType)Config.MainConfig.DefaultChoiceOutputType;
            if (parameters.SwitchesList.Contains("-multiple"))
                PressEnter = true;
            if (parameters.SwitchesList.Contains("-single"))
                PressEnter = false;

            // Add the provided working titles
            if (parameters.ArgumentsList.Length > 2)
            {
                var titles = parameters.ArgumentsList.Skip(2).ToArray();
                var split = parameters.ArgumentsText.Split('/');
                for (int i = 0; i < split.Length; i++)
                {
                    string answer = split[i];
                    string title = i >= titles.Length ? $"[{i + 1}]" : titles[i];
                    Titles.Add((answer, title));
                }
            }

            // Check for output type switches
            if (parameters.SwitchesList.Length > 0)
            {
                if (parameters.SwitchesList[0] == "-o")
                    OutputType = ChoiceOutputType.OneLine;
                if (parameters.SwitchesList[0] == "-t")
                    OutputType = ChoiceOutputType.TwoLines;
                if (parameters.SwitchesList[0] == "-m")
                    OutputType = ChoiceOutputType.Modern;
            }

            // Prompt for choice
            var settings = new ChoiceStyleSettings()
            {
                OutputType = OutputType,
                PressEnter = PressEnter,
            };
            string Answer = ChoiceStyle.PromptChoice(parameters.ArgumentsList[1], [.. Titles], settings);
            variableValue = Answer;
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(Translate.DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"));

    }
}
