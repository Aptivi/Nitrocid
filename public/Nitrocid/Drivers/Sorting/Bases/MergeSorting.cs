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
    internal class MergeSorting : BaseSortingDriver, ISortingDriver
    {
        /// <inheritdoc/>
        public override string DriverName =>
            "Merge";

        /// <inheritdoc/>
        public override byte[] SortNumbersInt8(byte[] unsorted)
        {
            // Implementation
            static byte[] Merge(byte[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                byte[] leftArray = new byte[leftLen];
                byte[] rightArray = new byte[rightLen];
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

            static byte[] MergeSort(byte[] target, int left, int right)
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

        /// <inheritdoc/>
        public override short[] SortNumbersInt16(short[] unsorted)
        {
            // Implementation
            static short[] Merge(short[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                short[] leftArray = new short[leftLen];
                short[] rightArray = new short[rightLen];
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

            static short[] MergeSort(short[] target, int left, int right)
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

        /// <inheritdoc/>
        public override int[] SortNumbersInt32(int[] unsorted)
        {
            static int[] Merge(int[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                int[] leftArray = new int[leftLen];
                int[] rightArray = new int[rightLen];
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

            static int[] MergeSort(int[] target, int left, int right)
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

        /// <inheritdoc/>
        public override long[] SortNumbersInt64(long[] unsorted)
        {
            static long[] Merge(long[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                long[] leftArray = new long[leftLen];
                long[] rightArray = new long[rightLen];
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

            static long[] MergeSort(long[] target, int left, int right)
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

        /// <inheritdoc/>
        public override Int128[] SortNumbersInt128(Int128[] unsorted)
        {
            static Int128[] Merge(Int128[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                Int128[] leftArray = new Int128[leftLen];
                Int128[] rightArray = new Int128[rightLen];
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

            static Int128[] MergeSort(Int128[] target, int left, int right)
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

        /// <inheritdoc/>
        public override float[] SortNumbersFloat(float[] unsorted)
        {
            static float[] Merge(float[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                float[] leftArray = new float[leftLen];
                float[] rightArray = new float[rightLen];
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

            static float[] MergeSort(float[] target, int left, int right)
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

        /// <inheritdoc/>
        public override double[] SortNumbersDouble(double[] unsorted)
        {
            static double[] Merge(double[] target, int left, int mid, int right)
            {
                int leftLen = mid - left + 1;
                int rightLen = right - mid;
                double[] leftArray = new double[leftLen];
                double[] rightArray = new double[rightLen];
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

            static double[] MergeSort(double[] target, int left, int right)
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
