//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Base.ConsoleBase.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Network.Connections;
using OpenAI.Chat;

namespace Nitrocid.Extras.ChatbotAI.Commands
{
    /// <summary>
    /// Opens a shell that allows you to talk to an assistant
    /// </summary>
    class ChatbotCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check if user has provided the API key via the switch
            string apiKey = ChatbotAIInit.ChatbotAIConfig.ChatGPTApiKey;
            if (parameters.ContainsSwitch("-apikey"))
                apiKey = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-apikey");

            // Check the model
            string modelUsed = "gpt-4.1-mini";
            if (parameters.ContainsSwitch("-model"))
                modelUsed = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-model");

            // Now, try to authenticate
            NetworkConnectionTools.OpenConnectionForShell("ChatbotShell",
                (_) => EstablishChatGPTConnection(apiKey, modelUsed),
                (_, connection) => EstablishChatGPTConnection(connection.Password, connection.Options["GPTModel"]?.ToString() ?? "gpt-4.1-mini"), "chatgpt.com");
            return 0;
        }

        private NetworkConnection EstablishChatGPTConnection(string apiKey, string model)
        {
            // Prompt for API key if needed
            while (string.IsNullOrEmpty(apiKey))
            {
                apiKey = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_CHATBOTAI_APIKEYPROMPT") + ": ");
                if (string.IsNullOrEmpty(apiKey))
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CHATBOTAI_APIKEYNOTPROVIDED"), ThemeColorType.Error);
            }

            // Try to authenticate
            var chatClient = new ChatClient(model, apiKey);
            return NetworkConnectionTools.EstablishConnection("ChatGPT", "chatgpt.com", "Chatbot", chatClient);
        }
    }
}
