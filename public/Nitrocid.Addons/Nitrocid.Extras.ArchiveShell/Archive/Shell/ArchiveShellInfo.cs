//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System.Collections.Generic;
using Nitrocid.Extras.ArchiveShell.Archive.Shell.Commands;
using Nitrocid.Extras.ArchiveShell.Archive.Shell.Presets;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;

namespace Nitrocid.Extras.ArchiveShell.Archive.Shell
{
    /// <summary>
    /// Common archive shell class
    /// </summary>
    internal class ArchiveShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Archive commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("cdir", /* Localizable */ "Gets current local directory",
                [
                    new CommandArgumentInfo()
                ], new CDirCommand()),

            new CommandInfo("chdir", /* Localizable */ "Changes directory",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "directory")
                    })
                ], new ChDirCommand()),

            new CommandInfo("chadir", /* Localizable */ "Changes archive directory",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "archivedirectory")
                    })
                ], new ChADirCommand()),

            new CommandInfo("get", /* Localizable */ "Extracts a file to a specified directory or a current directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "entry"),
                        new CommandArgumentPart(false, "where")
                    ],
                    [
                        new SwitchInfo("absolute", /* Localizable */ "Indicates that the target path is absolute")
                    ])
                ], new GetCommand()),

            new CommandInfo("list", /* Localizable */ "Lists all files inside the archive",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(false, "directory")
                    })
                ], new ListCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("pack", /* Localizable */ "Packs a local file to the archive",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "localfile"),
                        new CommandArgumentPart(false, "where")
                    })
                ], new PackCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new ArchiveDefaultPreset() },
            { "PowerLine1", new ArchivePowerLine1Preset() },
            { "PowerLine2", new ArchivePowerLine2Preset() },
            { "PowerLine3", new ArchivePowerLine3Preset() },
            { "PowerLineBG1", new ArchivePowerLineBG1Preset() },
            { "PowerLineBG2", new ArchivePowerLineBG2Preset() },
            { "PowerLineBG3", new ArchivePowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new ArchiveShell();

    }
}
