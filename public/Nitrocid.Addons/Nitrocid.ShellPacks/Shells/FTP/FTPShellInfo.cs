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
using Nitrocid.ShellPacks.Shells.FTP.Presets;
using Nitrocid.ShellPacks.Shells.FTP.Commands;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;

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
        public override BaseCommand[] Commands =>
        [
            new CatCommand(),
            new CdlCommand(),
            new CdrCommand(),
            new CpCommand(),
            new DelCommand(),
            new DetachCommand(),
            new ExecuteCommand(),
            new GetCommand(),
            new GetFolderCommand(),
            new IfmCommand(),
            new InfoCommand(),
            new LslCommand(),
            new LsrCommand(),
            new MkldirCommand(),
            new MkrdirCommand(),
            new MvCommand(),
            new PutCommand(),
            new PutFolderCommand(),
            new PwdlCommand(),
            new PwdrCommand(),
            new PermCommand(),
            new SumFileCommand(),
            new SumFilesCommand(),
            new TypeCommand(),
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

        public override string NetworkConnectionType => nameof(Base.Network.Connections.NetworkConnectionType.FTP);

    }
}
