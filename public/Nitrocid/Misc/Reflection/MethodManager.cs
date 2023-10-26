//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Reflection;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Method management module
    /// </summary>
    public static class MethodManager
    {

        /// <summary>
        /// Gets a method from method name
        /// </summary>
        /// <param name="Method">Method name. Use operator NameOf to get name.</param>
        /// <returns>Method information</returns>
        public static MethodBase GetMethod(string Method)
        {
            Type[] PossibleTypes;
            MethodInfo PossibleMethod;

            // Get types of possible flag locations
            PossibleTypes = Assembly.GetExecutingAssembly().GetTypes();

            // Get fields of flag modules
            foreach (Type PossibleType in PossibleTypes)
            {
                PossibleMethod = PossibleType.GetMethod(Method);
                if (PossibleMethod != null)
                    return PossibleMethod;
            }
            return null;
        }

    }
}
