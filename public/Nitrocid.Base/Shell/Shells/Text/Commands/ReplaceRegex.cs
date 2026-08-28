//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Files.Editors.TextEdit;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Replaces a word or phrase with another one using regular expressions
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a word or phrase enclosed in double quotes with another one enclosed in double quotes.
    /// </remarks>
    class ReplaceRegexCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "replaceregex";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEREGEX_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "regex", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEREGEX_ARGUMENT_REGEX_DESC"
                    }),
                    new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_TARGET_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string regexStr = parameters.ArgumentsList[0];
            string replacementStr = parameters.ArgumentsList[1];
            TextEditTools.ReplaceRegex(regexStr, replacementStr);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_REPLACE_SUCCESS"), true, ThemeColorType.Success);
            return 0;
        }

    }
}
