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

using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.ArchiveShell.Archive.Shell.Commands
{
    /// <summary>
    /// Extract a file from a ZIP archive
    /// </summary>
    /// <remarks>
    /// If you want to get a single file from the ZIP archive, you can use this command to extract such file to the current working directory, or a specified directory.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-absolute</term>
    /// <description>Uses the full target path</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class GetCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "get";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_GET_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "entry", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_GET_ARGUMENT_ENTRY_DESC"
                    }),
                    new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_GET_ARGUMENT_WHERE_DESC"
                    })
                ],
                [
                    new SwitchInfo("absolute", /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_SWITCH_ABSOLUTE_DESC")
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string Where = "";
            var Absolute = false;
            if (parameters.ArgumentsList.Length > 1)
            {
                if (parameters.ContainsSwitch("-absolute"))
                    Absolute = true;
                else
                    Where = FilesystemTools.NeutralizePath(parameters.ArgumentsList[1]);
            }
            ArchiveTools.ExtractFileEntry(parameters.ArgumentsList[0], Where, Absolute);
            return 0;
        }

    }
}
