
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
using KS.Shell.Shells.Debug.Commands;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Switches;
using System;
using KS.Shell.Shells.Debug.Presets;

namespace KS.Shell.Shells.Debug
{
    /// <summary>
    /// Common debug shell class
    /// </summary>
    internal class DebugShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Debug commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "currentbt",
                new CommandInfo("currentbt", ShellType, /* Localizable */ "Gets current backtrace",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Debug_CurrentBtCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "debuglog",
                new CommandInfo("debuglog", ShellType, /* Localizable */ "Easily fetches the debug log information using the session number",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "sessionNum", null, true)
                        })
                    }, new Debug_DebugLogCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "excinfo",
                new CommandInfo("excinfo", ShellType, /* Localizable */ "Gets message from kernel exception type. Useful for debugging",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "excNum", null, true)
                        })
                    }, new Debug_ExcInfoCommand())
            },

            { "keyinfo",
                new CommandInfo("keyinfo", ShellType, /* Localizable */ "Gets key information for a pressed key. Useful for debugging",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Debug_KeyInfoCommand())
            },

            { "lsaddons",
                new CommandInfo("lsaddons", ShellType, /* Localizable */ "Lists all available addons",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Debug_LsAddonsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsshells",
                new CommandInfo("lsshells", ShellType, /* Localizable */ "Lists all available shells",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Debug_LsShellsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "threadsbt",
                new CommandInfo("threadsbt", ShellType, /* Localizable */ "Gets backtrace for all threads",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Debug_ThreadsBtCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new DebugDefaultPreset() },
            { "PowerLine1", new DebugPowerLine1Preset() },
            { "PowerLine2", new DebugPowerLine2Preset() },
            { "PowerLine3", new DebugPowerLine3Preset() },
            { "PowerLineBG1", new DebugPowerLineBG1Preset() },
            { "PowerLineBG2", new DebugPowerLineBG2Preset() },
            { "PowerLineBG3", new DebugPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new DebugShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
