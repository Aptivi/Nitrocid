//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

namespace Nitrocid.Base.Kernel.Debugging.Testing.Facades.FacadeData
{
    internal class CliInfoPaneTestData : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal static List<string> strings = [];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            strings;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            string selected = item;

            // Check to see if we're given the test info
            if (string.IsNullOrEmpty(selected))
                return " No info.";
            else
                return $" {selected}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item)
        {
            string selected = item;
            return selected;
        }

        internal void Add(int index)
        {
            strings.Add($"[{index}] --+-- [{index}]");
        }

        internal void Remove(int index)
        {
            if (strings.Count > 0)
                strings.RemoveAt(index);
        }

        internal void RemoveLast()
        {
            if (strings.Count > 0)
                strings.RemoveAt(strings.Count - 1);
        }
    }
}
