
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.Admin;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Admin.Commands;

namespace KS.Shell.Shells.Admin
{
    /// <summary>
    /// Common admin shell class
    /// </summary>
    internal class AdminShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Admin commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cdbglog", new CommandInfo("cdbglog", ShellType, /* Localizable */ "Deletes everything in debug log", new CommandArgumentInfo(), new Admin_CdbgLogCommand()) },
            { "journal", new CommandInfo("journal", ShellType, /* Localizable */ "Gets current kernel journal log", new CommandArgumentInfo(), new JournalCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported) },
            { "lsevents", new CommandInfo("lsevents", ShellType, /* Localizable */ "Lists all fired events", new CommandArgumentInfo(), new LsEventsCommand()) },
        };

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

        public override BaseShell ShellBase => new AdminShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["AdminShell"];

    }
}
