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
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
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
            new CommandInfo("cdir", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDL_DESC", "Nitrocid.ShellPacks"), new CDirCommand()),

            new CommandInfo("chdir", LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_FS_COMMAND_CHDIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_CHDIR_ARGUMENT_DIRECTORY_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new ChDirCommand()),

            new CommandInfo("chadir", LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_CHADIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "archivedirectory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_ARGUMENT_ARCHIVEDIRECTORY_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new ChADirCommand()),

            new CommandInfo("get", LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_GET_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "entry", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_GET_ARGUMENT_ENTRY_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_GET_ARGUMENT_WHERE_DESC", "Nitrocid.ShellPacks")
                        })
                    ],
                    [
                        new SwitchInfo("absolute", LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_SWITCH_ABSOLUTE_DESC", "Nitrocid.ShellPacks"))
                    ])
                ], new GetCommand()),

            new CommandInfo("list", LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_LIST_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_ARGUMENT_ARCHIVEDIRECTORY_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new ListCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("pack", LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "localfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_ARGUMENT_LOCALFILE_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_COMMAND_PACK_ARGUMENT_WHERE_DESC", "Nitrocid.ShellPacks")
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
