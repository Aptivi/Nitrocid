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

using LibGit2Sharp;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Nitrocid.ShellPacks.Commands;
using Nitrocid.ShellPacks.Localized;
using Nitrocid.ShellPacks.Settings;
using Nitrocid.ShellPacks.Shells.Archive;
using Nitrocid.ShellPacks.Shells.FTP;
using Nitrocid.ShellPacks.Shells.Git;
using Nitrocid.ShellPacks.Shells.HTTP;
using Nitrocid.ShellPacks.Shells.Json;
using Nitrocid.ShellPacks.Shells.Mail;
using Nitrocid.ShellPacks.Shells.RSS;
using Nitrocid.ShellPacks.Shells.SFTP;
using Nitrocid.ShellPacks.Shells.Sql;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.ShellPacks
{
    internal class ShellsInit : IAddon
    {
        private static bool gitNativeLibIsSet = false;

        private readonly List<CommandInfo> archiveAddonCommands =
        [
            new CommandInfo("archive", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_ARCHIVE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "archivefile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_ARCHIVE_ARGUMENT_ARCHIVEFILE_DESC"
                        }),
                    ])
                ], new ArchiveCommand())
        ];

        private readonly List<CommandInfo> ftpAddonCommands =
        [
            new CommandInfo("ftp", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_FTP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_FTP_ARGUMENT_SERVER_DESC"
                        }),
                    ])
                ], new FtpCommandExec())
        ];

        private readonly List<CommandInfo> gitAddonCommands =
        [
            new CommandInfo("gitsh", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_GITSH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "repoPath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_GITSH_ARGUMENT_REPOPATH_DESC"
                        })
                    ]),
                ], new GitCommandExec())
        ];

        private readonly List<CommandInfo> httpAddonCommands =
        [
            new CommandInfo("http", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_HTTP_DESC", new HttpCommandExec())
        ];

        private readonly List<CommandInfo> jsonAddonCommands =
        [
            new CommandInfo("jsondiff", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_JSONDIFF_DESC",
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
                ], new JsonDiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonbeautify", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_JSONBEAUTIFY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_JSONFILE_DESC"
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_OUTPUT_DESC"
                        }),
                    ], true)
                ], new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonminify", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_JSONMINIFY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_JSONFILE_DESC"
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_ARGUMENT_OUTPUT_DESC"
                        }),
                    ], true)
                ], new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
        ];

        private readonly List<CommandInfo> mailAddonCommands =
        [
            new CommandInfo("mail", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_MAIL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "emailAddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_MAIL_ARGUMENT_ADDRESS_DESC"
                        }),
                    ])
                ], new MailCommandExec()),
            new CommandInfo("popmail", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_POPMAIL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "emailAddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_MAIL_ARGUMENT_ADDRESS_DESC"
                        }),
                    ])
                ], new PopMailCommandExec()),
        ];

        private readonly List<CommandInfo> rssAddonCommands =
        [
            new CommandInfo("rss", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_RSS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "feedlink", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_RSS_ARGUMENT_FEEDLINK_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_RSS_SWITCH_TUI_DESC"),
                    ])
                ], new RssCommandExec())
        ];

        private readonly List<CommandInfo> sftpAddonCommands =
        [
            new CommandInfo("sftp", /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_SFTP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_COMMON_COMMAND_SFTP_ARGUMENT_SERVER_DESC"
                        }),
                    ])
                ], new SftpCommandExec()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonShellPacks);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.AddonShellPacks);

        internal static ShellsConfig ShellsConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ShellsConfig)) ? (ShellsConfig)Config.baseConfigurations[nameof(ShellsConfig)] : Config.GetFallbackKernelConfig<ShellsConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new ShellsConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("ArchiveShell", new ArchiveShellInfo());
            ShellManager.RegisterShell("FTPShell", new FTPShellInfo());
            ShellManager.RegisterShell("GitShell", new GitShellInfo());
            ShellManager.RegisterShell("HTTPShell", new HTTPShellInfo());
            ShellManager.RegisterShell("JsonShell", new JsonShellInfo());
            ShellManager.RegisterShell("MailShell", new MailShellInfo());
            ShellManager.RegisterShell("RSSShell", new RSSShellInfo());
            ShellManager.RegisterShell("SFTPShell", new SFTPShellInfo());
            ShellManager.RegisterShell("SqlShell", new SqlShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. archiveAddonCommands]);
            CommandManager.RegisterCustomCommands("Shell", [.. ftpAddonCommands]);
            CommandManager.RegisterCustomCommands("Shell", [.. gitAddonCommands]);
            CommandManager.RegisterCustomCommands("Shell", [.. httpAddonCommands]);
            CommandManager.RegisterCustomCommands("Shell", [.. jsonAddonCommands]);
            CommandManager.RegisterCustomCommands("Shell", [.. mailAddonCommands]);
            CommandManager.RegisterCustomCommands("Shell", [.. rssAddonCommands]);
            CommandManager.RegisterCustomCommands("Shell", [.. sftpAddonCommands]);

            // Set the native lib path for Git
            if (!gitNativeLibIsSet)
            {
                gitNativeLibIsSet = true;
                GlobalSettings.NativeLibraryPath = PathsManagement.AddonsPath + "/ShellPacks/runtimes/" + KernelPlatform.GetCurrentGenericRid() + "/native/";
            }
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("ArchiveShell");
            ShellManager.UnregisterShell("FTPShell");
            ShellManager.UnregisterShell("GitShell");
            ShellManager.UnregisterShell("HTTPShell");
            ShellManager.UnregisterShell("JsonShell");
            ShellManager.UnregisterShell("MailShell");
            ShellManager.UnregisterShell("RSSShell");
            ShellManager.UnregisterShell("SFTPShell");
            ShellManager.UnregisterShell("SqlShell");
            ConfigTools.UnregisterBaseSetting(nameof(ShellsConfig));
            CommandManager.UnregisterCustomCommands("Shell", [.. archiveAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterCustomCommands("Shell", [.. ftpAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterCustomCommands("Shell", [.. gitAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterCustomCommands("Shell", [.. httpAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterCustomCommands("Shell", [.. jsonAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterCustomCommands("Shell", [.. mailAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterCustomCommands("Shell", [.. rssAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterCustomCommands("Shell", [.. sftpAddonCommands.Select((ci) => ci.Command)]);
        }
    }
}
