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

using Nitrocid.Base.Files.Folders;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        /// <summary>
        /// Controls how the files will be sorted
        /// </summary>
        public int SortMode { get; set; } = (int)FilesystemSortOptions.FullName;
        /// <summary>
        /// Controls the direction of filesystem sorting whether it's ascending or descending
        /// </summary>
        public int SortDirection { get; set; } = (int)FilesystemSortDirection.Ascending;
        /// <summary>
        /// Shows hidden files.
        /// </summary>
        public bool HiddenFiles { get; set; }
        /// <summary>
        /// If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface.
        /// </summary>
        public bool FullParseMode { get; set; }
        /// <summary>
        /// Shows what file is being processed during the filesystem operations
        /// </summary>
        public bool ShowFilesystemProgress { get; set; } = true;
        /// <summary>
        /// Shows the brief file details while listing files
        /// </summary>
        public bool ShowFileDetailsList { get; set; } = true;
        /// <summary>
        /// Hides the annoying message if the listing function tries to open an unauthorized folder
        /// </summary>
        public bool SuppressUnauthorizedMessages { get; set; } = true;
        /// <summary>
        /// Makes the "cat" command print the file's line numbers
        /// </summary>
        public bool PrintLineNumbers { get; set; }
        /// <summary>
        /// Sorts the filesystem list professionally
        /// </summary>
        public bool SortList { get; set; } = true;
        /// <summary>
        /// If enabled, shows the total folder size in list, depending on how to calculate the folder sizes according to the configuration.
        /// </summary>
        public bool ShowTotalSizeInList { get; set; }
        /// <summary>
        /// If enabled, sorts the list alphanumerically. Otherwise, sorts them alphabetically.
        /// </summary>
        public bool SortLogically { get; set; } = true;
    }
}
