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

using KS.ConsoleBase.Interactive;
using KS.Misc.Interactives;

namespace KS.Files.Operations
{
    /// <summary>
    /// File and folder selection class
    /// </summary>
    public static class Selection
    {
        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <returns>A selected file, or an empty string if not selected</returns>
        public static string SelectFile()
        {
            var selector = new FileSelectorCli();
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string selected = FileSelectorCli.SelectedFile;

            // TODO: Get rid of this workaround
            FileSelectorCli.selectedFile = "";
            return selected;
        }

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <returns>A selected folder, or an empty string if not selected</returns>
        public static string SelectFolder()
        {
            var selector = new FolderSelectorCli();
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string selected = FolderSelectorCli.SelectedFolder;

            // TODO: Get rid of this workaround
            FolderSelectorCli.selectedFolder = "";
            return selected;
        }
    }
}
