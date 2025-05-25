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

using Magico.Enumeration;
using System;
using System.Runtime.Serialization;

namespace Nitrocid.Drivers.Sorting
{
    /// <summary>
    /// Base sorting driver using the bubble sort algorithm
    /// </summary>
    [DataContract]
    public abstract class BaseSortingDriver : ISortingDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName =>
            "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType =>
            DriverTypes.Sorting;

        /// <inheritdoc/>
        public virtual bool DriverInternal =>
            false;

        /// <inheritdoc/>
        public virtual byte[] SortNumbersInt8(byte[] unsorted) =>
            ArrayTools.SortNumbers(unsorted);

        /// <inheritdoc/>
        public virtual short[] SortNumbersInt16(short[] unsorted) =>
            ArrayTools.SortNumbers(unsorted);

        /// <inheritdoc/>
        public virtual int[] SortNumbersInt32(int[] unsorted) =>
            ArrayTools.SortNumbers(unsorted);

        /// <inheritdoc/>
        public virtual long[] SortNumbersInt64(long[] unsorted) =>
            ArrayTools.SortNumbers(unsorted);

        /// <inheritdoc/>
        public virtual Int128[] SortNumbersInt128(Int128[] unsorted)
        {
            // Get the number of iterations
            int iteration = unsorted.Length;
            bool swap;

            // Now, iterate through the whole array to check to see if we need to sort or not
            for (int i = 0; i < iteration - 1; i++)
            {
                // Reset the swap requirement
                swap = false;

                // Now, compare the two values to see if they need sorting
                for (int j = 0; j < iteration - i - 1; j++)
                {
                    if (unsorted[j] > unsorted[j + 1])
                    {
                        (unsorted[j], unsorted[j + 1]) = (unsorted[j + 1], unsorted[j]);
                        swap = true;
                    }
                }

                // Break if swap is not required
                if (!swap)
                    break;
            }
            return unsorted;
        }

        /// <inheritdoc/>
        public virtual float[] SortNumbersFloat(float[] unsorted) =>
            ArrayTools.SortNumbers(unsorted);

        /// <inheritdoc/>
        public virtual double[] SortNumbersDouble(double[] unsorted) =>
            ArrayTools.SortNumbers(unsorted);
    }
}
