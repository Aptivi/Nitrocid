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
    /// Sorting driver interface for drivers
    /// </summary>
    public interface ISortingDriver : IDriver
    {
        /// <summary>
        /// Sorts the byte numbers
        /// </summary>
        /// <returns>Sorted array of byte numbers</returns>
        byte[] SortNumbersInt8(byte[] unsorted);

        /// <summary>
        /// Sorts the short numbers
        /// </summary>
        /// <returns>Sorted array of short numbers</returns>
        short[] SortNumbersInt16(short[] unsorted);

        /// <summary>
        /// Sorts the integers
        /// </summary>
        /// <returns>Sorted array of integers</returns>
        int[] SortNumbersInt32(int[] unsorted);

        /// <summary>
        /// Sorts the 64-bit integers
        /// </summary>
        /// <returns>Sorted array of 64-bit integers</returns>
        long[] SortNumbersInt64(long[] unsorted);

        /// <summary>
        /// Sorts the 128-bit integers (works the same as 64-bit integers until the migration to .NET 8.0 occurs)
        /// </summary>
        /// <returns>Sorted array of 128-bit integers</returns>
        long[] SortNumbersInt128(long[] unsorted);

        /// <summary>
        /// Sorts the single-precision decimal numbers
        /// </summary>
        /// <returns>Sorted array of single-precision decimal numbers</returns>
        float[] SortNumbersFloat(float[] unsorted);

        /// <summary>
        /// Sorts the double-precision decimal numbers
        /// </summary>
        /// <returns>Sorted array of double-precision decimal numbers</returns>
        double[] SortNumbersDouble(double[] unsorted);
    }
}
