
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
using System;

namespace KS.Misc.Contacts.Interactive
{
    /// <summary>
    /// Interactive contact manager colors (preserved for kernel config system)
    /// </summary>
    // TODO: Migrate this to a single TuiColors class on N-KS Beta 3.
    public static class ContactManagerCliColors
    {
        /// <summary>
        /// Contacts manager background color
        /// </summary>
        public static Color ContactsManagerBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// Contacts manager foreground color
        /// </summary>
        public static Color ContactsManagerForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// Contacts manager pane background color
        /// </summary>
        public static Color ContactsManagerPaneBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// Contacts manager pane separator color
        /// </summary>
        public static Color ContactsManagerPaneSeparatorColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkGreen_005f00));
        /// <summary>
        /// Contacts manager pane selected Contacts color (foreground)
        /// </summary>
        public static Color ContactsManagerPaneSelectedContactsForeColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// Contacts manager pane selected Contacts color (background)
        /// </summary>
        public static Color ContactsManagerPaneSelectedContactsBackColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// Contacts manager pane Contacts color (foreground)
        /// </summary>
        public static Color ContactsManagerPaneContactsForeColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkYellow));
        /// <summary>
        /// Contacts manager pane Contacts color (background)
        /// </summary>
        public static Color ContactsManagerPaneContactsBackColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// Contacts manager option background color
        /// </summary>
        public static Color ContactsManagerOptionBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.DarkCyan));
        /// <summary>
        /// Contacts manager key binding in option color
        /// </summary>
        public static Color ContactsManagerKeyBindingOptionColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Black));
        /// <summary>
        /// Contacts manager option foreground color
        /// </summary>
        public static Color ContactsManagerOptionForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Cyan));
        /// <summary>
        /// Contacts manager box background color
        /// </summary>
        public static Color ContactsManagerBoxBackgroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.Red));
        /// <summary>
        /// Contacts manager box foreground color
        /// </summary>
        public static Color ContactsManagerBoxForegroundColor { get; set; } = new(Convert.ToInt32(ConsoleColors.White));
    }
}
