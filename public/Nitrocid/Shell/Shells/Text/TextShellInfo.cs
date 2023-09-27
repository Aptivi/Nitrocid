
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
using KS.Shell.Shells.Text.Commands;
using System;
using KS.Shell.ShellBase.Switches;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.Shells.Text.Presets;

namespace KS.Shell.Shells.Text
{
    /// <summary>
    /// Common text shell class
    /// </summary>
    internal class TextShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// Text commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "addline",
                new CommandInfo("addline", ShellType, /* Localizable */ "Adds a new line with text at the end of the file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "text")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_AddLineCommand())
            },
            
            { "addlines",
                new CommandInfo("addlines", ShellType, /* Localizable */ "Adds the new lines at the end of the file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TextEdit_AddLinesCommand())
            },
            
            { "clear",
                new CommandInfo("clear", ShellType, /* Localizable */ "Clears the text file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TextEdit_ClearCommand())
            },
            
            { "delcharnum",
                new CommandInfo("delcharnum", ShellType, /* Localizable */ "Deletes a character from character number in specified line",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "charNum"),
                            new CommandArgumentPart(true, "lineNum")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_DelCharNumCommand())
            },
            
            { "delline",
                new CommandInfo("delline", ShellType, /* Localizable */ "Removes the specified line number",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "lineNum"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_DelLineCommand())
            },
            
            { "delword",
                new CommandInfo("delword", ShellType, /* Localizable */ "Deletes a word or phrase from line number",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "word/phrase"),
                            new CommandArgumentPart(true, "lineNum"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_DelWordCommand())
            },
            
            { "editline",
                new CommandInfo("editline", ShellType, /* Localizable */ "Edits the specified line",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "linenumber")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_EditLineCommand())
            },
            
            { "exitnosave",
                new CommandInfo("exitnosave", ShellType, /* Localizable */ "Exits the text editor",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TextEdit_ExitNoSaveCommand())
            },
            
            { "print",
                new CommandInfo("print", ShellType, /* Localizable */ "Prints the contents of the file with line numbers to the console",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "lineNum"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_PrintCommand(), CommandFlags.Wrappable)
            },
            
            { "querychar",
                new CommandInfo("querychar", ShellType, /* Localizable */ "Queries a character in a specified line or all lines",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "char"),
                            new CommandArgumentPart(true, "lineNum/all"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_QueryCharCommand(), CommandFlags.Wrappable)
            },
            
            { "queryword",
                new CommandInfo("queryword", ShellType, /* Localizable */ "Queries a word in a specified line or all lines",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "word/phrase"),
                            new CommandArgumentPart(true, "lineNum/all"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_QueryWordCommand(), CommandFlags.Wrappable)
            },
            
            { "querywordregex",
                new CommandInfo("querywordregex", ShellType, /* Localizable */ "Queries a word in a specified line or all lines using regular expressions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "regex"),
                            new CommandArgumentPart(true, "lineNum/all"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_QueryWordRegexCommand(), CommandFlags.Wrappable)
            },
            
            { "replace",
                new CommandInfo("replace", ShellType, /* Localizable */ "Replaces a word or phrase with another one",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "word/phrase"),
                            new CommandArgumentPart(true, "word/phrase")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_ReplaceCommand())
            },
            
            { "replaceinline",
                new CommandInfo("replaceinline", ShellType, /* Localizable */ "Replaces a word or phrase with another one in a line",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "word/phrase"),
                            new CommandArgumentPart(true, "word/phrase"),
                            new CommandArgumentPart(true, "lineNum/all"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_ReplaceInlineCommand())
            },
            
            { "replaceregex",
                new CommandInfo("replaceregex", ShellType, /* Localizable */ "Replaces a word or phrase with another one using regular expressions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "regex"),
                            new CommandArgumentPart(true, "word/phrase")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_ReplaceRegexCommand())
            },
            
            { "replaceinlineregex",
                new CommandInfo("replaceinlineregex", ShellType, /* Localizable */ "Replaces a word or phrase with another one in a line using regular expressions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "regex"),
                            new CommandArgumentPart(true, "word/phrase"),
                            new CommandArgumentPart(true, "lineNum/all"),
                            new CommandArgumentPart(false, "lineNum2")
                        }, Array.Empty<SwitchInfo>())
                    }, new TextEdit_ReplaceInlineRegexCommand())
            },
            
            { "save",
                new CommandInfo("save", ShellType, /* Localizable */ "Saves the file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TextEdit_SaveCommand())
            }
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new TextDefaultPreset() },
            { "PowerLine1", new TextPowerLine1Preset() },
            { "PowerLine2", new TextPowerLine2Preset() },
            { "PowerLine3", new TextPowerLine3Preset() },
            { "PowerLineBG1", new TextPowerLineBG1Preset() },
            { "PowerLineBG2", new TextPowerLineBG2Preset() },
            { "PowerLineBG3", new TextPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new TextShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["TextShell"];

    }
}
