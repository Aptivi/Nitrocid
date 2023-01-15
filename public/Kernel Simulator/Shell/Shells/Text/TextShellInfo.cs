
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using KS.Shell.Prompts.Presets.Text;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Text.Commands;

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
            { "addline", new CommandInfo("addline", ShellType, "Adds a new line with text at the end of the file", new CommandArgumentInfo(new[] { "<text>" }, true, 1), new TextEdit_AddLineCommand()) },
            { "addlines", new CommandInfo("addlines", ShellType, "Adds the new lines at the end of the file", new CommandArgumentInfo(), new TextEdit_AddLinesCommand()) },
            { "clear", new CommandInfo("clear", ShellType, "Clears the text file", new CommandArgumentInfo(), new TextEdit_ClearCommand()) },
            { "delcharnum", new CommandInfo("delcharnum", ShellType, "Deletes a character from character number in specified line", new CommandArgumentInfo(new[] { "<charnumber> <linenumber>" }, true, 2), new TextEdit_DelCharNumCommand()) },
            { "delline", new CommandInfo("delline", ShellType, "Removes the specified line number", new CommandArgumentInfo(new[] { "<linenumber> [linenumber2]" }, true, 1), new TextEdit_DelLineCommand()) },
            { "delword", new CommandInfo("delword", ShellType, "Deletes a word or phrase from line number", new CommandArgumentInfo(new[] { "\"<word/phrase>\" <linenumber> [linenumber2]" }, true, 2), new TextEdit_DelWordCommand()) },
            { "editline", new CommandInfo("editline", ShellType, "Edits the specified line", new CommandArgumentInfo(new[] { "<linenumber>" }, true, 1), new TextEdit_EditLineCommand()) },
            { "exitnosave", new CommandInfo("exitnosave", ShellType, "Exits the text editor", new CommandArgumentInfo(), new TextEdit_ExitNoSaveCommand()) },
            { "print", new CommandInfo("print", ShellType, "Prints the contents of the file with line numbers to the console", new CommandArgumentInfo(new[] { "[linenumber] [linenumber2]" }, false, 0), new TextEdit_PrintCommand()) },
            { "querychar", new CommandInfo("querychar", ShellType, "Queries a character in a specified line or all lines", new CommandArgumentInfo(new[] { "<char> <linenumber/all> [linenumber2]" }, true, 2), new TextEdit_QueryCharCommand()) },
            { "queryword", new CommandInfo("queryword", ShellType, "Queries a word in a specified line or all lines", new CommandArgumentInfo(new[] { "\"<word/phrase>\" <linenumber/all> [linenumber2]" }, true, 2), new TextEdit_QueryWordCommand()) },
            { "querywordregex", new CommandInfo("querywordregex", ShellType, "Queries a word in a specified line or all lines using regular expressions", new CommandArgumentInfo(new[] { "\"<regex>\" <linenumber/all> [linenumber2]" }, true, 2), new TextEdit_QueryWordRegexCommand()) },
            { "replace", new CommandInfo("replace", ShellType, "Replaces a word or phrase with another one", new CommandArgumentInfo(new[] { "\"<word/phrase>\" \"<word/phrase>\"" }, true, 2), new TextEdit_ReplaceCommand()) },
            { "replaceinline", new CommandInfo("replaceinline", ShellType, "Replaces a word or phrase with another one in a line", new CommandArgumentInfo(new[] { "\"<word/phrase>\" \"<word/phrase>\" <linenumber> [linenumber2]" }, true, 3), new TextEdit_ReplaceInlineCommand()) },
            { "replaceregex", new CommandInfo("replaceregex", ShellType, "Replaces a word or phrase with another one using regular expressions", new CommandArgumentInfo(new[] { "\"<regex>\" \"<word/phrase>\"" }, true, 2), new TextEdit_ReplaceRegexCommand()) },
            { "replaceinlineregex", new CommandInfo("replaceinlineregex", ShellType, "Replaces a word or phrase with another one in a line using regular expressions", new CommandArgumentInfo(new[] { "\"<regex>\" \"<word/phrase>\" <linenumber> [linenumber2]" }, true, 3), new TextEdit_ReplaceInlineRegexCommand()) },
            { "save", new CommandInfo("save", ShellType, "Saves the file", new CommandArgumentInfo(), new TextEdit_SaveCommand()) }
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
