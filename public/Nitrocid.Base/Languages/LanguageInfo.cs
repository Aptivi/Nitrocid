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
using System.Diagnostics;
using Nitrocid.Base.Kernel.Debugging;

namespace Nitrocid.Base.Languages
{
    /// <summary>
    /// Language information
    /// </summary>
    [DebuggerDisplay("[{threeLetterLanguageName}] {fullLanguageName}")]
    public class LanguageInfo
    {
        private readonly string threeLetterLanguageName;
        private readonly string fullLanguageName;
        private readonly Dictionary<string, string> strings = [];

        /// <summary>
        /// The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.
        /// </summary>
        public string ThreeLetterLanguageName =>
            threeLetterLanguageName;

        /// <summary>
        /// The full name of language without the country specifier.
        /// </summary>
        public string FullLanguageName =>
            fullLanguageName;

        /// <summary>
        /// The localization information containing strings (for info purposes)
        /// </summary>
        public Dictionary<string, string> Strings =>
            strings;

        /// <summary>
        /// Initializes the new instance of language information
        /// </summary>
        /// <param name="LangName">The three-letter language name found in resources. Some languages have translated variants, and they usually end with "_T" in resources and "-T" in KS.</param>
        /// <param name="FullLanguageName">The full name of language without the country specifier.</param>
        /// <param name="strings">List of strings (for info purposes)</param>
        public LanguageInfo(string LangName, string FullLanguageName, Dictionary<string, string> strings)
        {
            // Install values to the object instance
            threeLetterLanguageName = LangName;
            fullLanguageName = FullLanguageName;

            // Populate language strings
            this.strings = new Dictionary<string, string>(strings);
            DebugWriter.WriteDebug(DebugLevel.I, "{0} strings.", vars: [this.strings.Count]);
        }
    }
}
