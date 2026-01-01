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

using System.Collections.Generic;
using Terminaux.Inputs.Interactive;

namespace Nitrocid.Kernel.Debugging.Testing.Facades.FacadeData
{
    internal class CliDoublePaneTestData : BaseInteractiveTui<string, string>, IInteractiveTui<string, string>
    {
        internal static List<string> strings = [];
        internal static List<string> strings2 = [];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override IEnumerable<string> SecondaryDataSource =>
            strings2;

        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            string.IsNullOrEmpty(item) ? "No info." : item;

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary(string item) =>
            string.IsNullOrEmpty(item) ? "No info." : item;

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary(string item) =>
            item;

        internal void Add(int index, int index2)
        {
            if (CurrentPane == 2)
                strings2.Add($"[{index2}] --2-- [{index2}]");
            else
                strings.Add($"[{index}] --1-- [{index}]");
        }

        internal void Remove(int index, int index2)
        {
            if (CurrentPane == 2)
            {
                if (index2 < strings2.Count && strings2.Count > 0)
                    strings2.RemoveAt(index2 == 0 ? index2 : index2 - 1);
                if (SecondPaneCurrentSelection > strings2.Count)
                    InteractiveTuiTools.SelectionMovement(this, strings2.Count);
            }
            else
            {
                if (index < strings.Count && strings.Count > 0)
                    strings.RemoveAt(index == 0 ? index : index - 1);
                if (FirstPaneCurrentSelection > strings.Count)
                    InteractiveTuiTools.SelectionMovement(this, strings.Count);
            }
        }

        internal void RemoveLast()
        {
            if (CurrentPane == 2)
            {
                if (strings2.Count > 0)
                    strings2.RemoveAt(strings2.Count - 1);
                if (SecondPaneCurrentSelection > strings2.Count)
                    InteractiveTuiTools.SelectionMovement(this, strings2.Count);
            }
            else
            {
                if (strings.Count > 0)
                    strings.RemoveAt(strings.Count - 1);
                if (FirstPaneCurrentSelection > strings.Count)
                    InteractiveTuiTools.SelectionMovement(this, strings.Count);
            }
        }
    }
}
