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

namespace Nitrocid.Drivers.Sorting.Bases
{
    internal class SelectionSorting : BaseSortingDriver, ISortingDriver
    {
        /// <inheritdoc/>
        public override string DriverName =>
            "Selection";

        /// <inheritdoc/>
        public override byte[] SortNumbersInt8(byte[] unsorted)
        {
            // Implementation
            int length = unsorted.Length;
            for (int i = 0; i < length - 1; i++)
            {
                var smallest = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (unsorted[j] < unsorted[smallest])
                        smallest = j;
                }

                // Do the swap!
                (unsorted[smallest], unsorted[i]) = (unsorted[i], unsorted[smallest]);
            }
            return unsorted;
        }

        /// <inheritdoc/>
        public override short[] SortNumbersInt16(short[] unsorted)
        {
            // Implementation
            int length = unsorted.Length;
            for (int i = 0; i < length - 1; i++)
            {
                var smallest = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (unsorted[j] < unsorted[smallest])
                        smallest = j;
                }

                // Do the swap!
                (unsorted[smallest], unsorted[i]) = (unsorted[i], unsorted[smallest]);
            }
            return unsorted;
        }

        /// <inheritdoc/>
        public override int[] SortNumbersInt32(int[] unsorted)
        {
            // Implementation
            int length = unsorted.Length;
            for (int i = 0; i < length - 1; i++)
            {
                var smallest = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (unsorted[j] < unsorted[smallest])
                        smallest = j;
                }

                // Do the swap!
                (unsorted[smallest], unsorted[i]) = (unsorted[i], unsorted[smallest]);
            }
            return unsorted;
        }

        /// <inheritdoc/>
        public override long[] SortNumbersInt64(long[] unsorted)
        {
            // Implementation
            int length = unsorted.Length;
            for (int i = 0; i < length - 1; i++)
            {
                var smallest = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (unsorted[j] < unsorted[smallest])
                        smallest = j;
                }

                // Do the swap!
                (unsorted[smallest], unsorted[i]) = (unsorted[i], unsorted[smallest]);
            }
            return unsorted;
        }

        /// <inheritdoc/>
        public override Int128[] SortNumbersInt128(Int128[] unsorted)
        {
            // Implementation
            int length = unsorted.Length;
            for (int i = 0; i < length - 1; i++)
            {
                var smallest = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (unsorted[j] < unsorted[smallest])
                        smallest = j;
                }

                // Do the swap!
                (unsorted[smallest], unsorted[i]) = (unsorted[i], unsorted[smallest]);
            }
            return unsorted;
        }

        /// <inheritdoc/>
        public override float[] SortNumbersFloat(float[] unsorted)
        {
            // Implementation
            int length = unsorted.Length;
            for (int i = 0; i < length - 1; i++)
            {
                var smallest = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (unsorted[j] < unsorted[smallest])
                        smallest = j;
                }

                // Do the swap!
                (unsorted[smallest], unsorted[i]) = (unsorted[i], unsorted[smallest]);
            }
            return unsorted;
        }

        /// <inheritdoc/>
        public override double[] SortNumbersDouble(double[] unsorted)
        {
            // Implementation
            int length = unsorted.Length;
            for (int i = 0; i < length - 1; i++)
            {
                var smallest = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (unsorted[j] < unsorted[smallest])
                        smallest = j;
                }

                // Do the swap!
                (unsorted[smallest], unsorted[i]) = (unsorted[i], unsorted[smallest]);
            }
            return unsorted;
        }

    }
}
