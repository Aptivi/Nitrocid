using System;
using System.Data;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Microsoft.VisualBasic.CompilerServices;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

namespace KS.Misc.Games
{
    public static class Solver
    {

        public static int SolverMinimumNumber = 0;
        public static int SolverMaximumNumber = 1000;
        public static bool SolverShowInput;

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeSolver()
        {
            var RandomDriver = new Random();
            string RandomExpression;
            string UserEvaluated;
            var Operations = new string[] { "+", "-", "*", "/" };

            // Show tip to exit
            TextWriterColor.Write(Translate.DoTranslation("Press \"q\" to exit."), true, ColorTools.ColTypes.Neutral);
            DebugWriter.Wdbg(DebugLevel.I, "Initialized expressions.");
            while (true)
            {
                // Populate the numbers
                int FirstNumber = RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber);
                int OperationIndex = RandomDriver.Next(Operations.Length);
                int SecondNumber = RandomDriver.Next(SolverMinimumNumber, SolverMaximumNumber);

                // Generate the expression
                RandomExpression = FirstNumber.ToString() + Operations.ElementAt(OperationIndex) + SecondNumber.ToString();
                DebugWriter.Wdbg(DebugLevel.I, "Expression to be solved: {0}", RandomExpression);
                TextWriterColor.Write(RandomExpression, true, ColorTools.ColTypes.Input);

                // Wait for response
                UserEvaluated = SolverShowInput ? Input.ReadLine() : Input.ReadLineNoInput(Conversions.ToChar(""));
                DebugWriter.Wdbg(DebugLevel.I, "Evaluated: {0}", UserEvaluated);

                // Check to see if the user has entered the correct answer
                double UserEvaluatedNumber;
                double EvaluatedNumber = Conversions.ToDouble(new DataTable().Compute(RandomExpression, null));
                if (double.TryParse(UserEvaluated, out UserEvaluatedNumber))
                {
                    if (UserEvaluatedNumber == EvaluatedNumber)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber);
                        TextWriterColor.Write(Translate.DoTranslation("Solved perfectly!"), true, ColorTools.ColTypes.Neutral);
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Expression is {0} and equals {1}", UserEvaluated, EvaluatedNumber);
                        TextWriterColor.Write(Translate.DoTranslation("Solved incorrectly."), true, ColorTools.ColTypes.Neutral);
                    }
                }
                else if (UserEvaluated == "q")
                {
                    DebugWriter.Wdbg(DebugLevel.W, "User requested exit.");
                    break;
                }
                else if (ReadLineReboot.ReadLine.ReadRanToCompletion)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "User evaluated \"{0}\". However, it's not numeric.", UserEvaluated);
                    TextWriterColor.Write(Translate.DoTranslation("You can only write the numbers."), true, ColorTools.ColTypes.Error);
                }
            }
        }

    }
}