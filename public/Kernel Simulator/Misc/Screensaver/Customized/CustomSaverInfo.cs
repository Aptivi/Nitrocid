
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

namespace KS.Misc.Screensaver.Customized
{
    /// <summary>
    /// Custom screensaver information
    /// </summary>
    public class CustomSaverInfo
    {

        /// <summary>
        /// Name of the screensaver
        /// </summary>
        public string SaverName { get; private set; }
        /// <summary>
        /// File name of the screensaver
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// File path of the screensaver
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// The screensaver base code
        /// </summary>
        public BaseScreensaver ScreensaverBase { get; private set; }

        /// <summary>
        /// Creates new screensaver info instance
        /// </summary>
        internal CustomSaverInfo(string SaverName, string FileName, string FilePath, BaseScreensaver ScreensaverBase)
        {
            this.SaverName = string.IsNullOrWhiteSpace(SaverName) ? FileName : SaverName;
            this.FileName = FileName;
            this.FilePath = FilePath;
            this.ScreensaverBase = ScreensaverBase;
        }

    }
}
