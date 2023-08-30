
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
using KS.Languages;
using System;
using System.Collections;
using System.Collections.Generic;
using Terminaux.Colors;

namespace KS.Misc.Interactive
{
    /// <summary>
    /// A base class for your interactive user interface for terminal apps
    /// </summary>
    public class BaseInteractiveTui : IInteractiveTui
    {
        internal bool isExiting = false;

        /// <inheritdoc/>
        public static bool RedrawRequired { get; set; } = true;
        /// <inheritdoc/>
        public static int FirstPaneCurrentSelection { get; set; } = 1;
        /// <inheritdoc/>
        public static int SecondPaneCurrentSelection { get; set; } = 1;
        /// <inheritdoc/>
        public static string Status { get; set; } = "";
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public static Color BackgroundColor =>
            new(Config.MainConfig.TuiBackgroundColor);
        /// <inheritdoc/>
        public static Color ForegroundColor =>
            new(Config.MainConfig.TuiForegroundColor);
        /// <inheritdoc/>
        public static Color PaneBackgroundColor =>
            new(Config.MainConfig.TuiPaneBackgroundColor);
        /// <inheritdoc/>
        public static Color PaneSeparatorColor =>
            new(Config.MainConfig.TuiPaneSeparatorColor);
        /// <inheritdoc/>
        public static Color PaneSelectedSeparatorColor =>
            new(Config.MainConfig.TuiPaneSelectedSeparatorColor);
        /// <inheritdoc/>
        public static Color PaneSelectedItemForeColor =>
            new(Config.MainConfig.TuiPaneSelectedItemForeColor);
        /// <inheritdoc/>
        public static Color PaneSelectedItemBackColor =>
            new(Config.MainConfig.TuiPaneSelectedItemBackColor);
        /// <inheritdoc/>
        public static Color PaneItemForeColor =>
            new(Config.MainConfig.TuiPaneItemForeColor);
        /// <inheritdoc/>
        public static Color PaneItemBackColor =>
            new(Config.MainConfig.TuiPaneItemBackColor);
        /// <inheritdoc/>
        public static Color OptionBackgroundColor =>
            new(Config.MainConfig.TuiOptionBackgroundColor);
        /// <inheritdoc/>
        public static Color KeyBindingOptionColor =>
            new(Config.MainConfig.TuiKeyBindingOptionColor);
        /// <inheritdoc/>
        public static Color OptionForegroundColor =>
            new(Config.MainConfig.TuiOptionForegroundColor);
        /// <inheritdoc/>
        public static Color BoxBackgroundColor =>
            new(Config.MainConfig.TuiBoxBackgroundColor);
        /// <inheritdoc/>
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
            int primaryCount = InteractiveTuiTools.CountElements(PrimaryDataSource);
            int secondaryCount = InteractiveTuiTools.CountElements(SecondaryDataSource);
            if (FirstPaneCurrentSelection > primaryCount)
                FirstPaneCurrentSelection = primaryCount;
            if (SecondPaneCurrentSelection > secondaryCount)
                SecondPaneCurrentSelection = secondaryCount;
        }
    }
}
