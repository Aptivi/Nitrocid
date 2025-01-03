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
using System.Linq;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Mods.Modifications.ManPages
{
    /// <summary>
    /// Mod manual page management module
    /// </summary>
    public static class PageManager
    {

        // Variables
        internal static List<Manual> Pages = [];

        /// <summary>
        /// Lists all manual pages
        /// </summary>
        public static List<Manual> ListAllPages() =>
            ListAllPages("");

        /// <summary>
        /// Lists all manual pages
        /// </summary>
        /// <param name="SearchTerm">Keywords to search</param>
        public static List<Manual> ListAllPages(string SearchTerm)
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return Pages;
            else
            {
                var FoundPages = new List<Manual>();
                foreach (var ManualPage in Pages)
                    if (ManualPage.Title.Contains(SearchTerm))
                        FoundPages.Add(ManualPage);
                return FoundPages;
            }
        }

        /// <summary>
        /// Lists all manual pages by mod
        /// </summary>
        /// <param name="modName">Kernel modification name</param>
        public static List<Manual> ListAllPagesByMod(string modName)
        {
            if (!ModManager.Mods.ContainsKey(modName))
                throw new KernelException(KernelExceptionType.ModManual, Translate.DoTranslation("Tried to query the manuals for nonexistent mod {0}."), modName);

            // Populate manual pages by mod
            return Pages.Where((manual) => manual.ModName == modName).ToList();
        }

        /// <summary>
        /// Adds a manual page to the pages list
        /// </summary>
        /// <param name="modName">Kernel modification name</param>
        /// <param name="Name">Manual page name</param>
        /// <param name="Page">Manual page instance</param>
        public static void AddManualPage(string modName, string Name, Manual Page)
        {
            if (!ModManager.Mods.ContainsKey(modName))
                throw new KernelException(KernelExceptionType.ModManual, Translate.DoTranslation("Tried to initialize the manual {0} for nonexistent mod {1}."), Name, modName);

            // Check to see if title is defined
            if (string.IsNullOrWhiteSpace(Name))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Title not defined.");
                Name = Translate.DoTranslation("Untitled manual page") + $" {Pages.Count}";
            }

            // Add the page if valid
            if (!Pages.Any((man) => man.Title == Name))
            {
                if (Page.ValidManpage)
                    Pages.Add(Page);
                else
                    throw new KernelException(KernelExceptionType.InvalidManpage, Translate.DoTranslation("The manual page {0} is invalid."), Name);
            }
        }

        /// <summary>
        /// Removes a manual page from the list
        /// </summary>
        /// <param name="Name">Manual page name</param>
        public static bool RemoveManualPage(Manual Name) =>
            Pages.Remove(Name);

    }
}
