
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.Mail;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Mail.Commands;
using System;
using KS.Shell.ShellBase.Switches;
using KS.Shell.ShellBase.Arguments;

namespace KS.Shell.Shells.Mail
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
                new CommandInfo("cd", ShellType, /* Localizable */ "Changes current mail directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "folder")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_CdCommand())
            },
            
            { "detach",
                new CommandInfo("detach", ShellType, /* Localizable */ "Exits the shell without disconnecting",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Mail_DetachCommand())
            },
            
            { "lsdirs",
                new CommandInfo("lsdirs", ShellType, /* Localizable */ "Lists directories in your mail address",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Mail_LsDirsCommand())
            },
            
            { "list",
                new CommandInfo("list", ShellType, /* Localizable */ "Downloads messages and lists them",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "pageNum")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_ListCommand())
            },
            
            { "mkdir",
                new CommandInfo("mkdir", ShellType, /* Localizable */ "Makes a directory in the current working directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "foldername")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_MkdirCommand())
            },
            
            { "mv",
                new CommandInfo("mv", ShellType, /* Localizable */ "Moves a message",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailId"),
                            new CommandArgumentPart(true, "targetFolder")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_MvCommand())
            },
            
            { "mvall",
                new CommandInfo("mvall", ShellType, /* Localizable */ "Moves all messages from recipient",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "senderName"),
                            new CommandArgumentPart(true, "targetFolder")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_MvAllCommand())
            },
            
            { "read",
                new CommandInfo("read", ShellType, /* Localizable */ "Opens a message",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailid")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_ReadCommand())
            },
            
            { "readenc",
                new CommandInfo("readenc", ShellType, /* Localizable */ "Opens an encrypted message",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailid")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_ReadEncCommand())
            },
            
            { "ren",
                new CommandInfo("ren", ShellType, /* Localizable */ "Renames a folder",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "oldFolderName"),
                            new CommandArgumentPart(true, "newFolderName")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_RenCommand())
            },
            
            { "rm",
                new CommandInfo("rm", ShellType, /* Localizable */ "Removes a message",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "mailid")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_RmCommand())
            },
            
            { "rmall",
                new CommandInfo("rmall", ShellType, /* Localizable */ "Removes all messages from recipient",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "sendername")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_RmAllCommand())
            },
            
            { "rmdir",
                new CommandInfo("rmdir", ShellType, /* Localizable */ "Removes a directory from the current working directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "foldername")
                        }, Array.Empty<SwitchInfo>())
                    }, new Mail_RmdirCommand())
            },
            
            { "send",
                new CommandInfo("send", ShellType, /* Localizable */ "Sends a message to an address",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Mail_SendCommand())
            },

            { "sendenc",
                new CommandInfo("sendenc", ShellType, /* Localizable */ "Sends an encrypted message to an address",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Mail_SendEncCommand())
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

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["MailShell"];

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => Network.Base.Connections.NetworkConnectionType.Mail.ToString();

    }
}
