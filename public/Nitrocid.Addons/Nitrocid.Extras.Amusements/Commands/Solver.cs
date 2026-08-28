//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Languages;
using Nitrocid.Extras.Amusements.Amusements.Games;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

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
        public override string Command => 
            "solver";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_AMUSEMENTS_COMMAND_SOLVER_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            Solver.InitializeSolver();
            return 0;
        }
    }
}
