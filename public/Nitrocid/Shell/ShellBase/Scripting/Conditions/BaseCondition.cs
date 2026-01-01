//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

namespace Nitrocid.Shell.ShellBase.Scripting.Conditions
{
    /// <summary>
    /// Base UESH scripting condition class
    /// </summary>
    public abstract class BaseCondition : ICondition
    {

        /// <summary>
        /// Condition name
        /// </summary>
        public virtual string ConditionName => "none";

        /// <summary>
        /// Condition position (starts from 1)
        /// </summary>
        public virtual int ConditionPosition { get; } = 1;

        /// <summary>
        /// Condition required argument numbers (starts from 1)
        /// </summary>
        public virtual int ConditionRequiredArguments { get; } = 1;

        /// <summary>
        /// Is the condition satisfied?
        /// </summary>
        /// <param name="FirstVariable">The first UESH variable (can be unsanitized; can start without $)</param>
        /// <param name="SecondVariable">The second UESH variable (can be unsanitized; can start without $)</param>
        /// <returns>True if satisfied; otherwise, false</returns>
        public virtual bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Doing nothing because the condition is undefined. Returning true...");
            return true;
        }

        /// <summary>
        /// Is the condition satisfied?
        /// </summary>
        /// <param name="Variables">Array of UESH variables (can be unsanitized; can start without $)</param>
        /// <returns>True if satisfied; otherwise, false</returns>
        public bool IsConditionSatisfied(string[] Variables)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Doing nothing because the condition is undefined. Returning true...");
            return true;
        }

    }
}
