using System.Collections.Generic;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

// Kernel Simulator  Copyright (C) 2018-2019  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace KS.ManPages
{
    public static class PageManager
    {

        // Variables
        internal static Dictionary<string, Manual> Pages = new Dictionary<string, Manual>();

        /// <summary>
        /// Lists all manual pages
        /// </summary>
        public static Dictionary<string, Manual> ListAllPages()
        {
            return ListAllPages("");
        }

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
                DebugWriter.Wdbg(DebugLevel.W, "Title not defined.");
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
                    throw new Kernel.Exceptions.InvalidManpageException(Translate.DoTranslation("The manual page {0} is invalid."), Name);
                }
            }
        }

        /// <summary>
        /// Removes a manual page from the list
        /// </summary>
        /// <param name="Name">Manual page name</param>
        public static bool RemoveManualPage(string Name)
        {
            return Pages.Remove(Name);
        }

    }
}