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

using LibGit2Sharp;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Core.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
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
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.ShellPacks.Shells.RSS.Widgets;
using SpecProbe.Software.Platform;
using Nitrocid.ShellPacks.Shells.Archive.UESHCommands;
using Nitrocid.ShellPacks.Shells.FTP.UESHCommands;
using Nitrocid.ShellPacks.Shells.Git.UESHCommands;
using Nitrocid.ShellPacks.Shells.Json.UESHCommands;
using Nitrocid.ShellPacks.Shells.Mail.UESHCommands;
using Nitrocid.ShellPacks.Shells.RSS.UESHCommands;
using Nitrocid.ShellPacks.Shells.SFTP.UESHCommands;
using Nitrocid.ShellPacks.Shells.HTTP.UESHCommands;

namespace Nitrocid.ShellPacks
{
    internal class ShellsInit : IAddon
    {
        private static bool gitNativeLibIsSet = false;

        private readonly List<BaseCommand> archiveAddonCommands =
        [
            new ArchiveCommand(),
        ];

        private readonly List<BaseCommand> ftpAddonCommands =
        [
            new FtpCommandExec(),
        ];

        private readonly List<BaseCommand> gitAddonCommands =
        [
            new GitCommandExec(),
        ];

        private readonly List<BaseCommand> httpAddonCommands =
        [
            new HttpCommandExec(),
        ];

        private readonly List<BaseCommand> jsonAddonCommands =
        [
            new JsonDiffCommand(),
            new JsonBeautifyCommand(),
            new JsonMinifyCommand(),
        ];

        private readonly List<BaseCommand> mailAddonCommands =
        [
            new MailCommandExec(),
            new PopMailCommandExec(),
            new IspInfoCommand(),
        ];

        private readonly List<BaseCommand> rssAddonCommands =
        [
            new RssCommandExec(),
        ];

        private readonly List<BaseCommand> sftpAddonCommands =
        [
            new SftpCommandExec(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonShellPacks);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.AddonShellPacks);

        internal static ShellsConfig ShellsConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ShellsConfig)) ? (ShellsConfig)Config.baseConfigurations[nameof(ShellsConfig)] : Config.GetFallbackKernelConfig<ShellsConfig>();

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.ShellPacks.Resources.Languages.Output.Localizations", typeof(ShellsInit).Assembly));
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
            WidgetTools.AddBaseWidget(new RssFeedSingle());
            WidgetTools.AddBaseWidget(new RssFeeds());

            // Set the native lib path for Git
            if (!gitNativeLibIsSet)
            {
                gitNativeLibIsSet = true;
                GlobalSettings.NativeLibraryPath = PathsManagement.AddonsPath + "/ShellPacks/runtimes/" + PlatformHelper.GetCurrentGenericRid() + "/native/";
            }
        }

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
            WidgetTools.RemoveBaseWidget(nameof(RssFeedSingle));
            WidgetTools.RemoveBaseWidget(nameof(RssFeeds));
        }
    }
}
