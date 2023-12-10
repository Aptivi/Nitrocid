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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;

namespace KS.ConsoleBase.Inputs.Styles
{
    /// <summary>
    /// Input style module
    /// </summary>
    public static class InputStyle
    {

        /// <summary>
        /// Prompts user for input (answer the question with your own answers)
        /// </summary>
        public static string PromptInput(string Question)
        {
            while (true)
            {
                // Variables
                string Answer;
                DebugWriter.WriteDebug(DebugLevel.I, "Question: {0}", Question);

                // Ask a question
                TextWriterColor.WriteKernelColor(Question, false, KernelColorType.Question);
                KernelColorTools.SetConsoleColor(KernelColorType.Input);

                // Wait for an answer
                Answer = Input.ReadLine();
                KernelColorTools.SetConsoleColor(KernelColorType.NeutralText);
                DebugWriter.WriteDebug(DebugLevel.I, "Answer: {0}", Answer);

                return Answer;
            }
        }

        /// <summary>
        /// Prompts user for password (answer the question with your own answers)
        /// </summary>
        public static string PromptInputPassword(string Question)
        {
            while (true)
            {
                // Variables
                string Answer;
                DebugWriter.WriteDebug(DebugLevel.I, "Question: {0}", Question);

                // Ask a question
                TextWriterColor.WriteKernelColor(Question, false, KernelColorType.Question);
                KernelColorTools.SetConsoleColor(KernelColorType.Input);

                // Wait for an answer
                Answer = Input.ReadLineNoInput();
                KernelColorTools.SetConsoleColor(KernelColorType.NeutralText);
                DebugWriter.WriteDebug(DebugLevel.I, "Answer: {0}", Answer);

                return Answer;
            }
        }

    }
}
