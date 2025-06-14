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
using System.Linq;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lets the user make a selection
    /// </summary>
    /// <remarks>
    /// This command can be used in a scripting file that ends in .uesh file extension. It lets the user select the correct answers when answering this question and passes the chosen answer to the specified variable.
    /// </remarks>
    class SelectCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var Titles = new List<(string, string)>();

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

            // Prompt for selection
            int SelectedAnswer = SelectionStyle.PromptSelection(parameters.ArgumentsList[1], [.. Titles]);
            variableValue = $"{SelectedAnswer}";
            return 0;
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHOICE_HELPER"));

    }
}
