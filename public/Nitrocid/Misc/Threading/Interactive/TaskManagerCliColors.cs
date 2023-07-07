
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
using System;

namespace KS.Misc.Threading.Interactive
{
    /// <summary>
    /// Interactive task manager colors (preserved for kernel config system)
    /// </summary>
    // TODO: Migrate this to a single TuiColors class on N-KS Beta 3.
    public static class TaskManagerCliColors
    {
        /// <summary>
        /// Task manager background color
        /// </summary>
        public static Color TaskManagerBackgroundColor { get; set; } = new(Config.MainConfig.TaskManagerBackgroundColor);
        /// <summary>
        /// Task manager foreground color
        /// </summary>
        public static Color TaskManagerForegroundColor { get; set; } = new(Config.MainConfig.TaskManagerForegroundColor);
        /// <summary>
        /// Task manager pane background color
        /// </summary>
        public static Color TaskManagerPaneBackgroundColor { get; set; } = new(Config.MainConfig.TaskManagerPaneBackgroundColor);
        /// <summary>
        /// Task manager pane separator color
        /// </summary>
        public static Color TaskManagerPaneSeparatorColor { get; set; } = new(Config.MainConfig.TaskManagerPaneSeparatorColor);
        /// <summary>
        /// Task manager pane selected Tasks color (foreground)
        /// </summary>
        public static Color TaskManagerPaneSelectedTaskForeColor { get; set; } = new(Config.MainConfig.TaskManagerPaneSelectedTaskForeColor);
        /// <summary>
        /// Task manager pane selected Tasks color (background)
        /// </summary>
        public static Color TaskManagerPaneSelectedTaskBackColor { get; set; } = new(Config.MainConfig.TaskManagerPaneSelectedTaskBackColor);
        /// <summary>
        /// Task manager pane Tasks color (foreground)
        /// </summary>
        public static Color TaskManagerPaneTaskForeColor { get; set; } = new(Config.MainConfig.TaskManagerPaneTaskForeColor);
        /// <summary>
        /// Task manager pane Tasks color (background)
        /// </summary>
        public static Color TaskManagerPaneTaskBackColor { get; set; } = new(Config.MainConfig.TaskManagerPaneTaskBackColor);
        /// <summary>
        /// Task manager option background color
        /// </summary>
        public static Color TaskManagerOptionBackgroundColor { get; set; } = new(Config.MainConfig.TaskManagerOptionBackgroundColor);
        /// <summary>
        /// Task manager key binding in option color
        /// </summary>
        public static Color TaskManagerKeyBindingOptionColor { get; set; } = new(Config.MainConfig.TaskManagerKeyBindingOptionColor);
        /// <summary>
        /// Task manager option foreground color
        /// </summary>
        public static Color TaskManagerOptionForegroundColor { get; set; } = new(Config.MainConfig.TaskManagerOptionForegroundColor);
        /// <summary>
        /// Task manager box background color
        /// </summary>
        public static Color TaskManagerBoxBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Red));
        /// <summary>
        /// Task manager box foreground color
        /// </summary>
        public static Color TaskManagerBoxForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.White));
    }
}
