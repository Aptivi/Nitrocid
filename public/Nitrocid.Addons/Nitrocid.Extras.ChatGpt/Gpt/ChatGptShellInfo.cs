//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.ChatGpt.Gpt.Commands;
using Nitrocid.Extras.ChatGpt.Gpt.Presets;

namespace Nitrocid.Extras.ChatGpt.Gpt
{
    /// <summary>
    /// Common Git shell class
    /// </summary>
    internal class ChatGptShellInfo : BaseShellInfo, IShellInfo
    {

        private readonly CommandInfo ask =
            new("ask", /* Localizable */ "Asks ChatGPT a question",
                [
                    new CommandArgumentInfo([
                        new CommandArgumentPart(true, "text")
                    ])
                ], new AskCommand());

        /// <summary>
        /// ChatGPT commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "disclaimer",
                new CommandInfo("disclaimer", /* Localizable */ "Shows you a disclaimer about how to use this service appropriately",
                    [
                        new CommandArgumentInfo()
                    ], new DisclaimerCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new DefaultPreset() },
            { "PowerLine1", new PowerLine1Preset() },
            { "PowerLine2", new PowerLine2Preset() },
            { "PowerLine3", new PowerLine3Preset() },
            { "PowerLineBG1", new PowerLineBG1Preset() },
            { "PowerLineBG2", new PowerLineBG2Preset() },
            { "PowerLineBG3", new PowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new ChatGptShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

        /// <summary>
        /// Needed to seamlessly make it feel like you're in the chatroom with ChatGPT
        /// </summary>
        public override bool SlashCommand =>
            true;

        public override CommandInfo NonSlashCommandInfo =>
            ask;

    }
}
