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
using Nitrocid.ShellPacks.Shells.FTP.Presets;
using Nitrocid.ShellPacks.Shells.FTP.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;

namespace Nitrocid.ShellPacks.Shells.FTP
{
    /// <summary>
    /// Common FTP shell class
    /// </summary>
    internal class FTPShellInfo : BaseShellInfo<FTPShell>, IShellInfo
    {
        /// <summary>
        /// FTP commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("cat", /* Localizable */ "Reads the content of a remote file to the console",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote file"
                        })
                    ])
                ], new CatCommand(), CommandFlags.Wrappable),

            new CommandInfo("cdl", /* Localizable */ "Changes local directory to download to or upload from",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        })
                    ])
                ], new CdlCommand()),

            new CommandInfo("cdr", /* Localizable */ "Changes remote directory to download from or upload to",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        })
                    ])
                ], new CdrCommand()),

            new CommandInfo("cp", /* Localizable */ "Copies file or directory to another file or directory.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourcefileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Source file or directory"
                        }),
                        new CommandArgumentPart(true, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target file or directory"
                        })
                    ])
                ], new CpCommand()),

            new CommandInfo("del", /* Localizable */ "Deletes remote file from server",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote file to delete"
                        })
                    ])
                ], new DelCommand()),

            new CommandInfo("detach", /* Localizable */ "Exits the shell without disconnecting", new DetachCommand()),

            new CommandInfo("execute", /* Localizable */ "Executes an FTP server command",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Server command to execute"
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        })
                    ])
                ], new ExecuteCommand()),

            new CommandInfo("get", /* Localizable */ "Downloads remote file to local directory using binary or text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote file"
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("getfolder", /* Localizable */ "Downloads remote folder to local directory using binary or text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        })
                    ])
                ], new GetFolderCommand()),

            new CommandInfo("ifm", /* Localizable */ "Interactive system host file manager", new IfmCommand()),

            new CommandInfo("info", /* Localizable */ "FTP server information", new InfoCommand()),

            new CommandInfo("lsl", /* Localizable */ "Lists local directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "Shows the file details in the list", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", /* Localizable */ "Suppresses the annoying \"permission denied\" messages", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LslCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsr", /* Localizable */ "Lists remote directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "Shows the file details in the list", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsrCommand(), CommandFlags.Wrappable),

            new CommandInfo("mkldir", /* Localizable */ "Creates a local directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        }),
                    ], true)
                ], new MkldirCommand()),

            new CommandInfo("mkrdir", /* Localizable */ "Creates a remote directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        }),
                    ], true)
                ], new MkrdirCommand()),

            new CommandInfo("mv", /* Localizable */ "Moves file or directory to another file or directory. You can also use that to rename files.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourcefileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to source file or directory"
                        }),
                        new CommandArgumentPart(true, "targetfileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to target file or directory"
                        })
                    ])
                ], new MvCommand()),

            new CommandInfo("put", /* Localizable */ "Uploads local file to remote directory using binary or text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local file"
                        }),
                        new CommandArgumentPart(false, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        })
                    ])
                ], new PutCommand()),

            new CommandInfo("putfolder", /* Localizable */ "Uploads local folder to remote directory using binary or text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        }),
                        new CommandArgumentPart(false, "outputfolder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        })
                    ])
                ], new PutFolderCommand()),

            new CommandInfo("pwdl", /* Localizable */ "Gets current local directory", new PwdlCommand()),

            new CommandInfo("pwdr", /* Localizable */ "Gets current remote directory", new PwdrCommand()),

            new CommandInfo("perm", /* Localizable */ "Sets file permissions. This is supported only on FTP servers that run Unix.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        }),
                        new CommandArgumentPart(true, "permnumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Permission numbers according to chmod"
                        })
                    ])
                ], new PermCommand()),

            new CommandInfo("sumfile", /* Localizable */ "Calculates file sums.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote file"
                        }),
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Encryption algorithm"
                        })
                    ])
                ], new SumFileCommand()),

            new CommandInfo("sumfiles", /* Localizable */ "Calculates sums of files in specified directory.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        }),
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Encryption algorithm"
                        })
                    ])
                ], new SumFilesCommand()),

            new CommandInfo("type", /* Localizable */ "Sets the type for this session",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "a/b", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Specifies whether the transfer type is ASCII or Binary"
                        })
                    ])
                ], new TypeCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new FTPDefaultPreset() },
            { "PowerLine1", new FtpPowerLine1Preset() },
            { "PowerLine2", new FtpPowerLine2Preset() },
            { "PowerLine3", new FtpPowerLine3Preset() },
            { "PowerLineBG1", new FtpPowerLineBG1Preset() },
            { "PowerLineBG2", new FtpPowerLineBG2Preset() },
            { "PowerLineBG3", new FtpPowerLineBG3Preset() }
        };

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "FTP";
    }
}
