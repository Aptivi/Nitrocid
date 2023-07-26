
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

using KS.Languages;
using KS.Misc.Interactive;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KS.Kernel.Debugging.Testing.Facades.FacadeData
{
    internal class CliInfoPaneTestRefreshingData : BaseInteractiveTui, IInteractiveTui
    {
        internal static List<string> strings = new();
        private static int timesRendered = 0;

        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            new InteractiveTuiBinding(/* Localizable */ "Add",         ConsoleKey.F1, (_, index) => strings.Add($"[{index}] --+-- [{index}]"), true),
            new InteractiveTuiBinding(/* Localizable */ "Delete",      ConsoleKey.F2, (_, index) => strings.RemoveAt(index), true),
            new InteractiveTuiBinding(/* Localizable */ "Delete Last", ConsoleKey.F3, (_, _)     => strings.RemoveAt(strings.Count - 1), true),
            new InteractiveTuiBinding(/* Localizable */ "Redraw",      ConsoleKey.F4, (_, _)     => RedrawRequired = true, true),
        };

        /// <inheritdoc/>
        public override int RefreshInterval => 
            3000;

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            // Some variables
            string selected = (string)item;
            timesRendered++;

            // Check to see if we're given the test info
            if (string.IsNullOrEmpty(selected))
                Status = Translate.DoTranslation("No info.");
            else
                Status = $"{selected}";

            // Now, populate the info to the status
            return $"{timesRendered}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            string selected = (string)item;
            return selected;
        }
    }
}
