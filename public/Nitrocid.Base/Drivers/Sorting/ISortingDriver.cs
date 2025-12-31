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

using System.Numerics;

namespace Nitrocid.Base.Drivers.Sorting
{
    /// <summary>
    /// Sorting driver interface for drivers
    /// </summary>
    public interface ISortingDriver : IDriver
    {
        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <typeparam name="T">Numeric type that the array contains</typeparam>
        /// <returns>Sorted array of byte numbers</returns>
        T[] SortNumbers<T>(T[] unsorted)
            where T : INumber<T>;
    }
}
