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
using Nitrocid.ShellPacks.Shells.Mail.Commands;
using Nitrocid.ShellPacks.Shells.Mail.Presets;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Prompts;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// Common mail shell class
    /// </summary>
    internal class MailShellInfo : BaseShellInfo<MailShell>, IShellInfo
    {
        /// <summary>
        /// Mail commands
        /// </summary>
        public override List<BaseCommand> Commands =>
        [
            new CdCommand(),
            new DetachCommand(),
            new IspInfoCommand(),
            new LsDirsCommand(),
            new ListCommand(),
            new MkdirCommand(),
            new MvCommand(),
            new MvAllCommand(),
            new ReadCommand(),
            new RenCommand(),
            new RmCommand(),
            new RmAllCommand(),
            new RmdirCommand(),
            new SendCommand(),
            new SendEncCommand(),
            new TuiCommand(),
        ];

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

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => nameof(Base.Network.Connections.NetworkConnectionType.Mail);
    }
}
