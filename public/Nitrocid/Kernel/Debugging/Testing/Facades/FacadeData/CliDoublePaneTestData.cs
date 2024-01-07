//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Terminaux.Inputs.Interactive;
using Nitrocid.Languages;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nitrocid.Kernel.Debugging.Testing.Facades.FacadeData
{
    internal class CliDoublePaneTestData : BaseInteractiveTui, IInteractiveTui
    {
        internal static List<string> strings = [];
        internal static List<string> strings2 = [];

        public override List<InteractiveTuiBinding> Bindings { get; set; } =
        [
            new InteractiveTuiBinding("Add", ConsoleKey.F1, (_, index) => Add(index)),
            new InteractiveTuiBinding("Delete", ConsoleKey.F2, (_, index) => Remove(index)),
            new InteractiveTuiBinding("Delete Last", ConsoleKey.F3, (_, _) => RemoveLast()),
        ];

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override IEnumerable SecondaryDataSource =>
            strings2;

        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            string selected = (string)item;

            // Check to see if we're given the test info
            if (string.IsNullOrEmpty(selected))
                InteractiveTuiStatus.Status = Translate.DoTranslation("No info.");
            else
                InteractiveTuiStatus.Status = $"{selected}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            string selected = (string)item;
            return selected;
        }

        private static void Add(int index)
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
                strings2.Add($"[{index}] --2-- [{index}]");
            else
                strings.Add($"[{index}] --1-- [{index}]");
        }

        private static void Remove(int index)
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                if (index < strings2.Count)
                    strings2.RemoveAt(index - 1);
                if (InteractiveTuiStatus.SecondPaneCurrentSelection > strings2.Count)
                    InteractiveTuiStatus.SecondPaneCurrentSelection = strings2.Count;
            }
            else
            {
                if (index < strings.Count)
                    strings.RemoveAt(index - 1);
                if (InteractiveTuiStatus.FirstPaneCurrentSelection > strings.Count)
                    InteractiveTuiStatus.FirstPaneCurrentSelection = strings.Count;
            }
        }

        private static void RemoveLast()
        {
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                strings2.RemoveAt(strings2.Count - 1);
                if (InteractiveTuiStatus.SecondPaneCurrentSelection > strings2.Count)
                    InteractiveTuiStatus.SecondPaneCurrentSelection = strings2.Count;
            }
            else
            {
                strings.RemoveAt(strings.Count - 1);
                if (InteractiveTuiStatus.FirstPaneCurrentSelection > strings.Count)
                    InteractiveTuiStatus.FirstPaneCurrentSelection = strings.Count;
            }
        }
    }
}
