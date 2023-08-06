
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

using KS.Kernel.Configuration;
using Terminaux.Colors;

namespace KS.Files.Interactive
{
    /// <summary>
    /// Interactive file manager colors (preserved for kernel config system)
    /// </summary>
    // TODO: Migrate this to a single TuiColors class on N-KS Beta 3.
    public static class FileManagerCliColors
    {
        /// <summary>
        /// File manager background color
        /// </summary>
        public static Color FileManagerBackgroundColor { get; set; } = new(Config.MainConfig.FileManagerBackgroundColor);
        /// <summary>
        /// File manager foreground color
        /// </summary>
        public static Color FileManagerForegroundColor { get; set; } = new(Config.MainConfig.FileManagerForegroundColor);
        /// <summary>
        /// File manager pane background color
        /// </summary>
        public static Color FileManagerPaneBackgroundColor { get; set; } = new(Config.MainConfig.FileManagerPaneBackgroundColor);
        /// <summary>
        /// File manager pane separator color
        /// </summary>
        public static Color FileManagerPaneSeparatorColor { get; set; } = new(Config.MainConfig.FileManagerPaneSeparatorColor);
        /// <summary>
        /// File manager selected pane separator color
        /// </summary>
        public static Color FileManagerPaneSelectedSeparatorColor { get; set; } = new(Config.MainConfig.FileManagerPaneSelectedSeparatorColor);
        /// <summary>
        /// File manager pane selected Files color (foreground)
        /// </summary>
        public static Color FileManagerPaneSelectedFileForeColor { get; set; } = new(Config.MainConfig.FileManagerPaneSelectedFileForeColor);
        /// <summary>
        /// File manager pane selected Files color (background)
        /// </summary>
        public static Color FileManagerPaneSelectedFileBackColor { get; set; } = new(Config.MainConfig.FileManagerPaneSelectedFileBackColor);
        /// <summary>
        /// File manager pane Files color (foreground)
        /// </summary>
        public static Color FileManagerPaneFileForeColor { get; set; } = new(Config.MainConfig.FileManagerPaneFileForeColor);
        /// <summary>
        /// File manager pane Files color (background)
        /// </summary>
        public static Color FileManagerPaneFileBackColor { get; set; } = new(Config.MainConfig.FileManagerPaneFileBackColor);
        /// <summary>
        /// File manager option background color
        /// </summary>
        public static Color FileManagerOptionBackgroundColor { get; set; } = new(Config.MainConfig.FileManagerOptionBackgroundColor);
        /// <summary>
        /// File manager key binding in option color
        /// </summary>
        public static Color FileManagerKeyBindingOptionColor { get; set; } = new(Config.MainConfig.FileManagerKeyBindingOptionColor);
        /// <summary>
        /// File manager option foreground color
        /// </summary>
        public static Color FileManagerOptionForegroundColor { get; set; } = new(Config.MainConfig.FileManagerOptionForegroundColor);
        /// <summary>
        /// File manager box background color
        /// </summary>
        public static Color FileManagerBoxBackgroundColor { get; set; } = new(Config.MainConfig.FileManagerBoxBackgroundColor);
        /// <summary>
        /// File manager box foreground color
        /// </summary>
        public static Color FileManagerBoxForegroundColor { get; set; } = new(Config.MainConfig.FileManagerBoxForegroundColor);
    }
}
