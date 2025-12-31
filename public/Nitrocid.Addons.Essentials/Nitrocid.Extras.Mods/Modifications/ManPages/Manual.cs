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
using System.IO;
using System.Text;

namespace Nitrocid.Extras.Mods.Modifications.ManPages
{
    /// <summary>
    /// Manual page class instance
    /// </summary>
    public class Manual
    {

        /// <summary>
        /// Manual page file name
        /// </summary>
        public string Name { get; private set; } = "";
        /// <summary>
        /// Mod name containing this manual page
        /// </summary>
        public string ModName { get; private set; } = "";
        /// <summary>
        /// The manual page title
        /// </summary>
        public string Title { get; private set; } = "";
        /// <summary>
        /// The manual page revision
        /// </summary>
        public string Revision { get; private set; } = "";
        /// <summary>
        /// The body string (the contents of manual)
        /// </summary>
        public StringBuilder? Body { get; private set; }
        /// <summary>
        /// The list of todos
        /// </summary>
        public List<string>? Todos { get; private set; }
        /// <summary>
        /// Is the manual page valid?
        /// </summary>
        public bool ValidManpage { get; private set; }

        /// <summary>
        /// Makes a new instance of manual
        /// </summary>
        internal Manual(string modName, string ManualFileName)
        {
            string Title = "";
            string Revision = "";
            var Body = new StringBuilder();
            var Todos = new List<string>();
            ValidManpage = PageParser.CheckManual(ManualFileName, ref Title, ref Revision, ref Body, ref Todos);
            if (ValidManpage)
            {
                ModName = modName;
                this.Title = Title;
                this.Revision = Revision;
                this.Body = Body;
                this.Todos = Todos;
                Name = Path.GetFileName(ManualFileName);
            }
        }

    }
}
