//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Misc.Reflection;
using Terminaux.Shell.Prompts;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.Shells.Debug.Commands;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Shell.Shells.Debug.Presets;
using System.Linq;

namespace Nitrocid.Shell.Shells.Debug
{
    /// <summary>
    /// Common debug shell class
    /// </summary>
    internal class DebugShellInfo : BaseShellInfo<DebugShell>, IShellInfo
    {

        /// <summary>
        /// Debug commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("currentbt", /* Localizable */ "Gets current backtrace", new CurrentBtCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("debuglog", /* Localizable */ "Easily fetches the debug log information using the session GUID",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sessionGuid", new()
                        {
                            ArgumentDescription = /* Localizable */ "Session GUID to find"
                        })
                    ])
                ], new DebugLogCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("excinfo", /* Localizable */ "Gets message from kernel exception type. Useful for debugging",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "excNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Exception number"
                        })
                    ])
                ], new ExcInfoCommand()),

            new CommandInfo("getfieldvalue", /* Localizable */ "Gets a field value",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "field", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. FieldManager.GetAllFieldsNoEvaluation().Keys],
                            ArgumentDescription = /* Localizable */ "Field name to query"
                        })
                    ], true)
                ], new GetFieldValueCommand()),

            new CommandInfo("getpropertyvalue", /* Localizable */ "Gets a property value",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "property", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. PropertyManager.GetAllPropertiesNoEvaluation().Keys],
                            ArgumentDescription = /* Localizable */ "Property name to query"
                        })
                    ], true)
                ], new GetPropertyValueCommand()),

            new CommandInfo("keyinfo", /* Localizable */ "Gets key information for a pressed key. Useful for debugging", new KeyInfoCommand()),

            new CommandInfo("lsaddons", /* Localizable */ "Lists all available addons", new LsAddonsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonfields", /* Localizable */ "Lists all available fields from the specified addon",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "Addon name to query"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "Public static type name from addon"
                        }),
                    ])
                ], new LsAddonFieldsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonfuncs", /* Localizable */ "Lists all available functions from the specified addon",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "Addon name to query"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "Public static type name from addon"
                        }),
                    ])
                ], new LsAddonFuncsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonfuncparams", /* Localizable */ "Lists all available function parameters from a function",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "Addon name to query"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "Public static type name from addon"
                        }),
                        new CommandArgumentPart(true, "function", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableFunctions(arg[0], arg[1]).Keys.ToArray(),
                            ArgumentDescription = /* Localizable */ "Function name from the public static type to query"
                        }),
                    ])
                ], new LsAddonFuncParamsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonprops", /* Localizable */ "Lists all available properties from the specified addon",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "Addon name to query"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "Public static type name from addon"
                        }),
                    ])
                ], new LsAddonPropsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddontypes", /* Localizable */ "Lists all available types from the specified addon",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "Addon name to query"
                        }),
                    ])
                ], new LsAddonTypesCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsbaseaddons", /* Localizable */ "Lists all the base addons and their status", new LsBaseAddonsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsfields", /* Localizable */ "Lists all available public fields",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("suppress", /* Localizable */ "Suppresses the error messages", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsFieldsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsproperties", /* Localizable */ "Lists all available public properties",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("suppress", /* Localizable */ "Suppresses the error messages", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsPropertiesCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsshells", /* Localizable */ "Lists all available shells", new LsShellsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("previewsplash", /* Localizable */ "Previews the splash",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "splashName", new()
                        {
                            ArgumentDescription = /* Localizable */ "Splash name to query"
                        }),
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
                ], new PreviewSplashCommand()),

            new CommandInfo("showmainbuffer", /* Localizable */ "Shows the main buffer that was on the screen before starting Nitrocid KS (Unix systems only)", new ShowMainBufferCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),
        ];

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
    }
}
