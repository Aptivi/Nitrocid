﻿//
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

using KS.Drivers;
using KS.Drivers.RNG;
using System.Collections.Generic;
using System.Linq;

namespace KS.Misc.Reflection
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
        public static T[] RandomizeArray<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return array;

            // First, create a new list of random numbers with the array's value indexes
            List<(double, int)> valuesToShuffle = new();
            for (int i = 0; i < array.Length; i++)
                valuesToShuffle.Add((RandomDriver.RandomDouble(), i));

            // Second, re-order the above list
            var ordered = valuesToShuffle.OrderBy((val) => val.Item1).ToArray();

            // Then, get their values according to the shuffled indexes
            var randomizedIndexes = ordered.Select((val) => val.Item2).ToArray();

            // Finally, randomize the array
            var randomized = randomizedIndexes.Select((idx) => array[idx]).ToArray();
            return randomized;
        }

        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <returns>Sorted array of byte numbers</returns>
        public static byte[] SortNumbers(this byte[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt8(unsorted);

        /// <summary>
        /// Sorts the short numbers
        /// </summary>
        /// <returns>Sorted array of short numbers</returns>
        public static short[] SortNumbers(this short[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt16(unsorted);

        /// <summary>
        /// Sorts the integers
        /// </summary>
        /// <returns>Sorted array of integers</returns>
        public static int[] SortNumbers(this int[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt32(unsorted);

        /// <summary>
        /// Sorts the 64-bit integers
        /// </summary>
        /// <returns>Sorted array of 64-bit integers</returns>
        public static long[] SortNumbers(this long[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt64(unsorted);

        /// <summary>
        /// Sorts the single-precision decimal numbers
        /// </summary>
        /// <returns>Sorted array of single-precision decimal numbers</returns>
        public static float[] SortNumbers(this float[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersFloat(unsorted);

        /// <summary>
        /// Sorts the double-precision decimal numbers
        /// </summary>
        /// <returns>Sorted array of double-precision decimal numbers</returns>
        public static double[] SortNumbers(this double[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersDouble(unsorted);
    }
}