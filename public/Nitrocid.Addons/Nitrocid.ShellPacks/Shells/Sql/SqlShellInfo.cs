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
using Nitrocid.ShellPacks.Shells.Sql.Presets;
using Nitrocid.ShellPacks.Shells.Sql.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Languages;

namespace Nitrocid.ShellPacks.Shells.Sql
{
    /// <summary>
    /// Common Sql shell class
    /// </summary>
    internal class SqlShellInfo : BaseShellInfo<SqlShell>, IShellInfo
    {
        /// <summary>
        /// Sql commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("cmd", /* Localizable */ "NKS_SHELLPACKS_SQL_COMMAND_CMD_DESC", new CmdCommand()),

            new CommandInfo("dbinfo", /* Localizable */ "NKS_SHELLPACKS_SQL_COMMAND_DBINFO_DESC", new DbInfoCommand()),

            new CommandInfo("tui", /* Localizable */ "NKS_SHELLPACKS_SQL_COMMAND_TUI_DESC", new TuiCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new SqlDefaultPreset() },
            { "PowerLine1", new SqlPowerLine1Preset() },
            { "PowerLine2", new SqlPowerLine2Preset() },
            { "PowerLine3", new SqlPowerLine3Preset() },
            { "PowerLineBG1", new SqlPowerLineBG1Preset() },
            { "PowerLineBG2", new SqlPowerLineBG2Preset() },
            { "PowerLineBG3", new SqlPowerLineBG3Preset() }
        };
    }
}
