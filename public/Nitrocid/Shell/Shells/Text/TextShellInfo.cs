//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Terminaux.Shell.Prompts;
using Nitrocid.Shell.Shells.Text.Commands;
using Nitrocid.Shell.Shells.Text.Presets;

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
        public override BaseCommand[] Commands =>
        [
            new AddLineCommand(),
            new AddLinesCommand(),
            new ClearCommand(),
            new DelCharNumCommand(),
            new DelLineCommand(),
            new DelWordCommand(),
            new EditLineCommand(),
            new ExitNoSaveCommand(),
            new PrintCommand(),
            new QueryCharCommand(),
            new QueryWordCommand(),
            new QueryWordRegexCommand(),
            new ReplaceCommand(),
            new ReplaceInlineCommand(),
            new ReplaceRegexCommand(),
            new ReplaceInlineRegexCommand(),
            new SaveCommand(),
            new TuiCommand(),
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
