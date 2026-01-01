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

using System;

namespace Nitrocid.Drivers.Sorting
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
        /// Sorts the 128-bit integers
        /// </summary>
        /// <returns>Sorted array of 128-bit integers</returns>
        Int128[] SortNumbersInt128(Int128[] unsorted);

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
