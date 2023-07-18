
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
        public static Color ContactsManagerBackgroundColor { get; set; } = new(Config.MainConfig.ContactsManagerBackgroundColor);
        /// <summary>
        /// Contacts manager foreground color
        /// </summary>
        public static Color ContactsManagerForegroundColor { get; set; } = new(Config.MainConfig.ContactsManagerForegroundColor);
        /// <summary>
        /// Contacts manager pane background color
        /// </summary>
        public static Color ContactsManagerPaneBackgroundColor { get; set; } = new(Config.MainConfig.ContactsManagerPaneBackgroundColor);
        /// <summary>
        /// Contacts manager pane separator color
        /// </summary>
        public static Color ContactsManagerPaneSeparatorColor { get; set; } = new(Config.MainConfig.ContactsManagerPaneSeparatorColor);
        /// <summary>
        /// Contacts manager pane selected Contacts color (foreground)
        /// </summary>
        public static Color ContactsManagerPaneSelectedContactsForeColor { get; set; } = new(Config.MainConfig.ContactsManagerPaneSelectedContactsForeColor);
        /// <summary>
        /// Contacts manager pane selected Contacts color (background)
        /// </summary>
        public static Color ContactsManagerPaneSelectedContactsBackColor { get; set; } = new(Config.MainConfig.ContactsManagerPaneSelectedContactsBackColor);
        /// <summary>
        /// Contacts manager pane Contacts color (foreground)
        /// </summary>
        public static Color ContactsManagerPaneContactsForeColor { get; set; } = new(Config.MainConfig.ContactsManagerPaneContactsForeColor);
        /// <summary>
        /// Contacts manager pane Contacts color (background)
        /// </summary>
        public static Color ContactsManagerPaneContactsBackColor { get; set; } = new(Config.MainConfig.ContactsManagerPaneContactsBackColor);
        /// <summary>
        /// Contacts manager option background color
        /// </summary>
        public static Color ContactsManagerOptionBackgroundColor { get; set; } = new(Config.MainConfig.ContactsManagerOptionBackgroundColor);
        /// <summary>
        /// Contacts manager key binding in option color
        /// </summary>
        public static Color ContactsManagerKeyBindingOptionColor { get; set; } = new(Config.MainConfig.ContactsManagerKeyBindingOptionColor);
        /// <summary>
        /// Contacts manager option foreground color
        /// </summary>
        public static Color ContactsManagerOptionForegroundColor { get; set; } = new(Config.MainConfig.ContactsManagerOptionForegroundColor);
        /// <summary>
        /// Contacts manager box background color
        /// </summary>
        public static Color ContactsManagerBoxBackgroundColor { get; set; } = new(Config.MainConfig.ContactsManagerBoxBackgroundColor);
        /// <summary>
        /// Contacts manager box foreground color
        /// </summary>
        public static Color ContactsManagerBoxForegroundColor { get; set; } = new(Config.MainConfig.ContactsManagerBoxForegroundColor);
    }
}
