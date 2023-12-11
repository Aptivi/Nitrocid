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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.ChatGpt.Gpt.Commands
{
    /// <summary>
    /// Shows you a disclaimer
    /// </summary>
    /// <remarks>
    /// This command shows you a disclaimer about the usage of the ChatGPT tool, as well as a link to its Terms and Conditions.
    /// </remarks>
    class DisclaimerCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            TextWriters.Write(
                Translate.DoTranslation("ChatGPT is an Artificial Intelligence (AI) tool that allows you to initiate an almost-humanly conversation with the AI bot.") + " " +
                Translate.DoTranslation("It helps you solve problems and generates content for you according to the prompt given, such as poetry, case studies, letters, conversations, and even ideas and fun jokes.") + CharManager.NewLine + CharManager.NewLine +
                Translate.DoTranslation("However, neither ChatGPT nor OpenAI can generate content that violate the terms and conditions, which can be read in the below link.") + " " +
                Translate.DoTranslation("ChatGPT will stop generating such content and gives you a friendly reminder message that reminds you of the terms and conditions if it detects that your prompt potentially violates the below terms and conditions:") + CharManager.NewLine +
                "  - https://openai.com/policies/terms-of-use" + CharManager.NewLine + CharManager.NewLine +
                Translate.DoTranslation("Please note that this client is unofficial and is not in any way supported by OpenAI. This client is made by the makers of Nitrocid KS, Aptivi, and are not affiliated with OpenAI and its affiliates."),
                KernelColorType.Tip
            );
            return 0;
        }

    }
}
