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
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.ShellPacks.Shells.Json.Presets;
using Nitrocid.ShellPacks.Shells.Json.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Languages;

namespace Nitrocid.ShellPacks.Shells.Json
{
    /// <summary>
    /// Common JSON shell class
    /// </summary>
    internal class JsonShellInfo : BaseShellInfo<JsonShell>, IShellInfo
    {
        /// <summary>
        /// JSON commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("add", "NKS_SHELLPACKS_JSON_COMMAND_ADD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonValue", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_JSON_COMMAND_ADD_ARGUMENT_JSONVALUE_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("parentPath", "NKS_SHELLPACKS_JSON_COMMAND_ADD_SWITCH_PARENTPATH_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                        new SwitchInfo("type", "NKS_SHELLPACKS_JSON_COMMAND_ADD_SWITCH_TYPE_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsRequired = true
                        }),
                        new SwitchInfo("propName", "NKS_SHELLPACKS_JSON_COMMAND_SWITCH_PROPNAME_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                    ])
                ], new AddCommand()),

            new CommandInfo("clear", "NKS_SHELLPACKS_JSON_COMMAND_CLEAR_DESC", new ClearCommand()),

            new CommandInfo("exitnosave", "NKS_SHELLPACKS_JSON_COMMAND_EXITNOSAVE_DESC", new ExitNoSaveCommand()),

            new CommandInfo("findproperty", "NKS_SHELLPACKS_JSON_COMMAND_FINDPROPERTY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "propertyName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_JSON_COMMAND_ARGUMENT_PROPERTYNAME_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("parentProperty", "NKS_SHELLPACKS_JSON_COMMAND_FINDPROPERTY_SWITCH_PARENTPROPERTY_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ])
                ], new FindPropertyCommand()),

            new CommandInfo("jsoninfo", "NKS_SHELLPACKS_JSON_COMMAND_JSONINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("simplified", "NKS_SHELLPACKS_JSON_COMMAND_JSONINFO_SWITCH_SIMPLIFIED_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("showvals", "NKS_SHELLPACKS_JSON_COMMAND_JSONINFO_SWITCH_SHOWVALS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new JsonInfoCommand(), CommandFlags.Wrappable),

            new CommandInfo("print", "NKS_SHELLPACKS_JSON_COMMAND_PRINT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "propertyName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_JSON_COMMAND_ARGUMENT_PROPERTYNAME_DESC"
                        })
                    ])
                ], new PrintCommand(), CommandFlags.Wrappable),

            new CommandInfo("rm", "NKS_SHELLPACKS_JSON_COMMAND_RM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "objectPath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_JSON_COMMAND_RM_ARGUMENT_OBJECTPATH_DESC"
                        })
                    ])
                ], new RmCommand()),

            new CommandInfo("save", "NKS_SHELLPACKS_JSON_COMMAND_SAVE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("b", "NKS_SHELLPACKS_JSON_COMMAND_SAVE_SWITCH_B_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", "NKS_SHELLPACKS_JSON_COMMAND_SAVE_SWITCH_M_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["b"],
                            AcceptsValues = false
                        })
                    ])
                ], new SaveCommand()),

            new CommandInfo("set", "NKS_SHELLPACKS_JSON_COMMAND_SET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonValue", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_JSON_COMMAND_SET_ARGUMENT_JSONVALUE_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("parentPath", "NKS_SHELLPACKS_JSON_COMMAND_ADD_SWITCH_PARENTPATH_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                        new SwitchInfo("type", "NKS_SHELLPACKS_JSON_COMMAND_ADD_SWITCH_TYPE_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsRequired = true
                        }),
                        new SwitchInfo("propName", "NKS_SHELLPACKS_JSON_COMMAND_SWITCH_PROPNAME_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                    ])
                ], new SetCommand()),

            new CommandInfo("tui", "NKS_SHELLPACKS_JSON_COMMAND_TUI_DESC", new TuiCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new JsonDefaultPreset() },
            { "PowerLine1", new JsonPowerLine1Preset() },
            { "PowerLine2", new JsonPowerLine2Preset() },
            { "PowerLine3", new JsonPowerLine3Preset() },
            { "PowerLineBG1", new JsonPowerLineBG1Preset() },
            { "PowerLineBG2", new JsonPowerLineBG2Preset() },
            { "PowerLineBG3", new JsonPowerLineBG3Preset() }
        };
    }
}
