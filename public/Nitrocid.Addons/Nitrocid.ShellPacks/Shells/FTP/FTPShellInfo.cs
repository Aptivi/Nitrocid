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
            new CommandInfo("cat", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_CAT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CatCommand(), CommandFlags.Wrappable),

            new CommandInfo("cdl", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CdlCommand()),

            new CommandInfo("cdr", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CdrCommand()),

            new CommandInfo("cp", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_CP_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourcefileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_SOURCEFILEORDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_WHERE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CpCommand()),

            new CommandInfo("del", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_DEL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_DEL_ARGUMENT_REMOTEFILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new DelCommand()),

            new CommandInfo("detach", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", "Nitrocid.ShellPacks"), new DetachCommand()),

            new CommandInfo("execute", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_EXECUTE_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_EXECUTE_ARGUMENT_COMMAND_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new ExecuteCommand()),

            new CommandInfo("get", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_GET_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("getfolder", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_GETFOLDER_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "where", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new GetFolderCommand()),

            new CommandInfo("ifm", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_IFM_DESC", "Nitrocid.ShellPacks"), new IfmCommand()),

            new CommandInfo("info", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_SERVERINFO_TITLE", "Nitrocid.ShellPacks"), new InfoCommand()),

            new CommandInfo("lsl", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SHOWDETAILS_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SUPPRESSMESSAGES_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LslCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsr", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_SWITCH_SHOWDETAILS_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsrCommand(), CommandFlags.Wrappable),

            new CommandInfo("mkldir", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_MKLDIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                    ], true)
                ], new MkldirCommand()),

            new CommandInfo("mkrdir", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_MKRDIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                    ], true)
                ], new MkrdirCommand()),

            new CommandInfo("mv", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_MV_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sourcefileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_MV_ARGUMENT_SOURCEFILEORDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "targetfileordir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_MV_ARGUMENT_TARGETFILEORDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new MvCommand()),

            new CommandInfo("put", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_PUT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_PUT_ARGUMENT_FILE_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "output", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PutCommand()),

            new CommandInfo("putfolder", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_PUTFOLDER_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(false, "outputfolder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PutFolderCommand()),

            new CommandInfo("pwdl", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDL_DESC", "Nitrocid.ShellPacks"), new PwdlCommand()),

            new CommandInfo("pwdr", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDR_DESC", "Nitrocid.ShellPacks"), new PwdrCommand()),

            new CommandInfo("perm", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_PERM_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "permnumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_PERM_ARGUMENT_PERMNUMBER_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PermCommand()),

            new CommandInfo("sumfile", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_SUMFILE_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_ALGORITHM_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new SumFileCommand()),

            new CommandInfo("sumfiles", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_SUMFILES_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_ARGUMENT_ALGORITHM_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new SumFilesCommand()),

            new CommandInfo("type", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_TYPE_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "a/b", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_TYPE_ARGUMENT_TYPE_DESC", "Nitrocid.ShellPacks")
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
