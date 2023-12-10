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
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Arguments;
using Nitrocid.Extras.MailShell.Mail.Presets;
using Nitrocid.Extras.MailShell.Mail.Commands;

namespace Nitrocid.Extras.MailShell.Mail
{
    /// <summary>
    /// Common mail shell class
    /// </summary>
    internal class MailShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Mail commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cd",
                new CommandInfo("cd", /* Localizable */ "Changes current mail directory",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "folder")
                        })
                    ], new CdCommand())
            },

            { "detach",
                new CommandInfo("detach", /* Localizable */ "Exits the shell without disconnecting",
                    [
                        new CommandArgumentInfo()
                    ], new DetachCommand())
            },

            { "lsdirs",
                new CommandInfo("lsdirs", /* Localizable */ "Lists directories in your mail address",
                    [
                        new CommandArgumentInfo()
                    ], new LsDirsCommand())
            },

            { "list",
                new CommandInfo("list", /* Localizable */ "Downloads messages and lists them",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "pageNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    ], new ListCommand())
            },

            { "mkdir",
                new CommandInfo("mkdir", /* Localizable */ "Makes a directory in the current working directory",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "foldername")
                        })
                    ], new MkdirCommand())
            },

            { "mv",
                new CommandInfo("mv", /* Localizable */ "Moves a message",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailId", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(true, "targetFolder")
                        })
                    ], new MvCommand())
            },

            { "mvall",
                new CommandInfo("mvall", /* Localizable */ "Moves all messages from recipient",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "senderName"),
                            new CommandArgumentPart(true, "targetFolder")
                        })
                    ], new MvAllCommand())
            },

            { "read",
                new CommandInfo("read", /* Localizable */ "Opens a message",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    ], new ReadCommand())
            },

            { "readenc",
                new CommandInfo("readenc", /* Localizable */ "Opens an encrypted message",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    ], new ReadEncCommand())
            },

            { "ren",
                new CommandInfo("ren", /* Localizable */ "Renames a folder",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "oldFolderName"),
                            new CommandArgumentPart(true, "newFolderName")
                        })
                    ], new RenCommand())
            },

            { "rm",
                new CommandInfo("rm", /* Localizable */ "Removes a message",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    ], new RmCommand())
            },

            { "rmall",
                new CommandInfo("rmall", /* Localizable */ "Removes all messages from recipient",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "sendername")
                        })
                    ], new RmAllCommand())
            },

            { "rmdir",
                new CommandInfo("rmdir", /* Localizable */ "Removes a directory from the current working directory",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "foldername")
                        })
                    ], new RmdirCommand())
            },

            { "send",
                new CommandInfo("send", /* Localizable */ "Sends a message to an address",
                    [
                        new CommandArgumentInfo()
                    ], new SendCommand())
            },

            { "sendenc",
                new CommandInfo("sendenc", /* Localizable */ "Sends an encrypted message to an address",
                    [
                        new CommandArgumentInfo()
                    ], new SendEncCommand())
            }
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new MailDefaultPreset() },
            { "PowerLine1", new MailPowerLine1Preset() },
            { "PowerLine2", new MailPowerLine2Preset() },
            { "PowerLine3", new MailPowerLine3Preset() },
            { "PowerLineBG1", new MailPowerLineBG1Preset() },
            { "PowerLineBG2", new MailPowerLineBG2Preset() },
            { "PowerLineBG3", new MailPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new MailShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "Mail";

    }
}
