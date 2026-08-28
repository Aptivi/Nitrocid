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

using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// This command wraps your text file
    /// </summary>
    /// <remarks>
    /// This command wraps the contents of your text file with the specified number of characters (or columns) per line.
    /// </remarks>
    class WrapTextCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "wraptext";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_WRAPTEXT_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("columns", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_WRAPTEXT_SWITCH_COLUMNS_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true,
                        IsNumeric = true
                    })
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            int columns = 78;
            string textFile = parameters.ArgumentsList[0];
            if (parameters.ContainsSwitch("-columns"))
            {
                string parsedColumns = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-columns");
                if (!int.TryParse(parsedColumns, out columns))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_WRAPTEXT_COLUMNNUMINVALID"), true, ThemeColorType.Error);
                    return 20;
                }
            }
            FilesystemTools.WrapTextFile(textFile, columns);
            return 0;
        }

    }
}
