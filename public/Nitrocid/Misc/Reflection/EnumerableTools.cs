
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

using KS.Kernel.Debugging;
using System.Collections;

namespace KS.Misc.Reflection
{
    /// <summary>
    /// Tools for enumerables
    /// </summary>
    public static class EnumerableTools
    {
        /// <summary>
        /// Counts the number of elements in the <see cref="IEnumerable"/> instance
        /// </summary>
        /// <param name="enumerable">An instance of an enumerable to count the elements found in it</param>
        /// <returns>Number of elements</returns>
        public static int CountElements(IEnumerable enumerable)
        {
            int dataCount = 0;
            foreach (var item in enumerable)
            {
                // IEnumerable from System.Collections doesn't implement Count() or Length, hence the array, List<>, Dictionary<>,
                // and other collections have either Count or Length. This is an ugly hack that we should live with.
                dataCount++;
            }

            // Return the result
            DebugWriter.WriteDebug(DebugLevel.I, "{0} elements.", dataCount);
            return dataCount;
        }

        /// <summary>
        /// Gets an element from the <see cref="IEnumerable"/> instance with the provided item index number starting from zero
        /// </summary>
        /// <param name="enumerable">An instance of an enumerable to get an item from</param>
        /// <param name="index">An index of an element to search for</param>
        /// <returns>An element from the enumerable if found, or null if not found</returns>
        public static object GetElementFromIndex(IEnumerable enumerable, int index)
        {
            // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
            object dataObject = null;
            int steppedItems = 0;
            foreach (var item in enumerable)
            {
                steppedItems++;
                if (steppedItems == index + 1)
                {
                    // We found the item that we need! Assign it to dataObject so GetEntryFromItem() can formulate a string.
                    DebugWriter.WriteDebug(DebugLevel.I, "Found required item index {0}.", index);
                    dataObject = item;
                    break;
                }
            }
            return dataObject;
        }
    }
}
