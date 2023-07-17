
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Drivers;

namespace KS.Files.Operations
{
    /// <summary>
    /// Combination file operations module
    /// </summary>
    public static class Combination
    {

        /// <summary>
        /// Combines the text files and puts the combined output to the array
        /// </summary>
        /// <param name="Input">An input file</param>
        /// <param name="TargetInputs">The target inputs to merge</param>
        public static string[] CombineTextFiles(string Input, string[] TargetInputs) =>
            DriverHandler.CurrentFilesystemDriverLocal.CombineTextFiles(Input, TargetInputs);

        /// <summary>
        /// Combines the binary files and puts the combined output to the array
        /// </summary>
        /// <param name="Input">An input file</param>
        /// <param name="TargetInputs">The target inputs to merge</param>
        public static byte[] CombineBinaryFiles(string Input, string[] TargetInputs) =>
            DriverHandler.CurrentFilesystemDriverLocal.CombineBinaryFiles(Input, TargetInputs);

    }
}
