
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

using KS.Drivers;

namespace KS.Files.LineEndings
{
    /// <summary>
    /// Line endings conversion module
    /// </summary>
    public static class LineEndingsConverter
    {

        /// <summary>
        /// Converts the line endings to the newline style for the current platform
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        public static void ConvertLineEndings(string TextFile) =>
            DriverHandler.CurrentFilesystemDriver.ConvertLineEndings(TextFile);

        /// <summary>
        /// Converts the line endings to the specified newline style
        /// </summary>
        /// <param name="TextFile">Text file name with extension or file path</param>
        /// <param name="LineEndingStyle">Line ending style</param>
        public static void ConvertLineEndings(string TextFile, FilesystemNewlineStyle LineEndingStyle) =>
            DriverHandler.CurrentFilesystemDriver.ConvertLineEndings(TextFile, LineEndingStyle);

    }
}
