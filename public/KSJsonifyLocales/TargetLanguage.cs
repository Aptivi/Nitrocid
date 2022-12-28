
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

namespace KSJsonifyLocales
{
    /// <summary>
    /// Target language class
    /// </summary>
    public class TargetLanguage
    {
        /// <summary>
        /// The file name of the language
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// The language name
        /// </summary>
        public string LanguageName { get; private set; }
        /// <summary>
        /// Chooses whether the language is custom or from the KS resources
        /// </summary>
        public bool CustomLanguage { get; private set; }

        /// <summary>
        /// Makes a new class instance of TargetLanguage
        /// </summary>
        /// <param name="FileName">The file name of the language</param>
        /// <param name="LanguageName">The language name</param>
        /// <param name="CustomLanguage">Chooses whether the language is custom or from the KS resources</param>
        public TargetLanguage(string FileName, string LanguageName, bool CustomLanguage)
        {
            this.FileName = FileName;
            this.LanguageName = LanguageName;
            this.CustomLanguage = CustomLanguage;
        }
    }
}