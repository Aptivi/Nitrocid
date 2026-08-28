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
using Terminaux.Shell.Arguments;
using Nitrocid.Base.Shell.Shells.Admin.Commands;
using Nitrocid.Base.Shell.Shells.Admin.Presets;
using Nitrocid.Base.Arguments;

namespace Nitrocid.Base.Shell.Shells.Admin
{
    /// <summary>
    /// Common admin shell class
    /// </summary>
    internal class AdminShellInfo : BaseShellInfo<AdminShell>, IShellInfo
    {
        /// <summary>
        /// Admin commands
        /// </summary>
        public override BaseCommand[] Commands =>
        [
            new ArgHelpCommand(),
            new BootLogCommand(),
            new CdbgLogCommand(),
            new ClearFiredEventsCommand(),
            new JournalCommand(),
            new LsEventsCommand(),
            new LsUsersCommand(),
            new SaveNotifsCommand(),
            new UserFlagCommand(),
            new UserFullNameCommand(),
            new UserInfoCommand(),
            new UserLangCommand(),
            new UserCultureCommand(),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new AdminDefaultPreset() },
            { "PowerLine1", new AdminPowerLine1Preset() },
            { "PowerLine2", new AdminPowerLine2Preset() },
            { "PowerLine3", new AdminPowerLine3Preset() },
            { "PowerLineBG1", new AdminPowerLineBG1Preset() },
            { "PowerLineBG2", new AdminPowerLineBG2Preset() },
            { "PowerLineBG3", new AdminPowerLineBG3Preset() }
        };
    }
}
