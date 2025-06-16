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
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.Shells.Text.Commands;
using Nitrocid.Shell.Shells.Text.Presets;
using Nitrocid.Languages;

namespace Nitrocid.Shell.Shells.Text
{
    /// <summary>
    /// Common text shell class
    /// </summary>
    internal class TextShellInfo : BaseShellInfo<TextShell>, IShellInfo
    {
        /// <summary>
        /// Text commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("addline", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_ADDLINE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_ADDLINE_ARGUMENT_TEXT_DESC"
                        })
                    ])
                ], new AddLineCommand()),

            new CommandInfo("addlines", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_ADDLINES_DESC", new AddLinesCommand()),

            new CommandInfo("clear", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_CLEAR_DESC", new ClearCommand()),

            new CommandInfo("delcharnum", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELCHARNUM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "charNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELCHARNUM_ARGUMENT_CHARNUM_DESC"
                        }),
                        new CommandArgumentPart(true, "lineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELCHARNUM_ARGUMENT_LINENUM_DESC"
                        })
                    ])
                ], new DelCharNumCommand()),

            new CommandInfo("delline", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "lineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new DelLineCommand()),

            new CommandInfo("delword", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELWORD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELWORD_ARGUMENT_WORD_DESC"
                        }),
                        new CommandArgumentPart(true, "lineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new DelWordCommand()),

            new CommandInfo("editline", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_EDITLINE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "linenumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELCHARNUM_ARGUMENT_LINENUM_DESC"
                        })
                    ])
                ], new EditLineCommand()),

            new CommandInfo("exitnosave", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_EXITNOSAVE_DESC", new ExitNoSaveCommand()),

            new CommandInfo("print", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_PRINT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "lineNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new PrintCommand(), CommandFlags.Wrappable),

            new CommandInfo("querychar", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYCHAR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "char", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYCHAR_ARGUMENT_CHAR_DESC"
                        }),
                        new CommandArgumentPart(true, "lineNum/all", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYCHAR_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new QueryCharCommand(), CommandFlags.Wrappable),

            new CommandInfo("queryword", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYWORD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYWORD_ARGUMENT_WORD_DESC"
                        }),
                        new CommandArgumentPart(true, "lineNum/all", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYCHAR_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new QueryWordCommand(), CommandFlags.Wrappable),

            new CommandInfo("querywordregex", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYWORDREGEX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "regex", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYWORDREGEX_ARGUMENT_REGEX_DESC"
                        }),
                        new CommandArgumentPart(true, "lineNum/all", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_QUERYCHAR_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new QueryWordRegexCommand(), CommandFlags.Wrappable),

            new CommandInfo("replace", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_SOURCE_DESC"
                        }),
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_TARGET_DESC"
                        })
                    ])
                ], new ReplaceCommand()),

            new CommandInfo("replaceinline", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEINLINE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_SOURCE_DESC"
                        }),
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_TARGET_DESC"
                        }),
                        new CommandArgumentPart(true, "lineNum/all", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEINLINE_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new ReplaceInlineCommand()),

            new CommandInfo("replaceregex", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEREGEX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "regex", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEREGEX_ARGUMENT_REGEX_DESC"
                        }),
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_TARGET_DESC"
                        })
                    ])
                ], new ReplaceRegexCommand()),

            new CommandInfo("replaceinlineregex", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEINLINEREGEX_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "regex", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEREGEX_ARGUMENT_REGEX_DESC"
                        }),
                        new CommandArgumentPart(true, "word/phrase", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACE_ARGUMENT_TARGET_DESC"
                        }),
                        new CommandArgumentPart(true, "lineNum/all", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_REPLACEINLINE_ARGUMENT_LINENUM_DESC"
                        }),
                        new CommandArgumentPart(false, "lineNum2", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_DELLINE_ARGUMENT_LINENUM2_DESC"
                        })
                    ])
                ], new ReplaceInlineRegexCommand()),

            new CommandInfo("save", /* Localizable */ "NKS_SHELL_SHELLS_HEXTEXT_COMMAND_SAVE_DESC", new SaveCommand()),

            new CommandInfo("tui", /* Localizable */ "NKS_SHELL_SHELLS_TEXT_COMMAND_TUI_DESC", new TuiCommand()),
        ];

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
    }
}
