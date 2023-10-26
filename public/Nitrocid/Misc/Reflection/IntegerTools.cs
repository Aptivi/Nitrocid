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

using KS.Misc.Text;
using System;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Tools to manipulate with integers
    /// </summary>
    public static class IntegerTools
    {
        private static readonly string[] sizeOrders =
        {
            "B",  // Bytes
            "KB", // Kilobytes
            "MB", // Megabytes
            "GB", // Gigabytes
            "TB", // Terabytes
            "PB", // Petabytes
            "EB", // Exabytes
#if NET8_0
            "ZB", // Zettabytes
            "YB", // Yottabytes
            "RB", // Ronnabytes
            "QB"  // Quettabytes
#endif
        };

        /// <summary>
        /// Swaps the two numbers if the source is larger than the target
        /// </summary>
        /// <param name="SourceNumber">Number</param>
        /// <param name="TargetNumber">Number</param>
        public static void SwapIfSourceLarger(this ref int SourceNumber, ref int TargetNumber)
        {
            int Source = SourceNumber;
            int Target = TargetNumber;
            if (SourceNumber > TargetNumber)
            {
                SourceNumber = Target;
                TargetNumber = Source;
            }
        }

        /// <summary>
        /// Swaps the two numbers if the source is larger than the target
        /// </summary>
        /// <param name="SourceNumber">Number</param>
        /// <param name="TargetNumber">Number</param>
        public static void SwapIfSourceLarger(this ref long SourceNumber, ref long TargetNumber)
        {
            long Source = SourceNumber;
            long Target = TargetNumber;
            if (SourceNumber > TargetNumber)
            {
                SourceNumber = Target;
                TargetNumber = Source;
            }
        }

        /// <summary>
        /// Gets the amount of digits in a specified number
        /// </summary>
        /// <param name="Number">Number to query its digit count</param>
        /// <returns>How many digits are there in a number</returns>
        public static int GetDigits(this long Number) =>
            Number == 0 ? 1 : (int)Math.Log10(Math.Abs(Number)) + 1;

        /// <summary>
        /// Gets the amount of digits in a specified number
        /// </summary>
        /// <param name="Number">Number to query its digit count</param>
        /// <returns>How many digits are there in a number</returns>
        public static int GetDigits(this int Number) =>
            Number == 0 ? 1 : (int)Math.Log10(Math.Abs(Number)) + 1;

        /// <summary>
		/// Converts a file size in bytes to a human-readable format
		/// </summary>
		public static string SizeString(this int bytes) =>
            ((long)bytes).SizeString();

        /// <summary>
        /// Converts a file size in bytes to a human-readable format
        /// </summary>
        public static string SizeString(this uint bytes) =>
            ((long)bytes).SizeString();

        /// <summary>
        /// Converts a file size in bytes to a human-readable format
        /// </summary>
        public static string SizeString(this ulong bytes) =>
            ((long)bytes).SizeString();

        /// <summary>
        /// Converts a file size in bytes to a human-readable format
        /// </summary>
        public static string SizeString(this long bytes)
        {
            if (double.IsNaN(bytes) || bytes < 0)
                bytes = 0;

            double bytesForEachKilobyte = 1024;
            var orderIdx = 0;
            double len = bytes;
            while (len >= bytesForEachKilobyte && orderIdx < sizeOrders.Length - 1)
            {
                orderIdx++;
                len /= bytesForEachKilobyte;
            }

            return TextTools.FormatString("{0:0.#} {1}", len, sizeOrders[orderIdx]);
        }
    }
}
