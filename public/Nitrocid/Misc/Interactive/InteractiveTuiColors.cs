
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

using ColorSeq;
using KS.Kernel.Configuration;

namespace KS.Misc.Interactive
{
    /// <summary>
    /// Interactive TUI colors (preserved for kernel config system)
    /// </summary>
    public static class InteractiveTuiColors
    {
        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public static Color TuiBackgroundColor { get; set; } = new(Config.MainConfig.TuiBackgroundColor);
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public static Color TuiForegroundColor { get; set; } = new(Config.MainConfig.TuiForegroundColor);
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public static Color TuiPaneBackgroundColor { get; set; } = new(Config.MainConfig.TuiPaneBackgroundColor);
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public static Color TuiPaneSeparatorColor { get; set; } = new(Config.MainConfig.TuiPaneSeparatorColor);
        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        public static Color TuiPaneSelectedItemForeColor { get; set; } = new(Config.MainConfig.TuiPaneSelectedItemForeColor);
        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        public static Color TuiPaneSelectedItemBackColor { get; set; } = new(Config.MainConfig.TuiPaneSelectedItemBackColor);
        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        public static Color TuiPaneItemForeColor { get; set; } = new(Config.MainConfig.TuiPaneItemForeColor);
        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        public static Color TuiPaneItemBackColor { get; set; } = new(Config.MainConfig.TuiPaneItemBackColor);
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public static Color TuiOptionBackgroundColor { get; set; } = new(Config.MainConfig.TuiOptionBackgroundColor);
        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        public static Color TuiKeyBindingOptionColor { get; set; } = new(Config.MainConfig.TuiKeyBindingOptionColor);
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public static Color TuiOptionForegroundColor { get; set; } = new(Config.MainConfig.TuiOptionForegroundColor);
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public static Color TuiBoxBackgroundColor { get; set; } = new(Config.MainConfig.TuiBoxBackgroundColor);
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public static Color TuiBoxForegroundColor { get; set; } = new(Config.MainConfig.TuiBoxForegroundColor);
    }
}
