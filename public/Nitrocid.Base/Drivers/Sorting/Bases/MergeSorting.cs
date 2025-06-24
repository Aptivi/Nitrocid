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

using Nitrocid.Base.Drivers.Sorting;

namespace Nitrocid.Base.Drivers.Sorting.Bases
{
    internal class MergeSorting : BaseSortingDriver, ISortingDriver
    {
        /// <inheritdoc/>
        public override string DriverName =>
            "Merge";

        /// <inheritdoc/>
        public override T[] SortNumbers<T>(T[] unsorted)
        {
            // Implementation
            static T[] Merge(T[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                T[] leftArray = new T[leftLen];
                T[] rightArray = new T[rightLen];
                int l, r;

                // Build the temporary arrays
                for (l = 0; l < leftLen; ++l)
                    leftArray[l] = target[left + l];
                for (r = 0; r < rightLen; ++r)
                    rightArray[r] = target[mid + 1 + r];

                // Reset the indexes
                l = r = 0;
                int totalIdx = left;

                // Now, merge the two arrays!
                while (l < leftLen && r < rightLen)
                {
                    if (leftArray[l] <= rightArray[r])
                        target[totalIdx++] = leftArray[l++];
                    else
                        target[totalIdx++] = rightArray[r++];
                }

                // Copy the leftovers
                while (l < leftLen)
                    target[totalIdx++] = leftArray[l++];
                while (r < rightLen)
                    target[totalIdx++] = rightArray[r++];
                return target;
            }

            static T[] MergeSort(T[] target, int left, int right)
            {
                if (left < right)
                {
                    int mid = left + (right - left) / 2;
                    MergeSort(target, left, mid);
                    MergeSort(target, mid + 1, right);
                    target = Merge(target, left, mid, right);
                }
                return target;
            }

            // Now, sort the array
            unsorted = MergeSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }
    }
}
