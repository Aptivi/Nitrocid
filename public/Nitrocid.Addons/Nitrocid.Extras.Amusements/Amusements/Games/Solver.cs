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

using System;
using System.Data;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Inputs;

namespace Nitrocid.Extras.Amusements.Amusements.Games
{
    /// <summary>
    /// Solver game module
    /// </summary>
    public static class Solver
    {

        /// <summary>
        /// Minimum number for solver
        /// </summary>
        public static int SolverMinimumNumber
        {
            get => AmusementsInit.AmusementsConfig.SolverMinimumNumber;
            set => AmusementsInit.AmusementsConfig.SolverMinimumNumber = value < 0 ? 0 : value;
        }
        /// <summary>
        /// Maximum number for solver
        /// </summary>
        public static int SolverMaximumNumber
        {
            get => AmusementsInit.AmusementsConfig.SolverMaximumNumber;
            set => AmusementsInit.AmusementsConfig.SolverMaximumNumber = value < 0 ? 1000 : value;
        }
        /// <summary>
        /// Whether to show the input or not
        /// </summary>
        public static bool SolverShowInput =>
            AmusementsInit.AmusementsConfig.SolverShowInput;

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeSolver()
        {
            string RandomExpression;
            string UserEvaluated;
            var Operations = new string[] { "+", "-", "*", "/" };

            // Show tip to exit
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_AMUSEMENTS_SOLVER_PRESSTOEXIT", "Nitrocid.Extras.Amusements"));
            DebugWriter.WriteDebug(DebugLevel.I, "Initialized expressions.");
            while (true)
            {
                // Populate the numbers
                int FirstNumber = RandomDriver.Random(SolverMinimumNumber, SolverMaximumNumber);
                int OperationIndex = RandomDriver.Random(Operations.Length);
                int SecondNumber = RandomDriver.Random(SolverMinimumNumber, SolverMaximumNumber);

                // Generate the expression
                RandomExpression = FirstNumber.ToString() + Operations.ElementAt(OperationIndex) + SecondNumber.ToString();
                DebugWriter.WriteDebug(DebugLevel.I, "Expression to be solved: {0}", vars: [RandomExpression]);
                TextWriters.Write(RandomExpression, true, KernelColorType.Input);

                // Wait for response
                UserEvaluated = SolverShowInput ? InputTools.ReadLine() : InputTools.ReadLineNoInput(Convert.ToChar("\0"));
                DebugWriter.WriteDebug(DebugLevel.I, "Evaluated: {0}", vars: [UserEvaluated]);

                // Check to see if the user has entered the correct answer
                double UserEvaluatedNumber;
                double EvaluatedNumber = Convert.ToDouble(new DataTable().Compute(RandomExpression, null));
                if (double.TryParse(UserEvaluated, out UserEvaluatedNumber))
                {
                    if (UserEvaluatedNumber == EvaluatedNumber)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Expression is {0} and equals {1}", vars: [UserEvaluated, EvaluatedNumber]);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_AMUSEMENTS_SOLVER_CORRECT", "Nitrocid.Extras.Amusements"));
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Expression is {0} and equals {1}", vars: [UserEvaluated, EvaluatedNumber]);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_AMUSEMENTS_SOLVER_INCORRECT", "Nitrocid.Extras.Amusements"));
                    }
                }
                else if (UserEvaluated == "q")
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "User requested exit.");
                    break;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "User evaluated \"{0}\". However, it's not numeric.", vars: [UserEvaluated]);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_AMUSEMENTS_SOLVER_NEEDSNUM", "Nitrocid.Extras.Amusements"), true, KernelColorType.Error);
                }
            }
        }

    }
}
