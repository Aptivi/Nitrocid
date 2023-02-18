
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

using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Modifications
{
    /// <summary>
    /// Mod part information
    /// </summary>
    public class ModPartInfo
    {

        /// <summary>
        /// The mod name. If no name is specified, or if it only consists of whitespaces (space), the file name is taken.
        /// </summary>
        public string ModName { get; private set; }
        /// <summary>
        /// The part name.
        /// </summary>
        public string PartName { get; private set; }
        /// <summary>
        /// The mod part file name
        /// </summary>
        public string PartFileName { get; private set; }
        /// <summary>
        /// The mod part file path
        /// </summary>
        public string PartFilePath { get; private set; }
        /// <summary>
        /// The mod part script
        /// </summary>
        public IMod PartScript { get; private set; }

        /// <summary>
        /// Creates new mod info instance
        /// </summary>
        internal ModPartInfo(string ModName, string PartName, string PartFileName, string PartFilePath, IMod PartScript)
        {
            // Validate values. Check to see if the name is null. If so, it will take the mod file name.
            if (string.IsNullOrWhiteSpace(ModName))
            {
                ModName = PartFileName;
            }

            // Check to see if the part script is null. If so, throw exception.
            if (PartScript is null)
            {
                throw new KernelException(KernelExceptionType.ModNoParts, Translate.DoTranslation("Mod part is nothing."));
            }

            // Install values to new instance
            this.ModName = ModName;
            this.PartName = PartName;
            this.PartFileName = PartFileName;
            this.PartFilePath = PartFilePath;
            this.PartScript = PartScript;
        }

    }
}
