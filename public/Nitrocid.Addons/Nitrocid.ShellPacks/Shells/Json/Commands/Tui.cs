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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Terminaux.Inputs.Styles.Editor;

namespace Nitrocid.ShellPacks.Shells.Json.Commands
{
    /// <summary>
    /// Opens the JSON file in the interactive editor
    /// </summary>
    /// <remarks>
    /// This command will open the currently open JSON file in the interactive text editor.
    /// </remarks>
    class TuiCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (JsonShellCommon.FileStream is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_JSON_STREAMNOTOPEN"), ThemeColorType.Error);
                return 42;
            }
            string path = JsonShellCommon.FileStream.Name;
            List<string> lines = [.. FilesystemTools.ReadAllLinesNoBlock(path)];
            TextEditInteractive.OpenInteractive(ref lines);

            // Save the changes
            JsonTools.CloseJsonFile();
            FilesystemTools.WriteAllLinesNoBlock(path, [.. lines]);
            JsonTools.OpenJsonFile(path);
            return 0;
        }
    }
}
