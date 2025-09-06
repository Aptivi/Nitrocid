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
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.JsonShell.Json.Presets;
using Nitrocid.Extras.JsonShell.Json.Commands;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;

namespace Nitrocid.Extras.JsonShell.Json
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
            new CommandInfo("add", /* Localizable */ "Adds a new array, object, or property",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonValue", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "JSON value to add"
                        })
                    ],
                    [
                        new SwitchInfo("parentPath", /* Localizable */ "Specifies the parent path", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                        new SwitchInfo("type", /* Localizable */ "Specifies the type", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsRequired = true
                        }),
                        new SwitchInfo("propName", /* Localizable */ "Specifies the property name to be created with. This is used if the parent path is an object.", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                    ])
                ], new AddCommand()),

            new CommandInfo("clear", /* Localizable */ "Clears the JSON file", new ClearCommand()),

            new CommandInfo("exitnosave", /* Localizable */ "Exits the JSON shell without saving the changes", new ExitNoSaveCommand()),

            new CommandInfo("findproperty", /* Localizable */ "Finds a property",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "propertyName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "JSON property name"
                        })
                    ],
                    [
                        new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ])
                ], new FindPropertyCommand()),

            new CommandInfo("jsoninfo", /* Localizable */ "Shows information about the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("simplified", /* Localizable */ "Don't show individual properties", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("showvals", /* Localizable */ "Show all values", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new JsonInfoCommand(), CommandFlags.Wrappable),

            new CommandInfo("print", /* Localizable */ "Prints the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "propertyName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "JSON property name"
                        })
                    ])
                ], new PrintCommand(), CommandFlags.Wrappable),

            new CommandInfo("rm", /* Localizable */ "Removes a target object",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "objectPath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Object path"
                        })
                    ])
                ], new RmCommand()),

            new CommandInfo("save", /* Localizable */ "Saves the JSON file",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("b", /* Localizable */ "Beautified JSON", new SwitchOptions()
                        {
                            ConflictsWith = ["m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "Minified JSON", new SwitchOptions()
                        {
                            ConflictsWith = ["b"],
                            AcceptsValues = false
                        })
                    ])
                ], new SaveCommand()),

            new CommandInfo("set", /* Localizable */ "Sets a value to an existing array, object, or property",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "jsonValue", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "JSON value"
                        })
                    ],
                    [
                        new SwitchInfo("parentPath", /* Localizable */ "Specifies the parent path", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                        new SwitchInfo("type", /* Localizable */ "Specifies the type", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsRequired = true
                        }),
                        new SwitchInfo("propName", /* Localizable */ "Specifies the property name to be created with. This is used if the parent path is an object.", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        }),
                    ])
                ], new SetCommand()),

            new CommandInfo("tui", /* Localizable */ "Opens the JSON file in the interactive editor", new TuiCommand()),
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
