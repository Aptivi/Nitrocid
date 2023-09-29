
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

#if AIASSISTANT
using KS.ConsoleBase.Inputs;
using KS.Misc.Assistant;
#else
using KS.Kernel;
#endif

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// AI Assistant for Nitrocid (experimental)
    /// </summary>
    /// <remarks>
    /// Please note that this assistant may generate inaccurate information, so if you're confused by what it gives you, you can easily consult the Nitrocid documentation. This is an experimental AI assistant trained by ML.NET to help you use Nitrocid KS.
    /// </remarks>
    class AssistantCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
#if AIASSISTANT
            string question = Input.ReadLine(Translate.DoTranslation("Ask the Assistant") + ": ");
            var sampleData = new AssistantPredictor.ModelInput()
            {
                Question = question,
            };

            // Try to get the answer
            TextWriterColor.Write(Translate.DoTranslation("The Assistant is thinking..."), true, KernelColorType.Progress);
            var result = AssistantPredictor.Predict(sampleData);
            string answer = result.PredictedLabel;
            TextWriterColor.Write(Translate.DoTranslation("The Assistant says:") + $" {answer}", true, KernelColorType.Success);
#else
            TextWriterColor.Write(Translate.DoTranslation("The Assistant is unavailable for your current platform. You need a 64-bit computer to be able to use Assistant. You're running") + $" {KernelPlatform.GetCurrentRid()}. " + Translate.DoTranslation("If you believe this to be incorrect, report this issue to us!"), true, KernelColorType.Error);
#endif
            return 0;
        }

    }
}
