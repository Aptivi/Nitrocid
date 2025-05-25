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

using Nitrocid.Drivers;
using Nitrocid.Drivers.RNG;
using Nitrocid.Drivers.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MagicoArrayTools = Magico.Enumeration.ArrayTools;

namespace Nitrocid.Misc.Reflection
{
    /// <summary>
    /// Array tools that are useful
    /// </summary>
    public static class ArrayTools
    {
        /// <summary>
        /// Randomizes the array by shuffling elements, irrespective of the type, using a type of <seealso href="http://en.wikipedia.org/wiki/Schwartzian_transform">Schwartzian transform</seealso>
        /// </summary>
        /// <typeparam name="T">Target type. It's not necessarily an integer.</typeparam>
        /// <param name="array">Target array to sort randomly</param>
        /// <returns>A new array containing elements that are shuffled.</returns>
        public static T[]? RandomizeArray<T>(this T[] array) =>
            MagicoArrayTools.RandomizeArray(array);

        /// <summary>
        /// Randomizes the array by shuffling elements, irrespective of the type, using <see cref="Random.Shuffle{T}(T[])"/>
        /// </summary>
        /// <typeparam name="T">Target type. It's not necessarily an integer.</typeparam>
        /// <param name="array">Target array to sort randomly</param>
        /// <returns>A new array containing elements that are shuffled.</returns>
        public static T[] RandomizeArraySystem<T>(this T[] array)
        {
            MagicoArrayTools.RandomShared.Shuffle(array);
            return array;
        }

        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of byte numbers</returns>
        public static T[] SortNumbers<T>(this T[] unsorted)
            where T : INumber<T> =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbers(unsorted);

        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of byte numbers</returns>
        public static T[] SortNumbers<T>(this T[] unsorted, string driverName)
            where T : INumber<T> =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbers(unsorted);
    }
}
