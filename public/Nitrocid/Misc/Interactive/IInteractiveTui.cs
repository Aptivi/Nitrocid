
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

using System.Collections;
using System.Collections.Generic;
using Terminaux.Colors;

namespace KS.Misc.Interactive
{
    /// <summary>
    /// An interface for your interactive user interface for terminal apps
    /// </summary>
    public interface IInteractiveTui
    {
        /// <summary>
        /// Whether redrawing is required
        /// </summary>
        public static bool RedrawRequired { get; set; }
        /// <summary>
        /// Current selection for the first pane
        /// </summary>
        public static int FirstPaneCurrentSelection { get; set; }
        /// <summary>
        /// Current selection for the second pane
        /// </summary>
        public static int SecondPaneCurrentSelection { get; set; }
        /// <summary>
        /// Current status
        /// </summary>
        public static string Status { get; set; }
        /// <summary>
        /// Current pane
        /// </summary>
        public static int CurrentPane { get; set; }

        /// <summary>
        /// All key bindings for your interactive user interface
        /// </summary>
        public List<InteractiveTuiBinding> Bindings { get; set; }
        /// <summary>
        /// Whether the user can switch to the second path
        /// </summary>
        public bool SecondPaneInteractable { get; }
        /// <summary>
        /// How many milliseconds to wait before refreshing? Only applies to single-pane interactive TUI instances. 0 to disable.
        /// </summary>
        public int RefreshInterval { get; }
        /// <summary>
        /// Whether empty data is accepted
        /// </summary>
        public bool AcceptsEmptyData { get; }
        /// <summary>
        /// Whether delta-based refresh method is used or not
        /// </summary>
        public bool FastRefresh { get; }

        /// <summary>
        /// An array, a dictionary, a list, or an enumerable that holds data (pane one)
        /// </summary>
        public IEnumerable PrimaryDataSource { get; }
        /// <summary>
        /// An array, a dictionary, a list, or an enumerable that holds data (pane two)
        /// </summary>
        public IEnumerable SecondaryDataSource { get; }

        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public static Color BackgroundColor { get; }
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public static Color ForegroundColor { get; }
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public static Color PaneBackgroundColor { get; }
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public static Color PaneSeparatorColor { get; }
        /// <summary>
        /// Interactive TUI pane selected separator color
        /// </summary>
        public static Color PaneSelectedSeparatorColor { get; }
        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        public static Color PaneSelectedItemForeColor { get; }
        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        public static Color PaneSelectedItemBackColor { get; }
        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        public static Color PaneItemForeColor { get; }
        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        public static Color PaneItemBackColor { get; }
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public static Color OptionBackgroundColor { get; }
        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        public static Color KeyBindingOptionColor { get; }
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public static Color OptionForegroundColor { get; }
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public static Color BoxBackgroundColor { get; }
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public static Color BoxForegroundColor { get; }

        /// <summary>
        /// Gets an entry string from a specified item for listing
        /// </summary>
        /// <param name="item">Target item</param>
        public string GetEntryFromItem(object item);
        /// <summary>
        /// Gets the info from the item
        /// </summary>
        /// <param name="item">Target item</param>
        /// <returns>The rendered info so that <see cref="InteractiveTuiTools"/> can handle its rendering</returns>
        public string GetInfoFromItem(object item);
        /// <summary>
        /// Handles exiting the interactive TUI
        /// </summary>
        public void HandleExit();
        /// <summary>
        /// Renders the status
        /// </summary>
        /// <param name="item">Target item</param>
        public void RenderStatus(object item);
        /// <summary>
        /// Goes up to the last element upon overflow (caused by remove operation, ...). This applies to the first and the second pane.
        /// </summary>
        public void LastOnOverflow();
    }
}
