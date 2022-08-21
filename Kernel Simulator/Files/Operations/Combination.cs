
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.Files.Read;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Files.Operations
{
    public static class Combination
    {

        /// <summary>
        /// Combines the files and puts the combined output to the array
        /// </summary>
        /// <param name="Input">An input file</param>
        /// <param name="TargetInputs">The target inputs to merge</param>
        public static string[] CombineFiles(string Input, string[] TargetInputs)
        {
            try
            {
                var CombinedContents = new List<string>();

                // Add the input contents
                Filesystem.ThrowOnInvalidPath(Input);
                CombinedContents.AddRange(FileRead.ReadContents(Input));

                // Enumerate the target inputs
                foreach (string TargetInput in TargetInputs)
                {
                    Filesystem.ThrowOnInvalidPath(TargetInput);
                    CombinedContents.AddRange(FileRead.ReadContents(TargetInput));
                }

                // Return the combined contents
                return CombinedContents.ToArray();
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
                DebugWriter.Wdbg(DebugLevel.E, "Failed to combine files: {0}", ex.Message);
                throw new Kernel.Exceptions.FilesystemException(Translate.DoTranslation("Failed to combine files."), ex);
            }
        }

    }
}