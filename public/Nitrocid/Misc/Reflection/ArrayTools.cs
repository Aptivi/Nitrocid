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

using Nitrocid.Drivers;
using Nitrocid.Drivers.RNG;
using Nitrocid.Drivers.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static T[]? RandomizeArray<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return array;

            // First, create a new list of random numbers with the array's value indexes
            List<(double, T)> valuesToShuffle = [];
            for (int i = 0; i < array.Length; i++)
                valuesToShuffle.Add((RandomDriver.RandomDouble(), array[i]));

            // Then, randomize the array after ordering the numbers
            var randomized = valuesToShuffle
                .OrderBy((val) => val.Item1)
                .Select((kvp) => kvp.Item2)
                .ToArray();
            return randomized;
        }

        /// <summary>
        /// Randomizes the array by shuffling elements, irrespective of the type, using <see cref="Random.Shuffle{T}(T[])"/>
        /// </summary>
        /// <typeparam name="T">Target type. It's not necessarily an integer.</typeparam>
        /// <param name="array">Target array to sort randomly</param>
        /// <returns>A new array containing elements that are shuffled.</returns>
        public static T[] RandomizeArraySystem<T>(this T[] array)
        {
            Random.Shared.Shuffle(array);
            return array;
        }

        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of byte numbers</returns>
        public static byte[] SortNumbers(this byte[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt8(unsorted);

        /// <summary>
        /// Sorts the short numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of short numbers</returns>
        public static short[] SortNumbers(this short[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt16(unsorted);

        /// <summary>
        /// Sorts the integers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of integers</returns>
        public static int[] SortNumbers(this int[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt32(unsorted);

        /// <summary>
        /// Sorts the 64-bit integers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of 64-bit integers</returns>
        public static long[] SortNumbers(this long[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt64(unsorted);

        /// <summary>
        /// Sorts the 128-bit integers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of 128-bit integers</returns>
        public static Int128[] SortNumbers(this Int128[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt128(unsorted);

        /// <summary>
        /// Sorts the single-precision decimal numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of single-precision decimal numbers</returns>
        public static float[] SortNumbers(this float[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersFloat(unsorted);

        /// <summary>
        /// Sorts the double-precision decimal numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <returns>Sorted array of double-precision decimal numbers</returns>
        public static double[] SortNumbers(this double[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersDouble(unsorted);

        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of byte numbers</returns>
        public static byte[] SortNumbers(this byte[] unsorted, string driverName) =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbersInt8(unsorted);

        /// <summary>
        /// Sorts the short numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of short numbers</returns>
        public static short[] SortNumbers(this short[] unsorted, string driverName) =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbersInt16(unsorted);

        /// <summary>
        /// Sorts the integers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of integers</returns>
        public static int[] SortNumbers(this int[] unsorted, string driverName) =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbersInt32(unsorted);

        /// <summary>
        /// Sorts the 64-bit integers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of 64-bit integers</returns>
        public static long[] SortNumbers(this long[] unsorted, string driverName) =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbersInt64(unsorted);

        /// <summary>
        /// Sorts the 128-bit integers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of 128-bit integers</returns>
        public static Int128[] SortNumbers(this Int128[] unsorted, string driverName) =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbersInt128(unsorted);

        /// <summary>
        /// Sorts the single-precision decimal numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of single-precision decimal numbers</returns>
        public static float[] SortNumbers(this float[] unsorted, string driverName) =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbersFloat(unsorted);

        /// <summary>
        /// Sorts the double-precision decimal numbers
        /// </summary>
        /// <param name="unsorted">An unsorted array of numbers</param>
        /// <param name="driverName">The sorting driver name to use</param>
        /// <returns>Sorted array of double-precision decimal numbers</returns>
        public static double[] SortNumbers(this double[] unsorted, string driverName) =>
            DriverHandler.GetDriver<ISortingDriver>(driverName).SortNumbersDouble(unsorted);
    }
}
