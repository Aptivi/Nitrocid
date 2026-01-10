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
using Terminaux.Shell.Commands;
using Nitrocid.Base.Files.Editors.TextEdit;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Shell.Shells.Text.Commands
{
    /// <summary>
    /// Replaces a word or phrase with another one
    /// </summary>
    /// <remarks>
    /// You can use this command to replace a word or phrase enclosed in double quotes with another one enclosed in double quotes.
    /// </remarks>
    class ReplaceCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextEditTools.Replace(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_TEXT_REPLACE_SUCCESS"), true, ThemeColorType.Success);
            return 0;
        }

    }
}
