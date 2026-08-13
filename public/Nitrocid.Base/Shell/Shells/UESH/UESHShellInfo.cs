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

using System.Collections.Generic;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Nitrocid.Base.Shell.Shells.UESH.Commands;
using Nitrocid.Base.Shell.Shells.UESH.Presets;

namespace Nitrocid.Base.Shell.Shells.UESH
{
    /// <summary>
    /// UESH common shell properties
    /// </summary>
    internal class UESHShellInfo : BaseShellInfo<UESHShell>, IShellInfo
    {
        /// <summary>
        /// List of commands
        /// </summary>
        public override BaseCommand[] Commands =>
        [
            new TwoFactorCommand(),
            new AddGroupCommand(),
            new AddUserCommand(),
            new AddUserToGroupCommand(),
            new AdminCommand(),
            new AlarmCommand(),
            new BeepCommand(),
            new BlockDbgDevCommand(),
            new BulkRenameCommand(),
            new CatCommand(),
            new CDirCommand(),
            new ChangesCommand(),
            new ChAttrCommand(),
            new ChCultureCommand(),
            new ChDirCommand(),
            new ChHostNameCommand(),
            new ChkLockCommand(),
            new ChLangCommand(),
            new ChMalCommand(),
            new ChMotdCommand(),
            new ChPwdCommand(),
            new ChUsrNameCommand(),
            new CombineStrCommand(),
            new CombineCommand(),
            new CompareCommand(),
            new ConvertLineEndingsCommand(),
            new CowsayCommand(),
            new CopyCommand(),
            new DateCommand(),
            new DebugShellCommand(),
            new DecodeFileCommand(),
            new DecodeTextCommand(),
            new DirInfoCommand(),
            new DisconnDbgDevCommand(),
            new DiskInfoCommand(),
            new DismissNotifCommand(),
            new DismissNotifsCommand(),
            new DockCommand(),
            new DriverManCommand(),
            new EditCommand(),
            new EncodeFileCommand(),
            new EncodeTextCommand(),
            new ExtIpCommand(),
            new ExtIp6Command(),
            new FigletCommand(),
            new FileInfoCommand(),
            new FindCommand(),
            new FindRegCommand(),
            new GetCommand(),

#if NKS_EXTENSIONS
            new GetAddonsCommand(),
#endif

            new GetAllExtHandlersCommand(),
            new GetConfigValueCommand(),
            new GetDefaultExtHandlerCommand(),
            new GetDefaultExtHandlersCommand(),
            new GetExtHandlersCommand(),
            new GetKeyIvCommand(),
            new GroupFileCommand(),
            new GroupManTuiCommand(),
            new HostCommand(),
            new HwInfoCommand(),
            new IfmCommand(),
            new IsModeCommand(),
            new LicenseCommand(),
            new ListCommand(),
            new LockScreenCommand(),
            new LogoutCommand(),
            new LsConfigsCommand(),
            new LsConfigValuesCommand(),
            new LsConnectionsCommand(),
            new LsDbgDevCommand(),
            new LsExtHandlersCommand(),
            new LsDiskPartsCommand(),
            new LsDisksCommand(),
            new LsNetCommand(),
            new MdCommand(),
            new MkFileCommand(),
            new ModelineCommand(),
            new MoveCommand(),
            new PartInfoCommand(),
            new PathFindCommand(),
            new PermCommand(),
            new PermGroupCommand(),
            new PingCommand(),
            new PlatformCommand(),
            new PutCommand(),
            new RdebugCommand(),
            new RebootCommand(),
            new ReloadConfigCommand(),
            new RexecCommand(),
            new RmCommand(),
            new RmSecCommand(),
            new RmUserCommand(),
            new RmGroupCommand(),
            new RmUserFromGroupCommand(),
            new RRebootCommand(),
            new RShutdownCommand(),
            new SaveConfigCommand(),
            new SaveScreenCommand(),
            new SearchCommand(),
            new SearchWordCommand(),
            new SetSaverCommand(),
            new SettingsCommand(),
            new SetConfigValueCommand(),
            new SetExtHandlerCommand(),
            new ShowNotifsCommand(),
            new ShowTdCommand(),
            new ShowTdZoneCommand(),
            new ShutdownCommand(),
            new SplitFileCommand(),
            new SudoCommand(),
            new SumFileCommand(),
            new SumFilesCommand(),
            new SumTextCommand(),
            new SymlinkCommand(),
            new SysInfoCommand(),
            new TaskManCommand(),
            new ThemePrevCommand(),
            new ThemeSetCommand(),
            new UnblockDbgDevCommand(),
            new UnixPermCommand(),
            new UnixPermCalcCommand(),
            new UnZipCommand(),

#if SPECIFIERREL
            new UpdateCommand(),
#endif

            new UptimeCommand(),
            new UserManTuiCommand(),
            new UserManualCommand(),
            new VerifyCommand(),
            new VersionCommand(),
            new WhoamiCommand(),
            new WinElevateCommand(),
            new WrapTextCommand(),
            new ZipCommand(),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new DefaultPreset() },
            { "PowerLine1", new PowerLine1Preset() },
            { "PowerLine2", new PowerLine2Preset() },
            { "PowerLine3", new PowerLine3Preset() },
            { "PowerLineBG1", new PowerLineBG1Preset() },
            { "PowerLineBG2", new PowerLineBG2Preset() },
            { "PowerLineBG3", new PowerLineBG3Preset() }
        };
    }
}
