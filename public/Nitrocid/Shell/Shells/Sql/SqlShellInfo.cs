﻿
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
using KS.Shell.Shells.Sql.Commands;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.Shells.Sql.Presets;

namespace KS.Shell.Shells.Sql
{
    /// <summary>
    /// Common Sql shell class
    /// </summary>
    internal class SqlShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Sql commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cmd",
                new CommandInfo("cmd", ShellType, /* Localizable */ "Executes an SQL query",
                    new[] {
                        new CommandArgumentInfo()
                    }, new CmdCommand())
            },

            { "dbinfo",
                new CommandInfo("dbinfo", ShellType, /* Localizable */ "Database info",
                    new[] {
                        new CommandArgumentInfo()
                    }, new DbInfoCommand())
            },
        };

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

        public override BaseShell ShellBase => new SqlShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}