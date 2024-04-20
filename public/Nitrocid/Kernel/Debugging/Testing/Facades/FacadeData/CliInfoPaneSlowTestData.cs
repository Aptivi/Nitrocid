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
using System.Collections.Generic;

namespace Nitrocid.Kernel.Debugging.Testing.Facades.FacadeData
{
    internal class CliInfoPaneSlowTestData : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal static List<string> strings = [];

        public override InteractiveTuiBinding[] Bindings { get; } =
        [
            new InteractiveTuiBinding("Add", ConsoleKey.F1, (_, index) => strings.Add($"[{index}] --+-- [{index}]")),
            new InteractiveTuiBinding("Delete", ConsoleKey.F2, (_, index) => strings.RemoveAt(index)),
            new InteractiveTuiBinding("Delete Last", ConsoleKey.F3, (_, _) => strings.RemoveAt(strings.Count - 1)),
        ];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item) =>
            string.IsNullOrEmpty(item) ? Translate.DoTranslation("No info.") : item;

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;
    }
}
