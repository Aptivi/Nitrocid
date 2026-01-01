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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System;

namespace Nitrocid.Shell.ShellBase.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if a UESH variable is not of the correct type
    /// </summary>
    public class IsNotPlatCondition : BaseCondition, ICondition
    {

        /// <inheritdoc/>
        public override string ConditionName => "isnotplat";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 1;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 2;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            // FirstVariable is actually a platform needed for parsing.
            if (!IsPlatCondition.Platforms.TryGetValue(FirstVariable, out Func<bool>? platFunc))
                throw new KernelException(KernelExceptionType.UESHConditionParse, Translate.DoTranslation("Platform {0} specified is invalid."), FirstVariable);

            // Get the action needed to get the comparer and test the condition defined above
            return !platFunc();
        }

    }
}
