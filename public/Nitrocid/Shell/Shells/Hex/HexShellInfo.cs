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
using KS.Shell.Shells.Hex.Commands;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.Shells.Hex.Presets;

namespace KS.Shell.Shells.Hex
{
    /// <summary>
    /// Common hex shell class
    /// </summary>
    internal class HexShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Hex commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "addbyte",
                new CommandInfo("addbyte", ShellType, /* Localizable */ "Adds a new byte at the end of the file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "byte")
                        })
                    }, new AddByteCommand())
            },
            
            { "addbytes",
                new CommandInfo("addbytes", ShellType, /* Localizable */ "Adds the new bytes at the end of the file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new AddBytesCommand())
            },
            
            { "clear",
                new CommandInfo("clear", ShellType, /* Localizable */ "Clears the binary file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ClearCommand())
            },
            
            { "delbyte",
                new CommandInfo("delbyte", ShellType, /* Localizable */ "Deletes a byte using the byte number",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "bytenumber", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    }, new DelByteCommand())
            },
            
            { "delbytes",
                new CommandInfo("delbytes", ShellType, /* Localizable */ "Deletes the range of bytes",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "startbyte", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    }, new DelBytesCommand())
            },
            
            { "exitnosave",
                new CommandInfo("exitnosave", ShellType, /* Localizable */ "Exits the hex editor",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ExitNoSaveCommand())
            },
            
            { "print",
                new CommandInfo("print", ShellType, /* Localizable */ "Prints the contents of the file with byte numbers to the console",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    }, new PrintCommand(), CommandFlags.Wrappable)
            },
            
            { "querybyte",
                new CommandInfo("querybyte", ShellType, /* Localizable */ "Queries a byte in a specified range of bytes or all bytes",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "byte"),
                            new CommandArgumentPart(false, "startbyte", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(false, "endbyte", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        })
                    }, new QueryByteCommand(), CommandFlags.Wrappable)
            },
            
            { "replace",
                new CommandInfo("replace", ShellType, /* Localizable */ "Replaces a byte with another one",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "byte"),
                            new CommandArgumentPart(true, "replacebyte")
                        })
                    }, new ReplaceCommand())
            },
            
            { "save",
                new CommandInfo("save", ShellType, /* Localizable */ "Saves the file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SaveCommand())
            },
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new HexDefaultPreset() },
            { "PowerLine1", new HexPowerLine1Preset() },
            { "PowerLine2", new HexPowerLine2Preset() },
            { "PowerLine3", new HexPowerLine3Preset() },
            { "PowerLineBG1", new HexPowerLineBG1Preset() },
            { "PowerLineBG2", new HexPowerLineBG2Preset() },
            { "PowerLineBG3", new HexPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new HexShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
