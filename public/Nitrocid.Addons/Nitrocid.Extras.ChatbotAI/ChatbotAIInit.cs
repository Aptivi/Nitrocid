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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Core.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Extras.ChatbotAI.Commands;
using Nitrocid.Extras.ChatbotAI.Settings;
using Nitrocid.Extras.ChatbotAI.Shell;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.ChatbotAI
{
    internal class ChatbotAIInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("chatbot", /* Localizable */ "NKS_CHATBOTAI_COMMAND_CHATBOT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("apikey", /* Localizable */ "NKS_CHATBOTAI_COMMAND_CHATBOT_SWITCH_APIKEY_DESC"),

                        new SwitchInfo("model", /* Localizable */ "NKS_CHATBOTAI_COMMAND_CHATBOT_SWITCH_MODEL_DESC"),
                    ])
                ], new ChatbotCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasChatbotAI);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasChatbotAI);

        internal static ChatbotAIConfig ChatbotAIConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ChatbotAIConfig)) ? (ChatbotAIConfig)Config.baseConfigurations[nameof(ChatbotAIConfig)] : Config.GetFallbackKernelConfig<ChatbotAIConfig>();

        public void FinalizeAddon()
        { }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.ChatbotAI.Resources.Languages.Output.Localizations", typeof(ChatbotAIInit).Assembly));
            var config = new ChatbotAIConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("ChatbotShell", new ChatbotShellInfo());
            NetworkConnectionTools.RegisterCustomConnectionType("Chatbot");
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("ChatbotShell");
            NetworkConnectionTools.UnregisterCustomConnectionType("Chatbot");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ChatbotAIConfig));
        }
    }
}
