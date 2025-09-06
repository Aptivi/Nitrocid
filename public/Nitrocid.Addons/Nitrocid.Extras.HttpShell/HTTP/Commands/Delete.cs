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

using System;
using Nitrocid.Extras.HttpShell.Tools;
using Terminaux.Shell.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Inputs.Styles.Choice;
using Nitrocid.ConsoleBase.Colors;
using Textify.General;

namespace Nitrocid.Extras.HttpShell.HTTP.Commands
{
    /// <summary>
    /// Removes content from the HTTP server
    /// </summary>
    /// <remarks>
    /// If you want to test a DELETE function of the REST API, you can do so using this command.
    /// </remarks>
    class DeleteCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Print a message
            TextWriters.Write(Translate.DoTranslation("Deleting {0}..."), true, KernelColorType.Progress, parameters.ArgumentsList[0]);

            // Make a confirmation message so user will not accidentally delete a file or folder
            string answer = ChoiceStyle.PromptChoice(TextTools.FormatString(Translate.DoTranslation("Are you sure you want to delete {0}?"), parameters.ArgumentsList[0]), [("y", "Yes"), ("n", "No")]);
            if (answer != "y")
                return 1;

            try
            {
                var DeleteTask = HttpTools.HttpDelete(parameters.ArgumentsList[0]);
                DeleteTask.Wait();
                return 0;
            }
            catch (AggregateException aex)
            {
                TextWriters.Write(aex.Message + ":", true, KernelColorType.Error);
                foreach (Exception InnerException in aex.InnerExceptions)
                {
                    TextWriters.Write("- " + InnerException.Message, true, KernelColorType.Error);
                    if (InnerException.InnerException is not null)
                    {
                        TextWriters.Write("- " + InnerException.InnerException.Message, true, KernelColorType.Error);
                    }
                }
                return aex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriters.Write(ex.Message, true, KernelColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
