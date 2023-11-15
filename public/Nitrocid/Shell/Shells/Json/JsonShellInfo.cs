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
using KS.Shell.Shells.Json.Commands;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Switches;
using KS.Shell.Shells.Json.Presets;

namespace KS.Shell.Shells.Json
{
    /// <summary>
    /// Common JSON shell class
    /// </summary>
    internal class JsonShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// JSON commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "add",
                new CommandInfo("add", /* Localizable */ "Adds a new array, object, or property",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "jsonValue")
                        ], [
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
                    ], new AddCommand())
            },

            { "addarray",
                new CommandInfo("addarray", /* Localizable */ "Adds a new property containing the array",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "propName"),
                            new CommandArgumentPart(true, "propValue1"),
                            new CommandArgumentPart(false, "propValue2"),
                            new CommandArgumentPart(false, "propValue3...")
                        ], [
                            new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new AddArrayCommand(), CommandFlags.Obsolete)
            },

            { "addproperty",
                new CommandInfo("addproperty", /* Localizable */ "Adds a new property at the end of the JSON file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "propName"),
                            new CommandArgumentPart(true, "propValue")
                        ], [
                            new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new AddPropertyCommand(), CommandFlags.Obsolete)
            },
            
            { "addobject",
                new CommandInfo("addobject", /* Localizable */ "Adds a new object inside the array",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "arrayName"),
                            new CommandArgumentPart(true, "valueInArray")
                        ], [
                            new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new AddObjectCommand(), CommandFlags.Obsolete)
            },
            
            { "addobjectindexed",
                new CommandInfo("addobjectindexed", /* Localizable */ "Adds a new object inside an object specified by index",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "index", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(true, "valueInArray")
                        ], [
                            new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new AddObjectIndexedCommand(), CommandFlags.Obsolete)
            },
            
            { "clear",
                new CommandInfo("clear", /* Localizable */ "Clears the JSON file",
                    [
                        new CommandArgumentInfo()
                    ], new ClearCommand())
            },
            
            { "delproperty",
                new CommandInfo("delproperty", /* Localizable */ "Removes a property from the JSON file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "propertyName")
                        })
                    ], new DelPropertyCommand(), CommandFlags.Obsolete)
            },
            
            { "exitnosave",
                new CommandInfo("exitnosave", /* Localizable */ "Exits the JSON shell without saving the changes",
                    [
                        new CommandArgumentInfo()
                    ], new ExitNoSaveCommand())
            },

            { "findproperty",
                new CommandInfo("findproperty", /* Localizable */ "Finds a property",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "propertyName")
                        ], [
                            new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new FindPropertyCommand())
            },

            { "jsoninfo",
                new CommandInfo("jsoninfo", /* Localizable */ "Shows information about the JSON file",
                    [
                        new CommandArgumentInfo(new[] {
                            new SwitchInfo("simplified", /* Localizable */ "Don't show individual properties", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("showvals", /* Localizable */ "Show all values", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    ], new JsonInfoCommand(), CommandFlags.Wrappable)
            },
            
            { "print",
                new CommandInfo("print", /* Localizable */ "Prints the JSON file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "propertyName")
                        })
                    ], new PrintCommand(), CommandFlags.Wrappable)
            },
            
            { "rm",
                new CommandInfo("rm", /* Localizable */ "Removes a target object",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "objectPath")
                        })
                    ], new RmCommand())
            },
            
            { "rmobject",
                new CommandInfo("rmobject", /* Localizable */ "Removes an object",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "objectName")
                        ], [
                            new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new RmObjectCommand(), CommandFlags.Obsolete)
            },
            
            { "rmobjectindexed",
                new CommandInfo("rmobjectindexed", /* Localizable */ "Removes an object specified by index",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "index", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        ], [
                            new SwitchInfo("parentProperty", /* Localizable */ "Specifies the parent property", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new RmObjectIndexedCommand(), CommandFlags.Obsolete)
            },
            
            { "save",
                new CommandInfo("save", /* Localizable */ "Saves the JSON file",
                    [
                        new CommandArgumentInfo(new[] {
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
                        })
                    ], new SaveCommand())
            },

            { "set",
                new CommandInfo("set", /* Localizable */ "Sets a value to an existing array, object, or property",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "jsonValue")
                        ], [
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
                    ], new SetCommand())
            },

            { "tui",
                new CommandInfo("tui", /* Localizable */ "Opens the JSON file in the interactive text editor TUI",
                    [
                        new CommandArgumentInfo()
                    ], new TuiCommand())
            },
        };

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

        public override BaseShell ShellBase => new JsonShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
