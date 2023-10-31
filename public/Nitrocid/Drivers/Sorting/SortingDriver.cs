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

namespace KS.Drivers.Sorting
{
    /// <summary>
    /// Sorting driver module
    /// </summary>
    public static class SortingDriver
    {
        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <returns>Sorted array of byte numbers</returns>
        public static byte[] SortNumbersInt8(byte[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt8(unsorted);

        /// <summary>
        /// Sorts the short numbers
        /// </summary>
        /// <returns>Sorted array of short numbers</returns>
        public static short[] SortNumbersInt16(short[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt16(unsorted);

        /// <summary>
        /// Sorts the integers
        /// </summary>
        /// <returns>Sorted array of integers</returns>
        public static int[] SortNumbersInt32(int[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt32(unsorted);

        /// <summary>
        /// Sorts the 64-bit integers
        /// </summary>
        /// <returns>Sorted array of 64-bit integers</returns>
        public static long[] SortNumbersInt64(long[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt64(unsorted);

        /// <summary>
        /// Sorts the 128-bit integers (works the same as 64-bit integers until the migration to .NET 8.0 occurs)
        /// </summary>
        /// <returns>Sorted array of 128-bit integers</returns>
        public static long[] SortNumbersInt128(long[] unsorted) =>
            DriverHandler.CurrentSortingDriverLocal.SortNumbersInt128(unsorted);
    }
}
