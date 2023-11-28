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
using KS.Files.Paths;
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
        public static string SelectFile() =>
            SelectFile(PathsManagement.HomePath);

        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A selected file, or an empty string if not selected</returns>
        public static string SelectFile(string path)
        {
            var selector = new FileSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path)
            };
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string selected = FileSelectorCli.SelectedFile;
            return selected;
        }

        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <returns>A list of selected files, or an empty list if not selected</returns>
        public static string[] SelectFiles() =>
            SelectFiles(PathsManagement.HomePath);

        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A list of selected files, or an empty list if not selected</returns>
        public static string[] SelectFiles(string path)
        {
            var selector = new FilesSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path)
            };
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string[] selected = FilesSelectorCli.SelectedFiles;
            return selected;
        }

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <returns>A selected folder, or an empty string if not selected</returns>
        public static string SelectFolder() =>
            SelectFolder(PathsManagement.HomePath);

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A selected folder, or an empty string if not selected</returns>
        public static string SelectFolder(string path)
        {
            var selector = new FolderSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path)
            };
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string selected = FolderSelectorCli.SelectedFolder;
            return selected;
        }

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <returns>A list of selected folders, or an empty list if not selected</returns>
        public static string[] SelectFolders() =>
            SelectFolders(PathsManagement.HomePath);

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A list of selected folders, or an empty list if not selected</returns>
        public static string[] SelectFolders(string path)
        {
            var selector = new FoldersSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path)
            };
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string[] selected = FoldersSelectorCli.SelectedFolders;
            return selected;
        }
    }
}
