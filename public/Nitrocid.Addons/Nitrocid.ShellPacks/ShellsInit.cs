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
using Nitrocid.Files.Paths;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.ShellPacks.Commands;
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
            new CommandInfo("archive", /* Localizable */ "Opens the archive file to the archive shell",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "archivefile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to archive file"
                        }),
                    ])
                ], new ArchiveCommand())
        ];

        private readonly List<CommandInfo> ftpAddonCommands =
        [
            new CommandInfo("ftp", /* Localizable */ "Use an FTP shell to interact with servers",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "FTP server to connect to"
                        }),
                    ])
                ], new FtpCommandExec())
        ];

        private readonly List<CommandInfo> gitAddonCommands =
        [
            new CommandInfo("gitsh", /* Localizable */ "Git shell",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "repoPath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to a directory with Git repository"
                        })
                    ]),
                ], new GitCommandExec())
        ];

        private readonly List<CommandInfo> httpAddonCommands =
        [
            new CommandInfo("http", /* Localizable */ "Starts the HTTP shell", new HttpCommandExec())
        ];

        private readonly List<CommandInfo> jsonAddonCommands =
        [
            new CommandInfo("jsondiff", /* Localizable */ "Shows the difference between two JSON files",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file1", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "First JSON file"
                        }),
                        new CommandArgumentPart(true, "file2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Second JSON file"
                        }),
                    ])
                ], new JsonDiffCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonbeautify", /* Localizable */ "Beautifies the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to JSON file"
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to output JSON file"
                        }),
                    ], true)
                ], new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("jsonminify", /* Localizable */ "Minifies the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to JSON file"
                        }),
                        new CommandArgumentPart(true, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to output JSON file"
                        }),
                    ], true)
                ], new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
        ];

        private readonly List<CommandInfo> mailAddonCommands =
        [
            new CommandInfo("mail", /* Localizable */ "Opens the IMAP mail client",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "emailAddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "E-mail address to login to"
                        }),
                    ])
                ], new MailCommandExec()),
            new CommandInfo("popmail", /* Localizable */ "Opens the POP3 mail client",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "emailAddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "E-mail address to login to"
                        }),
                    ])
                ], new PopMailCommandExec()),
        ];

        private readonly List<CommandInfo> rssAddonCommands =
        [
            new CommandInfo("rss", /* Localizable */ "Opens an RSS shell to read the feeds",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "feedlink", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "RSS feed link"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "Opens an interactive RSS feed reader TUI"),
                    ])
                ], new RssCommandExec())
        ];

        private readonly List<CommandInfo> sftpAddonCommands =
        [
            new CommandInfo("sftp", /* Localizable */ "Lets you use an SSH FTP server",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "SFTP server to connect to"
                        }),
                    ])
                ], new SftpCommandExec()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonShellPacks);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static ShellsConfig ShellsConfig =>
            (ShellsConfig)Config.baseConfigurations[nameof(ShellsConfig)];

        void IAddon.FinalizeAddon()
        {
            var config = new ShellsConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("ArchiveShell", new ArchiveShellInfo());
            ShellManager.RegisterAddonShell("FTPShell", new FTPShellInfo());
            ShellManager.RegisterAddonShell("GitShell", new GitShellInfo());
            ShellManager.RegisterAddonShell("HTTPShell", new HTTPShellInfo());
            ShellManager.RegisterAddonShell("JsonShell", new JsonShellInfo());
            ShellManager.RegisterAddonShell("MailShell", new MailShellInfo());
            ShellManager.RegisterAddonShell("RSSShell", new RSSShellInfo());
            ShellManager.RegisterAddonShell("SFTPShell", new SFTPShellInfo());
            ShellManager.RegisterAddonShell("SqlShell", new SqlShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. archiveAddonCommands]);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. ftpAddonCommands]);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. gitAddonCommands]);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. httpAddonCommands]);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. jsonAddonCommands]);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. mailAddonCommands]);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. rssAddonCommands]);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. sftpAddonCommands]);

            // Set the native lib path for Git
            if (!gitNativeLibIsSet)
            {
                gitNativeLibIsSet = true;
                GlobalSettings.NativeLibraryPath = PathsManagement.AddonsPath + "/ShellPacks/runtimes/" + KernelPlatform.GetCurrentGenericRid() + "/native/";
            }
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("ArchiveShell");
            ShellManager.UnregisterAddonShell("FTPShell");
            ShellManager.UnregisterAddonShell("GitShell");
            ShellManager.UnregisterAddonShell("HTTPShell");
            ShellManager.UnregisterAddonShell("JsonShell");
            ShellManager.UnregisterAddonShell("MailShell");
            ShellManager.UnregisterAddonShell("RSSShell");
            ShellManager.UnregisterAddonShell("SFTPShell");
            ShellManager.UnregisterAddonShell("SqlShell");
            ConfigTools.UnregisterBaseSetting(nameof(ShellsConfig));
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. archiveAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. ftpAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. gitAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. httpAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. jsonAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. mailAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. rssAddonCommands.Select((ci) => ci.Command)]);
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. sftpAddonCommands.Select((ci) => ci.Command)]);
        }
    }
}
