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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Nitrocid.Extras.ChatbotAI.Shell.Presets;
using Nitrocid.Extras.ChatbotAI.Shell.Commands;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Arguments.Base;

namespace Nitrocid.Extras.ChatbotAI.Shell
{
    /// <summary>
    /// Common Chatbot AI shell class
    /// </summary>
    internal class ChatbotShellInfo : BaseShellInfo<ChatbotShell>, IShellInfo
    {
        /// <summary>
        /// Chatbot AI commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [ ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new ChatbotDefaultPreset() },
            { "PowerLine1", new ChatbotPowerLine1Preset() },
            { "PowerLine2", new ChatbotPowerLine2Preset() },
            { "PowerLine3", new ChatbotPowerLine3Preset() },
            { "PowerLineBG1", new ChatbotPowerLineBG1Preset() },
            { "PowerLineBG2", new ChatbotPowerLineBG2Preset() },
            { "PowerLineBG3", new ChatbotPowerLineBG3Preset() }
        };

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "Chatbot";

        public override bool SlashCommand => true;

        public override CommandInfo NonSlashCommandInfo => new("prompt", /* Localizable */ "NKS_CHATBOTAI_SHELL_COMMAND_PROMPT_DESC",
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "promptString", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_CHATBOTAI_SHELL_COMMAND_PROMPT_ARGUMENT_PROMPTSTRING_DESC"
                    })
                ])
            ], new PromptCommand());
    }
}
