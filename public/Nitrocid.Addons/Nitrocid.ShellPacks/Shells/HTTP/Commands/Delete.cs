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
using Nitrocid.ShellPacks.Tools;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Colors.Themes.Colors;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.HTTP.Commands
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
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPHTTPSFTP_DELETING"), true, ThemeColorType.Progress, parameters.ArgumentsList[0]);

            // Make a confirmation message so user will not accidentally delete a file or folder
            string answer = ChoiceStyle.PromptChoice(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPHTTPSFTP_DELETECONFIRM").FormatString(parameters.ArgumentsList[0]), [("y", "Yes"), ("n", "No")]);
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
                TextWriterColor.Write(aex.Message + ":", true, ThemeColorType.Error);
                foreach (Exception InnerException in aex.InnerExceptions)
                {
                    TextWriterColor.Write("- " + InnerException.Message, true, ThemeColorType.Error);
                    if (InnerException.InnerException is not null)
                    {
                        TextWriterColor.Write("- " + InnerException.InnerException.Message, true, ThemeColorType.Error);
                    }
                }
                return aex.GetHashCode();
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(ex.Message, true, ThemeColorType.Error);
                return ex.GetHashCode();
            }
        }

    }
}
