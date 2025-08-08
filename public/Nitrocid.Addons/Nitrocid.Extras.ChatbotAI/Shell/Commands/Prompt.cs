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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using OpenAI.Chat;

namespace Nitrocid.Extras.ChatbotAI.Shell.Commands
{
    /// <summary>
    /// Sends the prompt to the chatbot for assistance
    /// </summary>
    class PromptCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Try to get the chat client
            ChatClient? clientChat = (ChatClient?)ChatbotShellCommon.ClientChat?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.ChatbotAI, LanguageTools.GetLocalized("NKS_CHATBOTAI_EXCEPTION_NOCLIENT"));

            // Now, use streaming to generate a response
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CHATBOTAI_RESPONDING"), ThemeColorType.Progress);
            var completions = clientChat.CompleteChatStreaming(parameters.ArgumentsText);
            foreach (var update in completions)
            {
                if (update.ContentUpdate.Count > 0)
                    TextWriterColor.Write(update.ContentUpdate[0].Text, false);
            }
            TextWriterColor.Write("\n\n" + LanguageTools.GetLocalized("NKS_CHATBOTAI_POSTRESPONSE"), ThemeColorType.Warning);
            return 0;
        }

    }
}
