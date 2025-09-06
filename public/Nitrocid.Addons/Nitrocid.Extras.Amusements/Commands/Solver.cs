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

using Nitrocid.Extras.Amusements.Amusements.Games;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.Amusements.Commands
{
    /// <summary>
    /// Takes you to the math solver game
    /// </summary>
    /// <remarks>
    /// This game will give you an expression, calculates it secretly, and tells you to find the answer. If you are finished with the correct answer, press ENTER to verify that the answer is correct. It compares your answer with the calculated one, and if it's correct or wrong, it will tell you.
    /// </remarks>
    class SolverCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            Solver.InitializeSolver();
            return 0;
        }
    }
}
