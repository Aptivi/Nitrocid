
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
using KS.Shell.Shells.Admin.Commands;
using System;
using KS.Shell.ShellBase.Switches;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.Shells.Admin.Presets;
using KS.Arguments;
using System.Linq;

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
            { "arghelp",
                new CommandInfo("arghelp", ShellType, /* Localizable */ "Kernel arguments help system",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "argument", () => ArgumentParse.AvailableCMDLineArgs.Keys.ToArray())
                        })
                    }, new ArgHelpCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },
            
            { "bootlog",
                new CommandInfo("bootlog", ShellType, /* Localizable */ "Prints the boot log",
                    new[] {
                        new CommandArgumentInfo()
                    }, new BootLogCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },
            
            { "cdbglog",
                new CommandInfo("cdbglog", ShellType, /* Localizable */ "Deletes everything in debug log",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Admin_CdbgLogCommand())
            },

            { "clearfiredevents",
                new CommandInfo("clearfiredevents", ShellType, /* Localizable */ "Clears all fired events",
                    new[] {
                        new CommandArgumentInfo()
                    }, new Admin_ClearFiredEventsCommand())
            },

            { "journal",
                new CommandInfo("journal", ShellType, /* Localizable */ "Gets current kernel journal log",
                    new[] {
                        new CommandArgumentInfo()
                    }, new JournalCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsevents",
                new CommandInfo("lsevents", ShellType, /* Localizable */ "Lists all fired events",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsEventsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsusers",
                new CommandInfo("lsusers", ShellType, /* Localizable */ "Lists the users",
                    new[] {
                        new CommandArgumentInfo(true)
                    }, new LsUsersCommand())
            },

            { "userflag",
                new CommandInfo("userflag", ShellType, /* Localizable */ "Manipulates with the user main flags",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "user"),
                            new CommandArgumentPart(true, "admin/anonymous/disabled"),
                            new CommandArgumentPart(true, "false/true")
                        })
                    }, new UserFlagCommand())
            },

            { "userinfo",
                new CommandInfo("userinfo", ShellType, /* Localizable */ "Gets the user information",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "user")
                        })
                    }, new UserInfoCommand())
            },

            { "userlang",
                new CommandInfo("userlang", ShellType, /* Localizable */ "Changes the preferred user language",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "user"),
                            new CommandArgumentPart(true, "lang/clear")
                        })
                    }, new UserLangCommand())
            },
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

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
