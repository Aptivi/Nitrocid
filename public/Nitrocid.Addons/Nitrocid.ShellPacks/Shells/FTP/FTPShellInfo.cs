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
using Nitrocid.Languages;

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
            new CommandInfo("cat", "NKS_SHELLPACKS_FTPSFTP_COMMAND_CAT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC"
                        })
                    ])
                ], new CatCommand(), CommandFlags.Wrappable),

            new CommandInfo("cdl", "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        })
                    ])
                ], new CdlCommand()),

            new CommandInfo("cdr", "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        })
                    ])
                ], new CdrCommand()),

            new CommandInfo("cp", "NKS_SHELLPACKS_FTP_COMMAND_CP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourcefileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_SOURCEFILEORDIR_DESC"
                        }),
                        new CommandArgumentPart(true, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_WHERE_DESC"
                        })
                    ])
                ], new CpCommand()),

            new CommandInfo("del", "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_DEL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_DEL_ARGUMENT_REMOTEFILE_DESC"
                        })
                    ])
                ], new DelCommand()),

            new CommandInfo("detach", "NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", new DetachCommand()),

            new CommandInfo("execute", "NKS_SHELLPACKS_FTP_COMMAND_EXECUTE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_EXECUTE_ARGUMENT_COMMAND_DESC"
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        })
                    ])
                ], new ExecuteCommand()),

            new CommandInfo("get", "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_GET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC"
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("getfolder", "NKS_SHELLPACKS_FTP_COMMAND_GETFOLDER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        })
                    ])
                ], new GetFolderCommand()),

            new CommandInfo("ifm", "NKS_SHELLPACKS_FTP_COMMAND_IFM_DESC", new IfmCommand()),

            new CommandInfo("info", "NKS_SHELLPACKS_FTP_SERVERINFO_TITLE", new InfoCommand()),

            new CommandInfo("lsl", "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", "NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SHOWDETAILS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", "NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SUPPRESSMESSAGES_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LslCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsr", "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", "NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SHOWDETAILS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsrCommand(), CommandFlags.Wrappable),

            new CommandInfo("mkldir", "NKS_SHELLPACKS_FTPSFTP_COMMAND_MKLDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        }),
                    ], true)
                ], new MkldirCommand()),

            new CommandInfo("mkrdir", "NKS_SHELLPACKS_FTPSFTP_COMMAND_MKRDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        }),
                    ], true)
                ], new MkrdirCommand()),

            new CommandInfo("mv", "NKS_SHELLPACKS_FTP_COMMAND_MV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourcefileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_MV_ARGUMENT_SOURCEFILEORDIR_DESC"
                        }),
                        new CommandArgumentPart(true, "targetfileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_MV_ARGUMENT_TARGETFILEORDIR_DESC"
                        })
                    ])
                ], new MvCommand()),

            new CommandInfo("put", "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_PUT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_PUT_ARGUMENT_FILE_DESC"
                        }),
                        new CommandArgumentPart(false, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        })
                    ])
                ], new PutCommand()),

            new CommandInfo("putfolder", "NKS_SHELLPACKS_FTP_COMMAND_PUTFOLDER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        }),
                        new CommandArgumentPart(false, "outputfolder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        })
                    ])
                ], new PutFolderCommand()),

            new CommandInfo("pwdl", "NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDL_DESC", new PwdlCommand()),

            new CommandInfo("pwdr", "NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDR_DESC", new PwdrCommand()),

            new CommandInfo("perm", "NKS_SHELLPACKS_FTP_COMMAND_PERM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        }),
                        new CommandArgumentPart(true, "permnumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_PERM_ARGUMENT_PERMNUMBER_DESC"
                        })
                    ])
                ], new PermCommand()),

            new CommandInfo("sumfile", "NKS_SHELLPACKS_FTP_COMMAND_SUMFILE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC"
                        }),
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_ALGORITHM_DESC"
                        })
                    ])
                ], new SumFileCommand()),

            new CommandInfo("sumfiles", "NKS_SHELLPACKS_FTP_COMMAND_SUMFILES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        }),
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_ALGORITHM_DESC"
                        })
                    ])
                ], new SumFilesCommand()),

            new CommandInfo("type", "NKS_SHELLPACKS_FTP_COMMAND_TYPE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "a/b", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_FTP_COMMAND_TYPE_ARGUMENT_TYPE_DESC"
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
