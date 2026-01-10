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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Base.Languages;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int columns = 78;
            if (parameters.ContainsSwitch("-columns"))
            {
                string parsedColumns = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-columns");
                if (!int.TryParse(parsedColumns, out columns))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_WRAPTEXT_COLUMNNUMINVALID"), true, ThemeColorType.Error);
                    return 20;
                }
            }
            FilesystemTools.WrapTextFile(parameters.ArgumentsList[0], columns);
            return 0;
        }

    }
}
