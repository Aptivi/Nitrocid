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

using System.Collections.Generic;
using Nitrocid.ShellPacks.Shells.Archive.Commands;
using Nitrocid.ShellPacks.Shells.Archive.Presets;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Nitrocid.Languages;

namespace Nitrocid.ShellPacks.Shells.Archive
{
    /// <summary>
    /// Common archive shell class
    /// </summary>
    internal class ArchiveShellInfo : BaseShellInfo<ArchiveShell>, IShellInfo
    {
        /// <summary>
        /// Archive commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("cdir", /* Localizable */ "NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDL_DESC", new CDirCommand()),

            new CommandInfo("chdir", /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_FS_COMMAND_CHDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_CHDIR_ARGUMENT_DIRECTORY_DESC"
                        })
                    ])
                ], new ChDirCommand()),

            new CommandInfo("chadir", /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_CHADIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "archivedirectory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_ARGUMENT_ARCHIVEDIRECTORY_DESC"
                        })
                    ])
                ], new ChADirCommand()),

            new CommandInfo("get", /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_GET_DESC",
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
                ], new GetCommand()),

            new CommandInfo("list", /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_LIST_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_ARGUMENT_ARCHIVEDIRECTORY_DESC"
                        })
                    ])
                ], new ListCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("pack", /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "localfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_ARGUMENT_LOCALFILE_DESC"
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_ARGUMENT_WHERE_DESC"
                        })
                    ])
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
    }
}
