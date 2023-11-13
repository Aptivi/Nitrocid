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

using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using Terminaux.Colors;

namespace KS.ConsoleBase.Interactive
{
    /// <summary>
    /// A base class for your interactive user interface for terminal apps
    /// </summary>
    public class BaseInteractiveTui : IInteractiveTui
    {
        internal bool isExiting = false;

        /// <summary>
        /// Current selection for the first pane
        /// </summary>
        public static int FirstPaneCurrentSelection { get; set; } = 1;
        /// <summary>
        /// Current selection for the second pane
        /// </summary>
        public static int SecondPaneCurrentSelection { get; set; } = 1;
        /// <summary>
        /// Current status
        /// </summary>
        public static string Status { get; set; } = "";
        /// <summary>
        /// Current pane
        /// </summary>
        public static int CurrentPane { get; set; } = 1;

        /// <inheritdoc/>
        public virtual List<InteractiveTuiBinding> Bindings { get; set; }
        /// <inheritdoc/>
        public virtual bool SecondPaneInteractable => false;
        /// <inheritdoc/>
        public virtual int RefreshInterval => 0;
        /// <inheritdoc/>
        public virtual bool AcceptsEmptyData => false;

        /// <inheritdoc/>
        public virtual IEnumerable PrimaryDataSource => Array.Empty<string>();
        /// <inheritdoc/>
        public virtual IEnumerable SecondaryDataSource => Array.Empty<string>();

        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public static Color BackgroundColor =>
            new(Config.MainConfig.TuiBackgroundColor);
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public static Color ForegroundColor =>
            new(Config.MainConfig.TuiForegroundColor);
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public static Color PaneBackgroundColor =>
            new(Config.MainConfig.TuiPaneBackgroundColor);
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public static Color PaneSeparatorColor =>
            new(Config.MainConfig.TuiPaneSeparatorColor);
        /// <summary>
        /// Interactive TUI pane selected separator color
        /// </summary>
        public static Color PaneSelectedSeparatorColor =>
            new(Config.MainConfig.TuiPaneSelectedSeparatorColor);
        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        public static Color PaneSelectedItemForeColor =>
            new(Config.MainConfig.TuiPaneSelectedItemForeColor);
        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        public static Color PaneSelectedItemBackColor =>
            new(Config.MainConfig.TuiPaneSelectedItemBackColor);
        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        public static Color PaneItemForeColor =>
            new(Config.MainConfig.TuiPaneItemForeColor);
        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        public static Color PaneItemBackColor =>
            new(Config.MainConfig.TuiPaneItemBackColor);
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public static Color OptionBackgroundColor =>
            new(Config.MainConfig.TuiOptionBackgroundColor);
        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        public static Color KeyBindingOptionColor =>
            new(Config.MainConfig.TuiKeyBindingOptionColor);
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public static Color OptionForegroundColor =>
            new(Config.MainConfig.TuiOptionForegroundColor);
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public static Color BoxBackgroundColor =>
            new(Config.MainConfig.TuiBoxBackgroundColor);
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public static Color BoxForegroundColor =>
            new(Config.MainConfig.TuiBoxForegroundColor);

        /// <inheritdoc/>
        public virtual string GetEntryFromItem(object item) =>
            item is not null ? item.ToString() : "???";

        /// <inheritdoc/>
        public virtual string GetInfoFromItem(object item) =>
            item is not null ? Translate.DoTranslation("No info.") : "???";

        /// <inheritdoc/>
        public virtual void HandleExit() { }

        /// <inheritdoc/>
        public virtual void RenderStatus(object item) { }

        /// <inheritdoc/>
        public virtual void LastOnOverflow()
        {
            int primaryCount = EnumerableTools.CountElements(PrimaryDataSource);
            int secondaryCount = EnumerableTools.CountElements(SecondaryDataSource);
            if (FirstPaneCurrentSelection > primaryCount)
                FirstPaneCurrentSelection = primaryCount;
            if (SecondPaneCurrentSelection > secondaryCount)
                SecondPaneCurrentSelection = secondaryCount;
        }
    }
}
