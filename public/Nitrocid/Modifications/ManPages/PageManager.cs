
// Nitrocid KS  Copyright (C) 2018-2019  Aptivi
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

using System.Collections.Generic;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Modifications.ManPages
{
    /// <summary>
    /// Mod manual page management module
    /// </summary>
    public static class PageManager
    {

        // Variables
        internal static Dictionary<string, Manual> Pages = new();

        /// <summary>
        /// Lists all manual pages
        /// </summary>
        public static Dictionary<string, Manual> ListAllPages() => ListAllPages("");

        /// <summary>
        /// Lists all manual pages
        /// </summary>
        /// <param name="SearchTerm">Keywords to search</param>
        public static Dictionary<string, Manual> ListAllPages(string SearchTerm)
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                return Pages;
            }
            else
            {
                var FoundPages = new Dictionary<string, Manual>();
                foreach (string ManualPage in Pages.Keys)
                {
                    if (ManualPage.Contains(SearchTerm))
                    {
                        FoundPages.Add(ManualPage, Pages[ManualPage]);
                    }
                }
                return FoundPages;
            }
        }

        /// <summary>
        /// Adds a manual page to the pages list
        /// </summary>
        /// <param name="Name">Manual page name</param>
        /// <param name="Page">Manual page instance</param>
        public static void AddManualPage(string Name, Manual Page)
        {
            // Check to see if title is defined
            if (string.IsNullOrWhiteSpace(Name))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Title not defined.");
                Name = Translate.DoTranslation("Untitled manual page") + $" {Pages.Count}";
            }

            // Add the page if valid
            if (!Pages.ContainsKey(Name))
            {
                if (Page.ValidManpage)
                {
                    Pages.Add(Name, Page);
                }
                else
                {
                    throw new KernelException(KernelExceptionType.InvalidManpage, Translate.DoTranslation("The manual page {0} is invalid."), Name);
                }
            }
        }

        /// <summary>
        /// Removes a manual page from the list
        /// </summary>
        /// <param name="Name">Manual page name</param>
        public static bool RemoveManualPage(string Name) => Pages.Remove(Name);

    }
}
