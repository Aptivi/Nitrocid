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
using Textify.General;

namespace Nitrocid.Base.Misc.Reflection
{
    /// <summary>
    /// Tools to manipulate with integers
    /// </summary>
    public static class IntegerTools
    {
        private static readonly string[] sizeOrders =
        [
            "B",  // Bytes
            "KB", // Kilobytes
            "MB", // Megabytes
            "GB", // Gigabytes
            "TB", // Terabytes
            "PB", // Petabytes
            "EB", // Exabytes
            "ZB", // Zettabytes
            "YB", // Yottabytes
            "RB", // Ronnabytes
            "QB"  // Quettabytes
        ];

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

        /// <summary>
        /// Converts the number to the binary representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A binary representation of the number</returns>
        public static string ToBinary(this short num) =>
            Convert.ToString(num, 2);

        /// <summary>
        /// Converts the number to the octal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>An octal representation of the number</returns>
        public static string ToOctal(this short num) =>
            Convert.ToString(num, 8);

        /// <summary>
        /// Converts the number to the number representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A number representation of the number</returns>
        public static string ToNumber(this short num) =>
            Convert.ToString(num);

        /// <summary>
        /// Converts the number to the hexadecimal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A hexadecimal representation of the number</returns>
        public static string ToHex(this short num) =>
            Convert.ToString(num, 16);

        /// <summary>
        /// Converts the number to the binary representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A binary representation of the number</returns>
        public static string ToBinary(this int num) =>
            Convert.ToString(num, 2);

        /// <summary>
        /// Converts the number to the octal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>An octal representation of the number</returns>
        public static string ToOctal(this int num) =>
            Convert.ToString(num, 8);

        /// <summary>
        /// Converts the number to the number representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A number representation of the number</returns>
        public static string ToNumber(this int num) =>
            Convert.ToString(num);

        /// <summary>
        /// Converts the number to the hexadecimal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A hexadecimal representation of the number</returns>
        public static string ToHex(this int num) =>
            Convert.ToString(num, 16);

        /// <summary>
        /// Converts the number to the binary representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A binary representation of the number</returns>
        public static string ToBinary(this long num) =>
            Convert.ToString(num, 2);

        /// <summary>
        /// Converts the number to the octal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>An octal representation of the number</returns>
        public static string ToOctal(this long num) =>
            Convert.ToString(num, 8);

        /// <summary>
        /// Converts the number to the number representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A number representation of the number</returns>
        public static string ToNumber(this long num) =>
            Convert.ToString(num);

        /// <summary>
        /// Converts the number to the hexadecimal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A hexadecimal representation of the number</returns>
        public static string ToHex(this long num) =>
            Convert.ToString(num, 16);

        /// <summary>
        /// Converts the number to the binary representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A binary representation of the number</returns>
        public static string ToBinary(this byte num) =>
            Convert.ToString(num, 2);

        /// <summary>
        /// Converts the number to the octal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>An octal representation of the number</returns>
        public static string ToOctal(this byte num) =>
            Convert.ToString(num, 8);

        /// <summary>
        /// Converts the number to the number representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A number representation of the number</returns>
        public static string ToNumber(this byte num) =>
            Convert.ToString(num);

        /// <summary>
        /// Converts the number to the hexadecimal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A hexadecimal representation of the number</returns>
        public static string ToHex(this byte num) =>
            Convert.ToString(num, 16);

        /// <summary>
        /// Converts the number to the binary representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A binary representation of the number</returns>
        public static string ToBinary(this ushort num) =>
            Convert.ToString(num, 2);

        /// <summary>
        /// Converts the number to the octal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>An octal representation of the number</returns>
        public static string ToOctal(this ushort num) =>
            Convert.ToString(num, 8);

        /// <summary>
        /// Converts the number to the number representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A number representation of the number</returns>
        public static string ToNumber(this ushort num) =>
            Convert.ToString(num);

        /// <summary>
        /// Converts the number to the hexadecimal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A hexadecimal representation of the number</returns>
        public static string ToHex(this ushort num) =>
            Convert.ToString(num, 16);

        /// <summary>
        /// Converts the number to the binary representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A binary representation of the number</returns>
        public static string ToBinary(this uint num) =>
            Convert.ToString(num, 2);

        /// <summary>
        /// Converts the number to the octal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>An octal representation of the number</returns>
        public static string ToOctal(this uint num) =>
            Convert.ToString(num, 8);

        /// <summary>
        /// Converts the number to the number representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A number representation of the number</returns>
        public static string ToNumber(this uint num) =>
            Convert.ToString(num);

        /// <summary>
        /// Converts the number to the hexadecimal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A hexadecimal representation of the number</returns>
        public static string ToHex(this uint num) =>
            Convert.ToString(num, 16);

        /// <summary>
        /// Converts the number to the binary representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A binary representation of the number</returns>
        public static string ToBinary(this sbyte num) =>
            Convert.ToString(num, 2);

        /// <summary>
        /// Converts the number to the octal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>An octal representation of the number</returns>
        public static string ToOctal(this sbyte num) =>
            Convert.ToString(num, 8);

        /// <summary>
        /// Converts the number to the number representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A number representation of the number</returns>
        public static string ToNumber(this sbyte num) =>
            Convert.ToString(num);

        /// <summary>
        /// Converts the number to the hexadecimal representation
        /// </summary>
        /// <param name="num">Target number</param>
        /// <returns>A hexadecimal representation of the number</returns>
        public static string ToHex(this sbyte num) =>
            Convert.ToString(num, 16);
    }
}
