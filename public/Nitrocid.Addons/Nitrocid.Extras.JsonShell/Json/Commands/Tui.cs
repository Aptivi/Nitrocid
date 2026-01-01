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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.JsonShell.Tools;
using Nitrocid.Files.Operations;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Terminaux.Inputs.Styles.Editor;

namespace Nitrocid.Extras.JsonShell.Json.Commands
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
                TextWriters.Write(Translate.DoTranslation("The file stream is not open yet."), KernelColorType.Error);
                return 42;
            }
            string path = JsonShellCommon.FileStream.Name;
            List<string> lines = [.. Reading.ReadAllLinesNoBlock(path)];
            TextEditInteractive.OpenInteractive(ref lines);

            // Save the changes
            JsonTools.CloseJsonFile();
            Writing.WriteAllLinesNoBlock(path, [.. lines]);
            JsonTools.OpenJsonFile(path);
            return 0;
        }
    }
}
