//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Debug.Commands;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.Shells.Debug.Presets;
using KS.Shell.ShellBase.Switches;
using KS.Misc.Reflection;
using System.Linq;
using KS.Kernel.Extensions;

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
                new CommandInfo("currentbt", /* Localizable */ "Gets current backtrace",
                    [
                        new CommandArgumentInfo()
                    ], new CurrentBtCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "debuglog",
                new CommandInfo("debuglog", /* Localizable */ "Easily fetches the debug log information using the session number",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "sessionNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    ], new DebugLogCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "excinfo",
                new CommandInfo("excinfo", /* Localizable */ "Gets message from kernel exception type. Useful for debugging",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "excNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    ], new ExcInfoCommand())
            },

            { "getfieldvalue",
                new CommandInfo("getfieldvalue", /* Localizable */ "Gets a field value",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "field", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => FieldManager.GetAllFieldsNoEvaluation().Keys.ToArray(),
                            })
                        }, true)
                    ], new GetFieldValueCommand())
            },

            { "getpropertyvalue",
                new CommandInfo("getpropertyvalue", /* Localizable */ "Gets a property value",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "property", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => PropertyManager.GetAllPropertiesNoEvaluation().Keys.ToArray(),
                            })
                        }, true)
                    ], new GetPropertyValueCommand())
            },

            { "keyinfo",
                new CommandInfo("keyinfo", /* Localizable */ "Gets key information for a pressed key. Useful for debugging",
                    [
                        new CommandArgumentInfo()
                    ], new KeyInfoCommand())
            },

            { "lsaddons",
                new CommandInfo("lsaddons", /* Localizable */ "Lists all available addons",
                    [
                        new CommandArgumentInfo()
                    ], new LsAddonsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsaddonfields",
                new CommandInfo("lsaddonfields", /* Localizable */ "Lists all available fields from the specified addon",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => AddonTools.GetAddons(),
                            })
                        })
                    ], new LsAddonFieldsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsaddonfuncs",
                new CommandInfo("lsaddonfuncs", /* Localizable */ "Lists all available functions from the specified addon",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => AddonTools.GetAddons(),
                            })
                        })
                    ], new LsAddonFuncsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsaddonprops",
                new CommandInfo("lsaddonprops", /* Localizable */ "Lists all available properties from the specified addon",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => AddonTools.GetAddons(),
                            })
                        })
                    ], new LsAddonPropsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsfields",
                new CommandInfo("lsfields", /* Localizable */ "Lists all available public fields",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("suppress", /* Localizable */ "Suppresses the error messages", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    ], new LsFieldsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsproperties",
                new CommandInfo("lsproperties", /* Localizable */ "Lists all available public properties",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("suppress", /* Localizable */ "Suppresses the error messages", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    ], new LsPropertiesCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "lsshells",
                new CommandInfo("lsshells", /* Localizable */ "Lists all available shells",
                    [
                        new CommandArgumentInfo()
                    ], new LsShellsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "previewsplash",
                new CommandInfo("previewsplash", /* Localizable */ "Previews the splash",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "splashName"),
                        ],
                        [
                            new SwitchInfo("splashout", /* Localizable */ "Specifies whether to test out the important messages feature on splash", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("context", /* Localizable */ "Specifies the splash screen context", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            }),
                        ])
                    ], new PreviewSplashCommand())
            },

            { "showmainbuffer",
                new CommandInfo("showmainbuffer", /* Localizable */ "Shows the main buffer that was on the screen before starting Nitrocid KS (Unix systems only)",
                    [
                        new CommandArgumentInfo()
                    ], new ShowMainBufferCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
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
