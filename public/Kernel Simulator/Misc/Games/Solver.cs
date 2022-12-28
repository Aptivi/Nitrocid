
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Misc.Games
{
    /// <summary>
    /// Solver game module
    /// </summary>
    public static class Solver
    {

        /// <summary>
        /// Minimum number for solver
        /// </summary>
        public static int SolverMinimumNumber = 0;
        /// <summary>
        /// Maximum number for solver
        /// </summary>
        public static int SolverMaximumNumber = 1000;
        /// <summary>
        /// Whether to show the input or not
        /// </summary>
        public static bool SolverShowInput;

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeSolver()
        {
            string RandomExpression;
            string UserEvaluated;
            var Operations = new string[] { "+", "-", "*", "/" };

            // Show tip to exit
            TextWriterColor.Write(Translate.DoTranslation("Press \"q\" to exit."));
            DebugWriter.WriteDebug(DebugLevel.I, "Initialized expressions.");
            while (true)
            {
                // Populate the numbers
                int FirstNumber = RandomDriver.Random(SolverMinimumNumber, SolverMaximumNumber);
                int OperationIndex = RandomDriver.Random(Operations.Length);
                int SecondNumber = RandomDriver.Random(SolverMinimumNumber, SolverMaximumNumber);

                // Generate the expression
                RandomExpression = FirstNumber.ToString() + Operations.ElementAt(OperationIndex) + SecondNumber.ToString();
                DebugWriter.WriteDebug(DebugLevel.I, "Expression to be solved: {0}", RandomExpression);
                TextWriterColor.Write(RandomExpression, true, KernelColorType.Input);

                // Wait for response
                UserEvaluated = SolverShowInput ? Input.ReadLine() : Input.ReadLineNoInput(Convert.ToChar("\0"));
                DebugWriter.WriteDebug(DebugLevel.I, "Evaluated: {0}", UserEvaluated);

                // Check to see if the user has entered the correct answer
                double UserEvaluatedNumber;
                double EvaluatedNumber = Convert.ToDouble(new DataTable().Compute(RandomExpression, null));
                if (double.TryParse(UserEvaluated, out UserEvaluatedNumber))
                {
                    if (UserEvaluatedNumber == EvaluatedNumber)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber);
                        TextWriterColor.Write(Translate.DoTranslation("Solved perfectly!"));
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber);
                        TextWriterColor.Write(Translate.DoTranslation("Solved incorrectly."));
                    }
                }
                else if (UserEvaluated == "q")
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "User requested exit.");
                    break;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "User evaluated \"{0}\". However, it's not numeric.", UserEvaluated);
                    TextWriterColor.Write(Translate.DoTranslation("You can only write the numbers."), true, KernelColorType.Error);
                }
            }
        }

    }
}
