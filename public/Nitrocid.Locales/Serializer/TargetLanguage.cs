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

namespace Nitrocid.Locales.Serializer
{
    /// <summary>
    /// Target language class
    /// </summary>
    internal class TargetLanguage
    {
        /// <summary>
        /// The file name of the language
        /// </summary>
        internal string FileName { get; private set; }
        /// <summary>
        /// The language name
        /// </summary>
        internal string LanguageName { get; private set; }
        /// <summary>
        /// Chooses whether the language is custom or from the KS resources
        /// </summary>
        internal bool CustomLanguage { get; private set; }

        /// <summary>
        /// Makes a new class instance of TargetLanguage
        /// </summary>
        /// <param name="FileName">The file name of the language</param>
        /// <param name="LanguageName">The language name</param>
        /// <param name="CustomLanguage">Chooses whether the language is custom or from the KS resources</param>
        internal TargetLanguage(string FileName, string LanguageName, bool CustomLanguage)
        {
            this.FileName = FileName;
            this.LanguageName = LanguageName;
            this.CustomLanguage = CustomLanguage;
        }
    }
}
