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
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Misc.Reflection;
using Nitrocid.Shell.Prompts;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.Shells.Debug.Commands;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Shell.Shells.Debug.Presets;
using System.Linq;
using Nitrocid.Languages;

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
            new CommandInfo("currentbt", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_CURRENTBT_DESC", new CurrentBtCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("debuglog", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_DEBUGLOG_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sessionGuid", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_DEBUGLOG_ARGUMENT_SESSIONGUID_DESC"
                        })
                    ])
                ], new DebugLogCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("excinfo", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_EXCINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "excNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_EXCINFO_ARGUMENT_EXCNUMBER_DESC"
                        })
                    ])
                ], new ExcInfoCommand()),

            new CommandInfo("getfieldvalue", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_GETFIELDVALUE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "field", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. FieldManager.GetAllFieldsNoEvaluation().Keys],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_GETFIELDVALUE_ARGUMENT_NAME_DESC"
                        })
                    ], true)
                ], new GetFieldValueCommand()),

            new CommandInfo("getpropertyvalue", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_GETPROPERTYVALUE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "property", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. PropertyManager.GetAllPropertiesNoEvaluation().Keys],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_GETPROPERTYVALUE_ARGUMENT_NAME_DESC"
                        })
                    ], true)
                ], new GetPropertyValueCommand()),

            new CommandInfo("keyinfo", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_KEYINFO_DESC", new KeyInfoCommand()),

            new CommandInfo("lsaddons", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONS_DESC", new LsAddonsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonfields", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_NAME_DESC"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_TYPE_DESC"
                        }),
                    ])
                ], new LsAddonFieldsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonfuncs", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFUNCS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_NAME_DESC"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_TYPE_DESC"
                        }),
                    ])
                ], new LsAddonFuncsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonfuncparams", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFUNCPARAMS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_NAME_DESC"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_TYPE_DESC"
                        }),
                        new CommandArgumentPart(true, "function", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableFunctions(arg[0], arg[1]).Keys.ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFUNCPARAMS_ARGUMENT_FUNCTION_DESC"
                        }),
                    ])
                ], new LsAddonFuncParamsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddonprops", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONPROPERTIES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_NAME_DESC"
                        }),
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => InterAddonTools.ListAvailableTypes(arg[0]).Select((type) => type.FullName ?? "").ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_TYPE_DESC"
                        }),
                    ])
                ], new LsAddonPropsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsaddontypes", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONTYPES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "addon", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => AddonTools.GetAddons(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSADDONFIELDS_ARGUMENT_NAME_DESC"
                        }),
                    ])
                ], new LsAddonTypesCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsbaseaddons", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSBASEADDONS_DESC", new LsBaseAddonsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsfields", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSFIELDS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("suppress", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_SWITCH_SUPPRESS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsFieldsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsproperties", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSPROPERTIES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("suppress", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_SWITCH_SUPPRESS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsPropertiesCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsshells", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_LSSHELLS_DESC", new LsShellsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("previewsplash", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_PREVIEWSPLASH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "splashName", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_PREVIEWSPLASH_ARGUMENT_SPLASHNAME_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("splashout", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_PREVIEWSPLASH_SWITCH_SPLASHOUT_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("context", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_PREVIEWSPLASH_SWITCH_CONTEXT_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                    ])
                ], new PreviewSplashCommand()),

            new CommandInfo("showmainbuffer", /* Localizable */ "NKS_SHELL_SHELLS_DEBUG_COMMAND_SHOWMAINBUFFER_DESC", new ShowMainBufferCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),
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
