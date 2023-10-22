
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
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.SFTP.Commands;
using KS.Shell.ShellBase.Switches;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.Shells.SFTP.Presets;

namespace KS.Shell.Shells.SFTP
{
    /// <summary>
    /// Common SFTP shell class
    /// </summary>
    internal class SFTPShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// SFTP commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cat",
                new CommandInfo("cat", ShellType, /* Localizable */ "Reads the content of a remote file to the console",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file")
                        })
                    }, new CatCommand(), CommandFlags.Wrappable)
            },
            
            { "cdl",
                new CommandInfo("cdl", ShellType, /* Localizable */ "Changes local directory to download to or upload from",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory")
                        })
                    }, new CdlCommand())
            },
            
            { "cdr",
                new CommandInfo("cdr", ShellType, /* Localizable */ "Changes remote directory to download from or upload to",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory")
                        })
                    }, new CdrCommand())
            },
            
            { "del",
                new CommandInfo("del", ShellType, /* Localizable */ "Deletes remote file from server",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file")
                        })
                    }, new DelCommand())
            },
            
            { "detach",
                new CommandInfo("detach", ShellType, /* Localizable */ "Exits the shell without disconnecting",
                    new[] {
                        new CommandArgumentInfo()
                    }, new DetachCommand())
            },
            
            { "get",
                new CommandInfo("get", ShellType, /* Localizable */ "Downloads remote file to local directory using binary or text",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file")
                        })
                    }, new GetCommand())
            },
            
            { "lsl",
                new CommandInfo("lsl", ShellType, /* Localizable */ "Lists local directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "dir")
                        }, new[] {
                            new SwitchInfo("showdetails", /* Localizable */ "Shows the details of the files and folders", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("suppressmessages", /* Localizable */ "Suppresses the \"unauthorized\" messages", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new LslCommand(), CommandFlags.Wrappable)
            },
            
            { "lsr",
                new CommandInfo("lsr", ShellType, /* Localizable */ "Lists remote directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "dir")
                        }, new[] {
                            new SwitchInfo("showdetails", /* Localizable */ "Shows the details of the files and folders", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new LsrCommand(), CommandFlags.Wrappable)
            },
            
            { "put",
                new CommandInfo("put", ShellType, /* Localizable */ "Uploads local file to remote directory using binary or text",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file")
                        })
                    }, new PutCommand())
            },
            
            { "pwdl",
                new CommandInfo("pwdl", ShellType, /* Localizable */ "Gets current local directory",
                    new[] {
                        new CommandArgumentInfo()
                    }, new PwdlCommand())
            },

            { "pwdr",
                new CommandInfo("pwdr", ShellType, /* Localizable */ "Gets current remote directory",
                    new[] {
                        new CommandArgumentInfo()
                    }, new PwdrCommand())
            },
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new SFTPDefaultPreset() },
            { "PowerLine1", new SftpPowerLine1Preset() },
            { "PowerLine2", new SftpPowerLine2Preset() },
            { "PowerLine3", new SftpPowerLine3Preset() },
            { "PowerLineBG1", new SftpPowerLineBG1Preset() },
            { "PowerLineBG2", new SftpPowerLineBG2Preset() },
            { "PowerLineBG3", new SftpPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new SFTPShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => Network.Base.Connections.NetworkConnectionType.SFTP.ToString();

    }
}
