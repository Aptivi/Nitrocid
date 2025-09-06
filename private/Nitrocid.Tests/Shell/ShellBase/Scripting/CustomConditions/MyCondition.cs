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

using Nitrocid.Kernel.Debugging;
using Terminaux.Shell.Scripting;
using Terminaux.Shell.Scripting.Conditions;

namespace Nitrocid.Tests.Shell.ShellBase.Scripting.CustomConditions
{
    internal class MyCondition : BaseCondition, ICondition
    {

        /// <inheritdoc/>
        public override string ConditionName => "haslen";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 2;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 2;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0}...", vars: [FirstVariable]);
            string VarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", vars: [FirstVariable, VarValue]);
            Satisfied = VarValue.Length > 0;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", vars: [Satisfied]);
            return Satisfied;
        }
    }
}
