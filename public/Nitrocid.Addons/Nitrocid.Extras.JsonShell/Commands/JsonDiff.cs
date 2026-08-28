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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools;

namespace Nitrocid.Extras.JsonShell.Commands
{
    /// <summary>
    /// Shows a difference between two JSON files
    /// </summary>
    class JsonDiffCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "jsondiff";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_JSONDIFF_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "file1", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_JSONDIFF_ARGUMENT_FILE1_DESC"
                    }),
                    new CommandArgumentPart(true, "file2", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_JSONDIFF_ARGUMENT_FILE2_DESC"
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var source = JToken.Parse(FilesystemTools.ReadContentsText(parameters.ArgumentsList[0]));
            var target = JToken.Parse(FilesystemTools.ReadContentsText(parameters.ArgumentsList[1]));
            var diff = JsonTools.FindDifferences(source, target);
            TextWriterColor.Write(diff.ToString(Formatting.Indented), ThemeColorType.NeutralText);
            return 0;
        }
    }
}
